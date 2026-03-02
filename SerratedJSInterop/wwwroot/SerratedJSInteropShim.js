// This javascript is provided as a Static Web Asset by SerratedSharp.SerratedJSInterop for Blazor WASM or WasmBrowser.
console.log("Declaring SerratedJSInteropShim Shims with export");

var SerratedJSInteropShim = globalThis.SerratedJSInteropShim || {};
(function (SerratedJSInteropShim) {

    var HelpersShim = SerratedJSInteropShim.HelpersShim || {};// create child namespace

    HelpersShim.GetArrayObjectItems = function (arrayObject) {
        return arrayObject.items;
    }

    // Marshal a native JS array of objects into a form that can be returned
    // as JSObject[] on the .NET side. The interop marshaller handles the
    // conversion when used with [return: JSMarshalAs<JSType.Array<JSType.Object>>].
    HelpersShim.MarshalAsArrayOfObjects = function (arrayObject) {
        return arrayObject;
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
        if (params && params.length > 0) {
            params = params.map(unwrapSerratedPocoArg);
        }
        const rtn = jsObject[funcName].apply(jsObject, params);
        return rtn;
    };

    HelpersShim.PropertyByNameToObject = function (jsObject, propertyName) {
        const rtn = jsObject[propertyName];
        return rtn;
    };

    HelpersShim.SetPropertyByName = function (jsObject, propertyName, value) {
        value = unwrapSerratedPocoArg(value);
        Reflect.set(jsObject, propertyName, value);
        return jsObject[propertyName];
    };

    var serratedPocoPrefix = 'serratedPoco:';
    // If a string has the serratedPoco: prefix (from C# MarshalAsJson), JSON parse and return the object; else return original.
    function unwrapSerratedPocoArg(value) {
        if (typeof value !== 'string') return value;
        if (value.length < serratedPocoPrefix.length) return value;
        if (value.indexOf(serratedPocoPrefix) !== 0) return value;
        try {
            return JSON.parse(value.slice(serratedPocoPrefix.length));
        } catch (e) {
            return value;
        }
    }

    // Call constructor with optional arguments.
    HelpersShim.ObjectNew = function (path, args) {
        var constructor = HelpersShim.ResolvePath(path);
        if (typeof constructor !== 'function') {
            throw new Error(`"${path}" is not a constructor function`);
        }
        if (!args || args.length === 0) return new constructor();
        var unwrapped = args.map(unwrapSerratedPocoArg);
        return new constructor(...unwrapped);
    };

    // Resolve a fully qualified path like "PIXI.Rectangle" to constructor
    HelpersShim.ResolvePath = function (path) {
        var parts = path.split('.');
        var obj = globalThis;
        for (var i = 0; i < parts.length; i++) {
            if (obj === null || obj === undefined) {
                throw new Error(`Path resolution failed at "${parts.slice(0, i + 1).join('.')}"`);
            }
            obj = obj[parts[i]];
        }
        return obj;
    };

    SerratedJSInteropShim.HelpersShim = HelpersShim; // add to parent namespace

})(SerratedJSInteropShim = globalThis.SerratedJSInteropShim || (globalThis.SerratedJSInteropShim = {}));

export { SerratedJSInteropShim };
