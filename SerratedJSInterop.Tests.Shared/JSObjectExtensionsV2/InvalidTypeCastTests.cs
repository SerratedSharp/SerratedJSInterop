using SerratedSharp.SerratedDom;
using SerratedSharp.SerratedJSInterop;
using SerratedSharp.SerratedJQ.Plain;
using System.Runtime.InteropServices.JavaScript;
using Wasm;

namespace Tests.Wasm;

public partial class TestsContainer
{
    /// <summary>
    /// Tests that GetJSProperty throws InvalidCastException when requesting an incompatible type.
    /// For example, requesting an int[] when the JS property is a string.
    /// </summary>
    public class GetJSProperty_InvalidType_ThrowsInvalidCastException : JSTest
    {
        public override void Run()
        {
            StubHtmlIntoTestContainer(0);
            var doc = Document.GetDocument();
            var div = doc.CreateElement("div").JSObject;

            // Set a string property
            JSImportInstanceHelpers.SetProperty(div, "id", "test-invalid-cast");

            // Attempt to read a string property as an int[] - should throw
            bool threwException = false;
            try
            {
                var invalidResult = div.GetJSProperty<int[]>("id");
            }
            catch (InvalidCastException ex)
            {
                threwException = true;
                Assert(ex.Message.StartsWith("Failure to coerce type returned from JS interop"), "Exception message should start with expected text");
                Assert(ex.InnerException != null, "InnerException should not be null");
            }
            catch (NotImplementedException)
            {
                // CallJSFuncInternal throws NotImplementedException for unsupported array element types
                threwException = true;
            }

            Assert(threwException, "GetJSProperty<int[]> on a string property should throw InvalidCastException or NotImplementedException");
        }
    }

    /// <summary>
    /// Tests that GetJSProperty throws InvalidCastException when requesting a JSObject from a primitive value.
    /// </summary>
    public class GetJSProperty_PrimitiveAsJSObject_ThrowsInvalidCastException : JSTest
    {
        public override void Run()
        {
            StubHtmlIntoTestContainer(0);
            var doc = Document.GetDocument();
            var div = doc.CreateElement("div").JSObject;

            // Set a numeric property
            JSImportInstanceHelpers.SetProperty(div, "testNum", 42);

            // Attempt to read a numeric property as a JSObject - should throw
            bool threwException = false;
            try
            {
                var invalidResult = div.GetJSProperty<JSObject>("testNum");
            }
            catch (InvalidCastException ex)
            {
                threwException = true;
                Assert(ex.Message.StartsWith("Failure to coerce type returned from JS interop"), "Exception message should start with expected text");
                Assert(ex.InnerException != null, "InnerException should not be null");
            }

            Assert(threwException, "GetJSProperty<JSObject> on a numeric property should throw InvalidCastException");
        }
    }

    /// <summary>
    /// Tests that CallJS throws NotImplementedException when requesting an unsupported array element type.
    /// </summary>
    public class CallJS_UnsupportedArrayType_ThrowsNotImplementedException : JSTest
    {
        public override void Run()
        {
            StubHtmlIntoTestContainer(0);
            var doc = Document.GetDocument();
            var div = doc.CreateElement("div").JSObject;

            // Attempt to call a function expecting an unsupported array return type (e.g., int[])
            bool threwException = false;
            try
            {
                // getAttribute returns a string, but we're asking for int[] which is unsupported
                var invalidResult = div.CallJS<int[]>(funcName: "getAttribute", "id");
            }
            catch (NotImplementedException ex)
            {
                threwException = true;
                Assert(ex.Message.Contains("Int32"), "Exception message should mention the unsupported element type");
            }

            Assert(threwException, "CallJS<int[]> should throw NotImplementedException for unsupported array element types");
        }
    }

    /// <summary>
    /// Tests that CallJS throws InvalidCastException when the JS function returns a type incompatible with the requested type.
    /// </summary>
    public class CallJS_IncompatibleReturnType_ThrowsInvalidCastException : JSTest
    {
        public override void Run()
        {
            StubHtmlIntoTestContainer(0);
            var doc = Document.GetDocument();
            var div = doc.CreateElement("div").JSObject;
            JSImportInstanceHelpers.SetProperty(div, "id", "test-div");

            // getAttribute returns a string, but we're asking for a bool
            bool threwException = false;
            try
            {
                var invalidResult = div.CallJS<bool>(funcName: "getAttribute", "id");
            }
            catch (InvalidCastException ex)
            {
                threwException = true;
                Assert(ex.Message.StartsWith("Failure to coerce type returned from JS interop"), "Exception message should start with expected text");
                Assert(ex.InnerException != null, "InnerException should not be null");
            }

            Assert(threwException, "CallJS<bool> when JS returns a string should throw InvalidCastException");
        }
    }
}