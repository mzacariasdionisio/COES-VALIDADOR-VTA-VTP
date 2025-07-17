var controlador = siteRoot + 'InformeEjecutivoMen/reporteejecutivo/';

$(function () {
    CargarDemandaZonaSur();
});
function mostrarReporteByFiltros() {
    CargarDemandaZonaSur();
}

function CargarDemandaZonaSur() {
    var codigoVersion = getCodigoVersion();

    $.ajax({
        url: controlador + "CargarDemandaZonaSur",
        type: 'POST',
        data: {
            codigoVersion: codigoVersion
        },
        success: function (result) {

            $('#listado1').html(result.Resultado);

            GraficoColumnasBar(result.Grafico, "grafico1");
        },
        error: function (xhr, status) {
            alert("Ha ocurrido un error");
        },
        complete: function (xhr, status) {
        }
    });
};

