(function ($, undefined) {
    'use strict';

    var Form = function ($element, options) {
        $.extend(this, $.fn.form.defaults, $element.data(), typeof options === 'object' && options);

        this.setControls({
            form: $element,
            divMessage: $('#div-message', $element),
            ddlObraTypes: $('#ddl-obra-types', $element),
            txtPlannedDate: $('#txt-planned-date', $element),
            txtNotes: $('#txt-notes', $element),
            btnObraSave: $('#btn-obra-save', $element),
            btnObraCancel: $('#btn-obra-cancel', $element),
            btnObraDetailAdd: $('#btn-obra-detail-add', $element),
            btnObraDetailDelete: $('#btn-obra-detail-delete', $element),
            tblObraDetails: $('#tbl-obra-details', $element)
        });
    }
    Form.prototype = {
        constructor: Form,
        init: function () {
            var that = this,
                controls = this.getControls();

            controls.txtPlannedDate.Zebra_DatePicker({
                format: 'd/m/Y'
            });

            controls.btnObraSave.addEvent(this, 'click', this.btnObraSave_click);
            controls.btnObraCancel.addEvent(this, 'click', this.btnObraCancel_click);

            this.render();
        },
        render: function () {
            var that = this,
                controls = this.getControls();

            controls.ddlObraTypes.populateDropDown(this.fetchObraTypes());
            controls.ddlObraTypes.enable(false);
            controls.txtNotes.enable(false);

            var obraId = this.getObraId();
            //obtener el Tipo de Obra de la Fila
            var tObraId = this.getObraTypeId();
            console.log(tObraId);
            if (obraId != null) {
                $.ajax({
                    type: "POST",
                    url: this.getController() + 'FetchJsonResult',
                    data: {
                        obraId: parseInt(obraId),
                        tObraId: parseInt(tObraId)
                    },
                    cache: false,
                    success: function (response) {
                        console.log(response);
                        controls.form.attr('data-company-id', response.companyId);
                        controls.ddlObraTypes.val(response.obraTypeId);
                        controls.txtPlannedDate.val(response.plannedDate);
                        controls.txtNotes.val(response.notes);
                        
                        that.populateDetails(response.details);
                    },
                    error: function () {
                        controls.divMessage.showMessage('error', 'Hubo un error en la búsqueda de obras.');
                    }
                });
            } else {
                this.populateDetails(null);
            }
        },

        btnObraSave_click: function (sender, args) {
            var controls = this.getControls(),
                valEvaluate,
                message = '';

            controls.divMessage.hideMessage();

            valEvaluate = controls.ddlObraTypes.val();

            if (valEvaluate == null || ($.isNumeric(valEvaluate) && valEvaluate == 0)) {
                message += '<li>Debe seleccionar el tipo de obra.</li>';
            }

            valEvaluate = controls.txtPlannedDate.val();

            if (valEvaluate.length == 0) {
                message += '<li>Debe seleccionar la fecha planificada.</li>';
            }

            valEvaluate = controls.txtNotes.val();

            if (valEvaluate.length == 0) {
                message += '<li>Debe proporcionar el comentario.</li>';
            }

            if (message.length > 0) {
                controls.divMessage.showMessage('error', ('<ul>' + message + '</ul>'));
            } else {
                var data = {
                    id: this.getObraId(),
                    obraTypeId: this.getObraTypeIdSelected(),
                    plannedDate: this.getPlannedDated(),
                    notes: this.getNotes(),
                    details: this.getDetails()
                };

                $.ajax({
                    type: 'POST',
                    url: this.getController() + 'saveobrajsonresult',
                    data: { value: JSON.stringify(data) },
                    success: function (data, textStatus, jqXHR) {
                        controls.divMessage.showMessage('success', 'Se grabó correctamente');
                        $.app.window.close();
                    },
                    error: function (jqXHR, textStatus, errorThrow) {
                        controls.divMessage.showMessage('error', 'Existe un error al grabar');
                    }
                });
            }
        },
        btnObraCancel_click: function (sender, args) {
            $.app.window.close();
        },
        btnObraDetailAdd_click: function (sender, args) {
            var controls = this.getControls();

            controls.tblObraDetails.dataTable().fnAddData({ barId: -1, groupId: -1, teamId: -1, description: '' });
        },
        btnObraDetailDelete_click: function (sender, args) {
            var rowIndex = sender.parent().parent().index();

            this.getControls().tblObraDetails.dataTable().fnDeleteRow(rowIndex);
        },

        getObraId: function () {
            var obraId = this.getControls().form.data('obraId');

            if (obraId != null && $.isNumeric(obraId) && obraId > 0) {
                obraId = parseInt(obraId);
            } else {
                obraId = null;
            }

            return obraId;
        },
        getObraTypeId: function () {
            var obraId = this.getControls().form.data('obraTypeId');

            if (obraId != null && $.isNumeric(obraId) && obraId > 0) {
                obraId = parseInt(obraId);
            } else {
                obraId = null;
            }

            return obraId;
        },
        getObraTypeIdSelected: function () {
            return this.getControls().ddlObraTypes.val();
        },
        getPlannedDated: function () {
            return this.getControls().txtPlannedDate.val();
        },
        getNotes: function () {
            return this.getControls().txtNotes.val();
        },
        getDetails: function () {
            var controls = this.getControls(),
                $rows = controls.tblObraDetails.find('tbody > tr'),
                items = [],
                item;

            $.each($rows, function (rowIndex, row) {
                var $row = $(row);

                item = {
                    barId: $row.find('> td:eq(0) > select > option:selected').attr('value'),
                    groupId: $row.find('> td:eq(1) > select > option:selected').attr('value'),
                    teamId: $row.find('> td:eq(2) > select > option:selected').attr('value'),
                    description: $row.find('> td:eq(3) > input').val()
                };

                items.push(item);
            });


            return items;
        },

        populateDetails: function (data) {
            var that = this,
                controls = this.getControls();
            console.log(data)
            controls.tblObraDetails.dataTable({
                data: data,
                paging: false,
                searching: false,
                info: false,
                columns: [
                    {
                        data: 'barId',
                        title: 'Barra',
                        defaultContent: -1,
                        createdCell: function (td, value, data, row, col) {
                            var $select = $('<select>'),
                                bars = $.extend([], that.fetchBars(controls.form.data('companyId')));

                            bars.splice(0, 0, { id: null, text: '[ Ninguno ]' });

                            $select
                                .css('width', '100%')
                                .populateDropDown(bars, { selectedValue: value })
                                .enable(false);

                            $(td).append($select);
                        },
                        render: function (data, type, full) {
                            return '';
                        }
                    },
                    {
                        data: 'groupId',
                        title: 'Grupo',
                        defaultContent: -1,
                        createdCell: function (td, value, data, row, col) {
                            var $select = $('<select>'),
                                groups = $.extend([], that.fetchGroups(controls.form.data('companyId')));

                            groups.splice(0, 0, { id: null, text: '[ Ninguno ]' });

                            $select
                                .css('width', '100%')
                                .populateDropDown(groups, { selectedValue: value })
                                .enable(false);

                            $(td).append($select);
                        },
                        render: function (data, type, full) {
                            return '';
                        }
                    },
                    {
                        data: 'teamId',
                        title: 'Equipo',
                        defaultContent: -1,
                        createdCell: function (td, value, data, row, col) {
                            var $select = $('<select>'),
                                teams = $.extend([], that.fetchTeams(controls.form.data('companyId')));

                            teams.splice(0, 0, { id: null, text: '[ Ninguno ]' });

                            $select
                                .css('width', '100%')
                                .populateDropDown(teams, { selectedValue: value })
                                .enable(false);

                            $(td).append($select);
                        },
                        render: function (data, type, full) {
                            return '';
                        }
                    },
                    {
                        data: 'description',
                        title: 'Descripción',
                        defaultContent: '',
                        createdCell: function (td, value, data, row, col) {
                            var $input = $('<input>');

                            $input.css('width', '99.5%');
                            $input.val(value);
                            $input.enable(false);

                            $(td).append($input);
                        },
                        render: function (data, type, full) {
                            return '';
                        }
                    }
                ]
            });
        },

        fetchObraTypes: function () {
            var items;

            $.ajax({
                type: 'POST',
                url: this.getController() + 'fetchobratypesjsonresult',
                async: false,
                success: function (data, textStatus, jqXHR) {
                    items = data;
                },
                error: function (jqXHR, textStatus, errorThrow) {
                    items = null;
                }
            });

            return items;
        },
        fetchGroups: function (companyId) {
            var that = this;

            if (this._groups == null) {
                $.ajax({
                    type: 'POST',
                    url: this.getController() + 'fetchgroupsjsonresult',
                    data: { companyId: companyId },
                    async: false,
                    success: function (data, textStatus, jqXHR) {
                        that._groups = data;
                    },
                    error: function (jqXHR, textStatus, errorThrow) {
                        that._groups = null;
                    }
                });
            }

            return this._groups;
        },
        fetchTeams: function (companyId) {
            var that = this;

            if (this._teams == null) {
                $.ajax({
                    type: 'POST',
                    url: this.getController() + 'fetchteamsjsonresult',
                    data: { companyId: companyId },
                    async: false,
                    success: function (data, textStatus, jqXHR) {
                        that._teams = data;
                    },
                    error: function (jqXHR, textStatus, errorThrow) {
                        that._teams = null;
                    }
                });
            }

            return this._teams;
        },
        fetchBars: function (companyId) {
            var that = this;

            if (this._bars == null) {
                $.ajax({
                    type: 'POST',
                    data: { companyId: companyId },
                    url: this.getController() + 'fetchbarsjsonresult',
                    async: false,
                    success: function (data, textStatus, jqXHR) {
                        that._bars = data;
                    },
                    error: function (jqXHR, textStatus, errorThrow) {
                        that._bars = null;
                    }
                });
            }

            return this._bars;
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
        obraId: null,
        obraTypeId: null
    }
})(jQuery, null);