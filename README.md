# SerratedJSInterop

A class library to simplify creating .NET WebAssembly wrappers on JavaScript instances when using `System.Runtime.InteropServices.JavaScript` and [JSImport]. It reduces or eliminates the need for explicit JavaScript shims and per-method [JSImport] declarations, so you can map C# types to JS instances with minimal boilerplate. This project is a migration and formalization of SerratedSharp.JSInteropHelpers for broader use.

> **Work in progress:** The API is being refined and will be published to NuGet when ready.

## Prerequisites

- .NET 9 or later (or the target framework supported by the platform package you use).
- A project that compiles to WebAssembly: e.g. .NET WebAssembly Browser App (wasmbrowser), Standalone Blazor WebAssembly, or Uno Platform WASM.
- `System.Runtime.InteropServices.JavaScript` is used for interop.  
  - This differs from the typical Blazor IJSRuntime, but can operate side-by-side within Blazor WASM.

## Brief Example

This example demonstrates wrapping a JS type which performs interop for methods and properties.

```csharp
public class Image : IJSObjectWrapper<Image>
{
    static Image IJSObjectWrapper<Image>.WrapInstance(JSObject jsObject) => new Image(jsObject);
    public JSObject JSObject => jsObject;
    private JSObject jsObject;

    public Image() { jsObject = SerratedJS.New(nameof(Image)); }
    public Image(JSObject jsObject) { this.jsObject = jsObject; }

    public string Src { get => this.GetProperty<string>(); set => this.SetProperty(value); }
    public int Width { get => this.GetProperty<int>(); set => this.SetProperty(value); }
    public int Height { get => this.GetProperty<int>(); set => this.SetProperty(value); }
    public bool Complete => this.GetProperty<bool>();
}
```

## Quick Start

You typically reference a **platform-specific package** through NuGet that depends on SerratedJSInterop, rather than the core package directly:

- **SerratedSharp.SerratedJSInterop.Blazor** — Standalone Blazor WebAssembly projects.
- **SerratedSharp.SerratedJSInterop.WasmBrowser** — .NET WebAssembly Browser Apps (i.e. wasm-experimental workload).
- **SerratedSharp.SerratedJSInterop.Uno** — Uno Platform projects targeting WebAssembly.

These packages provide the required initialization (e.g. loading the JS shim). Additionally, the Blazor package includes utilities for marshalling `IJSRuntime` JSObjects to/from `System.Runtime.InteropServices.JavaScript.JSObject`.

TODO: Explicit steps for adding nuget package and initialization code for loading JS: https://github.com/SerratedSharp/SerratedJQ/blob/main/SerratedJQLibrary/JSInteropHelpers/readme.md#net-8-wasmbrowser-projects

### Minimal setup (WasmBrowser)

- Create a WebAssembly Browser App and add the appropriate SerratedJSInterop platform package.
- Ensure the module is imported at startup (the platform package usually handles this).
- Use `SerratedJS.New`, `GetProperty`/`SetProperty`, and `CallJS` from your wrapper types as in the Example above.

### Minimal setup (Blazor / Uno)

- Add the corresponding SerratedJSInterop platform package (Blazor or Uno).
- Follow the package’s readme for any one-time initialization (e.g. script loading or service registration).

## Usage

There are two main ways to use SerratedJSInterop: **wrapping a JS type** in a C# type that implements `IJSObjectWrapper<T>`, or **calling JS on raw `JSObject` references** without a dedicated wrapper. In both cases you avoid hand-written [JSImport] and shims for each member.

**Note:** `System.Runtime.InteropServices.JavaScript.JSObject` here is not the same as `Microsoft.JSInterop.IJSObjectReference` / Blazor’s JSObject. The platform packages may provide helpers to convert between them when needed.

### Instance wrapper

Implement `IJSObjectWrapper<YourType>` and expose a `JSObject`. Use `SerratedJS.New("JsTypeName")` for parameterless construction, or `SerratedJS.New("JsTypeName", SerratedJS.Params(...))` with arguments. Use `this.GetProperty<T>()`, `this.SetProperty(value)`, and `this.CallJS<T>(SerratedJS.Params(...))` so the framework infers the JS member name from the C# member name.

```csharp
public class Image : IJSObjectWrapper<Image>
{
    static Image IJSObjectWrapper<Image>.WrapInstance(JSObject jsObject) => new Image(jsObject);
    public JSObject JSObject => jsObject;
    private JSObject jsObject;

    public Image() { jsObject = SerratedJS.New(nameof(Image)); }
    public Image(JSObject jsObject) { this.jsObject = jsObject; }

    public string Src { get => this.GetProperty<string>(); set => this.SetProperty(value); }
    public int Width { get => this.GetProperty<int>(); set => this.SetProperty(value); }
    public int Height { get => this.GetProperty<int>(); set => this.SetProperty(value); }
    public int NaturalWidth => this.GetProperty<int>();
    public int NaturalHeight => this.GetProperty<int>();
    public bool Complete => this.GetProperty<bool>();
}
```

