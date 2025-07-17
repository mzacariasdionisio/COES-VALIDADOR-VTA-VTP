var sControlador = siteRoot + "CompensacionRSF/IncumpPR21/";

$(document).ready(function () {
    $('#tab-container').easytabs({
        animate: false
    });

    //------------------------------------Incumplimiento PR-21-------------------------------------------
    mostrarGrillaExcelPR21();

    $('#btnGrabarExcelPR21').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        grabarExcelPR21();
    });

    $('#btnEliminarDatosPR21').click(function () {
        if (confirm("¿Desea eliminar la información de la hoja de cálculo?")) {
            mostrarAlerta("Espere un momento, se esta procesando su solicitud");
            eliminarDatosPR21();
        }
    });

    $('#btnDescargarExcelPR21').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        descargarArchivoPR21(1);
    });

    uploadExcelPR21();

    //------------------------------------Porcentaje RPNS-------------------------------------------
    mostrarGrillaExcelRPNS();

    $('#btnGrabarExcelRPNS').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        grabarExcelRPNS();
    });

    $('#btnEliminarDatosRPNS').click(function () {
        if (confirm("¿Desea eliminar la información de la hoja de cálculo?")) {
            mostrarAlerta("Espere un momento, se esta procesando su solicitud");
            eliminarDatosRPNS();
        }
    });

    $('#btnDescargarExcelRPNS').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        descargarArchivoRPNS(1);
    });

    uploadExcelRPNS();
});

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
mostrarGrillaExcelPR21 = function () {
    var pericodi = document.getElementById('Pericodi').value;
    var vcrinccodi = document.getElementById('vcrinccodi').value;
    mostrarAlerta('Se esta consultando la información registrada');
    error = [];
    if (typeof hotPR21 != 'undefined') {
        hotPR21.destroy();
    }
    var container = document.getElementById('grillaExcelPR21');
    calculateSizeHandsontable(container);
    //console.log(container);
    $.ajax({
        type: 'POST',
        url: sControlador + "grillaExcelPR21",
        data: { pericodi: pericodi, vcrinccodi: vcrinccodi },
        dataType: 'json',
        success: function (result) {
            var columns = result.Columnas;
            //var headers = result.Headers;
            var widths = result.Widths;
            var data = result.Data;
            hotPR21 = new Handsontable(container, {
                data: data,
                //colHeaders: headers,
                rowHeaders: true,
                colWidths: widths,
                contextMenu: true,
                minSpareRows: 1,
                fixedRowsTop: 1,
                fixedColumnsLeft: 2,
                columns: columns,
                mergeCells: [{ row: 0, col: 0, rowspan: 2, colspan: 1 },
                             { row: 0, col: 3, rowspan: 1, colspan: 2 },
                             { row: 0, col: 5, rowspan: 1, colspan: 2 },
                             { row: 0, col: 7, rowspan: 1, colspan: 2 },
                             { row: 0, col: 9, rowspan: 1, colspan: 2 },
                             { row: 0, col: 11, rowspan: 1, colspan: 2 },
                             { row: 0, col: 13, rowspan: 1, colspan: 2 },
                             { row: 0, col: 15, rowspan: 1, colspan: 2 },
                             { row: 0, col: 17, rowspan: 1, colspan: 2 },
                             { row: 0, col: 19, rowspan: 1, colspan: 2 },
                             { row: 0, col: 21, rowspan: 1, colspan: 2 },
                             { row: 0, col: 23, rowspan: 1, colspan: 2 },
                             { row: 0, col: 25, rowspan: 1, colspan: 2 },
                             { row: 0, col: 27, rowspan: 1, colspan: 2 },
                             { row: 0, col: 29, rowspan: 1, colspan: 2 },
                             { row: 0, col: 31, rowspan: 1, colspan: 2 },
                             { row: 0, col: 33, rowspan: 1, colspan: 2 },
                             { row: 0, col: 35, rowspan: 1, colspan: 2 },
                             { row: 0, col: 37, rowspan: 1, colspan: 2 },
                             { row: 0, col: 39, rowspan: 1, colspan: 2 },
                             { row: 0, col: 41, rowspan: 1, colspan: 2 },
                             { row: 0, col: 43, rowspan: 1, colspan: 2 },
                             { row: 0, col: 45, rowspan: 1, colspan: 2 },
                             { row: 0, col: 47, rowspan: 1, colspan: 2 },
                             { row: 0, col: 49, rowspan: 1, colspan: 2 },
                             { row: 0, col: 51, rowspan: 1, colspan: 2 },
                             { row: 0, col: 53, rowspan: 1, colspan: 2 },
                             { row: 0, col: 55, rowspan: 1, colspan: 2 },
                             { row: 0, col: 57, rowspan: 1, colspan: 2 },
                             { row: 0, col: 59, rowspan: 1, colspan: 2 },
                             { row: 0, col: 61, rowspan: 1, colspan: 2 },
                             { row: 0, col: 63, rowspan: 1, colspan: 2 },
                             { row: 0, col: 65, rowspan: 1, colspan: 2 },
                             { row: 0, col: 67, rowspan: 1, colspan: 2 }],
                //before change
                cells: function (row, col, prop) {
                    //console.log("col:" + col + " row:" + row + " prop" + prop);
                    var cellProperties = {};
                    if (row == 0 || row == 1) {
                        cellProperties.renderer = firstRowRendererCabecera;
                    }
                    else if (col == 0 || col == 1) {
                        cellProperties.renderer = firstRowRendererCeleste;
                    }
                    return cellProperties;
                },
            });
            hotPR21.render();
            //$('#divAcciones').css('display', 'block');
            var iNumRegistros = hotPR21.countRows();
            if (iNumRegistros >= 1) iNumRegistros = iNumRegistros - 1;
            mostrarExito("Número de registros encontrados: " + iNumRegistros);
        },
        error: function () {
            mostrarError('Lo sentimos no se puede mostrar la hoja de cálculo')
        }
    });

};

