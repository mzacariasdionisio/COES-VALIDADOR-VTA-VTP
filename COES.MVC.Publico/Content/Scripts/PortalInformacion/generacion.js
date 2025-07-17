﻿var controlador = siteRoot + 'portalinformacion/';
var generacionEmpresa = [];
var generacionEmpresaDetalle = [];

$(document).ready(function () {
        
    $('#txtFechaInicial').change({
        pair: $('#txtFechaFinal'),
        onSelect: function (date) {            
            var date1 = getFecha(GetDateNormalFormat(date));
            var date2 = getFecha(GetDateNormalFormat($('#txtFechaFinal').val()));
            if (date1 > date2) {
                $('#txtFechaFinal').val(GetDateNormalFormat(date));
            }
        }
    });

    $('#txtFechaFinal').change({
        direction: true
    });

    $('#divScada').on('click', function () {
        $("#divScada").addClass("active");
        $("#divMedidores").removeClass("active");
        $("#btnScada").css("background-color", "var(--bs-gray-500)");
        $("#btnMedidores").css("background-color", "");


        $('#hfFuenteGeneracion').val("scada");
        $('#graficoGeneracion').show();
        $("#graficoGeneracionTotal").hide();
        $('#dashboardBack').hide();
        cargarGeneracion("scada"); 
    });

    $('#divMedidores').on('click', function () {
        $("#divMedidores").addClass("active");
        $("#divScada").removeClass("active");
        $("#btnMedidores").css("background-color", "var(--bs-gray-500)");
        $("#btnScada").css("background-color", "");

        $('#hfFuenteGeneracion').val("medidores");
        $('#graficoGeneracion').show();
        $("#graficoGeneracionTotal").hide();
        $('#dashboardBack').hide();
        cargarGeneracion("medidores"); 
    });
       
    $('.item-change-dashboard').click(function () {
        $('.item-change-dashboard').removeClass('active');
        $(this).addClass('active');
        var item = $(this).attr('data-fuente');
        $('#hfFuenteGeneracion').val(item);
        $('#graficoGeneracion').show();
        $("#graficoGeneracionTotal").hide();
        $('#dashboardBack').hide();
        cargarGeneracion(item);        
    });

    $('.item-change-dashboard').mouseover(function () {
        var item = $(this).attr('data-fuente');
        $('#tooltip-' + item).show();
    });

    $('.item-change-dashboard').mouseout(function () {
        $('.change-dashboard-tooltip').hide();
    });

    $('#generacion-header').click(function () {
        cargarGeneracionEmpresa();
    });

    $('#btnConsultar').click(function () {
        cargarGeneracion($('#hfFuenteGeneracion').val());
    });

    $('#btnExportar').click(function () {
        exportar($('#hfFuenteGeneracion').val());
    });

    $('#dashboardBack').click(function () {
        $('#graficoGeneracion').show();
        $("#graficoGeneracionTotal").hide();
        $('#dashboardBack').hide();
    });
 cargarDatos();
 cargarGeneracion('scada'); 
});

function cargarDatos() {
    $("#divScada").addClass("active");
    $("#divMedidores").removeClass("active");
    $("#btnScada").css("background-color", "var(--bs-gray-500)");
    $("#btnMedidores").css("background-color", "");
}
cargarGeneracion = function (fuente) {
    var indicador = (fuente == 'scada') ? 0 : 1;
    $.ajax({
        type: 'POST',
        url: controlador + "Generacion",
        data: {
            fechaInicial: GetDateNormalFormat($('#txtFechaInicial').val()),
            fechaFinal: GetDateNormalFormat($('#txtFechaFinal').val()),
            indicador: indicador            
        },
        datatype: 'json',
        success: function (result) {
            pintarGeneracion(result.ListadoPorEmpresa);
            graficarGeneracion(result.GraficoPorEmpresa);
            graficarGeneracionTipoCombustible(result.GraficoTipoCombustible);

            if (indicador == 1) {
                $('#dashboard-info-generacion').text('Datos cada 15 minutos medidores electrónicos (Obtenidos los primeros días de cada mes) - ' + $('#txtFechaFinal').val());
            }
            else {
                $('#dashboard-info-generacion').text('Datos instantáneos de potencia cada 30 minutos del sistema SCADA/COES - ' + $('#txtFechaFinal').val());
            }
        },
        error: function (request, status, error) {
            console.log(request.responseText);
        }
    });
}

