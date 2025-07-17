var name = "RelacionBarras/";
var controller = siteRoot + "PronosticoDemanda/" + name;
var imageRoot = siteRoot + "Content/Images/";

var barracodi;
var dt_seleccionados, dt_puntos, dt_agrupaciones;
var test;
$(document).ready(function () {
    //Mensaje de inicio
    SetMessage('#message', $('#main').data('msg'), $('#main').data('tpo'));

    //Aplica libreria MultipleSelect
    $('.f-select').each(function () {
        var element = this;
        $(element).multipleSelect({
            name: element.id,
            filter: true,
            single: false,
            placeholder: 'Seleccione',
            onClose: function () {
                var e = $('#id-barra').val();
                if (e) {
                    RefreshListEmpresa(e);
                }
                else {
                    ListEmpresa();
                }
                dt.ajax.reload();
            },
            onCheckAll: function () {
                var e = document.getElementById(this.name);
                if (e.checked == false) {
                    $(e).val(null);
                }
                else {
                    $(e).val($('#id-barra').val());
                }
            }
        });
    });

    $('#id-valbarra').on('click', function () {

        if (document.getElementById('id-valbarra').checked) {
            FiltroBarra(1);
        } else {
            FiltroBarra(0);
        }
 
    });

    //Aplica libreria MultipleSelect
    $('.f-selectEmpresa').each(function () {
        var element = this;
        $(element).multipleSelect({
            name: element.id,
            filter: true,
            single: false,
            placeholder: 'Seleccione',
            onClose: function () {
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
            placeholder: plc,
            onClose: function () {
                UpdatePopupDtPuntos();
            },
            onCheckAll: function () {
                var e = document.getElementById(this.name);
                $(e).val(null);
            }
        });
    });

    $('#btn-pop-guardar').on('click', function () {
        data = [];
        $.each(dt_seleccionados.rows().data(), function (i, item) {
            obj = {};
            obj["Ptomedicodi"] = item.id;
            obj["Prnpmpvarexoproceso"] = item.proceso;
            data.push(obj);
        });
        
        SaveRelacionBarra(data);
        
    });

    dt = $('#dt').DataTable({
        serverSide: true,
        ajax: {
            type: "POST",
            url: controller + 'List',
            contentType: 'application/json; charset=utf-8',
            data: function (d) {
                d.dataFiltros = GetFiltersValues();
                return JSON.stringify(d);
            },
            datatype: 'json',
            traditional: true
        },
        columns: [
            { title: 'Área operativa', data: 'area' },
            { title: 'Barra CP', data: 'cp' },
            { title: 'Barra PM', data: 'pm' },
            { title: 'Puntos de medición o Agrupaciones relacionadas', data: null },
            { title: '', data: null }
        ],
        columnDefs: [
            {
                //Formato de la lista de puntos demedición que conforman la agrupación
                targets: [3],
                createdCell: function (td, cellData, rowData, row, col) {
                    if (rowData.rel.length != 0) {
                        tb = $('<table></table>');
                        $.each(rowData.rel, function (i, item) {
                            r = $('<tr></tr>');
                            if (item.substring(0, 1) == "P") {
                                r.html('• ' + item.substring(1).fontcolor("3352FF"));
                            }
                            else {
                                r.html('• ' + item.substring(1).fontcolor("FF3C33"));
                            }
                            tb.append(r);
                        });

                        $(td).append(tb);
                        $(td).css('font-weight', 'bold');
                        td.firstChild.remove();
                    }
                    else {
                        $(td).html('No existen puntos o agrupaciones relacionadas');
                        $(td).css('font-weight', 'bold');
                    }
                }
            },
            {
                //Descripciones
                targets: '_all',
                createdCell: function (td, cellData, rowData, row, col) {
                    var str;
                    switch (col) {
                        case 0:
                            //Área operativa
                            str = rowData.area;
                            $(td).html(str);
                            $(td).css('font-weight', 'bold');
                            break;
                    }
                }
            },
            {
                //Botones
                targets: -1,
                defaultContent: '<input type="button" value="Editar"/>'
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
        pageLength: 10,
        info: false
    });

    dt_seleccionados = $('#tb-seleccionados').DataTable({
        data: [],
        columns: [
            { title: '', data: null },
            { title: 'Ubicación', data: 'ubicacion' },
            { title: 'Empresa', data: 'empresa' },
            { title: 'Nombre', data: 'nombre' },
            { title: 'Exógena', data: null },
            { title: '', data: null }
        ],
        columnDefs: [
            {
                //Botón de tipo
                targets: 0, width: '20px',
                createdCell: function (td, cellData, rowData, row, col) {
                    if (rowData.source == 'punto') {
                        str = '<img src="' + imageRoot + 'darrow.png" />'
                        $(td).html(str);
                        $(td).css('background', '#C6C6C6');
                    }
                    else {
                        str = '<img src="' + imageRoot + 'darrow.png" />'
                        $(td).html(str);
                        $(td).addClass('btn-detail');
                        $(td).css('background', '#2980B9');
                    }
                }
            },
            {
                targets: 4, width: '20px',
                createdCell: function (td, cellData, rowData, row, col) {
                    if (rowData.proceso == 'S') {
                        str = '<input ' +
                            'type="checkbox" ' +
                            'class="chk-proceso" ' +
                            'checked'+
                            ' />';
                        $(td).html(str);
                    }
                    else {
                        str = '<input ' +
                            'type="checkbox" ' +
                            'class="chk-proceso" ' +
                            ' />';
                        $(td).html(str);
                    }
                }
            },
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
        info: false
    });

    dt_puntos = $('#tb-puntos').DataTable({
        data: [],
        columns: [
            { title: '', data: null },
            { title: 'Ubicación', data: 'ubicacion' },
            { title: 'Empresa', data: 'empresa' },
            { title: 'Nombre', data: 'nombre' },
            { title: '', data: null }
        ],
        columnDefs: [
            {
                //Botón de tipo
                targets: 0, width: '20px',
                createdCell: function (td, cellData, rowData, row, col) {
                    str = '<img src="' + imageRoot + 'darrow.png" />'
                    $(td).html(str);
                    $(td).css('background', '#C6C6C6');
                }
            },
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

    dt_agrupaciones = $('#tb-agrupaciones').DataTable({
        data: [],
        columns: [
            { title: '', data: null },
            { title: 'Ubicación', data: 'ubicacion' },
            { title: 'Empresa', data: 'empresa' },
            { title: 'Nombre', data: 'nombre' },
            { title: '', data: null }
        ],
        columnDefs: [
            {
                //Botón de tipo
                targets: 0, width: '20px',
                createdCell: function (td, cellData, rowData, row, col) {
                    str = '<img src="' + imageRoot + 'darrow.png" />'
                    $(td).html(str);
                    $(td).addClass('btn-detail');
                    $(td).css('background', '#2980B9');
                }
            },
            {
                //Lista de Empresas
                targets: [2],
                createdCell: function (td, cellData, rowData, row, col) {
                    var acumulado = '';
                    $.each(rowData.empresa, function (i, item) {
                        var s = '';
                        s = '<p>' + item + '</p>';
                        acumulado += s;
                    });

                    $(td).html(acumulado);
                }
            },
            {
                //Botón de selección
                targets: -1, width: '20px',
                defaultContent: '<img src="' + imageRoot + 'btn-add.png" class="btn-agregar" />'
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
});

//Evento 'click' del boton "Editar" de la tabla principal
$(document).on('click', '#dt tr td input[type="button"]', function () {

    var row = $(this).closest('tr');
    var r = dt.row(row).data();
    barracodi = r.id;

    pop = $('#popup').bPopup({
        easing: 'easeOutBack',
        speed: 350,
        transition: 'fadeIn',
        modalClose: false,
        onClose: function () {
            dt_seleccionados.clear().draw();
            dt_puntos.clear().draw();
            dt_agrupaciones.clear().draw();
        }
    }, function () {
            $.ajax({
                type: 'POST',
                url: controller + "ListDataPopup",
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify({
                    barra: r.id
                }),
                datatype: 'json',
                traditional: true,
                success: function (result) {
                    //Set Filters
                    RefillDropDowList($('#id-pto-ubicacion'), result.ListUbicaciones,  'Areacodi', 'Areanomb');
                    RefillDropDowList($('#id-pto-empresa'), result.ListEmpresas, 'Emprcodi', 'Emprnomb');

                    //Set DataTables
                    dt_seleccionados.rows.add(result.DtSeleccionados).draw();
                    dt_puntos.rows.add(result.DtPuntos).draw();
                    dt_agrupaciones.rows.add(result.DtAgrupaciones).draw();
                },
                error: function () {
                    alert("Ha ocurrido un problema...");
                }
            });
    });
});

//Evento 'click' del boton "Agregar" de la tabla puntos de medición
$(document).on('click', '#tb-puntos tr td img.btn-agregar', function () {
    var tr = $(this).closest('tr');
    var row = dt_puntos.row(tr);
    var row_data = row.data();
    //Elimina la fila de la tabla de puntos
    row.remove().draw();

    //Agrega la fila a la tabla de seleccionados
    dt_seleccionados.rows.add([{
        id: row_data.id,
        nombre: row_data.nombre,
        ubicacion: row_data.ubicacion,
        empresa: row_data.empresa,
        barracodi: row_data.barracodi,
        source: 'punto'
    }]).draw();
});

//Evento 'click' del boton "Detalle" de la tabla agrupaciones
$(document).on('click', '#tb-agrupaciones tr td.btn-detail', function () {
    var tr = $(this).closest('tr');
    var row = dt_agrupaciones.row(tr);

    if (row.child.isShown()) {
        // This row is already open - close it
        row.child.hide();
        tr.find('.btn-detail img').removeClass('rotate180');
    }
    else {
        // Open this row
        row.child(FormatDetail(row.data())).show();
        tr.find('.btn-detail img').addClass('rotate180');
    }
});

//Evento 'click' del boton "Agregar" de la tabla agrupaciones
$(document).on('click', '#tb-agrupaciones tr td img.btn-agregar', function () {
    var tr = $(this).closest('tr');
    var row = dt_agrupaciones.row(tr);
    var row_data = row.data();
    //Elimina la fila de la tabla de puntos
    row.remove().draw();

    //Agrega la fila a la tabla de seleccionados
    dt_seleccionados.rows.add([{
        id: row_data.id,
        nombre: row_data.nombre,
        ubicacion: row_data.ubicacion,
        empresa: row_data.empresa,
        barracodi: row_data.barracodi,
        puntos: row_data.puntos,
        source: 'agrupacion'
    }]).draw();
});

//Evento 'click' del boton "Detalle" de la tabla seleccionados
$(document).on('click', '#tb-seleccionados tr td.btn-detail', function () {
    var tr = $(this).closest('tr');
    var row = dt_seleccionados.row(tr);
    console.log(row.data());

    if (row.child.isShown()) {
        // This row is already open - close it
        row.child.hide();
        tr.find('.btn-detail img').removeClass('rotate180');
    }
    else {
        // Open this row
        row.child(FormatDetail(row.data())).show();
        tr.find('.btn-detail img').addClass('rotate180');
    }
});

//Evento 'click' del boton "Eliminar" de la tabla seleccionados
$(document).on('click', '#tb-seleccionados tr td img.btn-eliminar', function () {
    var tr = $(this).closest('tr');
    var row = dt_seleccionados.row(tr);
    var row_data = row.data();
    //Elimina la fila de la tabla de puntos
    row.remove().draw();

    //Devuelve el elemento seleccionados a su tabla correspondiente
    if (row_data.source == 'punto') {
        //Agrega la fila a la tabla de seleccionados
        dt_puntos.rows.add([{
            id: row_data.id,
            nombre: row_data.nombre,
            ubicacion: row_data.ubicacion,
            empresa: row_data.empresa,
            barracodi: row_data.barracodi,
            source: 'punto'
        }]).draw();
    }
    else {
        dt_agrupaciones.rows.add([{
            id: row_data.id,
            nombre: row_data.nombre,
            ubicacion: row_data.ubicacion,
            empresa: row_data.empresa,
            barracodi: row_data.barracodi,
            puntos: row_data.puntos,
            source: 'agrupacion'
        }]).draw();
    }
});

//Evento 'change' del input "Prioridad" de la tabla seleccionados
$(document).on('change', '#tb-seleccionados tr td .chk-proceso', function () {
    elem = this;
    tr = $(this).closest('tr');
    row = dt_seleccionados.row(tr);
    
    //Obtiene el índice del elemento editado en el datatable
    elem_idx = row.index();
    dt_seleccionados.data()[elem_idx].proceso = 'S';
});

////Evento 'Buscar' del filtro nombre de la tabla Puntos
//$(document).on('keyup', '#id-pto-nombre', function () {
//    if (dt_puntos.column(1).search() !== this.value) {
//        dt_puntos
//            .column(1)
//            .search(this.value)
//            .draw();
//    }
//});

////Evento 'Buscar' del filtro empresa de la tabla Puntos
//$(document).on('keyup', '#id-pto-empresa', function () {
//    if (dt_puntos.column(2).search() !== this.value) {
//        dt_puntos
//            .column(2)
//            .search(this.value)
//            .draw();
//    }
//});


//Setea el modelo de envio de información 
function GetFiltersValues() {
    var model = {};
    model['SelArea'] = $('#id-area').val();
    model['SelBarra'] = $('#id-barra').val();
    model['SelEmpresas'] = $('#id-empresa').val();
    return model;
}

//Setea el modelo de envio de información del popup
function GetPopupFiltersValues() {
    var model = {};
    model['SelUbicaciones'] = $('#id-pto-ubicacion').val();
    model['SelEmpresas'] = $('#id-pto-empresa').val();
    return model;
}

//Configura el mensaje principal
function SetMessage(container, msg, tpo, del) {//{Contenedor, mensaje(string), tipoMensaje(string), delay(bool)}
    var new_class = "msg-" + tpo;//Identifica la nueva clase css
    $(container).removeClass($(container).attr('class'));//Quita la clase css previa
    $(container).addClass(new_class);
    $(container).html(msg);//Carga el mensaje
    //$(container).show();

    //Focus to message
    $('html, body').animate({ scrollTop: 0 }, 5);

    //Mensaje con delay o no
    if (del) $(container).show(0).delay(5000).hide(0);//5 Segundos
    else $(container).show();
}

//Da el formato para el boton detalle de los datatable
function FormatDetail(d) {
    str = $('<div></div>');
    str.css('padding-left', '30px');
    str.css('text-align', 'left');

    $.each(d.puntos, function (i, item) {
        s = $('<div></div>');
        s.html('• ' + item.nombre + ' (' + item.id + ')');
        str.append(s);
    });
    return str;
}

function SaveRelacionBarra(data) {
    $.ajax({
        type: 'POST',
        url: controller + 'Save',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            entData: data,
            idBarra: barracodi
        }),
        datatype: 'json',
        success: function (result) {
            SetMessage('#message', result, 'success', false);
            $("#popup").bPopup().close();
            //dt.ajax.reload();
            if (document.getElementById('id-valbarra').checked) {
                FiltroBarra(1);
            } else {
                FiltroBarra(0);
            }

        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

//Actualiza los filtros segun criterio
function UpdatePopupDtPuntos() {
    $.ajax({
        type: 'POST',
        url: controller + 'UpdatePopupDtPuntos',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            dataFiltros: GetPopupFiltersValues()
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {
            dt_puntos.clear().draw();
            dt_puntos.rows.add(result).draw();
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
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

//14042020
//Funcion para Rfrescar ls lista de Empresas
function RefreshListEmpresa(barra) {
    $.ajax({
        type: 'POST',
        url: controller + 'RefreshListEmpresa',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            barrasel: barra
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {

            RefillDropDowList($('#id-empresa'), result, 'Emprcodi', 'Emprnomb');

        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

//Funcion para listar Empresas
function ListEmpresa() {
    $.ajax({
        type: 'POST',
        url: controller + 'ListEmpresa',
        contentType: 'application/json; charset=utf-8',
        datatype: 'json',
        traditional: true,
        success: function (result) {

            RefillDropDowList($('#id-empresa'), result, 'Emprcodi', 'Emprnomb');
            dt.ajax.reload();
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}


//Funcion para listar Empresas
function FiltroBarra(val) {
    $.ajax({
        type: 'POST',
        url: controller + 'FiltroBarra',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            flag: val
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {

            RefillDropDowList($('#id-barra'), result.ListBarraPM, 'Grupocodi', 'Gruponomb');
            RefillDropDowList($('#id-empresa'), result.ListEmpresa, 'Emprcodi', 'Emprnomb');
            dt.ajax.reload();
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
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