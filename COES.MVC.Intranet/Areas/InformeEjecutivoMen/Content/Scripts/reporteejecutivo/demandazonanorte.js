var controlador = siteRoot + 'InformeEjecutivoMen/reporteejecutivo/';


$(function () {
    CargarDemandaZonaNorte();
});

function mostrarReporteByFiltros() {
    CargarDemandaZonaNorte();
}

function CargarDemandaZonaNorte() {
    var codigoVersion = getCodigoVersion();

    $.ajax({
        url: controlador + "CargarDemandaZonaNorte",
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

