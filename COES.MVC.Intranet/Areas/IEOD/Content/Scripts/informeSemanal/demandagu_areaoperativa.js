$(function () {
    getdemandaenergiagu();
});

function mostrarReporteByFiltros() {
    getdemandaenergiagu();
}

function getdemandaenergiagu() {
    var codigoVersion = getCodigoVersion();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarDemandaGUareaOperativa',
        data: {
            codigoVersion: codigoVersion
        },
        success: function (aData) {
            $('.filtro_fecha_desc').html(aData[0].FiltroFechaDesc);

            //
            $('#listado').html(aData[0].Resultado);

            //
            graficoBarraEvolucionXPto(aData[1].Grafico, "grafico2");

            //
            graficoPie(aData[2].Grafico, "grafico3_1");
            $("#txt_grafico3").html(aData[2].Grafico.XAxisTitle);
            graficoPie(aData[3].Grafico, "grafico3_2");

            //
            graficoBarraEvolucionSemanal(aData[4].Grafico, "grafico4");
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function graficoBarraEvolucionXPto(result, idGrafico) {
    var categoria = [];

    for (var d in result.Categorias) {
        var item = result.Categorias[d];
        if (item == null) {
            continue;
        }
        categoria.push({
            name: item.Name,
            categories: item.Categories,
        });
    }
     
    var opcion = {
        chart: {
            zoomType: 'xy',
            shadow: true,
            spacingTop: 50,
            spacingBottom: 50
        },
        title: {
            text: ''
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
            categories: categoria,
            labels: {
                rotation: -90
            },
            crosshair: true
        },
        yAxis: [{ //Primary Axes
            title: {
                text: getHtmlSaltoLinea(result.Series[0].YAxisTitle), 
                align: 'high',
                textAlign: 'center',
                rotation: 0,
                offset: 0,
                y: -25,
                x: -10,
            },
            labels: {
                style: {
                }
            },
            lineWidth: 1
        },
        { ///Secondary Axis
            title: {
                text: getHtmlSaltoLinea(result.Series[2].YAxisTitle),
                align: 'high',
                textAlign: 'center',
                rotation: 0,
                offset: 0,
                y: -25,                
            },
            labels: {
                format: '{value:,.1f}%'
            },
            opposite: true
        }],
        tooltip: {
            headerFormat: '<b>{series.name}</b><br>',
            pointFormat: '{point.y:.3f}'
        },
        plotOptions: {
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

function graficoPie(result, idGrafico) {
    var opcion, json, _titulo;
    _titulo = result.TitleText;
    json = result.Series;

    var jsondata = [];

    for (var i = 0; i < json.length; i++) {
        var jsonLista = json[i];
        jsondata.push({
            name: jsonLista.Name,
            y: jsonLista.Acumulado,
            color: jsonLista.Color,
            sliced: true
        });
    }

    Highcharts.chart(idGrafico, {
        chart: {
            plotBackgroundColor: null,
            plotBorderWidth: null,
            plotShadow: false,
            type: 'pie'
        },
        title: {
            text: _titulo
        },
        subtitle: {
            text: result.Subtitle,
            align: 'center',
            //x: -10,
            verticalAlign: 'bottom',
            floating: true,
            x: 10,
            y: 15
        },
        tooltip: {
            pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b> <br/> Valor(GWh): <b>{point.y:.1f}</b>'
        },
        plotOptions: {
            pie: {
                allowPointSelect: true,
                cursor: 'pointer',
                dataLabels: {
                    enabled: false
                },
                showInLegend: true
            }
        },
        series: [{
            name: 'Total',
            colorByPoint: true,
            data: jsondata
        }]
    });
}

function graficoBarraEvolucionSemanal(result, idGrafico) {
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
            categories: result.XAxisCategories,
            title: {
                text: result.XAxisTitle
            }
        },
        yAxis: [{ //Primary Axes
            title: {
                text: getHtmlSaltoLinea(result.Series[0].YAxisTitle),
                align: 'high',
                textAlign: 'center',
                rotation: 0,
                offset: 0,
                y: -25,
                x: -10,
            },
            labels: {
                style: {
                }
            },
            stackLabels: {
                enabled: false,
            },
            lineWidth: 1,
            reversedStacks : false
        },
        { ///Secondary Axis
            title: {
                text: '',
                align: 'high',
                textAlign: 'center',
                rotation: 0,
                offset: 0,
                y: -25,
                x: 15,
            },
            labels: {
                format: '{value:,.2f}'
            },
            lineWidth: 1,
            opposite: true
        }],
        tooltip: {
            headerFormat: '<b>Semana {point.x}</b><br>',
            pointFormat: '<span style="color:{series.color}">{series.name}</span>: <b>{point.y}</b><br/>',
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
