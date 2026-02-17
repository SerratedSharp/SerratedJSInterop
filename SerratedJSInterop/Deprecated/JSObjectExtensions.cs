using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.JavaScript;
using SerratedSharp.SerratedJSInterop.Internal;

namespace SerratedSharp.SerratedJSInterop;

[Obsolete("Use JSObjectExtensionsV2 (CallJS, GetProperty, SetProperty) instead.")]
public static class JSObjectExtensions
{
    [Obsolete("Use GetProperty (JSObjectExtensionsV2) instead.")]
    public static J GetPropertyOfSameName<J>(this IJSObjectWrapper wrapper, Breaker _ = default, [CallerMemberName] string? propertyName = null)
        => JSImportInstanceHelpers.GetPropertyOfSameName<J>(wrapper.JSObject, _, propertyName);

    [Obsolete("Use GetProperty (JSObjectExtensionsV2) instead.")]
    public static W GetPropertyOfSameNameAsWrapped<W>(this IJSObjectWrapper wrapper, Breaker _ = default, [CallerMemberName] string? propertyName = null)
        where W : IJSObjectWrapper<W>
        => JSImportInstanceHelpers.GetPropertyOfSameNameAsWrapped<W>(wrapper.JSObject, _, propertyName);

    [Obsolete("Use SetProperty (JSObjectExtensionsV2) instead.")]
    public static void SetPropertyOfSameName(this IJSObjectWrapper wrapper, object value, Breaker _ = default, [CallerMemberName] string? propertyName = null)
        => JSImportInstanceHelpers.SetPropertyOfSameName(wrapper.JSObject, value, _, propertyName);

    [Obsolete("Use CallJS (JSObjectExtensionsV2) instead.")]
    public static W CallJSOfSameNameAsWrapped<W>(this IJSObjectWrapper wrapper, object[] parameters, Breaker _ = default(Breaker), [CallerMemberName] string? funcName = null)
    where W : IJSObjectWrapper<W>
    => JSImportInstanceHelpers.CallJSOfSameNameAsWrapped<W>(wrapper.JSObject, parameters, _, funcName);

    [Obsolete("Use CallJS (JSObjectExtensionsV2) instead.")]
    public static W CallJSOfSameNameAsWrapped<W>(this W wrapper, object[] parameters, Breaker _ = default(Breaker), [CallerMemberName] string? funcName = null)
where W : IJSObjectWrapper<W>
=> JSImportInstanceHelpers.CallJSOfSameNameAsWrapped<W>(wrapper.JSObject, parameters, _, funcName);

    [Obsolete("Use CallJS (JSObjectExtensionsV2) instead.")]
    public static W CallJSOfSameNameAsWrapped<W>(this IJSObjectWrapper wrapper, Breaker _ = default, [CallerMemberName] string? funcName = null)
        where W : IJSObjectWrapper<W>
        => CallJSOfSameNameAsWrappedInternal<W>(wrapper, funcName!, _);

    [Obsolete("Use CallJS (JSObjectExtensionsV2) instead.")]
    public static W CallJSOfSameNameAsWrapped<W>(this W wrapper, Breaker _ = default, [CallerMemberName] string? funcName = null)
        where W : IJSObjectWrapper<W>
        => CallJSOfSameNameAsWrappedInternal<W>(wrapper, funcName!, _);

    [Obsolete("Use CallJS (JSObjectExtensionsV2) instead.")]
    public static W CallJSOfSameNameAsWrapped<W>(this IJSObjectWrapper wrapper, object param1, Breaker _ = default, [CallerMemberName] string? funcName = null)
        where W : IJSObjectWrapper<W>
        => CallJSOfSameNameAsWrappedInternal<W>(wrapper, funcName!, _, param1);

    [Obsolete("Use CallJS (JSObjectExtensionsV2) instead.")]
    public static W CallJSOfSameNameAsWrapped<W>(this W wrapper, object param1, Breaker _ = default, [CallerMemberName] string? funcName = null)
    where W : IJSObjectWrapper<W>
    => CallJSOfSameNameAsWrappedInternal<W>(wrapper, funcName!, _, param1);

    [Obsolete("Use CallJS (JSObjectExtensionsV2) instead.")]
    public static W CallJSOfSameNameAsWrapped<W>(this IJSObjectWrapper wrapper, object param1, object param2, Breaker _ = default, [CallerMemberName] string? funcName = null)
        where W : IJSObjectWrapper<W>
        => CallJSOfSameNameAsWrappedInternal<W>(wrapper, funcName!, _, param1, param2);

    [Obsolete("Use CallJS (JSObjectExtensionsV2) instead.")]
    public static W CallJSOfSameNameAsWrapped<W>(this W wrapper, object param1, object param2, Breaker _ = default, [CallerMemberName] string? funcName = null)
        where W : IJSObjectWrapper<W>
        => CallJSOfSameNameAsWrappedInternal<W>(wrapper, funcName!, _, param1, param2);

    [Obsolete("Use CallJS (JSObjectExtensionsV2) instead.")]
    public static W CallJSOfSameNameAsWrapped<W>(this IJSObjectWrapper wrapper, Breaker _, string funcName, params object[] parameters)
        where W : IJSObjectWrapper<W>
        => CallJSOfSameNameAsWrappedInternal<W>(wrapper, funcName, _, parameters);

    private static W CallJSOfSameNameAsWrappedInternal<W>(IJSObjectWrapper wrapper, string funcName, Breaker _, params object[] parameters)
        where W : IJSObjectWrapper<W>
        => JSImportInstanceHelpers.CallJSOfSameNameAsWrapped<W>(wrapper.JSObject, parameters, _, funcName);

    [Obsolete("Use CallJS (JSObjectExtensionsV2) instead.")]
    public static J CallJSOfSameName<J>(this IJSObjectWrapper wrapper, Breaker _ = default, [CallerMemberName] string? funcName = null)
        => CallJSOfSameNameInternal<J>(wrapper, funcName!, _);

    [Obsolete("Use CallJS (JSObjectExtensionsV2) instead.")]
    public static J CallJSOfSameName<J>(this IJSObjectWrapper wrapper, object param1, Breaker _ = default, [CallerMemberName] string? funcName = null)
        => CallJSOfSameNameInternal<J>(wrapper, funcName!, _, param1);

    [Obsolete("Use CallJS (JSObjectExtensionsV2) instead.")]
    public static J CallJSOfSameName<J>(this IJSObjectWrapper wrapper, object param1, object param2, Breaker _ = default, [CallerMemberName] string? funcName = null)
        => CallJSOfSameNameInternal<J>(wrapper, funcName!, _, param1, param2);

    [Obsolete("Use CallJS (JSObjectExtensionsV2) instead.")]
    public static J CallJSOfSameName<J>(this IJSObjectWrapper wrapper, Breaker _, string funcName, params object[] parameters)
        => CallJSOfSameNameInternal<J>(wrapper, funcName, _, parameters);

    private static J CallJSOfSameNameInternal<J>(IJSObjectWrapper wrapper, string funcName, Breaker _, params object[] parameters)
        => JSImportInstanceHelpers.CallJSOfSameName<J>(wrapper.JSObject, parameters, _, funcName);
}
