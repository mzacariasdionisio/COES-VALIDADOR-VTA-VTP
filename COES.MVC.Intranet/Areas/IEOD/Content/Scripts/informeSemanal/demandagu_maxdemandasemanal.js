$(function () {
    getdemandagumaximademandasemanal();
});

function mostrarReporteByFiltros() {
    getdemandagumaximademandasemanal();
}

function getdemandagumaximademandasemanal() {
    var codigoVersion = getCodigoVersion();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarDemandaGUMaximaDemandaSemanal',
        data: {
            codigoVersion: codigoVersion
        },
        success: function (aData) {
            $('.filtro_fecha_desc').html(aData[0].FiltroFechaDesc);
            $('#listado').html(aData[0].Resultado);

            $('#reporte').dataTable({
                "sDom": 't',
                "ordering": false,
                paging: false
            });

            graficodemandagumaximademandasemanalBarra(aData[1], "grafico1");
            graficodemandagumaximademandasemanalDiagramaCarga(aData[2], "grafico2");
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function graficodemandagumaximademandasemanalBarra(result, idGrafico) {
    var opcion;

    var series = [];
    for (var i = 0; i < result.Grafico.Series.length; i++) {
        var serie = result.Grafico.Series[i];
        var obj = {
            name: serie.Name,
            type: serie.Type,
            yAxis: serie.YAxis,
            data: result.Grafico.SeriesData[i],
            color: serie.Color
        };

        series.push(obj);
    }

    opcion = {
        chart: {
            type: 'column',
            shadow: true,
            inverted: true,
            spacingBottom: 50
        },
        title: {
            text: result.Grafico.TitleText
        },
        subtitle: {
            text: result.Grafico.Subtitle,
            align: 'left',
            //x: -10,
            verticalAlign: 'bottom',
            floating: true,
            x: 10,
            y: 15
        },
        xAxis: {
            title: {
                text: result.Grafico.XAxisTitle,
                textAlign: 'center',
                align: 'high',
                rotation: 0,
                x: 50
            },
            categories: result.Grafico.XAxisCategories,
            crosshair: true,
            scrollbar: {
                enabled: true
            },
        },
        yAxis: {
            title: {
                text: result.Grafico.YaxixTitle //MW
            },
            labels: {
                formatter: function () {
                    return this.value;
                }
            },
            lineWidth: 1,
            opposite: true
        },
        tooltip: {
            pointFormat: '{series.name} <b>{point.y:,.00f} MW</b><br/>'
        },
        plotOptions: {
            series: {
                label: {
                    connectorAllowed: false
                }
            }
        },
        series: series
    };

    $('#' + idGrafico).highcharts(opcion);
}

function graficodemandagumaximademandasemanalDiagramaCarga(result, idGrafico) {
    //generar series
    var series = [];
    for (var i = 0; i < result.Grafico.Series.length; i++) {
        var serie = result.Grafico.Series[i];
        var obj = {
            name: serie.Name,
            type: serie.Type,
            yAxis: serie.YAxis,
            data: result.Grafico.SeriesData[i],
            color: serie.Color
        };

        series.push(obj);
    }
    var dataHora = result.Grafico.XAxisCategories;
    var tituloGrafico = result.Grafico.TitleText;

    //Generar grafica
    opcion = {
        chart: {
            zoomType: 'xy',
            spacingTop: 50,
            spacingBottom: 40,
            shadow: true
        },
        title: {
            text: tituloGrafico
        },
        subtitle: {
            text: result.Grafico.Subtitle,
            verticalAlign: 'bottom',
        },
        xAxis: [{
            categories: dataHora,
            crosshair: true,
            tickInterval: 3
        }],
        yAxis: {
            title: {
                align: 'high',
                textAlign: 'center',
                rotation: 0,
                offset: 0,
                y: -25,
                x: -10,
                text: getHtmlSaltoLinea(result.Grafico.YaxixTitle)
            },
            labels: {
                format: '{value}',
            },
            lineWidth: 1
        },
        tooltip: {
            shared: true
        },
        legend: {
            align: 'center',
            verticalAlign: 'bottom',
            layout: 'horizontal'
        },
        plotOptions: {
            spline: {
                lineWidth: 1,
                states: {
                    hover: {
                        lineWidth: 5
                    }
                },
                marker: {
                    enabled: false
                }
            }
        },
        series: series
    };

    $('#' + idGrafico).highcharts(opcion);
}