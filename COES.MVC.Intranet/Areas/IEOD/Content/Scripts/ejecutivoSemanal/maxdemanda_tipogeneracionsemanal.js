$(function () {
    cargarMaximaDemandaTipoGeneracionSemanal();
});

function mostrarReporteByFiltros() {
    cargarMaximaDemandaTipoGeneracionSemanal();
}

function cargarMaximaDemandaTipoGeneracionSemanal() {
    var codigoVersion = getCodigoVersion();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarMaximaDemandaTipoGeneracionSemanal',
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

            $('#grafico1').css("display", "block");
            graficoBarraComparacionMDxTgeneracion(aData[1], 'grafico1');

            $('#grafico2').css("display", "block");
            graficoDiagramaCargaDespacho(aData[2], 'grafico2');

            $('#grafico3').css("display", "block");
            graficoBarraMDyEvolucionSemanal(aData[3].Grafico, 'grafico3');
            
            /*$('#grafico4').css("display", "block");
            GraficoBarraCrecyMDMensual(aData[4].Grafico, 'grafico4');*/
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function graficoBarraComparacionMDxTgeneracion(result, idGrafico) {
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
            spacingBottom: 60
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
        legend: {
            verticalAlign: 'top',
        },
        xAxis: {
            title: {
                text: result.Grafico.XAxisTitle,
                textAlign: 'center',
                align: 'high',
                rotation: 0
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
            },
            opposite: false,
            reversedStacks: false,
            lineWidth: 1
        },
        tooltip: {
            pointFormat: '{series.name} <b>{point.y:,.1f} ' + result.Grafico.YaxixTitle + '</b><br/>',
            shared: true
        },
        plotOptions: {
            column: {
                stacking: 'normal',
                dataLabels: {
                    enabled: false
                }
            },
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

function graficoDiagramaCargaDespacho(result, idGrafico) {
    var titulo = getHtmlSaltoLinea(result.Grafico.TitleText);

    var series = [];
    for (var i = 0; i < result.Grafico.Series.length; i++) {
        var serie = result.Grafico.Series[i];
        var obj = {
            showInLegend: !serie.NotShowInLegend,
            name: serie.Name,
            type: serie.Type,
            yAxis: serie.YAxis,
            data: result.Grafico.SeriesData[i],
            color: serie.Color
        };

        series.push(obj);
    }

    var opciones = {
        chart: {
            type: 'area',
            shadow: true,
            spacingBottom: 50
        },
        title: {
            text: titulo
        },
        subtitle: {
            text: result.Grafico.Subtitle,
            align: 'left',
            verticalAlign: 'bottom',
            floating: true,
            x: 10,
            y: 25
        },
        legend: {
            verticalAlign: 'top',
        },
        xAxis: {
            allowDecimals: false,
            title: {
                text: result.Grafico.XAxisTitle
            },
            categories: result.Grafico.XAxisCategories,
            labels: {
                format: '{value}'
            },
            tickInterval: 2
        },
        yAxis: {
            title: {
                text: result.Grafico.YaxixTitle,
                align: 'high',
                textAlign: 'center',
                rotation: 0,
                offset: 0,
                y: -25,
                x: -10,
            },
            labels: {
                formatter: function () {
                    return this.value;
                }
            },
            lineWidth: 1
        },
        tooltip: {
            pointFormat: '<span style="color:{series.color}">{series.name}</span>: <b>{point.y:,.2f}' + result.Grafico.YaxixTitle + '</b><br/>',
            shared: true
        },
        plotOptions: {
            area: {
                stacking: 'normal',
                marker: {
                    enabled: false,
                    symbol: 'circle',
                    radius: 2,
                    states: {
                        hover: {
                            enabled: true
                        }
                    }
                }
            }
        },
        series: series
    };

    $('#' + idGrafico).highcharts(opciones);
}

function graficoBarraMDyEvolucionSemanal(result, idGrafico) {
    var opcion = {
        chart: {
            zoomType: 'xy',
            shadow: true,
            spacingTop: 50,
            spacingBottom: 50
        },
        title: {
            text: result.TitleText
        },
        subtitle: {
            text: result.Subtitle,
            align: 'left',
            //x: -10,
            verticalAlign: 'bottom',
            floating: true,
            x: 10,
            y: 15
        },
        xAxis: {
            categories: result.XAxisCategories
        },
        yAxis: [{ //Primary Axes
            title: {
                text: result.Series[0].YAxisTitle,
                align: 'high',
                textAlign: 'center',
                rotation: 0,
                offset: 0,
                y: -25,
                x: -10,
            },
            labels: {
                format: '{value:,.1f}'
            },
            stackLabels: {
                enabled: false,
            },
            reversedStacks: false,
            lineWidth: 1,
            min: result.YaxixMin
        },
        { ///Secondary Axis
            title: {
                text: result.Series[1].YAxisTitle,
                align: 'high',
                textAlign: 'center',
                rotation: 0,
                offset: 0,
                y: -25,
                x: 15,
            },
            labels: {
                format: '{value:,.1f} %'
            },
            opposite: true,
            lineWidth: 1
            //min: -4
        }
        ],
        tooltip: {
            pointFormat: '<span style="color:{series.color}">{series.name}</span>: <b>{point.y:,.2f}</b> <br/>',
            shared: true
        },
        plotOptions: {
            column: {
                stacking: 'normal',
                dataLabels: {
                    enabled: false
                }
            },
            spline: { // PORCENTAJES
                marker: {
                    fillColor: '#FFFFFF',
                    lineWidth: 4,
                    lineColor: null // inherit from series
                }
            }
        },
        series: []
    };

    for (var i in result.Series) {
        opcion.series.push({
            name: result.Series[i].Name,
            type: result.Series[i].Type,
            color: result.Series[i].Color,
            yAxis: result.Series[i].YAxis,
            data: result.SeriesData[i]
        });
    }

    $('#' + idGrafico).highcharts(opcion);
}

function GraficoBarraCrecyMDMensual(result, idGrafico) {
    var subtitulo = result.Subtitle;
    for (var cont = 0; cont < result.ListaNota.length; cont++) {
        subtitulo += "\n" + result.ListaNota[cont];
    }

    var opcion = {
        chart: {
            zoomType: 'xy',
            shadow: true,
            spacingTop: 50,
            spacingBottom: 50
        },
        title: {
            text: result.TitleText
        },
        subtitle: {
            text: getHtmlSaltoLinea(subtitulo),
            align: 'left',
            //x: -10,
            verticalAlign: 'bottom',
            floating: true,
            x: 10,
            y: 15
        },
        xAxis: {
            categories: result.XAxisCategories,
            labels: { rotation: -90 }
        },
        yAxis: [{ //Primary Axes
            title: {
                text: result.Series[0].YAxisTitle,
                align: 'high',
                textAlign: 'center',
                rotation: 0,
                offset: 0,
                y: -25,
                x: -10,
            },
            labels: {
                format: '{value}',
                style: {
                }
            },
            stackLabels: {
                enabled: false,
            },
            reversedStacks: false,
            min: result.YaxixMin,
            lineWidth: 1
        },
        { ///Secondary Axis
            title: {
                text: result.Series[1].YAxisTitle,
                align: 'high',
                textAlign: 'center',
                rotation: 0,
                offset: 0,
                y: -25,
                x: 15,
            },
            labels: {
                format: '{value} %',
                style: {
                }
            },
            opposite: true,
            lineWidth: 1
        }
        ],
        legend: {
            reversed: true
        },
        tooltip: {
            pointFormat: '<span style="color:{series.color}">{series.name}</span>: <b>{point.y:,.2f}</b> <br/>',
            shared: true
        },
        plotOptions: {
            spline: {
                marker: {
                    fillColor: '#FFFFFF',
                    lineWidth: 4,
                    lineColor: null
                },
                dataLabels: {
                    enabled: true,
                    format: '{y:,.1f}',
                }
            },
            column: {
                stacking: 'normal',
                dataLabels: {
                    enabled: true,
                    format: '{y:,.2f}%',
                    color: '#FE2F2F',
                    verticalAlign: 'top',
                }
            }
        },
        series: []
    };

    for (var i in result.Series) {
        opcion.series.push({
            name: result.Series[i].Name,
            type: result.Series[i].Type,
            color: result.Series[i].Color,
            yAxis: result.Series[i].YAxis,
            data: result.SeriesData[i],
            zIndex: result.Series[i].ZIndex
        });
    }

    $('#' + idGrafico).highcharts(opcion);
}
