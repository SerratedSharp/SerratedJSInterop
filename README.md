# SerratedJSInterop

A library to simplify .NET WebAssembly interop with JavaScript when using `System.Runtime.InteropServices.JavaScript` and [JSImport]. It reduces or eliminates the need for explicit JavaScript shims and per-method [JSImport] declarations. This eases adhoc JS interop and reduces boilerplate code for C# wrappers.

> **Work in progress:** The API is being refined and will be published to NuGet when ready.

## Brief Example

This example demonstrates wrapping a JS type which performs interop for methods and properties.  This allows the creation of clean wrappers for JS types/libraries with minimal effort: 

```csharp
public class Audio : IJSObjectWrapper<Audio>
{    
    public JSObject JSObject { get; } // Handle to underlying JS object reference

    public Audio() {
         JSObject = SerratedJS.New(nameof(Audio)); // Interop to JS New constructor
    }
    
    public Audio(JSObject jsObject) {
        JSObject = jsObject; // Wrap an existing known JSObject reference
    }

    // Getter/setter with inferred JS property name
    public string Src
    {
        get => this.GetJSProperty<string>();
        set => this.SetJSProperty(value);
    }

    // JS property name inferred from C# member name "Duration" => "duration"
    public double Duration => this.GetJSProperty<double>();

    // Explicit JS property name specified
    public bool IsPaused => this.GetJSProperty<bool>("paused");        
    
    // Auto-map return to a custom C#/JS wrapper DomTokenList, which implements IJSObjectWrapper<J>
    public DomTokenList ControlsList => this.GetJSProperty<DomTokenList>();
    
    // Explicit name specified with `funcName:` and variable parameters
    // void return uses CallJS() instead of CallJS<T>()
    public void AddTextTrack(string kind, string label, string language)
        => this.CallJS(funcName: "addTextTrack", kind, label, language);
    
    // Inferred name with parameter, void return
    public void CanPlayType(string type) => this.CallJS(type);

    // Map return to a native JSObject instead of requesting a wrapped type
    public JSObject CaptureStream() => this.CallJS<JSObject>();
        
    // IJSObjectWrapper<Audio> utility method that allows library to auto-wrap JSObject's as requested.
    // Permits other methods to return this type such as GetJSProperty<Audio>() or CallJS<Audio>()
    static Audio IJSObjectWrapper<Audio>.WrapInstance(JSObject jsObject) => new Audio(jsObject);
}
```

Alternatively, adhoc interop without wrapper is possible on InteropServices JSObject:

```csharp
var spanJSO = documentJS.CallJS<JSObject>(funcName: "createElement", "span");
```

Both approaches can be mixed, with wrappers being returned from adhoc interop as needed.  This allows you to tailor the granularity and scope of type wrappers to your liking, seamlessly switching between working with JSObject and custom wrappers as desired.  It also allows easy access to native .NET interop methods without cumbersome inheritance hierarchies.  Each line of the following represents an interop operation, either through a wrapped interop member or directly through JSObject.

```csharp
// Wrapped method & accessor
HtmlElementWrapper span = documentWrapper.CreateElement("span");
int offsetWidth = span.OffsetWidth;
var spanJSObject = span.JSObject; // Get native JSObject
spanJSObject.SetJSProperty("Hello world", "textContent"); // Adhoc interop on JSObject
// Request return be wrapped with custom DomTokenListWrapper
DomTokenListWrapper classesList = spanJSObject.GetJSProperty<DomTokenListWrapper>("classList");
bool hasClass = classesList.Contains("my-class"); // custom wrapper method
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
- Use `this.GetJSProperty<T>()`, `this.SetJSProperty(value)`, `this.CallJS<T>(...)`, and `this.CallJS(...)` to map properties/methods to the underlying JSObject reference.
- Use `this.CallJS<T>(funcName:"someJSMethodName",...)`, `GetJSProperty<string>("someJSName")`, and `SetJSProperty(someValue, "someJSName")` to specify JS member names explicitly.

```csharp
public class Image : IJSObjectWrapper<Image>
{
    static Image IJSObjectWrapper<Image>.WrapInstance(JSObject jsObject) => new Image(jsObject);
    public JSObject JSObject => jsObject;
    private JSObject jsObject;

    public Image() { jsObject = SerratedJS.New(nameof(Image)); }
    public Image(JSObject jsObject) { this.jsObject = jsObject; }

    public string Src { get => this.GetJSProperty<string>(); set => this.SetJSProperty(value); }
    public int Width { get => this.GetJSProperty<int>(); set => this.SetJSProperty(value); }
    public int Height { get => this.GetJSProperty<int>(); set => this.SetJSProperty(value); }
    public int NaturalWidth => this.GetJSProperty<int>();
    public int NaturalHeight => this.GetJSProperty<int>();
    public bool Complete => this.GetJSProperty<bool>();
        
    // If we implemented a DOMRect wrapper, we could wrap the JSObject automatically with this.CallJS<DomRectWrapper>() 
    public JSObject GetBoundingClientRect() => this.CallJS<JSObject>();

