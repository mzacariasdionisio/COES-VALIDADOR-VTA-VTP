var controlador = siteRoot + 'StockCombustibles/Reportes/'
var tipoReporte = 1; // 1: Reporte Stock de Combustible, 2: Reporte Recepcion Acumulada
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

    $('#cbCentral').multipleSelect({
        width: '150px',
        filter: true
    });
    $('#cbRecurso').multipleSelect({
        width: '150px',
        filter: true
    });

    $('#cbAgente').multipleSelect({
        width: '150px',
        filter: true
    });

    $('#cbEstado').multipleSelect({
        width: '150px',
        filter: true,
        onClose: function (view) {
            cargarRecursoEnergetico();
        }
    });

    $('#FechaInicio').Zebra_DatePicker({
        pair: $('#FechaHasta'),
        onSelect: function (date) {
            var date1 = getFecha(date);
            var date2 = getFecha($('#FechaHasta').val());

            if (date1 > date2) {
                $('#FechaHasta').val(date);
            }
        }
    });
    $('#FechaHasta').Zebra_DatePicker({
        direction: true
    });

    $('#btnBuscar').click(function () {
        $('#listado').html("");
        $('#paginado').html("");
        $('#btnExpotar').show();
        $('#btnGrafico').show()
        tipoReporte = 1;
        buscarDatos();
        $('#idRecepcion').css('display', 'table-row');
    });

    $('#btnBuscarAcum').click(function () {
        tipoReporte = 2;
        $('#btnGrafico').hide();
        $('#btnExpotar').show();
        mostrarListadoAcum();
        $('#idRecepcion').css('display', 'none');
    });


    $('#btnGrafico').click(function () {
        $('#btnExpotar').hide();
        generarGrafico(1);


    });

    $('#btnExpotar').click(function () {
        exportarExcelReporte(tipoReporte);
    });

    cargarPrevio();
    cargarAgentes();

    cargarRecursoEnergetico();
    buscarDatos();

});

function cargarPrevio() {
    $('#cbTipoAgente').multipleSelect('checkAll');
    $('#cbCentralInt').multipleSelect('checkAll');
    $('#cbCentral').multipleSelect('checkAll');
    $('#cbRecurso').multipleSelect('checkAll');
    $('#cbAgente').multipleSelect('checkAll');
    $('#cbEstado').multipleSelect('checkAll');
    $('#btnExpotarAcum').hide();
}

function buscarDatos() {
    pintarPaginado();
    mostrarListado(1);
}

