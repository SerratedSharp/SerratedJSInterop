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
    // CONSIDER: Checking parameters for IJSObjectWrapper and conditionally do paramItem.JSObject to pass the native object autoamtically.

    /// <summary>
    /// Multiple arguments or no arguments. Note: To explicitly pass a single array object[] parameter and avoid unintentional expansion to multiple parameters, use <c>.ArrayParam()</c>.
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
    /// <returns>JSObject reference to the new object.</returns>
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

    // CONSIDER: Whether we need this overload.  If there is a wrapper type J, then it should probably just implement `JSObject = SerratedJS.New("type")`.  The only time an auto-wrapping New makes sense is if the wrapper didn't implement JS interop constructor, and instead relied on factory.  Even then we can just called the constructor taking JSObject.
    //public static J New<J>(string typePath, JSParams parameters = default) where J : IJSObjectWrapper<J>
    //{
    //    var jsObject = New(typePath, parameters);
    //    return JSImportInstanceHelpers.CastOrWrap<J>(jsObject);
    //}

    



}

/// <summary>
/// IJSObjectWrapper extensions
/// </summary>
public static class IJSObjectWrapperExtensionsV2
{
    /// <summary>
    /// <para>Call JS <c>funcName</c>(inferred via [CallerMemberName]) on this IJSObjectWrapper's JSObject.</para>
    /// <para>var someWrapper = wrapper.CallJS&lt;SomeWrapper&gt;(SerratedJS.JSParams(...))</para>
    /// </summary>
    /// <typeparam name="J">Type to return. Typically a JSObject, IJSObjectWrapper&lt;J&gt;, or type compatible with JSImport type marshalling.</typeparam>
    /// <param name="wrapper">The wrapper whose JSObject to invoke funcName on.</param>
    /// <param name="parameters">SerratedJS.JSParams bundle to pass to the JS function.</param>
    /// <param name="funcName">Name of the JS function on this wrapper's <c>JSObject</c> to call.</param>
    /// <returns>The JS call's return, casted or wrapped to requested type <c>J</c>.</returns>
    public static J CallJS<J>(this IJSObjectWrapper wrapper, JSParams parameters = default, [CallerMemberName] string? funcName = null)
        => JSImportInstanceHelpers.CallJSFunc<J>(wrapper.JSObject, funcName!, parameters.Args);

    /// <summary>
    /// <para>Call JS <c>funcName</c> on this IJSObjectWrapper's JSObject with the given JSParams.</para>
    /// <para>var someWrapper = wrapper.CallJS&lt;SomeWrapper&gt;("someJSFunc", SerratedJS.JSParams(...))</para>
    /// </summary>
    /// <typeparam name="J">Type to return. Typically a JSObject, IJSObjectWrapper&lt;J&gt;, or type compatible with JSImport type marshalling.</typeparam>
    /// <param name="wrapper">The wrapper whose JSObject to invoke funcName on.</param>
    /// <param name="funcName">Name of the JS function on this wrapper's <c>JSObject</c> to call.</param>
    /// <param name="parameters">SerratedJS.JSParams bundle to pass to the JS function.</param>
    /// <returns>The JS call's return, casted or wrapped to requested type <c>J</c>.</returns>
    public static J CallJS<J>(this IJSObjectWrapper wrapper, string funcName, JSParams parameters = default)
        => JSImportInstanceHelpers.CallJSFunc<J>(wrapper.JSObject, funcName, parameters.Args);

    /// <summary>
    /// <para>Call JS <c>funcName</c> on this IJSObjectWrapper's JSObject with the given arguments.</para>
    /// <para>var someWrapper = wrapper.CallJS&lt;SomeWrapper&gt;("someJSFunc", "param1", 25, someJSObject, someJSWrapper)</para>
    /// </summary>
    /// <typeparam name="J">Type to return. Typically a JSObject, IJSObjectWrapper&lt;J&gt;, or type compatible with JSImport type marshalling.</typeparam>
    /// <param name="wrapper">The wrapper whose JSObject to invoke funcName on.</param>
    /// <param name="funcName">Name of the JS function on this wrapper's <c>JSObject</c> to call.</param>
    /// <param name="parameters">Arguments to pass to the JS function.</param>
    /// <returns>The JS call's return, casted or wrapped to requested type <c>J</c>.</returns>
    public static J CallJS<J>(this IJSObjectWrapper wrapper, string funcName, params object[] parameters)
        => JSImportInstanceHelpers.CallJSFunc<J>(wrapper.JSObject, funcName, parameters);

