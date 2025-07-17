﻿var controlador = siteRoot + 'portalinformacion/';
var oTable = null;
$(document).ready(function () {
    obtenerPronosticoTR();
    cargarDatos();
    $('#txtFechaInicial').change({
        onSelect: function (date) {
            var date1 = getFecha(GetDateNormalFormat(date));
            var date2 = getFecha(GetDateNormalFormat($('#txtFechaFinal').val()));
            if (date1 > date2) {
                $('#txtFechaFinal').val(GetDateNormalFormat(date));
            }
            var dateMaximo = getFecha($('#txtDiaMaximoFechaFin').val());
            if (date1 > dateMaximo) {
                $('#txtFechaInicial').val($('#txtDiaMaximoFechaFin').val());
                $('#txtFechaFinal').val($('#txtDiaMaximoFechaFin').val());
            }
        } 
    });

    $('#txtFechaFinal').change({
        onSelect: function (date) {
            var date1 = getFecha(GetDateNormalFormat(date));
            var date2 = getFecha(GetDateNormalFormat($('#txtFechaInicial').val()));
            var dateMaximo = getFecha($('#txtDiaMaximoFechaFin').val());

            if (date1 > dateMaximo) {
                $('#txtFechaFinal').val($('#txtDiaMaximoFechaFin').val());
            }
            if (date1 < date2) {
                $('#txtFechaFinal').val($('#txtFechaInicial').val());
            }
        }   
    });

    $('.item-change-dashboard').click(function () {
        $('.item-change-dashboard').removeClass('active');
        $(this).addClass('active');
        var item = $(this).attr('data-fuente');
        
        if (item == "demanda") {
            $('#contenidoDemanda').show();
            $('#contenidoMedidor').hide();
            $('#filtroBusqueda').show();
        }
        else if (item == "maxima") {
            $('#contenidoDemanda').hide();
            $('#contenidoMedidor').show();
            $('#filtroBusqueda').hide();

            $('#btnReporteMaximaDemanda').click();
        }
    });


    var param = getParameterByName("indicador");

    if (param == "maxima") {
        $('.change-dashboard-item-l').removeClass('active');
        $('.change-dashboard-item-r').addClass('active');
        $("#btnReporteMaximaDemanda").css("background-color", "var(--bs-gray-500)");
        $("#btnReporteDiagramaCarga").css("background-color", "");
        $("#btnReporteRecursoEnergetico").css("background-color", "");
        $("#btnReporteRankingDemandaPotencia").css("background-color", "");
        $('#contenidoDemanda').hide();
        $('#contenidoMedidor').show();
        $('#filtroBusqueda').hide();
        cargarMaximaDemanda("https://www.coes.org.pe/Portal/medidores/MaximaDemanda/index", "1220");
    }


    $('.reporte-maxima').click(function () {
        cargarMaximaDemanda($(this).attr('data-url'), $(this).attr('data-alto'));
    });

    $('#btnConsultar').click(function () {
        cargarDemanda();
    });

    $('#btnExportar').click(function () {
        exportar();
    });

    $('#divMaximaDemanda').on('click', function () {
        $("#divMaximaDemanda").addClass("active");
        $("#divDemanda").removeClass("active");
        $("#btnMaximaDemanda").css("background-color", "var(--bs-gray-500)");
        $("#btnReporteMaximaDemanda").css("background-color", "var(--bs-gray-500)");
        $("#btnReporteDiagramaCarga").css("background-color", "");
        $("#btnReporteRecursoEnergetico").css("background-color", "");
        $("#btnReporteRankingDemandaPotencia").css("background-color", "");
        $("#btnDemanda").css("background-color", "");

        $("#divPronosticoTR").removeClass("active");
        $("#btnPronosticoTR").css("background-color", "");
        $('#contenidoPronosticoTR').hide();


        $('#contenidoDemanda').hide();
        $('#contenidoMedidor').show();
        $('#filtroBusqueda').hide();

        $('#btnReporteMaximaDemanda').click();
    });

        $('#divReporteMaxDemanda').on('click', function () {
            $("#btnReporteMaximaDemanda").css("background-color", "var(--bs-gray-500)");
            $("#btnReporteDiagramaCarga").css("background-color", "");
            $("#btnReporteRecursoEnergetico").css("background-color", "");
            $("#btnReporteRankingDemandaPotencia").css("background-color", "");
        });

        $('#divDiagramaCarga').on('click', function () {
            $("#btnReporteMaximaDemanda").css("background-color", "");
            $("#btnReporteDiagramaCarga").css("background-color", "var(--bs-gray-500)");
            $("#btnReporteRecursoEnergetico").css("background-color", "");
            $("#btnReporteRankingDemandaPotencia").css("background-color", "");
        });

        $('#divRecursoEnergetico').on('click', function () {
            $("#btnReporteMaximaDemanda").css("background-color", "");
            $("#btnReporteDiagramaCarga").css("background-color", "");
            $("#btnReporteRecursoEnergetico").css("background-color", "var(--bs-gray-500)");
            $("#btnReporteRankingDemandaPotencia").css("background-color", "");
        });

        $('#divRankingDemandaPotencia').on('click', function () {
            $("#btnReporteMaximaDemanda").css("background-color", "");
            $("#btnReporteDiagramaCarga").css("background-color", "");
            $("#btnReporteRecursoEnergetico").css("background-color", "");
            $("#btnReporteRankingDemandaPotencia").css("background-color", "var(--bs-gray-500)");
        });

    $('#divDemanda').on('click', function () {
        $("#divDemanda").addClass("active");
        $("#divMaximaDemanda").removeClass("active");
        $("#btnDemanda").css("background-color", "var(--bs-gray-500)");
        $("#btnMaximaDemanda").css("background-color", "");
        
        $("#divPronosticoTR").removeClass("active");
        $("#btnPronosticoTR").css("background-color", "");
        $('#contenidoPronosticoTR').hide();

        $('#contenidoDemanda').show();
        $('#contenidoMedidor').hide();
        $('#filtroBusqueda').show();
    });

    $('#divPronosticoTR').on('click', function () {
        $("#divPronosticoTR").addClass("active");
        $("#divMaximaDemanda").removeClass("active");
        $("#divDemanda").removeClass("active");
        $("#btnPronosticoTR").css("background-color", "var(--bs-gray-500)");
        $("#btnMaximaDemanda").css("background-color", "");
        $("#btnDemanda").css("background-color", "");


        $('#contenidoDemanda').hide();
        $('#contenidoMedidor').hide();
        $('#filtroBusqueda').hide();
        //$('#filtroBusqueda').show();
        $('#contenidoPronosticoTR').show();
    });


    cargarDemanda();
});

