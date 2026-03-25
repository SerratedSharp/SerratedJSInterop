#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace SerratedSharp.SerratedJSInterop;

using System.Diagnostics.CodeAnalysis;
using SerratedSharp.SerratedJSInterop.Internal;
using System;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.JavaScript;

internal static class JSImportInstanceHelpers
{
    /// <summary>Used by extension methods that call into CastOrWrap/GetProperty/CallJSFunc so their generic J matches trimmer requirements.</summary>
    internal const DynamicallyAccessedMemberTypes WrapperTypeMembers =
        DynamicallyAccessedMemberTypes.PublicMethods
        | DynamicallyAccessedMemberTypes.NonPublicMethods
        | DynamicallyAccessedMemberTypes.Interfaces;

    // J can be JSObject, primitive, object[] (JSType.Any / mixed arrays), string[]/double[]/JSObject[], or IJSObjectWrapper<J>
    public static J GetProperty<[DynamicallyAccessedMembers(WrapperTypeMembers)] J>(JSObject jsObject, string propertyName, bool applyJSCasing = true)
    {
        var name = applyJSCasing ? ToJSCasing(propertyName) : propertyName;
        object? genericObject = InstanceHelperJS.PropertyByNameToObject(jsObject, name);
        return CastOrWrap<J>(genericObject);
    }

    /// <summary>
    /// Sets a property on the JS object. When <paramref name="value"/> is an Action, Action&lt;JSObject&gt;, or Callback,
    /// sets a wrapped handler and returns it for later removal; otherwise returns null.
    /// </summary>
    public static JSObject? SetProperty(JSObject jsObject, string propertyName, object value, bool applyJSCasing = true)
    {
        var name = applyJSCasing ? ToJSCasing(propertyName) : propertyName;
        if (value is Callback packedParams)
            return CallbackShimProxy.SetPropertyWithCallback(jsObject, name, packedParams.ToShimAction(), isPackedParams: true);
        if (value is Action<JSObject> act)
            return CallbackShimProxy.SetPropertyWithCallback(jsObject, name, act, isPackedParams: false);
        if (value is Action act0)
            return CallbackShimProxy.SetPropertyWithCallback(jsObject, name, _ => act0(), isPackedParams: false);
        var unwrappedValue = value as IJSObjectWrapper;
        InstanceHelperJS.SetPropertyByName(jsObject, name, unwrappedValue?.JSObject ?? value);
        return null;
    }

    // J should be a JSObject, IJSObjectWrapper<J>, or other primitive JS type
    public static J CallJSFunc<[DynamicallyAccessedMembers(WrapperTypeMembers)] J>(JSObject jsObject, string funcName, params object[] parameters)
        => CallJSFuncInternal<J>(jsObject, funcName, applyJSCasing: true, parameters);

    public static J CallJSFuncExplicitName<[DynamicallyAccessedMembers(WrapperTypeMembers)] J>(JSObject jsObject, string funcName, params object[] parameters)
        => CallJSFuncInternal<J>(jsObject, funcName, applyJSCasing: false, parameters);

    private static J CallJSFuncInternal<[DynamicallyAccessedMembers(WrapperTypeMembers)] J>(JSObject jsObject, string funcName, bool applyJSCasing, params object[] parameters)
    {
        var name = applyJSCasing ? ToJSCasing(funcName) : funcName;
        var (callbackIndex, callback, isPackedParams) = TryGetCallbackAtIndex(parameters);
        if (callbackIndex >= 0 && callback != null)
        {
            object[] objs = UnwrapJSObjectParams(parameters);
            var otherParams = new object[objs.Length - 1];
            for (int i = 0, j = 0; i < objs.Length; i++)
            {
                if (i != callbackIndex)
                    otherParams[j++] = objs[i];
            }
            object? callbackResult = CallbackShimProxy.CallMethodWithCallbackAt(jsObject, name, callbackIndex, callback, otherParams, isPackedParams);
            return CastOrWrap<J>(callbackResult);
        }
        object[] objsNormal = UnwrapJSObjectParams(parameters);
        object? genericObject = null;
        Type type = typeof(J);

        if (type.IsArray)
        {
            switch (type.GetElementType())
            {
                case Type t when t == typeof(string):
                    genericObject = InstanceHelperJS.FuncByNameAsStringArray(jsObject, name, objsNormal);
                    break;
                case Type t when t == typeof(double):
                    genericObject = InstanceHelperJS.FuncByNameAsDoubleArray(jsObject, name, objsNormal);
                    break;
                case Type t when t == typeof(JSObject):
                    genericObject = InstanceHelperJS.FuncByNameAsObject(jsObject, name, objsNormal);
                    break;
                default:
                    throw new NotImplementedException($"CallJSFunc: Returning array of {type.GetElementType()} not implemented");
            }
        }
        else
        {
            genericObject = InstanceHelperJS.FuncByNameAsObject(jsObject, name, objsNormal);
        }

        return CastOrWrap<J>(genericObject);
    }

