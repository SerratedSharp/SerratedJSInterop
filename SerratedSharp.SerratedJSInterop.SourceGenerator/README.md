# SerratedSharp.SerratedJSInterop.SourceGenerator

Source generator for the Serrated JS Interop helpers.

## Generated output

- **HelpersJSInternal.g.cs** – `HelpersJSInternal` static class with Uno vs .NET browser routing and `IsUnoWasmBootstrapLoaded`, plus one method per `[AgnosticJSImport]` that delegates to `GlobalProxy.HelpersProxy` or `GlobalProxy.HelpersProxyForUno`.
- **HelpersJS.g.cs** – Partial method implementations on the attributed class (e.g. `HelpersJS`) that delegate to `HelpersJSInternal`.

## Not generated (by design)

**HelpersProxy** and **HelpersProxyForUno** stay in hand-written source (`HelpersProxy.cs`). They use `[JSImport]` partial methods, and the .NET runtime’s JS interop source generator adds the implementation. That generator runs on project source; it does not run on code produced by this generator, so generating the proxy classes would leave `[JSImport]` partial methods without an implementation. When you add or change `[AgnosticJSImport]` methods on `HelpersJS`, update `HelpersProxy` and `HelpersProxyForUno` in `HelpersProxy.cs` to match (same method names, JS names, and module/namespace).

## Attributes

- **AgnosticModuleReferenceAttribute** (on the class) – `(baseJSNamespace, moduleName)` used for JS import (e.g. `"SerratedInteropHelpers.HelpersProxy"`, `"SerratedInteropHelpers"`).
- **AgnosticJSImportAttribute** (on methods) – Marks a static partial method as an agnostic JS import. Optional constructor argument is the JS function name; if omitted, the method name is used.
