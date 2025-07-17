var controler = siteRoot + 'transfpotencia/peajeingreso/';
var error = [];

$(function () {
    $('#btnDescargarExcel').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        descargarArchivo(1);
    });

    $('#btnGrabarExcel').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        if (parseInt(error.length) > 0) {
            abrirPopup();
        } else {
            grabarExcel();
        }
    });

    $('#btnValidarGrillaExcel').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        if (parseInt(error.length) > 0) {
            abrirPopup();
        } else {
            mostrarExito("La hoja de cálculo no tiene errores...!");
        }
    });

    mostrarAlerta("Espere un momento, se esta procesando su solicitud");
    mostrarGrillaExcel();
    uploadExcel();
});

mostrarGrillaExcel = function () {
    var container = document.getElementById('grillaExcel');
    $.ajax({
        type: 'POST',
        url: controler + "grillaexcel",
        data: { pericodi: $('#EntidadRecalculoPotencia_Pericodi').val(), recpotcodi: $('#EntidadRecalculoPotencia_Recpotcodi').val() },
        dataType: 'json',
        success: function (result) {
            var sRegExVal = new RegExp(/^\d+(?:[\.\,]\d+)?$/); //Validacion solo numero con decimales sin comillas
            hot = new Handsontable(container, {
                data: result.Data,
                maxRows: result.Data.length,
                maxCols: result.Columnas.length,
                colHeaders: false,
                rowHeaders: true,
                colHeaders: result.Headers,
                colWidths: result.Widths,
                contextMenu: false,
                columns: result.Columnas,
                fixedRowsTop: result.FixedRowsTop,
                fixedColumnsLeft: result.FixedColumnsLeft,
                currentRowClassName: 'currentRow',
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

                        if (numColumna == '4') {
                            //Valida si el nuevo valor de la celda se encuentra en Peaje Mensual
                            lista = null;
                            if (sValorAnterior != null) sValorAnterior = sValorAnterior.toString();
                            if (sNuevoValor != null) sNuevoValor = sNuevoValor.toString();
                            bEsNumero = sRegExVal.test(sNuevoValor);
                            header = "Peaje Mensual";
                            sMensaje = "Valor del Peaje Mensual no es válido";
                        }
                        else if (numColumna == '5') {
                            //Valida si el nuevo valor de la celda se encuentra en Ingreso Tarifario Mensual
                            lista = null;
                            if (sValorAnterior != null) sValorAnterior = sValorAnterior.toString();
                            if (sNuevoValor != null) sNuevoValor = sNuevoValor.toString();
                            bEsNumero = sRegExVal.test(sNuevoValor);
                            header = "Ingreso Tarifario Mensual";
                            sMensaje = "Valor del Ingreso Tarifario Mensual no es válido";
                        }
                        else if (numColumna == '6') {
                            //Valida si el nuevo valor de la celda se encuentra en Regulado
                            lista = null;
                            if (sValorAnterior != null) sValorAnterior = sValorAnterior.toString();
                            if (sNuevoValor != null) sNuevoValor = sNuevoValor.toString();
                            bEsNumero = sRegExVal.test(sNuevoValor);
                            console.log(sValorAnterior);
                            console.log(sNuevoValor);
                            console.log(bEsNumero);
                            header = "Regulado";
                            sMensaje = "Valor de Regulado no es válido";
                        }
                        else if (numColumna == '7') {
                            //Valida si el nuevo valor de la celda se encuentra en Libre
                            lista = null;
                            if (sValorAnterior != null) sValorAnterior = sValorAnterior.toString();
                            if (sNuevoValor != null) sNuevoValor = sNuevoValor.toString();
                            bEsNumero = sRegExVal.test(sNuevoValor);
                            header = "Libre";
                            sMensaje = "Valor del Peaje unitario no es válido";
                        }
                        else if (numColumna == '8') {
                            //Valida si el nuevo valor de la celda se encuentra en Gran Usuario
                            lista = null;
                            if (sValorAnterior != null) sValorAnterior = sValorAnterior.toString();
                            if (sNuevoValor != null) sNuevoValor = sNuevoValor.toString();
                            bEsNumero = sRegExVal.test(sNuevoValor);
                            header = "Gran Usuario";
                            sMensaje = "Valor de Gran Usuario no es válido";
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
                    if (col === 0) {
                        //Oculta la primera columna: Pingcodi
                        cellProperties.renderer = getCustomRenderer;
                    }
                    if (col > 0 && col < 4) {
                        //Las columnas fijas
                        cellProperties.renderer = firstRowRenderer;
                    }
                    //else if (col >= 4) {
                    //    //Para el llenado de datos
                    //    cellProperties.renderer = negativeValueRenderer; 
                    //}
                    return cellProperties;
                }
            });
            hot.render();
            mostrarMensaje("Por favor verifique los datos");
        },
        error: function () {
            mostrarError('Lo sentimos no se puede mostrar la hoja de cálculo')
        }
    });
}

