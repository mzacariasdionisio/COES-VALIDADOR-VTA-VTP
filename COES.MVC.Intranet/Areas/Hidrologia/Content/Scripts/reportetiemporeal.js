var controlador = siteRoot + 'hidrologia/';

$(function () {
    $('#cbEmpresa').multipleSelect({
        width: '222px',
        filter: true
    });
    $('#cbUnidades').multipleSelect({
        width: '222px',
        filter: true
    });
    $('#Fecha').Zebra_DatePicker({

    });
    $('#FechaHasta').Zebra_DatePicker({

    });
    $('#Anho').Zebra_DatePicker({
        format: 'Y',
        onSelect: function () {
            cargarSemanaAnho()
        }
    });

    $('#btnBuscar').click(function () {
        $('#listado').html("");
        $('#btnExpotar').show();
        buscarDatosTiempoReal();
    });
    $('#btnGrafico').click(function () {
        $('#paginado').html("");
        generarGraficoTiempoReal(1);
        $('#btnExpotar').hide();

    });
    $('#btnExpotar').click(function () {
        exportarExcelTR();

    });
    cargarPrevio();
    //buscarDatos();

});

function cargarPrevio() {
    $('#cbEmpresa').multipleSelect('checkAll');
    $('#cbUnidades').multipleSelect('checkAll');
    $('input[name=rbidTipo][value=2]').attr('checked', true);

}

function buscarDatosTiempoReal() {
    pintarPaginado(1);
    mostrarListadoTiempoReal(1);
}

function pintarPaginado(id) { //id : tipo de reporte a mostrar 0: reporte grafico, 1: reporte listado
    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    if (empresa == "[object Object]") empresa = "-1";
    if (empresa == "") empresa = "-1";
    var tipopto = $('#cbUnidades').multipleSelect('getSelects');
    if (tipopto == "[object Object]") tipopto = "-1";
    if (tipopto == "") tipopto = "-1";

    var valor = $("input[name='rbidTipo']:checked").val();
    // $('#hfidTipo').val(valor.toString());
    $('#hfEmpresa').val(empresa);
    $('#hfUnidad').val(tipopto);
    $.ajax({
        type: 'POST',
        url: controlador + "reporte/PaginadoTiempoReal",
        data: {
            idsEmpresa: $('#hfEmpresa').val(),
            // tipoReporte: $('#hfidTipo').val(),
            fecha: $('#Fecha').val(),
            fechaFinal: $('#FechaHasta').val(),
            idsTipoPtoMed: $('#hfUnidad').val()
        },
        success: function (evt) {
            $('#paginado').html(evt);
            mostrarPaginado(id);
        },
        error: function () {
            alert("Ha ocurrido un error paginado");
        }
    });
}

function pintarBusqueda(nroPagina, itipo) {
    //itipo : tipo de reporte a mostrar 0: reporte grafico, 1: reporte listado

    if (itipo == 0) {
        generarGraficoTiempoReal(nroPagina);
    }
    else {
        mostrarListadoTiempoReal(nroPagina);
    }
}

function mostrarListadoTiempoReal(nroPagina) {
    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    if (empresa == "[object Object]") empresa = "-1";
    if (empresa == "") empresa = "-1";
    var tipopto = $('#cbUnidades').multipleSelect('getSelects');
    if (tipopto == "[object Object]") tipopto = "-1";
    if (tipopto == "") tipopto = "-1";

    var valor = $("input[name='rbidTipo']:checked").val();
    //  $('#hfidTipo').val(valor.toString());
    $('#hfEmpresa').val(empresa);
    $('#hfUnidad').val(tipopto);
    $.ajax({
        type: 'POST',
        url: controlador + "Reporte/listaTiempoReal",
        data: {
            idsEmpresa: $('#hfEmpresa').val(),
            //tipoReporte: $('#hfidTipo').val(),
            fecha: $('#Fecha').val(),
            fechaFinal: $('#FechaHasta').val(),
            idsTipoPtoMed: $('#hfUnidad').val(),
            nroPagina: nroPagina
        },
        success: function (evt) {
            $('#listado').css("width", $('#mainLayout').width() + "px");

            $('#listado').html(evt);

            $('#tabla').dataTable({
                // "aoColumns": aoColumns(),
                "bAutoWidth": false,
                "bSort": false,
                "scrollY": 430,
                "scrollX": true,
                "sDom": 't',
                "iDisplayLength": 50
            });

        },
        error: function () {
            alert("Ha ocurrido un error en lista");
        }
    });
}

function exportarExcelTR() {
    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    if (empresa == "[object Object]") empresa = "-1";
    if (empresa == "") empresa = "-1";
    var valor = $("input[name='rbidTipo']:checked").val();
    $('#hfEmpresa').val(empresa);

    $.ajax({
        type: 'POST',
        url: controlador + 'reporte/GenerarArchivoReporteTR',
        data: {
            idsEmpresa: $('#hfEmpresa').val(),
            fecha: $('#Fecha').val(),
            fechaFinal: $('#FechaHasta').val(),
            idsTipoPtoMed: $('#hfUnidad').val()
        },
        dataType: 'json',
        success: function (result) {

            if (result == 1) {

                fechaIni = $('#Fecha').val();
                fechaFin = $('#FechaHasta').val();
                window.location = controlador + "reporte/ExportarReporte?tipoReporte=2&fechaInicio=" + fechaIni + "&fechaFinal=" + fechaFin;
            }
        },

        error: function () {
            mostrarError();
        },
    });

}
         




