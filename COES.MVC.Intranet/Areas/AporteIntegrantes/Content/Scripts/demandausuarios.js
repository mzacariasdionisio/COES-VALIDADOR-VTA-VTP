var sControlador = siteRoot + "AporteIntegrantes/DemandaUsuarios/";
var sControladorDemandaMaxima = siteRoot + 'demandaMaxima/Informacion/';

$(document).ready(function () {
    $('#btnCopiarDU').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        copiarDU();
    });

    $('#btnExportarDU').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        exportarDU(1);
    });

});

exportarDU = function (formato) {
    //var formato = "1";
    var empresas = "";
    console.log($('#sPeriodoInicio').val());
    if ($('#sPeriodoInicio').val() != "" && $('#sPeriodoFinal').val() != "") {
        $.ajax({
            type: 'POST',
            url: sControladorDemandaMaxima + 'exportar',
            data: {
                empresas: empresas,
                tipos: 4,
                ini: $('#sPeriodoInicio').val(),
                fin: $('#sPeriodoFinal').val(),
                nivel: 15,
                max: 0
            },
            dataType: 'json',
            success: function (result) {
                if (result != -1) {
                    document.location.href = sControladorDemandaMaxima + 'descargar?formato=' + formato + '&file=' + result
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
    else {
        mostrarAlerta("Seleccione rango de fechas del periodo.");
    }
}

copiarDU = function () {
    var caiprscodi = document.getElementById('caiprscodi').value;
    var caiajcodi = document.getElementById('caiajcodi').value;
    if (caiajcodi == "") {
        mostrarError("Por favor, seleccionar una versión de recalculo");
    }
    else { 
        $.ajax({
            type: 'POST',
            url: sControlador + 'copiarDU',
            data: { caiprscodi: caiprscodi, caiajcodi: caiajcodi},
            dataType: 'json',
            success: function (model) {
                if (model.sError == "") {
                    mostrarExito("Felicidades, se copio correctamente la información de los Usuario Libres de " + model.sMensaje);
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