var controlador = siteRoot + 'Subastas/Oferta/';

var ANCHO = 900;
var HTML_ENVIO = '';

$(document).ready(function () {
    $('#dte-fechaInicial').Zebra_DatePicker({
        //direction: true, //hoy para adelante
        //select_other_months: true, //selecciona otros meses
        pair: $('#dte-fechaFinal'), //fechaFin >=  fechaIni
        onSelect: function (date) {
            $('#dte-fechaFinal').val(date);
        }
    });
    $('#dte-fechaFinal').Zebra_DatePicker({
        direction: 1, // >= FechaIni
        //select_other_months: true, //selecciona otros meses
    });

    mostrarTab();

    mostrarListado();

    $('#btn-consultar').click(function () {
        mostrarListado();
    });

    $('#btn-exportar').click(function () {        
        exportarExcel();
        
    });

    ANCHO = $("#mainLayout").width() > 900 ? $("#mainLayout").width() - 70 : 900;
}); 

function mostrarListado() {
    limpiarBarraMensaje("mensaje_error");

    var fechaInicio = $('#dte-fechaInicial').val();
    var fechaFin = $('#dte-fechaFinal').val();
    
    var diferenciaDias = obtenerDiasEntreFechas(fechaInicio, fechaFin);

    if (diferenciaDias <= 31) {
        $.ajax({
            type: 'POST',
            url: controlador + 'CargarListaReservaSecundaria',
            data: { fechaInicial: fechaInicio, fechaFinal: fechaFin },
            success: function (aData) {

                $('#hst-subasta-ingreso-subir').html(aData.ResultadoSubir);
                $('#hst-subasta-ingreso-bajar').html(aData.ResultadoBajar);

                var anchoReporte = $('#reporte-subir').width();
                $("#hst-subasta-ingreso-subir").css("width", (anchoReporte > ANCHO ? ANCHO : anchoReporte) + "px");
                $("#hst-subasta-ingreso-bajar").css("width", (anchoReporte > ANCHO ? ANCHO : anchoReporte) + "px");
                
                var tamAnchoh = parseInt($('.header').width());
                var tamA = tamAnchoh - 240;
                $('#spl-subasta_1').css("width", tamA + "px");
                $('#spl-subasta_1').css("height", "500px");
                $('#spl-subasta_1').css("overflow", "auto"); 

                $('#spl-subasta_2').css("width", tamA + "px");
                $('#spl-subasta_2').css("height", "500px");
                $('#spl-subasta_2').css("overflow", "auto"); 
            },
            error: function (err) {
                alert("Ha ocurrido un error");
            }
        });
    } else {
        
        mostrarMensaje("mensaje_error", "alert", "El máximo rango permitido para consulta es 32 días" );
        $("#hst-subasta-ingreso-subir").html("");
        $("#hst-subasta-ingreso-bajar").html("");
    }
};

function exportarExcel() {
    limpiarBarraMensaje("mensaje_error");
    var fechaInicio = $('#dte-fechaInicial').val();
    var fechaFin = $('#dte-fechaFinal').val();
    var diferenciaDias = obtenerDiasEntreFechas(fechaInicio, fechaFin);

    if (diferenciaDias <= 31) {
        $.ajax({
            type: 'POST',
            url: controlador + 'GenerarExcelReservaSecundaria',
            data: { fechaInicial: fechaInicio, fechaFinal: fechaFin },
            dataType: 'json',
            success: function (result) {
                switch (result.Resultado) {
                    case 1: window.location = controlador + "ExportarReporteXls?nameFile=" + result.Detalle; break;// si hay elementos
                    case 2: alert("No existen registros !"); break;// sino hay elementos
                    case -1: alert("Error: " + result.Mensaje); break;// Error en C#
                }
            },
            error: function (err) {
                alert("Error en la exportación del reporte");
            }
        });
    } else {
        mostrarMensaje("mensaje_error", "alert", "El máximo rango permitido para exportación es 32 días");        
    }
};

function mostrarTab() {
    $('#tab-container').easytabs();

    $('#tab-container').bind('easytabs:after', function () {
        
    });
}



function convertirFecha(fecha) {
    var arrayFecha = fecha.split('/');
    var dia = arrayFecha[0];
    var mes = arrayFecha[1];
    var anio = arrayFecha[2];

    var salida = anio + "-" + mes + "-" + dia;
    return salida;
}

function obtenerDiasEntreFechas(fechaInicio, fechaFin) {
    var ffecIni = convertirFecha(fechaInicio);
    var ffecFin = convertirFecha(fechaFin);
    var fechaInicio_ = new Date(ffecIni).getTime();
    var fechaFin_ = new Date(ffecFin).getTime();

    var diff = fechaFin_ - fechaInicio_;
    var difDias = diff / (1000 * 60 * 60 * 24);

    return difDias;
}

function mostrarMensaje(id, tipo, mensaje) {
    $("#" + id).css("display", "block");
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);

}

function limpiarBarraMensaje(id) {
    $('#' + id).css("display", "none");
    $('#' + id).removeClass();
    $('#' + id).addClass('action-message');
    $('#' + id).html('');
}