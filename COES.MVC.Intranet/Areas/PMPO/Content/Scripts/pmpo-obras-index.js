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
            btnObraNew: $('#btn-obra-new', $element),
            tblObras: $('#tbl-obras', $element),
            txtFecha: $('#txt-fecha', $element),
            chkHistoricos: $('#chk-historicos', $element),
            lblPeriod: $('#lbl-period', $element),
            divFechaMaxRemision: $('#div-fecha-max-remision', $element),
            divTiempoRestante: $('#div-tiempo-restante', $element),
            lblClock: $('#lbl-clock', $element),
            toolbar: $('.button-content', $element),
            content: $('#TableCumplimiento', $element),
        });
    }
    Form.prototype = {
        constructor: Form,
        m_enable: false,
        init: function () {
            var that = this,
                controls = this.getControls();
            controls.txtDateSince.prop('disabled', true);
            controls.txtDateUntil.prop('disabled', true);
            controls.txtDateSince.Zebra_DatePicker({
                format: 'm / Y',
                direction:-1
            });

            controls.txtDateUntil.Zebra_DatePicker({
                format: 'm / Y',
                direction:-1
            });

            controls.btnObraSearch.addEvent(this, 'click', this.btnObraSearch_click);
            controls.btnObraNew.addEvent(this, 'click', this.btnObraNew_click);
            controls.chkHistoricos.addEvent(this, 'click', this.chkHistoricos_click);

            this.render();
        },
        render: function () {
            var that = this,
                 controls = this.getControls();

            controls.txtFecha.Zebra_DatePicker({
                format: 'm / Y', // Muestra el contenido con el formato MES / AÑO
                direction:1,
                onSelect: function () {
                    controls.toolbar.hide();
                    controls.content.hide();
                    console.log(123456);
                    var mesSelected = that.getMesElaboracion().substr(0, 2) - 1;
                    var anyoSelected = that.getMesElaboracion().substr(5, 4);
                    var anyoFin = (anyoSelected == 1) ? 12 : (parseInt(anyoSelected) + 1);
                    var DateNow = new Date();
                    controls.lblPeriod.text($.app['const'].months[mesSelected].substr(0, 3).toUpperCase() + ' ' + anyoSelected + ' - ' + $.app['const'].months[(mesSelected - 1) == -1 ? 11 : mesSelected - 1].substr(0, 3).toUpperCase() + ' ' + anyoFin); // JC
                    console.log("mesSelected:" + mesSelected);
                    console.log("anyoSelected:" + anyoSelected);
                    $.ajax({
                        type: 'POST',
                        url: that.getController() + "GetDiffPeriodDate",
                        dataType: 'json',
                        data: { month: mesSelected + 1, year: anyoSelected },
                        cache: false,
                        async: false,
                        success: function (result) {
                            console.log('result mes =' + result);
                            if (result) {
                                controls.divFechaMaxRemision.show();
                                controls.divTiempoRestante.show();
                            }
                            else {
                                controls.divFechaMaxRemision.hide();
                                controls.divTiempoRestante.hide();
                            }
                        },
                        error: function () {
                            //controls.divMessage.showMessage('error', 'Error al descargar plantilla.');
                        }
                    });
                }
            });

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
                   });
            } else {
                controls.lblClock.html('Cerrado');
            }
        },

        btnObraSearch_click: function (sender, args) {
            var that = this,
                controls = this.getControls();

            controls.divMessage.hideMessage();

            this.fillObras();
        },
        fillObras: function () {
            var that = this,
                controls = this.getControls(),
                fechaIni = controls.txtDateSince.val(),
                fechaFin = controls.txtDateUntil.val(),
                idEmpresa = controls.ddlCompanies.find('> option:selected').attr('value'),
                idTipoObra = controls.ddlObraTypes.find('> option:selected').attr('value'),
                indFechaHist = (controls.chkHistoricos.prop('checked') ? "S" : "N"),
                fechaPer = controls.txtFecha.val();

            var splitfechaini = fechaIni.split('/');
            var splitfechafin = fechaFin.split('/');
            var sfechaIni = splitfechaini[1].trim().concat(splitfechaini[0].trim());
            var sfechaFin = splitfechafin[1].trim().concat(splitfechafin[0].trim());

            if (parseInt(sfechaIni) > parseInt(sfechaFin)) {
                controls.divMessage.showMessage('error', "Por favor escoja un rango correcto de fecha...");
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
                        idTipoObra: idTipoObra,
                        indFechaHist: indFechaHist,
                        fechaPer: fechaPer
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
        chkHistoricos_click: function () {
            var that = this,
                controls = this.getControls(),
                option = controls.chkHistoricos.prop('checked');

            if (option) {
                controls.txtDateSince.enable(true);
                controls.txtDateUntil.enable(true);
                controls.txtFecha.enable(false);
            }
            else {
                controls.txtDateSince.enable(false);
                controls.txtDateUntil.enable(false);
                controls.txtFecha.enable(true);
            }


        },
        btnObraNew_click: function (sender, args) {
            $.app.window.open({
                url: this.getController() + 'Edit',
                title: 'Nueva Obra',
                height: '400px',
                width: '600px'
            });
        },
        btnObraEdit_click: function (sender, args) {
            var that = this,
                id = sender.data('obraId'),
                typeId = sender.data('obraTypeId');

            $.app.window.open({
                url: this.getController() + 'Edit',
                data: { obraId: id, obraTypeId: typeId },
                title: 'Editar Obra',
                height: '400px',
                width: '600px',
                close: function () { that.fillObras(); }
            });
        },
        btnObraDelete_click: function (sender, args) {
            var that = this,
                controls = this.getControls(),
                id = sender.data('obraId'),
                typeId = sender.data('obraTypeId');

            controls.divMessage.hideMessage();

            $.ajax({
                type: "POST",
                url: this.getController() + 'DeleteObraJsonResult',
                data: {
                    obraId: id,
                    obraTypeId: typeId
                },
                cache: false,
                global: false,
                success: function (response) {
                    controls.divMessage.showMessage('success', 'El registro se eliminó satisfactoriamente.');
                    that.fillObras();
                },
                error: function () {
                    controls.divMessage.showMessage('error', 'Hubo un error en la búsqueda de obras.');
                }
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
                                $aEdit = $('<a>'),
                                $aDelete = $('<a>');

                            $aEdit
                                .data('obraId', data['id'])
                                .data('obraTypeId', data['obraTypeId'])
                                .html('Editar')
                                .css('cursor', 'pointer')
                                .addEvent(that, 'click', that.btnObraEdit_click);

                            $aDelete
                                .data('obraId', data['id'])
                                .data('obraTypeId', data['obraTypeId'])
                                .html('Eliminar')
                                .css('cursor', 'pointer')
                                .addEvent(that, 'click', that.btnObraDelete_click);

                            $td.append($aEdit);
                            $td.append('&nbsp;|&nbsp;');
                            $td.append($aDelete);
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
        getEnable: function () {
            return this.m_enable;
        },
        setEnable: function (value) {
            this.m_enable = value;
        },
        getDateForSubmission: function () {

            return this.dateForSubmission;
        },
        getMesElaboracion: function () {
            var value = this.getControls().txtFecha.val();

            return (value ? value : null);
        },
        getController: function () {
            return $.app.getSiteRoot() + "/PMPO/Obras/";
        },
        getControls: function () {
            return this.m_controls || {};
        },
        setControls: function (value) {
            this.m_controls = value;
        }
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