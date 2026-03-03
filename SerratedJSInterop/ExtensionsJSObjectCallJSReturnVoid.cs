using SerratedSharp.SerratedJSInterop.Internal;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.JavaScript;

namespace SerratedSharp.SerratedJSInterop;

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

public static class ExtensionsJSObjectCallJSReturnVoid
{
    #region Overloads of inferred name CallJS<VOID> for 0 thru 5 parameters

    /// <summary>
    /// <para>Call JS <c>funcName</c>(inferred via [CallerMemberName]) on this JSObject; no return value.</para>
    /// <para>jsObject.CallJS()</para>
    /// </summary>
    /// <param name="jsObject">Reference to the JS instance to invoke funcName on.</param>
    /// <param name="funcName">Name of the JS function on this <c>jsObject</c> to call, with first letter lower cased for JS casing conventions.</param>
    public static void CallJS(this JSObject jsObject, Breaker _ = default, [CallerMemberName] string? funcName = null)
        => JSImportInstanceHelpers.CallJSFuncVoid(jsObject, funcName!);

    /// <summary>
    /// <para>Call JS <c>funcName</c>(inferred via [CallerMemberName]) on this JSObject; no return value.</para>
    /// <para>jsObject.CallJS(param1)</para>
    /// </summary>
    /// <param name="jsObject">Reference to the JS instance to invoke funcName on.</param>
    /// <param name="param1">A parameter to pass to the JS function.  Must be JSObject, IJSObjectWrapper, or type compatible with JS interop.</param>
    /// <param name="funcName">Name of the JS function on this <c>jsObject</c> to call, with first letter lower cased for JS casing conventions.</param>
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
    /// <param name="funcName">Name of the JS function on this <c>jsObject</c> to call, with first letter lower cased for JS casing conventions.</param>
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
    /// <param name="funcName">Name of the JS function on this <c>jsObject</c> to call, with first letter lower cased for JS casing conventions.</param>
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
    /// <param name="funcName">Name of the JS function on this <c>jsObject</c> to call, with first letter lower cased for JS casing conventions.</param>
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
    /// <param name="funcName">Name of the JS function on this <c>jsObject</c> to call, with first letter lower cased for JS casing conventions.</param>
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
    /// <param name="funcName">Name of the JS function on this <c>jsObject</c> to call, with casing preserved.</param>
    /// <param name="parameters">SerratedJS.JSParams bundle to pass to the JS function.</param>
    [OverloadResolutionPriority(20)]
    public static void CallJS(this JSObject jsObject, string funcName, JSParams parameters)
        => JSImportInstanceHelpers.CallJSFuncVoidExplicitName(jsObject, funcName, parameters.Args);

    // this.CallJS("someJSFunc", "param1", 25, someJSObject, someJSWrapper);
    /// <summary>
    /// <para>Call JS <c>funcName</c> on this JSObject with the given arguments; no return value.</para>
    /// <para>this.CallJS("someJSFunc", "param1", 25, someJSObject, someJSWrapper)</para>
    /// </summary>
    /// <param name="jsObject">Reference to the JS instance to invoke funcName on.</param>
    /// <param name="funcName">Name of the JS function on this <c>jsObject</c> to call, with casing preserved.</param>
    /// <param name="parameters">Arguments to pass to the JS function.</param>
    public static void CallJS(this JSObject jsObject, string funcName, params object[] parameters)
        => JSImportInstanceHelpers.CallJSFuncVoidExplicitName(jsObject, funcName, parameters);

    // CONSIDER: Overloads for string and int arrays, and any other array types that get unintentionally multiplexed by `params`. This may already be solved by Serrated.JSParams.ArrayParam

    #endregion

    // this.CallJS(SerratedJS.Params(...));
    /// <summary>
    /// <para>Call JS <c>funcName</c>(inferred via [CallerMemberName]) on this JSObject; no return value.</para>
    /// <para>this.CallJS(SerratedJS.Params(...))</para>
    /// </summary>
    /// <param name="jsObject">Reference to the JS instance to invoke funcName on.</param>
    /// <param name="parameters">SerratedJS.JSParams bundle to pass to the JS function.</param>
    /// <param name="funcName">Name of the JS function on this <c>jsObject</c> to call, with first letter lower cased for JS casing conventions.</param>
    [OverloadResolutionPriority(20)]
    public static void CallJS(this JSObject jsObject, JSParams parameters, [CallerMemberName] string? funcName = null)
        => JSImportInstanceHelpers.CallJSFuncVoid(jsObject, funcName!, parameters.Args);

}


