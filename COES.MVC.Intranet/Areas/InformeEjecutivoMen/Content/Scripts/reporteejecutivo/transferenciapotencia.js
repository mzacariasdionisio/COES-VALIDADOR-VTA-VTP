var controlador = siteRoot + 'InformeEjecutivoMen/ReporteEjecutivo/';

$(function () {
    CargarTransferenciaPotencia();
});

function mostrarReporteByFiltros() {
    CargarTransferenciaPotencia();
}

function CargarTransferenciaPotencia() {
    var codigoVersion = getCodigoVersion();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarTransferenciaPotencia',
        data: {
            codigoVersion: codigoVersion
        },
        success: function (aData) {
            $('#listado1').html(aData.Resultados[0]);
            $('#listado2').html(aData.Resultados[1]);
            GraficoColumnasBar(aData.Grafico, 'grafico1');
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}
