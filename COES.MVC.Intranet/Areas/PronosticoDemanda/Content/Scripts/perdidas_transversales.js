var pop;
var dtDisponibles, dtSeleccionados, dtBarras;
var nombPerdida;
var listFormulas = [];

$(document).ready(function () {
    obtenerDtDisponibles();
    obtenerDtSeleccionados();

    listarFormulas();
    listarPerdidasTransversales();

    // Carga popup para el evento Nuevo
    $('#btn-add').on('click', function () {
        pop = $('#popup').bPopup({
            easing: 'easeOutBack',
            speed: 350,
            transition: 'fadeIn',
            modalClose: false,
            onClose: function () {
                dtSeleccionados.clear().draw();
                dtDisponibles.clear().draw();
            }
        }, function () {
            $("#txt-nombre").val('');
            listarRelaciones(0, '');
        });
    });

    $('#btn-guardar').on('click', function () {
        registrarRelaciones();
    });
});

function listarFormulas() {
    $.ajax({
        type: 'POST',
        url: controller + 'Listarformulas',
        contentType: 'application/json; charset=utf-8',
        datatype: 'json',
        traditional: true,
        success: function (result) {
            listFormulas = result;
        },
        error: function () {
            SetMessage('#message',
                'Ha ocurrido un error al intentar cargar las barras...',
                'error');
        }
    });
}

function listarPerdidasTransversales() {
    $.ajax({
        type: 'POST',
        url: controller + 'ListarPerdidasTransversales',
        contentType: 'application/json; charset=utf-8',
        datatype: 'json',
        traditional: true,
        success: function (result) {
            obtenerDtBarras(result);
        },
        error: function () {
            SetMessage('#message',
                'Ha ocurrido un error al intentar cargar las barras...',
                'error');
        }
    });
}

function obtenerDtBarras(entidades) {
    dtBarras = $('#tb-barras').DataTable({
        data: entidades,
        columns: [
            { title: 'PERDIDA', data: 'Prdtrnetqnomb' },
            { title: 'NOMBRE-GRUPO', data: 'Gruponomb' },
            { title: 'FORMULA', data: 'Prruabrev' },
            { title: '', data: null },
            { title: '', data: null }
        ],
        columnDefs: [
            {
                // Botón de edicion
                targets: 3,
                width: '20px',
                defaultContent: `<img src="${rutaImagenes}btn-add.png" class="btn-edit"/>`
            },
            {
                // Botón de eliminacion
                targets: 4,
                width: '20px',
                defaultContent: `<img src="${rutaImagenes}btn-cancel.png" class="btn-eliminar"/>`
            }
        ],
        createdRow: function (row, data) {
            $(row).find('.btn-edit')
                .on('click', function () {

                    pop = $('#popup').bPopup({
                        easing: 'easeOutBack',
                        speed: 350,
                        transition: 'fadeIn',
                        modalClose: false,
                        onClose: function () {
                            dtSeleccionados.clear().draw();
                            dtDisponibles.clear().draw();
                        }
                    }, function () {
                        nombPerdida = data.Prdtrnetqnomb;
                        $("#txt-nombre").val(data.Prdtrnetqnomb);
                        listarRelaciones(data.Prdtrncodi, data.Prdtrnetqnomb);
                    });

                });

            $(row).find('.btn-eliminar')
                .on('click', function () {

                    nombPerdida = data.Prdtrnetqnomb;
                    EliminarRelaciones();

                });
        },
        searching: false,
        bLengthChange: false,
        bSort: false,
        destroy: true,
        paging: true,
        pageLength: 10,
        info: false
    });
}