    /// <summary>
    /// <para>Call JS <c>funcName</c>(inferred via [CallerMemberName]) on this IJSObjectWrapper's JSObject; no return value.</para>
    /// <para>wrapper.CallJS(SerratedJS.JSParams(...))</para>
    /// </summary>
    /// <param name="wrapper">The wrapper whose JSObject to invoke funcName on.</param>
    /// <param name="parameters">SerratedJS.JSParams bundle to pass to the JS function.</param>
    /// <param name="funcName">Name of the JS function on this wrapper's <c>JSObject</c> to call.</param>
    public static void CallJS(this IJSObjectWrapper wrapper, JSParams parameters = default, [CallerMemberName] string? funcName = null)
        => JSImportInstanceHelpers.CallJSFuncVoid(wrapper.JSObject, funcName!, parameters.Args);

    /// <summary>
    /// <para>Call JS <c>funcName</c> on this IJSObjectWrapper's JSObject with the given JSParams; no return value.</para>
    /// <para>wrapper.CallJS("someJSFunc", SerratedJS.JSParams(...))</para>
    /// </summary>
    /// <param name="wrapper">The wrapper whose JSObject to invoke funcName on.</param>
    /// <param name="funcName">Name of the JS function on this wrapper's <c>JSObject</c> to call.</param>
    /// <param name="parameters">SerratedJS.JSParams bundle to pass to the JS function.</param>
    public static void CallJS(this IJSObjectWrapper wrapper, string funcName, JSParams parameters = default)
        => JSImportInstanceHelpers.CallJSFuncVoid(wrapper.JSObject, funcName, parameters.Args);

    /// <summary>
    /// <para>Call JS <c>funcName</c> on this IJSObjectWrapper's JSObject with the given arguments; no return value.</para>
    /// <para>wrapper.CallJS("someJSFunc", "param1", 25, someJSObject, someJSWrapper)</para>
    /// </summary>
    /// <param name="wrapper">The wrapper whose JSObject to invoke funcName on.</param>
    /// <param name="funcName">Name of the JS function on this wrapper's <c>JSObject</c> to call.</param>
    /// <param name="parameters">Arguments to pass to the JS function.</param>
    public static void CallJS(this IJSObjectWrapper wrapper, string funcName, params object[] parameters)
        => JSImportInstanceHelpers.CallJSFuncVoid(wrapper.JSObject, funcName, parameters);

    // CONSIDER: Overloads for string and int arrays, and any other array types that get unintentionally multiplexed by `params`. This may already be solved by Serrated.JSParams.ArrayParam

    /// <summary>
    /// <para>Get the value of <c>propertyName</c>(inferred via [CallerMemberName]) on this IJSObjectWrapper's JSObject.</para>
    /// <para>var value = wrapper.GetProperty&lt;string&gt;()</para>
    /// </summary>
    /// <typeparam name="J">Type of the property value. Typically a JSObject, IJSObjectWrapper&lt;J&gt;, or type compatible with JSImport type marshalling.</typeparam>
    /// <param name="wrapper">The wrapper whose JSObject to read the property from.</param>
    /// <param name="propertyName">Name of the property on this wrapper's <c>JSObject</c> to get.</param>
    /// <returns>The property value, casted or wrapped to requested type <c>J</c>.</returns>
    public static J GetProperty<J>(this IJSObjectWrapper wrapper, [CallerMemberName] string? propertyName = null)
        => JSImportInstanceHelpers.GetProperty<J>(wrapper.JSObject, propertyName!);

