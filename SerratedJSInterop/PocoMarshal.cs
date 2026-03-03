using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using System.Diagnostics.CodeAnalysis;

namespace SerratedSharp.SerratedJSInterop;

internal static class PocoMarshal
{
    /// <summary>
    /// Prefix used when serializing POCOs for interop; JS ObjectNew detects this and deserializes.
    /// </summary>
    public const string SerratedPocoPrefix = "serratedPoco:";
}

/// <summary>
/// Serializes a POCO to JSON and prefixes with SerratedPocoPrefix so JS can detect and deserialize without trying to parse every string.
/// </summary>
public static class MarshalAsJsonExtensions
{
    /// <summary>
    /// Reflection-based serialization. Not trim-safe when used with arbitrary object types under IL trimming.
    /// </summary>
    [RequiresUnreferencedCode("This overload uses reflection-based serialization and is not trim-safe. Favor MarshalAsJson<T>(this T, JsonTypeInfo<T>) or MarshalAsJson<T>(this T, JsonSerializerContext) when publishing trimmed apps.")]
    public static string MarshalAsJson(this object obj)
    {
        var json = JsonSerializer.Serialize(obj, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        return PocoMarshal.SerratedPocoPrefix + json;
    }

    /// <summary>
    /// Trim-friendly serialization using a source-generated or otherwise configured JsonTypeInfo for T.
    /// </summary>
    public static string MarshalAsJson<T>(this T obj, JsonTypeInfo<T> jsonTypeInfo)
    {
        if (jsonTypeInfo is null)
            throw new ArgumentNullException(nameof(jsonTypeInfo));

        var json = JsonSerializer.Serialize(obj, jsonTypeInfo);
        return PocoMarshal.SerratedPocoPrefix + json;
    }

    /// <summary>
    /// Trim-friendly serialization using a JsonSerializerContext that provides type info for T.
    /// </summary>
    public static string MarshalAsJson<T>(this T obj, JsonSerializerContext context)
    {
        if (context is null)
            throw new ArgumentNullException(nameof(context));

        var typeInfo = (JsonTypeInfo<T>?)context.GetTypeInfo(typeof(T));
        if (typeInfo is null)
            throw new InvalidOperationException($"JsonSerializerContext '{context.GetType().FullName}' does not contain metadata for type '{typeof(T).FullName}'.");

        var json = JsonSerializer.Serialize(obj, typeInfo);
        return PocoMarshal.SerratedPocoPrefix + json;
    }
}