function generarGraficoTiempoReal(nroPagina) {
    $('#reporte').css("display", "none");
    $('#excelGrafico').css("display", "block");
    //$('#excelGrafico').html("<img onclick='exportarGrafico();' width='32px' style='cursor: pointer; display: inline;' src='" + siteRoot + "Content/Images/ExportExcel.png' />");
    $('#graficos').css("display", "block");


    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    if (empresa == "[object Object]") empresa = "-1";
    if (empresa == "") empresa = "-1";
    var tipopto = $('#cbUnidades').multipleSelect('getSelects');
    if (tipopto == "[object Object]") tipopto = "-1";
    if (tipopto == "") tipopto = "-1";

    // var valor = $("input[name='rbidTipo']:checked").val();

    $('#hfEmpresa').val(empresa);
    $('#hfUnidad').val(tipopto);

    $.ajax({
        type: 'POST',
        url: controlador + "Reporte/graficoreporteTiempoReal",
        data: {
            idsEmpresa: $('#hfEmpresa').val(),
            fecha: $('#Fecha').val(),
            fechaFinal: $('#FechaHasta').val(),
            idsTipoPtoMed: $('#hfUnidad').val(),
            nroPagina: nroPagina
        },
        dataType: 'json',
        success: function (result) {
            graficoHSHidrologiaTR(result);
        },
        error: function () {
            alert("Ha ocurrido un error en generar grafico");
        }
    });
}

function exportarGrafico() {

    exportarGrafTR();
}

function exportarGrafTR() {
    $.ajax({
        type: 'POST',
        url: controlador + "reporte/GenerarArchivoGrafTR",
        data: {
            fechaInicial: $('#hfFechaDesde').val(), fechaFinal: $('#hfFechaHasta').val()
        },
        dataType: 'json',
        success: function (result) {
            if (result == 1) {
                window.location = controlador + "reporte/ExportarReporte?tipoReporte=3";
            }
            if (result == -1) {
                mostrarError();
            }
        },
        error: function () {
            alert("Error Grafico export a Excel");
        }
    });
}

function graficoHidrologiaVolumen(result) {
    var opcion = {
        chart: {
            type: 'areaspline'
        },
        title: {
            text: result.Grafico.titleText
        },
        subtitle: {
            text: result.Grafico.subtitleText
        },
        xAxis: {

            categories: result.Grafico.xAxisCategories,
            style: {

                fontSize: '5'
            },

            title: {
                text: result.Grafico.xAxisTitle
            },
        },
        yAxis: {
            title: {
                text: result.Grafico.yAxixTitle
            },
            min: 0
        },
        tooltip: {
            headerFormat: '<b>{series.name}</b><br>',
            pointFormat: '{point.x:%e. %b}: {point.y:.2f} m'
        },

        plotOptions: {
            areaspline: {
                marker: {
                    enabled: true
                },
                fillOpacity: 0.5
            }
        },

        series: []
    };
    for (var i in result.Grafico.seriesName) {
        opcion.series.push({
            name: result.Grafico.seriesName[i],
            data: result.Grafico.seriesData[i]
        });
    }
    $('#graficos').highcharts(opcion);
}

function graficoHidrologiaTR(result) {
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

            categories: result.Grafico.xAxisCategories,
            style: {

                fontSize: '5'
            },

            title: {
                text: result.Grafico.xAxisTitle
            },
        },
        yAxis: {
            title: {
                text: result.Grafico.yAxixTitle
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
    for (var i in result.Grafico.seriesName) {
        opcion.series.push({
            name: result.Grafico.seriesName[i],
            data: result.Grafico.seriesData[i]
        });
    }
    $('#graficos').highcharts(opcion);
}

function graficoHSHidrologiaTR(result) {
    var series = [];
    var itemseries = [];
    for (z = 0; z < result.Grafico.Series.length; z++) {
        {
            for (k = 0; k < result.Grafico.Series[z].Data.length; k++) {
                var seriePoint = [];
                var now = parseJsonDate(result.Grafico.Series[z].Data[k].X);
                var nowUTC = Date.UTC(now.getFullYear(), now.getMonth(), now.getDate(), now.getHours(), now.getMinutes(), now.getSeconds());
                seriePoint.push(nowUTC);
                seriePoint.push(result.Grafico.Series[z].Data[k].Y);
                itemseries.push(seriePoint);
            }
            series.push(itemseries);
            itemseries = [];
        }
    }

    var opcion = {


        title: {
            text: result.Grafico.titleText,
            style: {
                fontSize: '8'
            }
        },
        yAxis: {
            title: {
                text: result.Grafico.YaxixTitle,
                style: {
                    color: 'blue'
                }
            },
            labels: {
                style: {
                    color: 'blue'
                }
            },
            opposite: false
        },
        legend: {
            borderColor: "#909090",
            layout: 'vertical',
            align: 'left',
            verticalAlign: 'top',
            borderWidth: 0,
            enabled: true,
            itemStyle: {
                fontSize: "9px"
            }
        },

        series: []
    };
    for (i = 0; i < series.length; i++) {
        opcion.series.push({
            name: result.Grafico.Series[i].Name,
            //color: result.Grafico.Series[i].Color,
            data: series[i],
            type: result.Grafico.Series[i].Type
        });
    }
    $('#graficos').highcharts('StockChart', opcion);
}

function activaBtn() {
    $('#btnGrafico').show();
    $('#btnExpotar').show();
}

function desactivaBtn() {
    $('#btnGrafico').hide();
    $('#btnExpotar').hide();
}

function handleClick(myRadio) {
    currentValue = myRadio.value;
    if (currentValue == 2)
        $('#btnGrafico').show();
    else
        $('#btnGrafico').show();

    //desactivaBtn();
}
