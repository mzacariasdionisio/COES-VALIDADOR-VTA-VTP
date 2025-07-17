var controler = siteRoot + 'transfpotencia/retiropotenciasc/';
var error = [];
$(function () {

    $('#btnConsultar').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        mostrarGrillaExcel();
    });

    $('#btnDescargarExcel').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        descargarArchivo(1);
    });

    $('#btnEliminarDatos').click(function () {
        if (confirm("¿Desea eliminar la información de la hoja de cálculo?")) {
            mostrarAlerta("Espere un momento, se esta procesando su solicitud");
            eliminarDatos();
            mostrarGrillaExcel();
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

    $('#btnGrabarExcel').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        if (parseInt(error.length) > 0) {
            abrirPopup();
        } else {
            grabarExcel();
        }
    });

    UploadExcel();

    // maps function to lookup string
    Handsontable.renderers.registerRenderer('negativeValueRenderer', negativeValueRenderer);
});

mostrarGrillaExcel = function () {
    if (typeof hot != 'undefined') {
        hot.destroy();
    }
    var precioPPM = parseFloat($('#Recpotpreciopoteppm').val()) * 1.15; //El precio de potencia podrá ser hasta el precio PPM (CU02) más +15%. //console.log(precioPPM);
    var potenciaMaxima = parseFloat("400000"); //Los datos de potencia egreso, potencia calculada, potencia declarada y potencia activa no sean superiores a 400,000
    //console.log(potenciaMaxima);
    var container = document.getElementById('grillaExcel');
    $.ajax({
        type: 'POST',
        url: controler + "grillaexcel",
        data: { pericodi: $('#pericodi').val(), recpotcodi: $('#recpotcodi').val() },
        dataType: 'json',
        success: function (result) {
            console.log(result);
            var listaEmpresas = result.ListaEmpresas;
            var listaBarras = result.ListaBarras;
            var listaTipoUsuario = result.ListaTipoUsuario;
            var listaCalidad = result.ListaCalidad;

            var columns = result.Columnas;
            var widths = result.Widths;
            var RegExVal = new RegExp(/^\d+(?:[\.\,]\d+)?$/); //Validacion solo numero con decimales sin comillas
            hot = new Handsontable(container, {
                data: result.Data,
                maxCols: result.Columnas.length,
                colHeaders: false,
                rowHeaders: true,
                colWidths: widths,
                contextMenu: false,
                minSpareRows: 1,
                columns: columns,
                fixedRowsTop: result.fixedRowsTop,
                fixedColumnsLeft: result.FixedColumnsLeft,
                currentRowClassName: 'currentRow',
                mergeCells: [{ row: 0, col: 0, rowspan: 2, colspan: 1 },
                             { row: 0, col: 1, rowspan: 2, colspan: 1 },
                             { row: 0, col: 2, rowspan: 2, colspan: 1 },
                             { row: 0, col: 3, rowspan: 2, colspan: 1 },
                             { row: 0, col: 4, rowspan: 2, colspan: 1 },
                             { row: 0, col: 5, rowspan: 2, colspan: 1 },
                             { row: 0, col: 6, rowspan: 2, colspan: 1 },
                             { row: 0, col: 7, rowspan: 1, colspan: 3 },
                             { row: 0, col: 10, rowspan: 2, colspan: 1 }],
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
                        var sMensajeAux;

                        if (numColumna == '0') {
                            //Valida si el nuevo valor de la celda se encuentra en listaEmpresas 
                            lista = listaEmpresas;
                            bEsNumero = null;
                            header = "Cliente";
                            sMensaje = "Nombre del Cliente es incorrecto";
                        }
                        else if (numColumna == '1') {
                            //Valida si el nuevo valor de la celda se encuentra en listaBarras 
                            lista = listaBarras;
                            bEsNumero = null;
                            header = "Barra";
                            sMensaje = "Nombre de Barra es incorrecto";
                        }
                        else if (numColumna == '2') {
                            //Valida si el nuevo valor de la celda se encuentra en listaTipoUsuario 
                            lista = listaTipoUsuario;
                            bEsNumero = null;
                            header = "Tipo de Usuario";
                            sMensaje = "Tipo de usuario es incorrecto";
                        }
                        else if (numColumna == '3') {
                            //Valida si el nuevo valor de la celda se encuentra en Precio PPB
                            lista = null;
                            if (sValorAnterior != null) sValorAnterior = sValorAnterior.toString();
                            if (sNuevoValor != null) sNuevoValor = sNuevoValor.toString();
                            bEsNumero = RegExVal.test(sNuevoValor);
                            header = "Precio PPB";
                            sMensaje = "Valor del Precio PPB no es válido";
                        }
                        else if (numColumna == '4') {
                            //Valida si el nuevo valor de la celda se encuentra en Precio Potencia
                            lista = null;
                            if (sValorAnterior != null) sValorAnterior = sValorAnterior.toString();
                            if (sNuevoValor != null) sNuevoValor = sNuevoValor.toString();
                            bEsNumero = RegExVal.test(sNuevoValor);
                            header = "Precio Potencia";
                            sMensaje = "Valor del Precio Potencia no es válido";
                            if (bEsNumero) {   //Validamos la condición: El precio de potencia podrá ser hasta el precio PPM (CU02) más +15%.
                                if (precioPPM < sNuevoValor) {
                                    //console.log(sNuevoValor);
                                    bEsNumero = false;
                                    sMensaje = "El valor " + sNuevoValor + " del Precio Potencia es mayor a Precio PPM más +15% (" + precioPPM + ")";
                                }
                            }
                            if (sValorAnterior !== null) {
                                //Se fuerza que se elimine el valor anterior por si fue ingresado por precioPPM < sNuevoValor
                                //console.log(sValorAnterior);
                                sMensajeAux = "El valor " + sValorAnterior + " del Precio Potencia es mayor a Precio PPM más +15% (" + precioPPM + ")";
                                if ($.inArray(sValorAnterior.concat("_-_" + numFila + "_-_" + header + "_-_" + sMensajeAux), error) > -1) {
                                    //Elimina el valor del array error
                                    error = $.grep(error, function (value) { return value != sValorAnterior.concat("_-_" + numFila + "_-_" + header + "_-_" + sMensajeAux) });
                                }
                            }
                        }
                        else if (numColumna == '5') {
                            //Valida si el nuevo valor de la celda se encuentra en Potencia Egreso
                            lista = null;
                            if (sValorAnterior != null) sValorAnterior = sValorAnterior.toString();
                            if (sNuevoValor != null) sNuevoValor = sNuevoValor.toString();
                            bEsNumero = RegExVal.test(sNuevoValor);
                            header = "Potencia Egreso";
                            sMensaje = "Valor de la Potencia Egreso no es válido";
                            if (bEsNumero) {   //Validamos la condición: la potencia egreso no sean superiores a 400,000.
                                if (potenciaMaxima < sNuevoValor) {
                                    //console.log(sNuevoValor);
                                    //console.log(potenciaMaxima);
                                    bEsNumero = false;
                                    sMensaje = "El valor " + sNuevoValor + " de la Potencia Egreso no debe ser superior a 400,000";
                                }
                            }
                            if (sValorAnterior !== null) {
                                //Se fuerza que se elimine el valor anterior por si fue ingresado potenciaMaxima < sNuevoValor
                                //console.log(sValorAnterior);
                                sMensajeAux = "El valor " + sValorAnterior + " de la Potencia Egreso no debe ser superior a 400,000";
                                if ($.inArray(sValorAnterior.concat("_-_" + numFila + "_-_" + header + "_-_" + sMensajeAux), error) > -1) {
                                    //Elimina el valor del array error
                                    error = $.grep(error, function (value) { return value != sValorAnterior.concat("_-_" + numFila + "_-_" + header + "_-_" + sMensajeAux) });
                                }
                            }
                        }
                        else if (numColumna == '6') {
                            //Valida si el nuevo valor de la celda se encuentra en Peaje unitario
                            lista = null;
                            if (sValorAnterior != null) sValorAnterior = sValorAnterior.toString();
                            if (sNuevoValor != null) sNuevoValor = sNuevoValor.toString();
                            bEsNumero = RegExVal.test(sNuevoValor);
                            header = "Peaje Unitario";
                            sMensaje = "Valor del Peaje Unitario no es válido";
                        }
                        else if (numColumna == '7') {
                            //Valida si el nuevo valor de la celda se encuentra en listaBarras 
                            lista = listaBarras;
                            bEsNumero = null;
                            header = "FCO - Barra";
                            sMensaje = "Nombre de la Barra del Flujo de Caja Optimo es incorrecto";
                        }
                        else if (numColumna == '8') {
                            //Valida si el nuevo valor de la celda se encuentra en Potencia Activa
                            lista = null;
                            if (sValorAnterior != null) sValorAnterior = sValorAnterior.toString();
                            if (sNuevoValor != null) sNuevoValor = sNuevoValor.toString();
                            bEsNumero = RegExVal.test(sNuevoValor);
                            header = "Potencia Activa";
                            sMensaje = "Valor de la Potencia Activa del Flujo de Caja Optimo no es válido";
                            if (bEsNumero) {   //Validamos la condición: la Potencia Activa del Flujo de Caja Optimo no sean superiores a 400,000.
                                if (potenciaMaxima < sNuevoValor) {
                                    //console.log(sNuevoValor);
                                    //console.log(potenciaMaxima);
                                    bEsNumero = false;
                                    sMensaje = "El valor " + sNuevoValor + " de la Potencia Activa del Flujo de Caja Optimo no debe ser superior a 400,000";
                                }
                            }
                            if (sValorAnterior !== null) {
                                //Se fuerza que se elimine el valor anterior por si fue ingresado potenciaMaxima < sNuevoValor
                                //console.log(sValorAnterior);
                                sMensajeAux = "El valor " + sValorAnterior + " de la Potencia Activa del Flujo de Caja Optimo no debe ser superior a 400,000";
                                if ($.inArray(sValorAnterior.concat("_-_" + numFila + "_-_" + header + "_-_" + sMensajeAux), error) > -1) {
                                    //Elimina el valor del array error
                                    error = $.grep(error, function (value) { return value != sValorAnterior.concat("_-_" + numFila + "_-_" + header + "_-_" + sMensajeAux) });
                                }
                            }
                        }
                        else if (numColumna == '9') {
                            //Valida si el nuevo valor de la celda se encuentra en Potencia Reactiva
                            lista = null;
                            if (sValorAnterior != null) sValorAnterior = sValorAnterior.toString();
                            if (sNuevoValor != null) sNuevoValor = sNuevoValor.toString();
                            bEsNumero = RegExVal.test(sNuevoValor);
                            header = "Potencia Reactiva";
                            sMensaje = "Valor de la Potencia Reactiva del Flujo de Caja Optimo no válido";
                        }
                        else if (numColumna == '10') {
                            //Valida si el nuevo valor de la celda se encuentra en Calidad
                            lista = listaCalidad;
                            bEsNumero = null;
                            header = "Calidad";
                            sMensaje = "Tipo de Calidad es incorrecto";
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
                    if (row == 0 || row == 1) {
                        cellProperties.renderer = firstRowRendererCabecera;
                    }
                    else if (col == 0 || col == 1 || col == 2 || col == 10) {
                        cellProperties.renderer = firstRowRendererCeleste;
                    }
                    else if (col == 7 || col == 8 || col == 9) {
                        cellProperties.renderer = firstRowRendererAmarillo;
                    }
                    else if (col >= 3) {
                        //Para el llenado de datos
                        cellProperties.renderer = "negativeValueRenderer";
                    }
                    return cellProperties;
                }
            });
            hot.render();
            $('#divAcciones').css('display', 'block');
            var iNumRegistros = hot.countRows();
            if (iNumRegistros >= 2) iNumRegistros = iNumRegistros - 3;
            mostrarExito('Se han encontrado ' + iNumRegistros + ' registro(s)');
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

descargarArchivo = function (formato) {
    $.ajax({
        type: 'POST',
        url: controler + 'exportardata',
        data: { pericodi: $('#pericodi').val(), recpotcodi: $('#recpotcodi').val(), formato: formato },
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

eliminarDatos = function () {
    $.ajax({
        type: 'POST',
        url: controler + 'eliminardatos',
        data: { pericodi: $('#pericodi').val(), recpotcodi: $('#recpotcodi').val() },
        dataType: 'json',
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

grabarExcel = function () {
    $.ajax({
        type: "POST",
        url: controler + 'grabargrillaexcel',
        dataType: "json",
        contentType: 'application/json',
        traditional: true,
        data: JSON.stringify({
            pericodi: $('#pericodi').val(), recpotcodi: $('#recpotcodi').val(), datos: hot.getData()
        }),
        success: function (result) {
            if (result == "1") {
                mostrarExito('Los registros han sido correctamente almacenados...!');
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

UploadExcel = function () {
    uploader = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: 'btnSelecionarExcel',
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
    if (typeof hot != 'undefined') {
        hot.destroy();
    }
    var container = document.getElementById('grillaExcel');
    var precioPPM = parseFloat($('#Recpotpreciopoteppm').val()) * 1.15; //El precio de potencia podrá ser hasta el precio PPM (CU02) más +15%. //console.log(precioPPM);
    var potenciaMaxima = parseFloat("400000"); //Los datos de potencia egreso, potencia calculada, potencia declarada y potencia activa no sean superiores a 400,000
    $.ajax({
        type: 'POST',
        url: controler + 'procesararchivo',
        data: { sarchivo: sFile, pericodi: $('#pericodi').val(), recpotcodi: $('#recpotcodi').val() },
        dataType: 'json',
        cache: false,
        success: function (result) {
            var listaEmpresas = result.ListaEmpresas;
            var listaBarras = result.ListaBarras;
            var listaTipoUsuario = result.ListaTipoUsuario;
            var listaCalidad = result.ListaCalidad;
            var iRegError = result.RegError;
            var sMensajeError = result.MensajeError;

            var columns = result.Columnas;
            var widths = result.Widths;
            var RegExVal = new RegExp(/^\d+(?:[\.\,]\d+)?$/); //Validacion solo numero con decimales sin comillas
            hot = new Handsontable(container, {
                data: result.Data,
                maxCols: result.Columnas.length,
                colHeaders: false,
                rowHeaders: true,
                colWidths: widths,
                contextMenu: false,
                minSpareRows: 1,
                columns: columns,
                fixedRowsTop: result.fixedRowsTop,
                fixedColumnsLeft: result.FixedColumnsLeft,
                currentRowClassName: 'currentRow',
                mergeCells: [{ row: 0, col: 0, rowspan: 2, colspan: 1 },
                             { row: 0, col: 1, rowspan: 2, colspan: 1 },
                             { row: 0, col: 2, rowspan: 2, colspan: 1 },
                             { row: 0, col: 3, rowspan: 2, colspan: 1 },
                             { row: 0, col: 4, rowspan: 2, colspan: 1 },
                             { row: 0, col: 5, rowspan: 2, colspan: 1 },
                             { row: 0, col: 6, rowspan: 2, colspan: 1 },
                             { row: 0, col: 7, rowspan: 1, colspan: 3 },
                             { row: 0, col: 10, rowspan: 2, colspan: 1 }],
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
                        var sMensajeAux;

                        if (numColumna == '0') {
                            //Valida si el nuevo valor de la celda se encuentra en listaEmpresas 
                            lista = listaEmpresas;
                            bEsNumero = null;
                            header = "Cliente";
                            sMensaje = "Nombre del Cliente es incorrecto";
                        }
                        else if (numColumna == '1') {
                            //Valida si el nuevo valor de la celda se encuentra en listaBarras 
                            lista = listaBarras;
                            bEsNumero = null;
                            header = "Barra";
                            sMensaje = "Nombre de Barra es incorrecto";
                        }
                        else if (numColumna == '2') {
                            //Valida si el nuevo valor de la celda se encuentra en listaTipoUsuario 
                            lista = listaTipoUsuario;
                            bEsNumero = null;
                            header = "Tipo de Usuario";
                            sMensaje = "Tipo de usuario es incorrecto";
                        }
                        else if (numColumna == '3') {
                            //Valida si el nuevo valor de la celda se encuentra en Precio PPB
                            lista = null;
                            if (sValorAnterior != null) sValorAnterior = sValorAnterior.toString();
                            if (sNuevoValor != null) sNuevoValor = sNuevoValor.toString();
                            bEsNumero = RegExVal.test(sNuevoValor);
                            header = "Precio PPB";
                            sMensaje = "Valor del Precio PPB no es válido";
                        }
                        else if (numColumna == '4') {
                            //Valida si el nuevo valor de la celda se encuentra en Precio Potencia
                            lista = null;
                            if (sValorAnterior != null) sValorAnterior = sValorAnterior.toString();
                            if (sNuevoValor != null) sNuevoValor = sNuevoValor.toString();
                            bEsNumero = RegExVal.test(sNuevoValor);
                            header = "Precio Potencia";
                            sMensaje = "Valor del Precio Potencia no es válido";
                            if (bEsNumero) {   //Validamos la condición: El precio de potencia podrá ser hasta el precio PPM (CU02) más +15%.
                                if (precioPPM < sNuevoValor) {
                                    //console.log(sNuevoValor);
                                    bEsNumero = false;
                                    sMensaje = "El valor " + sNuevoValor + " del Precio Potencia es mayor a Precio PPM más +15% (" + precioPPM + ")";
                                }
                            }
                            if (sValorAnterior !== null) {
                                //Se fuerza que se elimine el valor anterior por si fue ingresado por precioPPM < sNuevoValor
                                //console.log(sValorAnterior);
                                sMensajeAux = "El valor " + sValorAnterior + " del Precio Potencia es mayor a Precio PPM más +15% (" + precioPPM + ")";
                                if ($.inArray(sValorAnterior.concat("_-_" + numFila + "_-_" + header + "_-_" + sMensajeAux), error) > -1) {
                                    //Elimina el valor del array error
                                    error = $.grep(error, function (value) { return value != sValorAnterior.concat("_-_" + numFila + "_-_" + header + "_-_" + sMensajeAux) });
                                }
                            }
                        }
                        else if (numColumna == '5') {
                            //Valida si el nuevo valor de la celda se encuentra en Potencia Egreso
                            lista = null;
                            if (sValorAnterior != null) sValorAnterior = sValorAnterior.toString();
                            if (sNuevoValor != null) sNuevoValor = sNuevoValor.toString();
                            bEsNumero = RegExVal.test(sNuevoValor);
                            header = "Potencia Egreso";
                            sMensaje = "Valor de la Potencia Egreso no es válido";
                            if (bEsNumero) {   //Validamos la condición: la potencia egreso no sean superiores a 400,000.
                                if (potenciaMaxima < sNuevoValor) {
                                    //console.log(sNuevoValor);
                                    //console.log(potenciaMaxima);
                                    bEsNumero = false;
                                    sMensaje = "El valor " + sNuevoValor + " de la Potencia Egreso no debe ser superior a 400,000";
                                }
                            }
                            if (sValorAnterior !== null) {
                                //Se fuerza que se elimine el valor anterior por si fue ingresado potenciaMaxima < sNuevoValor
                                //console.log(sValorAnterior);
                                sMensajeAux = "El valor " + sValorAnterior + " de la Potencia Egreso no debe ser superior a 400,000";
                                if ($.inArray(sValorAnterior.concat("_-_" + numFila + "_-_" + header + "_-_" + sMensajeAux), error) > -1) {
                                    //Elimina el valor del array error
                                    error = $.grep(error, function (value) { return value != sValorAnterior.concat("_-_" + numFila + "_-_" + header + "_-_" + sMensajeAux) });
                                }
                            }
                        }
                        else if (numColumna == '6') {
                            //Valida si el nuevo valor de la celda se encuentra en Peaje unitario
                            lista = null;
                            if (sValorAnterior != null) sValorAnterior = sValorAnterior.toString();
                            if (sNuevoValor != null) sNuevoValor = sNuevoValor.toString();
                            bEsNumero = RegExVal.test(sNuevoValor);
                            header = "Peaje Unitario";
                            sMensaje = "Valor del Peaje Unitario no es válido";
                        }
                        else if (numColumna == '7') {
                            //Valida si el nuevo valor de la celda se encuentra en listaBarras 
                            lista = listaBarras;
                            bEsNumero = null;
                            header = "FCO - Barra";
                            sMensaje = "Nombre de la Barra del Flujo de Caja Optimo es incorrecto";
                        }
                        else if (numColumna == '8') {
                            //Valida si el nuevo valor de la celda se encuentra en Potencia Activa
                            lista = null;
                            if (sValorAnterior != null) sValorAnterior = sValorAnterior.toString();
                            if (sNuevoValor != null) sNuevoValor = sNuevoValor.toString();
                            bEsNumero = RegExVal.test(sNuevoValor);
                            header = "Potencia Activa";
                            sMensaje = "Valor de la Potencia Activa del Flujo de Caja Optimo no es válido";
                            if (bEsNumero) {   //Validamos la condición: la Potencia Activa del Flujo de Caja Optimo no sean superiores a 400,000.
                                if (potenciaMaxima < sNuevoValor) {
                                    //console.log(sNuevoValor);
                                    //console.log(potenciaMaxima);
                                    bEsNumero = false;
                                    sMensaje = "El valor " + sNuevoValor + " de la Potencia Activa del Flujo de Caja Optimo no debe ser superior a 400,000";
                                }
                            }
                            if (sValorAnterior !== null) {
                                //Se fuerza que se elimine el valor anterior por si fue ingresado potenciaMaxima < sNuevoValor
                                //console.log(sValorAnterior);
                                sMensajeAux = "El valor " + sValorAnterior + " de la Potencia Activa del Flujo de Caja Optimo no debe ser superior a 400,000";
                                if ($.inArray(sValorAnterior.concat("_-_" + numFila + "_-_" + header + "_-_" + sMensajeAux), error) > -1) {
                                    //Elimina el valor del array error
                                    error = $.grep(error, function (value) { return value != sValorAnterior.concat("_-_" + numFila + "_-_" + header + "_-_" + sMensajeAux) });
                                }
                            }
                        }
                        else if (numColumna == '9') {
                            //Valida si el nuevo valor de la celda se encuentra en Potencia Reactiva
                            lista = null;
                            if (sValorAnterior != null) sValorAnterior = sValorAnterior.toString();
                            if (sNuevoValor != null) sNuevoValor = sNuevoValor.toString();
                            bEsNumero = RegExVal.test(sNuevoValor);
                            header = "Potencia Reactiva";
                            sMensaje = "Valor de la Potencia Reactiva del Flujo de Caja Optimo no válido";
                        }
                        else if (numColumna == '10') {
                            //Valida si el nuevo valor de la celda se encuentra en Calidad
                            lista = listaCalidad;
                            bEsNumero = null;
                            header = "Calidad";
                            sMensaje = "Tipo de Calidad es incorrecto";
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
                    if (row == 0 || row == 1) {
                        cellProperties.renderer = firstRowRendererCabecera;
                    }
                    else if (col == 0 || col == 1 || col == 2 || col == 10) {
                        cellProperties.renderer = firstRowRendererCeleste;
                    }
                    else if (col == 7 || col == 8 || col == 9) {
                        cellProperties.renderer = firstRowRendererAmarillo;
                    }
                    else if (col >= 3) {
                        //Para el llenado de datos
                        cellProperties.renderer = "negativeValueRenderer";
                    }
                    return cellProperties;
                }
            });
            hot.render();
            $('#divAcciones').css('display', 'block');
            if (iRegError > 0) {
                mostrarError("Lo sentimos, <b>" + iRegError + "</b> registro(s) no ha(n) sido leido(s) por presentar <b>errores</b>" + sMensajeError);
            }
            else {
                var iNumRegistros = hot.countRows();
                if (iNumRegistros >= 2) iNumRegistros = iNumRegistros - 3;
                mostrarMensaje("Numero de registros cargados: <b>" + iNumRegistros + "</b>, Por favor verifique los datos y luego proceda a <b>Grabar</b>");
            }
        },
        error: function () {
            mostrarError('Lo sentimos no se puede mostrar la hoja de cálculo')
        }
    });
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

Recargar = function () {
    var cmbPericodi = document.getElementById('pericodi');
    window.location.href = controler + "index?pericodi=" + cmbPericodi.value;
}