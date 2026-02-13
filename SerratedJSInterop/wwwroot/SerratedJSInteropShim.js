// This javascript is provided as a Static Web Asset by SerratedSharp.SerratedJSInterop for Blazor WASM or WasmBrowser.
console.log("Declaring SerratedJSInteropShim Shims with export");

var SerratedJSInteropShim = globalThis.SerratedJSInteropShim || {};
(function (SerratedJSInteropShim) {

    var HelpersShim = SerratedJSInteropShim.HelpersShim || {};// create child namespace

    HelpersShim.GetArrayObjectItems = function (arrayObject) {
        return arrayObject.items;
    }

    HelpersShim.LoadScript = function (relativeUrl) {
        return new Promise(function (resolve, reject) {
            var script = document.createElement("script");
            script.onload = resolve;
            script.onerror = reject;
            script.src = relativeUrl;
            document.getElementsByTagName("head")[0].appendChild(script);
        });
    };

    HelpersShim.FuncByNameToObject = function (jsObject, funcName, params) {
        const rtn = jsObject[funcName].apply(jsObject, params);
        return rtn;
    };

    HelpersShim.PropertyByNameToObject = function (jsObject, propertyName) {
        const rtn = jsObject[propertyName];
        return rtn;
    };

    HelpersShim.SetPropertyByName = function (jsObject, propertyName, value) {
        // Use Reflect.set to properly trigger setters/proxies when present
        Reflect.set(jsObject, propertyName, value);
        return jsObject[propertyName];
    };

    SerratedJSInteropShim.HelpersShim = HelpersShim; // add to parent namespace

})(SerratedJSInteropShim = globalThis.SerratedJSInteropShim || (globalThis.SerratedJSInteropShim = {}));

export { SerratedJSInteropShim };
