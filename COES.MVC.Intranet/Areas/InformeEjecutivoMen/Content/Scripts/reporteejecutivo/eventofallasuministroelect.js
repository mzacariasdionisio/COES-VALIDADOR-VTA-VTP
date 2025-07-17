//variables globales
var controlador = siteRoot + 'InformeEjecutivoMen/reporteejecutivo/'

$(function () {
    GetFallaTipoequipoCausa();
});

function mostrarReporteByFiltros() {
    GetFallaTipoequipoCausa();
}

function GetFallaTipoequipoCausa() {
    var codigoVersion = getCodigoVersion();

    $.ajax({
        type: 'POST',
        url: controlador + 'GetFallaTipoequipoCausa',
        data: {
            codigoVersion: codigoVersion
        },
        success: function (aData) {
            $('#listado').html(aData.Resultado);
            GraficoPie(aData.Graficos[0], "grafico1");
            GraficoColumnas(aData.Graficos[1], "grafico2");
            GraficoColumnasBar(aData.Graficos[2], "grafico3");

        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}
