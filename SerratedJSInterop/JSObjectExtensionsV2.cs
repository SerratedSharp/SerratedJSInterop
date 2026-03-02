using SerratedSharp.SerratedJSInterop.Internal;
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
    // CONSIDER: Checking parameters for IJSObjectWrapper and conditionally do paramItem.JSObject to pass the native object automatically.

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
    [OverloadResolutionPriority(20)]
    public static JSObject New(string typePath, JSParams parameters)
    {
        var args = parameters.Args ?? Array.Empty<object>();
        var unwrapped = JSImportInstanceHelpers.UnwrapJSObjectParams(args);
        return HelpersJS.ObjectNew(typePath, unwrapped);
    }

    /// <summary>
    /// Calls JS constructor for `typePath` constructor name and namespace if required.  E.g. "PIXI.Rectangle", "Image". Returns <see cref="JSObject"/>.
    /// </summary>
    /// <param name="typePath">JS constructor name. Include fully qualified path if necessary (e.g. "PIXI.Rectangle", "Image").  Is case sensitive.</param>
    /// <param name="parameters">Optional constructor arguments (e.g. SerratedJS.New("Rectangle", "green", 50, 100)).</param>
    /// <returns>JSObject reference to the new object.</returns>
    public static JSObject New(string typePath, params object[] parameters)
    {
        return New(typePath, Params(parameters));
    }

    // CONSIDER: Overloads for string[] and other array types to avoid unintentional expansion when passing arrays as constructor arguments.

}

/// <summary>
/// IJSObjectWrapper extensions
/// </summary>
public static class WrapperCallJSReturnTypeExtensions
{

    #region Overloads of inferred name CallJS<J> for 0 thru 5 parameters

    /// <summary>
    /// <para>Call JS <c>funcName</c>(inferred via [CallerMemberName]) on this IJSObjectWrapper's JSObject.</para>
    /// <para>var someWrapper = wrapper.CallJS&lt;SomeWrapper&gt;()</para>
    /// </summary>
    /// <typeparam name="J">Type to return. Typically a JSObject, IJSObjectWrapper&lt;J&gt;, or type compatible with JSImport type marshalling.</typeparam>
    /// <param name="wrapper">The wrapper whose JSObject to invoke funcName on.</param>
    /// <param name="funcName">Name of the JS function on this wrapper's <c>JSObject</c> to call.</param>
    /// <returns>The JS call's return, casted or wrapped to requested type <c>J</c>.</returns>
    public static J CallJS<J>(this IJSObjectWrapper wrapper, Breaker _ = default, [CallerMemberName] string? funcName = null)
        => JSImportInstanceHelpers.CallJSFunc<J>(wrapper.JSObject, funcName!);

    /// <summary>
    /// <para>Call JS <c>funcName</c>(inferred via [CallerMemberName]) on this IJSObjectWrapper's JSObject.</para>
    /// <para>var someWrapper = wrapper.CallJS&lt;SomeWrapper&gt;(...)</para>
    /// </summary>
    /// <typeparam name="J">Type to return. Typically a JSObject, IJSObjectWrapper&lt;J&gt;, or type compatible with JSImport type marshalling.</typeparam>
    /// <param name="wrapper">The wrapper whose JSObject to invoke funcName on.</param>
    /// <param name="param1">A parameter to pass to the JS function.  Must be JSObject, IJSObjectWrapper, or type compatible with JS interop.</param>
    /// <param name="funcName">Name of the JS function on this wrapper's <c>JSObject</c> to call.</param>
    /// <returns>The JS call's return, casted or wrapped to requested type <c>J</c>.</returns>
    [OverloadResolutionPriority(1)]
    public static J CallJS<J>(this IJSObjectWrapper wrapper, object param1, Breaker _ = default, [CallerMemberName] string? funcName = null)
        => JSImportInstanceHelpers.CallJSFunc<J>(wrapper.JSObject, funcName!, param1);

    /// <summary>
    /// <para>Call JS <c>funcName</c>(inferred via [CallerMemberName]) on this IJSObjectWrapper's JSObject.</para>
    /// <para>var someWrapper = wrapper.CallJS&lt;SomeWrapper&gt;(...)</para>
    /// </summary>
    /// <typeparam name="J">Type to return. Typically a JSObject, IJSObjectWrapper&lt;J&gt;, or type compatible with JSImport type marshalling.</typeparam>
    /// <param name="wrapper">The wrapper whose JSObject to invoke funcName on.</param>
    /// <param name="param1">First parameter to pass to the JS function.  Must be JSObject, IJSObjectWrapper, or type compatible with JS interop.</param>
    /// <param name="param2">Second parameter to pass to the JS function.  Must be JSObject, IJSObjectWrapper, or type compatible with JS interop.</param>
    /// <param name="funcName">Name of the JS function on this wrapper's <c>JSObject</c> to call.</param>
    /// <returns>The JS call's return, casted or wrapped to requested type <c>J</c>.</returns>
    [OverloadResolutionPriority(1)]
    public static J CallJS<J>(this IJSObjectWrapper wrapper, object param1, object param2, Breaker _ = default, [CallerMemberName] string? funcName = null)
        => JSImportInstanceHelpers.CallJSFunc<J>(wrapper.JSObject, funcName!, param1, param2);

    /// <summary>
    /// <para>Call JS <c>funcName</c>(inferred via [CallerMemberName]) on this IJSObjectWrapper's JSObject.</para>
    /// <para>var someWrapper = wrapper.CallJS&lt;SomeWrapper&gt;(...)</para>
    /// </summary>
    /// <typeparam name="J">Type to return. Typically a JSObject, IJSObjectWrapper&lt;J&gt;, or type compatible with JSImport type marshalling.</typeparam>
    /// <param name="wrapper">The wrapper whose JSObject to invoke funcName on.</param>
    /// <param name="param1">First parameter to pass to the JS function.  Must be JSObject, IJSObjectWrapper, or type compatible with JS interop.</param>
    /// <param name="param2">Second parameter to pass to the JS function.  Must be JSObject, IJSObjectWrapper, or type compatible with JS interop.</param>
    /// <param name="param3">Third parameter to pass to the JS function.  Must be JSObject, IJSObjectWrapper, or type compatible with JS interop.</param>
    /// <param name="funcName">Name of the JS function on this wrapper's <c>JSObject</c> to call.</param>
    /// <returns>The JS call's return, casted or wrapped to requested type <c>J</c>.</returns>
    [OverloadResolutionPriority(1)]
    public static J CallJS<J>(this IJSObjectWrapper wrapper, object param1, object param2, object param3, Breaker _ = default, [CallerMemberName] string? funcName = null)
        => JSImportInstanceHelpers.CallJSFunc<J>(wrapper.JSObject, funcName!, param1, param2, param3);

