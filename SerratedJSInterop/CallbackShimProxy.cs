using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices.JavaScript;

namespace SerratedSharp.SerratedJSInterop;

/// <summary>
/// Wrapper for the packed-arguments object passed to <see cref="Callback"/>.
/// Use <see cref="GetUnpacked"/> to get the individual JS callback arguments (primitives and/or <see cref="JSObject"/>) without knowing the packed shape.
/// </summary>
public readonly struct PackedParams : IJSObjectWrapper<PackedParams>
{
    /// <summary>The underlying packed JS object (typically <c>{ items: [...] }</c>).</summary>
    public JSObject JSObject { get; }

    /// <summary>Creates a wrapper around the packed object received from JS.</summary>
    public PackedParams(JSObject jsObject) => JSObject = jsObject;

    /// <inheritdoc />
    public static PackedParams WrapInstance(JSObject jsObject) => new PackedParams(jsObject);
    /// <summary>
    /// Returns the unpacked callback arguments from <c>items</c> via <see cref="HelpersJS.GetPackedCallbackItems"/> (<c>JSType.Array&lt;JSType.Any&gt;</c> for mixed primitives and objects).
    /// </summary>
    public object[] GetUnpacked() => HelpersJS.GetPackedCallbackItems(JSObject);
}

/// <summary>
/// Wraps an <see cref="Action{PackedParams}"/> so that when passed as a parameter to <see cref="CallJS"/> (or equivalent),
/// the callback is invoked with a <see cref="PackedParams"/> when JS passes multiple arguments; use <see cref="PackedParams.GetUnpacked"/> to get the args.
/// Example: <c>window.CallJS&lt;JSObject&gt;("setTimeout", Callback.CreatePacked(packed => { var args = packed.GetUnpacked(); }), 1000);</c>
/// </summary>
public readonly struct Callback
{
    /// <summary>The callback that receives <see cref="PackedParams"/> (use <see cref="PackedParams.GetUnpacked"/> to get the arguments).</summary>
    public Action<PackedParams> Action { get; }
    /// <summary>Creates a packed-params callback wrapper.</summary>
    public Callback(Action<PackedParams> action) => Action = action ?? throw new ArgumentNullException(nameof(action));
    /// <summary>Creates a packed-params callback wrapper. Use the generic overloads for type-safe access to individual arguments.</summary>
    public static Callback CreatePacked(Action<PackedParams> action) => new Callback(action);

    /// <summary>Creates a typed packed-params callback. The single JS callback argument is coerced to <typeparamref name="T1"/>.</summary>
    public static Callback Create<[DynamicallyAccessedMembers(JSImportInstanceHelpers.WrapperTypeMembers)] T1>(
        Action<T1> action) => new Callback(packed =>
    {
        var items = packed.GetUnpacked();
        if (items.Length < 1) throw new ArgumentException($"Expected at least 1 callback argument, got {items.Length}");
        action(JSImportInstanceHelpers.CastOrWrap<T1>(items[0]));
    });

    /// <summary>Creates a typed packed-params callback. The two JS callback arguments are coerced to <typeparamref name="T1"/> and <typeparamref name="T2"/>.</summary>
    public static Callback Create<[DynamicallyAccessedMembers(JSImportInstanceHelpers.WrapperTypeMembers)] T1, [DynamicallyAccessedMembers(JSImportInstanceHelpers.WrapperTypeMembers)] T2>(
        Action<T1, T2> action) => new Callback(packed =>
    {
        var items = packed.GetUnpacked();
        if (items.Length < 2) throw new ArgumentException($"Expected at least 2 callback arguments, got {items.Length}");
        action(
            JSImportInstanceHelpers.CastOrWrap<T1>(items[0]),
            JSImportInstanceHelpers.CastOrWrap<T2>(items[1]));
    });

    /// <summary>Creates a typed packed-params callback. The three JS callback arguments are coerced to <typeparamref name="T1"/>, <typeparamref name="T2"/>, and <typeparamref name="T3"/>.</summary>
    public static Callback Create<[DynamicallyAccessedMembers(JSImportInstanceHelpers.WrapperTypeMembers)] T1, [DynamicallyAccessedMembers(JSImportInstanceHelpers.WrapperTypeMembers)] T2, [DynamicallyAccessedMembers(JSImportInstanceHelpers.WrapperTypeMembers)] T3>(
        Action<T1, T2, T3> action) => new Callback(packed =>
    {
        var items = packed.GetUnpacked();
        if (items.Length < 3) throw new ArgumentException($"Expected at least 3 callback arguments, got {items.Length}");
        action(
            JSImportInstanceHelpers.CastOrWrap<T1>(items[0]),
            JSImportInstanceHelpers.CastOrWrap<T2>(items[1]),
            JSImportInstanceHelpers.CastOrWrap<T3>(items[2]));
    });

    /// <summary>Creates a typed packed-params callback. The four JS callback arguments are coerced to <typeparamref name="T1"/>, <typeparamref name="T2"/>, <typeparamref name="T3"/>, and <typeparamref name="T4"/>.</summary>
    public static Callback Create<[DynamicallyAccessedMembers(JSImportInstanceHelpers.WrapperTypeMembers)] T1, [DynamicallyAccessedMembers(JSImportInstanceHelpers.WrapperTypeMembers)] T2, [DynamicallyAccessedMembers(JSImportInstanceHelpers.WrapperTypeMembers)] T3, [DynamicallyAccessedMembers(JSImportInstanceHelpers.WrapperTypeMembers)] T4>(
        Action<T1, T2, T3, T4> action) => new Callback(packed =>
    {
        var items = packed.GetUnpacked();
        if (items.Length < 4) throw new ArgumentException($"Expected at least 4 callback arguments, got {items.Length}");
        action(
            JSImportInstanceHelpers.CastOrWrap<T1>(items[0]),
            JSImportInstanceHelpers.CastOrWrap<T2>(items[1]),
            JSImportInstanceHelpers.CastOrWrap<T3>(items[2]),
            JSImportInstanceHelpers.CastOrWrap<T4>(items[3]));
    });

    /// <summary>Returns the shim action (JSObject => invoke user action with new PackedParams(jsObject)). Used internally.</summary>
    internal Action<JSObject> ToShimAction()
    {
        var action = Action;
        return jsObj => action(new PackedParams(jsObj));
    }

    /// <summary>
    /// Wraps a no-arg callback and returns a <see cref="CallbackHandle"/>. Pass the handle to CallJS; retain for remove/unregister. Optional context is used for .bind(context) in JS.
    /// </summary>
    public static CallbackHandle MarshalAsHandle(Action action, JSObject? context = null)
    {
        if (action == null)
            throw new ArgumentNullException(nameof(action));
        return CallbackShimProxy.WrapCallback(_ => action(), isPackedParams: false, context);
    }

    /// <summary>
    /// Wraps a single-arg callback and returns a <see cref="CallbackHandle"/>. Pass the handle to CallJS; retain for remove/unregister. Optional context is used for .bind(context) in JS.
    /// </summary>
    public static CallbackHandle MarshalAsHandle(Action<JSObject> action, JSObject? context = null)
    {
        if (action == null)
            throw new ArgumentNullException(nameof(action));
        return CallbackShimProxy.WrapCallback(action, isPackedParams: false, context);
    }

    /// <summary>
    /// Wraps a packed-params callback and returns a <see cref="CallbackHandle"/>. JS invokes with multiple args; C# receives one packed object (use <see cref="PackedParams.GetUnpacked"/>). Optional context for .bind(context).
    /// </summary>
    public static CallbackHandle MarshalAsHandle(Callback packedParams, JSObject? context = null)
    {
        if (packedParams.Action == null)
            throw new ArgumentNullException(nameof(packedParams));
        return new CallbackHandle(CallbackShimProxy.WrapPackedParamsCallbackHandle(o =>
        {
            if (o is not JSObject jsPacked)
                throw new InvalidOperationException("Packed callback handle expected a JSObject { items: ... } from JS.");
            packedParams.Action(new PackedParams(jsPacked));
        }, context));
    }
}

