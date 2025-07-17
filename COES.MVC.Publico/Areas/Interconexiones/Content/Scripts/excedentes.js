var controlador = siteRoot + 'Interconexiones/';

$(function () {
       
    $('#FechaDesde').Zebra_DatePicker({

    });

    $('#FechaHasta').Zebra_DatePicker({

    });
    
    $('#btnBuscar').click(function () {

        buscarDatos();
    });   

    $('#btnGrafico').click(function () {
  
        generarGrafico();
    });

    $('#btnExpotar').click(function () {
        exportarExcel();
    });

    $('#btnZip').click(function () {
        abrirInfoZip();
    });

    buscarDatos();   
});

function abrirInfoZip(archivo) {
    document.location = controlador + 'reportes/excedenteszip/';
}


function buscarDatos() {
  
    mostrarListado();
}

function mostrarListado() {
    var version = $('#cbVersion').val();
    var horizonte = $('#cbHorizonte').val();
   
    $('#hfVersion').val(version);
    $('#hfHorizonte').val(horizonte);
   
    $.ajax({
        type: 'POST',
        url: controlador + "reportes/listaexcedentes",
        data: {
            idVersion: $('#hfVersion').val(),
            idHorizonte: $('#hfHorizonte').val(),
            fechaInicial: $('#FechaDesde').val(),
            fechaFinal: $('#FechaHasta').val()            
        },
        success: function (evt) {
            $('#listado').css("width", $('#mainLayout').width() + "px");

            $('#listado').html(evt);
            if ($('#tabla th').length > 1) {
                $('#tabla').dataTable({
                    "aoColumns": aoColumns(),
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
    
}

function aoColumns() {
    var ao = [];
    $("#tabla th").each(function (i, th) {
        switch (i) {
            case 0:
                ao.push({ "sWidth": "70px" });
                break;
            default:
                ao.push({ "sWidth": "100px" });
                break;
        }
    });
    return ao;
}

function generarGrafico() {
       
    var fechadesde = $('#FechaDesde').val();
    var fechahasta = $('#FechaHasta').val();   
    var version = $('#cbVersion').val();
    var horizonte = $('#cbHorizonte').val();

    $('#hfVersion').val(version);
    $('#hfHorizonte').val(horizonte);
    $('#hfFechaDesde').val(fechadesde);
    $('#hfFechaHasta').val(fechahasta);
    

    $.ajax({
        type: 'POST',
        url: controlador + "reportes/generagraficoexcedente",
        data: {
            idVersion: $('#hfVersion').val(),
            idHorizonte: $('#hfHorizonte').val(),
            fechaInicial: $('#FechaDesde').val(),
            fechaFinal: $('#FechaHasta').val()
        },
        dataType: 'json',
        success: function (result) {
            if (result.Grafico.SerieDataS[0].length > 0) {
                graficoRepImpoExpSt(result);
            }
        },
        error: function () {
            alert("Ha ocurrido un error en generar grafico");
        }
    });
}

graficoRepImpoExpSt = function (result) {
    var series = [];
    

    for (k = 0; k < result.Grafico.SerieDataS[0].length; k++) {
        var seriePoint = [];
        var now = parseJsonDate(result.Grafico.SerieDataS[0][k].X);
        var nowUTC = Date.UTC(now.getFullYear(), now.getMonth(), now.getDate(), now.getHours(), now.getMinutes(), now.getSeconds());
        seriePoint.push(nowUTC);
        seriePoint.push(result.Grafico.SerieDataS[0][k].Y);
        series.push(seriePoint);
    }
    var opcion = {
        rangeSelector: {
            selected: 1
        },

        title: {
            text: result.Grafico.titleText
        },
        yAxis: [{
            title: {
                text: "MW"
            },
            min: 0
            }
       ],
        legend: {
            layout: 'horizontal',
            align: 'center',
            verticalAlign: 'bottom',
            borderWidth: 0,
            enabled: true
        },
        series: [{
            name: result.Grafico.SeriesName[0],
            data: series,
            
        }]
    };

    $('#graficos').highcharts('StockChart', opcion);
}

function exportarExcel() {
    var fechadesde = $('#FechaDesde').val();
    var fechahasta = $('#FechaHasta').val();
    var version = $('#cbVersion').val();
    var horizonte = $('#cbHorizonte').val();

    $('#hfVersion').val(version);
    $('#hfHorizonte').val(horizonte);
    $('#hfFechaDesde').val(fechadesde);
    $('#hfFechaHasta').val(fechahasta);


    $.ajax({
        type: 'POST',
        url: controlador + "reportes/GenerarArchivoExcedente",
        data: {
            idVersion: $('#hfVersion').val(),
            idHorizonte: $('#hfHorizonte').val(),
            fechaInicial: $('#FechaDesde').val(),
            fechaFinal: $('#FechaHasta').val()
        },
        dataType: 'json',
        success: function (result) {
            if (result == 1) {
                window.location = controlador + "reportes/ExportarReporte?tipo=1";
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

function parseJsonDate(jsonDateString) {
    return new Date(parseInt(jsonDateString.replace('/Date(', '')));
}