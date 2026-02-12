// SerratedInteropHelpers – same as Blazor RCL; required for SetPropertyByName etc. when not using the RCL.
console.log("Declaring SerratedInteropHelpers Shims with export");

var SerratedInteropHelpers = globalThis.SerratedInteropHelpers || {};
(function (SerratedInteropHelpers) {

    var HelpersProxy = SerratedInteropHelpers.HelpersProxy || {};// create child namespace

    HelpersProxy.GetArrayObjectItems = function (arrayObject) {
        return arrayObject.items;
    }

    HelpersProxy.LoadScript = function (relativeUrl) {
        return new Promise(function (resolve, reject) {
            var script = document.createElement("script");
            script.onload = resolve;
            script.onerror = reject;
            script.src = relativeUrl;
            document.getElementsByTagName("head")[0].appendChild(script);
        });
    };

    HelpersProxy.FuncByNameToObject = function (jsObject, funcName, params) {
        const rtn = jsObject[funcName].apply(jsObject, params);
        return rtn;
    };

    HelpersProxy.PropertyByNameToObject = function (jsObject, propertyName) {
        const rtn = jsObject[propertyName];
        return rtn;
    };

    HelpersProxy.SetPropertyByName = function (jsObject, propertyName, value) {
        // Use Reflect.set to properly trigger setters/proxies when present
        Reflect.set(jsObject, propertyName, value);
        return jsObject[propertyName];
    };

    SerratedInteropHelpers.HelpersProxy = HelpersProxy; // add to parent namespace

})(SerratedInteropHelpers = globalThis.SerratedInteropHelpers || (globalThis.SerratedInteropHelpers = {}));

export { SerratedInteropHelpers };
