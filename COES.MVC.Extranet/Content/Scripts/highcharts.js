/*
 Highcharts JS v5.0.14 (2017-07-28)

 (c) 2009-2016 Torstein Honsi

 License: www.highcharts.com/license
*/
(function (M, S) { "object" === typeof module && module.exports ? module.exports = M.document ? S(M) : S : M.Highcharts = S(M) })("undefined" !== typeof window ? window : this, function (M) {
    M = function () {
        var a = window, C = a.document, A = a.navigator && a.navigator.userAgent || "", F = C && C.createElementNS && !!C.createElementNS("http://www.w3.org/2000/svg", "svg").createSVGRect, E = /(edge|msie|trident)/i.test(A) && !window.opera, m = !F, f = /Firefox/.test(A), l = f && 4 > parseInt(A.split("Firefox/")[1], 10); return a.Highcharts ? a.Highcharts.error(16, !0) : {
            product: "Highcharts",
            version: "5.0.14", deg2rad: 2 * Math.PI / 360, doc: C, hasBidiBug: l, hasTouch: C && void 0 !== C.documentElement.ontouchstart, isMS: E, isWebKit: /AppleWebKit/.test(A), isFirefox: f, isTouchDevice: /(Mobile|Android|Windows Phone)/.test(A), SVG_NS: "http://www.w3.org/2000/svg", chartCount: 0, seriesTypes: {}, symbolSizes: {}, svg: F, vml: m, win: a, marginNames: ["plotTop", "marginRight", "marginBottom", "plotLeft"], noop: function () { }, charts: []
        }
    }(); (function (a) {
        var C = [], A = a.charts, F = a.doc, E = a.win; a.error = function (m, f) {
            m = a.isNumber(m) ? "Highcharts error #" +
            m + ": www.highcharts.com/errors/" + m : m; if (f) throw Error(m); E.console && console.log(m)
        }; a.Fx = function (a, f, l) { this.options = f; this.elem = a; this.prop = l }; a.Fx.prototype = {
            dSetter: function () { var a = this.paths[0], f = this.paths[1], l = [], r = this.now, u = a.length, t; if (1 === r) l = this.toD; else if (u === f.length && 1 > r) for (; u--;) t = parseFloat(a[u]), l[u] = isNaN(t) ? a[u] : r * parseFloat(f[u] - t) + t; else l = f; this.elem.attr("d", l, null, !0) }, update: function () {
                var a = this.elem, f = this.prop, l = this.now, r = this.options.step; if (this[f + "Setter"]) this[f +
                "Setter"](); else a.attr ? a.element && a.attr(f, l, null, !0) : a.style[f] = l + this.unit; r && r.call(a, l, this)
            }, run: function (a, f, l) { var r = this, m = function (a) { return m.stopped ? !1 : r.step(a) }, t; this.startTime = +new Date; this.start = a; this.end = f; this.unit = l; this.now = this.start; this.pos = 0; m.elem = this.elem; m.prop = this.prop; m() && 1 === C.push(m) && (m.timerId = setInterval(function () { for (t = 0; t < C.length; t++) C[t]() || C.splice(t--, 1); C.length || clearInterval(m.timerId) }, 13)) }, step: function (m) {
                var f = +new Date, l, r = this.options, u = this.elem,
                t = r.complete, g = r.duration, d = r.curAnim; u.attr && !u.element ? m = !1 : m || f >= g + this.startTime ? (this.now = this.end, this.pos = 1, this.update(), l = d[this.prop] = !0, a.objectEach(d, function (a) { !0 !== a && (l = !1) }), l && t && t.call(u), m = !1) : (this.pos = r.easing((f - this.startTime) / g), this.now = this.start + (this.end - this.start) * this.pos, this.update(), m = !0); return m
            }, initPath: function (m, f, l) {
                function r(a) { var c, e; for (n = a.length; n--;) c = "M" === a[n] || "L" === a[n], e = /[a-zA-Z]/.test(a[n + 3]), c && e && a.splice(n + 1, 0, a[n + 1], a[n + 2], a[n + 1], a[n + 2]) }
                function u(a, c) { for (; a.length < v;) { a[0] = c[v - a.length]; var b = a.slice(0, e);[].splice.apply(a, [0, 0].concat(b)); D && (b = a.slice(a.length - e), [].splice.apply(a, [a.length, 0].concat(b)), n--) } a[0] = "M" } function t(a, c) { for (var q = (v - a.length) / e; 0 < q && q--;) y = a.slice().splice(a.length / J - e, e * J), y[0] = c[v - e - q * e], b && (y[e - 6] = y[e - 2], y[e - 5] = y[e - 1]), [].splice.apply(a, [a.length / J, 0].concat(y)), D && q-- } f = f || ""; var g, d = m.startX, k = m.endX, b = -1 < f.indexOf("C"), e = b ? 7 : 3, v, y, n; f = f.split(" "); l = l.slice(); var D = m.isArea, J = D ? 2 : 1, c; b && (r(f),
                r(l)); if (d && k) { for (n = 0; n < d.length; n++) if (d[n] === k[0]) { g = n; break } else if (d[0] === k[k.length - d.length + n]) { g = n; c = !0; break } void 0 === g && (f = []) } f.length && a.isNumber(g) && (v = l.length + g * J * e, c ? (u(f, l), t(l, f)) : (u(l, f), t(f, l))); return [f, l]
            }
        }; a.Fx.prototype.fillSetter = a.Fx.prototype.strokeSetter = function () { this.elem.attr(this.prop, a.color(this.start).tweenTo(a.color(this.end), this.pos), null, !0) }; a.extend = function (a, f) { var m; a || (a = {}); for (m in f) a[m] = f[m]; return a }; a.merge = function () {
            var m, f = arguments, l, r = {}, u =
            function (f, g) { "object" !== typeof f && (f = {}); a.objectEach(g, function (d, k) { !a.isObject(d, !0) || a.isClass(d) || a.isDOMElement(d) ? f[k] = g[k] : f[k] = u(f[k] || {}, d) }); return f }; !0 === f[0] && (r = f[1], f = Array.prototype.slice.call(f, 2)); l = f.length; for (m = 0; m < l; m++) r = u(r, f[m]); return r
        }; a.pInt = function (a, f) { return parseInt(a, f || 10) }; a.isString = function (a) { return "string" === typeof a }; a.isArray = function (a) { a = Object.prototype.toString.call(a); return "[object Array]" === a || "[object Array Iterator]" === a }; a.isObject = function (m,
        f) { return !!m && "object" === typeof m && (!f || !a.isArray(m)) }; a.isDOMElement = function (m) { return a.isObject(m) && "number" === typeof m.nodeType }; a.isClass = function (m) { var f = m && m.constructor; return !(!a.isObject(m, !0) || a.isDOMElement(m) || !f || !f.name || "Object" === f.name) }; a.isNumber = function (a) { return "number" === typeof a && !isNaN(a) }; a.erase = function (a, f) { for (var m = a.length; m--;) if (a[m] === f) { a.splice(m, 1); break } }; a.defined = function (a) { return void 0 !== a && null !== a }; a.attr = function (m, f, l) {
            var r; a.isString(f) ? a.defined(l) ?
            m.setAttribute(f, l) : m && m.getAttribute && (r = m.getAttribute(f)) : a.defined(f) && a.isObject(f) && a.objectEach(f, function (a, f) { m.setAttribute(f, a) }); return r
        }; a.splat = function (m) { return a.isArray(m) ? m : [m] }; a.syncTimeout = function (a, f, l) { if (f) return setTimeout(a, f, l); a.call(0, l) }; a.pick = function () { var a = arguments, f, l, r = a.length; for (f = 0; f < r; f++) if (l = a[f], void 0 !== l && null !== l) return l }; a.css = function (m, f) {
            a.isMS && !a.svg && f && void 0 !== f.opacity && (f.filter = "alpha(opacity\x3d" + 100 * f.opacity + ")"); a.extend(m.style,
            f)
        }; a.createElement = function (m, f, l, r, u) { m = F.createElement(m); var t = a.css; f && a.extend(m, f); u && t(m, { padding: 0, border: "none", margin: 0 }); l && t(m, l); r && r.appendChild(m); return m }; a.extendClass = function (m, f) { var l = function () { }; l.prototype = new m; a.extend(l.prototype, f); return l }; a.pad = function (a, f, l) { return Array((f || 2) + 1 - String(a).length).join(l || 0) + a }; a.relativeLength = function (a, f, l) { return /%$/.test(a) ? f * parseFloat(a) / 100 + (l || 0) : parseFloat(a) }; a.wrap = function (a, f, l) {
            var r = a[f]; a[f] = function () {
                var a = Array.prototype.slice.call(arguments),
                f = arguments, g = this; g.proceed = function () { r.apply(g, arguments.length ? arguments : f) }; a.unshift(r); a = l.apply(this, a); g.proceed = null; return a
            }
        }; a.getTZOffset = function (m) { var f = a.Date; return 6E4 * (f.hcGetTimezoneOffset && f.hcGetTimezoneOffset(m) || f.hcTimezoneOffset || 0) }; a.dateFormat = function (m, f, l) {
            if (!a.defined(f) || isNaN(f)) return a.defaultOptions.lang.invalidDate || ""; m = a.pick(m, "%Y-%m-%d %H:%M:%S"); var r = a.Date, u = new r(f - a.getTZOffset(f)), t = u[r.hcGetHours](), g = u[r.hcGetDay](), d = u[r.hcGetDate](), k = u[r.hcGetMonth](),
            b = u[r.hcGetFullYear](), e = a.defaultOptions.lang, v = e.weekdays, y = e.shortWeekdays, n = a.pad, r = a.extend({ a: y ? y[g] : v[g].substr(0, 3), A: v[g], d: n(d), e: n(d, 2, " "), w: g, b: e.shortMonths[k], B: e.months[k], m: n(k + 1), y: b.toString().substr(2, 2), Y: b, H: n(t), k: t, I: n(t % 12 || 12), l: t % 12 || 12, M: n(u[r.hcGetMinutes]()), p: 12 > t ? "AM" : "PM", P: 12 > t ? "am" : "pm", S: n(u.getSeconds()), L: n(Math.round(f % 1E3), 3) }, a.dateFormats); a.objectEach(r, function (a, e) { for (; -1 !== m.indexOf("%" + e) ;) m = m.replace("%" + e, "function" === typeof a ? a(f) : a) }); return l ? m.substr(0,
            1).toUpperCase() + m.substr(1) : m
        }; a.formatSingle = function (m, f) { var l = /\.([0-9])/, r = a.defaultOptions.lang; /f$/.test(m) ? (l = (l = m.match(l)) ? l[1] : -1, null !== f && (f = a.numberFormat(f, l, r.decimalPoint, -1 < m.indexOf(",") ? r.thousandsSep : ""))) : f = a.dateFormat(m, f); return f }; a.format = function (m, f) {
            for (var l = "{", r = !1, u, t, g, d, k = [], b; m;) {
                l = m.indexOf(l); if (-1 === l) break; u = m.slice(0, l); if (r) { u = u.split(":"); t = u.shift().split("."); d = t.length; b = f; for (g = 0; g < d; g++) b = b[t[g]]; u.length && (b = a.formatSingle(u.join(":"), b)); k.push(b) } else k.push(u);
                m = m.slice(l + 1); l = (r = !r) ? "}" : "{"
            } k.push(m); return k.join("")
        }; a.getMagnitude = function (a) { return Math.pow(10, Math.floor(Math.log(a) / Math.LN10)) }; a.normalizeTickInterval = function (m, f, l, r, u) { var t, g = m; l = a.pick(l, 1); t = m / l; f || (f = u ? [1, 1.2, 1.5, 2, 2.5, 3, 4, 5, 6, 8, 10] : [1, 2, 2.5, 5, 10], !1 === r && (1 === l ? f = a.grep(f, function (a) { return 0 === a % 1 }) : .1 >= l && (f = [1 / l]))); for (r = 0; r < f.length && !(g = f[r], u && g * l >= m || !u && t <= (f[r] + (f[r + 1] || f[r])) / 2) ; r++); return g = a.correctFloat(g * l, -Math.round(Math.log(.001) / Math.LN10)) }; a.stableSort =
        function (a, f) { var l = a.length, r, m; for (m = 0; m < l; m++) a[m].safeI = m; a.sort(function (a, g) { r = f(a, g); return 0 === r ? a.safeI - g.safeI : r }); for (m = 0; m < l; m++) delete a[m].safeI }; a.arrayMin = function (a) { for (var f = a.length, l = a[0]; f--;) a[f] < l && (l = a[f]); return l }; a.arrayMax = function (a) { for (var f = a.length, l = a[0]; f--;) a[f] > l && (l = a[f]); return l }; a.destroyObjectProperties = function (m, f) { a.objectEach(m, function (a, r) { a && a !== f && a.destroy && a.destroy(); delete m[r] }) }; a.discardElement = function (m) {
            var f = a.garbageBin; f || (f = a.createElement("div"));
            m && f.appendChild(m); f.innerHTML = ""
        }; a.correctFloat = function (a, f) { return parseFloat(a.toPrecision(f || 14)) }; a.setAnimation = function (m, f) { f.renderer.globalAnimation = a.pick(m, f.options.chart.animation, !0) }; a.animObject = function (m) { return a.isObject(m) ? a.merge(m) : { duration: m ? 500 : 0 } }; a.timeUnits = { millisecond: 1, second: 1E3, minute: 6E4, hour: 36E5, day: 864E5, week: 6048E5, month: 24192E5, year: 314496E5 }; a.numberFormat = function (m, f, l, r) {
            m = +m || 0; f = +f; var u = a.defaultOptions.lang, t = (m.toString().split(".")[1] || "").split("e")[0].length,
            g, d, k = m.toString().split("e"); -1 === f ? f = Math.min(t, 20) : a.isNumber(f) || (f = 2); d = (Math.abs(k[1] ? k[0] : m) + Math.pow(10, -Math.max(f, t) - 1)).toFixed(f); t = String(a.pInt(d)); g = 3 < t.length ? t.length % 3 : 0; l = a.pick(l, u.decimalPoint); r = a.pick(r, u.thousandsSep); m = (0 > m ? "-" : "") + (g ? t.substr(0, g) + r : ""); m += t.substr(g).replace(/(\d{3})(?=\d)/g, "$1" + r); f && (m += l + d.slice(-f)); k[1] && (m += "e" + k[1]); return m
        }; Math.easeInOutSine = function (a) { return -.5 * (Math.cos(Math.PI * a) - 1) }; a.getStyle = function (m, f, l) {
            if ("width" === f) return Math.min(m.offsetWidth,
            m.scrollWidth) - a.getStyle(m, "padding-left") - a.getStyle(m, "padding-right"); if ("height" === f) return Math.min(m.offsetHeight, m.scrollHeight) - a.getStyle(m, "padding-top") - a.getStyle(m, "padding-bottom"); if (m = E.getComputedStyle(m, void 0)) m = m.getPropertyValue(f), a.pick(l, !0) && (m = a.pInt(m)); return m
        }; a.inArray = function (a, f) { return f.indexOf ? f.indexOf(a) : [].indexOf.call(f, a) }; a.grep = function (a, f) { return [].filter.call(a, f) }; a.find = function (a, f) { return [].find.call(a, f) }; a.map = function (a, f) {
            for (var l = [], r = 0, m =
            a.length; r < m; r++) l[r] = f.call(a[r], a[r], r, a); return l
        }; a.offset = function (a) { var f = F.documentElement; a = a.getBoundingClientRect(); return { top: a.top + (E.pageYOffset || f.scrollTop) - (f.clientTop || 0), left: a.left + (E.pageXOffset || f.scrollLeft) - (f.clientLeft || 0) } }; a.stop = function (a, f) { for (var l = C.length; l--;) C[l].elem !== a || f && f !== C[l].prop || (C[l].stopped = !0) }; a.each = function (a, f, l) { return Array.prototype.forEach.call(a, f, l) }; a.objectEach = function (a, f, l) { for (var r in a) a.hasOwnProperty(r) && f.call(l, a[r], r, a) };
        a.addEvent = function (m, f, l) { function r(a) { a.target = a.srcElement || E; l.call(m, a) } var u = m.hcEvents = m.hcEvents || {}; m.addEventListener ? m.addEventListener(f, l, !1) : m.attachEvent && (m.hcEventsIE || (m.hcEventsIE = {}), l.hcGetKey || (l.hcGetKey = a.uniqueKey()), m.hcEventsIE[l.hcGetKey] = r, m.attachEvent("on" + f, r)); u[f] || (u[f] = []); u[f].push(l); return function () { a.removeEvent(m, f, l) } }; a.removeEvent = function (m, f, l) {
            function r(a, b) {
                m.removeEventListener ? m.removeEventListener(a, b, !1) : m.attachEvent && (b = m.hcEventsIE[b.hcGetKey],
                m.detachEvent("on" + a, b))
            } function u() { var d, b; m.nodeName && (f ? (d = {}, d[f] = !0) : d = g, a.objectEach(d, function (a, d) { if (g[d]) for (b = g[d].length; b--;) r(d, g[d][b]) })) } var t, g = m.hcEvents, d; g && (f ? (t = g[f] || [], l ? (d = a.inArray(l, t), -1 < d && (t.splice(d, 1), g[f] = t), r(f, l)) : (u(), g[f] = [])) : (u(), m.hcEvents = {}))
        }; a.fireEvent = function (m, f, l, r) {
            var u; u = m.hcEvents; var t, g; l = l || {}; if (F.createEvent && (m.dispatchEvent || m.fireEvent)) u = F.createEvent("Events"), u.initEvent(f, !0, !0), a.extend(u, l), m.dispatchEvent ? m.dispatchEvent(u) : m.fireEvent(f,
            u); else if (u) for (u = u[f] || [], t = u.length, l.target || a.extend(l, { preventDefault: function () { l.defaultPrevented = !0 }, target: m, type: f }), f = 0; f < t; f++) (g = u[f]) && !1 === g.call(m, l) && l.preventDefault(); r && !l.defaultPrevented && r(l)
        }; a.animate = function (m, f, l) {
            var r, u = "", t, g, d; a.isObject(l) || (d = arguments, l = { duration: d[2], easing: d[3], complete: d[4] }); a.isNumber(l.duration) || (l.duration = 400); l.easing = "function" === typeof l.easing ? l.easing : Math[l.easing] || Math.easeInOutSine; l.curAnim = a.merge(f); a.objectEach(f, function (d,
            b) { a.stop(m, b); g = new a.Fx(m, l, b); t = null; "d" === b ? (g.paths = g.initPath(m, m.d, f.d), g.toD = f.d, r = 0, t = 1) : m.attr ? r = m.attr(b) : (r = parseFloat(a.getStyle(m, b)) || 0, "opacity" !== b && (u = "px")); t || (t = d); t && t.match && t.match("px") && (t = t.replace(/px/g, "")); g.run(r, t, u) })
        }; a.seriesType = function (m, f, l, r, u) { var t = a.getOptions(), g = a.seriesTypes; t.plotOptions[m] = a.merge(t.plotOptions[f], l); g[m] = a.extendClass(g[f] || function () { }, r); g[m].prototype.type = m; u && (g[m].prototype.pointClass = a.extendClass(a.Point, u)); return g[m] }; a.uniqueKey =
        function () { var a = Math.random().toString(36).substring(2, 9), f = 0; return function () { return "highcharts-" + a + "-" + f++ } }(); E.jQuery && (E.jQuery.fn.highcharts = function () { var m = [].slice.call(arguments); if (this[0]) return m[0] ? (new (a[a.isString(m[0]) ? m.shift() : "Chart"])(this[0], m[0], m[1]), this) : A[a.attr(this[0], "data-highcharts-chart")] }); F && !F.defaultView && (a.getStyle = function (m, f) {
            var l = { width: "clientWidth", height: "clientHeight" }[f]; if (m.style[f]) return a.pInt(m.style[f]); "opacity" === f && (f = "filter"); if (l) return m.style.zoom =
            1, Math.max(m[l] - 2 * a.getStyle(m, "padding"), 0); m = m.currentStyle[f.replace(/\-(\w)/g, function (a, f) { return f.toUpperCase() })]; "filter" === f && (m = m.replace(/alpha\(opacity=([0-9]+)\)/, function (a, f) { return f / 100 })); return "" === m ? 1 : a.pInt(m)
        }); Array.prototype.forEach || (a.each = function (a, f, l) { for (var r = 0, m = a.length; r < m; r++) if (!1 === f.call(l, a[r], r, a)) return r }); Array.prototype.indexOf || (a.inArray = function (a, f) { var l, r = 0; if (f) for (l = f.length; r < l; r++) if (f[r] === a) return r; return -1 }); Array.prototype.filter || (a.grep =
        function (a, f) { for (var l = [], r = 0, m = a.length; r < m; r++) f(a[r], r) && l.push(a[r]); return l }); Array.prototype.find || (a.find = function (a, f) { var l, r = a.length; for (l = 0; l < r; l++) if (f(a[l], l)) return a[l] })
    })(M); (function (a) {
        var C = a.each, A = a.isNumber, F = a.map, E = a.merge, m = a.pInt; a.Color = function (f) { if (!(this instanceof a.Color)) return new a.Color(f); this.init(f) }; a.Color.prototype = {
            parsers: [{
                regex: /rgba\(\s*([0-9]{1,3})\s*,\s*([0-9]{1,3})\s*,\s*([0-9]{1,3})\s*,\s*([0-9]?(?:\.[0-9]+)?)\s*\)/, parse: function (a) {
                    return [m(a[1]),
                    m(a[2]), m(a[3]), parseFloat(a[4], 10)]
                }
            }, { regex: /rgb\(\s*([0-9]{1,3})\s*,\s*([0-9]{1,3})\s*,\s*([0-9]{1,3})\s*\)/, parse: function (a) { return [m(a[1]), m(a[2]), m(a[3]), 1] } }], names: { none: "rgba(255,255,255,0)", white: "#ffffff", black: "#000000" }, init: function (f) {
                var l, r, m, t; if ((this.input = f = this.names[f && f.toLowerCase ? f.toLowerCase() : ""] || f) && f.stops) this.stops = F(f.stops, function (g) { return new a.Color(g[1]) }); else if (f && "#" === f.charAt() && (l = f.length, f = parseInt(f.substr(1), 16), 7 === l ? r = [(f & 16711680) >> 16, (f & 65280) >>
                8, f & 255, 1] : 4 === l && (r = [(f & 3840) >> 4 | (f & 3840) >> 8, (f & 240) >> 4 | f & 240, (f & 15) << 4 | f & 15, 1])), !r) for (m = this.parsers.length; m-- && !r;) t = this.parsers[m], (l = t.regex.exec(f)) && (r = t.parse(l)); this.rgba = r || []
            }, get: function (a) { var f = this.input, r = this.rgba, m; this.stops ? (m = E(f), m.stops = [].concat(m.stops), C(this.stops, function (f, g) { m.stops[g] = [m.stops[g][0], f.get(a)] })) : m = r && A(r[0]) ? "rgb" === a || !a && 1 === r[3] ? "rgb(" + r[0] + "," + r[1] + "," + r[2] + ")" : "a" === a ? r[3] : "rgba(" + r.join(",") + ")" : f; return m }, brighten: function (a) {
                var f, r = this.rgba;
                if (this.stops) C(this.stops, function (f) { f.brighten(a) }); else if (A(a) && 0 !== a) for (f = 0; 3 > f; f++) r[f] += m(255 * a), 0 > r[f] && (r[f] = 0), 255 < r[f] && (r[f] = 255); return this
            }, setOpacity: function (a) { this.rgba[3] = a; return this }, tweenTo: function (a, l) { var f, m; a.rgba.length ? (f = this.rgba, a = a.rgba, m = 1 !== a[3] || 1 !== f[3], a = (m ? "rgba(" : "rgb(") + Math.round(a[0] + (f[0] - a[0]) * (1 - l)) + "," + Math.round(a[1] + (f[1] - a[1]) * (1 - l)) + "," + Math.round(a[2] + (f[2] - a[2]) * (1 - l)) + (m ? "," + (a[3] + (f[3] - a[3]) * (1 - l)) : "") + ")") : a = a.input || "none"; return a }
        }; a.color =
        function (f) { return new a.Color(f) }
    })(M); (function (a) {
        var C, A, F = a.addEvent, E = a.animate, m = a.attr, f = a.charts, l = a.color, r = a.css, u = a.createElement, t = a.defined, g = a.deg2rad, d = a.destroyObjectProperties, k = a.doc, b = a.each, e = a.extend, v = a.erase, y = a.grep, n = a.hasTouch, D = a.inArray, J = a.isArray, c = a.isFirefox, G = a.isMS, q = a.isObject, B = a.isString, K = a.isWebKit, p = a.merge, z = a.noop, I = a.objectEach, L = a.pick, h = a.pInt, w = a.removeEvent, P = a.stop, H = a.svg, O = a.SVG_NS, Q = a.symbolSizes, R = a.win; C = a.SVGElement = function () { return this }; e(C.prototype,
        {
            opacity: 1, SVG_NS: O, textProps: "direction fontSize fontWeight fontFamily fontStyle color lineHeight width textAlign textDecoration textOverflow textOutline".split(" "), init: function (a, h) { this.element = "span" === h ? u(h) : k.createElementNS(this.SVG_NS, h); this.renderer = a }, animate: function (x, h, c) { h = a.animObject(L(h, this.renderer.globalAnimation, !0)); 0 !== h.duration ? (c && (h.complete = c), E(this, x, h)) : (this.attr(x, null, c), h.step && h.step.call(this)); return this }, colorGradient: function (x, h, c) {
                var w = this.renderer,
                e, q, N, d, n, g, k, H, G, v, z = [], f; x.radialGradient ? q = "radialGradient" : x.linearGradient && (q = "linearGradient"); q && (N = x[q], n = w.gradients, k = x.stops, v = c.radialReference, J(N) && (x[q] = N = { x1: N[0], y1: N[1], x2: N[2], y2: N[3], gradientUnits: "userSpaceOnUse" }), "radialGradient" === q && v && !t(N.gradientUnits) && (d = N, N = p(N, w.getRadialAttr(v, d), { gradientUnits: "userSpaceOnUse" })), I(N, function (a, x) { "id" !== x && z.push(x, a) }), I(k, function (a) { z.push(a) }), z = z.join(","), n[z] ? v = n[z].attr("id") : (N.id = v = a.uniqueKey(), n[z] = g = w.createElement(q).attr(N).add(w.defs),
                g.radAttr = d, g.stops = [], b(k, function (x) { 0 === x[1].indexOf("rgba") ? (e = a.color(x[1]), H = e.get("rgb"), G = e.get("a")) : (H = x[1], G = 1); x = w.createElement("stop").attr({ offset: x[0], "stop-color": H, "stop-opacity": G }).add(g); g.stops.push(x) })), f = "url(" + w.url + "#" + v + ")", c.setAttribute(h, f), c.gradient = z, x.toString = function () { return f })
            }, applyTextOutline: function (x) {
                var h = this.element, c, w, p, e, q; -1 !== x.indexOf("contrast") && (x = x.replace(/contrast/g, this.renderer.getContrast(h.style.fill))); x = x.split(" "); w = x[x.length - 1];
                if ((p = x[0]) && "none" !== p && a.svg) {
                    this.fakeTS = !0; x = [].slice.call(h.getElementsByTagName("tspan")); this.ySetter = this.xSetter; p = p.replace(/(^[\d\.]+)(.*?)$/g, function (a, x, h) { return 2 * x + h }); for (q = x.length; q--;) c = x[q], "highcharts-text-outline" === c.getAttribute("class") && v(x, h.removeChild(c)); e = h.firstChild; b(x, function (a, x) {
                        0 === x && (a.setAttribute("x", h.getAttribute("x")), x = h.getAttribute("y"), a.setAttribute("y", x || 0), null === x && h.setAttribute("y", 0)); a = a.cloneNode(1); m(a, {
                            "class": "highcharts-text-outline",
                            fill: w, stroke: w, "stroke-width": p, "stroke-linejoin": "round"
                        }); h.insertBefore(a, e)
                    })
                }
            }, attr: function (a, h, c, w) {
                var x, p = this.element, e, q = this, b, N; "string" === typeof a && void 0 !== h && (x = a, a = {}, a[x] = h); "string" === typeof a ? q = (this[a + "Getter"] || this._defaultGetter).call(this, a, p) : (I(a, function (x, h) {
                    b = !1; w || P(this, h); this.symbolName && /^(x|y|width|height|r|start|end|innerR|anchorX|anchorY)$/.test(h) && (e || (this.symbolAttr(a), e = !0), b = !0); !this.rotation || "x" !== h && "y" !== h || (this.doTransform = !0); b || (N = this[h + "Setter"] ||
                    this._defaultSetter, N.call(this, x, h, p), this.shadows && /^(width|height|visibility|x|y|d|transform|cx|cy|r)$/.test(h) && this.updateShadows(h, x, N))
                }, this), this.afterSetters()); c && c(); return q
            }, afterSetters: function () { this.doTransform && (this.updateTransform(), this.doTransform = !1) }, updateShadows: function (a, h, c) { for (var x = this.shadows, w = x.length; w--;) c.call(x[w], "height" === a ? Math.max(h - (x[w].cutHeight || 0), 0) : "d" === a ? this.d : h, a, x[w]) }, addClass: function (a, h) {
                var x = this.attr("class") || ""; -1 === x.indexOf(a) &&
                (h || (a = (x + (x ? " " : "") + a).replace("  ", " ")), this.attr("class", a)); return this
            }, hasClass: function (a) { return -1 !== D(a, (this.attr("class") || "").split(" ")) }, removeClass: function (a) { return this.attr("class", (this.attr("class") || "").replace(a, "")) }, symbolAttr: function (a) { var x = this; b("x y r start end width height innerR anchorX anchorY".split(" "), function (h) { x[h] = L(a[h], x[h]) }); x.attr({ d: x.renderer.symbols[x.symbolName](x.x, x.y, x.width, x.height, x) }) }, clip: function (a) {
                return this.attr("clip-path", a ? "url(" +
                this.renderer.url + "#" + a.id + ")" : "none")
            }, crisp: function (a, h) { var x = this, c = {}, w; h = h || a.strokeWidth || 0; w = Math.round(h) % 2 / 2; a.x = Math.floor(a.x || x.x || 0) + w; a.y = Math.floor(a.y || x.y || 0) + w; a.width = Math.floor((a.width || x.width || 0) - 2 * w); a.height = Math.floor((a.height || x.height || 0) - 2 * w); t(a.strokeWidth) && (a.strokeWidth = h); I(a, function (a, h) { x[h] !== a && (x[h] = c[h] = a) }); return c }, css: function (a) {
                var x = this.styles, c = {}, w = this.element, p, q = "", b, d = !x, n = ["textOutline", "textOverflow", "width"]; a && a.color && (a.fill = a.color);
                x && I(a, function (a, h) { a !== x[h] && (c[h] = a, d = !0) }); d && (x && (a = e(x, c)), p = this.textWidth = a && a.width && "auto" !== a.width && "text" === w.nodeName.toLowerCase() && h(a.width), this.styles = a, p && !H && this.renderer.forExport && delete a.width, G && !H ? r(this.element, a) : (b = function (a, x) { return "-" + x.toLowerCase() }, I(a, function (a, x) { -1 === D(x, n) && (q += x.replace(/([A-Z])/g, b) + ":" + a + ";") }), q && m(w, "style", q)), this.added && ("text" === this.element.nodeName && this.renderer.buildText(this), a && a.textOutline && this.applyTextOutline(a.textOutline)));
                return this
            }, strokeWidth: function () { return this["stroke-width"] || 0 }, on: function (a, h) { var x = this, c = x.element; n && "click" === a ? (c.ontouchstart = function (a) { x.touchEventFired = Date.now(); a.preventDefault(); h.call(c, a) }, c.onclick = function (a) { (-1 === R.navigator.userAgent.indexOf("Android") || 1100 < Date.now() - (x.touchEventFired || 0)) && h.call(c, a) }) : c["on" + a] = h; return this }, setRadialReference: function (a) {
                var x = this.renderer.gradients[this.element.gradient]; this.element.radialReference = a; x && x.radAttr && x.animate(this.renderer.getRadialAttr(a,
                x.radAttr)); return this
            }, translate: function (a, h) { return this.attr({ translateX: a, translateY: h }) }, invert: function (a) { this.inverted = a; this.updateTransform(); return this }, updateTransform: function () {
                var a = this.translateX || 0, h = this.translateY || 0, c = this.scaleX, w = this.scaleY, p = this.inverted, e = this.rotation, q = this.element; p && (a += this.width, h += this.height); a = ["translate(" + a + "," + h + ")"]; p ? a.push("rotate(90) scale(-1,1)") : e && a.push("rotate(" + e + " " + (q.getAttribute("x") || 0) + " " + (q.getAttribute("y") || 0) + ")"); (t(c) ||
                t(w)) && a.push("scale(" + L(c, 1) + " " + L(w, 1) + ")"); a.length && q.setAttribute("transform", a.join(" "))
            }, toFront: function () { var a = this.element; a.parentNode.appendChild(a); return this }, align: function (a, h, c) {
                var x, w, p, e, q = {}; w = this.renderer; p = w.alignedObjects; var b, d; if (a) { if (this.alignOptions = a, this.alignByTranslate = h, !c || B(c)) this.alignTo = x = c || "renderer", v(p, this), p.push(this), c = null } else a = this.alignOptions, h = this.alignByTranslate, x = this.alignTo; c = L(c, w[x], w); x = a.align; w = a.verticalAlign; p = (c.x || 0) + (a.x ||
                0); e = (c.y || 0) + (a.y || 0); "right" === x ? b = 1 : "center" === x && (b = 2); b && (p += (c.width - (a.width || 0)) / b); q[h ? "translateX" : "x"] = Math.round(p); "bottom" === w ? d = 1 : "middle" === w && (d = 2); d && (e += (c.height - (a.height || 0)) / d); q[h ? "translateY" : "y"] = Math.round(e); this[this.placed ? "animate" : "attr"](q); this.placed = !0; this.alignAttr = q; return this
            }, getBBox: function (a, h) {
                var x, c = this.renderer, w, p = this.element, q = this.styles, d, n = this.textStr, k, N = c.cache, H = c.cacheKeys, G; h = L(h, this.rotation); w = h * g; d = q && q.fontSize; void 0 !== n && (G = n.toString(),
                -1 === G.indexOf("\x3c") && (G = G.replace(/[0-9]/g, "0")), G += ["", h || 0, d, q && q.width, q && q.textOverflow].join()); G && !a && (x = N[G]); if (!x) {
                    if (p.namespaceURI === this.SVG_NS || c.forExport) { try { (k = this.fakeTS && function (a) { b(p.querySelectorAll(".highcharts-text-outline"), function (x) { x.style.display = a }) }) && k("none"), x = p.getBBox ? e({}, p.getBBox()) : { width: p.offsetWidth, height: p.offsetHeight }, k && k("") } catch (W) { } if (!x || 0 > x.width) x = { width: 0, height: 0 } } else x = this.htmlGetBBox(); c.isSVG && (a = x.width, c = x.height, q && "11px" === q.fontSize &&
                    17 === Math.round(c) && (x.height = c = 14), h && (x.width = Math.abs(c * Math.sin(w)) + Math.abs(a * Math.cos(w)), x.height = Math.abs(c * Math.cos(w)) + Math.abs(a * Math.sin(w)))); if (G && 0 < x.height) { for (; 250 < H.length;) delete N[H.shift()]; N[G] || H.push(G); N[G] = x }
                } return x
            }, show: function (a) { return this.attr({ visibility: a ? "inherit" : "visible" }) }, hide: function () { return this.attr({ visibility: "hidden" }) }, fadeOut: function (a) { var x = this; x.animate({ opacity: 0 }, { duration: a || 150, complete: function () { x.attr({ y: -9999 }) } }) }, add: function (a) {
                var x =
                this.renderer, h = this.element, c; a && (this.parentGroup = a); this.parentInverted = a && a.inverted; void 0 !== this.textStr && x.buildText(this); this.added = !0; if (!a || a.handleZ || this.zIndex) c = this.zIndexSetter(); c || (a ? a.element : x.box).appendChild(h); if (this.onAdd) this.onAdd(); return this
            }, safeRemoveChild: function (a) { var x = a.parentNode; x && x.removeChild(a) }, destroy: function () {
                var a = this, h = a.element || {}, c = a.renderer.isSVG && "SPAN" === h.nodeName && a.parentGroup, w = h.ownerSVGElement; h.onclick = h.onmouseout = h.onmouseover = h.onmousemove =
                h.point = null; P(a); a.clipPath && w && (b(w.querySelectorAll("[clip-path]"), function (x) { -1 < x.getAttribute("clip-path").indexOf(a.clipPath.element.id + ")") && x.removeAttribute("clip-path") }), a.clipPath = a.clipPath.destroy()); if (a.stops) { for (w = 0; w < a.stops.length; w++) a.stops[w] = a.stops[w].destroy(); a.stops = null } a.safeRemoveChild(h); for (a.destroyShadows() ; c && c.div && 0 === c.div.childNodes.length;) h = c.parentGroup, a.safeRemoveChild(c.div), delete c.div, c = h; a.alignTo && v(a.renderer.alignedObjects, a); I(a, function (x, h) { delete a[h] });
                return null
            }, shadow: function (a, h, c) {
                var x = [], w, p, q = this.element, e, b, d, n; if (!a) this.destroyShadows(); else if (!this.shadows) {
                    b = L(a.width, 3); d = (a.opacity || .15) / b; n = this.parentInverted ? "(-1,-1)" : "(" + L(a.offsetX, 1) + ", " + L(a.offsetY, 1) + ")"; for (w = 1; w <= b; w++) p = q.cloneNode(0), e = 2 * b + 1 - 2 * w, m(p, { isShadow: "true", stroke: a.color || "#000000", "stroke-opacity": d * w, "stroke-width": e, transform: "translate" + n, fill: "none" }), c && (m(p, "height", Math.max(m(p, "height") - e, 0)), p.cutHeight = e), h ? h.element.appendChild(p) : q.parentNode.insertBefore(p,
                    q), x.push(p); this.shadows = x
                } return this
            }, destroyShadows: function () { b(this.shadows || [], function (a) { this.safeRemoveChild(a) }, this); this.shadows = void 0 }, xGetter: function (a) { "circle" === this.element.nodeName && ("x" === a ? a = "cx" : "y" === a && (a = "cy")); return this._defaultGetter(a) }, _defaultGetter: function (a) { a = L(this[a], this.element ? this.element.getAttribute(a) : null, 0); /^[\-0-9\.]+$/.test(a) && (a = parseFloat(a)); return a }, dSetter: function (a, h, c) {
                a && a.join && (a = a.join(" ")); /(NaN| {2}|^$)/.test(a) && (a = "M 0 0"); this[h] !==
                a && (c.setAttribute(h, a), this[h] = a)
            }, dashstyleSetter: function (a) { var x, c = this["stroke-width"]; "inherit" === c && (c = 1); if (a = a && a.toLowerCase()) { a = a.replace("shortdashdotdot", "3,1,1,1,1,1,").replace("shortdashdot", "3,1,1,1").replace("shortdot", "1,1,").replace("shortdash", "3,1,").replace("longdash", "8,3,").replace(/dot/g, "1,3,").replace("dash", "4,3,").replace(/,$/, "").split(","); for (x = a.length; x--;) a[x] = h(a[x]) * c; a = a.join(",").replace(/NaN/g, "none"); this.element.setAttribute("stroke-dasharray", a) } }, alignSetter: function (a) {
                this.element.setAttribute("text-anchor",
                { left: "start", center: "middle", right: "end" }[a])
            }, opacitySetter: function (a, h, c) { this[h] = a; c.setAttribute(h, a) }, titleSetter: function (a) { var h = this.element.getElementsByTagName("title")[0]; h || (h = k.createElementNS(this.SVG_NS, "title"), this.element.appendChild(h)); h.firstChild && h.removeChild(h.firstChild); h.appendChild(k.createTextNode(String(L(a), "").replace(/<[^>]*>/g, ""))) }, textSetter: function (a) { a !== this.textStr && (delete this.bBox, this.textStr = a, this.added && this.renderer.buildText(this)) }, fillSetter: function (a,
            h, c) { "string" === typeof a ? c.setAttribute(h, a) : a && this.colorGradient(a, h, c) }, visibilitySetter: function (a, h, c) { "inherit" === a ? c.removeAttribute(h) : this[h] !== a && c.setAttribute(h, a); this[h] = a }, zIndexSetter: function (a, c) {
                var x = this.renderer, w = this.parentGroup, p = (w || x).element || x.box, q, e = this.element, b; q = this.added; var d; t(a) && (e.zIndex = a, a = +a, this[c] === a && (q = !1), this[c] = a); if (q) {
                    (a = this.zIndex) && w && (w.handleZ = !0); c = p.childNodes; for (d = 0; d < c.length && !b; d++) w = c[d], q = w.zIndex, w !== e && (h(q) > a || !t(a) && t(q) || 0 >
                    a && !t(q) && p !== x.box) && (p.insertBefore(e, w), b = !0); b || p.appendChild(e)
                } return b
            }, _defaultSetter: function (a, h, c) { c.setAttribute(h, a) }
        }); C.prototype.yGetter = C.prototype.xGetter; C.prototype.translateXSetter = C.prototype.translateYSetter = C.prototype.rotationSetter = C.prototype.verticalAlignSetter = C.prototype.scaleXSetter = C.prototype.scaleYSetter = function (a, h) { this[h] = a; this.doTransform = !0 }; C.prototype["stroke-widthSetter"] = C.prototype.strokeSetter = function (a, h, c) {
            this[h] = a; this.stroke && this["stroke-width"] ?
            (C.prototype.fillSetter.call(this, this.stroke, "stroke", c), c.setAttribute("stroke-width", this["stroke-width"]), this.hasStroke = !0) : "stroke-width" === h && 0 === a && this.hasStroke && (c.removeAttribute("stroke"), this.hasStroke = !1)
        }; A = a.SVGRenderer = function () { this.init.apply(this, arguments) }; e(A.prototype, {
            Element: C, SVG_NS: O, init: function (a, h, w, p, q, e) {
                var x; p = this.createElement("svg").attr({ version: "1.1", "class": "highcharts-root" }).css(this.getStyle(p)); x = p.element; a.appendChild(x); -1 === a.innerHTML.indexOf("xmlns") &&
                m(x, "xmlns", this.SVG_NS); this.isSVG = !0; this.box = x; this.boxWrapper = p; this.alignedObjects = []; this.url = (c || K) && k.getElementsByTagName("base").length ? R.location.href.replace(/#.*?$/, "").replace(/<[^>]*>/g, "").replace(/([\('\)])/g, "\\$1").replace(/ /g, "%20") : ""; this.createElement("desc").add().element.appendChild(k.createTextNode("Created with Highcharts 5.0.14")); this.defs = this.createElement("defs").add(); this.allowHTML = e; this.forExport = q; this.gradients = {}; this.cache = {}; this.cacheKeys = []; this.imgCount =
                0; this.setSize(h, w, !1); var b; c && a.getBoundingClientRect && (h = function () { r(a, { left: 0, top: 0 }); b = a.getBoundingClientRect(); r(a, { left: Math.ceil(b.left) - b.left + "px", top: Math.ceil(b.top) - b.top + "px" }) }, h(), this.unSubPixelFix = F(R, "resize", h))
            }, getStyle: function (a) { return this.style = e({ fontFamily: '"Lucida Grande", "Lucida Sans Unicode", Arial, Helvetica, sans-serif', fontSize: "12px" }, a) }, setStyle: function (a) { this.boxWrapper.css(this.getStyle(a)) }, isHidden: function () { return !this.boxWrapper.getBBox().width }, destroy: function () {
                var a =
                this.defs; this.box = null; this.boxWrapper = this.boxWrapper.destroy(); d(this.gradients || {}); this.gradients = null; a && (this.defs = a.destroy()); this.unSubPixelFix && this.unSubPixelFix(); return this.alignedObjects = null
            }, createElement: function (a) { var h = new this.Element; h.init(this, a); return h }, draw: z, getRadialAttr: function (a, h) { return { cx: a[0] - a[2] / 2 + h.cx * a[2], cy: a[1] - a[2] / 2 + h.cy * a[2], r: h.r * a[2] } }, getSpanWidth: function (a, h) {
                var c = a.getBBox(!0).width; !H && this.forExport && (c = this.measureSpanWidth(h.firstChild.data,
                a.styles)); return c
            }, applyEllipsis: function (a, h, c, w) { var x = a.rotation, p = c, q, e = 0, b = c.length, d = function (a) { h.removeChild(h.firstChild); a && h.appendChild(k.createTextNode(a)) }, n; a.rotation = 0; p = this.getSpanWidth(a, h); if (n = p > w) { for (; e <= b;) q = Math.ceil((e + b) / 2), p = c.substring(0, q) + "\u2026", d(p), p = this.getSpanWidth(a, h), e === b ? e = b + 1 : p > w ? b = q - 1 : e = q; 0 === b && d("") } a.rotation = x; return n }, buildText: function (a) {
                var c = a.element, w = this, x = w.forExport, p = L(a.textStr, "").toString(), q = -1 !== p.indexOf("\x3c"), e = c.childNodes,
                d, n, g, G, v = m(c, "x"), z = a.styles, f = a.textWidth, I = z && z.lineHeight, B = z && z.textOutline, D = z && "ellipsis" === z.textOverflow, l = z && "nowrap" === z.whiteSpace, P = z && z.fontSize, t, J, u = e.length, z = f && !a.added && this.box, K = function (a) { var x; x = /(px|em)$/.test(a && a.style.fontSize) ? a.style.fontSize : P || w.style.fontSize || 12; return I ? h(I) : w.fontMetrics(x, a.getAttribute("style") ? a : c).h }; t = [p, D, l, I, B, P, f].join(); if (t !== a.textCache) {
                    for (a.textCache = t; u--;) c.removeChild(e[u]); q || B || D || f || -1 !== p.indexOf(" ") ? (d = /<.*class="([^"]+)".*>/,
                    n = /<.*style="([^"]+)".*>/, g = /<.*href="([^"]+)".*>/, z && z.appendChild(c), p = q ? p.replace(/<(b|strong)>/g, '\x3cspan style\x3d"font-weight:bold"\x3e').replace(/<(i|em)>/g, '\x3cspan style\x3d"font-style:italic"\x3e').replace(/<a/g, "\x3cspan").replace(/<\/(b|strong|i|em|a)>/g, "\x3c/span\x3e").split(/<br.*?>/g) : [p], p = y(p, function (a) { return "" !== a }), b(p, function (h, p) {
                        var q, e = 0; h = h.replace(/^\s+|\s+$/g, "").replace(/<span/g, "|||\x3cspan").replace(/<\/span>/g, "\x3c/span\x3e|||"); q = h.split("|||"); b(q, function (h) {
                            if ("" !==
                            h || 1 === q.length) {
                                var b = {}, z = k.createElementNS(w.SVG_NS, "tspan"), y, I; d.test(h) && (y = h.match(d)[1], m(z, "class", y)); n.test(h) && (I = h.match(n)[1].replace(/(;| |^)color([ :])/, "$1fill$2"), m(z, "style", I)); g.test(h) && !x && (m(z, "onclick", 'location.href\x3d"' + h.match(g)[1] + '"'), r(z, { cursor: "pointer" })); h = (h.replace(/<(.|\n)*?>/g, "") || " ").replace(/&lt;/g, "\x3c").replace(/&gt;/g, "\x3e"); if (" " !== h) {
                                    z.appendChild(k.createTextNode(h)); e ? b.dx = 0 : p && null !== v && (b.x = v); m(z, b); c.appendChild(z); !e && J && (!H && x && r(z, { display: "block" }),
                                    m(z, "dy", K(z))); if (f) {
                                        b = h.replace(/([^\^])-/g, "$1- ").split(" "); y = 1 < q.length || p || 1 < b.length && !l; var B = [], N, P = K(z), t = a.rotation; for (D && (G = w.applyEllipsis(a, z, h, f)) ; !D && y && (b.length || B.length) ;) a.rotation = 0, N = w.getSpanWidth(a, z), h = N > f, void 0 === G && (G = h), h && 1 !== b.length ? (z.removeChild(z.firstChild), B.unshift(b.pop())) : (b = B, B = [], b.length && !l && (z = k.createElementNS(O, "tspan"), m(z, { dy: P, x: v }), I && m(z, "style", I), c.appendChild(z)), N > f && (f = N)), b.length && z.appendChild(k.createTextNode(b.join(" ").replace(/- /g,
                                        "-"))); a.rotation = t
                                    } e++
                                }
                            }
                        }); J = J || c.childNodes.length
                    }), G && a.attr("title", a.textStr), z && z.removeChild(c), B && a.applyTextOutline && a.applyTextOutline(B)) : c.appendChild(k.createTextNode(p.replace(/&lt;/g, "\x3c").replace(/&gt;/g, "\x3e")))
                }
            }, getContrast: function (a) { a = l(a).rgba; return 510 < a[0] + a[1] + a[2] ? "#000000" : "#FFFFFF" }, button: function (a, h, c, w, q, b, d, n, g) {
                var x = this.label(a, h, c, g, null, null, null, null, "button"), k = 0; x.attr(p({ padding: 8, r: 2 }, q)); var z, H, v, f; q = p({
                    fill: "#f7f7f7", stroke: "#cccccc", "stroke-width": 1,
                    style: { color: "#333333", cursor: "pointer", fontWeight: "normal" }
                }, q); z = q.style; delete q.style; b = p(q, { fill: "#e6e6e6" }, b); H = b.style; delete b.style; d = p(q, { fill: "#e6ebf5", style: { color: "#000000", fontWeight: "bold" } }, d); v = d.style; delete d.style; n = p(q, { style: { color: "#cccccc" } }, n); f = n.style; delete n.style; F(x.element, G ? "mouseover" : "mouseenter", function () { 3 !== k && x.setState(1) }); F(x.element, G ? "mouseout" : "mouseleave", function () { 3 !== k && x.setState(k) }); x.setState = function (a) {
                    1 !== a && (x.state = k = a); x.removeClass(/highcharts-button-(normal|hover|pressed|disabled)/).addClass("highcharts-button-" +
                    ["normal", "hover", "pressed", "disabled"][a || 0]); x.attr([q, b, d, n][a || 0]).css([z, H, v, f][a || 0])
                }; x.attr(q).css(e({ cursor: "default" }, z)); return x.on("click", function (a) { 3 !== k && w.call(x, a) })
            }, crispLine: function (a, h) { a[1] === a[4] && (a[1] = a[4] = Math.round(a[1]) - h % 2 / 2); a[2] === a[5] && (a[2] = a[5] = Math.round(a[2]) + h % 2 / 2); return a }, path: function (a) { var h = { fill: "none" }; J(a) ? h.d = a : q(a) && e(h, a); return this.createElement("path").attr(h) }, circle: function (a, h, c) {
                a = q(a) ? a : { x: a, y: h, r: c }; h = this.createElement("circle"); h.xSetter =
                h.ySetter = function (a, h, c) { c.setAttribute("c" + h, a) }; return h.attr(a)
            }, arc: function (a, h, c, w, p, b) { q(a) ? (w = a, h = w.y, c = w.r, a = w.x) : w = { innerR: w, start: p, end: b }; a = this.symbol("arc", a, h, c, c, w); a.r = c; return a }, rect: function (a, h, c, w, p, b) { p = q(a) ? a.r : p; var x = this.createElement("rect"); a = q(a) ? a : void 0 === a ? {} : { x: a, y: h, width: Math.max(c, 0), height: Math.max(w, 0) }; void 0 !== b && (a.strokeWidth = b, a = x.crisp(a)); a.fill = "none"; p && (a.r = p); x.rSetter = function (a, h, c) { m(c, { rx: a, ry: a }) }; return x.attr(a) }, setSize: function (a, h, c) {
                var w =
                this.alignedObjects, p = w.length; this.width = a; this.height = h; for (this.boxWrapper.animate({ width: a, height: h }, { step: function () { this.attr({ viewBox: "0 0 " + this.attr("width") + " " + this.attr("height") }) }, duration: L(c, !0) ? void 0 : 0 }) ; p--;) w[p].align()
            }, g: function (a) { var h = this.createElement("g"); return a ? h.attr({ "class": "highcharts-" + a }) : h }, image: function (a, h, c, w, p) {
                var x = { preserveAspectRatio: "none" }; 1 < arguments.length && e(x, { x: h, y: c, width: w, height: p }); x = this.createElement("image").attr(x); x.element.setAttributeNS ?
                x.element.setAttributeNS("http://www.w3.org/1999/xlink", "href", a) : x.element.setAttribute("hc-svg-href", a); return x
            }, symbol: function (a, h, c, w, p, q) {
                var x = this, d, n = /^url\((.*?)\)$/, g = n.test(a), z = !g && (this.symbols[a] ? a : "circle"), G = z && this.symbols[z], H = t(h) && G && G.call(this.symbols, Math.round(h), Math.round(c), w, p, q), v, y; G ? (d = this.path(H), d.attr("fill", "none"), e(d, { symbolName: z, x: h, y: c, width: w, height: p }), q && e(d, q)) : g && (v = a.match(n)[1], d = this.image(v), d.imgwidth = L(Q[v] && Q[v].width, q && q.width), d.imgheight =
                L(Q[v] && Q[v].height, q && q.height), y = function () { d.attr({ width: d.width, height: d.height }) }, b(["width", "height"], function (a) { d[a + "Setter"] = function (a, h) { var c = {}, w = this["img" + h], p = "width" === h ? "translateX" : "translateY"; this[h] = a; t(w) && (this.element && this.element.setAttribute(h, w), this.alignByTranslate || (c[p] = ((this[h] || 0) - w) / 2, this.attr(c))) } }), t(h) && d.attr({ x: h, y: c }), d.isImg = !0, t(d.imgwidth) && t(d.imgheight) ? y() : (d.attr({ width: 0, height: 0 }), u("img", {
                    onload: function () {
                        var a = f[x.chartIndex]; 0 === this.width &&
                        (r(this, { position: "absolute", top: "-999em" }), k.body.appendChild(this)); Q[v] = { width: this.width, height: this.height }; d.imgwidth = this.width; d.imgheight = this.height; d.element && y(); this.parentNode && this.parentNode.removeChild(this); x.imgCount--; if (!x.imgCount && a && a.onload) a.onload()
                    }, src: v
                }), this.imgCount++)); return d
            }, symbols: {
                circle: function (a, h, c, w) { return this.arc(a + c / 2, h + w / 2, c / 2, w / 2, { start: 0, end: 2 * Math.PI, open: !1 }) }, square: function (a, h, c, w) { return ["M", a, h, "L", a + c, h, a + c, h + w, a, h + w, "Z"] }, triangle: function (a,
                h, c, w) { return ["M", a + c / 2, h, "L", a + c, h + w, a, h + w, "Z"] }, "triangle-down": function (a, h, c, w) { return ["M", a, h, "L", a + c, h, a + c / 2, h + w, "Z"] }, diamond: function (a, h, c, w) { return ["M", a + c / 2, h, "L", a + c, h + w / 2, a + c / 2, h + w, a, h + w / 2, "Z"] }, arc: function (a, h, c, w, p) {
                    var q = p.start, b = p.r || c, x = p.r || w || c, e = p.end - .001; c = p.innerR; w = L(p.open, .001 > Math.abs(p.end - p.start - 2 * Math.PI)); var d = Math.cos(q), n = Math.sin(q), g = Math.cos(e), e = Math.sin(e); p = .001 > p.end - q - Math.PI ? 0 : 1; b = ["M", a + b * d, h + x * n, "A", b, x, 0, p, 1, a + b * g, h + x * e]; t(c) && b.push(w ? "M" : "L", a + c *
                    g, h + c * e, "A", c, c, 0, p, 0, a + c * d, h + c * n); b.push(w ? "" : "Z"); return b
                }, callout: function (a, h, c, w, p) {
                    var q = Math.min(p && p.r || 0, c, w), b = q + 6, e = p && p.anchorX; p = p && p.anchorY; var d; d = ["M", a + q, h, "L", a + c - q, h, "C", a + c, h, a + c, h, a + c, h + q, "L", a + c, h + w - q, "C", a + c, h + w, a + c, h + w, a + c - q, h + w, "L", a + q, h + w, "C", a, h + w, a, h + w, a, h + w - q, "L", a, h + q, "C", a, h, a, h, a + q, h]; e && e > c ? p > h + b && p < h + w - b ? d.splice(13, 3, "L", a + c, p - 6, a + c + 6, p, a + c, p + 6, a + c, h + w - q) : d.splice(13, 3, "L", a + c, w / 2, e, p, a + c, w / 2, a + c, h + w - q) : e && 0 > e ? p > h + b && p < h + w - b ? d.splice(33, 3, "L", a, p + 6, a - 6, p, a, p - 6,
                    a, h + q) : d.splice(33, 3, "L", a, w / 2, e, p, a, w / 2, a, h + q) : p && p > w && e > a + b && e < a + c - b ? d.splice(23, 3, "L", e + 6, h + w, e, h + w + 6, e - 6, h + w, a + q, h + w) : p && 0 > p && e > a + b && e < a + c - b && d.splice(3, 3, "L", e - 6, h, e, h - 6, e + 6, h, c - q, h); return d
                }
            }, clipRect: function (h, c, w, p) { var q = a.uniqueKey(), b = this.createElement("clipPath").attr({ id: q }).add(this.defs); h = this.rect(h, c, w, p, 0).add(b); h.id = q; h.clipPath = b; h.count = 0; return h }, text: function (a, h, c, w) {
                var p = !H && this.forExport, q = {}; if (w && (this.allowHTML || !this.forExport)) return this.html(a, h, c); q.x = Math.round(h ||
                0); c && (q.y = Math.round(c)); if (a || 0 === a) q.text = a; a = this.createElement("text").attr(q); p && a.css({ position: "absolute" }); w || (a.xSetter = function (a, h, c) { var w = c.getElementsByTagName("tspan"), p, q = c.getAttribute(h), b; for (b = 0; b < w.length; b++) p = w[b], p.getAttribute(h) === q && p.setAttribute(h, a); c.setAttribute(h, a) }); return a
            }, fontMetrics: function (a, c) {
                a = a || c && c.style && c.style.fontSize || this.style && this.style.fontSize; a = /px/.test(a) ? h(a) : /em/.test(a) ? parseFloat(a) * (c ? this.fontMetrics(null, c.parentNode).f : 16) : 12;
                c = 24 > a ? a + 3 : Math.round(1.2 * a); return { h: c, b: Math.round(.8 * c), f: a }
            }, rotCorr: function (a, h, c) { var w = a; h && c && (w = Math.max(w * Math.cos(h * g), 4)); return { x: -a / 3 * Math.sin(h * g), y: w } }, label: function (h, c, q, d, n, g, k, z, G) {
                var x = this, H = x.g("button" !== G && "label"), v = H.text = x.text("", 0, 0, k).attr({ zIndex: 1 }), f, y, I = 0, B = 3, D = 0, r, l, P, m, J, O = {}, L, u, N = /^url\((.*?)\)$/.test(d), K = N, U, T, Q, R; G && H.addClass("highcharts-" + G); K = N; U = function () { return (L || 0) % 2 / 2 }; T = function () {
                    var a = v.element.style, h = {}; y = (void 0 === r || void 0 === l || J) && t(v.textStr) &&
                    v.getBBox(); H.width = (r || y.width || 0) + 2 * B + D; H.height = (l || y.height || 0) + 2 * B; u = B + x.fontMetrics(a && a.fontSize, v).b; K && (f || (H.box = f = x.symbols[d] || N ? x.symbol(d) : x.rect(), f.addClass(("button" === G ? "" : "highcharts-label-box") + (G ? " highcharts-" + G + "-box" : "")), f.add(H), a = U(), h.x = a, h.y = (z ? -u : 0) + a), h.width = Math.round(H.width), h.height = Math.round(H.height), f.attr(e(h, O)), O = {})
                }; Q = function () {
                    var a = D + B, h; h = z ? 0 : u; t(r) && y && ("center" === J || "right" === J) && (a += { center: .5, right: 1 }[J] * (r - y.width)); if (a !== v.x || h !== v.y) v.attr("x",
                    a), void 0 !== h && v.attr("y", h); v.x = a; v.y = h
                }; R = function (a, h) { f ? f.attr(a, h) : O[a] = h }; H.onAdd = function () { v.add(H); H.attr({ text: h || 0 === h ? h : "", x: c, y: q }); f && t(n) && H.attr({ anchorX: n, anchorY: g }) }; H.widthSetter = function (h) { r = a.isNumber(h) ? h : null }; H.heightSetter = function (a) { l = a }; H["text-alignSetter"] = function (a) { J = a }; H.paddingSetter = function (a) { t(a) && a !== B && (B = H.padding = a, Q()) }; H.paddingLeftSetter = function (a) { t(a) && a !== D && (D = a, Q()) }; H.alignSetter = function (a) { a = { left: 0, center: .5, right: 1 }[a]; a !== I && (I = a, y && H.attr({ x: P })) };
                H.textSetter = function (a) { void 0 !== a && v.textSetter(a); T(); Q() }; H["stroke-widthSetter"] = function (a, h) { a && (K = !0); L = this["stroke-width"] = a; R(h, a) }; H.strokeSetter = H.fillSetter = H.rSetter = function (a, h) { "r" !== h && ("fill" === h && a && (K = !0), H[h] = a); R(h, a) }; H.anchorXSetter = function (a, h) { n = H.anchorX = a; R(h, Math.round(a) - U() - P) }; H.anchorYSetter = function (a, h) { g = H.anchorY = a; R(h, a - m) }; H.xSetter = function (a) { H.x = a; I && (a -= I * ((r || y.width) + 2 * B)); P = Math.round(a); H.attr("translateX", P) }; H.ySetter = function (a) {
                    m = H.y = Math.round(a);
                    H.attr("translateY", m)
                }; var V = H.css; return e(H, { css: function (a) { if (a) { var h = {}; a = p(a); b(H.textProps, function (c) { void 0 !== a[c] && (h[c] = a[c], delete a[c]) }); v.css(h) } return V.call(H, a) }, getBBox: function () { return { width: y.width + 2 * B, height: y.height + 2 * B, x: y.x - B, y: y.y - B } }, shadow: function (a) { a && (T(), f && f.shadow(a)); return H }, destroy: function () { w(H.element, "mouseenter"); w(H.element, "mouseleave"); v && (v = v.destroy()); f && (f = f.destroy()); C.prototype.destroy.call(H); H = x = T = Q = R = null } })
            }
        }); a.Renderer = A
    })(M); (function (a) {
        var C =
        a.attr, A = a.createElement, F = a.css, E = a.defined, m = a.each, f = a.extend, l = a.isFirefox, r = a.isMS, u = a.isWebKit, t = a.pInt, g = a.SVGRenderer, d = a.win, k = a.wrap; f(a.SVGElement.prototype, {
            htmlCss: function (a) { var b = this.element; if (b = a && "SPAN" === b.tagName && a.width) delete a.width, this.textWidth = b, this.updateTransform(); a && "ellipsis" === a.textOverflow && (a.whiteSpace = "nowrap", a.overflow = "hidden"); this.styles = f(this.styles, a); F(this.element, a); return this }, htmlGetBBox: function () {
                var a = this.element; "text" === a.nodeName && (a.style.position =
                "absolute"); return { x: a.offsetLeft, y: a.offsetTop, width: a.offsetWidth, height: a.offsetHeight }
            }, htmlUpdateTransform: function () {
                if (this.added) {
                    var a = this.renderer, e = this.element, d = this.translateX || 0, g = this.translateY || 0, n = this.x || 0, k = this.y || 0, f = this.textAlign || "left", c = { left: 0, center: .5, right: 1 }[f], G = this.styles; F(e, { marginLeft: d, marginTop: g }); this.shadows && m(this.shadows, function (a) { F(a, { marginLeft: d + 1, marginTop: g + 1 }) }); this.inverted && m(e.childNodes, function (c) { a.invertChild(c, e) }); if ("SPAN" === e.tagName) {
                        var q =
                        this.rotation, B = t(this.textWidth), r = G && G.whiteSpace, p = [q, f, e.innerHTML, this.textWidth, this.textAlign].join(); p !== this.cTT && (G = a.fontMetrics(e.style.fontSize).b, E(q) && this.setSpanRotation(q, c, G), F(e, { width: "", whiteSpace: r || "nowrap" }), e.offsetWidth > B && /[ \-]/.test(e.textContent || e.innerText) && F(e, { width: B + "px", display: "block", whiteSpace: r || "normal" }), this.getSpanCorrection(e.offsetWidth, G, c, q, f)); F(e, { left: n + (this.xCorr || 0) + "px", top: k + (this.yCorr || 0) + "px" }); u && (G = e.offsetHeight); this.cTT = p
                    }
                } else this.alignOnAdd =
                !0
            }, setSpanRotation: function (a, e, g) { var b = {}, n = r ? "-ms-transform" : u ? "-webkit-transform" : l ? "MozTransform" : d.opera ? "-o-transform" : ""; b[n] = b.transform = "rotate(" + a + "deg)"; b[n + (l ? "Origin" : "-origin")] = b.transformOrigin = 100 * e + "% " + g + "px"; F(this.element, b) }, getSpanCorrection: function (a, e, d) { this.xCorr = -a * d; this.yCorr = -e }
        }); f(g.prototype, {
            html: function (a, e, d) {
                var b = this.createElement("span"), n = b.element, g = b.renderer, v = g.isSVG, c = function (a, c) {
                    m(["opacity", "visibility"], function (q) {
                        k(a, q + "Setter", function (a,
                        p, q, b) { a.call(this, p, q, b); c[q] = p })
                    })
                }; b.textSetter = function (a) { a !== n.innerHTML && delete this.bBox; n.innerHTML = this.textStr = a; b.htmlUpdateTransform() }; v && c(b, b.element.style); b.xSetter = b.ySetter = b.alignSetter = b.rotationSetter = function (a, c) { "align" === c && (c = "textAlign"); b[c] = a; b.htmlUpdateTransform() }; b.attr({ text: a, x: Math.round(e), y: Math.round(d) }).css({ fontFamily: this.style.fontFamily, fontSize: this.style.fontSize, position: "absolute" }); n.style.whiteSpace = "nowrap"; b.css = b.htmlCss; v && (b.add = function (a) {
                    var q,
                    e = g.box.parentNode, d = []; if (this.parentGroup = a) {
                        if (q = a.div, !q) {
                            for (; a;) d.push(a), a = a.parentGroup; m(d.reverse(), function (a) {
                                var p, n = C(a.element, "class"); n && (n = { className: n }); q = a.div = a.div || A("div", n, { position: "absolute", left: (a.translateX || 0) + "px", top: (a.translateY || 0) + "px", display: a.display, opacity: a.opacity, pointerEvents: a.styles && a.styles.pointerEvents }, q || e); p = q.style; f(a, {
                                    classSetter: function (a) { this.element.setAttribute("class", a); q.className = a }, on: function () {
                                        d[0].div && b.on.apply({ element: d[0].div },
                                        arguments); return a
                                    }, translateXSetter: function (c, h) { p.left = c + "px"; a[h] = c; a.doTransform = !0 }, translateYSetter: function (c, h) { p.top = c + "px"; a[h] = c; a.doTransform = !0 }
                                }); c(a, p)
                            })
                        }
                    } else q = e; q.appendChild(n); b.added = !0; b.alignOnAdd && b.htmlUpdateTransform(); return b
                }); return b
            }
        })
    })(M); (function (a) {
        var C, A, F = a.createElement, E = a.css, m = a.defined, f = a.deg2rad, l = a.discardElement, r = a.doc, u = a.each, t = a.erase, g = a.extend; C = a.extendClass; var d = a.isArray, k = a.isNumber, b = a.isObject, e = a.merge; A = a.noop; var v = a.pick, y = a.pInt,
        n = a.SVGElement, D = a.SVGRenderer, J = a.win; a.svg || (A = {
            docMode8: r && 8 === r.documentMode, init: function (a, b) { var c = ["\x3c", b, ' filled\x3d"f" stroked\x3d"f"'], e = ["position: ", "absolute", ";"], d = "div" === b; ("shape" === b || d) && e.push("left:0;top:0;width:1px;height:1px;"); e.push("visibility: ", d ? "hidden" : "visible"); c.push(' style\x3d"', e.join(""), '"/\x3e'); b && (c = d || "span" === b || "img" === b ? c.join("") : a.prepVML(c), this.element = F(c)); this.renderer = a }, add: function (a) {
                var c = this.renderer, b = this.element, e = c.box, d = a && a.inverted,
                e = a ? a.element || a : e; a && (this.parentGroup = a); d && c.invertChild(b, e); e.appendChild(b); this.added = !0; this.alignOnAdd && !this.deferUpdateTransform && this.updateTransform(); if (this.onAdd) this.onAdd(); this.className && this.attr("class", this.className); return this
            }, updateTransform: n.prototype.htmlUpdateTransform, setSpanRotation: function () {
                var a = this.rotation, b = Math.cos(a * f), q = Math.sin(a * f); E(this.element, {
                    filter: a ? ["progid:DXImageTransform.Microsoft.Matrix(M11\x3d", b, ", M12\x3d", -q, ", M21\x3d", q, ", M22\x3d",
                    b, ", sizingMethod\x3d'auto expand')"].join("") : "none"
                })
            }, getSpanCorrection: function (a, b, q, e, d) { var c = e ? Math.cos(e * f) : 1, n = e ? Math.sin(e * f) : 0, g = v(this.elemHeight, this.element.offsetHeight), k; this.xCorr = 0 > c && -a; this.yCorr = 0 > n && -g; k = 0 > c * n; this.xCorr += n * b * (k ? 1 - q : q); this.yCorr -= c * b * (e ? k ? q : 1 - q : 1); d && "left" !== d && (this.xCorr -= a * q * (0 > c ? -1 : 1), e && (this.yCorr -= g * q * (0 > n ? -1 : 1)), E(this.element, { textAlign: d })) }, pathToVML: function (a) {
                for (var c = a.length, b = []; c--;) k(a[c]) ? b[c] = Math.round(10 * a[c]) - 5 : "Z" === a[c] ? b[c] = "x" :
                (b[c] = a[c], !a.isArc || "wa" !== a[c] && "at" !== a[c] || (b[c + 5] === b[c + 7] && (b[c + 7] += a[c + 7] > a[c + 5] ? 1 : -1), b[c + 6] === b[c + 8] && (b[c + 8] += a[c + 8] > a[c + 6] ? 1 : -1))); return b.join(" ") || "x"
            }, clip: function (a) { var c = this, b; a ? (b = a.members, t(b, c), b.push(c), c.destroyClip = function () { t(b, c) }, a = a.getCSS(c)) : (c.destroyClip && c.destroyClip(), a = { clip: c.docMode8 ? "inherit" : "rect(auto)" }); return c.css(a) }, css: n.prototype.htmlCss, safeRemoveChild: function (a) { a.parentNode && l(a) }, destroy: function () { this.destroyClip && this.destroyClip(); return n.prototype.destroy.apply(this) },
            on: function (a, b) { this.element["on" + a] = function () { var a = J.event; a.target = a.srcElement; b(a) }; return this }, cutOffPath: function (a, b) { var c; a = a.split(/[ ,]/); c = a.length; if (9 === c || 11 === c) a[c - 4] = a[c - 2] = y(a[c - 2]) - 10 * b; return a.join(" ") }, shadow: function (a, b, e) {
                var c = [], q, p = this.element, d = this.renderer, n, g = p.style, h, w = p.path, k, H, f, D; w && "string" !== typeof w.value && (w = "x"); H = w; if (a) {
                    f = v(a.width, 3); D = (a.opacity || .15) / f; for (q = 1; 3 >= q; q++) k = 2 * f + 1 - 2 * q, e && (H = this.cutOffPath(w.value, k + .5)), h = ['\x3cshape isShadow\x3d"true" strokeweight\x3d"',
                    k, '" filled\x3d"false" path\x3d"', H, '" coordsize\x3d"10 10" style\x3d"', p.style.cssText, '" /\x3e'], n = F(d.prepVML(h), null, { left: y(g.left) + v(a.offsetX, 1), top: y(g.top) + v(a.offsetY, 1) }), e && (n.cutOff = k + 1), h = ['\x3cstroke color\x3d"', a.color || "#000000", '" opacity\x3d"', D * q, '"/\x3e'], F(d.prepVML(h), null, null, n), b ? b.element.appendChild(n) : p.parentNode.insertBefore(n, p), c.push(n); this.shadows = c
                } return this
            }, updateShadows: A, setAttr: function (a, b) { this.docMode8 ? this.element[a] = b : this.element.setAttribute(a, b) },
            classSetter: function (a) { (this.added ? this.element : this).className = a }, dashstyleSetter: function (a, b, e) { (e.getElementsByTagName("stroke")[0] || F(this.renderer.prepVML(["\x3cstroke/\x3e"]), null, null, e))[b] = a || "solid"; this[b] = a }, dSetter: function (a, b, e) { var c = this.shadows; a = a || []; this.d = a.join && a.join(" "); e.path = a = this.pathToVML(a); if (c) for (e = c.length; e--;) c[e].path = c[e].cutOff ? this.cutOffPath(a, c[e].cutOff) : a; this.setAttr(b, a) }, fillSetter: function (a, b, e) {
                var c = e.nodeName; "SPAN" === c ? e.style.color = a : "IMG" !==
                c && (e.filled = "none" !== a, this.setAttr("fillcolor", this.renderer.color(a, e, b, this)))
            }, "fill-opacitySetter": function (a, b, e) { F(this.renderer.prepVML(["\x3c", b.split("-")[0], ' opacity\x3d"', a, '"/\x3e']), null, null, e) }, opacitySetter: A, rotationSetter: function (a, b, e) { e = e.style; this[b] = e[b] = a; e.left = -Math.round(Math.sin(a * f) + 1) + "px"; e.top = Math.round(Math.cos(a * f)) + "px" }, strokeSetter: function (a, b, e) { this.setAttr("strokecolor", this.renderer.color(a, e, b, this)) }, "stroke-widthSetter": function (a, b, e) {
                e.stroked = !!a;
                this[b] = a; k(a) && (a += "px"); this.setAttr("strokeweight", a)
            }, titleSetter: function (a, b) { this.setAttr(b, a) }, visibilitySetter: function (a, b, e) { "inherit" === a && (a = "visible"); this.shadows && u(this.shadows, function (c) { c.style[b] = a }); "DIV" === e.nodeName && (a = "hidden" === a ? "-999em" : 0, this.docMode8 || (e.style[b] = a ? "visible" : "hidden"), b = "top"); e.style[b] = a }, xSetter: function (a, b, e) { this[b] = a; "x" === b ? b = "left" : "y" === b && (b = "top"); this.updateClipping ? (this[b] = a, this.updateClipping()) : e.style[b] = a }, zIndexSetter: function (a,
            b, e) { e.style[b] = a }
        }, A["stroke-opacitySetter"] = A["fill-opacitySetter"], a.VMLElement = A = C(n, A), A.prototype.ySetter = A.prototype.widthSetter = A.prototype.heightSetter = A.prototype.xSetter, A = {
            Element: A, isIE8: -1 < J.navigator.userAgent.indexOf("MSIE 8.0"), init: function (a, b, e) {
                var c, d; this.alignedObjects = []; c = this.createElement("div").css({ position: "relative" }); d = c.element; a.appendChild(c.element); this.isVML = !0; this.box = d; this.boxWrapper = c; this.gradients = {}; this.cache = {}; this.cacheKeys = []; this.imgCount = 0; this.setSize(b,
                e, !1); if (!r.namespaces.hcv) { r.namespaces.add("hcv", "urn:schemas-microsoft-com:vml"); try { r.createStyleSheet().cssText = "hcv\\:fill, hcv\\:path, hcv\\:shape, hcv\\:stroke{ behavior:url(#default#VML); display: inline-block; } " } catch (p) { r.styleSheets[0].cssText += "hcv\\:fill, hcv\\:path, hcv\\:shape, hcv\\:stroke{ behavior:url(#default#VML); display: inline-block; } " } }
            }, isHidden: function () { return !this.box.offsetWidth }, clipRect: function (a, e, d, n) {
                var c = this.createElement(), p = b(a); return g(c, {
                    members: [],
                    count: 0, left: (p ? a.x : a) + 1, top: (p ? a.y : e) + 1, width: (p ? a.width : d) - 1, height: (p ? a.height : n) - 1, getCSS: function (a) { var c = a.element, b = c.nodeName, h = a.inverted, w = this.top - ("shape" === b ? c.offsetTop : 0), p = this.left, c = p + this.width, e = w + this.height, w = { clip: "rect(" + Math.round(h ? p : w) + "px," + Math.round(h ? e : c) + "px," + Math.round(h ? c : e) + "px," + Math.round(h ? w : p) + "px)" }; !h && a.docMode8 && "DIV" === b && g(w, { width: c + "px", height: e + "px" }); return w }, updateClipping: function () { u(c.members, function (a) { a.element && a.css(c.getCSS(a)) }) }
                })
            }, color: function (c,
            b, e, d) {
                var q = this, p, n = /^rgba/, g, k, h = "none"; c && c.linearGradient ? k = "gradient" : c && c.radialGradient && (k = "pattern"); if (k) {
                    var w, v, H = c.linearGradient || c.radialGradient, f, D, y, x, r, B = ""; c = c.stops; var l, G = [], m = function () { g = ['\x3cfill colors\x3d"' + G.join(",") + '" opacity\x3d"', y, '" o:opacity2\x3d"', D, '" type\x3d"', k, '" ', B, 'focus\x3d"100%" method\x3d"any" /\x3e']; F(q.prepVML(g), null, null, b) }; f = c[0]; l = c[c.length - 1]; 0 < f[0] && c.unshift([0, f[1]]); 1 > l[0] && c.push([1, l[1]]); u(c, function (h, c) {
                        n.test(h[1]) ? (p = a.color(h[1]),
                        w = p.get("rgb"), v = p.get("a")) : (w = h[1], v = 1); G.push(100 * h[0] + "% " + w); c ? (y = v, x = w) : (D = v, r = w)
                    }); if ("fill" === e) if ("gradient" === k) e = H.x1 || H[0] || 0, c = H.y1 || H[1] || 0, f = H.x2 || H[2] || 0, H = H.y2 || H[3] || 0, B = 'angle\x3d"' + (90 - 180 * Math.atan((H - c) / (f - e)) / Math.PI) + '"', m(); else {
                        var h = H.r, t = 2 * h, J = 2 * h, A = H.cx, C = H.cy, E = b.radialReference, M, h = function () {
                            E && (M = d.getBBox(), A += (E[0] - M.x) / M.width - .5, C += (E[1] - M.y) / M.height - .5, t *= E[2] / M.width, J *= E[2] / M.height); B = 'src\x3d"' + a.getOptions().global.VMLRadialGradientURL + '" size\x3d"' + t + "," +
                            J + '" origin\x3d"0.5,0.5" position\x3d"' + A + "," + C + '" color2\x3d"' + r + '" '; m()
                        }; d.added ? h() : d.onAdd = h; h = x
                    } else h = w
                } else n.test(c) && "IMG" !== b.tagName ? (p = a.color(c), d[e + "-opacitySetter"](p.get("a"), e, b), h = p.get("rgb")) : (h = b.getElementsByTagName(e), h.length && (h[0].opacity = 1, h[0].type = "solid"), h = c); return h
            }, prepVML: function (a) {
                var c = this.isIE8; a = a.join(""); c ? (a = a.replace("/\x3e", ' xmlns\x3d"urn:schemas-microsoft-com:vml" /\x3e'), a = -1 === a.indexOf('style\x3d"') ? a.replace("/\x3e", ' style\x3d"display:inline-block;behavior:url(#default#VML);" /\x3e') :
                a.replace('style\x3d"', 'style\x3d"display:inline-block;behavior:url(#default#VML);')) : a = a.replace("\x3c", "\x3chcv:"); return a
            }, text: D.prototype.html, path: function (a) { var c = { coordsize: "10 10" }; d(a) ? c.d = a : b(a) && g(c, a); return this.createElement("shape").attr(c) }, circle: function (a, e, d) { var c = this.symbol("circle"); b(a) && (d = a.r, e = a.y, a = a.x); c.isCircle = !0; c.r = d; return c.attr({ x: a, y: e }) }, g: function (a) { var c; a && (c = { className: "highcharts-" + a, "class": "highcharts-" + a }); return this.createElement("div").attr(c) },
            image: function (a, b, e, d, n) { var c = this.createElement("img").attr({ src: a }); 1 < arguments.length && c.attr({ x: b, y: e, width: d, height: n }); return c }, createElement: function (a) { return "rect" === a ? this.symbol(a) : D.prototype.createElement.call(this, a) }, invertChild: function (a, b) { var c = this; b = b.style; var e = "IMG" === a.tagName && a.style; E(a, { flip: "x", left: y(b.width) - (e ? y(e.top) : 1), top: y(b.height) - (e ? y(e.left) : 1), rotation: -90 }); u(a.childNodes, function (b) { c.invertChild(b, a) }) }, symbols: {
                arc: function (a, b, e, d, n) {
                    var c = n.start,
                    q = n.end, g = n.r || e || d; e = n.innerR; d = Math.cos(c); var k = Math.sin(c), h = Math.cos(q), w = Math.sin(q); if (0 === q - c) return ["x"]; c = ["wa", a - g, b - g, a + g, b + g, a + g * d, b + g * k, a + g * h, b + g * w]; n.open && !e && c.push("e", "M", a, b); c.push("at", a - e, b - e, a + e, b + e, a + e * h, b + e * w, a + e * d, b + e * k, "x", "e"); c.isArc = !0; return c
                }, circle: function (a, b, e, d, n) { n && m(n.r) && (e = d = 2 * n.r); n && n.isCircle && (a -= e / 2, b -= d / 2); return ["wa", a, b, a + e, b + d, a + e, b + d / 2, a + e, b + d / 2, "e"] }, rect: function (a, b, e, d, n) {
                    return D.prototype.symbols[m(n) && n.r ? "callout" : "square"].call(0, a, b,
                    e, d, n)
                }
            }
        }, a.VMLRenderer = C = function () { this.init.apply(this, arguments) }, C.prototype = e(D.prototype, A), a.Renderer = C); D.prototype.measureSpanWidth = function (a, b) { var c = r.createElement("span"); a = r.createTextNode(a); c.appendChild(a); E(c, b); this.box.appendChild(c); b = c.offsetWidth; l(c); return b }
    })(M); (function (a) {
        function C() { var f = a.defaultOptions.global, l = r.moment; if (f.timezone) { if (l) return function (a) { return -l.tz(a, f.timezone).utcOffset() }; a.error(25) } return f.useUTC && f.getTimezoneOffset } function A() {
            var f =
            a.defaultOptions.global, t, g = f.useUTC, d = g ? "getUTC" : "get", k = g ? "setUTC" : "set"; a.Date = t = f.Date || r.Date; t.hcTimezoneOffset = g && f.timezoneOffset; t.hcGetTimezoneOffset = C(); t.hcMakeTime = function (a, e, d, k, n, f) { var b; g ? (b = t.UTC.apply(0, arguments), b += m(b)) : b = (new t(a, e, l(d, 1), l(k, 0), l(n, 0), l(f, 0))).getTime(); return b }; E("Minutes Hours Day Date Month FullYear".split(" "), function (a) { t["hcGet" + a] = d + a }); E("Milliseconds Seconds Minutes Hours Date Month FullYear".split(" "), function (a) { t["hcSet" + a] = k + a })
        } var F = a.color,
        E = a.each, m = a.getTZOffset, f = a.merge, l = a.pick, r = a.win; a.defaultOptions = {
            colors: "#7cb5ec #434348 #90ed7d #f7a35c #8085e9 #f15c80 #e4d354 #2b908f #f45b5b #91e8e1".split(" "), symbols: ["circle", "diamond", "square", "triangle", "triangle-down"], lang: {
                loading: "Loading...", months: "January February March April May June July August September October November December".split(" "), shortMonths: "Jan Feb Mar Apr May Jun Jul Aug Sep Oct Nov Dec".split(" "), weekdays: "Sunday Monday Tuesday Wednesday Thursday Friday Saturday".split(" "),
                decimalPoint: ".", numericSymbols: "kMGTPE".split(""), resetZoom: "Reset zoom", resetZoomTitle: "Reset zoom level 1:1", thousandsSep: " "
            }, global: { useUTC: !0, VMLRadialGradientURL: "http://code.highcharts.com/5.0.14/gfx/vml-radial-gradient.png" }, chart: { borderRadius: 0, defaultSeriesType: "line", ignoreHiddenSeries: !0, spacing: [10, 10, 15, 10], resetZoomButton: { theme: { zIndex: 20 }, position: { align: "right", x: -10, y: 10 } }, width: null, height: null, borderColor: "#335cad", backgroundColor: "#ffffff", plotBorderColor: "#cccccc" }, title: {
                text: "Chart title",
                align: "center", margin: 15, widthAdjust: -44
            }, subtitle: { text: "", align: "center", widthAdjust: -44 }, plotOptions: {}, labels: { style: { position: "absolute", color: "#333333" } }, legend: {
                enabled: !0, align: "center", layout: "horizontal", labelFormatter: function () { return this.name }, borderColor: "#999999", borderRadius: 0, navigation: { activeColor: "#003399", inactiveColor: "#cccccc" }, itemStyle: { color: "#333333", fontSize: "12px", fontWeight: "bold", textOverflow: "ellipsis" }, itemHoverStyle: { color: "#000000" }, itemHiddenStyle: { color: "#cccccc" },
                shadow: !1, itemCheckboxStyle: { position: "absolute", width: "13px", height: "13px" }, squareSymbol: !0, symbolPadding: 5, verticalAlign: "bottom", x: 0, y: 0, title: { style: { fontWeight: "bold" } }
            }, loading: { labelStyle: { fontWeight: "bold", position: "relative", top: "45%" }, style: { position: "absolute", backgroundColor: "#ffffff", opacity: .5, textAlign: "center" } }, tooltip: {
                enabled: !0, animation: a.svg, borderRadius: 3, dateTimeLabelFormats: {
                    millisecond: "%A, %b %e, %H:%M:%S.%L", second: "%A, %b %e, %H:%M:%S", minute: "%A, %b %e, %H:%M", hour: "%A, %b %e, %H:%M",
                    day: "%A, %b %e, %Y", week: "Week from %A, %b %e, %Y", month: "%B %Y", year: "%Y"
                }, footerFormat: "", padding: 8, snap: a.isTouchDevice ? 25 : 10, backgroundColor: F("#f7f7f7").setOpacity(.85).get(), borderWidth: 1, headerFormat: '\x3cspan style\x3d"font-size: 10px"\x3e{point.key}\x3c/span\x3e\x3cbr/\x3e', pointFormat: '\x3cspan style\x3d"color:{point.color}"\x3e\u25cf\x3c/span\x3e {series.name}: \x3cb\x3e{point.y}\x3c/b\x3e\x3cbr/\x3e', shadow: !0, style: {
                    color: "#333333", cursor: "default", fontSize: "12px", pointerEvents: "none",
                    whiteSpace: "nowrap"
                }
            }/*, credits: { enabled: !0, href: "http://www.highcharts.com", position: { align: "right", x: -10, verticalAlign: "bottom", y: -5 }, style: { cursor: "pointer", color: "#999999", fontSize: "9px" }, text: "Highcharts.com" }*/
        }; a.setOptions = function (r) { a.defaultOptions = f(!0, a.defaultOptions, r); A(); return a.defaultOptions }; a.getOptions = function () { return a.defaultOptions }; a.defaultPlotOptions = a.defaultOptions.plotOptions; A()
    })(M); (function (a) {
        var C = a.correctFloat, A = a.defined, F = a.destroyObjectProperties, E = a.isNumber,
        m = a.merge, f = a.pick, l = a.deg2rad; a.Tick = function (a, f, l, g) { this.axis = a; this.pos = f; this.type = l || ""; this.isNewLabel = this.isNew = !0; l || g || this.addLabel() }; a.Tick.prototype = {
            addLabel: function () {
                var a = this.axis, l = a.options, t = a.chart, g = a.categories, d = a.names, k = this.pos, b = l.labels, e = a.tickPositions, v = k === e[0], y = k === e[e.length - 1], d = g ? f(g[k], d[k], k) : k, g = this.label, e = e.info, n; a.isDatetimeAxis && e && (n = l.dateTimeLabelFormats[e.higherRanks[k] || e.unitName]); this.isFirst = v; this.isLast = y; l = a.labelFormatter.call({
                    axis: a,
                    chart: t, isFirst: v, isLast: y, dateTimeLabelFormat: n, value: a.isLog ? C(a.lin2log(d)) : d, pos: k
                }); A(g) ? g && g.attr({ text: l }) : (this.labelLength = (this.label = g = A(l) && b.enabled ? t.renderer.text(l, 0, 0, b.useHTML).css(m(b.style)).add(a.labelGroup) : null) && g.getBBox().width, this.rotation = 0)
            }, getLabelSize: function () { return this.label ? this.label.getBBox()[this.axis.horiz ? "height" : "width"] : 0 }, handleOverflow: function (a) {
                var r = this.axis, m = a.x, g = r.chart.chartWidth, d = r.chart.spacing, k = f(r.labelLeft, Math.min(r.pos, d[3])), d = f(r.labelRight,
                Math.max(r.pos + r.len, g - d[1])), b = this.label, e = this.rotation, v = { left: 0, center: .5, right: 1 }[r.labelAlign], y = b.getBBox().width, n = r.getSlotWidth(), D = n, J = 1, c, G = {}; if (e) 0 > e && m - v * y < k ? c = Math.round(m / Math.cos(e * l) - k) : 0 < e && m + v * y > d && (c = Math.round((g - m) / Math.cos(e * l))); else if (g = m + (1 - v) * y, m - v * y < k ? D = a.x + D * (1 - v) - k : g > d && (D = d - a.x + D * v, J = -1), D = Math.min(n, D), D < n && "center" === r.labelAlign && (a.x += J * (n - D - v * (n - Math.min(y, D)))), y > D || r.autoRotation && (b.styles || {}).width) c = D; c && (G.width = c, (r.options.labels.style || {}).textOverflow ||
                (G.textOverflow = "ellipsis"), b.css(G))
            }, getPosition: function (a, f, l, g) { var d = this.axis, k = d.chart, b = g && k.oldChartHeight || k.chartHeight; return { x: a ? d.translate(f + l, null, null, g) + d.transB : d.left + d.offset + (d.opposite ? (g && k.oldChartWidth || k.chartWidth) - d.right - d.left : 0), y: a ? b - d.bottom + d.offset - (d.opposite ? d.height : 0) : b - d.translate(f + l, null, null, g) - d.transB } }, getLabelPosition: function (a, f, m, g, d, k, b, e) {
                var v = this.axis, y = v.transA, n = v.reversed, D = v.staggerLines, r = v.tickRotCorr || { x: 0, y: 0 }, c = d.y; A(c) || (c = 0 === v.side ?
                m.rotation ? -8 : -m.getBBox().height : 2 === v.side ? r.y + 8 : Math.cos(m.rotation * l) * (r.y - m.getBBox(!1, 0).height / 2)); a = a + d.x + r.x - (k && g ? k * y * (n ? -1 : 1) : 0); f = f + c - (k && !g ? k * y * (n ? 1 : -1) : 0); D && (m = b / (e || 1) % D, v.opposite && (m = D - m - 1), f += v.labelOffset / D * m); return { x: a, y: Math.round(f) }
            }, getMarkPath: function (a, f, l, g, d, k) { return k.crispLine(["M", a, f, "L", a + (d ? 0 : -l), f + (d ? l : 0)], g) }, renderGridLine: function (a, f, l) {
                var g = this.axis, d = g.options, k = this.gridLine, b = {}, e = this.pos, v = this.type, y = g.tickmarkOffset, n = g.chart.renderer, D = v ? v + "Grid" :
                "grid", r = d[D + "LineWidth"], c = d[D + "LineColor"], d = d[D + "LineDashStyle"]; k || (b.stroke = c, b["stroke-width"] = r, d && (b.dashstyle = d), v || (b.zIndex = 1), a && (b.opacity = 0), this.gridLine = k = n.path().attr(b).addClass("highcharts-" + (v ? v + "-" : "") + "grid-line").add(g.gridGroup)); if (!a && k && (a = g.getPlotLinePath(e + y, k.strokeWidth() * l, a, !0))) k[this.isNew ? "attr" : "animate"]({ d: a, opacity: f })
            }, renderMark: function (a, l, m) {
                var g = this.axis, d = g.options, k = g.chart.renderer, b = this.type, e = b ? b + "Tick" : "tick", v = g.tickSize(e), y = this.mark, n = !y,
                D = a.x; a = a.y; var r = f(d[e + "Width"], !b && g.isXAxis ? 1 : 0), d = d[e + "Color"]; v && (g.opposite && (v[0] = -v[0]), n && (this.mark = y = k.path().addClass("highcharts-" + (b ? b + "-" : "") + "tick").add(g.axisGroup), y.attr({ stroke: d, "stroke-width": r })), y[n ? "attr" : "animate"]({ d: this.getMarkPath(D, a, v[0], y.strokeWidth() * m, g.horiz, k), opacity: l }))
            }, renderLabel: function (a, l, m, g) {
                var d = this.axis, k = d.horiz, b = d.options, e = this.label, v = b.labels, y = v.step, n = d.tickmarkOffset, D = !0, r = a.x; a = a.y; e && E(r) && (e.xy = a = this.getLabelPosition(r, a, e, k, v, n,
                g, y), this.isFirst && !this.isLast && !f(b.showFirstLabel, 1) || this.isLast && !this.isFirst && !f(b.showLastLabel, 1) ? D = !1 : !k || d.isRadial || v.step || v.rotation || l || 0 === m || this.handleOverflow(a), y && g % y && (D = !1), D && E(a.y) ? (a.opacity = m, e[this.isNewLabel ? "attr" : "animate"](a), this.isNewLabel = !1) : (e.attr("y", -9999), this.isNewLabel = !0), this.isNew = !1)
            }, render: function (a, l, m) {
                var g = this.axis, d = g.horiz, k = this.getPosition(d, this.pos, g.tickmarkOffset, l), b = k.x, e = k.y, g = d && b === g.pos + g.len || !d && e === g.pos ? -1 : 1; m = f(m, 1); this.isActive =
                !0; this.renderGridLine(l, m, g); this.renderMark(k, m, g); this.renderLabel(k, l, m, a)
            }, destroy: function () { F(this, this.axis) }
        }
    })(M); var S = function (a) {
        var C = a.addEvent, A = a.animObject, F = a.arrayMax, E = a.arrayMin, m = a.color, f = a.correctFloat, l = a.defaultOptions, r = a.defined, u = a.deg2rad, t = a.destroyObjectProperties, g = a.each, d = a.extend, k = a.fireEvent, b = a.format, e = a.getMagnitude, v = a.grep, y = a.inArray, n = a.isArray, D = a.isNumber, J = a.isString, c = a.merge, G = a.normalizeTickInterval, q = a.objectEach, B = a.pick, K = a.removeEvent, p = a.splat,
        z = a.syncTimeout, I = a.Tick, L = function () { this.init.apply(this, arguments) }; a.extend(L.prototype, {
            defaultOptions: {
                dateTimeLabelFormats: { millisecond: "%H:%M:%S.%L", second: "%H:%M:%S", minute: "%H:%M", hour: "%H:%M", day: "%e. %b", week: "%e. %b", month: "%b '%y", year: "%Y" }, endOnTick: !1, labels: { enabled: !0, style: { color: "#666666", cursor: "default", fontSize: "11px" }, x: 0 }, minPadding: .01, maxPadding: .01, minorTickLength: 2, minorTickPosition: "outside", startOfWeek: 1, startOnTick: !1, tickLength: 10, tickmarkPlacement: "between", tickPixelInterval: 100,
                tickPosition: "outside", title: { align: "middle", style: { color: "#666666" } }, type: "linear", minorGridLineColor: "#f2f2f2", minorGridLineWidth: 1, minorTickColor: "#999999", lineColor: "#ccd6eb", lineWidth: 1, gridLineColor: "#e6e6e6", tickColor: "#ccd6eb"
            }, defaultYAxisOptions: {
                endOnTick: !0, tickPixelInterval: 72, showLastLabel: !0, labels: { x: -8 }, maxPadding: .05, minPadding: .05, startOnTick: !0, title: { rotation: 270, text: "Values" }, stackLabels: {
                    allowOverlap: !1, enabled: !1, formatter: function () { return a.numberFormat(this.total, -1) },
                    style: { fontSize: "11px", fontWeight: "bold", color: "#000000", textOutline: "1px contrast" }
                }, gridLineWidth: 1, lineWidth: 0
            }, defaultLeftAxisOptions: { labels: { x: -15 }, title: { rotation: 270 } }, defaultRightAxisOptions: { labels: { x: 15 }, title: { rotation: 90 } }, defaultBottomAxisOptions: { labels: { autoRotation: [-45], x: 0 }, title: { rotation: 0 } }, defaultTopAxisOptions: { labels: { autoRotation: [-45], x: 0 }, title: { rotation: 0 } }, init: function (a, c) {
                var h = c.isX, b = this; b.chart = a; b.horiz = a.inverted && !b.isZAxis ? !h : h; b.isXAxis = h; b.coll = b.coll || (h ?
                "xAxis" : "yAxis"); b.opposite = c.opposite; b.side = c.side || (b.horiz ? b.opposite ? 0 : 2 : b.opposite ? 1 : 3); b.setOptions(c); var w = this.options, e = w.type; b.labelFormatter = w.labels.formatter || b.defaultLabelFormatter; b.userOptions = c; b.minPixelPadding = 0; b.reversed = w.reversed; b.visible = !1 !== w.visible; b.zoomEnabled = !1 !== w.zoomEnabled; b.hasNames = "category" === e || !0 === w.categories; b.categories = w.categories || b.hasNames; b.names = b.names || []; b.plotLinesAndBandsGroups = {}; b.isLog = "logarithmic" === e; b.isDatetimeAxis = "datetime" ===
                e; b.positiveValuesOnly = b.isLog && !b.allowNegativeLog; b.isLinked = r(w.linkedTo); b.ticks = {}; b.labelEdge = []; b.minorTicks = {}; b.plotLinesAndBands = []; b.alternateBands = {}; b.len = 0; b.minRange = b.userMinRange = w.minRange || w.maxZoom; b.range = w.range; b.offset = w.offset || 0; b.stacks = {}; b.oldStacks = {}; b.stacksTouched = 0; b.max = null; b.min = null; b.crosshair = B(w.crosshair, p(a.options.tooltip.crosshairs)[h ? 0 : 1], !1); c = b.options.events; -1 === y(b, a.axes) && (h ? a.axes.splice(a.xAxis.length, 0, b) : a.axes.push(b), a[b.coll].push(b)); b.series =
                b.series || []; a.inverted && !b.isZAxis && h && void 0 === b.reversed && (b.reversed = !0); q(c, function (a, h) { C(b, h, a) }); b.lin2log = w.linearToLogConverter || b.lin2log; b.isLog && (b.val2lin = b.log2lin, b.lin2val = b.lin2log)
            }, setOptions: function (a) { this.options = c(this.defaultOptions, "yAxis" === this.coll && this.defaultYAxisOptions, [this.defaultTopAxisOptions, this.defaultRightAxisOptions, this.defaultBottomAxisOptions, this.defaultLeftAxisOptions][this.side], c(l[this.coll], a)) }, defaultLabelFormatter: function () {
                var h = this.axis,
                c = this.value, e = h.categories, p = this.dateTimeLabelFormat, d = l.lang, n = d.numericSymbols, d = d.numericSymbolMagnitude || 1E3, q = n && n.length, x, g = h.options.labels.format, h = h.isLog ? Math.abs(c) : h.tickInterval; if (g) x = b(g, this); else if (e) x = c; else if (p) x = a.dateFormat(p, c); else if (q && 1E3 <= h) for (; q-- && void 0 === x;) e = Math.pow(d, q + 1), h >= e && 0 === 10 * c % e && null !== n[q] && 0 !== c && (x = a.numberFormat(c / e, -1) + n[q]); void 0 === x && (x = 1E4 <= Math.abs(c) ? a.numberFormat(c, -1) : a.numberFormat(c, -1, void 0, "")); return x
            }, getSeriesExtremes: function () {
                var a =
                this, b = a.chart; a.hasVisibleSeries = !1; a.dataMin = a.dataMax = a.threshold = null; a.softThreshold = !a.isXAxis; a.buildStacks && a.buildStacks(); g(a.series, function (h) {
                    if (h.visible || !b.options.chart.ignoreHiddenSeries) {
                        var c = h.options, w = c.threshold, e; a.hasVisibleSeries = !0; a.positiveValuesOnly && 0 >= w && (w = null); if (a.isXAxis) c = h.xData, c.length && (h = E(c), D(h) || h instanceof Date || (c = v(c, function (a) { return D(a) }), h = E(c)), a.dataMin = Math.min(B(a.dataMin, c[0]), h), a.dataMax = Math.max(B(a.dataMax, c[0]), F(c))); else if (h.getExtremes(),
                        e = h.dataMax, h = h.dataMin, r(h) && r(e) && (a.dataMin = Math.min(B(a.dataMin, h), h), a.dataMax = Math.max(B(a.dataMax, e), e)), r(w) && (a.threshold = w), !c.softThreshold || a.positiveValuesOnly) a.softThreshold = !1
                    }
                })
            }, translate: function (a, b, c, e, p, d) {
                var h = this.linkedParent || this, w = 1, n = 0, q = e ? h.oldTransA : h.transA; e = e ? h.oldMin : h.min; var g = h.minPixelPadding; p = (h.isOrdinal || h.isBroken || h.isLog && p) && h.lin2val; q || (q = h.transA); c && (w *= -1, n = h.len); h.reversed && (w *= -1, n -= w * (h.sector || h.len)); b ? (a = (a * w + n - g) / q + e, p && (a = h.lin2val(a))) :
                (p && (a = h.val2lin(a)), a = w * (a - e) * q + n + w * g + (D(d) ? q * d : 0)); return a
            }, toPixels: function (a, b) { return this.translate(a, !1, !this.horiz, null, !0) + (b ? 0 : this.pos) }, toValue: function (a, b) { return this.translate(a - (b ? 0 : this.pos), !0, !this.horiz, null, !0) }, getPlotLinePath: function (a, b, c, e, p) {
                var h = this.chart, w = this.left, d = this.top, n, q, g = c && h.oldChartHeight || h.chartHeight, k = c && h.oldChartWidth || h.chartWidth, f; n = this.transB; var v = function (a, h, b) { if (a < h || a > b) e ? a = Math.min(Math.max(h, a), b) : f = !0; return a }; p = B(p, this.translate(a,
                null, null, c)); a = c = Math.round(p + n); n = q = Math.round(g - p - n); D(p) ? this.horiz ? (n = d, q = g - this.bottom, a = c = v(a, w, w + this.width)) : (a = w, c = k - this.right, n = q = v(n, d, d + this.height)) : f = !0; return f && !e ? null : h.renderer.crispLine(["M", a, n, "L", c, q], b || 1)
            }, getLinearTickPositions: function (a, b, c) { var h, w = f(Math.floor(b / a) * a); c = f(Math.ceil(c / a) * a); var e = []; if (this.single) return [b]; for (b = w; b <= c;) { e.push(b); b = f(b + a); if (b === h) break; h = b } return e }, getMinorTickPositions: function () {
                var a = this, b = a.options, c = a.tickPositions, e = a.minorTickInterval,
                p = [], d = a.pointRangePadding || 0, n = a.min - d, d = a.max + d, q = d - n; if (q && q / e < a.len / 3) if (a.isLog) g(this.paddedTicks, function (h, b, c) { b && p.push.apply(p, a.getLogTickPositions(e, c[b - 1], c[b], !0)) }); else if (a.isDatetimeAxis && "auto" === b.minorTickInterval) p = p.concat(a.getTimeTicks(a.normalizeTimeTickInterval(e), n, d, b.startOfWeek)); else for (b = n + (c[0] - n) % e; b <= d && b !== p[0]; b += e) p.push(b); 0 !== p.length && a.trimTicks(p); return p
            }, adjustForMinRange: function () {
                var a = this.options, b = this.min, c = this.max, e, p, d, n, q, k, f, v; this.isXAxis &&
                void 0 === this.minRange && !this.isLog && (r(a.min) || r(a.max) ? this.minRange = null : (g(this.series, function (a) { k = a.xData; for (n = f = a.xIncrement ? 1 : k.length - 1; 0 < n; n--) if (q = k[n] - k[n - 1], void 0 === d || q < d) d = q }), this.minRange = Math.min(5 * d, this.dataMax - this.dataMin))); c - b < this.minRange && (p = this.dataMax - this.dataMin >= this.minRange, v = this.minRange, e = (v - c + b) / 2, e = [b - e, B(a.min, b - e)], p && (e[2] = this.isLog ? this.log2lin(this.dataMin) : this.dataMin), b = F(e), c = [b + v, B(a.max, b + v)], p && (c[2] = this.isLog ? this.log2lin(this.dataMax) : this.dataMax),
                c = E(c), c - b < v && (e[0] = c - v, e[1] = B(a.min, c - v), b = F(e))); this.min = b; this.max = c
            }, getClosest: function () { var a; this.categories ? a = 1 : g(this.series, function (h) { var b = h.closestPointRange, c = h.visible || !h.chart.options.chart.ignoreHiddenSeries; !h.noSharedTooltip && r(b) && c && (a = r(a) ? Math.min(a, b) : b) }); return a }, nameToX: function (a) {
                var h = n(this.categories), b = h ? this.categories : this.names, c = a.options.x, e; a.series.requireSorting = !1; r(c) || (c = !1 === this.options.uniqueNames ? a.series.autoIncrement() : y(a.name, b)); -1 === c ? h ||
                (e = b.length) : e = c; void 0 !== e && (this.names[e] = a.name); return e
            }, updateNames: function () { var a = this; 0 < this.names.length && (this.names.length = 0, this.minRange = this.userMinRange, g(this.series || [], function (h) { h.xIncrement = null; if (!h.points || h.isDirtyData) h.processData(), h.generatePoints(); g(h.points, function (b, c) { var e; b.options && (e = a.nameToX(b), void 0 !== e && e !== b.x && (b.x = e, h.xData[c] = e)) }) })) }, setAxisTranslation: function (a) {
                var h = this, b = h.max - h.min, c = h.axisPointRange || 0, e, p = 0, d = 0, n = h.linkedParent, q = !!h.categories,
                k = h.transA, f = h.isXAxis; if (f || q || c) e = h.getClosest(), n ? (p = n.minPointOffset, d = n.pointRangePadding) : g(h.series, function (a) { var b = q ? 1 : f ? B(a.options.pointRange, e, 0) : h.axisPointRange || 0; a = a.options.pointPlacement; c = Math.max(c, b); h.single || (p = Math.max(p, J(a) ? 0 : b / 2), d = Math.max(d, "on" === a ? 0 : b)) }), n = h.ordinalSlope && e ? h.ordinalSlope / e : 1, h.minPointOffset = p *= n, h.pointRangePadding = d *= n, h.pointRange = Math.min(c, b), f && (h.closestPointRange = e); a && (h.oldTransA = k); h.translationSlope = h.transA = k = h.options.staticScale || h.len /
                (b + d || 1); h.transB = h.horiz ? h.left : h.bottom; h.minPixelPadding = k * p
            }, minFromRange: function () { return this.max - this.range }, setTickInterval: function (h) {
                var b = this, c = b.chart, p = b.options, d = b.isLog, n = b.log2lin, q = b.isDatetimeAxis, x = b.isXAxis, v = b.isLinked, z = p.maxPadding, y = p.minPadding, l = p.tickInterval, I = p.tickPixelInterval, m = b.categories, J = b.threshold, t = b.softThreshold, L, u, K, A; q || m || v || this.getTickAmount(); K = B(b.userMin, p.min); A = B(b.userMax, p.max); v ? (b.linkedParent = c[b.coll][p.linkedTo], c = b.linkedParent.getExtremes(),
                b.min = B(c.min, c.dataMin), b.max = B(c.max, c.dataMax), p.type !== b.linkedParent.options.type && a.error(11, 1)) : (!t && r(J) && (b.dataMin >= J ? (L = J, y = 0) : b.dataMax <= J && (u = J, z = 0)), b.min = B(K, L, b.dataMin), b.max = B(A, u, b.dataMax)); d && (b.positiveValuesOnly && !h && 0 >= Math.min(b.min, B(b.dataMin, b.min)) && a.error(10, 1), b.min = f(n(b.min), 15), b.max = f(n(b.max), 15)); b.range && r(b.max) && (b.userMin = b.min = K = Math.max(b.dataMin, b.minFromRange()), b.userMax = A = b.max, b.range = null); k(b, "foundExtremes"); b.beforePadding && b.beforePadding(); b.adjustForMinRange();
                !(m || b.axisPointRange || b.usePercentage || v) && r(b.min) && r(b.max) && (n = b.max - b.min) && (!r(K) && y && (b.min -= n * y), !r(A) && z && (b.max += n * z)); D(p.softMin) && (b.min = Math.min(b.min, p.softMin)); D(p.softMax) && (b.max = Math.max(b.max, p.softMax)); D(p.floor) && (b.min = Math.max(b.min, p.floor)); D(p.ceiling) && (b.max = Math.min(b.max, p.ceiling)); t && r(b.dataMin) && (J = J || 0, !r(K) && b.min < J && b.dataMin >= J ? b.min = J : !r(A) && b.max > J && b.dataMax <= J && (b.max = J)); b.tickInterval = b.min === b.max || void 0 === b.min || void 0 === b.max ? 1 : v && !l && I === b.linkedParent.options.tickPixelInterval ?
                l = b.linkedParent.tickInterval : B(l, this.tickAmount ? (b.max - b.min) / Math.max(this.tickAmount - 1, 1) : void 0, m ? 1 : (b.max - b.min) * I / Math.max(b.len, I)); x && !h && g(b.series, function (a) { a.processData(b.min !== b.oldMin || b.max !== b.oldMax) }); b.setAxisTranslation(!0); b.beforeSetTickPositions && b.beforeSetTickPositions(); b.postProcessTickInterval && (b.tickInterval = b.postProcessTickInterval(b.tickInterval)); b.pointRange && !l && (b.tickInterval = Math.max(b.pointRange, b.tickInterval)); h = B(p.minTickInterval, b.isDatetimeAxis && b.closestPointRange);
                !l && b.tickInterval < h && (b.tickInterval = h); q || d || l || (b.tickInterval = G(b.tickInterval, null, e(b.tickInterval), B(p.allowDecimals, !(.5 < b.tickInterval && 5 > b.tickInterval && 1E3 < b.max && 9999 > b.max)), !!this.tickAmount)); this.tickAmount || (b.tickInterval = b.unsquish()); this.setTickPositions()
            }, setTickPositions: function () {
                var a = this.options, b, c = a.tickPositions, e = a.tickPositioner, p = a.startOnTick, d = a.endOnTick; this.tickmarkOffset = this.categories && "between" === a.tickmarkPlacement && 1 === this.tickInterval ? .5 : 0; this.minorTickInterval =
                "auto" === a.minorTickInterval && this.tickInterval ? this.tickInterval / 5 : a.minorTickInterval; this.single = this.min === this.max && r(this.min) && !this.tickAmount && (parseInt(this.min, 10) === this.min || !1 !== a.allowDecimals); this.tickPositions = b = c && c.slice(); !b && (b = this.isDatetimeAxis ? this.getTimeTicks(this.normalizeTimeTickInterval(this.tickInterval, a.units), this.min, this.max, a.startOfWeek, this.ordinalPositions, this.closestPointRange, !0) : this.isLog ? this.getLogTickPositions(this.tickInterval, this.min, this.max) : this.getLinearTickPositions(this.tickInterval,
                this.min, this.max), b.length > this.len && (b = [b[0], b.pop()]), this.tickPositions = b, e && (e = e.apply(this, [this.min, this.max]))) && (this.tickPositions = b = e); this.paddedTicks = b.slice(0); this.trimTicks(b, p, d); this.isLinked || (this.single && 2 > b.length && (this.min -= .5, this.max += .5), c || e || this.adjustTickAmount())
            }, trimTicks: function (a, b, c) {
                var h = a[0], e = a[a.length - 1], p = this.minPointOffset || 0; if (!this.isLinked) {
                    if (b && -Infinity !== h) this.min = h; else for (; this.min - p > a[0];) a.shift(); if (c) this.max = e; else for (; this.max + p < a[a.length -
                    1];) a.pop(); 0 === a.length && r(h) && a.push((e + h) / 2)
                }
            }, alignToOthers: function () { var a = {}, b, c = this.options; !1 === this.chart.options.chart.alignTicks || !1 === c.alignTicks || this.isLog || g(this.chart[this.coll], function (h) { var c = h.options, c = [h.horiz ? c.left : c.top, c.width, c.height, c.pane].join(); h.series.length && (a[c] ? b = !0 : a[c] = 1) }); return b }, getTickAmount: function () {
                var a = this.options, b = a.tickAmount, c = a.tickPixelInterval; !r(a.tickInterval) && this.len < c && !this.isRadial && !this.isLog && a.startOnTick && a.endOnTick && (b =
                2); !b && this.alignToOthers() && (b = Math.ceil(this.len / c) + 1); 4 > b && (this.finalTickAmt = b, b = 5); this.tickAmount = b
            }, adjustTickAmount: function () { var a = this.tickInterval, b = this.tickPositions, c = this.tickAmount, e = this.finalTickAmt, p = b && b.length; if (p < c) { for (; b.length < c;) b.push(f(b[b.length - 1] + a)); this.transA *= (p - 1) / (c - 1); this.max = b[b.length - 1] } else p > c && (this.tickInterval *= 2, this.setTickPositions()); if (r(e)) { for (a = c = b.length; a--;) (3 === e && 1 === a % 2 || 2 >= e && 0 < a && a < c - 1) && b.splice(a, 1); this.finalTickAmt = void 0 } }, setScale: function () {
                var a,
                b; this.oldMin = this.min; this.oldMax = this.max; this.oldAxisLength = this.len; this.setAxisSize(); b = this.len !== this.oldAxisLength; g(this.series, function (b) { if (b.isDirtyData || b.isDirty || b.xAxis.isDirty) a = !0 }); b || a || this.isLinked || this.forceRedraw || this.userMin !== this.oldUserMin || this.userMax !== this.oldUserMax || this.alignToOthers() ? (this.resetStacks && this.resetStacks(), this.forceRedraw = !1, this.getSeriesExtremes(), this.setTickInterval(), this.oldUserMin = this.userMin, this.oldUserMax = this.userMax, this.isDirty ||
                (this.isDirty = b || this.min !== this.oldMin || this.max !== this.oldMax)) : this.cleanStacks && this.cleanStacks()
            }, setExtremes: function (a, b, c, e, p) { var h = this, n = h.chart; c = B(c, !0); g(h.series, function (a) { delete a.kdTree }); p = d(p, { min: a, max: b }); k(h, "setExtremes", p, function () { h.userMin = a; h.userMax = b; h.eventArgs = p; c && n.redraw(e) }) }, zoom: function (a, b) {
                var h = this.dataMin, c = this.dataMax, e = this.options, p = Math.min(h, B(e.min, h)), e = Math.max(c, B(e.max, c)); if (a !== this.min || b !== this.max) this.allowZoomOutside || (r(h) && (a < p && (a =
                p), a > e && (a = e)), r(c) && (b < p && (b = p), b > e && (b = e))), this.displayBtn = void 0 !== a || void 0 !== b, this.setExtremes(a, b, !1, void 0, { trigger: "zoom" }); return !0
            }, setAxisSize: function () {
                var b = this.chart, c = this.options, e = c.offsets || [0, 0, 0, 0], p = this.horiz, d = this.width = Math.round(a.relativeLength(B(c.width, b.plotWidth - e[3] + e[1]), b.plotWidth)), n = this.height = Math.round(a.relativeLength(B(c.height, b.plotHeight - e[0] + e[2]), b.plotHeight)), q = this.top = Math.round(a.relativeLength(B(c.top, b.plotTop + e[0]), b.plotHeight, b.plotTop)),
                c = this.left = Math.round(a.relativeLength(B(c.left, b.plotLeft + e[3]), b.plotWidth, b.plotLeft)); this.bottom = b.chartHeight - n - q; this.right = b.chartWidth - d - c; this.len = Math.max(p ? d : n, 0); this.pos = p ? c : q
            }, getExtremes: function () { var a = this.isLog, b = this.lin2log; return { min: a ? f(b(this.min)) : this.min, max: a ? f(b(this.max)) : this.max, dataMin: this.dataMin, dataMax: this.dataMax, userMin: this.userMin, userMax: this.userMax } }, getThreshold: function (a) {
                var b = this.isLog, h = this.lin2log, c = b ? h(this.min) : this.min, b = b ? h(this.max) : this.max;
                null === a ? a = c : c > a ? a = c : b < a && (a = b); return this.translate(a, 0, 1, 0, 1)
            }, autoLabelAlign: function (a) { a = (B(a, 0) - 90 * this.side + 720) % 360; return 15 < a && 165 > a ? "right" : 195 < a && 345 > a ? "left" : "center" }, tickSize: function (a) { var b = this.options, h = b[a + "Length"], c = B(b[a + "Width"], "tick" === a && this.isXAxis ? 1 : 0); if (c && h) return "inside" === b[a + "Position"] && (h = -h), [h, c] }, labelMetrics: function () {
                var a = this.tickPositions && this.tickPositions[0] || 0; return this.chart.renderer.fontMetrics(this.options.labels.style && this.options.labels.style.fontSize,
                this.ticks[a] && this.ticks[a].label)
            }, unsquish: function () {
                var a = this.options.labels, b = this.horiz, c = this.tickInterval, e = c, p = this.len / (((this.categories ? 1 : 0) + this.max - this.min) / c), d, n = a.rotation, q = this.labelMetrics(), k, f = Number.MAX_VALUE, v, z = function (a) { a /= p || 1; a = 1 < a ? Math.ceil(a) : 1; return a * c }; b ? (v = !a.staggerLines && !a.step && (r(n) ? [n] : p < B(a.autoRotationLimit, 80) && a.autoRotation)) && g(v, function (a) { var b; if (a === n || a && -90 <= a && 90 >= a) k = z(Math.abs(q.h / Math.sin(u * a))), b = k + Math.abs(a / 360), b < f && (f = b, d = a, e = k) }) :
                a.step || (e = z(q.h)); this.autoRotation = v; this.labelRotation = B(d, n); return e
            }, getSlotWidth: function () { var a = this.chart, b = this.horiz, c = this.options.labels, e = Math.max(this.tickPositions.length - (this.categories ? 0 : 1), 1), p = a.margin[3]; return b && 2 > (c.step || 0) && !c.rotation && (this.staggerLines || 1) * this.len / e || !b && (p && p - a.spacing[3] || .33 * a.chartWidth) }, renderUnsquish: function () {
                var a = this.chart, b = a.renderer, e = this.tickPositions, p = this.ticks, d = this.options.labels, n = this.horiz, q = this.getSlotWidth(), k = Math.max(1,
                Math.round(q - 2 * (d.padding || 5))), f = {}, v = this.labelMetrics(), z = d.style && d.style.textOverflow, D, y = 0, l, I; J(d.rotation) || (f.rotation = d.rotation || 0); g(e, function (a) { (a = p[a]) && a.labelLength > y && (y = a.labelLength) }); this.maxLabelLength = y; if (this.autoRotation) y > k && y > v.h ? f.rotation = this.labelRotation : this.labelRotation = 0; else if (q && (D = { width: k + "px" }, !z)) for (D.textOverflow = "clip", l = e.length; !n && l--;) if (I = e[l], k = p[I].label) k.styles && "ellipsis" === k.styles.textOverflow ? k.css({ textOverflow: "clip" }) : p[I].labelLength >
                q && k.css({ width: q + "px" }), k.getBBox().height > this.len / e.length - (v.h - v.f) && (k.specCss = { textOverflow: "ellipsis" }); f.rotation && (D = { width: (y > .5 * a.chartHeight ? .33 * a.chartHeight : a.chartHeight) + "px" }, z || (D.textOverflow = "ellipsis")); if (this.labelAlign = d.align || this.autoLabelAlign(this.labelRotation)) f.align = this.labelAlign; g(e, function (a) { var b = (a = p[a]) && a.label; b && (b.attr(f), D && b.css(c(D, b.specCss)), delete b.specCss, a.rotation = f.rotation) }); this.tickRotCorr = b.rotCorr(v.b, this.labelRotation || 0, 0 !== this.side)
            },
            hasData: function () { return this.hasVisibleSeries || r(this.min) && r(this.max) && !!this.tickPositions }, addTitle: function (a) {
                var b = this.chart.renderer, c = this.horiz, h = this.opposite, e = this.options.title, p; this.axisTitle || ((p = e.textAlign) || (p = (c ? { low: "left", middle: "center", high: "right" } : { low: h ? "right" : "left", middle: "center", high: h ? "left" : "right" })[e.align]), this.axisTitle = b.text(e.text, 0, 0, e.useHTML).attr({ zIndex: 7, rotation: e.rotation || 0, align: p }).addClass("highcharts-axis-title").css(e.style).add(this.axisGroup),
                this.axisTitle.isNew = !0); e.style.width || this.isRadial || this.axisTitle.css({ width: this.len }); this.axisTitle[a ? "show" : "hide"](!0)
            }, generateTick: function (a) { var b = this.ticks; b[a] ? b[a].addLabel() : b[a] = new I(this, a) }, getOffset: function () {
                var a = this, b = a.chart, c = b.renderer, e = a.options, p = a.tickPositions, d = a.ticks, n = a.horiz, k = a.side, f = b.inverted && !a.isZAxis ? [1, 0, 3, 2][k] : k, v, z, D = 0, y, l = 0, I = e.title, m = e.labels, G = 0, J = b.axisOffset, b = b.clipOffset, t = [-1, 1, 1, -1][k], L = e.className, u = a.axisParent, K = this.tickSize("tick");
                v = a.hasData(); a.showAxis = z = v || B(e.showEmpty, !0); a.staggerLines = a.horiz && m.staggerLines; a.axisGroup || (a.gridGroup = c.g("grid").attr({ zIndex: e.gridZIndex || 1 }).addClass("highcharts-" + this.coll.toLowerCase() + "-grid " + (L || "")).add(u), a.axisGroup = c.g("axis").attr({ zIndex: e.zIndex || 2 }).addClass("highcharts-" + this.coll.toLowerCase() + " " + (L || "")).add(u), a.labelGroup = c.g("axis-labels").attr({ zIndex: m.zIndex || 7 }).addClass("highcharts-" + a.coll.toLowerCase() + "-labels " + (L || "")).add(u)); v || a.isLinked ? (g(p, function (b,
                c) { a.generateTick(b, c) }), a.renderUnsquish(), !1 === m.reserveSpace || 0 !== k && 2 !== k && { 1: "left", 3: "right" }[k] !== a.labelAlign && "center" !== a.labelAlign || g(p, function (a) { G = Math.max(d[a].getLabelSize(), G) }), a.staggerLines && (G *= a.staggerLines, a.labelOffset = G * (a.opposite ? -1 : 1))) : q(d, function (a, b) { a.destroy(); delete d[b] }); I && I.text && !1 !== I.enabled && (a.addTitle(z), z && !1 !== I.reserveSpace && (a.titleOffset = D = a.axisTitle.getBBox()[n ? "height" : "width"], y = I.offset, l = r(y) ? 0 : B(I.margin, n ? 5 : 10))); a.renderLine(); a.offset =
                t * B(e.offset, J[k]); a.tickRotCorr = a.tickRotCorr || { x: 0, y: 0 }; c = 0 === k ? -a.labelMetrics().h : 2 === k ? a.tickRotCorr.y : 0; l = Math.abs(G) + l; G && (l = l - c + t * (n ? B(m.y, a.tickRotCorr.y + 8 * t) : m.x)); a.axisTitleMargin = B(y, l); J[k] = Math.max(J[k], a.axisTitleMargin + D + t * a.offset, l, v && p.length && K ? K[0] + t * a.offset : 0); p = 2 * Math.floor(a.axisLine.strokeWidth() / 2); 0 < e.offset && (p -= 2 * e.offset); b[f] = Math.max(b[f] || p, p)
            }, getLinePath: function (a) {
                var b = this.chart, c = this.opposite, h = this.offset, e = this.horiz, p = this.left + (c ? this.width : 0) + h, h = b.chartHeight -
                this.bottom - (c ? this.height : 0) + h; c && (a *= -1); return b.renderer.crispLine(["M", e ? this.left : p, e ? h : this.top, "L", e ? b.chartWidth - this.right : p, e ? h : b.chartHeight - this.bottom], a)
            }, renderLine: function () { this.axisLine || (this.axisLine = this.chart.renderer.path().addClass("highcharts-axis-line").add(this.axisGroup), this.axisLine.attr({ stroke: this.options.lineColor, "stroke-width": this.options.lineWidth, zIndex: 7 })) }, getTitlePosition: function () {
                var a = this.horiz, b = this.left, c = this.top, e = this.len, p = this.options.title,
                d = a ? b : c, n = this.opposite, q = this.offset, k = p.x || 0, g = p.y || 0, f = this.axisTitle, v = this.chart.renderer.fontMetrics(p.style && p.style.fontSize, f), f = Math.max(f.getBBox(null, 0).height - v.h - 1, 0), e = { low: d + (a ? 0 : e), middle: d + e / 2, high: d + (a ? e : 0) }[p.align], b = (a ? c + this.height : b) + (a ? 1 : -1) * (n ? -1 : 1) * this.axisTitleMargin + [-f, f, v.f, -f][this.side]; return { x: a ? e + k : b + (n ? this.width : 0) + q + k, y: a ? b + g - (n ? this.height : 0) + q : e + g }
            }, renderMinorTick: function (a) {
                var b = this.chart.hasRendered && D(this.oldMin), c = this.minorTicks; c[a] || (c[a] = new I(this,
                a, "minor")); b && c[a].isNew && c[a].render(null, !0); c[a].render(null, !1, 1)
            }, renderTick: function (a, b) { var c = this.isLinked, e = this.ticks, h = this.chart.hasRendered && D(this.oldMin); if (!c || a >= this.min && a <= this.max) e[a] || (e[a] = new I(this, a)), h && e[a].isNew && e[a].render(b, !0, .1), e[a].render(b) }, render: function () {
                var b = this, c = b.chart, e = b.options, p = b.isLog, d = b.lin2log, n = b.isLinked, k = b.tickPositions, f = b.axisTitle, v = b.ticks, y = b.minorTicks, l = b.alternateBands, m = e.stackLabels, r = e.alternateGridColor, B = b.tickmarkOffset,
                G = b.axisLine, J = b.showAxis, t = A(c.renderer.globalAnimation), L, u; b.labelEdge.length = 0; b.overlap = !1; g([v, y, l], function (a) { q(a, function (a) { a.isActive = !1 }) }); if (b.hasData() || n) b.minorTickInterval && !b.categories && g(b.getMinorTickPositions(), function (a) { b.renderMinorTick(a) }), k.length && (g(k, function (a, c) { b.renderTick(a, c) }), B && (0 === b.min || b.single) && (v[-1] || (v[-1] = new I(b, -1, null, !0)), v[-1].render(-1))), r && g(k, function (e, h) {
                    u = void 0 !== k[h + 1] ? k[h + 1] + B : b.max - B; 0 === h % 2 && e < b.max && u <= b.max + (c.polar ? -B : B) && (l[e] ||
                    (l[e] = new a.PlotLineOrBand(b)), L = e + B, l[e].options = { from: p ? d(L) : L, to: p ? d(u) : u, color: r }, l[e].render(), l[e].isActive = !0)
                }), b._addedPlotLB || (g((e.plotLines || []).concat(e.plotBands || []), function (a) { b.addPlotBandOrLine(a) }), b._addedPlotLB = !0); g([v, y, l], function (a) { var b, e = [], h = t.duration; q(a, function (a, b) { a.isActive || (a.render(b, !1, 0), a.isActive = !1, e.push(b)) }); z(function () { for (b = e.length; b--;) a[e[b]] && !a[e[b]].isActive && (a[e[b]].destroy(), delete a[e[b]]) }, a !== l && c.hasRendered && h ? h : 0) }); G && (G[G.isPlaced ?
                "animate" : "attr"]({ d: this.getLinePath(G.strokeWidth()) }), G.isPlaced = !0, G[J ? "show" : "hide"](!0)); f && J && (e = b.getTitlePosition(), D(e.y) ? (f[f.isNew ? "attr" : "animate"](e), f.isNew = !1) : (f.attr("y", -9999), f.isNew = !0)); m && m.enabled && b.renderStackTotals(); b.isDirty = !1
            }, redraw: function () { this.visible && (this.render(), g(this.plotLinesAndBands, function (a) { a.render() })); g(this.series, function (a) { a.isDirty = !0 }) }, keepProps: "extKey hcEvents names series userMax userMin".split(" "), destroy: function (a) {
                var b = this, c = b.stacks,
                e = b.plotLinesAndBands, h; a || K(b); q(c, function (a, b) { t(a); c[b] = null }); g([b.ticks, b.minorTicks, b.alternateBands], function (a) { t(a) }); if (e) for (a = e.length; a--;) e[a].destroy(); g("stackTotalGroup axisLine axisTitle axisGroup gridGroup labelGroup cross".split(" "), function (a) { b[a] && (b[a] = b[a].destroy()) }); for (h in b.plotLinesAndBandsGroups) b.plotLinesAndBandsGroups[h] = b.plotLinesAndBandsGroups[h].destroy(); q(b, function (a, c) { -1 === y(c, b.keepProps) && delete b[c] })
            }, drawCrosshair: function (a, b) {
                var c, e = this.crosshair,
                h = B(e.snap, !0), p, d = this.cross; a || (a = this.cross && this.cross.e); this.crosshair && !1 !== (r(b) || !h) ? (h ? r(b) && (p = this.isXAxis ? b.plotX : this.len - b.plotY) : p = a && (this.horiz ? a.chartX - this.pos : this.len - a.chartY + this.pos), r(p) && (c = this.getPlotLinePath(b && (this.isXAxis ? b.x : B(b.stackY, b.y)), null, null, null, p) || null), r(c) ? (b = this.categories && !this.isRadial, d || (this.cross = d = this.chart.renderer.path().addClass("highcharts-crosshair highcharts-crosshair-" + (b ? "category " : "thin ") + e.className).attr({ zIndex: B(e.zIndex, 2) }).add(),
                d.attr({ stroke: e.color || (b ? m("#ccd6eb").setOpacity(.25).get() : "#cccccc"), "stroke-width": B(e.width, 1) }), e.dashStyle && d.attr({ dashstyle: e.dashStyle })), d.show().attr({ d: c }), b && !e.width && d.attr({ "stroke-width": this.transA }), this.cross.e = a) : this.hideCrosshair()) : this.hideCrosshair()
            }, hideCrosshair: function () { this.cross && this.cross.hide() }
        }); return a.Axis = L
    }(M); (function (a) {
        var C = a.Axis, A = a.Date, F = a.dateFormat, E = a.defaultOptions, m = a.defined, f = a.each, l = a.extend, r = a.getMagnitude, u = a.getTZOffset, t = a.normalizeTickInterval,
        g = a.pick, d = a.timeUnits; C.prototype.getTimeTicks = function (a, b, e, v) {
            var k = [], n = {}, D = E.global.useUTC, r, c = new A(b - Math.max(u(b), u(e))), G = A.hcMakeTime, q = a.unitRange, B = a.count, t, p; if (m(b)) {
                c[A.hcSetMilliseconds](q >= d.second ? 0 : B * Math.floor(c.getMilliseconds() / B)); if (q >= d.second) c[A.hcSetSeconds](q >= d.minute ? 0 : B * Math.floor(c.getSeconds() / B)); if (q >= d.minute) c[A.hcSetMinutes](q >= d.hour ? 0 : B * Math.floor(c[A.hcGetMinutes]() / B)); if (q >= d.hour) c[A.hcSetHours](q >= d.day ? 0 : B * Math.floor(c[A.hcGetHours]() / B)); if (q >= d.day) c[A.hcSetDate](q >=
                d.month ? 1 : B * Math.floor(c[A.hcGetDate]() / B)); q >= d.month && (c[A.hcSetMonth](q >= d.year ? 0 : B * Math.floor(c[A.hcGetMonth]() / B)), r = c[A.hcGetFullYear]()); if (q >= d.year) c[A.hcSetFullYear](r - r % B); if (q === d.week) c[A.hcSetDate](c[A.hcGetDate]() - c[A.hcGetDay]() + g(v, 1)); r = c[A.hcGetFullYear](); v = c[A.hcGetMonth](); var z = c[A.hcGetDate](), I = c[A.hcGetHours](); if (A.hcTimezoneOffset || A.hcGetTimezoneOffset) p = (!D || !!A.hcGetTimezoneOffset) && (e - b > 4 * d.month || u(b) !== u(e)), c = c.getTime(), t = u(c), c = new A(c + t); D = c.getTime(); for (b = 1; D <
                e;) k.push(D), D = q === d.year ? G(r + b * B, 0) : q === d.month ? G(r, v + b * B) : !p || q !== d.day && q !== d.week ? p && q === d.hour ? G(r, v, z, I + b * B, 0, 0, t) - t : D + q * B : G(r, v, z + b * B * (q === d.day ? 1 : 7)), b++; k.push(D); q <= d.hour && 1E4 > k.length && f(k, function (a) { 0 === a % 18E5 && "000000000" === F("%H%M%S%L", a) && (n[a] = "day") })
            } k.info = l(a, { higherRanks: n, totalRange: q * B }); return k
        }; C.prototype.normalizeTimeTickInterval = function (a, b) {
            var e = b || [["millisecond", [1, 2, 5, 10, 20, 25, 50, 100, 200, 500]], ["second", [1, 2, 5, 10, 15, 30]], ["minute", [1, 2, 5, 10, 15, 30]], ["hour", [1,
            2, 3, 4, 6, 8, 12]], ["day", [1, 2]], ["week", [1, 2]], ["month", [1, 2, 3, 4, 6]], ["year", null]]; b = e[e.length - 1]; var k = d[b[0]], g = b[1], n; for (n = 0; n < e.length && !(b = e[n], k = d[b[0]], g = b[1], e[n + 1] && a <= (k * g[g.length - 1] + d[e[n + 1][0]]) / 2) ; n++); k === d.year && a < 5 * k && (g = [1, 2, 5]); a = t(a / k, g, "year" === b[0] ? Math.max(r(a / k), 1) : 1); return { unitRange: k, count: a, unitName: b[0] }
        }
    })(M); (function (a) {
        var C = a.Axis, A = a.getMagnitude, F = a.map, E = a.normalizeTickInterval, m = a.pick; C.prototype.getLogTickPositions = function (a, l, r, u) {
            var f = this.options, g = this.len,
            d = this.lin2log, k = this.log2lin, b = []; u || (this._minorAutoInterval = null); if (.5 <= a) a = Math.round(a), b = this.getLinearTickPositions(a, l, r); else if (.08 <= a) for (var g = Math.floor(l), e, v, y, n, D, f = .3 < a ? [1, 2, 4] : .15 < a ? [1, 2, 4, 6, 8] : [1, 2, 3, 4, 5, 6, 7, 8, 9]; g < r + 1 && !D; g++) for (v = f.length, e = 0; e < v && !D; e++) y = k(d(g) * f[e]), y > l && (!u || n <= r) && void 0 !== n && b.push(n), n > r && (D = !0), n = y; else l = d(l), r = d(r), a = f[u ? "minorTickInterval" : "tickInterval"], a = m("auto" === a ? null : a, this._minorAutoInterval, f.tickPixelInterval / (u ? 5 : 1) * (r - l) / ((u ? g / this.tickPositions.length :
            g) || 1)), a = E(a, null, A(a)), b = F(this.getLinearTickPositions(a, l, r), k), u || (this._minorAutoInterval = a / 5); u || (this.tickInterval = a); return b
        }; C.prototype.log2lin = function (a) { return Math.log(a) / Math.LN10 }; C.prototype.lin2log = function (a) { return Math.pow(10, a) }
    })(M); (function (a, C) {
        var A = a.arrayMax, F = a.arrayMin, E = a.defined, m = a.destroyObjectProperties, f = a.each, l = a.erase, r = a.merge, u = a.pick; a.PlotLineOrBand = function (a, g) { this.axis = a; g && (this.options = g, this.id = g.id) }; a.PlotLineOrBand.prototype = {
            render: function () {
                var f =
                this, g = f.axis, d = g.horiz, k = f.options, b = k.label, e = f.label, v = k.to, l = k.from, n = k.value, D = E(l) && E(v), m = E(n), c = f.svgElem, G = !c, q = [], B = k.color, K = u(k.zIndex, 0), p = k.events, q = { "class": "highcharts-plot-" + (D ? "band " : "line ") + (k.className || "") }, z = {}, I = g.chart.renderer, L = D ? "bands" : "lines", h = g.log2lin; g.isLog && (l = h(l), v = h(v), n = h(n)); m ? (q = { stroke: B, "stroke-width": k.width }, k.dashStyle && (q.dashstyle = k.dashStyle)) : D && (B && (q.fill = B), k.borderWidth && (q.stroke = k.borderColor, q["stroke-width"] = k.borderWidth)); z.zIndex = K; L +=
                "-" + K; (B = g.plotLinesAndBandsGroups[L]) || (g.plotLinesAndBandsGroups[L] = B = I.g("plot-" + L).attr(z).add()); G && (f.svgElem = c = I.path().attr(q).add(B)); if (m) q = g.getPlotLinePath(n, c.strokeWidth()); else if (D) q = g.getPlotBandPath(l, v, k); else return; G && q && q.length ? (c.attr({ d: q }), p && a.objectEach(p, function (a, b) { c.on(b, function (a) { p[b].apply(f, [a]) }) })) : c && (q ? (c.show(), c.animate({ d: q })) : (c.hide(), e && (f.label = e = e.destroy()))); b && E(b.text) && q && q.length && 0 < g.width && 0 < g.height && !q.flat ? (b = r({
                    align: d && D && "center", x: d ?
                    !D && 4 : 10, verticalAlign: !d && D && "middle", y: d ? D ? 16 : 10 : D ? 6 : -4, rotation: d && !D && 90
                }, b), this.renderLabel(b, q, D, K)) : e && e.hide(); return f
            }, renderLabel: function (a, g, d, k) {
                var b = this.label, e = this.axis.chart.renderer; b || (b = { align: a.textAlign || a.align, rotation: a.rotation, "class": "highcharts-plot-" + (d ? "band" : "line") + "-label " + (a.className || "") }, b.zIndex = k, this.label = b = e.text(a.text, 0, 0, a.useHTML).attr(b).add(), b.css(a.style)); k = [g[1], g[4], d ? g[6] : g[1]]; g = [g[2], g[5], d ? g[7] : g[2]]; d = F(k); e = F(g); b.align(a, !1, {
                    x: d, y: e,
                    width: A(k) - d, height: A(g) - e
                }); b.show()
            }, destroy: function () { l(this.axis.plotLinesAndBands, this); delete this.axis; m(this) }
        }; a.extend(C.prototype, {
            getPlotBandPath: function (a, g) { var d = this.getPlotLinePath(g, null, null, !0), k = this.getPlotLinePath(a, null, null, !0), b = this.horiz, e = 1; a = a < this.min && g < this.min || a > this.max && g > this.max; k && d ? (a && (k.flat = k.toString() === d.toString(), e = 0), k.push(b && d[4] === k[4] ? d[4] + e : d[4], b || d[5] !== k[5] ? d[5] : d[5] + e, b && d[1] === k[1] ? d[1] + e : d[1], b || d[2] !== k[2] ? d[2] : d[2] + e)) : k = null; return k },
            addPlotBand: function (a) { return this.addPlotBandOrLine(a, "plotBands") }, addPlotLine: function (a) { return this.addPlotBandOrLine(a, "plotLines") }, addPlotBandOrLine: function (f, g) { var d = (new a.PlotLineOrBand(this, f)).render(), k = this.userOptions; d && (g && (k[g] = k[g] || [], k[g].push(f)), this.plotLinesAndBands.push(d)); return d }, removePlotBandOrLine: function (a) {
                for (var g = this.plotLinesAndBands, d = this.options, k = this.userOptions, b = g.length; b--;) g[b].id === a && g[b].destroy(); f([d.plotLines || [], k.plotLines || [], d.plotBands ||
                [], k.plotBands || []], function (e) { for (b = e.length; b--;) e[b].id === a && l(e, e[b]) })
            }, removePlotBand: function (a) { this.removePlotBandOrLine(a) }, removePlotLine: function (a) { this.removePlotBandOrLine(a) }
        })
    })(M, S); (function (a) {
        var C = a.dateFormat, A = a.each, F = a.extend, E = a.format, m = a.isNumber, f = a.map, l = a.merge, r = a.pick, u = a.splat, t = a.syncTimeout, g = a.timeUnits; a.Tooltip = function () { this.init.apply(this, arguments) }; a.Tooltip.prototype = {
            init: function (a, k) {
                this.chart = a; this.options = k; this.crosshairs = []; this.now = { x: 0, y: 0 };
                this.isHidden = !0; this.split = k.split && !a.inverted; this.shared = k.shared || this.split
            }, cleanSplit: function (a) { A(this.chart.series, function (d) { var b = d && d.tt; b && (!b.isActive || a ? d.tt = b.destroy() : b.isActive = !1) }) }, getLabel: function () {
                var a = this.chart.renderer, k = this.options; this.label || (this.split ? this.label = a.g("tooltip") : (this.label = a.label("", 0, 0, k.shape || "callout", null, null, k.useHTML, null, "tooltip").attr({ padding: k.padding, r: k.borderRadius }), this.label.attr({ fill: k.backgroundColor, "stroke-width": k.borderWidth }).css(k.style).shadow(k.shadow)),
                this.label.attr({ zIndex: 8 }).add()); return this.label
            }, update: function (a) { this.destroy(); l(!0, this.chart.options.tooltip.userOptions, a); this.init(this.chart, l(!0, this.options, a)) }, destroy: function () { this.label && (this.label = this.label.destroy()); this.split && this.tt && (this.cleanSplit(this.chart, !0), this.tt = this.tt.destroy()); clearTimeout(this.hideTimer); clearTimeout(this.tooltipTimeout) }, move: function (a, k, b, e) {
                var d = this, g = d.now, n = !1 !== d.options.animation && !d.isHidden && (1 < Math.abs(a - g.x) || 1 < Math.abs(k -
                g.y)), f = d.followPointer || 1 < d.len; F(g, { x: n ? (2 * g.x + a) / 3 : a, y: n ? (g.y + k) / 2 : k, anchorX: f ? void 0 : n ? (2 * g.anchorX + b) / 3 : b, anchorY: f ? void 0 : n ? (g.anchorY + e) / 2 : e }); d.getLabel().attr(g); n && (clearTimeout(this.tooltipTimeout), this.tooltipTimeout = setTimeout(function () { d && d.move(a, k, b, e) }, 32))
            }, hide: function (a) { var d = this; clearTimeout(this.hideTimer); a = r(a, this.options.hideDelay, 500); this.isHidden || (this.hideTimer = t(function () { d.getLabel()[a ? "fadeOut" : "hide"](); d.isHidden = !0 }, a)) }, getAnchor: function (a, k) {
                var b, e = this.chart,
                d = e.inverted, g = e.plotTop, n = e.plotLeft, l = 0, m = 0, c, r; a = u(a); b = a[0].tooltipPos; this.followPointer && k && (void 0 === k.chartX && (k = e.pointer.normalize(k)), b = [k.chartX - e.plotLeft, k.chartY - g]); b || (A(a, function (a) { c = a.series.yAxis; r = a.series.xAxis; l += a.plotX + (!d && r ? r.left - n : 0); m += (a.plotLow ? (a.plotLow + a.plotHigh) / 2 : a.plotY) + (!d && c ? c.top - g : 0) }), l /= a.length, m /= a.length, b = [d ? e.plotWidth - m : l, this.shared && !d && 1 < a.length && k ? k.chartY - g : d ? e.plotHeight - l : m]); return f(b, Math.round)
            }, getPosition: function (a, g, b) {
                var e = this.chart,
                d = this.distance, k = {}, n = b.h || 0, f, l = ["y", e.chartHeight, g, b.plotY + e.plotTop, e.plotTop, e.plotTop + e.plotHeight], c = ["x", e.chartWidth, a, b.plotX + e.plotLeft, e.plotLeft, e.plotLeft + e.plotWidth], m = !this.followPointer && r(b.ttBelow, !e.inverted === !!b.negative), q = function (a, b, c, e, p, q) { var h = c < e - d, g = e + d + c < b, f = e - d - c; e += d; if (m && g) k[a] = e; else if (!m && h) k[a] = f; else if (h) k[a] = Math.min(q - c, 0 > f - n ? f : f - n); else if (g) k[a] = Math.max(p, e + n + c > b ? e : e + n); else return !1 }, B = function (a, b, c, e) {
                    var h; e < d || e > b - d ? h = !1 : k[a] = e < c / 2 ? 1 : e > b - c / 2 ?
                    b - c - 2 : e - c / 2; return h
                }, t = function (a) { var b = l; l = c; c = b; f = a }, p = function () { !1 !== q.apply(0, l) ? !1 !== B.apply(0, c) || f || (t(!0), p()) : f ? k.x = k.y = 0 : (t(!0), p()) }; (e.inverted || 1 < this.len) && t(); p(); return k
            }, defaultFormatter: function (a) { var d = this.points || u(this), b; b = [a.tooltipFooterHeaderFormatter(d[0])]; b = b.concat(a.bodyFormatter(d)); b.push(a.tooltipFooterHeaderFormatter(d[0], !0)); return b }, refresh: function (a, g) {
                var b, e = this.options, d, k = a, n, f = {}, l = []; b = e.formatter || this.defaultFormatter; var f = this.shared, c; e.enabled &&
                (clearTimeout(this.hideTimer), this.followPointer = u(k)[0].series.tooltipOptions.followPointer, n = this.getAnchor(k, g), g = n[0], d = n[1], !f || k.series && k.series.noSharedTooltip ? f = k.getLabelConfig() : (A(k, function (a) { a.setState("hover"); l.push(a.getLabelConfig()) }), f = { x: k[0].category, y: k[0].y }, f.points = l, k = k[0]), this.len = l.length, f = b.call(f, this), c = k.series, this.distance = r(c.tooltipOptions.distance, 16), !1 === f ? this.hide() : (b = this.getLabel(), this.isHidden && b.attr({ opacity: 1 }).show(), this.split ? this.renderSplit(f,
                a) : (e.style.width || b.css({ width: this.chart.spacingBox.width }), b.attr({ text: f && f.join ? f.join("") : f }), b.removeClass(/highcharts-color-[\d]+/g).addClass("highcharts-color-" + r(k.colorIndex, c.colorIndex)), b.attr({ stroke: e.borderColor || k.color || c.color || "#666666" }), this.updatePosition({ plotX: g, plotY: d, negative: k.negative, ttBelow: k.ttBelow, h: n[2] || 0 })), this.isHidden = !1))
            }, renderSplit: function (d, k) {
                var b = this, e = [], g = this.chart, f = g.renderer, n = !0, l = this.options, m = 0, c = this.getLabel(); A(d.slice(0, k.length + 1),
                function (a, d) {
                    if (!1 !== a) {
                        d = k[d - 1] || { isHeader: !0, plotX: k[0].plotX }; var q = d.series || b, v = q.tt, p = d.series || {}, z = "highcharts-color-" + r(d.colorIndex, p.colorIndex, "none"); v || (q.tt = v = f.label(null, null, null, "callout").addClass("highcharts-tooltip-box " + z).attr({ padding: l.padding, r: l.borderRadius, fill: l.backgroundColor, stroke: l.borderColor || d.color || p.color || "#333333", "stroke-width": l.borderWidth }).add(c)); v.isActive = !0; v.attr({ text: a }); v.css(l.style).shadow(l.shadow); a = v.getBBox(); p = a.width + v.strokeWidth();
                        d.isHeader ? (m = a.height, p = Math.max(0, Math.min(d.plotX + g.plotLeft - p / 2, g.chartWidth - p))) : p = d.plotX + g.plotLeft - r(l.distance, 16) - p; 0 > p && (n = !1); a = (d.series && d.series.yAxis && d.series.yAxis.pos) + (d.plotY || 0); a -= g.plotTop; e.push({ target: d.isHeader ? g.plotHeight + m : a, rank: d.isHeader ? 1 : 0, size: q.tt.getBBox().height + 1, point: d, x: p, tt: v })
                    }
                }); this.cleanSplit(); a.distribute(e, g.plotHeight + m); A(e, function (a) {
                    var b = a.point, c = b.series; a.tt.attr({
                        visibility: void 0 === a.pos ? "hidden" : "inherit", x: n || b.isHeader ? a.x : b.plotX +
                        g.plotLeft + r(l.distance, 16), y: a.pos + g.plotTop, anchorX: b.isHeader ? b.plotX + g.plotLeft : b.plotX + c.xAxis.pos, anchorY: b.isHeader ? a.pos + g.plotTop - 15 : b.plotY + c.yAxis.pos
                    })
                })
            }, updatePosition: function (a) { var d = this.chart, b = this.getLabel(), b = (this.options.positioner || this.getPosition).call(this, b.width, b.height, a); this.move(Math.round(b.x), Math.round(b.y || 0), a.plotX + d.plotLeft, a.plotY + d.plotTop) }, getDateFormat: function (a, k, b, e) {
                var d = C("%m-%d %H:%M:%S.%L", k), f, n, l = { millisecond: 15, second: 12, minute: 9, hour: 6, day: 3 },
                m = "millisecond"; for (n in g) { if (a === g.week && +C("%w", k) === b && "00:00:00.000" === d.substr(6)) { n = "week"; break } if (g[n] > a) { n = m; break } if (l[n] && d.substr(l[n]) !== "01-01 00:00:00.000".substr(l[n])) break; "week" !== n && (m = n) } n && (f = e[n]); return f
            }, getXDateFormat: function (a, g, b) { g = g.dateTimeLabelFormats; var e = b && b.closestPointRange; return (e ? this.getDateFormat(e, a.x, b.options.startOfWeek, g) : g.day) || g.year }, tooltipFooterHeaderFormatter: function (a, g) {
                var b = g ? "footer" : "header"; g = a.series; var e = g.tooltipOptions, d = e.xDateFormat,
                k = g.xAxis, n = k && "datetime" === k.options.type && m(a.key), b = e[b + "Format"]; n && !d && (d = this.getXDateFormat(a, e, k)); n && d && (b = b.replace("{point.key}", "{point.key:" + d + "}")); return E(b, { point: a, series: g })
            }, bodyFormatter: function (a) { return f(a, function (a) { var b = a.series.tooltipOptions; return (b.pointFormatter || a.point.tooltipFormatter).call(a.point, b.pointFormat) }) }
        }
    })(M); (function (a) {
        var C = a.addEvent, A = a.attr, F = a.charts, E = a.color, m = a.css, f = a.defined, l = a.each, r = a.extend, u = a.find, t = a.fireEvent, g = a.isObject, d = a.offset,
        k = a.pick, b = a.removeEvent, e = a.splat, v = a.Tooltip, y = a.win; a.Pointer = function (a, b) { this.init(a, b) }; a.Pointer.prototype = {
            init: function (a, b) { this.options = b; this.chart = a; this.runChartClick = b.chart.events && !!b.chart.events.click; this.pinchDown = []; this.lastValidTouch = {}; v && (a.tooltip = new v(a, b.tooltip), this.followTouchMove = k(b.tooltip.followTouchMove, !0)); this.setDOMEvents() }, zoomOption: function (a) {
                var b = this.chart, e = b.options.chart, c = e.zoomType || "", b = b.inverted; /touch/.test(a.type) && (c = k(e.pinchType, c));
                this.zoomX = a = /x/.test(c); this.zoomY = c = /y/.test(c); this.zoomHor = a && !b || c && b; this.zoomVert = c && !b || a && b; this.hasZoom = a || c
            }, normalize: function (a, b) { var e, c; a = a || y.event; a.target || (a.target = a.srcElement); c = a.touches ? a.touches.length ? a.touches.item(0) : a.changedTouches[0] : a; b || (this.chartPosition = b = d(this.chart.container)); void 0 === c.pageX ? (e = Math.max(a.x, a.clientX - b.left), b = a.y) : (e = c.pageX - b.left, b = c.pageY - b.top); return r(a, { chartX: Math.round(e), chartY: Math.round(b) }) }, getCoordinates: function (a) {
                var b =
                { xAxis: [], yAxis: [] }; l(this.chart.axes, function (e) { b[e.isXAxis ? "xAxis" : "yAxis"].push({ axis: e, value: e.toValue(a[e.horiz ? "chartX" : "chartY"]) }) }); return b
            }, findNearestKDPoint: function (a, b, e) {
                var c; l(a, function (a) {
                    var d = !(a.noSharedTooltip && b) && 0 > a.options.findNearestPointBy.indexOf("y"); a = a.searchPoint(e, d); if ((d = g(a, !0)) && !(d = !g(c, !0))) var d = c.distX - a.distX, n = c.dist - a.dist, k = (a.series.group && a.series.group.zIndex) - (c.series.group && c.series.group.zIndex), d = 0 < (0 !== d && b ? d : 0 !== n ? n : 0 !== k ? k : c.series.index >
                    a.series.index ? -1 : 1); d && (c = a)
                }); return c
            }, getPointFromEvent: function (a) { a = a.target; for (var b; a && !b;) b = a.point, a = a.parentNode; return b }, getChartCoordinatesFromPoint: function (a, b) { var e = a.series, c = e.xAxis, e = e.yAxis; if (c && e) return b ? { chartX: c.len + c.pos - a.clientX, chartY: e.len + e.pos - a.plotY } : { chartX: a.clientX + c.pos, chartY: a.plotY + e.pos } }, getHoverData: function (b, e, d, c, f, q) {
                var n, v = []; c = !(!c || !b); var p = e && !e.stickyTracking ? [e] : a.grep(d, function (a) {
                    return a.visible && !(!f && a.directTouch) && k(a.options.enableMouseTracking,
                    !0) && a.stickyTracking
                }); e = (n = c ? b : this.findNearestKDPoint(p, f, q)) && n.series; n && (f && !e.noSharedTooltip ? (p = a.grep(d, function (a) { return a.visible && !(!f && a.directTouch) && k(a.options.enableMouseTracking, !0) && !a.noSharedTooltip }), l(p, function (a) { a = u(a.points, function (a) { return a.x === n.x }); g(a) && !a.isNull && v.push(a) })) : v.push(n)); return { hoverPoint: n, hoverSeries: e, hoverPoints: v }
            }, runPointActions: function (b, e) {
                var d = this.chart, c = d.tooltip, g = c ? c.shared : !1, n = e || d.hoverPoint, f = n && n.series || d.hoverSeries, f = this.getHoverData(n,
                f, d.series, !!e || f && f.directTouch && this.isDirectTouch, g, b), v, n = f.hoverPoint; v = f.hoverPoints; e = (f = f.hoverSeries) && f.tooltipOptions.followPointer; g = g && f && !f.noSharedTooltip; if (n && (n !== d.hoverPoint || c && c.isHidden)) { l(d.hoverPoints || [], function (b) { -1 === a.inArray(b, v) && b.setState() }); l(v || [], function (a) { a.setState("hover") }); if (d.hoverSeries !== f) f.onMouseOver(); d.hoverPoint && d.hoverPoint.firePointEvent("mouseOut"); n.firePointEvent("mouseOver"); d.hoverPoints = v; d.hoverPoint = n; c && c.refresh(g ? v : n, b) } else e &&
                c && !c.isHidden && (n = c.getAnchor([{}], b), c.updatePosition({ plotX: n[0], plotY: n[1] })); this.unDocMouseMove || (this.unDocMouseMove = C(d.container.ownerDocument, "mousemove", function (b) { var c = F[a.hoverChartIndex]; if (c) c.pointer.onDocumentMouseMove(b) })); l(d.axes, function (c) { var e = k(c.crosshair.snap, !0), p = e ? a.find(v, function (a) { return a.series[c.coll] === c }) : void 0; p || !e ? c.drawCrosshair(b, p) : c.hideCrosshair() })
            }, reset: function (a, b) {
                var d = this.chart, c = d.hoverSeries, g = d.hoverPoint, n = d.hoverPoints, f = d.tooltip, k =
                f && f.shared ? n : g; a && k && l(e(k), function (b) { b.series.isCartesian && void 0 === b.plotX && (a = !1) }); if (a) f && k && (f.refresh(k), g && (g.setState(g.state, !0), l(d.axes, function (a) { a.crosshair && a.drawCrosshair(null, g) }))); else { if (g) g.onMouseOut(); n && l(n, function (a) { a.setState() }); if (c) c.onMouseOut(); f && f.hide(b); this.unDocMouseMove && (this.unDocMouseMove = this.unDocMouseMove()); l(d.axes, function (a) { a.hideCrosshair() }); this.hoverX = d.hoverPoints = d.hoverPoint = null }
            }, scaleGroups: function (a, b) {
                var e = this.chart, c; l(e.series,
                function (d) { c = a || d.getPlotBox(); d.xAxis && d.xAxis.zoomEnabled && d.group && (d.group.attr(c), d.markerGroup && (d.markerGroup.attr(c), d.markerGroup.clip(b ? e.clipRect : null)), d.dataLabelsGroup && d.dataLabelsGroup.attr(c)) }); e.clipRect.attr(b || e.clipBox)
            }, dragStart: function (a) { var b = this.chart; b.mouseIsDown = a.type; b.cancelClick = !1; b.mouseDownX = this.mouseDownX = a.chartX; b.mouseDownY = this.mouseDownY = a.chartY }, drag: function (a) {
                var b = this.chart, e = b.options.chart, c = a.chartX, d = a.chartY, g = this.zoomHor, n = this.zoomVert,
                f = b.plotLeft, p = b.plotTop, k = b.plotWidth, v = b.plotHeight, l, h = this.selectionMarker, w = this.mouseDownX, m = this.mouseDownY, r = e.panKey && a[e.panKey + "Key"]; h && h.touch || (c < f ? c = f : c > f + k && (c = f + k), d < p ? d = p : d > p + v && (d = p + v), this.hasDragged = Math.sqrt(Math.pow(w - c, 2) + Math.pow(m - d, 2)), 10 < this.hasDragged && (l = b.isInsidePlot(w - f, m - p), b.hasCartesianSeries && (this.zoomX || this.zoomY) && l && !r && !h && (this.selectionMarker = h = b.renderer.rect(f, p, g ? 1 : k, n ? 1 : v, 0).attr({
                    fill: e.selectionMarkerFill || E("#335cad").setOpacity(.25).get(), "class": "highcharts-selection-marker",
                    zIndex: 7
                }).add()), h && g && (c -= w, h.attr({ width: Math.abs(c), x: (0 < c ? 0 : c) + w })), h && n && (c = d - m, h.attr({ height: Math.abs(c), y: (0 < c ? 0 : c) + m })), l && !h && e.panning && b.pan(a, e.panning)))
            }, drop: function (a) {
                var b = this, e = this.chart, c = this.hasPinched; if (this.selectionMarker) {
                    var d = { originalEvent: a, xAxis: [], yAxis: [] }, g = this.selectionMarker, n = g.attr ? g.attr("x") : g.x, k = g.attr ? g.attr("y") : g.y, p = g.attr ? g.attr("width") : g.width, v = g.attr ? g.attr("height") : g.height, I; if (this.hasDragged || c) l(e.axes, function (e) {
                        if (e.zoomEnabled && f(e.min) &&
                        (c || b[{ xAxis: "zoomX", yAxis: "zoomY" }[e.coll]])) { var h = e.horiz, g = "touchend" === a.type ? e.minPixelPadding : 0, q = e.toValue((h ? n : k) + g), h = e.toValue((h ? n + p : k + v) - g); d[e.coll].push({ axis: e, min: Math.min(q, h), max: Math.max(q, h) }); I = !0 }
                    }), I && t(e, "selection", d, function (a) { e.zoom(r(a, c ? { animation: !1 } : null)) }); this.selectionMarker = this.selectionMarker.destroy(); c && this.scaleGroups()
                } e && (m(e.container, { cursor: e._cursor }), e.cancelClick = 10 < this.hasDragged, e.mouseIsDown = this.hasDragged = this.hasPinched = !1, this.pinchDown =
                [])
            }, onContainerMouseDown: function (a) { a = this.normalize(a); this.zoomOption(a); a.preventDefault && a.preventDefault(); this.dragStart(a) }, onDocumentMouseUp: function (b) { F[a.hoverChartIndex] && F[a.hoverChartIndex].pointer.drop(b) }, onDocumentMouseMove: function (a) { var b = this.chart, e = this.chartPosition; a = this.normalize(a, e); !e || this.inClass(a.target, "highcharts-tracker") || b.isInsidePlot(a.chartX - b.plotLeft, a.chartY - b.plotTop) || this.reset() }, onContainerMouseLeave: function (b) {
                var e = F[a.hoverChartIndex]; e && (b.relatedTarget ||
                b.toElement) && (e.pointer.reset(), e.pointer.chartPosition = null)
            }, onContainerMouseMove: function (b) { var e = this.chart; f(a.hoverChartIndex) && F[a.hoverChartIndex] && F[a.hoverChartIndex].mouseIsDown || (a.hoverChartIndex = e.index); b = this.normalize(b); b.returnValue = !1; "mousedown" === e.mouseIsDown && this.drag(b); !this.inClass(b.target, "highcharts-tracker") && !e.isInsidePlot(b.chartX - e.plotLeft, b.chartY - e.plotTop) || e.openMenu || this.runPointActions(b) }, inClass: function (a, b) {
                for (var e; a;) {
                    if (e = A(a, "class")) {
                        if (-1 !==
                        e.indexOf(b)) return !0; if (-1 !== e.indexOf("highcharts-container")) return !1
                    } a = a.parentNode
                }
            }, onTrackerMouseOut: function (a) { var b = this.chart.hoverSeries; a = a.relatedTarget || a.toElement; this.isDirectTouch = !1; if (!(!b || !a || b.stickyTracking || this.inClass(a, "highcharts-tooltip") || this.inClass(a, "highcharts-series-" + b.index) && this.inClass(a, "highcharts-tracker"))) b.onMouseOut() }, onContainerClick: function (a) {
                var b = this.chart, e = b.hoverPoint, c = b.plotLeft, d = b.plotTop; a = this.normalize(a); b.cancelClick || (e && this.inClass(a.target,
                "highcharts-tracker") ? (t(e.series, "click", r(a, { point: e })), b.hoverPoint && e.firePointEvent("click", a)) : (r(a, this.getCoordinates(a)), b.isInsidePlot(a.chartX - c, a.chartY - d) && t(b, "click", a)))
            }, setDOMEvents: function () {
                var b = this, e = b.chart.container, d = e.ownerDocument; e.onmousedown = function (a) { b.onContainerMouseDown(a) }; e.onmousemove = function (a) { b.onContainerMouseMove(a) }; e.onclick = function (a) { b.onContainerClick(a) }; C(e, "mouseleave", b.onContainerMouseLeave); 1 === a.chartCount && C(d, "mouseup", b.onDocumentMouseUp);
                a.hasTouch && (e.ontouchstart = function (a) { b.onContainerTouchStart(a) }, e.ontouchmove = function (a) { b.onContainerTouchMove(a) }, 1 === a.chartCount && C(d, "touchend", b.onDocumentTouchEnd))
            }, destroy: function () {
                var e = this, d = this.chart.container.ownerDocument; e.unDocMouseMove && e.unDocMouseMove(); b(e.chart.container, "mouseleave", e.onContainerMouseLeave); a.chartCount || (b(d, "mouseup", e.onDocumentMouseUp), a.hasTouch && b(d, "touchend", e.onDocumentTouchEnd)); clearInterval(e.tooltipTimeout); a.objectEach(e, function (a, b) {
                    e[b] =
                    null
                })
            }
        }
    })(M); (function (a) {
        var C = a.charts, A = a.each, F = a.extend, E = a.map, m = a.noop, f = a.pick; F(a.Pointer.prototype, {
            pinchTranslate: function (a, f, m, t, g, d) { this.zoomHor && this.pinchTranslateDirection(!0, a, f, m, t, g, d); this.zoomVert && this.pinchTranslateDirection(!1, a, f, m, t, g, d) }, pinchTranslateDirection: function (a, f, m, t, g, d, k, b) {
                var e = this.chart, v = a ? "x" : "y", l = a ? "X" : "Y", n = "chart" + l, r = a ? "width" : "height", u = e["plot" + (a ? "Left" : "Top")], c, G, q = b || 1, B = e.inverted, K = e.bounds[a ? "h" : "v"], p = 1 === f.length, z = f[0][n], I = m[0][n],
                L = !p && f[1][n], h = !p && m[1][n], w; m = function () { !p && 20 < Math.abs(z - L) && (q = b || Math.abs(I - h) / Math.abs(z - L)); G = (u - I) / q + z; c = e["plot" + (a ? "Width" : "Height")] / q }; m(); f = G; f < K.min ? (f = K.min, w = !0) : f + c > K.max && (f = K.max - c, w = !0); w ? (I -= .8 * (I - k[v][0]), p || (h -= .8 * (h - k[v][1])), m()) : k[v] = [I, h]; B || (d[v] = G - u, d[r] = c); d = B ? 1 / q : q; g[r] = c; g[v] = f; t[B ? a ? "scaleY" : "scaleX" : "scale" + l] = q; t["translate" + l] = d * u + (I - d * z)
            }, pinch: function (a) {
                var l = this, u = l.chart, t = l.pinchDown, g = a.touches, d = g.length, k = l.lastValidTouch, b = l.hasZoom, e = l.selectionMarker,
                v = {}, y = 1 === d && (l.inClass(a.target, "highcharts-tracker") && u.runTrackerClick || l.runChartClick), n = {}; 1 < d && (l.initiated = !0); b && l.initiated && !y && a.preventDefault(); E(g, function (a) { return l.normalize(a) }); "touchstart" === a.type ? (A(g, function (a, b) { t[b] = { chartX: a.chartX, chartY: a.chartY } }), k.x = [t[0].chartX, t[1] && t[1].chartX], k.y = [t[0].chartY, t[1] && t[1].chartY], A(u.axes, function (a) {
                    if (a.zoomEnabled) {
                        var b = u.bounds[a.horiz ? "h" : "v"], e = a.minPixelPadding, d = a.toPixels(f(a.options.min, a.dataMin)), g = a.toPixels(f(a.options.max,
                        a.dataMax)), k = Math.max(d, g); b.min = Math.min(a.pos, Math.min(d, g) - e); b.max = Math.max(a.pos + a.len, k + e)
                    }
                }), l.res = !0) : l.followTouchMove && 1 === d ? this.runPointActions(l.normalize(a)) : t.length && (e || (l.selectionMarker = e = F({ destroy: m, touch: !0 }, u.plotBox)), l.pinchTranslate(t, g, v, e, n, k), l.hasPinched = b, l.scaleGroups(v, n), l.res && (l.res = !1, this.reset(!1, 0)))
            }, touch: function (l, m) {
                var r = this.chart, t, g; if (r.index !== a.hoverChartIndex) this.onContainerMouseLeave({ relatedTarget: !0 }); a.hoverChartIndex = r.index; 1 === l.touches.length ?
                (l = this.normalize(l), (g = r.isInsidePlot(l.chartX - r.plotLeft, l.chartY - r.plotTop)) && !r.openMenu ? (m && this.runPointActions(l), "touchmove" === l.type && (m = this.pinchDown, t = m[0] ? 4 <= Math.sqrt(Math.pow(m[0].chartX - l.chartX, 2) + Math.pow(m[0].chartY - l.chartY, 2)) : !1), f(t, !0) && this.pinch(l)) : m && this.reset()) : 2 === l.touches.length && this.pinch(l)
            }, onContainerTouchStart: function (a) { this.zoomOption(a); this.touch(a, !0) }, onContainerTouchMove: function (a) { this.touch(a) }, onDocumentTouchEnd: function (f) {
                C[a.hoverChartIndex] &&
                C[a.hoverChartIndex].pointer.drop(f)
            }
        })
    })(M); (function (a) {
        var C = a.addEvent, A = a.charts, F = a.css, E = a.doc, m = a.extend, f = a.noop, l = a.Pointer, r = a.removeEvent, u = a.win, t = a.wrap; if (!a.hasTouch && (u.PointerEvent || u.MSPointerEvent)) {
            var g = {}, d = !!u.PointerEvent, k = function () { var b = []; b.item = function (a) { return this[a] }; a.objectEach(g, function (a) { b.push({ pageX: a.pageX, pageY: a.pageY, target: a.target }) }); return b }, b = function (b, d, g, n) {
                "touch" !== b.pointerType && b.pointerType !== b.MSPOINTER_TYPE_TOUCH || !A[a.hoverChartIndex] ||
                (n(b), n = A[a.hoverChartIndex].pointer, n[d]({ type: g, target: b.currentTarget, preventDefault: f, touches: k() }))
            }; m(l.prototype, {
                onContainerPointerDown: function (a) { b(a, "onContainerTouchStart", "touchstart", function (a) { g[a.pointerId] = { pageX: a.pageX, pageY: a.pageY, target: a.currentTarget } }) }, onContainerPointerMove: function (a) { b(a, "onContainerTouchMove", "touchmove", function (a) { g[a.pointerId] = { pageX: a.pageX, pageY: a.pageY }; g[a.pointerId].target || (g[a.pointerId].target = a.currentTarget) }) }, onDocumentPointerUp: function (a) {
                    b(a,
                    "onDocumentTouchEnd", "touchend", function (a) { delete g[a.pointerId] })
                }, batchMSEvents: function (a) { a(this.chart.container, d ? "pointerdown" : "MSPointerDown", this.onContainerPointerDown); a(this.chart.container, d ? "pointermove" : "MSPointerMove", this.onContainerPointerMove); a(E, d ? "pointerup" : "MSPointerUp", this.onDocumentPointerUp) }
            }); t(l.prototype, "init", function (a, b, d) { a.call(this, b, d); this.hasZoom && F(b.container, { "-ms-touch-action": "none", "touch-action": "none" }) }); t(l.prototype, "setDOMEvents", function (a) {
                a.apply(this);
                (this.hasZoom || this.followTouchMove) && this.batchMSEvents(C)
            }); t(l.prototype, "destroy", function (a) { this.batchMSEvents(r); a.call(this) })
        }
    })(M); (function (a) {
        var C = a.addEvent, A = a.css, F = a.discardElement, E = a.defined, m = a.each, f = a.isFirefox, l = a.marginNames, r = a.merge, u = a.pick, t = a.setAnimation, g = a.stableSort, d = a.win, k = a.wrap; a.Legend = function (a, e) { this.init(a, e) }; a.Legend.prototype = {
            init: function (a, e) { this.chart = a; this.setOptions(e); e.enabled && (this.render(), C(this.chart, "endResize", function () { this.legend.positionCheckboxes() })) },
            setOptions: function (a) { var b = u(a.padding, 8); this.options = a; this.itemStyle = a.itemStyle; this.itemHiddenStyle = r(this.itemStyle, a.itemHiddenStyle); this.itemMarginTop = a.itemMarginTop || 0; this.padding = b; this.initialItemY = b - 5; this.itemHeight = this.maxItemWidth = 0; this.symbolWidth = u(a.symbolWidth, 16); this.pages = [] }, update: function (a, e) { var b = this.chart; this.setOptions(r(!0, this.options, a)); this.destroy(); b.isDirtyLegend = b.isDirtyBox = !0; u(e, !0) && b.redraw() }, colorizeItem: function (a, e) {
                a.legendGroup[e ? "removeClass" :
                "addClass"]("highcharts-legend-item-hidden"); var b = this.options, d = a.legendItem, g = a.legendLine, f = a.legendSymbol, k = this.itemHiddenStyle.color, b = e ? b.itemStyle.color : k, c = e ? a.color || k : k, l = a.options && a.options.marker, q = { fill: c }; d && d.css({ fill: b, color: b }); g && g.attr({ stroke: c }); f && (l && f.isMarker && (q = a.pointAttribs(), e || (q.stroke = q.fill = k)), f.attr(q))
            }, positionItem: function (a) {
                var b = this.options, d = b.symbolPadding, b = !b.rtl, g = a._legendItemPos, f = g[0], g = g[1], k = a.checkbox; (a = a.legendGroup) && a.element && a.translate(b ?
                    f : this.legendWidth - f - 2 * d - 4, g); k && (k.x = f, k.y = g)
            }, destroyItem: function (a) { var b = a.checkbox; m(["legendItem", "legendLine", "legendSymbol", "legendGroup"], function (b) { a[b] && (a[b] = a[b].destroy()) }); b && F(a.checkbox) }, destroy: function () { function a(a) { this[a] && (this[a] = this[a].destroy()) } m(this.getAllItems(), function (b) { m(["legendItem", "legendGroup"], a, b) }); m("clipRect up down pager nav box title group".split(" "), a, this); this.display = null }, positionCheckboxes: function (a) {
                var b = this.group && this.group.alignAttr,
                d, g = this.clipHeight || this.legendHeight, f = this.titleHeight; b && (d = b.translateY, m(this.allItems, function (e) { var k = e.checkbox, c; k && (c = d + f + k.y + (a || 0) + 3, A(k, { left: b.translateX + e.checkboxOffset + k.x - 20 + "px", top: c + "px", display: c > d - 6 && c < d + g - 6 ? "" : "none" })) }))
            }, renderTitle: function () {
                var a = this.options, e = this.padding, d = a.title, g = 0; d.text && (this.title || (this.title = this.chart.renderer.label(d.text, e - 3, e - 4, null, null, null, a.useHTML, null, "legend-title").attr({ zIndex: 1 }).css(d.style).add(this.group)), a = this.title.getBBox(),
                g = a.height, this.offsetWidth = a.width, this.contentGroup.attr({ translateY: g })); this.titleHeight = g
            }, setText: function (b) { var e = this.options; b.legendItem.attr({ text: e.labelFormat ? a.format(e.labelFormat, b) : e.labelFormatter.call(b) }) }, renderItem: function (a) {
                var b = this.chart, d = b.renderer, g = this.options, f = "horizontal" === g.layout, k = this.symbolWidth, l = g.symbolPadding, c = this.itemStyle, m = this.itemHiddenStyle, q = this.padding, B = f ? u(g.itemDistance, 20) : 0, t = !g.rtl, p = g.width, z = g.itemMarginBottom || 0, I = this.itemMarginTop,
                L = a.legendItem, h = !a.series, w = !h && a.series.drawLegendSymbol ? a.series : a, P = w.options, H = this.createCheckboxForItem && P && P.showCheckbox, P = k + l + B + (H ? 20 : 0), O = g.useHTML, A = a.options.className; L || (a.legendGroup = d.g("legend-item").addClass("highcharts-" + w.type + "-series highcharts-color-" + a.colorIndex + (A ? " " + A : "") + (h ? " highcharts-series-" + a.index : "")).attr({ zIndex: 1 }).add(this.scrollGroup), a.legendItem = L = d.text("", t ? k + l : -l, this.baseline || 0, O).css(r(a.visible ? c : m)).attr({ align: t ? "left" : "right", zIndex: 2 }).add(a.legendGroup),
                this.baseline || (k = c.fontSize, this.fontMetrics = d.fontMetrics(k, L), this.baseline = this.fontMetrics.f + 3 + I, L.attr("y", this.baseline)), this.symbolHeight = g.symbolHeight || this.fontMetrics.f, w.drawLegendSymbol(this, a), this.setItemEvents && this.setItemEvents(a, L, O), H && this.createCheckboxForItem(a)); this.colorizeItem(a, a.visible); c.width || L.css({ width: (g.itemWidth || g.width || b.spacingBox.width) - P }); this.setText(a); d = L.getBBox(); c = a.checkboxOffset = g.itemWidth || a.legendItemWidth || d.width + P; this.itemHeight = d = Math.round(a.legendItemHeight ||
                d.height || this.symbolHeight); f && this.itemX - q + c > (p || b.spacingBox.width - 2 * q - g.x) && (this.itemX = q, this.itemY += I + this.lastLineHeight + z, this.lastLineHeight = 0); this.maxItemWidth = Math.max(this.maxItemWidth, c); this.lastItemY = I + this.itemY + z; this.lastLineHeight = Math.max(d, this.lastLineHeight); a._legendItemPos = [this.itemX, this.itemY]; f ? this.itemX += c : (this.itemY += I + d + z, this.lastLineHeight = d); this.offsetWidth = p || Math.max((f ? this.itemX - q - (a.checkbox ? 0 : B) : c) + q, this.offsetWidth)
            }, getAllItems: function () {
                var a = []; m(this.chart.series,
                function (b) { var e = b && b.options; b && u(e.showInLegend, E(e.linkedTo) ? !1 : void 0, !0) && (a = a.concat(b.legendItems || ("point" === e.legendType ? b.data : b))) }); return a
            }, adjustMargins: function (a, e) {
                var b = this.chart, d = this.options, g = d.align.charAt(0) + d.verticalAlign.charAt(0) + d.layout.charAt(0); d.floating || m([/(lth|ct|rth)/, /(rtv|rm|rbv)/, /(rbh|cb|lbh)/, /(lbv|lm|ltv)/], function (f, k) {
                    f.test(g) && !E(a[k]) && (b[l[k]] = Math.max(b[l[k]], b.legend[(k + 1) % 2 ? "legendHeight" : "legendWidth"] + [1, -1, -1, 1][k] * d[k % 2 ? "x" : "y"] + u(d.margin,
                    12) + e[k]))
                })
            }, render: function () {
                var a = this, e = a.chart, d = e.renderer, f = a.group, k, l, t, c, u = a.box, q = a.options, B = a.padding; a.itemX = B; a.itemY = a.initialItemY; a.offsetWidth = 0; a.lastItemY = 0; f || (a.group = f = d.g("legend").attr({ zIndex: 7 }).add(), a.contentGroup = d.g().attr({ zIndex: 1 }).add(f), a.scrollGroup = d.g().add(a.contentGroup)); a.renderTitle(); k = a.getAllItems(); g(k, function (a, b) { return (a.options && a.options.legendIndex || 0) - (b.options && b.options.legendIndex || 0) }); q.reversed && k.reverse(); a.allItems = k; a.display = l =
                !!k.length; a.lastLineHeight = 0; m(k, function (b) { a.renderItem(b) }); t = (q.width || a.offsetWidth) + B; c = a.lastItemY + a.lastLineHeight + a.titleHeight; c = a.handleOverflow(c); c += B; u || (a.box = u = d.rect().addClass("highcharts-legend-box").attr({ r: q.borderRadius }).add(f), u.isNew = !0); u.attr({ stroke: q.borderColor, "stroke-width": q.borderWidth || 0, fill: q.backgroundColor || "none" }).shadow(q.shadow); 0 < t && 0 < c && (u[u.isNew ? "attr" : "animate"](u.crisp({ x: 0, y: 0, width: t, height: c }, u.strokeWidth())), u.isNew = !1); u[l ? "show" : "hide"](); a.legendWidth =
                t; a.legendHeight = c; m(k, function (b) { a.positionItem(b) }); l && f.align(r(q, { width: t, height: c }), !0, "spacingBox"); e.isResizing || this.positionCheckboxes()
            }, handleOverflow: function (a) {
                var b = this, d = this.chart, g = d.renderer, f = this.options, k = f.y, l = this.padding, d = d.spacingBox.height + ("top" === f.verticalAlign ? -k : k) - l, k = f.maxHeight, c, r = this.clipRect, q = f.navigation, B = u(q.animation, !0), t = q.arrowSize || 12, p = this.nav, z = this.pages, I, L = this.allItems, h = function (a) {
                    "number" === typeof a ? r.attr({ height: a }) : r && (b.clipRect = r.destroy(),
                    b.contentGroup.clip()); b.contentGroup.div && (b.contentGroup.div.style.clip = a ? "rect(" + l + "px,9999px," + (l + a) + "px,0)" : "auto")
                }; "horizontal" !== f.layout || "middle" === f.verticalAlign || f.floating || (d /= 2); k && (d = Math.min(d, k)); z.length = 0; a > d && !1 !== q.enabled ? (this.clipHeight = c = Math.max(d - 20 - this.titleHeight - l, 0), this.currentPage = u(this.currentPage, 1), this.fullHeight = a, m(L, function (a, b) {
                    var e = a._legendItemPos[1]; a = Math.round(a.legendItem.getBBox().height); var d = z.length; if (!d || e - z[d - 1] > c && (I || e) !== z[d - 1]) z.push(I ||
                    e), d++; b === L.length - 1 && e + a - z[d - 1] > c && z.push(e); e !== I && (I = e)
                }), r || (r = b.clipRect = g.clipRect(0, l, 9999, 0), b.contentGroup.clip(r)), h(c), p || (this.nav = p = g.g().attr({ zIndex: 1 }).add(this.group), this.up = g.symbol("triangle", 0, 0, t, t).on("click", function () { b.scroll(-1, B) }).add(p), this.pager = g.text("", 15, 10).addClass("highcharts-legend-navigation").css(q.style).add(p), this.down = g.symbol("triangle-down", 0, 0, t, t).on("click", function () { b.scroll(1, B) }).add(p)), b.scroll(0), a = d) : p && (h(), this.nav = p.destroy(), this.scrollGroup.attr({ translateY: 1 }),
                this.clipHeight = 0); return a
            }, scroll: function (a, e) {
                var b = this.pages, d = b.length; a = this.currentPage + a; var g = this.clipHeight, f = this.options.navigation, k = this.pager, c = this.padding; a > d && (a = d); 0 < a && (void 0 !== e && t(e, this.chart), this.nav.attr({ translateX: c, translateY: g + this.padding + 7 + this.titleHeight, visibility: "visible" }), this.up.attr({ "class": 1 === a ? "highcharts-legend-nav-inactive" : "highcharts-legend-nav-active" }), k.attr({ text: a + "/" + d }), this.down.attr({
                    x: 18 + this.pager.getBBox().width, "class": a === d ? "highcharts-legend-nav-inactive" :
                    "highcharts-legend-nav-active"
                }), this.up.attr({ fill: 1 === a ? f.inactiveColor : f.activeColor }).css({ cursor: 1 === a ? "default" : "pointer" }), this.down.attr({ fill: a === d ? f.inactiveColor : f.activeColor }).css({ cursor: a === d ? "default" : "pointer" }), e = -b[a - 1] + this.initialItemY, this.scrollGroup.animate({ translateY: e }), this.currentPage = a, this.positionCheckboxes(e))
            }
        }; a.LegendSymbolMixin = {
            drawRectangle: function (a, e) {
                var b = a.symbolHeight, d = a.options.squareSymbol; e.legendSymbol = this.chart.renderer.rect(d ? (a.symbolWidth - b) /
                2 : 0, a.baseline - b + 1, d ? b : a.symbolWidth, b, u(a.options.symbolRadius, b / 2)).addClass("highcharts-point").attr({ zIndex: 3 }).add(e.legendGroup)
            }, drawLineMarker: function (a) {
                var b = this.options, d = b.marker, g = a.symbolWidth, f = a.symbolHeight, k = f / 2, l = this.chart.renderer, c = this.legendGroup; a = a.baseline - Math.round(.3 * a.fontMetrics.b); var m; m = { "stroke-width": b.lineWidth || 0 }; b.dashStyle && (m.dashstyle = b.dashStyle); this.legendLine = l.path(["M", 0, a, "L", g, a]).addClass("highcharts-graph").attr(m).add(c); d && !1 !== d.enabled &&
                (b = Math.min(u(d.radius, k), k), 0 === this.symbol.indexOf("url") && (d = r(d, { width: f, height: f }), b = 0), this.legendSymbol = d = l.symbol(this.symbol, g / 2 - b, a - b, 2 * b, 2 * b, d).addClass("highcharts-point").add(c), d.isMarker = !0)
            }
        }; (/Trident\/7\.0/.test(d.navigator.userAgent) || f) && k(a.Legend.prototype, "positionItem", function (a, e) { var b = this, d = function () { e._legendItemPos && a.call(b, e) }; d(); setTimeout(d) })
    })(M); (function (a) {
        var C = a.addEvent, A = a.animate, F = a.animObject, E = a.attr, m = a.doc, f = a.Axis, l = a.createElement, r = a.defaultOptions,
        u = a.discardElement, t = a.charts, g = a.css, d = a.defined, k = a.each, b = a.extend, e = a.find, v = a.fireEvent, y = a.getStyle, n = a.grep, D = a.isNumber, J = a.isObject, c = a.isString, G = a.Legend, q = a.marginNames, B = a.merge, K = a.objectEach, p = a.Pointer, z = a.pick, I = a.pInt, L = a.removeEvent, h = a.seriesTypes, w = a.splat, P = a.svg, H = a.syncTimeout, O = a.win, Q = a.Renderer, R = a.Chart = function () { this.getArgs.apply(this, arguments) }; a.chart = function (a, b, c) { return new R(a, b, c) }; b(R.prototype, {
            callbacks: [], getArgs: function () {
                var a = [].slice.call(arguments);
                if (c(a[0]) || a[0].nodeName) this.renderTo = a.shift(); this.init(a[0], a[1])
            }, init: function (b, c) {
                var e, d, h = b.series, p = b.plotOptions || {}; b.series = null; e = B(r, b); for (d in e.plotOptions) e.plotOptions[d].tooltip = p[d] && B(p[d].tooltip) || void 0; e.tooltip.userOptions = b.chart && b.chart.forExport && b.tooltip.userOptions || b.tooltip; e.series = b.series = h; this.userOptions = b; b = e.chart; d = b.events; this.margin = []; this.spacing = []; this.bounds = { h: {}, v: {} }; this.callback = c; this.isResizing = 0; this.options = e; this.axes = []; this.series =
                []; this.hasCartesianSeries = b.showAxes; var g = this; g.index = t.length; t.push(g); a.chartCount++; d && K(d, function (a, b) { C(g, b, a) }); g.xAxis = []; g.yAxis = []; g.pointCount = g.colorCounter = g.symbolCounter = 0; g.firstRender()
            }, initSeries: function (b) { var c = this.options.chart; (c = h[b.type || c.type || c.defaultSeriesType]) || a.error(17, !0); c = new c; c.init(this, b); return c }, orderSeries: function (a) { var b = this.series; for (a = a || 0; a < b.length; a++) b[a] && (b[a].index = a, b[a].name = b[a].name || "Series " + (b[a].index + 1)) }, isInsidePlot: function (a,
            b, c) { var e = c ? b : a; a = c ? a : b; return 0 <= e && e <= this.plotWidth && 0 <= a && a <= this.plotHeight }, redraw: function (c) {
                var e = this.axes, d = this.series, h = this.pointer, p = this.legend, g = this.isDirtyLegend, f, q, l = this.hasCartesianSeries, n = this.isDirtyBox, z, m = this.renderer, x = m.isHidden(), w = []; this.setResponsive && this.setResponsive(!1); a.setAnimation(c, this); x && this.temporaryDisplay(); this.layOutTitles(); for (c = d.length; c--;) if (z = d[c], z.options.stacking && (f = !0, z.isDirty)) { q = !0; break } if (q) for (c = d.length; c--;) z = d[c], z.options.stacking &&
                (z.isDirty = !0); k(d, function (a) { a.isDirty && "point" === a.options.legendType && (a.updateTotals && a.updateTotals(), g = !0); a.isDirtyData && v(a, "updatedData") }); g && p.options.enabled && (p.render(), this.isDirtyLegend = !1); f && this.getStacks(); l && k(e, function (a) { a.updateNames(); a.setScale() }); this.getMargins(); l && (k(e, function (a) { a.isDirty && (n = !0) }), k(e, function (a) {
                    var c = a.min + "," + a.max; a.extKey !== c && (a.extKey = c, w.push(function () { v(a, "afterSetExtremes", b(a.eventArgs, a.getExtremes())); delete a.eventArgs })); (n || f) &&
                    a.redraw()
                })); n && this.drawChartBox(); v(this, "predraw"); k(d, function (a) { (n || a.isDirty) && a.visible && a.redraw(); a.isDirtyData = !1 }); h && h.reset(!0); m.draw(); v(this, "redraw"); v(this, "render"); x && this.temporaryDisplay(!0); k(w, function (a) { a.call() })
            }, get: function (a) { function b(b) { return b.id === a || b.options && b.options.id === a } var c, d = this.series, h; c = e(this.axes, b) || e(this.series, b); for (h = 0; !c && h < d.length; h++) c = e(d[h].points || [], b); return c }, getAxes: function () {
                var a = this, b = this.options, c = b.xAxis = w(b.xAxis ||
                {}), b = b.yAxis = w(b.yAxis || {}); k(c, function (a, b) { a.index = b; a.isX = !0 }); k(b, function (a, b) { a.index = b }); c = c.concat(b); k(c, function (b) { new f(a, b) })
            }, getSelectedPoints: function () { var a = []; k(this.series, function (b) { a = a.concat(n(b.data || [], function (a) { return a.selected })) }); return a }, getSelectedSeries: function () { return n(this.series, function (a) { return a.selected }) }, setTitle: function (a, b, c) {
                var e = this, d = e.options, h; h = d.title = B({ style: { color: "#333333", fontSize: d.isStock ? "16px" : "18px" } }, d.title, a); d = d.subtitle =
                B({ style: { color: "#666666" } }, d.subtitle, b); k([["title", a, h], ["subtitle", b, d]], function (a, b) { var c = a[0], d = e[c], h = a[1]; a = a[2]; d && h && (e[c] = d = d.destroy()); a && a.text && !d && (e[c] = e.renderer.text(a.text, 0, 0, a.useHTML).attr({ align: a.align, "class": "highcharts-" + c, zIndex: a.zIndex || 4 }).add(), e[c].update = function (a) { e.setTitle(!b && a, b && a) }, e[c].css(a.style)) }); e.layOutTitles(c)
            }, layOutTitles: function (a) {
                var c = 0, e, d = this.renderer, h = this.spacingBox; k(["title", "subtitle"], function (a) {
                    var e = this[a], p = this.options[a];
                    a = "title" === a ? -3 : p.verticalAlign ? 0 : c + 2; var g; e && (g = p.style.fontSize, g = d.fontMetrics(g, e).b, e.css({ width: (p.width || h.width + p.widthAdjust) + "px" }).align(b({ y: a + g }, p), !1, "spacingBox"), p.floating || p.verticalAlign || (c = Math.ceil(c + e.getBBox(p.useHTML).height)))
                }, this); e = this.titleOffset !== c; this.titleOffset = c; !this.isDirtyBox && e && (this.isDirtyBox = e, this.hasRendered && z(a, !0) && this.isDirtyBox && this.redraw())
            }, getChartSize: function () {
                var b = this.options.chart, c = b.width, b = b.height, e = this.renderTo; d(c) || (this.containerWidth =
                y(e, "width")); d(b) || (this.containerHeight = y(e, "height")); this.chartWidth = Math.max(0, c || this.containerWidth || 600); this.chartHeight = Math.max(0, a.relativeLength(b, this.chartWidth) || this.containerHeight || 400)
            }, temporaryDisplay: function (b) {
                var c = this.renderTo; if (b) for (; c && c.style;) c.hcOrigStyle && (a.css(c, c.hcOrigStyle), delete c.hcOrigStyle), c.hcOrigDetached && (m.body.removeChild(c), c.hcOrigDetached = !1), c = c.parentNode; else for (; c && c.style;) {
                    m.body.contains(c) || (c.hcOrigDetached = !0, m.body.appendChild(c));
                    if ("none" === y(c, "display", !1) || c.hcOricDetached) c.hcOrigStyle = { display: c.style.display, height: c.style.height, overflow: c.style.overflow }, b = { display: "block", overflow: "hidden" }, c !== this.renderTo && (b.height = 0), a.css(c, b), c.offsetWidth || c.style.setProperty("display", "block", "important"); c = c.parentNode; if (c === m.body) break
                }
            }, setClassName: function (a) { this.container.className = "highcharts-container " + (a || "") }, getContainer: function () {
                var e, d = this.options, h = d.chart, p, g; e = this.renderTo; var f = a.uniqueKey(), k; e ||
                (this.renderTo = e = h.renderTo); c(e) && (this.renderTo = e = m.getElementById(e)); e || a.error(13, !0); p = I(E(e, "data-highcharts-chart")); D(p) && t[p] && t[p].hasRendered && t[p].destroy(); E(e, "data-highcharts-chart", this.index); e.innerHTML = ""; h.skipClone || e.offsetWidth || this.temporaryDisplay(); this.getChartSize(); p = this.chartWidth; g = this.chartHeight; k = b({ position: "relative", overflow: "hidden", width: p + "px", height: g + "px", textAlign: "left", lineHeight: "normal", zIndex: 0, "-webkit-tap-highlight-color": "rgba(0,0,0,0)" }, h.style);
                this.container = e = l("div", { id: f }, k, e); this._cursor = e.style.cursor; this.renderer = new (a[h.renderer] || Q)(e, p, g, null, h.forExport, d.exporting && d.exporting.allowHTML); this.setClassName(h.className); this.renderer.setStyle(h.style); this.renderer.chartIndex = this.index
            }, getMargins: function (a) {
                var b = this.spacing, c = this.margin, e = this.titleOffset; this.resetMargins(); e && !d(c[0]) && (this.plotTop = Math.max(this.plotTop, e + this.options.title.margin + b[0])); this.legend.display && this.legend.adjustMargins(c, b); this.extraMargin &&
                (this[this.extraMargin.type] = (this[this.extraMargin.type] || 0) + this.extraMargin.value); this.extraTopMargin && (this.plotTop += this.extraTopMargin); a || this.getAxisMargins()
            }, getAxisMargins: function () { var a = this, b = a.axisOffset = [0, 0, 0, 0], c = a.margin; a.hasCartesianSeries && k(a.axes, function (a) { a.visible && a.getOffset() }); k(q, function (e, h) { d(c[h]) || (a[e] += b[h]) }); a.setChartSize() }, reflow: function (a) {
                var b = this, c = b.options.chart, e = b.renderTo, h = d(c.width) && d(c.height), p = c.width || y(e, "width"), c = c.height || y(e, "height"),
                e = a ? a.target : O; if (!h && !b.isPrinting && p && c && (e === O || e === m)) { if (p !== b.containerWidth || c !== b.containerHeight) clearTimeout(b.reflowTimeout), b.reflowTimeout = H(function () { b.container && b.setSize(void 0, void 0, !1) }, a ? 100 : 0); b.containerWidth = p; b.containerHeight = c }
            }, initReflow: function () { var a = this, b; b = C(O, "resize", function (b) { a.reflow(b) }); C(a, "destroy", b) }, setSize: function (b, c, e) {
                var d = this, h = d.renderer; d.isResizing += 1; a.setAnimation(e, d); d.oldChartHeight = d.chartHeight; d.oldChartWidth = d.chartWidth; void 0 !==
                b && (d.options.chart.width = b); void 0 !== c && (d.options.chart.height = c); d.getChartSize(); b = h.globalAnimation; (b ? A : g)(d.container, { width: d.chartWidth + "px", height: d.chartHeight + "px" }, b); d.setChartSize(!0); h.setSize(d.chartWidth, d.chartHeight, e); k(d.axes, function (a) { a.isDirty = !0; a.setScale() }); d.isDirtyLegend = !0; d.isDirtyBox = !0; d.layOutTitles(); d.getMargins(); d.redraw(e); d.oldChartHeight = null; v(d, "resize"); H(function () { d && v(d, "endResize", null, function () { --d.isResizing }) }, F(b).duration)
            }, setChartSize: function (a) {
                function b(a) {
                    a =
                    f[a] || 0; return Math.max(m || a, a) / 2
                } var c = this.inverted, e = this.renderer, d = this.chartWidth, h = this.chartHeight, p = this.options.chart, g = this.spacing, f = this.clipOffset, q, n, l, z, m; this.plotLeft = q = Math.round(this.plotLeft); this.plotTop = n = Math.round(this.plotTop); this.plotWidth = l = Math.max(0, Math.round(d - q - this.marginRight)); this.plotHeight = z = Math.max(0, Math.round(h - n - this.marginBottom)); this.plotSizeX = c ? z : l; this.plotSizeY = c ? l : z; this.plotBorderWidth = p.plotBorderWidth || 0; this.spacingBox = e.spacingBox = {
                    x: g[3], y: g[0],
                    width: d - g[3] - g[1], height: h - g[0] - g[2]
                }; this.plotBox = e.plotBox = { x: q, y: n, width: l, height: z }; m = 2 * Math.floor(this.plotBorderWidth / 2); c = Math.ceil(b(3)); e = Math.ceil(b(0)); this.clipBox = { x: c, y: e, width: Math.floor(this.plotSizeX - b(1) - c), height: Math.max(0, Math.floor(this.plotSizeY - b(2) - e)) }; a || k(this.axes, function (a) { a.setAxisSize(); a.setAxisTranslation() })
            }, resetMargins: function () {
                var a = this, b = a.options.chart; k(["margin", "spacing"], function (c) {
                    var e = b[c], d = J(e) ? e : [e, e, e, e]; k(["Top", "Right", "Bottom", "Left"], function (e,
                    h) { a[c][h] = z(b[c + e], d[h]) })
                }); k(q, function (b, c) { a[b] = z(a.margin[c], a.spacing[c]) }); a.axisOffset = [0, 0, 0, 0]; a.clipOffset = []
            }, drawChartBox: function () {
                var a = this.options.chart, b = this.renderer, c = this.chartWidth, e = this.chartHeight, d = this.chartBackground, h = this.plotBackground, p = this.plotBorder, g, f = this.plotBGImage, k = a.backgroundColor, q = a.plotBackgroundColor, l = a.plotBackgroundImage, n, z = this.plotLeft, m = this.plotTop, w = this.plotWidth, I = this.plotHeight, v = this.plotBox, r = this.clipRect, B = this.clipBox, y = "animate";
                d || (this.chartBackground = d = b.rect().addClass("highcharts-background").add(), y = "attr"); g = a.borderWidth || 0; n = g + (a.shadow ? 8 : 0); k = { fill: k || "none" }; if (g || d["stroke-width"]) k.stroke = a.borderColor, k["stroke-width"] = g; d.attr(k).shadow(a.shadow); d[y]({ x: n / 2, y: n / 2, width: c - n - g % 2, height: e - n - g % 2, r: a.borderRadius }); y = "animate"; h || (y = "attr", this.plotBackground = h = b.rect().addClass("highcharts-plot-background").add()); h[y](v); h.attr({ fill: q || "none" }).shadow(a.plotShadow); l && (f ? f.animate(v) : this.plotBGImage = b.image(l,
                z, m, w, I).add()); r ? r.animate({ width: B.width, height: B.height }) : this.clipRect = b.clipRect(B); y = "animate"; p || (y = "attr", this.plotBorder = p = b.rect().addClass("highcharts-plot-border").attr({ zIndex: 1 }).add()); p.attr({ stroke: a.plotBorderColor, "stroke-width": a.plotBorderWidth || 0, fill: "none" }); p[y](p.crisp({ x: z, y: m, width: w, height: I }, -p.strokeWidth())); this.isDirtyBox = !1
            }, propFromSeries: function () {
                var a = this, b = a.options.chart, c, e = a.options.series, d, p; k(["inverted", "angular", "polar"], function (g) {
                    c = h[b.type || b.defaultSeriesType];
                    p = b[g] || c && c.prototype[g]; for (d = e && e.length; !p && d--;) (c = h[e[d].type]) && c.prototype[g] && (p = !0); a[g] = p
                })
            }, linkSeries: function () { var a = this, b = a.series; k(b, function (a) { a.linkedSeries.length = 0 }); k(b, function (b) { var e = b.options.linkedTo; c(e) && (e = ":previous" === e ? a.series[b.index - 1] : a.get(e)) && e.linkedParent !== b && (e.linkedSeries.push(b), b.linkedParent = e, b.visible = z(b.options.visible, e.options.visible, b.visible)) }) }, renderSeries: function () { k(this.series, function (a) { a.translate(); a.render() }) }, renderLabels: function () {
                var a =
                this, c = a.options.labels; c.items && k(c.items, function (e) { var d = b(c.style, e.style), h = I(d.left) + a.plotLeft, p = I(d.top) + a.plotTop + 12; delete d.left; delete d.top; a.renderer.text(e.html, h, p).attr({ zIndex: 2 }).css(d).add() })
            }, render: function () {
                var a = this.axes, b = this.renderer, c = this.options, e, d, h; this.setTitle(); this.legend = new G(this, c.legend); this.getStacks && this.getStacks(); this.getMargins(!0); this.setChartSize(); c = this.plotWidth; e = this.plotHeight -= 21; k(a, function (a) { a.setScale() }); this.getAxisMargins(); d =
                1.1 < c / this.plotWidth; h = 1.05 < e / this.plotHeight; if (d || h) k(a, function (a) { (a.horiz && d || !a.horiz && h) && a.setTickInterval(!0) }), this.getMargins(); this.drawChartBox(); this.hasCartesianSeries && k(a, function (a) { a.visible && a.render() }); this.seriesGroup || (this.seriesGroup = b.g("series-group").attr({ zIndex: 3 }).add()); this.renderSeries(); this.renderLabels(); this.addCredits(); this.setResponsive && this.setResponsive(); this.hasRendered = !0
            }, addCredits: function (a) {
                var b = this; a = B(!0, this.options.credits, a); a.enabled && !this.credits &&
                (this.credits = this.renderer.text(a.text + (this.mapCredits || ""), 0, 0).addClass("highcharts-credits").on("click", function () { a.href && (O.location.href = a.href) }).attr({ align: a.position.align, zIndex: 8 }).css(a.style).add().align(a.position), this.credits.update = function (a) { b.credits = b.credits.destroy(); b.addCredits(a) })
            }, destroy: function () {
                var b = this, c = b.axes, e = b.series, d = b.container, h, p = d && d.parentNode; v(b, "destroy"); b.renderer.forExport ? a.erase(t, b) : t[b.index] = void 0; a.chartCount--; b.renderTo.removeAttribute("data-highcharts-chart");
                L(b); for (h = c.length; h--;) c[h] = c[h].destroy(); this.scroller && this.scroller.destroy && this.scroller.destroy(); for (h = e.length; h--;) e[h] = e[h].destroy(); k("title subtitle chartBackground plotBackground plotBGImage plotBorder seriesGroup clipRect credits pointer rangeSelector legend resetZoomButton tooltip renderer".split(" "), function (a) { var c = b[a]; c && c.destroy && (b[a] = c.destroy()) }); d && (d.innerHTML = "", L(d), p && u(d)); K(b, function (a, c) { delete b[c] })
            }, isReadyToRender: function () {
                var a = this; return P || O != O.top ||
                "complete" === m.readyState ? !0 : (m.attachEvent("onreadystatechange", function () { m.detachEvent("onreadystatechange", a.firstRender); "complete" === m.readyState && a.firstRender() }), !1)
            }, firstRender: function () {
                var a = this, b = a.options; if (a.isReadyToRender()) {
                    a.getContainer(); v(a, "init"); a.resetMargins(); a.setChartSize(); a.propFromSeries(); a.getAxes(); k(b.series || [], function (b) { a.initSeries(b) }); a.linkSeries(); v(a, "beforeRender"); p && (a.pointer = new p(a, b)); a.render(); if (!a.renderer.imgCount && a.onload) a.onload();
                    a.temporaryDisplay(!0)
                }
            }, onload: function () { k([this.callback].concat(this.callbacks), function (a) { a && void 0 !== this.index && a.apply(this, [this]) }, this); v(this, "load"); v(this, "render"); d(this.index) && !1 !== this.options.chart.reflow && this.initReflow(); this.onload = null }
        })
    })(M); (function (a) {
        var C, A = a.each, F = a.extend, E = a.erase, m = a.fireEvent, f = a.format, l = a.isArray, r = a.isNumber, u = a.pick, t = a.removeEvent; a.Point = C = function () { }; a.Point.prototype = {
            init: function (a, d, f) {
                this.series = a; this.color = a.color; this.applyOptions(d,
                f); a.options.colorByPoint ? (d = a.options.colors || a.chart.options.colors, this.color = this.color || d[a.colorCounter], d = d.length, f = a.colorCounter, a.colorCounter++, a.colorCounter === d && (a.colorCounter = 0)) : f = a.colorIndex; this.colorIndex = u(this.colorIndex, f); a.chart.pointCount++; return this
            }, applyOptions: function (a, d) {
                var g = this.series, b = g.options.pointValKey || g.pointValKey; a = C.prototype.optionsToObject.call(this, a); F(this, a); this.options = this.options ? F(this.options, a) : a; a.group && delete this.group; b && (this.y =
                this[b]); this.isNull = u(this.isValid && !this.isValid(), null === this.x || !r(this.y, !0)); this.selected && (this.state = "select"); "name" in this && void 0 === d && g.xAxis && g.xAxis.hasNames && (this.x = g.xAxis.nameToX(this)); void 0 === this.x && g && (this.x = void 0 === d ? g.autoIncrement(this) : d); return this
            }, optionsToObject: function (a) {
                var d = {}, g = this.series, b = g.options.keys, e = b || g.pointArrayMap || ["y"], f = e.length, m = 0, n = 0; if (r(a) || null === a) d[e[0]] = a; else if (l(a)) for (!b && a.length > f && (g = typeof a[0], "string" === g ? d.name = a[0] : "number" ===
                g && (d.x = a[0]), m++) ; n < f;) b && void 0 === a[m] || (d[e[n]] = a[m]), m++, n++; else "object" === typeof a && (d = a, a.dataLabels && (g._hasPointLabels = !0), a.marker && (g._hasPointMarkers = !0)); return d
            }, getClassName: function () {
                return "highcharts-point" + (this.selected ? " highcharts-point-select" : "") + (this.negative ? " highcharts-negative" : "") + (this.isNull ? " highcharts-null-point" : "") + (void 0 !== this.colorIndex ? " highcharts-color-" + this.colorIndex : "") + (this.options.className ? " " + this.options.className : "") + (this.zone && this.zone.className ?
                " " + this.zone.className.replace("highcharts-negative", "") : "")
            }, getZone: function () { var a = this.series, d = a.zones, a = a.zoneAxis || "y", f = 0, b; for (b = d[f]; this[a] >= b.value;) b = d[++f]; b && b.color && !this.options.color && (this.color = b.color); return b }, destroy: function () {
                var a = this.series.chart, d = a.hoverPoints, f; a.pointCount--; d && (this.setState(), E(d, this), d.length || (a.hoverPoints = null)); if (this === a.hoverPoint) this.onMouseOut(); if (this.graphic || this.dataLabel) t(this), this.destroyElements(); this.legendItem && a.legend.destroyItem(this);
                for (f in this) this[f] = null
            }, destroyElements: function () { for (var a = ["graphic", "dataLabel", "dataLabelUpper", "connector", "shadowGroup"], d, f = 6; f--;) d = a[f], this[d] && (this[d] = this[d].destroy()) }, getLabelConfig: function () { return { x: this.category, y: this.y, color: this.color, colorIndex: this.colorIndex, key: this.name || this.category, series: this.series, point: this, percentage: this.percentage, total: this.total || this.stackTotal } }, tooltipFormatter: function (a) {
                var d = this.series, g = d.tooltipOptions, b = u(g.valueDecimals, ""),
                e = g.valuePrefix || "", l = g.valueSuffix || ""; A(d.pointArrayMap || ["y"], function (d) { d = "{point." + d; if (e || l) a = a.replace(d + "}", e + d + "}" + l); a = a.replace(d + "}", d + ":,." + b + "f}") }); return f(a, { point: this, series: this.series })
            }, firePointEvent: function (a, d, f) { var b = this, e = this.series.options; (e.point.events[a] || b.options && b.options.events && b.options.events[a]) && this.importEvents(); "click" === a && e.allowPointSelect && (f = function (a) { b.select && b.select(null, a.ctrlKey || a.metaKey || a.shiftKey) }); m(this, a, d, f) }, visible: !0
        }
    })(M);
    (function (a) {
        var C = a.addEvent, A = a.animObject, F = a.arrayMax, E = a.arrayMin, m = a.correctFloat, f = a.Date, l = a.defaultOptions, r = a.defaultPlotOptions, u = a.defined, t = a.each, g = a.erase, d = a.extend, k = a.fireEvent, b = a.grep, e = a.isArray, v = a.isNumber, y = a.isString, n = a.merge, D = a.objectEach, J = a.pick, c = a.removeEvent, G = a.splat, q = a.SVGElement, B = a.syncTimeout, K = a.win; a.Series = a.seriesType("line", null, {
            lineWidth: 2, allowPointSelect: !1, showCheckbox: !1, animation: { duration: 1E3 }, events: {}, marker: {
                lineWidth: 0, lineColor: "#ffffff", radius: 4,
                states: { hover: { animation: { duration: 50 }, enabled: !0, radiusPlus: 2, lineWidthPlus: 1 }, select: { fillColor: "#cccccc", lineColor: "#000000", lineWidth: 2 } }
            }, point: { events: {} }, dataLabels: { align: "center", formatter: function () { return null === this.y ? "" : a.numberFormat(this.y, -1) }, style: { fontSize: "11px", fontWeight: "bold", color: "contrast", textOutline: "1px contrast" }, verticalAlign: "bottom", x: 0, y: 0, padding: 5 }, cropThreshold: 300, pointRange: 0, softThreshold: !0, states: {
                hover: {
                    animation: { duration: 50 }, lineWidthPlus: 1, marker: {},
                    halo: { size: 10, opacity: .25 }
                }, select: { marker: {} }
            }, stickyTracking: !0, turboThreshold: 1E3, findNearestPointBy: "x"
        }, {
            isCartesian: !0, pointClass: a.Point, sorted: !0, requireSorting: !0, directTouch: !1, axisTypes: ["xAxis", "yAxis"], colorCounter: 0, parallelArrays: ["x", "y"], coll: "series", init: function (a, b) {
                var c = this, e, h = a.series, p; c.chart = a; c.options = b = c.setOptions(b); c.linkedSeries = []; c.bindAxes(); d(c, { name: b.name, state: "", visible: !1 !== b.visible, selected: !0 === b.selected }); e = b.events; D(e, function (a, b) { C(c, b, a) }); if (e &&
                e.click || b.point && b.point.events && b.point.events.click || b.allowPointSelect) a.runTrackerClick = !0; c.getColor(); c.getSymbol(); t(c.parallelArrays, function (a) { c[a + "Data"] = [] }); c.setData(b.data, !1); c.isCartesian && (a.hasCartesianSeries = !0); h.length && (p = h[h.length - 1]); c._i = J(p && p._i, -1) + 1; a.orderSeries(this.insert(h))
            }, insert: function (a) {
                var b = this.options.index, c; if (v(b)) { for (c = a.length; c--;) if (b >= J(a[c].options.index, a[c]._i)) { a.splice(c + 1, 0, this); break } -1 === c && a.unshift(this); c += 1 } else a.push(this); return J(c,
                a.length - 1)
            }, bindAxes: function () { var b = this, c = b.options, e = b.chart, d; t(b.axisTypes || [], function (h) { t(e[h], function (a) { d = a.options; if (c[h] === d.index || void 0 !== c[h] && c[h] === d.id || void 0 === c[h] && 0 === d.index) b.insert(a.series), b[h] = a, a.isDirty = !0 }); b[h] || b.optionalAxis === h || a.error(18, !0) }) }, updateParallelArrays: function (a, b) {
                var c = a.series, e = arguments, d = v(b) ? function (e) { var d = "y" === e && c.toYData ? c.toYData(a) : a[e]; c[e + "Data"][b] = d } : function (a) {
                    Array.prototype[b].apply(c[a + "Data"], Array.prototype.slice.call(e,
                    2))
                }; t(c.parallelArrays, d)
            }, autoIncrement: function () { var a = this.options, b = this.xIncrement, c, e = a.pointIntervalUnit, b = J(b, a.pointStart, 0); this.pointInterval = c = J(this.pointInterval, a.pointInterval, 1); e && (a = new f(b), "day" === e ? a = +a[f.hcSetDate](a[f.hcGetDate]() + c) : "month" === e ? a = +a[f.hcSetMonth](a[f.hcGetMonth]() + c) : "year" === e && (a = +a[f.hcSetFullYear](a[f.hcGetFullYear]() + c)), c = a - b); this.xIncrement = b + c; return b }, setOptions: function (a) {
                var b = this.chart, c = b.options, e = c.plotOptions, d = (b.userOptions || {}).plotOptions ||
                {}, p = e[this.type]; this.userOptions = a; b = n(p, e.series, a); this.tooltipOptions = n(l.tooltip, l.plotOptions.series && l.plotOptions.series.tooltip, l.plotOptions[this.type].tooltip, c.tooltip.userOptions, e.series && e.series.tooltip, e[this.type].tooltip, a.tooltip); this.stickyTracking = J(a.stickyTracking, d[this.type] && d[this.type].stickyTracking, d.series && d.series.stickyTracking, this.tooltipOptions.shared && !this.noSharedTooltip ? !0 : b.stickyTracking); null === p.marker && delete b.marker; this.zoneAxis = b.zoneAxis; a = this.zones =
                (b.zones || []).slice(); !b.negativeColor && !b.negativeFillColor || b.zones || a.push({ value: b[this.zoneAxis + "Threshold"] || b.threshold || 0, className: "highcharts-negative", color: b.negativeColor, fillColor: b.negativeFillColor }); a.length && u(a[a.length - 1].value) && a.push({ color: this.color, fillColor: this.fillColor }); return b
            }, getCyclic: function (a, b, c) {
                var e, d = this.chart, p = this.userOptions, f = a + "Index", g = a + "Counter", k = c ? c.length : J(d.options.chart[a + "Count"], d[a + "Count"]); b || (e = J(p[f], p["_" + f]), u(e) || (d.series.length ||
                (d[g] = 0), p["_" + f] = e = d[g] % k, d[g] += 1), c && (b = c[e])); void 0 !== e && (this[f] = e); this[a] = b
            }, getColor: function () { this.options.colorByPoint ? this.options.color = null : this.getCyclic("color", this.options.color || r[this.type].color, this.chart.options.colors) }, getSymbol: function () { this.getCyclic("symbol", this.options.marker.symbol, this.chart.options.symbols) }, drawLegendSymbol: a.LegendSymbolMixin.drawLineMarker, setData: function (b, c, d, f) {
                var h = this, p = h.points, g = p && p.length || 0, k, q = h.options, l = h.chart, n = null, m = h.xAxis,
                z = q.turboThreshold, r = this.xData, B = this.yData, I = (k = h.pointArrayMap) && k.length; b = b || []; k = b.length; c = J(c, !0); if (!1 !== f && k && g === k && !h.cropped && !h.hasGroupedData && h.visible) t(b, function (a, b) { p[b].update && a !== q.data[b] && p[b].update(a, !1, null, !1) }); else {
                    h.xIncrement = null; h.colorCounter = 0; t(this.parallelArrays, function (a) { h[a + "Data"].length = 0 }); if (z && k > z) {
                        for (d = 0; null === n && d < k;) n = b[d], d++; if (v(n)) for (d = 0; d < k; d++) r[d] = this.autoIncrement(), B[d] = b[d]; else if (e(n)) if (I) for (d = 0; d < k; d++) n = b[d], r[d] = n[0], B[d] = n.slice(1,
                        I + 1); else for (d = 0; d < k; d++) n = b[d], r[d] = n[0], B[d] = n[1]; else a.error(12)
                    } else for (d = 0; d < k; d++) void 0 !== b[d] && (n = { series: h }, h.pointClass.prototype.applyOptions.apply(n, [b[d]]), h.updateParallelArrays(n, d)); y(B[0]) && a.error(14, !0); h.data = []; h.options.data = h.userOptions.data = b; for (d = g; d--;) p[d] && p[d].destroy && p[d].destroy(); m && (m.minRange = m.userMinRange); h.isDirty = l.isDirtyBox = !0; h.isDirtyData = !!p; d = !1
                } "point" === q.legendType && (this.processData(), this.generatePoints()); c && l.redraw(d)
            }, processData: function (b) {
                var c =
                this.xData, e = this.yData, d = c.length, h; h = 0; var p, f, g = this.xAxis, k, q = this.options; k = q.cropThreshold; var n = this.getExtremesFromAll || q.getExtremesFromAll, l = this.isCartesian, q = g && g.val2lin, m = g && g.isLog, v, r; if (l && !this.isDirty && !g.isDirty && !this.yAxis.isDirty && !b) return !1; g && (b = g.getExtremes(), v = b.min, r = b.max); if (l && this.sorted && !n && (!k || d > k || this.forceCrop)) if (c[d - 1] < v || c[0] > r) c = [], e = []; else if (c[0] < v || c[d - 1] > r) h = this.cropData(this.xData, this.yData, v, r), c = h.xData, e = h.yData, h = h.start, p = !0; for (k = c.length ||
                1; --k;) d = m ? q(c[k]) - q(c[k - 1]) : c[k] - c[k - 1], 0 < d && (void 0 === f || d < f) ? f = d : 0 > d && this.requireSorting && a.error(15); this.cropped = p; this.cropStart = h; this.processedXData = c; this.processedYData = e; this.closestPointRange = f
            }, cropData: function (a, b, c, e) { var d = a.length, p = 0, g = d, f = J(this.cropShoulder, 1), k; for (k = 0; k < d; k++) if (a[k] >= c) { p = Math.max(0, k - f); break } for (c = k; c < d; c++) if (a[c] > e) { g = c + f; break } return { xData: a.slice(p, g), yData: b.slice(p, g), start: p, end: g } }, generatePoints: function () {
                var a = this.options, b = a.data, c = this.data,
                e, d = this.processedXData, g = this.processedYData, f = this.pointClass, k = d.length, q = this.cropStart || 0, n, l = this.hasGroupedData, a = a.keys, m, v = [], r; c || l || (c = [], c.length = b.length, c = this.data = c); a && l && (this.options.keys = !1); for (r = 0; r < k; r++) n = q + r, l ? (m = (new f).init(this, [d[r]].concat(G(g[r]))), m.dataGroup = this.groupMap[r]) : (m = c[n]) || void 0 === b[n] || (c[n] = m = (new f).init(this, b[n], d[r])), m && (m.index = n, v[r] = m); this.options.keys = a; if (c && (k !== (e = c.length) || l)) for (r = 0; r < e; r++) r !== q || l || (r += k), c[r] && (c[r].destroyElements(),
                c[r].plotX = void 0); this.data = c; this.points = v
            }, getExtremes: function (a) {
                var b = this.yAxis, c = this.processedXData, d, h = [], p = 0; d = this.xAxis.getExtremes(); var g = d.min, f = d.max, k, q, n, l; a = a || this.stackedYData || this.processedYData || []; d = a.length; for (l = 0; l < d; l++) if (q = c[l], n = a[l], k = (v(n, !0) || e(n)) && (!b.positiveValuesOnly || n.length || 0 < n), q = this.getExtremesFromAll || this.options.getExtremesFromAll || this.cropped || (c[l] || q) >= g && (c[l] || q) <= f, k && q) if (k = n.length) for (; k--;) null !== n[k] && (h[p++] = n[k]); else h[p++] = n; this.dataMin =
                E(h); this.dataMax = F(h)
            }, translate: function () {
                this.processedXData || this.processData(); this.generatePoints(); var a = this.options, b = a.stacking, c = this.xAxis, e = c.categories, d = this.yAxis, g = this.points, f = g.length, k = !!this.modifyValue, q = a.pointPlacement, n = "between" === q || v(q), l = a.threshold, r = a.startFromThreshold ? l : 0, B, y, t, G, D = Number.MAX_VALUE; "between" === q && (q = .5); v(q) && (q *= J(a.pointRange || c.pointRange)); for (a = 0; a < f; a++) {
                    var K = g[a], A = K.x, C = K.y; y = K.low; var E = b && d.stacks[(this.negStacks && C < (r ? 0 : l) ? "-" : "") + this.stackKey],
                    F; d.positiveValuesOnly && null !== C && 0 >= C && (K.isNull = !0); K.plotX = B = m(Math.min(Math.max(-1E5, c.translate(A, 0, 0, 0, 1, q, "flags" === this.type)), 1E5)); b && this.visible && !K.isNull && E && E[A] && (G = this.getStackIndicator(G, A, this.index), F = E[A], C = F.points[G.key], y = C[0], C = C[1], y === r && G.key === E[A].base && (y = J(l, d.min)), d.positiveValuesOnly && 0 >= y && (y = null), K.total = K.stackTotal = F.total, K.percentage = F.total && K.y / F.total * 100, K.stackY = C, F.setOffset(this.pointXOffset || 0, this.barW || 0)); K.yBottom = u(y) ? d.translate(y, 0, 1, 0, 1) :
                    null; k && (C = this.modifyValue(C, K)); K.plotY = y = "number" === typeof C && Infinity !== C ? Math.min(Math.max(-1E5, d.translate(C, 0, 1, 0, 1)), 1E5) : void 0; K.isInside = void 0 !== y && 0 <= y && y <= d.len && 0 <= B && B <= c.len; K.clientX = n ? m(c.translate(A, 0, 0, 0, 1, q)) : B; K.negative = K.y < (l || 0); K.category = e && void 0 !== e[K.x] ? e[K.x] : K.x; K.isNull || (void 0 !== t && (D = Math.min(D, Math.abs(B - t))), t = B); K.zone = this.zones.length && K.getZone()
                } this.closestPointRangePx = D
            }, getValidPoints: function (a, c) {
                var e = this.chart; return b(a || this.points || [], function (a) {
                    return c &&
                    !e.isInsidePlot(a.plotX, a.plotY, e.inverted) ? !1 : !a.isNull
                })
            }, setClip: function (a) {
                var b = this.chart, c = this.options, e = b.renderer, d = b.inverted, p = this.clipBox, g = p || b.clipBox, f = this.sharedClipKey || ["_sharedClip", a && a.duration, a && a.easing, g.height, c.xAxis, c.yAxis].join(), k = b[f], q = b[f + "m"]; k || (a && (g.width = 0, b[f + "m"] = q = e.clipRect(-99, d ? -b.plotLeft : -b.plotTop, 99, d ? b.chartWidth : b.chartHeight)), b[f] = k = e.clipRect(g), k.count = { length: 0 }); a && !k.count[this.index] && (k.count[this.index] = !0, k.count.length += 1); !1 !== c.clip &&
                (this.group.clip(a || p ? k : b.clipRect), this.markerGroup.clip(q), this.sharedClipKey = f); a || (k.count[this.index] && (delete k.count[this.index], --k.count.length), 0 === k.count.length && f && b[f] && (p || (b[f] = b[f].destroy()), b[f + "m"] && (b[f + "m"] = b[f + "m"].destroy())))
            }, animate: function (a) { var b = this.chart, c = A(this.options.animation), e; a ? this.setClip(c) : (e = this.sharedClipKey, (a = b[e]) && a.animate({ width: b.plotSizeX }, c), b[e + "m"] && b[e + "m"].animate({ width: b.plotSizeX + 99 }, c), this.animate = null) }, afterAnimate: function () {
                this.setClip();
                k(this, "afterAnimate"); this.finishedAnimating = !0
            }, drawPoints: function () {
                var a = this.points, b = this.chart, c, e, d, f, g = this.options.marker, k, q, n, l, m = this[this.specialGroup] || this.markerGroup, r = J(g.enabled, this.xAxis.isRadial ? !0 : null, this.closestPointRangePx >= 2 * g.radius); if (!1 !== g.enabled || this._hasPointMarkers) for (e = 0; e < a.length; e++) d = a[e], c = d.plotY, f = d.graphic, k = d.marker || {}, q = !!d.marker, n = r && void 0 === k.enabled || k.enabled, l = d.isInside, n && v(c) && null !== d.y ? (c = J(k.symbol, this.symbol), d.hasImage = 0 === c.indexOf("url"),
                n = this.markerAttribs(d, d.selected && "select"), f ? f[l ? "show" : "hide"](!0).animate(n) : l && (0 < n.width || d.hasImage) && (d.graphic = f = b.renderer.symbol(c, n.x, n.y, n.width, n.height, q ? k : g).add(m)), f && f.attr(this.pointAttribs(d, d.selected && "select")), f && f.addClass(d.getClassName(), !0)) : f && (d.graphic = f.destroy())
            }, markerAttribs: function (a, b) {
                var c = this.options.marker, e = a.marker || {}, d = J(e.radius, c.radius); b && (c = c.states[b], b = e.states && e.states[b], d = J(b && b.radius, c && c.radius, d + (c && c.radiusPlus || 0))); a.hasImage && (d =
                0); a = { x: Math.floor(a.plotX) - d, y: a.plotY - d }; d && (a.width = a.height = 2 * d); return a
            }, pointAttribs: function (a, b) {
                var c = this.options.marker, e = a && a.options, d = e && e.marker || {}, f = this.color, g = e && e.color, p = a && a.color, e = J(d.lineWidth, c.lineWidth); a = a && a.zone && a.zone.color; f = g || a || p || f; a = d.fillColor || c.fillColor || f; f = d.lineColor || c.lineColor || f; b && (c = c.states[b], b = d.states && d.states[b] || {}, e = J(b.lineWidth, c.lineWidth, e + J(b.lineWidthPlus, c.lineWidthPlus, 0)), a = b.fillColor || c.fillColor || a, f = b.lineColor || c.lineColor ||
                f); return { stroke: f, "stroke-width": e, fill: a }
            }, destroy: function () {
                var a = this, b = a.chart, e = /AppleWebKit\/533/.test(K.navigator.userAgent), d, h, f = a.data || [], n, l; k(a, "destroy"); c(a); t(a.axisTypes || [], function (b) { (l = a[b]) && l.series && (g(l.series, a), l.isDirty = l.forceRedraw = !0) }); a.legendItem && a.chart.legend.destroyItem(a); for (h = f.length; h--;) (n = f[h]) && n.destroy && n.destroy(); a.points = null; clearTimeout(a.animationTimeout); D(a, function (a, b) { a instanceof q && !a.survive && (d = e && "group" === b ? "hide" : "destroy", a[d]()) });
                b.hoverSeries === a && (b.hoverSeries = null); g(b.series, a); b.orderSeries(); D(a, function (b, c) { delete a[c] })
            }, getGraphPath: function (a, b, c) {
                var e = this, d = e.options, f = d.step, g, p = [], k = [], q; a = a || e.points; (g = a.reversed) && a.reverse(); (f = { right: 1, center: 2 }[f] || f && 3) && g && (f = 4 - f); !d.connectNulls || b || c || (a = this.getValidPoints(a)); t(a, function (h, g) {
                    var n = h.plotX, l = h.plotY, m = a[g - 1]; (h.leftCliff || m && m.rightCliff) && !c && (q = !0); h.isNull && !u(b) && 0 < g ? q = !d.connectNulls : h.isNull && !b ? q = !0 : (0 === g || q ? g = ["M", h.plotX, h.plotY] : e.getPointSpline ?
                    g = e.getPointSpline(a, h, g) : f ? (g = 1 === f ? ["L", m.plotX, l] : 2 === f ? ["L", (m.plotX + n) / 2, m.plotY, "L", (m.plotX + n) / 2, l] : ["L", n, m.plotY], g.push("L", n, l)) : g = ["L", n, l], k.push(h.x), f && k.push(h.x), p.push.apply(p, g), q = !1)
                }); p.xMap = k; return e.graphPath = p
            }, drawGraph: function () {
                var a = this, b = this.options, c = (this.gappedPath || this.getGraphPath).call(this), e = [["graph", "highcharts-graph", b.lineColor || this.color, b.dashStyle]]; t(this.zones, function (c, d) {
                    e.push(["zone-graph-" + d, "highcharts-graph highcharts-zone-graph-" + d + " " +
                    (c.className || ""), c.color || a.color, c.dashStyle || b.dashStyle])
                }); t(e, function (e, d) { var h = e[0], f = a[h]; f ? (f.endX = c.xMap, f.animate({ d: c })) : c.length && (a[h] = a.chart.renderer.path(c).addClass(e[1]).attr({ zIndex: 1 }).add(a.group), f = { stroke: e[2], "stroke-width": b.lineWidth, fill: a.fillGraph && a.color || "none" }, e[3] ? f.dashstyle = e[3] : "square" !== b.linecap && (f["stroke-linecap"] = f["stroke-linejoin"] = "round"), f = a[h].attr(f).shadow(2 > d && b.shadow)); f && (f.startX = c.xMap, f.isArea = c.isArea) })
            }, applyZones: function () {
                var a =
                this, b = this.chart, c = b.renderer, e = this.zones, d, f, g = this.clips || [], k, q = this.graph, n = this.area, l = Math.max(b.chartWidth, b.chartHeight), m = this[(this.zoneAxis || "y") + "Axis"], r, v, B = b.inverted, y, u, G, D, K = !1; e.length && (q || n) && m && void 0 !== m.min && (v = m.reversed, y = m.horiz, q && q.hide(), n && n.hide(), r = m.getExtremes(), t(e, function (e, h) {
                    d = v ? y ? b.plotWidth : 0 : y ? 0 : m.toPixels(r.min); d = Math.min(Math.max(J(f, d), 0), l); f = Math.min(Math.max(Math.round(m.toPixels(J(e.value, r.max), !0)), 0), l); K && (d = f = m.toPixels(r.max)); u = Math.abs(d -
                    f); G = Math.min(d, f); D = Math.max(d, f); m.isXAxis ? (k = { x: B ? D : G, y: 0, width: u, height: l }, y || (k.x = b.plotHeight - k.x)) : (k = { x: 0, y: B ? D : G, width: l, height: u }, y && (k.y = b.plotWidth - k.y)); B && c.isVML && (k = m.isXAxis ? { x: 0, y: v ? G : D, height: k.width, width: b.chartWidth } : { x: k.y - b.plotLeft - b.spacingBox.x, y: 0, width: k.height, height: b.chartHeight }); g[h] ? g[h].animate(k) : (g[h] = c.clipRect(k), q && a["zone-graph-" + h].clip(g[h]), n && a["zone-area-" + h].clip(g[h])); K = e.value > r.max
                }), this.clips = g)
            }, invertGroups: function (a) {
                function b() {
                    t(["group",
                    "markerGroup"], function (b) { c[b] && (e.renderer.isVML && c[b].attr({ width: c.yAxis.len, height: c.xAxis.len }), c[b].width = c.yAxis.len, c[b].height = c.xAxis.len, c[b].invert(a)) })
                } var c = this, e = c.chart, d; c.xAxis && (d = C(e, "resize", b), C(c, "destroy", d), b(a), c.invertGroups = b)
            }, plotGroup: function (a, b, c, e, d) {
                var h = this[a], f = !h; f && (this[a] = h = this.chart.renderer.g().attr({ zIndex: e || .1 }).add(d)); h.addClass("highcharts-" + b + " highcharts-series-" + this.index + " highcharts-" + this.type + "-series highcharts-color-" + this.colorIndex +
                " " + (this.options.className || ""), !0); h.attr({ visibility: c })[f ? "attr" : "animate"](this.getPlotBox()); return h
            }, getPlotBox: function () { var a = this.chart, b = this.xAxis, c = this.yAxis; a.inverted && (b = c, c = this.xAxis); return { translateX: b ? b.left : a.plotLeft, translateY: c ? c.top : a.plotTop, scaleX: 1, scaleY: 1 } }, render: function () {
                var a = this, b = a.chart, c, e = a.options, d = !!a.animate && b.renderer.isSVG && A(e.animation).duration, f = a.visible ? "inherit" : "hidden", g = e.zIndex, k = a.hasRendered, q = b.seriesGroup, n = b.inverted; c = a.plotGroup("group",
                "series", f, g, q); a.markerGroup = a.plotGroup("markerGroup", "markers", f, g, q); d && a.animate(!0); c.inverted = a.isCartesian ? n : !1; a.drawGraph && (a.drawGraph(), a.applyZones()); a.drawDataLabels && a.drawDataLabels(); a.visible && a.drawPoints(); a.drawTracker && !1 !== a.options.enableMouseTracking && a.drawTracker(); a.invertGroups(n); !1 === e.clip || a.sharedClipKey || k || c.clip(b.clipRect); d && a.animate(); k || (a.animationTimeout = B(function () { a.afterAnimate() }, d)); a.isDirty = !1; a.hasRendered = !0
            }, redraw: function () {
                var a = this.chart,
                b = this.isDirty || this.isDirtyData, c = this.group, e = this.xAxis, d = this.yAxis; c && (a.inverted && c.attr({ width: a.plotWidth, height: a.plotHeight }), c.animate({ translateX: J(e && e.left, a.plotLeft), translateY: J(d && d.top, a.plotTop) })); this.translate(); this.render(); b && delete this.kdTree
            }, kdAxisArray: ["clientX", "plotY"], searchPoint: function (a, b) { var c = this.xAxis, e = this.yAxis, d = this.chart.inverted; return this.searchKDTree({ clientX: d ? c.len - a.chartY + c.pos : a.chartX - c.pos, plotY: d ? e.len - a.chartX + e.pos : a.chartY - e.pos }, b) },
            buildKDTree: function () { function a(c, e, d) { var h, f; if (f = c && c.length) return h = b.kdAxisArray[e % d], c.sort(function (a, b) { return a[h] - b[h] }), f = Math.floor(f / 2), { point: c[f], left: a(c.slice(0, f), e + 1, d), right: a(c.slice(f + 1), e + 1, d) } } this.buildingKdTree = !0; var b = this, c = -1 < b.options.findNearestPointBy.indexOf("y") ? 2 : 1; delete b.kdTree; B(function () { b.kdTree = a(b.getValidPoints(null, !b.directTouch), c, c); b.buildingKdTree = !1 }, b.options.kdNow ? 0 : 1) }, searchKDTree: function (a, b) {
                function c(a, b, h, k) {
                    var p = b.point, q = e.kdAxisArray[h %
                    k], n, l, m = p; l = u(a[d]) && u(p[d]) ? Math.pow(a[d] - p[d], 2) : null; n = u(a[f]) && u(p[f]) ? Math.pow(a[f] - p[f], 2) : null; n = (l || 0) + (n || 0); p.dist = u(n) ? Math.sqrt(n) : Number.MAX_VALUE; p.distX = u(l) ? Math.sqrt(l) : Number.MAX_VALUE; q = a[q] - p[q]; n = 0 > q ? "left" : "right"; l = 0 > q ? "right" : "left"; b[n] && (n = c(a, b[n], h + 1, k), m = n[g] < m[g] ? n : p); b[l] && Math.sqrt(q * q) < m[g] && (a = c(a, b[l], h + 1, k), m = a[g] < m[g] ? a : m); return m
                } var e = this, d = this.kdAxisArray[0], f = this.kdAxisArray[1], g = b ? "distX" : "dist"; b = -1 < e.options.findNearestPointBy.indexOf("y") ? 2 : 1; this.kdTree ||
                this.buildingKdTree || this.buildKDTree(); if (this.kdTree) return c(a, this.kdTree, b, b)
            }
        })
    })(M); (function (a) {
        var C = a.Axis, A = a.Chart, F = a.correctFloat, E = a.defined, m = a.destroyObjectProperties, f = a.each, l = a.format, r = a.objectEach, u = a.pick, t = a.Series; a.StackItem = function (a, d, f, b, e) {
            var g = a.chart.inverted; this.axis = a; this.isNegative = f; this.options = d; this.x = b; this.total = null; this.points = {}; this.stack = e; this.rightCliff = this.leftCliff = 0; this.alignOptions = {
                align: d.align || (g ? f ? "left" : "right" : "center"), verticalAlign: d.verticalAlign ||
                (g ? "middle" : f ? "bottom" : "top"), y: u(d.y, g ? 4 : f ? 14 : -6), x: u(d.x, g ? f ? -6 : 6 : 0)
            }; this.textAlign = d.textAlign || (g ? f ? "right" : "left" : "center")
        }; a.StackItem.prototype = {
            destroy: function () { m(this, this.axis) }, render: function (a) { var d = this.options, f = d.format, f = f ? l(f, this) : d.formatter.call(this); this.label ? this.label.attr({ text: f, visibility: "hidden" }) : this.label = this.axis.chart.renderer.text(f, null, null, d.useHTML).css(d.style).attr({ align: this.textAlign, rotation: d.rotation, visibility: "hidden" }).add(a) }, setOffset: function (a,
            d) { var f = this.axis, b = f.chart, e = f.translate(f.usePercentage ? 100 : this.total, 0, 0, 0, 1), f = f.translate(0), f = Math.abs(e - f); a = b.xAxis[0].translate(this.x) + a; e = this.getStackBox(b, this, a, e, d, f); if (d = this.label) d.align(this.alignOptions, null, e), e = d.alignAttr, d[!1 === this.options.crop || b.isInsidePlot(e.x, e.y) ? "show" : "hide"](!0) }, getStackBox: function (a, d, f, b, e, l) {
                var g = d.axis.reversed, k = a.inverted; a = a.plotHeight; d = d.isNegative && !g || !d.isNegative && g; return {
                    x: k ? d ? b : b - l : f, y: k ? a - f - e : d ? a - b - l : a - b, width: k ? l : e, height: k ?
                        e : l
                }
            }
        }; A.prototype.getStacks = function () { var a = this; f(a.yAxis, function (a) { a.stacks && a.hasVisibleSeries && (a.oldStacks = a.stacks) }); f(a.series, function (d) { !d.options.stacking || !0 !== d.visible && !1 !== a.options.chart.ignoreHiddenSeries || (d.stackKey = d.type + u(d.options.stack, "")) }) }; C.prototype.buildStacks = function () { var a = this.series, d = u(this.options.reversedStacks, !0), f = a.length, b; if (!this.isXAxis) { this.usePercentage = !1; for (b = f; b--;) a[d ? b : f - b - 1].setStackedPoints(); if (this.usePercentage) for (b = 0; b < f; b++) a[b].setPercentStacks() } };
        C.prototype.renderStackTotals = function () { var a = this.chart, d = a.renderer, f = this.stacks, b = this.stackTotalGroup; b || (this.stackTotalGroup = b = d.g("stack-labels").attr({ visibility: "visible", zIndex: 6 }).add()); b.translate(a.plotLeft, a.plotTop); r(f, function (a) { r(a, function (a) { a.render(b) }) }) }; C.prototype.resetStacks = function () { var a = this, d = a.stacks; a.isXAxis || r(d, function (d) { r(d, function (b, e) { b.touched < a.stacksTouched ? (b.destroy(), delete d[e]) : (b.total = null, b.cum = null) }) }) }; C.prototype.cleanStacks = function () {
            var a;
            this.isXAxis || (this.oldStacks && (a = this.stacks = this.oldStacks), r(a, function (a) { r(a, function (a) { a.cum = a.total }) }))
        }; t.prototype.setStackedPoints = function () {
            if (this.options.stacking && (!0 === this.visible || !1 === this.chart.options.chart.ignoreHiddenSeries)) {
                var f = this.processedXData, d = this.processedYData, k = [], b = d.length, e = this.options, l = e.threshold, m = e.startFromThreshold ? l : 0, n = e.stack, e = e.stacking, r = this.stackKey, t = "-" + r, c = this.negStacks, G = this.yAxis, q = G.stacks, B = G.oldStacks, K, p, z, I, A, h, w; G.stacksTouched +=
                1; for (A = 0; A < b; A++) h = f[A], w = d[A], K = this.getStackIndicator(K, h, this.index), I = K.key, z = (p = c && w < (m ? 0 : l)) ? t : r, q[z] || (q[z] = {}), q[z][h] || (B[z] && B[z][h] ? (q[z][h] = B[z][h], q[z][h].total = null) : q[z][h] = new a.StackItem(G, G.options.stackLabels, p, h, n)), z = q[z][h], null !== w && (z.points[I] = z.points[this.index] = [u(z.cum, m)], E(z.cum) || (z.base = I), z.touched = G.stacksTouched, 0 < K.index && !1 === this.singleStacks && (z.points[I][0] = z.points[this.index + "," + h + ",0"][0])), "percent" === e ? (p = p ? r : t, c && q[p] && q[p][h] ? (p = q[p][h], z.total = p.total =
                Math.max(p.total, z.total) + Math.abs(w) || 0) : z.total = F(z.total + (Math.abs(w) || 0))) : z.total = F(z.total + (w || 0)), z.cum = u(z.cum, m) + (w || 0), null !== w && (z.points[I].push(z.cum), k[A] = z.cum); "percent" === e && (G.usePercentage = !0); this.stackedYData = k; G.oldStacks = {}
            }
        }; t.prototype.setPercentStacks = function () {
            var a = this, d = a.stackKey, k = a.yAxis.stacks, b = a.processedXData, e; f([d, "-" + d], function (d) {
                for (var f = b.length, g, l; f--;) if (g = b[f], e = a.getStackIndicator(e, g, a.index, d), g = (l = k[d] && k[d][g]) && l.points[e.key]) l = l.total ? 100 /
                l.total : 0, g[0] = F(g[0] * l), g[1] = F(g[1] * l), a.stackedYData[f] = g[1]
            })
        }; t.prototype.getStackIndicator = function (a, d, f, b) { !E(a) || a.x !== d || b && a.key !== b ? a = { x: d, index: 0, key: b } : a.index++; a.key = [f, d, a.index].join(); return a }
    })(M); (function (a) {
        var C = a.addEvent, A = a.animate, F = a.Axis, E = a.createElement, m = a.css, f = a.defined, l = a.each, r = a.erase, u = a.extend, t = a.fireEvent, g = a.inArray, d = a.isNumber, k = a.isObject, b = a.isArray, e = a.merge, v = a.objectEach, y = a.pick, n = a.Point, D = a.Series, J = a.seriesTypes, c = a.setAnimation, G = a.splat; u(a.Chart.prototype,
        {
            addSeries: function (a, b, c) { var e, d = this; a && (b = y(b, !0), t(d, "addSeries", { options: a }, function () { e = d.initSeries(a); d.isDirtyLegend = !0; d.linkSeries(); b && d.redraw(c) })); return e }, addAxis: function (a, b, c, d) { var f = b ? "xAxis" : "yAxis", g = this.options; a = e(a, { index: this[f].length, isX: b }); b = new F(this, a); g[f] = G(g[f] || {}); g[f].push(a); y(c, !0) && this.redraw(d); return b }, showLoading: function (a) {
                var b = this, c = b.options, e = b.loadingDiv, d = c.loading, f = function () {
                    e && m(e, {
                        left: b.plotLeft + "px", top: b.plotTop + "px", width: b.plotWidth +
                        "px", height: b.plotHeight + "px"
                    })
                }; e || (b.loadingDiv = e = E("div", { className: "highcharts-loading highcharts-loading-hidden" }, null, b.container), b.loadingSpan = E("span", { className: "highcharts-loading-inner" }, null, e), C(b, "redraw", f)); e.className = "highcharts-loading"; b.loadingSpan.innerHTML = a || c.lang.loading; m(e, u(d.style, { zIndex: 10 })); m(b.loadingSpan, d.labelStyle); b.loadingShown || (m(e, { opacity: 0, display: "" }), A(e, { opacity: d.style.opacity || .5 }, { duration: d.showDuration || 0 })); b.loadingShown = !0; f()
            }, hideLoading: function () {
                var a =
                this.options, b = this.loadingDiv; b && (b.className = "highcharts-loading highcharts-loading-hidden", A(b, { opacity: 0 }, { duration: a.loading.hideDuration || 100, complete: function () { m(b, { display: "none" }) } })); this.loadingShown = !1
            }, propsRequireDirtyBox: "backgroundColor borderColor borderWidth margin marginTop marginRight marginBottom marginLeft spacing spacingTop spacingRight spacingBottom spacingLeft borderRadius plotBackgroundColor plotBackgroundImage plotBorderColor plotBorderWidth plotShadow shadow".split(" "),
            propsRequireUpdateSeries: "chart.inverted chart.polar chart.ignoreHiddenSeries chart.type colors plotOptions tooltip".split(" "), update: function (a, b, c) {
                var k = this, n = { credits: "addCredits", title: "setTitle", subtitle: "setSubtitle" }, q = a.chart, m, h, r = []; if (q) {
                    e(!0, k.options.chart, q); "className" in q && k.setClassName(q.className); if ("inverted" in q || "polar" in q) k.propFromSeries(), m = !0; "alignTicks" in q && (m = !0); v(q, function (a, b) {
                        -1 !== g("chart." + b, k.propsRequireUpdateSeries) && (h = !0); -1 !== g(b, k.propsRequireDirtyBox) &&
                        (k.isDirtyBox = !0)
                    }); "style" in q && k.renderer.setStyle(q.style)
                } a.colors && (this.options.colors = a.colors); a.plotOptions && e(!0, this.options.plotOptions, a.plotOptions); v(a, function (a, b) { if (k[b] && "function" === typeof k[b].update) k[b].update(a, !1); else if ("function" === typeof k[n[b]]) k[n[b]](a); "chart" !== b && -1 !== g(b, k.propsRequireUpdateSeries) && (h = !0) }); l("xAxis yAxis zAxis series colorAxis pane".split(" "), function (b) {
                    a[b] && (l(G(a[b]), function (a, e) {
                        (e = f(a.id) && k.get(a.id) || k[b][e]) && e.coll === b && (e.update(a,
                        !1), c && (e.touched = !0)); if (!e && c) if ("series" === b) k.addSeries(a, !1).touched = !0; else if ("xAxis" === b || "yAxis" === b) k.addAxis(a, "xAxis" === b, !1).touched = !0
                    }), c && l(k[b], function (a) { a.touched ? delete a.touched : r.push(a) }))
                }); l(r, function (a) { a.remove(!1) }); m && l(k.axes, function (a) { a.update({}, !1) }); h && l(k.series, function (a) { a.update({}, !1) }); a.loading && e(!0, k.options.loading, a.loading); m = q && q.width; q = q && q.height; d(m) && m !== k.chartWidth || d(q) && q !== k.chartHeight ? k.setSize(m, q) : y(b, !0) && k.redraw()
            }, setSubtitle: function (a) {
                this.setTitle(void 0,
                a)
            }
        }); u(n.prototype, {
            update: function (a, b, c, e) {
                function d() { f.applyOptions(a); null === f.y && h && (f.graphic = h.destroy()); k(a, !0) && (h && h.element && a && a.marker && void 0 !== a.marker.symbol && (f.graphic = h.destroy()), a && a.dataLabels && f.dataLabel && (f.dataLabel = f.dataLabel.destroy())); p = f.index; g.updateParallelArrays(f, p); q.data[p] = k(q.data[p], !0) || k(a, !0) ? f.options : a; g.isDirty = g.isDirtyData = !0; !g.fixedBox && g.hasCartesianSeries && (l.isDirtyBox = !0); "point" === q.legendType && (l.isDirtyLegend = !0); b && l.redraw(c) } var f =
                this, g = f.series, h = f.graphic, p, l = g.chart, q = g.options; b = y(b, !0); !1 === e ? d() : f.firePointEvent("update", { options: a }, d)
            }, remove: function (a, b) { this.series.removePoint(g(this, this.series.data), a, b) }
        }); u(D.prototype, {
            addPoint: function (a, b, c, e) {
                var d = this.options, f = this.data, g = this.chart, h = this.xAxis, h = h && h.hasNames && h.names, k = d.data, p, l, q = this.xData, n, m; b = y(b, !0); p = { series: this }; this.pointClass.prototype.applyOptions.apply(p, [a]); m = p.x; n = q.length; if (this.requireSorting && m < q[n - 1]) for (l = !0; n && q[n - 1] > m;) n--;
                this.updateParallelArrays(p, "splice", n, 0, 0); this.updateParallelArrays(p, n); h && p.name && (h[m] = p.name); k.splice(n, 0, a); l && (this.data.splice(n, 0, null), this.processData()); "point" === d.legendType && this.generatePoints(); c && (f[0] && f[0].remove ? f[0].remove(!1) : (f.shift(), this.updateParallelArrays(p, "shift"), k.shift())); this.isDirtyData = this.isDirty = !0; b && g.redraw(e)
            }, removePoint: function (a, b, e) {
                var d = this, f = d.data, g = f[a], k = d.points, h = d.chart, l = function () {
                    k && k.length === f.length && k.splice(a, 1); f.splice(a, 1);
                    d.options.data.splice(a, 1); d.updateParallelArrays(g || { series: d }, "splice", a, 1); g && g.destroy(); d.isDirty = !0; d.isDirtyData = !0; b && h.redraw()
                }; c(e, h); b = y(b, !0); g ? g.firePointEvent("remove", null, l) : l()
            }, remove: function (a, b, c) { function e() { d.destroy(); f.isDirtyLegend = f.isDirtyBox = !0; f.linkSeries(); y(a, !0) && f.redraw(b) } var d = this, f = d.chart; !1 !== c ? t(d, "remove", null, e) : e() }, update: function (a, b) {
                var c = this, d = c.chart, f = c.userOptions, g = c.oldType || c.type, k = a.type || f.type || d.options.chart.type, h = J[g].prototype, n,
                q = ["group", "markerGroup", "dataLabelsGroup", "navigatorSeries", "baseSeries"], m = c.finishedAnimating && { animation: !1 }; if (Object.keys && "data" === Object.keys(a).toString()) return this.setData(a.data, b); if (k && k !== g || void 0 !== a.zIndex) q.length = 0; l(q, function (a) { q[a] = c[a]; delete c[a] }); a = e(f, m, { index: c.index, pointStart: c.xData[0] }, { data: c.options.data }, a); c.remove(!1, null, !1); for (n in h) c[n] = void 0; u(c, J[k || g].prototype); l(q, function (a) { c[a] = q[a] }); c.init(d, a); c.oldType = g; d.linkSeries(); y(b, !0) && d.redraw(!1)
            }
        });
        u(F.prototype, {
            update: function (a, b) { var c = this.chart; a = c.options[this.coll][this.options.index] = e(this.userOptions, a); this.destroy(!0); this.init(c, u(a, { events: void 0 })); c.isDirtyBox = !0; y(b, !0) && c.redraw() }, remove: function (a) { for (var c = this.chart, e = this.coll, d = this.series, f = d.length; f--;) d[f] && d[f].remove(!1); r(c.axes, this); r(c[e], this); b(c.options[e]) ? c.options[e].splice(this.options.index, 1) : delete c.options[e]; l(c[e], function (a, b) { a.options.index = b }); this.destroy(); c.isDirtyBox = !0; y(a, !0) && c.redraw() },
            setTitle: function (a, b) { this.update({ title: a }, b) }, setCategories: function (a, b) { this.update({ categories: a }, b) }
        })
    })(M); (function (a) {
        var C = a.color, A = a.each, F = a.map, E = a.pick, m = a.Series, f = a.seriesType; f("area", "line", { softThreshold: !1, threshold: 0 }, {
            singleStacks: !1, getStackPoints: function (f) {
                var l = [], m = [], t = this.xAxis, g = this.yAxis, d = g.stacks[this.stackKey], k = {}, b = this.index, e = g.series, v = e.length, y, n = E(g.options.reversedStacks, !0) ? 1 : -1, D; f = f || this.points; if (this.options.stacking) {
                    for (D = 0; D < f.length; D++) k[f[D].x] =
                    f[D]; a.objectEach(d, function (a, b) { null !== a.total && m.push(b) }); m.sort(function (a, b) { return a - b }); y = F(e, function () { return this.visible }); A(m, function (a, c) {
                        var e = 0, f, r; if (k[a] && !k[a].isNull) l.push(k[a]), A([-1, 1], function (e) { var g = 1 === e ? "rightNull" : "leftNull", l = 0, q = d[m[c + e]]; if (q) for (D = b; 0 <= D && D < v;) f = q.points[D], f || (D === b ? k[a][g] = !0 : y[D] && (r = d[a].points[D]) && (l -= r[1] - r[0])), D += n; k[a][1 === e ? "rightCliff" : "leftCliff"] = l }); else {
                            for (D = b; 0 <= D && D < v;) { if (f = d[a].points[D]) { e = f[1]; break } D += n } e = g.translate(e, 0,
                            1, 0, 1); l.push({ isNull: !0, plotX: t.translate(a, 0, 0, 0, 1), x: a, plotY: e, yBottom: e })
                        }
                    })
                } return l
            }, getGraphPath: function (a) {
                var f = m.prototype.getGraphPath, l = this.options, t = l.stacking, g = this.yAxis, d, k, b = [], e = [], v = this.index, y, n = g.stacks[this.stackKey], D = l.threshold, A = g.getThreshold(l.threshold), c, l = l.connectNulls || "percent" === t, G = function (c, d, f) {
                    var k = a[c]; c = t && n[k.x].points[v]; var l = k[f + "Null"] || 0; f = k[f + "Cliff"] || 0; var q, m, k = !0; f || l ? (q = (l ? c[0] : c[1]) + f, m = c[0] + f, k = !!l) : !t && a[d] && a[d].isNull && (q = m = D); void 0 !==
                    q && (e.push({ plotX: y, plotY: null === q ? A : g.getThreshold(q), isNull: k, isCliff: !0 }), b.push({ plotX: y, plotY: null === m ? A : g.getThreshold(m), doCurve: !1 }))
                }; a = a || this.points; t && (a = this.getStackPoints(a)); for (d = 0; d < a.length; d++) if (k = a[d].isNull, y = E(a[d].rectPlotX, a[d].plotX), c = E(a[d].yBottom, A), !k || l) l || G(d, d - 1, "left"), k && !t && l || (e.push(a[d]), b.push({ x: d, plotX: y, plotY: c })), l || G(d, d + 1, "right"); d = f.call(this, e, !0, !0); b.reversed = !0; k = f.call(this, b, !0, !0); k.length && (k[0] = "L"); k = d.concat(k); f = f.call(this, e, !1, l); k.xMap =
                d.xMap; this.areaPath = k; return f
            }, drawGraph: function () {
                this.areaPath = []; m.prototype.drawGraph.apply(this); var a = this, f = this.areaPath, u = this.options, t = [["area", "highcharts-area", this.color, u.fillColor]]; A(this.zones, function (f, d) { t.push(["zone-area-" + d, "highcharts-area highcharts-zone-area-" + d + " " + f.className, f.color || a.color, f.fillColor || u.fillColor]) }); A(t, function (g) {
                    var d = g[0], k = a[d]; k ? (k.endX = f.xMap, k.animate({ d: f })) : (k = a[d] = a.chart.renderer.path(f).addClass(g[1]).attr({
                        fill: E(g[3], C(g[2]).setOpacity(E(u.fillOpacity,
                        .75)).get()), zIndex: 0
                    }).add(a.group), k.isArea = !0); k.startX = f.xMap; k.shiftUnit = u.step ? 2 : 1
                })
            }, drawLegendSymbol: a.LegendSymbolMixin.drawRectangle
        })
    })(M); (function (a) {
        var C = a.pick; a = a.seriesType; a("spline", "line", {}, {
            getPointSpline: function (a, F, E) {
                var m = F.plotX, f = F.plotY, l = a[E - 1]; E = a[E + 1]; var r, u, t, g; if (l && !l.isNull && !1 !== l.doCurve && !F.isCliff && E && !E.isNull && !1 !== E.doCurve && !F.isCliff) {
                    a = l.plotY; t = E.plotX; E = E.plotY; var d = 0; r = (1.5 * m + l.plotX) / 2.5; u = (1.5 * f + a) / 2.5; t = (1.5 * m + t) / 2.5; g = (1.5 * f + E) / 2.5; t !== r && (d =
                    (g - u) * (t - m) / (t - r) + f - g); u += d; g += d; u > a && u > f ? (u = Math.max(a, f), g = 2 * f - u) : u < a && u < f && (u = Math.min(a, f), g = 2 * f - u); g > E && g > f ? (g = Math.max(E, f), u = 2 * f - g) : g < E && g < f && (g = Math.min(E, f), u = 2 * f - g); F.rightContX = t; F.rightContY = g
                } F = ["C", C(l.rightContX, l.plotX), C(l.rightContY, l.plotY), C(r, m), C(u, f), m, f]; l.rightContX = l.rightContY = null; return F
            }
        })
    })(M); (function (a) {
        var C = a.seriesTypes.area.prototype, A = a.seriesType; A("areaspline", "spline", a.defaultPlotOptions.area, {
            getStackPoints: C.getStackPoints, getGraphPath: C.getGraphPath,
            drawGraph: C.drawGraph, drawLegendSymbol: a.LegendSymbolMixin.drawRectangle
        })
    })(M); (function (a) {
        var C = a.animObject, A = a.color, F = a.each, E = a.extend, m = a.isNumber, f = a.merge, l = a.pick, r = a.Series, u = a.seriesType, t = a.svg; u("column", "line", {
            borderRadius: 0, crisp: !0, groupPadding: .2, marker: null, pointPadding: .1, minPointLength: 0, cropThreshold: 50, pointRange: null, states: { hover: { halo: !1, brightness: .1, shadow: !1 }, select: { color: "#cccccc", borderColor: "#000000", shadow: !1 } }, dataLabels: { align: null, verticalAlign: null, y: null },
            softThreshold: !1, startFromThreshold: !0, stickyTracking: !1, tooltip: { distance: 6 }, threshold: 0, borderColor: "#ffffff"
        }, {
            cropShoulder: 0, directTouch: !0, trackerGroups: ["group", "dataLabelsGroup"], negStacks: !0, init: function () { r.prototype.init.apply(this, arguments); var a = this, d = a.chart; d.hasRendered && F(d.series, function (d) { d.type === a.type && (d.isDirty = !0) }) }, getColumnMetrics: function () {
                var a = this, d = a.options, f = a.xAxis, b = a.yAxis, e = f.reversed, m, r = {}, n = 0; !1 === d.grouping ? n = 1 : F(a.chart.series, function (c) {
                    var e = c.options,
                    d = c.yAxis, f; c.type !== a.type || !c.visible && a.chart.options.chart.ignoreHiddenSeries || b.len !== d.len || b.pos !== d.pos || (e.stacking ? (m = c.stackKey, void 0 === r[m] && (r[m] = n++), f = r[m]) : !1 !== e.grouping && (f = n++), c.columnIndex = f)
                }); var t = Math.min(Math.abs(f.transA) * (f.ordinalSlope || d.pointRange || f.closestPointRange || f.tickInterval || 1), f.len), u = t * d.groupPadding, c = (t - 2 * u) / (n || 1), d = Math.min(d.maxPointWidth || f.len, l(d.pointWidth, c * (1 - 2 * d.pointPadding))); a.columnMetrics = {
                    width: d, offset: (c - d) / 2 + (u + ((a.columnIndex || 0) +
                    (e ? 1 : 0)) * c - t / 2) * (e ? -1 : 1)
                }; return a.columnMetrics
            }, crispCol: function (a, d, f, b) { var e = this.chart, g = this.borderWidth, k = -(g % 2 ? .5 : 0), g = g % 2 ? .5 : 1; e.inverted && e.renderer.isVML && (g += 1); this.options.crisp && (f = Math.round(a + f) + k, a = Math.round(a) + k, f -= a); b = Math.round(d + b) + g; k = .5 >= Math.abs(d) && .5 < b; d = Math.round(d) + g; b -= d; k && b && (--d, b += 1); return { x: a, y: d, width: f, height: b } }, translate: function () {
                var a = this, d = a.chart, f = a.options, b = a.dense = 2 > a.closestPointRange * a.xAxis.transA, b = a.borderWidth = l(f.borderWidth, b ? 0 : 1), e = a.yAxis,
                m = a.translatedThreshold = e.getThreshold(f.threshold), t = l(f.minPointLength, 5), n = a.getColumnMetrics(), u = n.width, A = a.barW = Math.max(u, 1 + 2 * b), c = a.pointXOffset = n.offset; d.inverted && (m -= .5); f.pointPadding && (A = Math.ceil(A)); r.prototype.translate.apply(a); F(a.points, function (b) {
                    var f = l(b.yBottom, m), g = 999 + Math.abs(f), g = Math.min(Math.max(-g, b.plotY), e.len + g), k = b.plotX + c, n = A, r = Math.min(g, f), v, y = Math.max(g, f) - r; Math.abs(y) < t && t && (y = t, v = !e.reversed && !b.negative || e.reversed && b.negative, r = Math.abs(r - m) > t ? f - t : m - (v ?
                        t : 0)); b.barX = k; b.pointWidth = u; b.tooltipPos = d.inverted ? [e.len + e.pos - d.plotLeft - g, a.xAxis.len - k - n / 2, y] : [k + n / 2, g + e.pos - d.plotTop, y]; b.shapeType = "rect"; b.shapeArgs = a.crispCol.apply(a, b.isNull ? [k, m, n, 0] : [k, r, n, y])
                })
            }, getSymbol: a.noop, drawLegendSymbol: a.LegendSymbolMixin.drawRectangle, drawGraph: function () { this.group[this.dense ? "addClass" : "removeClass"]("highcharts-dense-data") }, pointAttribs: function (a, d) {
                var g = this.options, b, e = this.pointAttrToOptions || {}; b = e.stroke || "borderColor"; var l = e["stroke-width"] ||
                "borderWidth", m = a && a.color || this.color, n = a[b] || g[b] || this.color || m, r = a[l] || g[l] || this[l] || 0, e = g.dashStyle; a && this.zones.length && (m = a.getZone(), m = a.options.color || m && m.color || this.color); d && (a = f(g.states[d], a.options.states && a.options.states[d] || {}), d = a.brightness, m = a.color || void 0 !== d && A(m).brighten(a.brightness).get() || m, n = a[b] || n, r = a[l] || r, e = a.dashStyle || e); b = { fill: m, stroke: n, "stroke-width": r }; e && (b.dashstyle = e); return b
            }, drawPoints: function () {
                var a = this, d = this.chart, k = a.options, b = d.renderer, e =
                k.animationLimit || 250, l; F(a.points, function (g) { var n = g.graphic; if (m(g.plotY) && null !== g.y) { l = g.shapeArgs; if (n) n[d.pointCount < e ? "animate" : "attr"](f(l)); else g.graphic = n = b[g.shapeType](l).add(g.group || a.group); k.borderRadius && n.attr({ r: k.borderRadius }); n.attr(a.pointAttribs(g, g.selected && "select")).shadow(k.shadow, null, k.stacking && !k.borderRadius); n.addClass(g.getClassName(), !0) } else n && (g.graphic = n.destroy()) })
            }, animate: function (a) {
                var d = this, f = this.yAxis, b = d.options, e = this.chart.inverted, g = {}; t && (a ?
                (g.scaleY = .001, a = Math.min(f.pos + f.len, Math.max(f.pos, f.toPixels(b.threshold))), e ? g.translateX = a - f.len : g.translateY = a, d.group.attr(g)) : (g[e ? "translateX" : "translateY"] = f.pos, d.group.animate(g, E(C(d.options.animation), { step: function (a, b) { d.group.attr({ scaleY: Math.max(.001, b.pos) }) } })), d.animate = null))
            }, remove: function () { var a = this, d = a.chart; d.hasRendered && F(d.series, function (d) { d.type === a.type && (d.isDirty = !0) }); r.prototype.remove.apply(a, arguments) }
        })
    })(M); (function (a) {
        a = a.seriesType; a("bar", "column",
        null, { inverted: !0 })
    })(M); (function (a) {
        var C = a.Series; a = a.seriesType; a("scatter", "line", { lineWidth: 0, findNearestPointBy: "xy", marker: { enabled: !0 }, tooltip: { headerFormat: '\x3cspan style\x3d"color:{point.color}"\x3e\u25cf\x3c/span\x3e \x3cspan style\x3d"font-size: 0.85em"\x3e {series.name}\x3c/span\x3e\x3cbr/\x3e', pointFormat: "x: \x3cb\x3e{point.x}\x3c/b\x3e\x3cbr/\x3ey: \x3cb\x3e{point.y}\x3c/b\x3e\x3cbr/\x3e" } }, {
            sorted: !1, requireSorting: !1, noSharedTooltip: !0, trackerGroups: ["group", "markerGroup", "dataLabelsGroup"],
            takeOrdinalPosition: !1, drawGraph: function () { this.options.lineWidth && C.prototype.drawGraph.call(this) }
        })
    })(M); (function (a) { var C = a.pick, A = a.relativeLength; a.CenteredSeriesMixin = { getCenter: function () { var a = this.options, E = this.chart, m = 2 * (a.slicedOffset || 0), f = E.plotWidth - 2 * m, E = E.plotHeight - 2 * m, l = a.center, l = [C(l[0], "50%"), C(l[1], "50%"), a.size || "100%", a.innerSize || 0], r = Math.min(f, E), u, t; for (u = 0; 4 > u; ++u) t = l[u], a = 2 > u || 2 === u && /%$/.test(t), l[u] = A(t, [f, E, r, l[2]][u]) + (a ? m : 0); l[3] > l[2] && (l[3] = l[2]); return l } } })(M);
    (function (a) {
        var C = a.addEvent, A = a.defined, F = a.each, E = a.extend, m = a.inArray, f = a.noop, l = a.pick, r = a.Point, u = a.Series, t = a.seriesType, g = a.setAnimation; t("pie", "line", {
            center: [null, null], clip: !1, colorByPoint: !0, dataLabels: { distance: 30, enabled: !0, formatter: function () { return this.point.isNull ? void 0 : this.point.name }, x: 0 }, ignoreHiddenPoint: !0, legendType: "point", marker: null, size: null, showInLegend: !1, slicedOffset: 10, stickyTracking: !1, tooltip: { followPointer: !0 }, borderColor: "#ffffff", borderWidth: 1, states: {
                hover: {
                    brightness: .1,
                    shadow: !1
                }
            }
        }, {
            isCartesian: !1, requireSorting: !1, directTouch: !0, noSharedTooltip: !0, trackerGroups: ["group", "dataLabelsGroup"], axisTypes: [], pointAttribs: a.seriesTypes.column.prototype.pointAttribs, animate: function (a) { var d = this, b = d.points, e = d.startAngleRad; a || (F(b, function (a) { var b = a.graphic, f = a.shapeArgs; b && (b.attr({ r: a.startR || d.center[3] / 2, start: e, end: e }), b.animate({ r: f.r, start: f.start, end: f.end }, d.options.animation)) }), d.animate = null) }, updateTotals: function () {
                var a, f = 0, b = this.points, e = b.length, g,
                l = this.options.ignoreHiddenPoint; for (a = 0; a < e; a++) g = b[a], f += l && !g.visible ? 0 : g.isNull ? 0 : g.y; this.total = f; for (a = 0; a < e; a++) g = b[a], g.percentage = 0 < f && (g.visible || !l) ? g.y / f * 100 : 0, g.total = f
            }, generatePoints: function () { u.prototype.generatePoints.call(this); this.updateTotals() }, translate: function (a) {
                this.generatePoints(); var d = 0, b = this.options, e = b.slicedOffset, f = e + (b.borderWidth || 0), g, n, m, r = b.startAngle || 0, c = this.startAngleRad = Math.PI / 180 * (r - 90), r = (this.endAngleRad = Math.PI / 180 * (l(b.endAngle, r + 360) - 90)) - c, t =
                this.points, q, B = b.dataLabels.distance, b = b.ignoreHiddenPoint, u, p = t.length, z; a || (this.center = a = this.getCenter()); this.getX = function (b, c, e) { m = Math.asin(Math.min((b - a[1]) / (a[2] / 2 + e.labelDistance), 1)); return a[0] + (c ? -1 : 1) * Math.cos(m) * (a[2] / 2 + e.labelDistance) }; for (u = 0; u < p; u++) {
                    z = t[u]; z.labelDistance = l(z.options.dataLabels && z.options.dataLabels.distance, B); this.maxLabelDistance = Math.max(this.maxLabelDistance || 0, z.labelDistance); g = c + d * r; if (!b || z.visible) d += z.percentage / 100; n = c + d * r; z.shapeType = "arc"; z.shapeArgs =
                    { x: a[0], y: a[1], r: a[2] / 2, innerR: a[3] / 2, start: Math.round(1E3 * g) / 1E3, end: Math.round(1E3 * n) / 1E3 }; m = (n + g) / 2; m > 1.5 * Math.PI ? m -= 2 * Math.PI : m < -Math.PI / 2 && (m += 2 * Math.PI); z.slicedTranslation = { translateX: Math.round(Math.cos(m) * e), translateY: Math.round(Math.sin(m) * e) }; n = Math.cos(m) * a[2] / 2; q = Math.sin(m) * a[2] / 2; z.tooltipPos = [a[0] + .7 * n, a[1] + .7 * q]; z.half = m < -Math.PI / 2 || m > Math.PI / 2 ? 1 : 0; z.angle = m; g = Math.min(f, z.labelDistance / 5); z.labelPos = [a[0] + n + Math.cos(m) * z.labelDistance, a[1] + q + Math.sin(m) * z.labelDistance, a[0] + n + Math.cos(m) *
                    g, a[1] + q + Math.sin(m) * g, a[0] + n, a[1] + q, 0 > z.labelDistance ? "center" : z.half ? "right" : "left", m]
                }
            }, drawGraph: null, drawPoints: function () {
                var a = this, f = a.chart.renderer, b, e, g, l, n = a.options.shadow; n && !a.shadowGroup && (a.shadowGroup = f.g("shadow").add(a.group)); F(a.points, function (d) {
                    if (!d.isNull) {
                        e = d.graphic; l = d.shapeArgs; b = d.getTranslate(); var k = d.shadowGroup; n && !k && (k = d.shadowGroup = f.g("shadow").add(a.shadowGroup)); k && k.attr(b); g = a.pointAttribs(d, d.selected && "select"); e ? e.setRadialReference(a.center).attr(g).animate(E(l,
                        b)) : (d.graphic = e = f[d.shapeType](l).setRadialReference(a.center).attr(b).add(a.group), d.visible || e.attr({ visibility: "hidden" }), e.attr(g).attr({ "stroke-linejoin": "round" }).shadow(n, k)); e.addClass(d.getClassName())
                    }
                })
            }, searchPoint: f, sortByAngle: function (a, f) { a.sort(function (a, e) { return void 0 !== a.angle && (e.angle - a.angle) * f }) }, drawLegendSymbol: a.LegendSymbolMixin.drawRectangle, getCenter: a.CenteredSeriesMixin.getCenter, getSymbol: f
        }, {
            init: function () {
                r.prototype.init.apply(this, arguments); var a = this, f; a.name =
                l(a.name, "Slice"); f = function (b) { a.slice("select" === b.type) }; C(a, "select", f); C(a, "unselect", f); return a
            }, isValid: function () { return a.isNumber(this.y, !0) && 0 <= this.y }, setVisible: function (a, f) {
                var b = this, e = b.series, d = e.chart, g = e.options.ignoreHiddenPoint; f = l(f, g); a !== b.visible && (b.visible = b.options.visible = a = void 0 === a ? !b.visible : a, e.options.data[m(b, e.data)] = b.options, F(["graphic", "dataLabel", "connector", "shadowGroup"], function (e) { if (b[e]) b[e][a ? "show" : "hide"](!0) }), b.legendItem && d.legend.colorizeItem(b,
                a), a || "hover" !== b.state || b.setState(""), g && (e.isDirty = !0), f && d.redraw())
            }, slice: function (a, f, b) { var e = this.series; g(b, e.chart); l(f, !0); this.sliced = this.options.sliced = A(a) ? a : !this.sliced; e.options.data[m(this, e.data)] = this.options; this.graphic.animate(this.getTranslate()); this.shadowGroup && this.shadowGroup.animate(this.getTranslate()) }, getTranslate: function () { return this.sliced ? this.slicedTranslation : { translateX: 0, translateY: 0 } }, haloPath: function (a) {
                var d = this.shapeArgs; return this.sliced || !this.visible ?
                [] : this.series.chart.renderer.symbols.arc(d.x, d.y, d.r + a, d.r + a, { innerR: this.shapeArgs.r, start: d.start, end: d.end })
            }
        })
    })(M); (function (a) {
        var C = a.addEvent, A = a.arrayMax, F = a.defined, E = a.each, m = a.extend, f = a.format, l = a.map, r = a.merge, u = a.noop, t = a.pick, g = a.relativeLength, d = a.Series, k = a.seriesTypes, b = a.stableSort; a.distribute = function (a, d) {
            function e(a, b) { return a.target - b.target } var f, g = !0, k = a, c = [], m; m = 0; for (f = a.length; f--;) m += a[f].size; if (m > d) {
                b(a, function (a, b) { return (b.rank || 0) - (a.rank || 0) }); for (m = f = 0; m <=
                d;) m += a[f].size, f++; c = a.splice(f - 1, a.length)
            } b(a, e); for (a = l(a, function (a) { return { size: a.size, targets: [a.target] } }) ; g;) { for (f = a.length; f--;) g = a[f], m = (Math.min.apply(0, g.targets) + Math.max.apply(0, g.targets)) / 2, g.pos = Math.min(Math.max(0, m - g.size / 2), d - g.size); f = a.length; for (g = !1; f--;) 0 < f && a[f - 1].pos + a[f - 1].size > a[f].pos && (a[f - 1].size += a[f].size, a[f - 1].targets = a[f - 1].targets.concat(a[f].targets), a[f - 1].pos + a[f - 1].size > d && (a[f - 1].pos = d - a[f - 1].size), a.splice(f, 1), g = !0) } f = 0; E(a, function (a) {
                var b = 0; E(a.targets,
                function () { k[f].pos = a.pos + b; b += k[f].size; f++ })
            }); k.push.apply(k, c); b(k, e)
        }; d.prototype.drawDataLabels = function () {
            var b = this, d = b.options, g = d.dataLabels, k = b.points, l, m, c = b.hasRendered || 0, u, q, B = t(g.defer, !!d.animation), A = b.chart.renderer; if (g.enabled || b._hasPointLabels) b.dlProcessOptions && b.dlProcessOptions(g), q = b.plotGroup("dataLabelsGroup", "data-labels", B && !c ? "hidden" : "visible", g.zIndex || 6), B && (q.attr({ opacity: +c }), c || C(b, "afterAnimate", function () {
                b.visible && q.show(!0); q[d.animation ? "animate" : "attr"]({ opacity: 1 },
                { duration: 200 })
            })), m = g, E(k, function (c) {
                var e, k = c.dataLabel, n, h, p = c.connector, v = !k, B; l = c.dlOptions || c.options && c.options.dataLabels; if (e = t(l && l.enabled, m.enabled) && null !== c.y) g = r(m, l), n = c.getLabelConfig(), u = g.format ? f(g.format, n) : g.formatter.call(n, g), B = g.style, n = g.rotation, B.color = t(g.color, B.color, b.color, "#000000"), "contrast" === B.color && (c.contrastColor = A.getContrast(c.color || b.color), B.color = g.inside || 0 > t(c.labelDistance, g.distance) || d.stacking ? c.contrastColor : "#000000"), d.cursor && (B.cursor = d.cursor),
                h = { fill: g.backgroundColor, stroke: g.borderColor, "stroke-width": g.borderWidth, r: g.borderRadius || 0, rotation: n, padding: g.padding, zIndex: 1 }, a.objectEach(h, function (a, b) { void 0 === a && delete h[b] }); !k || e && F(u) ? e && F(u) && (k ? h.text = u : (k = c.dataLabel = A[n ? "text" : "label"](u, 0, -9999, g.shape, null, null, g.useHTML, null, "data-label"), k.addClass("highcharts-data-label-color-" + c.colorIndex + " " + (g.className || "") + (g.useHTML ? "highcharts-tracker" : ""))), k.attr(h), k.css(B).shadow(g.shadow), k.added || k.add(q), b.alignDataLabel(c,
                k, g, null, v)) : (c.dataLabel = k = k.destroy(), p && (c.connector = p.destroy()))
            })
        }; d.prototype.alignDataLabel = function (a, b, d, f, g) {
            var e = this.chart, c = e.inverted, k = t(a.plotX, -9999), l = t(a.plotY, -9999), n = b.getBBox(), r, p = d.rotation, v = d.align, u = this.visible && (a.series.forceDL || e.isInsidePlot(k, Math.round(l), c) || f && e.isInsidePlot(k, c ? f.x + 1 : f.y + f.height - 1, c)), y = "justify" === t(d.overflow, "justify"); if (u && (r = d.style.fontSize, r = e.renderer.fontMetrics(r, b).b, f = m({
                x: c ? this.yAxis.len - l : k, y: Math.round(c ? this.xAxis.len - k : l),
                width: 0, height: 0
            }, f), m(d, { width: n.width, height: n.height }), p ? (y = !1, k = e.renderer.rotCorr(r, p), k = { x: f.x + d.x + f.width / 2 + k.x, y: f.y + d.y + { top: 0, middle: .5, bottom: 1 }[d.verticalAlign] * f.height }, b[g ? "attr" : "animate"](k).attr({ align: v }), l = (p + 720) % 360, l = 180 < l && 360 > l, "left" === v ? k.y -= l ? n.height : 0 : "center" === v ? (k.x -= n.width / 2, k.y -= n.height / 2) : "right" === v && (k.x -= n.width, k.y -= l ? 0 : n.height)) : (b.align(d, null, f), k = b.alignAttr), y ? a.isLabelJustified = this.justifyDataLabel(b, d, k, n, f, g) : t(d.crop, !0) && (u = e.isInsidePlot(k.x,
            k.y) && e.isInsidePlot(k.x + n.width, k.y + n.height)), d.shape && !p)) b[g ? "attr" : "animate"]({ anchorX: c ? e.plotWidth - a.plotY : a.plotX, anchorY: c ? e.plotHeight - a.plotX : a.plotY }); u || (b.attr({ y: -9999 }), b.placed = !1)
        }; d.prototype.justifyDataLabel = function (a, b, d, f, g, k) {
            var c = this.chart, e = b.align, l = b.verticalAlign, m, n, p = a.box ? 0 : a.padding || 0; m = d.x + p; 0 > m && ("right" === e ? b.align = "left" : b.x = -m, n = !0); m = d.x + f.width - p; m > c.plotWidth && ("left" === e ? b.align = "right" : b.x = c.plotWidth - m, n = !0); m = d.y + p; 0 > m && ("bottom" === l ? b.verticalAlign =
            "top" : b.y = -m, n = !0); m = d.y + f.height - p; m > c.plotHeight && ("top" === l ? b.verticalAlign = "bottom" : b.y = c.plotHeight - m, n = !0); n && (a.placed = !k, a.align(b, null, g)); return n
        }; k.pie && (k.pie.prototype.drawDataLabels = function () {
            var b = this, f = b.data, g, k = b.chart, l = b.options.dataLabels, m = t(l.connectorPadding, 10), c = t(l.connectorWidth, 1), r = k.plotWidth, q = k.plotHeight, u, C = b.center, p = C[2] / 2, z = C[1], I, L, h, w, M = [[], []], H, O, Q, R, x = [0, 0, 0, 0]; b.visible && (l.enabled || b._hasPointLabels) && (E(f, function (a) {
                a.dataLabel && a.visible && a.dataLabel.shortened &&
                (a.dataLabel.attr({ width: "auto" }).css({ width: "auto", textOverflow: "clip" }), a.dataLabel.shortened = !1)
            }), d.prototype.drawDataLabels.apply(b), E(f, function (a) { a.dataLabel && a.visible && (M[a.half].push(a), a.dataLabel._pos = null) }), E(M, function (c, d) {
                var e, f, n = c.length, v = [], u; if (n) for (b.sortByAngle(c, d - .5), 0 < b.maxLabelDistance && (e = Math.max(0, z - p - b.maxLabelDistance), f = Math.min(z + p + b.maxLabelDistance, k.plotHeight), E(c, function (a) {
                0 < a.labelDistance && a.dataLabel && (a.top = Math.max(0, z - p - a.labelDistance), a.bottom =
                Math.min(z + p + a.labelDistance, k.plotHeight), u = a.dataLabel.getBBox().height || 21, a.positionsIndex = v.push({ target: a.labelPos[1] - a.top + u / 2, size: u, rank: a.y }) - 1)
                }), a.distribute(v, f + u - e)), R = 0; R < n; R++) g = c[R], f = g.positionsIndex, h = g.labelPos, I = g.dataLabel, Q = !1 === g.visible ? "hidden" : "inherit", e = h[1], v && F(v[f]) ? void 0 === v[f].pos ? Q = "hidden" : (w = v[f].size, O = g.top + v[f].pos) : O = e, delete g.positionIndex, H = l.justify ? C[0] + (d ? -1 : 1) * (p + g.labelDistance) : b.getX(O < g.top + 2 || O > g.bottom - 2 ? e : O, d, g), I._attr = { visibility: Q, align: h[6] },
                I._pos = { x: H + l.x + ({ left: m, right: -m }[h[6]] || 0), y: O + l.y - 10 }, h.x = H, h.y = O, t(l.crop, !0) && (L = I.getBBox().width, e = null, H - L < m ? (e = Math.round(L - H + m), x[3] = Math.max(e, x[3])) : H + L > r - m && (e = Math.round(H + L - r + m), x[1] = Math.max(e, x[1])), 0 > O - w / 2 ? x[0] = Math.max(Math.round(-O + w / 2), x[0]) : O + w / 2 > q && (x[2] = Math.max(Math.round(O + w / 2 - q), x[2])), I.sideOverflow = e)
            }), 0 === A(x) || this.verifyDataLabelOverflow(x)) && (this.placeDataLabels(), c && E(this.points, function (a) {
                var e; u = a.connector; if ((I = a.dataLabel) && I._pos && a.visible && 0 < a.labelDistance) {
                    Q =
                    I._attr.visibility; if (e = !u) a.connector = u = k.renderer.path().addClass("highcharts-data-label-connector highcharts-color-" + a.colorIndex).add(b.dataLabelsGroup), u.attr({ "stroke-width": c, stroke: l.connectorColor || a.color || "#666666" }); u[e ? "attr" : "animate"]({ d: b.connectorPath(a.labelPos) }); u.attr("visibility", Q)
                } else u && (a.connector = u.destroy())
            }))
        }, k.pie.prototype.connectorPath = function (a) {
            var b = a.x, d = a.y; return t(this.options.dataLabels.softConnector, !0) ? ["M", b + ("left" === a[6] ? 5 : -5), d, "C", b, d, 2 * a[2] - a[4],
            2 * a[3] - a[5], a[2], a[3], "L", a[4], a[5]] : ["M", b + ("left" === a[6] ? 5 : -5), d, "L", a[2], a[3], "L", a[4], a[5]]
        }, k.pie.prototype.placeDataLabels = function () { E(this.points, function (a) { var b = a.dataLabel; b && a.visible && ((a = b._pos) ? (b.sideOverflow && (b._attr.width = b.getBBox().width - b.sideOverflow, b.css({ width: b._attr.width + "px", textOverflow: "ellipsis" }), b.shortened = !0), b.attr(b._attr), b[b.moved ? "animate" : "attr"](a), b.moved = !0) : b && b.attr({ y: -9999 })) }, this) }, k.pie.prototype.alignDataLabel = u, k.pie.prototype.verifyDataLabelOverflow =
        function (a) { var b = this.center, d = this.options, e = d.center, f = d.minSize || 80, k, c = null !== d.size; c || (null !== e[0] ? k = Math.max(b[2] - Math.max(a[1], a[3]), f) : (k = Math.max(b[2] - a[1] - a[3], f), b[0] += (a[3] - a[1]) / 2), null !== e[1] ? k = Math.max(Math.min(k, b[2] - Math.max(a[0], a[2])), f) : (k = Math.max(Math.min(k, b[2] - a[0] - a[2]), f), b[1] += (a[0] - a[2]) / 2), k < b[2] ? (b[2] = k, b[3] = Math.min(g(d.innerSize || 0, k), k), this.translate(b), this.drawDataLabels && this.drawDataLabels()) : c = !0); return c }); k.column && (k.column.prototype.alignDataLabel = function (a,
        b, f, g, k) {
            var e = this.chart.inverted, c = a.series, l = a.dlBox || a.shapeArgs, m = t(a.below, a.plotY > t(this.translatedThreshold, c.yAxis.len)), n = t(f.inside, !!this.options.stacking); l && (g = r(l), 0 > g.y && (g.height += g.y, g.y = 0), l = g.y + g.height - c.yAxis.len, 0 < l && (g.height -= l), e && (g = { x: c.yAxis.len - g.y - g.height, y: c.xAxis.len - g.x - g.width, width: g.height, height: g.width }), n || (e ? (g.x += m ? 0 : g.width, g.width = 0) : (g.y += m ? g.height : 0, g.height = 0))); f.align = t(f.align, !e || n ? "center" : m ? "right" : "left"); f.verticalAlign = t(f.verticalAlign, e ||
            n ? "middle" : m ? "top" : "bottom"); d.prototype.alignDataLabel.call(this, a, b, f, g, k); a.isLabelJustified && a.contrastColor && a.dataLabel.css({ color: a.contrastColor })
        })
    })(M); (function (a) {
        var C = a.Chart, A = a.each, F = a.objectEach, E = a.pick, m = a.addEvent; C.prototype.callbacks.push(function (a) {
            function f() {
                var f = []; A(a.yAxis || [], function (a) { a.options.stackLabels && !a.options.stackLabels.allowOverlap && F(a.stacks, function (a) { F(a, function (a) { f.push(a.label) }) }) }); A(a.series || [], function (a) {
                    var l = a.options.dataLabels, g = a.dataLabelCollections ||
                    ["dataLabel"]; (l.enabled || a._hasPointLabels) && !l.allowOverlap && a.visible && A(g, function (d) { A(a.points, function (a) { a[d] && (a[d].labelrank = E(a.labelrank, a.shapeArgs && a.shapeArgs.height), f.push(a[d])) }) })
                }); a.hideOverlappingLabels(f)
            } f(); m(a, "redraw", f)
        }); C.prototype.hideOverlappingLabels = function (a) {
            var f = a.length, m, u, t, g, d, k, b, e, v, y = function (a, b, d, c, e, f, g, k) { return !(e > a + d || e + g < a || f > b + c || f + k < b) }; for (u = 0; u < f; u++) if (m = a[u]) m.oldOpacity = m.opacity, m.newOpacity = 1, m.width || (t = m.getBBox(), m.width = t.width, m.height =
            t.height); a.sort(function (a, b) { return (b.labelrank || 0) - (a.labelrank || 0) }); for (u = 0; u < f; u++) for (t = a[u], m = u + 1; m < f; ++m) if (g = a[m], t && g && t !== g && t.placed && g.placed && 0 !== t.newOpacity && 0 !== g.newOpacity && (d = t.alignAttr, k = g.alignAttr, b = t.parentGroup, e = g.parentGroup, v = 2 * (t.box ? 0 : t.padding || 0), d = y(d.x + b.translateX, d.y + b.translateY, t.width - v, t.height - v, k.x + e.translateX, k.y + e.translateY, g.width - v, g.height - v))) (t.labelrank < g.labelrank ? t : g).newOpacity = 0; A(a, function (a) {
                var b, d; a && (d = a.newOpacity, a.oldOpacity !== d &&
                a.placed && (d ? a.show(!0) : b = function () { a.hide() }, a.alignAttr.opacity = d, a[a.isOld ? "animate" : "attr"](a.alignAttr, null, b)), a.isOld = !0)
            })
        }
    })(M); (function (a) {
        var C = a.addEvent, A = a.Chart, F = a.createElement, E = a.css, m = a.defaultOptions, f = a.defaultPlotOptions, l = a.each, r = a.extend, u = a.fireEvent, t = a.hasTouch, g = a.inArray, d = a.isObject, k = a.Legend, b = a.merge, e = a.pick, v = a.Point, y = a.Series, n = a.seriesTypes, D = a.svg, J; J = a.TrackerMixin = {
            drawTrackerPoint: function () {
                var a = this, b = a.chart.pointer, d = function (a) {
                    var c = b.getPointFromEvent(a);
                    void 0 !== c && (b.isDirectTouch = !0, c.onMouseOver(a))
                }; l(a.points, function (a) { a.graphic && (a.graphic.element.point = a); a.dataLabel && (a.dataLabel.div ? a.dataLabel.div.point = a : a.dataLabel.element.point = a) }); a._hasTracking || (l(a.trackerGroups, function (c) { if (a[c]) { a[c].addClass("highcharts-tracker").on("mouseover", d).on("mouseout", function (a) { b.onTrackerMouseOut(a) }); if (t) a[c].on("touchstart", d); a.options.cursor && a[c].css(E).css({ cursor: a.options.cursor }) } }), a._hasTracking = !0)
            }, drawTrackerGraph: function () {
                var a =
                this, b = a.options, d = b.trackByArea, e = [].concat(d ? a.areaPath : a.graphPath), f = e.length, g = a.chart, k = g.pointer, m = g.renderer, n = g.options.tooltip.snap, h = a.tracker, r, u = function () { if (g.hoverSeries !== a) a.onMouseOver() }, v = "rgba(192,192,192," + (D ? .0001 : .002) + ")"; if (f && !d) for (r = f + 1; r--;) "M" === e[r] && e.splice(r + 1, 0, e[r + 1] - n, e[r + 2], "L"), (r && "M" === e[r] || r === f) && e.splice(r, 0, "L", e[r - 2] + n, e[r - 1]); h ? h.attr({ d: e }) : a.graph && (a.tracker = m.path(e).attr({
                    "stroke-linejoin": "round", visibility: a.visible ? "visible" : "hidden", stroke: v,
                    fill: d ? v : "none", "stroke-width": a.graph.strokeWidth() + (d ? 0 : 2 * n), zIndex: 2
                }).add(a.group), l([a.tracker, a.markerGroup], function (a) { a.addClass("highcharts-tracker").on("mouseover", u).on("mouseout", function (a) { k.onTrackerMouseOut(a) }); b.cursor && a.css({ cursor: b.cursor }); if (t) a.on("touchstart", u) }))
            }
        }; n.column && (n.column.prototype.drawTracker = J.drawTrackerPoint); n.pie && (n.pie.prototype.drawTracker = J.drawTrackerPoint); n.scatter && (n.scatter.prototype.drawTracker = J.drawTrackerPoint); r(k.prototype, {
            setItemEvents: function (a,
            d, e) { var c = this, f = c.chart.renderer.boxWrapper, g = "highcharts-legend-" + (a.series ? "point" : "series") + "-active"; (e ? d : a.legendGroup).on("mouseover", function () { a.setState("hover"); f.addClass(g); d.css(c.options.itemHoverStyle) }).on("mouseout", function () { d.css(b(a.visible ? c.itemStyle : c.itemHiddenStyle)); f.removeClass(g); a.setState() }).on("click", function (b) { var c = function () { a.setVisible && a.setVisible() }; b = { browserEvent: b }; a.firePointEvent ? a.firePointEvent("legendItemClick", b, c) : u(a, "legendItemClick", b, c) }) },
            createCheckboxForItem: function (a) { a.checkbox = F("input", { type: "checkbox", checked: a.selected, defaultChecked: a.selected }, this.options.itemCheckboxStyle, this.chart.container); C(a.checkbox, "click", function (b) { u(a.series || a, "checkboxClick", { checked: b.target.checked, item: a }, function () { a.select() }) }) }
        }); m.legend.itemStyle.cursor = "pointer"; r(A.prototype, {
            showResetZoom: function () {
                var a = this, b = m.lang, d = a.options.chart.resetZoomButton, e = d.theme, f = e.states, g = "chart" === d.relativeTo ? null : "plotBox"; this.resetZoomButton =
                a.renderer.button(b.resetZoom, null, null, function () { a.zoomOut() }, e, f && f.hover).attr({ align: d.position.align, title: b.resetZoomTitle }).addClass("highcharts-reset-zoom").add().align(d.position, !1, g)
            }, zoomOut: function () { var a = this; u(a, "selection", { resetSelection: !0 }, function () { a.zoom() }) }, zoom: function (a) {
                var b, c = this.pointer, f = !1, g; !a || a.resetSelection ? (l(this.axes, function (a) { b = a.zoom() }), c.initiated = !1) : l(a.xAxis.concat(a.yAxis), function (a) {
                    var d = a.axis; c[d.isXAxis ? "zoomX" : "zoomY"] && (b = d.zoom(a.min,
                    a.max), d.displayBtn && (f = !0))
                }); g = this.resetZoomButton; f && !g ? this.showResetZoom() : !f && d(g) && (this.resetZoomButton = g.destroy()); b && this.redraw(e(this.options.chart.animation, a && a.animation, 100 > this.pointCount))
            }, pan: function (a, b) {
                var c = this, d = c.hoverPoints, e; d && l(d, function (a) { a.setState() }); l("xy" === b ? [1, 0] : [1], function (b) {
                    b = c[b ? "xAxis" : "yAxis"][0]; var d = b.horiz, f = a[d ? "chartX" : "chartY"], d = d ? "mouseDownX" : "mouseDownY", g = c[d], h = (b.pointRange || 0) / 2, k = b.getExtremes(), l = b.toValue(g - f, !0) + h, h = b.toValue(g +
                    b.len - f, !0) - h, m = h < l, g = m ? h : l, l = m ? l : h, h = Math.min(k.dataMin, b.toValue(b.toPixels(k.min) - b.minPixelPadding)), m = Math.max(k.dataMax, b.toValue(b.toPixels(k.max) + b.minPixelPadding)), n; n = h - g; 0 < n && (l += n, g = h); n = l - m; 0 < n && (l = m, g -= n); b.series.length && g !== k.min && l !== k.max && (b.setExtremes(g, l, !1, !1, { trigger: "pan" }), e = !0); c[d] = f
                }); e && c.redraw(!1); E(c.container, { cursor: "move" })
            }
        }); r(v.prototype, {
            select: function (a, b) {
                var c = this, d = c.series, f = d.chart; a = e(a, !c.selected); c.firePointEvent(a ? "select" : "unselect", { accumulate: b },
                function () { c.selected = c.options.selected = a; d.options.data[g(c, d.data)] = c.options; c.setState(a && "select"); b || l(f.getSelectedPoints(), function (a) { a.selected && a !== c && (a.selected = a.options.selected = !1, d.options.data[g(a, d.data)] = a.options, a.setState(""), a.firePointEvent("unselect")) }) })
            }, onMouseOver: function (a) { var b = this.series.chart, c = b.pointer; a = a ? c.normalize(a) : c.getChartCoordinatesFromPoint(this, b.inverted); c.runPointActions(a, this) }, onMouseOut: function () {
                var a = this.series.chart; this.firePointEvent("mouseOut");
                l(a.hoverPoints || [], function (a) { a.setState() }); a.hoverPoints = a.hoverPoint = null
            }, importEvents: function () { if (!this.hasImportedEvents) { var c = this, d = b(c.series.options.point, c.options).events; c.events = d; a.objectEach(d, function (a, b) { C(c, b, a) }); this.hasImportedEvents = !0 } }, setState: function (a, b) {
                var c = Math.floor(this.plotX), d = this.plotY, g = this.series, k = g.options.states[a] || {}, l = f[g.type].marker && g.options.marker, m = l && !1 === l.enabled, n = l && l.states && l.states[a] || {}, h = !1 === n.enabled, t = g.stateMarkerGraphic, u =
                this.marker || {}, v = g.chart, y = g.halo, A, C = l && g.markerAttribs; a = a || ""; if (!(a === this.state && !b || this.selected && "select" !== a || !1 === k.enabled || a && (h || m && !1 === n.enabled) || a && u.states && u.states[a] && !1 === u.states[a].enabled)) {
                    C && (A = g.markerAttribs(this, a)); if (this.graphic) this.state && this.graphic.removeClass("highcharts-point-" + this.state), a && this.graphic.addClass("highcharts-point-" + a), this.graphic.animate(g.pointAttribs(this, a), e(v.options.chart.animation, k.animation)), A && this.graphic.animate(A, e(v.options.chart.animation,
                    n.animation, l.animation)), t && t.hide(); else { if (a && n) { l = u.symbol || g.symbol; t && t.currentSymbol !== l && (t = t.destroy()); if (t) t[b ? "animate" : "attr"]({ x: A.x, y: A.y }); else l && (g.stateMarkerGraphic = t = v.renderer.symbol(l, A.x, A.y, A.width, A.height).add(g.markerGroup), t.currentSymbol = l); t && t.attr(g.pointAttribs(this, a)) } t && (t[a && v.isInsidePlot(c, d, v.inverted) ? "show" : "hide"](), t.element.point = this) } (c = k.halo) && c.size ? (y || (g.halo = y = v.renderer.path().add((this.graphic || t).parentGroup)), y[b ? "animate" : "attr"]({ d: this.haloPath(c.size) }),
                    y.attr({ "class": "highcharts-halo highcharts-color-" + e(this.colorIndex, g.colorIndex) }), y.point = this, y.attr(r({ fill: this.color || g.color, "fill-opacity": c.opacity, zIndex: -1 }, c.attributes))) : y && y.point && y.point.haloPath && y.animate({ d: y.point.haloPath(0) }); this.state = a
                }
            }, haloPath: function (a) { return this.series.chart.renderer.symbols.circle(Math.floor(this.plotX) - a, this.plotY - a, 2 * a, 2 * a) }
        }); r(y.prototype, {
            onMouseOver: function () {
                var a = this.chart, b = a.hoverSeries; if (b && b !== this) b.onMouseOut(); this.options.events.mouseOver &&
                u(this, "mouseOver"); this.setState("hover"); a.hoverSeries = this
            }, onMouseOut: function () { var a = this.options, b = this.chart, d = b.tooltip, e = b.hoverPoint; b.hoverSeries = null; if (e) e.onMouseOut(); this && a.events.mouseOut && u(this, "mouseOut"); !d || this.stickyTracking || d.shared && !this.noSharedTooltip || d.hide(); this.setState() }, setState: function (a) {
                var b = this, c = b.options, d = b.graph, f = c.states, g = c.lineWidth, c = 0; a = a || ""; if (b.state !== a && (l([b.group, b.markerGroup, b.dataLabelsGroup], function (c) {
                c && (b.state && c.removeClass("highcharts-series-" +
                b.state), a && c.addClass("highcharts-series-" + a))
                }), b.state = a, !f[a] || !1 !== f[a].enabled) && (a && (g = f[a].lineWidth || g + (f[a].lineWidthPlus || 0)), d && !d.dashstyle)) for (g = { "stroke-width": g }, d.animate(g, e(b.chart.options.chart.animation, f[a] && f[a].animation)) ; b["zone-graph-" + c];) b["zone-graph-" + c].attr(g), c += 1
            }, setVisible: function (a, b) {
                var c = this, d = c.chart, e = c.legendItem, f, g = d.options.chart.ignoreHiddenSeries, k = c.visible; f = (c.visible = a = c.options.visible = c.userOptions.visible = void 0 === a ? !k : a) ? "show" : "hide"; l(["group",
                "dataLabelsGroup", "markerGroup", "tracker", "tt"], function (a) { if (c[a]) c[a][f]() }); if (d.hoverSeries === c || (d.hoverPoint && d.hoverPoint.series) === c) c.onMouseOut(); e && d.legend.colorizeItem(c, a); c.isDirty = !0; c.options.stacking && l(d.series, function (a) { a.options.stacking && a.visible && (a.isDirty = !0) }); l(c.linkedSeries, function (b) { b.setVisible(a, !1) }); g && (d.isDirtyBox = !0); !1 !== b && d.redraw(); u(c, f)
            }, show: function () { this.setVisible(!0) }, hide: function () { this.setVisible(!1) }, select: function (a) {
                this.selected = a = void 0 ===
                a ? !this.selected : a; this.checkbox && (this.checkbox.checked = a); u(this, a ? "select" : "unselect")
            }, drawTracker: J.drawTrackerGraph
        })
    })(M); (function (a) {
        var C = a.Chart, A = a.each, F = a.inArray, E = a.isArray, m = a.isObject, f = a.pick, l = a.splat; C.prototype.setResponsive = function (f) {
            var l = this.options.responsive, m = [], g = this.currentResponsive; l && l.rules && A(l.rules, function (d) { void 0 === d._id && (d._id = a.uniqueKey()); this.matchResponsiveRule(d, m, f) }, this); var d = a.merge.apply(0, a.map(m, function (d) {
                return a.find(l.rules, function (a) {
                    return a._id ===
                    d
                }).chartOptions
            })), m = m.toString() || void 0; m !== (g && g.ruleIds) && (g && this.update(g.undoOptions, f), m ? (this.currentResponsive = { ruleIds: m, mergedOptions: d, undoOptions: this.currentOptions(d) }, this.update(d, f)) : this.currentResponsive = void 0)
        }; C.prototype.matchResponsiveRule = function (a, l) {
            var m = a.condition; (m.callback || function () { return this.chartWidth <= f(m.maxWidth, Number.MAX_VALUE) && this.chartHeight <= f(m.maxHeight, Number.MAX_VALUE) && this.chartWidth >= f(m.minWidth, 0) && this.chartHeight >= f(m.minHeight, 0) }).call(this) &&
            l.push(a._id)
        }; C.prototype.currentOptions = function (f) { function r(f, d, k, b) { var e; a.objectEach(f, function (a, g) { if (!b && -1 < F(g, ["series", "xAxis", "yAxis"])) for (f[g] = l(f[g]), k[g] = [], e = 0; e < f[g].length; e++) d[g][e] && (k[g][e] = {}, r(a[e], d[g][e], k[g][e], b + 1)); else m(a) ? (k[g] = E(a) ? [] : {}, r(a, d[g] || {}, k[g], b + 1)) : k[g] = d[g] || null }) } var t = {}; r(f, this.options, t, 0); return t }
    })(M); return M
});