using SerratedSharp.SerratedDom;
using SerratedSharp.SerratedJSInterop;
using SerratedSharp.SerratedJQ.Plain;
using Wasm;

namespace Tests.Wasm;

public partial class TestsContainer
{

    public class GlobalJS_Console_Log_SeparateParameters_MixedTypes : JSTest
    {
        public override void Run()
        {
            var doc = Document.GetDocument();
            var body = doc.Body;
            var span = doc.CreateElement("span");

            // Pass as separate parameters: string, int, raw JSObjects (body, element), document
            GlobalJS.Console.Log(
                "Log test:",
                42,
                body.JSObject,
                span.JSObject,
                doc.JSObject
            );

            Assert(true, "Console.Log with separate params (string, int, elements, document) completed without throw.");
        }
    }

    public class GlobalJS_Console_Log_SeparateParameters_PrimitivesOnly : JSTest
    {
        public override void Run()
        {
            GlobalJS.Console.Log("one", 1, "two", 2, "three");

            Assert(true, "Console.Log with separate primitive params completed without throw.");
        }
    }
}
