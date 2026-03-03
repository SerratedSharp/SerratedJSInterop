using System.Diagnostics.CodeAnalysis;
using SerratedSharp.SerratedJSInterop.Internal;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.JavaScript;

namespace SerratedSharp.SerratedJSInterop;

public static class ExtensionsJSObjectCallJSReturnType
{
    #region Overloads of inferred name CallJS<[DynamicallyAccessedMembers(JSImportInstanceHelpers.WrapperTypeMembers)] J> for 0 thru 5 parameters

    /// <summary>
    /// <para>Call JS <c>funcName</c>(inferred via [CallerMemberName]) on this JSObject.</para>
    /// <para>var someWrapper = jsObject.CallJS&lt;SomeWrapper&gt;()</para>
    /// </summary>
    /// <typeparam name="J">Type to return. Typically a JSObject, IJSObjectWrapper&lt;J&gt;, or type compatible with JSImport type marshalling.</typeparam>
    /// <param name="jsObject">Reference to the JS instance to invoke funcName on.</param>
    /// <param name="funcName">Name of the JS function on this <c>jsObject</c> to call, with first letter lower cased for JS casing conventions.</param>
    /// <returns>The JS call's return, casted or wrapped to requested type <c>J</c>.</returns>
    public static J CallJS<[DynamicallyAccessedMembers(JSImportInstanceHelpers.WrapperTypeMembers)] J>(this JSObject jsObject, Breaker _ = default, [CallerMemberName] string? funcName = null)
        => JSImportInstanceHelpers.CallJSFunc<J>(jsObject, funcName!);

    /// <summary>
    /// <para>Call JS <c>funcName</c>(inferred via [CallerMemberName]) on this JSObject.</para>
    /// <para>var someWrapper = jsObject.CallJS&lt;SomeWrapper&gt;(...)</para>
    /// </summary>
    /// <typeparam name="J">Type to return. Typically a JSObject, IJSObjectWrapper&lt;J&gt;, or type compatible with JSImport type marshalling.</typeparam>
    /// <param name="jsObject">Reference to the JS instance to invoke funcName on.</param>
    /// <param name="param1">A parameter to pass to the JS function.  Must be JSObject, IJSObjectWrapper, or type compatible with JS interop.</param>
    /// <param name="funcName">Name of the JS function on this <c>jsObject</c> to call, with first letter lower cased for JS casing conventions.</param>
    /// <returns>The JS call's return, casted or wrapped to requested type <c>J</c>.</returns>
    [OverloadResolutionPriority(1)]
    public static J CallJS<[DynamicallyAccessedMembers(JSImportInstanceHelpers.WrapperTypeMembers)] J>(this JSObject jsObject, object param1, Breaker _ = default, [CallerMemberName] string? funcName = null)
        => JSImportInstanceHelpers.CallJSFunc<J>(jsObject, funcName!, param1);

