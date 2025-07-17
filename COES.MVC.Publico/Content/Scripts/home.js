var controlador = siteRoot + 'home/'
var contador = 0;
var intervals = [];
$(document).ready(function () {
    obtenerFrecuencia();
    //obtenerDemanda();
    //obtenerProduccion();
    $('.jcarousel').jcarousel({
        auto: 0,
        wrap: 'circular'
    }).jcarouselAutoscroll({
        target: '+=1',
        interval: 10000,
        create: $('.jcarousel').hover(function () {
            $(this).jcarouselAutoscroll('stop');
        }, function () {
            $(this).jcarouselAutoscroll('start');
        })
    });



    $('.jcarousel-pagination')
        .on('jcarouselpagination:active', 'a', function () {
            $(this).addClass('active');
        })
        .on('jcarouselpagination:inactive', 'a', function () {
            $(this).removeClass('active');
        })
        .jcarouselPagination({
            perPage: 1,
        });


    $('#tab-container').easytabs({
        animate: false
    });

    /*
    $('.tab-container').bind('easytabs:after', function () {
        $('#contentHolderEvento').perfectScrollbar({ suppressScrollX: true });
        $('#contentHolderComunicado').perfectScrollbar({ suppressScrollX: true });
    });

    $('#contentHolderEvento').perfectScrollbar({ suppressScrollX: true });
    $('#contentHolderComunicado').perfectScrollbar({ suppressScrollX: true });
    */

    Highcharts.setOptions({
        lang: {
            shortMonths: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'],
            weekdays: ['Domingo', 'Lunes', 'Martes', 'Miércoles', 'Jueves', 'Viernes', 'Sábado'],
            decimalPoint: ',',
            thousandsSep: ' '
        }
    });



    $('#cbGrafico').change(function () {
        clearAllInterval();
        var opcion = parseInt($('#cbGrafico').val());
        if (opcion == 1)
            obtenerFrecuencia();
        else if (opcion == 2) {
            obtenerDemanda();
        }
        else if (opcion == 3) {
            obtenerProduccion();
        }
    });

    //obtenerFrecuencia();
});

clearAllInterval = function () {
    for (var i = 1; i < 999999; i++)
        window.clearInterval(i);
}

/*-------Frecuencia--------*/

obtenerFrecuencia = function () {
    $.ajax({
        type: 'POST',
        url: controlador + "frecuencia",
        datatype: 'json',
        success: function (result) {
            graficarFrecuencia(result);
            obtenerDemanda();
            updateFrecuencia();
        },
        error: function () {
            //alert("Ha ocurrido un error");
        }
    });
}

updateFrecuencia = function () {
    if (contador > 200) {
        clearAllInterval();
        document.location.href = controlador;
    }
    else {
        actualizarFrecuencia();
        contador++;
    }
}

actualizarFrecuencia = function () {
    $.ajax({
        type: 'POST',
        url: controlador + "updatefrecuencia",
        datatype: 'json',
        global: false,
        success: function (result) {
            actualizarGraficoFrecuencia(result);
            setTimeout(function () {
                updateFrecuencia();
            }, 1000);

        },
        error: function () {
            //alert("Ha ocurrido un error");
            setTimeout(function () {
                updateFrecuencia();
            }, 1000);
        }
    });
}

graficarFrecuencia = function (data) {

    var now = new Date(data[data.length - 1].Fecha);
    var fecha = new Date(now.getFullYear(), now.getMonth(), now.getDate(), now.getHours(), now.getMinutes(), now.getSeconds());
    $('#resumenIndicador').html("<div class='grafl'><strong>Frecuencia Actual:</strong><br /> " + fecha.format("dd/mm/yyyy HH:MM:ss")
        + "</div><div class='grafr'>" + $.number(data[data.length - 1].Valor, 3, ',', ' ') + "<br/> Hz</div>");

    var series = [];
    var title = "";
    for (var k = 0; k < data.length; k++) {
        var seriePoint = [];
        var now = new Date(data[k].Fecha);
        var nowUTC = Date.UTC(now.getFullYear(), now.getMonth(), now.getDate(), now.getHours(), now.getMinutes(), now.getSeconds());
        seriePoint.push(nowUTC);
        seriePoint.push(data[k].Valor);
        series.push(seriePoint);
    }

    $('#chartIndicador').highcharts('StockChart', {
        rangeSelector: {
            buttons: [
                {
                    count: 30,
                    type: 'second',
                    text: '30S'
                },
                {
                    count: 1,
                    type: 'minute',
                    text: '1M'
                }
            ],
            selected: 1,
            allButtonsEnabled: false,
            visible: false,
            enabled: false
        },
        xAxis: {
            type: 'datetime'
        },
        yAxis: {
            max: 60.36,
            min: 59.64,
            labels: {
                formatter: function () {
                    return this.value + ' Hz';
                }
            },
            plotLines: [{
                value: 60.36,
                color: 'red',
                dashStyle: 'shortdash',
                width: 1,
                label: {
                    text: 'Frecuencia Maxima 60,36'
                }
            }, {
                value: 59.64,
                color: 'red',
                dashStyle: 'shortdash',
                width: 1,
                label: {
                    text: 'Frecuencia Minima 59,64'
                }
            }]
        },
        legend: {
            layout: 'horizontal',
            align: 'center',
            verticalAlign: 'top',
            borderWidth: 0,
            enabled: true,
            itemDistance: 2
        },
        tooltip: {
            pointFormat: '<b>{point.y} Hz</b><br/>',
            valueDecimals: 3
        },
        series: [{
            name: 'Frecuencia Tiempo Real',
            data: series,
            showInLegend: false,
        }]
    });

    //setInterval(function () {
    //    updateFrecuencia();
    //}, 1000);
};