function listarRelaciones(idPerdTransv, nomPerdTransv) {
    $.ajax({
        type: 'POST',
        url: controller + 'ListarRelacionesPerdidasTransv',
        contentType: 'application/json; charset=utf-8',
        datatype: 'json',
        data: JSON.stringify({
            idPerdTransv: idPerdTransv,
            nomPerdTransv: nomPerdTransv
        }),
        traditional: true,
        success: function (modelo) {
            // Llena la Grilla de Formulas (Abajo del Popup)
            dtDisponibles.rows.add(modelo.listBarras);
            dtDisponibles.draw();

            // Llena la Grilla de Formulas y Hs (Arriba del Popup)
            dtSeleccionados.rows.add(modelo.listaSeleccionados);
            dtSeleccionados.draw();
        },
        error: function () {
            SetMessage('#message',
                'Ha ocurrido un error al intentar ' +
                'cargar las relaciones de la barra seleccionada...',
                'error');
        }
    });
}

function obtenerDtDisponibles() {
    dtDisponibles = $('#tb-disponibles').DataTable({
        data: [],
        columns: [
            { title: 'BARRA', data: 'Gruponomb' },
            { title: '', data: null }
        ],
        columnDefs: [
            {
                // Botón de selección
                targets: -1,
                width: '20px',
                defaultContent: `<img src="${rutaImagenes}btn-add.png" class="btn-agregar"/>`
            }
        ],
        createdRow: function (row, data) {
            $(row).find('.btn-agregar')
                .on('click', function () {
                    const fila = dtDisponibles.row(row);
                    fila.remove().draw();

                    // Añade un registro vacio al detalle
                    dtSeleccionados.rows.add([{
                        Grupocodi: data.Grupocodi,
                        Gruponomb: data.Gruponomb,
                        Prrucodi: data.Prrucodi,
                        Prruabrev: data.Prruabrev,
                        TomarManual: false,
                        ArrayMediciones: new Array(48).fill(0)
                    }]).draw();

                });
        },
        searching: true ,
        bLengthChange: false,
        bSort: false,
        destroy: true,
        paging: true,
        pageLength: 10,
        info: false
    });
}

