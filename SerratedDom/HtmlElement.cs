using System.Runtime.InteropServices.JavaScript;
using System.Runtime.Versioning;
using SerratedSharp.SerratedJSInterop;

namespace SerratedSharp.SerratedDom;

// Alternatively we could inherit from Node like so:
// public class HtmlElement : Node, IJSObjectWrapper<HtmlElement>
// {
//     public HtmlElement(JSObject jsObject) : base(jsObject) { }

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
        get => this.GetJSProperty<string>();
        set => this.SetJSProperty(value);
    }

    public string ClassName
    {
        get => this.GetJSProperty<string>();
        set => this.SetJSProperty(value);
    }

    public HtmlElement? ParentElement => this.GetJSProperty<HtmlElement?>();
    public HtmlElement? FirstElementChild => this.GetJSProperty<HtmlElement?>();
    public HtmlElement? LastElementChild => this.GetJSProperty<HtmlElement?>();

    /// <summary>Returns whether this element has any child nodes. No parameters; demonstrates CallJS&lt;J&gt;() with inferred name.</summary>
    public bool HasChildNodes() => this.CallJS<bool>();
    public int OffsetWidth => this.GetJSProperty<int>();
    public int OffsetHeight => this.GetJSProperty<int>();
    public int ClientWidth => this.GetJSProperty<int>();
    public int ClientHeight => this.GetJSProperty<int>();

    // CallJS with inferred function name via [CallerMemberName]
    public void AppendChild(IJSObjectWrapper child)
        => this.CallJS(SerratedJS.Params(child.JSObject));

    // CallJS with IJSObjectWrapper<T> return, automatically wrapping the JSObject result in HtmlElement
    public HtmlElement RemoveChild(HtmlElement child)
        => this.CallJS<HtmlElement>(funcName: "removeChild", child);

    /// <summary>Inserts newChild before referenceChild (multiple params via SerratedJS.Params). Pass null as referenceChild to insert at end.</summary>
    public HtmlElement InsertBefore(HtmlElement newChild, HtmlElement? referenceChild)
        => this.CallJS<HtmlElement>(funcName: "insertBefore", SerratedJS.Params(newChild.JSObject, referenceChild?.JSObject));
     
    public void SetAttribute(string name, string value)
        => this.CallJS(funcName: "setAttribute", name, value); // CallJS with void return

    public string GetAttribute(string name)
        => this.CallJS<string>(funcName:"getAttribute", name); // CallJS with string return

    public string TextContent
    {
        get => this.GetJSProperty<string>("TextContent");
        set => this.SetJSProperty(propertyName:"TextContent", value);
    }

    public string InnerHtml
    {
        get => this.GetJSProperty<string>("InnerHtml");
        set => this.SetJSProperty(propertyName:"InnerHtml", value);
    }

    public string OuterHtml
    {
        get => this.GetJSProperty<string>("OuterHtml");
        set => this.SetJSProperty(propertyName:"OuterHtml", value);
    }

    /// <summary>Returns a DOMTokenList wrapper around this element's classList (does not implement IJSObjectWrapper).</summary>
    public DomTokenList ClassList => new DomTokenList(this.GetJSProperty<JSObject>());
}
