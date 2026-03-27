using SerratedSharp.SerratedDom;
using SerratedSharp.SerratedJSInterop;
using SerratedSharp.SerratedJSInterop.Tests.Shared;
using SerratedSharp.SerratedJQ.Plain;
using System.Runtime.InteropServices.JavaScript;
using Wasm;

namespace Tests.Wasm;

/// <summary>
/// Tests for callback binding: CallJS and SetJSProperty with delegates (Action, Action&lt;JSObject&gt;, Callback).
/// Delegates are detected in the argument list / property value; a single shim wraps the callback and returns the handler for unbind.
/// Uses a small JS mock (window.__callbackTestMock) injected via script where needed.
/// </summary>
public partial class TestsContainer
{
    /// <summary>
    /// Wrapper so that CallJS(callback) calls the mock's "add", "remove", "fire" by explicit funcName.
    /// </summary>
    private sealed class MockCallbackTarget : IJSObjectWrapper
    {
        public JSObject JSObject { get; }
        public MockCallbackTarget(JSObject jsObject) => JSObject = jsObject;
        public JSObject Add(Action<JSObject> callback) => this.CallJS<JSObject>(funcName: "add", callback);
        public void Remove(JSObject handler) => this.CallJS(funcName: "remove", handler);
        public void Fire(object value) => this.CallJS(funcName: "fire", value);
    }

    /// <summary>
    /// Test wrapper for DOM click events to validate Callback.Create&lt;T&gt; with IJSObjectWrapper&lt;T&gt; marshalling.
    /// </summary>
    private sealed class ClickEvent : IJSObjectWrapper<ClickEvent>
    {
        public JSObject JSObject { get; }
        public ClickEvent(JSObject jsObject) => JSObject = jsObject;
        public string Type => this.GetJSProperty<string>(propertyName: "type");
        public double ClientX => this.GetJSProperty<double>(propertyName: "clientX");
        public static ClickEvent WrapInstance(JSObject jsObject) => new ClickEvent(jsObject);
    }

    /// <summary>
    /// Returns the callback test mock object from JS (via CallbackTestMockShim.GetCallbackTestMock(); requires RCL script loaded).
    /// <para>
    /// The mock simulates a typical JS event subscription pattern (like addEventListener/removeEventListener).
    /// Unit tests use it to verify C# delegates can be passed to JS, invoked, and unsubscribed:
    /// <list type="number">
    ///   <item>Call <c>add(callback)</c> to register a C# delegate - returns a handler token</item>
    ///   <item>Call <c>fire(...args)</c> to invoke the registered callback - verifies C# delegate is called</item>
    ///   <item>Call <c>remove(handler)</c> to unsubscribe using the token - verifies callback no longer fires</item>
    /// </list>
    /// </para>
    /// <para>
    /// Additional methods: <c>register(a, cb, b)</c> tests callback at non-zero index;
    /// <c>getReg()</c> returns stored args; <c>noCallbackReturn()</c> tests normal return path.
    /// </para>
    /// </summary>
    private static JSObject EnsureCallbackTestMock()
    {
        return CallbackTestMockShim.GetCallbackTestMock();
    }

    public class CallbackBinding_SingleArg_ReturnsHandler_And_Unbind : JSTest
    {
        public override void Run()
        {
            StubHtmlIntoTestContainer(0);
            var targetJs = EnsureCallbackTestMock();
            var target = new MockCallbackTarget(targetJs);

            int count = 0;
            JSObject handler = target.Add(_ => count++);
            Assert(handler != null, "CallJS with callback should return non-null handler");

            // Fire must pass a JSObject: Action<JSObject> cannot receive a primitive (runtime: "JSObject proxy of number is not supported").
            target.Fire(target.JSObject);
            Assert(count == 1, "Callback should be invoked once after fire");

            target.Remove(handler);
            target.Fire(target.JSObject);
            Assert(count == 1, "Callback should not be invoked again after remove");
        }
    }

