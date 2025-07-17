var controlador = siteRoot + 'Combustibles/reporteGas/';

$(function () {

    $('#cbEmpresa').multipleSelect({
        width: '250px',
        filter: true,
    });

    $('#cbEmpresa').multipleSelect('checkAll');

    $('#fechaDesde').Zebra_DatePicker({
        format: "m-Y",
        pair: $('#fechaHasta'),
        direction: false,        
    });

    $('#fechaHasta').Zebra_DatePicker({
        format: "m-Y",
        pair: $('#fechaDesde'),
        direction: 0,        
    });

    $('#btnBuscar').click(function () {
        mostrarReporte();
    });

    $('#btnExportar').click(function () {
        exportarReporteCumplimiento();
    });

    mostrarReporte();
});



function mostrarReporte(tipoReporte) {
    limpiarBarraMensaje("mensaje");  

    var filtro = datosFiltro();
    var msg = validarDatosFiltro(filtro);

    if (msg == "") {
        $.ajax({
            type: 'POST',
            url: controlador + "obtenerListadoLogEnvios",
            dataType: 'json',
            data: {
                empresas: $('#hfEmpresa').val(),
                rangoIni: filtro.rangoIni,
                rangoFin: filtro.rangoFin
            },
            success: function (evt) {               
                if (evt.Resultado != "-1") {
                    var numRegistros = evt.NumRegistros;
                    var html = evt.HtmlLogEnvios;
                    $("#listadoReporte").html(html);

                    var tam = (parseInt($('#mainLayout').width()) );
                    $('#listado').css("width", tam + "px");
                   

                } else {
                    mostrarMensaje('mensaje', 'error', evt.Mensaje);
                }
            },
            error: function (err) {
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
            }
        });
    } else {
        mostrarMensaje('mensaje', 'error', msg);
    }
}

function datosFiltro() {
    var filtro = {};

    var empresa = $('#cbEmpresa').multipleSelect('getSelects');    
    if (empresa == "[object Object]") empresa = "-1";
    if (empresa == "") empresa = "-1";
    $('#hfEmpresa').val(empresa);

    var rangoIni = $("#fechaDesde").val();
    var rangoFin = $("#fechaHasta").val();

    filtro.empresa = empresa;
    filtro.rangoIni = rangoIni;
    filtro.rangoFin = rangoFin;

    return filtro;
}

function validarDatosFiltro(datos) {
    var msj = "";

    if (datos.empresa.length == 0) {
        msj += "<p>Debe escoger una empresa correcta.</p>";
    }

    if (datos.rangoIni == "") {
        if (datos.rangoFin == "") {
            msj += "<p>Debe escoger un rango inicial y final correctos.</p>";
        } else {
            msj += "<p>Debe escoger una fecha inicial correcta.</p>";
        }
    } else {
        if (datos.rangoFin == "") {
            msj += "<p>Debe escoger una fecha final correcta.</p>";
        }
        else {
            if (convertirFecha(datos.rangoIni) > convertirFecha(datos.rangoFin)) {
                msj += "<p>Debe escoger un rango correcto, la fecha final debe ser posterior o igual a la fecha inicial.</p>";
            }
        }
    }

    return msj;
}

function convertirFecha(fecha) {
    var arrayFecha = fecha.split('-');
    var mes = arrayFecha[0];
    var anio = arrayFecha[1];

    var salida = anio + mes;
    return salida;
}


function exportarReporteCumplimiento() {
    limpiarBarraMensaje("mensaje");

    var filtro = datosFiltro();
    var msg = validarDatosFiltro(filtro);

    if (msg == "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'ExportarReporteCumplimiento',
            data: {
                empresas: $('#hfEmpresa').val(),
                rangoIni: filtro.rangoIni,
                rangoFin: filtro.rangoFin
            },
            dataType: 'json',
            success: function (evt) {
                if (evt.Resultado != "-1") {
                    window.location = controlador + "Exportar?file_name=" + evt.Resultado;
                } else {
                    mostrarMensaje('mensaje', 'error', evt.Mensaje);
                }
            },
            error: function (err) {
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
            }
        });
    } else {
        mostrarMensaje('mensaje', 'error', msg);
    }
    
}

////////////////////////////////////////////////
// Util
////////////////////////////////////////////////
function cerrarPopup(id) {
    $("#" + id).bPopup().close()
}

function abrirPopup(contentPopup) {
    setTimeout(function () {
        $("#" + contentPopup).bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
            modalClose: false
        });
    }, 50);
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