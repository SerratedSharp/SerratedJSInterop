using System.Runtime.InteropServices.JavaScript;

namespace SerratedSharp.SerratedJSInterop;

/// <summary>
/// Agnostic stand-in for <see cref="JSMarshalAsAttribute{T}"/>. The source generator
/// emits <c>[JSMarshalAs&lt;T&gt;]</c> (or <c>[return: JSMarshalAs&lt;T&gt;]</c>) in the
/// generated code by dropping the "Agnostic" prefix. Use the same type arguments as
/// <see cref="JSType"/> (e.g. <see cref="JSType.Any"/>, <see cref="JSType.Array{T}"/>).
/// </summary>
/// <typeparam name="T">One of the types defined in <see cref="JSType"/>.</typeparam>
[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.ReturnValue, Inherited = false, AllowMultiple = false)]
public sealed class AgnosticJSMarshalAsAttribute<T> : Attribute where T : JSType
{
    /// <summary>
    /// Initializes a new instance configured by the generic parameter (same as <see cref="JSMarshalAsAttribute{T}"/>).
    /// </summary>
    public AgnosticJSMarshalAsAttribute() { }
}