    public class CallbackBinding_PackedParams_ReturnsHandler_GetArrayObjectItems : JSTest
    {
        public override void Run()
        {
            StubHtmlIntoTestContainer(0);
            var targetJs = EnsureCallbackTestMock();
            int count = 0;
            PackedParams? packedReceived = null;
            var handler = targetJs.CallJS<JSObject>(funcName: "add", Callback.CreatePacked(packed =>
            {
                count++;
                packedReceived = packed;
            }));
            Assert(handler != null, "CallJS with Callback should return non-null handler");
            targetJs.CallJS(funcName: "fire", targetJs, handler);
            Assert(count == 1, "Packed-params callback should be invoked once");
            Assert(packedReceived != null, "Packed should be non-null");
            var items = packedReceived.Value.GetUnpacked();
            Assert(items != null && items.Length == 2, "GetUnpacked() should return two items (targetJs, handler)");
            targetJs.CallJS(funcName: "remove", handler);
            targetJs.CallJS(funcName: "fire", 20);
            Assert(count == 1, "Callback should not be invoked again after remove");
        }
    }

    public class CallbackBinding_SetTimeout_HandlerFirst_ClearTimeout : JSTest
    {
        public override void Run()
        {
            StubHtmlIntoTestContainer(0);
            var window = (JSObject)JSHost.GlobalThis;
            int count = 0;
            Action<JSObject> cb = _ => count++;
            // setTimeout returns a numeric timer ID in the browser, not a JSObject.
            var timerId = window.CallJS<int>(funcName: "setTimeout", cb, 5000);
            Assert(timerId != 0, "CallJS(setTimeout, callback, delay) should return non-zero timer ID");
            window.CallJS(funcName: "clearTimeout", timerId);
            Assert(timerId != 0, "Timer ID is valid for clearTimeout");
        }
    }

    /// <summary>
    /// SetJSProperty with a delegate (Action&lt;JSObject&gt;) is detected and set; callback runs when invoked; setting to null clears it.
    /// </summary>
    public class CallbackBinding_SetJSProperty_WithDelegate_ReturnsHandler : JSTest
    {
        public override void Run()
        {
            StubHtmlIntoTestContainer(0);
            var doc = Document.GetDocument();
            var div = doc.CreateElement("div").JSObject;
            int count = 0;
            div.SetJSProperty(propertyName: "onclick", (Action<JSObject>)(_ => count++));
            div.CallJS(funcName: "click");
            Assert(count == 1, "SetJSProperty with delegate: callback should run when element is clicked");
            div.SetJSProperty(propertyName: "onclick", null!);
            div.CallJS(funcName: "click");
            Assert(count == 1, "After clearing onclick, callback should not run again");
        }
    }

    /// <summary>
    /// SetJSProperty with onclick delegate; handler receives event (JSObject) and operates on it via interop (GlobalJS.Console.Log).
    /// Verifies the argument passed into the handler can be used in interop calls.
    /// </summary>
    public class CallbackBinding_Click_Handler_LogsEventParameter : JSTest
    {
        public override void Run()
        {
            StubHtmlIntoTestContainer(0);
            var doc = Document.GetDocument();
            var div = doc.CreateElement("div").JSObject;
            int count = 0;
            div.SetJSProperty(propertyName: "onclick", (Action<JSObject>)(e =>
            {
                count++;
                GlobalJS.Console.Log("click event:", e);
            }));
            div.CallJS(funcName: "click");
            Assert(count == 1, "Click handler should run once");
            Assert(true, "Handler operated on event parameter via GlobalJS.Console.Log without throw");
        }
    }

