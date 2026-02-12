using System.Runtime.InteropServices.JavaScript;

namespace SerratedSharp.SerratedJSInterop
{
    public static class GlobalJS
    {
        public static class Console
        {
            static Lazy<JSObject> _console = new(() => JSHost.GlobalThis.GetPropertyAsJSObject("console"));

            /// <summary>
            /// console.log, but note all params gets logged as a single array
            /// </summary>
            /// <param name="parameters">JSObjects or strings to log.</param>
            public static void Log(params object[] parameters)
            {
                JSImportInstanceHelpers.CallJSOfSameName<object>(_console.Value, parameters);
            }
        }
    }
}
