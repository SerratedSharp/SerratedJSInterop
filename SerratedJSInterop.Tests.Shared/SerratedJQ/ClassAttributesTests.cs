using SerratedSharp.SerratedJSInterop;
using SerratedSharp.SerratedJQ.Plain;
using System;
using Wasm;

namespace Tests.Wasm;

public partial class TestsContainer
{

    // Class Attributes - https://api.jquery.com/category/manipulation/class-attribute/

    public class ClassAttributes_HasClass : JSTest
    {
        public override void Run()
        {
            JQueryPlainObject stubs = StubHtmlIntoTestContainer(1);
            result = tc.Children();
            Assert(result.HasClass("a"));
            Assert(result.Length == 1);
        }
    }

    public class ClassAttributes_AddClass : JSTest
    {
        public override void Run()
        {
            JQueryPlainObject stubs = StubHtmlIntoTestContainer(1);
            result = tc.Children();
            result.AddClass("x");
            Assert(result.HasClass("a") && result.HasClass("x"));
            Assert(result.Length == 1);
        }
    }

    public class ClassAttributes_AddClass_Array : JSTest
    {
        public override void Run()
        {
            JQueryPlainObject stubs = StubHtmlIntoTestContainer(1);
            result = tc.Children();
            result.AddClass(new string[] { "x", "y", "z" });
            Assert(result.HasClass("a") && result.HasClass("x") && result.HasClass("y") && result.HasClass("z"));
            Assert(result.Length == 1);
        }
    }

    public class ClassAttributes_RemoveClass : JSTest
    {
        public override void Run()
        {
            JQueryPlainObject stubs = StubHtmlIntoTestContainer(1);
            result = tc.Children();
            result.AddClass("x");
            result.RemoveClass("a");
            Assert(!result.HasClass("a") && result.HasClass("x"));
            Assert(result.Length == 1);
        }
    }

    public class ClassAttributes_RemoveClass_Array : JSTest
    {
        public override void Run()
        {
            JQueryPlainObject stubs = StubHtmlIntoTestContainer(1);
            result = tc.Children();
            result.AddClass("x");
            result.AddClass("y");
            result.AddClass("z");
            result.RemoveClass(new string[] { "a", "x" });
            Assert(!result.HasClass("a") && !result.HasClass("x") && result.HasClass("y") && result.HasClass("z"));
            Assert(result.Length == 1);
        }
    }

    public class ClassAttributes_ToggleClass : JSTest
    {
        public override void Run()
        {
            JQueryPlainObject stubs = StubHtmlIntoTestContainer(1);
            result = tc.Children();
            result.ToggleClass("x");
            Assert(result.HasClass("a") && result.HasClass("x"));
            Assert(result.Length == 1);
        }
    }

    public class ClassAttributes_ToggleClass_WithForce : JSTest
    {
        public override void Run()
        {
            JQueryPlainObject stubs = StubHtmlIntoTestContainer(1);
            result = tc.Children();
            result.ToggleClass("a", true);
            Assert(result.HasClass("a"));
            Assert(result.Length == 1);
        }
    }

    public class ClassAttributes_ToggleClass_WithForceFalse : JSTest
    {
        public override void Run()
        {
            JQueryPlainObject stubs = StubHtmlIntoTestContainer(1);
            result = tc.Children();
            result.ToggleClass("a", false);
            Assert(!result.HasClass("a"));
            Assert(result.Length == 1);
        }
    }

    public class ClassAttributes_ToggleClass_WithForceTrueAndForceFalse : JSTest
    {
        public override void Run()
        {
            JQueryPlainObject stubs = StubHtmlIntoTestContainer(1);
            result = tc.Children();
            result.ToggleClass("a", true);
            result.ToggleClass("a", false);
            Assert(!result.HasClass("a"));
            Assert(result.Length == 1);
        }
    }

    public class ClassAttributes_ToggleClass_Array : JSTest
    {
        public override void Run()
        {
            JQueryPlainObject stubs = StubHtmlIntoTestContainer(1);
            result = tc.Children();
            result.ToggleClass(new string[] { "a", "y", "z" });
            Assert(!result.HasClass("a") && result.HasClass("y") && result.HasClass("z"));
            Assert(result.Length == 1);
        }
    }

}
