using System.Runtime.InteropServices.JavaScript;
using SerratedSharp.SerratedDom;
using SerratedSharp.SerratedJSInterop;
using SerratedSharp.SerratedJQ.Plain;
using Wasm;

namespace Tests.Wasm;

public partial class TestsContainer
{
    public class New_JSObject_NoArgs : JQTest
    {
        public override void Run()
        {
            var img = SerratedJS.New("Image");
            Assert(img != null, "New<JSObject>(\"Image\") returned null");
        }
    }

    public class New_JSObject_WithArgs : JQTest
    {
        public override void Run()
        {
            
            var opt = SerratedJS.New("Option", SerratedJS.Params("Display text", "optValue"));
            Assert(opt != null, "New<JSObject>(\"Option\", ...) returned null");
            Assert(opt.GetProperty<string>("Text") == "Display text", "Option text was not set");
            Assert(opt.GetProperty<string>("Value") == "optValue", "Option value was not set");
        }
    }

    public class New_JSObject_Image_ThenWrapImage : JQTest
    {
        public override void Run()
        {
            StubHtmlIntoTestContainer(0);
            var imgJs = SerratedJS.New("Image");
            var img = new Image(imgJs);
            _ = tc.JSObject.CallJS<object>("append", SerratedJS.Params(img.JSObject));
            img.Alt = "test-alt";
            Assert(img.Alt == "test-alt", "Image Alt was not set after New");
            result = tc.Children();
            Assert(result.Length >= 1, "Appended element should appear in container");
        }
    }

    public class New_AsWrapped_DomElementProxy : JQTest
    {
        public override void Run()
        {
            var wrapped = new DomElementProxy(SerratedJS.New("Image"));
            Assert(wrapped != null, "New<DomElementProxy> returned null");
            Assert(wrapped.JSObject != null, "Wrapped JSObject is null");
            wrapped.Id = "new-api-proxy-id";
            Assert(wrapped.Id == "new-api-proxy-id", "Id was not set on wrapped instance");
        }
    }

    public class New_WithMarshalAsJson_CustomEvent : JQTest
    {
        public override void Run()
        {
            var evt = SerratedJS.New("CustomEvent", SerratedJS.Params("myevent", new { detail = 42 }.MarshalAsJson()));
            Assert(evt != null, "New CustomEvent returned null");
            var detail = evt.GetProperty<double>("detail");
            Assert(detail == 42, "CustomEvent detail was not 42");
        }
    }

    public class New_NoArgs_EmptyParams : JQTest
    {
        public override void Run()
        {
            var img = SerratedJS.New("Image", default);
            Assert(img != null, "New<JSObject>(\"Image\", default) returned null");
        }
    }

    public class Image_ParameterlessConstructor : JQTest
    {
        public override void Run()
        {
            var img = new Image();
            Assert(img != null, "new Image() returned null");
            Assert(img.JSObject != null, "JSObject is null");
            var tagName = img.GetProperty<string>("tagName");
            Assert(tagName == "IMG", "Expected tagName IMG for Image");
        }
    }
}
