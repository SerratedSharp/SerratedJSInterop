define(() => {

// This javascript is provided as a Static Web Asset by SerratedSharp.SerratedJSInterop for Blazor WASM or WasmBrowser.
console.log("Declaring SerratedJSInteropShim Shims with export");

var SerratedJSInteropShim = globalThis.SerratedJSInteropShim || {};
(function (SerratedJSInteropShim) {

    var HelpersShim = SerratedJSInteropShim.HelpersShim || {};// create child namespace

    HelpersShim.GetArrayObjectItems = function (arrayObject) {
        return arrayObject.items;
    }

    // Same body as GetArrayObjectItems; .NET uses [return: JSMarshalAs(JSType.Array(JSType.Any))] for mixed primitives + objects.
    HelpersShim.GetPackedCallbackItems = function (packed) {
        return packed.items;
    };

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

    // CallbackShim: wrap .NET callbacks for JS, pass wrapper to target method, return handler for unbind/remove.
    // Packed object uses { items: args }; C# PackedParams.GetUnpacked reads items as object[] (primitives + objects).
    // Core behavior: one implementation builds args with handler at callbackParamIndex (like FuncByNameToObject for CallJS).
    var CallbackShim = SerratedJSInteropShim.CallbackShim || {};

    function buildArgsAndApply(jsObject, funcName, handler, callbackParamIndex, otherParams) {
        const fn = jsObject?.[funcName];
        if (typeof fn !== "function") {
            throw new Error(`"${funcName}" is not a function on target.`);
        }
        const n = (otherParams && otherParams.length) ? otherParams.length : 0;
        const args = new Array(n + 1);
        for (var i = 0; i < args.length; i++) {
            if (i === callbackParamIndex) {
                args[i] = handler;
            } else {
                const idx = i < callbackParamIndex ? i : i - 1;
                args[i] = (otherParams && idx < n) ? unwrapSerratedPocoArg(otherParams[idx]) : undefined;
            }
        }
        return fn.apply(jsObject, args);
    }

    // Core: callback at any index. isPackedParams: true = handler receives (...args) => action({ items: args }); false = (arg) => action(arg).
    CallbackShim.CallMethodWithCallbackAt = function (jsObject, funcName, callbackParamIndex, action, otherParams, isPackedParams) {
        const handler = isPackedParams ? (...args) => action({ items: args }) : (arg) => action(arg);
        return buildArgsAndApply(jsObject, funcName, handler, callbackParamIndex, otherParams);
    };

    CallbackShim.SetPropertyWithCallback = function (jsObject, propertyName, action, isPackedParams) {
        const handler = isPackedParams ? (...args) => action({ items: args }) : (arg) => action(arg);
        jsObject[propertyName] = handler;
        return { __handler: handler };
    };

    // Wrap a .NET callback once; return the handler (SerratedJQ-style). Optional context is used for .bind(context) so handler has correct this.
    // Use the same (...args) / (e) shapes as CallMethodWithCallbackAt — classic function()+arguments can break once .bind(context) is applied,
    // and the managed Action<JSObject> may then receive the outer handler function instead of { items: [...] }.
    CallbackShim.WrapCallback = function (action, isPackedParams, context) {
        var handler = isPackedParams ? (...args) => action({ items: args }) : (e) => action(e);
        if (context != null) {
            handler = handler.bind(context);
        }
        return handler;
    };

    // CallbackHandle + packed params: same { items: args } shape as other packed shims so PackedParams uses one JSObject path.
    CallbackShim.WrapPackedParamsCallback = function (onPacked, context) {
        var handler = function () {
            var args = Array.prototype.slice.call(arguments);
            onPacked({ items: args });
        };
        if (context != null) {
            handler = handler.bind(context);
        }
        return handler;
    };

    SerratedJSInteropShim.CallbackShim = CallbackShim;

})(SerratedJSInteropShim = globalThis.SerratedJSInteropShim || (globalThis.SerratedJSInteropShim = {}));

});
