$(document).ready(function () {
    //Aplica la lib. Zebra_Datepicker
    $('.f-fecha').Zebra_DatePicker();

    $('.f-select-s').each(function () {
        var element = this;
        $(element).multipleSelect({
            single: true
        });
    });

    $('.f-select-m').each(function () {
        var element = this;
        $(element).multipleSelect({
            name: element.id,
            filter: true,
            single: false,
            placeholder: 'Seleccione',
            onClose: function () {
                var e = document.getElementById(this.name);
                UpdateFilters(e.id);
                dt.ajax.reload();
            },
            onCheckAll: function () {
                var e = document.getElementById(this.name);
                $(e).val(null);
            }
        });
    });

    $('#id-fuente').multipleSelect({
        single: true
    });

    $('#id-byid').on('change', function () {
        if ($(this).val()) {
            $('[class="f-select"]').each(function () {
                $(this).multipleSelect('disable');
            });
        }
        else {
            $('[class="f-select"]').each(function () {
                $(this).multipleSelect('enable');
            });
        }
        dt.ajax.reload();
    });

    dt = $('#dt').DataTable({
        serverSide: true,
        ajax: {
            type: "POST",
            url: controller + 'ReprocesarList',
            contentType: 'application/json; charset=utf-8',
            data: function (d) {
                d.dataFiltros = GetFiltersValues();
                return JSON.stringify(d);
            },
            datatype: 'json',
            traditional: true
        },
        columns: [
            { title: 'Id', data: 'Ptomedicodi' },
            { title: 'Punto de medición', data: null },
            { title: 'Ubicación', data: 'Areacodi' },
            { title: 'Empresa', data: 'Emprnomb' }
        ],
        columnDefs: [
            {
                //Descripciones compuestas
                targets: '_all',
                createdCell: function (td, cellData, rowData, row, col) {
                    var str;
                    switch (col) {
                        case 1:
                            //Punto de medición
                            str = rowData.Ptomedidesc
                            $(td).html(str);
                            break;
                        case 2:
                            //Ubicación
                            str = rowData.Tareaabrev + ' ' + rowData.Areanomb;
                            $(td).html(str);
                            break;
                    }
                }
            }
        ],
        initComplete: function () {
            $('#dt').css('width', '100%');
        },
        drawCallback: function () {
            $('#dt').css('width', '100%');
        },
        searching: false,
        bLengthChange: false,
        bSort: false,
        destroy: true,
        paging: true,
        pageLength: 20,
        info: false,
    });

    $('#btn-procesar').on('click', function () {
        SetMessage('#message-popup',
            'Dependiendo de la cantidad de puntos de medición seleccionados el proceso puede tardar varios minutos...',
            'warning', false);
        Ejecutar();
    });
});

function Ejecutar() {
    $.ajax({
        type: 'POST',
        url: controller + 'ReprocesarEjecutar',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            dataFiltros: GetFiltersValues()
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {
            SetMessage('#message', result.dataMsg, result.typeMsg, true);
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

//Actualiza los filtros de busqueda
function UpdateFilters(id) {
    $.ajax({
        type: 'POST',
        url: controller + 'ReprocesarUpdateFiltros',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            dataFiltros: GetFiltersValues()
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {
            switch (id) {
                case 'id-ubicacion':
                    //Actualiza la lista de puntos de medición
                    RefillDropDowList($('#id-punto'), result.ListPtomedicion, 'Ptomedicodi', 'Ptomedidesc');
                    break;
                case 'id-tpempresa':
                    //Actualiza la lista de empresas
                    RefillDropDowList($('#id-empresa'), result.ListEmpresa, 'Emprcodi', 'Emprnomb');
                    //Actualiza la lista de puntos de medición
                    RefillDropDowList($('#id-punto'), result.ListPtomedicion, 'Ptomedicodi', 'Ptomedidesc');
                    break;
                case 'id-empresa':
                    //Actualiza la lista de puntos de medición
                    RefillDropDowList($('#id-punto'), result.ListPtomedicion, 'Ptomedicodi', 'Ptomedidesc');
                    break;
            }
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

//Setea el modelo de envio de información
function GetFiltersValues() {
    var model = {};
    model['Fecha'] = $('#id-fecha').val();
    model['SelById'] = $('#id-byid').val();
    model['SelTipoDemanda'] = $('#id-tpdemanda').val();
    model['SelListTipoEmpresa'] = $('#id-tpempresa').val();
    model['SelUbicacion'] = $('#id-ubicacion').val();
    model['SelEmpresa'] = $('#id-empresa').val();
    model['SelPuntos'] = $('#id-punto').val();
    model['SelFuente'] = $('#id-fuente').val();
    return model;
}