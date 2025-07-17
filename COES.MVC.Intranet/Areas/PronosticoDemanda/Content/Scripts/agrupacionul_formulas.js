var imageRoot = siteRoot + "Content/Images/";

var dt_seleccionados, dt_disponibles;


$(document).ready(function () {

    $('#btn-popguardar').on('click', function () {
        const data = dt_seleccionados.rows()
                        .data().toArray();
        Save(data);
    });

    dt_seleccionados = $('#tb-seleccionados').DataTable({
        data: [],
        columns: [
            { title: '', data: null },
            { title: 'Id', data: 'Prrucodi', visible: false },
            { title: 'Formula', data: 'Prruabrev' },
            {title: 'Tomar Manual', data: null},
            { title: 'Prioridad', data: null },
            { title: '', data: null }
        ],
        columnDefs: [
            {
                targets: 0,
                width: '20px',
                createdCell: function (td, cellData, rowData, row, col) {
                    const str = `<img src="${imageRoot}darrow.png"/>`
                    $(td).html(str);
                    $(td).addClass('btn-colapsar');
                    $(td).css('background', '#2980B9');
                }
            },
            {
                targets: 3, width: '20px',
                createdCell: function (td, cellData, rowData, row, col) {
                    if (rowData.Prnafmflagesmanual == 'S') {
                        str = '<input ' +
                            'type="checkbox" ' +
                            'class="chk-proceso" ' +
                            'checked' +
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
                targets: 4, width: '20px',
                createdCell: function (td, cellData, rowData, row, col) {
                    str = $('<input type="number" class="ipt-prioridad" onkeydown="return false"/>');
                    str.css('width', '35px');
                    str.prop('min', 1);
                    str.data('prv', rowData.Prnafmprioridad);
                    str.val(rowData.Prnafmprioridad);
                    $(td).append(str);
                    td.firstChild.remove();
                }
            },
            {
                targets: 5, width: '20px',
                defaultContent: '<img src="' + imageRoot + 'btn-cancel.png" class="btn-eliminar"/>'
            }
        ],
        createdRow: function (row, data) {
            $(row).find('.btn-colapsar')
                .on('click', function () {
                    const fila = dt_seleccionados.row(row);
                    //Oculta la fila desplegable
                    if (fila.child.isShown()) {
                        fila.child.hide();
                        $(row).find('.btn-colapsar img').removeClass('rotate180');
                    }
                    //Muestra la fila desplegable
                    else {
                        //Crea los identificadores y elementos base de la fila desplegable
                        const idHt = `ht_${data.Prrucodi}`;
                        const elemHt = `<div id="${idHt}"></div>`;

                        //Wrapper de la fila desplegable para el scroll horizontal
                        let elemHtml = document.createElement('div');
                        elemHtml.style.width = '545px';
                        elemHtml.style.overflowX = 'overlay';
                        elemHtml.style.height = '75px';
                        elemHtml.innerHTML = elemHt;

                        fila.child(elemHtml).show();
                        crearHtColapsable(idHt, data.ArrayDatos, data.Prrucodi);
                        $(row).find('.btn-colapsar img').addClass('rotate180');
                    }
                });

            $(row).find('.btn-eliminar')
                .on('click', function () {
                    const fila = dt_seleccionados.row(row);
                    fila.remove().draw();
                    dt_disponibles.rows.add([{
                        Prrucodi: data.Prrucodi,
                        Prruabrev: data.Prruabrev
                    }]).draw();
                })
        },
        searching: false,
        bLengthChange: false,
        bSort: false,
        destroy: true,
        info: false
    });

    dt_disponibles = $('#tb-disponibles').DataTable({
        data: [],
        columns: [
            { title: 'Id', data: 'Prrucodi', visible: false },
            { title: 'Formulas', data: 'Prruabrev' },
            { title: '', data: null }
        ],
        columnDefs: [
            {
                //Botón de selección
                targets: 2, width: '20px',
                defaultContent: '<img src="' + imageRoot + 'btn-add.png" class="btn-agregar" />'
            }
        ],
        searching: true,
        bLengthChange: false,
        bSort: false,
        destroy: true,
        paging: true,
        pageLength: 10,
        info: false
    });

    dttabla = $('#dttabla').DataTable({
        serverSide: true,
        ajax: {
            type: "POST",
            url: controller + 'AgrupacionUsuariosLibresList',
            contentType: 'application/json; charset=utf-8',
            data: function (d) {
                return JSON.stringify(d);
            },
            datatype: 'json',
            traditional: true
        },
        columns: [
            { data: "id", title: "ID", visible: false },
            { data: "nombre", title: "NOMBRE" },
            { data: null, title: '' }
        ],
        columnDefs: [
            {
                //Botones
                targets: -1,
                defaultContent: '<input type="button" value="Relacionar Agrupación - Fórmula"/>'
            }
        ],
        initComplete: function () {
            $('#dttabla').css('width', '100%');
        },
        drawCallback: function () {
            $('#dttabla').css('width', '100%');
        },
        searching: false,
        bLengthChange: false,
        bSort: false,
        destroy: true,
        paging: true,
        pageLength: 10,
        info: false
    });
});

//Evento 'click' del boton "Editar" de la tabla principal
$(document).on('click', '#dttabla tr td input[type="button"]', function () {

    var row = $(this).closest('tr');
    var r = dttabla.row(row).data();

    pop = $('#popup').bPopup({
        easing: 'easeOutBack',
        speed: 350,
        transition: 'fadeIn',
        modalClose: false,
        onOpen: function () {
            document.getElementById("id-agrupacion").value = r.id;
        },
        onClose: function () {
            dt_seleccionados.clear().draw();
            dt_disponibles.clear().draw();
        }
    }, function () {
        $.ajax({
            type: 'POST',
            url: controller + "ListFormulasByAgrupacionPopUp",
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify({
                agrupacion: r.id
            }),
            datatype: 'json',
            traditional: true,
            success: function (result) {
                //Set DataTables 
                dt_seleccionados.rows.add(result.DtSeleccionados).draw();
                dt_disponibles.rows.add(result.DtDisponibles).draw();
            },
            error: function () {
                alert("Ha ocurrido un problema...");
            }
        });
    });
});

//Evento 'change' del input "Prioridad" de la tabla seleccionados
$(document).on('change', '#tb-seleccionados tr td .ipt-prioridad', function () {
    elem = this;
    tr = $(this).closest('tr');
    row = dt_seleccionados.row(tr);

    //Obtiene el índice del elemento editado en el datatable
    elem_idx = row.index();

    new_val = $(this).val();//Valor ingresado
    old_val = $(this).data('prv');//Valor antes del evento "onChange"

    cls_ipt = $('.ipt-prioridad');
    $.each(cls_ipt, function (i, item) {
        if ($(item).data('prv') == new_val) {
            $(item).val(old_val);
            $(item).data('prv', old_val);
            $(elem).data('prv', new_val);

            //Obtiene el índice del elemento actualizado
            tr = $(item).closest('tr');
            row = dt_seleccionados.row(tr);
            item_idx = row.index();

            //Actualiza el source del datatable para ambos elementos
            dt_seleccionados.data()[elem_idx].Prnafmprioridad = parseInt(new_val);
            dt_seleccionados.data()[item_idx].Prnafmprioridad = parseInt(old_val);
            return false;
        }
    });
});

//Evento 'click' del boton "Agregar" de la tabla de disponibles
$(document).on('click', '#tb-disponibles tr td img.btn-agregar', function () {
    var tr = $(this).closest('tr');
    var row = dt_disponibles.row(tr);
    var row_data = row.data();
    //Elimina la fila de la tabla de puntos
    row.remove().draw();

    //Calcula la prioridad para el pase
    prd = dt_seleccionados.data().length + 1;

    //Agrega la fila a la tabla de seleccionados
    dt_seleccionados.rows.add([{
        Prrucodi: row_data.Prrucodi,
        Prruabrev: row_data.Prruabrev,
        Prnafmprioridad: prd,
        ArrayDatos: new Array(48).fill(0),
        
    }]).draw();
    //Setea el nuevo máximo seleccionable para la prioridad
    cls_prd = $('#tb-seleccionados tr td input[type="number"]');
    $.each(cls_prd, function (i, item) {
        $(item).prop('max', prd);
    });
});

//Evento 'click' del boton "Eliminar" de la tabla seleccionados
$(document).on('click', '#tb-seleccionados tr td img.btn-eliminar', function () {
    var tr = $(this).closest('tr');
    var row = dt_seleccionados.row(tr);
    var row_data = row.data();
    //Elimina la fila de la tabla de seleccionados
    row.remove().draw();

    //Agrega la fila a la tabla de flujos disponibles
    dt_disponibles.rows.add([{
        Prrucodi: row_data.Prrucodi,
        Prruabrev: row_data.Prruabrev
    }]).draw();
});

function Save(d) {
    $.ajax({
        type: 'POST',
        url: controller + 'SaveAgrupacionULFormulas',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            idAgrupacion: $('#id-agrupacion').val(),
            listFormulas: d
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {
            pop.close();
            SetMessage('#message', result.dataMsg, result.typeMsg, true);
            dttabla.ajax.reload();
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

//Evento 'change' del input "Prioridad" de la tabla seleccionados
$(document).on('change', '#tb-seleccionados tr td .chk-proceso', function () {
    elem = this;
    tr = $(this).closest('tr');
    row = dt_seleccionados.row(tr);
    //Obtiene el índice del elemento editado en el datatable
    elem_idx = row.index();
    let estado;
    if ($(this).is(':checked')) {
        estado = 'S';
    } else {
        estado = 'N';
    }

    dt_seleccionados.data()[elem_idx].Prnafmflagesmanual = estado;
});

function crearHtColapsable(idContenedor, dataMed, dtRowCodi) {
    const dataHt = [];
    dataHt.push(dataMed);
    const contenedor = document.getElementById(idContenedor);
    window[`obj_${idContenedor}`] = new Handsontable(contenedor, {
        data: dataHt,
        fillHandle: true,
        height: '75',
        width: '2400',
        maxCols: 48,
        maxRows: 1,
        minSpareCols: 0,
        minSpareRows: 0,
        colHeaders: generarCabecera(),
        columns: generarColumnas(),
        afterChange: function (changes, source) {
            if (ValidarEventosHandsontable(source)) {
                for (var i = 0; i < changes.length; i++) {
                    const htColIndex = changes[i][1];
                    const htChangeValue = changes[i][3];

                    let dtRowData = dt_seleccionados
                        .rows()
                        .data()
                        .filter(x => x.Prrucodi == dtRowCodi);
                    dtRowData[0].ArrayDatos[htColIndex] = htChangeValue;
                }
            }
        },
    });
}

function generarColumnas() {
    let columnas = [];
    let i = 0;
    while (i < 48) {
        let e = {
            data: i,
            type: 'numeric',
            format: '0.00',
            readOnly: false,
            allowInvalid: false,
            allowEmpty: false
        };
        columnas.push(e);
        i++;
    }
    return columnas;
}

function generarCabecera() {
    let cabecera = [];
    let i = 0;
    let baseTime = new Date();
    baseTime.setHours(0, 0, 0, 0);
    while (i < 48) {
        const minutes = 30 * (i + 1);
        let t = new Date(baseTime);
        t.setMinutes(baseTime.getMinutes() + minutes);
        const hh = (t.getHours().toString().length == 1)
            ? `0${t.getHours()}` : t.getHours().toString();
        const mm = (t.getMinutes().toString().length == 1)
            ? `0${t.getMinutes()}` : t.getMinutes().toString();
        cabecera.push(`${hh}:${mm}`);
        i++;
    }
    return cabecera;
}

function ValidarEventosHandsontable(evento) {
    let valid = false;
    const ListEventos = ['edit', 'autofill', 'paste'];

    for (var i = 0; i < ListEventos.length; i++) {
        if (evento == ListEventos[i]) valid = true;
    }
    return valid;
}