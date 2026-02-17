using SerratedSharp.SerratedJQ.Plain;
using System;
using Wasm;

namespace Tests.Wasm;
public partial class TestsContainer
{
    public class TreeTraversal_Children : JSTest
    {
        public override void Run()
        {
            JQueryPlainObject stubs = StubHtmlIntoTestContainer(5);// a,b,c,d,e
            Assert(stubs.HasClass("a") && stubs.HasClass("e"));
            Assert(stubs.Length == 5);
        }
    }

    public class TreeTraversal_Children_Selector : JSTest
    {
        public override void Run()
        {
            StubHtmlIntoTestContainer(5);// a,b,c,d,e
            result = tc.Children(".a,.e");
            Assert(result.HasClass("a") && result.HasClass("e"));
            Assert(result.Length == 2);
        }
    }

    public class TreeTraversal_Closest : JSTest
    {
        public override void Run()
        {
            JQueryPlainObject stubs = StubHtmlIntoTestContainer(1);
            result = stubs.Closest(".tc");
            Assert(result.HasClass("tc"));
            Assert(result.Length == 1);
        }
    }

    public class TreeTraversal_Find : JSTest
    {
        public override void Run()
        {
            JQueryPlainObject stubs = StubHtmlIntoTestContainer(5);// a,b,c,d,e
            result = tc.Find(".a,.e");
            Assert(result.HasClass("a") && result.HasClass("e"));
            Assert(result.Length == 2);
        }
    }

    public class TreeTraversal_Next : JSTest
    {
        public override void Run()
        {
            JQueryPlainObject stubs = StubHtmlIntoTestContainer(5);// a,b,c,d,e
            result = stubs.Filter(".b").Next();
            Assert(result.HasClass("c"));
            Assert(result.Length == 1);
        }
    }

    public class TreeTraversal_Next_Selector : JSTest
    {
        public override void Run()
        {
            JQueryPlainObject stubs1 = StubHtmlIntoTestContainer(5);
            stubs1.Remove(".c");// a,b,d,e
            StubHtmlIntoTestContainer(5);// a,b,d,e,a,b,c,d,e
            StubHtmlIntoTestContainer(5);// a,b,d,e,a,b,c,d,e,a,b,c,d,e
            result = tc.Children().Filter(".b").Next(".c");
            Assert(result.HasClass("c"));
            Assert(result.Length == 2);
        }
    }

    public class TreeTraversal_NextAll : JSTest
    {
        public override void Run()
        {
            JQueryPlainObject stubs = StubHtmlIntoTestContainer(5);// a,b,c,d,e
            result = stubs.NextAll();
            Assert(result.HasClass("b") && result.HasClass("c") && result.HasClass("d") && result.HasClass("e"));
            Assert(result.Length == 4);
        }
    }

    public class TreeTraversal_NextAll_Selector : JSTest
    {
        public override void Run()
        {
            JQueryPlainObject stubs = StubHtmlIntoTestContainer(5);// a,b,c,d,e
            result = stubs.Filter(".c").NextAll("div");
            Assert(result.HasClass("d") && result.HasClass("e"));
            Assert(result.Length == 2);
        }
    }

    public class TreeTraversal_NextUntil : JSTest
    {
        public override void Run()
        {
            JQueryPlainObject stubs = StubHtmlIntoTestContainer(5);// a,b,c,d,e
            result = stubs.Filter(".b").NextUntil(".e");
            Assert(result.HasClass("c") && result.HasClass("d"));
            Assert(result.Length == 2);
        }
    }

    public class TreeTraversal_NextUntil_Selector : JSTest
    {
        public override void Run()
        {
            JQueryPlainObject stubs = StubHtmlIntoTestContainer(5);// a,b,c,d,e
            result = stubs.Filter(".b").NextUntil(".e", ".c");
            Assert(result.HasClass("c"));
            Assert(result.Length == 1);
        }
    }

    public class TreeTraversal_Parent : JSTest
    {
        public override void Run()
        {
            JQueryPlainObject stubs = StubHtmlIntoTestContainer(1);
            result = stubs.Parent();
            Assert(result.HasClass("tc"));
            Assert(result.Length == 1);
        }
    }

    public class TreeTraversal_Parent_Selector : JSTest
    {
        public override void Run()
        {
            JQueryPlainObject stubs = StubHtmlIntoTestContainer(1);
            result = stubs.Parent(".tc");
            Assert(result.HasClass("tc"));
            Assert(result.Length == 1);
        }
    }

