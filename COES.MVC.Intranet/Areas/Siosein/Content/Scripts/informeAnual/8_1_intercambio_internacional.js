$(function () {
    cargarListadoNumeral();
});

function mostrarReporteByFiltros() {
    cargarListadoNumeral();
}

function cargarListadoNumeral() {
    var codigoVersion = getCodigoVersion();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarIntercambiosIntEnergPot',
        data: {
            codigoVersion: codigoVersion
        },
        success: function (aData) {
            $('#listado').html(aData.Resultado);
            $('#listado2').html(aData.Resultado2);

            $("#grafico1").show();
            graficoReporteInterElect("#grafico1", aData.Grafico);
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function graficoReporteInterElect(idHtml, grafico) {
    var opcion = {
        chart: {
            type: 'spline'
        },
        title: {
            text: grafico.titleText,
            style: {
                fontSize: '8'
            }
        },
        subtitle: {
            text: grafico.subtitleText,
            style: {
                fontSize: '8'
            }
        },
        xAxis: {
            categories: grafico.XAxisCategories,
            labels: {
                rotation: -45,
            },

            title: {
                text: grafico.xAxisTitle
            },
        },
        yAxis: [{ //Primary Axes
            title: {
                text: grafico.YaxixTitle,
                color: grafico.Series[0].Color
            },
            labels: {

                style: {
                    color: grafico.Series[0].Color
                }
            }

        },
        { ///Secondary Axis
            title: {
                text: "Potencia (MW)",
                color: grafico.Series[1].Color
            },
            labels: {
                style: {
                    color: grafico.Series[1].Color
                }
            },
            opposite: true
        }],
        tooltip: {
            headerFormat: '<b>{series.name}</b><br>',
            pointFormat: '{point.y:.3f}'
        },

        plotOptions: {
            spline: {
                marker: {
                    enabled: true
                },
                dataLabels: {
                    enabled: true,
                },
            }
        },

        series: []
    };
    for (var i in grafico.Series) {
        opcion.series.push({
            name: grafico.Series[i].Name,
            data: grafico.SeriesData[i],
            type: grafico.Series[i].Type,
            color: grafico.Series[i].Color,
            yAxis: grafico.Series[i].YAxis
        });
    }
    $(idHtml).highcharts(opcion);
}