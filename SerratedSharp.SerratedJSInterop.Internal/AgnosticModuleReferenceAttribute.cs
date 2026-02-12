namespace SerratedSharp.SerratedJSInterop;

/// <summary>
/// Applied to a partial static class that declares agnostic JS interop methods.
/// Supplies the JavaScript namespace and module name used by the source generator
/// to emit HelpersProxy and HelpersProxyForUno implementations.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public sealed class AgnosticModuleReferenceAttribute : Attribute
{
    /// <summary>
    /// Base JavaScript namespace for the helper (e.g. "SerratedInteropHelpers.HelpersProxy").
    /// </summary>
    public string BaseJSNamespace { get; }

    /// <summary>
    /// Module name for JSImport when not using Uno (e.g. "SerratedInteropHelpers").
    /// </summary>
    public string ModuleName { get; }

    /// <summary>
    /// When true, proxy classes are nested in GlobalProxy (module-style). When false, they are top-level (instance-helper style). Default is false.
    /// </summary>
    public bool NestProxiesInGlobalProxy { get; }

    public AgnosticModuleReferenceAttribute(string baseJSNamespace, string moduleName, bool nestProxiesInGlobalProxy = false)
    {
        BaseJSNamespace = baseJSNamespace;
        ModuleName = moduleName;
        NestProxiesInGlobalProxy = nestProxiesInGlobalProxy;
    }
}
