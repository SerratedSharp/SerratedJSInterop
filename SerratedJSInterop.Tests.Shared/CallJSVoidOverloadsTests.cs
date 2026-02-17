using SerratedSharp.SerratedDom;
using SerratedSharp.SerratedJSInterop;
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

    public class CallJS_Void_ExplicitFuncName_AppendChild : JQTest
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

    public class CallJS_Void_CallerMemberName_AppendChild : JQTest
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
}
