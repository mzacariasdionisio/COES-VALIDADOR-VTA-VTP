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
    //pintarPaginado(1);
    //$("#cbInterconexion").val($("#hfInterconexion").val());
    mostrarListado();
}

function mostrarListado() {

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
                    "scrollY": 630,
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