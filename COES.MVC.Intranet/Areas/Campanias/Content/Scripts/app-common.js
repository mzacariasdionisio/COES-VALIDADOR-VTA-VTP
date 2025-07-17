
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
            const: {
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
            date: {
                format: function (date, format) {
                    return $.app.date.formatDate(date, format);
                },
                addMonth: function (date, value, settings) {
                    var currentMonth = date.getMonth();
                    var newDate = date.setMonth(currentMonth + value);

                    if (settings != null) {
                        newDate = $.formatDate(newDate, settings);
                    }

                    return newDate;
                },
                limitedDate: function (date) {
                    var fDateToDay = new Date();
                    fDateToDay = this.format(fDateToDay, { format: 'dd/mm/yy' });
                    return date.valueOf() > fDateToDay.valueOf() ? 'disabled' : '';
                },
                formatDate: function (date, settings) {

                    var formatDateTimeDefault = {

                        monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio',
                      'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre',
                          'Diciembre'],
                        monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul',
                                          'Ago', 'Sep', 'Oct', 'Nov', 'Dic'],
                        dayNames: ['Domingo', 'Lunes', 'Martes', 'Miercoles', 'Jueves',
                                   'Viernes', 'Sabado'],
                        dayNamesShort: ['Dom', 'Lun', 'Mar', 'Mie', 'Jue', 'Vie', 'Sab'],
                        ampmNames: ['a.m.', 'p.m.'],
                        getSuffix: function (num) {
                            if (num > 3 && num < 21) {
                                return 'th';
                            }

                            switch (num % 10) {
                                case 1: return "st";
                                case 2: return "nd";
                                case 3: return "rd";
                                default: return "th";
                            }
                        },
                        attribute: 'data-datetime',
                        formatAttribute: 'data-dateformat',
                        format: 'dd/mm/yy gg:ii:ss a'

                    };

                    if (!date)
                        return '';

                    if (typeof date != Date) {
                        if ((/^\//).test(date)) {
                            var dateString = date.replace(/\//g, '');

                            date = new Date(dateString);
                        }
                        else {
                            date = new Date(date);
                        }
                    }

                    settings = $.extend({}, formatDateTimeDefault, settings);

                    var format = settings.format;
                    var ticksTo1970 = (((1970 - 1) * 365 + Math.floor(1970 / 4)
                                        - Math.floor(1970 / 100)
                                        + Math.floor(1970 / 400)) * 24 * 60 * 60 * 10000000);

                    var output = '';
                    var literal = false;
                    var iFormat = 0;

                    // Check whether a format character is doubled
                    var lookAhead = function (match) {
                        var matches = (iFormat + 1 < format.length
                                       && format.charAt(iFormat + 1) == match);
                        if (matches) {
                            iFormat++;
                        }
                        return matches;
                    };

                    // Format a number, with leading zero if necessary
                    var formatNumber = function (match, value, len) {
                        var num = '' + value;
                        if (lookAhead(match)) {
                            while (num.length < len) {
                                num = '0' + num;
                            }
                        }
                        return num;
                    };

                    // Format a name, short or long as requested
                    var formatName = function (match, value, shortNames, longNames) {
                        return (lookAhead(match) ? longNames[value] : shortNames[value]);
                    };

                    // Get the value for the supplied unit, e.g. year for y
                    var getUnitValue = function (unit) {
                        switch (unit) {
                            case 'y': return date.getFullYear();
                            case 'm': return date.getMonth() + 1;
                            case 'd': return date.getDate();
                            case 'g': return date.getHours() % 12 || 12;
                            case 'h': return date.getHours();
                            case 'i': return date.getMinutes();
                            case 's': return date.getSeconds();
                            case 'u': return date.getMilliseconds();
                            default: return '';
                        }
                    };

                    for (iFormat = 0; iFormat < format.length; iFormat++) {
                        if (literal) {
                            if (format.charAt(iFormat) == "'" && !lookAhead("'")) {
                                literal = false;
                            }
                            else {
                                output += format.charAt(iFormat);
                            }
                        } else {
                            switch (format.charAt(iFormat)) {
                                case 'a':
                                    output += date.getHours() < 12
                                        ? settings.ampmNames[0]
                                        : settings.ampmNames[1];
                                    break;
                                case 'd':
                                    output += formatNumber('d', date.getDate(), 2);
                                    break;
                                case 'S':
                                    var v = getUnitValue(iFormat && format.charAt(iFormat - 1));
                                    output += (v && (settings.getSuffix || $.noop)(v)) || '';
                                    break;
                                case 'D':
                                    output += formatName('D',
                                                         date.getDay(),
                                                         settings.dayNamesShort,
                                                         settings.dayNames);
                                    break;
                                case 'o':
                                    var end = new Date(date.getFullYear(),
                                                       date.getMonth(),
                                                       date.getDate()).getTime();
                                    var start = new Date(date.getFullYear(), 0, 0).getTime();
                                    output += formatNumber(
                                        'o', Math.round((end - start) / 86400000), 3);
                                    break;
                                case 'g':
                                    output += formatNumber('g', date.getHours() % 12 || 12, 2);
                                    break;
                                case 'h':
                                    output += formatNumber('h', date.getHours(), 2);
                                    break;
                                case 'u':
                                    output += formatNumber('u', date.getMilliseconds(), 3);
                                    break;
                                case 'i':
                                    output += formatNumber('i', date.getMinutes(), 2);
                                    break;
                                case 'm':
                                    output += formatNumber('m', date.getMonth() + 1, 2);
                                    break;
                                case 'M':
                                    output += formatName('M',
                                                         date.getMonth(),
                                                         settings.monthNamesShort,
                                                         settings.monthNames);
                                    break;
                                case 's':
                                    output += formatNumber('s', date.getSeconds(), 2);
                                    break;
                                case 'y':
                                    output += (lookAhead('y')
                                               ? date.getFullYear()
                                               : (date.getYear() % 100 < 10 ? '0' : '')
                                               + date.getYear() % 100);
                                    break;
                                case '@':
                                    output += date.getTime();
                                    break;
                                case '!':
                                    output += date.getTime() * 10000 + ticksTo1970;
                                    break;
                                case "'":
                                    if (lookAhead("'")) {
                                        output += "'";
                                    } else {
                                        literal = true;
                                    }
                                    break;
                                default:
                                    output += format.charAt(iFormat);
                            }
                        }
                    }

                    return output;
                },
            },
            getSiteRoot: function () {

                return (siteRoot == '/' ? 'http://localhost:58722/' : siteRoot);
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
                    className = $.app.const.actionErrorClassName;
                    break;
                case 'success':
                    className = $.app.const.actionSuccessClassName;
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

var string = String;

if (!string.format) {
    string.format = function (format) {
        var args = Array.prototype.slice.call(arguments, 1);
        return format.replace(/{(\d+)}/g, function (match, number) {
            return typeof args[number] != 'undefined'
			  ? args[number]
			  : match
            ;
        });
    };
}