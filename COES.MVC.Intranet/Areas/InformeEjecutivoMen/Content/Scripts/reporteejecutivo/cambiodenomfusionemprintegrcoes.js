var controlador = siteRoot + 'InformeEjecutivoMen/reporteejecutivo/';

$(function () {
    ConsultarCambioDenomFusionEmprIntegrCoes();
});

function mostrarReporteByFiltros() {
    ConsultarCambioDenomFusionEmprIntegrCoes();
}

function ConsultarCambioDenomFusionEmprIntegrCoes() {
    var codigoVersion = getCodigoVersion();

    $.ajax({
        url: controlador + "ConsultarCambioDenomFusionEmprIntegrCoes",
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