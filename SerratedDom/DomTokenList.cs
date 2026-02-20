using System.Runtime.InteropServices.JavaScript;
using System.Runtime.Versioning;
using SerratedSharp.SerratedJSInterop;

namespace SerratedSharp.SerratedDom;

/// <summary>
/// Wraps a DOM DOMTokenList (e.g. element.classList). 
/// Does not implement IJSObjectWrapper, but uses private JSObject instead.
/// </summary>
[SupportedOSPlatform("browser")]
public sealed class DomTokenList
{
    private readonly JSObject _js;

    /// <summary>
    /// Wraps the given JSObject (e.g. from element.classList).
    /// </summary>
    public DomTokenList(JSObject jsObject)
    {
        _js = jsObject;
    }

    /// <summary>Number of tokens. Uses GetProperty (CallerMemberName).</summary>
    public int Length => _js.GetProperty<int>();

    // Infers CallerMemberName, with required SerratedJS.Params for overloads that both infer name and pass params
    public string Item(int index) => _js.CallJS<string>(SerratedJS.Params(index));

    // Explicit method name, with SerratedJS.Params (not required for this overload)
    public bool Contains(string token) => _js.CallJS<bool>("contains", SerratedJS.Params(token));

    // Explicit method name, without SerratedJS.Params
    public string ItemByIndex(int index) => _js.CallJS<string>("item", index);

    // Void return with inferred method name and SerratedJS.Params
    public void Add(string token) => _js.CallJS(SerratedJS.Params(token));

    // Void return with explicit method name and SerratedJS.Params(not required for this overload)
    public void Remove(string token) => _js.CallJS("remove", SerratedJS.Params(token));

    // Void return with explicit method name, without SerratedJS.Params
    public void RemoveMultiple(string t1, string t2) => _js.CallJS("remove", t1, t2);
}