    public void RemoveAttribute(string name) => this.CallJS(name);
    public void Focus() => this.CallJS();
    public void Blur() => this.CallJS();
}
```

### `IJSObjectWrapper<T>`

As shown in later examples, `IJSObjectWrapper<T>` isn't strictly required.  However when implemented, `IJSObjectWrapper<T>` provides the framework the means to automatically wrap a JSObject reference with your C# wrapper type by calling its `.WrapInstance()`.  This allows calls to `.GetJSProperty<J>()` and `.CallJS<J>()` to specify your wrapper `J` as the return type.  These internally call your implementation of .WrapInstance() to instantiate `J` from a `JSObject`.  

> [!NOTE] 
> There's no strict type checking of the JS type, and runtime errors will occur later in the object's lifecycle when interacting with an incorrectly mapped type.  E.g. calling `GetJSProperty<HtmlElement>("firstNode")` where the JS API might return a `Node` type rather than strictly `HtmlElement`.  Later attempts to access non-existent members on the incorrectly wrapped type likely fail with confusing errors. It's expected that the implementer uses knowledge of the JS APIs they're wrapping to map types appropriately.

### Inferred Member Names

Extension methods such as `GetJSProperty<T>()`, `SetJSProperty(value)`, and `CallJS<T>(...)` have overloads which use the calling C# member name to infer the JS property or method name via [CallerMemberName].  Additional overloads allow the function/property names to be specified explicitly as needed.

The first letter is lowercased for consistency with common JS lowerCamelCase conventions.  (Note: This lower casing convention is not applied for the SerratedJS.New() interop operator, nor for overloads passing explicit names via `funcName:` and `propertyName:`.)

```csharp
// Property getter "Body" infers JS property "body"
public HtmlElement Body => this.GetJSProperty<HtmlElement>();

// Inferred "CreateElement" -> "createElement"
public HtmlElement CreateElement(string tagName) 
  => this.CallJS<HtmlElement>(tagName);

// Infers "id"
public string Id { 
  get => this.GetJSProperty<string>(); 
  set => this.SetJSProperty(value); 
}

// Infers "appendChild"
public void AppendChild(IJSObjectWrapper child) 
  => this.CallJS(child.JSObject);
```

> [!NOTE] 
> **Pitfall:** Forgetting to specify an explicit name for adhoc interop where the parent name is not applicable.  For example, `void RunTest() => jsObject.GetJSProperty<int>()` would attempt to access property named "runTest" when the caller likely intended `jsObject.GetJSProperty<int>("someJSPropertyName")` 

**Explicit names:** JS names can be specified explicitly instead of being inferred. When calling `CallJS` with an explicit function name, you must use the **named parameter** `funcName:` (see [CallJS: explicit vs inferred name](#calljs-explicit-vs-inferred-name) below):

```csharp
public HtmlElement Head => this.GetJSProperty<HtmlElement>("head");

public HtmlElement? QuerySelector(string selector)
  => this.CallJS<HtmlElement?>(funcName: "querySelector", selector);

public string TextContent { 
  get => this.GetJSProperty<string>("textContent"); 
  set => this.SetJSProperty(propertyName: "textContent", value); 
}

public void SetAttribute(string name, string value) 
  => this.CallJS(funcName: "setAttribute", name, value);
```

> [!NOTE] 
> **Pitfall:** When specifying explicit property name for SetJSProperty, then the value comes first with the property name second.

### Operating on JSObject

IJSObjectWrapper is not required. Extension methods are also defined on `System.Runtime.InteropServices.JavaScript.JSObject`. 

These can be used for adhoc interop (use `funcName:` when specifying the JS function name explicitly):
```csharp
var doc = Document.GetDocument();
var spanJSObject = doc.JSObject.CallJS<JSObject>(funcName: "createElement", "span");
var tagName = spanJSObject.GetJSProperty<string>("tagName");
doc.Body.JSObject.CallJS(funcName: "appendChild", spanJSObject);
```

This is also useful for navigating through children where implementing a full wrapper isn't desired:

```
public class Document
{
    ...
    public int BodyOffsetWidth 
      => this.JSObject.GetJSProperty<JSObject>("body").GetJSProperty<int>("offsetWidth");
```

These can be used for an alternative approach to wrapping types without IJSObjectWrapper: 

```csharp
public sealed class DomTokenList
{
    private readonly JSObject jsObject;
    public DomTokenList(JSObject jsObject) { this.jsObject = jsObject; }

