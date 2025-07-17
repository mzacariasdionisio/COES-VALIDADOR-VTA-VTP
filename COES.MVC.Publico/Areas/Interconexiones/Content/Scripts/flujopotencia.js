var controlador = siteRoot + 'Interconexiones/';
var chartTab1;
var chartTab2;
$(function () {
    $('#tab-container').easytabs({
        animate: false
    });
    $("#tab1").on("click", function () {
        mostrarListado();
    });
    $("#tab2").on("click", function () {
        mostrarListado2();
    });
    $('#FechaDesde').Zebra_DatePicker({

    });

    $('#FechaHasta').Zebra_DatePicker({

    });
    $('#FechaDesde2').Zebra_DatePicker({

    });

    $('#FechaHasta2').Zebra_DatePicker({

    });
    $('#btnBuscar').click(function () {
        mostrarListado();
    });

    $('#btnExpotar').click(function () {
        exportarExcel();
    });
    $('#btnBuscar2').click(function () {
        mostrarListado2();
    });
    $('#btnExpotar2').click(function () {
        exportarExcel2();
    });
    mostrarListado();
    mostrarListado2();
});

function mostrarListado() {
    $("#cbInterconexion").val($("#hfInterconexion").val());
    $.ajax({
        type: 'POST',
        url: controlador + "reportes/ListaFlujoPotencia",
        data: {
            idPtomedicion: $('#cbInterconexion').val(),
            tipoInterconexion: 1,
            parametro: $('#cbParametro').val(),
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
                    "scrollY": 610,
                    "scrollX": true,
                    "sDom": 't',
                    "iDisplayLength": -1
                });
            }
            generarGrafico();
        },
        error: function () {
            alert("Ha ocurrido un error en lista");
        }
    });
    //
}

function generarGrafico() {

    var fechadesde = $('#FechaDesde').val();
    var fechahasta = $('#FechaHasta').val();

    $('#hfFechaDesde').val(fechadesde);
    $('#hfFechaHasta').val(fechahasta);

    $.ajax({
        type: 'POST',
        url: controlador + "reportes/GraficoRepFlujoPot",
        data: {
            idPtomedicion: $('#cbInterconexion').val(),
            tipoInterconexion: 1,
            parametro: $('#cbParametro').val(),
            fechaInicial: $('#FechaDesde').val(),
            fechaFinal: $('#FechaHasta').val()
        },
        dataType: 'json',
        success: function (result) {
            if (result.Grafico.SerieDataS[0].length > 0) {
                graficoReporteFlujoPotenciaSt(result);
            }
        },
        error: function () {
            alert("Ha ocurrido un error en generar grafico");
        }
    });
}

graficoReporteFlujoPotenciaSt = function (result) {
    var series = [];
    var series1 = [];

    for (k = 0; k < result.Grafico.SerieDataS[0].length; k++) {
        var seriePoint = [];
        var now = parseJsonDate(result.Grafico.SerieDataS[0][k].X);
        var nowUTC = Date.UTC(now.getFullYear(), now.getMonth(), now.getDate(), now.getHours(), now.getMinutes(), now.getSeconds());
        seriePoint.push(nowUTC);
        seriePoint.push(result.Grafico.SerieDataS[0][k].Y);
        series.push(seriePoint);
    }
    if (result.Grafico.SerieDataS[1])
        for (k = 0; k < result.Grafico.SerieDataS[1].length; k++) {
            var seriePoint = [];
            var now = parseJsonDate(result.Grafico.SerieDataS[1][k].X);
            var nowUTC = Date.UTC(now.getFullYear(), now.getMonth(), now.getDate(), now.getHours(), now.getMinutes(), now.getSeconds());
            seriePoint.push(nowUTC);
            seriePoint.push(result.Grafico.SerieDataS[1][k].Y);
            series1.push(seriePoint);
        }

    var opcion = {
        rangeSelector: {
            selected: 1
        },

        title: {
            text: result.Grafico.titleText,
            style: {
                fontSize: '8'
            }
        },
        yAxis: [{
            title: {
                text: result.Grafico.YAxixTitle[0]
            },
            min: 0
        },
    {
        title: {
            text: result.Grafico.YAxixTitle[1]
        },
        opposite: false

    }],
        legend: {
            layout: 'horizontal',
            align: 'center',
            verticalAlign: 'bottom',
            borderWidth: 0,
            enabled: true
        },
        series: []
    };
    opcion.series.push({
        name: result.Grafico.Series[0].Name,
        color: result.Grafico.Series[0].Color,
        data: series,
        type: result.Grafico.Series[0].Type
    });
    if (result.Grafico.SerieDataS[1]) {
        opcion.series.push({
            name: result.Grafico.Series[1].Name,
            color: result.Grafico.Series[1].Color,
            data: series1,
            type: result.Grafico.Series[1].Type
        });
    }

    $('#graficos').highcharts('StockChart', opcion);
}

