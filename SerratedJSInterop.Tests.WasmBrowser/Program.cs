using SerratedSharp.SerratedJSInterop;
using SerratedSharp.SerratedJQ.Plain;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices.JavaScript;
using System.Threading.Tasks;
using Wasm;

internal class Program
{
    private static async global::System.Threading.Tasks.Task Main(string[] args)
    {
        Console.WriteLine("Hello, Browser!");


        //await SerratedSharp.SerratedJQ.SerratedJQModule.ImportAsync("..");
        //await SerratedSharp.SerratedJQ.SerratedJQModule.LoadJQuery("https://ajax.googleapis.com/ajax/libs/jquery/3.7.1/jquery.min.js");

        //await JSInteropHelpersModule.ImportAsync("", "./SerratedInteropHelpers.js");
        
        // SerratedJSInterop 
        await SerratedJSInteropModule.ImportAsync();
        
        // Legacy InteropHelpers, needed to support SerratedJQ which still uses old.
        await SerratedSharp.SerratedJQ.SerratedJQModule.ImportAsync("..");
        await SerratedSharp.SerratedJQ.SerratedJQModule.LoadJQuery("https://ajax.googleapis.com/ajax/libs/jquery/3.7.1/jquery.min.js");
        

        await JQueryPlain.Ready();
        Console.WriteLine("JQuery Document Ready!");

        // Do something with JQuery
        JQueryPlain.Select("#out").Append("<b>Appended</b>");

        try
        {
            await TestOrchestrator.Begin();
        }
        catch (Exception ex)
        {
            GlobalJS.Console.Log(ex.ToString());
        }

        // Read the HTML of Body for test verification
        //string testResultsBody = JQueryPlain.Select("body").Html();

        if (args.Length == 1 && args[0] == "start")
            StopwatchSample.Start();

        while (true)
        {
            StopwatchSample.Render();
            await Task.Delay(1000);
        }
    }
}

partial class StopwatchSample
{
    private static Stopwatch stopwatch = new();

    public static void Start() => stopwatch.Start();
    public static void Render() => SetInnerText("#time", stopwatch.Elapsed.ToString(@"mm\:ss"));

    [JSImport("dom.setInnerText", "main.js")]
    internal static partial void SetInnerText(string selector, string content);

    [JSExport]
    internal static bool Toggle()
    {
        if (stopwatch.IsRunning)
        {
            stopwatch.Stop();
            return false;
        }
        else
        {
            stopwatch.Start();
            return true;
        }
    }

    [JSExport]
    internal static void Reset()
    {
        if (stopwatch.IsRunning)
            stopwatch.Restart();
        else
            stopwatch.Reset();

        Render();
    }

    [JSExport]
    internal static bool IsRunning() => stopwatch.IsRunning;
}
