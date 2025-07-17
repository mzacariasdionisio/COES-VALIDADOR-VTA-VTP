var controlador = siteRoot + 'InformeEjecutivoMen/reporteejecutivo/';

$(function () {
    ConsultarUtilizacionRecursosEnergeticosProduccionElectrica();
});

function mostrarReporteByFiltros() {
    ConsultarUtilizacionRecursosEnergeticosProduccionElectrica();
}

function ConsultarUtilizacionRecursosEnergeticosProduccionElectrica() {
    var codigoVersion = getCodigoVersion();

    $.ajax({
        url: controlador + "ConsultarUtilizacionRecursosEnergeticosProduccionElectrica",
        type: 'POST',
        data: {
            codigoVersion: codigoVersion
        },
        success: function (result) {
            $("#listado").html(result.Resultado);
            GraficoColumnasAgrupadas(result.Grafico, "grafico1");
        },
        error: function (xhr, status) {
        },
        complete: function (xhr, status) {

        }
    });
}

function GraficoColumnasAgrupadas(data,content) {

    var series = [];
    var categoria = [];

    for (var d in data.SerieData) {
        var item = data.SerieData[d];
        if (item == null) {
            continue;
        }
        series.push({
            name: item.Name,
            data: item.Data,
        });
    }


    for (var d in data.Categorias) {
        var item = data.Categorias[d];
        if (item == null) {
            continue;
        }
        categoria.push({
            name: item.Name,
            categories: item.Categories,
        });
    }




    Highcharts.chart(content, {
        chart: {
            type: 'column',
            shadow: true
        },
        tooltip: {
            pointFormat: '<span style="color:{series.color}">{series.name}</span>: <b>{point.y}</b> ({point.percentage:.0f}%)<br/>',
            shared: true
        },
        title: {
            text: data.TitleText
        },
        plotOptions: {
            column: {
                stacking: 'percent'
            }
        },
        yAxis: {
            labels: {
                format: data.YaxixLabelsFormat
            },
            title: {
                text: data.YAxixTitle[0]
            },
            stackLabels: {
                enabled: true,
                style: {
                    fontWeight: 'bold',
                    color: (Highcharts.theme && Highcharts.theme.textColor) || 'gray'
                },
                formatter: function () {
                    return Highcharts.numberFormat(this.total, 2) + data.TooltipValueSuffix;
                }
            },
            reversedStacks: false,
        },
        tooltip: {
            valueDecimals: 2,
            shared: true,
            valueSuffix: data.TooltipValueSuffix
        },
        xAxis: {
            categories: categoria,
            labels: {
                autoRotation: [0]
            },
            crosshair: true
        },
        series: series
    });

}
