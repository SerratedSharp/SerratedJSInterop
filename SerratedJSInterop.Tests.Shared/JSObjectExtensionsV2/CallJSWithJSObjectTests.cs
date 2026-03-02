using SerratedSharp.SerratedDom;
using SerratedSharp.SerratedJSInterop;
using SerratedSharp.SerratedJQ.Plain;
using System.Runtime.InteropServices.JavaScript;
using Wasm;

namespace Tests.Wasm;

public partial class TestsContainer
{
    private static class VoidCallJSHelper
    {
        public static void AppendChild(JSObject parent, JSObject child)
        {
            parent.CallJS(SerratedJS.Params(child));
        }
    }

    private static class JSObjectCallJSReturnHelper
    {
        public static JSObject GetBoundingClientRect(JSObject el) => el.CallJS<JSObject>();
        public static JSObject CreateElement(JSObject doc, string tagName) => doc.CallJS<JSObject>(tagName);
        public static JSObject InsertBefore(JSObject parent, object newChild, object refChild) => parent.CallJS<JSObject>(newChild, refChild);
        public static JSObject InsertBefore(JSObject parent, object a, object b, object c) => parent.CallJS<JSObject>(a, b, c);
        public static JSObject InsertBefore(JSObject parent, object a, object b, object c, object d) => parent.CallJS<JSObject>(a, b, c, d);
        public static JSObject InsertBefore(JSObject parent, object a, object b, object c, object d, object e) => parent.CallJS<JSObject>(a, b, c, d, e);
    }

    private static class JSObjectCallJSVoidHelper
    {
        public static void Blur(JSObject el) => el.CallJS();
        public static void AppendChild(JSObject parent, JSObject child) => parent.CallJS(child);
        public static void AppendChild(JSObject parent, object a, object b) => parent.CallJS(a, b);
        public static void AppendChild(JSObject parent, object a, object b, object c) => parent.CallJS(a, b, c);
        public static void AppendChild(JSObject parent, object a, object b, object c, object d) => parent.CallJS(a, b, c, d);
        public static void AppendChild(JSObject parent, object a, object b, object c, object d, object e) => parent.CallJS(a, b, c, d, e);
    }

    public class CallJS_JSObject_Returning_ExplicitFuncName_ParamsArray : JSTest
    {
        public override void Run()
        {
            StubHtmlIntoTestContainer(0);
            var doc = Document.GetDocument();
            var span = doc.JSObject.CallJS<JSObject>(funcName: "createElement", "span");
            Assert(span != null, "createElement via JSObject.CallJS<JSObject>(string, params object[]) should return non-null");
            var tagName = span.GetJSProperty<string>("tagName");
            Assert(tagName == "SPAN", "Created element tagName should be SPAN");
        }
    }

    public class CallJS_JSObject_Void_ExplicitFuncName_JSParams : JSTest
    {
        public override void Run()
        {
            StubHtmlIntoTestContainer(0);
            var doc = Document.GetDocument();
            var container = tc.Get(0);
            var div = doc.CreateElement("div").JSObject;
            JSImportInstanceHelpers.SetProperty(div, "id", "void-calljs-explicit-id");

            container.CallJS(funcName: "appendChild", SerratedJS.Params(div));

            var foundJs = tc.Find("#void-calljs-explicit-id").Get(0);
            Assert(foundJs != null, "appendChild via void CallJS(string, JSParams) should have appended the node");
            Assert(new HtmlElement(foundJs).TextContent == "", "Appended element should be in DOM");
        }
    }

    public class CallJS_JSObject_Void_CallerMemberName_AppendChild : JSTest
    {
        public override void Run()
        {
            StubHtmlIntoTestContainer(0);
            var doc = Document.GetDocument();
            var container = tc.Get(0);
            var span = doc.CreateElement("span").JSObject;
            JSImportInstanceHelpers.SetProperty(span, "id", "void-calljs-callername-id");

            VoidCallJSHelper.AppendChild(container, span);

            var found = tc.Find("#void-calljs-callername-id").Get(0);
            Assert(found != null, "appendChild via void CallJS(JSParams, CallerMemberName) should have appended the node");
        }
    }