    /// <summary>
    /// <para>Set the value of <c>propertyName</c>(inferred via [CallerMemberName]) on this IJSObjectWrapper's JSObject.</para>
    /// <para>wrapper.SetProperty(value)</para>
    /// </summary>
    /// <param name="wrapper">The wrapper whose JSObject to set the property on.</param>
    /// <param name="value">Value to set. IJSObjectWrapper instances are unwrapped to their JSObject.</param>
    /// <param name="propertyName">Name of the property on this wrapper's <c>JSObject</c> to set.</param>
    public static void SetProperty(this IJSObjectWrapper wrapper, object value, [CallerMemberName] string? propertyName = null)
    {
        object valueToSet = value is IJSObjectWrapper w ? w.JSObject : value;
        JSImportInstanceHelpers.SetProperty(wrapper.JSObject, propertyName!, valueToSet);
    }
}

/// <summary>
/// JSObject extensions
/// </summary>
public static class JSObjectExtensionsV2 {

    // var someWrapper = this.CallJS<SomeWrapper>(SerratedJS.JSParams(...))
    /// <summary>
    /// <para>Call JS <c>funcName</c>(inferred via [CallerMemberName]) on this JSObject.</para>
    /// <para>var someWrapper = this.CallJS&lt;SomeWrapper&gt;(SerratedJS.JSParams(...))</para>
    /// </summary>
    /// <typeparam name="J">Type to return. Typically a JSOBject, IJSObjectWrapper&lt;J&gt;, or type compatible with JSImport type marshalling.</typeparam>
    /// <param name="jsObject">Reference to the JS instance to invoke funcName on.</param>
    /// <param name="parameters">SerratedJS.JSParams bundle to pass to the JS function.</param>
    /// <param name="funcName">Name of the JS function on this <c>jsObject</c> to call.</param>
    /// <returns>The JS call's return, casted or wrapped to requested type <c>J</c>.</returns>
    public static J CallJS<J>(this JSObject jsObject, JSParams parameters = default, [CallerMemberName] string? funcName = null)
        => JSImportInstanceHelpers.CallJSFunc<J>(jsObject, funcName!, parameters.Args);

    // var someWrapper = this.CallJS<SomeWrapper>("someJSFunc", SerratedJS.JSParams(...));
    /// <summary>
    /// <para>Call JS <c>funcName</c> on this JSObject with the given JSParams.</para>
    /// <para>var someWrapper = this.CallJS&lt;SomeWrapper&gt;("someJSFunc", SerratedJS.JSParams(...))</para>
    /// </summary>
    /// <typeparam name="J">Type to return. Typically a JSObject, IJSObjectWrapper&lt;J&gt;, or type compatible with JSImport type marshalling.</typeparam>
    /// <param name="jsObject">Reference to the JS instance to invoke funcName on.</param>
    /// <param name="funcName">Name of the JS function on this <c>jsObject</c> to call.</param>
    /// <param name="parameters">SerratedJS.JSParams bundle to pass to the JS function.</param>
    /// <returns>The JS call's return, casted or wrapped to requested type <c>J</c>.</returns>
    public static J CallJS<J>(this JSObject jsObject, string funcName, JSParams parameters = default)
        => JSImportInstanceHelpers.CallJSFunc<J>(jsObject, funcName, parameters.Args);

    // var someWrapper = this.CallJS<SomeWrapper>("someJSFunc", "param1", 25, someJSOject, someJSWrapper);
    /// <summary>
    /// <para>Call JS <c>funcName</c> on this JSObject with the given arguments.</para>
    /// <para>var someWrapper = this.CallJS&lt;SomeWrapper&gt;("someJSFunc", "param1", 25, someJSObject, someJSWrapper)</para>
    /// </summary>
    /// <typeparam name="J">Type to return. Typically a JSObject, IJSObjectWrapper&lt;J&gt;, or type compatible with JSImport type marshalling.</typeparam>
    /// <param name="jsObject">Reference to the JS instance to invoke funcName on.</param>
    /// <param name="funcName">Name of the JS function on this <c>jsObject</c> to call.</param>
    /// <param name="parameters">Arguments to pass to the JS function.</param>
    /// <returns>The JS call's return, casted or wrapped to requested type <c>J</c>.</returns>
    public static J CallJS<J>(this JSObject jsObject, string funcName, params object[] parameters)
        => JSImportInstanceHelpers.CallJSFunc<J>(jsObject, funcName, parameters);

