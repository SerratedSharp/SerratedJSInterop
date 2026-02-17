using System.Runtime.InteropServices.JavaScript;
using System.Runtime.Versioning;
using SerratedSharp.SerratedJSInterop;

namespace SerratedSharp.SerratedDom;

/// <summary>
/// Wraps a DOM HTMLElement for use with Serrated JS interop.
/// </summary>
[SupportedOSPlatform("browser")]
public class HtmlElement : IJSObjectWrapper<HtmlElement>
{
    /// <inheritdoc />
    public static HtmlElement WrapInstance(JSObject jsObject) => new HtmlElement(jsObject);
    /// <inheritdoc />
    public JSObject JSObject => jsObject;

    internal JSObject jsObject;

    /// <summary>
    /// Creates an HTMLElement from an existing InteropServices JSObject.
    /// </summary>
    public HtmlElement(JSObject jsObject)
    {
        this.jsObject = jsObject;
    }

    /// <summary>Appends a child node (e.g. HTMLCanvasElement) to this element.</summary>
    public void AppendChild(IJSObjectWrapper child)
        => this.CallJS<object>(SerratedJS.Params(child.JSObject));

    /// <summary>Gets or sets the raw text content of the element and its descendants.</summary>
    public string TextContent
    {
        get => this.GetProperty<string>("TextContent");
        set => this.SetProperty(value, "TextContent");
    }

    /// <summary>Gets or sets the HTML markup for the element's contents (DOM innerHTML).</summary>
    public string InnerHtml
    {
        get => this.GetProperty<string>("InnerHtml");
        set => this.SetProperty(value, "InnerHtml");
    }

    /// <summary>Gets or sets the HTML markup for the element including the element itself (DOM outerHTML).</summary>
    public string OuterHtml
    {
        get => this.GetProperty<string>("OuterHtml");
        set => this.SetProperty(value, "OuterHtml");
    }
}
