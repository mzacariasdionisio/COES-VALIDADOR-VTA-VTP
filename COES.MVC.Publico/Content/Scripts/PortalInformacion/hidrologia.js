var controlador = siteRoot + 'portalinformacion/';

$(document).ready(function () {

    $('#txtFechaInicial').Zebra_DatePicker({
        direction: false,
        pair: $('#txtFechaFinal'),        
        onSelect: function (date) {
            var date1 = getFecha(date);
            var date2 = getFecha($('#txtFechaFinal').val());
            if (date1 > date2) {
                $('#txtFechaFinal').val(date);
            }
        }
    });

    $('#txtFechaFinal').Zebra_DatePicker({
        direction: true
    });
   
    $('#btnConsultar').click(function () {
        cargarHidrologia();
    });

    $('#btnGraficoVolumen').click(function () {
        graficoHidrologia('Mm3')
    });

    $('#btnGraficoNivel').click(function () {
        graficoHidrologia('Metros')
    });

    $('#btnExportar').click(function () {
        exportar();
    });   

    cargarHidrologia();
});

cargarHidrologia = function () {
    
    $('#contentGrafico').hide();
    $('#contenedorHidrologia').show();
    
    var oneDay = 24 * 60 * 60 * 1000;
    var firstDate = getFecha($('#txtFechaInicial').val());
    var secondDate = getFecha($('#txtFechaFinal').val());
    var diffDays = Math.round(Math.abs((firstDate - secondDate) / (oneDay)));

    if (diffDays <= 31) {

        $.ajax({
            type: 'POST',
            url: controlador + "listahidrologia",
            data: {
                fechaInicial: $('#txtFechaInicial').val(),
                fechaFinal: $('#txtFechaFinal').val()
            },
            success: function (evt) {
                $('#contenedorHidrologia').html(evt);
                $('#tbHidrologia').dataTable({
                    "scrollY": 477,
                    "scrollX": true,
                    "sDom": 't',
                    "ordering": false,
                    "paginate": false,
                    "iDisplayLength": 50
                });
            },
            error: function () {
                mostrarError();
            }
        });
    }
    else {
        alert("Solo puede seleccionar un mes como máximo.");
    }

}

exportar = function () {
    $.ajax({
        type: 'POST',
        url: controlador + "exportarhidrologia",
        dataType: 'json',
        data: {
            fechaInicial: $('#txtFechaInicial').val(),
            fechaFinal: $('#txtFechaFinal').val()            
        },
        success: function (result) {
            if (result == 1) {
                window.location = controlador + 'descargarhidrologia';
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

graficoHidrologia = function (fuente) {
    $('#contentGrafico').show();
    $('#contenedorHidrologia').hide();

    $.ajax({
        type: 'POST',
        url: controlador + "graficohidrologia",
        data: {
            fechaInicial: $('#txtFechaInicial').val(),
            fechaFinal: $('#txtFechaFinal').val(),
            fuente: fuente
        },
        dataType:'json',
        success: function (result) {
            if (result != -1) {
                plotearHidrologia(result, fuente);
            }
            else {
                mostrarError();
            }
        },
        error: function () {
            mostrarError();
        }
    });
}

plotearHidrologia = function (data, tipoGrafica) {
    var seriesOptions = [];
    var title = "";

    for (var i = 0; i < data.length; i++) {

        var serie = [];
        title = data[i].Name + " - " + title;

        for (var k = 0; k < data[i].Data.length; k++) {

            var seriePoint = [];
            var now = new Date(data[i].Data[k].Nombre);            
            var nowUTC = Date.UTC(now.getFullYear(), now.getMonth(), now.getDate(), now.getHours(), now.getMinutes(), now.getSeconds());

            seriePoint.push(nowUTC);
            seriePoint.push(data[i].Data[k].Valor);
            serie.push(seriePoint);
        }

        seriesOptions[i] = {
            name: data[i].Name,
            data: serie
        };
    }

    $('#graficoHidrologia').highcharts('StockChart', {
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
                    return this.value + ' ' + tipoGrafica;
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
            pointFormat: '<span style="color:{series.color}">{series.name}</span>: <b>{point.y} ' + tipoGrafica + '</b><br/>',
            valueDecimals: 2
        },
        series: seriesOptions
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