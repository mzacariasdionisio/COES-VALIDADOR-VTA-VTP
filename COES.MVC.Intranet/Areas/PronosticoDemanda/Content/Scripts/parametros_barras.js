$(document).ready(function () {
    //Aplica la lib. Zebra_Datepicker
    $('.f-fecha').Zebra_DatePicker({
        onSelect: function () {//Evento onChange
            var a = $('#id-desde').val();
            var b = $('#id-hasta').val();
            if (ValidateDates(a, b) == false) {
                b = AddDays(a);
                $('#id-hasta').val(b);
            }
            dt.ajax.reload();
        }
    });

    //Aplica la lib. MultiSelect
    $('.f-select').each(function () {
        var element = this;
        $(element).multipleSelect({
            name: element.id,
            filter: true,
            single: false,
            placeholder: 'Seleccione',
            onClose: function () {
                var e = document.getElementById(this.name);
                dt.ajax.reload();
            },
            onCheckAll: function () {
                var e = document.getElementById(this.name);
                $(e).val(null);
            }
        });
    });

    $('.chk-single input[type="checkbox"]').on('click', function () {
        ParameterSingleOnOff(this, '.chk-single', true);
    });

    $('.chk-group-popup input[type="checkbox"]').on('click', function () {
        ParameterGroupOnOff(this, '.chk-group-popup', false);
    });

    $('.chk-single-popup input[type="checkbox"]').on('click', function () {
        ParameterSingleOnOff(this, '.chk-single-popup', false);
    });

    $('#btn-defecto').on('click', function () {
        pop = $('#popup').bPopup({
            easing: 'easeOutBack',
            speed: 350,
            transition: 'fadeIn',
            modalClose: false,
            onClose: function () {
                //Reinicia los campos del popup
                $('.chk-group-popup input[type="checkbox"]').each(function () {
                    $(this).prop('checked', false);
                    ParameterGroupOnOff(this, '.chk-group-popup', true);
                });
            }
        },
            function () {
                $.ajax({
                    type: 'POST',
                    url: controller + 'ParametrosBarrasGetDefecto',
                    contentType: 'application/json; charset=utf-8',
                    datatype: 'json',
                    traditional: true,
                    success: function (result) {
                        //Setea el mensaje
                        SetMessage('#message-popup', 'Active e ingrese los nuevos parámetros por defecto', 'info');

                        var all_groups = $('.by-popup');//Contiene todos los grupos de parametros del popup
                        all_groups.each(function () {
                            var g_item = this;
                            var g_parameters = $(g_item).find('.chk-single-popup input[type="checkbox"]');
                            g_parameters.each(function () {
                                var s_item = this;
                                var field_name = s_item.id.replace('pop-', '');//Obtiene el nombre del campo del objeto
                                var s_target = $(s_item).closest('.chk-single-popup').next();

                                var s_item_num = $(s_target).find('input[type="number"]');
                                var s_item_rdo = $(s_target).find('input[type="radio"]');

                                //Evalua el tipo de entrada
                                if (s_item_num.length != 0) {
                                    //setea el valor correspondiente
                                    s_item_num.val(result[field_name]);
                                }
                                if (s_item_rdo.length != 0) {
                                    //setea el valor correspondiente
                                    s_item_rdo.each(function () {
                                        var rdo_option = this;
                                        test = rdo_option;
                                        console.log(rdo_option, 'rdo_res');
                                        if (rdo_option.value == result[field_name]) {
                                            $(rdo_option).prop('checked', true);
                                        }
                                    });
                                }
                            });
                        });
                    },
                    error: function () {
                        alert("Ha ocurrido un problema...");
                    }
                });
            });
    });

    $('#btn-guardar').on('click', function () {
        var fil = GetFiltersValues();
        var res = GetParameters();
        if (res == -1) {
            SetMessage('#message',
                'Debe activar al menos un parámetro para realizar el registro!',
                'warning', true);
            return false;
        }
        if (res == -2) {
            SetMessage('#message',
                'Existen parámetros activos a los que no se le han asignado valores!',
                'warning', true);
            return false;
        }

        Save(fil, res);
    });

    $('#btn-pop-guardar').on('click', function () {
        var res = GetParametersPopup();
        if (res == -1) {
            SetMessage('#message-popup',
                'Debe activar al menos un parámetro para realizar el registro!',
                'warning', true);
            return false;
        }
        if (res == -2) {
            SetMessage('#message-popup',
                'Existen parámetros activos a los que no se le han asignado valores!',
                'warning', true);
            return false;
        }

        UpdateDefecto(res);
    });
    
    
    dt = $('#dtgrid').DataTable({
        serverSide: true,
        ajax: {
            type: "POST",
            url: controller + 'ParametrosList',
            contentType: 'application/json; charset=utf-8',
            data: function (d) {
                d.idModulo = $('#main').data('mdl');
                d.dataFiltros = GetFiltersValues();
                return JSON.stringify(d);
            },
            datatype: 'json',
            traditional: true
        },
        columns: [
            { title: 'BARRA', data: 'Gruponomb' },
            { title: 'FECHA', data: 'StrCfgbarfecha' },
            { title: 'SUAVIZADO EXPONENCIAL', data: 'Cfgbarpse' },
            { title: 'FACTOR F', data: 'Cfgbarfactorf' }
        ],
        initComplete: function () {
            $('#dtgrid').css('width', '100%');
            $('.dataTables_scrollHeadInner').css('width', '100%');
            $('.dataTables_scrollHeadInner table').css('width', '98.85%');
        },
        drawCallback: function () {
            $('#dtgrid').css('width', '100%');
            $('.dataTables_scrollHeadInner').css('width', '100%');
            $('.dataTables_scrollHeadInner table').css('width', '98.85%');
        },
        //columnDefs: [
        //    {
        //        targets: '_all',
        //        createdCell: function (td, cellData, rowData, row, col) {
        //            if (rowData.Prncfgtiporeg != 1) {
        //                $(td).css('background', '#FFD7CF');
        //            }
        //        }

        //    }
        //],
        scrollY: '270px',
        searching: false,
        bLengthChange: false,
        bSort: false,
        destroy: true,
        scrollCollapse: true,
        paging: true,
        pageLength: 25,
        info: false,
    });
    
});

