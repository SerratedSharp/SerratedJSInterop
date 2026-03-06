using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace SerratedSharp.SerratedJSInterop;

public static class WrapperPropertyExtensions
{
    /// <summary>
    /// <para>Get the value of <c>propertyName</c>(inferred via [CallerMemberName]) on this IJSObjectWrapper's JSObject.</para>
    /// <para>var value = wrapper.GetJSProperty&lt;string&gt;()</para>
    /// </summary>
    /// <typeparam name="J">Type of the property value. Typically a JSObject, IJSObjectWrapper&lt;J&gt;, or type compatible with JSImport type marshalling.</typeparam>
    /// <param name="wrapper">The wrapper whose JSObject to read the property from.</param>
    /// <param name="propertyName">Name of the property on this wrapper's <c>JSObject</c> to get, with first letter lower cased for JS casing conventions.</param>
    /// <returns>The property value, casted or wrapped to requested type <c>J</c>.</returns>
    public static J GetJSProperty<[DynamicallyAccessedMembers(JSImportInstanceHelpers.WrapperTypeMembers)] J>(this IJSObjectWrapper wrapper, [CallerMemberName] string? propertyName = null)
        => JSImportInstanceHelpers.GetProperty<J>(wrapper.JSObject, propertyName!);

    /// <summary>
    /// <para>Set the value of <c>propertyName</c>(inferred via [CallerMemberName]) on this IJSObjectWrapper's JSObject.</para>
    /// <para>wrapper.SetJSProperty(value)</para>
    /// </summary>
    /// <param name="wrapper">The wrapper whose JSObject to set the property on.</param>
    /// <param name="value">Value to set. IJSObjectWrapper instances are unwrapped to their JSObject.</param>
    /// <param name="propertyName">Name of the property on this wrapper's <c>JSObject</c> to set, with first letter lower cased for JS casing conventions.</param>
    [OverloadResolutionPriority(10)]
    public static void SetJSProperty(this IJSObjectWrapper wrapper, object value, [CallerMemberName] string? propertyName = null)
    {
        var unwrappedValue = value as IJSObjectWrapper;
        JSImportInstanceHelpers.SetProperty(wrapper.JSObject, propertyName!, unwrappedValue?.JSObject ?? value);
    }

    /// <summary>
    /// <para>Set the value of <c>propertyName</c> on this IJSObjectWrapper's JSObject.</para>
    /// <para>wrapper.SetJSProperty(propertyName: "someProperty", "value")</para>
    /// </summary>
    /// <param name="wrapper">The wrapper whose JSObject to set the property on.</param>
    /// <param name="propertyName">Name of the property on this wrapper's <c>JSObject</c> to set, with casing preserved.</param>
    /// <param name="value">Value to set. IJSObjectWrapper instances are unwrapped to their JSObject.</param>
    public static void SetJSProperty(this IJSObjectWrapper wrapper, string propertyName, object value)
    {
        var unwrappedValue = value as IJSObjectWrapper;
        JSImportInstanceHelpers.SetProperty(wrapper.JSObject, propertyName, unwrappedValue?.JSObject ?? value, applyJSCasing: false);
    }
}


