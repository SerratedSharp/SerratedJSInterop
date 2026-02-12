using System.Runtime.InteropServices.JavaScript;

namespace SerratedSharp.SerratedJSInterop
{
    /// <summary>
    /// Source of truth for instance-helper proxy methods. The source generator emits
    /// InstanceHelperProxy, InstanceHelperProxyForDotNet, and InstanceHelperProxyForUno
    /// into the main SerratedJSInterop project from this class (same pattern as HelpersJSSource).
    /// </summary>
    [AgnosticModuleReference(baseJSNamespace: "SerratedJSInteropShim.HelpersShim", moduleName: "SerratedJSInteropShim")]
    public static partial class InstanceHelperJSSource
    {
        [AgnosticJSImport]
        [return: AgnosticJSMarshalAs<JSType.Any>]
        public static partial object PropertyByNameToObject(JSObject jqObject, string propertyName);

        [AgnosticJSImport]
        [return: AgnosticJSMarshalAs<JSType.Any>]
        public static partial object SetPropertyByName(JSObject jqObject, string propertyName, [AgnosticJSMarshalAs<JSType.Any>] object value);

        [AgnosticJSImport("FuncByNameToObject")]
        [return: AgnosticJSMarshalAs<JSType.Any>]
        public static partial object FuncByNameAsObject(JSObject jqObject, string funcName, [AgnosticJSMarshalAs<JSType.Array<JSType.Any>>] object[] parameters);

        [AgnosticJSImport("FuncByNameToObject")]
        [return: AgnosticJSMarshalAs<JSType.Array<JSType.Any>>]
        public static partial object[] FuncByNameAsArray(JSObject jqObject, string funcName, [AgnosticJSMarshalAs<JSType.Array<JSType.Any>>] object[] parameters);

        [AgnosticJSImport("FuncByNameToObject")]
        public static partial string[] FuncByNameAsStringArray(JSObject jqObject, string funcName, [AgnosticJSMarshalAs<JSType.Array<JSType.Any>>] object[] parameters);

        [AgnosticJSImport("FuncByNameToObject")]
        public static partial double[] FuncByNameAsDoubleArray(JSObject jqObject, string funcName, [AgnosticJSMarshalAs<JSType.Array<JSType.Any>>] object[] parameters);
    }

    // Stub implementations so this project compiles; this assembly is only used for code generation.
    public static partial class InstanceHelperJSSource
    {
        public static partial object PropertyByNameToObject(JSObject jqObject, string propertyName) => throw new NotSupportedException("This assembly is for code generation only.");
        public static partial object SetPropertyByName(JSObject jqObject, string propertyName, object value) => throw new NotSupportedException("This assembly is for code generation only.");
        public static partial object FuncByNameAsObject(JSObject jqObject, string funcName, object[] parameters) => throw new NotSupportedException("This assembly is for code generation only.");
        public static partial object[] FuncByNameAsArray(JSObject jqObject, string funcName, object[] parameters) => throw new NotSupportedException("This assembly is for code generation only.");
        public static partial string[] FuncByNameAsStringArray(JSObject jqObject, string funcName, object[] parameters) => throw new NotSupportedException("This assembly is for code generation only.");
        public static partial double[] FuncByNameAsDoubleArray(JSObject jqObject, string funcName, object[] parameters) => throw new NotSupportedException("This assembly is for code generation only.");
    }
}
