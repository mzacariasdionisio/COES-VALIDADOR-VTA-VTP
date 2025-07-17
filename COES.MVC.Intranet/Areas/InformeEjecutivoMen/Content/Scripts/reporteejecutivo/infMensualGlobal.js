function getFechaInicio() {
    var fechaInicio = $('#PickerMes').val();
    fechaInicio = fechaInicio + "-01";
    return fechaInicio;
}

/// Funciones para obtener el valor de los inputs
function getCodigoVersion() {
    var valor = $('#cboVersion').val();
    return valor || 0;
}

//Obtener formato html
function getHtmlSaltoLinea(str) {
    return str.replace(/(?:\r\n|\r|\n)/g, '<br>');
}