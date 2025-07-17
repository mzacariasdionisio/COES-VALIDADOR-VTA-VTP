var controler = siteRoot + "transfpotencia/ingresopotefrdetalle/";
var hot;
var data;
var error = [];
$(function () {
    mostrarGrillaExcel();

    $('#btnGrabarExcel').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        //console.log(error);
        if (parseInt(error.length) > 0) {
            abrirPopup();
            //console.log(error.length);
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

    $('#btnEliminarDatos').click(function () {
        if (confirm("¿Desea eliminar la información de la hoja de cálculo?")) {
            mostrarAlerta("Espere un momento, se esta procesando su solicitud");
            eliminarExcel();
            mostrarGrillaExcel();
        }
    });

    $('#btnDescargarExcel').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        descargarArchivo(1);
    });

    uploadExcel();
});


mostrarGrillaExcel = function () {
    mostrarAlerta("Espere un momento, se esta procesando su solicitud");
    if (typeof hot != 'undefined') {
        hot.destroy();
    }
    var container = document.getElementById('grillaExcel');
    $.ajax({
        type: 'POST',
        url: controler + "GrillaExcel",
        data: { ipefrcodi: $('#Ipefrcodi').val(), pericodi: $('#Pericodi').val(), recpotcodi: $('#Recpotcodi').val() },
        dataType: 'json',
        success: function (result) {
            var listaEmpresas = result.ListaEmpresas;
            var listaCentralGeneracion = result.ListaCentralGeneracion;

            var columns = result.Columnas;
            var headers = result.Headers;
            var widths = result.Widths;
            var sRegExVal = new RegExp(/^\d+(?:[\.\,]\d+)?$/); //Validacion solo numero con decimales sin comillas
            data = result.Data;
            hot = new Handsontable(container, {
                data: data,
                maxCols: result.Columnas.length,
                colHeaders: headers,
                colWidths: widths,
                columnSorting: false,
                contextMenu: false,
                minSpareRows: 1,
                rowHeaders: true,
                columns: columns,
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
                            //Valida si el nuevo valor de la celda se encuentra en listaEmpresas 
                            lista = listaEmpresas;
                            bEsNumero = null;
                            header = "Empresa";
                            sMensaje = "Nombre de Empresa incorrecto";
                        }
                        else if (numColumna == '1') {
                            //Valida si el nuevo valor de la celda se encuentra en listaCentralGeneracion 
                            lista = listaCentralGeneracion;
                            bEsNumero = null;
                            header = "Central de Generación";
                            sMensaje = "Nombre de la Central de Generación es incorrecto";
                        }
                        else if (numColumna == '2') {
                            //Valida si el nuevo valor de la celda se encuentra en Potencia Efectiva
                            lista = null;
                            if (sValorAnterior != null) sValorAnterior = sValorAnterior.toString();
                            if (sNuevoValor != null) sNuevoValor = sNuevoValor.toString();
                            bEsNumero = sRegExVal.test(sNuevoValor);
                            header = "Potencia Efectiva";
                            sMensaje = "Valor de la Potencia Efectiva no es válido";
                        }
                        else if (numColumna == '3') {
                            //Valida si el nuevo valor de la celda se encuentra en Potencia Firme
                            lista = null;
                            if (sValorAnterior != null) sValorAnterior = sValorAnterior.toString();
                            if (sNuevoValor != null) sNuevoValor = sNuevoValor.toString();
                            bEsNumero = sRegExVal.test(sNuevoValor);
                            header = "Potencia Firme";
                            sMensaje = "Valor de la Potencia Firme no es válido";
                        }
                        else if (numColumna == '4') {
                            //Valida si el nuevo valor de la celda se encuentra en Potencia Remunerable
                            lista = null;
                            if (sValorAnterior != null) sValorAnterior = sValorAnterior.toString();
                            if (sNuevoValor != null) sNuevoValor = sNuevoValor.toString();
                            bEsNumero = sRegExVal.test(sNuevoValor);
                            header = "Potencia Remunerable";
                            sMensaje = "Valor de la Potencia Remunerable no es válido";
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
                                }
                            }
                        }
                    }
                },
                cells: function (row, col, prop) {
                    //console.log("col:" + col + " row:" + row + " prop" + prop);
                    var cellProperties = {};
                    if (col == 0 || col == 1) {
                        cellProperties.renderer = firstRowRendererCeleste;
                    }
                    //else if (col >= 2) {
                    //    //Para el llenado de datos
                    //    cellProperties.renderer = "negativeValueRenderer";
                    //}
                    return cellProperties;
                }
            });
            hot.render();
            $('#divAcciones').css('display', 'block');
            var iNumRegistros = hot.countRows();
            if (iNumRegistros >= 1) iNumRegistros = iNumRegistros - 1;
            mostrarExito('Se han encontrado ' + iNumRegistros + ' registro(s)');
        },
        error: function () {
            mostrarError('Lo sentimos no se puede mostrar la hoja de cálculo')
        }
    });
}

