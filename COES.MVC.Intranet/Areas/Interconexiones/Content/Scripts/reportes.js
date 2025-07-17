var controlador = siteRoot + 'Interconexiones/';
$(function () {

    $('#FechaDesde').Zebra_DatePicker({

    });

    $('#FechaHasta').Zebra_DatePicker({

    });

    $('#btnBuscar').click(function () {
        buscarDatos();
    });

    $('#btnExpotar').click(function () {
        exportarExcel();
    });

    //cargarPrevio();
    buscarDatos();
});

function buscarDatos() {
    //$("#cbInterconexion").val($("#hfInterconexion").val());
    mostrarListado();
}

function mostrarListado() {

    $.ajax({
        type: 'POST',
        url: controlador + "reportes/ListarIntercambioElectricidad",
        data: {
            idPtomedicion: $('#cbInterconexion').val(),
            fechaInicial: $('#FechaDesde').val(),
            fechaFinal: $('#FechaHasta').val()
        },
        success: function (evt) {
            $('#listado').css("width", $('#mainLayout').width() + "px");

            $('#listado').html(evt);
            
            generarGrafico();
        },
        error: function () {
            alert("Ha ocurrido un error en lista");
        }
    });

}

function generarGrafico() {
    var fechadesde = $('#FechaDesde').val();
    var fechahasta = $('#FechaHasta').val();

    $('#hfFechaDesde').val(fechadesde);
    $('#hfFechaHasta').val(fechahasta);

    $.ajax({
        type: 'POST',
        url: controlador + "reportes/graficorepinterelect",
        data: {
            idPtomedicion: $('#cbInterconexion').val(),
            fechaInicial: $('#FechaDesde').val(),
            fechaFinal: $('#FechaHasta').val()
        },
        dataType: 'json',
        success: function (result) {
            if (result.Grafico.SeriesData[0]) {
                graficoReporteInterElect(result);
            }
        },
        error: function () {
            alert("Ha ocurrido un error en generar grafico");
        }
    });
}

graficoReporteInterElect = function (result) {
    var opcion = {
        chart: {
            type: 'spline'
        },
        title: {
            text: result.Grafico.titleText,
            style: {
                fontSize: '8'
            }
        },
        subtitle: {
            text: result.Grafico.subtitleText,
            style: {
                fontSize: '8'
            }
        },
        xAxis: {
            categories: result.Grafico.XAxisCategories,
            labels: {
                rotation: -45,
                style: {
                    color: Highcharts.getOptions().colors[1],
                    fontSize: '1'
                }
            },

            title: {
                text: result.Grafico.xAxisTitle
            },
        },
        yAxis: [{ //Primary Axes
            title: {
                text: result.Grafico.yAxixTitle,
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
                    text: "MW",
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

        plotOptions: {
            spline: {
                marker: {
                    enabled: true
                },
                dataLabels: {
                    enabled: true,
                    format: '{y:.3f}'
                },
                formatter: function () {
                    return $.number(this.value, 3, ',', ' ')
                },
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
            yAxis: result.Grafico.Series[i].YAxis
        });
    }
    $('#graficos').highcharts(opcion);
}

function exportarExcel() {
    var fechadesde = $('#FechaDesde').val();
    var fechahasta = $('#FechaHasta').val();
    var interconexion = $('#cbInterconexion').val();

    $('#hfInterconexion').val(interconexion);
    $('#hfFechaDesde').val(fechadesde);
    $('#hfFechaHasta').val(fechahasta);


    $.ajax({
        type: 'POST',
        url: controlador + "reportes/GenerarArchivoGrafInterElect",
        data: {
            idPtomedicion: $('#hfInterconexion').val(),
            fechaInicial: $('#hfFechaDesde').val(),
            fechaFinal: $('#hfFechaHasta').val()
        },
        dataType: 'json',
        success: function (result) {
            if (result == 1) {
                window.location = controlador + "reportes/ExportarReporte?tipo=2";
            }
            if (result == -1) {
                alert("Error en mostrar documento Excel")
            }
            if (result == 0) {
                alert("No existen registros");
            }
        },
        error: function () {
            alert("Error en Grafico export a Excel");
        }
    });


}