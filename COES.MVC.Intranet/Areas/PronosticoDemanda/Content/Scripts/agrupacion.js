var imageRoot = siteRoot + "Content/Images/";

$(document).ready(function () {
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
                UpdateFilters(e.id);
                dt.ajax.reload();
            },
            onCheckAll: function () {
                var e = document.getElementById(this.name);
                $(e).val(null);
            }
        });
    });
    
    $('.f-pop-select').each(function () {
        var element = this;
        var plc = $(this).data('plc');
        $(element).multipleSelect({
            name: element.id,
            filter: true,
            single: false,
            placeholder: plc
        });
    });
    
    $('#id-pronostico').multipleSelect({
        filter: false,
        single: true,
        onClose: function () {
            dt.ajax.reload();
        }
    });

    dt = $('#dt').DataTable({
        serverSide: true,
        ajax: {
            type: "POST",
            url: controller + 'AgrupacionesList',
            contentType: 'application/json; charset=utf-8',
            data: function (d) {
                d.dataFiltros = GetFiltersValues();
                return JSON.stringify(d);
            },
            datatype: 'json',
            traditional: true
        },
        columns: [
            { title: 'Agrupación', data: 'Ptomedidesc' },
            { title: 'Detalle', data: null },
            { title: 'Editar', data: null },
            { title: 'Eliminar', data: null },
        ],
        columnDefs: [
            {
                //Formato de la lista de puntos demedición que conforman la agrupación
                targets: [1],
                createdCell: function (td, cellData, rowData, row, col) {
                    tb = $('<table></table>');
                    $.each(rowData.ListPtomedidesc, function (i, item) {
                        r = $('<tr></tr>');
                        r.html(item.name + ' (' + item.id + ')');
                        tb.append(r);
                    });

                    $(td).append(tb);
                    $(td).css('font-weight', 'bold');
                    td.firstChild.remove();
                }
            },
            {
                //Boton eliminar
                targets: -1, width: '50px',
                defaultContent: '<img src="' + imageRoot + 'btn-cancel.png" class="btn-eliminar"/>'
            },
            {
                //Boton editar
                targets: -2, width: '50px',
                defaultContent: '<img src="' + imageRoot + 'btn-edit.png" class="btn-editar"/>'
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
        pageLength: 15,
        info: false,
    });

    dt_seleccionados = $('#tb-seleccionados').DataTable({
        data: [],
        columns: [
            { title: 'Punto de medición', data: 'Ptomedidesc' },
            { title: 'Empresa', data: 'Emprnomb' },
            { title: '', data: null }
        ],
        columnDefs: [
            {
                //Botón de eliminación
                targets: -1, width: '20px',
                defaultContent: '<img src="' + imageRoot + 'btn-cancel.png" class="btn-eliminar"/>'
            }
        ],
        searching: false,
        bLengthChange: false,
        bSort: false,
        destroy: true,
        paging: true,
        pageLength: 5,
        info: false
    });

    dt_disponibles = $('#tb-disponibles').DataTable({
        data: [],
        columns: [
            { title: 'Punto de medición', data: 'Ptomedidesc' },
            { title: 'Empresa', data: 'Emprnomb' },
            { title: '', data: null }
        ],
        columnDefs: [
            {
                //Botón de selección
                targets: -1, width: '20px',
                defaultContent: '<img src="' + imageRoot + 'btn-add.png" class="btn-agregar"/>'
            }
        ],
        searching: true,
        bLengthChange: false,
        bSort: false,
        destroy: true,
        paging: true,
        pageLength: 5,
        info: false
    });

    $('#btn-pop-guardar').on('click', function () {
        id_pto = $(this).data('s_id');
        id_grp = $(this).data('s_gr');
        id_area = $('#pop-area').val();
        s_prodem = $('#pop-pronostico').val();
        s_name = $('#pop-nombre').val();

        var sel_ids = [];
        $.each(dt_seleccionados.data(), function (i, item) {
            sel_ids.push(item.Ptomedicodi);
        });

        //Validaciones
        if (sel_ids.length == 0) {
            SetMessage('#pop-mensaje',
                'Es necesario seleccionar al menos un punto de medición',
                'error', true);
            return false;
        }
        if (!s_name) {
            SetMessage('#pop-mensaje',
                'Es necesario ingresar un nombre para la agrupación',
                'error', true);
            return false;
        }

        Save(sel_ids, id_pto, id_area, s_prodem, s_name, id_grp);
    });

    $('#btn-nuevo').on('click', function () {
        $.ajax({
            type: 'POST',
            url: controller + 'AgrupacionesData',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify({
                idPunto: -1
            }),
            datatype: 'json',
            traditional: true,
            success: function (result) {
                //Set id
                $('#btn-pop-guardar').data('s_id', -1);
                $('#btn-pop-guardar').data('s_gr', -1);

                //Set popup title
                $('#pop-title').html('Nueva Agrupación');

                //Display popup
                pop = $('#popup').bPopup({
                    easing: 'easeOutBack',
                    speed: 350,
                    transition: 'fadeIn',
                    modalClose: false,
                    onOpen: function () {
                        SetMessage('#pop-mensaje',
                            '• Se validara si el nombre a registrar se encuetra asignado a otra agrupación.<br>' +
                            '• Puede usar el filtro "Buscar" para resumir la información por punto o empresa.<br>' +
                            '• Para guardar los cambios presione el boton "Aceptar".',
                            'info');

                        dt_seleccionados.rows.add(result.selPuntos).draw();
                        dt_disponibles.rows.add(result.disPuntos).draw();
                    },
                    onClose: function () {
                        //Clean
                        $('#pop-nombre').val('');

                        dt_seleccionados.clear().draw();
                        dt_disponibles.clear().draw();
                    }
                });

            },
            error: function () {
                alert("Ha ocurrido un problema...");
            }
        });
    });
});

