using SerratedSharp.SerratedJSInterop.Internal;
using System.Runtime.CompilerServices;

namespace SerratedSharp.SerratedJSInterop;

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
    /// <param name="funcName">Name of the JS function on this wrapper's <c>JSObject</c> to call, with first letter lower cased for JS casing conventions.</param>
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
    /// <param name="funcName">Name of the JS function on this wrapper's <c>JSObject</c> to call, with first letter lower cased for JS casing conventions.</param>
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
    /// <param name="funcName">Name of the JS function on this wrapper's <c>JSObject</c> to call, with first letter lower cased for JS casing conventions.</param>
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
    /// <param name="funcName">Name of the JS function on this wrapper's <c>JSObject</c> to call, with first letter lower cased for JS casing conventions.</param>
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
    /// <param name="funcName">Name of the JS function on this wrapper's <c>JSObject</c> to call, with first letter lower cased for JS casing conventions.</param>
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
    /// <param name="funcName">Name of the JS function on this wrapper's <c>JSObject</c> to call, with first letter lower cased for JS casing conventions.</param>
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
    /// <param name="funcName">Name of the JS function on this wrapper's <c>JSObject</c> to call, with casing preserved.</param>
    /// <param name="parameters">SerratedJS.JSParams bundle to pass to the JS function.</param>
    /// <returns>The JS call's return, casted or wrapped to requested type <c>J</c>.</returns>
    [OverloadResolutionPriority(20)]
    public static J CallJS<J>(this IJSObjectWrapper wrapper, string funcName, JSParams parameters)
        => JSImportInstanceHelpers.CallJSFuncExplicitName<J>(wrapper.JSObject, funcName, parameters.Args);

    /// <summary>
    /// <para>Call JS <c>funcName</c> on this IJSObjectWrapper's JSObject with the given arguments.</para>
    /// <para>var someWrapper = wrapper.CallJS&lt;SomeWrapper&gt;("someJSFunc", "param1", 25, someJSObject, someJSWrapper)</para>
    /// <para>Note: Use the named parameter syntax <c>funcName:</c> to disambiguate from the inferred-name overload when passing a single string argument:
    /// <c>wrapper.CallJS&lt;J&gt;(funcName: "someJSFunc")</c></para>
    /// </summary>
    /// <typeparam name="J">Type to return. Typically a JSObject, IJSObjectWrapper&lt;J&gt;, or type compatible with JSImport type marshalling.</typeparam>
    /// <param name="wrapper">The wrapper whose JSObject to invoke funcName on.</param>
    /// <param name="funcName">Name of the JS function on this wrapper's <c>JSObject</c> to call, with casing preserved.</param>
    /// <param name="parameters">Arguments to pass to the JS function.</param>
    /// <returns>The JS call's return, casted or wrapped to requested type <c>J</c>.</returns>
    public static J CallJS<J>(this IJSObjectWrapper wrapper, string funcName, params object[] parameters)
        => JSImportInstanceHelpers.CallJSFuncExplicitName<J>(wrapper.JSObject, funcName, parameters);

    // CONSIDER: Overloads for string and int arrays, and any other array types that get unintentionally multiplexed by `params`. This may already be solved by Serrated.JSParams.ArrayParam

    #endregion

    /// <summary>
    /// <para>Call JS <c>funcName</c>(inferred via [CallerMemberName]) on this IJSObjectWrapper's JSObject.</para>
    /// <para>var someWrapper = wrapper.CallJS&lt;SomeWrapper&gt;(SerratedJS.Params(...))</para>
    /// </summary>
    /// <typeparam name="J">Type to return. Typically a JSObject, IJSObjectWrapper&lt;J&gt;, or type compatible with JSImport type marshalling.</typeparam>
    /// <param name="wrapper">The wrapper whose JSObject to invoke funcName on.</param>
    /// <param name="parameters">SerratedJS.JSParams bundle to pass to the JS function.</param>
    /// <param name="funcName">Name of the JS function on this wrapper's <c>JSObject</c> to call, with first letter lower cased for JS casing conventions.</param>
    /// <returns>The JS call's return, casted or wrapped to requested type <c>J</c>.</returns>
    [OverloadResolutionPriority(20)]
    public static J CallJS<J>(this IJSObjectWrapper wrapper, JSParams parameters, Breaker _ = default, [CallerMemberName] string? funcName = null)
        => JSImportInstanceHelpers.CallJSFunc<J>(wrapper.JSObject, funcName!, parameters.Args);

}