    /// <summary>
    /// <para>Call JS <c>funcName</c>(inferred via [CallerMemberName]) on this IJSObjectWrapper's JSObject.</para>
    /// <para>var someWrapper = wrapper.CallJS&lt;SomeWrapper&gt;(...)</para>
    /// </summary>
    /// <typeparam name="J">Type to return. Typically a JSObject, IJSObjectWrapper&lt;J&gt;, or type compatible with JSImport type marshalling.</typeparam>
    /// <param name="wrapper">The wrapper whose JSObject to invoke funcName on.</param>
    /// <param name="param1">First parameter to pass to the JS function.  Must be JSObject, IJSObjectWrapper, or type compatible with JS interop.</param>
    /// <param name="param2">Second parameter to pass to the JS function.  Must be JSObject, IJSObjectWrapper, or type compatible with JS interop.</param>
    /// <param name="param3">Third parameter to pass to the JS function.  Must be JSObject, IJSObjectWrapper, or type compatible with JS interop.</param>
    /// <param name="param4">Fourth parameter to pass to the JS function.  Must be JSObject, IJSObjectWrapper, or type compatible with JS interop.</param>
    /// <param name="funcName">Name of the JS function on this wrapper's <c>JSObject</c> to call.</param>
    /// <returns>The JS call's return, casted or wrapped to requested type <c>J</c>.</returns>
    [OverloadResolutionPriority(1)]
    public static J CallJS<J>(this IJSObjectWrapper wrapper, object param1, object param2, object param3, object param4, Breaker _ = default, [CallerMemberName] string? funcName = null)
        => JSImportInstanceHelpers.CallJSFunc<J>(wrapper.JSObject, funcName!, param1, param2, param3, param4);

    /// <summary>
    /// <para>Call JS <c>funcName</c>(inferred via [CallerMemberName]) on this IJSObjectWrapper's JSObject.</para>
    /// <para>var someWrapper = wrapper.CallJS&lt;SomeWrapper&gt;(...)</para>
    /// </summary>
    /// <typeparam name="J">Type to return. Typically a JSObject, IJSObjectWrapper&lt;J&gt;, or type compatible with JSImport type marshalling.</typeparam>
    /// <param name="wrapper">The wrapper whose JSObject to invoke funcName on.</param>
    /// <param name="param1">First parameter to pass to the JS function.  Must be JSObject, IJSObjectWrapper, or type compatible with JS interop.</param>
    /// <param name="param2">Second parameter to pass to the JS function.  Must be JSObject, IJSObjectWrapper, or type compatible with JS interop.</param>
    /// <param name="param3">Third parameter to pass to the JS function.  Must be JSObject, IJSObjectWrapper, or type compatible with JS interop.</param>
    /// <param name="param4">Fourth parameter to pass to the JS function.  Must be JSObject, IJSObjectWrapper, or type compatible with JS interop.</param>
    /// <param name="param5">Fifth parameter to pass to the JS function.  Must be JSObject, IJSObjectWrapper, or type compatible with JS interop.</param>
    /// <param name="funcName">Name of the JS function on this wrapper's <c>JSObject</c> to call.</param>
    /// <returns>The JS call's return, casted or wrapped to requested type <c>J</c>.</returns>
    [OverloadResolutionPriority(1)]
    public static J CallJS<J>(this IJSObjectWrapper wrapper, object param1, object param2, object param3, object param4, object param5, Breaker _ = default, [CallerMemberName] string? funcName = null)
        => JSImportInstanceHelpers.CallJSFunc<J>(wrapper.JSObject, funcName!, param1, param2, param3, param4, param5);

    #endregion

    #region Marshal helpers

    ///// <summary>
    ///// Marshal a native JavaScript array of objects referenced by this wrapper's <see cref="IJSObjectWrapper.JSObject"/>
    ///// into a <see cref="JSObject"/> array.
    ///// </summary>
    ///// <param name="wrapper">Wrapper whose <see cref="IJSObjectWrapper.JSObject"/> points at a native JS array.</param>
    ///// <returns>An array of <see cref="JSObject"/> references for each element in the underlying JS array.</returns>
    //public static JSObject[] MarshalAsArrayOfObjects(this IJSObjectWrapper wrapper)
    //    => HelpersJS.MarshalAsArrayOfObjects(wrapper.JSObject);

    #endregion

    #region Explicit funcName with variable Params, should be called with CallJS(funcName: "someFunc", ...) to force this overload

    /// <summary>
    /// <para>Call JS <c>funcName</c> on this IJSObjectWrapper's JSObject with the given JSParams.</para>
    /// <para>var someWrapper = wrapper.CallJS&lt;SomeWrapper&gt;("someJSFunc", SerratedJS.Params(...))</para>
    /// </summary>
    /// <typeparam name="J">Type to return. Typically a JSObject, IJSObjectWrapper&lt;J&gt;, or type compatible with JSImport type marshalling.</typeparam>
    /// <param name="wrapper">The wrapper whose JSObject to invoke funcName on.</param>
    /// <param name="funcName">Name of the JS function on this wrapper's <c>JSObject</c> to call.</param>
    /// <param name="parameters">SerratedJS.JSParams bundle to pass to the JS function.</param>
    /// <returns>The JS call's return, casted or wrapped to requested type <c>J</c>.</returns>
    [OverloadResolutionPriority(20)]
    public static J CallJS<J>(this IJSObjectWrapper wrapper, string funcName, JSParams parameters)
        => JSImportInstanceHelpers.CallJSFunc<J>(wrapper.JSObject, funcName, parameters.Args);

