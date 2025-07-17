function GraficoPie(data, content) {

    var series = [];

    for (var d in data.SerieData) {
        var item = data.SerieData[d];
        if (item === null) {
            continue;
        }
        series.push({ name: item.Name, y: item.Y, drilldown: item.Drilldown });
    }

    var drilldown = [];
    for (var i in data.Drilldown) {
        var drill = data.Drilldown[i];
        if (drill === null) {
            continue;
        }

        var datadrill = [];
        for (var j in drill.Data) {
            var obdata = drill.Data[j];
            datadrill.push({ name: obdata.Name, y: obdata.Y });
        }

        drilldown.push({ name: drill.Name, type: drill.Type, id: drill.Id, data: datadrill });
    }


    Highcharts.chart(content, {
        chart: {
            shadow: true,
            type: data.Type
        },
        title: {
            text: data.TitleText
        },
        tooltip: {
            pointFormat: '{series.name}: {point.percentage:.2f} %'
        },
        plotOptions: {
            pie: {
                allowPointSelect: true,
                cursor: 'pointer',
                dataLabels: {
                    enabled: true,
                    format: '{point.name}: {point.percentage:.2f} %',
                    style: {
                        color: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black'
                    }
                }
            }
        },
        series: [{ innerSize: data.SeriesInnerSize, data: series }],
        drilldown: { series: drilldown }
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
            data: item.Data
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
            crosshair: true,
            labels: {
                rotation: data.XAxisLabelsRotation
            },
            title: {
                text: data.XAxisTitle
            }
        },
        yAxis: {
            min: data.YaxixMin,
            labels: {
                format: data.YaxixLabelsFormat
            },
            title: {
                text: data.YAxixTitle[0]
            },
            stackLabels: {
                enabled: data.YAxisStackLabels,
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
            valueSuffix: data.TooltipValueSuffix,
            valuePrefix: data.TooltipValuePrefix
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

function GraficoLinea(data, content) {

    var series = [];

    for (var d in data.SerieData) {
        var item = data.SerieData[d];
        if (item === null) {
            continue;
        }
        series.push({ name: item.Name, data: item.Data, type: item.Type, color: item.Color });
    }

    Highcharts.chart(content, {
        chart: {
            type: 'line',
            shadow: data.Shadow
        },
        title: {
            text: data.TitleText
        },
        tooltip: {
            valueDecimals: data.TooltipValueDecimals,
            valuePrefix: data.TooltipValuePrefix,
            shared: true,
            valueSuffix: ' ' + data.YaxixLabelsFormat
        },
        xAxis: {
            categories: data.XAxisCategories,
            crosshair: true,
            title: {
                text: data.XAxisTitle
            }
        },
        yAxis: {
            title: {
                text: data.YAxixTitle
            },
            labels: {
                format: '{value} ' + data.YaxixLabelsFormat
            },
            max: data.YaxixMax,
            tickInterval: data.TickInterval
        },
        plotOptions: {
            line: {
                dataLabels: {
                    enabled: false
                },
                marker: {
                    enabled: true
                },
                enableMouseTracking: true
            }
        },
        legend: {
            layout: data.LegendLayout,
            align: data.LegendAlign,
            verticalAlign: data.LegendVerticalAlign
        },
        series: series,
        responsive: {
            rules: [{
                condition: {
                    maxWidth: 500
                },
                chartOptions: {
                    legend: {
                        layout: 'horizontal',
                        align: 'center',
                        verticalAlign: 'bottom'
                    }
                }
            }]
        }
    });
}

function GraficoColumnasBar(data, content) {

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
            color: item.Color
        });
    }

    Highcharts.chart(content, {
        chart: {
            type: data.Type,
            shadow: data.Shadow
        },
        title: {
            text: data.TitleText
        },
        tooltip: {
            valueDecimals: 2,
            shared: true,
            valueSuffix: ' ' + data.YaxixLabelsFormat
        },
        xAxis: {
            categories: data.XAxisCategories,
            crosshair: true,
            labels: {
                rotation: data.XAxisLabelsRotation
            }
        },
        yAxis: {
            labels: {
                format: '{value} ' + data.YaxixLabelsFormat
            },
            title: {
                text: data.YaxixTitle
            }
        },
        plotOptions: {
            column: {
                pointPadding: 0.2,
                borderWidth: 0
            }
        },
        series: series
    });

}

/**
 * Permite generar graficos combinados con dos ejes Y
 * @param {any} data objeto de tipo Graficoweb
 * @param {any} content id del content
 */
function GraficoCombinadoDual(data, content) {

    var series = [];

    for (var d in data.SerieData) {
        var item = data.SerieData[d];
        if (item === null) {
            continue;
        }
        series.push({
            type: item.Type,
            name: item.Name,
            data: item.Data,
            yAxis: item.YAxis,
            tooltip: {
                valuePrefix: item.TooltipValuePrefix,
                valueSuffix: item.TooltipValueSuffix
            }
        });
    }

    Highcharts.chart(content, {
        chart: {
            shadow: data.Shadow
        },
        title: {
            text: data.TitleText
        },
        xAxis: {
            categories: data.XAxisCategories,
            crosshair: true,
            title: {
                text: data.XAxisTitle
            }
        },
        yAxis: [{
            labels: {
                format: data.YAxisLabelsFormat[0]
            },
            title: {
                text: data.YAxixTitle[0]
            }
        },
        {
            gridLineWidth: 1,
            title: {
                text: data.YAxixTitle[1]
            },
            labels: {
                format: data.YAxisLabelsFormat[1]
            },
            opposite: true

        }], tooltip: {
            valueDecimals: data.TooltipValueDecimals,
            shared: true

        },
        series: series
    });
}

function GraficoPie3D(data, content) {

    var series = [];

    for (var d in data.SerieData) {
        var item = data.SerieData[d];
        if (item === null) {
            continue;
        }
        series.push({ name: item.Name, y: item.Y });
    }

    Highcharts.chart(content, {
        chart: {
            shadow: true,
            type: 'pie',
            options3d: {
                enabled: true,
                alpha: 45,
                beta: 0
            }
        },
        title: {
            text: data.TitleText
        },
        subtitle: {
            text: data.Subtitle
        },
        tooltip: {
            pointFormat: '{series.name}: <b>{point.percentage:.2f}%</b>'
        },
        plotOptions: {
            pie: {
                allowPointSelect: true,
                cursor: 'pointer',
                depth: 35,
                dataLabels: {
                    enabled: true,
                    format: '{point.name}'
                }
            }
        },
        series: [{
            type: 'pie',
            data: series
        }]
    });
}
