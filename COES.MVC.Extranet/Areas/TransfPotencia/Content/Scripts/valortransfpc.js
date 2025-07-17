var controler = siteRoot + 'transfpotencia/valortransfpc/';
var error = [];
$(function () {
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
});

RecargarReporte = function () {
    var cmbPericodi = document.getElementById('pericodiReporte');
    var cmbRecpotcodi = document.getElementById('recpotcodiReporte');
    window.location.href = controler + "index?pericodi=" + cmbPericodi.value + "&recpotcodi=" + cmbRecpotcodi.value + "#paso2";
}

descargarArchivo = function (reporte, formato) {
    $.ajax({
        type: 'POST',
        url: controler + 'exportardata',
        data: { pericodi: $('#pericodiReporte').val(), recpotcodi: $('#recpotcodiReporte').val(), reporte: reporte, formato: formato },
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