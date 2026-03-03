#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace SerratedSharp.SerratedJSInterop;

using System.Diagnostics.CodeAnalysis;
using SerratedSharp.SerratedJSInterop.Internal;
using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.JavaScript;

internal static class JSImportInstanceHelpers
{
    /// <summary>Used by extension methods that call into CastOrWrap/GetProperty/CallJSFunc so their generic J matches trimmer requirements.</summary>
    internal const DynamicallyAccessedMemberTypes WrapperTypeMembers =
        DynamicallyAccessedMemberTypes.PublicMethods
        | DynamicallyAccessedMemberTypes.NonPublicMethods
        | DynamicallyAccessedMemberTypes.Interfaces;

    // J can be JSObject, primitive, or IJSObjectWrapper<J>
    public static J GetProperty<[DynamicallyAccessedMembers(WrapperTypeMembers)] J>(JSObject jsObject, string propertyName, bool applyJSCasing = true)
    {
        var name = applyJSCasing ? ToJSCasing(propertyName) : propertyName;
        object? genericObject = InstanceHelperJS.PropertyByNameToObject(jsObject, name);
        return CastOrWrap<J>(genericObject);
    }

    public static void SetProperty(JSObject jsObject, string propertyName, object value, bool applyJSCasing = true)
    {
        var name = applyJSCasing ? ToJSCasing(propertyName) : propertyName;
        InstanceHelperJS.SetPropertyByName(jsObject, name, value);
    }

    // J should be a JSObject, IJSObjectWrapper<J>, or other primitive JS type
    public static J CallJSFunc<[DynamicallyAccessedMembers(WrapperTypeMembers)] J>(JSObject jsObject, string funcName, params object[] parameters)
        => CallJSFuncInternal<J>(jsObject, funcName, applyJSCasing: true, parameters);

    public static J CallJSFuncExplicitName<[DynamicallyAccessedMembers(WrapperTypeMembers)] J>(JSObject jsObject, string funcName, params object[] parameters)
        => CallJSFuncInternal<J>(jsObject, funcName, applyJSCasing: false, parameters);

    private static J CallJSFuncInternal<[DynamicallyAccessedMembers(WrapperTypeMembers)] J>(JSObject jsObject, string funcName, bool applyJSCasing, params object[] parameters)
    {
        var name = applyJSCasing ? ToJSCasing(funcName) : funcName;
        object[] objs = UnwrapJSObjectParams(parameters);
        object? genericObject;
        Type type = typeof(J);
        if (type.IsArray)
        {
            switch (type.GetElementType())
            {
                case Type t when t == typeof(string):
                    genericObject = InstanceHelperJS.FuncByNameAsStringArray(jsObject, name, objs);
                    return (J)genericObject;
                case Type t when t == typeof(double):
                    genericObject = InstanceHelperJS.FuncByNameAsDoubleArray(jsObject, name, objs);
                    return (J)genericObject;

                case Type t when t == typeof(JSObject): // JSObject[] array
                    genericObject = InstanceHelperJS.FuncByNameAsObject(jsObject, name, objs);
                    var array = (object[])genericObject;
                    return (J)(object)(array.Cast<JSObject>().ToArray());
                default:
                    throw new NotImplementedException($"CallJSFunc: Returning array of {type.GetElementType()} not implemented");
            }
        }
        else
        {
            genericObject = InstanceHelperJS.FuncByNameAsObject(jsObject, name, objs);
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
        object[] objs = UnwrapJSObjectParams(parameters);
        InstanceHelperJS.FuncByNameVoid(jsObject, name, objs);
    }

    // Casts the object to J, or if J is an IJSObjectWrapper, wraps the JSObject using WrapInstance.
    internal static J CastOrWrap<[DynamicallyAccessedMembers(WrapperTypeMembers)] J>(object? genericObject)
    {
        Type type = typeof(J);

        // Check if J implements IJSObjectWrapper<J> (only valid when J is a wrapper type; J may be JSObject or primitive)
        Type? wrapperInterface = TryGetIJSObjectWrapperOfSelf(type);
        if (wrapperInterface != null)
        {
            if (genericObject is JSObject jsObj)
            {
                var wrapMethod = type.GetMethod("WrapInstance",
                    BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);

                if (wrapMethod != null)
                {
                    return (J)wrapMethod.Invoke(null, new object[] { jsObj })!;
                }

                // Fallback: try explicit interface implementation
                var interfaceMap = type.GetInterfaceMap(wrapperInterface);
                for (int i = 0; i < interfaceMap.InterfaceMethods.Length; i++)
                {
                    if (interfaceMap.InterfaceMethods[i].Name == "WrapInstance")
                    {
                        return (J)interfaceMap.TargetMethods[i].Invoke(null, new object[] { jsObj })!;
                    }
                }
            }

            // If genericObject is null or not a JSObject, return default
            return default!;
        }

        // JS interop often returns numbers as double; unboxing (int)(object)boxedDouble throws.
        if (genericObject != null && type.IsValueType && !type.IsEnum)
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

    public static object[] UnwrapJSObjectParams(object[] parameters)
    {
        if(parameters == null)
            return Array.Empty<object>();

        // New param array with unwrapped JSOBjects if wrappers are found.  Only create array if/when we encounter first wrapper
        object[]? objs = null;
        for (int i = 0; i < parameters.Length; i++)
        {
            object param = parameters[i];
            if (param is IJSObjectWrapper wrapper)
            {
                // if first wrapper encountered, copy array up till this point
                if (objs == null)
                    objs = TypedArrayToObjectArray(parameters, i);

                objs[i] = wrapper.JSObject; // unwrap
            }
            else if (objs != null) // if we created new array for unwrapping 
            {
                // then finish copying any normal params into remainder of new array
                objs[i] = param;
            }
        }
        if (objs == null)
            objs = parameters;
        return objs;
    }

    private static object[] TypedArrayToObjectArray(object[] objs, int index)
    {
        if (objs.GetType().GetElementType() == typeof(object))
        {
            return objs;
        }
        else
        {            
            object[] objs2 = new object[objs.Length];
            Array.Copy(objs, objs2, index + 1);
            return objs2;
        }
    }

    
    // Returns IJSObjectWrapper&lt;T&gt; if type T implements it (i.e. T : IJSObjectWrapper<T>); otherwise null.
    // Avoids MakeGenericType with types that don't satisfy the constraint (e.g. JSObject, primitives).
    [return: DynamicallyAccessedMembers(WrapperTypeMembers)]
    [System.Diagnostics.CodeAnalysis.UnconditionalSuppressMessage("Trimming", "IL2073",
        Justification = "Returned Type is one of type.GetInterfaces(); type is annotated with WrapperTypeMembers.")]
    private static Type? TryGetIJSObjectWrapperOfSelf([DynamicallyAccessedMembers(WrapperTypeMembers)] Type type)
    {
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
                    return iface;
            }
        }
        return null;
    }

    // Lower-cases first character for JS lowerCamelCase. Use CallJSFuncExplicitName/CallJSFuncVoidExplicitName or SetProperty(..., applyJSCasing: false) to preserve caller casing.
    public static string ToJSCasing(string identifier)
        => Char.ToLowerInvariant(identifier[0]) + identifier.Substring(1);
}