    /// <summary>
    /// <para>Call JS <c>funcName</c> on this IJSObjectWrapper's JSObject with the given arguments.</para>
    /// <para>var someWrapper = wrapper.CallJS&lt;SomeWrapper&gt;("someJSFunc", "param1", 25, someJSObject, someJSWrapper)</para>
    /// <para>Note: Use the named parameter syntax <c>funcName:</c> to disambiguate from the inferred-name overload when passing a single string argument:
    /// <c>wrapper.CallJS&lt;J&gt;(funcName: "someJSFunc")</c></para>
    /// </summary>
    /// <typeparam name="J">Type to return. Typically a JSObject, IJSObjectWrapper&lt;J&gt;, or type compatible with JSImport type marshalling.</typeparam>
    /// <param name="wrapper">The wrapper whose JSObject to invoke funcName on.</param>
    /// <param name="funcName">Name of the JS function on this wrapper's <c>JSObject</c> to call.</param>
    /// <param name="parameters">Arguments to pass to the JS function.</param>
    /// <returns>The JS call's return, casted or wrapped to requested type <c>J</c>.</returns>
    public static J CallJS<J>(this IJSObjectWrapper wrapper, string funcName, params object[] parameters)
        => JSImportInstanceHelpers.CallJSFunc<J>(wrapper.JSObject, funcName, parameters);

    // CONSIDER: Overloads for string and int arrays, and any other array types that get unintentionally multiplexed by `params`. This may already be solved by Serrated.JSParams.ArrayParam

    #endregion

    /// <summary>
    /// <para>Call JS <c>funcName</c>(inferred via [CallerMemberName]) on this IJSObjectWrapper's JSObject.</para>
    /// <para>var someWrapper = wrapper.CallJS&lt;SomeWrapper&gt;(SerratedJS.Params(...))</para>
    /// </summary>
    /// <typeparam name="J">Type to return. Typically a JSObject, IJSObjectWrapper&lt;J&gt;, or type compatible with JSImport type marshalling.</typeparam>
    /// <param name="wrapper">The wrapper whose JSObject to invoke funcName on.</param>
    /// <param name="parameters">SerratedJS.JSParams bundle to pass to the JS function.</param>
    /// <param name="funcName">Name of the JS function on this wrapper's <c>JSObject</c> to call.</param>
    /// <returns>The JS call's return, casted or wrapped to requested type <c>J</c>.</returns>
    [OverloadResolutionPriority(20)]
    public static J CallJS<J>(this IJSObjectWrapper wrapper, JSParams parameters, Breaker _ = default, [CallerMemberName] string? funcName = null)
        => JSImportInstanceHelpers.CallJSFunc<J>(wrapper.JSObject, funcName!, parameters.Args);

}

public static class WrapperCallJSReturnVoidExtensions
{

    #region Overloads of inferred name CallJS<VOID> for 0 thru 5 parameters

    /// <summary>
    /// <para>Call JS <c>funcName</c>(inferred via [CallerMemberName]) on this IJSObjectWrapper's JSObject; no return value.</para>
    /// <para>wrapper.CallJS()</para>
    /// </summary>
    /// <param name="wrapper">The wrapper whose JSObject to invoke funcName on.</param>
    /// <param name="funcName">Name of the JS function on this wrapper's <c>JSObject</c> to call.</param>
    public static void CallJS(this IJSObjectWrapper wrapper, Breaker _ = default, [CallerMemberName] string? funcName = null)
        => JSImportInstanceHelpers.CallJSFuncVoid(wrapper.JSObject, funcName!);

    /// <summary>
    /// <para>Call JS <c>funcName</c>(inferred via [CallerMemberName]) on this IJSObjectWrapper's JSObject; no return value.</para>
    /// <para>wrapper.CallJS(param1)</para>
    /// </summary>
    /// <param name="wrapper">The wrapper whose JSObject to invoke funcName on.</param>
    /// <param name="param1">A parameter to pass to the JS function.  Must be JSObject, IJSObjectWrapper, or type compatible with JS interop.</param>
    /// <param name="funcName">Name of the JS function on this wrapper's <c>JSObject</c> to call.</param>
    [OverloadResolutionPriority(1)]
    public static void CallJS(this IJSObjectWrapper wrapper, object param1, Breaker _ = default, [CallerMemberName] string? funcName = null)
        => JSImportInstanceHelpers.CallJSFuncVoid(wrapper.JSObject, funcName!, param1);

    /// <summary>
    /// <para>Call JS <c>funcName</c>(inferred via [CallerMemberName]) on this IJSObjectWrapper's JSObject; no return value.</para>
    /// <para>wrapper.CallJS(param1, param2)</para>
    /// </summary>
    /// <param name="wrapper">The wrapper whose JSObject to invoke funcName on.</param>
    /// <param name="param1">First parameter to pass to the JS function.  Must be JSObject, IJSObjectWrapper, or type compatible with JS interop.</param>
    /// <param name="param2">Second parameter to pass to the JS function.  Must be JSObject, IJSObjectWrapper, or type compatible with JS interop.</param>
    /// <param name="funcName">Name of the JS function on this wrapper's <c>JSObject</c> to call.</param>
    [OverloadResolutionPriority(1)]
    public static void CallJS(this IJSObjectWrapper wrapper, object param1, object param2, Breaker _ = default, [CallerMemberName] string? funcName = null)
        => JSImportInstanceHelpers.CallJSFuncVoid(wrapper.JSObject, funcName!, param1, param2);

    /// <summary>
    /// <para>Call JS <c>funcName</c>(inferred via [CallerMemberName]) on this IJSObjectWrapper's JSObject; no return value.</para>
    /// <para>wrapper.CallJS(param1, param2, param3)</para>
    /// </summary>
    /// <param name="wrapper">The wrapper whose JSObject to invoke funcName on.</param>
    /// <param name="param1">First parameter to pass to the JS function.  Must be JSObject, IJSObjectWrapper, or type compatible with JS interop.</param>
    /// <param name="param2">Second parameter to pass to the JS function.  Must be JSObject, IJSObjectWrapper, or type compatible with JS interop.</param>
    /// <param name="param3">Third parameter to pass to the JS function.  Must be JSObject, IJSObjectWrapper, or type compatible with JS interop.</param>
    /// <param name="funcName">Name of the JS function on this wrapper's <c>JSObject</c> to call.</param>
    [OverloadResolutionPriority(1)]
    public static void CallJS(this IJSObjectWrapper wrapper, object param1, object param2, object param3, Breaker _ = default, [CallerMemberName] string? funcName = null)
        => JSImportInstanceHelpers.CallJSFuncVoid(wrapper.JSObject, funcName!, param1, param2, param3);

