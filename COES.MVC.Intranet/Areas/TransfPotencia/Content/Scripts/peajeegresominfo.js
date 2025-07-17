var controler = siteRoot + "transfpotencia/peajeegresominfo/";

var error = [];
$(document).ready(function () {
    $('#tab-container').easytabs({
        animate: false
    });

    $('#emprcodi').multipleSelect({
        width: '220px',
        filter: true,
        selectAll: false,
        single: true,
        onClick: function (view) {
            var emprcodi = $("#emprcodi").multipleSelect('getSelects');
            $('#hfComboEmpresa').val(emprcodi);
        },
        onClose: function (view) {
            var emprcodi = $("#emprcodi").multipleSelect('getSelects');
            $('#hfComboEmpresa').val(emprcodi);
        }
    });

    $('#cliemprcodi').multipleSelect({
        width: '220px',
        filter: true,
        selectAll: false,
        single: true,
        onClick: function (view) {
            var cliemprcodi = $("#cliemprcodi").multipleSelect('getSelects');
            $('#hfComboCliente').val(cliemprcodi);
        },
        onClose: function (view) {
            var cliemprcodi = $("#cliemprcodi").multipleSelect('getSelects');
            $('#hfComboCliente').val(cliemprcodi);
        }
    });

    $('#barrcodi').multipleSelect({
        width: '220px',
        filter: true,
        selectAll: false,
        single: true,
        onClick: function (view) {
            var barrcodi = $("#barrcodi").multipleSelect('getSelects');
            $('#hfComboBarra').val(barrcodi);
        },
        onClose: function (view) {
            var barrcodi = $("#barrcodi").multipleSelect('getSelects');
            $('#hfComboBarra').val(barrcodi);
        }
    });

    $('#barrcodifco').multipleSelect({
        width: '220px',
        filter: true,
        selectAll: false,
        single: true,
        onClick: function (view) {
            var barrcodifco = $("#barrcodifco").multipleSelect('getSelects');
            $('#hfComboBarrafco').val(barrcodifco);
        },
        onClose: function (view) {
            var barrcodifco = $("#barrcodifco").multipleSelect('getSelects');
            $('#hfComboBarrafco').val(barrcodifco);
        }
    });

    $('#btnConsultar').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        mostrarGrillaExcel();
    });

    $('#btnDescargarExcel').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        descargarArchivo(1);
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

    $('#btnConsultarVista').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        mostrarGrillaExcelConsulta();
    });

    $('#btnDescargarExcelConsulta').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        descargarArchivoConsulta(1);
    });

    $('#btnDescargarPdfConsulta').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        descargarArchivoConsulta(2);
    });

    $('#btnInfoFaltante').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        mostrarGrillaExcelInfoFaltante();
    });

    UploadExcel();

});

