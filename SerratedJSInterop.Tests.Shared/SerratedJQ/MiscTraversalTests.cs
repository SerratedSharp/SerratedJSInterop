using SerratedSharp.SerratedJSInterop;
using SerratedSharp.SerratedJQ.Plain;
using System;
using Wasm;

namespace Tests.Wasm;
public partial class TestsContainer
{
    public class MiscTraversal_Add : JSTest
    {
        public override void Run()
        {
            JQueryPlainObject stubs = StubHtmlIntoTestContainer(5);// a,b,c,d,e
            tc.Append("<span class='MiscTraversal_Add'></span>");
            result = tc.Find(".a").Add(".MiscTraversal_Add");
            Assert(result.HasClass("a") && result.HasClass("MiscTraversal_Add"));
            Console.WriteLine($"result: {result.Length}");
            Console.WriteLine($"result: {result.Parent().Html()}");
            Assert(result.Length == 2);
        }
    }

    public class MiscTraversal_Add_Object : JSTest
    {
        public override void Run()
        {
            JQueryPlainObject stubs = StubHtmlIntoTestContainer(5);// a,b,c,d,e
            result = stubs.Filter(".a").Add(stubs.Filter(".b"));
            Assert(result.HasClass("a") && result.HasClass("b"));
            Assert(result.Length == 2);
        }
    }

    public class MiscTraversal_AddSelectorWithContext : JSTest
    {
        public override void Run()
        {
            JQueryPlainObject stubs = StubHtmlIntoTestContainer(5);// a,b,c,d,e
            result = stubs.Filter(".a").Add(".b", tc);
            Assert(result.HasClass("a") && result.HasClass("b"));
            Assert(result.Length == 2);
        }
    }

    public class MiscTraversal_AddBack : JSTest
    {
        public override void Run()
        {
            JQueryPlainObject stubs = StubHtmlIntoTestContainer(5);// a,b,c,d,e
            result = stubs.Filter(".a").Next().AddBack();
            Assert(result.HasClass("a") && result.HasClass("b"));
            Assert(result.Length == 2);
        }
    }

    public class MiscTraversal_AddBackSelector : JSTest
    {
        public override void Run()
        {
            JQueryPlainObject stubs = StubHtmlIntoTestContainer(5);// a,b,c,d,e
            result = stubs.Filter(".a,.c").Next().AddBack(".c");
            Assert(result.HasClass("c") && result.HasClass("b") && result.HasClass("d"));
            // GlobalJS.Console.Log("AddBackSelector", result);
            Assert(result.Length == 3);
        }
    }

    public class MiscTraversal_Contents : JSTest
    {
        public override void Run()
        {
            JQueryPlainObject stubs = StubHtmlIntoTestContainer(5);// a,b,c,d,e        
            tc.Append("TextNode2");
            tc.Append("<div class='y'></div>");
            result = tc.Contents();
            Assert(result.Length == 7);
        }
    }

    public class MiscTraversal_End : JSTest
    {
        public override void Run()
        {
            JQueryPlainObject stubs = StubHtmlIntoTestContainer(5);// a,b,c,d,e
            result = stubs.Filter(".a").Next().End();
            Assert(result.HasClass("a"));
            Assert(result.Length == 1);
        }
    }
}
