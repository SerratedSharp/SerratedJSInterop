namespace SerratedSharp.SerratedJSInterop.Tests.BlazorWasm.Services;

public interface IAppInitializationService
{
    event EventHandler? FirstPageLoaded;
    void NotifyFirstPageLoaded();
}
