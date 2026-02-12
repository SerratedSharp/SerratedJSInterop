using System.Runtime.InteropServices.JavaScript;
using System.Threading.Tasks;

namespace SerratedSharp.SerratedJSInterop
{
    [Obsolete("This class is obsolete. Use JSInteropHelpersModule.ImportAsync instead.")]
    public static class JSDeclarationsForWasmBrowser
    {
        /// <summary>
        /// Loads embedded JS scripts that this library's interop depends on.
        /// </summary>
        /// <param name="basePath">Base path if site is not rooted at domain.</param>
        public static async Task LoadScripts(string basePath = "")
        {
            await JSInteropHelpersModule.ImportAsync(basePath);
        }
    }

    public static class JSInteropHelpersModule
    {
        /// <summary>
        /// Loads embedded JS scripts that this library's interop depends on.
        /// Leverages Static Web Assets from the SerratedSharp.SerratedJSInterop.Blazor RCL when referenced.
        /// Loaded with JSHost.ImportAsync().
        /// This is typically needed for a WASM Browser project, but is not needed for a Uno.Wasm.Bootstrap project.
        /// </summary>
        /// <param name="basePath">Base path, if site is not rooted at domain.</param>
        /// <param name="subPath">Sub path. Default is "/_content/SerratedSharp.SerratedJSInterop.Blazor/SerratedJSInteropShim.js" for loading from the Blazor RCL. Provide alternate subpath to concatenate with basePath if loading from custom location.</param>
        public static async Task ImportAsync(string basePath = "", string? subPath = null)
        {
            string path = basePath.TrimEnd('/') +
                (subPath ?? "/_content/SerratedSharp.SerratedJSInterop.Blazor/SerratedJSInteropShim.js");
            await JSHost.ImportAsync("SerratedJSInteropShim", path);
        }
    }
}