    /// <summary>
    /// <para>Call JS <c>funcName</c>(inferred via [CallerMemberName]) on this IJSObjectWrapper's JSObject; no return value.</para>
    /// <para>wrapper.CallJS(param1, param2, param3, param4)</para>
    /// </summary>
    /// <param name="wrapper">The wrapper whose JSObject to invoke funcName on.</param>
    /// <param name="param1">First parameter to pass to the JS function.  Must be JSObject, IJSObjectWrapper, or type compatible with JS interop.</param>
    /// <param name="param2">Second parameter to pass to the JS function.  Must be JSObject, IJSObjectWrapper, or type compatible with JS interop.</param>
    /// <param name="param3">Third parameter to pass to the JS function.  Must be JSObject, IJSObjectWrapper, or type compatible with JS interop.</param>
    /// <param name="param4">Fourth parameter to pass to the JS function.  Must be JSObject, IJSObjectWrapper, or type compatible with JS interop.</param>
    /// <param name="funcName">Name of the JS function on this wrapper's <c>JSObject</c> to call.</param>
    [OverloadResolutionPriority(1)]
    public static void CallJS(this IJSObjectWrapper wrapper, object param1, object param2, object param3, object param4, Breaker _ = default, [CallerMemberName] string? funcName = null)
        => JSImportInstanceHelpers.CallJSFuncVoid(wrapper.JSObject, funcName!, param1, param2, param3, param4);

    /// <summary>
    /// <para>Call JS <c>funcName</c>(inferred via [CallerMemberName]) on this IJSObjectWrapper's JSObject; no return value.</para>
    /// <para>wrapper.CallJS(param1, param2, param3, param4, param5)</para>
    /// </summary>
    /// <param name="wrapper">The wrapper whose JSObject to invoke funcName on.</param>
    /// <param name="param1">First parameter to pass to the JS function.  Must be JSObject, IJSObjectWrapper, or type compatible with JS interop.</param>
    /// <param name="param2">Second parameter to pass to the JS function.  Must be JSObject, IJSObjectWrapper, or type compatible with JS interop.</param>
    /// <param name="param3">Third parameter to pass to the JS function.  Must be JSObject, IJSObjectWrapper, or type compatible with JS interop.</param>
    /// <param name="param4">Fourth parameter to pass to the JS function.  Must be JSObject, IJSObjectWrapper, or type compatible with JS interop.</param>
    /// <param name="param5">Fifth parameter to pass to the JS function.  Must be JSObject, IJSObjectWrapper, or type compatible with JS interop.</param>
    /// <param name="funcName">Name of the JS function on this wrapper's <c>JSObject</c> to call.</param>
    [OverloadResolutionPriority(1)]
    public static void CallJS(this IJSObjectWrapper wrapper, object param1, object param2, object param3, object param4, object param5, Breaker _ = default, [CallerMemberName] string? funcName = null)
        => JSImportInstanceHelpers.CallJSFuncVoid(wrapper.JSObject, funcName!, param1, param2, param3, param4, param5);

    #endregion

    #region Explicit funcName with variable Params, should be called with CallJS(funcName: "someFunc", ...) to force this overload

    /// <summary>
    /// <para>Call JS <c>funcName</c> on this IJSObjectWrapper's JSObject with the given JSParams; no return value.</para>
    /// <para>wrapper.CallJS("someJSFunc", SerratedJS.Params(...))</para>
    /// </summary>
    /// <param name="wrapper">The wrapper whose JSObject to invoke funcName on.</param>
    /// <param name="funcName">Name of the JS function on this wrapper's <c>JSObject</c> to call.</param>
    /// <param name="parameters">SerratedJS.JSParams bundle to pass to the JS function.</param>
    [OverloadResolutionPriority(20)]
    public static void CallJS(this IJSObjectWrapper wrapper, string funcName, JSParams parameters)
        => JSImportInstanceHelpers.CallJSFuncVoid(wrapper.JSObject, funcName, parameters.Args);

    /// <summary>
    /// <para>Call JS <c>funcName</c> on this IJSObjectWrapper's JSObject with the given arguments; no return value.</para>
    /// <para>wrapper.CallJS("someJSFunc", "param1", 25, someJSObject, someJSWrapper)</para>
    /// <para>Note: Use the named parameter syntax <c>funcName:</c> to disambiguate from the inferred-name overload when passing a single string argument:
    /// <c>wrapper.CallJS(funcName: "someJSFunc")</c></para>
    /// </summary>
    /// <param name="wrapper">The wrapper whose JSObject to invoke funcName on.</param>
    /// <param name="funcName">Name of the JS function on this wrapper's <c>JSObject</c> to call.</param>
    /// <param name="parameters">Arguments to pass to the JS function.</param>
    public static void CallJS(this IJSObjectWrapper wrapper, string funcName, params object[] parameters)
        => JSImportInstanceHelpers.CallJSFuncVoid(wrapper.JSObject, funcName, parameters);

    // CONSIDER: Overloads for string and int arrays, and any other array types that get unintentionally multiplexed by `params`. This may already be solved by Serrated.JSParams.ArrayParam

    #endregion

    /// <summary>
    /// <para>Call JS <c>funcName</c>(inferred via [CallerMemberName]) on this IJSObjectWrapper's JSObject; no return value.</para>
    /// <para>wrapper.CallJS(SerratedJS.Params(...))</para>
    /// </summary>
    /// <param name="wrapper">The wrapper whose JSObject to invoke funcName on.</param>
    /// <param name="parameters">SerratedJS.JSParams bundle to pass to the JS function.</param>
    /// <param name="funcName">Name of the JS function on this wrapper's <c>JSObject</c> to call.</param>
    [OverloadResolutionPriority(20)]
    public static void CallJS(this IJSObjectWrapper wrapper, JSParams parameters, Breaker _ = default, [CallerMemberName] string? funcName = null)
        => JSImportInstanceHelpers.CallJSFuncVoid(wrapper.JSObject, funcName!, parameters.Args);

}