//Evento 'click' del boton "Eliminar" de la tabla puntos de medición
$(document).on('click', '#dt tr td img.btn-eliminar', function () {
    row = $(this).closest('tr');
    r = dt.row(row).data();
    s_id = r.Ptomedicodi;

    $.ajax({
        type: 'POST',
        url: controller + 'AgrupacionesDelete',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            idPunto: s_id
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {
            SetMessage('#message', result.dataMsg, result.typeMsg, true);
            return false;
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
});

//Evento 'click' del boton "Editar" de la tabla puntos de medición
$(document).on('click', '#dt tr td img.btn-editar', function () {
    row = $(this).closest('tr');
    r = dt.row(row).data();
    s_id = r.Ptomedicodi;
    s_gr = r.Ptogrpcodi;
    s_name = r.Ptomedidesc;

    $.ajax({
        type: 'POST',
        url: controller + 'AgrupacionesData',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            idPunto: s_id
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {
            //Set id
            $('#btn-pop-guardar').data('s_id', s_id);
            $('#btn-pop-guardar').data('s_gr', s_gr);

            //Set popup title
            $('#pop-title').html('Editar: ' + s_name);

            //Display popup
            pop = $('#popup').bPopup({
                easing: 'easeOutBack',
                speed: 350,
                transition: 'fadeIn',
                modalClose: false,
                onOpen: function () {
                    SetMessage('#pop-mensaje',
                        '• Se validara si el nombre a registrar se encuetra asignado a otra agrupación.<br>' +
                        '• Puede usar el filtro "Buscar" para resumir la información por punto o empresa.<br>' +
                        '• Para guardar los cambios presione el boton "Aceptar".',
                        'info');

                    //Set "Agrupación" data
                    $('#pop-nombre').val(result.entName);
                    $('#pop-area').val(result.selArea);

                    dt_seleccionados.rows.add(result.selPuntos).draw();
                    dt_disponibles.rows.add(result.disPuntos).draw();
                },
                onClose: function () {
                    //Clean
                    $('#pop-nombre').val('');
                    dt_seleccionados.clear().draw();
                    dt_disponibles.clear().draw();
                }
            });

        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
});

//Evento 'click' del boton "Agregar" de la tabla disponibles
$(document).on('click', '#tb-disponibles tr td img.btn-agregar', function () {
    var tr = $(this).closest('tr');
    var row = dt_disponibles.row(tr);
    var row_data = row.data();
    //Elimina la fila de la tabla de puntos
    row.remove().draw();

    //Agrega el elemento a la tabla de seleccionados
    dt_seleccionados.rows.add([row_data]).draw();
});

//Evento 'click' del boton "Eliminar" de la tabla seleccionados
$(document).on('click', '#tb-seleccionados tr td img.btn-eliminar', function () {
    var tr = $(this).closest('tr');
    var row = dt_seleccionados.row(tr);
    var row_data = row.data();
    //Elimina la fila de la tabla de puntos
    row.remove().draw();

    //Devuelve el elemento seleccionados a la tabla disponibles
    dt_disponibles.rows.add([row_data]).draw();
});

//Setea el modelo de envio de información
function GetFiltersValues() {
    var model = {};
    model['SelById'] = $('#id-byid').val();
    model['SelArea'] = $('#id-area').val();
    model['SelEmpresa'] = $('#id-empresa').val();
    model['SelAgrupacion'] = $('#id-agrupacion').val();
    model['EsPronostico'] = $('#id-pronostico').val();
    return model;
}

//Llena el contenido de una lista desplegable
function RefillDropDowList(element, data, data_id, data_name) {
    //Vacia el elemento
    element.empty();
    //Carga el elemento
    $.each(data, function (i, item) {
        var n_value = i, n_html = item;
        if (data_id) n_value = item[data_id];
        if (data_name) n_html = item[data_name];
        element.append($('<option></option>').val(n_value).html(n_html));
    });
    //Actualiza la lib.multipleselect
    element.multipleSelect('refresh');
}

function Save(sel_ptos, id_pto, id_area, s_prodem, s_name, s_gr) {
    $.ajax({
        type: 'POST',
        url: controller + 'AgrupacionesSave',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            selPuntos: sel_ptos,
            idPunto: id_pto,
            idArea: id_area,
            esPronostico: s_prodem,
            nomAgrupacion: s_name,
            idAgrupacion: s_gr
        }),
        datatype: 'json',
        success: function (result) {
            SetMessage('#message', result.typeMsg, result.dataMsg, false);
            $("#popup").bPopup().close();
            dt.ajax.reload()
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

//Actualiza los filtros segun criterio
function UpdateFilters(id) {
    $.ajax({
        type: 'POST',
        url: controller + 'AgrupacionesUpdateFiltros',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            dataFiltros: GetFiltersValues()
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {
            switch (id) {
                case 'id-area':
                    RefillDropDowList($('#id-agrupacion'), result.ListAgrupacion, 'Ptomedicodi', 'Ptomedidesc');
                    break;
                case 'id-empresa':
                    RefillDropDowList($('#id-agrupacion'), result.ListAgrupacion, 'Ptomedicodi', 'Ptomedidesc');
                    break;
            }
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}