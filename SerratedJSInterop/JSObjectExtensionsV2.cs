using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.JavaScript;

namespace SerratedSharp.SerratedJSInterop;

/// <summary>
/// Holds the argument array for a JS call. Use SerratedJS.Params(...) to create.
/// </summary>
public readonly struct JSParams
{
    internal object[] Args { get; }

    internal JSParams(object[] args)
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
    /// Pass a string array as a single JS argument (avoids params expansion).
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

    /// <summary>
    /// Calls JS constructor for `typePath` constructor name and namespace if required.  E.g. "PIXI.Rectangle", "Image". Returns <see cref="JSObject"/>.
    /// Use `SerratedJS.Params("param1", 2)` if passing constructor arguments.
    /// </summary>
    /// <param name="typePath">JS constructor name. Include fully qualified path if necessary (e.g. "PIXI.Rectangle", "Image").  Is case sensitive.</param>
    /// <param name="parameters">Optional constructor arguments (e.g. SerratedJS.Params("green", 50, 100)).</param>
    /// <returns>InteropServies.JSObject reference to the new object.</returns>
    /// <example>
    /// // Implement interop constructor in type implementing IJSObjectWrapper
    /// public Image() 
    /// {
    ///     JSObject = SerratedJS.New(nameof(Image));
    /// }
    /// </example>
    public static JSObject New(string typePath, JSParams parameters = default)
    {
        var args = parameters.Args ?? Array.Empty<object>();
        var unwrapped = JSImportInstanceHelpers.UnwrapJSObjectParams(args);
        return HelpersJS.ObjectNew(typePath, unwrapped);
    }

    ///// <summary>
    ///// Calls JS constructor for `typePath` type name and namespace if required.  E.g. "PIXI.Rectangle", "Image". Returns type J, where J implements <see cref="IJSObjectWrapper{J}"/>.
    ///// Use `SerratedJS.Params("param1", 2)` if passing constructor arguments.
    ///// Consider using non-generic SerratedJS.New("typeName") if implementing wrapper.
    ///// </summary>
    ///// <typeparam name="J">A type implementing <see cref="IJSObjectWrapper{J}"/> (e.g. HtmlElement, Image, DomElementProxy).</typeparam>
    ///// <param name="typePath">JS constructor name. Include fully qualified path if necessary.</param>
    ///// <param name="parameters">Optional constructor arguments (e.g. SerratedJS.Params(0, 0, 100, 100)).</param>
    ///// <returns>Requested J wrapping the new JSObject instance via <c>IJSObjectWrapper{J}.AsWrapped()</c> instance.</returns>
    // CONSIDER: Whether we need this overload.  If there is a wrapper type J, then it should probably just implement `JSObject = SerratedJS.New("type")`.  The only time an auto-wrapping New makes sense is if the wrapper didn't implement JS interop constructor, and instead relied on factory.  Even then we can just called the constructor taking JSObject.
    //public static J New<J>(string typePath, JSParams parameters = default) where J : IJSObjectWrapper<J>
    //{
    //    var jsObject = New(typePath, parameters);
    //    return JSImportInstanceHelpers.CastOrWrap<J>(jsObject);
    //}

    



}

/// <summary>
/// V2 extension methods: CallJS with SerratedJS.Params and [CallerMemberName] for function name.
/// </summary>
public static class IJSObjectWrapperExtensionsV2
{
    public static J CallJS<J>(this IJSObjectWrapper wrapper, JSParams parameters = default, [CallerMemberName] string? funcName = null)
        => JSImportInstanceHelpers.CallJSFunc<J>(wrapper.JSObject, funcName!, parameters.Args);

    public static J CallJS<J>(this IJSObjectWrapper wrapper, string? funcName, JSParams parameters = default)
        => JSImportInstanceHelpers.CallJSFunc<J>(wrapper.JSObject, funcName!, parameters.Args);


    public static J GetProperty<J>(this IJSObjectWrapper wrapper, [CallerMemberName] string? propertyName = null)
        => JSImportInstanceHelpers.GetProperty<J>(wrapper.JSObject, propertyName!);

    public static void SetProperty(this IJSObjectWrapper wrapper, object value, [CallerMemberName] string? propertyName = null)
    {
        object valueToSet = value is IJSObjectWrapper w ? w.JSObject : value;
       JSImportInstanceHelpers.SetProperty(wrapper.JSObject, propertyName!, valueToSet);
    }
}

public static class JSObjectExtensionsV2 {

    public static J CallJS<J>(this JSObject jsObject, JSParams parameters = default, [CallerMemberName] string? funcName = null)
        => JSImportInstanceHelpers.CallJSFunc<J>(jsObject, funcName!, parameters.Args);

    public static J CallJS<J>(this JSObject jsObject, string funcName, JSParams parameters = default)
        => JSImportInstanceHelpers.CallJSFunc<J>(jsObject, funcName!, parameters.Args);

    public static J GetProperty<J>(this JSObject jsObject, [CallerMemberName] string? propertyName = null)
        => JSImportInstanceHelpers.GetProperty<J>(jsObject, propertyName!);

    public static void SetProperty(this JSObject jsObject, object value, [CallerMemberName] string? propertyName = null)
        => JSImportInstanceHelpers.SetProperty(jsObject, propertyName!, value);

}
