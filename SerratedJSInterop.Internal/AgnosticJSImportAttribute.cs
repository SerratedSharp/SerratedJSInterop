namespace SerratedSharp.SerratedJSInterop;

/// <summary>
/// Marks a static partial method as an agnostic JS import. The source generator
/// will emit the consumed API (e.g. Helpers), .NET browser (HelpersProxy), and Uno
/// (HelpersProxyForUno) implementations. If <paramref name="jsName"/> is not
/// specified, the method name is used as the JavaScript function name.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
public sealed class AgnosticJSImportAttribute : Attribute
{
    /// <summary>
    /// The JavaScript function or property name. Defaults to the method name when null or empty.
    /// </summary>
    public string? JsName { get; }

    public AgnosticJSImportAttribute() { }

    public AgnosticJSImportAttribute(string jsName)
    {
        JsName = jsName;
    }
}
