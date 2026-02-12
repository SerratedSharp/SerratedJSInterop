// Generated from SerratedSharp.SerratedJSInterop.Internal.InstanceHelperJSSource → Generated/InstanceHelperJS.g.cs (consumed API type: InstanceHelperJS)

/*
using System.Runtime.InteropServices.JavaScript;

namespace SerratedSharp.SerratedJSInterop;

public static class InstanceHelperProxy
{
    public static object PropertyByNameToObject(JSObject jqObject, string propertyName)
    {
        if (HelpersJSInternal.IsUnoWasmBootstrapLoaded)
            return InstanceHelperProxyForUno.PropertyByNameToObject(jqObject, propertyName);
        else
            return InstanceHelperProxyForDotNet.PropertyByNameToObject(jqObject, propertyName);
    }

    public static object SetPropertyByName(JSObject jqObject, string propertyName, object value)
    {
        if (HelpersJSInternal.IsUnoWasmBootstrapLoaded)
            return InstanceHelperProxyForUno.SetPropertyByName(jqObject, propertyName, value);
        else
            return InstanceHelperProxyForDotNet.SetPropertyByName(jqObject, propertyName, value);
    }

    public static object FuncByNameAsObject(JSObject jqObject, string funcName, object[] parameters)
    {
        if (HelpersJSInternal.IsUnoWasmBootstrapLoaded)
            return InstanceHelperProxyForUno.FuncByNameAsObject(jqObject, funcName, parameters);
        else
            return InstanceHelperProxyForDotNet.FuncByNameAsObject(jqObject, funcName, parameters);
    }

    public static object[] FuncByNameAsArray(JSObject jqObject, string funcName, object[] parameters)
    {
        if (HelpersJSInternal.IsUnoWasmBootstrapLoaded)
            return InstanceHelperProxyForUno.FuncByNameAsArray(jqObject, funcName, parameters);
        else
            return InstanceHelperProxyForDotNet.FuncByNameAsArray(jqObject, funcName, parameters);
    }

    public static string[] FuncByNameAsStringArray(JSObject jqObject, string funcName, object[] parameters)
    {
        if (HelpersJSInternal.IsUnoWasmBootstrapLoaded)
            return InstanceHelperProxyForUno.FuncByNameAsStringArray(jqObject, funcName, parameters);
        else
            return InstanceHelperProxyForDotNet.FuncByNameAsStringArray(jqObject, funcName, parameters);
    }

    public static double[] FuncByNameAsDoubleArray(JSObject jqObject, string funcName, object[] parameters)
    {
        if (HelpersJSInternal.IsUnoWasmBootstrapLoaded)
            return InstanceHelperProxyForUno.FuncByNameAsDoubleArray(jqObject, funcName, parameters);
        else
            return InstanceHelperProxyForDotNet.FuncByNameAsDoubleArray(jqObject, funcName, parameters);
    }
}

public static partial class InstanceHelperProxyForDotNet
{
    private const string baseJSNamespace = "SerratedInteropHelpers.HelpersProxy";
    private const string moduleName = "SerratedInteropHelpers";

    #region Instance Proxies

    [JSImport(baseJSNamespace + ".PropertyByNameToObject", moduleName)]
    [return: JSMarshalAs<JSType.Any>]
    public static partial
        object PropertyByNameToObject(JSObject jqObject, string propertyName);

    [JSImport(baseJSNamespace + ".SetPropertyByName", moduleName)]
    [return: JSMarshalAs<JSType.Any>]
    public static partial
        object SetPropertyByName(JSObject jqObject, string propertyName, [JSMarshalAs<JSType.Any>] object value);

    [JSImport(baseJSNamespace + ".FuncByNameToObject", moduleName)]
    [return: JSMarshalAs<JSType.Any>]
    public static partial
        object FuncByNameAsObject(JSObject jqObject, string funcName, [JSMarshalAs<JSType.Array<JSType.Any>>] object[] parameters);

    [JSImport(baseJSNamespace + ".FuncByNameToObject", moduleName)]
    [return: JSMarshalAs<JSType.Array<JSType.Any>>]
    public static partial
        object[] FuncByNameAsArray(JSObject jqObject, string funcName, [JSMarshalAs<JSType.Array<JSType.Any>>] object[] parameters);

    [JSImport(baseJSNamespace + ".FuncByNameToObject", moduleName)]
    public static partial
        string[] FuncByNameAsStringArray(JSObject jqObject, string funcName, [JSMarshalAs<JSType.Array<JSType.Any>>] object[] parameters);

    [JSImport(baseJSNamespace + ".FuncByNameToObject", moduleName)]
    public static partial
        double[] FuncByNameAsDoubleArray(JSObject jqObject, string funcName, [JSMarshalAs<JSType.Array<JSType.Any>>] object[] parameters);

    #endregion
}

public static partial class InstanceHelperProxyForUno
{
    private const string baseJSNamespace = "globalThis.SerratedInteropHelpers.HelpersProxy";
    private const string moduleName = "";

    #region Instance Proxies

    [JSImport(baseJSNamespace + ".PropertyByNameToObject", moduleName)]
    [return: JSMarshalAs<JSType.Any>]
    public static partial
        object PropertyByNameToObject(JSObject jqObject, string propertyName);

    [JSImport(baseJSNamespace + ".SetPropertyByName", moduleName)]
    [return: JSMarshalAs<JSType.Any>]
    public static partial
        object SetPropertyByName(JSObject jqObject, string propertyName, [JSMarshalAs<JSType.Any>] object value);

    [JSImport(baseJSNamespace + ".FuncByNameToObject", moduleName)]
    [return: JSMarshalAs<JSType.Any>]
    public static partial
        object FuncByNameAsObject(JSObject jqObject, string funcName, [JSMarshalAs<JSType.Array<JSType.Any>>] object[] parameters);

    [JSImport(baseJSNamespace + ".FuncByNameToObject", moduleName)]
    [return: JSMarshalAs<JSType.Array<JSType.Any>>]
    public static partial
        object[] FuncByNameAsArray(JSObject jqObject, string funcName, [JSMarshalAs<JSType.Array<JSType.Any>>] object[] parameters);

    [JSImport(baseJSNamespace + ".FuncByNameToObject", moduleName)]
    public static partial
        string[] FuncByNameAsStringArray(JSObject jqObject, string funcName, [JSMarshalAs<JSType.Array<JSType.Any>>] object[] parameters);

    [JSImport(baseJSNamespace + ".FuncByNameToObject", moduleName)]
    public static partial
        double[] FuncByNameAsDoubleArray(JSObject jqObject, string funcName, [JSMarshalAs<JSType.Array<JSType.Any>>] object[] parameters);

    #endregion
}
*/