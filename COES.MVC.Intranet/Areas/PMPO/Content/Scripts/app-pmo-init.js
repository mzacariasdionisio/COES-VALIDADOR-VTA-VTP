function mostrarError() {
    $('#mensaje').removeClass();
    $('#mensaje').addClass("action-error");
    $('#mensaje').text("Ha ocurrido un error");
}

function mostrarExitoOperacion(mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').addClass("action-exito");
    $('#mensaje').text(mensaje);
}

function mostrarAlerta(mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').addClass("action-alert");
    $('#mensaje').text(mensaje);
}