obtenerPronosticoTR = function () {
    $.ajax({
        type: 'GET',
        url: controlador + "ObtenerPronosticoTiempoReal",
        datatype: 'json',
        success: function (data) {
            let bodyContent = "";
            if (data && data.length > 1) {
                data.forEach(item => {
                    bodyContent += "<tr><td>" + item.fecha +
                        "</td><td>" + (item.Ejecutado == -1 ? "-" : item.Ejecutado) +
                        "</td><td>" + (item.Rdo == -1 ? "-" : item.Rdo) +
                        "</td><td>" + (item.PronosticoTiempoReal == -1 ? "-" : item.PronosticoTiempoReal) +
                        "</td><td>" + (item.PronosticoMinimo == -1 ? "-" : item.PronosticoMinimo) +
                        "</td><td>" + (item.PronosticoMaximo == -1 ? "-" : item.PronosticoMaximo) +
                        "</td></tr>";
                });

                graficarPronosticoTiempoReal(data);
            }

            $("#tablaPronosticoTR > tbody").html(bodyContent);
        },
        error: function () {
            alert("Error");
        }
    });

    $.ajax({
        type: 'GET',
        url: controlador + "ObtenerPronosticoTiempoRealFechaDeActualizacion",
        datatype: 'json',
        success: function (fecha) {
            $("#textPronosticoTrFechaActualizacion").html(fecha);
        },
        error: function () {
            alert("Error");
        }
    });
}

