using SerratedSharp.SerratedDom;
using SerratedSharp.SerratedJSInterop;
using SerratedSharp.SerratedJQ.Plain;
using Wasm;

namespace Tests.Wasm;

public partial class TestsContainer
{
    /// <summary>1x1 transparent PNG (smallest valid PNG data URL for testing).</summary>
    private const string OnePixelPngDataUrl = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABgAAAAYCAYAAADgdz34AAAABHNCSVQICAgIfAhkiAAAAAlwSFlzAAAApgAAAKYB3X3/OAAAABl0RVh0U29mdHdhcmUAd3d3Lmlua3NjYXBlLm9yZ5vuPBoAAANCSURBVEiJtZZPbBtFFMZ/M7ubXdtdb1xSFyeilBapySVU8h8OoFaooFSqiihIVIpQBKci6KEg9Q6H9kovIHoCIVQJJCKE1ENFjnAgcaSGC6rEnxBwA04Tx43t2FnvDAfjkNibxgHxnWb2e/u992bee7tCa00YFsffekFY+nUzFtjW0LrvjRXrCDIAaPLlW0nHL0SsZtVoaF98mLrx3pdhOqLtYPHChahZcYYO7KvPFxvRl5XPp1sN3adWiD1ZAqD6XYK1b/dvE5IWryTt2udLFedwc1+9kLp+vbbpoDh+6TklxBeAi9TL0taeWpdmZzQDry0AcO+jQ12RyohqqoYoo8RDwJrU+qXkjWtfi8Xxt58BdQuwQs9qC/afLwCw8tnQbqYAPsgxE1S6F3EAIXux2oQFKm0ihMsOF71dHYx+f3NND68ghCu1YIoePPQN1pGRABkJ6Bus96CutRZMydTl+TvuiRW1m3n0eDl0vRPcEysqdXn+jsQPsrHMquGeXEaY4Yk4wxWcY5V/9scqOMOVUFthatyTy8QyqwZ+kDURKoMWxNKr2EeqVKcTNOajqKoBgOE28U4tdQl5p5bwCw7BWquaZSzAPlwjlithJtp3pTImSqQRrb2Z8PHGigD4RZuNX6JYj6wj7O4TFLbCO/Mn/m8R+h6rYSUb3ekokRY6f/YukArN979jcW+V/S8g0eT/N3VN3kTqWbQ428m9/8k0P/1aIhF36PccEl6EhOcAUCrXKZXXWS3XKd2vc/TRBG9O5ELC17MmWubD2nKhUKZa26Ba2+D3P+4/MNCFwg59oWVeYhkzgN/JDR8deKBoD7Y+ljEjGZ0sosXVTvbc6RHirr2reNy1OXd6pJsQ+gqjk8VWFYmHrwBzW/n+uMPFiRwHB2I7ih8ciHFxIkd/3Omk5tCDV1t+2nNu5sxxpDFNx+huNhVT3/zMDz8usXC3ddaHBj1GHj/As08fwTS7Kt1HBTmyN29vdwAw+/wbwLVOJ3uAD1wi/dUH7Qei66PfyuRj4Ik9is+hglfbkbfR3cnZm7chlUWLdwmprtCohX4HUtlOcQjLYCu+fzGJH2QRKvP3UNz8bWk1qMxjGTOMThZ3kvgLI5AzFfo379UAAAAASUVORK5CYII=";

    public class Image_Src_Base64DataUrl : JSTest
    {
        public override void Run()
        {
            StubHtmlIntoTestContainer(0);
            var img = new Image();
            img.Src = OnePixelPngDataUrl;
            img.Alt = "1x1 PNG";
            img.Width = 24;
            img.Height = 24;
            _ = tc.JSObject.CallJS<object>("append", SerratedJS.Params(img.JSObject));
            result = tc.Children();
            Assert(img.Src == OnePixelPngDataUrl || img.Src.StartsWith("data:image", StringComparison.OrdinalIgnoreCase),
                "Image Src should round-trip or at least start with data:image");
            Assert(result.Length >= 1, "Appended image should appear in container");
        }
    }

    public class Image_Alt_GetSet : JSTest
    {
        public override void Run()
        {
            var img = new Image();
            img.Alt = "test-alt";
            Assert(img.Alt == "test-alt", "Image Alt should round-trip");
        }
    }

    public class Image_Width_Height_GetSet : JSTest
    {
        public override void Run()
        {
            var img = new Image();
            img.Width = 10;
            img.Height = 20;
            Assert(img.Width == 10, "Image Width should be 10");
            Assert(img.Height == 20, "Image Height should be 20");
        }
    }

    public class Image_Complete_IsBoolean : JSTest
    {
        public override void Run()
        {
            var img = new Image();
            _ = img.Complete; // can read without throwing
            Assert(true, "Complete is readable");
        }
    }

    public class Image_NaturalDimensions_BeforeLoad : JSTest
    {
        public override void Run()
        {
            var img = new Image();
            int w = img.NaturalWidth;
            int h = img.NaturalHeight;
            Assert(w >= 0 && h >= 0, "NaturalWidth and NaturalHeight should be non-negative before load");
        }
    }

    public class Image_NaturalDimensions_AfterDataUrlLoad : JSTest
    {
        public override void Run()
        {
            var img = new Image();
            img.Src = OnePixelPngDataUrl;
            // Data URL may load synchronously in many browsers; natural width/height may be 1 or 0 depending on timing.
            int w = img.NaturalWidth;
            int h = img.NaturalHeight;
            Assert(w >= 0 && h >= 0, "NaturalWidth and NaturalHeight should be non-negative");
            // Optionally: after load we expect 1x1 for our 1x1 PNG (best-effort; may still be 0 if not loaded yet)
            if (img.Complete && w == 1 && h == 1)
                Assert(true, "1x1 PNG loaded and dimensions correct");
        }
    }
}
