var controlador = siteRoot + 'InformeEjecutivoMen/reporteejecutivo/';

$(function () {
    ConsultaCongestionEqTransmision();
});

function mostrarReporteByFiltros() {
    ConsultaCongestionEqTransmision();
}
 
function ConsultaCongestionEqTransmision() {
    var codigoVersion = getCodigoVersion();

    $.ajax({
        url: controlador + "ConsultaCongestionEqTransmision",
        type: 'POST',
        data: {
            codigoVersion: codigoVersion
        },
        success: function (result) {
            $("#listado").html(result.Resultado);
            GraficoColumnas(result.Grafico, "grafico1");
        },
        error: function (xhr, status) {
        },
        complete: function (xhr, status) {

        }
    });
}
