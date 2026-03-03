using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.JavaScript;

namespace SerratedSharp.SerratedJSInterop;

public static class ExtensionsJSObjectProperty
{
    // this.GetJSProperty("someProperty");
    /// <summary>
    /// <para>Get the value of <c>propertyName</c>(inferred via [CallerMemberName]) on this JSObject.</para>
    /// <para>this.GetJSProperty("someProperty")</para>
    /// </summary>
    /// <typeparam name="J">Type of the property value. Typically a JSObject, IJSObjectWrapper&lt;J&gt;, or type compatible with JSImport type marshalling.</typeparam>
    /// <param name="jsObject">Reference to the JS instance to read the property from.</param>
    /// <param name="propertyName">Name of the property on this <c>jsObject</c> to get.  Omit to infer from [CallerMemberName], with first letter lower cased for JS casing conventions.</param>
    /// <returns>The property value, casted or wrapped to requested type <c>J</c>.</returns>
    public static J GetJSProperty<J>(this JSObject jsObject, [CallerMemberName] string? propertyName = null)
        => JSImportInstanceHelpers.GetProperty<J>(jsObject, propertyName!);

    // this.SetJSProperty("someProperty", "value");
    /// <summary>
    /// <para>Set the value of <c>propertyName</c>(inferred via [CallerMemberName]) on this JSObject.</para>
    /// <para>this.SetJSProperty("value")</para>
    /// </summary>
    /// <param name="jsObject">Reference to the JS instance to set the property on.</param>
    /// <param name="value">Value to set.</param>
    /// <param name="propertyName">Name of the property on this <c>jsObject</c> to set.  Omit to infer from [CallerMemberName], with first letter lower cased for JS casing conventions.</param>
    [OverloadResolutionPriority(10)]
    public static void SetJSProperty(this JSObject jsObject, object value, [CallerMemberName] string? propertyName = null)
    {
        var unwrappedValue = value as IJSObjectWrapper;
        JSImportInstanceHelpers.SetProperty(jsObject, propertyName!, unwrappedValue?.JSObject ?? value);
    }


    /// <summary>
    /// <para>Set the value of <c>propertyName</c> on this JSObject with an explicit propertyName.</para>
    /// <para>this.SetJSProperty(propertyName: "someProperty", "value")</para>
    /// </summary>
    /// <param name="jsObject">Reference to the JS instance to set the property on.</param>
    /// <param name="propertyName">Name of the property on this <c>jsObject</c> to set, with casing preserved.</param>
    /// <param name="value">Value to set.</param>
    public static void SetJSProperty(this JSObject jsObject, string propertyName, object value)
    {
        var unwrappedValue = value as IJSObjectWrapper;
        JSImportInstanceHelpers.SetProperty(jsObject, propertyName, unwrappedValue?.JSObject ?? value, applyJSCasing: false);
    }

}


