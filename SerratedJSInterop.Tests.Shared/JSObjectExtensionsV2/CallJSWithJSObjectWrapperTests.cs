using SerratedSharp.SerratedDom;
using SerratedSharp.SerratedJSInterop;
using Wasm;

namespace Tests.Wasm;

public partial class TestsContainer
{
    /// <summary>
    /// Helper so that IJSObjectWrapper.CallJS(JSParams, [CallerMemberName]) is invoked with funcName = "AppendChild" (→ "appendChild" in JS).
    /// </summary>
    private static class VoidCallJSHelperWrapper
    {
        public static void AppendChild(IJSObjectWrapper parent, object child)
        {
            parent.CallJS(SerratedJS.Params(child));
        }
    }

    public class CallJS_Wrapper_Returning_ExplicitFuncName_ParamsArray : JSTest
    {
        public override void Run()
        {
            StubHtmlIntoTestContainer(0);
            var doc = Document.GetDocument();
            var div = doc.CallJS<HtmlElement>("createElement", "div");
            Assert(div != null, "createElement via IJSObjectWrapper.CallJS<HtmlElement>(string, params object[]) should return non-null");
            Assert(div!.JSObject != null, "Created element JSObject should not be null");
            var tagName = div.GetProperty<string>("tagName");
            Assert(tagName == "DIV", "Created element tagName should be DIV");
        }
    }

    public class CallJS_Wrapper_Void_CallerMemberName_AppendChild : JSTest
    {
        public override void Run()
        {
            StubHtmlIntoTestContainer(0);
            var doc = Document.GetDocument();
            var body = doc.Body;
            var span = doc.CreateElement("span");
            JSImportInstanceHelpers.SetProperty(span.JSObject, "id", "calljs-wrapper-void-callername-id");

            VoidCallJSHelperWrapper.AppendChild(body, span);

            var found = doc.GetElementById("calljs-wrapper-void-callername-id");
            Assert(found != null, "appendChild via IJSObjectWrapper.CallJS(JSParams, CallerMemberName) should have appended the node");
        }
    }

    public class CallJS_Wrapper_Void_ExplicitFuncName_JSParams : JSTest
    {
        public override void Run()
        {
            StubHtmlIntoTestContainer(0);
            var doc = Document.GetDocument();
            var body = doc.Body;
            var div = doc.CreateElement("div");
            JSImportInstanceHelpers.SetProperty(div.JSObject, "id", "calljs-wrapper-void-explicit-jsparams-id");

            body.CallJS("appendChild", SerratedJS.Params(div));

            var found = doc.GetElementById("calljs-wrapper-void-explicit-jsparams-id");
            Assert(found != null, "appendChild via IJSObjectWrapper.CallJS(string, JSParams) should have appended the node");
        }
    }

    public class CallJS_Wrapper_Void_ExplicitFuncName_ParamsArray : JSTest
    {
        public override void Run()
        {
            StubHtmlIntoTestContainer(0);
            var doc = Document.GetDocument();
            var body = doc.Body;
            var div = doc.CreateElement("div");
            JSImportInstanceHelpers.SetProperty(div.JSObject, "id", "calljs-wrapper-void-params-id");

            body.CallJS("appendChild", div);

            var found = doc.GetElementById("calljs-wrapper-void-params-id");
            Assert(found != null, "appendChild via IJSObjectWrapper.CallJS(string, params object[]) should have appended the node");
        }
    }
}