pintarGeneracion = function (data) {

    generacionEmpresa = [];

    $('#tbGeneracion tbody').empty();
    var total = 0;
    for (var i in data) {
        if (i % 2 == 0)
            $('#tbGeneracion tbody').append('<tr style="cursor:pointer" onclick="verDetalleGeneracion(\'' + data[i].Emprnomb + '\' )"><td>' + data[i].Emprnomb + '</td><td style="text-align:right">' + $.number(data[i].Meditotal, 3, ',', ' ') + '</td></tr>');
        else
            $('#tbGeneracion tbody').append('<tr style="cursor:pointer" class="odd" onclick="verDetalleGeneracion(\'' + data[i].Emprnomb + '\' )"><td>' + data[i].Emprnomb + '</td><td style="text-align:right">' + $.number(data[i].Meditotal, 3, ',', ' ') + '</td></tr>');

        total = total + data[i].Meditotal;
        generacionEmpresa.push([data[i].Emprnomb, parseFloat(data[i].Meditotal)]);
    }
    $('#spanTotalGeneracion').text($.number(total, 3, ',', ' '));
    $('#totalGeneracion').text($.number(total, 3, ',', ' '));
}

verDetalleGeneracion = function (emprnomb)
{
    $('#graficoGeneracion').hide();
    $("#graficoGeneracionTotal").show();
    $('#dashboardBack').show();

    var dataPie = [];

    for (var j in generacionEmpresaDetalle)
    {
        if (generacionEmpresaDetalle[j].Name == emprnomb)
        {
            for (var k in generacionEmpresaDetalle[j].Data)
            {
                dataPie.push([generacionEmpresaDetalle[j].Data[k].Nombre, generacionEmpresaDetalle[j].Data[k].Valor])
            }
        }
    }

    $('#graficoGeneracionTotal').highcharts({
        chart: {
            plotBackgroundColor: null,
            plotBorderWidth: null,
            plotShadow: false
        },
        title: {
            text: emprnomb
        },
        tooltip: {
            pointFormat: '{series.name}: <b>{point.y:.3f} MWh</b>'
        },
        plotOptions: {
            pie: {
                borderWidth: 0,
                allowPointSelect: true,
                cursor: 'pointer',
                dataLabels: {
                    enabled: true,
                    color: '#000000',
                    connectorColor: '#000000',
                    format: '{point.percentage:.3f} %'
                },
                showInLegend: true
            }
        },
        series: [{
            type: 'pie',
            name: 'Generacion',
            data: dataPie
        }]
    });

}

cargarGeneracionEmpresa = function ()
{
    $('#graficoGeneracion').hide();
    $('#graficoGeneracionTotal').show();
    $('#dashboardBack').show();

    $('#graficoGeneracionTotal').highcharts({
        chart: {
            plotBackgroundColor: null,
            plotBorderWidth: null,
            plotShadow: false
        },
        title: {
            text: "Total Generación"
        },
        tooltip: {
            pointFormat: '{series.name}: <b>{point.y:.3f} MWh</b>'
        },
        plotOptions: {
            pie: {
                allowPointSelect: true,
                cursor: 'pointer',
                dataLabels: {
                    enabled: true,
                    color: '#000000',
                    connectorColor: '#000000',
                    format: '<b>{point.name}</b>: {point.percentage:.3f} %'
                }
            }
        },
        series: [{
            type: 'pie',
            name: 'Generacion',
            data: generacionEmpresa
        }]
    });
}

graficarGeneracion = function (data) {

    $('#graficoGeneracion').show();
    $('#graficoGeneracionTotal').hide();

    generacionEmpresaDetalle = data.SeriesAdicional;

    var options = {
        chart: {
            type: 'bar',
            renderTo: 'graficoGeneracion',
        },
        title: {
            text: ''
        },
        tooltip: {
            pointFormat: '{series.name}: <b>{point.y:.3f} MWh</b>'
        },
        xAxis: {
            categories:
                []
        },
        yAxis: {
            min: 0,
            labels: {
                format: '{value} MWh'
            },
            title: {
                text: ''
            }
        },
        legend: {
            backgroundColor: '#FFFFFF'
        },
        plotOptions: {
            series: {
                stacking: 'normal'
            }
        },
        series: []
    };
    
    var chart = new Highcharts.Chart(options);
    chart.xAxis[0].setCategories(data.Categorias);
    var dataSeries = [];
        
    for (var i = 0; i < data.Series.length; i++) {
        var colorSerie = '';
        if (data.Series[i].Name == 'HIDROELÉCTRICA') {
            colorSerie = '#4572A7';
        }
        else if (data.Series[i].Name == "SOLAR") {
            colorSerie = '#FFD700';
        }
        else if (data.Series[i].Name == 'TERMOELÉCTRICA') {
            colorSerie = '#FF0000';
        }
        else if (data.Series[i].Name == 'EÓLICA') {
            colorSerie = '#69C9E0';
        }

        dataSeries.push({
            name: data.Series[i].Name,
            data: data.Series[i].Data,
            color: colorSerie
        });
    }

    var serieNuevo = [];
    for (var i in dataSeries) {
        if (dataSeries[i]['name'] == 'HIDROELÉCTRICA') {
            serieNuevo.push(dataSeries[i]);
            break;
        }
    }
    for (var i in dataSeries) {

        if (dataSeries[i]['name'] == 'TERMOELÉCTRICA') {
            serieNuevo.push(dataSeries[i]);
            break;
        }
    }
    for (var i in dataSeries) {

        if (dataSeries[i]['name'] == 'SOLAR') {
            serieNuevo.push(dataSeries[i]);
            break;
        }
    }
    for (var i in dataSeries) {

        if (dataSeries[i]['name'] == 'EÓLICA') {
            serieNuevo.push(dataSeries[i]);
            break;
        }
    }

    for (var j in serieNuevo) {
        chart.addSeries({
            name: serieNuevo[j].name,
            data: serieNuevo[j].data,
            color: serieNuevo[j].color
        });
    }  
}