    public class TreeTraversal_Parents : JSTest
    {
        public override void Run()
        {
            JQueryPlainObject stubs = StubHtmlIntoTestContainer(1);
            result = stubs.Parents();
            Assert(result.HasClass("tc"));
            Assert(result.Length > 2);
        }
    }

    public class TreeTraversal_Parents_Selector : JSTest
    {
        public override void Run()
        {
            JQueryPlainObject stubs = StubHtmlIntoTestContainer(1);
            result = stubs.Parents(".tc");
            Assert(result.HasClass("tc"));
            Assert(result.Length == 1);
        }
    }

    public class TreeTraversal_ParentsUntil : JSTest
    {
        public override void Run()
        {
            JQueryPlainObject stubs = StubHtmlIntoTestContainer(1);
            result = stubs.ParentsUntil();
            Assert(result.HasClass("tc"));
            Assert(result.Length > 2);
        }
    }

    public class TreeTraversal_ParentsUntil_Selector : JSTest
    {
        public override void Run()
        {
            JQueryPlainObject stubs = StubHtmlIntoTestContainer(2);
            stubs.First().Append("<div class='x'></div>");
            result = tc.Find(".x").ParentsUntil(".tc");
            Assert(result.HasClass("a"));
            Assert(result.Length == 1);
        }
    }

    public class TreeTraversal_Prev : JSTest
    {
        public override void Run()
        {
            JQueryPlainObject stubs = StubHtmlIntoTestContainer(5);// a,b,c,d,e
            result = stubs.Filter(".d").Prev();
            Assert(result.HasClass("c"));
            Assert(result.Length == 1);
        }
    }

    public class TreeTraversal_Prev_Selector : JSTest
    {
        public override void Run()
        {
            JQueryPlainObject stubs1 = StubHtmlIntoTestContainer(5);
            stubs1.Remove(".c");// a,b,d,e
            StubHtmlIntoTestContainer(5);// a,b,d,e,a,b,c,d,e
            StubHtmlIntoTestContainer(5);// a,b,d,e,a,b,c,d,e,a,b,c,d,e
            result = tc.Children().Filter(".c").Prev(".b");
            Assert(result.HasClass("b"));
            Assert(result.Length == 2);
        }
    }

    public class TreeTraversal_PrevAll : JSTest
    {
        public override void Run()
        {
            JQueryPlainObject stubs = StubHtmlIntoTestContainer(5);// a,b,c,d,e
            result = stubs.PrevAll();
            Assert(result.HasClass("a") && result.HasClass("b") && result.HasClass("c") && result.HasClass("d"));
            Assert(result.Length == 4);
        }
    }

    public class TreeTraversal_PrevAll_Selector : JSTest
    {
        public override void Run()
        {
            JQueryPlainObject stubs = StubHtmlIntoTestContainer(5);// a,b,c,d,e
            result = stubs.Filter(".c").PrevAll("div");
            Assert(result.HasClass("b") && result.HasClass("a"));
            Assert(result.Length == 2);
        }
    }

    public class TreeTraversal_PrevUntil : JSTest
    {
        public override void Run()
        {
            JQueryPlainObject stubs = StubHtmlIntoTestContainer(5);// a,b,c,d,e
            result = stubs.Filter(".d").PrevUntil(".a");
            Assert(result.HasClass("c") && result.HasClass("b"));
            Assert(result.Length == 2);
        }
    }

    public class TreeTraversal_PrevUntil_Selector : JSTest
    {
        public override void Run()
        {
            JQueryPlainObject stubs = StubHtmlIntoTestContainer(5);
            result = stubs.Filter(".d").PrevUntil(".a", ".c");
            Assert(result.HasClass("c"));
            Assert(result.Length == 1);
        }
    }

    public class TreeTraversal_Siblings : JSTest
    {
        public override void Run()
        {
            JQueryPlainObject stubs = StubHtmlIntoTestContainer(5);
            result = stubs.Filter(".c").Siblings();
            Assert(result.HasClass("a") && result.HasClass("e"));
            Assert(result.Length == 4);
        }
    }

    public class TreeTraversal_Siblings_Selector : JSTest
    {
        public override void Run()
        {
            JQueryPlainObject stubs = StubHtmlIntoTestContainer(5);
            result = stubs.Filter(".c").Siblings(".b");
            Assert(result.HasClass("b"));
            Assert(result.Length == 1);
        }
    }
}
