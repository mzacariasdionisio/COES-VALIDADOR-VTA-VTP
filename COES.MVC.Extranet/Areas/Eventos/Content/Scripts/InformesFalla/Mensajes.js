function mostrarExito(mensaje) {
    $('#mensajeEvento').removeClass("action-error");
    $('#mensajeEvento').removeClass("action-alert");
    $('#mensajeEvento').removeClass("action-message");
    $('#mensajeEvento').addClass("action-exito");
    $('#mensajeEvento').html(mensaje);
    $('#mensajeEvento').css("display", "block");
    setTimeout(function () {
        $('#mensajeEvento').fadeOut('fast');
    }, 8000);
}
function mostrarAlerta(alerta) {
    $('#mensajeEvento').removeClass("action-message");
    $('#mensajeEvento').removeClass("action-exito");
    $('#mensajeEvento').addClass("action-alert");
    $('#mensajeEvento').html(alerta);
    $('#mensajeEvento').css("display", "block");
    setTimeout(function () {
        $('#mensajeEvento').fadeOut('fast');
    }, 8000);
}
function mostrarError(alerta) {
    $('#mensajeEvento').removeClass("action-message");
    $('#mensajeEvento').removeClass("action-alert");
    $('#mensajeEvento').removeClass("action-exito");
    $('#mensajeEvento').addClass("action-error");
    $('#mensajeEvento').html(alerta);
    $('#mensajeEvento').css("display", "block");
}
function mostrarAlertaUsuario(alerta) {
    $('#mensajeEventoIndex').removeClass("action-message");
    $('#mensajeEventoIndex').removeClass("action-exito");
    $('#mensajeEventoIndex').addClass("action-alert");
    $('#mensajeEventoIndex').html(alerta);
    $('#mensajeEventoIndex').css("display", "block");
    //setTimeout(function () {
    //    $('#mensajeEvento').fadeOut('fast');
    //}, 3000);
}