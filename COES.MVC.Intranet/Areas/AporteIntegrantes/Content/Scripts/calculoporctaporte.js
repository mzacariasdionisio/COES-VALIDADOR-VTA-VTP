var sControlador = siteRoot + "AporteIntegrantes/CalculoPorctAporte/";

$(document).ready(function () {

    $('#tab-container').easytabs({
        animate: false
    });

    $('#btnProcesar').click(function () {
        buscar();
    });

    $('#btnDescargarRepEnergia').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        descargarArchivo('Energia', 1);
    });

    $('#btnDescargarRepPotencia').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        descargarArchivo('Potencia', 1);
    });

    $('#btnDescargarRepTransmision').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        descargarArchivo('Transmision', 1);
    }); 

    $('#btnDescargarRepPorcentaje').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        descargarArchivo('Porcentaje', 1);
    });
});

descargarArchivo = function (reporte, formato) {
    var caiprscodi = document.getElementById('caiprscodi').value;
    var caiajcodi = document.getElementById('caiajcodi').value;
    $.ajax({
        type: 'POST',
        url: sControlador + 'exportardata',
        data: { caiprscodi: caiprscodi, caiajcodi: caiajcodi, reporte: reporte, formato: formato },
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
    var caiprscodi = document.getElementById('caiprscodi').value;
    window.location.href = sControlador + "index?caiprscodi=" + caiprscodi;
}

refrescarReporte = function () {
    var caiprscodiRep = document.getElementById('caiprscodiRep').value;
    window.location.href = sControlador + "index?caiprscodi=" + caiprscodiRep + "#paso2";
}