    public int Length => jsObject.GetJSProperty<int>();
    public string Item(int index) => jsObject.CallJS<string>(index);
    public bool Contains(string token) => jsObject.CallJS<bool>(funcName: "contains", token);
    public void Add(string token) => jsObject.CallJS(token);
    public void Remove(string token) => jsObject.CallJS(funcName: "remove", token);
}
```

When other type's members wish to return this type, they will need to perform the wrapping explicitly:

```csharp
public class SomeOther {
    public DomTokenList GetTokenList() {
      var jsoTokenList = this.JSObject.CallJS<JSObject>("getTokenList");
      return new DomTokenList(jsoTokenList);
    }
}
```

### Passing Parameters

Parameters can be a mix of primitives/literals(assuming supported by interop), InteropServices JSObject, and IJSObjectWrapper.  Overloads are provided to support 0 thru 5 parameters when used in combination with inferred names via `[CallerMemberName]`.

For additional parameters, bundle them in SerratedJS.Params(...), or use the `funcName:` explicit name pattern which supports unlimited parameters via `params` keyword.  (This nuance is due to challenges of leveraging both `[CallerMemberName]` and `params` in the same overload.)    

This demonstrates two techniques that could be used if more than 5 parameters are needed:

```csharp
public HtmlElement InsertBefore(HtmlElement newChild, HtmlElement? referenceChild)
    => this.CallJS<HtmlElement>(
        SerratedJS.Params(newChild.JSObject, referenceChild?.JSObject, "3", 4, 5.0f));
```

When function names are specified explicitly, use the **`funcName:`** named parameter and SerratedJS.Params can be optionally omitted.  The `funcName:` param must be named explicitly to ensure the appropriate overload is selected:

```csharp
var el = doc.CallJS<HtmlElement?>(funcName: "querySelector", ".container");
element.CallJS(funcName: "setAttribute", "data-foo", "bar");
body.CallJS(funcName: "appendChild", div);
parent.CallJS<HtmlElement>(funcName: "insertBefore", child1, child2, "3", 4, 5.0f);
```

### CallJS: Explicit vs Inferred Name

`CallJS` has overloads that **infer** the JS function name from the calling C# member ([CallerMemberName]) and overloads that take an **explicit** function name. When you pass a single string argument, the compiler cannot tell whether that string is the function name (explicit call with zero args) or the first parameter (inferred-name call). To always select the explicit-name overload, use the **named parameter** `funcName:`:

```csharp
// Inferred name: C# method name → JS function name; "html" is the first argument
public JQueryPlainObject Append(string html) => this.CallJS<JQueryPlainObject>(html);

// Explicit name: call JS function "log" with no arguments
console.CallJS(funcName: "log");

// Explicit name: call JS function "createElement" with one argument
var div = document.CallJS<HtmlElement>(funcName: "createElement", "div");
```

Using `funcName:` is required when calling with an explicit name and zero parameters, and is recommended for all explicit-name calls for clarity and to avoid overload ambiguity.

### Passing POCO/POJO/Literal-Only Objects

Some JS API's may require data be passed in an object structure such as an `options` object.  To avoid multiple interop calls to set properties or avoid defining a wrapper for simple data-only objects, SerratedJSInterop provides the `MarshalAsJson()` extension method which serializes a .NET POCO via JSON and then deserializes it when invoking the JS member.  In the below example we use simple anonymous types to declare data only objects and marshal them via JSON serialization for a constructor and also a member access:

```csharp
var evt = SerratedJS.New("CustomEvent", 
    "myevent", 
    new { detail = 42 }.MarshalAsJson() // 2nd param pass via JSON
  );

element.SetJSProperty(
    new { is = "my-element" }.MarshalAsJson(), "options");
```

This does not require an extra roundtrip. The payload is serialized in .NET and the payload is part of the existing interop call. Once the call is beyond the JS boundary, then the parameter is deserialized into a native JS object just before invoking the JS method.

Note JSObject references would not be preserved across such deserialization.  This approach is only appropriate for data-only objects where the properties are primitives or simple serializable types.  For objects with JSObject references, it's recommended to define a wrapper type and use the standard interop patterns.

### Singleton

There are different approaches to implementing singletons or static interop wrappers, depending on the preference of the implementor.  The below demonstrates a combination of approaches.  The type could either be accessed statically via `Document.GetDocument()` or registered with DI of choice to be injected as/where needed.  

`Lazy<>` is used to ensure no interop occurs until used, and is not accessed "too early" in the page's life cycle.

```csharp
public class Document : IJSObjectWrapper
{
    public static Document GetDocument() => new Document();
    static readonly Lazy<JSObject> _document = new(() => JSHost.GlobalThis.GetJSProperty<JSObject>("document"));
    public JSObject JSObject => _document.Value;

    public HtmlElement Body => this.GetJSProperty<HtmlElement>();
    public HtmlElement CreateElement(string tagName) => this.CallJS<HtmlElement>(tagName);
    public HtmlElement GetElementById(string id) => this.CallJS<HtmlElement>(id);
}
```

### Static/Global Wrapper

An example of a pure static wrapper.

```csharp
public static class GlobalJS
{
    public static class Console
    {
        static readonly Lazy<JSObject> _console = new(() => JSHost.GlobalThis.GetJSProperty<JSObject>("console"));

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


This project is a migration of SerratedSharp.JSInteropHelpers previously used internally for other projects, with SerratedJSInterop formalized for broader use.