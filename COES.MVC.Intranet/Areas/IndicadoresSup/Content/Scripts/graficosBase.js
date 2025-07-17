function GraficoPie(data, content) {

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
            type: data.Type
        },
        title: {
            text: data.TitleText
        },
        tooltip: {
            pointFormat: '{series.name}: {point.percentage:.2f}%'
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
        series: [{innerSize: data.SeriesInnerSize, data: series }]
    });
}

 
function GraficoColumnas(data,content) {

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
                    return Highcharts.numberFormat(this.total, 2) + data.TooltipValueSuffix;
                }
            },
            reversedStacks: false
        },
        tooltip: {
            valueDecimals: 2,
            shared: true,
            valueSuffix: data.TooltipValueSuffix
        },
        plotOptions: {
            column: {
                stacking: 'normal',
                dataLabels: {
                    enabled: true,
                    color: (Highcharts.theme && Highcharts.theme.dataLabelsColor) || 'white',
                    formatter: function () {
                        return Highcharts.numberFormat(this.y, 2) + data.TooltipValueSuffix;
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
        series.push({ name: item.Name, data: item.Data, type: item.Type});
    }

    Highcharts.chart(content, {
        chart: {
            type: 'line',
            shadow: true
        },
        title: {
            text: data.TitleText
        },
        tooltip: {
            valueSuffix: ' ' + data.YaxixLabelsFormat
        },
        xAxis: {
            categories: data.XAxisCategories
        },
        yAxis: {
            title: {
                text: data.YaxixTitle
            },
            labels: {
                format: '{value} ' + data.YaxixLabelsFormat
            }
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
            layout: 'vertical',
            align: 'right',
            verticalAlign: 'middle'
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
        });
    }

    Highcharts.chart(content, {
        chart: {
            type: data.Type,
            shadow:true
        },
        title: {
            text: data.TitleText
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
                format: data.YaxixLabelsFormat
            },
            title: {
                text: data.YaxixTitle
            }
        },
        tooltip: {
            valueDecimals: 2,
            shared: true,
            valueSuffix: data.TooltipValueSuffix
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

