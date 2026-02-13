
# SerratedJSInterop Overview

Reduces or eliminates need for explicit JavaScript shims and [JSImport] declarations when using the [JSImport]/System.Runtime.InteropServices.JavaScript flavor of .NET/JavaScript interop. Simplifies interop by reducing amount of boilerplate code needed when implementing JavaScript type wrappers or calling native JavaScript.

# Initialization/Startup

Typically you will not reference the SerratedJSInterop Nuget package directly, but instead reference one of the platform-specific packages that in turn depend on this package, such as: 

- SerratedSharp.SerratedJSInterop.Blazor, for Standalone Blazor WebAssembly projects.
- SerratedSharp.SerratedJSInterop.WasmBrowser, for .NET WebAssembly Browser Apps (from the wasm-experimental workload).
- SerratedSharp.SerratedJSInterop.Uno, for .NET Uno Platform projects targeting WebAssembly.

(TODO: Make list items into links to readme's)

These platform-specific packages provide some necessary initialization code and in some cases helpers.  For example, the Blazor package provides additional utilities for marshelling IJSRuntime JSObjects to InteropServices JSObjects.

# Usage


