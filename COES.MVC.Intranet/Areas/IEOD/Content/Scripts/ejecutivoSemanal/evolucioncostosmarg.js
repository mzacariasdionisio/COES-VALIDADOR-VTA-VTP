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
        url: controlador + 'CargarListaEvolCostosMarginalesProm',
        data: {
            codigoVersion: codigoVersion
        },
        success: function (aData) {
            $('.filtro_fecha_desc').html(aData.FiltroFechaDesc);
            $('#listado').html(aData.Resultado);

            if (aData.Grafico.Series != null) {// si existen registros
                $('#grafico1').css("display", "block");
                GraficoEvolCostosMarginalesProm(aData);
            }
            else {// No existen registros
                $('#grafico1').css("display", "none");
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });

}

function GraficoEvolCostosMarginalesProm(result) {
    var opcion;
    opcion = {
        chart: {
            zoomType: 'xy'
        },
        title: {
            text: result.Grafico.TitleText,
            /* style: {
                 fontSize: '14'
             }*/
        },
        xAxis: {
            categories: result.Grafico.XAxisCategories
        },
        yAxis: [{ //Primary Axes
            title: {
                text: result.Grafico.Series[0].YAxisTitle,
                color: result.Grafico.Series[0].Color
            },
            labels: {
                style: {
                    color: result.Grafico.Series[0].Color
                }
            }

        },
        { ///Secondary Axis
            title: {
                text: result.Grafico.Series[1].YAxisTitle,
                color: result.Grafico.Series[1].Color
            },
            labels: {

                style: {
                    color: result.Grafico.Series[1].Color
                }
            },
            opposite: true
        }
            /*{ // Tertiary yAxis
                gridLineWidth: 0,
                title: {
                    text: result.Grafico.Series[2].YAxisTitle,
                    color: result.Grafico.Series[2].Color
                },
                labels: {
                    style: {
                        color: result.Grafico.Series[2].Color
                    }
                },
                opposite: true
            }*/],
        tooltip: {
            headerFormat: '<b>{series.name}</b><br>',
            pointFormat: '{point.y:.3f}'
        },
        subtitle: {
            text: result.Grafico.Subtitle,
            align: 'left',
            verticalAlign: 'bottom',
            floating: true,
            x: 10,
            y: 9
        },
        plotOptions: {
            spline: {
                marker: {
                    enabled: true
                }
            }
        },

        series: []
    };

    for (var i in result.Grafico.Series) {
        opcion.series.push({
            name: result.Grafico.Series[i].Name,
            type: result.Grafico.Series[i].Type,
            color: result.Grafico.Series[i].Color,
            yAxis: result.Grafico.Series[i].YAxis,
            data: result.Grafico.SeriesData[i]
        });
    }

    $('#grafico1').highcharts(opcion);
}