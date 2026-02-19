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
        get => this.GetProperty<string>();
        set => this.SetProperty(value);
    }

    public string Alt
    {
        get => this.GetProperty<string>();
        set => this.SetProperty(value);
    }

    public int Width
    {
        get => this.GetProperty<int>();
        set => this.SetProperty(value);
    }

    public int Height
    {
        get => this.GetProperty<int>();
        set => this.SetProperty(value);
    }

    public int NaturalWidth => this.GetProperty<int>();
    public int NaturalHeight => this.GetProperty<int>();
    public bool Complete => this.GetProperty<bool>();
    public string CurrentSrc => this.GetProperty<string>();
    public string CrossOrigin
    {
        get => this.GetProperty<string>();
        set => this.SetProperty(value);
    }
}
