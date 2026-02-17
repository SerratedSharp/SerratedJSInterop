using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SerratedSharp.SerratedJQ.Plain;
using SerratedSharp.SerratedJSInterop.Tests.BlazorWasm.Services;
using Wasm;

namespace SerratedSharp.SerratedJSInterop.Tests.BlazorWasm;
public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");

        builder.Services.AddSingleton<IAppInitializationService, AppInitializationService>();
        builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

        await SetupInterop();

        var host = builder.Build();
        var appInit = host.Services.GetRequiredService<IAppInitializationService>();
        appInit.FirstPageLoaded += (s, e) => _ = OnFirstPageLoadedAsync();
        await host.RunAsync();
    }

    private static async Task OnFirstPageLoadedAsync()
    {
        JQueryPlain.Select("#out").Append("<b>Appended</b>");
        try
        {
            await TestOrchestrator.Begin();
        }
        catch (Exception ex)
        {
            GlobalJS.Console.Log(ex.ToString());
        }
    }

    private static async Task SetupInterop()
    {
        await SerratedJSInteropModule.ImportAsync();
        // Legacy InteropHelpers, needed to support SerratedJQ which still uses old.
        await SerratedSharp.SerratedJQ.SerratedJQModule.ImportAsync("..");
        await SerratedSharp.SerratedJQ.SerratedJQModule.LoadJQuery("https://ajax.googleapis.com/ajax/libs/jquery/3.7.1/jquery.min.js");

        await JQueryPlain.Ready();
        Console.WriteLine("JQuery Document Ready!");
    }
}
