using SerratedSharp.SerratedDom;
using SerratedSharp.SerratedJSInterop;
using SerratedSharp.SerratedJQ.Plain;
using Wasm;

namespace Tests.Wasm;

public partial class TestsContainer
{
    public class Document_GetDocument_NotNull : JSTest
    {
        public override void Run()
        {
            var doc = Document.GetDocument();
            Assert(doc != null, "GetDocument() should not return null");
            Assert(doc.JSObject != null, "Document JSObject should not be null");
        }
    }

    public class Document_Body_NotNull : JSTest
    {
        public override void Run()
        {
            var doc = Document.GetDocument();
            var body = doc.Body;
            Assert(body != null, "Document.Body should not be null");
            Assert(body.JSObject != null, "Body JSObject should not be null");
        }
    }

    public class Document_CreateElement_Div : JSTest
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

    public class Document_CreateElement_Span : JSTest
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

    public class Document_GetElementById : JSTest
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

    public class Document_GetElementById_Missing : JSTest
    {
        public override void Run()
        {
            var doc = Document.GetDocument();
            var el = doc.GetElementById("id-that-does-not-exist-xyz");
            Assert(el == null, "GetElementById should return null for missing id");
        }
    }

    public class Document_DocumentElement_NotNull : JSTest
    {
        public override void Run()
        {
            var doc = Document.GetDocument();
            var root = doc.DocumentElement;
            Assert(root != null, "DocumentElement should not be null");
            var tagName = root.GetProperty<string>("tagName");
            Assert(tagName == "HTML", "DocumentElement tagName should be HTML");
        }
    }

    public class Document_Head_NotNull : JSTest
    {
        public override void Run()
        {
            var doc = Document.GetDocument();
            var head = doc.Head;
            Assert(head != null, "Head should not be null");
            Assert(head.JSObject != null, "Head JSObject should not be null");
        }
    }

    public class Document_DocumentURI_NonEmpty : JSTest
    {
        public override void Run()
        {
            var doc = Document.GetDocument();
            var uri = doc.DocumentURI;
            Assert(uri != null, "DocumentURI should not be null");
            Assert(uri.Length > 0, "DocumentURI should be non-empty");
        }
    }

    public class Document_CharacterSet_NonEmpty : JSTest
    {
        public override void Run()
        {
            var doc = Document.GetDocument();
            var cs = doc.CharacterSet;
            Assert(cs != null, "CharacterSet should not be null");
            Assert(cs.Length > 0, "CharacterSet should be non-empty");
        }
    }

    public class Document_QuerySelector_ById : JSTest
    {
        public override void Run()
        {
            StubHtmlIntoTestContainer(0);
            tc.Append("<div id='qsel-target'>qsel-content</div>");
            var doc = Document.GetDocument();
            var el = doc.QuerySelector("#qsel-target");
            Assert(el != null, "QuerySelector should return non-null for existing selector");
            Assert(el!.TextContent == "qsel-content", "QuerySelector element TextContent should match");
        }
    }

    public class Document_QuerySelector_Missing : JSTest
    {
        public override void Run()
        {
            var doc = Document.GetDocument();
            var el = doc.QuerySelector("#nonexistent-xyz-123");
            Assert(el == null, "QuerySelector should return null for missing selector");
        }
    }

    public class Document_CreateElement_ExplicitFuncName : JSTest
    {
        public override void Run()
        {
            var doc = Document.GetDocument();
            var div = doc.CallJS<HtmlElement>("createElement", "div");
            Assert(div != null, "createElement via CallJS(string, params object[]) should return non-null");
            var tagName = div.GetProperty<string>("tagName");
            Assert(tagName == "DIV", "Created element tagName should be DIV");
        }
    }
}
