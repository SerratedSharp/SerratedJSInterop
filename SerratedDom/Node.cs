using System.Runtime.InteropServices.JavaScript;
using System.Runtime.Versioning;
using SerratedSharp.SerratedJSInterop;

namespace SerratedSharp.SerratedDom;

/// <summary>
/// Wraps a DOM Node for use with Serrated JS interop. Element types such as HtmlElement extend Node.
/// </summary>
[SupportedOSPlatform("browser")]
public class Node : IJSObjectWrapper<Node>
{
    /// <inheritdoc />
    public static Node WrapInstance(JSObject jsObject) => new Node(jsObject);
    /// <inheritdoc />
    public JSObject JSObject => jsObject;

    internal JSObject jsObject;

    /// <summary>
    /// Creates a Node wrapper for an existing DOM Node JSObject.
    /// </summary>
    public Node(JSObject jsObject)
    {
        this.jsObject = jsObject;
    }

    /// <summary>
    /// Returns whether this node has any child nodes. No parameters; uses CallJS&lt;J&gt;() with inferred name.
    /// </summary>
    public bool HasChildNodes() => this.CallJS<bool>();

    /// <summary>
    /// Inserts newChild before referenceChild. Uses multiple params (two nodes) via SerratedJS.Params.
    /// Pass null as referenceChild to insert at the end (like appendChild).
    /// </summary>
    /// <returns>The inserted node (newChild).</returns>
    public Node InsertBefore(Node newChild, Node? referenceChild)
        => this.CallJS<Node>("insertBefore", SerratedJS.Params(newChild.JSObject, referenceChild?.JSObject));
}
