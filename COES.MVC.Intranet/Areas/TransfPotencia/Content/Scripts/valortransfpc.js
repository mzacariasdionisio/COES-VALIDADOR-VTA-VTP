var controler = siteRoot + 'transfpotencia/valortransfpc/';
var error = [];
$(function () {
    $('#tab-container').easytabs({
        animate: false
    });

    $('#btnProcesarValorizacion').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando la Valorización de Transferencia de Potencia y Compensación");
        ProcesarValorizacion();
    });

    $('#btnBorrarValorizacion').click(function () {
        mostrarAlerta("Espere un momento, se esta procediendo a borrar los cálculos de la Valorización de Transferencia de Potencia y Compensación");
        BorrarValorizacion();
    });

    $('#btnVerificacionResultado').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando la Verificación de Valorización de Transferencia de Potencia y Compensación");
        ProcesarVerificacion();
    });

    $('#btnDescargarPeajeEgresoExcel').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        descargarArchivo('PeajeEgreso', 1);
    });

    $('#btnDescargarRSCExcel').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        descargarArchivo('RetirosSC', 1);
    });

    $('#btnDescargarPeajePagarseExcel').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        descargarArchivo('PeajePagarse', 1);
    });

    $('#btnDescargarIngresoTarifarioExcel').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        descargarArchivo('IngresoTarifario', 1);
    });

    $('#btnDescargarPeajeRecaudadoExcel').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        descargarArchivo('PeajeRecaudado', 1);
    });
    
    $('#btnDescargarPotenciaValorExcel').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        descargarArchivo('PotenciaValor', 1);
    });

    $('#btnDescargarEgresoExcel').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        descargarArchivo('Egresos', 1);
    });

    $('#btnDescargarIngresoPotenciaExcel').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        descargarArchivo('IngresoPotencia', 1);
    });

    $('#btnDescargarValorTransfPotenciaExcel').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        descargarArchivo('ValorTransfPotencia', 1);
    });

    $('#btnDescargarMatrizExcel').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        descargarArchivo('Matriz', 1);
    });

    $('#btnDescargarTodoExcel').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        descargarArchivo('General', 1);
    });

    $('#btnDescargarUnificado').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        descargarArchivo('Unificado', 1);
    });

    $('#btnMigrarSaldo').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando la migración de saldos");
        MigrarSaldo();
    });

    $('#btnMigrarVTP').click(function () {
        mostrarAlerta("Espere un momento, se esta procediendo la migración de la información de la Valorización de Transferencia de Potencia y Compensación");
        MigrarVTP();
    });

    //Botones PDF
    $('#btnDescargarRSCPdf').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        descargarArchivo('RetirosSC', 2);
    });

    $('#btnDescargarPeajePagarsePdf').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        descargarArchivo('PeajePagarse', 2);
    });

    $('#btnDescargarIngresoTarifarioPdf').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        descargarArchivo('IngresoTarifario', 2);
    });

    $('#btnDescargarPeajeRecaudadoPdf').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        descargarArchivo('PeajeRecaudado', 2);
    });

    $('#btnDescargarPotenciaValorPdf').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        descargarArchivo('PotenciaValor', 2);
    });

    $('#btnDescargarEgresoPdf').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        descargarArchivo('Egresos', 2);
    });

    $('#btnDescargarIngresoPotenciaPdf').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        descargarArchivo('IngresoPotencia', 2);
    });

    $('#btnDescargarValorTransfPotenciaPdf').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        descargarArchivo('ValorTransfPotencia', 2);
    });

    $('#btnDescargarMatrizPdf').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        descargarArchivo('Matriz', 2);
    });
});

Recargar = function () {
    var cmbPericodi = document.getElementById('pericodi');
    var cmbRecpotcodi = document.getElementById('recpotcodi');
    window.location.href = controler + "index?pericodi=" + cmbPericodi.value + "&recpotcodi=" + cmbRecpotcodi.value;
}

RecargarReporte = function () {
    var cmbPericodi = document.getElementById('pericodiReporte');
    var cmbRecpotcodi = document.getElementById('recpotcodiReporte');
    window.location.href = controler + "index?pericodi=" + cmbPericodi.value + "&recpotcodi=" + cmbRecpotcodi.value + "#paso2";
}