    public class CallJS_JSObject_Void_ExplicitFuncName_ParamsArray : JSTest
    {
        public override void Run()
        {
            StubHtmlIntoTestContainer(0);
            var doc = Document.GetDocument();
            var container = tc.Get(0);
            var div = doc.CreateElement("div").JSObject;
            JSImportInstanceHelpers.SetProperty(div, "id", "calljs-jsobject-void-params-id");

            container.CallJS(funcName: "appendChild", div);

            var found = tc.Find("#calljs-jsobject-void-params-id").Get(0);
            Assert(found != null, "appendChild via JSObject.CallJS(string, params object[]) should have appended the node");
        }
    }

    // --- JSObject CallJS<J> inferred name (0..5 params) ---

    public class CallJS_JSObject_Returning_CallerMemberName_0Params : JSTest
    {
        public override void Run()
        {
            StubHtmlIntoTestContainer(0);
            var container = tc.Get(0);
            var rect = JSObjectCallJSReturnHelper.GetBoundingClientRect(container);
            Assert(rect != null, "getBoundingClientRect via JSObject.CallJS<JSObject>() (0 params) should return non-null");
        }
    }

    public class CallJS_JSObject_Returning_CallerMemberName_1Param : JSTest
    {
        public override void Run()
        {
            StubHtmlIntoTestContainer(0);
            var doc = Document.GetDocument();
            var div = JSObjectCallJSReturnHelper.CreateElement(doc.JSObject, "div");
            Assert(div != null, "createElement via JSObject.CallJS<JSObject>(param1) should return non-null");
            var tagName = div.GetJSProperty<string>("tagName");
            Assert(tagName == "DIV", "Created element tagName should be DIV");
        }
    }

    public class CallJS_JSObject_Returning_CallerMemberName_2Params : JSTest
    {
        public override void Run()
        {
            StubHtmlIntoTestContainer(0);
            var doc = Document.GetDocument();
            var container = tc.Get(0);
            var div = doc.CreateElement("div").JSObject;
            var span = doc.CreateElement("span").JSObject;
            JSImportInstanceHelpers.SetProperty(span, "id", "jso-return-2p-id");
            container.CallJS(funcName: "appendChild", div);
            var inserted = JSObjectCallJSReturnHelper.InsertBefore(container, span, div);
            Assert(inserted != null, "insertBefore via JSObject.CallJS<JSObject>(param1, param2) should return non-null");
            var found = tc.Find("#jso-return-2p-id").Get(0);
            Assert(found != null, "insertBefore should have inserted the node");
        }
    }

    public class CallJS_JSObject_Returning_CallerMemberName_3Params : JSTest
    {
        public override void Run()
        {
            StubHtmlIntoTestContainer(0);
            var doc = Document.GetDocument();
            var container = tc.Get(0);
            var newChild = doc.CreateElement("span").JSObject;
            var refChild = doc.CreateElement("div").JSObject;
            container.CallJS(funcName: "appendChild", refChild);
            var inserted = JSObjectCallJSReturnHelper.InsertBefore(container, newChild, refChild, null);
            Assert(inserted != null, "insertBefore via JSObject.CallJS<JSObject>(p1, p2, p3) should return non-null");
        }
    }

    public class CallJS_JSObject_Returning_CallerMemberName_4Params : JSTest
    {
        public override void Run()
        {
            StubHtmlIntoTestContainer(0);
            var doc = Document.GetDocument();
            var container = tc.Get(0);
            var a = doc.CreateElement("span").JSObject;
            var b = doc.CreateElement("div").JSObject;
            container.CallJS(funcName: "appendChild", b);
            var inserted = JSObjectCallJSReturnHelper.InsertBefore(container, a, b, null, null);
            Assert(inserted != null, "insertBefore via JSObject.CallJS<JSObject>(p1, p2, p3, p4) should return non-null");
        }
    }

