namespace SerratedSharp.SerratedJSInterop.Tests.BlazorWasm.Services;

public class AppInitializationService : IAppInitializationService
{
    public event EventHandler? FirstPageLoaded;
    private bool _raised;

    public void NotifyFirstPageLoaded()
    {
        if (_raised) return;
        _raised = true;
        FirstPageLoaded?.Invoke(this, EventArgs.Empty);
    }
}
