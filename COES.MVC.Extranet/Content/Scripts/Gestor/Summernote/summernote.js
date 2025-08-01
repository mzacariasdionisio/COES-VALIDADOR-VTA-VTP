﻿/*! Summernote v0.8.2 | (c) 2013-2015 Alan Hong and other contributors | MIT license */ ! function (a) {
    "function" == typeof define && define.amd ? define(["jquery"], a) : "object" == typeof module && module.exports ? module.exports = a(require("jquery")) : a(window.jQuery)
}(function ($) {
    "use strict";
    var func = function () {
        var a = function (a) {
            return function (b) {
                return a === b
            }
        },
            b = function (a, b) {
                return a === b
            },
            c = function (a) {
                return function (b, c) {
                    return b[a] === c[a]
                }
            },
            d = function () {
                return !0
            },
            e = function () {
                return !1
            },
            f = function (a) {
                return function () {
                    return !a.apply(a, arguments)
                }
            },
            g = function (a, b) {
                return function (c) {
                    return a(c) && b(c)
                }
            },
            h = function (a) {
                return a
            },
            i = function (a, b) {
                return function () {
                    return a[b].apply(a, arguments)
                }
            },
            j = 0,
            k = function (a) {
                var b = ++j + "";
                return a ? a + b : b
            },
            l = function (a) {
                var b = $(document);
                return {
                    top: a.top + b.scrollTop(),
                    left: a.left + b.scrollLeft(),
                    width: a.right - a.left,
                    height: a.bottom - a.top
                }
            },
            m = function (a) {
                var b = {};
                for (var c in a) a.hasOwnProperty(c) && (b[a[c]] = c);
                return b
            },
            n = function (a, b) {
                return b = b || "", b + a.split(".").map(function (a) {
                    return a.substring(0, 1).toUpperCase() + a.substring(1)
                }).join("")
            },
            o = function (a, b, c) {
                var d;
                return function () {
                    var e = this,
                        f = arguments,
                        g = function () {
                            d = null, c || a.apply(e, f)
                        },
                        h = c && !d;
                    clearTimeout(d), d = setTimeout(g, b), h && a.apply(e, f)
                }
            };
        return {
            eq: a,
            eq2: b,
            peq2: c,
            ok: d,
            fail: e,
            self: h,
            not: f,
            and: g,
            invoke: i,
            uniqueId: k,
            rect2bnd: l,
            invertObject: m,
            namespaceToCamel: n,
            debounce: o
        }
    }(),
        list = function () {
            var a = function (a) {
                return a[0]
            },
                b = function (a) {
                    return a[a.length - 1]
                },
                c = function (a) {
                    return a.slice(0, a.length - 1)
                },
                d = function (a) {
                    return a.slice(1)
                },
                e = function (a, b) {
                    for (var c = 0, d = a.length; d > c; c++) {
                        var e = a[c];
                        if (b(e)) return e
                    }
                },
                f = function (a, b) {
                    for (var c = 0, d = a.length; d > c; c++)
                        if (!b(a[c])) return !1;
                    return !0
                },
                g = function (a, b) {
                    return $.inArray(b, a)
                },
                h = function (a, b) {
                    return -1 !== g(a, b)
                },
                i = function (a, b) {
                    return b = b || func.self, a.reduce(function (a, c) {
                        return a + b(c)
                    }, 0)
                },
                j = function (a) {
                    for (var b = [], c = -1, d = a.length; ++c < d;) b[c] = a[c];
                    return b
                },
                k = function (a) {
                    return !a || !a.length
                },
                l = function (c, e) {
                    if (!c.length) return [];
                    var f = d(c);
                    return f.reduce(function (a, c) {
                        var d = b(a);
                        return e(b(d), c) ? d[d.length] = c : a[a.length] = [c], a
                    }, [
                        [a(c)]
                    ])
                },
                m = function (a) {
                    for (var b = [], c = 0, d = a.length; d > c; c++) a[c] && b.push(a[c]);
                    return b
                },
                n = function (a) {
                    for (var b = [], c = 0, d = a.length; d > c; c++) h(b, a[c]) || b.push(a[c]);
                    return b
                },
                o = function (a, b) {
                    var c = g(a, b);
                    return -1 === c ? null : a[c + 1]
                },
                p = function (a, b) {
                    var c = g(a, b);
                    return -1 === c ? null : a[c - 1]
                };
            return {
                head: a,
                last: b,
                initial: c,
                tail: d,
                prev: p,
                next: o,
                find: e,
                contains: h,
                all: f,
                sum: i,
                from: j,
                isEmpty: k,
                clusterBy: l,
                compact: m,
                unique: n
            }
        }(),
        isSupportAmd = "function" == typeof define && define.amd,
        isFontInstalled = function (a) {
            var b = "Comic Sans MS" === a ? "Courier New" : "Comic Sans MS",
                c = $("<div>").css({
                    position: "absolute",
                    left: "-9999px",
                    top: "-9999px",
                    fontSize: "200px"
                }).text("mmmmmmmmmwwwwwww").appendTo(document.body),
                d = c.css("fontFamily", b).width(),
                e = c.css("fontFamily", a + "," + b).width();
            return c.remove(), d !== e
        },
        userAgent = navigator.userAgent,
        isMSIE = /MSIE|Trident/i.test(userAgent),
        browserVersion;
    if (isMSIE) {
        var matches = /MSIE (\d+[.]\d+)/.exec(userAgent);
        matches && (browserVersion = parseFloat(matches[1])), matches = /Trident\/.*rv:([0-9]{1,}[\.0-9]{0,})/.exec(userAgent), matches && (browserVersion = parseFloat(matches[1]))
    }
    var isEdge = /Edge\/\d+/.test(userAgent),
        hasCodeMirror = !!window.CodeMirror;
    if (!hasCodeMirror && isSupportAmd && "undefined" != typeof require)
        if ("undefined" != typeof require.resolve) try {
            require.resolve("codemirror"), hasCodeMirror = !0
        } catch (e) { } else "undefined" != typeof eval("require").specified && (hasCodeMirror = eval("require").specified("codemirror"));
    var agent = {
        isMac: navigator.appVersion.indexOf("Mac") > -1,
        isMSIE: isMSIE,
        isEdge: isEdge,
        isFF: !isEdge && /firefox/i.test(userAgent),
        isPhantom: /PhantomJS/i.test(userAgent),
        isWebkit: !isEdge && /webkit/i.test(userAgent),
        isChrome: !isEdge && /chrome/i.test(userAgent),
        isSafari: !isEdge && /safari/i.test(userAgent),
        browserVersion: browserVersion,
        jqueryVersion: parseFloat($.fn.jquery),
        isSupportAmd: isSupportAmd,
        hasCodeMirror: hasCodeMirror,
        isFontInstalled: isFontInstalled,
        isW3CRangeSupport: !!document.createRange
    },
        NBSP_CHAR = String.fromCharCode(160),
        ZERO_WIDTH_NBSP_CHAR = "\ufeff",
        dom = function () {
            var a = function (a) {
                return a && $(a).hasClass("note-editable")
            },
                b = function (a) {
                    return a && $(a).hasClass("note-control-sizing")
                },
                c = function (a) {
                    return a = a.toUpperCase(),
                        function (b) {
                            return b && b.nodeName.toUpperCase() === a
                        }
                },
                d = function (a) {
                    return a && 3 === a.nodeType
                },
                e = function (a) {
                    return a && 1 === a.nodeType
                },
                f = function (a) {
                    return a && /^BR|^IMG|^HR|^IFRAME|^BUTTON/.test(a.nodeName.toUpperCase())
                },
                g = function (b) {
                    return a(b) ? !1 : b && /^DIV|^P|^LI|^H[1-7]/.test(b.nodeName.toUpperCase())
                },
                h = function (a) {
                    return a && /^H[1-7]/.test(a.nodeName.toUpperCase())
                },
                i = c("PRE"),
                j = c("LI"),
                k = function (a) {
                    return g(a) && !j(a)
                },
                l = c("TABLE"),
                m = c("DATA"),
                n = function (a) {
                    return !(s(a) || o(a) || p(a) || g(a) || l(a) || r(a) || m(a))
                },
                o = function (a) {
                    return a && /^UL|^OL/.test(a.nodeName.toUpperCase())
                },
                p = c("HR"),
                q = function (a) {
                    return a && /^TD|^TH/.test(a.nodeName.toUpperCase())
                },
                r = c("BLOCKQUOTE"),
                s = function (b) {
                    return q(b) || r(b) || a(b)
                },
                t = c("A"),
                u = function (a) {
                    return n(a) && !!D(a, g)
                },
                v = function (a) {
                    return n(a) && !D(a, g)
                },
                w = c("BODY"),
                x = function (a, b) {
                    return a.nextSibling === b || a.previousSibling === b
                },
                y = function (a, b) {
                    b = b || func.ok;
                    var c = [];
                    return a.previousSibling && b(a.previousSibling) && c.push(a.previousSibling), c.push(a), a.nextSibling && b(a.nextSibling) && c.push(a.nextSibling), c
                },
                z = agent.isMSIE && agent.browserVersion < 11 ? "&nbsp;" : "<br>",
                A = function (a) {
                    return d(a) ? a.nodeValue.length : a ? a.childNodes.length : 0
                },
                B = function (a) {
                    var b = A(a);
                    return 0 === b ? !0 : d(a) || 1 !== b || a.innerHTML !== z ? list.all(a.childNodes, d) && "" === a.innerHTML ? !0 : !1 : !0
                },
                C = function (a) {
                    f(a) || A(a) || (a.innerHTML = z)
                },
                D = function (b, c) {
                    for (; b;) {
                        if (c(b)) return b;
                        if (a(b)) break;
                        b = b.parentNode
                    }
                    return null
                },
                E = function (b, c) {
                    for (b = b.parentNode; b && 1 === A(b) ;) {
                        if (c(b)) return b;
                        if (a(b)) break;
                        b = b.parentNode
                    }
                    return null
                },
                F = function (b, c) {
                    c = c || func.fail;
                    var d = [];
                    return D(b, function (b) {
                        return a(b) || d.push(b), c(b)
                    }), d
                },
                G = function (a, b) {
                    var c = F(a);
                    return list.last(c.filter(b))
                },
                H = function (a, b) {
                    for (var c = F(a), d = b; d; d = d.parentNode)
                        if ($.inArray(d, c) > -1) return d;
                    return null
                },
                I = function (a, b) {
                    b = b || func.fail;
                    for (var c = []; a && !b(a) ;) c.push(a), a = a.previousSibling;
                    return c
                },
                J = function (a, b) {
                    b = b || func.fail;
                    for (var c = []; a && !b(a) ;) c.push(a), a = a.nextSibling;
                    return c
                },
                K = function (a, b) {
                    var c = [];
                    return b = b || func.ok,
                        function d(e) {
                            a !== e && b(e) && c.push(e);
                            for (var f = 0, g = e.childNodes.length; g > f; f++) d(e.childNodes[f])
                        }(a), c
                },
                L = function (a, b) {
                    var c = a.parentNode,
                        d = $("<" + b + ">")[0];
                    return c.insertBefore(d, a), d.appendChild(a), d
                },
                M = function (a, b) {
                    var c = b.nextSibling,
                        d = b.parentNode;
                    return c ? d.insertBefore(a, c) : d.appendChild(a), a
                },
                N = function (a, b) {
                    return $.each(b, function (b, c) {
                        a.appendChild(c)
                    }), a
                },
                O = function (a) {
                    return 0 === a.offset
                },
                P = function (a) {
                    return a.offset === A(a.node)
                },
                Q = function (a) {
                    return O(a) || P(a)
                },
                R = function (a, b) {
                    for (; a && a !== b;) {
                        if (0 !== V(a)) return !1;
                        a = a.parentNode
                    }
                    return !0
                },
                S = function (a, b) {
                    if (!b) return !1;
                    for (; a && a !== b;) {
                        if (V(a) !== A(a.parentNode) - 1) return !1;
                        a = a.parentNode
                    }
                    return !0
                },
                T = function (a, b) {
                    return O(a) && R(a.node, b)
                },
                U = function (a, b) {
                    return P(a) && S(a.node, b)
                },
                V = function (a) {
                    for (var b = 0; a = a.previousSibling;) b += 1;
                    return b
                },
                W = function (a) {
                    return !!(a && a.childNodes && a.childNodes.length)
                },
                X = function (b, c) {
                    var d, e;
                    if (0 === b.offset) {
                        if (a(b.node)) return null;
                        d = b.node.parentNode, e = V(b.node)
                    } else W(b.node) ? (d = b.node.childNodes[b.offset - 1], e = A(d)) : (d = b.node, e = c ? 0 : b.offset - 1);
                    return {
                        node: d,
                        offset: e
                    }
                },
                Y = function (b, c) {
                    var d, e;
                    if (A(b.node) === b.offset) {
                        if (a(b.node)) return null;
                        d = b.node.parentNode, e = V(b.node) + 1
                    } else W(b.node) ? (d = b.node.childNodes[b.offset], e = 0) : (d = b.node, e = c ? A(b.node) : b.offset + 1);
                    return {
                        node: d,
                        offset: e
                    }
                },
                Z = function (a, b) {
                    return a.node === b.node && a.offset === b.offset
                },
                _ = function (a) {
                    if (d(a.node) || !W(a.node) || B(a.node)) return !0;
                    var b = a.node.childNodes[a.offset - 1],
                        c = a.node.childNodes[a.offset];
                    return b && !f(b) || c && !f(c) ? !1 : !0
                },
                aa = function (a, b) {
                    for (; a;) {
                        if (b(a)) return a;
                        a = X(a)
                    }
                    return null
                },
                ba = function (a, b) {
                    for (; a;) {
                        if (b(a)) return a;
                        a = Y(a)
                    }
                    return null
                },
                ca = function (a) {
                    if (!d(a.node)) return !1;
                    var b = a.node.nodeValue.charAt(a.offset - 1);
                    return b && " " !== b && b !== NBSP_CHAR
                },
                da = function (a, b, c, d) {
                    for (var e = a; e && (c(e), !Z(e, b)) ;) {
                        var f = d && a.node !== e.node && b.node !== e.node;
                        e = Y(e, f)
                    }
                },
                ea = function (a, b) {
                    var c = F(b, func.eq(a));
                    return c.map(V).reverse()
                },
                fa = function (a, b) {
                    for (var c = a, d = 0, e = b.length; e > d; d++) c = c.childNodes.length <= b[d] ? c.childNodes[c.childNodes.length - 1] : c.childNodes[b[d]];
                    return c
                },
                ga = function (a, b) {
                    var c = b && b.isSkipPaddingBlankHTML,
                        e = b && b.isNotSplitEdgePoint;
                    if (Q(a) && (d(a.node) || e)) {
                        if (O(a)) return a.node;
                        if (P(a)) return a.node.nextSibling
                    }
                    if (d(a.node)) return a.node.splitText(a.offset);
                    var f = a.node.childNodes[a.offset],
                        g = M(a.node.cloneNode(!1), a.node);
                    return N(g, J(f)), c || (C(a.node), C(g)), g
                },
                ha = function (a, b, c) {
                    var d = F(b.node, func.eq(a));
                    return d.length ? 1 === d.length ? ga(b, c) : d.reduce(function (a, d) {
                        return a === b.node && (a = ga(b, c)), ga({
                            node: d,
                            offset: a ? dom.position(a) : A(d)
                        }, c)
                    }) : null
                },
                ia = function (a, b) {
                    var c, d, e = b ? g : s,
                        f = F(a.node, e),
                        h = list.last(f) || a.node;
                    e(h) ? (c = f[f.length - 2], d = h) : (c = h, d = c.parentNode);
                    var i = c && ha(c, a, {
                        isSkipPaddingBlankHTML: b,
                        isNotSplitEdgePoint: b
                    });
                    return i || d !== a.node || (i = a.node.childNodes[a.offset]), {
                        rightNode: i,
                        container: d
                    }
                },
                ja = function (a) {
                    return document.createElement(a)
                },
                ka = function (a) {
                    return document.createTextNode(a)
                },
                la = function (a, b) {
                    if (a && a.parentNode) {
                        if (a.removeNode) return a.removeNode(b);
                        var c = a.parentNode;
                        if (!b) {
                            var d, e, f = [];
                            for (d = 0, e = a.childNodes.length; e > d; d++) f.push(a.childNodes[d]);
                            for (d = 0, e = f.length; e > d; d++) c.insertBefore(f[d], a)
                        }
                        c.removeChild(a)
                    }
                },
                ma = function (b, c) {
                    for (; b && !a(b) && c(b) ;) {
                        var d = b.parentNode;
                        la(b), b = d
                    }
                },
                na = function (a, b) {
                    if (a.nodeName.toUpperCase() === b.toUpperCase()) return a;
                    var c = ja(b);
                    return a.style.cssText && (c.style.cssText = a.style.cssText), N(c, list.from(a.childNodes)), M(c, a), la(a), c
                },
                oa = c("TEXTAREA"),
                pa = function (a, b) {
                    var c = oa(a[0]) ? a.val() : a.html();
                    return b ? c.replace(/[\n\r]/g, "") : c
                },
                qa = function (a, b) {
                    var c = pa(a);
                    if (b) {
                        var d = /<(\/?)(\b(?!!)[^>\s]*)(.*?)(\s*\/?>)/g;
                        c = c.replace(d, function (a, b, c) {
                            c = c.toUpperCase();
                            var d = /^DIV|^TD|^TH|^P|^LI|^H[1-7]/.test(c) && !!b,
                                e = /^BLOCKQUOTE|^TABLE|^TBODY|^TR|^HR|^UL|^OL/.test(c);
                            return a + (d || e ? "\n" : "")
                        }), c = $.trim(c)
                    }
                    return c
                },
                ra = function (a) {
                    var b = $(a),
                        c = b.offset(),
                        d = b.outerHeight(!0);
                    return {
                        left: c.left,
                        top: c.top + d
                    }
                },
                sa = function (a, b) {
                    Object.keys(b).forEach(function (c) {
                        a.on(c, b[c])
                    })
                },
                ta = function (a, b) {
                    Object.keys(b).forEach(function (c) {
                        a.off(c, b[c])
                    })
                };
            return {
                NBSP_CHAR: NBSP_CHAR,
                ZERO_WIDTH_NBSP_CHAR: ZERO_WIDTH_NBSP_CHAR,
                blank: z,
                emptyPara: "<p>" + z + "</p>",
                makePredByNodeName: c,
                isEditable: a,
                isControlSizing: b,
                isText: d,
                isElement: e,
                isVoid: f,
                isPara: g,
                isPurePara: k,
                isHeading: h,
                isInline: n,
                isBlock: func.not(n),
                isBodyInline: v,
                isBody: w,
                isParaInline: u,
                isPre: i,
                isList: o,
                isTable: l,
                isData: m,
                isCell: q,
                isBlockquote: r,
                isBodyContainer: s,
                isAnchor: t,
                isDiv: c("DIV"),
                isLi: j,
                isBR: c("BR"),
                isSpan: c("SPAN"),
                isB: c("B"),
                isU: c("U"),
                isS: c("S"),
                isI: c("I"),
                isImg: c("IMG"),
                isTextarea: oa,
                isEmpty: B,
                isEmptyAnchor: func.and(t, B),
                isClosestSibling: x,
                withClosestSiblings: y,
                nodeLength: A,
                isLeftEdgePoint: O,
                isRightEdgePoint: P,
                isEdgePoint: Q,
                isLeftEdgeOf: R,
                isRightEdgeOf: S,
                isLeftEdgePointOf: T,
                isRightEdgePointOf: U,
                prevPoint: X,
                nextPoint: Y,
                isSamePoint: Z,
                isVisiblePoint: _,
                prevPointUntil: aa,
                nextPointUntil: ba,
                isCharPoint: ca,
                walkPoint: da,
                ancestor: D,
                singleChildAncestor: E,
                listAncestor: F,
                lastAncestor: G,
                listNext: J,
                listPrev: I,
                listDescendant: K,
                commonAncestor: H,
                wrap: L,
                insertAfter: M,
                appendChildNodes: N,
                position: V,
                hasChildren: W,
                makeOffsetPath: ea,
                fromOffsetPath: fa,
                splitTree: ha,
                splitPoint: ia,
                create: ja,
                createText: ka,
                remove: la,
                removeWhile: ma,
                replace: na,
                html: qa,
                value: pa,
                posFromPlaceholder: ra,
                attachEvents: sa,
                detachEvents: ta
            }
        }(),
        Context = function (a, b) {
            var c = this,
                d = $.summernote.ui;
            return this.memos = {}, this.modules = {}, this.layoutInfo = {}, this.options = b, this.initialize = function () {
                return this.layoutInfo = d.createLayout(a, b), this._initialize(), a.hide(), this
            }, this.destroy = function () {
                this._destroy(), a.removeData("summernote"), d.removeLayout(a, this.layoutInfo)
            }, this.reset = function () {
                var a = c.isDisabled();
                this.code(dom.emptyPara), this._destroy(), this._initialize(), a && c.disable()
            }, this._initialize = function () {
                var a = $.extend({}, this.options.buttons);
                Object.keys(a).forEach(function (b) {
                    c.memo("button." + b, a[b])
                });
                var b = $.extend({}, this.options.modules, $.summernote.plugins || {});
                Object.keys(b).forEach(function (a) {
                    c.module(a, b[a], !0)
                }), Object.keys(this.modules).forEach(function (a) {
                    c.initializeModule(a)
                })
            }, this._destroy = function () {
                Object.keys(this.modules).reverse().forEach(function (a) {
                    c.removeModule(a)
                }), Object.keys(this.memos).forEach(function (a) {
                    c.removeMemo(a)
                })
            }, this.code = function (b) {
                var c = this.invoke("codeview.isActivated");
                return void 0 === b ? (this.invoke("codeview.sync"), c ? this.layoutInfo.codable.val() : this.layoutInfo.editable.html()) : (c ? this.layoutInfo.codable.val(b) : this.layoutInfo.editable.html(b), a.val(b), this.triggerEvent("change", b), void 0)
            }, this.isDisabled = function () {
                return "false" === this.layoutInfo.editable.attr("contenteditable")
            }, this.enable = function () {
                this.layoutInfo.editable.attr("contenteditable", !0), this.invoke("toolbar.activate", !0)
            }, this.disable = function () {
                this.invoke("codeview.isActivated") && this.invoke("codeview.deactivate"), this.layoutInfo.editable.attr("contenteditable", !1), this.invoke("toolbar.deactivate", !0)
            }, this.triggerEvent = function () {
                var b = list.head(arguments),
                    c = list.tail(list.from(arguments)),
                    d = this.options.callbacks[func.namespaceToCamel(b, "on")];
                d && d.apply(a[0], c), a.trigger("summernote." + b, c)
            }, this.initializeModule = function (b) {
                var c = this.modules[b];
                c.shouldInitialize = c.shouldInitialize || func.ok, c.shouldInitialize() && (c.initialize && c.initialize(), c.events && dom.attachEvents(a, c.events))
            }, this.module = function (a, b, c) {
                return 1 === arguments.length ? this.modules[a] : (this.modules[a] = new b(this), void (c || this.initializeModule(a)))
            }, this.removeModule = function (b) {
                var c = this.modules[b];
                c.shouldInitialize() && (c.events && dom.detachEvents(a, c.events), c.destroy && c.destroy()), delete this.modules[b]
            }, this.memo = function (a, b) {
                return 1 === arguments.length ? this.memos[a] : void (this.memos[a] = b)
            }, this.removeMemo = function (a) {
                this.memos[a] && this.memos[a].destroy && this.memos[a].destroy(), delete this.memos[a]
            }, this.createInvokeHandler = function (a, b) {
                return function (d) {
                    d.preventDefault(), c.invoke(a, b || $(d.target).closest("[data-value]").data("value"))
                }
            }, this.invoke = function () {
                var a = list.head(arguments),
                    b = list.tail(list.from(arguments)),
                    c = a.split("."),
                    d = c.length > 1,
                    e = d && list.head(c),
                    f = d ? list.last(c) : list.head(c),
                    g = this.modules[e || "editor"];
                return !e && this[f] ? this[f].apply(this, b) : g && g[f] && g.shouldInitialize() ? g[f].apply(g, b) : void 0
            }, this.initialize()
        };
    $.fn.extend({
        summernote: function () {
            var a = $.type(list.head(arguments)),
                b = "string" === a,
                c = "object" === a,
                d = c ? list.head(arguments) : {};
            d = $.extend({}, $.summernote.options, d), d.langInfo = $.extend(!0, {}, $.summernote.lang["en-US"], $.summernote.lang[d.lang]), d.icons = $.extend(!0, {}, $.summernote.options.icons, d.icons), this.each(function (a, b) {
                var c = $(b);
                if (!c.data("summernote")) {
                    var e = new Context(c, d);
                    c.data("summernote", e), c.data("summernote").triggerEvent("init", e.layoutInfo)
                }
            });
            var e = this.first();
            if (e.length) {
                var f = e.data("summernote");
                if (b) return f.invoke.apply(f, list.from(arguments));
                d.focus && f.invoke("editor.focus")
            }
            return this
        }
    });
    var Renderer = function (a, b, c, d) {
        this.render = function (e) {
            var f = $(a);
            if (c && c.contents && f.html(c.contents), c && c.className && f.addClass(c.className), c && c.data && $.each(c.data, function (a, b) {
                    f.attr("data-" + a, b)
            }), c && c.click && f.on("click", c.click), b) {
                var g = f.find(".note-children-container");
                b.forEach(function (a) {
                    a.render(g.length ? g : f)
                })
            }
            return d && d(f, c), c && c.callback && c.callback(f), e && e.append(f), f
        }
    },
        renderer = {
            create: function (a, b) {
                return function () {
                    var c = $.isArray(arguments[0]) ? arguments[0] : [],
                        d = "object" == typeof arguments[1] ? arguments[1] : arguments[0];
                    return d && d.children && (c = d.children), new Renderer(a, c, d, b)
                }
            }
        },
        editor = renderer.create('<div class="note-editor note-frame panel panel-default"/>'),
        toolbar = renderer.create('<div class="note-toolbar panel-heading"/>'),
        editingArea = renderer.create('<div class="note-editing-area"/>'),
        codable = renderer.create('<textarea class="note-codable"/>'),
        editable = renderer.create('<div class="note-editable panel-body" contentEditable="true"/>'),
        statusbar = renderer.create(['<div class="note-statusbar">', '  <div class="note-resizebar">', '    <div class="note-icon-bar"/>', '    <div class="note-icon-bar"/>', '    <div class="note-icon-bar"/>', "  </div>", "</div>"].join("")),
        airEditor = renderer.create('<div class="note-editor"/>'),
        airEditable = renderer.create('<div class="note-editable" contentEditable="true"/>'),
        buttonGroup = renderer.create('<div class="note-btn-group btn-group">'),
        button = renderer.create('<button type="button" class="note-btn btn btn-default btn-sm" tabindex="-1">', function (a, b) {
            b && b.tooltip && a.attr({
                title: b.tooltip
            }).tooltip({
                container: "body",
                trigger: "hover",
                placement: "bottom"
            })
        }),
        dropdown = renderer.create('<div class="dropdown-menu">', function (a, b) {
            var c = $.isArray(b.items) ? b.items.map(function (a) {
                var c = "string" == typeof a ? a : a.value || "",
                    d = b.template ? b.template(a) : a;
                return '<li><a href="#" data-value="' + c + '">' + d + "</a></li>"
            }).join("") : b.items;
            a.html(c)
        }),
        dropdownCheck = renderer.create('<div class="dropdown-menu note-check">', function (a, b) {
            var c = $.isArray(b.items) ? b.items.map(function (a) {
                var c = "string" == typeof a ? a : a.value || "",
                    d = b.template ? b.template(a) : a;
                return '<li><a href="#" data-value="' + c + '">' + icon(b.checkClassName) + " " + d + "</a></li>"
            }).join("") : b.items;
            a.html(c)
        }),
        palette = renderer.create('<div class="note-color-palette"/>', function (a, b) {
            for (var c = [], d = 0, e = b.colors.length; e > d; d++) {
                for (var f = b.eventName, g = b.colors[d], h = [], i = 0, j = g.length; j > i; i++) {
                    var k = g[i];
                    h.push(['<button type="button" class="note-color-btn"', 'style="background-color:', k, '" ', 'data-event="', f, '" ', 'data-value="', k, '" ', 'title="', k, '" ', 'data-toggle="button" tabindex="-1"></button>'].join(""))
                }
                c.push('<div class="note-color-row">' + h.join("") + "</div>")
            }
            a.html(c.join("")), a.find(".note-color-btn").tooltip({
                container: "body",
                trigger: "hover",
                placement: "bottom"
            })
        }),
        dialog = renderer.create('<div class="modal" aria-hidden="false" tabindex="-1"/>', function (a, b) {
            b.fade && a.addClass("fade"), a.html(['<div class="modal-dialog">', '  <div class="modal-content">', b.title ? '    <div class="modal-header">      <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>      <h4 class="modal-title">' + b.title + "</h4>    </div>" : "", '    <div class="modal-body">' + b.body + "</div>", b.footer ? '    <div class="modal-footer">' + b.footer + "</div>" : "", "  </div>", "</div>"].join(""))
        }),
        popover = renderer.create(['<div class="note-popover popover in">', '  <div class="arrow"/>', '  <div class="popover-content note-children-container"/>', "</div>"].join(""), function (a, b) {
            var c = "undefined" != typeof b.direction ? b.direction : "bottom";
            a.addClass(c), b.hideArrow && a.find(".arrow").hide()
        }),
        icon = function (a, b) {
            return b = b || "i", "<" + b + ' class="' + a + '"/>'
        },
        ui = {
            editor: editor,
            toolbar: toolbar,
            editingArea: editingArea,
            codable: codable,
            editable: editable,
            statusbar: statusbar,
            airEditor: airEditor,
            airEditable: airEditable,
            buttonGroup: buttonGroup,
            button: button,
            dropdown: dropdown,
            dropdownCheck: dropdownCheck,
            palette: palette,
            dialog: dialog,
            popover: popover,
            icon: icon,
            toggleBtn: function (a, b) {
                a.toggleClass("disabled", !b), a.attr("disabled", !b)
            },
            toggleBtnActive: function (a, b) {
                a.toggleClass("active", b)
            },
            onDialogShown: function (a, b) {
                a.one("shown.bs.modal", b)
            },
            onDialogHidden: function (a, b) {
                a.one("hidden.bs.modal", b)
            },
            showDialog: function (a) {
                a.modal("show")
            },
            hideDialog: function (a) {
                a.modal("hide")
            },
            createLayout: function (a, b) {
                var c = (b.airMode ? ui.airEditor([ui.editingArea([ui.airEditable()])]) : ui.editor([ui.toolbar(), ui.editingArea([ui.codable(), ui.editable()]), ui.statusbar()])).render();
                return c.insertAfter(a), {
                    note: a,
                    editor: c,
                    toolbar: c.find(".note-toolbar"),
                    editingArea: c.find(".note-editing-area"),
                    editable: c.find(".note-editable"),
                    codable: c.find(".note-codable"),
                    statusbar: c.find(".note-statusbar")
                }
            },
            removeLayout: function (a, b) {
                a.html(b.editable.html()), b.editor.remove(), a.show()
            }
        };
    $.summernote = $.summernote || {
        lang: {}
    }, $.extend($.summernote.lang, {
        "en-US": {
            font: {
                bold: "Bold",
                italic: "Italic",
                underline: "Underline",
                clear: "Remove Font Style",
                height: "Line Height",
                name: "Font Family",
                strikethrough: "Strikethrough",
                subscript: "Subscript",
                superscript: "Superscript",
                size: "Font Size"
            },
            image: {
                image: "Picture",
                insert: "Insert Image",
                resizeFull: "Resize Full",
                resizeHalf: "Resize Half",
                resizeQuarter: "Resize Quarter",
                floatLeft: "Float Left",
                floatRight: "Float Right",
                floatNone: "Float None",
                shapeRounded: "Shape: Rounded",
                shapeCircle: "Shape: Circle",
                shapeThumbnail: "Shape: Thumbnail",
                shapeNone: "Shape: None",
                dragImageHere: "Drag image or text here",
                dropImage: "Drop image or Text",
                selectFromFiles: "Select from files",
                maximumFileSize: "Maximum file size",
                maximumFileSizeError: "Maximum file size exceeded.",
                url: "Image URL",
                remove: "Remove Image"
            },
            video: {
                video: "Video",
                videoLink: "Video Link",
                insert: "Insert Video",
                url: "Video URL?",
                providers: "(YouTube, Vimeo, Vine, Instagram, DailyMotion or Youku)"
            },
            link: {
                link: "Link",
                insert: "Insert Link",
                unlink: "Unlink",
                edit: "Edit",
                textToDisplay: "Text to display",
                url: "To what URL should this link go?",
                openInNewWindow: "Open in new window"
            },
            table: {
                table: "Table"
            },
            hr: {
                insert: "Insert Horizontal Rule"
            },
            style: {
                style: "Style",
                normal: "Normal",
                blockquote: "Quote",
                pre: "Code",
                h1: "Header 1",
                h2: "Header 2",
                h3: "Header 3",
                h4: "Header 4",
                h5: "Header 5",
                h6: "Header 6"
            },
            lists: {
                unordered: "Unordered list",
                ordered: "Ordered list"
            },
            options: {
                help: "Help",
                fullscreen: "Full Screen",
                codeview: "Code View"
            },
            paragraph: {
                paragraph: "Paragraph",
                outdent: "Outdent",
                indent: "Indent",
                left: "Align left",
                center: "Align center",
                right: "Align right",
                justify: "Justify full"
            },
            color: {
                recent: "Recent Color",
                more: "More Color",
                background: "Background Color",
                foreground: "Foreground Color",
                transparent: "Transparent",
                setTransparent: "Set transparent",
                reset: "Reset",
                resetToDefault: "Reset to default"
            },
            shortcut: {
                shortcuts: "Keyboard shortcuts",
                close: "Close",
                textFormatting: "Text formatting",
                action: "Action",
                paragraphFormatting: "Paragraph formatting",
                documentStyle: "Document Style",
                extraKeys: "Extra keys"
            },
            help: {
                insertParagraph: "Insert Paragraph",
                undo: "Undoes the last command",
                redo: "Redoes the last command",
                tab: "Tab",
                untab: "Untab",
                bold: "Set a bold style",
                italic: "Set a italic style",
                underline: "Set a underline style",
                strikethrough: "Set a strikethrough style",
                removeFormat: "Clean a style",
                justifyLeft: "Set left align",
                justifyCenter: "Set center align",
                justifyRight: "Set right align",
                justifyFull: "Set full align",
                insertUnorderedList: "Toggle unordered list",
                insertOrderedList: "Toggle ordered list",
                outdent: "Outdent on current paragraph",
                indent: "Indent on current paragraph",
                formatPara: "Change current block's format as a paragraph(P tag)",
                formatH1: "Change current block's format as H1",
                formatH2: "Change current block's format as H2",
                formatH3: "Change current block's format as H3",
                formatH4: "Change current block's format as H4",
                formatH5: "Change current block's format as H5",
                formatH6: "Change current block's format as H6",
                insertHorizontalRule: "Insert horizontal rule",
                "linkDialog.show": "Show Link Dialog"
            },
            history: {
                undo: "Undo",
                redo: "Redo"
            },
            specialChar: {
                specialChar: "SPECIAL CHARACTERS",
                select: "Select Special characters"
            }
        }
    });
    var key = function () {
        var a = {
            BACKSPACE: 8,
            TAB: 9,
            ENTER: 13,
            SPACE: 32,
            LEFT: 37,
            UP: 38,
            RIGHT: 39,
            DOWN: 40,
            NUM0: 48,
            NUM1: 49,
            NUM2: 50,
            NUM3: 51,
            NUM4: 52,
            NUM5: 53,
            NUM6: 54,
            NUM7: 55,
            NUM8: 56,
            B: 66,
            E: 69,
            I: 73,
            J: 74,
            K: 75,
            L: 76,
            R: 82,
            S: 83,
            U: 85,
            V: 86,
            Y: 89,
            Z: 90,
            SLASH: 191,
            LEFTBRACKET: 219,
            BACKSLASH: 220,
            RIGHTBRACKET: 221
        };
        return {
            isEdit: function (b) {
                return list.contains([a.BACKSPACE, a.TAB, a.ENTER, a.SPACE], b)
            },
            isMove: function (b) {
                return list.contains([a.LEFT, a.UP, a.RIGHT, a.DOWN], b)
            },
            nameFromCode: func.invertObject(a),
            code: a
        }
    }(),
        range = function () {
            var a = function (a, b) {
                var c, d, e = a.parentElement(),
                    f = document.body.createTextRange(),
                    g = list.from(e.childNodes);
                for (c = 0; c < g.length; c++)
                    if (!dom.isText(g[c])) {
                        if (f.moveToElementText(g[c]), f.compareEndPoints("StartToStart", a) >= 0) break;
                        d = g[c]
                    }
                if (0 !== c && dom.isText(g[c - 1])) {
                    var h = document.body.createTextRange(),
                        i = null;
                    h.moveToElementText(d || e), h.collapse(!d), i = d ? d.nextSibling : e.firstChild;
                    var j = a.duplicate();
                    j.setEndPoint("StartToStart", h);
                    for (var k = j.text.replace(/[\r\n]/g, "").length; k > i.nodeValue.length && i.nextSibling;) k -= i.nodeValue.length, i = i.nextSibling;
                    i.nodeValue;
                    b && i.nextSibling && dom.isText(i.nextSibling) && k === i.nodeValue.length && (k -= i.nodeValue.length, i = i.nextSibling), e = i, c = k
                }
                return {
                    cont: e,
                    offset: c
                }
            },
                b = function (a) {
                    var b = function (a, c) {
                        var d, e;
                        if (dom.isText(a)) {
                            var f = dom.listPrev(a, func.not(dom.isText)),
                                g = list.last(f).previousSibling;
                            d = g || a.parentNode, c += list.sum(list.tail(f), dom.nodeLength), e = !g
                        } else {
                            if (d = a.childNodes[c] || a, dom.isText(d)) return b(d, 0);
                            c = 0, e = !1
                        }
                        return {
                            node: d,
                            collapseToStart: e,
                            offset: c
                        }
                    },
                        c = document.body.createTextRange(),
                        d = b(a.node, a.offset);
                    return c.moveToElementText(d.node), c.collapse(d.collapseToStart), c.moveStart("character", d.offset), c
                },
                c = function (a, d, e, f) {
                    this.sc = a, this.so = d, this.ec = e, this.eo = f;
                    var g = function () {
                        if (agent.isW3CRangeSupport) {
                            var c = document.createRange();
                            return c.setStart(a, d), c.setEnd(e, f), c
                        }
                        var g = b({
                            node: a,
                            offset: d
                        });
                        return g.setEndPoint("EndToEnd", b({
                            node: e,
                            offset: f
                        })), g
                    };
                    this.getPoints = function () {
                        return {
                            sc: a,
                            so: d,
                            ec: e,
                            eo: f
                        }
                    }, this.getStartPoint = function () {
                        return {
                            node: a,
                            offset: d
                        }
                    }, this.getEndPoint = function () {
                        return {
                            node: e,
                            offset: f
                        }
                    }, this.select = function () {
                        var a = g();
                        if (agent.isW3CRangeSupport) {
                            var b = document.getSelection();
                            b.rangeCount > 0 && b.removeAllRanges(), b.addRange(a)
                        } else a.select();
                        return this
                    }, this.scrollIntoView = function (a) {
                        var b = $(a).height();
                        return a.scrollTop + b < this.sc.offsetTop && (a.scrollTop += Math.abs(a.scrollTop + b - this.sc.offsetTop)), this
                    }, this.normalize = function () {
                        var a = function (a, b) {
                            if (dom.isVisiblePoint(a) && !dom.isEdgePoint(a) || dom.isVisiblePoint(a) && dom.isRightEdgePoint(a) && !b || dom.isVisiblePoint(a) && dom.isLeftEdgePoint(a) && b || dom.isVisiblePoint(a) && dom.isBlock(a.node) && dom.isEmpty(a.node)) return a;
                            var c = dom.ancestor(a.node, dom.isBlock);
                            if ((dom.isLeftEdgePointOf(a, c) || dom.isVoid(dom.prevPoint(a).node)) && !b || (dom.isRightEdgePointOf(a, c) || dom.isVoid(dom.nextPoint(a).node)) && b) {
                                if (dom.isVisiblePoint(a)) return a;
                                b = !b
                            }
                            var d = b ? dom.nextPointUntil(dom.nextPoint(a), dom.isVisiblePoint) : dom.prevPointUntil(dom.prevPoint(a), dom.isVisiblePoint);
                            return d || a
                        },
                            b = a(this.getEndPoint(), !1),
                            d = this.isCollapsed() ? b : a(this.getStartPoint(), !0);
                        return new c(d.node, d.offset, b.node, b.offset)
                    }, this.nodes = function (a, b) {
                        a = a || func.ok;
                        var c = b && b.includeAncestor,
                            d = b && b.fullyContains,
                            e = this.getStartPoint(),
                            f = this.getEndPoint(),
                            g = [],
                            h = [];
                        return dom.walkPoint(e, f, function (b) {
                            if (!dom.isEditable(b.node)) {
                                var e;
                                d ? (dom.isLeftEdgePoint(b) && h.push(b.node), dom.isRightEdgePoint(b) && list.contains(h, b.node) && (e = b.node)) : e = c ? dom.ancestor(b.node, a) : b.node, e && a(e) && g.push(e)
                            }
                        }, !0), list.unique(g)
                    }, this.commonAncestor = function () {
                        return dom.commonAncestor(a, e)
                    }, this.expand = function (b) {
                        var g = dom.ancestor(a, b),
                            h = dom.ancestor(e, b);
                        if (!g && !h) return new c(a, d, e, f);
                        var i = this.getPoints();
                        return g && (i.sc = g, i.so = 0), h && (i.ec = h, i.eo = dom.nodeLength(h)), new c(i.sc, i.so, i.ec, i.eo)
                    }, this.collapse = function (b) {
                        return b ? new c(a, d, a, d) : new c(e, f, e, f)
                    }, this.splitText = function () {
                        var b = a === e,
                            g = this.getPoints();
                        return dom.isText(e) && !dom.isEdgePoint(this.getEndPoint()) && e.splitText(f), dom.isText(a) && !dom.isEdgePoint(this.getStartPoint()) && (g.sc = a.splitText(d), g.so = 0, b && (g.ec = g.sc, g.eo = f - d)), new c(g.sc, g.so, g.ec, g.eo)
                    }, this.deleteContents = function () {
                        if (this.isCollapsed()) return this;
                        var a = this.splitText(),
                            b = a.nodes(null, {
                                fullyContains: !0
                            }),
                            d = dom.prevPointUntil(a.getStartPoint(), function (a) {
                                return !list.contains(b, a.node)
                            }),
                            e = [];
                        return $.each(b, function (a, b) {
                            var c = b.parentNode;
                            d.node !== c && 1 === dom.nodeLength(c) && e.push(c), dom.remove(b, !1)
                        }), $.each(e, function (a, b) {
                            dom.remove(b, !1)
                        }), new c(d.node, d.offset, d.node, d.offset).normalize()
                    };
                    var h = function (b) {
                        return function () {
                            var c = dom.ancestor(a, b);
                            return !!c && c === dom.ancestor(e, b)
                        }
                    };
                    this.isOnEditable = h(dom.isEditable), this.isOnList = h(dom.isList), this.isOnAnchor = h(dom.isAnchor), this.isOnCell = h(dom.isCell), this.isOnData = h(dom.isData), this.isLeftEdgeOf = function (a) {
                        if (!dom.isLeftEdgePoint(this.getStartPoint())) return !1;
                        var b = dom.ancestor(this.sc, a);
                        return b && dom.isLeftEdgeOf(this.sc, b)
                    }, this.isCollapsed = function () {
                        return a === e && d === f
                    }, this.wrapBodyInlineWithPara = function () {
                        if (dom.isBodyContainer(a) && dom.isEmpty(a)) return a.innerHTML = dom.emptyPara, new c(a.firstChild, 0, a.firstChild, 0);
                        var b = this.normalize();
                        if (dom.isParaInline(a) || dom.isPara(a)) return b;
                        var d;
                        if (dom.isInline(b.sc)) {
                            var e = dom.listAncestor(b.sc, func.not(dom.isInline));
                            d = list.last(e), dom.isInline(d) || (d = e[e.length - 2] || b.sc.childNodes[b.so])
                        } else d = b.sc.childNodes[b.so > 0 ? b.so - 1 : 0];
                        var f = dom.listPrev(d, dom.isParaInline).reverse();
                        if (f = f.concat(dom.listNext(d.nextSibling, dom.isParaInline)), f.length) {
                            var g = dom.wrap(list.head(f), "p");
                            dom.appendChildNodes(g, list.tail(f))
                        }
                        return this.normalize()
                    }, this.insertNode = function (a) {
                        var b = this.wrapBodyInlineWithPara().deleteContents(),
                            c = dom.splitPoint(b.getStartPoint(), dom.isInline(a));
                        return c.rightNode ? c.rightNode.parentNode.insertBefore(a, c.rightNode) : c.container.appendChild(a), a
                    }, this.pasteHTML = function (a) {
                        var b = $("<div></div>").html(a)[0],
                            c = list.from(b.childNodes),
                            d = this.wrapBodyInlineWithPara().deleteContents();
                        return c.reverse().map(function (a) {
                            return d.insertNode(a)
                        }).reverse()
                    }, this.toString = function () {
                        var a = g();
                        return agent.isW3CRangeSupport ? a.toString() : a.text
                    }, this.getWordRange = function (a) {
                        var b = this.getEndPoint();
                        if (!dom.isCharPoint(b)) return this;
                        var d = dom.prevPointUntil(b, function (a) {
                            return !dom.isCharPoint(a)
                        });
                        return a && (b = dom.nextPointUntil(b, function (a) {
                            return !dom.isCharPoint(a)
                        })), new c(d.node, d.offset, b.node, b.offset)
                    }, this.bookmark = function (b) {
                        return {
                            s: {
                                path: dom.makeOffsetPath(b, a),
                                offset: d
                            },
                            e: {
                                path: dom.makeOffsetPath(b, e),
                                offset: f
                            }
                        }
                    }, this.paraBookmark = function (b) {
                        return {
                            s: {
                                path: list.tail(dom.makeOffsetPath(list.head(b), a)),
                                offset: d
                            },
                            e: {
                                path: list.tail(dom.makeOffsetPath(list.last(b), e)),
                                offset: f
                            }
                        }
                    }, this.getClientRects = function () {
                        var a = g();
                        return a.getClientRects()
                    }
                };
            return {
                create: function (a, b, d, e) {
                    if (4 === arguments.length) return new c(a, b, d, e);
                    if (2 === arguments.length) return d = a, e = b, new c(a, b, d, e);
                    var f = this.createFromSelection();
                    return f || 1 !== arguments.length ? f : (f = this.createFromNode(arguments[0]), f.collapse(dom.emptyPara === arguments[0].innerHTML))
                },
                createFromSelection: function () {
                    var b, d, e, f;
                    if (agent.isW3CRangeSupport) {
                        var g = document.getSelection();
                        if (!g || 0 === g.rangeCount) return null;
                        if (dom.isBody(g.anchorNode)) return null;
                        var h = g.getRangeAt(0);
                        b = h.startContainer, d = h.startOffset, e = h.endContainer, f = h.endOffset
                    } else {
                        var i = document.selection.createRange(),
                            j = i.duplicate();
                        j.collapse(!1);
                        var k = i;
                        k.collapse(!0);
                        var l = a(k, !0),
                            m = a(j, !1);
                        dom.isText(l.node) && dom.isLeftEdgePoint(l) && dom.isTextNode(m.node) && dom.isRightEdgePoint(m) && m.node.nextSibling === l.node && (l = m), b = l.cont, d = l.offset, e = m.cont, f = m.offset
                    }
                    return new c(b, d, e, f)
                },
                createFromNode: function (a) {
                    var b = a,
                        c = 0,
                        d = a,
                        e = dom.nodeLength(d);
                    return dom.isVoid(b) && (c = dom.listPrev(b).length - 1, b = b.parentNode), dom.isBR(d) ? (e = dom.listPrev(d).length - 1, d = d.parentNode) : dom.isVoid(d) && (e = dom.listPrev(d).length, d = d.parentNode), this.create(b, c, d, e)
                },
                createFromNodeBefore: function (a) {
                    return this.createFromNode(a).collapse(!0)
                },
                createFromNodeAfter: function (a) {
                    return this.createFromNode(a).collapse()
                },
                createFromBookmark: function (a, b) {
                    var d = dom.fromOffsetPath(a, b.s.path),
                        e = b.s.offset,
                        f = dom.fromOffsetPath(a, b.e.path),
                        g = b.e.offset;
                    return new c(d, e, f, g)
                },
                createFromParaBookmark: function (a, b) {
                    var d = a.s.offset,
                        e = a.e.offset,
                        f = dom.fromOffsetPath(list.head(b), a.s.path),
                        g = dom.fromOffsetPath(list.last(b), a.e.path);
                    return new c(f, d, g, e)
                }
            }
        }(),
        async = function () {
            var a = function (a) {
                return $.Deferred(function (b) {
                    $.extend(new FileReader, {
                        onload: function (a) {
                            var c = a.target.result;
                            b.resolve(c)
                        },
                        onerror: function () {
                            b.reject(this)
                        }
                    }).readAsDataURL(a)
                }).promise()
            },
                b = function (a) {
                    return $.Deferred(function (b) {
                        var c = $("<img>");
                        c.one("load", function () {
                            c.off("error abort"), b.resolve(c)
                        }).one("error abort", function () {
                            c.off("load").detach(), b.reject(c)
                        }).css({
                            display: "none"
                        }).appendTo(document.body).attr("src", a)
                    }).promise()
                };
            return {
                readFileAsDataURL: a,
                createImage: b
            }
        }(),
        History = function (a) {
            var b = [],
                c = -1,
                d = a[0],
                e = function () {
                    var b = range.create(d),
                        c = {
                            s: {
                                path: [],
                                offset: 0
                            },
                            e: {
                                path: [],
                                offset: 0
                            }
                        };
                    return {
                        contents: a.html(),
                        bookmark: b ? b.bookmark(d) : c
                    }
                },
                f = function (b) {
                    null !== b.contents && a.html(b.contents), null !== b.bookmark && range.createFromBookmark(d, b.bookmark).select()
                };
            this.rewind = function () {
                a.html() !== b[c].contents && this.recordUndo(), c = 0, f(b[c])
            }, this.reset = function () {
                b = [], c = -1, a.html(""), this.recordUndo()
            }, this.undo = function () {
                a.html() !== b[c].contents && this.recordUndo(), c > 0 && (c--, f(b[c]))
            }, this.redo = function () {
                b.length - 1 > c && (c++, f(b[c]))
            }, this.recordUndo = function () {
                c++, b.length > c && (b = b.slice(0, c)), b.push(e())
            }
        },
        Style = function () {
            var a = function (a, b) {
                if (agent.jqueryVersion < 1.9) {
                    var c = {};
                    return $.each(b, function (b, d) {
                        c[d] = a.css(d)
                    }), c
                }
                return a.css.call(a, b)
            };
            this.fromNode = function (b) {
                var c = ["font-family", "font-size", "text-align", "list-style-type", "line-height"],
                    d = a(b, c) || {};
                return d["font-size"] = parseInt(d["font-size"], 10), d
            }, this.stylePara = function (a, b) {
                $.each(a.nodes(dom.isPara, {
                    includeAncestor: !0
                }), function (a, c) {
                    $(c).css(b)
                })
            }, this.styleNodes = function (a, b) {
                a = a.splitText();
                var c = b && b.nodeName || "SPAN",
                    d = !(!b || !b.expandClosestSibling),
                    e = !(!b || !b.onlyPartialContains);
                if (a.isCollapsed()) return [a.insertNode(dom.create(c))];
                var f = dom.makePredByNodeName(c),
                    g = a.nodes(dom.isText, {
                        fullyContains: !0
                    }).map(function (a) {
                        return dom.singleChildAncestor(a, f) || dom.wrap(a, c)
                    });
                if (d) {
                    if (e) {
                        var h = a.nodes();
                        f = func.and(f, function (a) {
                            return list.contains(h, a)
                        })
                    }
                    return g.map(function (a) {
                        var b = dom.withClosestSiblings(a, f),
                            c = list.head(b),
                            d = list.tail(b);
                        return $.each(d, function (a, b) {
                            dom.appendChildNodes(c, b.childNodes), dom.remove(b)
                        }), list.head(b)
                    })
                }
                return g
            }, this.current = function (a) {
                var b = $(dom.isElement(a.sc) ? a.sc : a.sc.parentNode),
                    c = this.fromNode(b);
                try {
                    c = $.extend(c, {
                        "font-bold": document.queryCommandState("bold") ? "bold" : "normal",
                        "font-italic": document.queryCommandState("italic") ? "italic" : "normal",
                        "font-underline": document.queryCommandState("underline") ? "underline" : "normal",
                        "font-subscript": document.queryCommandState("subscript") ? "subscript" : "normal",
                        "font-superscript": document.queryCommandState("superscript") ? "superscript" : "normal",
                        "font-strikethrough": document.queryCommandState("strikethrough") ? "strikethrough" : "normal"
                    })
                } catch (d) { }
                if (a.isOnList()) {
                    var e = ["circle", "disc", "disc-leading-zero", "square"],
                        f = $.inArray(c["list-style-type"], e) > -1;
                    c["list-style"] = f ? "unordered" : "ordered"
                } else c["list-style"] = "none";
                var g = dom.ancestor(a.sc, dom.isPara);
                if (g && g.style["line-height"]) c["line-height"] = g.style.lineHeight;
                else {
                    var h = parseInt(c["line-height"], 10) / parseInt(c["font-size"], 10);
                    c["line-height"] = h.toFixed(1)
                }
                return c.anchor = a.isOnAnchor() && dom.ancestor(a.sc, dom.isAnchor), c.ancestors = dom.listAncestor(a.sc, dom.isEditable), c.range = a, c
            }
        },
        Bullet = function () {
            var a = this;
            this.insertOrderedList = function (a) {
                this.toggleList("OL", a)
            }, this.insertUnorderedList = function (a) {
                this.toggleList("UL", a)
            }, this.indent = function (a) {
                var b = this,
                    c = range.create(a).wrapBodyInlineWithPara(),
                    d = c.nodes(dom.isPara, {
                        includeAncestor: !0
                    }),
                    e = list.clusterBy(d, func.peq2("parentNode"));
                $.each(e, function (a, c) {
                    var d = list.head(c);
                    dom.isLi(d) ? b.wrapList(c, d.parentNode.nodeName) : $.each(c, function (a, b) {
                        $(b).css("marginLeft", function (a, b) {
                            return (parseInt(b, 10) || 0) + 25
                        })
                    })
                }), c.select()
            }, this.outdent = function (a) {
                var b = this,
                    c = range.create(a).wrapBodyInlineWithPara(),
                    d = c.nodes(dom.isPara, {
                        includeAncestor: !0
                    }),
                    e = list.clusterBy(d, func.peq2("parentNode"));
                $.each(e, function (a, c) {
                    var d = list.head(c);
                    dom.isLi(d) ? b.releaseList([c]) : $.each(c, function (a, b) {
                        $(b).css("marginLeft", function (a, b) {
                            return b = parseInt(b, 10) || 0, b > 25 ? b - 25 : ""
                        })
                    })
                }), c.select()
            }, this.toggleList = function (b, c) {
                var d = range.create(c).wrapBodyInlineWithPara(),
                    e = d.nodes(dom.isPara, {
                        includeAncestor: !0
                    }),
                    f = d.paraBookmark(e),
                    g = list.clusterBy(e, func.peq2("parentNode"));
                if (list.find(e, dom.isPurePara)) {
                    var h = [];
                    $.each(g, function (c, d) {
                        h = h.concat(a.wrapList(d, b))
                    }), e = h
                } else {
                    var i = d.nodes(dom.isList, {
                        includeAncestor: !0
                    }).filter(function (a) {
                        return !$.nodeName(a, b)
                    });
                    i.length ? $.each(i, function (a, c) {
                        dom.replace(c, b)
                    }) : e = this.releaseList(g, !0)
                }
                range.createFromParaBookmark(f, e).select()
            }, this.wrapList = function (a, b) {
                var c = list.head(a),
                    d = list.last(a),
                    e = dom.isList(c.previousSibling) && c.previousSibling,
                    f = dom.isList(d.nextSibling) && d.nextSibling,
                    g = e || dom.insertAfter(dom.create(b || "UL"), d);
                return a = a.map(function (a) {
                    return dom.isPurePara(a) ? dom.replace(a, "LI") : a
                }), dom.appendChildNodes(g, a), f && (dom.appendChildNodes(g, list.from(f.childNodes)), dom.remove(f)), a
            }, this.releaseList = function (a, b) {
                var c = [];
                return $.each(a, function (a, d) {
                    var e = list.head(d),
                        f = list.last(d),
                        g = b ? dom.lastAncestor(e, dom.isList) : e.parentNode,
                        h = g.childNodes.length > 1 ? dom.splitTree(g, {
                            node: f.parentNode,
                            offset: dom.position(f) + 1
                        }, {
                            isSkipPaddingBlankHTML: !0
                        }) : null,
                        i = dom.splitTree(g, {
                            node: e.parentNode,
                            offset: dom.position(e)
                        }, {
                            isSkipPaddingBlankHTML: !0
                        });
                    d = b ? dom.listDescendant(i, dom.isLi) : list.from(i.childNodes).filter(dom.isLi), (b || !dom.isList(g.parentNode)) && (d = d.map(function (a) {
                        return dom.replace(a, "P")
                    })), $.each(list.from(d).reverse(), function (a, b) {
                        dom.insertAfter(b, g)
                    });
                    var j = list.compact([g, i, h]);
                    $.each(j, function (a, b) {
                        var c = [b].concat(dom.listDescendant(b, dom.isList));
                        $.each(c.reverse(), function (a, b) {
                            dom.nodeLength(b) || dom.remove(b, !0)
                        })
                    }), c = c.concat(d)
                }), c
            }
        },
        Typing = function () {
            var a = new Bullet;
            this.insertTab = function (a, b) {
                var c = dom.createText(new Array(b + 1).join(dom.NBSP_CHAR));
                a = a.deleteContents(), a.insertNode(c, !0), a = range.create(c, b), a.select()
            }, this.insertParagraph = function (b) {
                var c = range.create(b);
                c = c.deleteContents(), c = c.wrapBodyInlineWithPara();
                var d, e = dom.ancestor(c.sc, dom.isPara);
                if (e) {
                    if (dom.isEmpty(e) && dom.isLi(e)) return void a.toggleList(e.parentNode.nodeName);
                    if (dom.isEmpty(e) && dom.isPara(e) && dom.isBlockquote(e.parentNode)) dom.insertAfter(e, e.parentNode), d = e;
                    else {
                        d = dom.splitTree(e, c.getStartPoint());
                        var f = dom.listDescendant(e, dom.isEmptyAnchor);
                        f = f.concat(dom.listDescendant(d, dom.isEmptyAnchor)), $.each(f, function (a, b) {
                            dom.remove(b)
                        }), (dom.isHeading(d) || dom.isPre(d)) && dom.isEmpty(d) && (d = dom.replace(d, "p"))
                    }
                } else {
                    var g = c.sc.childNodes[c.so];
                    d = $(dom.emptyPara)[0], g ? c.sc.insertBefore(d, g) : c.sc.appendChild(d)
                }
                range.create(d, 0).normalize().select().scrollIntoView(b)
            }
        },
        Table = function () {
            this.tab = function (a, b) {
                var c = dom.ancestor(a.commonAncestor(), dom.isCell),
                    d = dom.ancestor(c, dom.isTable),
                    e = dom.listDescendant(d, dom.isCell),
                    f = list[b ? "prev" : "next"](e, c);
                f && range.create(f, 0).select()
            }, this.createTable = function (a, b, c) {
                for (var d, e = [], f = 0; a > f; f++) e.push("<td>" + dom.blank + "</td>");
                d = e.join("");
                for (var g, h = [], i = 0; b > i; i++) h.push("<tr>" + d + "</tr>");
                g = h.join("");
                var j = $("<table>" + g + "</table>");
                return c && c.tableClassName && j.addClass(c.tableClassName), j[0]
            }
        },
        KEY_BOGUS = "bogus",
        Editor = function (a) {
            var b = this,
                c = a.layoutInfo.note,
                d = a.layoutInfo.editor,
                e = a.layoutInfo.editable,
                f = a.options,
                g = f.langInfo,
                h = e[0],
                i = null,
                j = new Style,
                k = new Table,
                l = new Typing,
                m = new Bullet,
                n = new History(e);
            this.initialize = function () {
                e.on("keydown", function (c) {
                    c.keyCode === key.code.ENTER && a.triggerEvent("enter", c), a.triggerEvent("keydown", c), c.isDefaultPrevented() || (f.shortcuts ? b.handleKeyMap(c) : b.preventDefaultEditableShortCuts(c))
                }).on("keyup", function (b) {
                    a.triggerEvent("keyup", b)
                }).on("focus", function (b) {
                    a.triggerEvent("focus", b)
                }).on("blur", function (b) {
                    a.triggerEvent("blur", b)
                }).on("mousedown", function (b) {
                    a.triggerEvent("mousedown", b)
                }).on("mouseup", function (b) {
                    a.triggerEvent("mouseup", b)
                }).on("scroll", function (b) {
                    a.triggerEvent("scroll", b)
                }).on("paste", function (b) {
                    a.triggerEvent("paste", b)
                }), e.html(dom.html(c) || dom.emptyPara);
                var g = agent.isMSIE ? "DOMCharacterDataModified DOMSubtreeModified DOMNodeInserted" : "input";
                e.on(g, func.debounce(function () {
                    a.triggerEvent("change", e.html())
                }, 250)), d.on("focusin", function (b) {
                    a.triggerEvent("focusin", b)
                }).on("focusout", function (b) {
                    a.triggerEvent("focusout", b)
                }), f.airMode || (f.width && d.outerWidth(f.width), f.height && e.outerHeight(f.height), f.maxHeight && e.css("max-height", f.maxHeight), f.minHeight && e.css("min-height", f.minHeight)), n.recordUndo()
            }, this.destroy = function () {
                e.off()
            }, this.handleKeyMap = function (b) {
                var c = f.keyMap[agent.isMac ? "mac" : "pc"],
                    d = [];
                b.metaKey && d.push("CMD"), b.ctrlKey && !b.altKey && d.push("CTRL"), b.shiftKey && d.push("SHIFT");
                var e = key.nameFromCode[b.keyCode];
                e && d.push(e);
                var g = c[d.join("+")];
                g ? (b.preventDefault(), a.invoke(g)) : key.isEdit(b.keyCode) && this.afterCommand()
            }, this.preventDefaultEditableShortCuts = function (a) {
                (a.ctrlKey || a.metaKey) && list.contains([66, 73, 85], a.keyCode) && a.preventDefault()
            }, this.createRange = function () {
                return this.focus(), range.create(h)
            }, this.saveRange = function (a) {
                i = this.createRange(), a && i.collapse().select()
            }, this.restoreRange = function () {
                i && (i.select(), this.focus())
            }, this.saveTarget = function (a) {
                e.data("target", a)
            }, this.clearTarget = function () {
                e.removeData("target")
            }, this.restoreTarget = function () {
                return e.data("target")
            }, this.currentStyle = function () {
                var a = range.create();
                return a && (a = a.normalize()), a ? j.current(a) : j.fromNode(e)
            }, this.styleFromNode = function (a) {
                return j.fromNode(a)
            }, this.undo = function () {
                a.triggerEvent("before.command", e.html()), n.undo(), a.triggerEvent("change", e.html())
            }, a.memo("help.undo", g.help.undo), this.redo = function () {
                a.triggerEvent("before.command", e.html()), n.redo(), a.triggerEvent("change", e.html())
            }, a.memo("help.redo", g.help.redo);
            for (var o = this.beforeCommand = function () {
                    a.triggerEvent("before.command", e.html()), b.focus()
            }, p = this.afterCommand = function (b) {
                    n.recordUndo(), b || a.triggerEvent("change", e.html())
            }, q = ["bold", "italic", "underline", "strikethrough", "superscript", "subscript", "justifyLeft", "justifyCenter", "justifyRight", "justifyFull", "formatBlock", "removeFormat", "backColor", "foreColor", "fontName"], r = 0, s = q.length; s > r; r++) this[q[r]] = function (a) {
                return function (b) {
                    o(), document.execCommand(a, !1, b), p(!0)
                }
            }(q[r]), a.memo("help." + q[r], g.help[q[r]]);
            this.tab = function () {
                var a = this.createRange();
                a.isCollapsed() && a.isOnCell() ? k.tab(a) : (o(), l.insertTab(a, f.tabSize), p())
            }, a.memo("help.tab", g.help.tab), this.untab = function () {
                var a = this.createRange();
                a.isCollapsed() && a.isOnCell() && k.tab(a, !0)
            }, a.memo("help.untab", g.help.untab), this.wrapCommand = function (a) {
                return function () {
                    o(), a.apply(b, arguments), p()
                }
            }, this.insertParagraph = this.wrapCommand(function () {
                l.insertParagraph(h)
            }), a.memo("help.insertParagraph", g.help.insertParagraph), this.insertOrderedList = this.wrapCommand(function () {
                m.insertOrderedList(h)
            }), a.memo("help.insertOrderedList", g.help.insertOrderedList), this.insertUnorderedList = this.wrapCommand(function () {
                m.insertUnorderedList(h)
            }), a.memo("help.insertUnorderedList", g.help.insertUnorderedList), this.indent = this.wrapCommand(function () {
                m.indent(h)
            }), a.memo("help.indent", g.help.indent), this.outdent = this.wrapCommand(function () {
                m.outdent(h)
            }), a.memo("help.outdent", g.help.outdent), this.insertImage = function (b, c) {
                return async.createImage(b, c).then(function (a) {
                    o(), "function" == typeof c ? c(a) : ("string" == typeof c && a.attr("data-filename", c), a.css("width", Math.min(e.width(), a.width()))), a.show(), range.create(h).insertNode(a[0]), range.createFromNodeAfter(a[0]).select(), p()
                }).fail(function (b) {
                    a.triggerEvent("image.upload.error", b)
                })
            }, this.insertImages = function (c) {
                $.each(c, function (c, d) {
                    var e = d.name;
                    f.maximumImageFileSize && f.maximumImageFileSize < d.size ? a.triggerEvent("image.upload.error", g.image.maximumFileSizeError) : async.readFileAsDataURL(d).then(function (a) {
                        return b.insertImage(a, e)
                    }).fail(function () {
                        a.triggerEvent("image.upload.error")
                    })
                })
            }, this.insertImagesOrCallback = function (b) {
                var c = f.callbacks;
                c.onImageUpload ? a.triggerEvent("image.upload", b) : this.insertImages(b)
            }, this.insertNode = this.wrapCommand(function (a) {
                var b = this.createRange();
                b.insertNode(a), range.createFromNodeAfter(a).select()
            }), this.insertText = this.wrapCommand(function (a) {
                var b = this.createRange(),
                    c = b.insertNode(dom.createText(a));
                range.create(c, dom.nodeLength(c)).select()
            }), this.getSelectedText = function () {
                var a = this.createRange();
                return a.isOnAnchor() && (a = range.createFromNode(dom.ancestor(a.sc, dom.isAnchor))), a.toString()
            }, this.pasteHTML = this.wrapCommand(function (a) {
                var b = this.createRange().pasteHTML(a);
                range.createFromNodeAfter(list.last(b)).select()
            }), this.formatBlock = this.wrapCommand(function (a) {
                a = agent.isMSIE ? "<" + a + ">" : a, document.execCommand("FormatBlock", !1, a)
            }), this.formatPara = function () {
                this.formatBlock("P")
            }, a.memo("help.formatPara", g.help.formatPara);
            for (var r = 1; 6 >= r; r++) this["formatH" + r] = function (a) {
                return function () {
                    this.formatBlock("H" + a)
                }
            }(r), a.memo("help.formatH" + r, g.help["formatH" + r]);
            this.fontSize = function (a) {
                var b = this.createRange();
                if (b && b.isCollapsed()) {
                    var c = j.styleNodes(b),
                        d = list.head(c);
                    $(c).css({
                        "font-size": a + "px"
                    }), d && !dom.nodeLength(d) && (d.innerHTML = dom.ZERO_WIDTH_NBSP_CHAR, range.createFromNodeAfter(d.firstChild).select(), e.data(KEY_BOGUS, d))
                } else o(), $(j.styleNodes(b)).css({
                    "font-size": a + "px"
                }), p()
            }, this.insertHorizontalRule = this.wrapCommand(function () {
                var a = this.createRange().insertNode(dom.create("HR"));
                a.nextSibling && range.create(a.nextSibling, 0).normalize().select()
            }), a.memo("help.insertHorizontalRule", g.help.insertHorizontalRule), this.removeBogus = function () {
                var a = e.data(KEY_BOGUS);
                if (a) {
                    var b = list.find(list.from(a.childNodes), dom.isText),
                        c = b.nodeValue.indexOf(dom.ZERO_WIDTH_NBSP_CHAR); -1 !== c && b.deleteData(c, 1), dom.isEmpty(a) && dom.remove(a), e.removeData(KEY_BOGUS)
                }
            }, this.lineHeight = this.wrapCommand(function (a) {
                j.stylePara(this.createRange(), {
                    lineHeight: a
                })
            }), this.unlink = function () {
                var a = this.createRange();
                if (a.isOnAnchor()) {
                    var b = dom.ancestor(a.sc, dom.isAnchor);
                    a = range.createFromNode(b), a.select(), o(), document.execCommand("unlink"), p()
                }
            }, this.createLink = this.wrapCommand(function (a) {
                var b = a.url,
                    c = a.text,
                    d = a.isNewWindow,
                    e = a.range || this.createRange(),
                    g = e.toString() !== c;
                "string" == typeof b && (b = b.trim()), f.onCreateLink && (b = f.onCreateLink(b));
                var h = [];
                if (g) {
                    e = e.deleteContents();
                    var i = e.insertNode($("<A>" + c + "</A>")[0]);
                    h.push(i)
                } else h = j.styleNodes(e, {
                    nodeName: "A",
                    expandClosestSibling: !0,
                    onlyPartialContains: !0
                });
                $.each(h, function (a, c) {
                    b = /^[A-Za-z][A-Za-z0-9+-.]*\:[\/\/]?/.test(b) ? b : "http://" + b, $(c).attr("href", b), d ? $(c).attr("target", "_blank") : $(c).removeAttr("target")
                });
                var k = range.createFromNodeBefore(list.head(h)),
                    l = k.getStartPoint(),
                    m = range.createFromNodeAfter(list.last(h)),
                    n = m.getEndPoint();
                range.create(l.node, l.offset, n.node, n.offset).select()
            }), this.getLinkInfo = function () {
                var a = this.createRange().expand(dom.isAnchor),
                    b = $(list.head(a.nodes(dom.isAnchor)));
                return {
                    range: a,
                    text: a.toString(),
                    isNewWindow: b.length ? "_blank" === b.attr("target") : !1,
                    url: b.length ? b.attr("href") : ""
                }
            }, this.color = this.wrapCommand(function (a) {
                var b = a.foreColor,
                    c = a.backColor;
                b && document.execCommand("foreColor", !1, b), c && document.execCommand("backColor", !1, c)
            }), this.insertTable = this.wrapCommand(function (a) {
                var b = a.split("x"),
                    c = this.createRange().deleteContents();
                c.insertNode(k.createTable(b[0], b[1], f))
            }), this.floatMe = this.wrapCommand(function (a) {
                var b = $(this.restoreTarget());
                b.css("float", a)
            }), this.resize = this.wrapCommand(function (a) {
                var b = $(this.restoreTarget());
                b.css({
                    width: 100 * a + "%",
                    height: ""
                })
            }), this.resizeTo = function (a, b, c) {
                var d;
                if (c) {
                    var e = a.y / a.x,
                        f = b.data("ratio");
                    d = {
                        width: f > e ? a.x : a.y / f,
                        height: f > e ? a.x * f : a.y
                    }
                } else d = {
                    width: a.x,
                    height: a.y
                };
                b.css(d)
            }, this.removeMedia = this.wrapCommand(function () {
                var b = $(this.restoreTarget()).detach();
                a.triggerEvent("media.delete", b, e)
            }), this.hasFocus = function () {
                return e.is(":focus")
            }, this.focus = function () {
                this.hasFocus() || e.focus()
            }, this.isEmpty = function () {
                return dom.isEmpty(e[0]) || dom.emptyPara === e.html()
            }, this.empty = function () {
                a.invoke("code", dom.emptyPara)
            }
        },
        Clipboard = function (a) {
            var b = this,
                c = a.layoutInfo.editable;
            this.events = {
                "summernote.keydown": function (c, d) {
                    b.needKeydownHook() && (d.ctrlKey || d.metaKey) && d.keyCode === key.code.V && (a.invoke("editor.saveRange"), b.$paste.focus(), setTimeout(function () {
                        b.pasteByHook()
                    }, 0))
                }
            }, this.needKeydownHook = function () {
                return agent.isMSIE && agent.browserVersion > 10 || agent.isFF
            }, this.initialize = function () {
                this.needKeydownHook() ? (this.$paste = $('<div tabindex="-1" />').attr("contenteditable", !0).css({
                    position: "absolute",
                    left: -1e5,
                    opacity: 0
                }), c.before(this.$paste), this.$paste.on("paste", function (b) {
                    a.triggerEvent("paste", b)
                })) : c.on("paste", this.pasteByEvent)
            }, this.destroy = function () {
                this.needKeydownHook() && (this.$paste.remove(), this.$paste = null)
            }, this.pasteByHook = function () {
                var b = this.$paste[0].firstChild;
                if (dom.isImg(b)) {
                    for (var c = b.src, d = atob(c.split(",")[1]), e = new Uint8Array(d.length), f = 0; f < d.length; f++) e[f] = d.charCodeAt(f);
                    var g = new Blob([e], {
                        type: "image/png"
                    });
                    g.name = "clipboard.png", a.invoke("editor.restoreRange"), a.invoke("editor.focus"), a.invoke("editor.insertImagesOrCallback", [g])
                } else {
                    var h = $("<div />").html(this.$paste.html()).html();
                    a.invoke("editor.restoreRange"), a.invoke("editor.focus"), h && a.invoke("editor.pasteHTML", h)
                }
                this.$paste.empty()
            }, this.pasteByEvent = function (b) {
                var c = b.originalEvent.clipboardData;
                if (c && c.items && c.items.length) {
                    var d = list.head(c.items);
                    "file" === d.kind && -1 !== d.type.indexOf("image/") && a.invoke("editor.insertImagesOrCallback", [d.getAsFile()]), a.invoke("editor.afterCommand")
                }
            }
        },
        Dropzone = function (a) {
            var b = $(document),
                c = a.layoutInfo.editor,
                d = a.layoutInfo.editable,
                e = a.options,
                f = e.langInfo,
                g = {},
                h = $(['<div class="note-dropzone">', '  <div class="note-dropzone-message"/>', "</div>"].join("")).prependTo(c),
                i = function () {
                    Object.keys(g).forEach(function (a) {
                        b.off(a.substr(2).toLowerCase(), g[a])
                    }), g = {}
                };
            this.initialize = function () {
                e.disableDragAndDrop ? (g.onDrop = function (a) {
                    a.preventDefault()
                }, b.on("drop", g.onDrop)) : this.attachDragAndDropEvent()
            }, this.attachDragAndDropEvent = function () {
                var e = $(),
                    i = h.find(".note-dropzone-message");
                g.onDragenter = function (b) {
                    var d = a.invoke("codeview.isActivated"),
                        g = c.width() > 0 && c.height() > 0;
                    d || e.length || !g || (c.addClass("dragover"), h.width(c.width()), h.height(c.height()), i.text(f.image.dragImageHere)), e = e.add(b.target)
                }, g.onDragleave = function (a) {
                    e = e.not(a.target), e.length || c.removeClass("dragover")
                }, g.onDrop = function () {
                    e = $(), c.removeClass("dragover")
                }, b.on("dragenter", g.onDragenter).on("dragleave", g.onDragleave).on("drop", g.onDrop), h.on("dragenter", function () {
                    h.addClass("hover"), i.text(f.image.dropImage)
                }).on("dragleave", function () {
                    h.removeClass("hover"), i.text(f.image.dragImageHere)
                }), h.on("drop", function (b) {
                    var c = b.originalEvent.dataTransfer;
                    c && c.files && c.files.length ? (b.preventDefault(), d.focus(), a.invoke("editor.insertImagesOrCallback", c.files)) : $.each(c.types, function (b, d) {
                        var e = c.getData(d);
                        d.toLowerCase().indexOf("text") > -1 ? a.invoke("editor.pasteHTML", e) : $(e).each(function () {
                            a.invoke("editor.insertNode", this)
                        })
                    })
                }).on("dragover", !1)
            }, this.destroy = function () {
                i()
            }
        },
        CodeMirror;
    agent.hasCodeMirror && (agent.isSupportAmd ? require(["codemirror"], function (a) {
        CodeMirror = a
    }) : CodeMirror = window.CodeMirror);
    var Codeview = function (a) {
        var b = a.layoutInfo.editor,
            c = a.layoutInfo.editable,
            d = a.layoutInfo.codable,
            e = a.options;
        this.sync = function () {
            var a = this.isActivated();
            a && agent.hasCodeMirror && d.data("cmEditor").save()
        }, this.isActivated = function () {
            return b.hasClass("codeview")
        }, this.toggle = function () {
            this.isActivated() ? this.deactivate() : this.activate(), a.triggerEvent("codeview.toggled")
        }, this.activate = function () {
            if (d.val(dom.html(c, e.prettifyHtml)), d.height(c.height()), a.invoke("toolbar.updateCodeview", !0), b.addClass("codeview"), d.focus(), agent.hasCodeMirror) {
                var f = CodeMirror.fromTextArea(d[0], e.codemirror);
                if (e.codemirror.tern) {
                    var g = new CodeMirror.TernServer(e.codemirror.tern);
                    f.ternServer = g, f.on("cursorActivity", function (a) {
                        g.updateArgHints(a)
                    })
                }
                f.setSize(null, c.outerHeight()), d.data("cmEditor", f)
            }
        }, this.deactivate = function () {
            if (agent.hasCodeMirror) {
                var f = d.data("cmEditor");
                d.val(f.getValue()), f.toTextArea()
            }
            var g = dom.value(d, e.prettifyHtml) || dom.emptyPara,
                h = c.html() !== g;
            c.html(g), c.height(e.height ? d.height() : "auto"), b.removeClass("codeview"), h && a.triggerEvent("change", c.html(), c), c.focus(), a.invoke("toolbar.updateCodeview", !1)
        }, this.destroy = function () {
            this.isActivated() && this.deactivate()
        }
    },
        EDITABLE_PADDING = 24,
        Statusbar = function (a) {
            var b = $(document),
                c = a.layoutInfo.statusbar,
                d = a.layoutInfo.editable,
                e = a.options;
            this.initialize = function () {
                e.airMode || e.disableResizeEditor || c.on("mousedown", function (a) {
                    a.preventDefault(), a.stopPropagation();
                    var c = d.offset().top - b.scrollTop();
                    b.on("mousemove", function (a) {
                        var b = a.clientY - (c + EDITABLE_PADDING);
                        b = e.minheight > 0 ? Math.max(b, e.minheight) : b, b = e.maxHeight > 0 ? Math.min(b, e.maxHeight) : b, d.height(b)
                    }).one("mouseup", function () {
                        b.off("mousemove")
                    })
                })
            }, this.destroy = function () {
                c.off(), c.remove()
            }
        },
        Fullscreen = function (a) {
            var b = a.layoutInfo.editor,
                c = a.layoutInfo.toolbar,
                d = a.layoutInfo.editable,
                e = a.layoutInfo.codable,
                f = $(window),
                g = $("html, body");
            this.toggle = function () {
                var h = function (a) {
                    d.css("height", a.h), e.css("height", a.h), e.data("cmeditor") && e.data("cmeditor").setsize(null, a.h)
                };
                b.toggleClass("fullscreen"), this.isFullscreen() ? (d.data("orgHeight", d.css("height")), f.on("resize", function () {
                    h({
                        h: f.height() - c.outerHeight()
                    })
                }).trigger("resize"), g.css("overflow", "hidden")) : (f.off("resize"), h({
                    h: d.data("orgHeight")
                }), g.css("overflow", "visible")), a.invoke("toolbar.updateFullscreen", this.isFullscreen())
            }, this.isFullscreen = function () {
                return b.hasClass("fullscreen")
            }
        },
        Handle = function (a) {
            var b = this,
                c = $(document),
                d = a.layoutInfo.editingArea,
                e = a.options;
            this.events = {
                "summernote.mousedown": function (a, c) {
                    b.update(c.target) && c.preventDefault()
                },
                "summernote.keyup summernote.scroll summernote.change summernote.dialog.shown": function () {
                    b.update()
                }
            }, this.initialize = function () {
                this.$handle = $(['<div class="note-handle">', '<div class="note-control-selection">', '<div class="note-control-selection-bg"></div>', '<div class="note-control-holder note-control-nw"></div>', '<div class="note-control-holder note-control-ne"></div>', '<div class="note-control-holder note-control-sw"></div>', '<div class="', e.disableResizeImage ? "note-control-holder" : "note-control-sizing", ' note-control-se"></div>', e.disableResizeImage ? "" : '<div class="note-control-selection-info"></div>', "</div>", "</div>"].join("")).prependTo(d), this.$handle.on("mousedown", function (d) {
                    if (dom.isControlSizing(d.target)) {
                        d.preventDefault(), d.stopPropagation();
                        var e = b.$handle.find(".note-control-selection").data("target"),
                            f = e.offset(),
                            g = c.scrollTop();
                        c.on("mousemove", function (c) {
                            a.invoke("editor.resizeTo", {
                                x: c.clientX - f.left,
                                y: c.clientY - (f.top - g)
                            }, e, !c.shiftKey), b.update(e[0])
                        }).one("mouseup", function (b) {
                            b.preventDefault(), c.off("mousemove"), a.invoke("editor.afterCommand")
                        }), e.data("ratio") || e.data("ratio", e.height() / e.width())
                    }
                })
            }, this.destroy = function () {
                this.$handle.remove()
            }, this.update = function (b) {
                var c = dom.isImg(b),
                    d = this.$handle.find(".note-control-selection");
                if (a.invoke("imagePopover.update", b), c) {
                    var e = $(b),
                        f = e.position(),
                        g = {
                            w: e.outerWidth(!0),
                            h: e.outerHeight(!0)
                        };
                    d.css({
                        display: "block",
                        left: f.left,
                        top: f.top,
                        width: g.w,
                        height: g.h
                    }).data("target", e);
                    var h = g.w + "x" + g.h;
                    d.find(".note-control-selection-info").text(h), a.invoke("editor.saveTarget", b)
                } else this.hide();
                return c
            }, this.hide = function () {
                a.invoke("editor.clearTarget"), this.$handle.children().hide()
            }
        },
        AutoLink = function (a) {
            var b = this,
                c = "http://",
                d = /^([A-Za-z][A-Za-z0-9+-.]*\:[\/\/]?|mailto:[A-Z0-9._%+-]+@)?(www\.)?(.+)$/i;
            this.events = {
                "summernote.keyup": function (a, c) {
                    c.isDefaultPrevented() || b.handleKeyup(c)
                },
                "summernote.keydown": function (a, c) {
                    b.handleKeydown(c)
                }
            }, this.initialize = function () {
                this.lastWordRange = null
            }, this.destroy = function () {
                this.lastWordRange = null
            }, this.replace = function () {
                if (this.lastWordRange) {
                    var b = this.lastWordRange.toString(),
                        e = b.match(d);
                    if (e && (e[1] || e[2])) {
                        var f = e[1] ? b : c + b,
                            g = $("<a />").html(b).attr("href", f)[0];
                        this.lastWordRange.insertNode(g), this.lastWordRange = null, a.invoke("editor.focus")
                    }
                }
            }, this.handleKeydown = function (b) {
                if (list.contains([key.code.ENTER, key.code.SPACE], b.keyCode)) {
                    var c = a.invoke("editor.createRange").getWordRange();
                    this.lastWordRange = c
                }
            }, this.handleKeyup = function (a) {
                list.contains([key.code.ENTER, key.code.SPACE], a.keyCode) && this.replace()
            }
        },
        AutoSync = function (a) {
            var b = a.layoutInfo.note;
            this.events = {
                "summernote.change": function () {
                    b.val(a.invoke("code"))
                }
            }, this.shouldInitialize = function () {
                return dom.isTextarea(b[0])
            }
        },
        Placeholder = function (a) {
            var b = this,
                c = a.layoutInfo.editingArea,
                d = a.options;
            this.events = {
                "summernote.init summernote.change": function () {
                    b.update()
                },
                "summernote.codeview.toggled": function () {
                    b.update()
                }
            }, this.shouldInitialize = function () {
                return !!d.placeholder
            }, this.initialize = function () {
                this.$placeholder = $('<div class="note-placeholder">'), this.$placeholder.on("click", function () {
                    a.invoke("focus")
                }).text(d.placeholder).prependTo(c)
            }, this.destroy = function () {
                this.$placeholder.remove()
            }, this.update = function () {
                var b = !a.invoke("codeview.isActivated") && a.invoke("editor.isEmpty");
                this.$placeholder.toggle(b)
            }
        },
        Buttons = function (a) {
            var b = this,
                c = $.summernote.ui,
                d = a.layoutInfo.toolbar,
                e = a.options,
                f = e.langInfo,
                g = func.invertObject(e.keyMap[agent.isMac ? "mac" : "pc"]),
                h = this.representShortcut = function (a) {
                    var b = g[a];
                    return e.shortcuts && b ? (agent.isMac && (b = b.replace("CMD", "⌘").replace("SHIFT", "⇧")), b = b.replace("BACKSLASH", "\\").replace("SLASH", "/").replace("LEFTBRACKET", "[").replace("RIGHTBRACKET", "]"), " (" + b + ")") : ""
                };
            this.initialize = function () {
                this.addToolbarButtons(), this.addImagePopoverButtons(), this.addLinkPopoverButtons(), this.fontInstalledMap = {}
            }, this.destroy = function () {
                delete this.fontInstalledMap
            }, this.isFontInstalled = function (a) {
                return b.fontInstalledMap.hasOwnProperty(a) || (b.fontInstalledMap[a] = agent.isFontInstalled(a) || list.contains(e.fontNamesIgnoreCheck, a)), b.fontInstalledMap[a]
            }, this.addToolbarButtons = function () {
                a.memo("button.style", function () {
                    return c.buttonGroup([c.button({
                        className: "dropdown-toggle",
                        contents: c.icon(e.icons.magic) + " " + c.icon(e.icons.caret, "span"),
                        tooltip: f.style.style,
                        data: {
                            toggle: "dropdown"
                        }
                    }), c.dropdown({
                        className: "dropdown-style",
                        items: a.options.styleTags,
                        template: function (a) {
                            "string" == typeof a && (a = {
                                tag: a,
                                title: f.style.hasOwnProperty(a) ? f.style[a] : a
                            });
                            var b = a.tag,
                                c = a.title,
                                d = a.style ? ' style="' + a.style + '" ' : "",
                                e = a.className ? ' class="' + a.className + '"' : "";
                            return "<" + b + d + e + ">" + c + "</" + b + ">"
                        },
                        click: a.createInvokeHandler("editor.formatBlock")
                    })]).render()
                }), a.memo("button.bold", function () {
                    return c.button({
                        className: "note-btn-bold",
                        contents: c.icon(e.icons.bold),
                        tooltip: f.font.bold + h("bold"),
                        click: a.createInvokeHandler("editor.bold")
                    }).render()
                }), a.memo("button.italic", function () {
                    return c.button({
                        className: "note-btn-italic",
                        contents: c.icon(e.icons.italic),
                        tooltip: f.font.italic + h("italic"),
                        click: a.createInvokeHandler("editor.italic")
                    }).render()
                }), a.memo("button.underline", function () {
                    return c.button({
                        className: "note-btn-underline",
                        contents: c.icon(e.icons.underline),
                        tooltip: f.font.underline + h("underline"),
                        click: a.createInvokeHandler("editor.underline")
                    }).render()
                }), a.memo("button.clear", function () {
                    return c.button({
                        contents: c.icon(e.icons.eraser),
                        tooltip: f.font.clear + h("removeFormat"),
                        click: a.createInvokeHandler("editor.removeFormat")
                    }).render()
                }), a.memo("button.strikethrough", function () {
                    return c.button({
                        className: "note-btn-strikethrough",
                        contents: c.icon(e.icons.strikethrough),
                        tooltip: f.font.strikethrough + h("strikethrough"),
                        click: a.createInvokeHandler("editor.strikethrough")
                    }).render()
                }), a.memo("button.superscript", function () {
                    return c.button({
                        className: "note-btn-superscript",
                        contents: c.icon(e.icons.superscript),
                        tooltip: f.font.superscript,
                        click: a.createInvokeHandler("editor.superscript")
                    }).render()
                }), a.memo("button.subscript", function () {
                    return c.button({
                        className: "note-btn-subscript",
                        contents: c.icon(e.icons.subscript),
                        tooltip: f.font.subscript,
                        click: a.createInvokeHandler("editor.subscript")
                    }).render()
                }), a.memo("button.fontname", function () {
                    return c.buttonGroup([c.button({
                        className: "dropdown-toggle",
                        contents: '<span class="note-current-fontname"/> ' + c.icon(e.icons.caret, "span"),
                        tooltip: f.font.name,
                        data: {
                            toggle: "dropdown"
                        }
                    }), c.dropdownCheck({
                        className: "dropdown-fontname",
                        checkClassName: e.icons.menuCheck,
                        items: e.fontNames.filter(b.isFontInstalled),
                        template: function (a) {
                            return '<span style="font-family:' + a + '">' + a + "</span>"
                        },
                        click: a.createInvokeHandler("editor.fontName")
                    })]).render()
                }), a.memo("button.fontsize", function () {
                    return c.buttonGroup([c.button({
                        className: "dropdown-toggle",
                        contents: '<span class="note-current-fontsize"/>' + c.icon(e.icons.caret, "span"),
                        tooltip: f.font.size,
                        data: {
                            toggle: "dropdown"
                        }
                    }), c.dropdownCheck({
                        className: "dropdown-fontsize",
                        checkClassName: e.icons.menuCheck,
                        items: e.fontSizes,
                        click: a.createInvokeHandler("editor.fontSize")
                    })]).render()
                }), a.memo("button.color", function () {
                    return c.buttonGroup({
                        className: "note-color",
                        children: [c.button({
                            className: "note-current-color-button",
                            contents: c.icon(e.icons.font + " note-recent-color"),
                            tooltip: f.color.recent,
                            click: function (b) {
                                var c = $(b.currentTarget);
                                a.invoke("editor.color", {
                                    backColor: c.attr("data-backColor"),
                                    foreColor: c.attr("data-foreColor")
                                })
                            },
                            callback: function (a) {
                                var b = a.find(".note-recent-color");
                                b.css("background-color", "#FFFF00"), a.attr("data-backColor", "#FFFF00")
                            }
                        }), c.button({
                            className: "dropdown-toggle",
                            contents: c.icon(e.icons.caret, "span"),
                            tooltip: f.color.more,
                            data: {
                                toggle: "dropdown"
                            }
                        }), c.dropdown({
                            items: ["<li>", '<div class="btn-group">', '  <div class="note-palette-title">' + f.color.background + "</div>", "  <div>", '    <button type="button" class="note-color-reset btn btn-default" data-event="backColor" data-value="inherit">', f.color.transparent, "    </button>", "  </div>", '  <div class="note-holder" data-event="backColor"/>', "</div>", '<div class="btn-group">', '  <div class="note-palette-title">' + f.color.foreground + "</div>", "  <div>", '    <button type="button" class="note-color-reset btn btn-default" data-event="removeFormat" data-value="foreColor">', f.color.resetToDefault, "    </button>", "  </div>", '  <div class="note-holder" data-event="foreColor"/>', "</div>", "</li>"].join(""),
                            callback: function (a) {
                                a.find(".note-holder").each(function () {
                                    var a = $(this);
                                    a.append(c.palette({
                                        colors: e.colors,
                                        eventName: a.data("event")
                                    }).render())
                                })
                            },
                            click: function (b) {
                                var c = $(b.target),
                                    d = c.data("event"),
                                    e = c.data("value");
                                if (d && e) {
                                    var f = "backColor" === d ? "background-color" : "color",
                                        g = c.closest(".note-color").find(".note-recent-color"),
                                        h = c.closest(".note-color").find(".note-current-color-button");
                                    g.css(f, e), h.attr("data-" + d, e), a.invoke("editor." + d, e)
                                }
                            }
                        })]
                    }).render()
                }), a.memo("button.ul", function () {
                    return c.button({
                        contents: c.icon(e.icons.unorderedlist),
                        tooltip: f.lists.unordered + h("insertUnorderedList"),
                        click: a.createInvokeHandler("editor.insertUnorderedList")
                    }).render()
                }), a.memo("button.ol", function () {
                    return c.button({
                        contents: c.icon(e.icons.orderedlist),
                        tooltip: f.lists.ordered + h("insertOrderedList"),
                        click: a.createInvokeHandler("editor.insertOrderedList")
                    }).render()
                });
                var d = c.button({
                    contents: c.icon(e.icons.alignLeft),
                    tooltip: f.paragraph.left + h("justifyLeft"),
                    click: a.createInvokeHandler("editor.justifyLeft")
                }),
                    g = c.button({
                        contents: c.icon(e.icons.alignCenter),
                        tooltip: f.paragraph.center + h("justifyCenter"),
                        click: a.createInvokeHandler("editor.justifyCenter")
                    }),
                    i = c.button({
                        contents: c.icon(e.icons.alignRight),
                        tooltip: f.paragraph.right + h("justifyRight"),
                        click: a.createInvokeHandler("editor.justifyRight")
                    }),
                    j = c.button({
                        contents: c.icon(e.icons.alignJustify),
                        tooltip: f.paragraph.justify + h("justifyFull"),
                        click: a.createInvokeHandler("editor.justifyFull")
                    }),
                    k = c.button({
                        contents: c.icon(e.icons.outdent),
                        tooltip: f.paragraph.outdent + h("outdent"),
                        click: a.createInvokeHandler("editor.outdent")
                    }),
                    l = c.button({
                        contents: c.icon(e.icons.indent),
                        tooltip: f.paragraph.indent + h("indent"),
                        click: a.createInvokeHandler("editor.indent")
                    });
                a.memo("button.justifyLeft", func.invoke(d, "render")), a.memo("button.justifyCenter", func.invoke(g, "render")), a.memo("button.justifyRight", func.invoke(i, "render")), a.memo("button.justifyFull", func.invoke(j, "render")), a.memo("button.outdent", func.invoke(k, "render")), a.memo("button.indent", func.invoke(l, "render")), a.memo("button.paragraph", function () {
                    return c.buttonGroup([c.button({
                        className: "dropdown-toggle",
                        contents: c.icon(e.icons.alignLeft) + " " + c.icon(e.icons.caret, "span"),
                        tooltip: f.paragraph.paragraph,
                        data: {
                            toggle: "dropdown"
                        }
                    }), c.dropdown([c.buttonGroup({
                        className: "note-align",
                        children: [d, g, i, j]
                    }), c.buttonGroup({
                        className: "note-list",
                        children: [k, l]
                    })])]).render()
                }), a.memo("button.height", function () {
                    return c.buttonGroup([c.button({
                        className: "dropdown-toggle",
                        contents: c.icon(e.icons.textHeight) + " " + c.icon(e.icons.caret, "span"),
                        tooltip: f.font.height,
                        data: {
                            toggle: "dropdown"
                        }
                    }), c.dropdownCheck({
                        items: e.lineHeights,
                        checkClassName: e.icons.menuCheck,
                        className: "dropdown-line-height",
                        click: a.createInvokeHandler("editor.lineHeight")
                    })]).render()
                }), a.memo("button.table", function () {
                    return c.buttonGroup([c.button({
                        className: "dropdown-toggle",
                        contents: c.icon(e.icons.table) + " " + c.icon(e.icons.caret, "span"),
                        tooltip: f.table.table,
                        data: {
                            toggle: "dropdown"
                        }
                    }), c.dropdown({
                        className: "note-table",
                        items: ['<div class="note-dimension-picker">', '  <div class="note-dimension-picker-mousecatcher" data-event="insertTable" data-value="1x1"/>', '  <div class="note-dimension-picker-highlighted"/>', '  <div class="note-dimension-picker-unhighlighted"/>', "</div>", '<div class="note-dimension-display">1 x 1</div>'].join("")
                    })], {
                        callback: function (c) {
                            var d = c.find(".note-dimension-picker-mousecatcher");
                            d.css({
                                width: e.insertTableMaxSize.col + "em",
                                height: e.insertTableMaxSize.row + "em"
                            }).mousedown(a.createInvokeHandler("editor.insertTable")).on("mousemove", b.tableMoveHandler)
                        }
                    }).render()
                }), a.memo("button.link", function () {
                    return c.button({
                        contents: c.icon(e.icons.link),
                        tooltip: f.link.link + h("linkDialog.show"),
                        click: a.createInvokeHandler("linkDialog.show")
                    }).render()
                }), a.memo("button.picture", function () {
                    return c.button({
                        contents: c.icon(e.icons.picture),
                        tooltip: f.image.image,
                        click: a.createInvokeHandler("imageDialog.show")
                    }).render()
                }), a.memo("button.video", function () {
                    return c.button({
                        contents: c.icon(e.icons.video),
                        tooltip: f.video.video,
                        click: a.createInvokeHandler("videoDialog.show")
                    }).render()
                }), a.memo("button.hr", function () {
                    return c.button({
                        contents: c.icon(e.icons.minus),
                        tooltip: f.hr.insert + h("insertHorizontalRule"),
                        click: a.createInvokeHandler("editor.insertHorizontalRule")
                    }).render()
                }), a.memo("button.fullscreen", function () {
                    return c.button({
                        className: "btn-fullscreen",
                        contents: c.icon(e.icons.arrowsAlt),
                        tooltip: f.options.fullscreen,
                        click: a.createInvokeHandler("fullscreen.toggle")
                    }).render()
                }), a.memo("button.codeview", function () {
                    return c.button({
                        className: "btn-codeview",
                        contents: c.icon(e.icons.code),
                        tooltip: f.options.codeview,
                        click: a.createInvokeHandler("codeview.toggle")
                    }).render()
                }), a.memo("button.redo", function () {
                    return c.button({
                        contents: c.icon(e.icons.redo),
                        tooltip: f.history.redo + h("redo"),
                        click: a.createInvokeHandler("editor.redo")
                    }).render()
                }), a.memo("button.undo", function () {
                    return c.button({
                        contents: c.icon(e.icons.undo),
                        tooltip: f.history.undo + h("undo"),
                        click: a.createInvokeHandler("editor.undo")
                    }).render()
                }), a.memo("button.help", function () {
                    return c.button({
                        contents: c.icon(e.icons.question),
                        tooltip: f.options.help,
                        click: a.createInvokeHandler("helpDialog.show")
                    }).render()
                })
            }, this.addImagePopoverButtons = function () {
                a.memo("button.imageSize100", function () {
                    return c.button({
                        contents: '<span class="note-fontsize-10">100%</span>',
                        tooltip: f.image.resizeFull,
                        click: a.createInvokeHandler("editor.resize", "1")
                    }).render()
                }), a.memo("button.imageSize50", function () {
                    return c.button({
                        contents: '<span class="note-fontsize-10">50%</span>',
                        tooltip: f.image.resizeHalf,
                        click: a.createInvokeHandler("editor.resize", "0.5")
                    }).render()
                }), a.memo("button.imageSize25", function () {
                    return c.button({
                        contents: '<span class="note-fontsize-10">25%</span>',
                        tooltip: f.image.resizeQuarter,
                        click: a.createInvokeHandler("editor.resize", "0.25")
                    }).render()
                }), a.memo("button.floatLeft", function () {
                    return c.button({
                        contents: c.icon(e.icons.alignLeft),
                        tooltip: f.image.floatLeft,
                        click: a.createInvokeHandler("editor.floatMe", "left")
                    }).render()
                }), a.memo("button.floatRight", function () {
                    return c.button({
                        contents: c.icon(e.icons.alignRight),
                        tooltip: f.image.floatRight,
                        click: a.createInvokeHandler("editor.floatMe", "right")
                    }).render()
                }), a.memo("button.floatNone", function () {
                    return c.button({
                        contents: c.icon(e.icons.alignJustify),
                        tooltip: f.image.floatNone,
                        click: a.createInvokeHandler("editor.floatMe", "none")
                    }).render()
                }), a.memo("button.removeMedia", function () {
                    return c.button({
                        contents: c.icon(e.icons.trash),
                        tooltip: f.image.remove,
                        click: a.createInvokeHandler("editor.removeMedia")
                    }).render()
                })
            }, this.addLinkPopoverButtons = function () {
                a.memo("button.linkDialogShow", function () {
                    return c.button({
                        contents: c.icon(e.icons.link),
                        tooltip: f.link.edit,
                        click: a.createInvokeHandler("linkDialog.show")
                    }).render()
                }), a.memo("button.unlink", function () {
                    return c.button({
                        contents: c.icon(e.icons.unlink),
                        tooltip: f.link.unlink,
                        click: a.createInvokeHandler("editor.unlink")
                    }).render()
                })
            }, this.build = function (b, d) {
                for (var e = 0, f = d.length; f > e; e++) {
                    for (var g = d[e], h = g[0], i = g[1], j = c.buttonGroup({
                        className: "note-" + h
                    }).render(), k = 0, l = i.length; l > k; k++) {
                        var m = a.memo("button." + i[k]);
                        m && j.append("function" == typeof m ? m(a) : m)
                    }
                    j.appendTo(b)
                }
            }, this.updateCurrentStyle = function () {
                var c = a.invoke("editor.currentStyle");
                if (this.updateBtnStates({
                        ".note-btn-bold": function () {
                            return "bold" === c["font-bold"]
                },
                        ".note-btn-italic": function () {
                            return "italic" === c["font-italic"]
                },
                        ".note-btn-underline": function () {
                            return "underline" === c["font-underline"]
                },
                        ".note-btn-subscript": function () {
                            return "subscript" === c["font-subscript"]
                },
                        ".note-btn-superscript": function () {
                            return "superscript" === c["font-superscript"]
                },
                        ".note-btn-strikethrough": function () {
                            return "strikethrough" === c["font-strikethrough"]
                }
                }), c["font-family"]) {
                    var e = c["font-family"].split(",").map(function (a) {
                        return a.replace(/[\'\"]/g, "").replace(/\s+$/, "").replace(/^\s+/, "")
                    }),
                        f = list.find(e, b.isFontInstalled);
                    d.find(".dropdown-fontname li a").each(function () {
                        var a = $(this).data("value") + "" == f + "";
                        this.className = a ? "checked" : ""
                    }), d.find(".note-current-fontname").text(f)
                }
                if (c["font-size"]) {
                    var g = c["font-size"];
                    d.find(".dropdown-fontsize li a").each(function () {
                        var a = $(this).data("value") + "" == g + "";
                        this.className = a ? "checked" : ""
                    }), d.find(".note-current-fontsize").text(g)
                }
                if (c["line-height"]) {
                    var h = c["line-height"];
                    d.find(".dropdown-line-height li a").each(function () {
                        var a = $(this).data("value") + "" == h + "";
                        this.className = a ? "checked" : ""
                    })
                }
            }, this.updateBtnStates = function (a) {
                $.each(a, function (a, b) {
                    c.toggleBtnActive(d.find(a), b())
                })
            }, this.tableMoveHandler = function (a) {
                var b, c = 18,
                    d = $(a.target.parentNode),
                    f = d.next(),
                    g = d.find(".note-dimension-picker-mousecatcher"),
                    h = d.find(".note-dimension-picker-highlighted"),
                    i = d.find(".note-dimension-picker-unhighlighted");
                if (void 0 === a.offsetX) {
                    var j = $(a.target).offset();
                    b = {
                        x: a.pageX - j.left,
                        y: a.pageY - j.top
                    }
                } else b = {
                    x: a.offsetX,
                    y: a.offsetY
                };
                var k = {
                    c: Math.ceil(b.x / c) || 1,
                    r: Math.ceil(b.y / c) || 1
                };
                h.css({
                    width: k.c + "em",
                    height: k.r + "em"
                }), g.data("value", k.c + "x" + k.r), 3 < k.c && k.c < e.insertTableMaxSize.col && i.css({
                    width: k.c + 1 + "em"
                }), 3 < k.r && k.r < e.insertTableMaxSize.row && i.css({
                    height: k.r + 1 + "em"
                }), f.html(k.c + " x " + k.r)
            }
        },
        Toolbar = function (a) {
            var b = $.summernote.ui,
                c = a.layoutInfo.note,
                d = a.layoutInfo.toolbar,
                e = a.options;
            this.shouldInitialize = function () {
                return !e.airMode
            }, this.initialize = function () {
                e.toolbar = e.toolbar || [], e.toolbar.length ? a.invoke("buttons.build", d, e.toolbar) : d.hide(), e.toolbarContainer && d.appendTo(e.toolbarContainer), c.on("summernote.keyup summernote.mouseup summernote.change", function () {
                    a.invoke("buttons.updateCurrentStyle")
                }), a.invoke("buttons.updateCurrentStyle")
            }, this.destroy = function () {
                d.children().remove()
            }, this.updateFullscreen = function (a) {
                b.toggleBtnActive(d.find(".btn-fullscreen"), a)
            }, this.updateCodeview = function (a) {
                b.toggleBtnActive(d.find(".btn-codeview"), a), a ? this.deactivate() : this.activate()
            }, this.activate = function (a) {
                var c = d.find("button");
                a || (c = c.not(".btn-codeview")), b.toggleBtn(c, !0)
            }, this.deactivate = function (a) {
                var c = d.find("button");
                a || (c = c.not(".btn-codeview")), b.toggleBtn(c, !1)
            }
        },
        LinkDialog = function (a) {
            var b = this,
                c = $.summernote.ui,
                d = a.layoutInfo.editor,
                e = a.options,
                f = e.langInfo;
            this.initialize = function () {
                var a = e.dialogsInBody ? $(document.body) : d,
                    b = '<div class="form-group"><label>' + f.link.textToDisplay + '</label><input class="note-link-text form-control" type="text" /></div><div class="form-group"><label>' + f.link.url + '</label><input class="note-link-url form-control" type="text" value="http://" /></div>' + (e.disableLinkTarget ? "" : '<div class="checkbox"><label><input type="checkbox" checked> ' + f.link.openInNewWindow + "</label></div>"),
                    g = '<button href="#" class="btn btn-primary note-link-btn disabled" disabled>' + f.link.insert + "</button>";
                this.$dialog = c.dialog({
                    className: "link-dialog",
                    title: f.link.insert,
                    fade: e.dialogsFade,
                    body: b,
                    footer: g
                }).render().appendTo(a)
            }, this.destroy = function () {
                c.hideDialog(this.$dialog), this.$dialog.remove()
            }, this.bindEnterKey = function (a, b) {
                a.on("keypress", function (a) {
                    a.keyCode === key.code.ENTER && b.trigger("click")
                })
            }, this.toggleLinkBtn = function (a, b, d) {
                c.toggleBtn(a, b.val() && d.val())
            }, this.showLinkDialog = function (d) {
                return $.Deferred(function (e) {
                    var f = b.$dialog.find(".note-link-text"),
                        g = b.$dialog.find(".note-link-url"),
                        h = b.$dialog.find(".note-link-btn"),
                        i = b.$dialog.find("input[type=checkbox]");
                    c.onDialogShown(b.$dialog, function () {
                        a.triggerEvent("dialog.shown"), d.url || (d.url = d.text), f.val(d.text);
                        var c = function () {
                            b.toggleLinkBtn(h, f, g), d.text = f.val()
                        };
                        f.on("input", c).on("paste", function () {
                            setTimeout(c, 0)
                        });
                        var j = function () {
                            b.toggleLinkBtn(h, f, g), d.text || f.val(g.val())
                        };
                        g.on("input", j).on("paste", function () {
                            setTimeout(j, 0)
                        }).val(d.url).trigger("focus"), b.toggleLinkBtn(h, f, g), b.bindEnterKey(g, h), b.bindEnterKey(f, h), i.prop("checked", d.isNewWindow), h.one("click", function (a) {
                            a.preventDefault(), e.resolve({
                                range: d.range,
                                url: g.val(),
                                text: f.val(),
                                isNewWindow: i.is(":checked")
                            }), b.$dialog.modal("hide")
                        })
                    }), c.onDialogHidden(b.$dialog, function () {
                        f.off("input paste keypress"), g.off("input paste keypress"), h.off("click"), "pending" === e.state() && e.reject()
                    }), c.showDialog(b.$dialog)
                }).promise()
            }, this.show = function () {
                var b = a.invoke("editor.getLinkInfo");
                a.invoke("editor.saveRange"), this.showLinkDialog(b).then(function (b) {
                    a.invoke("editor.restoreRange"), a.invoke("editor.createLink", b)
                }).fail(function () {
                    a.invoke("editor.restoreRange")
                })
            }, a.memo("help.linkDialog.show", e.langInfo.help["linkDialog.show"])
        },
        LinkPopover = function (a) {
            var b = this,
                c = $.summernote.ui,
                d = a.options;
            this.events = {
                "summernote.keyup summernote.mouseup summernote.change summernote.scroll": function () {
                    b.update()
                },
                "summernote.dialog.shown": function () {
                    b.hide()
                }
            }, this.shouldInitialize = function () {
                return !list.isEmpty(d.popover.link)
            }, this.initialize = function () {
                this.$popover = c.popover({
                    className: "note-link-popover",
                    callback: function (a) {
                        var b = a.find(".popover-content");
                        b.prepend('<span><a target="_blank"></a>&nbsp;</span>')
                    }
                }).render().appendTo("body");
                var b = this.$popover.find(".popover-content");
                a.invoke("buttons.build", b, d.popover.link)
            }, this.destroy = function () {
                this.$popover.remove()
            }, this.update = function () {
                if (!a.invoke("editor.hasFocus")) return void this.hide();
                var b = a.invoke("editor.createRange");
                if (b.isCollapsed() && b.isOnAnchor()) {
                    var c = dom.ancestor(b.sc, dom.isAnchor),
                        d = $(c).attr("href");
                    this.$popover.find("a").attr("href", d).html(d);
                    var e = dom.posFromPlaceholder(c);
                    this.$popover.css({
                        display: "block",
                        left: e.left,
                        top: e.top
                    })
                } else this.hide()
            }, this.hide = function () {
                this.$popover.hide()
            }
        },
        ImageDialog = function (a) {
            var b = this,
                c = $.summernote.ui,
                d = a.layoutInfo.editor,
                e = a.options,
                f = e.langInfo;
            this.initialize = function () {
                var a = e.dialogsInBody ? $(document.body) : d,
                    b = "";
                if (e.maximumImageFileSize) {
                    var g = Math.floor(Math.log(e.maximumImageFileSize) / Math.log(1024)),
                        h = 1 * (e.maximumImageFileSize / Math.pow(1024, g)).toFixed(2) + " " + " KMGTP"[g] + "B";
                    b = "<small>" + f.image.maximumFileSize + " : " + h + "</small>"
                }
                var i = '<div class="form-group note-group-select-from-files"><label>' + f.image.selectFromFiles + '</label><input class="note-image-input form-control" type="file" name="files" accept="image/*" multiple="multiple" />' + b + '</div><div class="form-group note-group-image-url" style="overflow:auto;"><label>' + f.image.url + '</label><input class="note-image-url form-control col-md-12" type="text" /></div>',
                    j = '<button href="#" class="btn btn-primary note-image-btn disabled" disabled>' + f.image.insert + "</button>";
                this.$dialog = c.dialog({
                    title: f.image.insert,
                    fade: e.dialogsFade,
                    body: i,
                    footer: j
                }).render().appendTo(a)
            }, this.destroy = function () {
                c.hideDialog(this.$dialog), this.$dialog.remove()
            }, this.bindEnterKey = function (a, b) {
                a.on("keypress", function (a) {
                    a.keyCode === key.code.ENTER && b.trigger("click")
                })
            }, this.show = function () {
                a.invoke("editor.saveRange"), this.showImageDialog().then(function (d) {
                    c.hideDialog(b.$dialog), a.invoke("editor.restoreRange"), "string" == typeof d ? a.invoke("editor.insertImage", d) : a.invoke("editor.insertImagesOrCallback", d)
                }).fail(function () {
                    a.invoke("editor.restoreRange")
                })
            }, this.showImageDialog = function () {
                return $.Deferred(function (d) {
                    var e = b.$dialog.find(".note-image-input"),
                        f = b.$dialog.find(".note-image-url"),
                        g = b.$dialog.find(".note-image-btn");
                    c.onDialogShown(b.$dialog, function () {
                        a.triggerEvent("dialog.shown"), e.replaceWith(e.clone().on("change", function () {
                            d.resolve(this.files || this.value)
                        }).val("")), g.click(function (a) {
                            a.preventDefault(), d.resolve(f.val())
                        }), f.on("keyup paste", function () {
                            var a = f.val();
                            c.toggleBtn(g, a)
                        }).val("").trigger("focus"), b.bindEnterKey(f, g)
                    }), c.onDialogHidden(b.$dialog, function () {
                        e.off("change"), f.off("keyup paste keypress"), g.off("click"), "pending" === d.state() && d.reject()
                    }), c.showDialog(b.$dialog)
                })
            }
        },
        ImagePopover = function (a) {
            var b = $.summernote.ui,
                c = a.options;
            this.shouldInitialize = function () {
                return !list.isEmpty(c.popover.image)
            }, this.initialize = function () {
                this.$popover = b.popover({
                    className: "note-image-popover"
                }).render().appendTo("body");
                var d = this.$popover.find(".popover-content");
                a.invoke("buttons.build", d, c.popover.image)
            }, this.destroy = function () {
                this.$popover.remove()
            }, this.update = function (a) {
                if (dom.isImg(a)) {
                    var b = dom.posFromPlaceholder(a);
                    this.$popover.css({
                        display: "block",
                        left: b.left,
                        top: b.top
                    })
                } else this.hide()
            }, this.hide = function () {
                this.$popover.hide()
            }
        },
        VideoDialog = function (a) {
            var b = this,
                c = $.summernote.ui,
                d = a.layoutInfo.editor,
                e = a.options,
                f = e.langInfo;
            this.initialize = function () {
                var a = e.dialogsInBody ? $(document.body) : d,
                    b = '<div class="form-group row-fluid"><label>' + f.video.url + ' <small class="text-muted">' + f.video.providers + '</small></label><input class="note-video-url form-control span12" type="text" /></div>',
                    g = '<button href="#" class="btn btn-primary note-video-btn disabled" disabled>' + f.video.insert + "</button>";
                this.$dialog = c.dialog({
                    title: f.video.insert,
                    fade: e.dialogsFade,
                    body: b,
                    footer: g
                }).render().appendTo(a)
            }, this.destroy = function () {
                c.hideDialog(this.$dialog), this.$dialog.remove()
            }, this.bindEnterKey = function (a, b) {
                a.on("keypress", function (a) {
                    a.keyCode === key.code.ENTER && b.trigger("click")
                })
            }, this.createVideoNode = function (a) {
                var b, c = /^(?:https?:\/\/)?(?:www\.)?(?:youtu\.be\/|youtube\.com\/(?:embed\/|v\/|watch\?v=|watch\?.+&v=))((\w|-){11})(?:\S+)?$/,
                    d = a.match(c),
                    e = /(?:www\.|\/\/)instagram\.com\/p\/(.[a-zA-Z0-9_-]*)/,
                    f = a.match(e),
                    g = /\/\/vine\.co\/v\/([a-zA-Z0-9]+)/,
                    h = a.match(g),
                    i = /\/\/(player\.)?vimeo\.com\/([a-z]*\/)*([0-9]{6,11})[?]?.*/,
                    j = a.match(i),
                    k = /.+dailymotion.com\/(video|hub)\/([^_]+)[^#]*(#video=([^_&]+))?/,
                    l = a.match(k),
                    m = /\/\/v\.youku\.com\/v_show\/id_(\w+)=*\.html/,
                    n = a.match(m),
                    o = /^.+.(mp4|m4v)$/,
                    p = a.match(o),
                    q = /^.+.(ogg|ogv)$/,
                    r = a.match(q),
                    s = /^.+.(webm)$/,
                    t = a.match(s);
                if (d && 11 === d[1].length) {
                    var u = d[1];
                    b = $("<iframe>").attr("frameborder", 0).attr("src", "//www.youtube.com/embed/" + u).attr("width", "640").attr("height", "360")
                } else if (f && f[0].length) b = $("<iframe>").attr("frameborder", 0).attr("src", "https://instagram.com/p/" + f[1] + "/embed/").attr("width", "612").attr("height", "710").attr("scrolling", "no").attr("allowtransparency", "true");
                else if (h && h[0].length) b = $("<iframe>").attr("frameborder", 0).attr("src", h[0] + "/embed/simple").attr("width", "600").attr("height", "600").attr("class", "vine-embed");
                else if (j && j[3].length) b = $("<iframe webkitallowfullscreen mozallowfullscreen allowfullscreen>").attr("frameborder", 0).attr("src", "//player.vimeo.com/video/" + j[3]).attr("width", "640").attr("height", "360");
                else if (l && l[2].length) b = $("<iframe>").attr("frameborder", 0).attr("src", "//www.dailymotion.com/embed/video/" + l[2]).attr("width", "640").attr("height", "360");
                else if (n && n[1].length) b = $("<iframe webkitallowfullscreen mozallowfullscreen allowfullscreen>").attr("frameborder", 0).attr("height", "498").attr("width", "510").attr("src", "//player.youku.com/embed/" + n[1]);
                else {
                    if (!(p || r || t)) return !1;
                    b = $("<video controls>").attr("src", a).attr("width", "640").attr("height", "360")
                }
                return b.addClass("note-video-clip"), b[0]
            }, this.show = function () {
                var d = a.invoke("editor.getSelectedText");
                a.invoke("editor.saveRange"), this.showVideoDialog(d).then(function (d) {
                    c.hideDialog(b.$dialog), a.invoke("editor.restoreRange");
                    var e = b.createVideoNode(d);
                    e && a.invoke("editor.insertNode", e)
                }).fail(function () {
                    a.invoke("editor.restoreRange")
                })
            }, this.showVideoDialog = function (d) {
                return $.Deferred(function (e) {
                    var f = b.$dialog.find(".note-video-url"),
                        g = b.$dialog.find(".note-video-btn");
                    c.onDialogShown(b.$dialog, function () {
                        a.triggerEvent("dialog.shown"), f.val(d).on("input", function () {
                            c.toggleBtn(g, f.val())
                        }).trigger("focus"), g.click(function (a) {
                            a.preventDefault(), e.resolve(f.val())
                        }), b.bindEnterKey(f, g)
                    }), c.onDialogHidden(b.$dialog, function () {
                        f.off("input"), g.off("click"), "pending" === e.state() && e.reject()
                    }), c.showDialog(b.$dialog)
                })
            }
        },
        HelpDialog = function (a) {
            var b = this,
                c = $.summernote.ui,
                d = a.layoutInfo.editor,
                e = a.options,
                f = e.langInfo;
            this.createShortCutList = function () {
                var b = e.keyMap[agent.isMac ? "mac" : "pc"];
                return Object.keys(b).map(function (c) {
                    var d = b[c],
                        e = $('<div><div class="help-list-item"/></div>');
                    return e.append($("<label><kbd>" + c + "</kdb></label>").css({
                        width: 180,
                        "margin-right": 10
                    })).append($("<span/>").html(a.memo("help." + d) || d)), e.html()
                }).join("")
            }, this.initialize = function () {
                var a = e.dialogsInBody ? $(document.body) : d,
                    b = ['<p class="text-center">', '<a href="http://summernote.org/" target="_blank">Summernote 0.8.2</a> · ', '<a href="https://github.com/summernote/summernote" target="_blank">Project</a> · ', '<a href="https://github.com/summernote/summernote/issues" target="_blank">Issues</a>', "</p>"].join("");
                this.$dialog = c.dialog({
                    title: f.options.help,
                    fade: e.dialogsFade,
                    body: this.createShortCutList(),
                    footer: b,
                    callback: function (a) {
                        a.find(".modal-body").css({
                            "max-height": 300,
                            overflow: "scroll"
                        })
                    }
                }).render().appendTo(a)
            }, this.destroy = function () {
                c.hideDialog(this.$dialog), this.$dialog.remove()
            }, this.showHelpDialog = function () {
                return $.Deferred(function (d) {
                    c.onDialogShown(b.$dialog, function () {
                        a.triggerEvent("dialog.shown"), d.resolve()
                    }), c.showDialog(b.$dialog)
                }).promise()
            }, this.show = function () {
                a.invoke("editor.saveRange"), this.showHelpDialog().then(function () {
                    a.invoke("editor.restoreRange")
                })
            }
        },
        AirPopover = function (a) {
            var b = this,
                c = $.summernote.ui,
                d = a.options,
                e = 20;
            this.events = {
                "summernote.keyup summernote.mouseup summernote.scroll": function () {
                    b.update()
                },
                "summernote.change summernote.dialog.shown": function () {
                    b.hide()
                },
                "summernote.focusout": function (a, c) {
                    agent.isFF || c.relatedTarget && dom.ancestor(c.relatedTarget, func.eq(b.$popover[0])) || b.hide()
                }
            }, this.shouldInitialize = function () {
                return d.airMode && !list.isEmpty(d.popover.air)
            }, this.initialize = function () {
                this.$popover = c.popover({
                    className: "note-air-popover"
                }).render().appendTo("body");
                var b = this.$popover.find(".popover-content");
                a.invoke("buttons.build", b, d.popover.air)
            }, this.destroy = function () {
                this.$popover.remove()
            }, this.update = function () {
                var b = a.invoke("editor.currentStyle");
                if (b.range && !b.range.isCollapsed()) {
                    var c = list.last(b.range.getClientRects());
                    if (c) {
                        var d = func.rect2bnd(c);
                        this.$popover.css({
                            display: "block",
                            left: Math.max(d.left + d.width / 2, 0) - e,
                            top: d.top + d.height
                        })
                    }
                } else this.hide()
            }, this.hide = function () {
                this.$popover.hide()
            }
        },
        HintPopover = function (a) {
            var b = this,
                c = $.summernote.ui,
                d = 5,
                e = a.options.hint || [],
                f = a.options.hintDirection || "bottom",
                g = $.isArray(e) ? e : [e];
            this.events = {
                "summernote.keyup": function (a, c) {
                    c.isDefaultPrevented() || b.handleKeyup(c)
                },
                "summernote.keydown": function (a, c) {
                    b.handleKeydown(c)
                },
                "summernote.dialog.shown": function () {
                    b.hide()
                }
            }, this.shouldInitialize = function () {
                return g.length > 0
            }, this.initialize = function () {
                this.lastWordRange = null, this.$popover = c.popover({
                    className: "note-hint-popover",
                    hideArrow: !0,
                    direction: ""
                }).render().appendTo("body"), this.$popover.hide(), this.$content = this.$popover.find(".popover-content"), this.$content.on("click", ".note-hint-item", function () {
                    b.$content.find(".active").removeClass("active"), $(this).addClass("active"), b.replace()
                })
            }, this.destroy = function () {
                this.$popover.remove()
            }, this.selectItem = function (a) {
                this.$content.find(".active").removeClass("active"), a.addClass("active"), this.$content[0].scrollTop = a[0].offsetTop - this.$content.innerHeight() / 2
            }, this.moveDown = function () {
                var a = this.$content.find(".note-hint-item.active"),
                    b = a.next();
                if (b.length) this.selectItem(b);
                else {
                    var c = a.parent().next();
                    c.length || (c = this.$content.find(".note-hint-group").first()), this.selectItem(c.find(".note-hint-item").first())
                }
            }, this.moveUp = function () {
                var a = this.$content.find(".note-hint-item.active"),
                    b = a.prev();
                if (b.length) this.selectItem(b);
                else {
                    var c = a.parent().prev();
                    c.length || (c = this.$content.find(".note-hint-group").last()), this.selectItem(c.find(".note-hint-item").last())
                }
            }, this.replace = function () {
                var b = this.$content.find(".note-hint-item.active");
                if (b.length) {
                    var c = this.nodeFromItem(b);
                    this.lastWordRange.insertNode(c), range.createFromNode(c).collapse().select(), this.lastWordRange = null, this.hide(), a.invoke("editor.focus")
                }
            }, this.nodeFromItem = function (a) {
                var b = g[a.data("index")],
                    c = a.data("item"),
                    d = b.content ? b.content(c) : c;
                return "string" == typeof d && (d = dom.createText(d)), d
            }, this.createItemTemplates = function (a, b) {
                var c = g[a];
                return b.map(function (b, d) {
                    var e = $('<div class="note-hint-item"/>');
                    return e.append(c.template ? c.template(b) : b + ""), e.data({
                        index: a,
                        item: b
                    }), 0 === a && 0 === d && e.addClass("active"), e
                })
            }, this.handleKeydown = function (a) {
                this.$popover.is(":visible") && (a.keyCode === key.code.ENTER ? (a.preventDefault(), this.replace()) : a.keyCode === key.code.UP ? (a.preventDefault(), this.moveUp()) : a.keyCode === key.code.DOWN && (a.preventDefault(), this.moveDown()))
            }, this.searchKeyword = function (a, b, c) {
                var d = g[a];
                if (d && d.match.test(b) && d.search) {
                    var e = d.match.exec(b);
                    d.search(e[1], c)
                } else c()
            }, this.createGroup = function (a, c) {
                var d = $('<div class="note-hint-group note-hint-group-' + a + '"/>');
                return this.searchKeyword(a, c, function (c) {
                    c = c || [], c.length && (d.html(b.createItemTemplates(a, c)), b.show())
                }), d
            }, this.handleKeyup = function (c) {
                if (list.contains([key.code.ENTER, key.code.UP, key.code.DOWN], c.keyCode)) {
                    if (c.keyCode === key.code.ENTER && this.$popover.is(":visible")) return
                } else {
                    var e = a.invoke("editor.createRange").getWordRange(),
                        h = e.toString();
                    if (g.length && h) {
                        this.$content.empty();
                        var i = func.rect2bnd(list.last(e.getClientRects()));
                        i && (this.$popover.hide(), this.lastWordRange = e, g.forEach(function (a, c) {
                            a.match.test(h) && b.createGroup(c, h).appendTo(b.$content)
                        }), "top" === f ? this.$popover.css({
                            left: i.left,
                            top: i.top - this.$popover.outerHeight() - d
                        }) : this.$popover.css({
                            left: i.left,
                            top: i.top + i.height + d
                        }))
                    } else this.hide()
                }
            }, this.show = function () {
                this.$popover.show()
            }, this.hide = function () {
                this.$popover.hide()
            }
        };
    $.summernote = $.extend($.summernote, {
        version: "0.8.2",
        ui: ui,
        dom: dom,
        plugins: {},
        options: {
            modules: {
                editor: Editor,
                clipboard: Clipboard,
                dropzone: Dropzone,
                codeview: Codeview,
                statusbar: Statusbar,
                fullscreen: Fullscreen,
                handle: Handle,
                hintPopover: HintPopover,
                autoLink: AutoLink,
                autoSync: AutoSync,
                placeholder: Placeholder,
                buttons: Buttons,
                toolbar: Toolbar,
                linkDialog: LinkDialog,
                linkPopover: LinkPopover,
                imageDialog: ImageDialog,
                imagePopover: ImagePopover,
                videoDialog: VideoDialog,
                helpDialog: HelpDialog,
                airPopover: AirPopover
            },
            buttons: {},
            lang: "en-US",
            toolbar: [
                ["style", ["style"]],
                ["font", ["bold", "underline", "clear"]],
                ["fontname", ["fontname"]],
                ["color", ["color"]],
                ["para", ["ul", "ol", "paragraph"]],
                ["table", ["table"]],
                ["insert", ["link", "picture", "video"]],
                ["view", ["fullscreen", "codeview", "help"]]
            ],
            popover: {
                image: [
                    ["imagesize", ["imageSize100", "imageSize50", "imageSize25"]],
                    ["float", ["floatLeft", "floatRight", "floatNone"]],
                    ["remove", ["removeMedia"]]
                ],
                link: [
                    ["link", ["linkDialogShow", "unlink"]]
                ],
                air: [
                    ["color", ["color"]],
                    ["font", ["bold", "underline", "clear"]],
                    ["para", ["ul", "paragraph"]],
                    ["table", ["table"]],
                    ["insert", ["link", "picture"]]
                ]
            },
            airMode: !1,
            width: null,
            height: null,
            focus: !1,
            tabSize: 4,
            styleWithSpan: !0,
            shortcuts: !0,
            textareaAutoSync: !0,
            direction: null,
            styleTags: ["p", "blockquote", "pre", "h1", "h2", "h3", "h4", "h5", "h6"],
            fontNames: ["Arial", "Arial Black", "Comic Sans MS", "Courier New", "Helvetica Neue", "Helvetica", "Impact", "Lucida Grande", "Tahoma", "Times New Roman", "Verdana"],
            fontSizes: ["8", "9", "10", "11", "12", "14", "18", "24", "36"],
            colors: [
                ["#000000", "#424242", "#636363", "#9C9C94", "#CEC6CE", "#EFEFEF", "#F7F7F7", "#FFFFFF"],
                ["#FF0000", "#FF9C00", "#FFFF00", "#00FF00", "#00FFFF", "#0000FF", "#9C00FF", "#FF00FF"],
                ["#F7C6CE", "#FFE7CE", "#FFEFC6", "#D6EFD6", "#CEDEE7", "#CEE7F7", "#D6D6E7", "#E7D6DE"],
                ["#E79C9C", "#FFC69C", "#FFE79C", "#B5D6A5", "#A5C6CE", "#9CC6EF", "#B5A5D6", "#D6A5BD"],
                ["#E76363", "#F7AD6B", "#FFD663", "#94BD7B", "#73A5AD", "#6BADDE", "#8C7BC6", "#C67BA5"],
                ["#CE0000", "#E79439", "#EFC631", "#6BA54A", "#4A7B8C", "#3984C6", "#634AA5", "#A54A7B"],
                ["#9C0000", "#B56308", "#BD9400", "#397B21", "#104A5A", "#085294", "#311873", "#731842"],
                ["#630000", "#7B3900", "#846300", "#295218", "#083139", "#003163", "#21104A", "#4A1031"]
            ],
            lineHeights: ["1.0", "1.2", "1.4", "1.5", "1.6", "1.8", "2.0", "3.0"],
            tableClassName: "table table-bordered",
            insertTableMaxSize: {
                col: 10,
                row: 10
            },
            dialogsInBody: !1,
            dialogsFade: !1,
            maximumImageFileSize: null,
            callbacks: {
                onInit: null,
                onFocus: null,
                onBlur: null,
                onEnter: null,
                onKeyup: null,
                onKeydown: null,
                onImageUpload: null,
                onImageUploadError: null
            },
            codemirror: {
                mode: "text/html",
                htmlMode: !0,
                lineNumbers: !0
            },
            keyMap: {
                pc: {
                    ENTER: "insertParagraph",
                    "CTRL+Z": "undo",
                    "CTRL+Y": "redo",
                    TAB: "tab",
                    "SHIFT+TAB": "untab",
                    "CTRL+B": "bold",
                    "CTRL+I": "italic",
                    "CTRL+U": "underline",
                    "CTRL+SHIFT+S": "strikethrough",
                    "CTRL+BACKSLASH": "removeFormat",
                    "CTRL+SHIFT+L": "justifyLeft",
                    "CTRL+SHIFT+E": "justifyCenter",
                    "CTRL+SHIFT+R": "justifyRight",
                    "CTRL+SHIFT+J": "justifyFull",
                    "CTRL+SHIFT+NUM7": "insertUnorderedList",
                    "CTRL+SHIFT+NUM8": "insertOrderedList",
                    "CTRL+LEFTBRACKET": "outdent",
                    "CTRL+RIGHTBRACKET": "indent",
                    "CTRL+NUM0": "formatPara",
                    "CTRL+NUM1": "formatH1",
                    "CTRL+NUM2": "formatH2",
                    "CTRL+NUM3": "formatH3",
                    "CTRL+NUM4": "formatH4",
                    "CTRL+NUM5": "formatH5",
                    "CTRL+NUM6": "formatH6",
                    "CTRL+ENTER": "insertHorizontalRule",
                    "CTRL+K": "linkDialog.show"
                },
                mac: {
                    ENTER: "insertParagraph",
                    "CMD+Z": "undo",
                    "CMD+SHIFT+Z": "redo",
                    TAB: "tab",
                    "SHIFT+TAB": "untab",
                    "CMD+B": "bold",
                    "CMD+I": "italic",
                    "CMD+U": "underline",
                    "CMD+SHIFT+S": "strikethrough",
                    "CMD+BACKSLASH": "removeFormat",
                    "CMD+SHIFT+L": "justifyLeft",
                    "CMD+SHIFT+E": "justifyCenter",
                    "CMD+SHIFT+R": "justifyRight",
                    "CMD+SHIFT+J": "justifyFull",
                    "CMD+SHIFT+NUM7": "insertUnorderedList",
                    "CMD+SHIFT+NUM8": "insertOrderedList",
                    "CMD+LEFTBRACKET": "outdent",
                    "CMD+RIGHTBRACKET": "indent",
                    "CMD+NUM0": "formatPara",
                    "CMD+NUM1": "formatH1",
                    "CMD+NUM2": "formatH2",
                    "CMD+NUM3": "formatH3",
                    "CMD+NUM4": "formatH4",
                    "CMD+NUM5": "formatH5",
                    "CMD+NUM6": "formatH6",
                    "CMD+ENTER": "insertHorizontalRule",
                    "CMD+K": "linkDialog.show"
                }
            },
            icons: {
                align: "note-icon-align",
                alignCenter: "note-icon-align-center",
                alignJustify: "note-icon-align-justify",
                alignLeft: "note-icon-align-left",
                alignRight: "note-icon-align-right",
                indent: "note-icon-align-indent",
                outdent: "note-icon-align-outdent",
                arrowsAlt: "note-icon-arrows-alt",
                bold: "note-icon-bold",
                caret: "note-icon-caret",
                circle: "note-icon-circle",
                close: "note-icon-close",
                code: "note-icon-code",
                eraser: "note-icon-eraser",
                font: "note-icon-font",
                frame: "note-icon-frame",
                italic: "note-icon-italic",
                link: "note-icon-link",
                unlink: "note-icon-chain-broken",
                magic: "note-icon-magic",
                menuCheck: "note-icon-check",
                minus: "note-icon-minus",
                orderedlist: "note-icon-orderedlist",
                pencil: "note-icon-pencil",
                picture: "note-icon-picture",
                question: "note-icon-question",
                redo: "note-icon-redo",
                square: "note-icon-square",
                strikethrough: "note-icon-strikethrough",
                subscript: "note-icon-subscript",
                superscript: "note-icon-superscript",
                table: "note-icon-table",
                textHeight: "note-icon-text-height",
                trash: "note-icon-trash",
                underline: "note-icon-underline",
                undo: "note-icon-undo",
                unorderedlist: "note-icon-unorderedlist",
                video: "note-icon-video"
            }
        }
    })
});