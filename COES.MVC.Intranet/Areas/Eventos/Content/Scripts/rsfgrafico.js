var controlador = siteRoot + 'eventos/rsf/';

$(function () {
    consultar();
    graficar();
});


consultar = function () {
    $.ajax({
        type: "POST",
        url: controlador + 'graficodetalle',         
        data: {
            fecha: $('#hfFecha').val()
        },
        success: function (evt) {
            $('#contenedor').html(evt);            
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
};

graficar = function () {
    $.ajax({
        type: 'POST',
        url: controlador + 'ObtenerGraficoPrecio',
        data: {
            fecha: $('#hfFecha').val()
        },
        dataType: 'json',
        success: function (result) {
            pintarGrafico(result);
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
};


pintarGrafico = function (result) {

    var data = [];

    for (var i in result) {
        data.push({ name: result[i].Nombre, data: result[i].ListaValores });
    }

    Highcharts.chart('contenedorGrafico', {
        title: {
            text: 'Precios Ofertados por URS'
        },
        yAxis: {
            title: {
                text: 'S/ / MW-mes'
            }
        },
        xAxis: {
            //accessibility: {
            //    rangeDescription: 'Range: 2010 to 2017'
            //}
        },
        legend: {
        },
        plotOptions: {
            series: {
                label: {
                },
                pointStart: 0
            }
        },
        series: data
    });
};

mostrarMensaje = function (id, tipo, mensaje) {
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);
};



