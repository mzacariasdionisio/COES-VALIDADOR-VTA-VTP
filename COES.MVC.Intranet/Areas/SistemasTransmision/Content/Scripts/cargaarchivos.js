var controler = siteRoot + "sistemastransmision/cargaarchivos/";
var error = [];
$(document).ready(function () {
    $('#tab-container').easytabs({
        animate: false
    });

    $('#btnConsultarDE').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        mostrarGrillaExcelDE();
    });

    $('#btnDescargarExcelDE').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        descargarArchivoDE(1);
    });

    $('#btnValidarGrillaExcelDE').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        if (parseInt(error.length) > 0) {
            abrirPopupEN();
        } else {
            mostrarExito("La hoja de cálculo no tiene errores...!");
        }
    });

    $('#btnEliminarDatosDE').click(function () {
        if (confirm("¿Desea eliminar la información de la hoja de cálculo?")) {
            mostrarAlerta("Espere un momento, se esta procesando su solicitud");
            eliminarDatosDE();
        }
    });

    $('#btnGrabarExcelDE').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        if (parseInt(error.length) > 0) {
            abrirPopupEN();
        } else {
            grabarExcelDE();
        }
    });

    uploadExcelDE();
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    $('#btnConsultarEN').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        mostrarGrillaExcelEN();
    });

    $('#btnDescargarExcelEN').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        descargarArchivoEN(1);
    });

    $('#btnValidarGrillaExcelEN').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        if (parseInt(error.length) > 0) {
            abrirPopupEN();
        } else {
            mostrarExito("La hoja de cálculo no tiene errores...!");
        }
    });

    $('#btnEliminarDatosEN').click(function () {
        if (confirm("¿Desea eliminar la información de la hoja de cálculo?")) {
            mostrarAlerta("Espere un momento, se esta procesando su solicitud");
            eliminarDatosEN();
        }
    });

    $('#btnGrabarExcelEN').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        if (parseInt(error.length) > 0) {
            abrirPopupEN();
        } else {
            grabarExcelEN();
        }
    });

    uploadExcelEN();
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    $('#btnConsultarFA').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        mostrarGrillaExcelFA();
    });

    $('#btnDescargarExcelFA').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        descargarArchivoFA(1);
    });

    $('#btnValidarGrillaExcelFA').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        if (parseInt(error.length) > 0) {
            abrirPopupFA();
        } else {
            mostrarExito("La hoja de cálculo no tiene errores...!");
        }
    });

    $('#btnEliminarDatosFA').click(function () {
        if (confirm("¿Desea eliminar la información de la hoja de cálculo?")) {
            mostrarAlerta("Espere un momento, se esta procesando su solicitud");
            eliminarDatosFA();
            //mostrarGrillaExcelFA();
        }
    });

    $('#btnGrabarExcelFA').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        if (parseInt(error.length) > 0) {
            abrirPopupFA();
        } else {
            grabarExcelFA();
        }
    });

    uploadExcelFA();

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    $('#btnConsultarCR').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        mostrarGrillaExcelCR();
    });

    $('#btnGrabarExcelCR').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        if (parseInt(error.length) > 0) {
            abrirPopupCR();
        } else {
            grabarExcelCR();
        }
    });

    $('#btnEliminarDatosCR').click(function () {
        if (confirm("¿Desea eliminar la información de la hoja de cálculo?")) {
            mostrarAlerta("Espere un momento, se esta procesando su solicitud");
            eliminarDatosCR();
            //mostrarGrillaExcelCR();
        }
    });

    $('#btnEliminarDatosCR2').click(function () {
        if (confirm("¿Desea eliminar la información de la hoja de cálculo?")) {
            mostrarAlerta("Espere un momento, se esta procesando su solicitud");
            eliminarDatosCR();
            //mostrarGrillaExcelCR();
        }
    });

    $('#btnDescargarExcelCR').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        descargarArchivoCR(1);
    });

    $('#btnValidarGrillaExcelCR').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        if (parseInt(error.length) > 0) {
            abrirPopupCR();
        } else {
            mostrarExito("La hoja de cálculo no tiene errores...!");
        }
    });

    uploadExcelCR();

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    $('#btnConsultarCM').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        mostrarGrillaExcelCM();
    });

    $('#btnGrabarExcelCM').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        if (parseInt(error.length) > 0) {
            abrirPopupCM();
        } else {
            grabarExcelCM();
        }
    });

    $('#btnEliminarDatosCM').click(function () {
        if (confirm("¿Desea eliminar la información de la hoja de cálculo?")) {
            mostrarAlerta("Espere un momento, se esta procesando su solicitud");
            eliminarDatosCM();
            //mostrarGrillaExcelCR();
        }
    });

    $('#btnEliminarDatosCM2').click(function () {
        if (confirm("¿Desea eliminar la información de la hoja de cálculo?")) {
            mostrarAlerta("Espere un momento, se esta procesando su solicitud");
            eliminarDatosCM();
            //mostrarGrillaExcelCR();
        }
    });

    $('#btnDescargarExcelCM').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        descargarArchivoCM(1);
    });

    $('#btnValidarGrillaExcelCM').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        if (parseInt(error.length) > 0) {
            abrirPopupCM();
        } else {
            mostrarExito("La hoja de cálculo no tiene errores...!");
        }
    });

    uploadExcelCM();
});

////////////////////////////////////////////RESPONSABILIDAD DE PAGO//////////////////////////////////////////////////
recargarCR = function () {
    var cmbPericodi = document.getElementById('stpercodiCR');
    window.location.href = controler + "index?stpercodi=" + cmbPericodi.value + "#paso4";
}

mostrarGrillaExcelCR = function () {
    error = [];
    if (typeof hot != 'undefined') {
        hot.destroy();
    }
    var container = document.getElementById('grillaExcelCR');
    $.ajax({
        type: 'POST',
        url: controler + "grillaexcelcr",
        data: { stpercodi: $('#stpercodiCR').val(), strecacodi: $('#strecacodiCR').val() },
        dataType: 'json',
        success: function (result) {
            //var ListaSistemaTrans = result.ListaSistemaTrans;
            var ListaCentralesGen = result.ListaCentralesGen;
            var bGrabar = result.Grabar;
            var columns = result.Columnas;
            var iNroColumnas = result.NumeroColumnas;
            //var headers = result.Headers;
            var widths = result.Widths;
            var data = result.Data;
            var sRegExVal = new RegExp(/^-{0,1}\d*\.{0,1}\d+$/); //^\d+(?:[\.\,]\d+)?$
            hot = new Handsontable(container, {
                data: data,
                maxCols: result.Columnas.length,
                colHeaders: false,
                rowHeaders: true,
                colWidths: widths,
                contextMenu: true,
                minSpareRows: 1,
                columnSorting: false,
                columns: columns,
                fixedRowsTop: result.FixedRowsTop,
                fixedColumnsLeft: result.FixedColumnsLeft,
                beforeChange: function (changes, source) {
                    for (var i = changes.length - 1; i >= 0; i--) {
                        var numFila = changes[i][0];
                        var numColumna = changes[i][1];
                        var sValorAnterior = changes[i][2];
                        var sNuevoValor = changes[i][3];
                        var lista;
                        var bEsNumero;
                        var header;
                        var sMensaje;
                        if (numColumna == 0) {
                            lista = ListaCentralesGen;
                            bEsNumero = null;
                            header = "Central Generación";
                            sMensaje = "Nombre de Central de Generación incorrecto";
                        }
                            //else if (numColumna == 1) {
                            //    lista = ListaSistemaTrans;
                            //    bEsNumero = null;
                            //    header = "Sistema Transmisión";
                            //    sMensaje = "Nombre del Sistema incorrecto";
                            //}
                        else if ((numColumna % 2) == 1) {
                            //Valida si el nuevo valor de la celda R(pu)
                            lista = null;
                            if (sValorAnterior != null) sValorAnterior = sValorAnterior.toString();
                            if (sValorAnterior != null) sValorAnterior = sValorAnterior.toString();
                            if (sNuevoValor != null) {
                                if (sNuevoValor == 1 || sNuevoValor == 0) { sNuevoValor = sNuevoValor.toString(); }
                                else {
                                    sNuevoValor = sNuevoValor.toString();
                                    sNuevoValor = "";
                                }
                            } else { return true; }
                            bEsNumero = sRegExVal.test(sNuevoValor);
                            header = "ELEMENTO";
                            sMensaje = "Valor del elemento en " + data[0][numColumna] + " no es válido";
                        }
                        else if ((numColumna % 2) == 0) {
                            //Valida si el nuevo valor de la celda X(pu)
                            lista = null;
                            if (sValorAnterior != null) sValorAnterior = sValorAnterior.toString();
                            if (sValorAnterior != null) sValorAnterior = sValorAnterior.toString();
                            if (sNuevoValor != null) {
                                if (sNuevoValor == 1 || sNuevoValor == 0) { sNuevoValor = sNuevoValor.toString(); }
                                else {
                                    sNuevoValor = sNuevoValor.toString();
                                    sNuevoValor = "";
                                }
                            } else { return true; }
                            bEsNumero = sRegExVal.test(sNuevoValor);
                            header = "ELEMENTO";
                            sMensaje = "Valor del elemento en " + data[0][numColumna] + " no es válido";
                        }
                        else {
                            lista = null;
                            bEsNumero = null;
                            header = "";
                            sMensaje = "";
                        }
                        if ((bEsNumero == true) || ($.inArray(sNuevoValor, lista) > -1)) {   //Valida si el valor anterior se encuentra en el array "error"
                            if (sValorAnterior !== null) {
                                if ($.inArray(sValorAnterior.concat("_-_" + numFila + "_-_" + header + "_-_" + sMensaje), error) > -1) {
                                    error = $.grep(error, function (value) { return value != sValorAnterior.concat("_-_" + numFila + "_-_" + header + "_-_" + sMensaje) });
                                }
                            }
                        }
                        else {
                            if (sNuevoValor === "") {
                                return false;
                            }
                            else {
                                if ($.inArray(sNuevoValor.concat("_-_" + numFila + "_-_" + header + "_-_" + sMensaje), error) > -1) {
                                }
                                else {
                                    error.push(sNuevoValor.concat("_-_" + numFila + "_-_" + header + "_-_" + sMensaje));
                                }
                            }
                        }
                    }
                },
                cells: function (row, col, prop) {
                    //console.log("col:" + col + " row:" + row + " prop" + prop);
                    var cellProperties = {};
                    if (row == 0) {
                        cellProperties.renderer = firstRowRendererCabecera;
                    }
                    else if (col == 0) {
                        cellProperties.renderer = firstRowRendererCeleste;
                    }
                    //else if (col >= 4) {
                    //    //Para el llenado de datos
                    //    cellProperties.renderer = "negativeValueRenderer";
                    //}
                    return cellProperties;
                },
            });
            hot.render();
            if (!bGrabar) {
                $('#btnValidarGrillaExcelCR').css('display', 'block');
                $('#btnGrabarExcelCR').css('display', 'block');
                $('#btnEliminarDatosCR2').css('display', 'block');
            }
            else {
                $('#btnValidarGrillaExcelCR').css('display', 'none');
                $('#btnGrabarExcelCR').css('display', 'none');
                $('#btnEliminarDatosCR2').css('display', 'none');
            }
            $('#divAccionesCR').css('display', 'block');
            var iNumRegistros = hot.countRows();
            if (iNumRegistros >= 1) iNumRegistros = iNumRegistros - 2;
            mostrarExito('Se han encontrado ' + iNumRegistros + ' registro(s)');
        },
        error: function () {
            mostrarError('Lo sentimos no se puede mostrar la hoja de cálculo')
        }
    });
}

