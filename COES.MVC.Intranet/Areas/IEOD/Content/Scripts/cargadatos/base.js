function loadInfoFile(id, fileName, fileSize) {
    $('#' + id).html(fileName + " (" + fileSize + ")");
    $('#' + id).removeClass('action-alert');
    $('#' + id).addClass('action-exito');
    $('#' + id).css('margin-bottom', '10px');
}

function loadValidacionFile(id, mensaje) {
    $('#' + id).html(mensaje);
    $('#' + id).removeClass('action-exito');
    $('#' + id).addClass('action-alert');
    $('#' + id).css('margin-bottom', '10px');
}

function mostrarProgreso(id, porcentaje) {
    $('#' + id).text(porcentaje + "%");
}

function mostrarMensajeDefecto() {
    mostrarMensaje('mensaje', 'message', 'Complete los datos solicitados.');
}

function mostrarMensaje(id, tipo, mensaje) {
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);
}

function limpiarMensaje(id) {
    $('#' + id).removeClass();
    $('#' + id).html('');
}

function getFecha(date) {
    var parts = date.split("/");
    var date1 = new Date(parts[1] + "/" + parts[0] + "/" + parts[2]);
    return date1.getTime();
}

function validarNumero(texto) {
    return /^-?[\d.]+(?:e-?\d+)?$/.test(texto);
}

function validarEntero(texto) {
    return /^-?[0-9]+$/.test(texto);
}
