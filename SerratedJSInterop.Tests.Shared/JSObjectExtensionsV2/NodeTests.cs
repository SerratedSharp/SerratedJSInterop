using SerratedSharp.SerratedDom;
using SerratedSharp.SerratedJSInterop;
using Wasm;

namespace Tests.Wasm;

public partial class TestsContainer
{
    public class Node_InsertBefore_InsertsBeforeReference : JSTest
    {
        public override void Run()
        {
            StubHtmlIntoTestContainer(0);
            var doc = Document.GetDocument();
            var container = doc.CreateElement("div");
            container.Id = "insert-before-container";
            var first = doc.CreateElement("span");
            first.Id = "first";
            var second = doc.CreateElement("span");
            second.Id = "second";

            container.AppendChild(first);
            container.InsertBefore(second, first);

            Assert(container.FirstElementChild != null, "Container should have a first child");
            Assert(container.FirstElementChild!.Id == "second", "InsertBefore(second, first) should make second the first child");
            Assert(container.LastElementChild != null && container.LastElementChild.Id == "first", "First should now be the last child (second is first)");
        }
    }

    public class Node_InsertBefore_NullReference_InsertsAtEnd : JSTest
    {
        public override void Run()
        {
            StubHtmlIntoTestContainer(0);
            var doc = Document.GetDocument();
            var container = doc.CreateElement("div");
            var first = doc.CreateElement("span");
            first.Id = "a";
            var last = doc.CreateElement("span");
            last.Id = "b";

            container.AppendChild(first);
            container.InsertBefore(last, null);

            Assert(container.LastElementChild != null && container.LastElementChild.Id == "b", "InsertBefore(last, null) should append at end");
        }
    }
}
