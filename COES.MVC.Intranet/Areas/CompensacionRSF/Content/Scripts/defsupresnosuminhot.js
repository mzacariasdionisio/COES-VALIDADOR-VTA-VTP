var sControlador = siteRoot + "CompensacionRSF/DefSupResNoSumin/";
var iTipoCargaSubida = 1;
var iTipoCargaBajada = 2;

$(document).ready(function () {  
    $('#tab-container').easytabs({
        animate: false
    });

    //--------------------------------------Deficit-----------------------------------------------------
    mostrarGrillaExcelDT();

    $('#btnGrabarExcelDT').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        grabarExcelDT();
    });

    $('#btnEliminarDatosDT').click(function () {
        if (confirm("¿Desea eliminar la información de la hoja de cálculo?")) {
            mostrarAlerta("Espere un momento, se esta procesando su solicitud");
            eliminarDatosDT();
        }
    });

    $('#btnDescargarExcelDT').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        descargarArchivoDT(1);
    });

    uploadExcelDT();
    //-------------------------------------Superavit-----------------------------------------------------
    mostrarGrillaExcelST();

    $('#btnGrabarExcelST').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        grabarExcelST();
    });

    $('#btnEliminarDatosST').click(function () {
        if (confirm("¿Desea eliminar la información de la hoja de cálculo?")) {
            mostrarAlerta("Espere un momento, se esta procesando su solicitud");
            eliminarDatosST();
        }
    });

    $('#btnDescargarExcelST').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        descargarArchivoST(1);
    });

    uploadExcelST();
    //------------------------------------Reserva No Suministrada subida----------------------------------------
    mostrarGrillaExcelRNS();

    $('#btnGrabarExcelRNS').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        grabarExcelRNS();
    });

    $('#btnEliminarDatosRNS').click(function () {
        if (confirm("¿Desea eliminar la información de la hoja de cálculo?")) {
            mostrarAlerta("Espere un momento, se esta procesando su solicitud");
            eliminarDatosRNS();
        }
    });

    $('#btnDescargarExcelRNS').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        descargarArchivoRNS(1);
    });

    uploadExcelRNS();
    //------------------------------------Reserva No Suministrada bajada----------------------------------------
    mostrarGrillaExcelRNSb();

    $('#btnGrabarExcelRNSb').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        grabarExcelRNSb();
    });

    $('#btnEliminarDatosRNSb').click(function () {
        if (confirm("¿Desea eliminar la información de la hoja de cálculo?")) {
            mostrarAlerta("Espere un momento, se esta procesando su solicitud");
            eliminarDatosRNSb();
        }
    });

    $('#btnDescargarExcelRNSb').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        descargarArchivoRNSb(1);
    });

    uploadExcelRNSb();
});

