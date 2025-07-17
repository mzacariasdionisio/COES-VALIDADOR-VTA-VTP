function mostrarmensajes(enPlazo, idEnvio, fechaEnvio, tipoPlazo, habilitarEditar, esEmpresaVigente) {
    var envio = $("#hfIdEnvio").val();
    hideMensaje();
    hideMensajeEvento();
    hideMsgFueraPlazo();
    if (envio > 0) {
        var plazo = (enPlazo) ? "<strong style='color:green'>En plazo</strong>" : "<strong style='color:red'>Fuera de plazo</strong>";
        var mensaje = "<strong>Código de envío</strong>: " + envio + ", <strong>Fecha de envío:</strong> " + fechaEnvio + ", <strong>Enviado</strong> " + plazo;
        mostrarMensaje(mensaje);
    }
    else {
        if (tipoPlazo == undefined || tipoPlazo == null) {
            if (!enPlazo) {
                mostrarMsgFueraPlazo();
            } else {
                mostrarMsgEnPlazo();
            }
        } else {
            if (esEmpresaVigente) {
                switch (tipoPlazo) {
                    case ENVIO_PLAZO_DESHABILITADO: mostrarMsgDeshabilitado(); break;
                    case ENVIO_FUERA_PLAZO: mostrarMsgFueraPlazo(); break;
                    case ENVIO_EN_PLAZO: mostrarMsgEnPlazo(); break;
                }
            } else {
                mostrarMsgEmpresaNoVigente();
            }
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

function mostrarMsgEnPlazo() {
    $('#mensajePlazo').html("Por favor complete los datos. Existe tolerancia de 1 hora de acuerdo al horario seleccionado. <strong>Plazo del Envio: </strong> <strong style='color:green'>En plazo</strong>");
    $('#mensajePlazo').css("display", "block");
}

function mostrarMsgFueraPlazo() {
    $('#mensajePlazo').html("Por favor complete los datos. <strong>Plazo del Envio: </strong> <strong style='color:red'>Fuera de plazo</strong>");
    $('#mensajePlazo').css("display", "block");
}

function mostrarMsgDeshabilitado() {
    $('#mensajePlazo').html("No está permitido el envió de información, solo para consulta. <strong>Plazo del Envio: </strong> <strong style='color:blue'>Deshabilitado</strong>");
    $('#mensajePlazo').css("display", "block");
}

function mostrarMsgEmpresaNoVigente() {
    var msjNoVigente = "La empresa se encuentra <strong style='color:blue'>No Vigente</strong>.";
    msjNoVigente = msjNoVigente + " No está permitido el envió de información, solo para consulta.";
    $('#mensajePlazo').html(msjNoVigente);
    $('#mensajePlazo').css("display", "block");
}

function hideMsgFueraPlazo() {
    $('#mensajePlazo').css("display", "none");
}
