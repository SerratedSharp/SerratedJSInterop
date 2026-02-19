using System.Runtime.InteropServices.JavaScript;

namespace SerratedSharp.SerratedJSInterop
{
    public static partial class InstanceHelperJS
    {
        public static object PropertyByNameToObject(JSObject jqObject, string propertyName)
        {
            if (AgnosticRuntime.IsUnoWasmBootstrapLoaded)
                return InstanceHelperJSProxyForUno.PropertyByNameToObject(jqObject, propertyName);
            else
                return InstanceHelperJSProxyForDotNet.PropertyByNameToObject(jqObject, propertyName);
        }

        public static object SetPropertyByName(JSObject jqObject, string propertyName, object value)
        {
            if (AgnosticRuntime.IsUnoWasmBootstrapLoaded)
                return InstanceHelperJSProxyForUno.SetPropertyByName(jqObject, propertyName, value);
            else
                return InstanceHelperJSProxyForDotNet.SetPropertyByName(jqObject, propertyName, value);
        }

        public static object FuncByNameAsObject(JSObject jqObject, string funcName, object[] parameters)
        {
            if (AgnosticRuntime.IsUnoWasmBootstrapLoaded)
                return InstanceHelperJSProxyForUno.FuncByNameAsObject(jqObject, funcName, parameters);
            else
                return InstanceHelperJSProxyForDotNet.FuncByNameAsObject(jqObject, funcName, parameters);
        }

        public static object[] FuncByNameAsArray(JSObject jqObject, string funcName, object[] parameters)
        {
            if (AgnosticRuntime.IsUnoWasmBootstrapLoaded)
                return InstanceHelperJSProxyForUno.FuncByNameAsArray(jqObject, funcName, parameters);
            else
                return InstanceHelperJSProxyForDotNet.FuncByNameAsArray(jqObject, funcName, parameters);
        }

        public static string[] FuncByNameAsStringArray(JSObject jqObject, string funcName, object[] parameters)
        {
            if (AgnosticRuntime.IsUnoWasmBootstrapLoaded)
                return InstanceHelperJSProxyForUno.FuncByNameAsStringArray(jqObject, funcName, parameters);
            else
                return InstanceHelperJSProxyForDotNet.FuncByNameAsStringArray(jqObject, funcName, parameters);
        }

        public static double[] FuncByNameAsDoubleArray(JSObject jqObject, string funcName, object[] parameters)
        {
            if (AgnosticRuntime.IsUnoWasmBootstrapLoaded)
                return InstanceHelperJSProxyForUno.FuncByNameAsDoubleArray(jqObject, funcName, parameters);
            else
                return InstanceHelperJSProxyForDotNet.FuncByNameAsDoubleArray(jqObject, funcName, parameters);
        }

        public static void FuncByNameVoid(JSObject jqObject, string funcName, object[] parameters)
        {
            if (AgnosticRuntime.IsUnoWasmBootstrapLoaded)
                InstanceHelperJSProxyForUno.FuncByNameVoid(jqObject, funcName, parameters);
            else
                InstanceHelperJSProxyForDotNet.FuncByNameVoid(jqObject, funcName, parameters);
        }

    }
}

namespace SerratedSharp.SerratedJSInterop
{
    public static partial class InstanceHelperJSProxyForDotNet
    {
        private const string baseJSNamespace = "SerratedJSInteropShim.HelpersShim";
        private const string moduleName = "SerratedJSInteropShim";

        [JSImport(baseJSNamespace + ".PropertyByNameToObject", moduleName)]
        [return: JSMarshalAs<JSType.Any>]
        public static partial object PropertyByNameToObject(JSObject jqObject, string propertyName);

        [JSImport(baseJSNamespace + ".SetPropertyByName", moduleName)]
        [return: JSMarshalAs<JSType.Any>]
        public static partial object SetPropertyByName(JSObject jqObject, string propertyName, [JSMarshalAs<JSType.Any>] object value);

        [JSImport(baseJSNamespace + ".FuncByNameToObject", moduleName)]
        [return: JSMarshalAs<JSType.Any>]
        public static partial object FuncByNameAsObject(JSObject jqObject, string funcName, [JSMarshalAs<JSType.Array<JSType.Any>>] object[] parameters);

        [JSImport(baseJSNamespace + ".FuncByNameToObject", moduleName)]
        [return: JSMarshalAs<JSType.Array<JSType.Any>>]
        public static partial object[] FuncByNameAsArray(JSObject jqObject, string funcName, [JSMarshalAs<JSType.Array<JSType.Any>>] object[] parameters);

        [JSImport(baseJSNamespace + ".FuncByNameToObject", moduleName)]
        public static partial string[] FuncByNameAsStringArray(JSObject jqObject, string funcName, [JSMarshalAs<JSType.Array<JSType.Any>>] object[] parameters);

        [JSImport(baseJSNamespace + ".FuncByNameToObject", moduleName)]
        public static partial double[] FuncByNameAsDoubleArray(JSObject jqObject, string funcName, [JSMarshalAs<JSType.Array<JSType.Any>>] object[] parameters);

        [JSImport(baseJSNamespace + ".FuncByNameToObject", moduleName)]
        [return: JSMarshalAs<JSType.Discard>]
        public static partial void FuncByNameVoid(JSObject jqObject, string funcName, [JSMarshalAs<JSType.Array<JSType.Any>>] object[] parameters);

    }

    public static partial class InstanceHelperJSProxyForUno
    {
        private const string baseJSNamespace = "globalThis.SerratedJSInteropShim.HelpersShim";
        private const string moduleName = "";

        [JSImport(baseJSNamespace + ".PropertyByNameToObject", moduleName)]
        [return: JSMarshalAs<JSType.Any>]
        public static partial object PropertyByNameToObject(JSObject jqObject, string propertyName);

        [JSImport(baseJSNamespace + ".SetPropertyByName", moduleName)]
        [return: JSMarshalAs<JSType.Any>]
        public static partial object SetPropertyByName(JSObject jqObject, string propertyName, [JSMarshalAs<JSType.Any>] object value);

        [JSImport(baseJSNamespace + ".FuncByNameToObject", moduleName)]
        [return: JSMarshalAs<JSType.Any>]
        public static partial object FuncByNameAsObject(JSObject jqObject, string funcName, [JSMarshalAs<JSType.Array<JSType.Any>>] object[] parameters);

        [JSImport(baseJSNamespace + ".FuncByNameToObject", moduleName)]
        [return: JSMarshalAs<JSType.Array<JSType.Any>>]
        public static partial object[] FuncByNameAsArray(JSObject jqObject, string funcName, [JSMarshalAs<JSType.Array<JSType.Any>>] object[] parameters);

        [JSImport(baseJSNamespace + ".FuncByNameToObject", moduleName)]
        public static partial string[] FuncByNameAsStringArray(JSObject jqObject, string funcName, [JSMarshalAs<JSType.Array<JSType.Any>>] object[] parameters);

        [JSImport(baseJSNamespace + ".FuncByNameToObject", moduleName)]
        public static partial double[] FuncByNameAsDoubleArray(JSObject jqObject, string funcName, [JSMarshalAs<JSType.Array<JSType.Any>>] object[] parameters);

        [JSImport(baseJSNamespace + ".FuncByNameToObject", moduleName)]
        [return: JSMarshalAs<JSType.Discard>]
        public static partial void FuncByNameVoid(JSObject jqObject, string funcName, [JSMarshalAs<JSType.Array<JSType.Any>>] object[] parameters);

    }
}