//--------------------------------------Deficit-----------------------------------------------------
mostrarGrillaExcelDT = function () {
    var vcrdsrcodi = document.getElementById('vcrdsrcodi').value;
    error = [];
    if (typeof hotdef != 'undefined') {
        hotdef.destroy();
    }
    var container = document.getElementById('grillaExcelDT');
    $.ajax({
        type: 'POST',
        url: sControlador + "grillaExcelDT",
        data: { vcrdsrcodi: vcrdsrcodi },
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

grabarExcelDT = function () {
    var vcrdsrcodi = document.getElementById('vcrdsrcodi').value;
    $.ajax({
        type: "POST",
        url: sControlador + 'GrabarGrillaExcelDT',
        dataType: "json",
        contentType: 'application/json',
        traditional: true,
        data: JSON.stringify({
            vcrdsrcodi: vcrdsrcodi, datos: hotdef.getData()
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

eliminarDatosDT = function () {
    var vcrdsrcodi = document.getElementById('vcrdsrcodi').value;
    $.ajax({
        type: 'POST',
        url: sControlador + 'eliminardatosdt',
        data: { vcrdsrcodi: vcrdsrcodi },
        dataType: 'json',
        success: function (result) {
            if (result == "1") {
                mostrarExito("Felicidades, la información se elimino correctamente...!");
                mostrarGrillaExcelDT();
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

descargarArchivoDT = function (formato) {
    var vcrdsrcodi = document.getElementById('vcrdsrcodi').value;
    $.ajax({
        type: 'POST',
        url: sControlador + 'ExportarDataDT',
        data: { vcrdsrcodi: vcrdsrcodi, formato: formato },
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

uploadExcelDT = function () {
    uploaderDT = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: 'btnSelecionarExcelDT',
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
                if (uploaderDT.files.length == 2) {
                    uploaderDT.removeFile(uploaderDT.files[0]);
                }
                uploaderDT.start();
                up.refresh();
            },
            UploadProgress: function (up, file) {
                mostrarAlerta("Por favor espere ...(<strong>" + file.percent + "%</strong>)");
            },
            UploadComplete: function (up, file) {
                mostrarAlerta("Subida completada <strong> procesando el archivo...</strong>");
                procesarArchivoDT(file[0].name);
            },
            Error: function (up, err) {
                mostrarError(err.code + "-" + err.message);
            }
        }
    });
    uploaderDT.init();
}

procesarArchivoDT = function (sFile) {
    var vcrdsrcodi = document.getElementById('vcrdsrcodi').value;
    if (typeof hotdef != 'undefined') {
        hotdef.destroy();
    }
    var container = document.getElementById('grillaExcelDT');
    $.ajax({
        type: 'POST',
        url: sControlador + 'procesararchivoDT',
        data: { sarchivo: sFile, vcrdsrcodi: vcrdsrcodi },
        dataType: 'json',
        cache: false,
        success: function (result) {
            var listaempresas = result.ListaEmpresas;
            var listaurs = result.ListaURS;
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

//-------------------------------------Superavit-----------------------------------------------------
mostrarGrillaExcelST = function () {
    var vcrdsrcodi = document.getElementById('vcrdsrcodi').value;
    error = [];
    if (typeof hotsup != 'undefined') {
        hotsup.destroy();
    }
    var container = document.getElementById('grillaExcelST');
    $.ajax({
        type: 'POST',
        url: sControlador + "grillaExcelST",
        data: { vcrdsrcodi: vcrdsrcodi },
        dataType: 'json',
        success: function (result) {
            var columns = result.Columnas;
            var headers = result.Headers;
            var widths = result.Widths;
            var data = result.Data;
            hotsup = new Handsontable(container, {
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
            hotsup.render();
        },
        error: function () {
            mostrarError('Lo sentimos no se puede mostrar la hoja de cálculo')
        }
    });

};

grabarExcelST = function () {
    var vcrdsrcodi = document.getElementById('vcrdsrcodi').value;
    $.ajax({
        type: "POST",
        url: sControlador + 'GrabarGrillaExcelST',
        dataType: "json",
        contentType: 'application/json',
        traditional: true,
        data: JSON.stringify({
            vcrdsrcodi: vcrdsrcodi, datos: hotsup.getData()
        }),
        success: function (result) {
            var sError = result.sError;
            var sMensaje = result.sMensaje;
            if (sError == "") {
                var iNumRegistros = hotsup.countRows();
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

eliminarDatosST = function () {
    var vcrdsrcodi = document.getElementById('vcrdsrcodi').value;
    $.ajax({
        type: 'POST',
        url: sControlador + 'eliminardatosst',
        data: { vcrdsrcodi: vcrdsrcodi },
        dataType: 'json',
        success: function (result) {
            if (result == "1") {
                mostrarExito("Felicidades, la información se elimino correctamente...!");
                mostrarGrillaExcelST();
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

descargarArchivoST = function (formato) {
    var vcrdsrcodi = document.getElementById('vcrdsrcodi').value;
    $.ajax({
        type: 'POST',
        url: sControlador + 'ExportarDataST',
        data: { vcrdsrcodi: vcrdsrcodi, formato: formato },
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

uploadExcelST = function () {
    uploaderST = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: 'btnSelecionarExcelST',
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
                if (uploaderST.files.length == 2) {
                    uploaderST.removeFile(uploaderST.files[0]);
                }
                uploaderST.start();
                up.refresh();
            },
            UploadProgress: function (up, file) {
                mostrarAlerta("Por favor espere ...(<strong>" + file.percent + "%</strong>)");
            },
            UploadComplete: function (up, file) {
                mostrarAlerta("Subida completada <strong> procesando el archivo...</strong>");
                procesarArchivoST(file[0].name);
            },
            Error: function (up, err) {
                mostrarError(err.code + "-" + err.message);
            }
        }
    });
    uploaderST.init();
}

procesarArchivoST = function (sFile) {
    var vcrdsrcodi = document.getElementById('vcrdsrcodi').value;
    if (typeof hotsup != 'undefined') {
        hotsup.destroy();
    }
    var container = document.getElementById('grillaExcelST');
    $.ajax({
        type: 'POST',
        url: sControlador + 'procesararchivoST',
        data: { sarchivo: sFile, vcrdsrcodi: vcrdsrcodi },
        dataType: 'json',
        cache: false,
        success: function (result) {
            var listaempresas = result.ListaEmpresas;
            var listaurs = result.ListaURS;
            var iRegError = result.RegError;
            var sMensajeError = result.MensajeError;
            var bGrabar = result.Grabar;
            var columns = result.Columnas;
            var headers = result.Headers;
            var widths = result.Widths;
            var data = result.Data;
            hotsup = new Handsontable(container, {
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
            hotsup.render();
            var iNumRegistros = hotsup.countRows();
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

//------------------------------------Reserva No Suministrada----------------------------------------
mostrarGrillaExcelRNS = function () {
    var vcrdsrcodi = document.getElementById('vcrdsrcodi').value;
    error = [];
    if (typeof hotrns != 'undefined') {
        hotrns.destroy();
    }
    var container = document.getElementById('grillaExcelRNS');
    $.ajax({
        type: 'POST',
        url: sControlador + "grillaExcelRNS",
        data: { vcrdsrcodi: vcrdsrcodi, vcvrnstipocarga: iTipoCargaSubida },
        dataType: 'json',
        success: function (result) {
            var columns = result.Columnas;
            var headers = result.Headers;
            var widths = result.Widths;
            var data = result.Data;
            hotrns = new Handsontable(container, {
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
            hotrns.render();
        },
        error: function () {
            mostrarError('Lo sentimos no se puede mostrar la hoja de cálculo')
        }
    });

};

grabarExcelRNS = function () {
    var vcrdsrcodi = document.getElementById('vcrdsrcodi').value;
    $.ajax({
        type: "POST",
        url: sControlador + 'GrabarGrillaExcelRNS',
        dataType: "json",
        contentType: 'application/json',
        traditional: true,
        data: JSON.stringify({
            vcrdsrcodi: vcrdsrcodi, datos: hotrns.getData(), vcvrnstipocarga :iTipoCargaSubida
        }),
        success: function (result) {
            var sError = result.sError;
            var sMensaje = result.sMensaje;
            if (sError == "") {
                var iNumRegistros = hotrns.countRows();
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

eliminarDatosRNS = function () {
    var vcrdsrcodi = document.getElementById('vcrdsrcodi').value;
    $.ajax({
        type: 'POST',
        url: sControlador + 'eliminardatosrns',
        data: { vcrdsrcodi: vcrdsrcodi, vcvrnstipocarga: iTipoCargaSubida },
        dataType: 'json',
        success: function (result) {
            if (result == "1") {
                mostrarExito("Felicidades, la información se elimino correctamente...!");
                mostrarGrillaExcelRNS();
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

descargarArchivoRNS = function (formato) {
    var vcrdsrcodi = document.getElementById('vcrdsrcodi').value;
    $.ajax({
        type: 'POST',
        url: sControlador + 'ExportarDataRNS',
        data: { vcrdsrcodi: vcrdsrcodi, vcvrnstipocarga: iTipoCargaSubida, formato: formato },
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

uploadExcelRNS = function () {
    uploaderRNS = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: 'btnSelecionarExcelRNS',
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
                if (uploaderRNS.files.length == 2) {
                    uploaderRNS.removeFile(uploaderRNS.files[0]);
                }
                uploaderRNS.start();
                up.refresh();
            },
            UploadProgress: function (up, file) {
                mostrarAlerta("Por favor espere ...(<strong>" + file.percent + "%</strong>)");
            },
            UploadComplete: function (up, file) {
                mostrarAlerta("Subida completada <strong> procesando el archivo...</strong>");
                procesarArchivoRNS(file[0].name);
            },
            Error: function (up, err) {
                mostrarError(err.code + "-" + err.message);
            }
        }
    });
    uploaderRNS.init();
}

procesarArchivoRNS = function (sFile) {
    var vcrdsrcodi = document.getElementById('vcrdsrcodi').value;
    if (typeof hotrns != 'undefined') {
        hotrns.destroy();
    }
    var container = document.getElementById('grillaExcelRNS');
    $.ajax({
        type: 'POST',
        url: sControlador + 'procesararchivoRNS',
        data: { sarchivo: sFile, vcrdsrcodi: vcrdsrcodi, vcvrnstipocarga: iTipoCargaSubida },
        dataType: 'json',
        cache: false,
        success: function (result) {
            var listaempresas = result.ListaEmpresas;
            var listaurs = result.ListaURS;
            var iRegError = result.RegError;
            var sMensajeError = result.MensajeError;
            var bGrabar = result.Grabar;
            var columns = result.Columnas;
            var headers = result.Headers;
            var widths = result.Widths;
            var data = result.Data;
            hotrns = new Handsontable(container, {
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
            hotrns.render();
            var iNumRegistros = hotrns.countRows();
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

//------------------------------------Reserva No Suministrada bajada---------------------------------
mostrarGrillaExcelRNSb = function () {
    var vcrdsrcodi = document.getElementById('vcrdsrcodi').value;
    error = [];
    if (typeof hotrns != 'undefined') {
        hotrnsb.destroy();
    }
    var container = document.getElementById('grillaExcelRNSb');
    $.ajax({
        type: 'POST',
        url: sControlador + "grillaExcelRNS",
        data: { vcrdsrcodi: vcrdsrcodi, vcvrnstipocarga: iTipoCargaBajada },
        dataType: 'json',
        success: function (result) {
            var columns = result.Columnas;
            var headers = result.Headers;
            var widths = result.Widths;
            var data = result.Data;
            hotrnsb = new Handsontable(container, {
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
            hotrnsb.render();
        },
        error: function () {
            mostrarError('Lo sentimos no se puede mostrar la hoja de cálculo')
        }
    });
};

grabarExcelRNSb = function () {
    var vcrdsrcodi = document.getElementById('vcrdsrcodi').value;
    $.ajax({
        type: "POST",
        url: sControlador + 'GrabarGrillaExcelRNS',
        dataType: "json",
        contentType: 'application/json',
        traditional: true,
        data: JSON.stringify({
            vcrdsrcodi: vcrdsrcodi, datos: hotrnsb.getData(), vcvrnstipocarga: iTipoCargaBajada
        }),
        success: function (result) {
            var sError = result.sError;
            var sMensaje = result.sMensaje;
            if (sError == "") {
                var iNumRegistros = hotrnsb.countRows();
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

eliminarDatosRNSb = function () {
    var vcrdsrcodi = document.getElementById('vcrdsrcodi').value;
    $.ajax({
        type: 'POST',
        url: sControlador + 'eliminardatosrns',
        data: { vcrdsrcodi: vcrdsrcodi, vcvrnstipocarga: iTipoCargaBajada },
        dataType: 'json',
        success: function (result) {
            if (result == "1") {
                mostrarExito("Felicidades, la información se elimino correctamente...!");
                mostrarGrillaExcelRNSb();
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

descargarArchivoRNSb = function (formato) {
    var vcrdsrcodi = document.getElementById('vcrdsrcodi').value;
    $.ajax({
        type: 'POST',
        url: sControlador + 'ExportarDataRNS',
        data: { vcrdsrcodi: vcrdsrcodi, vcvrnstipocarga: iTipoCargaBajada, formato: formato },
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

uploadExcelRNSb = function () {
    uploaderRNSb = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: 'btnSelecionarExcelRNSb',
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
                if (uploaderRNSb.files.length == 2) {
                    uploaderRNSb.removeFile(uploaderRNSb.files[0]);
                }
                uploaderRNSb.start();
                up.refresh();
            },
            UploadProgress: function (up, file) {
                mostrarAlerta("Por favor espere ...(<strong>" + file.percent + "%</strong>)");
            },
            UploadComplete: function (up, file) {
                mostrarAlerta("Subida completada <strong> procesando el archivo...</strong>");
                procesarArchivoRNSb(file[0].name);
            },
            Error: function (up, err) {
                mostrarError(err.code + "-" + err.message);
            }
        }
    });
    uploaderRNSb.init();
}

procesarArchivoRNSb = function (sFile) {
    var vcrdsrcodi = document.getElementById('vcrdsrcodi').value;
    if (typeof hotrnsb != 'undefined') {
        hotrnsb.destroy();
    }
    var container = document.getElementById('grillaExcelRNSb');
    $.ajax({
        type: 'POST',
        url: sControlador + 'procesararchivoRNS',
        data: { sarchivo: sFile, vcrdsrcodi: vcrdsrcodi, vcvrnstipocarga: iTipoCargaBajada },
        dataType: 'json',
        cache: false,
        success: function (result) {
            var listaempresas = result.ListaEmpresas;
            var listaurs = result.ListaURS;
            var iRegError = result.RegError;
            var sMensajeError = result.MensajeError;
            var bGrabar = result.Grabar;
            var columns = result.Columnas;
            var headers = result.Headers;
            var widths = result.Widths;
            var data = result.Data;
            hotrnsb = new Handsontable(container, {
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
            hotrnsb.render();
            var iNumRegistros = hotrnsb.countRows();
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