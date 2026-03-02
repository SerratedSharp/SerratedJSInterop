using System.Runtime.InteropServices.JavaScript;

namespace SerratedSharp.SerratedJSInterop
{
    /// <summary>
    /// Source of truth for agnostic helper methods. The source generator emits
    /// Helpers (consumed API), HelpersProxy, and HelpersProxyForUno into the main
    /// SerratedJSInterop project from this class (same pattern as InstanceHelperJSSource).
    /// </summary>
    [AgnosticModuleReference(baseJSNamespace:"SerratedJSInteropShim.HelpersShim", moduleName: "SerratedJSInteropShim", nestProxiesInGlobalProxy: true)]
    public static partial class HelpersJSSource
    {
        [AgnosticJSImport]
        public static partial Task LoadScript(string relativeUrl);

        [AgnosticJSImport("GetArrayObjectItems")]
        [return: AgnosticJSMarshalAs<JSType.Array<JSType.Object>>]
        public static partial JSObject[] GetArrayObjectItems(JSObject jqObject);

        /// <summary>
        /// Marshal a native JavaScript array of objects into a <see cref="JSObject"/> array.
        /// Expects <paramref name="arrayObject"/> to be a real JS array (for example, the result of jQuery.get()).
        /// </summary>
        [AgnosticJSImport("MarshalAsArrayOfObjects")]
        [return: AgnosticJSMarshalAs<JSType.Array<JSType.Object>>]
        public static partial JSObject[] MarshalAsArrayOfObjects(JSObject arrayObject);

        [AgnosticJSImport("ObjectNew")]
        [return: AgnosticJSMarshalAs<JSType.Object>]
        public static partial JSObject ObjectNew(string path, [AgnosticJSMarshalAs<JSType.Array<JSType.Any>>] object[] args);
    }

    // Stub implementations so this project compiles; this assembly is only used for code generation.
    public static partial class HelpersJSSource
    {
        public static partial Task LoadScript(string relativeUrl) => throw new NotSupportedException("This assembly is for code generation only.");
        public static partial JSObject[] GetArrayObjectItems(JSObject jqObject) => throw new NotSupportedException("This assembly is for code generation only.");
        public static partial JSObject[] MarshalAsArrayOfObjects(JSObject arrayObject) => throw new NotSupportedException("This assembly is for code generation only.");
        public static partial JSObject ObjectNew(string path, object[] args) => throw new NotSupportedException("This assembly is for code generation only.");
    }
}
