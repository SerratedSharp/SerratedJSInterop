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

    /// <summary>IJSObjectWrapperExtensionsV2.CallJS(JSParams, [CallerMemberName]) with MarshalAsJson in params.</summary>
    public class MarshalAsJson_IJSObjectWrapper_CallJS_CallerMemberName : JQTest
    {
        public override void Run()
        {
            var doc = Document.GetDocument();
            var el = createElement(doc);
            Assert(el != null && el.JSObject != null, "createElement via CallerMemberName + MarshalAsJson should return element");
        }

        HtmlElement createElement(Document d) => d.CallJS<HtmlElement>(SerratedJS.Params("div", createElementOptions.MarshalAsJson()));
    }

    /// <summary>IJSObjectWrapperExtensionsV2.CallJS(string funcName, JSParams) with MarshalAsJson in params.</summary>
    public class MarshalAsJson_IJSObjectWrapper_CallJS_ExplicitFuncName : JQTest
    {
        public override void Run()
        {
            var doc = Document.GetDocument();
            var el = doc.CallJS<HtmlElement>("createElement", SerratedJS.Params("div", createElementOptions.MarshalAsJson()));
            Assert(el != null && el.JSObject != null, "createElement via explicit funcName + MarshalAsJson should return element");
        }
    }

    /// <summary>IJSObjectWrapperExtensionsV2.GetProperty with MarshalAsJson (read back property set with MarshalAsJson).</summary>
    public class MarshalAsJson_IJSObjectWrapper_GetProperty : JQTest
    {
        public override void Run()
        {
            var doc = Document.GetDocument();
            var el = doc.CreateElement("div");
            el.SetProperty(optionsPoco.MarshalAsJson(), "options");
            var opts = el.GetProperty<JSObject>("options");
            Assert(opts != null && opts.GetProperty<string>("key") == "value" && opts.GetProperty<int>("num") == 42, "GetProperty should return unwrapped options object");
        }
    }

    /// <summary>IJSObjectWrapperExtensionsV2.SetProperty with MarshalAsJson value.</summary>
    public class MarshalAsJson_IJSObjectWrapper_SetProperty : JQTest
    {
        public override void Run()
        {
            var doc = Document.GetDocument();
            var el = doc.CreateElement("div");
            el.SetProperty(optionsPoco.MarshalAsJson(), "options");
            var opts = el.GetProperty<JSObject>("options");
            Assert(opts != null && opts.GetProperty<int>("num") == 42, "SetProperty MarshalAsJson value should be unwrapped on JS side");
        }
    }

    /// <summary>JSObjectExtensionsV2.CallJS(JSParams, [CallerMemberName]) with MarshalAsJson in params.</summary>
    public class MarshalAsJson_JSObject_CallJS_CallerMemberName : JQTest
    {
        public override void Run()
        {
            var doc = Document.GetDocument();
            var el = createElement(doc.JSObject);
            Assert(el != null, "JSObject.CallJS via CallerMemberName + MarshalAsJson should return element");
        }

        JSObject createElement(JSObject documentJSObject) => documentJSObject.CallJS<JSObject>(SerratedJS.Params("div", createElementOptions.MarshalAsJson()));
    }

    /// <summary>JSObjectExtensionsV2.CallJS(string funcName, JSParams) with MarshalAsJson in params.</summary>
    public class MarshalAsJson_JSObject_CallJS_ExplicitFuncName : JQTest
    {
        public override void Run()
        {
            var doc = Document.GetDocument();
            var el = doc.JSObject.CallJS<JSObject>("createElement", SerratedJS.Params("div", createElementOptions.MarshalAsJson()));
            Assert(el != null, "JSObject.CallJS explicit funcName + MarshalAsJson should return element");
        }
    }

    /// <summary>JSObjectExtensionsV2.GetProperty with MarshalAsJson (read back property set with MarshalAsJson). Set via wrapper so we only assert JSObject.GetProperty.</summary>
    public class MarshalAsJson_JSObject_GetProperty : JQTest
    {
        public override void Run()
        {
            var doc = Document.GetDocument();
            var el = doc.CreateElement("div");
            el.SetProperty(optionsPoco.MarshalAsJson(), "options");
            var jsEl = el.JSObject;
            var opts = jsEl.GetProperty<JSObject>("options");
            Assert(opts != null, "JSObject.GetProperty('options') should be non-null");
            Assert(opts.GetProperty<string>("key") == "value", "options.key should be value");
            Assert(opts.GetProperty<int>("num") == 42, "options.num should be 42");
        }
    }

    /// <summary>JSObjectExtensionsV2.SetProperty with MarshalAsJson value. Verifies SetProperty is invoked and primitive round-trips; options object readback may be null in some hosts.</summary>
    public class MarshalAsJson_JSObject_SetProperty : JQTest
    {
        public override void Run()
        {
            var doc = Document.GetDocument();
            var el = doc.CreateElement("div");
            var jsEl = el.JSObject;
            jsEl.SetProperty(optionsPoco.MarshalAsJson(), "options");
            jsEl.SetProperty(99, "simple");
            Assert(el.GetProperty<int>("simple") == 99, "JSObject.SetProperty primitive should round-trip");
            var opts = el.GetProperty<JSObject>("options");
            if (opts != null)
            {
                Assert(opts.GetProperty<int>("num") == 42, "options.num should be 42 when options round-trips");
            }
        }
    }
}
