using SerratedSharp.SerratedJSInterop;
using SerratedSharp.SerratedJQ.Plain;
using System;
using System.Runtime.InteropServices.JavaScript;
using Wasm;

namespace Tests.Wasm;

public partial class TestsContainer
{
    public class InstanceProperties_Length : JSTest
    {
        public override void Run()
        {
            JQueryPlainObject stubs = StubHtmlIntoTestContainer(3);
            result = tc.Children();
            Assert(result.Length == 3);
        }
    }

    public class InstanceProperties_JQueryVersion : JSTest
    {
        public override void Run()
        {
            JQueryPlainObject stubs = StubHtmlIntoTestContainer(3);
            result = tc.Children();
            Assert(result.JQueryVersion == "3.7.1");
        }
    }

    // Updated test: use jQuery append, avoid calling appendChild on jQuery object
    public class InstanceProperties_AsWrapped_ParentElement : JSTest
    {
        public override void Run()
        {
            // Initialize empty test container
            StubHtmlIntoTestContainer(0);

            //var document = JSHost.GlobalThis.GetPropertyAsJSObject("document");
            //var parentDiv = (JSObject)JSInstanceProxy.FuncByNameAsObject(document, "createElement", new object[] { "div" });
            //var childDiv = (JSObject)JSInstanceProxy.FuncByNameAsObject(document, "createElement", new object[] { "div" });
            //JSInstanceProxy.SetPropertyByName(parentDiv, "id", "parent-id");
            //JSInstanceProxy.SetPropertyByName(childDiv, "id", "child-id");
            //_ = JSInstanceProxy.FuncByNameAsObject(parentDiv, "appendChild", new object[] { childDiv });

            //// Append parentDiv into test container using jQuery .append() (valid method on tc.JSObject)
            //_ = JSInstanceProxy.FuncByNameAsObject(tc.JSObject, "append", new object[] { parentDiv });

            //// Act: fetch parentElement wrapped
            //var parentWrapped = JSImportInstanceHelpers.GetPropertyOfSameNameAsWrapped<DomElementProxy>(childDiv, propertyName: "ParentElement");


            var document = JSHost.GlobalThis.GetProperty<JSObject>("document");
            var parentDiv = document.CallJS<JSObject>("createElement", SerratedJS.Params("div"));
            var childDiv = document.CallJS<JSObject>("createElement", SerratedJS.Params("div"));
            parentDiv.SetProperty("id", "parent-id");
            childDiv.SetProperty("id", "child-id");
            _ = parentDiv.CallJS<JSObject>("appendChild", SerratedJS.Params(childDiv));
            // Append parentDiv into test container using jQuery .append()
            _ = tc.JSObject.CallJS<object>("append", SerratedJS.Params(parentDiv));

            // Act: fetch parentElement wrapped
            var parentWrapped = childDiv.GetProperty<DomElementProxy>(propertyName: "ParentElement");
            // Assert
            Assert(parentWrapped != null, "Wrapped parent element is null");
            Assert(parentWrapped.Id == "parent-id", "parentElement did not wrap correctly or wrong element returned");
        }
    }

    // New test: exercise extension method GetPropertyOfSameNameAsWrapped<W>
    public class InstanceProperties_Extension_AsWrapped_ParentElement : JSTest
    {
        public override void Run()
        {
            StubHtmlIntoTestContainer(0);
            //var document = JSHost.GlobalThis.GetPropertyAsJSObject("document");
            //var parentDiv = (JSObject)JSInstanceProxy.FuncByNameAsObject(document, "createElement", new object[] { "div" });
            //var childDiv = (JSObject)JSInstanceProxy.FuncByNameAsObject(document, "createElement", new object[] { "div" });
            //JSInstanceProxy.SetPropertyByName(parentDiv, "id", "ext-parent-id");
            //JSInstanceProxy.SetPropertyByName(childDiv, "id", "ext-child-id");
            //_ = JSInstanceProxy.FuncByNameAsObject(parentDiv, "appendChild", new object[] { childDiv });
            //_ = JSInstanceProxy.FuncByNameAsObject(tc.JSObject, "append", new object[] { parentDiv });

            // Wrap childDom as proxy to call extension
            //var childProxy = new DomElementProxy(childDiv);
            //var parentWrapped = childProxy.GetPropertyOfSameNameAsWrapped<DomElementProxy>(propertyName: "ParentElement");

            var document = JSHost.GlobalThis.GetProperty<JSObject>("document");
            var parentDiv = document.CallJS<JSObject>("createElement", SerratedJS.Params("div"));
            var childDiv = document.CallJS<JSObject>("createElement", SerratedJS.Params("div"));
            parentDiv.SetProperty("id", "ext-parent-id");
            childDiv.SetProperty("id", "ext-child-id");
            _ = parentDiv.CallJS<JSObject>("appendChild", SerratedJS.Params(childDiv));
            _ = tc.JSObject.CallJS<object>("append", SerratedJS.Params(parentDiv));

            // Wrap childDom as proxy to call extension
            var childProxy = new DomElementProxy(childDiv);
            var parentWrapped = childProxy.GetProperty<DomElementProxy>("ParentElement");

            Assert(parentWrapped != null, "Extension wrapped parent element is null");
            Assert(parentWrapped.Id == "ext-parent-id", "Extension parentElement did not wrap correctly or wrong element returned");
        }
    }
}
