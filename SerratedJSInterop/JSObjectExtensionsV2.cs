using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.JavaScript;

namespace SerratedSharp.SerratedJSInterop;

/// <summary>
/// Holds the argument array for a JS call. Use SerratedJS.Params(...) to create.
/// </summary>
public readonly struct JSParams
{
    internal object[]? Args { get; }

    internal JSParams(object[]? args)
    {
        Args = args;
    }
}

/// <summary>
/// Factory for JSParams. Use Params(...) so explicit arrays are passed as a single argument when desired.
/// </summary>
public static class SerratedJS
{
    /// <summary>
    /// Multiple arguments or no arguments. Note: To explicitly pass a single array object[] parameter and avoid unintentional expansion ton multiple parameters, use <c>.ArrayParam()</c>.
    /// </summary>
    public static JSParams Params(params object[] parameters)
    {
        return new JSParams(parameters);
    }

    /// <summary>
    /// Pass the string array as a single JS argument (avoids params expansion).
    /// </summary>
    public static JSParams Params(string[] array)
    {
        return new JSParams(new object[] { array });
    }

    /// <summary>
    /// Use when passing a single array param, to avoid unintentional expansion to multiple parameters.
    /// </summary>
    public static JSParams ArrayParam(object[] array)
    {
        return new JSParams(new object[] { array });
    }

}

/// <summary>
/// V2 extension methods: CallJS with SerratedJS.Params and [CallerMemberName] for function name.
/// </summary>
public static class IJSObjectWrapperExtensionsV2
{
    public static J CallJS<J>(this IJSObjectWrapper wrapper, JSParams parameters = default, [CallerMemberName] string? funcName = null)
        => JSImportInstanceHelpers.CallJSFunc<J>(wrapper.JSObject, funcName, parameters.Args);

    public static J CallJS<J>(this IJSObjectWrapper wrapper, string? funcName, JSParams parameters = default)
        => JSImportInstanceHelpers.CallJSFunc<J>(wrapper.JSObject, funcName, parameters.Args);


    public static J GetProperty<J>(this IJSObjectWrapper wrapper, [CallerMemberName] string? propertyName = null)
        => JSImportInstanceHelpers.GetProperty<J>(wrapper.JSObject, propertyName);

    public static void SetProperty(this IJSObjectWrapper wrapper, object value, [CallerMemberName] string? propertyName = null)
    {
        object valueToSet = value is IJSObjectWrapper w ? w.JSObject : value;
        JSImportInstanceHelpers.SetProperty(wrapper.JSObject, propertyName, valueToSet);
    }
}

public static class JSObjectExtensionsV2 {

    public static J CallJS<J>(this JSObject jsObject, JSParams parameters = default, [CallerMemberName] string? funcName = null)
        => JSImportInstanceHelpers.CallJSFunc<J>(jsObject, funcName, parameters.Args);

    public static J CallJS<J>(this JSObject jsObject, string funcName, JSParams parameters = default)
        => JSImportInstanceHelpers.CallJSFunc<J>(jsObject, funcName, parameters.Args);

    public static J GetProperty<J>(this JSObject jsObject, [CallerMemberName] string? propertyName = null)
        => JSImportInstanceHelpers.GetProperty<J>(jsObject, propertyName);

    public static void SetProperty(this JSObject jsObject, object value, [CallerMemberName] string? propertyName = null)
        => JSImportInstanceHelpers.SetProperty(jsObject, propertyName, value);

}
