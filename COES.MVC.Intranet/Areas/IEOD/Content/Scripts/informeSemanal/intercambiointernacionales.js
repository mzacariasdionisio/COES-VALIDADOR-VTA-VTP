$(function () {
    cargarListaIntercambioInternacionalesSemanal();
});

function mostrarReporteByFiltros() {
    cargarListaIntercambioInternacionalesSemanal();
}

function cargarListaIntercambioInternacionalesSemanal() {
    var grafic = "grafico1";
    var codigoVersion = getCodigoVersion();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaIntercambioInternacionalesSemanal',
        data: {
            codigoVersion: codigoVersion
        },
        success: function (aData) {
            $('.filtro_fecha_desc').html(aData.FiltroFechaDesc);

            $('#listado').html(aData.Resultado);

            if (aData.Grafico.SeriesData[0]) {// si existen registros
                $('#' + grafic).css("display", "block");
                FlujoMaximoInterconexiones(aData, grafic);
            }
            else {// No existen registros
                $('#' + grafic).css("display", "none");
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function FlujoMaximoInterconexiones(result, idGrafico) {
    var opcion;
    opcion = {
        chart: {
            zoomType: 'xy'
        },
        title: {
            text: result.Grafico.TitleText,
            style: {
                fontSize: '14'
            }
        },
        xAxis: {
            categories: result.Grafico.XAxisCategories,
            type: 'datetime'
        },
        yAxis: [{ //Primary Axes
            title: {
                text: result.Grafico.YaxixTitle,
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
                    text: result.Grafico.YaxixTitle2,
                    color: result.Grafico.Series[1].Color
                },
                labels: {

                    style: {
                        color: result.Grafico.Series[1].Color
                    }
                },
                opposite: true
            }],
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
            //spline: {
            //    marker: {
            //        enabled: true
            //    }
            //}
            column: {
                stacking: 'true'//'percent'
            }
        },

        series: []
    };

    for (var i in result.Grafico.Series) {
        opcion.series.push({
            name: result.Grafico.Series[i].Name,
            data: result.Grafico.SeriesData[i],
            type: result.Grafico.Series[i].Type,
            color: result.Grafico.Series[i].Color,
            yAxis: result.Grafico.Series[i].YAxis,
            zIndex: result.Grafico.Series[i].ZIndex
        });
    }

    $('#' + idGrafico).highcharts(opcion);
}