var controlador = siteRoot + 'Siosein/TablasPrieDeclaracionMen/'

$(function () {

    cargarListaDifusionHorasOperacion();
    cargarGraficoDifusionHorasOperacion();
});

function cargarListaDifusionHorasOperacion() {

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaDifusionHorasOperacion',
        data: { mesAnio: $('#txtFecha').val() },
        success: function (aData) {
            $('#listado1').html(aData.Resultado);
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function cargarGraficoDifusionHorasOperacion() {
    $.ajax({
        type: 'POST',
        url: controlador + 'CargarGraficoDifusionHorasOperacion',
        data: { idEmpresa: $('#hfEmpresa').val(), mesAnio: $('#txtFecha').val() },
        dataType: 'json',
        success: function (aData) {
            if (aData.nRegistros > 0) {
                disenioGrafico(aData, 'idGrafico1', 1);
            } else $('#idGrafico1').html('Sin Grafico - No existen registros a mostrar!');
        },
        error: function () {
            alert('Ha ocurrido un error');
        }
    });
}

function disenioGrafico(result, grafico, tip) {
    var opcion;
    if (tip == 1) {
        opcion = {
            title: {
                text: result.Grafico.TitleText,
                style: {
                    fontSize: '14'
                }
            },
            xAxis: {
                categories: result.Grafico.XAxisCategories
            },
            yAxis: [{ //Primary Axes
                title: {
                    text: result.Grafico.YaxixTitle
                },
                labels: {
                    style: {
                        color: result.Grafico.Series[0].Color
                    }
                }

            }],
            tooltip: {
                headerFormat: '<b>{series.name}</b><br>',
                pointFormat: '{point.y:.3f}'
            },
            plotOptions: {
                column: {
                    stacking: 'normal'
                }
            },

            series: []
        };

        for (var i in result.Grafico.Series) {
            opcion.series.push({
                name: result.Grafico.Series[i].Name,
                data: result.Grafico.SeriesData[i],
                type: result.Grafico.Series[i].Type,
                yAxis: result.Grafico.Series[i].YAxis
            });
        }
    }
    $('#' + grafico).highcharts(opcion);
}