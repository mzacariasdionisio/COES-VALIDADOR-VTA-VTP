var controlador = siteRoot + 'InformeEjecutivoMen/reporteejecutivo/';


$(function () {
    CargarDemandaZonaCentro();
});

function mostrarReporteByFiltros() {
    CargarDemandaZonaCentro();
}

function CargarDemandaZonaCentro() {
    var codigoVersion = getCodigoVersion();

    $.ajax({
        url: controlador + "CargarDemandaZonaCentro",
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

