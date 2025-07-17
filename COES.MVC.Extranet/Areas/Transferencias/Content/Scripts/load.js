abrirPopup = function () {
    $('#popup').bPopup({
        easing: 'easeOutBack',
        speed: 450,
        transition: 'slideDown'
    });
}

generarExcel = function () {
    $.ajax({
        type: 'POST',
        url: controlador + 'generarexcel',
        dataType: 'json',
        success: function (result) {
            if (result == 1) {
                window.location = controlador + 'abrirexcel';
            }
            if (result == -1) {
                mostrarError();
            }
        },
        error: function () {
            mostrarError();
        }
    });
}

generarWord = function () {
    $.ajax({
        type: 'POST',
        url: controlador + 'generarword',
        dataType: 'json',
        success: function (result) {
            if (result == 1) {
                window.location = controlador + 'abrirword';
            }
            if (result == -1) {
                mostrarError();
            }
        },
        error: function () {
            mostrarError();
        }
    });
}

generarPdf = function () {
    $.ajax({
        type: 'POST',
        url: controlador + 'generarpdf',
        dataType: 'json',
        success: function (result) {
            if (result == 1) {
                window.location = controlador + 'abrirpdf';
            }
            if (result == -1) {
                mostrarError();
            }
        },
        error: function () {
            mostrarError();
        }
    });
}

loadInfoFile = function (fileName, fileSize) {
    $('#fileInfo').html(fileName + " (" + fileSize + ")");
}

loadValidacionFile = function (mensaje) {
    $('#fileInfo').html(mensaje);
}

mostrarProgreso = function (porcentaje) {
    $('#progreso').text(porcentaje + "%");
}

mostrarError = function () {
    alert("Ha ocurrido un error.");
}

mostrarMensaje = function (mensaje) {
    alert(mensaje);
}

mostrarExito = function (mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-exito');
}

mostrarAlerta = function (mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-alert');
}