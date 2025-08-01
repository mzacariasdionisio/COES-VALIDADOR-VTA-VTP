/*
 Highcharts JS v7.0.0 (2018-12-11)

 3D features for Highcharts JS

 @license: www.highcharts.com/license
*/
(function (A) { "object" === typeof module && module.exports ? module.exports = A : "function" === typeof define && define.amd ? define(function () { return A }) : A("undefined" !== typeof Highcharts ? Highcharts : void 0) })(function (A) {
    (function (b) {
        var y = b.deg2rad, z = b.pick; b.perspective3D = function (b, q, v) { q = 0 < v && v < Number.POSITIVE_INFINITY ? v / (b.z + q.z + v) : 1; return { x: b.x * q, y: b.y * q } }; b.perspective = function (u, q, v) {
            var x = q.options.chart.options3d, f = v ? q.inverted : !1, w = {
                x: q.plotWidth / 2, y: q.plotHeight / 2, z: x.depth / 2, vd: z(x.depth, 1) * z(x.viewDistance,
                    0)
            }, p = q.scale3d || 1, e = y * x.beta * (f ? -1 : 1), x = y * x.alpha * (f ? -1 : 1), a = Math.cos(x), l = Math.cos(-e), r = Math.sin(x), k = Math.sin(-e); v || (w.x += q.plotLeft, w.y += q.plotTop); return u.map(function (e) { var d; d = (f ? e.y : e.x) - w.x; var c = (f ? e.x : e.y) - w.y; e = (e.z || 0) - w.z; d = { x: l * d - k * e, y: -r * k * d + a * c - l * r * e, z: a * k * d + r * c + a * l * e }; c = b.perspective3D(d, w, w.vd); c.x = c.x * p + w.x; c.y = c.y * p + w.y; c.z = d.z * p + w.z; return { x: f ? c.y : c.x, y: f ? c.x : c.y, z: c.z } })
        }; b.pointCameraDistance = function (b, q) {
            var v = q.options.chart.options3d, x = q.plotWidth / 2; q = q.plotHeight /
                2; v = z(v.depth, 1) * z(v.viewDistance, 0) + v.depth; return Math.sqrt(Math.pow(x - b.plotX, 2) + Math.pow(q - b.plotY, 2) + Math.pow(v - b.plotZ, 2))
        }; b.shapeArea = function (b) { var q = 0, v, x; for (v = 0; v < b.length; v++)x = (v + 1) % b.length, q += b[v].x * b[x].y - b[x].x * b[v].y; return q / 2 }; b.shapeArea3d = function (u, q, v) { return b.shapeArea(b.perspective(u, q, v)) }
    })(A); (function (b) {
        function y(a, d, h, b, g, C, e, p) {
            var m = [], F = C - g; return C > g && C - g > Math.PI / 2 + .0001 ? (m = m.concat(y(a, d, h, b, g, g + Math.PI / 2, e, p)), m = m.concat(y(a, d, h, b, g + Math.PI / 2, C, e, p))) : C < g &&
                g - C > Math.PI / 2 + .0001 ? (m = m.concat(y(a, d, h, b, g, g - Math.PI / 2, e, p)), m = m.concat(y(a, d, h, b, g - Math.PI / 2, C, e, p))) : ["C", a + h * Math.cos(g) - h * c * F * Math.sin(g) + e, d + b * Math.sin(g) + b * c * F * Math.cos(g) + p, a + h * Math.cos(C) + h * c * F * Math.sin(C) + e, d + b * Math.sin(C) - b * c * F * Math.cos(C) + p, a + h * Math.cos(C) + e, d + b * Math.sin(C) + p]
        } var z = Math.cos, u = Math.PI, q = Math.sin, v = b.animObject, x = b.charts, f = b.color, w = b.defined, p = b.deg2rad, e = b.extend, a = b.merge, l = b.perspective, r = b.pick, k = b.SVGElement, t = b.SVGRenderer, d = b.wrap, c, n, g; c = 4 * (Math.sqrt(2) - 1) / 3 /
            (u / 2); t.prototype.toLinePath = function (a, d) { var h = []; a.forEach(function (a) { h.push("L", a.x, a.y) }); a.length && (h[0] = "M", d && h.push("Z")); return h }; t.prototype.toLineSegments = function (a) { var d = [], h = !0; a.forEach(function (a) { d.push(h ? "M" : "L", a.x, a.y); h = !h }); return d }; t.prototype.face3d = function (a) {
                var c = this, h = this.createElement("path"); h.vertexes = []; h.insidePlotArea = !1; h.enabled = !0; d(h, "attr", function (a, d) {
                    if ("object" === typeof d && (w(d.enabled) || w(d.vertexes) || w(d.insidePlotArea))) {
                        this.enabled = r(d.enabled,
                            this.enabled); this.vertexes = r(d.vertexes, this.vertexes); this.insidePlotArea = r(d.insidePlotArea, this.insidePlotArea); delete d.enabled; delete d.vertexes; delete d.insidePlotArea; var h = l(this.vertexes, x[c.chartIndex], this.insidePlotArea), m = c.toLinePath(h, !0), h = b.shapeArea(h), h = this.enabled && 0 < h ? "visible" : "hidden"; d.d = m; d.visibility = h
                    } return a.apply(this, [].slice.call(arguments, 1))
                }); d(h, "animate", function (a, d) {
                    if ("object" === typeof d && (w(d.enabled) || w(d.vertexes) || w(d.insidePlotArea))) {
                        this.enabled = r(d.enabled,
                            this.enabled); this.vertexes = r(d.vertexes, this.vertexes); this.insidePlotArea = r(d.insidePlotArea, this.insidePlotArea); delete d.enabled; delete d.vertexes; delete d.insidePlotArea; var h = l(this.vertexes, x[c.chartIndex], this.insidePlotArea), m = c.toLinePath(h, !0), h = b.shapeArea(h), h = this.enabled && 0 < h ? "visible" : "hidden"; d.d = m; this.attr("visibility", h)
                    } return a.apply(this, [].slice.call(arguments, 1))
                }); return h.attr(a)
            }; t.prototype.polyhedron = function (a) {
                var c = this, h = this.g(), m = h.destroy; this.styledMode || h.attr({ "stroke-linejoin": "round" });
                h.faces = []; h.destroy = function () { for (var a = 0; a < h.faces.length; a++)h.faces[a].destroy(); return m.call(this) }; d(h, "attr", function (a, d, m, b, g) { if ("object" === typeof d && w(d.faces)) { for (; h.faces.length > d.faces.length;)h.faces.pop().destroy(); for (; h.faces.length < d.faces.length;)h.faces.push(c.face3d().add(h)); for (var e = 0; e < d.faces.length; e++)c.styledMode && delete d.faces[e].fill, h.faces[e].attr(d.faces[e], null, b, g); delete d.faces } return a.apply(this, [].slice.call(arguments, 1)) }); d(h, "animate", function (a, d,
                    m, b) { if (d && d.faces) { for (; h.faces.length > d.faces.length;)h.faces.pop().destroy(); for (; h.faces.length < d.faces.length;)h.faces.push(c.face3d().add(h)); for (var g = 0; g < d.faces.length; g++)h.faces[g].animate(d.faces[g], m, b); delete d.faces } return a.apply(this, [].slice.call(arguments, 1)) }); return h.attr(a)
            }; n = {
                initArgs: function (a) {
                    var d = this, h = d.renderer, c = h[d.pathType + "Path"](a), m = c.zIndexes; d.parts.forEach(function (a) { d[a] = h.path(c[a]).attr({ "class": "highcharts-3d-" + a, zIndex: m[a] || 0 }).add(d) }); d.attr({
                        "stroke-linejoin": "round",
                        zIndex: m.group
                    }); d.originalDestroy = d.destroy; d.destroy = d.destroyParts
                }, singleSetterForParts: function (a, d, h, c, g, e) { var m = {}; c = [null, null, c || "attr", g, e]; var p = h && h.zIndexes; h ? (b.objectEach(h, function (d, c) { m[c] = {}; m[c][a] = d; p && (m[c].zIndex = h.zIndexes[c] || 0) }), c[1] = m) : (m[a] = d, c[0] = m); return this.processParts.apply(this, c) }, processParts: function (a, d, c, g, e) { var h = this; h.parts.forEach(function (m) { d && (a = b.pick(d[m], !1)); if (!1 !== a) h[m][c](a, g, e) }); return h }, destroyParts: function () {
                    this.processParts(null, null,
                        "destroy"); return this.originalDestroy.call(this)
                }
            }; g = b.merge(n, {
                parts: ["front", "top", "side"], pathType: "cuboid", attr: function (a, d, c, b) { if ("string" === typeof a && "undefined" !== typeof d) { var h = a; a = {}; a[h] = d } return a.shapeArgs || w(a.x) ? this.singleSetterForParts("d", null, this.renderer[this.pathType + "Path"](a.shapeArgs || a)) : k.prototype.attr.call(this, a, void 0, c, b) }, animate: function (a, d, c) {
                    w(a.x) && w(a.y) ? (a = this.renderer[this.pathType + "Path"](a), this.singleSetterForParts("d", null, a, "animate", d, c), this.attr({ zIndex: a.zIndexes.group })) :
                        a.opacity ? this.processParts(a, null, "animate", d, c) : k.prototype.animate.call(this, a, d, c); return this
                }, fillSetter: function (a) { this.singleSetterForParts("fill", null, { front: a, top: f(a).brighten(.1).get(), side: f(a).brighten(-.1).get() }); this.color = this.fill = a; return this }, opacitySetter: function (a) { return this.singleSetterForParts("opacity", a) }
            }); t.prototype.elements3d = { base: n, cuboid: g }; t.prototype.element3d = function (a, d) { var c = this.g(); b.extend(c, this.elements3d[a]); c.initArgs(d); return c }; t.prototype.cuboid =
                function (a) { return this.element3d("cuboid", a) }; b.SVGRenderer.prototype.cuboidPath = function (a) {
                    function d(a) { return f[a] } var c = a.x, g = a.y, e = a.z, p = a.height, m = a.width, k = a.depth, r = x[this.chartIndex], n, t, v = r.options.chart.options3d.alpha, q = 0, f = [{ x: c, y: g, z: e }, { x: c + m, y: g, z: e }, { x: c + m, y: g + p, z: e }, { x: c, y: g + p, z: e }, { x: c, y: g + p, z: e + k }, { x: c + m, y: g + p, z: e + k }, { x: c + m, y: g, z: e + k }, { x: c, y: g, z: e + k }], f = l(f, r, a.insidePlotArea); t = function (a, c) {
                        var h = [[], -1]; a = a.map(d); c = c.map(d); 0 > b.shapeArea(a) ? h = [a, 0] : 0 > b.shapeArea(c) && (h = [c,
                            1]); return h
                    }; n = t([3, 2, 1, 0], [7, 6, 5, 4]); a = n[0]; m = n[1]; n = t([1, 6, 7, 0], [4, 5, 2, 3]); p = n[0]; k = n[1]; n = t([1, 2, 5, 6], [0, 7, 4, 3]); t = n[0]; n = n[1]; 1 === n ? q += 1E4 * (1E3 - c) : n || (q += 1E4 * c); q += 10 * (!k || 0 <= v && 180 >= v || 360 > v && 357.5 < v ? r.plotHeight - g : 10 + g); 1 === m ? q += 100 * e : m || (q += 100 * (1E3 - e)); return { front: this.toLinePath(a, !0), top: this.toLinePath(p, !0), side: this.toLinePath(t, !0), zIndexes: { group: Math.round(q) }, isFront: m, isTop: k }
                }; b.SVGRenderer.prototype.arc3d = function (c) {
                    function g(d) {
                        var c = !1, h = {}, g; d = a(d); for (g in d) -1 !== l.indexOf(g) &&
                            (h[g] = d[g], delete d[g], c = !0); return c ? h : !1
                    } var h = this.g(), m = h.renderer, l = "x y r innerR start end".split(" "); c = a(c); c.alpha = (c.alpha || 0) * p; c.beta = (c.beta || 0) * p; h.top = m.path(); h.side1 = m.path(); h.side2 = m.path(); h.inn = m.path(); h.out = m.path(); h.onAdd = function () { var a = h.parentGroup, d = h.attr("class"); h.top.add(h);["out", "inn", "side1", "side2"].forEach(function (c) { h[c].attr({ "class": d + " highcharts-3d-side" }).add(a) }) };["addClass", "removeClass"].forEach(function (a) {
                        h[a] = function () {
                            var d = arguments;["top", "out",
                                "inn", "side1", "side2"].forEach(function (c) { h[c][a].apply(h[c], d) })
                        }
                    }); h.setPaths = function (a) { var d = h.renderer.arc3dPath(a), c = 100 * d.zTop; h.attribs = a; h.top.attr({ d: d.top, zIndex: d.zTop }); h.inn.attr({ d: d.inn, zIndex: d.zInn }); h.out.attr({ d: d.out, zIndex: d.zOut }); h.side1.attr({ d: d.side1, zIndex: d.zSide1 }); h.side2.attr({ d: d.side2, zIndex: d.zSide2 }); h.zIndex = c; h.attr({ zIndex: c }); a.center && (h.top.setRadialReference(a.center), delete a.center) }; h.setPaths(c); h.fillSetter = function (a) {
                        var d = f(a).brighten(-.1).get();
                        this.fill = a; this.side1.attr({ fill: d }); this.side2.attr({ fill: d }); this.inn.attr({ fill: d }); this.out.attr({ fill: d }); this.top.attr({ fill: a }); return this
                    };["opacity", "translateX", "translateY", "visibility"].forEach(function (a) { h[a + "Setter"] = function (a, d) { h[d] = a;["out", "inn", "side1", "side2", "top"].forEach(function (c) { h[c].attr(d, a) }) } }); d(h, "attr", function (a, d) { var c; "object" === typeof d && (c = g(d)) && (e(h.attribs, c), h.setPaths(h.attribs)); return a.apply(this, [].slice.call(arguments, 1)) }); d(h, "animate", function (d,
                        c, e, p) { var m, k = this.attribs, l, n = "data-" + Math.random().toString(26).substring(2, 9); delete c.center; delete c.z; delete c.depth; delete c.alpha; delete c.beta; l = v(r(e, this.renderer.globalAnimation)); l.duration && (m = g(c), h[n] = 0, c[n] = 1, h[n + "Setter"] = b.noop, m && (l.step = function (d, c) { function h(a) { return k[a] + (r(m[a], k[a]) - k[a]) * c.pos } c.prop === n && c.elem.setPaths(a(k, { x: h("x"), y: h("y"), r: h("r"), innerR: h("innerR"), start: h("start"), end: h("end") })) }), e = l); return d.call(this, c, e, p) }); h.destroy = function () {
                            this.top.destroy();
                            this.out.destroy(); this.inn.destroy(); this.side1.destroy(); this.side2.destroy(); k.prototype.destroy.call(this)
                        }; h.hide = function () { this.top.hide(); this.out.hide(); this.inn.hide(); this.side1.hide(); this.side2.hide() }; h.show = function (a) { this.top.show(a); this.out.show(a); this.inn.show(a); this.side1.show(a); this.side2.show(a) }; return h
                }; t.prototype.arc3dPath = function (a) {
                    function d(a) { a %= 2 * Math.PI; a > Math.PI && (a = 2 * Math.PI - a); return a } var c = a.x, g = a.y, b = a.start, e = a.end - .00001, p = a.r, k = a.innerR || 0, l = a.depth ||
                        0, m = a.alpha, n = a.beta, r = Math.cos(b), t = Math.sin(b); a = Math.cos(e); var v = Math.sin(e), f = p * Math.cos(n), p = p * Math.cos(m), x = k * Math.cos(n), w = k * Math.cos(m), k = l * Math.sin(n), B = l * Math.sin(m), l = ["M", c + f * r, g + p * t], l = l.concat(y(c, g, f, p, b, e, 0, 0)), l = l.concat(["L", c + x * a, g + w * v]), l = l.concat(y(c, g, x, w, e, b, 0, 0)), l = l.concat(["Z"]), A = 0 < n ? Math.PI / 2 : 0, n = 0 < m ? 0 : Math.PI / 2, A = b > -A ? b : e > -A ? -A : b, D = e < u - n ? e : b < u - n ? u - n : e, E = 2 * u - n, m = ["M", c + f * z(A), g + p * q(A)], m = m.concat(y(c, g, f, p, A, D, 0, 0)); e > E && b < E ? (m = m.concat(["L", c + f * z(D) + k, g + p * q(D) + B]), m =
                            m.concat(y(c, g, f, p, D, E, k, B)), m = m.concat(["L", c + f * z(E), g + p * q(E)]), m = m.concat(y(c, g, f, p, E, e, 0, 0)), m = m.concat(["L", c + f * z(e) + k, g + p * q(e) + B]), m = m.concat(y(c, g, f, p, e, E, k, B)), m = m.concat(["L", c + f * z(E), g + p * q(E)]), m = m.concat(y(c, g, f, p, E, D, 0, 0))) : e > u - n && b < u - n && (m = m.concat(["L", c + f * Math.cos(D) + k, g + p * Math.sin(D) + B]), m = m.concat(y(c, g, f, p, D, e, k, B)), m = m.concat(["L", c + f * Math.cos(e), g + p * Math.sin(e)]), m = m.concat(y(c, g, f, p, e, D, 0, 0))); m = m.concat(["L", c + f * Math.cos(D) + k, g + p * Math.sin(D) + B]); m = m.concat(y(c, g, f, p, D, A, k, B));
                    m = m.concat(["Z"]); n = ["M", c + x * r, g + w * t]; n = n.concat(y(c, g, x, w, b, e, 0, 0)); n = n.concat(["L", c + x * Math.cos(e) + k, g + w * Math.sin(e) + B]); n = n.concat(y(c, g, x, w, e, b, k, B)); n = n.concat(["Z"]); r = ["M", c + f * r, g + p * t, "L", c + f * r + k, g + p * t + B, "L", c + x * r + k, g + w * t + B, "L", c + x * r, g + w * t, "Z"]; c = ["M", c + f * a, g + p * v, "L", c + f * a + k, g + p * v + B, "L", c + x * a + k, g + w * v + B, "L", c + x * a, g + w * v, "Z"]; v = Math.atan2(B, -k); g = Math.abs(e + v); a = Math.abs(b + v); b = Math.abs((b + e) / 2 + v); g = d(g); a = d(a); b = d(b); b *= 1E5; e = 1E5 * a; g *= 1E5; return {
                        top: l, zTop: 1E5 * Math.PI + 1, out: m, zOut: Math.max(b,
                            e, g), inn: n, zInn: Math.max(b, e, g), side1: r, zSide1: .99 * g, side2: c, zSide2: .99 * e
                    }
                }
    })(A); (function (b) {
        function y(b, e) {
            var a = b.plotLeft, p = b.plotWidth + a, r = b.plotTop, k = b.plotHeight + r, t = a + b.plotWidth / 2, d = r + b.plotHeight / 2, c = Number.MAX_VALUE, n = -Number.MAX_VALUE, g = Number.MAX_VALUE, m = -Number.MAX_VALUE, f, h = 1; f = [{ x: a, y: r, z: 0 }, { x: a, y: r, z: e }];[0, 1].forEach(function (a) { f.push({ x: p, y: f[a].y, z: f[a].z }) });[0, 1, 2, 3].forEach(function (a) { f.push({ x: f[a].x, y: k, z: f[a].z }) }); f = v(f, b, !1); f.forEach(function (a) {
                c = Math.min(c, a.x);
                n = Math.max(n, a.x); g = Math.min(g, a.y); m = Math.max(m, a.y)
            }); a > c && (h = Math.min(h, 1 - Math.abs((a + t) / (c + t)) % 1)); p < n && (h = Math.min(h, (p - t) / (n - t))); r > g && (h = 0 > g ? Math.min(h, (r + d) / (-g + r + d)) : Math.min(h, 1 - (r + d) / (g + d) % 1)); k < m && (h = Math.min(h, Math.abs((k - d) / (m - d)))); return h
        } var z = b.addEvent, u = b.Chart, q = b.merge, v = b.perspective, x = b.pick, f = b.wrap; u.prototype.is3d = function () { return this.options.chart.options3d && this.options.chart.options3d.enabled }; u.prototype.propsRequireDirtyBox.push("chart.options3d"); u.prototype.propsRequireUpdateSeries.push("chart.options3d");
        z(u, "afterInit", function () { var b = this.options; this.is3d() && (b.series || []).forEach(function (e) { "scatter" === (e.type || b.chart.type || b.chart.defaultSeriesType) && (e.type = "scatter3d") }) }); z(u, "addSeries", function (b) { this.is3d() && "scatter" === b.options.type && (b.options.type = "scatter3d") }); b.wrap(b.Chart.prototype, "isInsidePlot", function (b) { return this.is3d() || b.apply(this, [].slice.call(arguments, 1)) }); var w = b.getOptions(); q(!0, w, {
            chart: {
                options3d: {
                    enabled: !1, alpha: 0, beta: 0, depth: 100, fitToPlot: !0, viewDistance: 25,
                    axisLabelPosition: null, frame: { visible: "default", size: 1, bottom: {}, top: {}, left: {}, right: {}, back: {}, front: {} }
                }
            }
        }); z(u, "afterGetContainer", function () {
            this.styledMode && (this.renderer.definition({ tagName: "style", textContent: ".highcharts-3d-top{filter: url(#highcharts-brighter)}\n.highcharts-3d-side{filter: url(#highcharts-darker)}\n" }), [{ name: "darker", slope: .6 }, { name: "brighter", slope: 1.4 }].forEach(function (b) {
                this.renderer.definition({
                    tagName: "filter", id: "highcharts-" + b.name, children: [{
                        tagName: "feComponentTransfer",
                        children: [{ tagName: "feFuncR", type: "linear", slope: b.slope }, { tagName: "feFuncG", type: "linear", slope: b.slope }, { tagName: "feFuncB", type: "linear", slope: b.slope }]
                    }]
                })
            }, this))
        }); f(u.prototype, "setClassName", function (b) { b.apply(this, [].slice.call(arguments, 1)); this.is3d() && (this.container.className += " highcharts-3d-chart") }); z(b.Chart, "afterSetChartSize", function () {
            var b = this.options.chart.options3d; if (this.is3d()) {
                var e = this.inverted, a = this.clipBox, l = this.margin; a[e ? "y" : "x"] = -(l[3] || 0); a[e ? "x" : "y"] = -(l[0] ||
                    0); a[e ? "height" : "width"] = this.chartWidth + (l[3] || 0) + (l[1] || 0); a[e ? "width" : "height"] = this.chartHeight + (l[0] || 0) + (l[2] || 0); this.scale3d = 1; !0 === b.fitToPlot && (this.scale3d = y(this, b.depth)); this.frame3d = this.get3dFrame()
            }
        }); z(u, "beforeRedraw", function () { this.is3d() && (this.isDirtyBox = !0) }); z(u, "beforeRender", function () { this.is3d() && (this.frame3d = this.get3dFrame()) }); f(u.prototype, "renderSeries", function (b) { var e = this.series.length; if (this.is3d()) for (; e--;)b = this.series[e], b.translate(), b.render(); else b.call(this) });
        z(u, "afterDrawChartBox", function () {
            if (this.is3d()) {
                var p = this.renderer, e = this.options.chart.options3d, a = this.get3dFrame(), l = this.plotLeft, r = this.plotLeft + this.plotWidth, k = this.plotTop, t = this.plotTop + this.plotHeight, e = e.depth, d = l - (a.left.visible ? a.left.size : 0), c = r + (a.right.visible ? a.right.size : 0), n = k - (a.top.visible ? a.top.size : 0), g = t + (a.bottom.visible ? a.bottom.size : 0), m = 0 - (a.front.visible ? a.front.size : 0), f = e + (a.back.visible ? a.back.size : 0), h = this.hasRendered ? "animate" : "attr"; this.frame3d = a; this.frameShapes ||
                    (this.frameShapes = { bottom: p.polyhedron().add(), top: p.polyhedron().add(), left: p.polyhedron().add(), right: p.polyhedron().add(), back: p.polyhedron().add(), front: p.polyhedron().add() }); this.frameShapes.bottom[h]({
                        "class": "highcharts-3d-frame highcharts-3d-frame-bottom", zIndex: a.bottom.frontFacing ? -1E3 : 1E3, faces: [{ fill: b.color(a.bottom.color).brighten(.1).get(), vertexes: [{ x: d, y: g, z: m }, { x: c, y: g, z: m }, { x: c, y: g, z: f }, { x: d, y: g, z: f }], enabled: a.bottom.visible }, {
                            fill: b.color(a.bottom.color).brighten(.1).get(), vertexes: [{
                                x: l,
                                y: t, z: e
                            }, { x: r, y: t, z: e }, { x: r, y: t, z: 0 }, { x: l, y: t, z: 0 }], enabled: a.bottom.visible
                        }, { fill: b.color(a.bottom.color).brighten(-.1).get(), vertexes: [{ x: d, y: g, z: m }, { x: d, y: g, z: f }, { x: l, y: t, z: e }, { x: l, y: t, z: 0 }], enabled: a.bottom.visible && !a.left.visible }, { fill: b.color(a.bottom.color).brighten(-.1).get(), vertexes: [{ x: c, y: g, z: f }, { x: c, y: g, z: m }, { x: r, y: t, z: 0 }, { x: r, y: t, z: e }], enabled: a.bottom.visible && !a.right.visible }, {
                            fill: b.color(a.bottom.color).get(), vertexes: [{ x: c, y: g, z: m }, { x: d, y: g, z: m }, { x: l, y: t, z: 0 }, { x: r, y: t, z: 0 }],
                            enabled: a.bottom.visible && !a.front.visible
                        }, { fill: b.color(a.bottom.color).get(), vertexes: [{ x: d, y: g, z: f }, { x: c, y: g, z: f }, { x: r, y: t, z: e }, { x: l, y: t, z: e }], enabled: a.bottom.visible && !a.back.visible }]
                    }); this.frameShapes.top[h]({
                        "class": "highcharts-3d-frame highcharts-3d-frame-top", zIndex: a.top.frontFacing ? -1E3 : 1E3, faces: [{ fill: b.color(a.top.color).brighten(.1).get(), vertexes: [{ x: d, y: n, z: f }, { x: c, y: n, z: f }, { x: c, y: n, z: m }, { x: d, y: n, z: m }], enabled: a.top.visible }, {
                            fill: b.color(a.top.color).brighten(.1).get(), vertexes: [{
                                x: l,
                                y: k, z: 0
                            }, { x: r, y: k, z: 0 }, { x: r, y: k, z: e }, { x: l, y: k, z: e }], enabled: a.top.visible
                        }, { fill: b.color(a.top.color).brighten(-.1).get(), vertexes: [{ x: d, y: n, z: f }, { x: d, y: n, z: m }, { x: l, y: k, z: 0 }, { x: l, y: k, z: e }], enabled: a.top.visible && !a.left.visible }, { fill: b.color(a.top.color).brighten(-.1).get(), vertexes: [{ x: c, y: n, z: m }, { x: c, y: n, z: f }, { x: r, y: k, z: e }, { x: r, y: k, z: 0 }], enabled: a.top.visible && !a.right.visible }, {
                            fill: b.color(a.top.color).get(), vertexes: [{ x: d, y: n, z: m }, { x: c, y: n, z: m }, { x: r, y: k, z: 0 }, { x: l, y: k, z: 0 }], enabled: a.top.visible &&
                                !a.front.visible
                        }, { fill: b.color(a.top.color).get(), vertexes: [{ x: c, y: n, z: f }, { x: d, y: n, z: f }, { x: l, y: k, z: e }, { x: r, y: k, z: e }], enabled: a.top.visible && !a.back.visible }]
                    }); this.frameShapes.left[h]({
                        "class": "highcharts-3d-frame highcharts-3d-frame-left", zIndex: a.left.frontFacing ? -1E3 : 1E3, faces: [{ fill: b.color(a.left.color).brighten(.1).get(), vertexes: [{ x: d, y: g, z: m }, { x: l, y: t, z: 0 }, { x: l, y: t, z: e }, { x: d, y: g, z: f }], enabled: a.left.visible && !a.bottom.visible }, {
                            fill: b.color(a.left.color).brighten(.1).get(), vertexes: [{
                                x: d,
                                y: n, z: f
                            }, { x: l, y: k, z: e }, { x: l, y: k, z: 0 }, { x: d, y: n, z: m }], enabled: a.left.visible && !a.top.visible
                        }, { fill: b.color(a.left.color).brighten(-.1).get(), vertexes: [{ x: d, y: g, z: f }, { x: d, y: n, z: f }, { x: d, y: n, z: m }, { x: d, y: g, z: m }], enabled: a.left.visible }, { fill: b.color(a.left.color).brighten(-.1).get(), vertexes: [{ x: l, y: k, z: e }, { x: l, y: t, z: e }, { x: l, y: t, z: 0 }, { x: l, y: k, z: 0 }], enabled: a.left.visible }, { fill: b.color(a.left.color).get(), vertexes: [{ x: d, y: g, z: m }, { x: d, y: n, z: m }, { x: l, y: k, z: 0 }, { x: l, y: t, z: 0 }], enabled: a.left.visible && !a.front.visible },
                        { fill: b.color(a.left.color).get(), vertexes: [{ x: d, y: n, z: f }, { x: d, y: g, z: f }, { x: l, y: t, z: e }, { x: l, y: k, z: e }], enabled: a.left.visible && !a.back.visible }]
                    }); this.frameShapes.right[h]({
                        "class": "highcharts-3d-frame highcharts-3d-frame-right", zIndex: a.right.frontFacing ? -1E3 : 1E3, faces: [{ fill: b.color(a.right.color).brighten(.1).get(), vertexes: [{ x: c, y: g, z: f }, { x: r, y: t, z: e }, { x: r, y: t, z: 0 }, { x: c, y: g, z: m }], enabled: a.right.visible && !a.bottom.visible }, {
                            fill: b.color(a.right.color).brighten(.1).get(), vertexes: [{ x: c, y: n, z: m },
                            { x: r, y: k, z: 0 }, { x: r, y: k, z: e }, { x: c, y: n, z: f }], enabled: a.right.visible && !a.top.visible
                        }, { fill: b.color(a.right.color).brighten(-.1).get(), vertexes: [{ x: r, y: k, z: 0 }, { x: r, y: t, z: 0 }, { x: r, y: t, z: e }, { x: r, y: k, z: e }], enabled: a.right.visible }, { fill: b.color(a.right.color).brighten(-.1).get(), vertexes: [{ x: c, y: g, z: m }, { x: c, y: n, z: m }, { x: c, y: n, z: f }, { x: c, y: g, z: f }], enabled: a.right.visible }, { fill: b.color(a.right.color).get(), vertexes: [{ x: c, y: n, z: m }, { x: c, y: g, z: m }, { x: r, y: t, z: 0 }, { x: r, y: k, z: 0 }], enabled: a.right.visible && !a.front.visible },
                        { fill: b.color(a.right.color).get(), vertexes: [{ x: c, y: g, z: f }, { x: c, y: n, z: f }, { x: r, y: k, z: e }, { x: r, y: t, z: e }], enabled: a.right.visible && !a.back.visible }]
                    }); this.frameShapes.back[h]({
                        "class": "highcharts-3d-frame highcharts-3d-frame-back", zIndex: a.back.frontFacing ? -1E3 : 1E3, faces: [{ fill: b.color(a.back.color).brighten(.1).get(), vertexes: [{ x: c, y: g, z: f }, { x: d, y: g, z: f }, { x: l, y: t, z: e }, { x: r, y: t, z: e }], enabled: a.back.visible && !a.bottom.visible }, {
                            fill: b.color(a.back.color).brighten(.1).get(), vertexes: [{ x: d, y: n, z: f }, {
                                x: c,
                                y: n, z: f
                            }, { x: r, y: k, z: e }, { x: l, y: k, z: e }], enabled: a.back.visible && !a.top.visible
                        }, { fill: b.color(a.back.color).brighten(-.1).get(), vertexes: [{ x: d, y: g, z: f }, { x: d, y: n, z: f }, { x: l, y: k, z: e }, { x: l, y: t, z: e }], enabled: a.back.visible && !a.left.visible }, { fill: b.color(a.back.color).brighten(-.1).get(), vertexes: [{ x: c, y: n, z: f }, { x: c, y: g, z: f }, { x: r, y: t, z: e }, { x: r, y: k, z: e }], enabled: a.back.visible && !a.right.visible }, { fill: b.color(a.back.color).get(), vertexes: [{ x: l, y: k, z: e }, { x: r, y: k, z: e }, { x: r, y: t, z: e }, { x: l, y: t, z: e }], enabled: a.back.visible },
                        { fill: b.color(a.back.color).get(), vertexes: [{ x: d, y: g, z: f }, { x: c, y: g, z: f }, { x: c, y: n, z: f }, { x: d, y: n, z: f }], enabled: a.back.visible }]
                    }); this.frameShapes.front[h]({
                        "class": "highcharts-3d-frame highcharts-3d-frame-front", zIndex: a.front.frontFacing ? -1E3 : 1E3, faces: [{ fill: b.color(a.front.color).brighten(.1).get(), vertexes: [{ x: d, y: g, z: m }, { x: c, y: g, z: m }, { x: r, y: t, z: 0 }, { x: l, y: t, z: 0 }], enabled: a.front.visible && !a.bottom.visible }, {
                            fill: b.color(a.front.color).brighten(.1).get(), vertexes: [{ x: c, y: n, z: m }, { x: d, y: n, z: m }, {
                                x: l,
                                y: k, z: 0
                            }, { x: r, y: k, z: 0 }], enabled: a.front.visible && !a.top.visible
                        }, { fill: b.color(a.front.color).brighten(-.1).get(), vertexes: [{ x: d, y: n, z: m }, { x: d, y: g, z: m }, { x: l, y: t, z: 0 }, { x: l, y: k, z: 0 }], enabled: a.front.visible && !a.left.visible }, { fill: b.color(a.front.color).brighten(-.1).get(), vertexes: [{ x: c, y: g, z: m }, { x: c, y: n, z: m }, { x: r, y: k, z: 0 }, { x: r, y: t, z: 0 }], enabled: a.front.visible && !a.right.visible }, { fill: b.color(a.front.color).get(), vertexes: [{ x: r, y: k, z: 0 }, { x: l, y: k, z: 0 }, { x: l, y: t, z: 0 }, { x: r, y: t, z: 0 }], enabled: a.front.visible },
                        { fill: b.color(a.front.color).get(), vertexes: [{ x: c, y: g, z: m }, { x: d, y: g, z: m }, { x: d, y: n, z: m }, { x: c, y: n, z: m }], enabled: a.front.visible }]
                    })
            }
        }); u.prototype.retrieveStacks = function (b) { var e = this.series, a = {}, l, p = 1; this.series.forEach(function (k) { l = x(k.options.stack, b ? 0 : e.length - 1 - k.index); a[l] ? a[l].series.push(k) : (a[l] = { series: [k], position: p }, p++) }); a.totalStacks = p + 1; return a }; u.prototype.get3dFrame = function () {
            var p = this, e = p.options.chart.options3d, a = e.frame, l = p.plotLeft, f = p.plotLeft + p.plotWidth, k = p.plotTop, t =
                p.plotTop + p.plotHeight, d = e.depth, c = function (a) { a = b.shapeArea3d(a, p); return .5 < a ? 1 : -.5 > a ? -1 : 0 }, n = c([{ x: l, y: t, z: d }, { x: f, y: t, z: d }, { x: f, y: t, z: 0 }, { x: l, y: t, z: 0 }]), g = c([{ x: l, y: k, z: 0 }, { x: f, y: k, z: 0 }, { x: f, y: k, z: d }, { x: l, y: k, z: d }]), m = c([{ x: l, y: k, z: 0 }, { x: l, y: k, z: d }, { x: l, y: t, z: d }, { x: l, y: t, z: 0 }]), q = c([{ x: f, y: k, z: d }, { x: f, y: k, z: 0 }, { x: f, y: t, z: 0 }, { x: f, y: t, z: d }]), h = c([{ x: l, y: t, z: 0 }, { x: f, y: t, z: 0 }, { x: f, y: k, z: 0 }, { x: l, y: k, z: 0 }]), c = c([{ x: l, y: k, z: d }, { x: f, y: k, z: d }, { x: f, y: t, z: d }, { x: l, y: t, z: d }]), w = !1, u = !1, y = !1, z = !1;[].concat(p.xAxis,
                    p.yAxis, p.zAxis).forEach(function (a) { a && (a.horiz ? a.opposite ? u = !0 : w = !0 : a.opposite ? z = !0 : y = !0) }); var A = function (a, c, d) { for (var b = ["size", "color", "visible"], g = {}, e = 0; e < b.length; e++)for (var m = b[e], h = 0; h < a.length; h++)if ("object" === typeof a[h]) { var f = a[h][m]; if (void 0 !== f && null !== f) { g[m] = f; break } } a = d; !0 === g.visible || !1 === g.visible ? a = g.visible : "auto" === g.visible && (a = 0 < c); return { size: x(g.size, 1), color: x(g.color, "none"), frontFacing: 0 < c, visible: a } }, a = {
                        bottom: A([a.bottom, a.top, a], n, w), top: A([a.top, a.bottom, a],
                            g, u), left: A([a.left, a.right, a.side, a], m, y), right: A([a.right, a.left, a.side, a], q, z), back: A([a.back, a.front, a], c, !0), front: A([a.front, a.back, a], h, !1)
                    }; "auto" === e.axisLabelPosition ? (q = function (a, c) { return a.visible !== c.visible || a.visible && c.visible && a.frontFacing !== c.frontFacing }, e = [], q(a.left, a.front) && e.push({ y: (k + t) / 2, x: l, z: 0, xDir: { x: 1, y: 0, z: 0 } }), q(a.left, a.back) && e.push({ y: (k + t) / 2, x: l, z: d, xDir: { x: 0, y: 0, z: -1 } }), q(a.right, a.front) && e.push({ y: (k + t) / 2, x: f, z: 0, xDir: { x: 0, y: 0, z: 1 } }), q(a.right, a.back) && e.push({
                        y: (k +
                            t) / 2, x: f, z: d, xDir: { x: -1, y: 0, z: 0 }
                    }), n = [], q(a.bottom, a.front) && n.push({ x: (l + f) / 2, y: t, z: 0, xDir: { x: 1, y: 0, z: 0 } }), q(a.bottom, a.back) && n.push({ x: (l + f) / 2, y: t, z: d, xDir: { x: -1, y: 0, z: 0 } }), g = [], q(a.top, a.front) && g.push({ x: (l + f) / 2, y: k, z: 0, xDir: { x: 1, y: 0, z: 0 } }), q(a.top, a.back) && g.push({ x: (l + f) / 2, y: k, z: d, xDir: { x: -1, y: 0, z: 0 } }), m = [], q(a.bottom, a.left) && m.push({ z: (0 + d) / 2, y: t, x: l, xDir: { x: 0, y: 0, z: -1 } }), q(a.bottom, a.right) && m.push({ z: (0 + d) / 2, y: t, x: f, xDir: { x: 0, y: 0, z: 1 } }), t = [], q(a.top, a.left) && t.push({
                        z: (0 + d) / 2, y: k, x: l, xDir: {
                            x: 0,
                            y: 0, z: -1
                        }
                    }), q(a.top, a.right) && t.push({ z: (0 + d) / 2, y: k, x: f, xDir: { x: 0, y: 0, z: 1 } }), l = function (a, c, d) { if (0 === a.length) return null; if (1 === a.length) return a[0]; for (var b = 0, g = v(a, p, !1), e = 1; e < g.length; e++)d * g[e][c] > d * g[b][c] ? b = e : d * g[e][c] === d * g[b][c] && g[e].z < g[b].z && (b = e); return a[b] }, a.axes = { y: { left: l(e, "x", -1), right: l(e, "x", 1) }, x: { top: l(g, "y", -1), bottom: l(n, "y", 1) }, z: { top: l(t, "y", -1), bottom: l(m, "y", 1) } }) : a.axes = {
                        y: { left: { x: l, z: 0, xDir: { x: 1, y: 0, z: 0 } }, right: { x: f, z: 0, xDir: { x: 0, y: 0, z: 1 } } }, x: {
                            top: {
                                y: k, z: 0, xDir: {
                                    x: 1,
                                    y: 0, z: 0
                                }
                            }, bottom: { y: t, z: 0, xDir: { x: 1, y: 0, z: 0 } }
                        }, z: { top: { x: y ? f : l, y: k, xDir: y ? { x: 0, y: 0, z: 1 } : { x: 0, y: 0, z: -1 } }, bottom: { x: y ? f : l, y: t, xDir: y ? { x: 0, y: 0, z: 1 } : { x: 0, y: 0, z: -1 } } }
                    }; return a
        }; b.Fx.prototype.matrixSetter = function () { var f; if (1 > this.pos && (b.isArray(this.start) || b.isArray(this.end))) { var e = this.start || [1, 0, 0, 1, 0, 0], a = this.end || [1, 0, 0, 1, 0, 0]; f = []; for (var l = 0; 6 > l; l++)f.push(this.pos * a[l] + (1 - this.pos) * e[l]) } else f = this.end; this.elem.attr(this.prop, f, null, !0) }
    })(A); (function (b) {
        function y(d, c, b) {
            if (!d.chart.is3d() ||
                "colorAxis" === d.coll) return c; var g = d.chart, e = x * g.options.chart.options3d.alpha, f = x * g.options.chart.options3d.beta, h = a(b && d.options.title.position3d, d.options.labels.position3d); b = a(b && d.options.title.skew3d, d.options.labels.skew3d); var k = g.frame3d, n = g.plotLeft, t = g.plotWidth + n, r = g.plotTop, q = g.plotHeight + r, g = !1, v = 0, w = 0, u = { x: 0, y: 1, z: 0 }; c = d.swapZ({ x: c.x, y: c.y, z: 0 }); if (d.isZAxis) if (d.opposite) { if (null === k.axes.z.top) return {}; w = c.y - r; c.x = k.axes.z.top.x; c.y = k.axes.z.top.y; n = k.axes.z.top.xDir; g = !k.top.frontFacing } else {
                    if (null ===
                        k.axes.z.bottom) return {}; w = c.y - q; c.x = k.axes.z.bottom.x; c.y = k.axes.z.bottom.y; n = k.axes.z.bottom.xDir; g = !k.bottom.frontFacing
                } else if (d.horiz) if (d.opposite) { if (null === k.axes.x.top) return {}; w = c.y - r; c.y = k.axes.x.top.y; c.z = k.axes.x.top.z; n = k.axes.x.top.xDir; g = !k.top.frontFacing } else { if (null === k.axes.x.bottom) return {}; w = c.y - q; c.y = k.axes.x.bottom.y; c.z = k.axes.x.bottom.z; n = k.axes.x.bottom.xDir; g = !k.bottom.frontFacing } else if (d.opposite) {
                    if (null === k.axes.y.right) return {}; v = c.x - t; c.x = k.axes.y.right.x; c.z = k.axes.y.right.z;
                    n = k.axes.y.right.xDir; n = { x: n.z, y: n.y, z: -n.x }
                } else { if (null === k.axes.y.left) return {}; v = c.x - n; c.x = k.axes.y.left.x; c.z = k.axes.y.left.z; n = k.axes.y.left.xDir } "chart" !== h && ("flap" === h ? d.horiz ? (f = Math.sin(e), e = Math.cos(e), d.opposite && (f = -f), g && (f = -f), u = { x: n.z * f, y: e, z: -n.x * f }) : n = { x: Math.cos(f), y: 0, z: Math.sin(f) } : "ortho" === h ? d.horiz ? (u = Math.cos(e), h = Math.sin(f) * u, e = -Math.sin(e), f = -u * Math.cos(f), u = { x: n.y * f - n.z * e, y: n.z * h - n.x * f, z: n.x * e - n.y * h }, e = 1 / Math.sqrt(u.x * u.x + u.y * u.y + u.z * u.z), g && (e = -e), u = {
                    x: e * u.x, y: e * u.y,
                    z: e * u.z
                }) : n = { x: Math.cos(f), y: 0, z: Math.sin(f) } : d.horiz ? u = { x: Math.sin(f) * Math.sin(e), y: Math.cos(e), z: -Math.cos(f) * Math.sin(e) } : n = { x: Math.cos(f), y: 0, z: Math.sin(f) }); c.x += v * n.x + w * u.x; c.y += v * n.y + w * u.y; c.z += v * n.z + w * u.z; g = p([c], d.chart)[0]; b && (0 > l(p([c, { x: c.x + n.x, y: c.y + n.y, z: c.z + n.z }, { x: c.x + u.x, y: c.y + u.y, z: c.z + u.z }], d.chart)) && (n = { x: -n.x, y: -n.y, z: -n.z }), d = p([{ x: c.x, y: c.y, z: c.z }, { x: c.x + n.x, y: c.y + n.y, z: c.z + n.z }, { x: c.x + u.x, y: c.y + u.y, z: c.z + u.z }], d.chart), g.matrix = [d[1].x - d[0].x, d[1].y - d[0].y, d[2].x - d[0].x, d[2].y -
                    d[0].y, g.x, g.y], g.matrix[4] -= g.x * g.matrix[0] + g.y * g.matrix[2], g.matrix[5] -= g.x * g.matrix[1] + g.y * g.matrix[3]); return g
        } var z, u = b.addEvent, q = b.Axis, v = b.Chart, x = b.deg2rad, f = b.extend, w = b.merge, p = b.perspective, e = b.perspective3D, a = b.pick, l = b.shapeArea, r = b.splat, k = b.Tick, t = b.wrap; w(!0, q.prototype.defaultOptions, { labels: { position3d: "offset", skew3d: !1 }, title: { position3d: null, skew3d: null } }); u(q, "afterSetOptions", function () {
            var d; this.chart.is3d && this.chart.is3d() && "colorAxis" !== this.coll && (d = this.options, d.tickWidth =
                a(d.tickWidth, 0), d.gridLineWidth = a(d.gridLineWidth, 1))
        }); t(q.prototype, "getPlotLinePath", function (a) {
            var c = a.apply(this, [].slice.call(arguments, 1)); if (!this.chart.is3d() || "colorAxis" === this.coll || null === c) return c; var d = this.chart, b = d.options.chart.options3d, b = this.isZAxis ? d.plotWidth : b.depth, d = d.frame3d, c = [this.swapZ({ x: c[1], y: c[2], z: 0 }), this.swapZ({ x: c[1], y: c[2], z: b }), this.swapZ({ x: c[4], y: c[5], z: 0 }), this.swapZ({ x: c[4], y: c[5], z: b })], b = []; this.horiz ? (this.isZAxis ? (d.left.visible && b.push(c[0], c[2]),
                d.right.visible && b.push(c[1], c[3])) : (d.front.visible && b.push(c[0], c[2]), d.back.visible && b.push(c[1], c[3])), d.top.visible && b.push(c[0], c[1]), d.bottom.visible && b.push(c[2], c[3])) : (d.front.visible && b.push(c[0], c[2]), d.back.visible && b.push(c[1], c[3]), d.left.visible && b.push(c[0], c[1]), d.right.visible && b.push(c[2], c[3])); b = p(b, this.chart, !1); return this.chart.renderer.toLineSegments(b)
        }); t(q.prototype, "getLinePath", function (a) {
            return this.chart.is3d() && "colorAxis" !== this.coll ? [] : a.apply(this, [].slice.call(arguments,
                1))
        }); t(q.prototype, "getPlotBandPath", function (a) { if (!this.chart.is3d() || "colorAxis" === this.coll) return a.apply(this, [].slice.call(arguments, 1)); var d = arguments, b = d[2], e = [], d = this.getPlotLinePath(d[1]), b = this.getPlotLinePath(b); if (d && b) for (var f = 0; f < d.length; f += 6)e.push("M", d[f + 1], d[f + 2], "L", d[f + 4], d[f + 5], "L", b[f + 4], b[f + 5], "L", b[f + 1], b[f + 2], "Z"); return e }); t(k.prototype, "getMarkPath", function (a) {
            var d = a.apply(this, [].slice.call(arguments, 1)), d = [y(this.axis, { x: d[1], y: d[2], z: 0 }), y(this.axis, {
                x: d[4],
                y: d[5], z: 0
            })]; return this.axis.chart.renderer.toLineSegments(d)
        }); u(k, "afterGetLabelPosition", function (a) { f(a.pos, y(this.axis, a.pos)) }); t(q.prototype, "getTitlePosition", function (a) { var d = a.apply(this, [].slice.call(arguments, 1)); return y(this, d, !0) }); u(q, "drawCrosshair", function (a) { this.chart.is3d() && "colorAxis" !== this.coll && a.point && (a.point.crosshairPos = this.isXAxis ? a.point.axisXpos : this.len - a.point.axisYpos) }); u(q, "destroy", function () {
            ["backFrame", "bottomFrame", "sideFrame"].forEach(function (a) {
                this[a] &&
                    (this[a] = this[a].destroy())
            }, this)
        }); q.prototype.swapZ = function (a, c) { return this.isZAxis ? (c = c ? 0 : this.chart.plotLeft, { x: c + a.z, y: a.y, z: a.x - c }) : a }; z = b.ZAxis = function () { this.init.apply(this, arguments) }; f(z.prototype, q.prototype); f(z.prototype, {
            isZAxis: !0, setOptions: function (a) { a = w({ offset: 0, lineWidth: 0 }, a); q.prototype.setOptions.call(this, a); this.coll = "zAxis" }, setAxisSize: function () {
                q.prototype.setAxisSize.call(this); this.width = this.len = this.chart.options.chart.options3d.depth; this.right = this.chart.chartWidth -
                    this.width - this.left
            }, getSeriesExtremes: function () { var d = this, c = d.chart; d.hasVisibleSeries = !1; d.dataMin = d.dataMax = d.ignoreMinPadding = d.ignoreMaxPadding = null; d.buildStacks && d.buildStacks(); d.series.forEach(function (b) { if (b.visible || !c.options.chart.ignoreHiddenSeries) d.hasVisibleSeries = !0, b = b.zData, b.length && (d.dataMin = Math.min(a(d.dataMin, b[0]), Math.min.apply(null, b)), d.dataMax = Math.max(a(d.dataMax, b[0]), Math.max.apply(null, b))) }) }
        }); u(v, "afterGetAxes", function () {
            var a = this, c = this.options, c = c.zAxis =
                r(c.zAxis || {}); a.is3d() && (this.zAxis = [], c.forEach(function (d, c) { d.index = c; d.isX = !0; (new z(a, d)).setScale() }))
        }); t(q.prototype, "getSlotWidth", function (d, c) {
            if (this.chart.is3d() && c && c.label && this.categories && this.chart.frameShapes) {
                var b = this.chart, g = this.ticks, f = this.gridGroup.element.childNodes[0].getBBox(), k = b.frameShapes.left.getBBox(), h = b.options.chart.options3d, b = { x: b.plotWidth / 2, y: b.plotHeight / 2, z: h.depth / 2, vd: a(h.depth, 1) * a(h.viewDistance, 0) }, l, p, h = c.pos, r = g[h - 1], g = g[h + 1]; 0 !== h && r && r.label.xy &&
                    (l = e({ x: r.label.xy.x, y: r.label.xy.y, z: null }, b, b.vd)); g && g.label.xy && (p = e({ x: g.label.xy.x, y: g.label.xy.y, z: null }, b, b.vd)); g = { x: c.label.xy.x, y: c.label.xy.y, z: null }; g = e(g, b, b.vd); return Math.abs(l ? g.x - l.x : p ? p.x - g.x : f.x - k.x)
            } return d.apply(this, [].slice.call(arguments, 1))
        })
    })(A); (function (b) {
        var y = b.addEvent, z = b.perspective, u = b.pick; y(b.Series, "afterTranslate", function () { this.chart.is3d() && this.translate3dPoints() }); b.Series.prototype.translate3dPoints = function () {
            var b = this.chart, v = u(this.zAxis, b.options.zAxis[0]),
                x = [], f, w, p; for (p = 0; p < this.data.length; p++)f = this.data[p], v && v.translate ? (w = v.isLog && v.val2lin ? v.val2lin(f.z) : f.z, f.plotZ = v.translate(w), f.isInside = f.isInside ? w >= v.min && w <= v.max : !1) : f.plotZ = 0, f.axisXpos = f.plotX, f.axisYpos = f.plotY, f.axisZpos = f.plotZ, x.push({ x: f.plotX, y: f.plotY, z: f.plotZ }); b = z(x, b, !0); for (p = 0; p < this.data.length; p++)f = this.data[p], v = b[p], f.plotX = v.x, f.plotY = v.y, f.plotZ = v.z
        }
    })(A); (function (b) {
        function y(b) {
            var e = b.apply(this, [].slice.call(arguments, 1)); this.chart.is3d && this.chart.is3d() &&
                (e.stroke = this.options.edgeColor || e.fill, e["stroke-width"] = q(this.options.edgeWidth, 1)); return e
        } var z = b.addEvent, u = b.perspective, q = b.pick, v = b.Series, x = b.seriesTypes, f = b.svg, w = b.wrap; w(x.column.prototype, "translate", function (b) { b.apply(this, [].slice.call(arguments, 1)); this.chart.is3d() && this.translate3dShapes() }); w(b.Series.prototype, "alignDataLabel", function (b) { arguments[3].outside3dPlot = arguments[1].outside3dPlot; b.apply(this, [].slice.call(arguments, 1)) }); w(b.Series.prototype, "justifyDataLabel",
            function (b) { return arguments[2].outside3dPlot ? !1 : b.apply(this, [].slice.call(arguments, 1)) }); x.column.prototype.translate3dPoints = function () { }; x.column.prototype.translate3dShapes = function () {
                var b = this, e = b.chart, a = b.options, f = a.depth || 25, r = (a.stacking ? a.stack || 0 : b.index) * (f + (a.groupZPadding || 1)), k = b.borderWidth % 2 ? .5 : 0; e.inverted && !b.yAxis.reversed && (k *= -1); !1 !== a.grouping && (r = 0); r += a.groupZPadding || 1; b.data.forEach(function (a) {
                    a.outside3dPlot = null; if (null !== a.y) {
                        var d = a.shapeArgs, c = a.tooltipPos, l;
                        [["x", "width"], ["y", "height"]].forEach(function (c) { l = d[c[0]] - k; 0 > l && (d[c[1]] += d[c[0]] + k, d[c[0]] = -k, l = 0); l + d[c[1]] > b[c[0] + "Axis"].len && 0 !== d[c[1]] && (d[c[1]] = b[c[0] + "Axis"].len - d[c[0]]); if (0 !== d[c[1]] && (d[c[0]] >= b[c[0] + "Axis"].len || d[c[0]] + d[c[1]] <= k)) { for (var e in d) d[e] = 0; a.outside3dPlot = !0 } }); "rect" === a.shapeType && (a.shapeType = "cuboid"); d.z = r; d.depth = f; d.insidePlotArea = !0; c = u([{ x: c[0], y: c[1], z: r }], e, !0)[0]; a.tooltipPos = [c.x, c.y]
                    }
                }); b.z = r
            }; w(x.column.prototype, "animate", function (b) {
                if (this.chart.is3d()) {
                    var e =
                        arguments[1], a = this.yAxis, l = this, p = this.yAxis.reversed; f && (e ? l.data.forEach(function (b) { null !== b.y && (b.height = b.shapeArgs.height, b.shapey = b.shapeArgs.y, b.shapeArgs.height = 1, p || (b.shapeArgs.y = b.stackY ? b.plotY + a.translate(b.stackY) : b.plotY + (b.negative ? -b.height : b.height))) }) : (l.data.forEach(function (a) { null !== a.y && (a.shapeArgs.height = a.height, a.shapeArgs.y = a.shapey, a.graphic && a.graphic.animate(a.shapeArgs, l.options.animation)) }), this.drawDataLabels(), l.animate = null))
                } else b.apply(this, [].slice.call(arguments,
                    1))
            }); w(x.column.prototype, "plotGroup", function (b, e, a, f, r, k) { this.chart.is3d() && k && !this[e] && (this.chart.columnGroup || (this.chart.columnGroup = this.chart.renderer.g("columnGroup").add(k)), this[e] = this.chart.columnGroup, this.chart.columnGroup.attr(this.getPlotBox()), this[e].survive = !0); return b.apply(this, Array.prototype.slice.call(arguments, 1)) }); w(x.column.prototype, "setVisible", function (b, e) {
                var a = this, f; a.chart.is3d() && a.data.forEach(function (b) {
                    f = (b.visible = b.options.visible = e = void 0 === e ? !b.visible :
                        e) ? "visible" : "hidden"; a.options.data[a.data.indexOf(b)] = b.options; b.graphic && b.graphic.attr({ visibility: f })
                }); b.apply(this, Array.prototype.slice.call(arguments, 1))
            }); x.column.prototype.handle3dGrouping = !0; z(v, "afterInit", function () {
                if (this.chart.is3d() && this.handle3dGrouping) {
                    var b = this.options, e = b.grouping, a = b.stacking, f = q(this.yAxis.options.reversedStacks, !0), r = 0; if (void 0 === e || e) {
                        e = this.chart.retrieveStacks(a); r = b.stack || 0; for (a = 0; a < e[r].series.length && e[r].series[a] !== this; a++); r = 10 * (e.totalStacks -
                            e[r].position) + (f ? a : -a); this.xAxis.reversed || (r = 10 * e.totalStacks - r)
                    } b.zIndex = r
                }
            }); w(x.column.prototype, "pointAttribs", y); x.columnrange && (w(x.columnrange.prototype, "pointAttribs", y), x.columnrange.prototype.plotGroup = x.column.prototype.plotGroup, x.columnrange.prototype.setVisible = x.column.prototype.setVisible); w(v.prototype, "alignDataLabel", function (b) {
                if (this.chart.is3d() && this instanceof x.column) {
                    var e = arguments, a = e[4], e = e[1], f = { x: a.x, y: a.y, z: this.z }, f = u([f], this.chart, !0)[0]; a.x = f.x; a.y = e.outside3dPlot ?
                        -9E9 : f.y
                } b.apply(this, [].slice.call(arguments, 1))
            }); w(b.StackItem.prototype, "getStackBox", function (f, e) { var a = f.apply(this, [].slice.call(arguments, 1)); if (e.is3d()) { var l = { x: a.x, y: a.y, z: 0 }, l = b.perspective([l], e, !0)[0]; a.x = l.x; a.y = l.y } return a })
    })(A); (function (b) {
        var y = b.deg2rad, z = b.pick, u = b.seriesTypes, q = b.svg; b = b.wrap; b(u.pie.prototype, "translate", function (b) {
            b.apply(this, [].slice.call(arguments, 1)); if (this.chart.is3d()) {
                var q = this, f = q.options, v = f.depth || 0, p = q.chart.options.chart.options3d, e = p.alpha,
                    a = p.beta, l = f.stacking ? (f.stack || 0) * v : q._i * v, l = l + v / 2; !1 !== f.grouping && (l = 0); q.data.forEach(function (b) { var k = b.shapeArgs; b.shapeType = "arc3d"; k.z = l; k.depth = .75 * v; k.alpha = e; k.beta = a; k.center = q.center; k = (k.end + k.start) / 2; b.slicedTranslation = { translateX: Math.round(Math.cos(k) * f.slicedOffset * Math.cos(e * y)), translateY: Math.round(Math.sin(k) * f.slicedOffset * Math.cos(e * y)) } })
            }
        }); b(u.pie.prototype.pointClass.prototype, "haloPath", function (b) { var q = arguments; return this.series.chart.is3d() ? [] : b.call(this, q[1]) });
        b(u.pie.prototype, "pointAttribs", function (b, q, f) { b = b.call(this, q, f); f = this.options; this.chart.is3d() && !this.chart.styledMode && (b.stroke = f.edgeColor || q.color || this.color, b["stroke-width"] = z(f.edgeWidth, 1)); return b }); b(u.pie.prototype, "drawDataLabels", function (b) {
            if (this.chart.is3d()) {
                var q = this.chart.options.chart.options3d; this.data.forEach(function (b) {
                    var f = b.shapeArgs, p = f.r, e = (f.start + f.end) / 2; b = b.labelPosition; var a = b.connectorPosition, l = -p * (1 - Math.cos((f.alpha || q.alpha) * y)) * Math.sin(e), r = p * (Math.cos((f.beta ||
                        q.beta) * y) - 1) * Math.cos(e);[b.natural, a.breakAt, a.touchingSliceAt].forEach(function (a) { a.x += r; a.y += l })
                })
            } b.apply(this, [].slice.call(arguments, 1))
        }); b(u.pie.prototype, "addPoint", function (b) { b.apply(this, [].slice.call(arguments, 1)); this.chart.is3d() && this.update(this.userOptions, !0) }); b(u.pie.prototype, "animate", function (b) {
            if (this.chart.is3d()) {
                var u = arguments[1], f = this.options.animation, v = this.center, p = this.group, e = this.markerGroup; q && (!0 === f && (f = {}), u ? (p.oldtranslateX = p.translateX, p.oldtranslateY =
                    p.translateY, u = { translateX: v[0], translateY: v[1], scaleX: .001, scaleY: .001 }, p.attr(u), e && (e.attrSetters = p.attrSetters, e.attr(u))) : (u = { translateX: p.oldtranslateX, translateY: p.oldtranslateY, scaleX: 1, scaleY: 1 }, p.animate(u, f), e && e.animate(u, f), this.animate = null))
            } else b.apply(this, [].slice.call(arguments, 1))
        })
    })(A); (function (b) {
        var y = b.Point, z = b.seriesType, u = b.seriesTypes; z("scatter3d", "scatter", { tooltip: { pointFormat: "x: \x3cb\x3e{point.x}\x3c/b\x3e\x3cbr/\x3ey: \x3cb\x3e{point.y}\x3c/b\x3e\x3cbr/\x3ez: \x3cb\x3e{point.z}\x3c/b\x3e\x3cbr/\x3e" } },
            { pointAttribs: function (q) { var v = u.scatter.prototype.pointAttribs.apply(this, arguments); this.chart.is3d() && q && (v.zIndex = b.pointCameraDistance(q, this.chart)); return v }, axisTypes: ["xAxis", "yAxis", "zAxis"], pointArrayMap: ["x", "y", "z"], parallelArrays: ["x", "y", "z"], directTouch: !0 }, { applyOptions: function () { y.prototype.applyOptions.apply(this, arguments); void 0 === this.z && (this.z = 0); return this } })
    })(A); (function (b) {
        var y = b.addEvent, z = b.Axis, u = b.SVGRenderer, q = b.VMLRenderer; q && (b.setOptions({ animate: !1 }), q.prototype.face3d =
            u.prototype.face3d, q.prototype.polyhedron = u.prototype.polyhedron, q.prototype.elements3d = u.prototype.elements3d, q.prototype.element3d = u.prototype.element3d, q.prototype.cuboid = u.prototype.cuboid, q.prototype.cuboidPath = u.prototype.cuboidPath, q.prototype.toLinePath = u.prototype.toLinePath, q.prototype.toLineSegments = u.prototype.toLineSegments, q.prototype.arc3d = function (b) { b = u.prototype.arc3d.call(this, b); b.css({ zIndex: b.zIndex }); return b }, b.VMLRenderer.prototype.arc3dPath = b.SVGRenderer.prototype.arc3dPath,
            y(z, "render", function () { this.sideFrame && (this.sideFrame.css({ zIndex: 0 }), this.sideFrame.front.attr({ fill: this.sideFrame.color })); this.bottomFrame && (this.bottomFrame.css({ zIndex: 1 }), this.bottomFrame.front.attr({ fill: this.bottomFrame.color })); this.backFrame && (this.backFrame.css({ zIndex: 0 }), this.backFrame.front.attr({ fill: this.backFrame.color })) }))
    })(A)
});
//# sourceMappingURL=highcharts-3d.js.map
