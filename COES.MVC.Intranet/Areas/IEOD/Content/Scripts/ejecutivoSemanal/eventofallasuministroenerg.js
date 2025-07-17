$(function () {
    cargarEventoFallaSuministroEnerg();
});

function mostrarReporteByFiltros() {
    cargarEventoFallaSuministroEnerg();
}

function cargarEventoFallaSuministroEnerg() {
    var codigoVersion = getCodigoVersion();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarEventoFallaSuministroEnerg',
        data: {
            codigoVersion: codigoVersion
        },
        success: function (aData) {
            $('.filtro_fecha_desc').html(aData.FiltroFechaDesc);

            $('#listado').html(aData.Resultado);
            
            if (aData.Graficos[0].SerieData != null) {
                $('#grafico1').css("display", "block");
                GraficoPie(aData.Graficos[0], "grafico1");
            } else {
                $('#grafico1').css("display", "none");
            }

            if (aData.Graficos[1].SerieData != null) {
                $('#grafico2').css("display", "block");
                GraficoColumnas(aData.Graficos[1], "grafico2");
            } else {
                $('#grafico2').css("display", "none");
            }
            
            if (aData.Graficos[2].SerieData != null) {
                $('#grafico3').css("display", "block");
                GraficoColumnasBar(aData.Graficos[2], "grafico3");
            } else {
                $('#grafico3').css("display", "none");
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

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
            pointFormat: '{series.name}: {point.percentage:.2f} %'
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
        series: [{ innerSize: data.SeriesInnerSize, data: series }]
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
            data: item.Data
        });
    }

    Highcharts.chart(content, {
        chart: {
            type: data.Type,
            shadow: true
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
                pointPadding: 0.2,
                borderWidth: 0
            }
        },
        series: series
    });

}
