
//! Filename  : Extranet/PMPO/app-common.js
//! version   : 1.0.0 (last revision Aug 14, 2016)
//! author    : Miguel Reyna Huancas - Soft Technology Solutions S.A.C.
//! copyright : Comité De Operación Económica del Sistema Interconectado Nacional (COES)
//! www.coes.org.pe

(function ($, undefined) {
    "use strict";

    var jQueryWindow = function () {
    }
    jQueryWindow.prototype.open = function (options, callback) {
        var $divPopup = $('<div>'),
            $form = $('<form>'),
            $btnClose = $('<div>'),
            $divTitle = $('<div>'),
            $divContent = $('<div>');

        options = options || {};
        options = {
            data: options.data || null,
            url: options.url || 'about:blank',
            title: options.title || '',
            modal: (options.modal != null ? !!options.modal : true),
            width: options.width || '300px',
            height: options.height || '300px',
            close: options.close || null,
            beforeClose: options.beforeClose || null
        };

        $btnClose
            .addClass('button')
            .addClass('b-close')
            .html('<span>X</span>');

        $divTitle
            .addClass('popup-title')
            .html(options.title)
            .disableSelection();

        $divContent
            .addClass('panel-text');

        $form
            .attr('target', 'xxxyyy')
            .attr('method', 'post')
            .attr('action', options.url)
            .hide();

        if (options.data != null) {
            $.each(options.data, function (key, value) {
                var $input = $('<input>');

                $input
                    .attr('type', 'hidden')
                    .attr('name', key)
                    .val(typeof value == 'string' ? value : JSON.stringify(value));

                $form.append($input);
            });
        }

        $divPopup
            .addClass('general-popup')
            .append($btnClose)
            .append($divTitle)
            .append($form)
            .append($divContent)
            .appendTo(document.body);

        window.$__Popup = $divPopup.bPopup({
            content: 'iframe',
            contentContainer: $divContent,
            positionStyle: 'fixed',
            follow: [false, false],
            easing: 'easeOutBack',
            speed: 450,
            modal: options.modal,
            transition: 'slideDown',
            loadData: options.data,
            loadUrl: 'about:blank',
            iframeAttr: 'name="xxxyyy" scrolling="yes" frameborder="0" style="width: ' + options.width + '; height: ' + options.height + ';"',
            onBeforeClose: function () {
                var close = (options.beforeClose == null || options.beforeClose());

                return close;
            },
            onClose: function () {
                jQueryWindow._callback();
                $divPopup.remove();
            }
        });

        $form.submit();
        $form.remove();

        $divPopup.draggable({
            handle: $divTitle,
        });

        $divPopup.resizable({
            alsoResize: $divPopup.find('.b-iframe'),
            handles: 'n, e, s, w, ne, se, sw, nw',
            minHeight: 100,
            minWidth: 280,
            distance: 0
        });

        if (window.$__Popup) {
            window.$_windowOnClose = options.close;
        } else {
            window.$_windowOnClose = null;
        }
    }
    jQueryWindow.prototype.close = function (args) {
        var p = window.parent || window;

        p.$('#loading').bPopup({
            onOpen: function () { },
            onClose: function () { },
            loadCallback: function () {
//                alert(0029);
            }
        },
        function () {
            var t = setTimeout(function () {
                if (window.parent) {
                    jQueryWindow._callback(args);
                    window.parent.$__Popup.close();
                } else {
                    window.close();
                }

                p.$('#loading').bPopup().close();

                clearTimeout(t);
            }, 3000);
        });
    }
    jQueryWindow._callback = function (args) {
        if (window.parent.$_windowOnClose) {
            window.parent.$_windowOnClose(args);
            window.parent.$_windowOnClose = null;
        }
    }

    $.extend($, {
        app: {
            window: new jQueryWindow(),
            'const': {
                days: ['Domingo', 'Lunes', 'Martes', 'Miercoles', 'Jueves', 'Viernes', 'Sabado'],
                months: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                today: 'Hoy',
                loadingRecords: 'Cargando...',
                processing: 'Procesando...',
                zeroRecords: 'No se encontraron registros coincidentes.',
                emptyTable: 'No hay datos disponibles en la tabla.',
                formatDate: 'd/m/Y',
                actionSuccessClassName: 'action-exito',
                actionErrorClassName: 'action-error'
            },
            getSiteRoot: function () {

                return siteRoot;  //(siteRoot == '|' ? 'http://localhost:65158/' : siteRoot);
            }
        }
    });

    $.fn.extend({
        enable: function (value) {
            return this.each(function () {
                this.disabled = (value != null ? !value : false);

                var $this = $(this),
                    $next = $this.next();

                if (!value) {
                    $this.addClass('disabled');

                    if ($next.hasClass('Zebra_DatePicker_Icon')) {
                        $next.addClass('Zebra_DatePicker_Icon_Disabled');
                    }

                } else {
                    $this.removeClass('disabled');

                    if ($next.hasClass('Zebra_DatePicker_Icon')) {
                        $next.removeClass('Zebra_DatePicker_Icon_Disabled');
                    }
                }
            });
        },
        populateDropDown: function (items, options) {
            return this.each(function () {
                var $this = $(this);

                options = $.extend($.fn.populateDropDown.defaults, $this.data(), options);

                $this.empty();
                $this.enable(false);

                if (items != null && items.length > 0) {
                    var value,
                        text;

                    $.each(items, function (index, item) {
                        value = item[options.fieldId];
                        text = item[options.fieldText];

                        $this.append($("<option>", {
                            value: value,
                            text: text,
                            selected: (options.selectedValue == value || index == options.selectedIndex)
                        }));
                    });

                    $this.enable();
                }
            });
        },
        hideMessage: function () {
            return this.each(function () {
                var $that = $(this);

                $that.hide();

                var className = $that.attr('class');

                if (className) {
                    $that.removeClass(className);
                }

                $that.empty();
            });
        },
        showMessage: function (type, message) {
            var className;

            switch (type) {
                case 'error':
                    className = $.app['const'].actionErrorClassName;
                    break;
                case 'success':
                    className = $.app['const'].actionSuccessClassName;
                    break;
                default:
                    className = '';
                    break;
            }

            return this.each(function () {
                $(this)
                    .addClass(className)
                    .html(message)
                    .show();
            })
        },
        addEvent: function (sender, name, event) {
            return this.each(function () {
                var that = this,
                    control = $(this);

                function facadeEvent(e) { if (that.disabled == null || !that.disabled) { var args = { event: e, control: control }; event.call(sender, control, args); } }

                control.unbind(name);
                control.bind(name, facadeEvent);
            })
        }
    });

    $.fn.populateDropDown.defaults = {
        fieldId: 'id',
        fieldText: 'text',
        selectedValue: null,
        selectedIndex: -1
    }
})(jQuery, null);