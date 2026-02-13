using System.Runtime.InteropServices.JavaScript;

namespace SerratedSharp.SerratedJSInterop
{
    /// <summary>Shared runtime flag for Uno vs .NET WASM. Used by generated proxy routing.</summary>
    public static class AgnosticRuntime
    {
        /// <summary>
        /// Gets a value indicating whether being used from Uno.Wasm.Bootstrap rather than .NET WasmBrowser.
        /// </summary>
        internal static bool IsUnoWasmBootstrapLoaded => isUnoWasmBootstrapLoaded.Value;
        private static Lazy<bool> isUnoWasmBootstrapLoaded = new Lazy<bool>(() =>
        {
            bool isUnoPresent = false;
            if (JSHost.GlobalThis.HasProperty("IsFromUno"))
            {
                if (JSHost.GlobalThis.GetPropertyAsBoolean("IsFromUno"))
                    isUnoPresent = true;
            }
            return isUnoPresent;
        });
    }
}