function parseJsonDate(jsonDateString) {
    return new Date(parseInt(jsonDateString.replace('/Date(', '')));
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
        url: controlador + "reportes/GenerarArchivoGrafFlujoPotencia",
        data: {
            idPtomedicion: $('#hfInterconexion').val(),
            tipoInterconexion: 1,
            parametro: $('#cbParametro').val(),
            fechaInicial: $('#hfFechaDesde').val(),
            fechaFinal: $('#hfFechaHasta').val()
        },
        dataType: 'json',
        success: function (result) {
            if (result == 1) {
                window.location = controlador + "reportes/ExportarReporte?tipo=3";
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

function mostrarListado2() {
    $("#cbInterconexion2").val($("#hfInterconexion").val());
    $.ajax({
        type: 'POST',
        url: controlador + "reportes/ListarIntercambioElectricidad",
        data: {
            idPtomedicion: $('#cbInterconexion2').val(),
            fechaInicial: $('#FechaDesde2').val(),
            fechaFinal: $('#FechaHasta2').val()
        },
        success: function (evt) {
            $('#listado2').css("width", $('#mainLayout').width() + "px");

            $('#listado2').html(evt);
            if ($('#tabla2 th').length > 1) {
                $('#tabla2').dataTable({
                    //"aoColumns": aoColumns(),
                    "bSort": false,
                    "scrollY": 630,
                    "scrollX": true,
                    "sDom": 't',
                    "iDisplayLength": -1
                });
            }
            generarGrafico2();
        },
        error: function () {
            alert("Ha ocurrido un error en lista");
        }
    });

}

function generarGrafico2() {
    var fechadesde = $('#FechaDesde2').val();
    var fechahasta = $('#FechaHasta2').val();

    $('#hfFechaDesde').val(fechadesde);
    $('#hfFechaHasta').val(fechahasta);

    $.ajax({
        type: 'POST',
        url: controlador + "reportes/graficorepinterelect",
        data: {
            idPtomedicion: $('#cbInterconexion2').val(),
            fechaInicial: fechadesde,
            fechaFinal: fechahasta
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
            renderTo: 'graficos2',
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
            categories: result.Grafico.xAxisCategories,
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
            pointFormat: '{point.x:%e. %b}: {point.y:.2f} m'
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
            data: result.Grafico.SeriesData[i],
            type: result.Grafico.Series[i].Type,
            color: result.Grafico.Series[i].Color,
            yAxis: result.Grafico.Series[i].YAxis
        });
    }
    //chartTab2 = $('#graficos2').highcharts(opcion);
    chartTab2 = new Highcharts.Chart(opcion);
}

function exportarExcel2() {
    var fechadesde = $('#FechaDesde2').val();
    var fechahasta = $('#FechaHasta2').val();
    var interconexion = $('#cbInterconexion2').val();

    $('#hfInterconexion').val(interconexion);
    $('#hfFechaDesde').val(fechadesde);
    $('#hfFechaHasta').val(fechahasta);


    $.ajax({
        type: 'POST',
        url: controlador + "reportes/GenerarArchivoGrafInterElect",
        data: {
            idPtomedicion: interconexion,
            fechaInicial: fechadesde,
            fechaFinal: fechahasta
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