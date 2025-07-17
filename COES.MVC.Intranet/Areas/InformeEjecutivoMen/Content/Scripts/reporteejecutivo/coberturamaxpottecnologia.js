var controlador = siteRoot + 'InformeEjecutivoMen/reporteejecutivo/';

$(function () {
    ConsultarCoberturaMaxPotenciaCoincidenteTecnologia();
});

function mostrarReporteByFiltros() {
    ConsultarCoberturaMaxPotenciaCoincidenteTecnologia();
}

function ConsultarCoberturaMaxPotenciaCoincidenteTecnologia() {
    var codigoVersion = getCodigoVersion();

    $.ajax({
        url: controlador + "ConsultarCoberturaMaxPotenciaCoincidenteTecnologia",
        type: 'POST',
        data: {
            codigoVersion: codigoVersion
        },
        success: function (result) {

            if (result.Resultados != null && result.Resultados.length > 0) {
                var html = "Equipos que no tienen clasificación:<br/>";
                html += '<ul>';
                for (var i = 0; i < result.Resultados.length; i++) {
                    html += `<li>${result.Resultados[i]}</li>`;
                }
                html += '</ul>';
                $("#html_mensaje_tec").html(html);
            }

            GraficoPie(result.Grafico, "graficoDMaximaPotenCoincidenteTecnologia");
        },
        error: function (xhr, status) {
        },
        complete: function (xhr, status) {

        }
    });
}

