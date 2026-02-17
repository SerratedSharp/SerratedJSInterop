using System.Text.Json;

namespace SerratedSharp.SerratedJSInterop;

internal static class PocoMarshal
{
    /// <summary>
    /// Prefix used when serializing POCOs for interop; JS ObjectNew detects this and deserializes.
    /// </summary>
    public const string SerratedPocoPrefix = "serratedPoco:";
}

/// <summary>
/// Serializes a POCO to JSON (camelCase) and prefixes with SerratedPocoPrefix so JS can detect and deserialize without trying to parse every string.
/// </summary>
public static class MarshalAsJsonExtensions
{
    /// <summary>
    /// Serializes a POCO to JSON (camelCase) and prefixes with SerratedPocoPrefix so JS can detect and deserialize without trying to parse every string.
    /// </summary>
    public static string MarshalAsJson(this object obj)
    {
        var json = JsonSerializer.Serialize(obj, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        return PocoMarshal.SerratedPocoPrefix + json;
    }
}
