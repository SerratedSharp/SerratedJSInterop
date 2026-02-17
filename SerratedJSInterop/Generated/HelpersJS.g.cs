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

        public static JSObject ObjectNew(string path, object[] args)
        {
            if (AgnosticRuntime.IsUnoWasmBootstrapLoaded)
                return GlobalProxy.HelpersJSProxyForUno.ObjectNew(path, args);
            else
                return GlobalProxy.HelpersJSProxyForDotNet.ObjectNew(path, args);
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

            [JSImport(baseJSNamespace + ".ObjectNew", moduleName)]
            public static partial JSObject ObjectNew(string path, [JSMarshalAs<JSType.Array<JSType.Any>>] object[] args);

        }

        internal partial class HelpersJSProxyForUno
        {
            private const string baseJSNamespace = "globalThis.SerratedJSInteropShim.HelpersShim";

            [JSImport(baseJSNamespace + ".LoadScript")]
            public static partial Task LoadScript(string relativeUrl);

            [JSImport(baseJSNamespace + ".GetArrayObjectItems")]
            [return: JSMarshalAs<JSType.Array<JSType.Object>>]
            public static partial JSObject[] GetArrayObjectItems(JSObject jqObject);

            [JSImport(baseJSNamespace + ".ObjectNew")]
            public static partial JSObject ObjectNew(string path, [JSMarshalAs<JSType.Array<JSType.Any>>] object[] args);

        }
    }
}
