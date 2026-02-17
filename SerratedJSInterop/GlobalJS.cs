using System.Runtime.InteropServices.JavaScript;

namespace SerratedSharp.SerratedJSInterop
{
    public static class GlobalJS
    {
        public static class Console
        {
            static Lazy<JSObject> _console = new(() => JSHost.GlobalThis.GetProperty<JSObject>("console"));

            /// <summary>
            /// console.log(...)
            /// </summary>
            public static void Log(params object[] parameters)
            {
                _console.Value.CallJS(SerratedJS.Params(parameters));
            }
        }
    }
}
