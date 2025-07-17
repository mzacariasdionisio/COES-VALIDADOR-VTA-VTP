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
    $("#cbInterconexion").val($("#hfInterconexion").val());
    mostrarListado();
}

function mostrarListado() {
    $("#cbInterconexion").val($("#hfInterconexion").val());
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
            if ($('#tabla th').length > 1) {
                $('#tabla').dataTable({
                    //"aoColumns": aoColumns(),
                    "bSort": false,
                    "scrollY": 630,
                    "scrollX": true,
                    "sDom": 't',
                    "iDisplayLength": 50
                });
            }
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
            graficoReporteInterElect(result);
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
            text: result.Grafico.titleText
        },
        subtitle: {
            text: result.Grafico.subtitleText
        },
        xAxis: {
            categories: result.Grafico.XAxisCategories,
            labels: {
                rotation: -90,
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
                text: result.Grafico.yAxixTitle
            },
            labels: {

                style: {
                    color: Highcharts.getOptions().colors[1]
                }
            }

        },
            { ///Secondary Axis
                title: {
                    text: "MW"
                },
                labels: {

                    style: {
                        color: Highcharts.getOptions().colors[0]
                    }
                },
                opposite: true
            }],
        tooltip: {
            headerFormat: '<b>{series.name}</b><br>',
            pointFormat: '{point.y:.3f} m'
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
    for (var i in result.Grafico.seriesName) {
        opcion.series.push({
            name: result.Grafico.seriesName[i],
            data: result.Grafico.seriesData[i],
            type: result.Grafico.seriesType[i],
            yAxis: result.Grafico.seriesYAxis[i]
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