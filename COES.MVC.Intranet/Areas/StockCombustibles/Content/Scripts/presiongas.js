var controlador = siteRoot + 'StockCombustibles/Reportes/'
var isListado = true; // variable para determinar si el reporte es listado o grafico
$(function () {
    $('#cbTipoAgente').multipleSelect({
        width: '150px',
        filter: true,
        onClose: function (view) {
            cargarAgentes();
        }
    });
    $('#cbCentralInt').multipleSelect({
        width: '150px',
        filter: true
    });
    $('#cbGeneracion').multipleSelect({
        width: '150px',
        filter: true
    });

    $('#cbAgente').multipleSelect({
        width: '150px',
        filter: true
    });

    //$('#cbParametros').multipleSelect({
    //    width: '150px',
    //    filter: true
    //});
    $('#FechaInicio').Zebra_DatePicker({

    });
    $('#FechaHasta').Zebra_DatePicker({
    });

    $('#btnBuscar').click(function () {
        isListado = true;
        $('#listado').html("");        
        buscarDatos(1);
    });
    $('#btnGrafico').click(function () {                
        isListado = false;
        generarGrafico(1);
        mostrarPaginado2();

    });
    $('#btnExpotar').click(function () {
        exportarExcel();

    });
    cargarPrevio();
    cargarAgentes();
    buscarDatos(1);

});

function cargarPrevio() {
    $('#cbTipoAgente').multipleSelect('checkAll');
    $('#cbCentralInt').multipleSelect('checkAll');
    $('#cbParametros').multipleSelect('checkAll');
}

function buscarDatos(nPagina) {    
    generarListado(nPagina);
    mostrarPaginado2();
}

function pintarBusqueda(nPagina) {
    if (isListado) // si es reporte listado
        generarListado(nPagina);
    else{
        generarGrafico(nPagina);
        mostrarPaginado2();
    }

}

