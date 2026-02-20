# SerratedJSInterop

A class library to simplify creating .NET WebAssembly wrappers on JavaScript instances when using `System.Runtime.InteropServices.JavaScript` and [JSImport]. It reduces or eliminates the need for explicit JavaScript shims and per-method [JSImport] declarations, so you can map C# types to JS instances with minimal boilerplate. This project is a migration and formalization of SerratedSharp.JSInteropHelpers for broader use.

> **Work in progress:** The API is being refined and will be published to NuGet when ready.

## Brief Example

This example demonstrates wrapping a JS type which performs interop for methods and properties.  This allows the creation of clean wrappers for JS types/libraries with minimal effort: 

```csharp
public class Audio : IJSObjectWrapper<Audio>
{    
    public JSObject JSObject { get; }

    public Audio() {
         JSObject = SerratedJS.New(nameof(Audio)); // interop to JS New constructor
    }
    
    public Audio(JSObject jsObject) {
        JSObject = jsObject;
    }

    public string Src
    {
        get => this.GetProperty<string>();
        set => this.SetProperty(value);
    }

    public double Duration => this.GetProperty<double>();
    public bool IsPaused => this.GetProperty<bool>("paused");        
    // Returns automatically wrapped by types implementing IJSOjbectWrapper<J>
    public DomTokenList ControlsList => this.GetProperty<DomTokenList>();
    public void AddTextTrack(string kind, string label, string language)
        => this.CallJS("addTextTrack", kind, label, language);
    public void CanPlayType(string type) => this.CallJS(type);
    public JSObject CaptureStream() => this.CallJS<JSObject>();
        
    static Audio IJSObjectWrapper<Audio>.WrapInstance(JSObject jsObject) => new Audio(jsObject);
}
```

## Prerequisites

- .NET 9 or later (or the target framework supported by the platform package you use).

