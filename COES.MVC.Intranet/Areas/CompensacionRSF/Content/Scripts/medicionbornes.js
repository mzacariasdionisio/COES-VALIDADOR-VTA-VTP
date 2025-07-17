var sControlador = siteRoot + "compensacionrsf/medicionbornes/";


$(document).ready(function () {

    buscar();

    $('#tab-container').easytabs({
        animate: false
    });

    $('#btnCopiarMB').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        copiarMB();
    });

    $('#btnExportarMB').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        exportarMB(1);
    });

    $('#btnGrabar').click(function () {
        grabarLista();
    });

    $('#btnBuscar').click(function () {
        buscar();
    });

    uploadExcelMB();
});

buscar = function () {
    var pericodi = document.getElementById('pericodi').value;
    var vcrecacodi = document.getElementById('vcrecacodi').value;
    $.ajax({
        type: 'POST',
        url: sControlador + "lista",
        data: { pericodi: pericodi, vcrecacodi: vcrecacodi },
        success: function (evt) {
            $('#listado').html(evt);
            //oTable = $('#tabla').dataTable({
            //    "sPaginationType": "full_numbers",
            //    "destroy": "true",
            //    "aaSorting": [[0, "asc"], [1, "asc"], [2, "asc"]]
            //});
        },
        error: function () {
            mostrarError();
        }
    });
};

exportarMB = function (formato) {
    var pericodi = document.getElementById('pericodi').value;
    $.ajax({
        type: 'POST',
        url: sControlador + 'exportarMB',
        data: { pericodi: pericodi },
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

copiarMB = function () {
    var pericodi = document.getElementById('pericodi').value;
    var vcrecacodi = document.getElementById('vcrecacodi').value;
    $.ajax({
        type: 'POST',
        url: sControlador + 'copiarMB',
        data: { pericodi: pericodi, vcrecacodi: vcrecacodi },
        dataType: 'json',
        success: function (model) {
            if (model.sError == "") {
                mostrarExito("Felicidades, se copio correctamente " + model.iNumReg + " registros.");
                refrescar();
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

uploadExcelMB = function () {
    uploaderMB = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: 'btnImportarMB',
        url: sControlador + 'uploadexcel',
        flash_swf_url: 'Scripts/Moxie.swf',
        silverlight_xap_url: 'Scripts/Moxie.xap',
        multi_selection: false,
        filters: {
            max_file_size: '100mb',
            mime_types: [
                { title: "Archivos Excel .xlsx", extensions: "xlsx,xls" }
            ]
        },
        init: {
            FilesAdded: function (up, files) {
                if (uploaderMB.files.length == 2) {
                    uploaderMB.removeFile(uploaderMB.files[0]);
                }
                uploaderMB.start();
                up.refresh();
            },
            UploadProgress: function (up, file) {
                mostrarAlerta("Por favor espere ...(<strong>" + file.percent + "%</strong>)");
            },
            UploadComplete: function (up, file) {
                mostrarAlerta("Subida completada <strong> procesando el archivo...</strong>");
                procesarArchivoMB(file[0].name);
            },
            Error: function (up, err) {
                mostrarError(err.code + "-" + err.message);
            }
        }
    });
    uploaderMB.init();
}

procesarArchivoMB = function (sFile) {
    var pericodi = document.getElementById('pericodi').value;
    var vcrecacodi = document.getElementById('vcrecacodi').value;

    $.ajax({
        type: 'POST',
        url: sControlador + 'procesarArchivoMB',
        data: { sarchivo: sFile, pericodi: pericodi, vcrecacodi: vcrecacodi },
        dataType: 'json',
        cache: false,
        success: function (model) {
            if (model.sError == "") {
                mostrarExito("Felicidades, se copio correctamente " + model.iNumReg + " registros.");
                refrescar();
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

grabarLista = function () {
    var iNumReg = document.getElementById('Count').value;
    if (iNumReg > 0) {
        var items = checkMark();
        var pericodi = document.getElementById('pericodiCI').value;
        var vcrecacodi = document.getElementById('vcrecacodiCI').value;
        $.ajax({
            type: 'POST',
            url: sControlador + 'grabarlistacargo',
            data: { pericodi: pericodi, vcrecacodi: vcrecacodi, items: items },
            dataType: 'json',
            success: function (model) {
                if (model.sError == "") {
                    mostrarExito("Felicidades, tenemos " + model.iNumReg + " unidades considerados en el cargo del incumplimiento");
                    refrescarCI();
                }
                else {
                    mostrarError("Lo sentimos, ha ocurrido el siguiente error: " + model.sError);
                }
            },
            error: function () {
                mostrarError('Ha ocurrido un error', $tipoMensajeError, $modoMensajeCuadro);
            }
        });

    }
    else { return; }
}

refrescar = function () {
    var cmbPericodi = document.getElementById('pericodi');
    window.location.href = sControlador + "index?pericodi=" + cmbPericodi.value;
}

refrescarCI = function () {
    var cmbPericodi = document.getElementById('pericodiCI');
    var cmbRecacodi = document.getElementById('vcrecacodiCI');
    window.location.href = sControlador + "index?pericodi=" + cmbPericodi.value + "&vcrecacodi=" + cmbRecacodi.value + "#paso2";
}