function obtenerDtSeleccionados(entidades) {
    dtSeleccionados = $('#tb-seleccionados').DataTable({
        data: entidades,
        columns: [
            { title: '', data: null }, // boton para func.colapsable
            { title: 'BARRA', data: 'Gruponomb' }, // Columna tipo combo
            { title: 'FÓRMULA', data: 'Prruabrev' },
            { title: 'TOMAR MANUAL', data: null }, // Columna tipo checkbox
            { title: '', data: null } // boton para func.eliminar
        ],
        columnDefs: [
            {
                // Botón de tipo colapsable
                targets: 0,
                width: '20px',
                createdCell: function (td, cellData, rowData, row, col) {
                    const str = `<img src="${rutaImagenes}darrow.png"/>`
                    $(td).html(str);
                    $(td).addClass('btn-colapsar');
                    $(td).css('background', '#2980B9');
                }
            },
            {
                // Columna de tipo combo
                targets: 2,
                width: '200px',
                createdCell: function (td, cellData, rowData, row, col) {
                    var str = $('<select class="cbo-formula">');
                    str.attr({ style: 'width : 200px;' })
                    str.append('<option value="-1">-- Seleccione --</option>')
                    $.each(listFormulas, function (i, item) {
                        str.append('<option value =' + item.Prrucodi + '>' + item.Prruabrev + '</option>');
                    });
                    str.append('</select>')
                    $(td).html(str);

                    return str;
                }
            },

            {
                // Columna de tipo Checkbox
                targets: 3,
                width: '20px',
                createdCell: function (td, cellData, rowData, row, col) {
                    const str = (rowData.Prdtrnflagesmanual === false)
                        ? '<input type="checkbox" class="chk-manual"/>'
                        : '<input type="checkbox" class="chk-manual" checked/>';
                    $(td).html(str);
                    return str;
                }
            },
            {
                // Botón de tipo imagen eliminar
                targets: -1,
                width: '20px',
                defaultContent: `<img src="${rutaImagenes}btn-cancel.png" class="btn-eliminar"/>`
            }
        ],
        createdRow: function (row, data) {
            $(row).find('.btn-colapsar')
                .on('click', function () {
                    const fila = dtSeleccionados.row(row);

                    // Oculta la fila desplegable
                    if (fila.child.isShown()) {
                        fila.child.hide();
                        $(row).find('.btn-colapsar img').removeClass('rotate180');
                    }

                    // Muestra la fila desplegable
                    else {
                        // Crea los identificadores y elementos base de la fila desplegable
                        const idHt = `ht_${data.Grupocodi}`;
                        const elemHt = `<div id="${idHt}"></div>`;

                        // Wrapper de la fila desplegable para el scroll horizontal
                        let elemHtml = document.createElement('div');
                        elemHtml.style.width = '545px';
                        elemHtml.style.overflowX = 'overlay';
                        elemHtml.style.height = '75px';
                        elemHtml.innerHTML = elemHt;

                        fila.child(elemHtml).show();
                        crearHtColapsable(idHt, data.ArrayMediciones, data.Grupocodi);
                        $(row).find('.btn-colapsar img').addClass('rotate180');
                    }
                });

            $(row).find('.cbo-formula') // Setea la propiedad de la lista con id del combo 
                .on('change', function () {
                    const cboFormula = $(this).val();
                    data.Prrucodi = cboFormula;
                });

            $(row).find('.btn-eliminar')
                .on('click', function () {
                    const fila = dtSeleccionados.row(row);
                    fila.remove().draw();
                    dtDisponibles.rows.add([{
                        Grupocodi: data.Grupocodi,
                        Gruponomb: data.Gruponomb,
                        Prrucodi: data.Prrucodi,
                        Prruabrev: data.Prruabrev,

                        Prdtrnflagesmanual: data.Prdtrnflagesmanual
                    }]).draw();

                });

            $(row).find('.chk-manual')
                .on('click', function () {
                    if (data.Prdtrnflagesmanual === false)
                        data.Prdtrnflagesmanual = true;
                    else
                        data.Prdtrnflagesmanual = false;
                });

            $(row).find('.cbo-formula') // Captura el id del combo para el evento editar
                .val(data.Prrucodi)
        },
        searching: false,
        bLengthChange: false,
        bSort: false,
        destroy: true,
        paging: true,
        pageLength: 10,
        info: false
    });
}

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

                    let dtRowData = dtSeleccionados
                        .rows()
                        .data()
                        .filter(x => x.Grupocodi == dtRowCodi);
                    dtRowData[0].ArrayMediciones[htColIndex] = htChangeValue;
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

function registrarRelaciones() {
    var strnombPerdida = $("#txt-nombre").val()

    const datosSeleccionados = dtSeleccionados
        .rows()
        .data()
        .toArray();

    if (datosSeleccionados.length == 0) {
        SetMessage('#pop-mensaje',
                'No se puede registrar una relación vacia...',
                'warning',true);
        return false;
    }

    $.ajax({
        type: 'POST',
        url: controller + 'RegistrarRelacionPerdidaTransv',
        contentType: 'application/json; charset=utf-8',
        datatype: 'json',
        data: JSON.stringify({
            nombPerdida: strnombPerdida,
            listaSeleccionados: datosSeleccionados
        }),
        traditional: true,
        success: function (result) {
            listarPerdidasTransversales();
            SetMessage('#message', result.dataMsg, result.typeMsg);
        },
        error: function () {
            SetMessage('#message',
                'Ha ocurrido un error al intentar cargar las barras...',
                'error');
        },
        complete: function () {
            pop.close();
        }
    });
}

function EliminarRelaciones() {

    $.ajax({
        type: 'POST',
        url: controller + 'EliminarRelacionPerdidaTransv',
        contentType: 'application/json; charset=utf-8',
        datatype: 'json',
        data: JSON.stringify({
            nombPerdida: nombPerdida
        }),
        traditional: true,
        success: function (result) {
            listarPerdidasTransversales();
            SetMessage('#message', result.dataMsg, result.typeMsg);
        },
        error: function () {
            SetMessage('#message',
                'Ha ocurrido un error al intentar cargar las barras...',
                'error');
        }
    });
}