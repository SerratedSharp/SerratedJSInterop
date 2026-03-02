using System.Runtime.InteropServices.JavaScript;

namespace SerratedSharp.SerratedJSInterop
{
    /// <summary>Shared runtime flag for Uno vs .NET WASM. Used by generated proxy routing and by consumers (e.g. SerratedJQ) for JSImport module path selection.</summary>
    public static class AgnosticRuntime
    {
        /// <summary>
        /// Gets a value indicating whether the app is running under Uno.Wasm.Bootstrap rather than .NET WasmBrowser.
        /// </summary>
        public static bool IsUnoWasmBootstrapLoaded => _isUnoWasmBootstrapLoaded.Value;

        private static readonly Lazy<bool> _isUnoWasmBootstrapLoaded = new Lazy<bool>(() =>
        {
            if (JSHost.GlobalThis.HasProperty("IsFromUno") && JSHost.GlobalThis.GetPropertyAsBoolean("IsFromUno"))
                return true;
            return false;
        });
    }
}
