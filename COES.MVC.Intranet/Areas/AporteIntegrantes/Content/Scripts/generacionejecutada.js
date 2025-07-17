var sControlador = siteRoot + "AporteIntegrantes/GeneracionEjecutada/";

$(document).ready(function () {
    $('#btnCopiarGE').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        copiarGE();
    });

    $('#btnExportarGE').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        exportarGE(1);
    });

});

exportarGE = function (formato) {
    var caiprscodi = document.getElementById('caiprscodi').value;
    $.ajax({
        type: 'POST',
        url: sControlador + 'exportarGE',
        data: { caiprscodi: caiprscodi },
        dataType: 'json',
        success: function (result) {
            if (result == 1) {
                window.location = sControlador + 'abrirarchivo';
                mostrarExito("Felicidades, el archivo se descargo correctamente...!");
            }
            else if (result == 2) { 
                // sino hay elementos
                alert("No existen registros !");
            }
            else if (result == -1) { 
                mostrarMensaje('mensaje', 'Ha ocurrido un error', $tipoMensajeError, $modoMensajeCuadro);
            }
            else {
                mostrarMensaje('mensaje', 'Ha ocurrido un error', $tipoMensajeError, $modoMensajeCuadro);
            }
        },
        error: function () {
            mostrarMensaje('mensaje', 'Ha ocurrido un error', $tipoMensajeError, $modoMensajeCuadro);
        }
    });
}

copiarGE = function () {
    var caiprscodi = document.getElementById('caiprscodi').value;
    var caiajcodi = document.getElementById('caiajcodi').value;
    if (caiajcodi == "") {
        mostrarError("Por favor, seleccionar una versión de recalculo");
    }
    else { 
        $.ajax({
            type: 'POST',
            url: sControlador + 'copiarGE',
            data: { caiprscodi: caiprscodi, caiajcodi: caiajcodi},
            dataType: 'json',
            success: function (model) {
                if (model.sError == "") {
                    mostrarExito("Felicidades, se copio correctamente la información de " + model.iNumReg + " registros.");
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
}

refrescar = function () {
    var cmbPresupuesto = document.getElementById('caiprscodi');
    window.location.href = sControlador + "index?caiprscodi=" + cmbPresupuesto.value;
}