# SerratedSharp.SerratedJSInterop.Internal

This project holds the source for JS interop helpers, from which .NET and Uno flavors of JSImport declarations are generated.

Classes tagged with `[AgnosticModuleReference]` and methods tagged with `[AgnosticJSImport]` (e.g. `HelpersJSSource`) are consumed by the **SerratedSharp.SerratedJSInterop.SourceGenerator** project. The generator runs when this project is built and writes the generated C# (HelpersJS, HelpersJSInternal, HelpersProxy, HelpersProxyForUno) into the **SerratedSharp.SerratedJSInterop** project’s `Generated/` folder. The main SerratedJSInterop project compiles those files so the runtime `[JSImport]` generator can add implementations for the proxy types.

For more detail on the generator and attributes, see the [SourceGenerator README](../SerratedSharp.SerratedJSInterop.SourceGenerator/README.md).
