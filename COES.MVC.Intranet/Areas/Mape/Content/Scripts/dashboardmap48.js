var controlador = siteRoot + 'mape/mape/';
$(function () {

    $('#txtFecha').Zebra_DatePicker();

    $('#btnConsultar').on('click',
        function () {
            generarDashboard();
        }
    );

});



function generarDashboard() {
    $.ajax({
        url: controlador + "ConstruirDashboard",
        data: { fecha: $('#txtFecha').val() },
        type: 'POST',
        success: function (result) {

            if (result.length > 0) {
                graficoEvolucionMapeMensualMaxPromMin(result[0]);
                graficoEvolucionMensual(result[1], "evolucionMapeMensual");
                graficoEvolucionMensual(result[2], "evolucionDesviacionEstandarMensual");
                graficoEvolucionMensual(result[3], "evolucionMediaMensual");
                graficoDiagramaDesviacion(result[4]);
                GraficoTacometro(result[5], 'tacometroMapeDiario');
                GraficoTacometro(result[6], 'tacometroMapeMensual');
                GraficoTacometro(result[7], 'tacometroMapeAnual');
            }
            else {
                alert("Error, no se genero versión para la fecha seleccionada");
            }
        },
        error: function (xhr, status) {
        },
        complete: function (xhr, status) {
        }
    });
};

function graficoEvolucionMapeMensualMaxPromMin(dataResult) {

    var data = dataResult.Grafico;


    Highcharts.stockChart('evolucionMapeMensualMaxPromMin', {
        chart: {
            shadow: true
        },
        rangeSelector: {
            selected: 4
        },
        tooltip: {
            pointFormat: '<span style="color:{point.color}">●</span> <b> {series.name}</b><br/>Promedio Mas: {point.open}<br/>Máximo: {point.high}<br/>Mínimo: {point.low}<br/>Promedio Menos: {point.close}<br/>',
            valueSuffix: ' ' + data.YaxixLabelsFormat
        },
        title: {
            text: data.TitleText
        },
        yAxis: {
            labels: {
                format: '{value} ' + data.YaxixLabelsFormat
            }
        },
        series: [{
            type: data.Series[0].Type,
            name: data.Series[0].Name,
            data: data.SeriesData,
            dataGrouping: {
                units: [
                    [
                        'month',
                        [1, 2, 3, 4, 6]
                    ],
                    [
                        'year',
                        [1, 2, 3]
                    ]
                ]
            }
        }]
    });
};

function graficoEvolucionMensual(dataResult, content) {
    var data = dataResult.Grafico;


    var series = [];

    for (var d in data.SerieData) {
        var item = data.SerieData[d];
        if (item === null) {
            continue;
        }
        series.push({ name: item.Name, data: item.Data });
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

function graficoDiagramaDesviacion(dataResult) {

    var data = dataResult.Grafico;
    if (data.SerieData.length < 1) {
        return;
    }
    var series = [];
    for (var d in data.SerieData) {
        var item = data.SerieData[d];
        if (item === null) {
            continue;
        }
        series.push({ name: item.Name, data: item.Data });
    }

    Highcharts.chart("DiagramaDistribucionDesciacion", {
        chart: {
            type: 'spline'
            , shadow: true
        },
        title: {
            text: data.TitleText
        },
        tooltip: {
            valueSuffix: ' %'
        },
        xAxis: {
            categories: data.XAxisCategories,
            plotLines: [{
                color: '#CCD6EB',
                width: 1,
                value: 14 // Position, you'll have to translate this to the values on your x axis
            }]
        },
        yAxis: {
            title: {
                text: data.YaxixTitle
            },
            labels: {
                format: '{value} %'
            }
        },
        plotOptions: {
            spline: {
                marker: {
                    enabled: false
                }
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



function GraficoTacometro(dataResult, content) {
    var data = dataResult.Grafico;
    if (data.PlotBands.length < 1) {
        return;
    }
    var dataPlot = [];
    for (var i in data.PlotBands) {
        var item = data.PlotBands[i];
        if (item === null) {
            continue;
        }
        dataPlot.push({ from: item.From, to: item.To, color: item.Color, thickness: item.Thickness });
    }

    var series = [];
    for (var d in data.SerieData) {
        item= data.SerieData[d];
        var align = (d % 2 === 0) ? "right" : "left";
        series.push({
            name: item.Name,
            color: item.Color,
            data: item.Data,
            tooltip: {
                valueSuffix: ' %'
            },
            dial: {
                backgroundColor: item.Color
            },
            showInLegend: true,
            dataLabels: {
                align: align,
                enabled: true,
                color: item.Color,
                allowOverlap: false,
                allowOverlap: true
            }
        });
    }

    Highcharts.chart(content, {

        chart: {
            type: 'gauge', shadow: true
        },

        title: {
            text: data.TitleText
        },
        pane: {
            startAngle: -90,
            endAngle: 90,
            background: null
        },
        yAxis: {
            min: data.YaxixMin,
            max: data.YaxixMax,
            lineColor: 'transparent',
            minorTickWidth: 0,
            tickLength: 0,
            tickPositions: data.YaxixTickPositions,
            labels: {
                step: 1,
                distance: 10
            },
            plotBands: dataPlot
        }, legend: {
            align: 'center',
            verticalAlign: 'top'
        },

        series: series

    });


};
