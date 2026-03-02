using System.Runtime.InteropServices.JavaScript;
using SerratedSharp.SerratedDom;
using SerratedSharp.SerratedJSInterop;
using SerratedSharp.SerratedJQ.Plain;
using Wasm;

namespace Tests.Wasm;

public partial class TestsContainer
{
    // Minimal DOM element wrapper to exercise GetProperty/SetProperty CallerMemberName helpers
    private class DomElementProxy : IJSObjectWrapper<DomElementProxy>
    {
        public JSObject JSObject { get; }
        public DomElementProxy(JSObject js) { JSObject = js; }
        static DomElementProxy IJSObjectWrapper<DomElementProxy>.WrapInstance(JSObject js) => new(js);

        public string Id
        {
            get => this.GetJSProperty<string>();
            set => this.SetJSProperty(value);
        }

        public string Title
        {
            get => this.GetJSProperty<string>();
            set => this.SetJSProperty(value);
        }

        public string TextContent
        {
            get => this.GetJSProperty<string>();
            set => this.SetJSProperty(value);
        }
    }

    private class PlainObjectWrapper : IJSObjectWrapper<PlainObjectWrapper>
    {
        public JSObject JSObject { get; }
        public PlainObjectWrapper(JSObject js) { JSObject = js; }
        static PlainObjectWrapper IJSObjectWrapper<PlainObjectWrapper>.WrapInstance(JSObject js) => new(js);
    }

    public class Setters_Wrapper_SetJSProperty_WithWrapperValue : JSTest
    {
        public override void Run()
        {
            StubHtmlIntoTestContainer(0);
            var doc = Document.GetDocument();
            var plainJs = SerratedJS.New("Object");
            var holder = new PlainObjectWrapper(plainJs);
            var child = doc.CreateElement("div");
            child.Id = "setter-wrapper-ref-node";

            holder.SetJSProperty(propertyName: "nodeRef", child);

            var refBack = holder.GetJSProperty<HtmlElement>("nodeRef");
            Assert(refBack != null, "GetJSProperty(nodeRef) should return the element set via SetJSProperty");
            Assert(refBack.Id == "setter-wrapper-ref-node", "Returned element should be the same node (by Id)");
        }
    }

    public class Setters_OnDomElement : JSTest
    {
        public override void Run()
        {
            // Arrange: create a new <div> and append to test container (not document body)
            StubHtmlIntoTestContainer(0);
            var document = JSHost.GlobalThis.GetPropertyAsJSObject("document");
            var div = document.CallJS<JSObject>(funcName: "createElement", "div");
            var container = tc.Get(0);
            _ = container.CallJS<object>(funcName: "appendChild", div);

            var el = new DomElementProxy(div);

            // Act: set via CallerMemberName-based setters
            el.Id = "setter-test-id";
            el.Title = "setter-test-title";
            el.TextContent = "setter-test-text";

            // Assert via getters and find within test container
            Assert(el.Id == "setter-test-id", "Id was not set via setter");
            Assert(el.Title == "setter-test-title", "Title was not set via setter");
            Assert(el.TextContent == "setter-test-text", "TextContent was not set via setter");

            var byId = tc.Find("#setter-test-id").Get(0);
            Assert(byId != null, "find within tc should return the element");
        }
    }
}
