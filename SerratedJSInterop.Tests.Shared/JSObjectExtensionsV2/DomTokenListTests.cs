using SerratedSharp.SerratedDom;
using SerratedSharp.SerratedJSInterop;
using Wasm;

namespace Tests.Wasm;

public partial class TestsContainer
{
    public class DOMTokenList_Length_AfterAddRemove : JSTest
    {
        public override void Run()
        {
            StubHtmlIntoTestContainer(0);
            var doc = Document.GetDocument();
            var div = doc.CreateElement("div");
            var classList = div.ClassList;
            Assert(classList.Length == 0, "Length should be 0 initially");
            classList.Add("a");
            Assert(classList.Length == 1, "Length should be 1 after Add(\"a\")");
            classList.Add("b");
            Assert(classList.Length == 2, "Length should be 2 after Add(\"b\")");
            classList.Remove("a");
            Assert(classList.Length == 1, "Length should be 1 after Remove(\"a\")");
        }
    }

    public class DOMTokenList_Contains_AfterAdd : JSTest
    {
        public override void Run()
        {
            StubHtmlIntoTestContainer(0);
            var doc = Document.GetDocument();
            var div = doc.CreateElement("div");
            var classList = div.ClassList;
            classList.Add("foo");
            Assert(classList.Contains("foo") == true, "Contains(\"foo\") should be true after Add(\"foo\")");
            Assert(classList.Contains("bar") == false, "Contains(\"bar\") should be false");
        }
    }

    public class DOMTokenList_Item_ItemByIndex : JSTest
    {
        public override void Run()
        {
            StubHtmlIntoTestContainer(0);
            var doc = Document.GetDocument();
            var div = doc.CreateElement("div");
            var classList = div.ClassList;
            classList.Add("x");
            classList.Add("y");
            Assert(classList.Item(0) == "x", "Item(0) should be \"x\"");
            Assert(classList.ItemByIndex(1) == "y", "ItemByIndex(1) should be \"y\"");
        }
    }

    public class DOMTokenList_RemoveMultiple : JSTest
    {
        public override void Run()
        {
            StubHtmlIntoTestContainer(0);
            var doc = Document.GetDocument();
            var div = doc.CreateElement("div");
            var classList = div.ClassList;
            classList.Add("a");
            classList.Add("b");
            classList.Add("c");
            classList.RemoveMultiple("a", "c");
            Assert(classList.Length == 1, "Length should be 1 after RemoveMultiple(\"a\", \"c\")");
            Assert(classList.Contains("b"), "Should still contain \"b\"");
        }
    }
}
