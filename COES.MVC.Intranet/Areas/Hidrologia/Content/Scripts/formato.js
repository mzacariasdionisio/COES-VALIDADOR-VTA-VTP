var controlador = siteRoot + 'hidrologia/'
var oTable;
var oTableHead;
$(function () {

    $('#cbArea').val("0");
    $('#cbFmtOrigen').val("0");
    $('#btnBuscar').click(function () {
        buscarFormato();
    });

    $('#btnNuevo').click(function () {
        nuevoFormato(0);
    });

    $('#tablaPtos a.edicion').click(function (e) {
        e.preventDefault();

        /* Get the row as a parent of the link that was clicked on */
        alert("HI");
        var nRow = $(this).parents('tr')[0];

        if (nEditing !== null && nEditing != nRow) {
            /* Currently editing - but not this row - restore the old before continuing to edit mode */
            restoreRow(oTable, nEditing);
            editRow(oTable, nRow);
            nEditing = nRow;
        }
        else if (nEditing == nRow && this.innerHTML == "Save") {
            /* Editing this row and want to save it */
            saveRow(oTable, nEditing);
            nEditing = null;
        }
        else {
            /* No edit in progress - let's start one */
            editRow(oTable, nRow);
            nEditing = nRow;
        }
    });

    buscarFormato();
    $(window).resize(function () {
        $('#listado').css("width", $('#mainLayout').width() + "px");
    });

});

function buscarFormato() {
    mostrarListado();
}

function mostrarListado() {
    var codigoApp = parseInt($("#hdCodigoApp").val()) || 0;

    $.ajax({
        type: 'POST',
        url: controlador + "formatomedicion/listaformato",
        data: {
            area: $('#cbArea').val(),
            formatcodiOrigen: $('#cbFmtOrigen').val(),
            app: codigoApp
        },
        success: function (evt) {
            $('#listado').css("width", $('#mainLayout').width() + "px");

            $('#listado').html(evt);
            $('#tabla').dataTable({
                bJQueryUI: true,
                "scrollY": 550,
                "scrollX": false,
                "sDom": 'ft',
                "ordering": true,
                "iDisplayLength": 200
            });
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function mostrarDetalle2(formato) {

    $.ajax({
        type: 'POST',
        url: controlador + "formatomedicion/ListaHoja",
        data: {
            formato: formato
        },
        success: function (evt) {

            $('#detalle').html(evt);

        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });

}

function mostrarDetalle(formato) {
    var codigoApp = parseInt($("#hdCodigoApp").val()) || 0;
    var direccion = controlador + "formatomedicion/IndexDetalle?id=" + formato + "&app=" + codigoApp;

    if (formato == 127 || formato == 128) {
        direccion = controlador + "formatomedicion/DetalleEquipos?id=" + formato + "&app=" + codigoApp;
    }
    location.href = direccion;
}

function nuevoFormato(id) {

    $.ajax({
        type: 'POST',
        url: controlador + "formatomedicion/agregar?id=" + id,
        success: function (evt) {
            $('#edicionGrupo').html(evt);
            setTimeout(function () {
                $('#popupFormato').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown'
                });
            }, 50);
            $('#alerta').css("display", "none");
            $('#mensaje').css("display", "none");
        },
        error: function () {
            mostrarError();
        }
    });
}

function agregarFormato() {
    area = $('#cbAreas').val();
    resolucion = $("#cbResolucion").val();
    horizonte = $("#cbHorizonte").val();
    periodo = $("#cbPeriodo").val();
    nombre = $('#txtNombre').val();
    descripcion = $('#txtDescripcion').val();
    lectura = $('#idLecturas').val();
    tituloHoja = $('#txtTitulo').val();
    diaplazo = $('#txtDiaPlazo').val();
    minPlazo = $('#txtMinPlazo').val();

    if (nombre == "") {
        alert("Ingresar Nombre del Formato");
        return;
    }
    if (area == 0) {
        alert("Seleccionar el area");
        return;
    }
    if (resolucion == 0) {
        alert("Seleccionar Resolución");
        return;
    }
    if (horizonte == 0) {
        alert("Seleccionar Horizonte");
        return;
    }
    if (periodo == 0) {
        alert("Seleccionar Periodo");
        return;
    }
    if (tituloHoja == "") {
        alert("Ingresar nombre del Titulo");
        return;
    }
    if (lectura == 0) {
        alert("Seleccionar Lectura");
        return;
    }

    $.ajax({

        type: 'POST',
        url: controlador + 'formatomedicion/agregarformato',
        dataType: 'json',
        data: {
            nombre: nombre, area: area, resolucion: resolucion, horizonte: horizonte, periodo: periodo, lectura: lectura,
            tituloHoja: tituloHoja, diaPlazo: diaplazo, minPlazo: minPlazo, descripcion: descripcion
        },
        cache: false,
        success: function (resultado) {
            if (resultado == 1) {
                $('#popupUnidad').bPopup().close();
                // cargar();
            }
            else {
                alert("Error al grabar formato");
            }

        },
        error: function () {
            alert("Error al grabar formato");
        }

    });
}

function mostrarVerificacionFormato(formato) {
    var codigoApp = parseInt($("#hdCodigoApp").val()) || 0;
    location.href = controlador + "formatomedicion/indexVerificacionFormato?id=" + formato + "&app=" + codigoApp;
}

