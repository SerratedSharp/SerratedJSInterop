using System.Runtime.InteropServices.JavaScript;
using System.Runtime.Versioning;
using SerratedSharp.SerratedJSInterop;

namespace SerratedSharp.SerratedDom;

/// <summary>
/// Wraps the global window.document. Can be retrieved via static GetDocument(), `new Document()`, or registered in DI as a singleton.
/// All Document instances share the same underlying JS reference.
/// </summary>
[SupportedOSPlatform("browser")]
public class Document:IJSObjectWrapper
{
    public static Document GetDocument() => new Document();
    static readonly Lazy<JSObject> _document = new(() => JSHost.GlobalThis.GetProperty<JSObject>("document"));
    public JSObject JSObject => _document.Value;
    
    public HtmlElement Body => this.GetProperty<HtmlElement>(); // Name inferred with [CallerMemberName]
    public HtmlElement DocumentElement => this.GetProperty<HtmlElement>(nameof(DocumentElement)); // Name provided explicitly, but derived from nameof()
    public HtmlElement Head => this.GetProperty<HtmlElement>("head");// Explicit name
    public string DocumentURI => this.GetProperty<string>();
    public string CharacterSet => this.GetProperty<string>();


    public HtmlElement CreateElement(string tagName)
        => this.CallJS<HtmlElement>(SerratedJS.Params(tagName));

    // Optionally provide explicit function name.
    public HtmlElement? QuerySelector(string selector)
        => this.CallJS<HtmlElement?>("querySelector", selector);

    public HtmlElement GetElementById(string id)
        => this.CallJS<HtmlElement>(SerratedJS.Params(id));
}
