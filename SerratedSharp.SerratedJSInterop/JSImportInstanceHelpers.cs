namespace SerratedSharp.SerratedJSInterop;

using SerratedSharp.SerratedJSInterop.Internal;
using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.JavaScript;

public static class JSImportInstanceHelpers
{
    // J should be a JSObject or other prmitiive JS type
    public static J GetPropertyOfSameName<J>(JSObject jsObject, Breaker _ = default(Breaker), [CallerMemberName] string? propertyName = null)
    {
        return GetProperty<J>(jsObject, propertyName!);
    }

    // Added: Returns property as a wrapped instance of W
    public static W GetPropertyOfSameNameAsWrapped<W>(JSObject jsObject, Breaker _ = default(Breaker), [CallerMemberName] string? propertyName = null)
        where W : IJSObjectWrapper<W>
    {
        JSObject jsProp = GetPropertyOfSameName<JSObject>(jsObject, _, propertyName);
        return W.WrapInstance(jsProp);
    }

    // J can be JSObject, primitive, or IJSObjectWrapper<J> (uses CastOrWrap like CallJSFunc)
    public static J GetProperty<J>(JSObject jsObject, string propertyName)
    {
        object? genericObject = InstanceHelperJS.PropertyByNameToObject(jsObject, ToJSCasing(propertyName));
        return CastOrWrap<J>(genericObject);
    }

    public static void SetProperty(JSObject jsObject, string propertyName, object value)
        => InstanceHelperJS.SetPropertyByName(jsObject, ToJSCasing(propertyName), value);

    public static void SetPropertyOfSameName(JSObject jsObject, object value, Breaker _ = default(Breaker), [CallerMemberName] string? propertyName = null)
        => SetProperty(jsObject, propertyName!, value);

    // This call automatically wraps a JSObject using type W's WrapInstance interface
    public static W CallJSOfSameNameAsWrapped<W>(JSObject jsObject, object[] parameters, Breaker _ = default(Breaker), [CallerMemberName] string? funcName = null)
        where W : IJSObjectWrapper<W>
    {
        JSObject jsObjectRtn = CallJSOfSameName<JSObject>(jsObject, parameters, _, funcName);
        return W.WrapInstance(jsObjectRtn);
    }

    // J should be a JSObject or other prmitiive JS type
    public static J CallJSOfSameName<J>(JSObject jsObject, object[] parameters, Breaker _ = default(Breaker), [CallerMemberName] string? funcName = null)
    {
        return CallJSFunc<J>(jsObject, funcName!, parameters);
    }

    // J should be a JSObject, IJSObjectWrapper<J>, or other primitive JS type
    public static J CallJSFunc<J>(JSObject jsObject, string funcName, params object[] parameters)
    {
        object[] objs = UnwrapJSObjectParams(parameters);
        object? genericObject;
        Type type = typeof(J);
        if (type.IsArray)
        {
            switch (type.GetElementType())
            {
                case Type t when t == typeof(string):
                    genericObject = InstanceHelperJS.FuncByNameAsStringArray(jsObject, ToJSCasing(funcName), objs);
                    return (J)genericObject;
                case Type t when t == typeof(double):
                    genericObject = InstanceHelperJS.FuncByNameAsDoubleArray(jsObject, ToJSCasing(funcName), objs);
                    return (J)genericObject;
                default:
                    throw new NotImplementedException($"CallJSFunc: Returning array of {type.GetElementType()} not implemented");
            }
        }
        else
        {
            genericObject = InstanceHelperJS.FuncByNameAsObject(jsObject, ToJSCasing(funcName), objs);
        }

        return CastOrWrap<J>(genericObject);
    }

    /// <summary>
    /// Casts the object to J, or if J is an IJSObjectWrapper, wraps the JSObject using WrapInstance.
    /// </summary>
    private static J CastOrWrap<J>(object? genericObject)
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

        return (J)genericObject!;
    }

    public static object[] UnwrapJSObjectParams(object[] parameters)
    {
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

    /// <summary>
    /// Returns IJSObjectWrapper&lt;T&gt; if type T implements it (i.e. T : IJSObjectWrapper&lt;T&gt;); otherwise null.
    /// Avoids MakeGenericType with types that don't satisfy the constraint (e.g. JSObject, primitives).
    /// </summary>
    private static Type? TryGetIJSObjectWrapperOfSelf(Type type)
    {
        if (!typeof(IJSObjectWrapper).IsAssignableFrom(type))
            return null;

        Type openGeneric = typeof(IJSObjectWrapper<>);
        foreach (Type iface in type.GetInterfaces())
        {
            if (iface.IsGenericType && iface.GetGenericTypeDefinition() == openGeneric)
            {
                Type[] args = iface.GetGenericArguments();
                if (args.Length == 1 && args[0] == type)
                    return iface;
            }
        }
        return null;
    }

    // lower cases first character
    public static string ToJSCasing(string identifier)
        => Char.ToLowerInvariant(identifier[0]) + identifier.Substring(1);
}
