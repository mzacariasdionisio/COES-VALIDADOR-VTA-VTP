var controlador = siteRoot + 'Medidores/ReportesInterconexion/';
$(function () {

    $('#FechaDesde').Zebra_DatePicker({
    });

    $('#FechaHasta').Zebra_DatePicker({
    });
    $('#btnBuscar').click(function () {
        buscarHistoricos();
    });

    $('#btnExpotar').click(function () {
        exportar();
    });

    $("#cbEmpresa").val($("#hfEmpresa").val());
    buscarHistoricos();
});

function buscarHistoricos() {

    pintarPaginado();
    mostrarListado(1);
}

function pintarPaginado() {
    var ptomedicion = $("#cbLinea").val()
    $.ajax({
        type: 'POST',
        url: controlador + "paginado",
        data: {ptomedicion: ptomedicion , fechaInicial: $('#FechaDesde').val(), fechaFinal: $('#FechaHasta').val()},
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
    var ptomedicion = $("#cbLinea").val();
    $.ajax({
        type: 'POST',
        url: controlador + "ListaHistorico",
        data: {
            ptomedicion: ptomedicion, fechaInicial: $('#FechaDesde').val(), fechaFinal: $('#FechaHasta').val(),
            pagina: nroPagina
        },
        success: function (evt) {
            $('#listado').css("width", $('#mainLayout').width() + "px");

            $('#listado').html(evt);
            $('#tabla').dataTable({
                "scrollY": 430,
                "scrollX": true,
                "sDom": 't',
                "ordering": false,
                "iDisplayLength": 96
            });
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function exportar()
{
    var ptomedicion = $("#cbLinea").val();
    $.ajax({
        type: 'POST',
        url: controlador + 'GenerarArchivoReporte',
        data: { ptomedicion: ptomedicion, fechaInicial: $('#FechaDesde').val(), fechaFinal: $('#FechaHasta').val()},
        dataType: 'json',
        success: function (result) {
            if (result == 1) {
                window.location = controlador + "ExportarReporte";
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