/// <summary>
/// A handle to a wrapped JS callback. Create via <see cref="Callback.MarshalAsHandle(Action{JSObject}, JSObject?)"/> (or overloads) and pass to CallJS
/// when you need to reuse the same callback (e.g. register with multiple targets) without re-wrapping. Optional context is used for .bind(context) so the handler has the correct this.
/// </summary>
public readonly struct CallbackHandle : IJSObjectWrapper<CallbackHandle>
{
    /// <summary>The wrapped JS handler (function) that invokes the .NET callback.</summary>
    public JSObject JSObject { get; }

    internal CallbackHandle(JSObject jsObject) => JSObject = jsObject;

    /// <inheritdoc />
    public static CallbackHandle WrapInstance(JSObject jsObject) => new CallbackHandle(jsObject);
}

/// <summary>
/// Proxy for SerratedJSInteropShim.CallbackShim. Wraps .NET callbacks for JS, passes the wrapper to the target method, and returns the handler for unbind/remove.
/// Used by CallJS and SetJSProperty when a delegate or <see cref="Callback"/> is detected.
/// </summary>
public static class CallbackShimProxy
{
    /// <summary>
    /// Sets <c>target[propertyName] = handler</c>. When <paramref name="isPackedParams"/> is true, handler passes packed object to action. Used internally by SetProperty.
    /// </summary>
    internal static JSObject SetPropertyWithCallback(JSObject jsObject, string propertyName, Action<JSObject> action, bool isPackedParams)
    {
        if (AgnosticRuntime.IsUnoWasmBootstrapLoaded)
            return CallbackShimProxyForUno.SetPropertyWithCallback(jsObject, propertyName, action, isPackedParams);
        return CallbackShimProxyForDotNet.SetPropertyWithCallback(jsObject, propertyName, action, isPackedParams);
    }