public static class WrapperPropertyExtensions
{
    /// <summary>
    /// <para>Get the value of <c>propertyName</c>(inferred via [CallerMemberName]) on this IJSObjectWrapper's JSObject.</para>
    /// <para>var value = wrapper.GetJSProperty&lt;string&gt;()</para>
    /// </summary>
    /// <typeparam name="J">Type of the property value. Typically a JSObject, IJSObjectWrapper&lt;J&gt;, or type compatible with JSImport type marshalling.</typeparam>
    /// <param name="wrapper">The wrapper whose JSObject to read the property from.</param>
    /// <param name="propertyName">Name of the property on this wrapper's <c>JSObject</c> to get.</param>
    /// <returns>The property value, casted or wrapped to requested type <c>J</c>.</returns>
    public static J GetJSProperty<J>(this IJSObjectWrapper wrapper, [CallerMemberName] string? propertyName = null)
        => JSImportInstanceHelpers.GetProperty<J>(wrapper.JSObject, propertyName!);

    /// <summary>
    /// <para>Set the value of <c>propertyName</c>(inferred via [CallerMemberName]) on this IJSObjectWrapper's JSObject.</para>
    /// <para>wrapper.SetJSProperty(value)</para>
    /// </summary>
    /// <param name="wrapper">The wrapper whose JSObject to set the property on.</param>
    /// <param name="value">Value to set. IJSObjectWrapper instances are unwrapped to their JSObject.</param>
    /// <param name="propertyName">Name of the property on this wrapper's <c>JSObject</c> to set.</param>
    [OverloadResolutionPriority(10)]
    public static void SetJSProperty(this IJSObjectWrapper wrapper, object value, [CallerMemberName] string? propertyName = null)
    {
        var unwrappedValue = value as IJSObjectWrapper;
        JSImportInstanceHelpers.SetProperty(wrapper.JSObject, propertyName!, unwrappedValue?.JSObject ?? value);
    }

    /// <summary>
    /// <para>Set the value of <c>propertyName</c> on this IJSObjectWrapper's JSObject.</para>
    /// <para>wrapper.SetJSProperty(propertyName: "someProperty", "value")</para>
    /// </summary>
    /// <param name="wrapper">The wrapper whose JSObject to set the property on.</param>
    /// <param name="propertyName">Name of the property on this wrapper's <c>JSObject</c> to set.</param>
    /// <param name="value">Value to set. IJSObjectWrapper instances are unwrapped to their JSObject.</param>
    public static void SetJSProperty(this IJSObjectWrapper wrapper, string propertyName, object value)
    {
        var unwrappedValue = value as IJSObjectWrapper;
        JSImportInstanceHelpers.SetProperty(wrapper.JSObject, propertyName, unwrappedValue?.JSObject ?? value);

    }
}


public static class JSObjectCallJSReturnTypeExtensions
{
    #region Overloads of inferred name CallJS<J> for 0 thru 5 parameters

    /// <summary>
    /// <para>Call JS <c>funcName</c>(inferred via [CallerMemberName]) on this JSObject.</para>
    /// <para>var someWrapper = jsObject.CallJS&lt;SomeWrapper&gt;()</para>
    /// </summary>
    /// <typeparam name="J">Type to return. Typically a JSObject, IJSObjectWrapper&lt;J&gt;, or type compatible with JSImport type marshalling.</typeparam>
    /// <param name="jsObject">Reference to the JS instance to invoke funcName on.</param>
    /// <param name="funcName">Name of the JS function on this <c>jsObject</c> to call.</param>
    /// <returns>The JS call's return, casted or wrapped to requested type <c>J</c>.</returns>
    public static J CallJS<J>(this JSObject jsObject, Breaker _ = default, [CallerMemberName] string? funcName = null)
        => JSImportInstanceHelpers.CallJSFunc<J>(jsObject, funcName!);

    /// <summary>
    /// <para>Call JS <c>funcName</c>(inferred via [CallerMemberName]) on this JSObject.</para>
    /// <para>var someWrapper = jsObject.CallJS&lt;SomeWrapper&gt;(...)</para>
    /// </summary>
    /// <typeparam name="J">Type to return. Typically a JSObject, IJSObjectWrapper&lt;J&gt;, or type compatible with JSImport type marshalling.</typeparam>
    /// <param name="jsObject">Reference to the JS instance to invoke funcName on.</param>
    /// <param name="param1">A parameter to pass to the JS function.  Must be JSObject, IJSObjectWrapper, or type compatible with JS interop.</param>
    /// <param name="funcName">Name of the JS function on this <c>jsObject</c> to call.</param>
    /// <returns>The JS call's return, casted or wrapped to requested type <c>J</c>.</returns>
    [OverloadResolutionPriority(1)]
    public static J CallJS<J>(this JSObject jsObject, object param1, Breaker _ = default, [CallerMemberName] string? funcName = null)
        => JSImportInstanceHelpers.CallJSFunc<J>(jsObject, funcName!, param1);

    /// <summary>
    /// <para>Call JS <c>funcName</c>(inferred via [CallerMemberName]) on this JSObject.</para>
    /// <para>var someWrapper = jsObject.CallJS&lt;SomeWrapper&gt;(...)</para>
    /// </summary>
    /// <typeparam name="J">Type to return. Typically a JSObject, IJSObjectWrapper&lt;J&gt;, or type compatible with JSImport type marshalling.</typeparam>
    /// <param name="jsObject">Reference to the JS instance to invoke funcName on.</param>
    /// <param name="param1">First parameter to pass to the JS function.  Must be JSObject, IJSObjectWrapper, or type compatible with JS interop.</param>
    /// <param name="param2">Second parameter to pass to the JS function.  Must be JSObject, IJSObjectWrapper, or type compatible with JS interop.</param>
    /// <param name="funcName">Name of the JS function on this <c>jsObject</c> to call.</param>
    /// <returns>The JS call's return, casted or wrapped to requested type <c>J</c>.</returns>
    [OverloadResolutionPriority(1)]
    public static J CallJS<J>(this JSObject jsObject, object param1, object param2, Breaker _ = default, [CallerMemberName] string? funcName = null)
        => JSImportInstanceHelpers.CallJSFunc<J>(jsObject, funcName!, param1, param2);