function graficarPronosticoTiempoReal(data) {
    var intervalosMinimoMaximoList = [];
    var rdoList = [];
    var pronosticoTiempoRealList = [];
    var ejecutadoList = [];
    
    data.forEach(item => {
        fechaSplit = item.fecha.split(' ');
        fechaDateSplit = fechaSplit[0].split('-');
        fechaTimeSplit = fechaSplit[1].split(':');
        nowDat = new Date(fechaDateSplit[0], (fechaDateSplit[1] - 1), fechaDateSplit[2], fechaTimeSplit[0], fechaTimeSplit[1], fechaTimeSplit[2]);
        pFechaObject = Date.UTC(nowDat.getFullYear(), nowDat.getMonth(), nowDat.getDate(), nowDat.getHours(), nowDat.getMinutes(), nowDat.getSeconds());

        if (item.Ejecutado == -1)
            ejecutadoList.push([pFechaObject, null]);
        else
            ejecutadoList.push([pFechaObject, item.Ejecutado]);

        if (item.Rdo == -1)
            rdoList.push([pFechaObject, null]);
        else
            rdoList.push([pFechaObject, item.Rdo]);

        if (item.PronosticoMinimo == -1 || item.PronosticoMaximo == -1)
            intervalosMinimoMaximoList.push([pFechaObject, null, null]);
        else
            intervalosMinimoMaximoList.push([pFechaObject, item.PronosticoMinimo, item.PronosticoMaximo]);        

        if (item.PronosticoTiempoReal == -1)
            pronosticoTiempoRealList.push([pFechaObject, null]);
        else
            pronosticoTiempoRealList.push([pFechaObject, item.PronosticoTiempoReal]);
    });   

    var chartseries = [{
        name: 'Ejecutado',
        data: ejecutadoList,
        zIndex: 1,
        marker: {
            fillColor: 'white',
            lineWidth: 1,
            lineColor: '#33B8FF'
        },
        color: '#33B8FF',
        },
        {
        name: 'Programación Diaria / Reprogramación Diaria',
        data: rdoList,
        zIndex: 1,
        marker: {
            fillColor: 'white',
            lineWidth: 1,
            lineColor: '#ff6961'
        },
        color: '#ff6961',
        dashStyle: 'shortdot'
    },
    {
        name: 'Pronóstico de Demanda Automático de Tiempo Real',
        data: pronosticoTiempoRealList,
        zIndex: 1,
        marker: {
            fillColor: 'white',
            lineWidth: 1,
            lineColor: '#005187' 
        },
        color: '#005187',
        dashStyle: 'spline'
    },
    {
        name: 'Rango mínimo y máximo del pronóstico TR',
        data: intervalosMinimoMaximoList,
        type: 'arearange',
        lineWidth: 0,
        linkedTo: ':previous',
        color: '#1192e8', 
        fillOpacity: 0.3,
        zIndex: 0,
        marker: {
            enabled: false
        },
    }];

    $('#graficoPronosticoTR').highcharts('StockChart', {

        rangeSelector: {
            buttons: [
                {
                    count: 1,
                    type: 'day',
                    text: 'D'
                },
                {
                    count: 1,
                    type: 'week',
                    text: 'S'
                },
                {
                    count: 1,
                    type: 'month',
                    text: 'M'
                },
                {
                    type: 'all',
                    text: 'Todo'
                }
            ],
            selected: 3,
            allButtonsEnabled: true,
            visible: false
        },
        xAxis: {
            type: 'datetime'
        },
        yAxis: {
            labels: {
                formatter: function () {
                    return this.value + ' ' + "MW";
                }
            },
            plotLines: [{
                value: 0,
                width: 2,
                color: 'silver'
            }]
        },
        legend: {
            layout: 'horizontal',
            align: 'center',
            verticalAlign: 'top',
            borderWidth: 0,
            enabled: true
        },
        tooltip: {
            valueDecimals: 2
        },
        colors: ['#33B8FF', '#3DD19D', '#FF7A33'],
        series: chartseries
    });
}

getParameterByName = function(name) {
    var match = RegExp('[?&]' + name + '=([^&]*)').exec(window.location.search);
    return match && decodeURIComponent(match[1].replace(/\+/g, ' '));
}

cargarDemanda = function () {
    $.ajax({
        type: 'POST',
        url: controlador + "Demanda",
        data: {
            fechaInicial: GetDateNormalFormat($('#txtFechaInicial').val()),
            fechaFinal: GetDateNormalFormat($('#txtFechaFinal').val())
        },
        datatype: 'json',
        success: function (result) {
            pintarDemanda(result.Data);
            graficarDemanda(result.Chart);
            $('#hfIndDemanda').val("S");
        },
        error: function () {
            alert("Error");
        }
    });
}

pintarDemanda = function (data) {

    $('#tablaDemanda tbody').empty();
    for (var i in data) {
        var ejecutado = "";
        if (data[i].ValorEjecutado != null) {
            ejecutado = $.number(data[i].ValorEjecutado, 3, ',', ' ');
        }
        if (i % 2 == 0) {
            $('#tablaDemanda tbody').append('<tr><td>' + data[i].Fecha + '</td><td style="text-align:right">' +
                ejecutado + '</td><td style="text-align:right">' +
                $.number(data[i].ValorProgramacionDiaria, 3, ',', ' ') + '</td><td style="text-align:right">' +
                $.number(data[i].ValorProgramacionSemanal, 3, ',', ' ') + '</td></tr>');
        }
        else {
            $('#tablaDemanda tbody').append('<tr class="odd"><td>' + data[i].Fecha + '</td><td style="text-align:right">' +
                ejecutado + '</td><td style="text-align:right">' +
                $.number(data[i].ValorProgramacionDiaria, 3, ',', ' ') + '</td><td style="text-align:right">' +
                $.number(data[i].ValorProgramacionSemanal, 3, ',', ' ') + '</td></tr>');
        }
    }


}