    public static void CallJSFuncVoid(JSObject jsObject, string funcName, params object[] parameters)
        => CallJSFuncVoidInternal(jsObject, funcName, applyJSCasing: true, parameters);

    public static void CallJSFuncVoidExplicitName(JSObject jsObject, string funcName, params object[] parameters)
        => CallJSFuncVoidInternal(jsObject, funcName, applyJSCasing: false, parameters);

    private static void CallJSFuncVoidInternal(JSObject jsObject, string funcName, bool applyJSCasing, params object[] parameters)
    {
        var name = applyJSCasing ? ToJSCasing(funcName) : funcName;
        var (callbackIndex, callback, isPackedParams) = TryGetCallbackAtIndex(parameters);
        if (callbackIndex >= 0 && callback != null)
        {
            // if has a callback param, bundle params with the callback first(to comply with JSImport signature)
            // and an index indicating the true position of the callback param so it and otherParams can be reordered on the JS side
            object[] objs = UnwrapJSObjectParams(parameters);
            var otherParams = new object[objs.Length - 1];
            for (int i = 0, j = 0; i < objs.Length; i++)
            {
                if (i != callbackIndex)
                    otherParams[j++] = objs[i];
            }
            _ = CallbackShimProxy.CallMethodWithCallbackAt(jsObject, name, callbackIndex, callback, otherParams, isPackedParams);
            return;
        }
        else // normal CallJS/invocation through FuncByNameVoid
        {
            object[] objsNormal = UnwrapJSObjectParams(parameters);
            InstanceHelperJS.FuncByNameVoid(jsObject, name, objsNormal);
        }
    }

    /// <summary>
    /// Finds the first parameter that is an Action, Action&lt;JSObject&gt;, or Callback and returns its index, normalized Action&lt;JSObject&gt;, and isPackedParams.
    /// Used to route CallJS to the callback-at-index shim when a delegate or Callback is passed as a normal object parameter.
    /// </summary>
    private static (int index, Action<JSObject>? callback, bool isPackedParams) TryGetCallbackAtIndex(object[] parameters)
    {
        if (parameters == null)
            return (-1, null, false);
        for (int i = 0; i < parameters.Length; i++)
        {
            object param = parameters[i];
            if (param is Callback packedParams)
                return (i, packedParams.ToShimAction(), true);
            if (param is Action<JSObject> act)
                return (i, act, false);
            if (param is Action act0)
                return (i, _ => act0(), false);
        }
        return (-1, null, false);
    }

    // Casts the object to J, or if J is an IJSObjectWrapper, wraps the JSObject using the cached WrapInstance delegate.
    internal static J CastOrWrap<[DynamicallyAccessedMembers(WrapperTypeMembers)] J>(object? genericObject)
    {
        if (genericObject is null)
            return default!;

        try
        {
            Type type = typeof(J);

            // Array types: convert object[] from interop to string[], double[], or JSObject[] as requested.
            if (type.IsArray)
            {
                Type elementType = type.GetElementType()!;
                if (genericObject is object[] objArray)
                {
                    // elementType is J's declared element type (e.g. string for string[]), not the runtime type of each slot;
                    // typeof(string) != typeof(object), so only GetJSProperty<object[]> matches typeof(object).
                    if (elementType == typeof(string))
                        return (J)(object)Array.ConvertAll(objArray, o => o?.ToString() ?? "");
                    if (elementType == typeof(double))
                        return (J)(object)Array.ConvertAll(objArray, o => o is double d ? d : Convert.ToDouble(o));
                    if (elementType == typeof(JSObject))
                        return (J)(object)objArray.Cast<JSObject>().ToArray();
                    if (elementType == typeof(object))
                        return (J)(object)objArray; // mixed JSType.Any array; only matches J == object[]
                    throw new NotImplementedException($"CastOrWrap: Array of {elementType} not implemented");
                }
                return (J)genericObject!; // already string[] or double[] from typed interop
            }

            // If J implements IJSObjectWrapper<J> and we have a JSObject, wrap it via cached delegate
            if (genericObject is JSObject jsObj && WrapperInvoker<J>.Wrap != null)
                return WrapperInvoker<J>.Wrap(jsObj);

            // JS interop often returns numbers as double; unboxing (int)(object)boxedDouble throws.
            if (type.IsValueType && !type.IsEnum)
            {
                TypeCode code = Type.GetTypeCode(type);
                if (code >= TypeCode.SByte && code <= TypeCode.Decimal)
                {
                    try
                    {
                        return (J)Convert.ChangeType(genericObject, type);
                    }
                    catch (InvalidCastException) { /* fall through to direct cast */ }
                }
            }

            return (J)genericObject!;
        }
        catch (InvalidCastException ex)
        {
            string sourceTypeName = genericObject?.GetType().FullName ?? "null";
            string targetTypeName = typeof(J).FullName ?? typeof(J).Name;
            throw new InvalidCastException(
                $"Failure to coerce type returned from JS interop, from ({sourceTypeName}) into ({targetTypeName}). See inner exception for details.",
                ex);
        }
    }

