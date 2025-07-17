//variables globales
var controlador = siteRoot + 'InformeEjecutivoMen/reporteejecutivo/';

$(function () {
    CargarEnergiaInterumpidaFallasZonas();
});

function mostrarReporteByFiltros() {
    CargarEnergiaInterumpidaFallasZonas();
}

function CargarEnergiaInterumpidaFallasZonas() {
    var codigoVersion = getCodigoVersion();

    $.ajax({
        type: 'POST',
        url: controlador + 'GetEnergiaInterumpidaFallasZonas',
        data: {
            codigoVersion: codigoVersion
        },
        success: function (aData) {
            $('#listado').html(aData.Resultado);
            GraficoColumnasBar(aData.Grafico, "grafico1");
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}
