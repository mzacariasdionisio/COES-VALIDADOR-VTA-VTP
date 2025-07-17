var controlador = siteRoot + 'web/cmvstarifa/';

$(function () {
    consultar();
});

consultar = function () {
    $.ajax({
        type: 'POST',
        url: controlador + 'lista',
        success: function (evt) {
            $('#listado').html(evt);
            $('#tabla').dataTable({
            });
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
};

procesarArchivo = function () {
    $.ajax({
        type: 'POST',
        url: controlador + 'procesar',
        dataType: 'json',
        cache: false,
        success: function (resultado) {
            if (resultado == 1) {
                consultar();
                mostrarMensaje('mensaje', 'exito', 'El archivo se proceso correctamente.');
            }
            else
                mostrarMensaje('mensaje', 'error', resultado);
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
}

mostrarMensaje = function (id, tipo, mensaje) {
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);
};

function loadInfoFile(fileName, fileSize) {
    $('#fileInfo').html(fileName + " (" + fileSize + ")");
}

function loadValidacionFile(mensaje) {
    $('#fileInfo').html(mensaje);
}

function mostrarProgreso(porcentaje) {
    $('#progreso').text(porcentaje + "%");
}
