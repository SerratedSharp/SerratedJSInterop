# SerratedJSInterop.SourceGenerator

Source generator that reads `[AgnosticModuleReference]` and `[AgnosticJSImport]` from the Internal project and generates the helper and proxy C# (e.g. `HelpersJS.g.cs`, `InstanceHelperJS.g.cs`) used by the main SerratedJSInterop assembly.

Ensures both Uno and .NET flavors of bindings are generated.