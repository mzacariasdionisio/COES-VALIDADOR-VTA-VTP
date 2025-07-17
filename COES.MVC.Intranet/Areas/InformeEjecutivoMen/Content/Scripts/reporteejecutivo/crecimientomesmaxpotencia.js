var controlador = siteRoot + 'InformeEjecutivoMen/reporteejecutivo/';

$(function () {
    ConsultarCrecimientoMensualMaxPotencia();
});

function mostrarReporteByFiltros() {
    ConsultarCrecimientoMensualMaxPotencia();
}

function ConsultarCrecimientoMensualMaxPotencia() {
    var codigoVersion = getCodigoVersion();

    $.ajax({
        url: controlador + "ConsultarCrecimientoMensualMaxPotencia",
        type: 'POST',
        data: {
            codigoVersion: codigoVersion
        },
        success: function (result) {

            if (result.Resultados != null && result.Resultados.length > 0) {
                var html = '';
                for (var i = 0; i < result.Resultados.length; i++) {
                    html += `${result.Resultados[i]}<br/>`;
                }
                $("#html_mensaje_crec").html(html);
            }

            GraficoCombinadoDual(result.Grafico, 'graficoMaximaPotenciaCoincidente');
        },
        error: function (xhr, status) {
        },
        complete: function (xhr, status) {
        }
    });
}