descargarArchivo = function (formato) {
    $.ajax({
        type: 'POST',
        url: controler + 'exportardata',
        data: { pericodi: $('#EntidadRecalculoPotencia_Pericodi').val(), recpotcodi: $('#EntidadRecalculoPotencia_Recpotcodi').val(), formato: formato },
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

grabarExcel = function () {
    $.ajax({
        type: "POST",
        url: controler + 'grabargrillaexcel',
        dataType: "json",
        contentType: 'application/json',
        traditional: true,
        data: JSON.stringify({ pericodi: $('#EntidadRecalculoPotencia_Pericodi').val(), recpotcodi: $('#EntidadRecalculoPotencia_Recpotcodi').val(), datos: hot.getData() }),
        success: function (result) {
            if (result == "1") {
                mostrarExito('Operación Exitosa');
            }
            else {
                mostrarError('Ha ocurrido un error: ' + result);
            }
        },
        error: function (response) {
            mostrarError(response.status + " " + response.statusText);
        }
    });
}

firstRowRenderer = function (instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);
    td.style.color = 'black';
    td.style.background = '#CEC';
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

uploadExcel = function () {
    uploader = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: 'btnSelecionarExcel',
        url: controler + 'uploadExcel',
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
                if (uploader.files.length == 2) {
                    uploader.removeFile(uploader.files[0]);
                }
                uploader.start();
                up.refresh();
            },
            UploadProgress: function (up, file) {
                mostrarAlerta("Por favor espere ...(<strong>" + file.percent + "%</strong>)");
            },
            UploadComplete: function (up, file) {
                mostrarAlerta("Subida completada <strong>Por favor espere ...</strong>");
                procesarArchivo(file[0].name);
            },
            Error: function (up, err) {
                mostrarError(err.code + "-" + err.message);
            }
        }
    });
    uploader.init();
}

