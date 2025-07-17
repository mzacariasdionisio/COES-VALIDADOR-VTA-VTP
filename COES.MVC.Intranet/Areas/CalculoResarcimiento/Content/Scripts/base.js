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

function validarFecha(fecha) {
    return /^(0[1-9]|[12][0-9]|3[01])[- /.](0[1-9]|1[012])[- /.](19|20)\d\d$/.test(fecha);
}

function validarExcelPorcentaje(texto) {
    if (texto == "") return true;
    if (validarNumero(texto)) {
        if (parseFloat(texto) >= 0 && parseFloat(texto) <= 100)
            return true;
    }
    return false;
}