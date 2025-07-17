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
        url: controlador + 'CargarListaFactorPlantaRER',
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

            $('#grafico2').css("display", "block");
            graficoBarraFactorPlantaAcumuladoCentralRER(aData[1].Grafico, "grafico2");

            for (var i = 2; i < aData.length; i++) {
                $('#grafico1_' + (i - 1)).css("display", "block");
                graficoProdGenyFactorPlantaXTgeneracionRER(aData[i].Grafico, 'grafico1_' + (i - 1));
                $("#txt_grafico1").html(aData[i].Grafico.Subtitle);
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function graficoProdGenyFactorPlantaXTgeneracionRER(result, idGrafico) {
    var opcion = {
        chart: {
            zoomType: 'xy',
            shadow: true,
            spacingTop: 10,
            spacingBottom: 10
        },
        title: {
            text: result.TitleText
        },
        subtitle: {
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
            reversedStacks: false
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
                format: '{value:,.2f}'
            },
            opposite: true
        }],
        tooltip: {
            pointFormat: '<span style="color:{series.color}">{series.name}</span>: <b>{point.y}</b> <br/>',
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

function graficoBarraFactorPlantaAcumuladoCentralRER(result, idGrafico) {
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
        legend: {
            align: 'center',
            verticalAlign: 'top',
            layout: 'horizontal'
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
                text: result.Series[0].YAxisTitle,
                align: 'high',
                textAlign: 'center',
                rotation: 0,
                offset: 0,
                y: -25,
                x: -10,
            },
            labels: {
                format: '{value:,.2f}'
            }
        }],
        tooltip: {
            pointFormat: '<span style="color:{series.color}">{series.name}</span>: <b>{point.y}</b> <br/>',
            shared: true
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