    /// <summary>
    /// <para>Call JS <c>funcName</c>(inferred via [CallerMemberName]) on this JSObject.</para>
    /// <para>var someWrapper = jsObject.CallJS&lt;SomeWrapper&gt;(...)</para>
    /// </summary>
    /// <typeparam name="J">Type to return. Typically a JSObject, IJSObjectWrapper&lt;J&gt;, or type compatible with JSImport type marshalling.</typeparam>
    /// <param name="jsObject">Reference to the JS instance to invoke funcName on.</param>
    /// <param name="param1">First parameter to pass to the JS function.  Must be JSObject, IJSObjectWrapper, or type compatible with JS interop.</param>
    /// <param name="param2">Second parameter to pass to the JS function.  Must be JSObject, IJSObjectWrapper, or type compatible with JS interop.</param>
    /// <param name="param3">Third parameter to pass to the JS function.  Must be JSObject, IJSObjectWrapper, or type compatible with JS interop.</param>
    /// <param name="funcName">Name of the JS function on this <c>jsObject</c> to call.</param>
    /// <returns>The JS call's return, casted or wrapped to requested type <c>J</c>.</returns>
    [OverloadResolutionPriority(1)]
    public static J CallJS<J>(this JSObject jsObject, object param1, object param2, object param3, Breaker _ = default, [CallerMemberName] string? funcName = null)
        => JSImportInstanceHelpers.CallJSFunc<J>(jsObject, funcName!, param1, param2, param3);

    /// <summary>
    /// <para>Call JS <c>funcName</c>(inferred via [CallerMemberName]) on this JSObject.</para>
    /// <para>var someWrapper = jsObject.CallJS&lt;SomeWrapper&gt;(...)</para>
    /// </summary>
    /// <typeparam name="J">Type to return. Typically a JSObject, IJSObjectWrapper&lt;J&gt;, or type compatible with JSImport type marshalling.</typeparam>
    /// <param name="jsObject">Reference to the JS instance to invoke funcName on.</param>
    /// <param name="param1">First parameter to pass to the JS function.  Must be JSObject, IJSObjectWrapper, or type compatible with JS interop.</param>
    /// <param name="param2">Second parameter to pass to the JS function.  Must be JSObject, IJSObjectWrapper, or type compatible with JS interop.</param>
    /// <param name="param3">Third parameter to pass to the JS function.  Must be JSObject, IJSObjectWrapper, or type compatible with JS interop.</param>
    /// <param name="param4">Fourth parameter to pass to the JS function.  Must be JSObject, IJSObjectWrapper, or type compatible with JS interop.</param>
    /// <param name="funcName">Name of the JS function on this <c>jsObject</c> to call.</param>
    /// <returns>The JS call's return, casted or wrapped to requested type <c>J</c>.</returns>
    [OverloadResolutionPriority(1)]
    public static J CallJS<J>(this JSObject jsObject, object param1, object param2, object param3, object param4, Breaker _ = default, [CallerMemberName] string? funcName = null)
        => JSImportInstanceHelpers.CallJSFunc<J>(jsObject, funcName!, param1, param2, param3, param4);

    /// <summary>
    /// <para>Call JS <c>funcName</c>(inferred via [CallerMemberName]) on this JSObject.</para>
    /// <para>var someWrapper = jsObject.CallJS&lt;SomeWrapper&gt;(...)</para>
    /// </summary>
    /// <typeparam name="J">Type to return. Typically a JSObject, IJSObjectWrapper&lt;J&gt;, or type compatible with JSImport type marshalling.</typeparam>
    /// <param name="jsObject">Reference to the JS instance to invoke funcName on.</param>
    /// <param name="param1">First parameter to pass to the JS function.  Must be JSObject, IJSObjectWrapper, or type compatible with JS interop.</param>
    /// <param name="param2">Second parameter to pass to the JS function.  Must be JSObject, IJSObjectWrapper, or type compatible with JS interop.</param>
    /// <param name="param3">Third parameter to pass to the JS function.  Must be JSObject, IJSObjectWrapper, or type compatible with JS interop.</param>
    /// <param name="param4">Fourth parameter to pass to the JS function.  Must be JSObject, IJSObjectWrapper, or type compatible with JS interop.</param>
    /// <param name="param5">Fifth parameter to pass to the JS function.  Must be JSObject, IJSObjectWrapper, or type compatible with JS interop.</param>
    /// <param name="funcName">Name of the JS function on this <c>jsObject</c> to call.</param>
    /// <returns>The JS call's return, casted or wrapped to requested type <c>J</c>.</returns>
    [OverloadResolutionPriority(1)]
    public static J CallJS<J>(this JSObject jsObject, object param1, object param2, object param3, object param4, object param5, Breaker _ = default, [CallerMemberName] string? funcName = null)
        => JSImportInstanceHelpers.CallJSFunc<J>(jsObject, funcName!, param1, param2, param3, param4, param5);

    #endregion


    #region Explicit funcName with variable Params, should be called with CallJS(funcName: "someFunc", ...) to force this overload

    // var someWrapper = this.CallJS<SomeWrapper>("someJSFunc", SerratedJS.Params(...));
    /// <summary>
    /// <para>Call JS <c>funcName</c> on this JSObject with the given JSParams.</para>
    /// <para>var someWrapper = this.CallJS&lt;SomeWrapper&gt;("someJSFunc", SerratedJS.Params(...))</para>
    /// </summary>
    /// <typeparam name="J">Type to return. Typically a JSObject, IJSObjectWrapper&lt;J&gt;, or type compatible with JSImport type marshalling.</typeparam>
    /// <param name="jsObject">Reference to the JS instance to invoke funcName on.</param>
    /// <param name="funcName">Name of the JS function on this <c>jsObject</c> to call.</param>
    /// <param name="parameters">SerratedJS.JSParams bundle to pass to the JS function.</param>
    /// <returns>The JS call's return, casted or wrapped to requested type <c>J</c>.</returns>
    public static J CallJS<J>(this JSObject jsObject, string funcName, JSParams parameters)
        => JSImportInstanceHelpers.CallJSFunc<J>(jsObject, funcName, parameters.Args);

    // var someWrapper = this.CallJS<SomeWrapper>("someJSFunc", "param1", 25, someJSObject, someJSWrapper);
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

    // CONSIDER: Overloads for string and int arrays, and any other array types that get unintentionally multiplexed by `params`. This may already be solved by Serrated.JSParams.ArrayParam

    #endregion