grabarExcel = function () {
    //console.log($('#Ipefrcodi').val());
    //console.log($('#Pericodi').val());
    //console.log($('#Recpotcodi').val());
    //console.log(hot.getData());
    $.ajax({
        type: "POST",
        url: controler + 'grabargrillaexcel',
        dataType: "json",
        contentType: 'application/json',
        traditional: true,
        data: JSON.stringify({ ipefrcodi: $('#Ipefrcodi').val(), pericodi: $('#Pericodi').val(), recpotcodi: $('#Recpotcodi').val(), datos: hot.getData() }),
        success: function (result) {
            //console.log(result);
            if (result == "1") {
                mostrarExito('La información se ha guardo correctamente');
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
    mostrarAlerta("Espere un momento, se esta procesando su solicitud");
    if (typeof hot != 'undefined') {
        hot.destroy();
    }
    var container = document.getElementById('grillaExcel');
    $.ajax({
        type: 'POST',
        url: controler + 'procesararchivo',
        data: { sarchivo: sFile, ipefrcodi: $('#Ipefrcodi').val(), pericodi: $('#Pericodi').val(), recpotcodi: $('#Recpotcodi').val() },
        dataType: 'json',
        success: function (result) {
            var listaEmpresas = result.ListaEmpresas;
            var listaCentralGeneracion = result.ListaCentralGeneracion;
            var iRegError = result.RegError;
            var sMensajeError = result.MensajeError;

            var columns = result.Columnas;
            var headers = result.Headers;
            var widths = result.Widths;
            var sRegExVal = new RegExp(/^\d+(?:[\.\,]\d+)?$/); //Validacion solo numero con decimales sin comillas
            data = result.Data;
            hot = new Handsontable(container, {
                data: data,
                maxCols: result.Columnas.length,
                colHeaders: headers,
                colWidths: widths,
                columnSorting: false,
                contextMenu: false,
                minSpareRows: 1,
                rowHeaders: true,
                columns: columns,
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
                            //Valida si el nuevo valor de la celda se encuentra en listaEmpresas 
                            lista = listaEmpresas;
                            bEsNumero = null;
                            header = "Empresa";
                            sMensaje = "Nombre de Empresa incorrecto";
                        }
                        else if (numColumna == '1') {
                            //Valida si el nuevo valor de la celda se encuentra en listaCentralGeneracion 
                            lista = listaCentralGeneracion;
                            bEsNumero = null;
                            header = "Central de Generación";
                            sMensaje = "Nombre de la Central de Generación es incorrecto";
                        }
                        else if (numColumna == '2') {
                            //Valida si el nuevo valor de la celda se encuentra en Potencia Efectiva
                            lista = null;
                            if (sValorAnterior != null) sValorAnterior = sValorAnterior.toString();
                            if (sNuevoValor != null) sNuevoValor = sNuevoValor.toString();
                            bEsNumero = sRegExVal.test(sNuevoValor);
                            header = "Potencia Efectiva";
                            sMensaje = "Valor de la Potencia Efectiva no es válido";
                        }
                        else if (numColumna == '3') {
                            //Valida si el nuevo valor de la celda se encuentra en Potencia Firme
                            lista = null;
                            if (sValorAnterior != null) sValorAnterior = sValorAnterior.toString();
                            if (sNuevoValor != null) sNuevoValor = sNuevoValor.toString();
                            bEsNumero = sRegExVal.test(sNuevoValor);
                            header = "Potencia Firme";
                            sMensaje = "Valor de la Potencia Firme no es válido";
                        }
                        else if (numColumna == '4') {
                            //Valida si el nuevo valor de la celda se encuentra en Potencia Remunerable
                            lista = null;
                            if (sValorAnterior != null) sValorAnterior = sValorAnterior.toString();
                            if (sNuevoValor != null) sNuevoValor = sNuevoValor.toString();
                            bEsNumero = sRegExVal.test(sNuevoValor);
                            header = "Potencia Remunerable";
                            sMensaje = "Valor de la Potencia Remunerable no es válido";
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
                                }
                            }
                        }
                    }
                },
                cells: function (row, col, prop) {
                    //console.log("col:" + col + " row:" + row + " prop" + prop);
                    var cellProperties = {};
                    if (col == 0 || col == 1) {
                        cellProperties.renderer = firstRowRendererCeleste;
                    }
                    //else if (col >= 2) {
                    //    //Para el llenado de datos
                    //    cellProperties.renderer = "negativeValueRenderer";
                    //}
                    return cellProperties;
                }
            });
            hot.render();
            if (iRegError > 0) {
                mostrarError("Lo sentimos, <b>" + iRegError + "</b> registro(s) no ha(n) sido leido(s) por presentar <b>errores</b>" + sMensajeError);
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
    //console.log(error.length);
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

eliminarExcel = function () {
    $.ajax({
        type: "POST",
        url: controler + 'eliminardatos',
        data: { ipefrcodi: $('#Ipefrcodi').val(), pericodi: $('#Pericodi').val(), recpotcodi: $('#Recpotcodi').val() },
        dataType: "json",
        success: function (result) {
            if (result == "1") {
                mostrarExito("Felicidades, la información se elimino correctamente...!");
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

descargarArchivo = function (formato) {
    $.ajax({
        type: 'POST',
        url: controler + 'exportardata',
        data: { ipefrcodi: $('#Ipefrcodi').val(), pericodi: $('#Pericodi').val(), recpotcodi: $('#Recpotcodi').val(), intervalo: $('#Intervalo').val(), formato: formato },
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