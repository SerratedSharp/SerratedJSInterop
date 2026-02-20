using System.Runtime.InteropServices.JavaScript;
using System.Runtime.Versioning;
using SerratedSharp.SerratedJSInterop;

namespace SerratedSharp.SerratedDom;

/// <summary>
/// Wraps a DOM HTMLAudioElement (global constructor: Audio). In the DOM, HTMLAudioElement extends HTMLMediaElement extends HTMLElement.
/// </summary>
[SupportedOSPlatform("browser")]
public class Audio : IJSObjectWrapper<Audio>
{
    static Audio IJSObjectWrapper<Audio>.WrapInstance(JSObject jsObject) => new Audio(jsObject);

    public JSObject JSObject { get; }

    public Audio() 
    {
         JSObject = SerratedJS.New(nameof(Audio));
    }
    // Cleaner approach, leverage the JSObject constructor:
    //public Audio() : this(SerratedJS.New(nameof(Audio))) 
    //{ }

    // Alternatively, if inheriting from another wrapper, call base passing new JSObject:
    // public Audio() : base(SerratedJS.New(nameof(Audio))) { }

    public Audio(JSObject jsObject)
    {
        JSObject = jsObject;
    }

    public Audio(JSObject jsObject, string src) : this(jsObject)
    {
        this.Src = src;
    }

    public Audio(string src) : this(SerratedJS.New(nameof(Audio), SerratedJS.Params(src)))
    {}

    public string Src
    {
        get => this.GetProperty<string>();
        set => this.SetProperty(value);
    }

    public double Duration => this.GetProperty<double>();
    public bool IsPaused => this.GetProperty<bool>("paused");        
    public DomTokenList ControlsList => this.GetProperty<DomTokenList>(); // Returning another wrapped type
    public void AddTextTrack(string kind, string label, string language)
        => this.CallJS("addTextTrack", kind, label, language);
    public void CanPlayType(string type) => this.CallJS(type);
    public JSObject CaptureStream() => this.CallJS<JSObject>("captureStream");
    // Alternatively, if we have a CaptureStream wrapper, it can automatically wrap returned JSObject:
    // public CaptureStream CaptureStream() => this.CallJS<CaptureStream>("captureStream");

    public double Volume
    {
        get => this.GetProperty<double>();
        set => this.SetProperty(value);
    }

    public bool IsMuted
    {
        get => this.GetProperty<bool>("muted");
        set => this.SetProperty(value, "muted");
    }

    public void Pause() => this.CallJS();
    public void Load() => this.CallJS();

}
