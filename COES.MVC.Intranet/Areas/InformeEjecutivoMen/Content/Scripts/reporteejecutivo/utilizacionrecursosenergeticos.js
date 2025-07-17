var controlador = siteRoot + 'InformeEjecutivoMen/reporteejecutivo/';

$(function () {
    ConsultarUtilizacionRecursosEnergeticos();
});

function mostrarReporteByFiltros() {
    ConsultarUtilizacionRecursosEnergeticos();
}

function ConsultarUtilizacionRecursosEnergeticos() {
    var codigoVersion = getCodigoVersion();

    $.ajax({
        url: controlador + "ConsultarUtilizacionRecursosEnergeticos",
        type: 'POST',
        data: {
            codigoVersion: codigoVersion
        },
        success: function (result) {
            $("#listado").html(result.Resultado);

            if (result.Resultados != null && result.Resultados.length > 0) {
                var html = "";
                html += '<ul>';
                for (var i = 0; i < result.Resultados.length; i++) {
                    html += `<li>${result.Resultados[i]}</li>`;
                }
                html += '</ul>';
                $("#html_mensaje_fenerg").html(html);
            }

            GraficoPie(result.Graficos[0], "grafico1");
            GraficoColumnas(result.Graficos[1], "grafico2");
            GraficoPie(result.Graficos[2], "grafico3");
            GraficoColumnas(result.Graficos[3], "grafico4");
        },
        error: function (xhr, status) {
        },
        complete: function (xhr, status) {

        }
    });
}
