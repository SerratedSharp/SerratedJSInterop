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

    public string Id
    {
        get => this.GetProperty<string>();
        set => this.SetProperty(value);
    }

    public string ClassName
    {
        get => this.GetProperty<string>();
        set => this.SetProperty(value);
    }

    public HtmlElement? ParentElement => this.GetProperty<HtmlElement?>();
    public HtmlElement? FirstElementChild => this.GetProperty<HtmlElement?>();
    public HtmlElement? LastElementChild => this.GetProperty<HtmlElement?>();
    public int OffsetWidth => this.GetProperty<int>();
    public int OffsetHeight => this.GetProperty<int>();
    public int ClientWidth => this.GetProperty<int>();
    public int ClientHeight => this.GetProperty<int>();

    // CallJS with inferred function name via [CallerMemberName]
    public void AppendChild(IJSObjectWrapper child)
        => this.CallJS(SerratedJS.Params(child.JSObject));

    // CallJS with IJSObjectWrapper<T> return, automatically wrapping the JSObject result in HtmlElement
    public HtmlElement RemoveChild(HtmlElement child)
        => this.CallJS<HtmlElement>("removeChild", child); 
     
    public void SetAttribute(string name, string value)
        => this.CallJS("setAttribute", name, value); // CallJS with void return

    public string GetAttribute(string name)
        => this.CallJS<string>("getAttribute", name); // CallJS with string return

    public string TextContent
    {
        get => this.GetProperty<string>("TextContent");
        set => this.SetProperty(value, "TextContent");
    }

    public string InnerHtml
    {
        get => this.GetProperty<string>("InnerHtml");
        set => this.SetProperty(value, "InnerHtml");
    }

    public string OuterHtml
    {
        get => this.GetProperty<string>("OuterHtml");
        set => this.SetProperty(value, "OuterHtml");
    }
}
