var controlador = siteRoot + 'Medidores/ReportesInterconexion/';
$(function () {

    $('#FechaDesde').Zebra_DatePicker({
    });

    $('#FechaHasta').Zebra_DatePicker({
    });
    $('#btnBuscar').click(function () {
        buscarEnvios();
    });
    $('#btnExportar').click(function () {
        exportar();
    });
    $("#cbEmpresa").val($("#hfEmpresa").val());
    $("#cbEstado").val(-1);
    buscarEnvios();
});

function buscarEnvios() {

    pintarPaginado();
    mostrarListado(1);
}

function pintarPaginado() {
    var empresa = $("#cbEmpresa").val()
    $.ajax({
        type: 'POST',
        url: controlador + "paginadoenvio",
        data: { empresa: empresa, fechaInicial: $('#FechaDesde').val(), fechaFinal: $('#FechaHasta').val(), estado: $('#cbEstado').val() },
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
    var empresa = $('#cbEmpresa').val();
    var estado = $('#cbEstado').val();
    
    $.ajax({
        type: 'POST',
        url: controlador + "lista",
        data: {
            empresa: empresa,estado : estado ,fechaInicial: $('#FechaDesde').val(), fechaFinal: $('#FechaHasta').val(),
            nroPagina: nroPagina
        },
        success: function (evt) {
            $('#listado').css("width", $('#mainLayout').width() + "px");

            $('#listado').html(evt);
            $('#tabla').dataTable({
                "scrollY": 430,
                "scrollX": true,
                "sDom": 't',
                "ordering": true,
                "iDisplayLength": 50
            });
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function abrirArchivo(archivo)
{
    //window.location = siteRoot + 'Medidores/Uploads/' + archivo;
    window.location = controlador + 'DescargarArchivoEnvio?archivo=' + archivo;
}

function exportar() {
    var empresa = $("#cbEmpresa").val();
    var estado = $('#cbEstado').val();
    $.ajax({
        type: 'POST',
        url: controlador + 'GenerarArchivoReporteEnvio',
        data: { empresa: empresa,estado:estado, fechaInicial: $('#FechaDesde').val(), fechaFinal: $('#FechaHasta').val() },
        dataType: 'json',
        success: function (result) {
            if (result == 1) {
                window.location = controlador + "ExportarReporteEnvio";
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

function mostrarError()
{
    alert("Error");
}