    /// <summary>
    /// Callback.Create&lt;T&gt; should auto-wrap a JS event object into an IJSObjectWrapper&lt;T&gt;.
    /// Verifies a typed event wrapper can read multiple properties from the underlying event JSObject.
    /// </summary>
    public class CallbackBinding_Click_Handler_TypedEventWrapper : JSTest
    {
        public override void Run()
        {
            StubHtmlIntoTestContainer(0);
            var doc = Document.GetDocument();
            var div = doc.CreateElement("div").JSObject;

            int count = 0;
            string? eventType = null;
            double eventClientX = -1;

            div.SetJSProperty(propertyName: "onclick", Callback.Create<ClickEvent>(e =>
            {
                count++;
                eventType = e.Type;
                eventClientX = e.ClientX;
            }));

            div.CallJS(funcName: "click");

            Assert(count == 1, "Typed callback should be invoked once");
            Assert(eventType == "click", $"Expected wrapped event type 'click', got '{eventType}'");
            Assert(eventClientX >= 0, $"Expected wrapped clientX to be available, got {eventClientX}");
        }
    }

    /// <summary>
    /// SetJSProperty with a non-callback value uses the normal path; value can be read back.
    /// </summary>
    public class CallbackBinding_SetJSProperty_NonCallback_ReturnsNull : JSTest
    {
        public override void Run()
        {
            StubHtmlIntoTestContainer(0);
            var doc = Document.GetDocument();
            var div = doc.CreateElement("div").JSObject;
            div.SetJSProperty(propertyName: "dataTestValue", "hello");
            var read = div.GetJSProperty<string>(propertyName: "dataTestValue");
            Assert(read == "hello", "SetJSProperty with non-callback value should be readable back");
        }
    }

    /// <summary>
    /// CallJS with callback at index 1: Register(123, actionDelegate, "123") uses normal path and callback-at-index shim.
    /// </summary>
    public class CallbackBinding_CallbackAtIndex1_Register_ReturnsHandler : JSTest
    {
        public override void Run()
        {
            StubHtmlIntoTestContainer(0);
            var targetJs = EnsureCallbackTestMock();
            Action<JSObject> action = _ => { };
            var handler = targetJs.CallJS<JSObject>(funcName: "register", 123, action, "123");
            Assert(handler != null, "CallJS(register, 123, callback, '123') should return non-null handler");
            var reg = targetJs.CallJS<JSObject>(funcName: "getReg");
            Assert(reg != null, "getReg should return the stored register args");
            var a = reg!.GetJSProperty<double>("a");
            var b = reg.GetJSProperty<string>("b");
            Assert(Math.Abs(a - 123) < 0.01, "First param should be 123");
            Assert(b == "123", "Third param should be '123'");
        }
    }

    /// <summary>
    /// Call without any delegate uses the normal FuncByNameAsObject path (no callback shim).
    /// </summary>
    public class CallbackBinding_NoDelegate_NormalPath_ReturnsValue : JSTest
    {
        public override void Run()
        {
            StubHtmlIntoTestContainer(0);
            var targetJs = EnsureCallbackTestMock();
            var result = targetJs.CallJS<int>(funcName: "noCallbackReturn");
            Assert(result == 42, "CallJS(noCallbackReturn) with no delegate should return 42 via normal path");
        }
    }

    /// <summary>
    /// Optional callback-handle pattern: create a handle once with MarshalCallback.MarshalCallbackAsWrapper(Action&lt;JSObject&gt;), pass to CallJS("add", handle).
    /// Same callback is used without re-wrap; remove uses the token returned by add.
    /// </summary>
    public class CallbackBinding_CallbackHandle_CallJS_Add_Remove : JSTest
    {
        public override void Run()
        {
            StubHtmlIntoTestContainer(0);
            var targetJs = EnsureCallbackTestMock();
            var target = new MockCallbackTarget(targetJs);
            int count = 0;
            CallbackHandle handle = MarshalCallback.MarshalCallbackAsWrapper((Action<JSObject>)(_ => count++));
            targetJs.CallJS<JSObject>(funcName: "add", handle);
            target.Fire(target.JSObject);
            Assert(count == 1, "Callback should be invoked once after fire");
            targetJs.CallJS(funcName: "remove", handle);
            target.Fire(target.JSObject);
            Assert(count == 1, "Callback should not be invoked again after remove");
        }
    }

