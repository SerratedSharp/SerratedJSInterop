using SerratedSharp.SerratedDom;
using SerratedSharp.SerratedJSInterop;
using SerratedSharp.SerratedJQ.Plain;
using System.ComponentModel;
using System.Runtime.InteropServices.JavaScript;
using Wasm;

namespace Tests.Wasm;

public partial class TestsContainer
{
    public class DOMTokenList_Length_AfterAddRemove : JSTest
    {
        public override void Run()
        {
            // Arrange: stub in test container; get underlying DOM element via .Get(0) and wrap as HtmlElement
            StubHtmlIntoTestContainer(1);
            result = tc.Children();
            JSObject firstChildJs = tc.Children().Get(0);
            var el = new HtmlElement(firstChildJs);
            var classList = el.ClassList;

            // Initially the stub has one class "a"
            Assert(classList.Length == 1, "Length should be 1 initially for stub '.a'");

            classList.Add("b");
            Assert(classList.Length == 2, "Length should be 2 after Add(\"b\")");

            classList.Add("c");
            Assert(classList.Length == 3, "Length should be 3 after Add(\"c\")");

            classList.Remove("a");
            Assert(classList.Length == 2, "Length should be 2 after Remove(\"a\")");
        }
    }

    public class DOMTokenList_Contains_AfterAdd : JSTest
    {
        public override void Run()
        {
            // Arrange: stub in test container; get underlying DOM element via .Get(0) and wrap as HtmlElement
            StubHtmlIntoTestContainer(1);
            result = tc.Children();
            JSObject firstChildJs = tc.Children().Get(0);
            var el = new HtmlElement(firstChildJs);
            var classList = el.ClassList;

            classList.Add("foo");

            Assert(classList.Contains("a") == true, "Contains(\"a\") should be true for stub '.a'");
            Assert(classList.Contains("foo") == true, "Contains(\"foo\") should be true after Add(\"foo\")");
            Assert(classList.Contains("bar") == false, "Contains(\"bar\") should be false");
        }
    }

    public class DOMTokenList_Item_ItemByIndex : JSTest
    {
        public override void Run()
        {
            // Arrange: stub element in test container; get underlying DOM element via .Get(0) and wrap as HtmlElement
            StubHtmlIntoTestContainer(1);
            result = tc.Children();
            JSObject firstChildJs = tc.Children().Get(0);
            var el = new HtmlElement(firstChildJs);
            var classList = el.ClassList;

            classList.Add("x");
            classList.Add("y");

            Assert(classList.Item(0) == "a", "Item(0) should be \"a\" from the stub");
            Assert(classList.ItemByIndex(1) == "x", "ItemByIndex(1) should be \"x\"");
            Assert(classList.ItemByIndex(2) == "y", "ItemByIndex(2) should be \"y\"");
        }
    }

    public class DOMTokenList_RemoveMultiple : JSTest
    {
        public override void Run()
        {
            // Arrange: stub element in test container; get underlying DOM element via .Get(0) and wrap as HtmlElement
            StubHtmlIntoTestContainer(1);
            result = tc.Children();
            JSObject firstChildJs = tc.Children().Get(0);
            var el = new HtmlElement(firstChildJs);
            var classList = el.ClassList;

            classList.Add("b");
            classList.Add("c");
            classList.RemoveMultiple("a", "c");

            Assert(classList.Length == 1, "Length should be 1 after RemoveMultiple(\"a\", \"c\") starting from classes \"a b c\"");
            Assert(classList.Contains("b"), "Should still contain \"b\"");
        }
    }
}
