var controlador = siteRoot + 'InformeEjecutivoMen/ReporteEjecutivo/'

$(function () {        
    CargarCostosMarginalesModoOpe();
});

function mostrarReporteByFiltros() {
    CargarCostosMarginalesModoOpe();
}

function CargarCostosMarginalesModoOpe() {
    var codigoVersion = getCodigoVersion();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarCostosMarginalesModoOpe',
        data: {
            codigoVersion: codigoVersion
        },
        success: function (aData) {
            GraficoLinea(aData.Graficos[0], "grafico1");
            GraficoLinea(aData.Graficos[1], "grafico2");
        },
        error: function (err) { alert("Ha ocurrido un error"); }
    });
}