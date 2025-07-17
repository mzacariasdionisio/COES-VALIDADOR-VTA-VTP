function mostrarmensajes(enPlazo, idEnvio, fechaEnvio) {
    //Mensaje descriptivo del envio
    var envio = $("#hfIdEnvio").val();
    //hideMensaje();
    hideMensajeEvento();
    hideMsgFueraPlazo();
    if (envio > 0) {
        var plazo = (enPlazo) ? "en plazo" : "fuera de plazo";
        var mensaje = "Código de envío : " + idEnvio + "   , Fecha de envío: " + fechaEnvio + "   , Enviado en " + plazo;
        mostrarMensaje(mensaje);
    }
    else {
        if (!enPlazo) {
            mostrarMsgFueraPlazo();
        }
    }
}


mostrarMensaje = function (mensaje) {
    $('#mensaje').removeClass("action-exito");
    $('#mensaje').removeClass("action-alert");
    $('#mensaje').addClass("action-message");
    $('#mensaje').html(mensaje);
    $('#mensaje').css("display", "block");
}

mostrarError = function (alerta) {
    $('#mensajeEvento').removeClass("action-message");
    $('#mensajeEvento').removeClass("action-alert");
    $('#mensajeEvento').removeClass("action-exito");
    $('#mensajeEvento').addClass("action-error");
    $('#mensajeEvento').html(alerta);
    $('#mensajeEvento').css("display", "block");
}

mostrarExito = function (mensaje) {
    $('#mensajeEvento').removeClass("action-error");
    $('#mensajeEvento').removeClass("action-alert");
    $('#mensajeEvento').removeClass("action-message");
    $('#mensajeEvento').addClass("action-exito");
    $('#mensajeEvento').html(mensaje);
    $('#mensajeEvento').css("display", "block");
}



hideMensaje = function () {

    $('#mensaje').css("display", "none");
}

showMensaje = function () {

    $('#mensaje').css("display", "block");
}

hideMensajeEvento = function () {

    $('#mensajeEvento').css("display", "none");
}

mostrarMsgFueraPlazo = function () {
    $('#mensajePlazo').html("Formato Fuera de Plazo");
    $('#mensajePlazo').css("display", "block");
}

hideMsgFueraPlazo = function () {
    $('#mensajePlazo').css("display", "none");
}

function mostrarAlerta(mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-alert');
}