    public class CallJS_JSObject_Returning_CallerMemberName_5Params : JSTest
    {
        public override void Run()
        {
            StubHtmlIntoTestContainer(0);
            var doc = Document.GetDocument();
            var container = tc.Get(0);
            var a = doc.CreateElement("span").JSObject;
            var b = doc.CreateElement("div").JSObject;
            container.CallJS(funcName: "appendChild", b);
            var inserted = JSObjectCallJSReturnHelper.InsertBefore(container, a, b, null, null, null);
            Assert(inserted != null, "insertBefore via JSObject.CallJS<JSObject>(p1, p2, p3, p4, p5) should return non-null");
        }
    }

    // --- JSObject void CallJS inferred name (0..5 params) ---

    public class CallJS_JSObject_Void_CallerMemberName_0Params : JSTest
    {
        public override void Run()
        {
            StubHtmlIntoTestContainer(0);
            var container = tc.Get(0);
            JSObjectCallJSVoidHelper.Blur(container);
            Assert(true, "void CallJS() (0 params) should not throw");
        }
    }

    public class CallJS_JSObject_Void_CallerMemberName_1Param : JSTest
    {
        public override void Run()
        {
            StubHtmlIntoTestContainer(0);
            var doc = Document.GetDocument();
            var container = tc.Get(0);
            var span = doc.CreateElement("span").JSObject;
            JSImportInstanceHelpers.SetProperty(span, "id", "jso-void-1p-id");
            JSObjectCallJSVoidHelper.AppendChild(container, span);
            var found = tc.Find("#jso-void-1p-id").Get(0);
            Assert(found != null, "void CallJS(param1) should have appended the node");
        }
    }

    public class CallJS_JSObject_Void_CallerMemberName_2Params : JSTest
    {
        public override void Run()
        {
            StubHtmlIntoTestContainer(0);
            var doc = Document.GetDocument();
            var container = tc.Get(0);
            var div = doc.CreateElement("div").JSObject;
            JSImportInstanceHelpers.SetProperty(div, "id", "jso-void-2p-id");
            JSObjectCallJSVoidHelper.AppendChild(container, div, null);
            var found = tc.Find("#jso-void-2p-id").Get(0);
            Assert(found != null, "void CallJS(p1, p2) should have appended the node");
        }
    }

    public class CallJS_JSObject_Void_CallerMemberName_3Params : JSTest
    {
        public override void Run()
        {
            StubHtmlIntoTestContainer(0);
            var doc = Document.GetDocument();
            var container = tc.Get(0);
            var div = doc.CreateElement("div").JSObject;
            JSImportInstanceHelpers.SetProperty(div, "id", "jso-void-3p-id");
            JSObjectCallJSVoidHelper.AppendChild(container, div, null, null);
            var found = tc.Find("#jso-void-3p-id").Get(0);
            Assert(found != null, "void CallJS(p1, p2, p3) should have appended the node");
        }
    }

    public class CallJS_JSObject_Void_CallerMemberName_4Params : JSTest
    {
        public override void Run()
        {
            StubHtmlIntoTestContainer(0);
            var doc = Document.GetDocument();
            var container = tc.Get(0);
            var div = doc.CreateElement("div").JSObject;
            JSImportInstanceHelpers.SetProperty(div, "id", "jso-void-4p-id");
            JSObjectCallJSVoidHelper.AppendChild(container, div, null, null, null);
            var found = tc.Find("#jso-void-4p-id").Get(0);
            Assert(found != null, "void CallJS(p1, p2, p3, p4) should have appended the node");
        }
    }

    public class CallJS_JSObject_Void_CallerMemberName_5Params : JSTest
    {
        public override void Run()
        {
            StubHtmlIntoTestContainer(0);
            var doc = Document.GetDocument();
            var container = tc.Get(0);
            var div = doc.CreateElement("div").JSObject;
            JSImportInstanceHelpers.SetProperty(div, "id", "jso-void-5p-id");
            JSObjectCallJSVoidHelper.AppendChild(container, div, null, null, null, null);
            var found = tc.Find("#jso-void-5p-id").Get(0);
            Assert(found != null, "void CallJS(p1, p2, p3, p4, p5) should have appended the node");
        }
    }
}
