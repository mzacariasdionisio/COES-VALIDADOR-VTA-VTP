$(function () {
    mostrarListado();
});

function mostrarReporteByFiltros() {
    mostrarListado();
}

function mostrarListado() {
    var codigoVersion = getCodigoVersion();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaIngresoOpComercSEIN',
        data: {
            codigoVersion: codigoVersion
        },
        success: function (aData) {
            $('#listado').html(aData.Resultado);

            if (aData.Graficos[0].SerieData != null) {
                $('#grafico1').css("display", "block");
                GraficoPie(aData.Graficos[0], "grafico1");
            } else {
                $('#grafico1').css("display", "none");
            }


            if (aData.Graficos[1].SerieData.length > 0) {
                $('#grafico2').css("display", "block");
                GraficoColumnasBar(aData.Graficos[1], "grafico2");
            } else {
                $('#grafico2').css("display", "none");
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
                text: data.YaxixTitle,
                align: 'high',
                textAlign: 'center',
                rotation: 0,
                offset: 0,
                y: -25,
                x: -20,
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