- A project that compiles to WebAssembly: 
  - A WebAssembly Browser App project created according to [JavaScript `[JSImport]`/`[JSExport]` interop with a WebAssembly Browser App project](https://learn.microsoft.com/en-us/aspnet/core/client-side/dotnet-interop/wasm-browser-app?view=aspnetcore-8.0).
  - A Blazor client-side project created according to [JavaScript JSImport/JSExport interop with ASP.NET Core Blazor](https://learn.microsoft.com/en-us/aspnet/core/blazor/javascript-interoperability/import-export-interop?view=aspnetcore-8.0).
    - Note: `System.Runtime.InteropServices.JavaScript` is used for SerratedJSInterop.  This differs from the typical Blazor IJSRuntime, but can operate side-by-side within Blazor WASM.
  - A WASM framework/platform that supports [JSImport]/[JSExport] interop (System.Runtime.InteropServices.JavaScript), such as Uno Platform WASM.  
    - (Note: As of the newest migration, validation of compatibility with Uno has not been completed.  Anyone willing to do so please see: [Issue #33](https://github.com/SerratedSharp/SerratedJSInterop/issues/33))


## Quick Start

You typically reference a **platform-specific package** through NuGet that depends on SerratedJSInterop, rather than the core package directly:

- **SerratedSharp.SerratedJSInterop.Blazor** — Standalone Blazor WebAssembly projects.
- **SerratedSharp.SerratedJSInterop.WasmBrowser** — .NET WebAssembly Browser Apps (i.e. wasm-experimental workload).
- **SerratedSharp.SerratedJSInterop.Uno** — Uno Platform projects targeting WebAssembly.

1) Add a reference to one of the above platform-specific packages via NuGet.
2) Add the following call to Program.Main() to load the JS module:

```csharp
await SerratedJSInteropModule.ImportAsync();
```

This assumes your app is rooted at the domain.  If your site is hosted at a subpath such as `example.com/myapp/`, you will need to specify the base URL for the JS module:

```csharp
await SerratedJSInteropModule.ImportAsync("/myapp");
```

In some cases the context of the WASM runtime loader is initialized from a subpath of the site, and would require the following:
```csharp
await SerratedJSInteropModule.ImportAsync("..");
```

The NuGet package leverages RCL format for bundling static JS assets, and this method will load the module from `/_content/SerratedSharp.SerratedJSInterop/SerratedJSInteropShim.js`.

## Usage

There are two main ways to use SerratedJSInterop: wrapping a JS type with C# class implementing `IJSObjectWrapper<T>` or calling extensions on `JSObject` references. In both cases, you avoid implementing boilerplate code for [JSImport] declarations and JavaScript shims.

> [!NOTE] 
> This library leverages newer `System.Runtime.InteropServices.JavaScript` for interop, which differs from the typical Blazor `IJSRuntime`.  Both can operate side-by-side within a Blazor WASM project, and this library provides utilities for marshalling between the distinct `JSObject` types.

### Instance Wrapper

- Implement `IJSObjectWrapper<YourType>` and expose a `JSObject`. 
- Implement the required static WrapInstance method, which is leveraged by the library to automatically wrap returned instances for calls such as `CallJS<YourType>()`.
- Use `SerratedJS.New("JsTypeName")` for parameterless construction, or `SerratedJS.New("JsTypeName", "param1", 2, someJSobject3)` with variable arguments. 
- Use `this.GetProperty<T>()`, `this.SetProperty(value)`, and `this.CallJS<T>(SerratedJS.Params(...))` to map properties/methods to the underlying JSObject reference.

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

### `IJSObjectWrapper<T>`

As shown in later examples, `IJSObjectWrapper<T>` isn't strictly required.  However when implemented, `IJSObjectWrapper<T>` provides the framework the means to automatically wrap a JSObject reference with your C# wrapper type by calling its `.WrapInstance()`.  This allows calls to `.GetProperty<J>()` and `.CallJS<J>()` to specify your wrapper `J` as the return type.  These internally calls your implementation of .WrapInstance() to instantiate `J` from a `JSObject`.  

> [!NOTE] 
> There's no strict type checking of the JS type, and runtime errors will occur later in the object's lifecycle when interacting with an incorrectly mapped type.  E.g. calling `GetProperty<HtmlElement>("firstNode")` where the JS API might return a `Node` type rather than strictly `HtmlElement`.  Later attempts to access non-existant members on the incorrectly wrapped type likely fail with confusing errors. It's expected that the implementer uses knowledge of the JS APIs they're wrapping to map types appropriately.

### Inferred Member Names

Extension methods such as `GetProperty<T>()`, `SetProperty(value)`, and `CallJS<T>(SerratedJS.Params(...))` have overloads which use the calling C# member name to infer the JS property or method name via [CallerMemberName].  Additional overloads allow the function/property names to be specified explicitly as needed.

The first letter is lowercased for consistency with common JS lowerCamelCase conventions.  (Note: This lower casing convention is not applied for the SerratedJSNew() interop operator.)

```csharp
// Property getter "Body" infers JS property "body"
public HtmlElement Body => this.GetProperty<HtmlElement>();

// Inferred name and passing parameters via JSParams
public HtmlElement CreateElement(string tagName) 
  => this.CallJS<HtmlElement>(SerratedJS.Params(tagName));

public string Id { 
  get => this.GetProperty<string>(); 
  set => this.SetProperty(value); 
}

public void AppendChild(IJSObjectWrapper child) 
  => this.CallJS(SerratedJS.Params(child.JSObject));
```

**Explicit names:** JS names can be specified explicitely isntead of being inferred:

```csharp
public HtmlElement Head => this.GetProperty<HtmlElement>("head");

public HtmlElement? QuerySelector(string selector)
  => this.CallJS<HtmlElement?>("querySelector", selector);

public string TextContent { 
  get => this.GetProperty<string>("TextContent"); 
  set => this.SetProperty(value, "TextContent"); 
}

public void SetAttribute(string name, string value) 
  => this.CallJS("setAttribute", name, value);
```

> [!NOTE] 
> **Pitfall:** When specifying explicit property name for SetProperty, then the value comes first with the property name second.  Consider using explicit parameter names to avoid confusion: `this.SetProperty(propertyName: "TextContent", value)`.

### Operating on JSObject

IJSObjectWrapper is not required. Extension methods are also defined on `System.Runtime.InteropServices.JavaScript.JSObject`. 

These can be used for adhoc interop:
```csharp
var doc = Document.GetDocument();
var spanJSObject = doc.JSObject.CallJS<JSObject>("createElement", "span");
var tagName = spanJSObject.GetProperty<string>("tagName");
doc.Body.JSObject.CallJS("appendChild", spanJSObject);
```

This is also useful for navigating through children where implementing a full wrapper isn't desired:

```
public class Document
{
    ...
    public int BodyOffsetWidth 
      => this.JSObject.GetProperty<JSObject>("body").GetProperty<int>("offsetWidth");
```

These can be used for an alternative approach to wrapping types without IJSObjectWrapper: 

```csharp
public sealed class DomTokenList
{
    private readonly JSObject _js;
    public DomTokenList(JSObject jsObject) { _js = jsObject; }

    public int Length => _js.GetProperty<int>();
    public string Item(int index) => _js.CallJS<string>(SerratedJS.Params(index));
    public bool Contains(string token) => _js.CallJS<bool>("contains", SerratedJS.Params(token));
    public void Add(string token) => _js.CallJS(SerratedJS.Params(token));
    public void Remove(string token) => _js.CallJS("remove", SerratedJS.Params(token));
}
```

### Passing Parameters

Leveraging both `[CallerMemberName]` and `params` in the same method overload presents challenges. To overcome these, SerratedJSInterop requires a `SerratedJS.Params(...)` helper when using `CallJS` overloads that infer the member name from the caller.  This allows a variable number of parameters while also supporting `[CallerMemberName]`.

SerratedJS.Params can handle a mix of primitives/literals(assuming supported by interop), InteropServices JSObject, and IJSObjectWrapper, such as `SerratedJS.Params("div", jsObject, wrapper, 5)`.

This is required when passing parameters with an inferred caller name:

```csharp
public HtmlElement CreateElement(string tagName)
    => this.CallJS<HtmlElement>(SerratedJS.Params(tagName));

public void AppendChild(IJSObjectWrapper child)
    => this.CallJS(SerratedJS.Params(child.JSObject));

public HtmlElement InsertBefore(HtmlElement newChild, HtmlElement? referenceChild)
    => this.CallJS<HtmlElement>("insertBefore", SerratedJS.Params(newChild.JSObject, referenceChild?.JSObject));
```

When function names are specified explicitly, then SerratedJS.Params can be optionally ommitted:

```csharp
var el = doc.CallJS<HtmlElement?>("querySelector", ".container");
element.CallJS("setAttribute", "data-foo", "bar");
body.CallJS("appendChild", div);
parent.CallJS<HtmlElement>("insertBefore", child1, child2);
```

### Passing POCO/POJO/Literal-Only Objects

Some JS API's may require data be passed in an object structure such as an `options` object.  To avoid multiple interop calls to set properties or define a wrapper for simple data-only objects, SerratedJSInterop provides the `MarshalAsJson()` extension method which serializes a .NET POCO via JSON and then deserializes it when invoking the JS member.  In the below example we use simple anonymous types to declare data only objects and marshal them via JSON serialization for a constructor and also a member access:

```csharp
var evt = SerratedJS.New("CustomEvent", 
    "myevent", 
    new { detail = 42 }.MarshalAsJson() // 2nd param pass via JSON
  );

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

### Static/Global Wrapper

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

- **SerratedDom** — DOM/HTML wrappers (Document, HtmlElement, Image, DomTokenList, Location) that use SerratedJSInterop. See the [SerratedDom](SerratedDom/) folder and [SerratedDom/readme.md](SerratedDom/readme.md). Types like `Document` and `HtmlElement` implement `IJSObjectWrapper`; `DomTokenList` and `Location` use a private `JSObject` with **JSObjectExtensionsV2** only.
- **SerratedJQ** — TODO: Link once migrated to new library.
- **Unit tests** — The [SerratedJSInterop.Tests.Shared](SerratedJSInterop.Tests.Shared/) project contains tests that exercise the extensions (e.g. `JSObjectExtensionsV2/DocumentTests.cs`, `HtmlElementTests.cs`, `DomTokenListTests.cs`, `LocationTests.cs`). Some tests use internal or test-only setup; treat them as usage examples rather than a stable public API. 

## Security Considerations

Do not pass user-controlled/sourced strings to `funcName`, `propertyName`, nor `typePath` parameters.  These parameters should only be populated with strings determined at compile time, typically via string literals, `nameof()`, or leveraging overloads which infer via `[CallerMemberName]`.

This ensures no potential for XSS to occur where a user supplied string could invoke an unexpected function, property, or constructor.  Keep in mind "user supplied" could indirectly mean a value retrieved from a DB or API which may have originated at some point in the past from a user.

## Release Notes

_(Release notes will be added here when the library is published to NuGet.)_
