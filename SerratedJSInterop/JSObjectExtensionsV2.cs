using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.JavaScript;

namespace SerratedSharp.SerratedJSInterop;

/// <summary>
/// Holds the argument array for a JS call. Use SerratedJS.Params(...) to create.
/// </summary>
public readonly struct JSParams
{
    internal object[] Args { get; }

    internal JSParams(object[] args)
    {
        Args = args;
    }
}



/// <summary>
/// Factory for JSParams. Use Params(...) so explicit arrays are passed as a single argument when desired.
/// </summary>
public static class SerratedJS
{
    // CONSIDER: Checking parameters for IJSObjectWrapper and conditionally do paramItem.JSObject to pass the native object automatically.

    /// <summary>
    /// Multiple arguments or no arguments. Note: To explicitly pass a single array object[] parameter and avoid unintentional expansion to multiple parameters, use <c>.ArrayParam()</c>.
    /// </summary>
    public static JSParams Params(params object[] parameters)
    {
        return new JSParams(parameters);
    }

    /// <summary>
    /// Pass a string array as a single JS argument (avoids params expansion).
    /// </summary>
    public static JSParams Params(string[] array)
    {
        return new JSParams(new object[] { array });
    }

    /// <summary>
    /// Use when passing a single array param, to avoid unintentional expansion to multiple parameters.
    /// </summary>
    public static JSParams ArrayParam(object[] array)
    {
        return new JSParams(new object[] { array });
    }

    /// <summary>
    /// Calls JS constructor for `typePath` constructor name and namespace if required.  E.g. "PIXI.Rectangle", "Image". Returns <see cref="JSObject"/>.
    /// Use `SerratedJS.Params("param1", 2)` if passing constructor arguments.
    /// </summary>
    /// <param name="typePath">JS constructor name. Include fully qualified path if necessary (e.g. "PIXI.Rectangle", "Image").  Is case sensitive.</param>
    /// <param name="parameters">Optional constructor arguments (e.g. SerratedJS.Params("green", 50, 100)).</param>
    /// <returns>JSObject reference to the new object.</returns>
    /// <example>
    /// // Implement interop constructor in type implementing IJSObjectWrapper
    /// public Image() 
    /// {
    ///     JSObject = SerratedJS.New(nameof(Image));
    /// }
    /// </example>
    [OverloadResolutionPriority(20)]
    public static JSObject New(string typePath, JSParams parameters)
    {
        var args = parameters.Args ?? Array.Empty<object>();
        var unwrapped = JSImportInstanceHelpers.UnwrapJSObjectParams(args);
        return HelpersJS.ObjectNew(typePath, unwrapped);
    }

    /// <summary>
    /// Calls JS constructor for `typePath` constructor name and namespace if required.  E.g. "PIXI.Rectangle", "Image". Returns <see cref="JSObject"/>.
    /// </summary>
    /// <param name="typePath">JS constructor name. Include fully qualified path if necessary (e.g. "PIXI.Rectangle", "Image").  Is case sensitive.</param>
    /// <param name="parameters">Optional constructor arguments (e.g. SerratedJS.New("Rectangle", "green", 50, 100)).</param>
    /// <returns>JSObject reference to the new object.</returns>
    public static JSObject New(string typePath, params object[] parameters)
    {
        return New(typePath, Params(parameters));
    }

    // CONSIDER: Overloads for string[] and other array types to avoid unintentional expansion when passing arrays as constructor arguments.

}


