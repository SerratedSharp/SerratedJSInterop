using System.Collections.Generic;
using System.Runtime.InteropServices.JavaScript;
using SerratedSharp.SerratedDom;
using SerratedSharp.SerratedJSInterop;
using SerratedSharp.SerratedJQ.Plain;
using Wasm;

namespace Tests.Wasm;

public partial class TestsContainer
{
    static readonly object createElementOptions = new Dictionary<string, string> { ["is"] = "my-div" };
    static readonly object optionsPoco = new { key = "value", num = 42 };

    public class MarshalAsJson_IJSObjectWrapper_CallJS_CallerMemberName : JSTest
    {
        public override void Run()
        {
            var doc = Document.GetDocument();
            var el = createElement(doc);
            Assert(el != null && el.JSObject != null, "createElement via CallerMemberName + MarshalAsJson should return element");
        }

        HtmlElement createElement(Document d) => d.CallJS<HtmlElement>(SerratedJS.Params("div", createElementOptions.MarshalAsJson()));
    }

    public class MarshalAsJson_IJSObjectWrapper_CallJS_ExplicitFuncName : JSTest
    {
        public override void Run()
        {
            var doc = Document.GetDocument();
            var el = doc.CallJS<HtmlElement>(funcName: "createElement", SerratedJS.Params("div", createElementOptions.MarshalAsJson()));
            Assert(el != null && el.JSObject != null, "createElement via explicit funcName + MarshalAsJson should return element");
        }
    }

    public class MarshalAsJson_IJSObjectWrapper_GetProperty : JSTest
    {
        public override void Run()
        {
            var doc = Document.GetDocument();
            var el = doc.CreateElement("div");
            el.SetJSProperty(optionsPoco.MarshalAsJson(), "options");
            var opts = el.GetJSProperty<JSObject>("options");
            Assert(opts != null && opts.GetJSProperty<string>("key") == "value" && opts.GetJSProperty<int>("num") == 42, "GetProperty should return unwrapped options object");
        }
    }

    public class MarshalAsJson_IJSObjectWrapper_SetProperty : JSTest
    {
        public override void Run()
        {
            var doc = Document.GetDocument();
            var el = doc.CreateElement("div");
            el.SetJSProperty(optionsPoco.MarshalAsJson(), "options");
            var opts = el.GetJSProperty<JSObject>("options");
            Assert(opts != null && opts.GetJSProperty<int>("num") == 42, "SetProperty MarshalAsJson value should be unwrapped on JS side");
        }
    }

    public class MarshalAsJson_JSObject_CallJS_CallerMemberName : JSTest
    {
        public override void Run()
        {
            var doc = Document.GetDocument();
            var el = createElement(doc.JSObject);
            Assert(el != null, "JSObject.CallJS via CallerMemberName + MarshalAsJson should return element");
        }

        JSObject createElement(JSObject documentJSObject) => documentJSObject.CallJS<JSObject>(SerratedJS.Params("div", createElementOptions.MarshalAsJson()));
    }

    public class MarshalAsJson_JSObject_CallJS_ExplicitFuncName : JSTest
    {
        public override void Run()
        {
            var doc = Document.GetDocument();
            var el = doc.JSObject.CallJS<JSObject>(funcName: "createElement", SerratedJS.Params("div", createElementOptions.MarshalAsJson()));
            Assert(el != null, "JSObject.CallJS explicit funcName + MarshalAsJson should return element");
        }
    }

    public class MarshalAsJson_JSObject_GetProperty : JSTest
    {
        public override void Run()
        {
            var doc = Document.GetDocument();
            var el = doc.CreateElement("div");
            el.SetJSProperty(optionsPoco.MarshalAsJson(), "options");
            var jsEl = el.JSObject;
            var opts = jsEl.GetJSProperty<JSObject>("options");
            Assert(opts != null, "JSObject.GetProperty('options') should be non-null");
            Assert(opts.GetJSProperty<string>("key") == "value", "options.key should be value");
            Assert(opts.GetJSProperty<int>("num") == 42, "options.num should be 42");
        }
    }

    public class MarshalAsJson_JSObject_SetProperty : JSTest
    {
        public override void Run()
        {
            var doc = Document.GetDocument();
            var el = doc.CreateElement("div");
            var jsEl = el.JSObject;
            jsEl.SetProperty(optionsPoco.MarshalAsJson(), "options");
            jsEl.SetJSProperty(99, "simple");
            Assert(el.GetJSProperty<int>("simple") == 99, "JSObject.SetProperty primitive should round-trip");
            var opts = el.GetJSProperty<JSObject>("options");
            if (opts != null)
            {
                Assert(opts.GetJSProperty<int>("num") == 42, "options.num should be 42 when options round-trips");
            }
        }
    }


    public class MarshalAsJson_JSObject_SetProperty_Explicit : JSTest
    {
        public override void Run()
        {
            var doc = Document.GetDocument();
            var el = doc.CreateElement("div");
            var jsEl = el.JSObject;
            jsEl.SetProperty(optionsPoco.MarshalAsJson(), "options");
            jsEl.SetJSProperty(propertyName: "simple", 99);
            Assert(el.GetJSProperty<int>("simple") == 99, "JSObject.SetProperty primitive should round-trip");
            var opts = el.GetJSProperty<JSObject>("options");
            if (opts != null)
            {
                Assert(opts.GetJSProperty<int>("num") == 42, "options.num should be 42 when options round-trips");
            }
        }
    }
}
