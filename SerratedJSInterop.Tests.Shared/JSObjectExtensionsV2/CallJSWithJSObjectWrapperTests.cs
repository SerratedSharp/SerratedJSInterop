using SerratedSharp.SerratedDom;
using SerratedSharp.SerratedJSInterop;
using SerratedSharp.SerratedJQ.Plain;
using Wasm;

namespace Tests.Wasm;

public partial class TestsContainer
{
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
            var div = doc.CallJS<HtmlElement>(funcName:"createElement", "div");
            Assert(div != null, "createElement via IJSObjectWrapper.CallJS<HtmlElement>(string, params object[]) should return non-null");
            Assert(div!.JSObject != null, "Created element JSObject should not be null");
            var tagName = div.GetJSProperty<string>("tagName");
            Assert(tagName == "DIV", "Created element tagName should be DIV");
        }
    }

    public class CallJS_Wrapper_Void_CallerMemberName_AppendChild : JSTest
    {
        public override void Run()
        {
            StubHtmlIntoTestContainer(0);
            var doc = Document.GetDocument();
            var container = new HtmlElement(tc.Get(0));
            var span = doc.CreateElement("span");
            JSImportInstanceHelpers.SetProperty(span.JSObject, "id", "calljs-wrapper-void-callername-id");

            VoidCallJSHelperWrapper.AppendChild(container, span);

            var found = tc.Find("#calljs-wrapper-void-callername-id").Get(0);
            Assert(found != null, "appendChild via IJSObjectWrapper.CallJS(JSParams, CallerMemberName) should have appended the node");
        }
    }

    public class CallJS_Wrapper_Void_ExplicitFuncName_JSParams : JSTest
    {
        public override void Run()
        {
            StubHtmlIntoTestContainer(0);
            var doc = Document.GetDocument();
            var container = new HtmlElement(tc.Get(0));
            var div = doc.CreateElement("div");
            JSImportInstanceHelpers.SetProperty(div.JSObject, "id", "calljs-wrapper-void-explicit-jsparams-id");

            container.CallJS(funcName: "appendChild", SerratedJS.Params(div));

            var found = tc.Find("#calljs-wrapper-void-explicit-jsparams-id").Get(0);
            Assert(found != null, "appendChild via IJSObjectWrapper.CallJS(string, JSParams) should have appended the node");
        }
    }

    public class CallJS_Wrapper_Void_ExplicitFuncName_ParamsArray : JSTest
    {
        public override void Run()
        {
            StubHtmlIntoTestContainer(0);
            var doc = Document.GetDocument();
            var container = new HtmlElement(tc.Get(0));
            var div = doc.CreateElement("div");
            JSImportInstanceHelpers.SetProperty(div.JSObject, "id", "calljs-wrapper-void-params-id");

            container.CallJS(funcName: "appendChild", div);

            var found = tc.Find("#calljs-wrapper-void-params-id").Get(0);
            Assert(found != null, "appendChild via IJSObjectWrapper.CallJS(string, params object[]) should have appended the node");
        }
    }
}
