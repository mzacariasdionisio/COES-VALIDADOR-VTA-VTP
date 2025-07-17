var sControlador = siteRoot + 'sistemastransmision/calculo/';
var error = [];
$(function () {
    //-----------------------------------------------------------------------------------------------------------------------------------

    $('#btnDescargarReporte301Excel').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        descargarArchivo('Reporte301Excel', 1);
    });

    $('#btnDescargarReporte302Excel').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        descargarArchivo('Reporte302Excel', 1);
    });

    $('#btnDescargarReporte303Excel').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        descargarArchivo('Reporte303Excel', 1);
    });

    $('#btnDescargarReporteDistElecExcel').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        descargarArchivo('ReporteDistElecExcel', 1);
    });

    $('#btnDescargarFactorParticipacionExcel').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        descargarArchivo('FactorParticipacion', 1);
    });

    $('#btnDescargarFactorParticipacionRecalculadoExcel').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        descargarArchivo('FactorParticipacionRecalculado', 1);
    });

    $('#btnDescargarCompensacionMensualExcel').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        descargarArchivo('CompensacionMensual', 1);
    });

    $('#btnDescargarCompensacionMensualFiltradaExcel').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        descargarArchivo('CompensacionMensualFiltrada', 1);
    });

    $('#btnDescargarResponsabilidadPagoExcel').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        descargarArchivo('ResponsabilidadPago', 1);
    });
});

recargarReporte = function () {
    var cmbStpercodi = document.getElementById('stpercodiReporte');
    var cmbStrecacodi = document.getElementById('stpercodiReporte');
    window.location.href = sControlador + "index?stpercodi=" + cmbStpercodi.value + "&stpercodi=" + cmbStrecacodi.value + "#paso2";
}

descargarArchivo = function (reporte, formato) {
    $.ajax({
        type: 'POST',
        url: sControlador + 'exportardata',
        data: { stpercodi: $('#stpercodiReporte').val(), strecacodi: $('#strecacodiReporte').val(), reporte: reporte, formato: formato },
        dataType: 'json',
        success: function (result) {
            if (result != -1) {
                window.location = sControlador + 'abrirarchivo?formato=' + formato + '&file=' + result;
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