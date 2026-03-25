// Test-only: provides GetCallbackTestMock and EvalInGlobal on SerratedJSInteropShim.HelpersShim.
// Load this script after SerratedJSInteropShim.js so HelpersShim exists (or we create the namespace).
(function () {
    var g = typeof globalThis !== 'undefined' ? globalThis : (typeof window !== 'undefined' ? window : this);
    if (!g.SerratedJSInteropShim) g.SerratedJSInteropShim = {};
    if (!g.SerratedJSInteropShim.HelpersShim) g.SerratedJSInteropShim.HelpersShim = {};
    g.SerratedJSInteropShim.HelpersShim.EvalInGlobal = function (code) {
        (function () { eval(code); }).call(g);
    };
    g.SerratedJSInteropShim.HelpersShim.GetCallbackTestMock = function () {
        if (!g.__callbackTestMock) {
            g.__callbackTestMock = {
                add: function (f) { this.fn = f; return { __handler: f }; },
                remove: function (tokenOrHandle) { var f = tokenOrHandle && (tokenOrHandle.__handler || tokenOrHandle); if (f && f === this.fn) this.fn = null; },
                fire: function () { if (this.fn) this.fn.apply(this, arguments); },
                register: function (a, cb, b) { this._reg = { a: a, cb: cb, b: b }; return { __handler: cb }; },
                getReg: function () { return this._reg; },
                noCallbackReturn: function () { return 42; }
            };
        }
        return g.__callbackTestMock;
    };
})();
