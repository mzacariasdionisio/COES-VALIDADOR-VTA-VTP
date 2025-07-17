(function ($, undefined) {
    'use strict';

    var Form = function ($element, options) {
        $.extend(this, $.fn.form.defaults, $element.data(), typeof options === 'object' && options);

        this.setControls({
            form: $element,
            divMessage: $('#div-message', $element),
            ddlCompanies: $('#cboEmpresa', $element),
            ddlObraTypes: $('#cboTipoObra', $element),
            txtDateSince: $('#txt-date-since', $element),
            txtDateUntil: $('#txt-date-until', $element),
            btnObraSearch: $('#btn-obra-search', $element),
            tblObras: $('#tbl-obras', $element),
            lblClock: $('#lbl-clock', $element),
        });
    }
    Form.prototype = {
        constructor: Form,
        init: function () {
            var that = this,
                controls = this.getControls();

            controls.txtDateSince.Zebra_DatePicker({
                format: 'm / Y'
            });

            controls.txtDateUntil.Zebra_DatePicker({
                format: 'm / Y'
            });

            controls.btnObraSearch.addEvent(this, 'click', this.btnObraSearch_click);

            this.render();
        },
        render: function () {
            var that = this,
                controls = this.getControls();

            if (this.dateForSubmission != null) {
                controls.lblClock
                   .countdown(this.dateForSubmission)
                   .on('update.countdown', function (e) {
                       if (!that.getEnable()) {
                           that.setEnable(true);
                       }

                       $(this).html(e.strftime('%D dia%!d ' + '%H hr ' + '%M min ' + '%S sec'));
                   })
                   .on('finish.countdown', function (e) {
                       that.setEnable(false);

                       $(this).html('Cerrado');
                       controls.lblClock.css('color', 'red');
                   });
            } else {
                that.setEnable(false);
                controls.lblClock.html('Cerrado');
                controls.lblClock.css('color', 'red');
            }

        },

        btnObraSearch_click: function (sender, args) {
            this.fillObras();
        },
        fillObras: function () {
            var that = this,
                controls = this.getControls(),
                fechaIni = controls.txtDateSince.val(),
                fechaFin = controls.txtDateUntil.val(),
                idEmpresa = controls.ddlCompanies.find('> option:selected').attr('value'),
                idTipoObra = controls.ddlObraTypes.find('> option:selected').attr('value');

            controls.divMessage.hideMessage();

            var splitfechaini = fechaIni.split('/');
            var splitfechafin = fechaFin.split('/');
            var sfechaIni = splitfechaini[1].trim().concat(splitfechaini[0].trim());
            var sfechaFin = splitfechafin[1].trim().concat(splitfechafin[0].trim());

            if (parseInt(sfechaIni) > parseInt(sfechaFin)) {
                controls.divMessage.showMessage('error', "Por favor escoja un rango correcto de fecha ...");
            } else if (idTipoObra == null || !$.isNumeric(idTipoObra) || idTipoObra <= 0) {
                controls.divMessage.showMessage('error', "Por favor seleccione el tipo...")
            } else {
                $.ajax({
                    type: "POST",
                    url: this.getController() + 'FetchObrasJsonResult',
                    data: {
                        fechaIni: fechaIni,
                        fechaFin: fechaFin,
                        idEmpresa: idEmpresa,
                        idTipoObra: idTipoObra
                    },
                    cache: false,
                    global: false,
                    success: function (response) {
                        that.populateGrid(response);
                    },
                    error: function () {
                        controls.divMessage.showMessage('error', 'Hubo un error en la búsqueda de obras.');
                    }
                });
            }
        },
        btnObraEdit_click: function (sender, args) {
            var that = this,
                id = sender.data('obraId'),
                typeId = sender.data('obraTypeId');

            $.app.window.open({
                url: this.getController() + 'Edit',
                data: {
                    obraId: id,
                    obraTypeId: typeId
                },
                title: 'Editar Obra',
                height: '400px',
                width: '600px',
                close: function () { that.fillObras(); }
            });
        },

        populateGrid: function (obras) {
            var that = this,
                controls = this.getControls();

            controls.tblObras.dataTable({
                destroy: true,
                paging: false,
                searching: false,
                ordering: false,
                info: false,
                language: {
                    loadingRecords: $.app['const'].loadingRecords,
                    zeroRecords: $.app['const'].zeroRecords,
                },
                data: obras,
                columns: [
                    {
                        data: null,
                        title: '',
                        defaultContent: '',
                        createdCell: function (td, value, data, rowIndex, colIndex){
                            var $td = $(td),
                                $a = $('<a>');

                            $a
                                .data('obraId', data['id'])
                                .data('obraTypeId', data['obraTypeId'])
                                .html('Editar')
                                .addEvent(that, 'click', that.btnObraEdit_click);

                            $td.append($a);
                        },
                        render: function () {
                            return '';
                        }
                    },
                    { data: 'id', title: 'ID Obra' },
                    { data: 'CompanyName', title: 'Empresa' },
                    { data: 'obraTypeName', title: 'Tipo de Obra' },
                    { data: 'plannedDate', title: 'Fecha Planificada' },
                    { data: 'gruponomb', title: 'Grupo' },
                    { data: 'barranomb', title: 'Barra' },
                    { data: 'equinomb', title: 'Equipo' },
                    { data: 'obraEnFormato', title: 'EnFormato' },
                    { data: 'notes', title: 'Descripci&oacute;n Obra' }
                ]
            });
        },

        getController: function () {
            return $.app.getSiteRoot() + "/PMPO/Obras/";
        },
        getControls: function () {
            return this.m_controls || {};
        },
        setControls: function (value) {
            this.m_controls = value;
        },
        getEnable: function () {
            return this.m_enable;
        },
        setEnable: function (value) {
            this.m_enable = value;
        },
        getDateForSubmission: function () {
            return this.dateForSubmission;
        },
    }

    $.fn.form = function () {
        var option = arguments[0],
            args = arguments,
            value,
            allowedMethods = [];

        this.each(function () {
            var $this = $(this),
                data = $this.data('form'),
                options = $.extend({}, $.fn.form.defaults,
                    $this.data(), typeof option === 'object' && option);

            if (!data) {
                data = new Form($this, options);
                $this.data('form', data);
            }

            if (typeof option === 'string') {
                if ($.inArray(option, allowedMethods) < 0) {
                    throw "Unknown method: " + option;
                }
                value = data[option](args[1]);
            } else {
                data.init();
                if (args[1]) {
                    value = data[args[1]].apply(data, [].slice.call(args, 2));
                }
            }
        });

        return value || this;
    }

    $.fn.form.defaults = {
        dateForSubmission: null,
    }
})(jQuery, null);