mostrarGrillaExcel = function () {
    error = [];
    if (typeof hot != 'undefined') {
        hot.destroy();
    }
    var precioPPM = parseFloat($('#Recpotpreciopoteppm').val()) * 1.15; //El precio de potencia podrá ser hasta el precio PPM (CU02) más +15%. //console.log(precioPPM);
    var potenciaMaxima = parseFloat("400000"); //Los datos de potencia egreso, potencia calculada, potencia declarada y potencia activa no sean superiores a 400,000
    var container = document.getElementById('grillaExcel');
    $.ajax({
        type: 'POST',
        url: controler + "grillaexcel",
        data: { pericodi: $('#pericodi').val(), recpotcodi: $('#recpotcodi').val() },
        dataType: 'json',
        success: function (result) {
            var listaBarras = result.ListaBarras;
            var listaEmpresas = result.ListaEmpresas;
            var listaLicitacion = result.ListaLicitacion;
            var listaTipoUsuario = result.ListaTipoUsuario;
            var listaCalidad = result.ListaCalidad;
            var bGrabar = result.Grabar;

            var columns = result.Columnas;
            var widths = result.Widths;
            var sRegExVal = new RegExp(/^-{0,1}\d*\.{0,1}\d+$/); //^\d+(?:[\.\,]\d+)?$
            hot = new Handsontable(container, {
                data: result.Data,
                maxCols: result.Columnas.length,
                maxRows: result.Data.length,
                colHeaders: false,
                rowHeaders: true,
                colWidths: widths,
                //contextMenu: true,
                minSpareRows: 1,
                columns: columns,
                height: 700,
                fixedRowsTop: 2,
                //fixedColumnsLeft: result.FixedColumnsLeft,
                currentRowClassName: 'currentRow',
                mergeCells: [{ row: 0, col: 0, rowspan: 2, colspan: 1 },
                { row: 0, col: 1, rowspan: 2, colspan: 1 },
                { row: 0, col: 2, rowspan: 2, colspan: 1 },
                { row: 0, col: 3, rowspan: 2, colspan: 1 },
                { row: 0, col: 4, rowspan: 2, colspan: 1 },
                { row: 0, col: 5, rowspan: 1, colspan: 2 },
                { row: 0, col: 7, rowspan: 1, colspan: 3 },
                { row: 0, col: 10, rowspan: 1, colspan: 3 },
                { row: 0, col: 13, rowspan: 2, colspan: 1 },
                { row: result.Data.length - 1, col: 0, rowspan: 1, colspan: 5 }
                ],

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
                            header = "Empresa Generadora";
                            sMensaje = "Nombre del Empresa es incorrecto";
                        }
                        if (numColumna == '1') {
                            //Valida si el nuevo valor de la celda se encuentra en listaEmpresas 
                            lista = listaEmpresas;
                            bEsNumero = null;
                            header = "Cliente";
                            sMensaje = "Nombre del Cliente es incorrecto";
                        }
                        else if (numColumna == '2') {
                            //Valida si el nuevo valor de la celda se encuentra en listaBarras 
                            lista = listaBarras;
                            bEsNumero = null;
                            header = "Barra";
                            sMensaje = "Nombre de Barra es incorrecto";
                        }
                        else if (numColumna == '3') {
                            //Valida si el nuevo valor de la celda se encuentra en listaTipoUsuario 
                            lista = listaTipoUsuario;
                            bEsNumero = null;
                            header = "Tipo de Usuario";
                            sMensaje = "Tipo de usuario es incorrecto";
                        }
                        else if (numColumna == '4') {
                            //Valida si el nuevo valor de la celda se encuentra en listaLicitacion 
                            lista = listaLicitacion;
                            bEsNumero = null;
                            header = "Licitación";
                            sMensaje = "El valor de Licitacion es incorrecto";
                        }
                        else if (numColumna == '5') {
                            //Valida si el nuevo valor de la celda se encuentra en Precio Potencia
                            lista = null;
                            if (sValorAnterior != null) sValorAnterior = sValorAnterior.toString();
                            if (sNuevoValor != null) sNuevoValor = sNuevoValor.toString();
                            bEsNumero = sRegExVal.test(sNuevoValor);
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
                        else if (numColumna == '6') {
                            //Valida si el nuevo valor de la celda se encuentra en Potencia Egreso
                            lista = null;
                            if (sValorAnterior != null) sValorAnterior = sValorAnterior.toString();
                            if (sNuevoValor != null) sNuevoValor = sNuevoValor.toString();
                            bEsNumero = sRegExVal.test(sNuevoValor);
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
                        else if (numColumna == '7') {
                            //Valida si el nuevo valor de la celda se encuentra en Potencia Calculada
                            lista = null;
                            if (sValorAnterior != null) sValorAnterior = sValorAnterior.toString();
                            if (sNuevoValor != null) sNuevoValor = sNuevoValor.toString();
                            bEsNumero = sRegExVal.test(sNuevoValor);
                            header = "Potencia Calculada";
                            sMensaje = "Valor de la Potencia Calculada no es válido";
                        }
                        else if (numColumna == '8') {
                            //Valida si el nuevo valor de la celda se encuentra en Potencia Declarada
                            lista = null;
                            if (sValorAnterior != null) sValorAnterior = sValorAnterior.toString();
                            if (sNuevoValor != null) sNuevoValor = sNuevoValor.toString();
                            bEsNumero = sRegExVal.test(sNuevoValor);
                            header = "Potencia Declarada";
                            sMensaje = "Valor de la Potencia Declarada no es válido";
                        }
                        else if (numColumna == '9') {
                            //Valida si el nuevo valor de la celda se encuentra en Peaje unitario
                            lista = null;
                            if (sValorAnterior != null) sValorAnterior = sValorAnterior.toString();
                            if (sNuevoValor != null) sNuevoValor = sNuevoValor.toString();
                            bEsNumero = sRegExVal.test(sNuevoValor);
                            header = "Peaje Unitario";
                            sMensaje = "Valor del Peaje Unitario no es válido";
                        }
                        else if (numColumna == '10') {
                            //Valida si el nuevo valor de la celda se encuentra en listaBarras 
                            lista = listaBarras;
                            bEsNumero = null;
                            header = "FCO - Barra";
                            sMensaje = "Nombre de la Barra del Flujo de Caja Optimo es incorrecto";
                        }
                        else if (numColumna == '11') {
                            //Valida si el nuevo valor de la celda se encuentra en Potencia Activa
                            lista = null;
                            if (sValorAnterior != null) sValorAnterior = sValorAnterior.toString();
                            if (sNuevoValor != null) sNuevoValor = sNuevoValor.toString();
                            bEsNumero = sRegExVal.test(sNuevoValor);
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
                        else if (numColumna == '12') {
                            //Valida si el nuevo valor de la celda se encuentra en Potencia Reactiva
                            lista = null;
                            if (sValorAnterior != null) sValorAnterior = sValorAnterior.toString();
                            if (sNuevoValor != null) sNuevoValor = sNuevoValor.toString();
                            bEsNumero = sRegExVal.test(sNuevoValor);
                            header = "Potencia Reactiva";
                            sMensaje = "Valor de la Potencia Reactiva del Flujo de Caja Optimo no válido";
                        }
                        else if (numColumna == '13') {
                            //Valida si el nuevo valor de la celda se encuentra en Calidad
                            lista = listaCalidad;
                            bEsNumero = null;
                            header = "Calidad";
                            sMensaje = "Tipo de Calidad es incorrecto";
                        }
                        //else {
                        //    lista = null;
                        //    bEsNumero = null;
                        //    header = "";
                        //    sMensaje = "";
                        //}
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
                    else if (col == 0 || col == 1 || col == 2 || col == 3 || col == 4 || col == 13) {
                        cellProperties.renderer = firstRowRendererCeleste;
                    }
                    else if (col == 10 || col == 11 || col == 12) {
                        cellProperties.renderer = firstRowRendererAmarillo;
                    }

                    if (row == result.Data.length - 1) {
                        cellProperties.renderer = firstRowRendererCabecera;
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
                $('#btnValidarGrillaExcel').css('display', 'block');
                $('#btnGrabarExcel').css('display', 'block');
                $('#btnEliminarDatos').css('display', 'block');
            }
            else {
                $('#btnValidarGrillaExcel').css('display', 'none');
                $('#btnGrabarExcel').css('display', 'none');
                $('#btnEliminarDatos').css('display', 'none');
            }
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

descargarArchivo = function (formato) {
    var emprcodi = -10;
    $.ajax({
        type: 'POST',
        url: controler + 'exportardata',
        data: { pericodi: $('#pericodi').val(), recpotcodi: $('#recpotcodi').val(), emprcodi: emprcodi, formato: formato },
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
            pericodi: $('#pericodi').val(), recpotcodi: $('#recpotcodi').val(), datos: hot.getData()
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
                mostrarAlerta("Subida completada <strong> procesando el archivo...</strong>");
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
    var precioPPM = parseFloat($('#Recpotpreciopoteppm').val()) * 1.15; //El precio de potencia podrá ser hasta el precio PPM (CU02) más +15%. //console.log(PrecioPPM);
    var potenciaMaxima = parseFloat("400000"); //Los datos de potencia egreso, potencia calculada, potencia declarada y potencia activa no sean superiores a 400,000
    $.ajax({
        type: 'POST',
        url: controler + 'procesararchivo',
        data: { sarchivo: sFile, pericodi: $('#pericodi').val(), recpotcodi: $('#recpotcodi').val() },
        dataType: 'json',
        cache: false,
        success: function (result) {
            var listaBarras = result.ListaBarras;
            var listaEmpresas = result.ListaEmpresas;
            var listaLicitacion = result.ListaLicitacion;
            var listaTipoUsuario = result.ListaTipoUsuario;
            var listaCalidad = result.ListaCalidad;
            var bGrabar = result.Grabar;
            var iRegError = result.RegError;
            var sMensajeError = result.MensajeError;

            var columns = result.Columnas;
            var widths = result.Widths;
            var sRegExVal = new RegExp(/^-{0,1}\d*\.{0,1}\d+$/); //^\d+(?:[\.\,]\d+)?$
            hot = new Handsontable(container, {
                data: result.Data,
                maxCols: result.Columnas.length,
                colHeaders: false,
                rowHeaders: true,
                colWidths: widths,
                contextMenu: false,
                minSpareRows: 1,
                columns: columns,
                fixedRowsTop: result.FixedRowsTop,
                fixedColumnsLeft: result.FixedColumnsLeft,
                currentRowClassName: 'currentRow',
                mergeCells: [{ row: 0, col: 0, rowspan: 2, colspan: 1 },
                { row: 0, col: 1, rowspan: 2, colspan: 1 },
                { row: 0, col: 2, rowspan: 2, colspan: 1 },
                { row: 0, col: 3, rowspan: 2, colspan: 1 },
                { row: 0, col: 4, rowspan: 2, colspan: 1 },
                { row: 0, col: 5, rowspan: 1, colspan: 2 },
                { row: 0, col: 7, rowspan: 1, colspan: 3 },
                { row: 0, col: 10, rowspan: 1, colspan: 3 },
                { row: 0, col: 13, rowspan: 2, colspan: 1 }],
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
                            header = "Empresa Generadora";
                            sMensaje = "Nombre del Empresa es incorrecto";
                        }
                        if (numColumna == '1') {
                            //Valida si el nuevo valor de la celda se encuentra en listaEmpresas 
                            lista = listaEmpresas;
                            bEsNumero = null;
                            header = "Cliente";
                            sMensaje = "Nombre del Cliente es incorrecto";
                        }
                        else if (numColumna == '2') {
                            //Valida si el nuevo valor de la celda se encuentra en listaBarras 
                            lista = listaBarras;
                            bEsNumero = null;
                            header = "Barra";
                            sMensaje = "Nombre de Barra es incorrecto";
                        }
                        else if (numColumna == '3') {
                            //Valida si el nuevo valor de la celda se encuentra en listaTipoUsuario 
                            lista = listaTipoUsuario;
                            bEsNumero = null;
                            header = "Tipo de Usuario";
                            sMensaje = "Tipo de usuario es incorrecto";
                        }
                        else if (numColumna == '4') {
                            //Valida si el nuevo valor de la celda se encuentra en listaLicitacion 
                            lista = listaLicitacion;
                            bEsNumero = null;
                            header = "Licitación";
                            sMensaje = "El valor de Licitacion es incorrecto";
                        }
                        else if (numColumna == '5') {
                            //Valida si el nuevo valor de la celda se encuentra en Precio Potencia
                            lista = null;
                            if (sValorAnterior != null) sValorAnterior = sValorAnterior.toString();
                            if (sNuevoValor != null) sNuevoValor = sNuevoValor.toString();
                            bEsNumero = sRegExVal.test(sNuevoValor);
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
                        else if (numColumna == '6') {
                            //Valida si el nuevo valor de la celda se encuentra en Potencia Egreso
                            lista = null;
                            if (sValorAnterior != null) sValorAnterior = sValorAnterior.toString();
                            if (sNuevoValor != null) sNuevoValor = sNuevoValor.toString();
                            bEsNumero = sRegExVal.test(sNuevoValor);
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
                        else if (numColumna == '7') {
                            //Valida si el nuevo valor de la celda se encuentra en Potencia Calculada
                            lista = null;
                            if (sValorAnterior != null) sValorAnterior = sValorAnterior.toString();
                            if (sNuevoValor != null) sNuevoValor = sNuevoValor.toString();
                            bEsNumero = sRegExVal.test(sNuevoValor);
                            header = "Potencia Calculada";
                            sMensaje = "Valor de la Potencia Calculada no es válido";
                        }
                        else if (numColumna == '8') {
                            //Valida si el nuevo valor de la celda se encuentra en Potencia Declarada
                            lista = null;
                            if (sValorAnterior != null) sValorAnterior = sValorAnterior.toString();
                            if (sNuevoValor != null) sNuevoValor = sNuevoValor.toString();
                            bEsNumero = sRegExVal.test(sNuevoValor);
                            header = "Potencia Declarada";
                            sMensaje = "Valor de la Potencia Declarada no es válido";
                        }
                        else if (numColumna == '9') {
                            //Valida si el nuevo valor de la celda se encuentra en Peaje unitario
                            lista = null;
                            if (sValorAnterior != null) sValorAnterior = sValorAnterior.toString();
                            if (sNuevoValor != null) sNuevoValor = sNuevoValor.toString();
                            bEsNumero = sRegExVal.test(sNuevoValor);
                            header = "Peaje Unitario";
                            sMensaje = "Valor del Peaje Unitario no es válido";
                        }
                        else if (numColumna == '10') {
                            //Valida si el nuevo valor de la celda se encuentra en listaBarras 
                            lista = listaBarras;
                            bEsNumero = null;
                            header = "FCO - Barra";
                            sMensaje = "Nombre de la Barra del Flujo de Caja Optimo es incorrecto";
                        }
                        else if (numColumna == '11') {
                            //Valida si el nuevo valor de la celda se encuentra en Potencia Activa
                            lista = null;
                            if (sValorAnterior != null) sValorAnterior = sValorAnterior.toString();
                            if (sNuevoValor != null) sNuevoValor = sNuevoValor.toString();
                            bEsNumero = sRegExVal.test(sNuevoValor);
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
                        else if (numColumna == '12') {
                            //Valida si el nuevo valor de la celda se encuentra en Potencia Reactiva
                            lista = null;
                            if (sValorAnterior != null) sValorAnterior = sValorAnterior.toString();
                            if (sNuevoValor != null) sNuevoValor = sNuevoValor.toString();
                            bEsNumero = sRegExVal.test(sNuevoValor);
                            header = "Potencia Reactiva";
                            sMensaje = "Valor de la Potencia Reactiva del Flujo de Caja Optimo no válido";
                        }
                        else if (numColumna == '13') {
                            //Valida si el nuevo valor de la celda se encuentra en Calidad
                            lista = listaCalidad;
                            bEsNumero = null;
                            header = "Calidad";
                            sMensaje = "Tipo de Calidad es incorrecto";
                        }
                        //else {
                        //    lista = null;
                        //    bEsNumero = null;
                        //    header = "";
                        //    sMensaje = "";
                        //}
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
                    else if (col == 0 || col == 1 || col == 2 || col == 3 || col == 4 || col == 13) {
                        cellProperties.renderer = firstRowRendererCeleste;
                    }
                    else if (col == 10 || col == 11 || col == 12) {
                        cellProperties.renderer = firstRowRendererAmarillo;
                    }
                    //else if (col >= 4) {
                    //    //Para el llenado de datos
                    //    cellProperties.renderer = "negativeValueRenderer";
                    //}
                    return cellProperties;
                }
            });
            hot.render();
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
                var iNumRegistros = hot.countRows();
                if (iNumRegistros >= 2) iNumRegistros = iNumRegistros - 3;
                mostrarMensaje("Numero de registros cargados: <b>" + iNumRegistros + "</b>, Por favor verifique los datos y luego proceda a <b>Grabar</b> en el icono del menú <b>Enviar datos</b>");
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
    for (var i = error.length - 1; i >= 0; i--) {
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

RecargarConsulta = function () {
    var cmbPericodi = document.getElementById('pericodiConsulta');
    
    window.location.href = controler + "index?pericodi=" + cmbPericodi.value + "#paso2";

}

mostrarGrillaExcelConsulta = function () {
    if (typeof hotConsulta != 'undefined') {
        hotConsulta.destroy();
    }
    var container = document.getElementById('grillaExcelConsulta');
    var pericodi = $('#pericodiConsulta').val();
    var recpotcodi = $('#recpotcodiConsulta').val();
    var pegrmitipousuario = $('#pegrmitipousuario').val();
    var pegrmilicitacion = $('#pegrmilicitacion').val();
    var pegrmicalidad = $('#pegrmicalidad').val();

    if ($('#hfComboEmpresa').val() == "") $('#hfComboEmpresa').val("0");
    if ($('#hfComboCliente').val() == "") $('#hfComboCliente').val("0");
    if ($('#hfComboBarra').val() == "") $('#hfComboBarra').val("0");
    if ($('#hfComboBarrafco').val() == "") $('#hfComboBarrafco').val("0");



    //console.log(emprcodi);
    $.ajax({
        type: 'POST',
        url: controler + "grillaexcelconsulta",
        data: {
            pericodi: pericodi,
            recpotcodi: recpotcodi,
            emprcodi: $('#hfComboEmpresa').val(),
            cliemprcodi: $('#hfComboCliente').val(),
            barrcodi: $('#hfComboBarra').val(),
            barrcodifco: $('#hfComboBarrafco').val(),
            pegrmitipousuario: pegrmitipousuario,
            pegrmilicitacion: pegrmilicitacion,
            pegrmicalidad: pegrmicalidad
        },
        dataType: 'json',
        success: function (result) {
            var columns = result.Columnas;
            var widths = result.Widths;
            hotConsulta = new Handsontable(container, {
                data: result.Data,
                maxCols: result.Columnas.length,
                colHeaders: false,
                rowHeaders: true,
                colWidths: widths,
                contextMenu: false,
                columns: columns,
                fixedRowsTop: result.FixedRowsTop,
                fixedColumnsLeft: result.FixedColumnsLeft,
                height: 600,
                currentRowClassName: 'currentRow',
                mergeCells: [{ row: 0, col: 0, rowspan: 2, colspan: 1 },
                { row: 0, col: 1, rowspan: 2, colspan: 1 },
                { row: 0, col: 2, rowspan: 2, colspan: 1 },
                { row: 0, col: 3, rowspan: 2, colspan: 1 },
                { row: 0, col: 4, rowspan: 2, colspan: 1 },
                { row: 0, col: 5, rowspan: 1, colspan: 2 },
                { row: 0, col: 7, rowspan: 1, colspan: 3 },
                { row: 0, col: 10, rowspan: 1, colspan: 3 },
                { row: 0, col: 13, rowspan: 2, colspan: 1 },
                { row: result.Data.length - 1, col: 0, rowspan: 1, colspan: 5 }
                ],
                cells: function (row, col, prop) {
                    //console.log("col:" + col + " row:" + row + " prop" + prop);
                    var cellProperties = {};
                    if (row == 0 || row == 1) {
                        cellProperties.renderer = firstRowRendererCabecera;
                    }
                    else if (col == 0 || col == 1 || col == 2 || col == 3 || col == 4 || col == 13) {
                        cellProperties.renderer = firstRowRendererCeleste;
                    }
                    else if (col == 10 || col == 11 || col == 12) {
                        cellProperties.renderer = firstRowRendererAmarillo;
                    }
                    //else if (col >= 4) {
                    //    //Para el llenado de datos
                    //    cellProperties.renderer = "negativeValueRenderer";
                    //}
                    if (row == result.Data.length - 1) {
                        cellProperties.renderer = firstRowRendererCabecera;
                    }
                    return cellProperties;
                },
            });
            $('#divAccionesConsulta').css('display', 'block');
            var iNumRegistros = hotConsulta.countRows();
            if (iNumRegistros >= 2) iNumRegistros = iNumRegistros - 2;
            mostrarExito('Se han encontrado ' + iNumRegistros + ' registro(s)');
        },
        error: function () {
            mostrarError('Lo sentimos no se puede mostrar la hoja de cálculo')
        }
    });
}

descargarArchivoConsulta = function (formato) {
    var pericodi = $('#pericodiConsulta').val();
    var recpotcodi = $('#recpotcodiConsulta').val();
    var emprcodi;
    var cliemprcodi;
    var barrcodi;
    var barrcodifco;
    var pegrmitipousuario = $('#pegrmitipousuario').val();
    var pegrmilicitacion = $('#pegrmilicitacion').val();
    var pegrmicalidad = $('#pegrmicalidad').val();

    if ($('#hfComboEmpresa').val() == "") $('#hfComboEmpresa').val("0");
    if ($('#hfComboCliente').val() == "") $('#hfComboCliente').val("0");
    if ($('#hfComboBarra').val() == "") $('#hfComboBarra').val("0");
    if ($('#hfComboBarrafco').val() == "") $('#hfComboBarrafco').val("0");

    $.ajax({
        type: 'POST',
        url: controler + 'exportardata',
        data: {
            pericodi: pericodi,
            recpotcodi: recpotcodi,
            emprcodi: $('#hfComboEmpresa').val(),
            formato: formato,
            cliemprcodi: $('#hfComboCliente').val(),
            barrcodi: $('#hfComboBarra').val(),
            barrcodifco: $('#hfComboBarrafco').val(),
            pegrmitipousuario: pegrmitipousuario,
            pegrmilicitacion: pegrmilicitacion,
            pegrmicalidad: pegrmicalidad
        },
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

RecargarInfoFaltante = function () {
    var cmbPericodi = document.getElementById('pericodiInfoFaltante');
    window.location.href = controler + "index?pericodi=" + cmbPericodi.value + "#paso3";
}

mostrarGrillaExcelInfoFaltante = function () {
    if (typeof hotInfoFaltante != 'undefined') {
        hotInfoFaltante.destroy();
    }
    var container = document.getElementById('grillaExcelInfoFaltante');
    var pericodi = $('#pericodiInfoFaltante').val();
    var recpotcodi = $('#recpotcodiInfoFaltante').val();

    $.ajax({
        type: 'POST',
        url: controler + "grillaexcelinfofaltante",
        data: { pericodi: pericodi, recpotcodi: recpotcodi },
        dataType: 'json',
        success: function (result) {
            var columns = result.Columnas;
            var widths = result.Widths;
            hotInfoFaltante = new Handsontable(container, {
                data: result.Data,
                maxCols: result.Columnas.length,
                colHeaders: false,
                rowHeaders: true,
                colWidths: widths,
                contextMenu: false,
                columns: columns,
                currentRowClassName: 'currentRow',
                cells: function (row, col, prop) {
                    //console.log("col:" + col + " row:" + row + " prop" + prop);
                    var cellProperties = {};
                    if (row == 0) {
                        cellProperties.renderer = firstRowRendererCabecera;
                    }
                    else {
                        cellProperties.renderer = firstRowRendererCeleste;
                    }
                    return cellProperties;
                },
            });
            var iNumRegistros = hotInfoFaltante.countRows();
            if (iNumRegistros >= 2) iNumRegistros = iNumRegistros - 3;
            mostrarMensaje("Numero de registros faltantes con respecto al periodo anterior: <b>" + iNumRegistros + "</b>");
        },
        error: function () {
            mostrarError('Lo sentimos no se puede mostrar la hoja de cálculo')
        }
    });
}