function cargarAgentes() {
    var tipoAgente = $('#cbTipoAgente').multipleSelect('getSelects');

    if (tipoAgente == "[object Object]") tipoAgente = "-1";
    $('#hfTipoAgente').val(tipoAgente);
    $.ajax({
        type: 'POST',
        url: controlador + 'CargarAgentes',

        data: { idTipoAgente: $('#hfTipoAgente').val() },

        success: function (aData) {
            $('#agentes').html(aData);
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function generarListado(nPagina) {
    var tipoAgente = $('#cbTipoAgente').multipleSelect('getSelects');
    if (tipoAgente == "[object Object]") tipoAgente = -1;
    if (tipoAgente == "") tipoAgente = "-1";

    var centralInt = $('#cbCentralInt').multipleSelect('getSelects');
    if (centralInt == "[object Object]") centralInt = -1;
    if (centralInt == "") centralInt = "-1";


    var agente = $('#cbAgente').multipleSelect('getSelects');
    if (agente == "[object Object]") agente = "-1";
    if (agente == "") agente = "-1";

    var parametro = $('#cbParametro').val();

    var fechaInicio = $('#FechaInicio').val();
    var fechaFin = $('#FechaHasta').val();


    $('#hfTipoAgente').val(tipoAgente);
    $('#hfCentralInt').val(centralInt);
    $('#hfAgente').val(agente);
    $('#hfParametro').val(parametro);
    $.ajax({
        type: 'POST',
        url: controlador + "listaPresionGasTemperatura",
        data: {
            idsTipoAgente: $('#hfTipoAgente').val(),
            idsCentralInt: $('#hfCentralInt').val(),
            idsAgente: $('#hfAgente').val(),
            idParametro: $('#hfParametro').val(),
            idsFechaIni: fechaInicio,
            idsFechaFin: fechaFin,
            nroPagina: nPagina
        },
        success: function (evt) {            
            $('#listado').css("width", $('#mainLayout').width() + "px");

            $('#listado').html(evt);
            if ($('#tabla th').length > 2) {
                $('#tabla').dataTable({
                    //  "aoColumns": aoColumns(),
                    "bAutoWidth": false,
                    "bSort": false,
                    "scrollY": 430,
                    "scrollX": true,
                    "sDom": 't',
                    "iDisplayLength": 100
                });
            }


        },
        error: function () {
            alert("Ha ocurrido un error en lista");
        }
    });

}

function generarGrafico(nPagina) {
    $('#reporte').css("display", "none");
    //$('#paginado').css("display", "none");       
    $('#excelGrafico').css("display", "block");
    $('#excelGrafico').html("<img onclick='exportarGrafico();' width='32px' style='cursor: pointer; display: inline;' src='" + siteRoot + "Content/Images/ExportExcel.png' />");
    $('#graficos').css("display", "block");
    var tipoAgente = $('#cbTipoAgente').multipleSelect('getSelects');
    if (tipoAgente == "[object Object]") tipoAgente = -1;
    if (tipoAgente == "") tipoAgente = "-1";

    var centralInt = $('#cbCentralInt').multipleSelect('getSelects');
    if (centralInt == "[object Object]") centralInt = -1;
    if (centralInt == "") centralInt = "-1";


    var agente = $('#cbAgente').multipleSelect('getSelects');
    if (agente == "[object Object]") agente = "-1";
    if (agente == "") agente = "-1";

    var parametro = $('#cbParametro').val();

    var fechaInicio = $('#FechaInicio').val();
    var fechaFin = $('#FechaHasta').val();


    $('#hfTipoAgente').val(tipoAgente);
    $('#hfCentralInt').val(centralInt);
    $('#hfAgente').val(agente);
    $('#hfParametro').val(parametro);
    $.ajax({
        type: 'POST',
        url: controlador + "GraficoRptPresionGasTemperatura",
        data: {
            idsTipoAgente: $('#hfTipoAgente').val(),
            idsCentralInt: $('#hfCentralInt').val(),
            idsAgente: $('#hfAgente').val(),
            idParametro: $('#hfParametro').val(),
            idsFechaIni: fechaInicio,
            idsFechaFin: fechaFin,
            nroPagina: nPagina
        },
        dataType: 'json',
        success: function (result) {
            if (result.Grafico.SeriesName.length > 0) {// si existen registros
                graficoPresionGasTemperatura(result);
            }
            else {// No existen registros
                $('#excelGrafico').html("No existen registros !");
                $('#graficos').css("display", "none");
            }
        },
        error: function () {
            alert("Ha ocurrido un error en generar grafico");
        }
    });
}

function graficoPresionGasTemperatura(result) {
    //result.Grafico.subtitleText = "Sub Title Presion Gas";

    var opcion = {
        chart: {
            type: 'spline'
        },
        title: {
            text: result.Grafico.TitleText,
        },
        subtitle: {
            text: result.Grafico.subtitleText,
        },
        xAxis: {

            categories: result.Grafico.XAxisCategories,
            style: {

                fontSize: '3'
            },

            title: {
                text: result.Grafico.XAxisTitle,
            },
        },
        yAxis: {
            title: {
                text: result.Grafico.YaxixTitle,
            },
            min: 0
        },
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
    for (var i in result.Grafico.SeriesName) {
        opcion.series.push({
            showInLegend: true,
            name: result.Grafico.SeriesName[i],
            data: result.Grafico.SeriesData[i]
            //type: result.GrafHightcharts.seriesType[i],
            //yAxis: result.GrafHightcharts.seriesYAxis[i]
        });
    }
    $('#graficos').highcharts(opcion);

}

function mostrarPaginado2() {

    $.ajax({
        type: 'POST',
        url: controlador + 'paginado',
        success: function (evt) {
            $('#paginado').html(evt);
            mostrarPaginado();
        },
        error: function () {
            alert("error en paginado");
        }
    });
}

function exportarExcel() {
    var tipoAgente = $('#cbTipoAgente').multipleSelect('getSelects');
    if (tipoAgente == "[object Object]") tipoAgente = -1;
    if (tipoAgente == "") tipoAgente = "-1";

    var centralInt = $('#cbCentralInt').multipleSelect('getSelects');
    if (centralInt == "[object Object]") centralInt = -1;
    if (centralInt == "") centralInt = "-1";


    var agente = $('#cbAgente').multipleSelect('getSelects');
    if (agente == "[object Object]") agente = "-1";
    if (agente == "") agente = "-1";

    var parametro = $('#cbParametro').val();

    var fechaInicio = $('#FechaInicio').val();
    var fechaFin = $('#FechaHasta').val();


    $('#hfTipoAgente').val(tipoAgente);
    $('#hfCentralInt').val(centralInt);
    $('#hfAgente').val(agente);
    $('#hfParametro').val(parametro);

    $.ajax({
        type: 'POST',
        url: controlador + 'GenerarRptExcelPresTemp',
        data: {
            idsTipoAgente: $('#hfTipoAgente').val(),
            idsCentralInt: $('#hfCentralInt').val(),
            idsAgente: $('#hfAgente').val(),
            idParametro: $('#hfParametro').val(),
            idsFechaIni: fechaInicio,
            idsFechaFin: fechaFin
        },
        dataType: 'json',
        success: function (result) {
            if (result == 1) {//
                window.location = controlador + "ExportarReporte?tipo=3";
            }
            if (result == -1) {
                alert("Error en reporte result")
            }
            if (result == 2) // NO existen registros
                alert("No existen registros !");
        },
        error: function () {
            alert("Error en reporte");;
        }
    });
}

function exportarGrafico() {
    var parametro = $('#cbParametro').val();
    $('#hfParametro').val(parametro);
    $.ajax({
        type: 'POST',
        url: controlador + "GenerarArchivoGraficoPresTemp",
        data: {
            //fechaInicial: $('#hfFecha').val(), fechaFinal: $('#hfFechaHasta').val()
            idParametro: $('#hfParametro').val(),
        },
        dataType: 'json',
        success: function (result) {
            if (result == 1) {
                window.location = controlador + "ExportarReporte?tipo=6";
            }
            if (result == -1) {
                alert("Error en generar datos archivo grafico !");
            }
        },
        error: function () {
            alert("Error en exportar grafico a Excel");
        }
    });
}