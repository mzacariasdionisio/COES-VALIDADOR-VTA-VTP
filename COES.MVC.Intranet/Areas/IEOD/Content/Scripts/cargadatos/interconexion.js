var controlador = siteRoot + 'ieod/cargadatos/';

$(function () {

    $('#txtFechaConsulta').Zebra_DatePicker({
    });

    $('#btnConsultar').on('click', function () {
        consultar();
    });

    $('#btnDescargarFormato').on('click', function () {
        descargarFormato();
    });

    $('#btnGrafico').on('click', function () {
        mostrarGrafico();
    });
});

function consultar() {
    mostrarMensajeDefecto();

    if ($('#txtFechaConsulta').val() != "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'ObtenerInterconexiones',
            data: {                
                fecha: $('#txtFechaConsulta').val()
            },
            dataType: 'json',
            success: function (result) {
                if (result.Result == 1) {
               
                    
                    cargarGrillaConsultaInterconexion(result);

                    if (result.ExisteDatos != -1) {                      
                        pintarGraficoInterconexion(result);
                        $('#hfDatos').val("1");
                    }
                    else {
                        mostrarMensaje('mensaje', 'alert', 'Para la fecha seleccionada no existen datos.');
                        $('#hfDatos').val("-1");
                    }
                }
                else {
                    mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
                }
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            }
        });
    }
    else {
        mostrarMensaje('mensaje', 'alert', 'Debe seleccionar fecha de consulta.');
    }
}

function descargarFormato() {
    $.ajax({
        type: 'POST',
        url: controlador + "ExportarInterconexion",
        data: {            
            fecha: $('#txtFechaConsulta').val()
        },
        dataType: 'json',
        cache: false,
        success: function (resultado) {
            if (resultado == 1) {
                location.href = controlador + "DescargarInterconexion";
            }
            else {
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
            }
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
}

function mostrarGrafico() {
    $('#popupGrafico').bPopup({});

    if ($('#hfDatos').val() == "-1") {
        mostrarMensaje('mensajeGrafico', 'alert', 'Para la fecha seleccionada no existen datos.');
    }
    else {
        limpiarMensaje('mensajeGrafico');
    }
  
}

function pintarGraficoInterconexion(result) {
    var colIniSN = result.ListaMerge[1].Col;
    var colFinSN = result.ListaMerge[1].Col + result.ListaMerge[1].Colspan - 2;
    var colIniSC = result.ListaMerge[2].Col;
    var colFinSC = result.ListaMerge[2].Col + result.ListaMerge[1].Colspan - 2;
    var serieSN = [];
    var serieSC = [];

    for (var i = colIniSN; i <= colFinSN; i++) {
        var data = [];
        for (var j = 0; j < 48; j++) {
            if (result.Data[4 + j][i]!="")
                data.push(parseFloat(result.Data[4 + j][i]));
            else
                data.push(0);
        }
              
        serieSN.push({ name: result.Data[3][i], data: data });
    }

    for (var i = colIniSC; i <= colFinSC; i++) {
        var data = [];
        for (var j = 0; j < 48; j++) {
            if (result.Data[4 + j][i] != "")
                data.push(parseFloat(result.Data[4 + j][i]));
            else
                data.push(0);
        }        
        serieSC.push({ name: result.Data[3][i], data: data });
    }

    showGrafico('graficoCN', 'INTERCONEXIÓN ENTRE SISTEMAS OPERATIVOS \n CENTRO - NORTE', serieSN);
    showGrafico('graficoCS', 'INTERCONEXIÓN ENTRE SISTEMAS OPERATIVOS \n CENTRO - SUR', serieSC);

}

function showGrafico(container, titulo, series) {
   
    Highcharts.chart(container, {
        chart: {
            type: 'column'
        },
        title: {
            text: titulo,
            align: 'center'
        },
        xAxis: {
            categories: [
                '00:30', '01:00', '01:30', '02:00', '02:30', '03:00', '03:30', '04:00',
                '04:30', '05:00', '05:30', '06:00', '06:30', '07:00', '07:30', '08:00',
                '08:30', '09:00', '09:30', '10:00', '10:30', '11:00', '11:30', '12:00',
                '12:30', '13:00', '13:30', '14:00', '14:30', '15:00', '15:30', '16:00',
                '16:30', '17:00', '17:30', '18:00', '18:30', '19:00', '19:30', '20:00',
                '20:30', '21:00', '21:30', '22:00', '22:30', '23:00', '23:30', '00:00'
            ]
        },
        yAxis: {           
            title: {
                text: ''
            }           
        },
       
        tooltip: {
            headerFormat: '<b>{point.x}</b><br/>',
            pointFormat: '{series.name}: {point.y}<br/>Total: {point.stackTotal}'
        },
        plotOptions: {
            column: {
                stacking: 'normal',                
            }
        },
        series: series
    });
}

function limpiarMensaje(id) {
    $('#' + id).removeClass();
    $('#' + id).html('');
}