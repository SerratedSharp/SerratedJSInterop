# Overview

SerratedJSInterop provides helpers for use with the [JSImport]/System.Runtime.InteropServices.JavaScript flavor of .NET/Javascript interop. These helpers are designed to reduce or eliminate the need for explicit Javascript shims and [JSImport] declarations, significantly reducing boilerplate code for JS interop, while preserving much of the performance that this flavor of interop excels at.

# Initialization/Startup

Typically you will not reference this Nuget package directly, but instead reference one of the platform-specific packages that depend on this package, such as: 

- SerratedSharp.SerratedJSInterop.Blazor, for Standalone Blazor WebAssembly projects.
- SerratedSharp.SerratedJSInterop.WasmBrowser, for .NET WebAssembly Browser Apps (from the wasm-experimental workload).
- SerratedSharp.SerratedJSInterop.Uno, for .NET Uno Platform projects targeting WebAssembly.

(TODO: Make list items into links to readme's)

This is necesary since the way these projects load and initialize Javascript is different, and the platform-specific packages contain the necessary initialization code to ensure the JS shims leveraged by SerratedJSInterop are loaded correctly.  

See instructions in the appropriate platform-specific package for details on how to initialize SerratedJSInterop in that environment.

# Usage


