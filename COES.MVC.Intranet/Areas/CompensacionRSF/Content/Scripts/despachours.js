var sControlador = siteRoot + "compensacionrsf/despachours/";

$(document).ready(function () {
    $('#btnCopiarDURS').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        copiarDURS();
    });

    $('#btnExportarDURS').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        exportarDURS(1);
    });

    $('#btnConsultarURS').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        buscar();
    });

    uploadExcelDU();
});

buscar = function () {
    var vcrecacodi = document.getElementById('vcrecacodi').value;
    $.ajax({
        type: 'POST',
        url: sControlador + "lista",
        data: { vcrecacodi: vcrecacodi },
        success: function (evt) {
            $('#listado').html(evt);
            oTable = $('#tabla').dataTable({
                "sPaginationType": "full_numbers",
                "destroy": "true"
            });
        },
        error: function () {
            mostrarError();
        }
    });
};

exportarDURS = function (formato) {
    var pericodi = document.getElementById('pericodi').value;
    $.ajax({
        type: 'POST',
        url: sControlador + 'exportarDURS',
        data: { pericodi: pericodi },
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

copiarDURS = function () {
    var pericodi = document.getElementById('pericodi').value;
    var vcrecacodi = document.getElementById('vcrecacodi').value;
    if (vcrecacodi == "") {
        mostrarError("Por favor, seleccionar una versión de recalculo");
    }
    else { 
        $.ajax({
            type: 'POST',
            url: sControlador + 'copiarDURS',
            data: { pericodi: pericodi, vcrecacodi: vcrecacodi},
            dataType: 'json',
            success: function (model) {
                if (model.sError == "") {
                    mostrarExito("Felicidades, se copio correctamente la información de " + model.iNumReg + " dias.");
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
    var cmbPericodi = document.getElementById('pericodi');
    window.location.href = sControlador + "index?pericodi=" + cmbPericodi.value;
}

uploadExcelDU = function () {
    var pericodi = document.getElementById('pericodi').value;
    var vcrecacodi = document.getElementById('vcrecacodi').value;
    //console.log(vcrecacodi);
    if (vcrecacodi == "") {
        mostrarError("Por favor, seleccionar una versión de recalculo");
    }
    else { 
        uploaderDU = new plupload.Uploader({
            runtimes: 'html5,flash,silverlight,html4',
            browse_button: 'btnSelecionarExcelDU',
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
                    if (uploaderDU.files.length == 2) {
                        uploaderDU.removeFile(uploaderDU.files[0]);
                    }
                    uploaderDU.start();
                    up.refresh();
                },
                UploadProgress: function (up, file) {
                    mostrarAlerta("Por favor espere ...(<strong>" + file.percent + "%</strong>)");
                },
                UploadComplete: function (up, file) {
                    mostrarAlerta("Subida completada <strong> procesando el archivo...</strong>");
                    procesarArchivoDU(file[0].name);
                },
                Error: function (up, err) {
                    mostrarError(err.code + "-" + err.message);
                }
            }
        });
        uploaderDU.init();
    }
}

procesarArchivoDU = function (sFile) {
    var vcrecacodi = document.getElementById('vcrecacodi').value;
    $.ajax({
        type: 'POST',
        url: sControlador + 'ProcesarArchivoDU',
        data: { sarchivo: sFile, vcrecacodi: vcrecacodi },
        dataType: 'json',
        cache: false,
        success: function (result) {
            if (result.sError == "")
                mostrarExito(result.sMensaje);
            else
                mostrarError("Lo sentimos no se puedo grabar la información ingresada " + result.sError);
        },
        error: function () {
            mostrarError('Lo sentimos no se puedo grabar la información ingresada');
        }
    });
}