You can also call methods with an explicit name: `this.CallJS<HtmlElement>("createElement", "div")` or `this.CallJS("setAttribute", name, value)` for void methods.

### Inferred Member Names

Extension methods such as `GetProperty<T>()`, `SetProperty(value)`, and `CallJS<T>(SerratedJS.Params(...))` have overloads which use the calling C# member name to infer the JS property or method name via [CallerMemberName].  Additional overloads allow the function/property names to be specified explicitly as needed. 

TODO: Examples of inferred and explicit names for properties and methods.


### Operating on JSObject

Wrappers are not required.  Extension methods are available for System.Runtime.InteropServices.JavaScript.JSObject can be used directly for ad-hoc interop without defining a wrapper type.  Use `JSObject.GetProperty<T>()`, `JSObject.SetProperty(value)`, and `JSObject.CallJS<T>(SerratedJS.Params(...))` to interact with the JS instance.  The same inferred member name overloads are available on JSObject as well.

TODO: Example of New and operations on JSObject.  Also show a non-IJSObjectWrapper that has a JSObject instance but still infers member names.

### Passing Parameters

Leveraging both `[CallerMemberName]` and `params` in the same method overload presents challenges.  To overcome these, SerratedJSInterop requires a `SerratedJS.Params(...)` helper when using overloads that infer member names from caller.  

This is not required when using extension method overloads where the member name is passed explicitely.

TODO: Example of both approaches.

### Passing POCO/POJO/Literal-Only Objects

Some JS API's may require data be passed in an object structure such as an `options` object.  To avoid multiple interop calls to set properties or define a wrapper for simple data-only objects, SerratedJSInterop provides the `MarshalAsJson()` extension method which serializes a .NET POCO via JSON and then deserializes it when invoking the JS member.  In the below example we use simple anonymous types to declare data only objects and marshal them via JSON serialization for a constructor and also a member access:

```csharp
var evt = SerratedJS.New("CustomEvent", 
  SerratedJS.Params("myevent", 
    new { detail = 42 }.MarshalAsJson() // 2nd param pass via JSON
    ));

element.SetProperty(
    new { is = "my-element" }.MarshalAsJson(), "options");
```

### Singleton

There are different approaches to implementing singletons or static interop wrappers, depending on the preference of the implementor.  The below demonstrates a combination of approaches.  The type could either be accessed statically via `Document.GetDocument()` or registered with DI of choice to be injected as/where needed.  

`Lazy<>` is used to ensure no interop occurs until used, and is not accessed "too early" in the page's life cycle.

```csharp
public class Document : IJSObjectWrapper
{
    public static Document GetDocument() => new Document();
    static readonly Lazy<JSObject> _document = new(() => JSHost.GlobalThis.GetProperty<JSObject>("document"));
    public JSObject JSObject => _document.Value;

    public HtmlElement Body => this.GetProperty<HtmlElement>();
    public HtmlElement CreateElement(string tagName) => this.CallJS<HtmlElement>(SerratedJS.Params(tagName));
    public HtmlElement GetElementById(string id) => this.CallJS<HtmlElement>(SerratedJS.Params(id));
}
```

### Static / global wrapper

An example of a pure static wrapper.

```csharp
public static class GlobalJS
{
    public static class Console
    {
        static readonly Lazy<JSObject> _console = new(() => JSHost.GlobalThis.GetProperty<JSObject>("console"));

        public static void Log(params object[] parameters) =>
            _console.Value.CallJS(SerratedJS.Params(parameters));
    }
}
```

## Additional Examples

- SerratedDom - TODO: Link to SerratedDom folder
- SerratedJQ - TODO: Link once migrated to new library
- Unit Tests - TODO: Link to unit tests, but note that these n some cases leverage internal methods for test setup. 

## Security Considerations

Do not pass user-controlled/sourced strings to `funcName`, `propertyName`, nor `typePath` parameters.  These parameters should only be populated with strings determined at compile time, typically via string literals, `nameof()`, or leveraging overloads which infer via `[CallerMemberName]`.

This ensures no potential for XSS to occur where a user supplied string could invoke an unexpected function, property, or constructor.  Keep in mind "user supplied" could indirectly mean a value retrieved from a DB or API which may have originated at some point in the past from a user.

## Release Notes

_(Release notes will be added here when the library is published to NuGet.)_