actualizarGraficoFrecuencia = function (data) {

    if (data != null) {
        if (data.length > 0) {
            var actual = new Date(data[data.length - 1].Fecha);
            var fecha = new Date(actual.getFullYear(), actual.getMonth(), actual.getDate(), actual.getHours(), actual.getMinutes(), actual.getSeconds());
            $('#resumenIndicador').html("<div class='grafl'><strong>Frecuencia Actual:</strong><br /> " + fecha.format("dd/mm/yyyy HH:MM:ss")
                + "</div><div class='grafr'>" + $.number(data[data.length - 1].Valor, 3, ',', ' ') + "<br/> Hz</div>");
            //var seriesFrecuencia = window.Highcharts.charts[window.Highcharts.charts.length - 3].series[0];
            var seriesFrecuencia = window.Highcharts.charts[0].series[0];
            for (var k in data) {
                var now = new Date(data[k].Fecha);
                var fecha = Date.UTC(now.getFullYear(), now.getMonth(), now.getDate(), now.getHours(), now.getMinutes(), now.getSeconds());
                seriesFrecuencia.addPoint([fecha, data[k].Valor], true, false);
            }
        }
    }
}

/*-------Demanda--------*/

obtenerDemanda = function () {
    $.ajax({
        type: 'POST',
        url: controlador + "demanda",
        datatype: 'json',
        success: function (result) {
            graficarDemanda(result);
            obtenerProduccion();
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

graficarDemanda = function (result) {

    $('#resumenIndicadorDemanda').html("<div class='grafl'><strong>Demanda media horaria ejecutada:</strong><br /> " + result.LastDate
        + "</div><div class='grafr'>" + $.number(result.LastValue, 3, ',', ' ') + "<br /> MW</div>");
    var data = result.Chart;

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

    $('#chartIndicadorDemanda').highcharts('StockChart', {
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
            pointFormat: '<span style="color:{series.color}">{series.name}</span>: <b>{point.y} ' + "MW" + '</b><br/>',
            valueDecimals: 2
        },
        colors: ['#33B8FF', '#3DD19D', '#FF7A33'],
        series: seriesOptions
    });
}

/*-------Producción--------*/

obtenerProduccion = function () {
    $.ajax({
        type: 'POST',
        url: controlador + "produccion",
        datatype: 'json',
        success: function (result) {
            graficarProduccion(result);
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

graficarProduccion = function (result) {

    $('#resumenIndicadorProduccion').html("<div class='grafl'><strong>Producción Acumulada:</strong><br />" + result.LastDate
        + "</div><div class='grafr'>" + $.number(result.LastValue, 3, ',', ' ') + "<br /> MWh</div>");
    var data = result.Data;

    var dataPie = [];

    for (var i = 0; i < data.length; i++) {
        var piePoint = { name: data[i].Nombre, y: parseFloat(data[i].Valor), color: getColorCombustible(data[i].Nombre) };
        dataPie.push(piePoint);
    }
    console.log(data);
    $('#chartIndicadorProduccion').highcharts({
        chart: {
            plotBackgroundColor: null,
            plotBorderWidth: null,
            plotShadow: false
        },
        title: {
            text: 'title',
            style: {
                color: '#fff'
            }
        },
        tooltip: {
            pointFormat: '<span>{series.name}</span>: <b>{point.y} MWh</b><br/>',
            valueDecimals: 2
        },
        plotOptions: {
            pie: {
                allowPointSelect: false,
                cursor: 'pointer',
                dataLabels: {
                    enabled: true,
                    color: '#000000',
                    connectorColor: '#000000',
                    format: '{point.percentage:.1f} %'
                },
                showInLegend: true,
                events: {
                    click: function (event) {
                        document.location.href = "/portal/portalinformacion/generacion";
                    }
                }
            }
        },
        series: [{
            type: 'pie',
            name: 'Producción',
            data: dataPie
        }]
    });
}

/*--------Agenda----------*/

openAgenda = function () {
    $.ajax({
        type: 'POST',
        url: controlador + 'anexos',
        global: false,
        success: function (evt) {
            $('#contenidoAgenda').html(evt);
            setTimeout(function () {
                $('#popupAgenda').bPopup({
                    autoClose: false
                });
            }, 50);
        },
        error: function () {

        }
    });
}