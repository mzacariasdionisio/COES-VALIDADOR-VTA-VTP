var controlador = siteRoot + 'InformeEjecutivoMen/reporteejecutivo/';

$(function () {
    ConsultaInterconexiones();
});

function mostrarReporteByFiltros() {
    ConsultaInterconexiones();
}

function ConsultaInterconexiones() {
    var codigoVersion = getCodigoVersion();

    $.ajax({
        url: controlador + "ConsultaInterconexiones",
        type: 'POST',
        data: {
            codigoVersion: codigoVersion
        },
        success: function (result) {

            $("#listado").html(result.Resultados[0]);
            $("#listado1").html(result.Resultados[1]);

            if (result.Graficos.length > 0) {
                GraficoColumnas(result.Graficos[0], "grafico1");
            }
            if (result.Graficos.length > 1) {
                GraficoColumnas(result.Graficos[1], "grafico2");
            }
        },
        error: function (xhr, status) {
        },
        complete: function (xhr, status) {

        }
    });
}