    /// <summary>
    /// Calls <c>target[funcName](...args)</c> with the wrapped callback inserted at <paramref name="callbackParamIndex"/>.
    /// When <paramref name="isPackedParams"/> is true, JS invokes the callback with multiple args; C# receives one packed object (use <see cref="PackedParams.GetUnpacked"/>).
    /// Used internally by CallJS when a delegate or <see cref="Callback"/> is detected in the parameter list.
    /// </summary>
    internal static object CallMethodWithCallbackAt(JSObject jsObject, string funcName, int callbackParamIndex, Action<JSObject> action, object[] otherParams, bool isPackedParams)
    {
        if (AgnosticRuntime.IsUnoWasmBootstrapLoaded)
            return CallbackShimProxyForUno.CallMethodWithCallbackAt(jsObject, funcName, callbackParamIndex, action, otherParams, isPackedParams);
        return CallbackShimProxyForDotNet.CallMethodWithCallbackAt(jsObject, funcName, callbackParamIndex, action, otherParams, isPackedParams);
    }

    /// <summary>
    /// Wraps a .NET callback once and returns a <see cref="CallbackHandle"/> (SerratedJQ-style). Optional context is used for .bind(context) in JS.
    /// </summary>
    public static CallbackHandle WrapCallback(Action<JSObject> action, bool isPackedParams = false, JSObject? context = null)
    {
        if (action == null)
            throw new ArgumentNullException(nameof(action));
        JSObject jsHandler = AgnosticRuntime.IsUnoWasmBootstrapLoaded
            ? CallbackShimProxyForUno.WrapCallback(action, isPackedParams, context)
            : CallbackShimProxyForDotNet.WrapCallback(action, isPackedParams, context);
        return new CallbackHandle(jsHandler);
    }

    /// <summary>CallbackHandle path for <see cref="Callback"/>: invokes managed code with <c>{ items: arguments }</c> as <see cref="JSObject"/> (same shape as other packed shims).</summary>
    internal static JSObject WrapPackedParamsCallbackHandle(Action<object?> onItems, JSObject? context = null)
    {
        if (onItems == null)
            throw new ArgumentNullException(nameof(onItems));
        return AgnosticRuntime.IsUnoWasmBootstrapLoaded
            ? CallbackShimProxyForUno.WrapPackedParamsCallback(onItems, context)
            : CallbackShimProxyForDotNet.WrapPackedParamsCallback(onItems, context);
    }

}

/// <summary>
/// Backward-compatibility API for creating callback handles. Prefer <see cref="Callback.MarshalAsHandle(Action, JSObject?)"/> and overloads.
/// </summary>
[Obsolete("Use Callback.MarshalAsHandle(...) instead.")]
public static class MarshalCallback
{
    /// <summary>
    /// Wraps a no-arg callback and returns a <see cref="CallbackHandle"/>. Pass the handle to CallJS; retain for remove/unregister. Optional context is used for .bind(context) in JS.
    /// </summary>
    public static CallbackHandle MarshalCallbackAsWrapper(Action action, JSObject? context = null)
    {
        return Callback.MarshalAsHandle(action, context);
    }