grabarExcelPR21 = function () {
    mostrarAlerta('Se esta consultando la información registrada');
    var pericodi = document.getElementById('Pericodi').value;
    var vcrinccodi = document.getElementById('vcrinccodi').value;
    $.ajax({
        type: "POST",
        url: sControlador + 'GrabarGrillaExcelPR21',
        dataType: "json",
        contentType: 'application/json',
        traditional: true,
        data: JSON.stringify({
            pericodi: pericodi, vcrinccodi: vcrinccodi, datos: hotPR21.getData()
        }),
        success: function (result) {
            var sError = result.sError;
            var sMensaje = result.sMensaje;
            if (sError == "") {
                var iNumRegistros = hotPR21.countRows();
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

eliminarDatosPR21 = function () {
    var pericodi = document.getElementById('Pericodi').value;
    var vcrinccodi = document.getElementById('vcrinccodi').value;
    $.ajax({
        type: 'POST',
        url: sControlador + 'eliminardatospr21',
        data: { pericodi: pericodi, vcrinccodi: vcrinccodi },
        dataType: 'json',
        success: function (result) {
            if (result == "1") {
                mostrarExito("Felicidades, la información se elimino correctamente...!");
                mostrarGrillaExcelPR21();
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

descargarArchivoPR21 = function (formato) {
    var pericodi = document.getElementById('Pericodi').value;
    var vcrinccodi = document.getElementById('vcrinccodi').value;
    $.ajax({
        type: 'POST',
        url: sControlador + 'ExportarDataPR21',
        data: { pericodi: pericodi, vcrinccodi: vcrinccodi, formato: formato },
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

uploadExcelPR21 = function () {
    uploaderPR21 = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: 'btnSelecionarExcelPR21',
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
                if (uploaderPR21.files.length == 2) {
                    uploaderPR21.removeFile(uploaderPR21.files[0]);
                }
                uploaderPR21.start();
                up.refresh();
            },
            UploadProgress: function (up, file) {
                mostrarAlerta("Por favor espere ...(<strong>" + file.percent + "%</strong>)");
            },
            UploadComplete: function (up, file) {
                mostrarAlerta("Subida completada <strong> procesando el archivo...</strong>");
                procesarArchivoPR21(file[0].name);
            },
            Error: function (up, err) {
                mostrarError(err.code + "-" + err.message);
            }
        }
    });
    uploaderPR21.init();
}

procesarArchivoPR21 = function (sFile) {
    var pericodi = document.getElementById('Pericodi').value;
    var vcrinccodi = document.getElementById('vcrinccodi').value;
    if (typeof hotPR21 != 'undefined') {
        hotPR21.destroy();
    }
    var container = document.getElementById('grillaExcelPR21');
    calculateSizeHandsontable(container);
    $.ajax({
        type: 'POST',
        url: sControlador + 'procesararchivoPR21',
        data: { sarchivo: sFile, pericodi: pericodi, vcrinccodi: vcrinccodi },
        dataType: 'json',
        cache: false,
        success: function (result) {
            var iRegError = result.RegError;
            var sMensajeError = result.MensajeError;
            var columns = result.Columnas;
            //var headers = result.Headers;
            var widths = result.Widths;
            var data = result.Data;
            hotPR21 = new Handsontable(container, {
                data: data,
                //colHeaders: headers,
                rowHeaders: true,
                colWidths: widths,
                contextMenu: true,
                minSpareRows: 1,
                fixedRowsTop: 1,
                fixedColumnsLeft: 2,
                columns: columns,
                mergeCells: [{ row: 0, col: 0, rowspan: 2, colspan: 1 },
                             { row: 0, col: 3, rowspan: 1, colspan: 2 },
                             { row: 0, col: 5, rowspan: 1, colspan: 2 },
                             { row: 0, col: 7, rowspan: 1, colspan: 2 },
                             { row: 0, col: 9, rowspan: 1, colspan: 2 },
                             { row: 0, col: 11, rowspan: 1, colspan: 2 },
                             { row: 0, col: 13, rowspan: 1, colspan: 2 },
                             { row: 0, col: 15, rowspan: 1, colspan: 2 },
                             { row: 0, col: 17, rowspan: 1, colspan: 2 },
                             { row: 0, col: 19, rowspan: 1, colspan: 2 },
                             { row: 0, col: 21, rowspan: 1, colspan: 2 },
                             { row: 0, col: 23, rowspan: 1, colspan: 2 },
                             { row: 0, col: 25, rowspan: 1, colspan: 2 },
                             { row: 0, col: 27, rowspan: 1, colspan: 2 },
                             { row: 0, col: 29, rowspan: 1, colspan: 2 },
                             { row: 0, col: 31, rowspan: 1, colspan: 2 },
                             { row: 0, col: 33, rowspan: 1, colspan: 2 },
                             { row: 0, col: 35, rowspan: 1, colspan: 2 },
                             { row: 0, col: 37, rowspan: 1, colspan: 2 },
                             { row: 0, col: 39, rowspan: 1, colspan: 2 },
                             { row: 0, col: 41, rowspan: 1, colspan: 2 },
                             { row: 0, col: 43, rowspan: 1, colspan: 2 },
                             { row: 0, col: 45, rowspan: 1, colspan: 2 },
                             { row: 0, col: 47, rowspan: 1, colspan: 2 },
                             { row: 0, col: 49, rowspan: 1, colspan: 2 },
                             { row: 0, col: 51, rowspan: 1, colspan: 2 },
                             { row: 0, col: 53, rowspan: 1, colspan: 2 },
                             { row: 0, col: 55, rowspan: 1, colspan: 2 },
                             { row: 0, col: 57, rowspan: 1, colspan: 2 },
                             { row: 0, col: 59, rowspan: 1, colspan: 2 },
                             { row: 0, col: 61, rowspan: 1, colspan: 2 },
                             { row: 0, col: 63, rowspan: 1, colspan: 2 },
                             { row: 0, col: 65, rowspan: 1, colspan: 2 },
                             { row: 0, col: 67, rowspan: 1, colspan: 2 }],
                //before change
                cells: function (row, col, prop) {
                    //console.log("col:" + col + " row:" + row + " prop" + prop);
                    var cellProperties = {};
                    if (row == 0 || row == 1) {
                        cellProperties.renderer = firstRowRendererCabecera;
                    }
                    else if (col == 0 || col == 1) {
                        cellProperties.renderer = firstRowRendererCeleste;
                    }
                    return cellProperties;
                },
            });
            hotPR21.render();
            var iNumRegistros = hotPR21.countRows();
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

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
mostrarGrillaExcelRPNS = function () {
    var pericodi = document.getElementById('Pericodi').value;
    var vcrinccodi = document.getElementById('vcrinccodi').value;
    mostrarAlerta('Se esta consultando la información registrada');
    errorRPNS = [];
    if (typeof hotRPNS != 'undefined') {
        hotRPNS.destroy();
    }
    var container = document.getElementById('grillaExcelRPNS');
    calculateSizeHandsontable(container);
    //console.log(container);
    $.ajax({
        type: 'POST',
        url: sControlador + "grillaExcelRPNS",
        data: { pericodi: pericodi, vcrinccodi: vcrinccodi },
        dataType: 'json',
        success: function (result) {
            var columns = result.Columnas;
            //var headers = result.Headers;
            var widths = result.Widths;
            var data = result.Data;
            hotRPNS = new Handsontable(container, {
                data: data,
                //colHeaders: headers,
                rowHeaders: true,
                colWidths: widths,
                contextMenu: true,
                minSpareRows: 1,
                fixedRowsTop: 1,
                fixedColumnsLeft: 2,
                columns: columns,
                //mergeCells: [{ row: 0, col: 1, rowspan: 2, colspan: 1 }, { row: 0, col: 2, rowspan: 2, colspan: 1 }],
                //before change
                cells: function (row, col, prop) {
                    //console.log("col:" + col + " row:" + row + " prop" + prop);
                    var cellProperties = {};
                    if (row == 0 || row == 1) {
                        cellProperties.renderer = firstRowRendererCabecera;
                    }
                    else if (col == 0 || col == 1) {
                        cellProperties.renderer = firstRowRendererCeleste;
                    }
                    return cellProperties;
                },
            });
            hotRPNS.render();
            //$('#divAcciones').css('display', 'block');
            var iNumRegistros = hotRPNS.countRows();
            if (iNumRegistros >= 1) iNumRegistros = iNumRegistros - 1;
            mostrarExito("Número de registros encontrados: " + iNumRegistros);
        },
        error: function () {
            mostrarError('Lo sentimos no se puede mostrar la hoja de cálculo')
        }
    });

};

grabarExcelRPNS = function () {
    mostrarAlerta('Se esta consultando la información registrada');
    var pericodi = document.getElementById('Pericodi').value;
    var vcrinccodi = document.getElementById('vcrinccodi').value;
    $.ajax({
        type: "POST",
        url: sControlador + 'GrabarGrillaExcelRPNS',
        dataType: "json",
        contentType: 'application/json',
        traditional: true,
        data: JSON.stringify({
            pericodi: pericodi, vcrinccodi: vcrinccodi, datos: hotRPNS.getData()
        }),
        success: function (result) {
            var sError = result.sError;
            var sMensaje = result.sMensaje;
            if (sError == "") {
                var iNumRegistros = hotRPNS.countRows();
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

eliminarDatosRPNS = function () {
    var pericodi = document.getElementById('Pericodi').value;
    var vcrinccodi = document.getElementById('vcrinccodi').value;
    $.ajax({
        type: 'POST',
        url: sControlador + 'eliminardatosRPNS',
        data: { pericodi: pericodi, vcrinccodi: vcrinccodi },
        dataType: 'json',
        success: function (result) {
            if (result == "1") {
                mostrarExito("Felicidades, la información se elimino correctamente...!");
                mostrarGrillaExcelRPNS();
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

descargarArchivoRPNS = function (formato) {
    var pericodi = document.getElementById('Pericodi').value;
    var vcrinccodi = document.getElementById('vcrinccodi').value;
    $.ajax({
        type: 'POST',
        url: sControlador + 'ExportarDataRPNS',
        data: { pericodi: pericodi, vcrinccodi: vcrinccodi, formato: formato },
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

uploadExcelRPNS = function () {
    uploaderRPNS = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: 'btnSelecionarExcelRPNS',
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
                if (uploaderRPNS.files.length == 2) {
                    uploaderRPNS.removeFile(uploaderRPNS.files[0]);
                }
                uploaderRPNS.start();
                up.refresh();
            },
            UploadProgress: function (up, file) {
                mostrarAlerta("Por favor espere ...(<strong>" + file.percent + "%</strong>)");
            },
            UploadComplete: function (up, file) {
                mostrarAlerta("Subida completada <strong> procesando el archivo...</strong>");
                procesarArchivoRPNS(file[0].name);
            },
            Error: function (up, err) {
                mostrarError(err.code + "-" + err.message);
            }
        }
    });
    uploaderRPNS.init();
}

procesarArchivoRPNS = function (sFile) {
    var pericodi = document.getElementById('Pericodi').value;
    var vcrinccodi = document.getElementById('vcrinccodi').value;
    if (typeof hotRPNS != 'undefined') {
        hotRPNS.destroy();
    }
    var container = document.getElementById('grillaExcelRPNS');
    calculateSizeHandsontable(container);
    $.ajax({
        type: 'POST',
        url: sControlador + 'procesararchivoRPNS',
        data: { sarchivo: sFile, pericodi: pericodi, vcrinccodi: vcrinccodi },
        dataType: 'json',
        cache: false,
        success: function (result) {
            var iRegError = result.RegError;
            var sMensajeError = result.MensajeError;
            var columns = result.Columnas;
            //var headers = result.Headers;
            var widths = result.Widths;
            var data = result.Data;
            hotRPNS = new Handsontable(container, {
                data: data,
                //colHeaders: headers,
                rowHeaders: true,
                colWidths: widths,
                contextMenu: true,
                minSpareRows: 1,
                fixedRowsTop: 1,
                fixedColumnsLeft: 2,
                columns: columns,
                //mergeCells: [{ row: 0, col: 1, rowspan: 1, colspan: 2 }, { row: 0, col: 2, rowspan: 1, colspan: 2 }],
                //before change
                cells: function (row, col, prop) {
                    //console.log("col:" + col + " row:" + row + " prop" + prop);
                    var cellProperties = {};
                    if (row == 0 || row == 1) {
                        cellProperties.renderer = firstRowRendererCabecera;
                    }
                    else if (col == 0 || col == 1) {
                        cellProperties.renderer = firstRowRendererCeleste;
                    }
                    return cellProperties;
                },
            });
            hotRPNS.render();
            var iNumRegistros = hotRPNS.countRows();
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

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
function calculateSizeHandsontable(container) {
    var offset = Handsontable.Dom.offset(container);
    var iAltura = $(window).height() - offset.top - 30;
    //console.log($(window).height());
    //console.log(offset.top);
    //console.log(iAltura);
    container.style.height = iAltura + 'px';
    container.style.overflow = 'hidden';
    //container.style.width = '1040px';
}

firstRowRendererCabecera = function (instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);
    td.style.color = 'white';
    td.style.background = '#3D8AB8';
    td.style.fontWeight = 'bold';
    cellProperties.className = "htCenter",
    cellProperties.readOnly = true;
    //console.log(cellProperties);
}

firstRowRendererCeleste = function (instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);
    td.style.color = 'black';
    td.style.background = '#E8F6FF';
}

firstRowRendererAmarillo = function (instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);
    td.style.color = 'black';
    td.style.background = '#FFFFD7';
}