    // var someWrapper = this.CallJS<SomeWrapper>(SerratedJS.Params(...))
    /// <summary>
    /// <para>Call JS <c>funcName</c>(inferred via [CallerMemberName]) on this JSObject.</para>
    /// <para>var someWrapper = this.CallJS&lt;SomeWrapper&gt;(SerratedJS.Params(...))</para>
    /// </summary>
    /// <typeparam name="J">Type to return. Typically a JSObject, IJSObjectWrapper&lt;J&gt;, or type compatible with JSImport type marshalling.</typeparam>
    /// <param name="jsObject">Reference to the JS instance to invoke funcName on.</param>
    /// <param name="parameters">SerratedJS.JSParams bundle to pass to the JS function.</param>
    /// <param name="funcName">Name of the JS function on this <c>jsObject</c> to call.</param>
    /// <returns>The JS call's return, casted or wrapped to requested type <c>J</c>.</returns>
    [OverloadResolutionPriority(20)]
    public static J CallJS<J>(this JSObject jsObject, JSParams parameters, [CallerMemberName] string? funcName = null)
        => JSImportInstanceHelpers.CallJSFunc<J>(jsObject, funcName!, parameters.Args);

}

//public static class JSObjectMarshalExtensions
//{
//    /// <summary>
//    /// Marshal a native JavaScript array of objects referenced by this <see cref="JSObject"/>
//    /// into a <see cref="JSObject"/> array.
//    /// </summary>
//    /// <param name="jsArray">A <see cref="JSObject"/> pointing at a native JS array.</param>
//    /// <returns>An array of <see cref="JSObject"/> references for each element in the underlying JS array.</returns>
//    public static JSObject[] MarshalAsArrayOfObjects(this JSObject jsArray)
//        => HelpersJS.MarshalAsArrayOfObjects(jsArray);
//}

public static class JSObjectCallJSReturnVoidExtensions
{
    #region Overloads of inferred name CallJS<VOID> for 0 thru 5 parameters

    /// <summary>
    /// <para>Call JS <c>funcName</c>(inferred via [CallerMemberName]) on this JSObject; no return value.</para>
    /// <para>jsObject.CallJS()</para>
    /// </summary>
    /// <param name="jsObject">Reference to the JS instance to invoke funcName on.</param>
    /// <param name="funcName">Name of the JS function on this <c>jsObject</c> to call.</param>
    public static void CallJS(this JSObject jsObject, Breaker _ = default, [CallerMemberName] string? funcName = null)
        => JSImportInstanceHelpers.CallJSFuncVoid(jsObject, funcName!);

    /// <summary>
    /// <para>Call JS <c>funcName</c>(inferred via [CallerMemberName]) on this JSObject; no return value.</para>
    /// <para>jsObject.CallJS(param1)</para>
    /// </summary>
    /// <param name="jsObject">Reference to the JS instance to invoke funcName on.</param>
    /// <param name="param1">A parameter to pass to the JS function.  Must be JSObject, IJSObjectWrapper, or type compatible with JS interop.</param>
    /// <param name="funcName">Name of the JS function on this <c>jsObject</c> to call.</param>
    [OverloadResolutionPriority(1)]
    public static void CallJS(this JSObject jsObject, object param1, Breaker _ = default, [CallerMemberName] string? funcName = null)
        => JSImportInstanceHelpers.CallJSFuncVoid(jsObject, funcName!, param1);

    /// <summary>
    /// <para>Call JS <c>funcName</c>(inferred via [CallerMemberName]) on this JSObject; no return value.</para>
    /// <para>jsObject.CallJS(param1, param2)</para>
    /// </summary>
    /// <param name="jsObject">Reference to the JS instance to invoke funcName on.</param>
    /// <param name="param1">First parameter to pass to the JS function.  Must be JSObject, IJSObjectWrapper, or type compatible with JS interop.</param>
    /// <param name="param2">Second parameter to pass to the JS function.  Must be JSObject, IJSObjectWrapper, or type compatible with JS interop.</param>
    /// <param name="funcName">Name of the JS function on this <c>jsObject</c> to call.</param>
    [OverloadResolutionPriority(1)]
    public static void CallJS(this JSObject jsObject, object param1, object param2, Breaker _ = default, [CallerMemberName] string? funcName = null)
        => JSImportInstanceHelpers.CallJSFuncVoid(jsObject, funcName!, param1, param2);

    /// <summary>
    /// <para>Call JS <c>funcName</c>(inferred via [CallerMemberName]) on this JSObject; no return value.</para>
    /// <para>jsObject.CallJS(param1, param2, param3)</para>
    /// </summary>
    /// <param name="jsObject">Reference to the JS instance to invoke funcName on.</param>
    /// <param name="param1">First parameter to pass to the JS function.  Must be JSObject, IJSObjectWrapper, or type compatible with JS interop.</param>
    /// <param name="param2">Second parameter to pass to the JS function.  Must be JSObject, IJSObjectWrapper, or type compatible with JS interop.</param>
    /// <param name="param3">Third parameter to pass to the JS function.  Must be JSObject, IJSObjectWrapper, or type compatible with JS interop.</param>
    /// <param name="funcName">Name of the JS function on this <c>jsObject</c> to call.</param>
    [OverloadResolutionPriority(1)]
    public static void CallJS(this JSObject jsObject, object param1, object param2, object param3, Breaker _ = default, [CallerMemberName] string? funcName = null)
        => JSImportInstanceHelpers.CallJSFuncVoid(jsObject, funcName!, param1, param2, param3);

    /// <summary>
    /// <para>Call JS <c>funcName</c>(inferred via [CallerMemberName]) on this JSObject; no return value.</para>
    /// <para>jsObject.CallJS(param1, param2, param3, param4)</para>
    /// </summary>
    /// <param name="jsObject">Reference to the JS instance to invoke funcName on.</param>
    /// <param name="param1">First parameter to pass to the JS function.  Must be JSObject, IJSObjectWrapper, or type compatible with JS interop.</param>
    /// <param name="param2">Second parameter to pass to the JS function.  Must be JSObject, IJSObjectWrapper, or type compatible with JS interop.</param>
    /// <param name="param3">Third parameter to pass to the JS function.  Must be JSObject, IJSObjectWrapper, or type compatible with JS interop.</param>
    /// <param name="param4">Fourth parameter to pass to the JS function.  Must be JSObject, IJSObjectWrapper, or type compatible with JS interop.</param>
    /// <param name="funcName">Name of the JS function on this <c>jsObject</c> to call.</param>
    [OverloadResolutionPriority(1)]
    public static void CallJS(this JSObject jsObject, object param1, object param2, object param3, object param4, Breaker _ = default, [CallerMemberName] string? funcName = null)
        => JSImportInstanceHelpers.CallJSFuncVoid(jsObject, funcName!, param1, param2, param3, param4);