graficarGeneracionTipoCombustible = function (data) {
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
        seriesOptions[i] = {
            name: data.Series[i].Name,
            data: serie,
            type: 'area',
            color: getColorCombustible(data.Series[i].Name)
        };
    }

    var serieNuevo = [];

    for (var j in seriesOptions) {        
        if (seriesOptions[j]['name'] == 'DIESEL') {
            serieNuevo.push(seriesOptions[j]);
            break;
        }
    }
    for (var j in seriesOptions) {
        if (seriesOptions[j]['name'] == 'RESIDUAL') {
            serieNuevo.push(seriesOptions[j]);
            break;
        }
    }
    for (var j in seriesOptions) {
        if (seriesOptions[j]['name'] == 'CARBÓN') {
            serieNuevo.push(seriesOptions[j]);
            break;
        }
    }
    for (var j in seriesOptions) {
        if (seriesOptions[j]['name'] == 'GAS') {
            serieNuevo.push(seriesOptions[j]);
            break;
        }
    }
    for (var j in seriesOptions) {
        if (seriesOptions[j]['name'] == 'HÍDRICO') {
            serieNuevo.push(seriesOptions[j]);
            break;
        }
    }
    for (var j in seriesOptions) {
        if (seriesOptions[j]['name'] == 'BIOGÁS') {
            serieNuevo.push(seriesOptions[j]);
            break;
        }
    }
    for (var j in seriesOptions) {
        if (seriesOptions[j]['name'] == 'BAGAZO') {
            serieNuevo.push(seriesOptions[j]);
            break;
        }
    }
    for (var j in seriesOptions) {
        if (seriesOptions[j]['name'] == 'SOLAR') {
            serieNuevo.push(seriesOptions[j]);
            break;
        }
    }
    for (var j in seriesOptions) {
        if (seriesOptions[j]['name'] == 'EÓLICA') {
            serieNuevo.push(seriesOptions[j]);
            break;
        }
    }   

    $('#graficoCombustible').highcharts('StockChart', {
        chart: {
            type: 'area'
        },
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
            selected: 0,
            allButtonsEnabled: true,
            visible: false

        },
        xAxis: {
            type: 'datetime'
        },
        yAxis: {
            labels: {
                formatter: function () {
                    return this.value + ' %';
                }
            },
            plotLines: [{
                value: 0,
                width: 2,
                color: 'silver'
            }]
        },
        legend: {
            layout: 'vertical',
            align: 'right',
            verticalAlign: 'middle',
            borderWidth: 0,
            enabled: true
        },
        tooltip: {
            pointFormat: '<span style="color:{series.color}">{series.name}</span>: <b>{point.percentage:.0f}%</b> ({point.y:,.3f} MWh)<br/>',
            shared: true
        },
        plotOptions: {
            area: {
                stacking: 'percent',
                lineColor: '#ffffff',
                lineWidth: 1,
                marker: {
                    lineWidth: 1,
                    lineColor: '#ffffff'
                }
            }
        },
        series: serieNuevo
    });
}

exportar = function (fuente) {  
    var indicador = (fuente == 'scada') ? 0 : 1;

    $.ajax({
        type: 'POST',
        url: controlador + "exportargeneracion",
        dataType: 'json',
        data: {
            fechaInicial: GetDateNormalFormat($('#txtFechaInicial').val()),
            fechaFinal: GetDateNormalFormat($('#txtFechaFinal').val()),
            indicador: indicador
        },
        success: function (result) {
            if (result == 1) {
                window.location = controlador + 'descargargeneracion';
            }
            else {
                mostrarError
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

mostrarError = function () {
    alert("Error");
}

// Convierte formato yyyy-mm-dd a dd/mm/yyyy
GetDateNormalFormat = function (fechaFormatoISO) {
    let fechaNormal = fechaFormatoISO.substring(8, 10) + "/" + fechaFormatoISO.substring(5, 7) + "/" + fechaFormatoISO.substring(0, 4);
    return fechaNormal;
}