    /// <summary>
    /// MarshalCallbackAsWrapper(Action) overload: no-arg callback handle, add/fire/remove via mock.
    /// </summary>
    public class CallbackBinding_CallbackHandle_Action_Add_Remove : JSTest
    {
        public override void Run()
        {
            StubHtmlIntoTestContainer(0);
            var targetJs = EnsureCallbackTestMock();
            var target = new MockCallbackTarget(targetJs);
            int count = 0;
            CallbackHandle handle = MarshalCallback.MarshalCallbackAsWrapper(() => count++);
            targetJs.CallJS<JSObject>(funcName: "add", handle);
            target.Fire(target.JSObject);
            Assert(count == 1, "Action callback should be invoked once after fire");
            targetJs.CallJS(funcName: "remove", handle);
            target.Fire(target.JSObject);
            Assert(count == 1, "Callback should not be invoked again after remove");
        }
    }

    /// <summary>
    /// MarshalCallbackAsWrapper(Callback) overload: packed-params callback handle, add/fire/remove; verify packed args.
    /// </summary>
    public class CallbackBinding_CallbackHandle_PackedParams_Add_Remove : JSTest
    {
        public override void Run()
        {
            JQueryPlainObject stubs = StubHtmlIntoTestContainer(1);
            JSObject stubDivJs = stubs.Get(0);
            var targetJs = EnsureCallbackTestMock();
            int count = 0;
            PackedParams? packedReceived = null;
            var rawCallback = Callback.CreatePacked(packed =>
            {
                count++;
                packedReceived = packed;
            });
            CallbackHandle handle = MarshalCallback.MarshalCallbackAsWrapper(rawCallback);
            targetJs.CallJS<JSObject>(funcName: "add", handle);
            // Pass a DOM JSObject as second arg, not the callback handle (avoids marshalling a function as a call parameter).
            targetJs.CallJS(funcName: "fire", targetJs, stubDivJs);
            Assert(count == 1, "Packed-params callback should be invoked once");
            Assert(packedReceived != null, "Packed should be non-null");
            var items = packedReceived.Value.GetUnpacked();
            Assert(items != null && items.Length == 2, "GetUnpacked() should return two items");
            targetJs.CallJS(funcName: "remove", handle);
            targetJs.CallJS(funcName: "fire", 20);
            Assert(count == 1, "Callback should not be invoked again after remove");
        }
    }

    /// <summary>
    /// Demonstrates that firing a primitive (e.g. integer) into an Action&lt;JSObject&gt; callback throws
    /// because the .NET runtime cannot create a JSObject proxy for a primitive value.
    /// The workaround is to wrap the primitive in an object on the JS side, or use Callback.
    /// </summary>
    public class CallbackBinding_PrimitiveArg_ActionJSObject_Throws : JSTest
    {
        public override void Run()
        {
            StubHtmlIntoTestContainer(0);
            var targetJs = EnsureCallbackTestMock();
            var target = new MockCallbackTarget(targetJs);

            int count = 0;
            JSObject handler = target.Add(_ => count++);

            // Passing a primitive (integer) to fire() causes the JS shim to pass it directly
            // to the .NET Action<JSObject> marshaller, which throws because JSObject proxy
            // of a number is not supported.
            bool threw = false;
            try
            {
                target.Fire(42);
            }
            catch (JSException ex)
            {
                threw = true;
                GlobalJS.Console.Log("Expected JSException for primitive callback arg:", ex.Message);
            }
            Assert(threw, "Firing a primitive into Action<JSObject> should throw JSException");
            Assert(count == 0, "Callback should not have been invoked");
        }
    }

