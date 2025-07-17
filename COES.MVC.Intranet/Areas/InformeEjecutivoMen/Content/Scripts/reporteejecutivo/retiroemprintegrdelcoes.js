var controlador = siteRoot + 'InformeEjecutivoMen/reporteejecutivo/';

$(function () {
    ConsultarRetiroEmprIntegrDelCoes();
});

function mostrarReporteByFiltros() {
    ConsultarRetiroEmprIntegrDelCoes();
}

function ConsultarRetiroEmprIntegrDelCoes() {
    var codigoVersion = getCodigoVersion();

    $.ajax({
        url: controlador + "ConsultarRetiroEmprIntegrDelCoes",
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


