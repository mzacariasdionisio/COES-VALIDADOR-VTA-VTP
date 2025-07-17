var controlador = siteRoot + 'Combustibles/reporteGas/';

$(function () {

    $('#cbEmpresa').multipleSelect({
        width: '250px',
        filter: true,
    });

    $('#cbEstado').multipleSelect({
        width: '250px',
        filter: true,
    });

    $('#cbEmpresa').multipleSelect('checkAll');
    $('#cbEstado').multipleSelect('checkAll');

    $('#FechaDesde').Zebra_DatePicker({
        format: "m-Y",
        //pair: $('#FechaHasta'),
        //direction: -1,
    });

    $('#FechaHasta').Zebra_DatePicker({
        format: "m-Y",
        //pair: $('#FechaDesde'),
        //direction: -1,
    });

    setFechas();

    $('#btnExportar').click(function () {
        DescargarReporte();
    });

});

function DescargarReporte() {

    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    var estado = $('#cbEstado').multipleSelect('getSelects');
    if (empresa == "[object Object]") empresa = "-1";
    if (estado == "[object Object]") empresa = "-1";
    if (empresa == "") empresa = "-1";
    if (estado == "") estado = "-1";

    $('#hfEmpresa').val(empresa);
    $('#hfEstado').val(estado);

    var msj = validarDatos();
    if (msj == "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'ExportarReporteParticipanteGen',
            data: {
                empresas: $('#hfEmpresa').val(),
                estados: $('#hfEstado').val(),
                finicios: $('#FechaDesde').val(),
                ffins: $('#FechaHasta').val(),
                tipoArchivo: $('#cbArchivo').val()
            },
            dataType: 'json',
            success: function (evt) {
                if (evt.Resultado != "-1") {
                    limpiarBarraMensaje("mensaje");
                    window.location = controlador + "ExportarZipParticipanteGen?file_name=" + evt.Resultado;
                } else {
                    alert("Ha ocurrido un error: " + evt.Mensaje);
                }
            },
            error: function (err) {
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
            }
        });
    }
    else {
        mostrarMensaje('mensaje', 'alert', msj);
    }
}


//validar consulta
function validarDatos() {
    var validacion = "<ul>";
    var flag = true;

    if ($('#FechaHasta').val() < $('#FechaDesde').val()) {
        validacion = validacion + "<li>Fecha Hasta: debe ser mayor a la fecha inicio .</li>";//Campo Requerido
        flag = false;
    }
    if ($('#cbArchivo').val() == 0 ) {
        validacion = validacion + "<li>Archivos: debe seleccionar una opción válida.</li>";//Campo Requerido
        flag = false;
    }

    validacion = validacion + "</ul>";

    if (flag == true) validacion = "";

    return validacion;
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

function setFechas() {
    var fecha = new Date();
    var fechafin = new Date();
    var stFecha = obtenerFechaMes(fecha, 0);
    var stFechaFin = obtenerFechaMes(fechafin, 1);
    $('#FechaDesde').val(stFecha);
    $('#FechaHasta').val(stFecha);

    $('#FechaDesde').Zebra_DatePicker({
        format: "m-Y",
        pair: $('#FechaHasta'),
        direction: ["01-2000", stFechaFin]
    });

    $('#FechaHasta').Zebra_DatePicker({
        format: "m-Y",
        //pair: $('#FechaDesde'),
        direction: [true, stFechaFin]
    });
}

function obtenerFechaMes(fecha , numero ) {
    fecha = new Date(fecha.setMonth(fecha.getMonth() + numero));
    var mesfin = "0" + (fecha.getMonth() + 1).toString();
    mesfin = mesfin.substr(mesfin.length - 2, mesfin.length);
    var stFecha = mesfin + "-" + fecha.getFullYear();

    return stFecha;
}