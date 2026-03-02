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
    public int Length => _js.GetJSProperty<int>();

    // Infers CallerMemberName
    public string Item(int index) => _js.CallJS<string>(index);

    // Infers CallerMemberName
    public bool Contains(string token) => _js.CallJS<bool>(token);

    // Explicit method name
    public string ItemByIndex(int index) => _js.CallJS<string>(funcName: "item", index);

    // Void return with inferred method name and SerratedJS.Params (not required in this case)
    public void Add(string token) => _js.CallJS(SerratedJS.Params(token));
        
    // Void return(no <J>), explicit method name, optionally use SerratedJS.Params
    public void Remove(string token) => _js.CallJS(funcName: "remove", SerratedJS.Params(token));

    // Void return with explicit method name, passing multiple parameters
    public void RemoveMultiple(string t1, string t2) => _js.CallJS(funcName: "remove", t1, t2);

    // Alternatively with SerratedJS.Params
    public void RemoveMultipleV2(string t1, string t2) => _js.CallJS(funcName: "remove", SerratedJS.Params(t1, t2));
}