function pintarPaginado() {
    var tipoAgente = $('#cbTipoAgente').multipleSelect('getSelects');
    if (tipoAgente == "[object Object]") tipoAgente = -1;
    if (tipoAgente == "") tipoAgente = "-1";

    var centralInt = $('#cbCentralInt').multipleSelect('getSelects');
    if (centralInt == "[object Object]") centralInt = -1;
    if (centralInt == "") centralInt = "-1";

    var central = $('#cbCentral').multipleSelect('getSelects');
    if (central == "[object Object]") central = -1;
    if (central == "") central = "-1";

    var recurso = $('#cbRecurso').multipleSelect('getSelects');
    if (recurso == "[object Object]") recurso = "-1";
    if (recurso == "") recurso = "-1";

    var agente = $('#cbAgente').multipleSelect('getSelects');
    if (agente == "[object Object]") agente = "-1";
    if (agente == "") agente = "-1";

    var estado = $('#cbEstado').multipleSelect('getSelects');
    if (estado == "[object Object]") estado = "-1";
    if (estado == "") estado = "-1";

    var fechaInicio = $('#FechaInicio').val();
    var fechaFin = $('#FechaHasta').val();


    $('#hfTipoAgente').val(tipoAgente);
    $('#hfCentralInt').val(centralInt);
    $('#hfRecurso').val(recurso);
    $('#hfAgente').val(agente);
    $('#hfEstado').val(estado);
    $('#hfCentral').val(central);
    $.ajax({
        type: 'POST',
        url: controlador + "PaginadoStock",
        data: {
            idsTipoAgente: $('#hfTipoAgente').val(),
            idsCentralInt: $('#hfCentralInt').val(),
            idsRecurso: $('#hfRecurso').val(),
            idsAgente: $('#hfAgente').val(),
            idsEstado: $('#hfEstado').val(),
            idsFechaIni: fechaInicio,
            idsFechaFin: fechaFin,
            idsEquipo: $('#hfCentral').val()
        },
        success: function (evt) {
            $('#paginado').html(evt);
            mostrarPaginado();
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function pintarBusqueda(nPagina) {
    mostrarListado(nPagina);
}

function mostrarListado(nPagina) {
    var tipoAgente = $('#cbTipoAgente').multipleSelect('getSelects');
    if (tipoAgente == "[object Object]") tipoAgente = -1;
    if (tipoAgente == "") tipoAgente = "-1";

    var centralInt = $('#cbCentralInt').multipleSelect('getSelects');
    if (centralInt == "[object Object]") centralInt = -1;
    if (centralInt == "") centralInt = "-1";

    var central = $('#cbCentral').multipleSelect('getSelects');
    if (central == "[object Object]") central = -1;
    if (central == "") central = "-1";

    var recurso = $('#cbRecurso').multipleSelect('getSelects');
    if (recurso == "[object Object]") recurso = "-1";
    if (recurso == "") recurso = "-1";

    var agente = $('#cbAgente').multipleSelect('getSelects');
    if (agente == "[object Object]") agente = "-1";
    if (agente == "") agente = "-1";

    var estado = $('#cbEstado').multipleSelect('getSelects');
    if (estado == "[object Object]") estado = "-1";
    if (estado == "") estado = "-1";

    var fechaInicio = $('#FechaInicio').val();
    var fechaFin = $('#FechaHasta').val();


    $('#hfTipoAgente').val(tipoAgente);
    $('#hfCentralInt').val(centralInt);
    $('#hfRecurso').val(recurso);
    $('#hfAgente').val(agente);
    $('#hfEstado').val(estado);
    $('#hfCentral').val(central);
    $.ajax({
        type: 'POST',
        url: controlador + "listaStockCombustible",
        data: {
            idsTipoAgente: $('#hfTipoAgente').val(),
            idsCentralInt: $('#hfCentralInt').val(),
            idsRecurso: $('#hfRecurso').val(),
            idsAgente: $('#hfAgente').val(),
            idsEstado: $('#hfEstado').val(),
            idsFechaIni: fechaInicio,
            idsFechaFin: fechaFin,
            idsEquipo: $('#hfCentral').val(),
            nroPagina: nPagina
        },
        success: function (evt) {
            var ancho = (parseInt($('#mainLayout').width()) - 20) + "px";
            $('#listado').css("width", ancho);
            $('#listado').html(evt);

            $("#reporte").css("overflow", "auto");
            $("#reporte").css("width", "auto");
            $("#reporte").css("height", "auto");

            if(parseInt($('#mainLayout').width()) - 20 > 1500){
                $("#tabla").css("width", ancho);
            }
        },
        error: function () {
            alert("Ha ocurrido un error en lista");
        }
    });
}

function mostrarListadoAcum() {
    var tipoAgente = $('#cbTipoAgente').multipleSelect('getSelects');
    if (tipoAgente == "[object Object]") tipoAgente = -1;
    if (tipoAgente == "") tipoAgente = "-1";

    var centralInt = $('#cbCentralInt').multipleSelect('getSelects');
    if (centralInt == "[object Object]") centralInt = -1;
    if (centralInt == "") centralInt = "-1";

    var central = $('#cbCentral').multipleSelect('getSelects');
    if (central == "[object Object]") central = -1;
    if (central == "") central = "-1";

    var recurso = $('#cbRecurso').multipleSelect('getSelects');
    if (recurso == "[object Object]") recurso = "-1";
    if (recurso == "") recurso = "-1";

    var agente = $('#cbAgente').multipleSelect('getSelects');
    if (agente == "[object Object]") agente = "-1";
    if (agente == "") agente = "-1";

    var estado = $('#cbEstado').multipleSelect('getSelects');
    if (estado == "[object Object]") estado = "-1";
    if (estado == "") estado = "-1";

    var fechaInicio = $('#FechaInicio').val();
    var fechaFin = $('#FechaHasta').val();


    $('#hfTipoAgente').val(tipoAgente);
    $('#hfCentralInt').val(centralInt);
    $('#hfRecurso').val(recurso);
    $('#hfAgente').val(agente);
    $('#hfEstado').val(estado);
    $('#hfCentral').val(central);
    $('#paginado').html("");
    $.ajax({
        type: 'POST',
        url: controlador + "listaAcumuladoCombustible",
        data: {
            idsTipoAgente: $('#hfTipoAgente').val(),
            idsCentralInt: $('#hfCentralInt').val(),
            idsRecurso: $('#hfRecurso').val(),
            idsAgente: $('#hfAgente').val(),
            idsEstado: $('#hfEstado').val(),
            idsFechaIni: fechaInicio,
            idsFechaFin: fechaFin,
            idsEquipo: $('#hfCentral').val()
        },
        success: function (evt) {
            $('#listado').css("width", $('#mainLayout').width() + "px");

            $('#listado').html(evt);

            $('#tabla').dataTable({
                "bAutoWidth": false,
                "bSort": false,
                "scrollY": 430,
                "scrollX": true,
                "sDom": 't',
                "ordering": true,
                "iDisplayLength": 50
            });

        },
        error: function () {
            alert("Ha ocurrido un error en lista");
        }
    });
}

function generarGrafico(op) {
    $('#reporte').css("display", "none");
    //////$('#paginado').css("display", "none");       
    $('#excelGrafico').css("display", "block");
    $('#excelGrafico').html("<img onclick='exportarGrafico();' width='32px' style='cursor: pointer; display: inline;' src='" + siteRoot + "Content/Images/ExportExcel.png' />");
    $('#graficos').css("display", "block");

    var tipoAgente = $('#cbTipoAgente').multipleSelect('getSelects');
    if (tipoAgente == "[object Object]") tipoAgente = -1;
    if (tipoAgente == "") tipoAgente = "-1";

    var centralInt = $('#cbCentralInt').multipleSelect('getSelects');
    if (centralInt == "[object Object]") centralInt = -1;
    if (centralInt == "") centralInt = "-1";

    var central = $('#cbCentral').multipleSelect('getSelects');
    if (central == "[object Object]") central = -1;
    if (central == "") central = "-1";

    var recurso = $('#cbRecurso').multipleSelect('getSelects');
    if (recurso == "[object Object]") recurso = "-1";
    if (recurso == "") recurso = "-1";

    var agente = $('#cbAgente').multipleSelect('getSelects');
    if (agente == "[object Object]") agente = "-1";
    if (agente == "") agente = "-1";

    var estado = $('#cbEstado').multipleSelect('getSelects');
    if (estado == "[object Object]") estado = "-1";
    if (estado == "") estado = "-1";

    var fechaInicio = $('#FechaInicio').val();
    var fechaFin = $('#FechaHasta').val();


    $('#hfTipoAgente').val(tipoAgente);
    $('#hfCentralInt').val(centralInt);
    $('#hfRecurso').val(recurso);
    $('#hfAgente').val(agente);
    $('#hfEstado').val(estado);
    $('#hfFecha').val(fechaInicio);
    $('#hfFechaHasta').val(fechaFin);

    $.ajax({
        type: 'POST',
        url: controlador + "GraficoReporteStock",
        data: {
            idsTipoAgente: $('#hfTipoAgente').val(),
            idsCentralInt: $('#hfCentralInt').val(),
            idsRecurso: $('#hfRecurso').val(),
            idsAgente: $('#hfAgente').val(),
            idsEstado: $('#hfEstado').val(),
            idsFechaIni: fechaInicio,
            idsFechaFin: fechaFin,
            idsEquipo: central
        },
        dataType: 'json',
        success: function (result) {
            if (result.Grafico.Series.length > 0) {// si existen registros
                //if (result.Grafico.SerieDataS[0].length > 0) {
                switch (parseInt(op)) {
                    case 1:// Gráfico Stock Evolución
                        graficoStock(result, op);
                        break;
                    case 2: //Gráfico Recepción Evolucion
                        graficoStock(result, op);
                        break;
                    default:  // Gráfico Recepción Acumulado
                        graficoStockBarra(result);
                        break;
                }
            }
            else {
                $('#graficos').css("display", "none");
                $('#excelGrafico').html("No existen registros !");
                //alert("No existen registros !");
            }
        },
        error: function () {
            alert("Ha ocurrido un error en generar grafico");
        }
    });
}

function graficoStock(result, opc) {
    var series = [[]];
    for (i = 0; i < result.Grafico.Series.length; i++) {
        series[i] = [];
        for (k = 0; k < result.Grafico.SerieDataS[i].length; k++) {
            var seriePoint = [];
            var now = parseJsonDate(result.Grafico.SerieDataS[i][k].X);
            var nowUTC = Date.UTC(now.getFullYear(), now.getMonth(), now.getDate(), now.getHours(), now.getMinutes(), now.getSeconds());

            seriePoint.push(nowUTC);
            if (opc == 1)//sTOCK
                seriePoint.push(result.Grafico.SerieDataS[i][k].Y);
            else
                seriePoint.push(result.Grafico.SerieDataS[i][k].Z);
            series[i].push(seriePoint);
        }
    }

    if (opc == 1)
        titleText = "STOCK" + result.Grafico.TitleText;
    else
        titleText = "RECEPCION" + result.Grafico.TitleText;
    var opcion = {
        rangeSelector: {
            selected: 1
        },

        title: {
            text: titleText,
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


    for (i = 0; i < result.Grafico.Series.length; i++) {
        opcion.series.push({
            name: result.Grafico.Series[i].Name,
            //color: result.Grafico.Series[0].Color,
            data: series[i],
            //type: result.Grafico.Series[i].Type
        });
    }
    if (opc == 1) // stock
        $('#stock').highcharts('StockChart', opcion);
    else // recepción evolucion    
        $('#recep1').highcharts('StockChart', opcion);
}

////GRAFICO TIPO column
function graficoStockBarra(result) {
    var valor = [[]];
    var serieName = []; //nombre de las centrales
    var fechas = [];
    var titleText = "";
    var mes = "";
    for (j = 0; j < result.Grafico.Series.length; j++) {
        serieName[j] = result.Grafico.Series[j].Name;
    }

    for (i = 0; i < result.Grafico.SerieDataS[0].length; i++) {
        valor[i] = [];
        var now = parseJsonDate(result.Grafico.SerieDataS[0][i].X);
        diaActual = now.getDate();
        diaActual = (diaActual < 9) ? '0' + diaActual : diaActual;
        mesActual = nombreMes(now.getMonth());
        diaActual = diaActual + mesActual + now.getFullYear();
        fechas.push(diaActual);
        //fechas.push(diaActual.concat(mesActual, now.getFullYear()));
    }
    titleText = "RECECPCIÓN" + result.Grafico.TitleText;
    for (k = 0; k < result.Grafico.Series.length; k++) {

        for (z = 0; z < result.Grafico.SerieDataS[k].length; z++) {
            valor[z].push(result.Grafico.SerieDataS[k][z].Z);
        }
    }

    var opcion = {
        chart: {
            type: 'column'
        },
        title: {
            text: titleText
        },
        xAxis: {
            categories: serieName
        },
        yAxis: {
            min: 0,
            title: {
                text: 'Total stock de combustible'
            },
            stackLabels: {
                enabled: true,
                style: {
                    fontWeight: 'bold',
                    color: (Highcharts.theme && Highcharts.theme.textColor) || 'gray'
                }
            }
        },
        legend: {
            layout: 'horizontal',
            align: 'center',
            verticalAlign: 'bottom',
            enabled: true,

            backgroundColor: (Highcharts.theme && Highcharts.theme.background2) || 'white',
            borderColor: '#CCC',
            borderWidth: 1,
            shadow: false,

        },
        tooltip: {
            headerFormat: '<b>{point.x}</b><br/>',
            pointFormat: '{series.name}: {point.y}<br/>Total: {point.stackTotal}'
        },
        plotOptions: {
            column: {
                stacking: 'normal',
                dataLabels: {
                    enabled: true,
                    color: (Highcharts.theme && Highcharts.theme.dataLabelsColor) || 'white',
                    style: {
                        textShadow: '0 0 3px black'
                    }
                }
            }
        },
        series: []
    };

    for (i = 0; i < fechas.length; i++) {
        opcion.series.push({
            name: fechas[i],
            data: valor[i]
        });
    }

    $('#recep').highcharts(opcion);

}

function parseJsonDate(jsonDateString) {
    return new Date(parseInt(jsonDateString.replace('/Date(', '')));
}

function mostrarPaginado() {
    $.ajax({
        type: 'POST',
        url: controlador + 'Paginado',
        success: function (evt) {
            $('#paginado').html(evt);
            mostrarPaginado();
        },
        error: function () {
            alert("Error paginado");
        }
    });
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
            cargarCentral();
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function cargarCentral() {
    var agente = $('#cbAgente').multipleSelect('getSelects');
    if (agente == "[object Object]") agente = "-1";

    $('#hfAgente').val(agente);
    $.ajax({
        type: 'POST',
        url: controlador + 'CargarCentrales',

        data: { idsAgente: $('#hfAgente').val() },

        success: function (aData) {
            $('#central').html(aData);
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function cargarRecursoEnergetico() {
    var estadoFisico = $('#cbEstado').multipleSelect('getSelects');
    var codReporte = 1;
    if (estadoFisico == "[object Object]") estadoFisico = "-1";
    $('#hfEstado').val(estadoFisico);
    $.ajax({
        type: 'POST',
        url: controlador + 'CargarRecursoEnergetico',

        data: {
            idEstadoFisico: $('#hfEstado').val(),
            iCodReporte: codReporte
        },

        success: function (aData) {
            $('#recurso').html(aData);
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });

}

function exportarExcelReporte(tipo) {
    var tipoAgente = $('#cbTipoAgente').multipleSelect('getSelects');
    if (tipoAgente == "[object Object]") tipoAgente = -1;
    if (tipoAgente == "") tipoAgente = "-1";

    var centralInt = $('#cbCentralInt').multipleSelect('getSelects');
    if (centralInt == "[object Object]") centralInt = -1;
    if (centralInt == "") centralInt = "-1";

    var central = $('#cbCentral').multipleSelect('getSelects');
    if (central == "[object Object]") central = -1;
    if (central == "") central = "-1";

    var recurso = $('#cbRecurso').multipleSelect('getSelects');
    if (recurso == "[object Object]") recurso = "-1";
    if (recurso == "") recurso = "-1";

    var agente = $('#cbAgente').multipleSelect('getSelects');
    if (agente == "[object Object]") agente = "-1";
    if (agente == "") agente = "-1";

    var estado = $('#cbEstado').multipleSelect('getSelects');
    if (estado == "[object Object]") estado = "-1";
    if (estado == "") estado = "-1";

    var fechaInicio = $('#FechaInicio').val();
    var fechaFin = $('#FechaHasta').val();


    $('#hfTipoAgente').val(tipoAgente);
    $('#hfCentralInt').val(centralInt);
    $('#hfRecurso').val(recurso);
    $('#hfAgente').val(agente);
    $('#hfEstado').val(estado);
    $('#hfCentral').val(central);

    $.ajax({
        type: 'POST',
        url: controlador + 'GenerarArchivoReporteXLS',
        data: {
            idsTipoAgente: $('#hfTipoAgente').val(),
            idsCentralInt: $('#hfCentralInt').val(),
            idsRecurso: $('#hfRecurso').val(),
            idsAgente: $('#hfAgente').val(),
            idsEstado: $('#hfEstado').val(),
            idsFechaIni: fechaInicio,
            idsFechaFin: fechaFin,
            iTipoReporte: tipo,
            idsEquipo: $('#hfCentral').val()
        },
        dataType: 'json',
        success: function (result) {
            if (result == 1) {//
                switch (tipo) {
                    case 1: //Exporta  reporte stock de combustible
                        window.location = controlador + "ExportarReporte?tipo=1";
                        break;
                    case 2: //exporta reporte recepción acumulada
                        window.location = controlador + "ExportarReporte?tipo=9";
                        break;
                    case 3:
                        window.location = controlador + "ExportarReporte?tipo=10";
                        break;
                }
            }
            if (result == -1) {
                alert("Error en reporte result")
            }
            if (result == 2) { // No existen registros
                alert("No existen registros !");
            }
        },
        error: function () {
            alert("Error en reporte");;
        }
    });
}

function exportarGrafico() {
    $.ajax({
        type: 'POST',
        url: controlador + "GenerarArchivoGraficoStock",
        data: {
            fechaInicial: $('#hfFecha').val(),
            fechaFinal: $('#hfFechaHasta').val()
        },
        dataType: 'json',
        success: function (result) {
            if (result == 1) {
                window.location = controlador + "ExportarReporte?tipo=4";
            }
            if (result == -1) {
                alert("Error al exportar datos !");
            }
            if (result == 2) { //No existen registros
                alert("No existen registros !");
            }
        },
        error: function () {
            alert("Error en exportar grafico a Excel");
        }
    });
}

function nombreMes(nmes) {
    mes = new Array("Ene", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Sep", "Oct", "Nov", "Dic");
    return mes[nmes]
}

function popupDetalleAcumulado(id) {
    $('#hPtoMediCodi').val(id);
    $.ajax({
        type: 'POST',
        url: controlador + "VerDetalleAcumulado",
        data: { ptomedicodi: $('#hPtoMediCodi').val() },
        success: function (evt) {
            $('#DetalleAcumulado').html(evt);
            setTimeout(function () {
                $('#popupaDetAcumulado').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown'
                });
            }, 50);
        },
        error: function () {
            alert("Ha ocurrido un error en detalle recepción de combustibles acumulados");
        }
    });
}

function cancelarDetalleAcumulado() {
    $('#popupaDetAcumulado').bPopup().close();
}