var controlador = siteRoot + 'Interconexiones/';
$(function () {

    $('#FechaDesde').Zebra_DatePicker({
    });

    $('#FechaHasta').Zebra_DatePicker({
    });
    $("#cbMedidor").change(function () {
        buscarHistoricos();
    });
    $('#btnBuscar').click(function () {
        buscarHistoricos();
    });

    $('#btnExpotar').click(function () {
        exportarExcel();
    });

    buscarHistoricos();
});

function buscarHistoricos() {

    pintarPaginado();
    mostrarListado(1);
}

function pintarPaginado() {
    var ptomedicion = $('#cbInterconexion').val();
    $.ajax({
        type: 'POST',
        url: controlador + "reportes/paginado",
        data: { ptomedicion: ptomedicion, fechaInicial: $('#FechaDesde').val(), fechaFinal: $('#FechaHasta').val() },
        success: function (evt) {
            $('#paginado').html(evt);
            mostrarPaginado();
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function pintarBusqueda(nroPagina) {
    mostrarListado(nroPagina);
}

function mostrarListado(nroPagina) {
    var ptomedicion = $('#cbInterconexion').val();
    var medidor = $('#cbMedidor').val();

    $.ajax({
        type: 'POST',
        url: controlador + "reportes/ListaHistorico",
        data: {
            ptomedicion: ptomedicion, medidor: medidor, fechaInicial: $('#FechaDesde').val(), fechaFinal: $('#FechaHasta').val(),
            pagina: nroPagina
        },
        success: function (evt) {
            $('#listado').css("width", $('#mainLayout').width() + "px");

            $('#listado').html(evt);
            $('#tabla').dataTable({
                "scrollY": 430,
                "scrollX": true,
                "sDom": 't',
                "ordering": true,
                "iDisplayLength": 96
            });
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function exportarExcel() {
    var ptomedicion = $('#cbInterconexion').val();
    var medidor = $('#cbMedidor').val();

    $.ajax({
        type: 'POST',
        url: controlador + 'reportes/GenerarArchivoReporteHistorico',
        data: { ptomedicion: ptomedicion, medidor: medidor, fechaInicial: $('#FechaDesde').val(), fechaFinal: $('#FechaHasta').val() },
        dataType: 'json',
        success: function (result) {
            if (result == 1) {
                window.location = controlador + "reportes/ExportarReporte?tipo=4";
            }
            if (result == 0) {
                alert("No existen registros");
            }
            if (result == -1) {
                mostrarError();
            }
        },
        error: function () {
            mostrarError();
        }
    });
}

function mostrarError() {
    alert("Error");
}