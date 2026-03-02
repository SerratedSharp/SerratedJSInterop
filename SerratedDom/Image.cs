using System.Runtime.InteropServices.JavaScript;
using System.Runtime.Versioning;
using SerratedSharp.SerratedJSInterop;

namespace SerratedSharp.SerratedDom;

/// <summary>
/// Wraps a DOM HTMLImageElement (Image). In the DOM, HTMLImageElement extends HTMLElement.
/// </summary>
[SupportedOSPlatform("browser")]
public class Image : HtmlElement, IJSObjectWrapper<Image>
{
    static Image IJSObjectWrapper<Image>.WrapInstance(JSObject jsObject) => new Image(jsObject);

    /// <summary>
    /// Creates a new image via SerratedJS.New("Image").
    /// </summary>
    public Image() : base(SerratedJS.New(nameof(Image)))
    {
        
    }

    /// <summary>
    /// Wraps an existing Image/HTMLImageElement JSObject.
    /// </summary>
    public Image(JSObject jsObject) : base(jsObject)
    {
    }

    public string Src
    {
        get => this.GetJSProperty<string>();
        set => this.SetJSProperty(value);
    }

    public string Alt
    {
        get => this.GetJSProperty<string>();
        set => this.SetJSProperty(value);
    }

    public int Width
    {
        get => this.GetJSProperty<int>();
        set => this.SetJSProperty(value);
    }

    public int Height
    {
        get => this.GetJSProperty<int>();
        set => this.SetJSProperty(value);
    }

    public int NaturalWidth => this.GetJSProperty<int>();
    public int NaturalHeight => this.GetJSProperty<int>();
    public bool Complete => this.GetJSProperty<bool>();
    public string CurrentSrc => this.GetJSProperty<string>();
    public string CrossOrigin
    {
        get => this.GetJSProperty<string>();
        set => this.SetJSProperty(value);
    }

    /// <summary>Returns the DOMRect for this element (inherited from Element).</summary>
    public JSObject GetBoundingClientRect() => this.CallJS<JSObject>();

    /// <summary>Removes the named attribute (inherited from Element).</summary>
    public void RemoveAttribute(string name) => this.CallJS(funcName: "removeAttribute", name);

    /// <summary>Gives focus to this element (inherited from HTMLElement).</summary>
    public void Focus() => this.CallJS();

    /// <summary>Removes focus from this element (inherited from HTMLElement).</summary>
    public void Blur() => this.CallJS();
}
