var controlador = siteRoot + 'InformeEjecutivoMen/reporteejecutivo/';

$(function () {
    ConsultarDespachoMaxPotenciaCoincidente();
});

function mostrarReporteByFiltros() {
    ConsultarDespachoMaxPotenciaCoincidente();
}

function ConsultarDespachoMaxPotenciaCoincidente() {
    var codigoVersion = getCodigoVersion();

    $.ajax({
        url: controlador + "ConsultarDespachoMaxPotenciaCoincidente",
        type: 'POST',
        data: {
            codigoVersion: codigoVersion
        },
        success: function (result) {
            switch (result.Estado) {
                case TipoEstado.OK:
                    GraficoArea(result.Grafico);
                    break;
                case TipoEstado.ERROR:
                    alert(result.Mensaje);
                    break;
            }
        },
        error: function (xhr, status) {
        },
        complete: function (xhr, status) {

        }
    });
}

function GraficoArea(data) {

    var series = [];

    for (var d in data.SerieData) {
        var item = data.SerieData[d];
        if (item == null) {
            continue;
        }
        series.push({
            name: item.Name,
            data: item.Data,
            type: item.Type,
            pointWidth: item.PointWidth,
            borderWidth: item.BorderWidth,
        });
    }

    Highcharts.chart('graficoDiaMaximaPotenCoincidente', {
        chart: {
            type: 'area',
            shadow: true
        },
        title: {
            text: data.TitleText
        },
        xAxis: {
            categories: data.XAxisCategories,
            crosshair: true
        },
        yAxis: {
            title: {
                text: data.YAxixTitle[0]
            },
            labels: {
                format: data.YaxixLabelsFormat
            }
        },
        tooltip: {
            valueDecimals: 2,
            shared: true,
            valueSuffix: data.TooltipValueSuffix
        },
        plotOptions: {
            area: {
                stacking: 'normal'
            },
            series: {
                lineWidth: 1
            }
        },
        series: series
    });

}
