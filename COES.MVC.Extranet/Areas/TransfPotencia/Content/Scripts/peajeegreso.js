var controler = siteRoot + 'transfpotencia/peajeegreso/';
var error = [];
$(function () {

    $('#btnConsultar').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        mostrarGrillaExcel(0);

        if ($('#pegrcodi').val() == 0)
        { mostrarMensaje('Por favor, registrar la información de Egresos y Peajes'); }
    });

    $('#btnDescargarExcel').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        descargarArchivo(1);
    });

    $('#btnEliminarDatos').click(function () {
        if (confirm("¿Desea eliminar la información de la hoja de cálculo?")) {
            mostrarAlerta("Espere un momento, se esta procesando su solicitud");
            eliminarDatos();
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

    $('#btnVerEnvios').click(function () {
        verEnviosAnteriores();
    });

    if (($('#pericodi').val() > 0) && ($('#recpotcodi').val() > 0) && ($('#pegrcodi').val() > 0)) {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        mostrarGrillaExcel($('#pegrcodi').val());
    }

    UploadExcel();

});

mostrarGrillaExcel = function (iPegrcodi) {
    var pegrcodi = 0;
    if (iPegrcodi > 0) {
        pegrcodi = $('#pegrcodi').val();
    }
    //console.log(iPegrcodi);
    //console.log($('#pegrcodi').val());
    $.ajax({
        type: 'POST',
        url: controler + "grillaexcel",
        data: { pericodi: $('#pericodi').val(), recpotcodi: $('#recpotcodi').val(), emprcodi: $('#emprcodi').val(), pegrcodi: pegrcodi },
        dataType: 'json',
        success: function (result) {
            configurarExcelWeb(result);
            $('#numregistros').val(result.NumRegistros);
            //console.log($('#numregistros').val());
            //ASSETEC 20190219
            //console.log(result.Pegrcodi);
            $('#pegrcodi').val(result.Pegrcodi);
            //console.log($('#pegrcodi').val());
            var bGrabar = result.Grabar;
            if (!bGrabar) {
                $('#btnSelecionarExcel').css('display', 'block');
                $('#btnValidarGrillaExcel').css('display', 'block');
                $('#btnGrabarExcel').css('display', 'block');
                $('#btnEliminarDatos').css('display', 'block');
            }
            else {
                $('#btnSelecionarExcel').css('display', 'none');
                $('#btnValidarGrillaExcel').css('display', 'none');
                $('#btnGrabarExcel').css('display', 'none');
                $('#btnEliminarDatos').css('display', 'none');
            }
            $('#divAcciones').css('display', 'block');
            mostrarExito('Código del envío: ' + $('#pegrcodi').val() + ", Fecha de envío: " + $('#pegrfeccreacion').val() + " - Número de registros cargados: " + $('#numregistros').val());
        },
        error: function () {
            mostrarError('Lo sentimos no se puede mostrar la hoja de cálculo')
        }
    });
}

configurarExcelWeb = function (result) {
    error = [];
    if (typeof hot != 'undefined') {
        hot.destroy();
    }
    var precioPPM = parseFloat($('#Recpotpreciopoteppm').val()) * 1.15; //El precio de potencia podrá ser hasta el precio PPM (CU02) más +15%. //console.log(precioPPM);
    var potenciaMaxima = parseFloat("400000"); //Los datos de potencia egreso, potencia calculada, potencia declarada y potencia activa no sean superiores a 400,000
    var container = document.getElementById('grillaExcel');
    calculateSizeHandsontable(container);
    $('#testado').val(result.sEstado);
    //console.log($('#testado').val());
    //-------------------------------------------------------------------------------------------------
    var listaBarras = result.ListaBarras;
    var listaEmpresas = result.ListaEmpresas;
    var listaLicitacion = result.ListaLicitacion;
    var listaTipoUsuario = result.ListaTipoUsuario;
    var listaCalidad = result.ListaCalidad;
    
    var columns = result.Columnas;
    var widths = result.Widths;
    var sRegExVal = new RegExp(/^-{0,1}\d*\.{0,1}\d+$/); //^\d+(?:[\.\,]\d+)?$
    hot = new Handsontable(container, {
        data: result.Data,
        colHeaders: false,
        rowHeaders: true,
        colWidths: widths,
        contextMenu: true,
        minSpareRows: 1,
        columns: columns,
        fixedRowsTop: result.FixedRowsTop,
        fixedColumnsLeft: result.FixedColumnsLeft,
        currentRowClassName: 'currentRow',
        mergeCells: [{ row: 0, col: 0, rowspan: 2, colspan: 1 },
        { row: 0, col: 1, rowspan: 2, colspan: 1 },
        { row: 0, col: 2, rowspan: 2, colspan: 1 },
        { row: 0, col: 3, rowspan: 2, colspan: 1 },
        { row: 0, col: 4, rowspan: 1, colspan: 2 },
        { row: 0, col: 6, rowspan: 1, colspan: 3 },
        { row: 0, col: 9, rowspan: 1, colspan: 3 },
        { row: 0, col: 12, rowspan: 2, colspan: 1 }],
        beforeChange: function (changes, source) {
            //changes[i][0] = fila
            //changes[i][1] = columna
            //changes[i][2] = valor anteriror
            //changes[i][3] = nuevo valor                  
            for (var i = changes.length - 1; i >= 0; i--) {
                var numFila = changes[i][0];
                var numColumna = changes[i][1];
                var valorAnterior = changes[i][2];
                var nuevoValor = changes[i][3];
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
                    //Valida si el nuevo valor de la celda se encuentra en listaLicitacion 
                    lista = listaLicitacion;
                    bEsNumero = null;
                    header = "Licitación";
                    sMensaje = "El valor de Licitacion es incorrecto";
                }
                else if (numColumna == '4') {
                    //Valida si el nuevo valor de la celda se encuentra en Precio Potencia
                    lista = null;
                    if (valorAnterior != null) valorAnterior = valorAnterior.toString();
                    if (nuevoValor != null) nuevoValor = nuevoValor.toString();
                    bEsNumero = sRegExVal.test(nuevoValor);
                    header = "Precio Potencia";
                    sMensaje = "Valor del Precio Potencia no es válido";
                    if (bEsNumero) {   //Validamos la condición: El precio de potencia podrá ser hasta el precio PPM (CU02) más +15%.
                        if (precioPPM < nuevoValor) {
                            //console.log(nuevoValor);
                            bEsNumero = false;
                            sMensaje = "El valor " + nuevoValor + " del Precio Potencia es mayor a Precio PPM más +15% (" + precioPPM + ")";
                        }
                    }
                    if (valorAnterior !== null) {
                        //Se fuerza que se elimine el valor anterior por si fue ingresado por precioPPM < nuevoValor
                        //console.log(valorAnterior);
                        sMensajeAux = "El valor " + valorAnterior + " del Precio Potencia es mayor a Precio PPM más +15% (" + precioPPM + ")";
                        if ($.inArray(valorAnterior.concat("_-_" + numFila + "_-_" + header + "_-_" + sMensajeAux), error) > -1) {
                            //Elimina el valor del array error
                            error = $.grep(error, function (value) { return value != valorAnterior.concat("_-_" + numFila + "_-_" + header + "_-_" + sMensajeAux) });
                        }
                    }
                }
                else if (numColumna == '5') {
                    //Valida si el nuevo valor de la celda se encuentra en Potencia Egreso
                    lista = null;
                    if (valorAnterior != null) valorAnterior = valorAnterior.toString();
                    if (nuevoValor != null) nuevoValor = nuevoValor.toString();
                    bEsNumero = sRegExVal.test(nuevoValor);
                    header = "Potencia Egreso";
                    sMensaje = "Valor de la Potencia Egreso no es válido";
                    if (bEsNumero) {   //Validamos la condición: la potencia egreso no sean superiores a 400,000.
                        if (potenciaMaxima < nuevoValor) {
                            //console.log(nuevoValor);
                            //console.log(potenciaMaxima);
                            bEsNumero = false;
                            sMensaje = "El valor " + nuevoValor + " de la Potencia Egreso no debe ser superior a 400,000";
                        }
                    }
                    if (valorAnterior !== null) {
                        //Se fuerza que se elimine el valor anterior por si fue ingresado potenciaMaxima < nuevoValor
                        //console.log(valorAnterior);
                        sMensajeAux = "El valor " + valorAnterior + " de la Potencia Egreso no debe ser superior a 400,000";
                        if ($.inArray(valorAnterior.concat("_-_" + numFila + "_-_" + header + "_-_" + sMensajeAux), error) > -1) {
                            //Elimina el valor del array error
                            error = $.grep(error, function (value) { return value != valorAnterior.concat("_-_" + numFila + "_-_" + header + "_-_" + sMensajeAux) });
                        }
                    }
                }
                else if (numColumna == '6') {
                    //Valida si el nuevo valor de la celda se encuentra en Potencia Calculada
                    lista = null;
                    if (valorAnterior != null) valorAnterior = valorAnterior.toString();
                    if (nuevoValor != null) nuevoValor = nuevoValor.toString();
                    bEsNumero = sRegExVal.test(nuevoValor);
                    header = "Potencia Calculada";
                    sMensaje = "Valor de la Potencia Calculada no es válido";
                }
                else if (numColumna == '7') {
                    //Valida si el nuevo valor de la celda se encuentra en Potencia Declarada
                    lista = null;
                    if (valorAnterior != null) valorAnterior = valorAnterior.toString();
                    if (nuevoValor != null) nuevoValor = nuevoValor.toString();
                    bEsNumero = sRegExVal.test(nuevoValor);
                    header = "Potencia Declarada";
                    sMensaje = "Valor de la Potencia Declarada no es válido";
                }
                else if (numColumna == '8') {
                    //Valida si el nuevo valor de la celda se encuentra en Peaje unitario
                    lista = null;
                    if (valorAnterior != null) valorAnterior = valorAnterior.toString();
                    if (nuevoValor != null) nuevoValor = nuevoValor.toString();
                    bEsNumero = sRegExVal.test(nuevoValor);
                    header = "Peaje Unitario";
                    sMensaje = "Valor del Peaje Unitario no es válido";
                }
                else if (numColumna == '9') {
                    //Valida si el nuevo valor de la celda se encuentra en listaBarras 
                    lista = listaBarras;
                    bEsNumero = null;
                    header = "FCO - Barra";
                    sMensaje = "Nombre de la Barra del Flujo de Caja Optimo es incorrecto";
                }
                else if (numColumna == '10') {
                    //Valida si el nuevo valor de la celda se encuentra en Potencia Activa
                    lista = null;
                    if (valorAnterior != null) valorAnterior = valorAnterior.toString();
                    if (nuevoValor != null) nuevoValor = nuevoValor.toString();
                    bEsNumero = sRegExVal.test(nuevoValor);
                    header = "Potencia Activa";
                    sMensaje = "Valor de la Potencia Activa del Flujo de Caja Optimo no es válido";
                    if (bEsNumero) {   //Validamos la condición: la Potencia Activa del Flujo de Caja Optimo no sean superiores a 400,000.
                        if (potenciaMaxima < nuevoValor) {
                            //console.log(nuevoValor);
                            //console.log(potenciaMaxima);
                            bEsNumero = false;
                            sMensaje = "El valor " + nuevoValor + " de la Potencia Activa del Flujo de Caja Optimo no debe ser superior a 400,000";
                        }
                    }
                    if (valorAnterior !== null) {
                        //Se fuerza que se elimine el valor anterior por si fue ingresado potenciaMaxima < nuevoValor
                        //console.log(valorAnterior);
                        sMensajeAux = "El valor " + valorAnterior + " de la Potencia Activa del Flujo de Caja Optimo no debe ser superior a 400,000";
                        if ($.inArray(valorAnterior.concat("_-_" + numFila + "_-_" + header + "_-_" + sMensajeAux), error) > -1) {
                            //Elimina el valor del array error
                            error = $.grep(error, function (value) { return value != valorAnterior.concat("_-_" + numFila + "_-_" + header + "_-_" + sMensajeAux) });
                        }
                    }
                }
                else if (numColumna == '11') {
                    //Valida si el nuevo valor de la celda se encuentra en Potencia Reactiva
                    lista = null;
                    if (valorAnterior != null) valorAnterior = valorAnterior.toString();
                    if (nuevoValor != null) nuevoValor = nuevoValor.toString();
                    bEsNumero = sRegExVal.test(nuevoValor);
                    header = "Potencia Reactiva";
                    sMensaje = "Valor de la Potencia Reactiva del Flujo de Caja Optimo no válido";
                }
                else if (numColumna == '12') {
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
                if ((bEsNumero == true) || ($.inArray(nuevoValor, lista) > -1)) {   //Valida si el valor anterior se encuentra en el array "error"
                    if (valorAnterior !== null) {
                        if ($.inArray(valorAnterior.concat("_-_" + numFila + "_-_" + header + "_-_" + sMensaje), error) > -1) {
                            //Elimina el valor del array error
                            error = $.grep(error, function (value) { return value != valorAnterior.concat("_-_" + numFila + "_-_" + header + "_-_" + sMensaje) });
                            //console.log(error);
                        }
                    }
                }
                else {   //Si el nuevo valor es vacio retorna el valor anterior
                    if (nuevoValor === "") {
                        return false;
                    }
                    else {   //Busca si el nuevo valor 'error' se encuentra en el array 'error'
                        if ($.inArray(nuevoValor.concat("_-_" + numFila + "_-_" + header + "_-_" + sMensaje), error) > -1) {
                        }
                        else {
                            //Si no se encuentra se agrega
                            error.push(nuevoValor.concat("_-_" + numFila + "_-_" + header + "_-_" + sMensaje));
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
            else if (col == 0 || col == 1 || col == 2 || col == 3 || col == 12) {
                cellProperties.renderer = firstRowRendererCeleste;
            }
            else if (col == 9 || col == 10 || col == 11) {
                cellProperties.renderer = firstRowRendererAmarillo;
            }
            //else if (col >= 3) {
            //    //Para el llenado de datos
            //    cellProperties.renderer = "negativeValueRenderer";
            //}
            return cellProperties;
        },
    });
    hot.render();
}

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

descargarArchivo = function (formato) {
    $.ajax({
        type: 'POST',
        url: controler + 'exportardata',
        data: { pericodi: $('#pericodi').val(), recpotcodi: $('#recpotcodi').val(), pegrcodi: $('#pegrcodi').val(), formato: formato },
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
        data: { pericodi: $('#pericodi').val(), recpotcodi: $('#recpotcodi').val(), emprcodi: $('#emprcodi').val(), pegrcodi: pegrcodi },
        dataType: 'json',
        success: function (result) {
            if (result == "1") {
                alert("Felicidades, la información se elimino correctamente...!");
                mostrarGrillaExcel(0);
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
            pericodi: $('#pericodi').val(), recpotcodi: $('#recpotcodi').val(), emprcodi: $('#emprcodi').val(), testado: $('#testado').val(), datos: hot.getData()
        }),
        success: function (result) {
            if (result.sError == "") {
                var iPegrcodi = result.Pegrcodi;
                var sFecha = result.sFecha;
                var pegrcodi = document.getElementById('pegrcodi');
                pegrcodi.value = iPegrcodi;
                var iNumRegistros = result.NumRegistros;
                var sPlazo = result.sPlazo;
                if (sPlazo == "S") {
                    mostrarExito('Operación Exitosa - ' + "Código de envío: <b>" + iPegrcodi + "</b>, Fecha de envío: <b>" + sFecha + "</b>, Numero de registros ingresados: <b>" + iNumRegistros + "</b>");
                }
                else {
                    //sPlazo == "N"e
                    var sAlerta = "<br><span  style = 'margin-left: 30px;'>La información remitida será considerado como fuera de plazo y quedará a consideración del COES su inclusión en el proceso de valorización</span>";
                    mostrarAlerta('Operación completada - ' + "Código de envío: <b>" + iPegrcodi + "</b>, Fecha de envío: <b>" + sFecha + "</b>, Numero de registros ingresados: <b>" + iNumRegistros + "</b>" + sAlerta);
                }
            }
            else {
                mostrarError('Lo sentimos, ha ocurrido un error: ' + result.sError);
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
    $.ajax({
        type: 'POST',
        url: controler + 'procesararchivo',
        data: { sarchivo: sFile, pegrcodi: $('#pegrcodi').val() },
        dataType: 'json',
        success: function (result) {
            configurarExcelWeb(result);
            var iRegError = result.RegError;
            var sMensajeError = result.MensajeError;
            var iNumRegistros = result.NumRegistros;
            var bGrabar = result.Grabar;
            if (!bGrabar) {
                $('#btnValidarGrillaExcel').css('display', 'block');
                $('#btnGrabarExcel').css('display', 'block');
            }
            else {
                $('#btnValidarGrillaExcel').css('display', 'none');
                $('#btnGrabarExcel').css('display', 'none');
            }
            $('#divAcciones').css('display', 'block');
            if (iRegError > 0) {
                mostrarError("Lo sentimos, <b>" + iRegError + "</b> registro(s) no ha(n) sido leido(s) por presentar <b>errores</b>" + sMensajeError);
            }
            else {
                mostrarExito("Numero de registros cargados: <b>" + iNumRegistros + "</b>, por favor verifique los datos y luego proceda a <b>Grabar</b> en el icono del menú <b>Enviar datos</b>");
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

recargar = function () {
    var cmbPericodi = document.getElementById('pericodi');
    window.location.href = controler + "index?pericodi=" + cmbPericodi.value;
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

seleccionarEmpresa = function () {
    $.ajax({
        type: 'POST',
        url: controler + "EscogerEmpresa",
        success: function (evt) {
            $('#popup').html(evt);
            setTimeout(function () {
                $('#popup').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown'
                });
            }, 50);
        },
        error: function () {
            alert("Lo sentimos, ha ocurrido un error inesperado");
        }
    });
}

verEnviosAnteriores = function () {
    $.ajax({
        type: 'POST',
        url: controler + "listaenvios",
        data: { pericodi: $('#pericodi').val(), recpotcodi: $('#recpotcodi').val(), emprcodi: $('#emprcodi').val() },
        success: function (evt) {
            $('#popup').html(evt);
            setTimeout(function () {
                $('#popup').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown'
                });
            }, 50);
        },
        error: function () {
            alert("Lo sentimos, ha ocurrido un error inesperado");
        }
    });
}