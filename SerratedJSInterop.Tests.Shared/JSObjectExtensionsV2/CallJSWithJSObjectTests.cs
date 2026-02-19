using SerratedSharp.SerratedDom;
using SerratedSharp.SerratedJSInterop;
using System.Runtime.InteropServices.JavaScript;
using Wasm;

namespace Tests.Wasm;

public partial class TestsContainer
{
    /// <summary>
    /// Helper so that JSObject.CallJS(JSParams, [CallerMemberName]) is invoked with funcName = "AppendChild" (→ "appendChild" in JS).
    /// </summary>
    private static class VoidCallJSHelper
    {
        public static void AppendChild(JSObject parent, JSObject child)
        {
            //parent.CallJS< .CallJS<JSObject>(

            parent.CallJS(SerratedJS.Params(child));
        }
    }

    public class CallJS_JSObject_Returning_ExplicitFuncName_ParamsArray : JSTest
    {
        public override void Run()
        {
            StubHtmlIntoTestContainer(0);
            var doc = Document.GetDocument();
            var span = doc.JSObject.CallJS<JSObject>("createElement", "span");
            Assert(span != null, "createElement via JSObject.CallJS<JSObject>(string, params object[]) should return non-null");
            var tagName = span.GetProperty<string>("tagName");
            Assert(tagName == "SPAN", "Created element tagName should be SPAN");
        }
    }

    public class CallJS_JSObject_Void_ExplicitFuncName_JSParams : JSTest
    {
        public override void Run()
        {
            StubHtmlIntoTestContainer(0);
            var doc = Document.GetDocument();
            var body = doc.Body.JSObject;
            var div = doc.CreateElement("div").JSObject;
            JSImportInstanceHelpers.SetProperty(div, "id", "void-calljs-explicit-id");

            body.CallJS("appendChild", SerratedJS.Params(div));

            var found = doc.GetElementById("void-calljs-explicit-id");
            Assert(found != null, "appendChild via void CallJS(string, JSParams) should have appended the node");
            Assert(found.TextContent == "", "Appended element should be in DOM");
        }
    }

    public class CallJS_JSObject_Void_CallerMemberName_AppendChild : JSTest
    {
        public override void Run()
        {
            StubHtmlIntoTestContainer(0);
            var doc = Document.GetDocument();
            var body = doc.Body.JSObject;
            var span = doc.CreateElement("span").JSObject;
            JSImportInstanceHelpers.SetProperty(span, "id", "void-calljs-callername-id");

            VoidCallJSHelper.AppendChild(body, span);

            var found = doc.GetElementById("void-calljs-callername-id");
            Assert(found != null, "appendChild via void CallJS(JSParams, CallerMemberName) should have appended the node");
        }
    }

    public class CallJS_JSObject_Void_ExplicitFuncName_ParamsArray : JSTest
    {
        public override void Run()
        {
            StubHtmlIntoTestContainer(0);
            var doc = Document.GetDocument();
            var body = doc.Body.JSObject;
            var div = doc.CreateElement("div").JSObject;
            JSImportInstanceHelpers.SetProperty(div, "id", "calljs-jsobject-void-params-id");

            body.CallJS("appendChild", div);

            var found = doc.GetElementById("calljs-jsobject-void-params-id");
            Assert(found != null, "appendChild via JSObject.CallJS(string, params object[]) should have appended the node");
        }
    }
}
