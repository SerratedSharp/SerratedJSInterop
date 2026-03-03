using SerratedSharp.SerratedJSInterop.Internal;
using System.Runtime.CompilerServices;

namespace SerratedSharp.SerratedJSInterop;

public static class ExtensionsWrapperCallJSReturnVoid
{

    #region Overloads of inferred name CallJS<VOID> for 0 thru 5 parameters

    /// <summary>
    /// <para>Call JS <c>funcName</c>(inferred via [CallerMemberName]) on this IJSObjectWrapper's JSObject; no return value.</para>
    /// <para>wrapper.CallJS()</para>
    /// </summary>
    /// <param name="wrapper">The wrapper whose JSObject to invoke funcName on.</param>
    /// <param name="funcName">Name of the JS function on this wrapper's <c>JSObject</c> to call, with first letter lower cased for JS casing conventions.</param>
    public static void CallJS(this IJSObjectWrapper wrapper, Breaker _ = default, [CallerMemberName] string? funcName = null)
        => JSImportInstanceHelpers.CallJSFuncVoid(wrapper.JSObject, funcName!);

    /// <summary>
    /// <para>Call JS <c>funcName</c>(inferred via [CallerMemberName]) on this IJSObjectWrapper's JSObject; no return value.</para>
    /// <para>wrapper.CallJS(param1)</para>
    /// </summary>
    /// <param name="wrapper">The wrapper whose JSObject to invoke funcName on.</param>
    /// <param name="param1">A parameter to pass to the JS function.  Must be JSObject, IJSObjectWrapper, or type compatible with JS interop.</param>
    /// <param name="funcName">Name of the JS function on this wrapper's <c>JSObject</c> to call, with first letter lower cased for JS casing conventions.</param>
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
    /// <param name="funcName">Name of the JS function on this wrapper's <c>JSObject</c> to call, with first letter lower cased for JS casing conventions.</param>
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
    /// <param name="funcName">Name of the JS function on this wrapper's <c>JSObject</c> to call, with first letter lower cased for JS casing conventions.</param>
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
    /// <param name="funcName">Name of the JS function on this wrapper's <c>JSObject</c> to call, with first letter lower cased for JS casing conventions.</param>
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
    /// <param name="funcName">Name of the JS function on this wrapper's <c>JSObject</c> to call, with first letter lower cased for JS casing conventions.</param>
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
    /// <param name="funcName">Name of the JS function on this wrapper's <c>JSObject</c> to call, with casing preserved.</param>
    /// <param name="parameters">SerratedJS.JSParams bundle to pass to the JS function.</param>
    [OverloadResolutionPriority(20)]
    public static void CallJS(this IJSObjectWrapper wrapper, string funcName, JSParams parameters)
        => JSImportInstanceHelpers.CallJSFuncVoidExplicitName(wrapper.JSObject, funcName, parameters.Args);

    /// <summary>
    /// <para>Call JS <c>funcName</c> on this IJSObjectWrapper's JSObject with the given arguments; no return value.</para>
    /// <para>wrapper.CallJS("someJSFunc", "param1", 25, someJSObject, someJSWrapper)</para>
    /// <para>Note: Use the named parameter syntax <c>funcName:</c> to disambiguate from the inferred-name overload when passing a single string argument:
    /// <c>wrapper.CallJS(funcName: "someJSFunc")</c></para>
    /// </summary>
    /// <param name="wrapper">The wrapper whose JSObject to invoke funcName on.</param>
    /// <param name="funcName">Name of the JS function on this wrapper's <c>JSObject</c> to call, with casing preserved.</param>
    /// <param name="parameters">Arguments to pass to the JS function.</param>
    public static void CallJS(this IJSObjectWrapper wrapper, string funcName, params object[] parameters)
        => JSImportInstanceHelpers.CallJSFuncVoidExplicitName(wrapper.JSObject, funcName, parameters);

    // CONSIDER: Overloads for string and int arrays, and any other array types that get unintentionally multiplexed by `params`. This may already be solved by Serrated.JSParams.ArrayParam

    #endregion

    /// <summary>
    /// <para>Call JS <c>funcName</c>(inferred via [CallerMemberName]) on this IJSObjectWrapper's JSObject; no return value.</para>
    /// <para>wrapper.CallJS(SerratedJS.Params(...))</para>
    /// </summary>
    /// <param name="wrapper">The wrapper whose JSObject to invoke funcName on.</param>
    /// <param name="parameters">SerratedJS.JSParams bundle to pass to the JS function.</param>
    /// <param name="funcName">Name of the JS function on this wrapper's <c>JSObject</c> to call, with first letter lower cased for JS casing conventions.</param>
    [OverloadResolutionPriority(20)]
    public static void CallJS(this IJSObjectWrapper wrapper, JSParams parameters, Breaker _ = default, [CallerMemberName] string? funcName = null)
        => JSImportInstanceHelpers.CallJSFuncVoid(wrapper.JSObject, funcName!, parameters.Args);

}


