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
        url: controlador + 'CargarListaProduccionTipoGen',
        data: {
            codigoVersion: codigoVersion
        },
        success: function (aData) {
            $('#listado').html(aData[0].Resultado);
            $('#reporte').dataTable({
                "sDom": 't',
                "ordering": false,
                paging: false
            });

            $('#idGraficoContainer').html('');

            $('#grafico1').css("display", "block");
            graficoProduccionEnergiaElectricaAcumulada(aData[1].Grafico);
            
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function graficoProduccionEnergiaElectricaAcumulada(result) {
    var opcion;
    var json = result.Series;
    var jsondata = [];
    for (var i in json) {
        var jsonValor = [];
        var jsonLista = json[i].Data;
        for (var j in jsonLista) {
            jsonValor.push(jsonLista[j].Y);
        }
        jsondata.push({
            name: json[i].Name,
            data: jsonValor,
            color: json[i].Color,
        });
    }

    opcion = {
        chart: {
            type: 'column',
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
            categories: result.XAxisCategories,
            type: 'datetime'
        },
        yAxis: {
            title: {
                text: getHtmlSaltoLinea(result.YaxixTitle),
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
            lineWidth: 1
        },
        tooltip: {
            pointFormat: 'Producción <b>{point.y:,.1f}</b><br/> en el año {series.name}'
        },
        plotOptions: {
            area: {
                stacking: 'normal',
                pointStart: 1,
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
        series: jsondata
    };
    $('#grafico1').highcharts(opcion);
}

function graficoEvolucionSemanalEnergia(result) {
    var opcion;

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

    opcion = {
        chart: {
            zoomType: 'xy',
            shadow: true,
            spacingTop: 20,
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
            categories: categoria,
            labels: {
                autoRotation: [0]
            },
            crosshair: true,
            tickInterval: 3
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
                //color: result.Grafico.Series[0].Color
            },
            labels: {
                style: {
                    //color: result.Grafico.Series[0].Color
                }
            },
            lineWidth: 1,
            min: result.YaxixMin
        },
        { ///Secondary Axis
            title: {
                text: getHtmlSaltoLinea(result.Series[2].YAxisTitle),
                align: 'high',
                textAlign: 'center',
                rotation: 0,
                offset: 0,
                y: -25,
                x: 0,
            },
            labels: {
                format: '{value} %'
            },
            lineWidth: 1,
            opposite: true,
            min: -5
        }],
        tooltip: {
            pointFormat: '<span style="color:{series.color}">{series.name}</span>: <b>{point.y:,.2f}</b> <br/>',
            shared: true
        },
        plotOptions: {
            spline: { // PORCENTAJES
                marker: {
                    fillColor: '#FFFFFF',
                    lineWidth: 3,
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
    $('#grafico2').highcharts(opcion);
}