graficarDemanda = function (data) {
    var seriesOptions = [];
    var title = "";

    for (var i = 0; i < data.Series.length; i++) {
        var serie = [];
        title = data.Series[i].Name + " - " + title;

        for (var k = 0; k < data.Series[i].Data.length; k++) {
            var seriePoint = [];
            var now = new Date(data.Series[i].Data[k].Nombre);
            var nowUTC = Date.UTC(now.getFullYear(), now.getMonth(), now.getDate(), now.getHours(), now.getMinutes(), now.getSeconds());
            seriePoint.push(nowUTC);
            seriePoint.push(data.Series[i].Data[k].Valor);
            serie.push(seriePoint);
        }

        var dashStyle = "shortdot";
        if (data.Series[i].Name != "Ejecutado") {
            dashStyle = "shortdot";
        }
        else {
            dashStyle = "";
        }

        seriesOptions[i] = {
            name: data.Series[i].Name,
            data: serie,
            type: 'spline',
            dashStyle: dashStyle
        };
    }

    $('#graficoDemanda').highcharts('StockChart', {

        rangeSelector: {
            buttons: [
                {
                    count: 1,
                    type: 'day',
                    text: 'D'
                },
                {
                    count: 1,
                    type: 'week',
                    text: 'S'
                },
                {
                    count: 1,
                    type: 'month',
                    text: 'M'
                },
                {
                    type: 'all',
                    text: 'Todo'
                }
            ],
            selected: 3,
            allButtonsEnabled: true,
            visible: false
        },
        xAxis: {
            type: 'datetime'
        },
        yAxis: {
            labels: {
                formatter: function () {
                    return this.value + ' ' + "MW";
                }
            },
            plotLines: [{
                value: 0,
                width: 2,
                color: 'silver'
            }]
        },
        legend: {
            layout: 'horizontal',
            align: 'center',
            verticalAlign: 'top',
            borderWidth: 0,
            enabled: true
        },
        tooltip: {
            pointFormat: '<span style="color:{series.color}">{series.name}</span>: <b>{point.y}</b><br/>',
            valueDecimals: 2
        },
        colors: ['#33B8FF', '#3DD19D', '#FF7A33'],
        series: seriesOptions
    });
}

cargarMaximaDemanda = function (url, alto)
{    
    $('#ifrMaximaDemanda').attr("src", url);
    $('#ifrMaximaDemanda').attr("height", alto);
}

exportar = function () { 
    $.ajax({
        type: 'POST',
        url: controlador + "exportardemanda",
        dataType: 'json',
        data: {
            fechaInicial: GetDateNormalFormat($('#txtFechaInicial').val()),
                fechaFinal: GetDateNormalFormat($('#txtFechaFinal').val())
        },
        success: function (result) {
            if (result == 1) {
                window.location = controlador + 'descargardemanda';
            }
            else {
                mostrarError();
            }
        },
        error: function () {
            mostrarError()
        }
    });
}

getFecha = function (date) {
    var parts = date.split("/");
    var date = new Date(parts[1] + "/" + parts[0] + "/" + parts[2]);
    return date.getTime();
}

mostrarError = function ()
{
    alert("Error");
}


function cargarDatos() {
    var param = getParameterByName("indicador");
    if (param == "maxima") {
        $("#divMaximaDemanda").addClass("active");
        $("#divDemanda").removeClass("active");
        $("#btnMaximaDemanda").css("background-color", "var(--bs-gray-500)");
        $("#btnDemanda").css("background-color", "");
    } else {

        $("#divDemanda").addClass("active");
        $("#divMaximaDemanda").removeClass("active");
        $("#btnDemanda").css("background-color", "var(--bs-gray-500)");
        $("#btnMaximaDemanda").css("background-color", "");

        $('#contenidoDemanda').show();
        $('#contenidoMedidor').hide();
        $('#filtroBusqueda').show();
}

}

// Convierte formato yyyy-mm-dd a dd/mm/yyyy
GetDateNormalFormat = function (fechaFormatoISO) {
    let fechaNormal = fechaFormatoISO.substring(8, 10) + "/" + fechaFormatoISO.substring(5, 7) + "/" + fechaFormatoISO.substring(0, 4);
    return fechaNormal;
}
