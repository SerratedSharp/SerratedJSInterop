using System.Runtime.InteropServices.JavaScript;

namespace SerratedSharp.SerratedJSInterop.Tests.Shared;

/// <summary>
/// Test-only: returns the callback test mock and EvalInGlobal from JS.
/// Requires the RCL's wwwroot/CallbackTestMockShim.js to be loaded after SerratedJSInteropShim.js.
/// </summary>
public static partial class CallbackTestMockShim
{
    /// <summary>
    /// Returns the JS callback test mock object. Call after the host has loaded CallbackTestMockShim.js.
    /// </summary>
    public static JSObject GetCallbackTestMock()
    {
        if (AgnosticRuntime.IsUnoWasmBootstrapLoaded)
            return CallbackTestMockShimProxyForUno.GetCallbackTestMock();
        return CallbackTestMockShimProxyForDotNet.GetCallbackTestMock();
    }

    /// <summary>
    /// Evaluates the given code in the global scope (shim context). Test-only; use only in controlled scenarios.
    /// </summary>
    public static void EvalInGlobal(string code)
    {
        if (AgnosticRuntime.IsUnoWasmBootstrapLoaded)
            CallbackTestMockShimProxyForUno.EvalInGlobal(code);
        else
            CallbackTestMockShimProxyForDotNet.EvalInGlobal(code);
    }

    private static partial class CallbackTestMockShimProxyForDotNet
    {
        [JSImport("SerratedJSInteropShim.HelpersShim.GetCallbackTestMock", "SerratedJSInteropShim")]
        public static partial JSObject GetCallbackTestMock();

        [JSImport("SerratedJSInteropShim.HelpersShim.EvalInGlobal", "SerratedJSInteropShim")]
        [return: JSMarshalAs<JSType.Discard>]
        public static partial void EvalInGlobal([JSMarshalAs<JSType.String>] string code);
    }

    private static partial class CallbackTestMockShimProxyForUno
    {
        [JSImport("globalThis.SerratedJSInteropShim.HelpersShim.GetCallbackTestMock")]
        public static partial JSObject GetCallbackTestMock();

        [JSImport("globalThis.SerratedJSInteropShim.HelpersShim.EvalInGlobal")]
        [return: JSMarshalAs<JSType.Discard>]
        public static partial void EvalInGlobal([JSMarshalAs<JSType.String>] string code);
    }
}