    /// <summary>
    /// <para>Call JS <c>funcName</c>(inferred via [CallerMemberName]) on this JSObject.</para>
    /// <para>var someWrapper = jsObject.CallJS&lt;SomeWrapper&gt;(...)</para>
    /// </summary>
    /// <typeparam name="J">Type to return. Typically a JSObject, IJSObjectWrapper&lt;J&gt;, or type compatible with JSImport type marshalling.</typeparam>
    /// <param name="jsObject">Reference to the JS instance to invoke funcName on.</param>
    /// <param name="param1">First parameter to pass to the JS function.  Must be JSObject, IJSObjectWrapper, or type compatible with JS interop.</param>
    /// <param name="param2">Second parameter to pass to the JS function.  Must be JSObject, IJSObjectWrapper, or type compatible with JS interop.</param>
    /// <param name="funcName">Name of the JS function on this <c>jsObject</c> to call, with first letter lower cased for JS casing conventions.</param>
    /// <returns>The JS call's return, casted or wrapped to requested type <c>J</c>.</returns>
    [OverloadResolutionPriority(1)]
    public static J CallJS<[DynamicallyAccessedMembers(JSImportInstanceHelpers.WrapperTypeMembers)] J>(this JSObject jsObject, object param1, object param2, Breaker _ = default, [CallerMemberName] string? funcName = null)
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
    /// <param name="funcName">Name of the JS function on this <c>jsObject</c> to call, with first letter lower cased for JS casing conventions.</param>
    /// <returns>The JS call's return, casted or wrapped to requested type <c>J</c>.</returns>
    [OverloadResolutionPriority(1)]
    public static J CallJS<[DynamicallyAccessedMembers(JSImportInstanceHelpers.WrapperTypeMembers)] J>(this JSObject jsObject, object param1, object param2, object param3, Breaker _ = default, [CallerMemberName] string? funcName = null)
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
    /// <param name="funcName">Name of the JS function on this <c>jsObject</c> to call, with first letter lower cased for JS casing conventions.</param>
    /// <returns>The JS call's return, casted or wrapped to requested type <c>J</c>.</returns>
    [OverloadResolutionPriority(1)]
    public static J CallJS<[DynamicallyAccessedMembers(JSImportInstanceHelpers.WrapperTypeMembers)] J>(this JSObject jsObject, object param1, object param2, object param3, object param4, Breaker _ = default, [CallerMemberName] string? funcName = null)
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
    /// <param name="funcName">Name of the JS function on this <c>jsObject</c> to call, with first letter lower cased for JS casing conventions.</param>
    /// <returns>The JS call's return, casted or wrapped to requested type <c>J</c>.</returns>
    [OverloadResolutionPriority(1)]
    public static J CallJS<[DynamicallyAccessedMembers(JSImportInstanceHelpers.WrapperTypeMembers)] J>(this JSObject jsObject, object param1, object param2, object param3, object param4, object param5, Breaker _ = default, [CallerMemberName] string? funcName = null)
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
    /// <param name="funcName">Name of the JS function on this <c>jsObject</c> to call, with casing preserved.</param>
    /// <param name="parameters">SerratedJS.JSParams bundle to pass to the JS function.</param>
    /// <returns>The JS call's return, casted or wrapped to requested type <c>J</c>.</returns>
    public static J CallJS<[DynamicallyAccessedMembers(JSImportInstanceHelpers.WrapperTypeMembers)] J>(this JSObject jsObject, string funcName, JSParams parameters)
        => JSImportInstanceHelpers.CallJSFuncExplicitName<J>(jsObject, funcName, parameters.Args);

    // var someWrapper = this.CallJS<SomeWrapper>("someJSFunc", "param1", 25, someJSObject, someJSWrapper);
    /// <summary>
    /// <para>Call JS <c>funcName</c> on this JSObject with the given arguments.</para>
    /// <para>var someWrapper = this.CallJS&lt;SomeWrapper&gt;("someJSFunc", "param1", 25, someJSObject, someJSWrapper)</para>
    /// </summary>
    /// <typeparam name="J">Type to return. Typically a JSObject, IJSObjectWrapper&lt;J&gt;, or type compatible with JSImport type marshalling.</typeparam>
    /// <param name="jsObject">Reference to the JS instance to invoke funcName on.</param>
    /// <param name="funcName">Name of the JS function on this <c>jsObject</c> to call, with casing preserved.</param>
    /// <param name="parameters">Arguments to pass to the JS function.</param>
    /// <returns>The JS call's return, casted or wrapped to requested type <c>J</c>.</returns>
    public static J CallJS<[DynamicallyAccessedMembers(JSImportInstanceHelpers.WrapperTypeMembers)] J>(this JSObject jsObject, string funcName, params object[] parameters)
        => JSImportInstanceHelpers.CallJSFuncExplicitName<J>(jsObject, funcName, parameters);

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
    /// <param name="funcName">Name of the JS function on this <c>jsObject</c> to call, with first letter lower cased for JS casing conventions.</param>
    /// <returns>The JS call's return, casted or wrapped to requested type <c>J</c>.</returns>
    [OverloadResolutionPriority(20)]
    public static J CallJS<[DynamicallyAccessedMembers(JSImportInstanceHelpers.WrapperTypeMembers)] J>(this JSObject jsObject, JSParams parameters, [CallerMemberName] string? funcName = null)
        => JSImportInstanceHelpers.CallJSFunc<J>(jsObject, funcName!, parameters.Args);

}