    // this.CallJS("someJSFunc", SerratedJS.JSParams(...));
    /// <summary>
    /// <para>Call JS <c>funcName</c>(inferred via [CallerMemberName]) on this JSObject; no return value.</para>
    /// <para>this.CallJS(SerratedJS.JSParams(...))</para>
    /// </summary>
    /// <param name="jsObject">Reference to the JS instance to invoke funcName on.</param>
    /// <param name="parameters">SerratedJS.JSParams bundle to pass to the JS function.</param>
    /// <param name="funcName">Name of the JS function on this <c>jsObject</c> to call.</param>
    public static void CallJS(this JSObject jsObject, JSParams parameters = default, [CallerMemberName] string? funcName = null)
        => JSImportInstanceHelpers.CallJSFuncVoid(jsObject, funcName!, parameters.Args);

    // this.CallJS("someJSFunc", SerratedJS.JSParams(...));
    /// <summary>
    /// <para>Call JS <c>funcName</c> on this JSObject with the given JSParams; no return value.</para>
    /// <para>this.CallJS("someJSFunc", SerratedJS.JSParams(...))</para>
    /// </summary>
    /// <param name="jsObject">Reference to the JS instance to invoke funcName on.</param>
    /// <param name="funcName">Name of the JS function on this <c>jsObject</c> to call.</param>
    /// <param name="parameters">SerratedJS.JSParams bundle to pass to the JS function.</param>
    public static void CallJS(this JSObject jsObject, string funcName, JSParams parameters = default)
        => JSImportInstanceHelpers.CallJSFuncVoid(jsObject, funcName, parameters.Args);

    // this.CallJS("someJSFunc", "param1", 25, someJSOject, someJSWrapper);
    /// <summary>
    /// <para>Call JS <c>funcName</c> on this JSObject with the given arguments; no return value.</para>
    /// <para>this.CallJS("someJSFunc", "param1", 25, someJSObject, someJSWrapper)</para>
    /// </summary>
    /// <param name="jsObject">Reference to the JS instance to invoke funcName on.</param>
    /// <param name="funcName">Name of the JS function on this <c>jsObject</c> to call.</param>
    /// <param name="parameters">Arguments to pass to the JS function.</param>
    public static void CallJS(this JSObject jsObject, string funcName, params object[] parameters)
        => JSImportInstanceHelpers.CallJSFuncVoid(jsObject, funcName, parameters);

    // CONSIDER: Overloads for string and int arrays, and any other array types that get unintentionally multiplexed by `params`. This may already be solved by Serrated.JSParams.ArrayParam

    // this.GetProperty("someProperty");
    /// <summary>
    /// <para>Get the value of <c>propertyName</c>(inferred via [CallerMemberName]) on this JSObject.</para>
    /// <para>this.GetProperty("someProperty")</para>
    /// </summary>
    /// <typeparam name="J">Type of the property value. Typically a JSObject, IJSObjectWrapper&lt;J&gt;, or type compatible with JSImport type marshalling.</typeparam>
    /// <param name="jsObject">Reference to the JS instance to read the property from.</param>
    /// <param name="propertyName">Name of the property on this <c>jsObject</c> to get.</param>
    /// <returns>The property value, casted or wrapped to requested type <c>J</c>.</returns>
    public static J GetProperty<J>(this JSObject jsObject, [CallerMemberName] string? propertyName = null)
        => JSImportInstanceHelpers.GetProperty<J>(jsObject, propertyName!);

    // this.SetProperty("someProperty", "value");
    /// <summary>
    /// <para>Set the value of <c>propertyName</c>(inferred via [CallerMemberName]) on this JSObject.</para>
    /// <para>this.SetProperty("someProperty", "value")</para>
    /// </summary>
    /// <param name="jsObject">Reference to the JS instance to set the property on.</param>
    /// <param name="value">Value to set.</param>
    /// <param name="propertyName">Name of the property on this <c>jsObject</c> to set.</param>
    public static void SetProperty(this JSObject jsObject, object value, [CallerMemberName] string? propertyName = null)
        => JSImportInstanceHelpers.SetProperty(jsObject, propertyName!, value);

}