//Envia la información modificada para su registro
function Save(filters, data) {
    $.ajax({
        type: 'POST',
        url: controller + 'ParametrosBarrasSave',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            dataFiltros: filters,
            dataParametros: data
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {
            dt.ajax.reload();
            SetMessage('#message', result.dataMsg, result.typeMsg);
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

function UpdateDefecto(data) {
    $.ajax({
        type: 'POST',
        url: controller + 'ParametrosBarrasUpdateDefecto',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            dataParametros: data
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {
            pop.close();
            dt.ajax.reload();
            SetMessage('#message', result.dataMsg, result.typeMsg);
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

//Recoge los parametros activos para el registro
function GetParameters() {
    var res_object = {};
    var all_groups = $('.parametros-group');//Contiene todos los grupos de parametros
    $.each(all_groups, function (i, g_item) {
        var all_checked = $(g_item).find('.chk-single input[type="checkbox"]:checked');//Obtiene todos los elementos individuales seleccionados
        $.each(all_checked, function (j, s_item) {
            var field_value;
            var field_name = s_item.id;//Obtiene el nombre del parametro a registrar
            var s_target = $(s_item).closest('.chk-single').next();
            //Obtiene las posibles entradas
            var s_item_num = $(s_target).find('input[type="number"]');
            var s_item_rdo = $(s_target).find('input[type="radio"]:checked');
            //Evalua el tipo de entrada
            if (s_item_num.length != 0) {
                //obtener el valor
                field_value = s_item_num.val();
            }
            if (s_item_rdo.length != 0) {
                //obtener el valor
                field_value = s_item_rdo.val();
            }
            //Carga el campo del objeto
            res_object[field_name] = field_value;
        });
    });

    //Validaciones
    //Objeto vacio
    if (jQuery.isEmptyObject(res_object)) {
        res_object = -1;
    }
    //Campos null or empty
    $.each(res_object, function (i, value) {
        if (!value) {
            res_object = -2;
            return false;
        }
    });

    return res_object;
}

function GetParametersPopup() {
    var res_object = {};
    var all_groups = $('.by-popup');//Contiene todos los grupos de parametros del popup
    $.each(all_groups, function (i, g_item) {
        var all_checked = $(g_item).find('.chk-single-popup input[type="checkbox"]:checked');//Obtiene todos los elementos individuales seleccionados
        $.each(all_checked, function (j, s_item) {
            var field_value;
            var field_name = s_item.id.replace('pop-', '');//Obtiene el nombre del parametro a registrar
            var s_target = $(s_item).closest('.chk-single-popup').next();
            //Obtiene las posibles entradas
            var s_item_num = $(s_target).find('input[type="number"]');
            var s_item_rdo = $(s_target).find('input[type="radio"]:checked');
            //Evalua el tipo de entrada
            if (s_item_num.length != 0) {
                //obtener el valor
                field_value = s_item_num.val();
            }
            if (s_item_rdo.length != 0) {
                //obtener el valor
                field_value = s_item_rdo.val();
            }
            //Carga el campo del objeto
            res_object[field_name] = field_value;
        });
    });

    //Validaciones
    //Objeto vacio
    if (jQuery.isEmptyObject(res_object)) {
        res_object = -1;
    }
    //Campos null or empty
    $.each(res_object, function (i, value) {
        if (!value) {
            res_object = -2;
            return false;
        }
    });

    return res_object;
}

//Setea el modelo de envio de información
function GetFiltersValues() {
    var model = {};
    model['FechaIni'] = $('#id-desde').val();
    model['FechaFin'] = $('#id-hasta').val();
    model['SelBarra'] = $('#id-barra').val();
    return model;
}

//Limpia las opciones del grupo de parámetros
function CleanParameters() {
    $('.parametros-group input[type="radio"]').prop('checked', false);
    $('.parametros-group input[type="number"]').val('');
}

//Activa o desactiva un solo item
function ParameterSingleOnOff(element, elem_class, clear) {
    var target = $(element).closest(elem_class).next();
    var is_num = $(target).find('input[type="number"]');
    var is_rdo = $(target).find('input[type="radio"]');

    if (element.checked) {
        is_num.prop('disabled', false);
        is_rdo.prop('disabled', false);
    }
    else {
        is_num.prop('disabled', true);
        is_rdo.prop('disabled', true);

        if (clear) {
            is_num.prop('value', '');
            is_rdo.prop('checked', false);
        }
    }
}

//Activa o desactiva items de manera grupal
function ParameterGroupOnOff(element, elem_class, clear) {
    console.log(clear);
    var target = $(element).closest(elem_class).next();
    var all_chk = $(target).find('input[type="checkbox"]');
    var all_num = $(target).find('input[type="number"]');
    var all_rdo = $(target).find('input[type="radio"]');

    if (element.checked) {
        all_chk.prop('checked', true);
        all_num.prop('disabled', false);
        all_rdo.prop('disabled', false);
    }
    else {
        all_chk.prop('checked', false);

        all_num.prop('disabled', true);
        all_rdo.prop('disabled', true);

        if (clear) {
            all_num.prop('value', '');
            all_rdo.prop('checked', false);
        }
    }
}