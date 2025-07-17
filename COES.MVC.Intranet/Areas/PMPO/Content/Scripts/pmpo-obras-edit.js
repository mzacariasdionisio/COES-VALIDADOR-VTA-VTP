(function ($, undefined) {
    'use strict';

    var Form = function ($element, options) {
        $.extend(this, $.fn.form.defaults, $element.data(), typeof options === 'object' && options);

        this.setControls({
            form: $element,
            divMessage: $('#div-message', $element),
            ddlObraTypes: $('#ddl-obra-types', $element),
            ddlObraFormat: $('#ddl-obra-format', $element),
            txtPlannedDate: $('#txt-planned-date', $element),
            txtNotes: $('#txt-notes', $element),
            btnObraSave: $('#btn-obra-save', $element),
            btnObraCancel: $('#btn-obra-cancel', $element),
            btnObraDetailAdd: $('#btn-obra-detail-add', $element),
            btnObraDetailDelete: $('#btn-obra-detail-delete', $element),
            tblObraDetails: $('#tbl-obra-details', $element),
            ddlCompanies: $('#ddl-companies', $element)
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
            controls.btnObraDetailAdd.addEvent(this, 'click', this.btnObraDetailAdd_click);
            controls.btnObraDetailDelete.addEvent(this, 'click', this.btnObraDetailDelete_click);
            controls.btnObraCancel.addEvent(this, 'click', this.btnObraCancel_click);
            controls.ddlCompanies.addEvent(this, 'change', this.ddlCompanies_change);

            this.render();
        },
        render: function () {
            var that = this,
                controls = this.getControls();

            console.log('carga datos ini..');
            controls.ddlCompanies.populateDropDown(this.fetchCompanies());
            controls.ddlObraTypes.populateDropDown(this.fetchObraTypes());
            controls.ddlObraFormat.populateDropDown(this.FlagFormatObra());
            console.log('this.FlagFormatObra()> ----' + this.FlagFormatObra());
            //controls.ddlObraFormat.enable(true);

            var obraId = this.getObraId(),
                tObraId = this.getObraTypeId();

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
                        console.log('Respuesta .... Valor de Format seleccionado' + controls.ddlObraFormat.val()); ///AQUI CAMBIA
                        controls.ddlCompanies.val(response.companyId);
                        controls.ddlObraTypes.val(response.obraTypeId);
                        controls.txtPlannedDate.val(response.plannedDate);
                        controls.txtNotes.val(response.notes);
                        controls.ddlObraFormat.val(response.ObraFlagFormat);

                        that.populateDetails(response.details);
                    },
                    error: function () {
                        controls.divMessage.showMessage('error', 'Hubo un error en la búsqueda de obras.');
                    }
                });
            } else {
                console.log('puebla sin dato de Obra');
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

            var $tbodyDetails = controls.tblObraDetails.find('> tbody > tr[role="row"]');

            if ($tbodyDetails.length > 0) {
                for (var i = 0, j = $tbodyDetails.length; i < j; i++) {
                    var $row = $($tbodyDetails[i]),
                        barId = $row.find('> td:eq(0) > select > option:selected').attr('value'),
                        groupId = $row.find('> td:eq(1) > select > option:selected').attr('value'),
                        teamId = $row.find('> td:eq(2) > select > option:selected').attr('value'),
                        description = $row.find('> td:eq(3) > input').val();

                    if (barId == null && groupId == null && teamId == null) {
                        message += '<li>Debe seleccionar una barra o grupo o equipo.</li>';
                        break;
                    }
                    if (description.length == 0) {
                        message += '<li>Debe proporcionar la descripción del detalle de obra.</li>';
                        break;
                    }
                }
            } else {
                message += '<li>Debe proporcionar el detalle de obra.</li>';
            }

            if (message.length > 0) {
                controls.divMessage.showMessage('error', ('<ul>' + message + '</ul>'));
            } else {
                console.log('Aqui reecupera valores...companyID = ' + this.getCompanyIdSelected());
                console.log('Valor de Format seleccionado[' + controls.ddlObraFormat.val() + "]"); ///AQUI CAMBIA
                console.log('Valor de Company seleccionado[' + controls.ddlCompanies.val() + "]"); ///AQUI CAMBIA
                console.log('Invocar FetchJsonResult..');

                var data = {
                    id: this.getObraId(),
                    companyId: this.getCompanyIdSelected(),
                    obraTypeId: this.getObraTypeIdSelected(),
                    formato: this.getObraFormatSelected(),
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


            var controls = this.getControls(),
                count = 1;

            controls.tblObraDetails.dataTable().fnAddData({ barId: -1, groupId: -1, teamId: -1, description: '' });
            if (controls.tblObraDetails.data('count') == null)
            {
                controls.tblObraDetails.data('count', count);
            }
            else
            {
                    count = controls.tblObraDetails.data('count');
                    count++;
                    console.log('count Add = ' + count);
                    controls.tblObraDetails.data('count', count);
            }
        },
        ddlCompanies_change: function (sender, args) {

            this._bars = null;
            this._groups = null;
            this._teams = null;

            this.populateDetails(null);
            //console.log('Ingreso a company list');

            //var that = this,
            //    controls = this.getControls();

            //var companyId = this.getCompanyIdSelected();

            //var count = 0;
            //controls.tblObraDetails.data('count', count);
            //this.getControls().tblObraDetails.dataTable().fnDeleteRow(count);

            //console.log('Cambio combo empresa companyID = '+companyId);
        },
        btnObraDetailDelete_click: function (sender, args) {
            var rowIndex = sender.parent().parent().index() - 1,
                controls = this.getControls(),
                count;
            console.log('rowIndex -- ' + rowIndex);

            count = controls.tblObraDetails.data('count');
            count = count - 1;
            console.log('count  Eliminar = ' + count);
            controls.tblObraDetails.data('count', count);

            this.getControls().tblObraDetails.dataTable().fnDeleteRow(count);
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
        getObraIdDet: function () {
            var obraIdDet = this.getControls().form.data('obraIdDet');

            if (obraIdDet != null && $.isNumeric(obraIdDet) && obraIdDet > 0) {
                obraIdDet = parseInt(obraIdDet);
            } else {
                obraIdDet = null;
            }

            return obraIdDet;
        },
        getCompanyIdSelected: function () {
            return this.getControls().ddlCompanies.val();
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
        getObraFormatSelected: function () {
            console.log('valor ddlObraFormat=  ' + this.getControls().ddlObraFormat.val());
            return this.getControls().ddlObraFormat.val();
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
                    description: $row.find('> td:eq(3) > input').val(),
                    Obradetcodi: $row.find('> td:eq(3) > input').data('Obradetcodi')
                };

                items.push(item);
            });


            return items;
        },

        populateDetails: function (data) {
            var that = this,
                controls = this.getControls(),
                companyId = this.getCompanyIdSelected();

            console.log('companyIDselected = ' + companyId);

            controls.tblObraDetails.dataTable({
                data: data,
                paging: false,
                searching: false,
                info: false,
                destroy: true,
                columns: [
                    {
                        data: 'barId',
                        title: 'Barra',
                        defaultContent: -1,
                        createdCell: function (td, value, data, row, col) {
                            var $select = $('<select>'),
                                bars = $.extend([], that.fetchBars(companyId));

                            bars.splice(0, 0, { id: null, text: '[ Ninguno ]' });

                            $select
                                .css('width', '100%')
                                .populateDropDown(bars, { selectedValue: value })
                                .addEvent(that, 'change', that.bars_change);

                            $(td).append($select);
                        },
                        render: function (data, type, full) {
                            console.log("data: " + data);
                            return '';
                        }
                    },
                    {
                        data: 'groupId',
                        title: 'Grupo',
                        defaultContent: -1,
                        createdCell: function (td, value, data, row, col) {
                            var $select = $('<select>'),
                                groups = $.extend([], that.fetchGroups(companyId));

                            groups.splice(0, 0, { id: null, text: '[ Ninguno ]' });

                            $select
                                .css('width', '100%')
                                .populateDropDown(groups, { selectedValue: value })
                                .addEvent(that, 'change', that.groups_change);

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
                                teams = $.extend([], that.fetchTeams(companyId));

                            teams.splice(0, 0, { id: null, text: '[ Ninguno ]' });

                            $select
                                .css('width', '100%')
                                .populateDropDown(teams, { selectedValue: value })
                                .addEvent(that, 'change', that.teams_change);

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
                            $input.data('Obradetcodi', data['Obradetcodi'])
                            
                            $(td).append($input);
                        },
                        render: function (data, type, full) {
                            console.log('data[Obradetcodi]> -------' + data['Obradetcodi']);
                            return '';
                        }
                    }
                ]
            });
        },

        bars_change: function (sender, args) {
            var $tr = sender.parents('tr');

            this.select_change_helper($tr, 'bar');
        },
        groups_change: function (sender, args) {
            var $tr = sender.parents('tr');

            this.select_change_helper($tr, 'group');
        },
        teams_change: function (sender, args) {
            var $tr = sender.parents('tr');

            this.select_change_helper($tr, 'team');
        },

        select_change_helper: function ($tr, type) {
            /*var $bars = $tr.find('td:eq(0) > select'),
                $groups = $tr.find('td:eq(1) > select'),
                $teams = $tr.find('td:eq(2) > select'),
                $current,
                other1,
                other2,
                value;

            switch (type) {
                case 'bar':
                    $current = $bars;
                    other1 = { $control: $groups, event: this.groups_change };
                    other2 = { $control: $teams, event: this.teams_change };

                    break;
                case 'group':
                    $current = $groups;
                    other1 = { $control: $bars, event: this.bars_change };
                    other2 = { $control: $teams, event: this.teams_change };

                    break;
                case 'team':
                    $current = $teams;
                    other1 = { $control: $bars, event: this.bars_change };
                    other2 = { $control: $groups, event: this.groups_change };

                    break;
                default:
                    throw 'No soportado.'
            }

            other1.$control
                .unbind('change')
                .prop('selectedIndex', 0)
                .addEvent(this, 'change', other1.event);

            other2.$control
                .unbind('change')
                .prop('selectedIndex', 0)
                .addEvent(this, 'change', other2.event);

            value = $current.find(' > option:selected').attr('value');*/
        },

        fetchCompanies: function () {
            var items;

            $.ajax({
                type: 'POST',
                url: this.getController() + 'fetchcompaniesjsonresult',
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

            console.log('fechas[0]-ID: ' + items[0].id);
            console.log('fechas[0]-TEXT: ' + items[0].text);
            return items;
        },
        FlagFormatObra: function () {
            //var items;

            //$.ajax({
            //    type: 'POST',
            //    url: this.getController() + 'FetchObraFlagformatJsonResult',
            //    async: false,
            //    success: function (data, textStatus, jqXHR) {
            //        items = data;
            //    },
            //    error: function (jqXHR, textStatus, errorThrow) {
            //        items = null;
            //    }
            //});

            //console.log('fechas[0]-ID: ' + items[0].id);
            //console.log('fechas[0]-TEXT: ' + items[0].text);
            //return items;

            return [{ id: 1, text: 'Si' }, { id: 2, text: 'No' }];

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
                    url: this.getController() + 'fetchbarsjsonresult',
                    data: { companyId: companyId },
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