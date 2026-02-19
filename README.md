
# Work in Progress

This is a migration of SerratedSharp.JSInteropHelpers which was used internally for other projects, but never formalized for wider consumption.  This project is actively being refined to finalize the API and will be published to NuGet when ready.

# SerratedJSInterop Overview

Reduces or eliminates need for explicit JavaScript shims and [JSImport] declarations when using the [JSImport]/System.Runtime.InteropServices.JavaScript flavor of .NET/JavaScript interop. Simplifies interop by reducing amount of boilerplate code needed when implementing JavaScript type wrappers or calling native JavaScript.

# Initialization/Startup

Typically you will not reference the SerratedJSInterop Nuget package directly, but instead reference one of the platform-specific packages that in turn depend on this package, such as: 

- SerratedSharp.SerratedJSInterop.Blazor, for Standalone Blazor WebAssembly projects.
- SerratedSharp.SerratedJSInterop.WasmBrowser, for .NET WebAssembly Browser Apps (from the wasm-experimental workload).
- SerratedSharp.SerratedJSInterop.Uno, for .NET Uno Platform projects targeting WebAssembly.

(TODO: Make list items into links to readme's)

These platform-specific packages provide some necessary initialization code and in some cases helpers.  For example, the Blazor package provides additional utilities for marshalling IJSRuntime JSObjects to InteropServices JSObjects.

# Usage

There are two typical ways to leverage SerratedJSInterop.  One is to aid in the creation of type wrappers, and the other is to aid in accessing JS objects without declaring an explicit wrapper.  In both cases, SerratedJSInterop reduces or eliminates the need for [JSImport] declarations and typical JavaScript shims that would otherwise be needed in these scenarios.

Note: System.Runtime.InteropServices.JavaScript.JSObject used below differs from IJSRuntime's JSObject more commonly found in Blazor.  These are distinct types, but can be marshalled between one another with aid of interop helpers.

## Instance Wrapper

```csharp
public class Image : IJSObjectWrapper<Image>
{
    // IJSObjectWrapper: Allows framework to automatically box JSObject's into your wrapper.  
    // Works in concert with methods such as .CallJS<Image>()
    static Image IJSObjectWrapper<Image>.WrapInstance(JSObject jsObject) => new Image(jsObject);

    // IJSObjectWrapper.JSObject: Handle to System.InteropServices JSObject reference.
    // This determines what JS instance our .NET type wraps.
    // It is also the instance that helpers such as this.CallJS<> and Get/SetProperty will operate on.
    public JSObject JSObject => jsObject;
    private JSObject jsObject;

    // Interop call to JS New constructor
    public Image() 
    {
        JSObject = SerratedJS.New(nameof(Image));
        // Equivalent of: SerratedJS.New("Image"));
    }
        
    // Optional constructor.  Aids in wrapping existing Image JSObject reference.
    public Image(JSObject jsObject)
    {
        JSObject = jsObject;
    }

    // Map properties to JS interop property
    public string Src
    {
        // Infers property name by parent properties name via [CallerMemberName], and is converted to lowerCamelCase "Src" => "src"
        get => this.GetProperty<string>();
        set => this.SetProperty(value);
    }

    public int Width
    {
        get => this.GetProperty<int>();        
        set => this.SetProperty(value);

        // Property name can be specified explicitly:
        // get => this.GetProperty<int>("width");
        // set => this.SetProperty("width", value);
    }

    public int Height
    {
        get => this.GetProperty<int>();
        set => this.SetProperty(value);
    }

    public int NaturalWidth => this.GetProperty<int>();
    public int NaturalHeight => this.GetProperty<int>();
    public bool Complete => this.GetProperty<bool>();
     
}
```

## Singleton Wrapper

There are different approaches to implementing singletons or static interop wrappers, depending on the preference of the implementor.  The below demonstrates a type that could either be accessed statically via `GetDocument()` or registered with DI to be injected as/where needed.  `Lazy<>` is used to ensure no interop occurs if it is unused, and is not accessed "too early" in the page's life cycle during initialization.  This is just one example, and SerratedJSInterop doesn't tie you to a particular approach for managing singletons.

```csharp
// Wraps the global window.document
public class Document:IJSObjectWrapper
{
    public static Document GetDocument() => new Document(); // Optional global static accessor

    // Get reference on first attempted usage, stored as a static shared instance.
    static readonly Lazy<JSObject> _document = new(() => JSHost.GlobalThis.GetProperty<JSObject>("document"));
    
    // Map per instance property to shared static instance, forcing singleton behavior.
    public JSObject JSObject => _document.Value;

    public HtmlElement Body => this.GetProperty<HtmlElement>();

    public HtmlElement CreateElement(string tagName)
        => this.CallJS<HtmlElement>(SerratedJS.Params(tagName));
    
    public HtmlElement GetElementById(string id)
        => this.CallJS<HtmlElement>(SerratedJS.Params(id));
}
```

## Static Wrapper

A purely static wrapper.

```
public static class GlobalJS
{
    public static class Console
    {
        static Lazy<JSObject> _console = new(() => JSHost.GlobalThis.GetProperty<JSObject>("console"));

        /// <summary>
        /// console.log(...)
        /// </summary>
        public static void Log(params object[] parameters)
        {
            _console.Value.CallJS(SerratedJS.Params(parameters));
        }
    }
}
```