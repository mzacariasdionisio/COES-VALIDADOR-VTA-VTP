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
            $('.filtro_fecha_desc').html(aData[0].FiltroFechaDesc);

            $('#listado').html(aData[0].Resultado);
            $('#reporte').dataTable({
                "sDom": 't',
                "ordering": false,
                paging: false
            });

            $('#idGraficoContainer').html('');

            $('#grafico1').css("display", "block");
            graficoProduccionEnergiaElectricaAcumulada(aData[1].Grafico);

            $('#grafico2').css("display", "block");
            graficoBarraEnergiayEvolucionAnual(aData[2].Grafico, 'grafico2');
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

function graficoBarraEnergiayEvolucionAnual(result, idGrafico) {
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
        }],
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
