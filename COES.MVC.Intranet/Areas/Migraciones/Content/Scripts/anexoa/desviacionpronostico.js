var controlador = siteRoot + 'Migraciones/AnexoA/'
$(function () {
   
    $('#btnExportar').click(function () {
        exportarExcelReporte(1);
    });
    buscar();
});

buscar = function () {
    pintarBusqueda(1);
    pintarPaginado();
}

pintarPaginado = function () {    
    $.ajax({
        type: 'POST',
        url: controlador + "paginadoDesvDemanda",
        data: {
            fechaInicio: $('#txtFechaInicio').val(),
            fechaFin: $('#txtFechaFin').val()           
        },
        success: function (evt) {
            $('#paginado').html(evt);
            mostrarPaginado();
        },
        error: function () {
            alert('Error Paginado!');
        }
    });
}

function mostrarPaginado() {
    var nroToShow = parseInt($('#hfNroMostrar').val());
    var nroPaginas = parseInt($('#hfNroPaginas').val());
    var nroActual = parseInt($('#hfPaginaActual').val());

    $('.pag-ini').css('display', 'none');
    $('.pag-item').css('display', 'none');
    $('.pag-fin').css('display', 'none');
    $('.pag-item').removeClass('paginado-activo');

    $('#pag' + nroActual).addClass('paginado-activo');

    if (nroToShow - nroPaginas >= 0) {
        $('.pag-item').css('display', 'block');
        $('.pag-ini').css('display', 'none');
        $('.pag-fin').css('display', 'none');
    }
    else {
        $('.pag-fin').css('display', 'block');
        if (nroActual > 1) {
            $('.pag-ini').css('display', 'block');
        }
        var anterior = 0;
        var siguiente = 0;

        if (nroActual == 1) {

            anterior = 1;
            siguiente = nroToShow;
        }
        else {
            if (nroActual + nroToShow - 1 - nroPaginas > 0) {
                siguiente = nroPaginas;
                anterior = nroPaginas - nroToShow + 1;
            }
            else {
                anterior = nroActual;
                siguiente = nroActual + nroToShow - 1;
            }
        }

        for (j = anterior; j <= siguiente; j++) {
            $('#pag' + j).css('display', 'block')
        }
    }
}

pintarBusqueda = function (nroPagina) {
    $('#hfNroPagina').val(nroPagina);
    cargarLista(nroPagina);
}

function cargarLista(nroPagina) {

    var fechaInicio = $('#txtFechaInicio').val();
    var fechaFin = $('#txtFechaFin').val();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaDesviacionesDemandaPronostico',
        data: { fechaInicio: fechaInicio, fechaFin: fechaFin, nroPagina: nroPagina },
        success: function (aData) {
            $('#listado1').html(aData.Resultado);
            //$('#listado').css("width", $('#mainLayout').width() + "px");
            if (aData.Total > 0) {
                $('#tabla').dataTable({
                    "bSort": false,
                    "scrollY": 630,
                    "scrollX": true,
                    "sDom": 't',
                    "iDisplayLength": -1
                });
            }
            
            pintarPaginado();
            //$('#idGraficoContainer').html('');
            cargarGrafico();
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });



}

function cargarGrafico() {

    var fechaInicio = $('#txtFechaInicio').val();
    var fechaFin = $('#txtFechaFin').val();
    $.ajax({
        type: 'POST',
        url: controlador + 'CargarGraficoDesviacionesPronostico',
        data: { fechaInicio: fechaInicio, fechaFin: fechaFin},
        dataType: 'json',
        success: function (aData) {
            if (aData.Grafico.Series.length > 0) {
                graficoReporteDesviacionesDemanda(aData);                
            }            
        },
        error: function () {
            alert('Ha ocurrido un error');
        }
    });
}

graficoReporteDesviacionesDemanda = function (result) {
    var series = [];
    var series1 = [];
    var series2 = [];

    if (result.Grafico.SerieDataS[0][0] != null) {
        for (k = 0; k < result.Grafico.SerieDataS[0].length; k++) {
            var seriePoint = [];
            var now = parseJsonDate(result.Grafico.SerieDataS[0][k].X);
            var nowUTC = Date.UTC(now.getFullYear(), now.getMonth(), now.getDate(), now.getHours(), now.getMinutes(), now.getSeconds());
            seriePoint.push(nowUTC);
            seriePoint.push(result.Grafico.SerieDataS[0][k].Y);
            series.push(seriePoint);
        }
    }
    if (result.Grafico.SerieDataS[1][0] != null) {
        for (k = 0; k < result.Grafico.SerieDataS[1].length; k++) {
            var seriePoint = [];
            var now = parseJsonDate(result.Grafico.SerieDataS[1][k].X);
            var nowUTC = Date.UTC(now.getFullYear(), now.getMonth(), now.getDate(), now.getHours(), now.getMinutes(), now.getSeconds());
            seriePoint.push(nowUTC);
            seriePoint.push(result.Grafico.SerieDataS[1][k].Y);
            series1.push(seriePoint);
        }
    }
    /*if (result.Grafico.SerieDataS[2][0] != null) {
        for (k = 0; k < result.Grafico.SerieDataS[2].length; k++) {
            var seriePoint = [];
            var now = parseJsonDate(result.Grafico.SerieDataS[2][k].X);
            var nowUTC = Date.UTC(now.getFullYear(), now.getMonth(), now.getDate(), now.getHours(), now.getMinutes(), now.getSeconds());
            seriePoint.push(nowUTC);
            seriePoint.push(result.Grafico.SerieDataS[2][k].Y);
            series2.push(seriePoint);
        }
    }*/

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
   /* if (result.Grafico.SerieDataS[2]) {
        opcion.series.push({
            name: result.Grafico.Series[2].Name,
            color: result.Grafico.Series[2].Color,
            data: series2,
            type: result.Grafico.Series[2].Type
        });
    }*/

    $('#graficos').highcharts('StockChart', opcion);
}
function parseJsonDate(jsonDateString) {
    return new Date(parseInt(jsonDateString.replace('/Date(', '')));
}


function exportarExcelReporte(nroPagina) {

    var fechaInicio = $("#txtFechaInicio").val();
    var fechaFin = $("#txtFechaFin").val();


    $.ajax({
        type: 'POST',
        url: controlador + 'GenerarReporteDesviacionDemandaPronostico',
        data: {

            fechaInicio: fechaInicio,
            fechaFin: fechaFin            
        },
        dataType: 'json',
        success: function (result) {
            if (result == 1) {
                window.location = controlador + "ExportarReporte";
            }
            if (result == -1) {
                alert("Error en reporte result");
            }
            if (result == 2) {
                // Si no existen registros
                alert("No existen registros !");
            }
        },
        error: function () {
            alert("Error en reporte");;
        }

    });

}