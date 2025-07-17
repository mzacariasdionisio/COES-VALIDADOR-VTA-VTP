var sControlador = siteRoot + "compensacionrsf/reservaasignada/";

$(document).ready(function () {
    $('#btnCopiarPM').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        copiarPM();
    });

    $('#btnExportarPM').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        exportarPM(1);
    });

    //$('#btnRefrescar').click(function () {
    //    refrescar();
    //});
});

exportarPM = function (formato) {
    var pericodi = document.getElementById('pericodi').value;
    $.ajax({
        type: 'POST',
        url: sControlador + 'exportarPM',
        data: { pericodi: pericodi },
        dataType: 'json',
        success: function (result) {
            if (result != -1) {
                window.location = sControlador + 'abrirarchivo?formato=' + formato + '&file=' + result;
                mostrarExito("Felicidades, el archivo se descargo correctamente...!");
            }
            else {
                mostrarMensaje('Ha ocurrido un error');
            }
        },
        error: function () {
            mostrarMensaje('Ha ocurrido un error');
        }
    });
}

copiarPM = function () {
    var pericodi = document.getElementById('pericodi').value;
    var vcrecacodi = document.getElementById('vcrecacodi').value;
    $.ajax({
        type: 'POST',
        url: sControlador + 'copiarPM',
        data: { pericodi: pericodi, vcrecacodi: vcrecacodi},
        dataType: 'json',
        success: function (model) {
            if (model.sError == "") {
                mostrarExito("Felicidades, se copio correctamente " + model.iNumReg + " registros.");
            }
            else {
                mostrarError("Lo sentimos, ha ocurrido el siguiente error: " + model.sError);
            }
        },
        error: function () {
            mostrarMensaje('mensaje', 'Ha ocurrido un error', $tipoMensajeError, $modoMensajeCuadro);
        }
    });
}

refrescar = function () {
    var cmbPericodi = document.getElementById('pericodi');
    window.location.href = sControlador + "index?pericodi=" + cmbPericodi.value;
}