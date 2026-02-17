using System.Runtime.InteropServices.JavaScript;
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
            get => this.GetProperty<string>();
            set => this.SetProperty(value);
        }

        public string Title
        {
            get => this.GetProperty<string>();
            set => this.SetProperty(value);
        }

        public string TextContent
        {
            get => this.GetProperty<string>();
            set => this.SetProperty(value);
        }
    }

    public class Setters_OnDomElement : JSTest
    {
        public override void Run()
        {
            // Arrange: create a new <div> and append to body
            var document = JSHost.GlobalThis.GetPropertyAsJSObject("document");
            var body = document.GetProperty<JSObject>("body"); //(JSObject)JSInstanceProxy.PropertyByNameToObject(document, "body");
            // JS Console.Log
            GlobalJS.Console.Log("Document body:", body);


            var div = document.CallJS<JSObject>("createElement", SerratedJS.Params("div"));
                //(JSObject)JSInstanceProxy.FuncByNameAsObject(document, "createElement", new object[] { "div" });
            _ = body.CallJS<object>("appendChild", SerratedJS.Params(div));
            //JSInstanceProxy.FuncByNameAsObject(body, "appendChild", new object[] { div });

            var el = new DomElementProxy(div);

            // Act: set via CallerMemberName-based setters
            el.Id = "setter-test-id";
            el.Title = "setter-test-title";
            el.TextContent = "setter-test-text";

            // Assert via getters and DOM queries
            Assert(el.Id == "setter-test-id", "Id was not set via setter");
            Assert(el.Title == "setter-test-title", "Title was not set via setter");
            Assert(el.TextContent == "setter-test-text", "TextContent was not set via setter");

            var byId = document.CallJS<JSObject>("getElementById", SerratedJS.Params(new string[] { "setter-test-id" }));
            //(JSObject)JSInstanceProxy.FuncByNameAsObject(document, "getElementById", new object[] { "setter-test-id" });
            Assert(byId != null, "getElementById returned null");
        }
    }
}
