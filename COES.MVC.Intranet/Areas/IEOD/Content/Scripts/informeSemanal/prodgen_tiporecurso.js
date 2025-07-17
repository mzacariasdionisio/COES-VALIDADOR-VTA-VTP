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
        url: controlador + 'CargarListaProduccionTipoRecurso',
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

            $('#idGraficoContainer').html('');

            $('#grafico1').css("display", "block");
            graficoComparacionProduccionEnergiaAcumuladaXTipoRecursoEnergetico(aData[1].Grafico);

            $('#grafico2').css("display", "block");
            graficoEvolucionSemanalRecursosEnergeticos(aData[2].Grafico);

            graficoPie(aData[3].Grafico, "grafico3_1");
            graficoPie(aData[4].Grafico, "grafico3_2");

        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function graficoComparacionProduccionEnergiaAcumuladaXTipoRecursoEnergetico(result) {
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
            inverted: true,
            spacingTop: 20,
            spacingBottom: 50,
            spacingLeft: 10,
            spacingRight: 70
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
        legend: {
            align: 'center',
            verticalAlign: 'top',
            layout: 'horizontal'
        },
        xAxis: {
            categories: result.XAxisCategories,
            type: 'datetime'
        },
        yAxis: {
            title: {
                text: result.YaxixTitle,
                align: 'high',
                textAlign: 'center',
                rotation: 0,
                offset: 0,
                y: 0,
                x: 40,
            },
            labels: {
                formatter: function () {
                    return this.value;
                }
            }
        },
        tooltip: {
            pointFormat: 'Producción <b>{point.y:,.0f}</b><br/> en el año {series.name}'
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

function graficoEvolucionSemanalRecursosEnergeticos(result) {
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
                text: result.Series[0].YAxisTitle,
                align: 'high',
                textAlign: 'center',
                rotation: 0,
                offset: 0,
                y: -25,
                x: -10,
                //color: result.Grafico.Series[0].Color
            },
            reversedStacks: false,
            labels: {
                format: '{value} %'
            },
            min: result.YaxixMin,
            max: result.YaxixMax
        }],
        tooltip: {
            pointFormat: '<span style="color:{series.color}">{series.name}</span>: <b>{point.y}%</b><br/>',
            shared: true
        },
        plotOptions: {
            column: {
                borderWidth: 0,
                stacking: 'normal',
                dataLabels: {
                    enabled: false
                }
            },
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

function graficoPie(result, idGrafico) {
    var  json = result.Series;

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
            shadow: true,
            plotBackgroundColor: null,
            plotBorderWidth: null,
            type: 'pie',
            spacingBottom: 50
        },
        title: {
            text: result.TitleText
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
                    enabled: true,
                    format: '<b>{point.name}</b>: {point.percentage:.1f} %'
                },
                showInLegend: false
            }
        },
        series: [{
            name: 'Total',
            colorByPoint: true,
            data: jsondata
        }]
    });
}