grabarExcelCR = function () {
    $.ajax({
        type: "POST",
        url: controler + 'grabargrillaexcelcr',
        dataType: "json",
        contentType: 'application/json',
        traditional: true,
        data: JSON.stringify({
            stpercodi: $('#stpercodiCR').val(), strecacodi: $('#strecacodiCR').val(), datos: hot.getData()
        }),
        success: function (result) {
            var sError = result.sError;
            var sMensaje = result.sMensaje;
            if (sError == "") {
                var iNumRegistros = hot.countRows();
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

eliminarDatosCR = function () {
    $.ajax({
        type: 'POST',
        url: controler + 'eliminardatoscr',
        data: { stpercodi: $('#stpercodiCR').val(), strecacodi: $('#strecacodiCR').val() },
        dataType: 'json',
        success: function (result) {
            if (result == "1") {
                alert("Felicidades, la información se elimino correctamente...!");
                mostrarGrillaExcelCR(0);
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

descargarArchivoCR = function (formato) {
    $.ajax({
        type: 'POST',
        url: controler + 'exportardatacr',
        data: { stpercodi: $('#stpercodiCR').val(), strecacodi: $('#strecacodiCR').val(), formato: formato },
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

abrirPopupCR = function () {
    var html = '<span class="button b-close"><span>X</span></span>';
    html += '<p><b>Corregir los siguientes errores</b><p>';
    html += '<table border="0" class="pretty tabla-icono" id="tabla">'
    html += '<thead>'
    html += '<tr>'
    html += '<th>Fila</th>'
    html += '<th>Columna</th>'
    html += '<th>Valor</th>'
    html += '<th>Comentario</th>'
    html += '</tr>'
    html += '</thead>'
    html += '<tbody>'
    for (var i = error.length - 1; i >= 0 ; i--) {
        var sStyle = "background : #ffffff;";
        if (i % 2)
            var sStyle = "background : #fbf4bf;";
        var SplitError = error[i].split("_-_");
        html += '<tr id="Fila_' + i + '">'
        html += '<td style="text-align:right;' + sStyle + '">' + (parseInt(SplitError[1]) + 1) + '</td>'
        html += '<td style="text-align:left;' + sStyle + '">' + SplitError[2] + '</td>'
        html += '<td style="text-align:left;' + sStyle + '">' + SplitError[0] + '</td>'
        html += '<td style="text-align:left;' + sStyle + '">' + SplitError[3] + '</td>'
        html += '</tr>'
    }
    html += '</tbody>'
    html += '</table>'

    $('#popup').html(html);
    $('#popup').bPopup({
        easing: 'easeOutBack',
        speed: 450,
        transition: 'slideDown'
    });
    mostrarError("Lo sentimos, la hoja del cálculo tiene errores");
}

uploadExcelCR = function () {
    uploaderCR = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: 'btnSelecionarExcelCR',
        url: controler + 'uploadexcel',
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
                if (uploaderCR.files.length == 2) {
                    uploaderCR.removeFile(uploaderCR.files[0]);
                }
                uploaderCR.start();
                up.refresh();
            },
            UploadProgress: function (up, file) {
                mostrarAlerta("Por favor espere ...(<strong>" + file.percent + "%</strong>)");
            },
            UploadComplete: function (up, file) {
                mostrarAlerta("Subida completada <strong> procesando el archivo...</strong>");
                procesarArchivoCR(file[0].name);
            },
            Error: function (up, err) {
                mostrarError(err.code + "-" + err.message);
            }
        }
    });
    uploaderCR.init();
}

procesarArchivoCR = function (sFile) {
    error = [];
    if (typeof hot != 'undefined') {
        hot.destroy();
    }
    var container = document.getElementById('grillaExcelCR');
    $.ajax({
        type: 'POST',
        url: controler + "procesararchivocr",
        data: { sarchivo: sFile, stpercodi: $('#stpercodiCR').val(), strecacodi: $('#strecacodiCR').val() },
        dataType: 'json',
        success: function (result) {
            //var ListaSistemaTrans = result.ListaSistemaTrans;
            var ListaCentralesGen = result.ListaCentralesGen;
            var bGrabar = result.Grabar;
            var columns = result.Columnas;
            var iNroColumnas = result.NumeroColumnas;
            //var headers = result.Headers;
            var widths = result.Widths;
            var data = result.Data;
            var sRegExVal = new RegExp(/^-{0,1}\d*\.{0,1}\d+$/); //^\d+(?:[\.\,]\d+)?$
            hot = new Handsontable(container, {
                data: data,
                maxCols: result.Columnas.length,
                colHeaders: false,
                rowHeaders: true,
                colWidths: widths,
                contextMenu: true,
                minSpareRows: 1,
                columnSorting: false,
                columns: columns,
                fixedRowsTop: result.FixedRowsTop,
                fixedColumnsLeft: result.FixedColumnsLeft,
                beforeChange: function (changes, source) {
                    for (var i = changes.length - 1; i >= 0; i--) {
                        var numFila = changes[i][0];
                        var numColumna = changes[i][1];
                        var sValorAnterior = changes[i][2];
                        var sNuevoValor = changes[i][3];
                        var lista;
                        var bEsNumero;
                        var header;
                        var sMensaje;
                        if (numColumna == 0) {
                            lista = ListaCentralesGen;
                            bEsNumero = null;
                            header = "Central Generación";
                            sMensaje = "Nombre de Central de Generación incorrecto";
                        }
                            //else if (numColumna == 1) {
                            //    lista = ListaSistemaTrans;
                            //    bEsNumero = null;
                            //    header = "Sistema Transmisión";
                            //    sMensaje = "Nombre del Sistema incorrecto";
                            //}
                        else if ((numColumna % 2) == 1) {
                            //Valida si el nuevo valor de la celda R(pu)
                            lista = null;
                            if (sValorAnterior != null) sValorAnterior = sValorAnterior.toString();
                            if (sValorAnterior != null) sValorAnterior = sValorAnterior.toString();
                            if (sNuevoValor != null) {
                                if (sNuevoValor == 1 || sNuevoValor == 0) { sNuevoValor = sNuevoValor.toString(); }
                                else {
                                    sNuevoValor = sNuevoValor.toString();
                                    sNuevoValor = "";
                                }
                            } else { return true; }
                            bEsNumero = sRegExVal.test(sNuevoValor);
                            header = "ELEMENTO";
                            sMensaje = "Valor del elemento en " + data[0][numColumna] + " no es válido";
                        }
                        else if ((numColumna % 2) == 0) {
                            //Valida si el nuevo valor de la celda X(pu)
                            lista = null;
                            if (sValorAnterior != null) sValorAnterior = sValorAnterior.toString();
                            if (sValorAnterior != null) sValorAnterior = sValorAnterior.toString();
                            if (sNuevoValor != null) {
                                if (sNuevoValor == 1 || sNuevoValor == 0) { sNuevoValor = sNuevoValor.toString(); }
                                else {
                                    sNuevoValor = sNuevoValor.toString();
                                    sNuevoValor = "";
                                }
                            } else { return true; }
                            bEsNumero = sRegExVal.test(sNuevoValor);
                            header = "ELEMENTO";
                            sMensaje = "Valor del elemento en " + data[0][numColumna] + " no es válido";
                        }
                        else {
                            lista = null;
                            bEsNumero = null;
                            header = "";
                            sMensaje = "";
                        }
                        if ((bEsNumero == true) || ($.inArray(sNuevoValor, lista) > -1)) {   //Valida si el valor anterior se encuentra en el array "error"
                            if (sValorAnterior !== null) {
                                if ($.inArray(sValorAnterior.concat("_-_" + numFila + "_-_" + header + "_-_" + sMensaje), error) > -1) {
                                    error = $.grep(error, function (value) { return value != sValorAnterior.concat("_-_" + numFila + "_-_" + header + "_-_" + sMensaje) });
                                }
                            }
                        }
                        else {
                            if (sNuevoValor === "") {
                                return false;
                            }
                            else {
                                if ($.inArray(sNuevoValor.concat("_-_" + numFila + "_-_" + header + "_-_" + sMensaje), error) > -1) {
                                }
                                else {
                                    error.push(sNuevoValor.concat("_-_" + numFila + "_-_" + header + "_-_" + sMensaje));
                                }
                            }
                        }
                    }
                },
                cells: function (row, col, prop) {
                    //console.log("col:" + col + " row:" + row + " prop" + prop);
                    var cellProperties = {};
                    if (row == 0) {
                        cellProperties.renderer = firstRowRendererCabecera;
                    }
                    else if (col == 0) {
                        cellProperties.renderer = firstRowRendererCeleste;
                    }
                    //else if (col >= 4) {
                    //    //Para el llenado de datos
                    //    cellProperties.renderer = "negativeValueRenderer";
                    //}
                    return cellProperties;
                },
            });
            hot.render();
            if (!bGrabar) {
                $('#btnValidarGrillaExcelCR').css('display', 'block');
                $('#btnGrabarExcelCR').css('display', 'block');
                $('#btnEliminarDatosCR2').css('display', 'block');
            }
            else {
                $('#btnValidarGrillaExcelCR').css('display', 'none');
                $('#btnGrabarExcelCR').css('display', 'none');
                $('#btnEliminarDatosCR2').css('display', 'none');
            }
            $('#divAccionesCR').css('display', 'block');
            var iNumRegistros = hot.countRows();
            if (iNumRegistros >= 1) iNumRegistros = iNumRegistros - 2;
            mostrarExito('Se han encontrado ' + iNumRegistros + ' registro(s)');
        },
        error: function () {
            mostrarError('Lo sentimos no se puede mostrar la hoja de cálculo')
        }
    });
}

///////////////////////////////////////////////CENTRALES MENSUALES/////////////////////////////////////////////
recargarCM = function () {
    var cmbPericodi = document.getElementById('stpercodiCM');
    window.location.href = controler + "index?stpercodi=" + cmbPericodi.value + "#paso5";
}

mostrarGrillaExcelCM = function () {
    error = [];
    if (typeof hot != 'undefined') {
        hot.destroy();
    }
    var container = document.getElementById('grillaExcelCM');
    $.ajax({
        type: 'POST',
        url: controler + "grillaexcelcm",
        data: { stpercodi: $('#stpercodiCM').val(), strecacodi: $('#strecacodiCM').val() },
        dataType: 'json',
        success: function (result) {
            //var ListaSistemaTrans = result.ListaSistemaTrans;
            var ListaCentralesGen = result.ListaCentralesGen;
            var bGrabar = result.Grabar;
            var columns = result.Columnas;
            var iNroColumnas = result.NumeroColumnas;
            //var headers = result.Headers;
            var widths = result.Widths;
            var data = result.Data;
            var sRegExVal = new RegExp(/^-{0,1}\d*\.{0,1}\d+$/); //^\d+(?:[\.\,]\d+)?$
            hot = new Handsontable(container, {
                data: data,
                maxCols: result.Columnas.length,
                colHeaders: false,
                rowHeaders: true,
                colWidths: widths,
                contextMenu: true,
                minSpareRows: 1,
                columnSorting: false,
                columns: columns,
                fixedRowsTop: result.FixedRowsTop,
                fixedColumnsLeft: result.FixedColumnsLeft,
                beforeChange: function (changes, source) {
                    for (var i = changes.length - 1; i >= 0; i--) {
                        var numFila = changes[i][0];
                        var numColumna = changes[i][1];
                        var sValorAnterior = changes[i][2];
                        var sNuevoValor = changes[i][3];
                        var lista;
                        var bEsNumero;
                        var header;
                        var sMensaje;
                        if (numColumna == 0) {
                            lista = ListaCentralesGen;
                            bEsNumero = null;
                            header = "Central Generación";
                            sMensaje = "Nombre de Central de Generación incorrecto";
                        }
                            //else if (numColumna == 1) {
                            //    lista = ListaSistemaTrans;
                            //    bEsNumero = null;
                            //    header = "Sistema Transmisión";
                            //    sMensaje = "Nombre del Sistema incorrecto";
                            //}
                        else if ((numColumna % 2) == 1) {
                            //Valida si el nuevo valor de la celda R(pu)
                            lista = null;
                            if (sValorAnterior != null) sValorAnterior = sValorAnterior.toString();
                            if (sNuevoValor != null) sNuevoValor = sNuevoValor.toString();
                            bEsNumero = sRegExVal.test(sNuevoValor);
                            header = "ELEMENTO";
                            sMensaje = "Valor del elemento en " + data[0][numColumna] + " no es válido";
                        }
                        else if ((numColumna % 2) == 0) {
                            //Valida si el nuevo valor de la celda X(pu)
                            lista = null;
                            if (sValorAnterior != null) sValorAnterior = sValorAnterior.toString();
                            if (sNuevoValor != null) sNuevoValor = sNuevoValor.toString();
                            bEsNumero = sRegExVal.test(sNuevoValor);
                            header = "ELEMENTO";
                            sMensaje = "Valor del elemento en " + data[0][numColumna] + " no es válido";
                        }
                        else {
                            lista = null;
                            bEsNumero = null;
                            header = "";
                            sMensaje = "";
                        }
                        if ((bEsNumero == true) || ($.inArray(sNuevoValor, lista) > -1)) {   //Valida si el valor anterior se encuentra en el array "error"
                            if (sValorAnterior !== null) {
                                if ($.inArray(sValorAnterior.concat("_-_" + numFila + "_-_" + header + "_-_" + sMensaje), error) > -1) {
                                    error = $.grep(error, function (value) { return value != sValorAnterior.concat("_-_" + numFila + "_-_" + header + "_-_" + sMensaje) });
                                }
                            }
                        }
                        else {
                            if (sNuevoValor === "") {
                                return false;
                            }
                            else {
                                if ($.inArray(sNuevoValor.concat("_-_" + numFila + "_-_" + header + "_-_" + sMensaje), error) > -1) {
                                }
                                else {
                                    error.push(sNuevoValor.concat("_-_" + numFila + "_-_" + header + "_-_" + sMensaje));
                                }
                            }
                        }
                    }
                },
                cells: function (row, col, prop) {
                    //console.log("col:" + col + " row:" + row + " prop" + prop);
                    var cellProperties = {};
                    if (row == 0) {
                        cellProperties.renderer = firstRowRendererCabecera;
                    }
                    else if (col == 0) {
                        cellProperties.renderer = firstRowRendererCeleste;
                    }
                    //else if (col >= 4) {
                    //    //Para el llenado de datos
                    //    cellProperties.renderer = "negativeValueRenderer";
                    //}
                    return cellProperties;
                },
            });
            hot.render();
            if (!bGrabar) {
                $('#btnValidarGrillaExcelCM').css('display', 'block');
                $('#btnGrabarExcelCM').css('display', 'block');
                $('#btnEliminarDatosCM2').css('display', 'block');
            }
            else {
                $('#btnValidarGrillaExcelCM').css('display', 'none');
                $('#btnGrabarExcelCM').css('display', 'none');
                $('#btnEliminarDatosCM2').css('display', 'none');
            }
            $('#divAccionesCM').css('display', 'block');
            var iNumRegistros = hot.countRows();
            if (iNumRegistros >= 1) iNumRegistros = iNumRegistros - 2;
            mostrarExito('Se han encontrado ' + iNumRegistros + ' registro(s)');
        },
        error: function () {
            mostrarError('Lo sentimos no se puede mostrar la hoja de cálculo')
        }
    });
}

grabarExcelCM = function () {
    $.ajax({
        type: "POST",
        url: controler + 'grabargrillaexcelcm',
        dataType: "json",
        contentType: 'application/json',
        traditional: true,
        data: JSON.stringify({
            stpercodi: $('#stpercodiCM').val(), strecacodi: $('#strecacodiCM').val(), datos: hot.getData()
        }),
        success: function (result) {
            var sError = result.sError;
            var sMensaje = result.sMensaje;
            if (sError == "") {
                var iNumRegistros = hot.countRows();
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

eliminarDatosCM = function () {
    $.ajax({
        type: 'POST',
        url: controler + 'eliminardatoscm',
        data: { stpercodi: $('#stpercodiCM').val(), strecacodi: $('#strecacodiCM').val() },
        dataType: 'json',
        success: function (result) {
            if (result == "1") {
                alert("Felicidades, la información se elimino correctamente...!");
                mostrarGrillaExcelCM(0);
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

descargarArchivoCM = function (formato) {
    $.ajax({
        type: 'POST',
        url: controler + 'exportardatacm',
        data: { stpercodi: $('#stpercodiCM').val(), strecacodi: $('#strecacodiCM').val(), formato: formato },
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

abrirPopupCM = function () {
    var html = '<span class="button b-close"><span>X</span></span>';
    html += '<p><b>Corregir los siguientes errores</b><p>';
    html += '<table border="0" class="pretty tabla-icono" id="tabla">'
    html += '<thead>'
    html += '<tr>'
    html += '<th>Fila</th>'
    html += '<th>Columna</th>'
    html += '<th>Valor</th>'
    html += '<th>Comentario</th>'
    html += '</tr>'
    html += '</thead>'
    html += '<tbody>'
    for (var i = error.length - 1; i >= 0 ; i--) {
        var sStyle = "background : #ffffff;";
        if (i % 2)
            var sStyle = "background : #fbf4bf;";
        var SplitError = error[i].split("_-_");
        html += '<tr id="Fila_' + i + '">'
        html += '<td style="text-align:right;' + sStyle + '">' + (parseInt(SplitError[1]) + 1) + '</td>'
        html += '<td style="text-align:left;' + sStyle + '">' + SplitError[2] + '</td>'
        html += '<td style="text-align:left;' + sStyle + '">' + SplitError[0] + '</td>'
        html += '<td style="text-align:left;' + sStyle + '">' + SplitError[3] + '</td>'
        html += '</tr>'
    }
    html += '</tbody>'
    html += '</table>'

    $('#popup').html(html);
    $('#popup').bPopup({
        easing: 'easeOutBack',
        speed: 450,
        transition: 'slideDown'
    });
    mostrarError("Lo sentimos, la hoja del cálculo tiene errores");
}

uploadExcelCM = function () {
    uploaderCM = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: 'btnSelecionarExcelCM',
        url: controler + 'uploadexcel',
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
                if (uploaderCM.files.length == 2) {
                    uploaderCM.removeFile(uploaderCM.files[0]);
                }
                uploaderCM.start();
                up.refresh();
            },
            UploadProgress: function (up, file) {
                mostrarAlerta("Por favor espere ...(<strong>" + file.percent + "%</strong>)");
            },
            UploadComplete: function (up, file) {
                mostrarAlerta("Subida completada <strong> procesando el archivo...</strong>");
                procesarArchivoCM(file[0].name);
            },
            Error: function (up, err) {
                mostrarError(err.code + "-" + err.message);
            }
        }
    });
    uploaderCM.init();
    browse_button: '';
}

procesarArchivoCM = function (sFile) {
    error = [];
    if (typeof hot != 'undefined') {
        hot.destroy();
    }
    var container = document.getElementById('grillaExcelCM');
    $.ajax({
        type: 'POST',
        url: controler + "procesararchivocm",
        data: { sarchivo: sFile, stpercodi: $('#stpercodiCM').val(), strecacodi: $('#strecacodiCM').val() },
        dataType: 'json',
        success: function (result) {
            //var ListaSistemaTrans = result.ListaSistemaTrans;
            var ListaCentralesGen = result.ListaCentralesGen;
            var bGrabar = result.Grabar;
            var columns = result.Columnas;
            var iNroColumnas = result.NumeroColumnas;
            //var headers = result.Headers;
            var widths = result.Widths;
            var data = result.Data;
            var sRegExVal = new RegExp(/^-{0,1}\d*\.{0,1}\d+$/); //^\d+(?:[\.\,]\d+)?$
            hot = new Handsontable(container, {
                data: data,
                maxCols: result.Columnas.length,
                colHeaders: false,
                rowHeaders: true,
                colWidths: widths,
                contextMenu: true,
                minSpareRows: 1,
                columnSorting: false,
                columns: columns,
                fixedRowsTop: result.FixedRowsTop,
                fixedColumnsLeft: result.FixedColumnsLeft,
                beforeChange: function (changes, source) {
                    for (var i = changes.length - 1; i >= 0; i--) {
                        var numFila = changes[i][0];
                        var numColumna = changes[i][1];
                        var sValorAnterior = changes[i][2];
                        var sNuevoValor = changes[i][3];
                        var lista;
                        var bEsNumero;
                        var header;
                        var sMensaje;
                        if (numColumna == 0) {
                            lista = ListaCentralesGen;
                            bEsNumero = null;
                            header = "Central Generación";
                            sMensaje = "Nombre de Central de Generación incorrecto";
                        }
                            //else if (numColumna == 1) {
                            //    lista = ListaSistemaTrans;
                            //    bEsNumero = null;
                            //    header = "Sistema Transmisión";
                            //    sMensaje = "Nombre del Sistema incorrecto";
                            //}
                        else if ((numColumna % 2) == 1) {
                            //Valida si el nuevo valor de la celda R(pu)
                            lista = null;
                            if (sValorAnterior != null) sValorAnterior = sValorAnterior.toString();
                            if (sNuevoValor != null) sNuevoValor = sNuevoValor.toString();
                            bEsNumero = sRegExVal.test(sNuevoValor);
                            header = "ELEMENTO";
                            sMensaje = "Valor del elemento en " + data[0][numColumna] + " no es válido";
                        }
                        else if ((numColumna % 2) == 0) {
                            //Valida si el nuevo valor de la celda X(pu)
                            lista = null;
                            if (sValorAnterior != null) sValorAnterior = sValorAnterior.toString();
                            if (sNuevoValor != null) sNuevoValor = sNuevoValor.toString();
                            bEsNumero = sRegExVal.test(sNuevoValor);
                            header = "ELEMENTO";
                            sMensaje = "Valor del elemento en " + data[0][numColumna] + " no es válido";
                        }
                        else {
                            lista = null;
                            bEsNumero = null;
                            header = "";
                            sMensaje = "";
                        }
                        if ((bEsNumero == true) || ($.inArray(sNuevoValor, lista) > -1)) {   //Valida si el valor anterior se encuentra en el array "error"
                            if (sValorAnterior !== null) {
                                if ($.inArray(sValorAnterior.concat("_-_" + numFila + "_-_" + header + "_-_" + sMensaje), error) > -1) {
                                    error = $.grep(error, function (value) { return value != sValorAnterior.concat("_-_" + numFila + "_-_" + header + "_-_" + sMensaje) });
                                }
                            }
                        }
                        else {
                            if (sNuevoValor === "") {
                                return false;
                            }
                            else {
                                if ($.inArray(sNuevoValor.concat("_-_" + numFila + "_-_" + header + "_-_" + sMensaje), error) > -1) {
                                }
                                else {
                                    error.push(sNuevoValor.concat("_-_" + numFila + "_-_" + header + "_-_" + sMensaje));
                                }
                            }
                        }
                    }
                },
                cells: function (row, col, prop) {
                    //console.log("col:" + col + " row:" + row + " prop" + prop);
                    var cellProperties = {};
                    if (row == 0) {
                        cellProperties.renderer = firstRowRendererCabecera;
                    }
                    else if (col == 0) {
                        cellProperties.renderer = firstRowRendererCeleste;
                    }
                    //else if (col >= 4) {
                    //    //Para el llenado de datos
                    //    cellProperties.renderer = "negativeValueRenderer";
                    //}
                    return cellProperties;
                },
            });
            hot.render();
            if (!bGrabar) {
                $('#btnValidarGrillaExcelCM').css('display', 'block');
                $('#btnGrabarExcelCM').css('display', 'block');
                $('#btnEliminarDatosCM2').css('display', 'block');
            }
            else {
                $('#btnValidarGrillaExcelCM').css('display', 'none');
                $('#btnGrabarExcelCM').css('display', 'none');
                $('#btnEliminarDatosCM2').css('display', 'none');
            }
            $('#divAccionesCM').css('display', 'block');
            var iNumRegistros = hot.countRows();
            if (iNumRegistros >= 1) iNumRegistros = iNumRegistros - 2;
            mostrarExito('Se han encontrado ' + iNumRegistros + ' registro(s)');
        },
        error: function () {
            mostrarError('Lo sentimos no se puede mostrar la hoja de cálculo')
        }
    });
}

////////////////////////////////////////////FACTOR ACTUALIZACION//////////////////////////////////////////////////////////////////////////////////////
mostrarGrillaExcelFA = function () {
    error = [];
    if (typeof hot != 'undefined') {
        hot.destroy();
    }
    var container = document.getElementById('grillaExcelFA');
    $.ajax({
        type: 'POST',
        url: controler + "grillaExcelFA",
        data: { stpercodi: $('#stpercodiFA').val(), strecacodi: $('#strecacodiFA').val() },
        dataType: 'json',
        success: function (result) {
            var ListaSistemaTransmisor = result.ListaSistemasTrans;
            var bGrabar = result.Grabar;
            var columns = result.Columnas;
            var headers = result.Headers;
            var widths = result.Widths;
            var data = result.Data;
            var sRegExVal = new RegExp(/^-{0,1}\d*\.{0,1}\d+$/); //^\d+(?:[\.\,]\d+)?$
            hot = new Handsontable(container, {
                data: data,
                maxCols: result.Columnas.length,
                colHeaders: headers,
                colWidths: widths,
                columnSorting: false,
                contextMenu: false,
                minSpareRows: 0,
                rowHeaders: true,
                columns: columns,
                fixedRowsTop: result.FixedRowsTop,
                fixedColumnsLeft: result.FixedColumnsLeft,
                beforeChange: function (changes, source) {
                    //changes[i][0] = fila
                    //changes[i][1] = columna
                    //changes[i][2] = valor anteriror
                    //changes[i][3] = nuevo valor                  
                    for (var i = changes.length - 1; i >= 0; i--) {
                        var numFila = changes[i][0];
                        var numColumna = changes[i][1];
                        var sValorAnterior = changes[i][2];
                        var sNuevoValor = changes[i][3];
                        var lista;
                        var bEsNumero;
                        var header;
                        var sMensaje;
                        if (numColumna == '0') {
                            //Valida si el nuevo valor de la celda se encuentra en listaCentrales
                            lista = ListaSistemaTransmisor;
                            bEsNumero = null;
                            header = "Sistema Transmisión";
                            sMensaje = "Nombre de Sistema de Transmisión incorrecto";
                        }
                        else if (numColumna == '1') {
                            //Valida si el nuevo valor de la celda Energia
                            lista = null;
                            if (sValorAnterior != null) sValorAnterior = sValorAnterior.toString();
                            if (sNuevoValor != null) sNuevoValor = sNuevoValor.toString();
                            bEsNumero = sRegExVal.test(sNuevoValor);
                            header = "FACTOR";
                            sMensaje = "Valor del Factor no es válido";
                        }
                        else {
                            lista = null;
                            bEsNumero = null;
                            header = "";
                            sMensaje = "";
                        }
                        if ((bEsNumero == true) || ($.inArray(sNuevoValor, lista) > -1)) {   //Valida si el valor anterior se encuentra en el array "error"
                            if (sValorAnterior !== null) {
                                if ($.inArray(sValorAnterior.concat("_-_" + numFila + "_-_" + header + "_-_" + sMensaje), error) > -1) {
                                    //Elimina el valor del array error
                                    error = $.grep(error, function (value) { return value != sValorAnterior.concat("_-_" + numFila + "_-_" + header + "_-_" + sMensaje) });
                                    //console.log(error);
                                }
                            }
                        }
                        else {   //Si el nuevo valor es vacio retorna el valor anterior
                            if (sNuevoValor === "") {
                                return false;
                            }
                            else {   //Busca si el nuevo valor 'error' se encuentra en el array 'error'
                                if ($.inArray(sNuevoValor.concat("_-_" + numFila + "_-_" + header + "_-_" + sMensaje), error) > -1) {
                                }
                                else {
                                    //Si no se encuentra se agrega
                                    error.push(sNuevoValor.concat("_-_" + numFila + "_-_" + header + "_-_" + sMensaje));
                                    //console.log(error);
                                }
                            }
                        }
                    }
                }
            });
            hot.render();
            if (!bGrabar) {
                $('#btnValidarGrillaExcelFA').css('display', 'block');
                $('#btnGrabarExcelFA').css('display', 'block');
                $('#btnEliminarDatosFA').css('display', 'block');
            }
            else {
                $('#btnValidarGrillaExcelFA').css('display', 'none');
                $('#btnGrabarExcelFA').css('display', 'none');
                $('#btnEliminarDatosFA').css('display', 'none');
            }
            $('#divAccionesFA').css('display', 'block');
            var iNumRegistros = hot.countRows();
            if (iNumRegistros >= 1) iNumRegistros = iNumRegistros - 1;
            mostrarExito('Se han encontrado ' + iNumRegistros + ' registro(s)');
        },
        error: function () {
            mostrarError('Lo sentimos no se puede mostrar la hoja de cálculo')
        }
    });
}

grabarExcelFA = function () {
    $.ajax({
        type: "POST",
        url: controler + 'grabargrillaexcelfa',
        dataType: "json",
        contentType: 'application/json',
        traditional: true,
        data: JSON.stringify({
            stpercodi: $('#stpercodiFA').val(), strecacodi: $('#strecacodiFA').val(), datos: hot.getData()
        }),
        success: function (result) {
            var sError = result.sError;
            var sMensaje = result.sMensaje;
            if (sError == "") {
                var iNumRegistros = hot.countRows();
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

eliminarDatosFA = function () {
    $.ajax({
        type: 'POST',
        url: controler + 'eliminardatosfa',
        data: { stpercodi: $('#stpercodiFA').val(), strecacodi: $('#strecacodiFA').val() },
        dataType: 'json',
        success: function (result) {
            if (result == "1") {
                alert("Felicidades, la información se elimino correctamente...!");
                mostrarGrillaExcelEN(0);
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

descargarArchivoFA = function (formato) {
    $.ajax({
        type: 'POST',
        url: controler + 'exportardatafa',
        data: { stpercodi: $('#stpercodiFA').val(), strecacodi: $('#strecacodiFA').val(), formato: formato },
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

uploadExcelFA = function () {
    uploaderFA = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: 'btnSelecionarExcelFA',
        url: controler + 'uploadexcel',
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
                if (uploaderFA.files.length == 2) {
                    uploaderFA.removeFile(uploaderFA.files[0]);
                }
                uploaderFA.start();
                up.refresh();
            },
            UploadProgress: function (up, file) {
                mostrarAlerta("Por favor espere ...(<strong>" + file.percent + "%</strong>)");
            },
            UploadComplete: function (up, file) {
                mostrarAlerta("Subida completada <strong> procesando el archivo...</strong>");
                procesarArchivoFA(file[0].name);
            },
            Error: function (up, err) {
                mostrarError(err.code + "-" + err.message);
            }
        }
    });
    uploaderFA.init();
}

procesarArchivoFA = function (sFile) {
    if (typeof hot != 'undefined') {
        hot.destroy();
    }
    var container = document.getElementById('grillaExcelFA');
    $.ajax({
        type: 'POST',
        url: controler + 'procesararchivofa',
        data: { sarchivo: sFile, stpercodi: $('#stpercodiFA').val(), strecacodi: $('#strecacodiFA').val() },
        dataType: 'json',
        cache: false,
        success: function (result) {
            var ListaSistemasTrans = result.ListaSistemasTrans;
            var iRegError = result.RegError;
            var sMensajeError = result.MensajeError;
            var bGrabar = result.Grabar;
            var columns = result.Columnas;
            var headers = result.Headers;
            var widths = result.Widths;
            var data = result.Data;
            var sRegExVal = new RegExp(/^-{0,1}\d*\.{0,1}\d+$/); //^\d+(?:[\.\,]\d+)?$
            hot = new Handsontable(container, {
                data: data,
                maxCols: result.Columnas.length,
                colHeaders: headers,
                colWidths: widths,
                columnSorting: false,
                contextMenu: false,
                minSpareRows: 0,
                rowHeaders: true,
                columns: columns,
                fixedRowsTop: result.FixedRowsTop,
                fixedColumnsLeft: result.FixedColumnsLeft,
                beforeChange: function (changes, source) {
                    //changes[i][0] = fila
                    //changes[i][1] = columna
                    //changes[i][2] = valor anteriror
                    //changes[i][3] = nuevo valor                  
                    for (var i = changes.length - 1; i >= 0; i--) {
                        var numFila = changes[i][0];
                        var numColumna = changes[i][1];
                        var sValorAnterior = changes[i][2];
                        var sNuevoValor = changes[i][3];
                        var lista;
                        var bEsNumero;
                        var header;
                        var sMensaje;
                        if (numColumna == '0') {
                            //Valida si el nuevo valor de la celda se encuentra en listaCentrales
                            lista = ListaSistemasTrans;
                            bEsNumero = null;
                            header = "Sistema Transmisión";
                            sMensaje = "Nombre de Sistema de Transmisión incorrecto";
                        }
                        else if (numColumna == '1') {
                            //Valida si el nuevo valor de la celda Energia
                            lista = null;
                            if (sValorAnterior != null) sValorAnterior = sValorAnterior.toString();
                            if (sNuevoValor != null) sNuevoValor = sNuevoValor.toString();
                            bEsNumero = sRegExVal.test(sNuevoValor);
                            header = "Factor";
                            sMensaje = "Valor del Factor no es válido";
                        }
                        else {
                            lista = null;
                            bEsNumero = null;
                            header = "";
                            sMensaje = "";
                        }
                        if ((bEsNumero == true) || ($.inArray(sNuevoValor, lista) > -1)) {   //Valida si el valor anterior se encuentra en el array "error"
                            if (sValorAnterior !== null) {
                                if ($.inArray(sValorAnterior.concat("_-_" + numFila + "_-_" + header + "_-_" + sMensaje), error) > -1) {
                                    //Elimina el valor del array error
                                    error = $.grep(error, function (value) { return value != sValorAnterior.concat("_-_" + numFila + "_-_" + header + "_-_" + sMensaje) });
                                    //console.log(error);
                                }
                            }
                        }
                        else {   //Si el nuevo valor es vacio retorna el valor anterior
                            if (sNuevoValor === "") {
                                return false;
                            }
                            else {   //Busca si el nuevo valor 'error' se encuentra en el array 'error'
                                if ($.inArray(sNuevoValor.concat("_-_" + numFila + "_-_" + header + "_-_" + sMensaje), error) > -1) {
                                }
                                else {
                                    //Si no se encuentra se agrega
                                    error.push(sNuevoValor.concat("_-_" + numFila + "_-_" + header + "_-_" + sMensaje));
                                    //console.log(error);
                                }
                            }
                        }
                    }
                }
            });
            hot.render();
            if (!bGrabar) {
                $('#btnValidarGrillaExcelFA').css('display', 'block');
                $('#btnGrabarExcelFA').css('display', 'block');
                $('#btnEliminarDatosFA').css('display', 'block');
            }
            else {
                $('#btnValidarGrillaExcelFA').css('display', 'none');
                $('#btnGrabarExcelFA').css('display', 'none');
                $('#btnEliminarDatosFA').css('display', 'none');
            }
            $('#divAccionesFA').css('display', 'block');
            var iNumRegistros = hot.countRows();
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

recargarFA = function () {
    var cmbPericodi = document.getElementById('stpercodiFA');
    window.location.href = controler + "index?stpercodi=" + cmbPericodi.value + "#paso3";
}

abrirPopupFA = function () {
    var html = '<span class="button b-close"><span>X</span></span>';
    html += '<p><b>Corregir los siguientes errores</b><p>';
    html += '<table border="0" class="pretty tabla-icono" id="tabla">'
    html += '<thead>'
    html += '<tr>'
    html += '<th>Fila</th>'
    html += '<th>Columna</th>'
    html += '<th>Valor</th>'
    html += '<th>Comentario</th>'
    html += '</tr>'
    html += '</thead>'
    html += '<tbody>'
    for (var i = error.length - 1; i >= 0 ; i--) {
        var sStyle = "background : #ffffff;";
        if (i % 2)
            var sStyle = "background : #fbf4bf;";
        var SplitError = error[i].split("_-_");
        html += '<tr id="Fila_' + i + '">'
        html += '<td style="text-align:right;' + sStyle + '">' + (parseInt(SplitError[1]) + 1) + '</td>'
        html += '<td style="text-align:left;' + sStyle + '">' + SplitError[2] + '</td>'
        html += '<td style="text-align:left;' + sStyle + '">' + SplitError[0] + '</td>'
        html += '<td style="text-align:left;' + sStyle + '">' + SplitError[3] + '</td>'
        html += '</tr>'
    }
    html += '</tbody>'
    html += '</table>'

    $('#popup').html(html);
    $('#popup').bPopup({
        easing: 'easeOutBack',
        speed: 450,
        transition: 'slideDown'
    });
    mostrarError("Lo sentimos, la hoja del cálculo tiene errores");
}

/////////////////////////////////////////////DISTANCIAS ELECTRICAS/////////////////////////////////////////////////////////////////////////////////////

mostrarGrillaExcelDE = function () {
    error = [];
    if (typeof hot != 'undefined') {
        hot.destroy();
    }
    var container = document.getElementById('grillaExcelDE');
    $.ajax({
        type: 'POST',
        url: controler + "grillaexcelde",
        data: { stpercodi: $('#stpercodiDE').val(), strecacodi: $('#strecacodiDE').val() },
        dataType: 'json',
        success: function (result) {
            var ListaBarras = result.ListaBarras;
            var bGrabar = result.Grabar;
            var columns = result.Columnas;
            var iNroColumnas = result.NumeroColumnas;
            var widths = result.Widths;
            var data = result.Data;
            var sRegExVal = new RegExp(/^-{0,1}\d*\.{0,1}\d+$/); //^\d+(?:[\.\,]\d+)?$
            hot = new Handsontable(container, {
                data: data,
                maxCols: result.Columnas.length,
                colHeaders: false,
                rowHeaders: true,
                colWidths: widths,
                contextMenu: true,
                minSpareRows: 1,
                rowHeaders: true,
                columns: columns,
                fixedRowsTop: result.FixedRowsTop,
                fixedColumnsLeft: result.FixedColumnsLeft,
                currentRowClassName: 'currentRow',
                mergeCells: [{ row: 0, col: 0, rowspan: 2, colspan: 1 }],
                beforeChange: function (changes, source) {
                    //changes[i][0] = fila
                    //changes[i][1] = columna
                    //changes[i][2] = valor anteriror
                    //changes[i][3] = nuevo valor                  
                    for (var i = changes.length - 1; i >= 0; i--) {
                        var numFila = changes[i][0];
                        var numColumna = changes[i][1];
                        var sValorAnterior = changes[i][2];
                        var sNuevoValor = changes[i][3];
                        var lista;
                        var bEsNumero;
                        var header;
                        var sMensaje;
                        if (numColumna == 0) {
                            //Valida si el nuevo valor de la celda se encuentra en ListaSistemasTransmision
                            lista = ListaBarras;
                            bEsNumero = null;
                            header = "Barras";
                            sMensaje = "Nombre de la Barra incorrecto";
                        }
                        else if ((numColumna % 2) == 1) {
                            //Valida si el nuevo valor de la celda R(pu)
                            lista = null;
                            if (sValorAnterior != null) sValorAnterior = sValorAnterior.toString();
                            if (sNuevoValor != null) sNuevoValor = sNuevoValor.toString();
                            bEsNumero = sRegExVal.test(sNuevoValor);
                            header = "R(pu)";
                            sMensaje = "Valor de la R(pu) en " + data[0][numColumna] + " no es válido";
                        }
                        else if ((numColumna % 2) == 0) {
                            //Valida si el nuevo valor de la celda X(pu)
                            lista = null;
                            if (sValorAnterior != null) sValorAnterior = sValorAnterior.toString();
                            if (sNuevoValor != null) sNuevoValor = sNuevoValor.toString();
                            bEsNumero = sRegExVal.test(sNuevoValor);
                            header = "X(pu)";
                            sMensaje = "Valor de la X(pu) en " + data[0][numColumna - 1] + " no es válido";
                        }
                        else {
                            lista = null;
                            bEsNumero = null;
                            header = "";
                            sMensaje = "";
                        }
                        if ((bEsNumero == true) || ($.inArray(sNuevoValor, lista) > -1)) {   //Valida si el valor anterior se encuentra en el array "error"
                            if (sValorAnterior !== null) {
                                if ($.inArray(sValorAnterior.concat("_-_" + numFila + "_-_" + header + "_-_" + sMensaje), error) > -1) {
                                    //Elimina el valor del array error
                                    error = $.grep(error, function (value) { return value != sValorAnterior.concat("_-_" + numFila + "_-_" + header + "_-_" + sMensaje) });
                                    //console.log(error);
                                }
                            }
                        }
                        else {   //Si el nuevo valor es vacio retorna el valor anterior
                            if (sNuevoValor === "") {
                                return false;
                            }
                            else {   //Busca si el nuevo valor 'error' se encuentra en el array 'error'
                                if ($.inArray(sNuevoValor.concat("_-_" + numFila + "_-_" + header + "_-_" + sMensaje), error) > -1) {
                                }
                                else {
                                    //Si no se encuentra se agrega
                                    error.push(sNuevoValor.concat("_-_" + numFila + "_-_" + header + "_-_" + sMensaje));
                                    //console.log(error);
                                }
                            }
                        }
                    }
                },
                cells: function (row, col, prop) {
                    //console.log("col:" + col + " row:" + row + " prop" + prop);
                    var cellProperties = {};
                    if (row == 0 || row == 1) {
                        cellProperties.renderer = firstRowRendererCabecera;
                    }
                    else if (col == 0) {
                        cellProperties.renderer = firstRowRendererCeleste;
                    }
                    //else if (col >= 4) {
                    //    //Para el llenado de datos
                    //    cellProperties.renderer = "negativeValueRenderer";
                    //}
                    return cellProperties;
                },
            });
            hot.render();
            if (!bGrabar) {
                $('#btnValidarGrillaExcelDE').css('display', 'block');
                $('#btnGrabarExcelDE').css('display', 'block');
                $('#btnEliminarDatosDE').css('display', 'block');
            }
            else {
                $('#btnValidarGrillaExcelDE').css('display', 'none');
                $('#btnGrabarExcelDE').css('display', 'none');
                $('#btnEliminarDatosDE').css('display', 'none');
            }
            $('#divAccionesDE').css('display', 'block');
            var iNumRegistros = hot.countRows();
            if (iNumRegistros >= 2) iNumRegistros = iNumRegistros - 3;
            mostrarExito('Se han encontrado ' + iNumRegistros + ' registro(s)');
        },
        error: function () {
            mostrarError('Lo sentimos no se puede mostrar la hoja de cálculo')
        }
    });
}

descargarArchivoDE = function (formato) {
    $.ajax({
        type: 'POST',
        url: controler + 'exportardatade',
        data: { stpercodi: $('#stpercodiDE').val(), strecacodi: $('#strecacodiDE').val(), formato: formato },
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

eliminarDatosDE = function () {
    $.ajax({
        type: 'POST',
        url: controler + 'eliminardatosde',
        data: { stpercodi: $('#stpercodiDE').val(), strecacodi: $('#strecacodiDE').val() },
        dataType: 'json',
        success: function (result) {
            if (result == "1") {
                alert("Felicidades, la información se elimino correctamente...!");
                mostrarGrillaExcelDE(0);
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

grabarExcelDE = function () {
    $.ajax({
        type: "POST",
        url: controler + 'grabargrillaexcelde',
        dataType: "json",
        contentType: 'application/json',
        traditional: true,
        data: JSON.stringify({
            stpercodi: $('#stpercodiDE').val(), strecacodi: $('#strecacodiDE').val(), datos: hot.getData()
        }),
        success: function (result) {
            var sError = result.sError;
            var sMensaje = result.sMensaje;
            if (sError == "") {
                var iNumRegistros = hot.countRows();
                if (iNumRegistros >= 2) iNumRegistros = iNumRegistros - 3;
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

uploadExcelDE = function () {
    uploaderDE = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: 'btnSelecionarExcelDE',
        url: controler + 'uploadexcel',
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
                if (uploaderDE.files.length == 2) {
                    uploaderDE.removeFile(uploaderDE.files[0]);
                }
                uploaderDE.start();
                up.refresh();
            },
            UploadProgress: function (up, file) {
                mostrarAlerta("Por favor espere ...(<strong>" + file.percent + "%</strong>)");
            },
            UploadComplete: function (up, file) {
                mostrarAlerta("Subida completada <strong> procesando el archivo...</strong>");
                procesarArchivoDE(file[0].name);
            },
            Error: function (up, err) {
                mostrarError(err.code + "-" + err.message);
            }
        }
    });
    uploaderDE.init();
}

procesarArchivoDE = function (sFile) {
    if (typeof hot != 'undefined') {
        hot.destroy();
    }
    var container = document.getElementById('grillaExcelDE');
    $.ajax({
        type: 'POST',
        url: controler + 'procesararchivode',
        data: { sarchivo: sFile, stpercodi: $('#stpercodiDE').val(), strecacodi: $('#strecacodiDE').val() },
        dataType: 'json',
        cache: false,
        success: function (result) {
            var ListaBarras = result.ListaBarras;
            var iRegError = result.RegError;
            var sMensajeError = result.MensajeError;
            var bGrabar = result.Grabar;
            var columns = result.Columnas;
            var iNroColumnas = result.NumeroColumnas;
            var widths = result.Widths;
            var data = result.Data;
            var sRegExVal = new RegExp(/^-{0,1}\d*\.{0,1}\d+$/); //^\d+(?:[\.\,]\d+)?$
            hot = new Handsontable(container, {
                data: data,
                maxCols: result.Columnas.length,
                colHeaders: false,
                rowHeaders: true,
                colWidths: widths,
                contextMenu: true,
                minSpareRows: 1,
                rowHeaders: true,
                columns: columns,
                fixedRowsTop: result.FixedRowsTop,
                fixedColumnsLeft: result.FixedColumnsLeft,
                currentRowClassName: 'currentRow',
                mergeCells: [{ row: 0, col: 0, rowspan: 2, colspan: 1 }],
                beforeChange: function (changes, source) {
                    //changes[i][0] = fila
                    //changes[i][1] = columna
                    //changes[i][2] = valor anteriror
                    //changes[i][3] = nuevo valor                  
                    for (var i = changes.length - 1; i >= 0; i--) {
                        var numFila = changes[i][0];
                        var numColumna = changes[i][1];
                        var sValorAnterior = changes[i][2];
                        var sNuevoValor = changes[i][3];
                        var lista;
                        var bEsNumero;
                        var header;
                        var sMensaje;
                        if (numColumna == 0) {
                            //Valida si el nuevo valor de la celda se encuentra en ListaSistemasTransmision
                            lista = ListaBarras;
                            bEsNumero = null;
                            header = "Barras";
                            sMensaje = "Nombre de la Barra incorrecto";
                        }
                        else if ((numColumna % 2) == 1) {
                            //Valida si el nuevo valor de la celda R(pu)
                            lista = null;
                            if (sValorAnterior != null) sValorAnterior = sValorAnterior.toString();
                            if (sNuevoValor != null) sNuevoValor = sNuevoValor.toString();
                            bEsNumero = sRegExVal.test(sNuevoValor);
                            header = "R(pu)";
                            sMensaje = "Valor de la R(pu) en " + data[0][numColumna] + " no es válido";
                        }
                        else if ((numColumna % 2) == 0) {
                            //Valida si el nuevo valor de la celda X(pu)
                            lista = null;
                            if (sValorAnterior != null) sValorAnterior = sValorAnterior.toString();
                            if (sNuevoValor != null) sNuevoValor = sNuevoValor.toString();
                            bEsNumero = sRegExVal.test(sNuevoValor);
                            header = "X(pu)";
                            sMensaje = "Valor de la X(pu) en " + data[0][numColumna - 1] + " no es válido";
                        }
                        else {
                            lista = null;
                            bEsNumero = null;
                            header = "";
                            sMensaje = "";
                        }
                        if ((bEsNumero == true) || ($.inArray(sNuevoValor, lista) > -1)) {   //Valida si el valor anterior se encuentra en el array "error"
                            if (sValorAnterior !== null) {
                                if ($.inArray(sValorAnterior.concat("_-_" + numFila + "_-_" + header + "_-_" + sMensaje), error) > -1) {
                                    //Elimina el valor del array error
                                    error = $.grep(error, function (value) { return value != sValorAnterior.concat("_-_" + numFila + "_-_" + header + "_-_" + sMensaje) });
                                    //console.log(error);
                                }
                            }
                        }
                        else {   //Si el nuevo valor es vacio retorna el valor anterior
                            if (sNuevoValor === "") {
                                return false;
                            }
                            else {   //Busca si el nuevo valor 'error' se encuentra en el array 'error'
                                if ($.inArray(sNuevoValor.concat("_-_" + numFila + "_-_" + header + "_-_" + sMensaje), error) > -1) {
                                }
                                else {
                                    //Si no se encuentra se agrega
                                    error.push(sNuevoValor.concat("_-_" + numFila + "_-_" + header + "_-_" + sMensaje));
                                    //console.log(error);
                                }
                            }
                        }
                    }
                },
                cells: function (row, col, prop) {
                    //console.log("col:" + col + " row:" + row + " prop" + prop);
                    var cellProperties = {};
                    if (row == 0 || row == 1) {
                        cellProperties.renderer = firstRowRendererCabecera;
                    }
                    else if (col == 0) {
                        cellProperties.renderer = firstRowRendererCeleste;
                    }
                    //else if (col >= 4) {
                    //    //Para el llenado de datos
                    //    cellProperties.renderer = "negativeValueRenderer";
                    //}
                    return cellProperties;
                },
            });
            hot.render();
            if (!bGrabar) {
                $('#btnValidarGrillaExcelDE').css('display', 'block');
                $('#btnGrabarExcelDE').css('display', 'block');
                $('#btnEliminarDatosDE').css('display', 'block');
            }
            else {
                $('#btnValidarGrillaExcelDE').css('display', 'none');
                $('#btnGrabarExcelDE').css('display', 'none');
                $('#btnEliminarDatosDE').css('display', 'none');
            }
            $('#divAccionesDE').css('display', 'block');
            var iNumRegistros = hot.countRows();
            if (iNumRegistros >= 2) iNumRegistros = iNumRegistros - 3;
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

recargarDE = function () {
    var cmbPericodi = document.getElementById('stpercodiDE');
    window.location.href = controler + "index?stpercodi=" + cmbPericodi.value;
}

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
mostrarGrillaExcelEN = function () {
    error = [];
    if (typeof hot != 'undefined') {
        hot.destroy();
    }
    var container = document.getElementById('grillaExcelEN');
    $.ajax({
        type: 'POST',
        url: controler + "grillaexcelen",
        data: { stpercodi: $('#stpercodiEN').val(), strecacodi: $('#strecacodiEN').val() },
        dataType: 'json',
        success: function (result) {
            var listaCentralGeneracion = result.ListaCentralGeneracion;
            var bGrabar = result.Grabar;
            var columns = result.Columnas;
            var headers = result.Headers;
            var widths = result.Widths;
            var data = result.Data;
            var sRegExVal = new RegExp(/^-{0,1}\d*\.{0,1}\d+$/); //^\d+(?:[\.\,]\d+)?$
            hot = new Handsontable(container, {
                data: data,
                maxCols: result.Columnas.length,
                colHeaders: headers,
                colWidths: widths,
                columnSorting: false,
                contextMenu: true,
                minSpareRows: 1,
                rowHeaders: true,
                columns: columns,
                fixedRowsTop: result.FixedRowsTop,
                fixedColumnsLeft: result.FixedColumnsLeft,
                beforeChange: function (changes, source) {
                    //changes[i][0] = fila
                    //changes[i][1] = columna
                    //changes[i][2] = valor anteriror
                    //changes[i][3] = nuevo valor                  
                    for (var i = changes.length - 1; i >= 0; i--) {
                        var numFila = changes[i][0];
                        var numColumna = changes[i][1];
                        var sValorAnterior = changes[i][2];
                        var sNuevoValor = changes[i][3];
                        var lista;
                        var bEsNumero;
                        var header;
                        var sMensaje;
                        if (numColumna == '0') {
                            //Valida si el nuevo valor de la celda se encuentra en listaCentrales
                            lista = listaCentralGeneracion;
                            bEsNumero = null;
                            header = "Central Generación";
                            sMensaje = "Nombre de Central de Generación incorrecto";
                        }
                        else if (numColumna == '1') {
                            //Valida si el nuevo valor de la celda Energia
                            lista = null;
                            if (sValorAnterior != null) sValorAnterior = sValorAnterior.toString();
                            if (sNuevoValor != null) sNuevoValor = sNuevoValor.toString();
                            bEsNumero = sRegExVal.test(sNuevoValor);
                            header = "Energia";
                            sMensaje = "Valor de la Energia no es válido";
                        }
                        else {
                            lista = null;
                            bEsNumero = null;
                            header = "";
                            sMensaje = "";
                        }
                        if ((bEsNumero == true) || ($.inArray(sNuevoValor, lista) > -1)) {   //Valida si el valor anterior se encuentra en el array "error"
                            if (sValorAnterior !== null) {
                                if ($.inArray(sValorAnterior.concat("_-_" + numFila + "_-_" + header + "_-_" + sMensaje), error) > -1) {
                                    //Elimina el valor del array error
                                    error = $.grep(error, function (value) { return value != sValorAnterior.concat("_-_" + numFila + "_-_" + header + "_-_" + sMensaje) });
                                    //console.log(error);
                                }
                            }
                        }
                        else {   //Si el nuevo valor es vacio retorna el valor anterior
                            if (sNuevoValor === "") {
                                return false;
                            }
                            else {   //Busca si el nuevo valor 'error' se encuentra en el array 'error'
                                if ($.inArray(sNuevoValor.concat("_-_" + numFila + "_-_" + header + "_-_" + sMensaje), error) > -1) {
                                }
                                else {
                                    //Si no se encuentra se agrega
                                    error.push(sNuevoValor.concat("_-_" + numFila + "_-_" + header + "_-_" + sMensaje));
                                    //console.log(error);
                                }
                            }
                        }
                    }
                }
            });
            hot.render();
            if (!bGrabar) {
                $('#btnValidarGrillaExcelEN').css('display', 'block');
                $('#btnGrabarExcelEN').css('display', 'block');
                $('#btnEliminarDatosEN').css('display', 'block');
            }
            else {
                $('#btnValidarGrillaExcelEN').css('display', 'none');
                $('#btnGrabarExcelEN').css('display', 'none');
                $('#btnEliminarDatosEN').css('display', 'none');
            }
            $('#divAccionesEN').css('display', 'block');
            var iNumRegistros = hot.countRows();
            if (iNumRegistros >= 1) iNumRegistros = iNumRegistros - 1;
            mostrarExito('Se han encontrado ' + iNumRegistros + ' registro(s)');
        },
        error: function () {
            mostrarError('Lo sentimos no se puede mostrar la hoja de cálculo')
        }
    });
}

grabarExcelEN = function () {
    $.ajax({
        type: "POST",
        url: controler + 'grabargrillaexcelen',
        dataType: "json",
        contentType: 'application/json',
        traditional: true,
        data: JSON.stringify({
            stpercodi: $('#stpercodiEN').val(), strecacodi: $('#strecacodiEN').val(), datos: hot.getData()
        }),
        success: function (result) {
            var sError = result.sError;
            var sMensaje = result.sMensaje;
            if (sError == "") {
                var iNumRegistros = hot.countRows();
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

eliminarDatosEN = function () {
    $.ajax({
        type: 'POST',
        url: controler + 'eliminardatosen',
        data: { stpercodi: $('#stpercodiEN').val(), strecacodi: $('#strecacodiEN').val() },
        dataType: 'json',
        success: function (result) {
            if (result == "1") {
                alert("Felicidades, la información se elimino correctamente...!");
                mostrarGrillaExcelEN(0);
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

descargarArchivoEN = function (formato) {
    $.ajax({
        type: 'POST',
        url: controler + 'exportardataen',
        data: { stpercodi: $('#stpercodiEN').val(), strecacodi: $('#strecacodiEN').val(), formato: formato },
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

uploadExcelEN = function () {
    uploaderEN = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: 'btnSelecionarExcelEN',
        url: controler + 'uploadexcel',
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
                if (uploaderEN.files.length == 2) {
                    uploaderEN.removeFile(uploaderEN.files[0]);
                }
                uploaderEN.start();
                up.refresh();
            },
            UploadProgress: function (up, file) {
                mostrarAlerta("Por favor espere ...(<strong>" + file.percent + "%</strong>)");
            },
            UploadComplete: function (up, file) {
                mostrarAlerta("Subida completada <strong> procesando el archivo...</strong>");
                procesarArchivoEN(file[0].name);
            },
            Error: function (up, err) {
                mostrarError(err.code + "-" + err.message);
            }
        }
    });
    uploaderEN.init();
}

procesarArchivoEN = function (sFile) {
    if (typeof hot != 'undefined') {
        hot.destroy();
    }
    var container = document.getElementById('grillaExcelEN');
    $.ajax({
        type: 'POST',
        url: controler + 'procesararchivoen',
        data: { sarchivo: sFile, stpercodi: $('#stpercodiEN').val(), strecacodi: $('#strecacodiEN').val() },
        dataType: 'json',
        cache: false,
        success: function (result) {
            var listaCentralGeneracion = result.ListaCentralGeneracion;
            var iRegError = result.RegError;
            var sMensajeError = result.MensajeError;
            var bGrabar = result.Grabar;
            var columns = result.Columnas;
            var headers = result.Headers;
            var widths = result.Widths;
            var data = result.Data;
            var sRegExVal = new RegExp(/^-{0,1}\d*\.{0,1}\d+$/); //^\d+(?:[\.\,]\d+)?$
            hot = new Handsontable(container, {
                data: data,
                maxCols: result.Columnas.length,
                colHeaders: headers,
                colWidths: widths,
                columnSorting: false,
                contextMenu: true,
                minSpareRows: 1,
                rowHeaders: true,
                columns: columns,
                fixedRowsTop: result.FixedRowsTop,
                fixedColumnsLeft: result.FixedColumnsLeft,
                beforeChange: function (changes, source) {
                    //changes[i][0] = fila
                    //changes[i][1] = columna
                    //changes[i][2] = valor anteriror
                    //changes[i][3] = nuevo valor                  
                    for (var i = changes.length - 1; i >= 0; i--) {
                        var numFila = changes[i][0];
                        var numColumna = changes[i][1];
                        var sValorAnterior = changes[i][2];
                        var sNuevoValor = changes[i][3];
                        var lista;
                        var bEsNumero;
                        var header;
                        var sMensaje;
                        if (numColumna == '0') {
                            //Valida si el nuevo valor de la celda se encuentra en listaCentrales
                            lista = listaCentralGeneracion;
                            bEsNumero = null;
                            header = "Central Generación";
                            sMensaje = "Nombre de Central de Generación incorrecto";
                        }
                        else if (numColumna == '1') {
                            //Valida si el nuevo valor de la celda Energia
                            lista = null;
                            if (sValorAnterior != null) sValorAnterior = sValorAnterior.toString();
                            if (sNuevoValor != null) sNuevoValor = sNuevoValor.toString();
                            bEsNumero = sRegExVal.test(sNuevoValor);
                            header = "Energia";
                            sMensaje = "Valor de la Energia no es válido";
                        }
                        else {
                            lista = null;
                            bEsNumero = null;
                            header = "";
                            sMensaje = "";
                        }
                        if ((bEsNumero == true) || ($.inArray(sNuevoValor, lista) > -1)) {   //Valida si el valor anterior se encuentra en el array "error"
                            if (sValorAnterior !== null) {
                                if ($.inArray(sValorAnterior.concat("_-_" + numFila + "_-_" + header + "_-_" + sMensaje), error) > -1) {
                                    //Elimina el valor del array error
                                    error = $.grep(error, function (value) { return value != sValorAnterior.concat("_-_" + numFila + "_-_" + header + "_-_" + sMensaje) });
                                    //console.log(error);
                                }
                            }
                        }
                        else {   //Si el nuevo valor es vacio retorna el valor anterior
                            if (sNuevoValor === "") {
                                return false;
                            }
                            else {   //Busca si el nuevo valor 'error' se encuentra en el array 'error'
                                if ($.inArray(sNuevoValor.concat("_-_" + numFila + "_-_" + header + "_-_" + sMensaje), error) > -1) {
                                }
                                else {
                                    //Si no se encuentra se agrega
                                    error.push(sNuevoValor.concat("_-_" + numFila + "_-_" + header + "_-_" + sMensaje));
                                    //console.log(error);
                                }
                            }
                        }
                    }
                }
            });
            hot.render();
            if (!bGrabar) {
                $('#btnValidarGrillaExcelEN').css('display', 'block');
                $('#btnGrabarExcelEN').css('display', 'block');
                $('#btnEliminarDatosEN').css('display', 'block');
            }
            else {
                $('#btnValidarGrillaExcelEN').css('display', 'none');
                $('#btnGrabarExcelEN').css('display', 'none');
                $('#btnEliminarDatosEN').css('display', 'none');
            }
            $('#divAccionesEN').css('display', 'block');
            var iNumRegistros = hot.countRows();
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

recargarEN = function () {
    var cmbPericodi = document.getElementById('stpercodiEN');
    window.location.href = controler + "index?stpercodi=" + cmbPericodi.value + "#paso2";
}

abrirPopupEN = function () {
    var html = '<span class="button b-close"><span>X</span></span>';
    html += '<p><b>Corregir los siguientes errores</b><p>';
    html += '<table border="0" class="pretty tabla-icono" id="tabla">'
    html += '<thead>'
    html += '<tr>'
    html += '<th>Fila</th>'
    html += '<th>Columna</th>'
    html += '<th>Valor</th>'
    html += '<th>Comentario</th>'
    html += '</tr>'
    html += '</thead>'
    html += '<tbody>'
    for (var i = error.length - 1; i >= 0 ; i--) {
        var sStyle = "background : #ffffff;";
        if (i % 2)
            var sStyle = "background : #fbf4bf;";
        var SplitError = error[i].split("_-_");
        html += '<tr id="Fila_' + i + '">'
        html += '<td style="text-align:right;' + sStyle + '">' + (parseInt(SplitError[1]) + 1) + '</td>'
        html += '<td style="text-align:left;' + sStyle + '">' + SplitError[2] + '</td>'
        html += '<td style="text-align:left;' + sStyle + '">' + SplitError[0] + '</td>'
        html += '<td style="text-align:left;' + sStyle + '">' + SplitError[3] + '</td>'
        html += '</tr>'
    }
    html += '</tbody>'
    html += '</table>'

    $('#popup').html(html);
    $('#popup').bPopup({
        easing: 'easeOutBack',
        speed: 450,
        transition: 'slideDown'
    });
    mostrarError("Lo sentimos, la hoja del cálculo tiene errores");
}

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

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

negativeValueRenderer = function (instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);
    // if row contains negative number
    if (parseInt(value, 10) < 0) {
        // add class "negative"
        td.style.color = '#FA5858';
        td.style.fontWeight = 'bold';
    }
    if (!value || value === '') {
        td.style.background = '#E6E6E6';
    }
    else {
        td.style.background = '#FFFFFF';
    }
}

getCustomRenderer = function (instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);
    td.style.color = '#2980B9';
    td.style.background = '#2980B9';
    //console.log(value);
}