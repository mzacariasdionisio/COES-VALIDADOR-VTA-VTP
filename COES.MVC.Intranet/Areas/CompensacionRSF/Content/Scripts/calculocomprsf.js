var sControlador = siteRoot + "compensacionrsf/calculocomprsf/";


$(document).ready(function () {
    $('#tab-container').easytabs({
        animate: false
    });

    $('#btnProcesarCalculo').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando el Cálculo de Compensaciones por RSF");
        procesarCalculo();
    });

    $('#btnBorrarCalculo').click(function () {
        mostrarAlerta("Espere un momento, se esta procediendo a borrar el Cálculo de Compensaciones por RSF");
        borrarCalculo();
    });

    //-----------------------------------------------------------------------------------------------------------------------------------

    $('#btnDescargarRepSuperavitExcel').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        descargarArchivo('Superavit', 1);
    });

    $('#btnDescargarRepReservaNoSuministradaExcel').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        descargarArchivo('ReservaNoSuministrada', 1);
    });

    $('#btnDescargarRepReservaAsignadaExcel').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        descargarArchivo('ReservaAsignada', 1);
    });

    $('#btnDescargarRepCostoOportunidadExcel').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        descargarArchivo('CostoOportunidad', 1);
    });

    $('#btnDescargarRepCompensacionExcel').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        descargarArchivo('Compensacion', 1);
    });

    $('#btnDescargarRepAsignacionPagoDiarioExcel').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        descargarArchivo('AsignacionPagoDiario', 1);
    });

    $('#btnDescargarRepCostoServicioRSFDiarioExcel').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        descargarArchivo('CostoServicioRSFDiario', 1);
    });

    $('#btnDescargarRepCuadroPR21Excel').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        descargarArchivo('CuadroPR21', 1);
    });

    $('#btnDescargarRepReporteResumenExcel').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        descargarArchivo('ReporteResumen', 1);
    });
});

procesarCalculo = function () {
    var pericodi = document.getElementById('pericodi').value;
    var vcrecacodi = document.getElementById('vcrecacodi').value;
    $.ajax({
        type: 'POST',
        url: sControlador + 'procesarcalculo',
        data: { pericodi: pericodi, vcrecacodi: vcrecacodi },
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

borrarCalculo = function () {
    var pericodi = document.getElementById('pericodi').value;
    var vcrecacodi = document.getElementById('vcrecacodi').value;
    $.ajax({
        type: 'POST',
        url: sControlador + 'borrarcalculo',
        data: { pericodi: pericodi, vcrecacodi: vcrecacodi },
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
    var pericodi = document.getElementById('pericodiRep').value;
    var vcrecacodi = document.getElementById('vcrecacodiRep').value;
    $.ajax({
        type: 'POST',
        url: sControlador + 'exportardata',
        data: { pericodi: pericodi, vcrecacodi: vcrecacodi, reporte: reporte, formato: formato },
        dataType: 'json',
        success: function (result) {
            if (result != -1) {
                window.location = sControlador + 'abrirarchivo?formato=' + formato + '&file=' + result;
                mostrarExito("Felicidades, el archivo se descargo correctamente...!");
            }
            else {
                mostrarError('Ha ocurrido un error', $tipoMensajeError, $modoMensajeCuadro);
            }
        },
        error: function () {
            mostrarError('Ha ocurrido un error', $tipoMensajeError, $modoMensajeCuadro);
        }
    });
}

refrescar = function () {
    var cmbPericodi = document.getElementById('pericodi');
    window.location.href = sControlador + "index?pericodi=" + cmbPericodi.value;
}

refrescarRep = function () {
    var cmbPericodi = document.getElementById('pericodiRep');
    window.location.href = sControlador + "index?pericodi=" + cmbPericodi.value + "#paso2";
}