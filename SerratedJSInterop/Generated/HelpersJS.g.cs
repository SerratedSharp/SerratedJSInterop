using System.Runtime.InteropServices.JavaScript;

namespace SerratedSharp.SerratedJSInterop
{
    public static partial class HelpersJS
    {
        public static async Task LoadScript(string relativeUrl)
        {
            if (AgnosticRuntime.IsUnoWasmBootstrapLoaded)
                await GlobalProxy.HelpersJSProxyForUno.LoadScript(relativeUrl);
            else
                await GlobalProxy.HelpersJSProxyForDotNet.LoadScript(relativeUrl);
        }

        public static JSObject[] GetArrayObjectItems(JSObject jqObject)
        {
            if (AgnosticRuntime.IsUnoWasmBootstrapLoaded)
                return GlobalProxy.HelpersJSProxyForUno.GetArrayObjectItems(jqObject);
            else
                return GlobalProxy.HelpersJSProxyForDotNet.GetArrayObjectItems(jqObject);
        }

    }
}

namespace SerratedSharp.SerratedJSInterop
{
    internal static partial class GlobalProxy
    {
        internal partial class HelpersJSProxyForDotNet
        {
            private const string baseJSNamespace = "SerratedJSInteropShim.HelpersShim";
            private const string moduleName = "SerratedJSInteropShim";

            [JSImport(baseJSNamespace + ".LoadScript", moduleName)]
            public static partial Task LoadScript(string relativeUrl);

            [JSImport(baseJSNamespace + ".GetArrayObjectItems", moduleName)]
            [return: JSMarshalAs<JSType.Array<JSType.Object>>]
            public static partial JSObject[] GetArrayObjectItems(JSObject jqObject);

        }

        internal partial class HelpersJSProxyForUno
        {
            private const string baseJSNamespace = "globalThis.SerratedJSInteropShim.HelpersShim";

            [JSImport(baseJSNamespace + ".LoadScript")]
            public static partial Task LoadScript(string relativeUrl);

            [JSImport(baseJSNamespace + ".GetArrayObjectItems")]
            [return: JSMarshalAs<JSType.Array<JSType.Object>>]
            public static partial JSObject[] GetArrayObjectItems(JSObject jqObject);

        }
    }
}
