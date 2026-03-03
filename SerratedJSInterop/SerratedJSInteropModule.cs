using System.Runtime.InteropServices.JavaScript;
using System.Threading.Tasks;

namespace SerratedSharp.SerratedJSInterop
{
    public static class SerratedJSInteropModule
    {
        /// <summary>
        /// Loads embedded JS scripts that this library's interop depends on.
        /// Leverages Static Web Assets from this RCL (SerratedSharp.SerratedJSInterop) when referenced.
        /// Loaded with JSHost.ImportAsync().
        /// This is typically needed for a WASM Browser project, but is not needed for a Uno.Wasm.Bootstrap project.
        /// </summary>
        /// <param name="basePath">Base path, if site is not rooted at domain.</param>
        /// <param name="subPath">Sub path. Default is "/_content/SerratedSharp.SerratedJSInterop/SerratedJSInteropShim.js". Provide alternate subpath to concatenate with basePath if loading from custom location.</param>
        public static async Task ImportAsync(string basePath = "", string? subPath = null)
        {
            string path = basePath.TrimEnd('/') +
                (subPath ?? "/_content/SerratedSharp.SerratedJSInterop/SerratedJSInteropShim.js");
            await JSHost.ImportAsync("SerratedJSInteropShim", path);
        }
    }
}
