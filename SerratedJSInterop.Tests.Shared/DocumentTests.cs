using SerratedSharp.SerratedDom;
using SerratedSharp.SerratedJSInterop;
using SerratedSharp.SerratedJQ.Plain;
using Wasm;

namespace Tests.Wasm;

public partial class TestsContainer
{
    public class Document_GetDocument_NotNull : JQTest
    {
        public override void Run()
        {
            var doc = Document.GetDocument();
            Assert(doc != null, "GetDocument() should not return null");
            Assert(doc.JSObject != null, "Document JSObject should not be null");
        }
    }

    public class Document_Body_NotNull : JQTest
    {
        public override void Run()
        {
            var doc = Document.GetDocument();
            var body = doc.Body;
            Assert(body != null, "Document.Body should not be null");
            Assert(body.JSObject != null, "Body JSObject should not be null");
        }
    }

    public class Document_CreateElement_Div : JQTest
    {
        public override void Run()
        {
            var doc = Document.GetDocument();
            var div = doc.CreateElement("div");
            Assert(div != null, "CreateElement(\"div\") should not return null");
            Assert(div.JSObject != null, "Created element JSObject should not be null");
            var tagName = div.GetProperty<string>("tagName");
            Assert(tagName == "DIV", "Created element tagName should be DIV");
        }
    }

    public class Document_CreateElement_Span : JQTest
    {
        public override void Run()
        {
            var doc = Document.GetDocument();
            var span = doc.CreateElement("span");
            Assert(span != null, "CreateElement(\"span\") should not return null");
            var tagName = span.GetProperty<string>("tagName");
            Assert(tagName == "SPAN", "Created element tagName should be SPAN");
        }
    }

    public class Document_GetElementById : JQTest
    {
        public override void Run()
        {
            StubHtmlIntoTestContainer(0);
            tc.Append("<div id='doc-getbyid-target'>doc-test-content</div>");
            var doc = Document.GetDocument();
            var el = doc.GetElementById("doc-getbyid-target");
            Assert(el != null, "GetElementById should return non-null for existing id");
            Assert(el.JSObject != null, "Element JSObject should not be null");
            Assert(el.TextContent == "doc-test-content", "Element TextContent should match");
        }
    }

    public class Document_GetElementById_Missing : JQTest
    {
        public override void Run()
        {
            var doc = Document.GetDocument();
            var el = doc.GetElementById("id-that-does-not-exist-xyz");
            Assert(el == null, "GetElementById should return null for missing id");
        }
    }
}