    /// <summary>
    /// <para>Call JS <c>funcName</c>(inferred via [CallerMemberName]) on this JSObject; no return value.</para>
    /// <para>jsObject.CallJS(param1, param2, param3, param4, param5)</para>
    /// </summary>
    /// <param name="jsObject">Reference to the JS instance to invoke funcName on.</param>
    /// <param name="param1">First parameter to pass to the JS function.  Must be JSObject, IJSObjectWrapper, or type compatible with JS interop.</param>
    /// <param name="param2">Second parameter to pass to the JS function.  Must be JSObject, IJSObjectWrapper, or type compatible with JS interop.</param>
    /// <param name="param3">Third parameter to pass to the JS function.  Must be JSObject, IJSObjectWrapper, or type compatible with JS interop.</param>
    /// <param name="param4">Fourth parameter to pass to the JS function.  Must be JSObject, IJSObjectWrapper, or type compatible with JS interop.</param>
    /// <param name="param5">Fifth parameter to pass to the JS function.  Must be JSObject, IJSObjectWrapper, or type compatible with JS interop.</param>
    /// <param name="funcName">Name of the JS function on this <c>jsObject</c> to call.</param>
    [OverloadResolutionPriority(1)]
    public static void CallJS(this JSObject jsObject, object param1, object param2, object param3, object param4, object param5, Breaker _ = default, [CallerMemberName] string? funcName = null)
        => JSImportInstanceHelpers.CallJSFuncVoid(jsObject, funcName!, param1, param2, param3, param4, param5);

    #endregion


    #region Explicit funcName with variable Params, should be called with CallJS(funcName: "someFunc", ...) to force this overload

    // this.CallJS("someJSFunc", SerratedJS.Params(...));
    /// <summary>
    /// <para>Call JS <c>funcName</c> on this JSObject with the given JSParams; no return value.</para>
    /// <para>this.CallJS("someJSFunc", SerratedJS.Params(...))</para>
    /// </summary>
    /// <param name="jsObject">Reference to the JS instance to invoke funcName on.</param>
    /// <param name="funcName">Name of the JS function on this <c>jsObject</c> to call.</param>
    /// <param name="parameters">SerratedJS.JSParams bundle to pass to the JS function.</param>
    [OverloadResolutionPriority(20)]
    public static void CallJS(this JSObject jsObject, string funcName, JSParams parameters)
        => JSImportInstanceHelpers.CallJSFuncVoid(jsObject, funcName, parameters.Args);

    // this.CallJS("someJSFunc", "param1", 25, someJSObject, someJSWrapper);
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

    #endregion

    // this.CallJS(SerratedJS.Params(...));
    /// <summary>
    /// <para>Call JS <c>funcName</c>(inferred via [CallerMemberName]) on this JSObject; no return value.</para>
    /// <para>this.CallJS(SerratedJS.Params(...))</para>
    /// </summary>
    /// <param name="jsObject">Reference to the JS instance to invoke funcName on.</param>
    /// <param name="parameters">SerratedJS.JSParams bundle to pass to the JS function.</param>
    /// <param name="funcName">Name of the JS function on this <c>jsObject</c> to call.</param>
    [OverloadResolutionPriority(20)]
    public static void CallJS(this JSObject jsObject, JSParams parameters, [CallerMemberName] string? funcName = null)
        => JSImportInstanceHelpers.CallJSFuncVoid(jsObject, funcName!, parameters.Args);

}


public static class JSObjectPropertyExtensions
{
    // this.GetJSProperty("someProperty");
    /// <summary>
    /// <para>Get the value of <c>propertyName</c>(inferred via [CallerMemberName]) on this JSObject.</para>
    /// <para>this.GetJSProperty("someProperty")</para>
    /// </summary>
    /// <typeparam name="J">Type of the property value. Typically a JSObject, IJSObjectWrapper&lt;J&gt;, or type compatible with JSImport type marshalling.</typeparam>
    /// <param name="jsObject">Reference to the JS instance to read the property from.</param>
    /// <param name="propertyName">Name of the property on this <c>jsObject</c> to get.  Omit to infer from [CallerMemberName].</param>
    /// <returns>The property value, casted or wrapped to requested type <c>J</c>.</returns>
    public static J GetJSProperty<J>(this JSObject jsObject, [CallerMemberName] string? propertyName = null)
        => JSImportInstanceHelpers.GetProperty<J>(jsObject, propertyName!);

    // this.SetJSProperty("someProperty", "value");
    /// <summary>
    /// <para>Set the value of <c>propertyName</c>(inferred via [CallerMemberName]) on this JSObject.</para>
    /// <para>this.SetJSProperty("value")</para>
    /// </summary>
    /// <param name="jsObject">Reference to the JS instance to set the property on.</param>
    /// <param name="value">Value to set.</param>
    /// <param name="propertyName">Name of the property on this <c>jsObject</c> to set.  Omit to infer from [CallerMemberName].</param>
    [OverloadResolutionPriority(10)]
    public static void SetJSProperty(this JSObject jsObject, object value, [CallerMemberName] string? propertyName = null)
    {
        var unwrappedValue = value as IJSObjectWrapper;
        JSImportInstanceHelpers.SetProperty(jsObject, propertyName!, unwrappedValue?.JSObject ?? value);
    }


    /// <summary>
    /// <para>Set the value of <c>propertyName</c> on this JSObject with an explicit propertyName.</para>
    /// <para>this.SetJSProperty(propertyName: "someProperty", "value")</para>
    /// </summary>
    /// <param name="jsObject">Reference to the JS instance to set the property on.</param>
    /// <param name="propertyName">Name of the property on this <c>jsObject</c> to set.</param>
    /// <param name="value">Value to set.</param>
    public static void SetJSProperty(this JSObject jsObject, string propertyName, object value)
    {
        var unwrappedValue = value as IJSObjectWrapper;
        JSImportInstanceHelpers.SetProperty(jsObject, propertyName, unwrappedValue?.JSObject ?? value);
    }

}