    /// <summary>
    /// Wraps a single-arg callback and returns a <see cref="CallbackHandle"/>. Pass the handle to CallJS; retain for remove/unregister. Optional context is used for .bind(context) in JS.
    /// </summary>
    public static CallbackHandle MarshalCallbackAsWrapper(Action<JSObject> action, JSObject? context = null)
    {
        return Callback.MarshalAsHandle(action, context);
    }

    /// <summary>
    /// Wraps a packedParams callback and returns a <see cref="CallbackHandle"/>. JS invokes with multiple args; C# receives one packed object (use <see cref="PackedParams.GetUnpacked"/>). Optional context for .bind(context).
    /// </summary>
    public static CallbackHandle MarshalCallbackAsWrapper(Callback packedParams, JSObject? context = null)
    {
        return Callback.MarshalAsHandle(packedParams, context);
    }
}

internal static partial class CallbackShimProxyForDotNet
{
    private const string BaseJSNamespace = "SerratedJSInteropShim.CallbackShim";
    private const string ModuleName = "SerratedJSInteropShim";

    [JSImport(BaseJSNamespace + ".SetPropertyWithCallback", ModuleName)]
    [return: JSMarshalAs<JSType.Object>]
    public static partial JSObject SetPropertyWithCallback(JSObject jsObject, string propertyName, [JSMarshalAs<JSType.Function<JSType.Object>>] Action<JSObject> action, bool isPackedParams);

    [JSImport(BaseJSNamespace + ".CallMethodWithCallbackAt", ModuleName)]
    [return: JSMarshalAs<JSType.Any>]
    public static partial object CallMethodWithCallbackAt(JSObject jsObject, string funcName, int callbackParamIndex, [JSMarshalAs<JSType.Function<JSType.Object>>] Action<JSObject> action, [JSMarshalAs<JSType.Array<JSType.Any>>] object[] otherParams, bool isPackedParams);

    [JSImport(BaseJSNamespace + ".WrapCallback", ModuleName)]
    [return: JSMarshalAs<JSType.Object>]
    public static partial JSObject WrapCallback([JSMarshalAs<JSType.Function<JSType.Object>>] Action<JSObject> action, bool isPackedParams, [JSMarshalAs<JSType.Object>] JSObject? context);

    [JSImport(BaseJSNamespace + ".WrapPackedParamsCallback", ModuleName)]
    [return: JSMarshalAs<JSType.Object>]
    public static partial JSObject WrapPackedParamsCallback([JSMarshalAs<JSType.Function<JSType.Any>>] Action<object?> onItems, [JSMarshalAs<JSType.Object>] JSObject? context);
}

internal static partial class CallbackShimProxyForUno
{
    private const string BaseJSNamespace = "globalThis.SerratedJSInteropShim.CallbackShim";

    [JSImport(BaseJSNamespace + ".SetPropertyWithCallback")]
    [return: JSMarshalAs<JSType.Object>]
    public static partial JSObject SetPropertyWithCallback(JSObject jsObject, string propertyName, [JSMarshalAs<JSType.Function<JSType.Object>>] Action<JSObject> action, bool isPackedParams);

    [JSImport(BaseJSNamespace + ".CallMethodWithCallbackAt")]
    [return: JSMarshalAs<JSType.Any>]
    public static partial object CallMethodWithCallbackAt(JSObject jsObject, string funcName, int callbackParamIndex, [JSMarshalAs<JSType.Function<JSType.Object>>] Action<JSObject> action, [JSMarshalAs<JSType.Array<JSType.Any>>] object[] otherParams, bool isPackedParams);

    [JSImport(BaseJSNamespace + ".WrapCallback")]
    [return: JSMarshalAs<JSType.Object>]
    public static partial JSObject WrapCallback([JSMarshalAs<JSType.Function<JSType.Object>>] Action<JSObject> action, bool isPackedParams, [JSMarshalAs<JSType.Object>] JSObject? context);

    [JSImport(BaseJSNamespace + ".WrapPackedParamsCallback")]
    [return: JSMarshalAs<JSType.Object>]
    public static partial JSObject WrapPackedParamsCallback([JSMarshalAs<JSType.Function<JSType.Any>>] Action<object?> onItems, [JSMarshalAs<JSType.Object>] JSObject? context);
}