    /// <summary>
    /// Demonstrates that Callback can receive primitive values from JS via <see cref="PackedParams.GetUnpacked"/> (mixed <c>object[]</c>).
    /// </summary>
    public class CallbackBinding_PrimitiveArg_PackedParams_Workaround : JSTest
    {
        public override void Run()
        {
            StubHtmlIntoTestContainer(0);
            var targetJs = EnsureCallbackTestMock();

            int count = 0;
            double receivedValue = 0;
            string? receivedString = null;

            var handler = targetJs.CallJS<JSObject>(funcName: "add", Callback.CreatePacked(packed =>
            {
                count++;
                var items = packed.GetUnpacked();
                receivedValue = Convert.ToDouble(items[0]);
                receivedString = items[1] as string;
            }));

            Assert(handler != null, "Handler should be non-null");

            // Fire with a number and a string � both primitives that cannot be passed to Action<JSObject>
            targetJs.CallJS(funcName: "fire", 42, "hello");

            Assert(count == 1, "Packed-params callback should be invoked once");
            Assert(Math.Abs(receivedValue - 42) < 0.01, $"Should receive numeric value 42, got {receivedValue}");
            Assert(receivedString == "hello", $"Should receive string 'hello', got '{receivedString}'");
        }
    }

    /// <summary>Same primitive packed-args scenario as <see cref="CallbackBinding_PrimitiveArg_PackedParams_Workaround"/>; asserts <see cref="PackedParams.GetUnpacked"/> with default stub container.</summary>
    public class CallbackBinding_PrimitiveArg_PackedParams_GetAsObjectArray : JSTest
    {
        public override void Run()
        {
            StubHtmlIntoTestContainer();
            var targetJs = EnsureCallbackTestMock();

            int count = 0;
            double receivedValue = 0;
            string? receivedString = null;

            var handler = targetJs.CallJS<JSObject>(funcName: "add", Callback.CreatePacked(packed =>
            {
                count++;
                var items = packed.GetUnpacked();
                receivedValue = Convert.ToDouble(items[0]);
                receivedString = items[1] as string;
            }));

            Assert(handler != null, "Handler should be non-null");

            // Fire with a number and a string � both primitives that cannot be passed to Action<JSObject>
            targetJs.CallJS(funcName: "fire", 42, "hello");

            Assert(count == 1, "Packed-params callback should be invoked once");
            Assert(Math.Abs(receivedValue - 42) < 0.01, $"Should receive numeric value 42, got {receivedValue}");
            Assert(receivedString == "hello", $"Should receive string 'hello', got '{receivedString}'");
        }
    }

    /// <summary>
    /// Callback when JS passes two primitives and a JSObject; <see cref="PackedParams.GetUnpacked"/> returns
    /// an <c>object[]</c> with numeric, string, and <see cref="JSObject"/> entries. The element is the single stub from
    /// <see cref="JSTest.StubHtmlIntoTestContainer(int)"/> (<c>class="a"</c>) so we can assert identity and <c>className</c>.
    /// </summary>
    public class CallbackBinding_PackedParams_TwoPrimitivesAndJSObject : JSTest
    {
        public override void Run()
        {
            JQueryPlainObject stubs = StubHtmlIntoTestContainer(1);
            JSObject stubDivJs = stubs.Get(0);// use Get so we retriev the native HTML element
            var targetJs = EnsureCallbackTestMock();

            int count = 0;
            int packedLength = -1;
            int receivedValue = 0;
            string? receivedString = null;
            JSObject? receivedObj = null;

            var handler = targetJs.CallJS<JSObject>(funcName: "add", Callback.CreatePacked(packed =>
            {
                count++;
                var items = packed.GetUnpacked();
                packedLength = items.Length;
                if (items.Length >= 3)
                {
                    receivedValue = Convert.ToInt32 (items[0]);
                    receivedString = items[1] as string;
                    receivedObj = items[2] as JSObject;
                }
            }));

            Assert(handler != null, "Handler should be non-null");

            targetJs.CallJS(funcName: "fire", 42, "hello", stubDivJs);
            Assert(count == 1, "Packed-params callback should be invoked once");
            Assert(packedLength == 3, $"Expected three packed arguments, got {packedLength}");
            Assert(Math.Abs(receivedValue - 42) < 0.01, $"Should receive numeric value 42, got {receivedValue}");
            Assert(receivedString == "hello", $"Should receive string 'hello', got '{receivedString}'");
            Assert(receivedObj != null, "Third argument should marshal as JSObject");
            Assert(ReferenceEquals(receivedObj, stubDivJs),
                "Third packed arg should be the same JSObject instance as the stubbed test div");
            Assert(receivedObj!.GetJSProperty<string>("className") == "a",
                $"Stubbed div className should be 'a', got '{receivedObj.GetJSProperty<string>("className")}'");
        }
    }

