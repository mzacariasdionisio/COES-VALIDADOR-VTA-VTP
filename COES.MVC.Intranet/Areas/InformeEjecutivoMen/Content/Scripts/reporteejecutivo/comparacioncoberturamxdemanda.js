var controlador = siteRoot + 'InformeEjecutivoMen/reporteejecutivo/';

$(function () {
    ConsultarComparacionCoberturaMaxDemanda();
});

function mostrarReporteByFiltros() {
    ConsultarComparacionCoberturaMaxDemanda();
}

function ConsultarComparacionCoberturaMaxDemanda() {
    var codigoVersion = getCodigoVersion();

    $.ajax({
        url: controlador + "ConsultarComparacionCoberturaMaxDemanda",
        type: 'POST',
        data: {
            codigoVersion: codigoVersion
        },
        success: function (result) {

            GraficoColumnas(result.Grafico, "graficoMaximaDemandoXGeneracion");

        },
        error: function (xhr, status) {
        },
        complete: function (xhr, status) {
        }
    });
}
