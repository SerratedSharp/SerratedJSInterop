using SerratedSharp.SerratedDom;
using SerratedSharp.SerratedJSInterop;
using Wasm;

namespace Tests.Wasm;

public partial class TestsContainer
{
    public class HtmlElement_Id_GetSet : JSTest
    {
        public override void Run()
        {
            StubHtmlIntoTestContainer(0);
            var doc = Document.GetDocument();
            var div = doc.CreateElement("div");
            div.Id = "test-id-1";
            Assert(div.Id == "test-id-1", "Id should round-trip after set");
        }
    }

    public class HtmlElement_ClassName_GetSet : JSTest
    {
        public override void Run()
        {
            StubHtmlIntoTestContainer(0);
            var doc = Document.GetDocument();
            var div = doc.CreateElement("div");
            div.ClassName = "test-class";
            Assert(div.ClassName == "test-class", "ClassName should round-trip after set");
        }
    }

    public class HtmlElement_ParentElement_AfterAppend : JSTest
    {
        public override void Run()
        {
            StubHtmlIntoTestContainer(0);
            var doc = Document.GetDocument();
            var body = doc.Body;
            var div = doc.CreateElement("div");
            div.Id = "parent-test-child";
            body.AppendChild(div);
            Assert(div.ParentElement != null, "ParentElement should be non-null after append to body");
            Assert(div.ParentElement!.JSObject != null, "ParentElement JSObject should not be null");
        }
    }

    public class HtmlElement_FirstElementChild_LastElementChild : JSTest
    {
        public override void Run()
        {
            StubHtmlIntoTestContainer(0);
            var doc = Document.GetDocument();
            var container = doc.CreateElement("div");
            container.Id = "container-fe";
            var first = doc.CreateElement("span");
            first.Id = "first-child";
            var last = doc.CreateElement("span");
            last.Id = "last-child";
            container.AppendChild(first);
            container.AppendChild(last);
            doc.Body.AppendChild(container);
            Assert(container.FirstElementChild != null, "FirstElementChild should be non-null");
            Assert(container.FirstElementChild!.Id == "first-child", "FirstElementChild should match first appended");
            Assert(container.LastElementChild != null, "LastElementChild should be non-null");
            Assert(container.LastElementChild!.Id == "last-child", "LastElementChild should match last appended");
        }
    }

    public class HtmlElement_OffsetWidth_OffsetHeight : JSTest
    {
        public override void Run()
        {
            StubHtmlIntoTestContainer(0);
            var doc = Document.GetDocument();
            var div = doc.CreateElement("div");
            doc.Body.AppendChild(div);
            var w = div.OffsetWidth;
            var h = div.OffsetHeight;
            Assert(w >= 0, "OffsetWidth should be non-negative");
            Assert(h >= 0, "OffsetHeight should be non-negative");
        }
    }

    public class HtmlElement_ClientWidth_ClientHeight : JSTest
    {
        public override void Run()
        {
            StubHtmlIntoTestContainer(0);
            var doc = Document.GetDocument();
            var div = doc.CreateElement("div");
            doc.Body.AppendChild(div);
            var w = div.ClientWidth;
            var h = div.ClientHeight;
            Assert(w >= 0, "ClientWidth should be non-negative");
            Assert(h >= 0, "ClientHeight should be non-negative");
        }
    }

    public class HtmlElement_AppendChild_Void : JSTest
    {
        public override void Run()
        {
            StubHtmlIntoTestContainer(0);
            var doc = Document.GetDocument();
            var body = doc.Body;
            var div = doc.CreateElement("div");
            div.Id = "append-void-target";
            body.AppendChild(div);
            var found = doc.GetElementById("append-void-target");
            Assert(found != null, "AppendChild (void) should have appended the node so GetElementById finds it");
        }
    }

    public class HtmlElement_RemoveChild_ReturnsRemoved : JSTest
    {
        public override void Run()
        {
            StubHtmlIntoTestContainer(0);
            var doc = Document.GetDocument();
            var parent = doc.CreateElement("div");
            parent.Id = "remove-parent";
            var child = doc.CreateElement("span");
            child.Id = "remove-child";
            parent.AppendChild(child);
            doc.Body.AppendChild(parent);
            var removed = parent.RemoveChild(child);
            Assert(removed != null, "RemoveChild should return non-null");
            Assert(removed.Id == "remove-child", "Removed element Id should match");
            var foundInDoc = doc.GetElementById("remove-child");
            Assert(foundInDoc == null, "Removed child should no longer be in document");
        }
    }

    public class HtmlElement_SetAttribute_GetAttribute : JSTest
    {
        public override void Run()
        {
            StubHtmlIntoTestContainer(0);
            var doc = Document.GetDocument();
            var el = doc.CreateElement("div");
            el.SetAttribute("data-foo", "bar");
            var value = el.GetAttribute("data-foo");
            Assert(value == "bar", "GetAttribute should return value set by SetAttribute");
        }
    }
}
