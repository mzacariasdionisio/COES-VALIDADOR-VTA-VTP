var sControlador = siteRoot + "compensacionrsf/saldomesdirecto/";

$(document).ready(function () {
    //--------------------------------------SaldoMes-----------------------------------------------------
    mostrarGrillaExcelSD();

    $('#btnGrabarExcelSD').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        grabarExcelSD();
    });

    $('#btnEliminarDatosSD').click(function () {
        if (confirm("¿Desea eliminar la información de la hoja de cálculo?")) {
            mostrarAlerta("Espere un momento, se esta procesando su solicitud");
            eliminarDatosSD();
        }
    });

    $('#btnDescargarExcelSD').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        descargarArchivoSD(1);
    });

    uploadExcelSD();      

});

mostrarGrillaExcelSD = function () {
    var vcrecacodi = document.getElementById('vcrecacodi').value;
    error = [];
    if (typeof hotdef != 'undefined') {
        hotdef.destroy();
    }
    var container = document.getElementById('grillaExcelSD');
    $.ajax({
        type: 'POST',
        url: sControlador + "grillaExcelSD",
        data: { vcrecacodi: vcrecacodi },
        dataType: 'json',
        success: function (result) {
            var columns = result.Columnas;
            var headers = result.Headers;
            var widths = result.Widths;
            var data = result.Data;
            hotdef = new Handsontable(container, {
                data: data,
                colHeaders: headers,
                rowHeaders: true,
                colWidths: widths,
                contextMenu: true,
                minSpareRows: 1,
                fixedRowsTop: 1,
                fixedColumnsLeft: 2,
                columns: columns
            });
            hotdef.render();
        },
        error: function () {
            mostrarError('Lo sentimos no se puede mostrar la hoja de cálculo')
        }
    });

};

grabarExcelSD = function () {
    var vcrecacodi = document.getElementById('vcrecacodi').value;
    var pericodi = document.getElementById('pericodi').value;
    $.ajax({
        type: "POST",
        url: sControlador + 'GrabarGrillaExcelSD',
        dataType: "json",
        contentType: 'application/json',
        traditional: true,
        data: JSON.stringify({
            pericodi: pericodi, vcrecacodi: vcrecacodi, datos: hotdef.getData()
        }),
        success: function (result) {
            var sError = result.sError;
            var sMensaje = result.sMensaje;
            if (sError == "") {
                var iNumRegistros = hotdef.countRows();
                if (iNumRegistros >= 1) iNumRegistros = iNumRegistros - 1;
                mostrarExito('Operación Exitosa - ' + sMensaje + ", numero de registros almacenados: " + iNumRegistros);
            }
            else {
                mostrarError('Lo sentimo ocurrio un error: ' + sError);
            }
        },
        error: function (response) {
            mostrarError(response.status + " " + response.statusText);
        }
    });
}

eliminarDatosSD = function () {
    var vcrecacodi = document.getElementById('vcrecacodi').value;
    $.ajax({
        type: 'POST',
        url: sControlador + 'eliminardatossd',
        data: { vcrecacodi: vcrecacodi },
        dataType: 'json',
        success: function (result) {
            if (result == "1") {
                mostrarExito("Felicidades, la información se elimino correctamente...!");
                mostrarGrillaExcelSD();
            }
            else if (result == "-1") {
                mostrarError("Lo sentimos, se ha producido un error...");
            }
        },
        error: function () {
            mostrarError("Lo sentimos, se ha producido un error");
        }
    });
}

descargarArchivoSD = function (formato) {
    var vcrecacodi = document.getElementById('vcrecacodi').value;
    var pericodi = document.getElementById('pericodi').value;
    $.ajax({
        type: 'POST',
        url: sControlador + 'ExportarDataSD',
        data: { pericodi: pericodi, vcrecacodi: vcrecacodi, formato: formato },
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

uploadExcelSD = function () {
    uploaderSD = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: 'btnSelecionarExcelSD',
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
                if (uploaderSD.files.length == 2) {
                    uploaderSD.removeFile(uploaderSD.files[0]);
                }
                uploaderSD.start();
                up.refresh();
            },
            UploadProgress: function (up, file) {
                mostrarAlerta("Por favor espere ...(<strong>" + file.percent + "%</strong>)");
            },
            UploadComplete: function (up, file) {
                mostrarAlerta("Subida completada <strong> procesando el archivo...</strong>");
                procesarArchivoSD(file[0].name);
            },
            Error: function (up, err) {
                mostrarError(err.code + "-" + err.message);
            }
        }
    });
    uploaderSD.init();
}

procesarArchivoSD = function (sFile) {
    var pericodi = document.getElementById('pericodi').value;
    var vcrecacodi = document.getElementById('vcrecacodi').value;
    if (typeof hotdef != 'undefined') {
        hotdef.destroy();
    }
    var container = document.getElementById('grillaExcelSD');
    $.ajax({
        type: 'POST',
        url: sControlador + 'procesararchivoSD',
        data: { sarchivo: sFile, pericodi: pericodi, vcrecacodi: vcrecacodi },
        dataType: 'json',
        cache: false,
        success: function (result) {
            var iRegError = result.RegError;
            var sMensajeError = result.MensajeError;
            var bGrabar = result.Grabar;
            var columns = result.Columnas;
            var headers = result.Headers;
            var widths = result.Widths;
            var data = result.Data;
            hotdef = new Handsontable(container, {
                data: data,
                maxCols: result.Columnas.length,
                colHeaders: headers,
                colWidths: widths,
                columnSorting: false,
                contextMenu: true,
                minSpareRows: 1,
                rowHeaders: true,
                columns: columns
            });
            hotdef.render();
            var iNumRegistros = hotdef.countRows();
            if (iNumRegistros >= 1) iNumRegistros = iNumRegistros - 1;
            if (iRegError > 0) {
                mostrarError("Lo sentimos, <b>" + iRegError + "</b> registro(s) no ha(n) sido leido(s) por presentar <b>errores</b>" + sMensajeError);
            }
            else {
                mostrarMensaje("Numero de registros cargados: <b>" + iNumRegistros + "</b>, Por favor verifique los datos y luego proceda a <b>Grabar</b> en el icono del menú <b>Enviar datos</b>");
            }
        },
        error: function () {
            mostrarError('Lo sentimos no se puede mostrar la hoja de cálculo')
        }
    });
}

refrescar = function () {
    var cmbPericodi = document.getElementById('pericodi');
    window.location.href = sControlador + "index?pericodi=" + cmbPericodi.value;
}