using System.Runtime.InteropServices.JavaScript;
using System.Runtime.Versioning;
using SerratedSharp.SerratedJSInterop;

namespace SerratedSharp.SerratedDom;

/// <summary>
/// Wraps the global window.location.
/// Does not implement IJSObjectWrapper, but uses private JSObject instead.
/// </summary>
[SupportedOSPlatform("browser")]
public sealed class Location
{
    private readonly JSObject _js;

    private static readonly Lazy<JSObject> location = new(() =>
        JSHost.GlobalThis.GetJSProperty<JSObject>("location"));

    public Location(JSObject jsObject)
    {
        _js = jsObject;
    }

    public static Location GetLocation() => new Location(location.Value);

    public string Href
    {
        get => _js.GetJSProperty<string>();
        set => _js.SetJSProperty(value);
    }

    public void Reload() => _js.CallJS();

    public void Assign(string url) => _js.CallJS(SerratedJS.Params(url));
}
