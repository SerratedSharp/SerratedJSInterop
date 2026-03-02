using SerratedSharp.SerratedDom;
using SerratedSharp.SerratedJSInterop;
using Wasm;

namespace Tests.Wasm;

public partial class TestsContainer
{
    public class Location_Href_GetSet : JSTest
    {
        public override void Run()
        {
            var location = Location.GetLocation();
            var href = location.Href;
            Assert(href != null && href.Length > 0, "Href should be non-null and non-empty");
            // Do not set Href or call Assign/Reload - they cause navigation/page reload and break the test runner.
        }
    }

    public class Location_GetLocation_AndHref : JSTest
    {
        public override void Run()
        {
            var location = Location.GetLocation();
            Assert(location != null, "GetLocation() should return non-null");
            var href = location.Href;
            Assert(href != null && href.Length > 0, "Href should be non-null and non-empty");
        }
    }
}
