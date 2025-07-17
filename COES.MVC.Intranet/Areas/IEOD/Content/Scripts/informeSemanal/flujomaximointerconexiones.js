$(function () {
    cargarListaFlujoMaximoInterconexiones();
});

function mostrarReporteByFiltros() {
    cargarListaFlujoMaximoInterconexiones();
}

function cargarListaFlujoMaximoInterconexiones() {
    var codigoVersion = getCodigoVersion();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaFlujoMaximoInterconexiones',
        data: {
            codigoVersion: codigoVersion
        },
        success: function (result) {
            $('.filtro_fecha_desc').html(result.FiltroFechaDesc);

            for (var i = 0; i < 2; i++) {
                $('#listado' + (i + 1)).html(result.ResultadosHtml[i]);
                GraficoColumnas(result.Graficos[i], "grafico"+(i+1));
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function GraficoColumnas(data, content) {

    var series = [];

    if (data === undefined) {
        return;
    }

    for (var d in data.SerieData) {
        var item = data.SerieData[d];
        if (item === null) {
            continue;
        }
        series.push({
            name: item.Name,
            data: item.Data,
        });
    }

    Highcharts.chart(content, {
        chart: {
            type: 'column',
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
            min: 0,
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
                    return Highcharts.numberFormat(this.total, data.PlotOptionsDataLabelsDigit) + data.TooltipValueSuffix;
                }
            },
            reversedStacks: false
        },
        tooltip: {
            valueDecimals: data.TooltipValueDecimals,
            shared: true,
            valueSuffix: data.TooltipValueSuffix
        },
        subtitle: {
            text: data.Subtitle,
            align: 'left',
            verticalAlign: 'bottom',
            floating: true,
            x: 10,
            y: 9
        },
        plotOptions: {
            column: {
                stacking: 'normal',
                dataLabels: {
                    enabled: data.PlotOptionsDataLabels,
                    color: (Highcharts.theme && Highcharts.theme.dataLabelsColor) || 'white',
                    formatter: function () {
                        return Highcharts.numberFormat(this.y, data.PlotOptionsDataLabelsDigit) + data.TooltipValueSuffix;
                    }
                }
            }
        },
        series: series
    });
}
