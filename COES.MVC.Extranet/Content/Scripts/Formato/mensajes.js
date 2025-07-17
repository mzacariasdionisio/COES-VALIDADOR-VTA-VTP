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

function mostrarMensajeEnvioFuenteDatos(evtHot) {
    var envio = $("#hfIdEnvio").val();
    if (envio > 0) {
        var plazo = (evtHot.PlazoEnvio.TipoPlazo == "P") ? "<strong style='color:green'>En plazo</strong>" : "<strong style='color:red'>Fuera de plazo</strong>";
        var mensaje = "<strong>Código de envío</strong>: " + evtHot.PlazoEnvio.IdEnvio + ", <strong>Fecha de envío: </strong>" + evtHot.FechaEnvio + ", <strong>Enviado </strong>" + plazo;
        return mensaje;
    }
    else {
        var esEmpresaVigente = evtHot.EsEmpresaVigente;
        if (esEmpresaVigente) {
            if (evtHot.PlazoEnvio.TipoPlazo != "D") {
                var mensaje = "<strong style='color:green'>En plazo</strong>";;
                if (evtHot.PlazoEnvio.TipoPlazo != "P") mensaje = "<strong style='color:red'>Fuera de plazo</strong>";
                return "Por favor complete los datos. <strong>Plazo del Envio: </strong>" + mensaje;
            } else {
                var mensaje = "<strong style='color:blue'>Deshabilitado</strong>";
                return "No está permitido el envió de información, solo para Consulta. <strong>Plazo del Envio: </strong>" + mensaje;
            }
        } else {
            var msjNoVigente = "La empresa se encuentra <strong style='color:blue'>No Vigente</strong>.";
            return msjNoVigente + " No está permitido el envió de información, solo para consulta.";
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
