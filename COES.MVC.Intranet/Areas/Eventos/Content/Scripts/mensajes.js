function mostrarExito(mensaje) {
    $('#mensaje').removeClass("action-error");
    $('#mensaje').removeClass("action-alert");
    $('#mensaje').removeClass("action-message");
    $('#mensaje').addClass("action-exito");
    $('#mensaje').html(mensaje);
    $('#mensaje').css("display", "block");
}
function mostrarAlerta(alerta) {
    $('#mensaje').removeClass("action-message");
    $('#mensaje').removeClass("action-exito");
    $('#mensaje').addClass("action-alert");
    $('#mensaje').html(alerta);
    $('#mensaje').css("display", "block");
    setTimeout(function () {
        $('#mensaje').fadeOut('fast');
    }, 3500);
}
function mostrarError(alerta) {
    $('#mensaje').removeClass("action-message");
    $('#mensaje').removeClass("action-alert");
    $('#mensaje').removeClass("action-exito");
    $('#mensaje').addClass("action-error");
    $('#mensaje').html(alerta);
    $('#mensaje').css("display", "block");
}