var controlador = siteRoot + 'InformeEjecutivoMen/reporteejecutivo/';

$(function () {
    ConsultarIngresoEmprIntegrAlCoes();
});

function mostrarReporteByFiltros() {
    ConsultarIngresoEmprIntegrAlCoes();
}

function ConsultarIngresoEmprIntegrAlCoes() {
    var codigoVersion = getCodigoVersion();

    $.ajax({
        url: controlador + "ConsultarIngresoEmprIntegrAlCoes",
        type: 'POST',
        data: {
            codigoVersion: codigoVersion
        },
        success: function (result) {
            $("#listado").html(result.Resultado);
        },
        error: function (xhr, status) {
        },
        complete: function (xhr, status) {

        }
    });
}


