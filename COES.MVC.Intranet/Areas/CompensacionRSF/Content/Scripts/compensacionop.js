var sControlador = siteRoot + "compensacionrsf/CompensacionOP/";

$(document).ready(function () {

    uploadExcelCO();

    $('#btnExportarPM').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        exportarPM(1);
    });
});

exportarPM = function (formato) {
    var pericodi = document.getElementById('pericodi').value;
    var vcrecacodi = document.getElementById('vcrecacodi').value;
    $.ajax({
        type: 'POST',
        url: sControlador + 'ExportarCompOp',
        data: { pericodi: pericodi, vcrecacodi: vcrecacodi },
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

refrescar = function () {
    var cmbPericodi = document.getElementById('pericodi');
    window.location.href = sControlador + "index?pericodi=" + cmbPericodi.value;
}

uploadExcelCO = function () {
    uploaderCO = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: 'btnSelecionarExcelCO',
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
                if (uploaderCO.files.length == 2) {
                    uploaderCO.removeFile(uploaderCO.files[0]);
                }
                uploaderCO.start();
                up.refresh();
            },
            UploadProgress: function (up, file) {
                mostrarAlerta("Por favor espere ...(<strong>" + file.percent + "%</strong>)");
            },
            UploadComplete: function (up, file) {
                mostrarAlerta("Subida completada <strong> procesando el archivo...</strong>");
                procesarArchivoCO(file[0].name);
            },
            Error: function (up, err) {
                mostrarError(err.code + "-" + err.message);
            }
        }
    });
    uploaderCO.init();
}

procesarArchivoCO = function (sFile) {
    $.ajax({
        type: 'POST',
        url: sControlador + 'procesararchivoco',
        data: { sarchivo: sFile, vcrecacodi: $('#vcrecacodi').val() },
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