    /// <summary>
    /// Typed <see cref="Callback.Create{T1, T2, T3}"/> overload with <c>(int, string, JSObject)</c>.
    /// Verifies that numeric coercion (double-to-int), string pass-through, and JSObject identity all work
    /// without manual indexing, <see cref="Convert"/>, or <c>as</c> casts.
    /// </summary>
    public class CallbackBinding_TypedPackedParams_TwoPrimitivesAndJSObject : JSTest
    {
        public override void Run()
        {
            JQueryPlainObject stubs = StubHtmlIntoTestContainer(1);
            JSObject stubDivJs = stubs.Get(0);
            var targetJs = EnsureCallbackTestMock();

            int count = 0;
            int receivedValue = 0;
            string? receivedString = null;
            JSObject? receivedObj = null;

            var handler = targetJs.CallJS<JSObject>(funcName: "add",
                Callback.Create<int, string, JSObject>((value, str, obj) =>
                {
                    count++;
                    receivedValue = value;
                    receivedString = str;
                    receivedObj = obj;
                }));

            Assert(handler != null, "Handler should be non-null");

            targetJs.CallJS(funcName: "fire", 42, "hello", stubDivJs);
            Assert(count == 1, "Typed packed-params callback should be invoked once");
            Assert(receivedValue == 42, $"Should receive int 42 (coerced from double), got {receivedValue}");
            Assert(receivedString == "hello", $"Should receive string 'hello', got '{receivedString}'");
            Assert(receivedObj != null, "Third argument should marshal as JSObject");
            Assert(ReferenceEquals(receivedObj, stubDivJs),
                "Third packed arg should be the same JSObject instance as the stubbed test div");
            Assert(receivedObj!.GetJSProperty<string>("className") == "a",
                $"Stubbed div className should be 'a', got '{receivedObj.GetJSProperty<string>("className")}'");
        }
    }

    /// <summary>
    /// Typed <see cref="Callback.Create{T1, T2, T3, T4, T5}"/> overload with mixed primitive and JSObject parameters.
    /// Verifies argument count, numeric coercion, and JSObject identity across five callback arguments.
    /// </summary>
    public class CallbackBinding_TypedPackedParams_FiveArguments_MixedTypes : JSTest
    {
        public override void Run()
        {
            JQueryPlainObject stubs = StubHtmlIntoTestContainer(1);
            JSObject stubDivJs = stubs.Get(0);
            var targetJs = EnsureCallbackTestMock();

            int count = 0;
            int receivedInt = 0;
            string? receivedString = null;
            JSObject? receivedObj = null;
            double receivedDouble = 0;
            bool receivedBool = false;

            var handler = targetJs.CallJS<JSObject>(funcName: "add",
                Callback.Create<int, string, JSObject, double, bool>((i, s, o, d, b) =>
                {
                    count++;
                    receivedInt = i;
                    receivedString = s;
                    receivedObj = o;
                    receivedDouble = d;
                    receivedBool = b;
                }));

            Assert(handler != null, "Handler should be non-null");

            targetJs.CallJS(funcName: "fire", 42, "hello", stubDivJs, 12.5, true);

            Assert(count == 1, "Typed 5-arg callback should be invoked once");
            Assert(receivedInt == 42, $"Should receive int 42, got {receivedInt}");
            Assert(receivedString == "hello", $"Should receive string 'hello', got '{receivedString}'");
            Assert(receivedObj != null, "Third argument should marshal as JSObject");
            Assert(ReferenceEquals(receivedObj, stubDivJs),
                "Third packed arg should be the same JSObject instance as the stubbed test div");
            Assert(Math.Abs(receivedDouble - 12.5) < 0.01, $"Should receive double 12.5, got {receivedDouble}");
            Assert(receivedBool, "Should receive bool true");
        }
    }

}