    /// <summary>
    /// Per-type cached delegate for calling IJSObjectWrapper&lt;T&gt;.WrapInstance.
    /// Reflection runs once per T when the static field initializes; all subsequent calls use the fast delegate.
    /// </summary>
    private static class WrapperInvoker<[DynamicallyAccessedMembers(WrapperTypeMembers)] T>
    {
        internal static readonly Func<JSObject, T>? Wrap = ResolveWrapper();

        [UnconditionalSuppressMessage("Trimming", "IL2060",
            Justification = "MakeGenericMethod is called with a type verified at runtime to satisfy IJSObjectWrapper<T>; T is annotated with WrapperTypeMembers.")]
        [UnconditionalSuppressMessage("Trimming", "IL2070",
            Justification = "T is annotated with WrapperTypeMembers which preserves Interfaces and Methods.")]
        private static Func<JSObject, T>? ResolveWrapper()
        {
            Type type = typeof(T);
            if (!typeof(IJSObjectWrapper).IsAssignableFrom(type))
                return null;

            Type openGeneric = typeof(IJSObjectWrapper<>);
            Type[] interfaces = type.GetInterfaces();
            for (int i = 0; i < interfaces.Length; i++)
            {
                Type iface = interfaces[i];
                if (iface.IsGenericType && iface.GetGenericTypeDefinition() == openGeneric)
                {
                    Type[] args = iface.GetGenericArguments();
                    if (args.Length == 1 && args[0] == type)
                    {
                        // T implements IJSObjectWrapper<T>. Create a cached delegate that calls
                        // the constrained TWrapper.WrapInstance via MakeGenericMethod bridge.
                        MethodInfo openMethod = typeof(WrapperInvoker<T>)
                            .GetMethod(nameof(CallWrapInstance), BindingFlags.NonPublic | BindingFlags.Static)!;
                        MethodInfo closedMethod = openMethod.MakeGenericMethod(type);
                        return (Func<JSObject, T>)Delegate.CreateDelegate(typeof(Func<JSObject, T>), closedMethod);
                    }
                }
            }
            return null;
        }

        // Constrained bridge: the compiler can resolve TWrapper.WrapInstance here because of the constraint.
        // Called indirectly via the cached delegate created by ResolveWrapper.
        private static TWrapper CallWrapInstance<TWrapper>(JSObject jsObj)
            where TWrapper : IJSObjectWrapper<TWrapper>
        {
            return TWrapper.WrapInstance(jsObj);
        }
    }

    public static object[] UnwrapJSObjectParams(object[] parameters)
    {
        if (parameters == null)
            return Array.Empty<object>();

        object[]? objs = null;
        for (int i = 0; i < parameters.Length; i++)
        {
            object param = parameters[i];
            if (param is IJSObjectWrapper wrapper)
            {
                if (objs == null)
                    objs = TypedArrayToObjectArray(parameters, i);
                objs[i] = wrapper.JSObject;
            }
            else if (objs != null)
            {
                objs[i] = param;
            }
        }
        return objs ?? parameters;
    }

    private static object[] TypedArrayToObjectArray(object[] objs, int index)
    {
        if (objs.GetType().GetElementType() == typeof(object))
            return objs;

        object[] objs2 = new object[objs.Length];
        Array.Copy(objs, objs2, index + 1);
        return objs2;
    }

    
    public static string ToJSCasing(string identifier)
    {
        if (string.IsNullOrEmpty(identifier))
            return identifier;

        if (identifier.Length == 1)
            return char.ToLowerInvariant(identifier[0]).ToString();
        // Lower-cases first character for JS lowerCamelCase.
        return char.ToLowerInvariant(identifier[0]) + identifier.Substring(1);
    }
}