procesarArchivo = function (sFile) {
    hot.destroy();
    var container = document.getElementById('grillaExcel');
    var retorno = 0;
    $.ajax({
        type: 'POST',
        url: controler + 'procesararchivo',
        data: { sarchivo: sFile, pericodi: $('#EntidadRecalculoPotencia_Pericodi').val(), recpotcodi: $('#EntidadRecalculoPotencia_Recpotcodi').val() },
        dataType: 'json',
        cache: false,
        success: function (result) {
            var iRegError = result.RegError;
            var sMensajeError = result.MensajeError;
            var sRegExVal = new RegExp(/^\d+(?:[\.\,]\d+)?$/); //Validacion solo numero con decimales sin comillas

            try {
                hot = new Handsontable(container, {
                    data: result.Data,
                    maxRows: result.Data.length,
                    maxCols: result.Data[0].length,
                    colHeaders: false,
                    rowHeaders: true,
                    colHeaders: result.Headers,
                    colWidths: result.Widths,
                    columnSorting: false,
                    contextMenu: false,
                    columns: result.Columnas,
                    fixedRowsTop: result.fixedRowsTop,
                    fixedColumnsLeft: result.FixedColumnsLeft,
                    currentRowClassName: 'currentRow',
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

                            if (numColumna == '4') {
                                //Valida si el nuevo valor de la celda se encuentra en Peaje Mensual
                                lista = null;
                                if (sValorAnterior != null) sValorAnterior = sValorAnterior.toString();
                                if (sNuevoValor != null) sNuevoValor = sNuevoValor.toString();
                                bEsNumero = sRegExVal.test(sNuevoValor);
                                header = "Peaje Mensual";
                                sMensaje = "Valor del Peaje Mensual no es válido";
                            }
                            else if (numColumna == '5') {
                                //Valida si el nuevo valor de la celda se encuentra en Ingreso Tarifario Mensual
                                lista = null;
                                if (sValorAnterior != null) sValorAnterior = sValorAnterior.toString();
                                if (sNuevoValor != null) sNuevoValor = sNuevoValor.toString();
                                bEsNumero = sRegExVal.test(sNuevoValor);
                                header = "Ingreso Tarifario Mensual";
                                sMensaje = "Valor del Ingreso Tarifario Mensual no es válido";
                            }
                            else if (numColumna == '6') {
                                //Valida si el nuevo valor de la celda se encuentra en Regulado
                                lista = null;
                                if (sValorAnterior != null) sValorAnterior = sValorAnterior.toString();
                                if (sNuevoValor != null) sNuevoValor = sNuevoValor.toString();
                                bEsNumero = sRegExVal.test(sNuevoValor);
                                console.log(sValorAnterior);
                                console.log(sNuevoValor);
                                console.log(bEsNumero);
                                header = "Regulado";
                                sMensaje = "Valor de Regulado no es válido";
                            }
                            else if (numColumna == '7') {
                                //Valida si el nuevo valor de la celda se encuentra en Libre
                                lista = null;
                                if (sValorAnterior != null) sValorAnterior = sValorAnterior.toString();
                                if (sNuevoValor != null) sNuevoValor = sNuevoValor.toString();
                                bEsNumero = sRegExVal.test(sNuevoValor);
                                header = "Libre";
                                sMensaje = "Valor del Peaje unitario no es válido";
                            }
                            else if (numColumna == '8') {
                                //Valida si el nuevo valor de la celda se encuentra en Gran Usuario
                                lista = null;
                                if (sValorAnterior != null) sValorAnterior = sValorAnterior.toString();
                                if (sNuevoValor != null) sNuevoValor = sNuevoValor.toString();
                                bEsNumero = sRegExVal.test(sNuevoValor);
                                header = "Gran Usuario";
                                sMensaje = "Valor de Gran Usuario no es válido";
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
                        if (col === 0) {
                            //Oculta la primera columna: Pingcodi
                            cellProperties.renderer = getCustomRenderer;
                        }
                        if (col > 0 && col < 4) {
                            //Las columnas fijas
                            cellProperties.renderer = firstRowRenderer;
                        }
                        //else if (col >= 4) {
                        //    //Para el llenado de datos
                        //    cellProperties.renderer = negativeValueRenderer; 
                        //}
                        return cellProperties;
                    }
                });
            } catch (cError) {
                console.error(cError);
            }

            hot.render();
            if (iRegError > 0) {
                mostrarError("Lo sentimos, <b>" + iRegError + "</b> registro(s) no ha(n) sido leido(s) por presentar <b>errores</b>: " + sMensajeError);
            }
            else {
                mostrarMensaje("Por favor verifique los datos y luego proceda a <b>Grabar</b>");
            }
        },
        error: function () {
            mostrarError('Lo sentimos no se puede mostrar la hoja de cálculo')
        }
    });
}

CargarDataExcelWeb = function () {
    hot.destroy();
    mostrarGrillaExcel();
}

abrirPopup = function () {
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