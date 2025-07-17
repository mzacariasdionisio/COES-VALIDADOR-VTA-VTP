function mostrarmensajes(enPlazo,idEnvio,fechaEnvio)
{
    //Mensaje descriptivo del envio
    //console.log(idEnvio);
    var envio = $("#hfIdEnvio").val();
    hideMensaje();
    hideMensajeEvento();
    hideMsgFueraPlazo();
    if (idEnvio > 0) {
        var plazo = (enPlazo) ? "en plazo" : "fuera de plazo";
        var mensaje = "Código de envío : " + idEnvio + "   , Fecha de envío: " + fechaEnvio + "   , Enviado " + plazo;
        mostrarMensaje(mensaje);
    }
    else {
        if (!enPlazo) {
            mostrarMsgFueraPlazo();
        }
    }
}

function mostrarAlerta(alerta) {
    $('#mensajeEvento').removeClass("action-message");
    $('#mensajeEvento').removeClass("action-exito");
    $('#mensajeEvento').addClass("action-alert");
    $('#mensajeEvento').html(alerta);
    $('#mensajeEvento').css("display", "block");
}

function mostrarMensaje(mensaje) {
    $('#mensaje').removeClass("action-exito");
    $('#mensaje').removeClass("action-alert");
    $('#mensaje').addClass("action-exito");
    $('#mensaje').html(mensaje);
    $('#mensaje').css("display", "block");
}

function mostrarEvento(mensaje) {
    $('#mensajeEvento').removeClass("action-error");
    $('#mensajeEvento').removeClass("action-exito");
    $('#mensajeEvento').removeClass("action-alert");
    $('#mensajeEvento').addClass("action-message");
    $('#mensajeEvento').html(mensaje);
    $('#mensajeEvento').css("display", "block");
}

function mostrarExito(mensaje) {
    $('#mensajeEvento').removeClass("action-error");
    $('#mensajeEvento').removeClass("action-alert");
    $('#mensajeEvento').removeClass("action-message");
    $('#mensajeEvento').addClass("action-exito");
    $('#mensajeEvento').html(mensaje);
    $('#mensajeEvento').css("display", "block");
}

function mostrarError(alerta) {
    $('#mensajeEvento').removeClass("action-message");
    $('#mensajeEvento').removeClass("action-alert");
    $('#mensajeEvento').removeClass("action-exito");
    $('#mensajeEvento').addClass("action-error");
    $('#mensajeEvento').html(alerta);
    $('#mensajeEvento').css("display", "block");
}

function hideMensaje() {

    $('#mensaje').css("display", "none");
}

function hideMensajeEvento() {

    $('#mensajeEvento').css("display", "none");
}

function mostrarMsgFueraPlazo() {
    $('#mensajePlazo').html("Formato Fuera de Plazo");
    $('#mensajePlazo').css("display", "block");
}

function hideMsgFueraPlazo() {
    $('#mensajePlazo').css("display", "none");
}