RecargarVerificacion = function () {
    var cmbPericodi = document.getElementById('pericodiVerificacion');
    var cmbRecpotcodi = document.getElementById('recpotcodiVerificacion');
    window.location.href = controler + "index?pericodi=" + cmbPericodi.value + "&recpotcodi=" + cmbRecpotcodi.value + "#paso4";
}

RecargarSaldo = function () {
    var cmbPericodi = document.getElementById('pericodiSaldo');
    var cmbRecpotcodi = document.getElementById('recpotcodiSaldo');
    window.location.href = controler + "index?pericodi=" + cmbPericodi.value + "&recpotcodi=" + cmbRecpotcodi.value + "#paso3";
}

ProcesarValorizacion = function () {
    $.ajax({
        type: 'POST',
        url: controler + 'procesarvalorizacion',
        data: { pericodi: $('#pericodi').val(), recpotcodi: $('#recpotcodi').val() },
        dataType: 'json',
        success: function (result) {
            if (result == "1") {
                mostrarExito('Listo...! Usted puede consultar la información calculada');
            }
            else {
                mostrarAlerta(result);
            }
        },
        error: function () {
            mostrarError("Lo sentimos, se ha producido un error");
        }
    });
}

BorrarValorizacion = function () {
    $.ajax({
        type: 'POST',
        url: controler + 'borrarvalorizacion',
        data: { pericodi: $('#pericodi').val(), recpotcodi: $('#recpotcodi').val() },
        dataType: 'json',
        success: function (result) {
            if (result == "1") {
                mostrarExito('Listo...! la información ha sido correctamente eliminada');
            }
            else {
                mostrarAlerta(result);
            }
        },
        error: function () {
            mostrarError("Lo sentimos, se ha producido un error");
        }
    });
}

descargarArchivo = function (reporte, formato) {
    $.ajax({
        type: 'POST',
        url: controler + 'exportardata',
        data: { pericodi: $('#pericodiReporte').val(), recpotcodi: $('#recpotcodiReporte').val(), reporte: reporte, formato: formato },
        dataType: 'json',
        success: function (result) {
            if (result != -1) {

                if (reporte == "General") formato = 3;

                window.location = controler + 'abrirarchivo?formato=' + formato + '&file=' + result;
                mostrarExito("Felicidades, el archivo se descargo correctamente...!");
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

MigrarSaldo = function () {
    $.ajax({
        type: 'POST',
        url: controler + 'migrarsaldo',
        data: { pericodi: $('#pericodiSaldo').val(), recpotcodi: $('#recpotcodiSaldo').val(), migracodi: $('#migracodi').val() },
        dataType: 'json',
        success: function (result) {
            if (result == "1") {
                mostrarExito('Listo...! Usted puede volver a ejecutar el proceso de valorización');
            }
            else {
                mostrarAlerta(result);
            }
        },
        error: function () {
            mostrarError("Lo sentimos, se ha producido un error");
        }
    });
}

MigrarVTP = function () {
    $.ajax({
        type: 'POST',
        url: controler + 'migrarvtp',
        data: { pericodi: $('#pericodiSaldo').val(), recpotcodi: $('#recpotcodiSaldo').val(), migracodi: $('#migracodi').val() },
        dataType: 'json',
        success: function (result) {
            if (result == "1") {
                mostrarExito('Listo...! Usted puede volver a ejecutar el proceso de valorización');
            }
            else {
                mostrarAlerta(result);
            }
        },
        error: function () {
            mostrarError("Lo sentimos, se ha producido un error");
        }
    });
}


ProcesarVerificacion = function () {
    var cmbPericodi = document.getElementById('pericodiVerificacion');
    var cmbRecpotcodi = document.getElementById('recpotcodiVerificacion');
    var reporte = 'VerificacionResultados';
    var formato = 1;

    $.ajax({
        type: 'POST',
        url: controler + 'ExportarVerificacion',
        data: { pericodi: cmbPericodi.value, recpotcodi: cmbRecpotcodi.value, reporte: reporte, formato: formato },
        dataType: 'json',
        success: function (result) {
            if (result != -1) {

                window.location = controler + 'abrirarchivo?formato=' + formato + '&file=' + result;
                mostrarExito("Felicidades, el archivo se descargo correctamente...!");
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