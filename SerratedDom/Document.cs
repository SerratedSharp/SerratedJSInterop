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

    public HtmlElement Body => this.GetProperty<HtmlElement>();

    public HtmlElement CreateElement(string tagName)
        => this.CallJS<HtmlElement>(SerratedJS.Params(tagName));
    
    public HtmlElement GetElementById(string id)
        => this.CallJS<HtmlElement>(SerratedJS.Params(id));
}
