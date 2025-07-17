/* ------------------------------------------------------------------------
Class: util
Uso: Contiene funciones genéricas
Fecha creación: 28.10.2015
Version: 1.0.0
-------------------------------------------------------------------------*/

$mensajeExitoGeneral = "La operación se ha ejecutado exitósamente";
$mensajeErrorGeneral = "Se ha producido un error, por favor inténtelo nuevamente.";

//Tipos de mensajes
$tipoMensajeError = 1;
$tipoMensajeExito = 2;
$tipoMensajeAlerta = 3;
$tipoMensajeMensaje = 4;

//Modo en que se visualizan los mensajes
$modoMensajePopup = 1;
$modoMensajeCuadro = 2;


mostrarMensaje = function (identificador, mensaje, tipo, modo) {

    if (modo == $modoMensajeCuadro) {

        var estilo = "";

        $('#' + identificador).removeClass();
        $('#' + identificador).html(mensaje);

        switch (tipo) {
            case $tipoMensajeError:
                {
                    estilo = "action-error";
                    break;
                }
            case $tipoMensajeExito:
                {
                    estilo = "action-exito";
                    break;
                }
            case $tipoMensajeAlerta:
                {
                    estilo = "action-alert";
                    break;
                }
            case $tipoMensajeMensaje:
                {
                    estilo = "action-message";
                    break;
                }
        }

        $('#' + identificador).addClass(estilo);
    }
}

mostrarConfirmacion = function () {

    mensaje = arguments[0];
    operacion = arguments[1];
    var parametros = [];

    if (arguments.length > 2) {

        for (var i = 2; i < arguments.length; i++) {
            parametros.push(arguments[i]);
        }

        if (mensaje == '') mensaje = 'Por favor confirme la operación';
        $('#mensajeConfirmacion').html(mensaje);
        $('#popupConfirmarOperacion').bPopup({
        });

        $("#btnAceptarConfirmacion").unbind("click");
        $('#btnAceptarConfirmacion').click(function () {
            operacion(parametros);
        });

        $('#btnCancelarConfirmacion').click(function () {
            $('#popupConfirmarOperacion').bPopup().close();
        })
    }
}

validarDecimal = function (n) {
    if (n == "")
        return false;

    var count = 0;
    var strCheck = "0123456789.";
    var i;

    for (i in n) {
        if (strCheck.indexOf(n[i]) == -1)
            return false;
        else {
            if (n[i] == '.') {
                count = count + 1;
            }
        }
    }
    if (count > 1) return false;
    return true;
}

validarHora = function (hora) {
    if (!hora || hora.length < 1) { return false; }
    var time = hora.split(':');
    return (time.length === 2
           && parseInt(time[0], 10) >= 0
           && parseInt(time[0], 10) <= 23
           && parseInt(time[1], 10) >= 0
           && parseInt(time[1], 10) <= 59) ||
           (time.length === 3
           && parseInt(time[0], 10) >= 0
           && parseInt(time[0], 10) <= 23
           && parseInt(time[1], 10) >= 0
           && parseInt(time[1], 10) <= 59
           && parseInt(time[2], 10) >= 0
           && parseInt(time[2], 10) <= 59)
}

validarEmail = function (email) {
    var regex = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    return regex.test(email);
}

getFecha = function (date) {
    var parts = date.split("/");
    var date = new Date(parts[1] + "/" + parts[0] + "/" + parts[2]);
    return date.getTime();
}
validarNumero = function (item, evt) {
    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;

    if (charCode == 46) {
        var regex = new RegExp(/\./g)
        var count = $(item).val().match(regex).length;
        if (count > 1) {
            return false;
        }
    }

    if (charCode == 45) {
        var regex = new RegExp(/\-/g)
        var count = $(item).val().match(regex).length;
        if (count > 0) {
            return false;
        }
    }
    if (charCode > 31 && charCode != 45 && (charCode < 48 || charCode > 57)) {

        return false;
    }

    return true;
}


$(document).on("keypress", '.numeroDecimal', function (a) {

    return validarNumero(a.currentTarget, a);
});