var controler = siteRoot + 'transferencias/ingresoinfopotenciapeaje/';
var error = [];
var errorPrecioPotenciaArray = [];
var errorPotenciaCoincidenteArray = [];
var errorPeajeUnitarioArray = [];
var errorPotenciaCoincidente = true;
var carga = false;
var withButtons = true;
var cantidadRows = 0;
var instanciaTabla;

$(function () {




    $('#btnConsultar').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        mostrarGrillaExcel($('#pegrcodi').val());

        // descomentar luego
        //if ($('#pegrcodi').val() == 0) { mostrarMensaje('Por favor, registrar la información de Egresos y Peajes'); }
    });

    $('#btnDescargarExcel').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        descargarArchivo(1);
    });

    $('#btnEliminarDatos').click(function () {
        if (confirm("¿Desea eliminar la información de la hoja de cálculo?")) {
            mostrarAlerta("Espere un momento, se esta procesando su solicitud");
            eliminarDatos2();
        }
    });

    $('#btnValidarGrillaExcel').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        if ((errorPrecioPotenciaArray.length > 0 || errorPotenciaCoincidenteArray.length > 0 || errorPeajeUnitarioArray.length > 0) || !carga) {
            abrirPopup();
        } else {
            mostrarExito("La hoja de cálculo no tiene errores...!");
        }
    });

    $('#btnGrabarExcel').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        if ((errorPrecioPotenciaArray.length > 0 || errorPotenciaCoincidenteArray.length > 0 || errorPeajeUnitarioArray.length > 0) || !carga) {
            abrirPopup();
        } else {
            grabarExcel();
        }
    });

    $('#btnVerEnvios').click(function () {
        if ($('#pegrcodi').val() > 0) {
            verEnviosAnteriores();
        } else {
            mostrarExito("No existen validaciones porque no se a detectado envíos...!");
        }
    });

    $('#btnVerValidaciones').click(function () {
        verValidaciones();
    });
    eliminarDatos2 = function () {

        $.ajax({
            type: 'POST',
            url: controler + 'eliminardatos',
            data: { pericodi: $('#pericodi').val(), recpotcodi: $('#recpotcodi').val(), emprcodi: $('#emprcodi').val(), pegrcodi: $('#pegrcodi').val() },
            dataType: 'json',
            success: function (result) {
                if (result == "1") {
                    alert("Felicidades, la información se elimino correctamente...!");
                    mostrarGrillaExcel($('#pegrcodi').val());
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

    verValidaciones = function () {
        $.ajax({
            type: 'POST',
            url: controler + "listavalidaciones",
            data: { pegrcodi: $('#pegrcodi').val() },
            success: function (evt) {
                $('#popup2').html(evt);
                setTimeout(function () {
                    $('#popup2').bPopup({
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

    if (($('#pericodi').val() > 0) && ($('#recpotcodi').val() > 0) && ($('#pegrcodi').val() > 0)) {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        mostrarGrillaExcel($('#pegrcodi').val());
    }


    UploadExcel();

});


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
        data: {
            sarchivo: sFile, pegrcodi: $('#pegrcodi').val(), pericodi: $('#pericodi').val(),
            recpotcodi: $('#recpotcodi').val(),
            emprcodi: $('#emprcodi').val()
        },
        dataType: 'json',
        success: function (result) {
            carga = true;
            errorPrecioPotenciaArray = [];
            errorPotenciaCoincidenteArray = [];
            errorPeajeUnitarioArray = [];
            configurarExcelWeb(result, 1);
            var iRegError = result.RegError;
            var sMensajeError = result.MensajeError;
            var iNumRegistros = result.NumRegistros;
            var bGrabar = result.bGrabar;
            if (!bGrabar) {
                $('#btnValidarGrillaExcel').css('display', 'block');
                $('#btnGrabarExcel').css('display', 'block');
                //$('#btnVerValidaciones').css('display', 'block');
            }
            else {
                $('#btnValidarGrillaExcel').css('display', 'none');
                $('#btnGrabarExcel').css('display', 'none');
                //$('#btnVerValidaciones').css('display', 'none');
            }
            $('#divAcciones').css('display', 'block');
            if (iRegError > 0) {
                mostrarErrorHtml("Lo sentimos, " + iRegError + " registro(s) no ha(n) sido leido(s) por presentar errores " + sMensajeError);
            } else {
                mostrarExito("Numero de registros cargados: <b>" + iNumRegistros + "</b>, por favor verifique los datos y luego proceda a <b>Grabar</b> en el icono del menú <b>Enviar datos</b>");
            }
        },
        error: function () {
            mostrarError('Lo sentimos no se puede mostrar la hoja de cálculo')
        }
    });
}


mostrarErrorAlert = function (error) {
    alert(error);
}


recargar = function () {
    var cmbPericodi = document.getElementById('pericodi');
    window.location.href = controler + "index?pericodi=" + cmbPericodi.value;
}
limpiar = function () {
    $('#pegrcodi').val("");
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
        error: function (err) {
            console.log(err);
            alert("Lo sentimos, ha ocurrido un error inesperado");
        }
    });
}

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
            carga = result.procesoCorrecto;
            configurarExcelWeb(result);
            $('#numregistros').val(result.NumRegistros);
            //console.log($('#numregistros').val());
            //ASSETEC 20190219
            //console.log(result.Pegrcodi);
            $('#pegrcodi').val(result.Pegrcodi);
            //console.log($('#pegrcodi').val());
            var bGrabar = result.bGrabar;
            if (!bGrabar) {
                $('#btnSelecionarExcel').css('display', 'block');
                $('#btnValidarGrillaExcel').css('display', 'block');
                $('#btnGrabarExcel').css('display', 'block');
                $('#btnEliminarDatos').css('display', 'block');
                $('#btnVerValidaciones').css('display', 'block');
            }
            else {
                $('#btnSelecionarExcel').css('display', 'none');
                $('#btnValidarGrillaExcel').css('display', 'none');
                $('#btnGrabarExcel').css('display', 'none');
                $('#btnEliminarDatos').css('display', 'none');
                //$('#btnVerValidaciones').css('display', 'none');
            }
            $('#divAcciones').css('display', 'block');
            mostrarExito('Código del envío: ' + $('#pegrcodi').val() + ", Fecha de envío: " + $('#pegrfeccreacion').val() + " - Número de registros cargados: " + $('#numregistros').val());
        },
        error: function () {
            mostrarError('Lo sentimos no se puede mostrar la hoja de cálculo')
        }
    });
}

validarNegativos = function () {
    valida = false;
    for (let x = 1; x < cantidadRows; x++) {
        if ((parseInt(instanciaTabla.getData()[x][5]) < 0 || parseInt(instanciaTabla.getData()[x][6]) < 0 ||
            parseInt(instanciaTabla.getData()[x][7]) < 0 || parseInt(instanciaTabla.getData()[x][8]) < 0 ||
            parseInt(instanciaTabla.getData()[x][9]) < 0)) {
            valida = true;
            break;
        }
    }
    return valida;
}

configurarExcelWeb = function (result, option = 0) {
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
    cantidadRows = result.NumRegistros;
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
        //mergeCells: [{ row: 0, col: 0, rowspan: 2, colspan: 1 },
        //{ row: 0, col: 1, rowspan: 2, colspan: 1 },
        //{ row: 0, col: 2, rowspan: 2, colspan: 1 },
        //{ row: 0, col: 3, rowspan: 2, colspan: 1 },
        //{ row: 0, col: 4, rowspan: 1, colspan: 2 },
        //{ row: 0, col: 6, rowspan: 1, colspan: 3 },
        //{ row: 0, col: 9, rowspan: 1, colspan: 3 },
        //{ row: 0, col: 12, rowspan: 2, colspan: 1 }],
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
                carga = true;

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
                else if (numColumna == '5') {
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
                            let index = errorPrecioPotenciaArray.indexOf(numFila.toString() + numColumna.toString());
                            if (index == -1) {
                                errorPrecioPotenciaArray.push(numFila.toString() + numColumna.toString());
                            }
                        }
                        let index = errorPrecioPotenciaArray.indexOf(numFila.toString() + numColumna.toString());
                        if (index > -1) {
                            errorPrecioPotenciaArray.splice(index, 1);
                        }
                    } else {
                        if (hot.getData()[numFila][1] != null) {
                            let index = errorPrecioPotenciaArray.indexOf(numFila.toString() + numColumna.toString());
                            if (index == -1) {
                                errorPrecioPotenciaArray.push(numFila.toString() + numColumna.toString());
                            }
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
                else if (numColumna == '6') {
                    //Valida si el nuevo valor de la celda se encuentra en Potencia Egreso
                    lista = null;
                    if (valorAnterior != null) valorAnterior = valorAnterior.toString();
                    if (nuevoValor != null) nuevoValor = nuevoValor.toString();
                    bEsNumero = sRegExVal.test(nuevoValor);
                    header = "Potencia Egreso";
                    sMensaje = "Valor de la Potencia Egreso no es válido";
                    if (bEsNumero) {   //Validamos la condición: la potencia egreso no sean superiores a 400,000.
                        if (potenciaMaxima < nuevoValor) {
                            bEsNumero = false;
                            sMensaje = "El valor " + nuevoValor + " de la Potencia Egreso no debe ser superior a 400,000";
                        }
                        if (nuevoValor == "0" || nuevoValor == 0) {
                            let index = errorPotenciaCoincidenteArray.indexOf(numFila.toString() + numColumna.toString());
                            if (index == -1) {
                                errorPotenciaCoincidenteArray.push(numFila.toString() + numColumna.toString());
                            }
                        } else {
                            let index = errorPotenciaCoincidenteArray.indexOf(numFila.toString() + numColumna.toString());
                            if (index > -1) {
                                errorPotenciaCoincidenteArray.splice(index, 1);
                            }
                        }
                    } else {
                        if (hot.getData()[numFila][1] != null) {
                            let index = errorPotenciaCoincidenteArray.indexOf(numFila.toString() + numColumna.toString());
                            if (index == -1) {
                                errorPotenciaCoincidenteArray.push(numFila.toString() + numColumna.toString());
                            }
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
                else if (numColumna == '7') {
                    //Valida si el nuevo valor de la celda se encuentra en Potencia Declarada
                    lista = null;
                    if (valorAnterior != null) valorAnterior = valorAnterior.toString();
                    if (nuevoValor != null) nuevoValor = nuevoValor.toString();
                    bEsNumero = sRegExVal.test(nuevoValor);
                    header = "Potencia Declarada";
                    sMensaje = "Valor de la Potencia Declarada no es válido";
                    if (bEsNumero) {   //Validamos la condición: la potencia egreso no sean superiores a 400,000.
                        if (potenciaMaxima < nuevoValor) {
                            //console.log(nuevoValor);
                            //console.log(potenciaMaxima);
                            bEsNumero = false;
                            sMensaje = "El valor " + nuevoValor + " de la Potencia Egreso no debe ser superior a 400,000";
                        }
                        let index = errorPotenciaCoincidenteArray.indexOf(numFila.toString() + numColumna.toString());
                        if (index > -1) {
                            errorPotenciaCoincidenteArray.splice(index, 1);
                        }
                    } else {
                        if (hot.getData()[numFila][1] != null) {
                            let index = errorPotenciaCoincidenteArray.indexOf(numFila.toString() + numColumna.toString());
                            if (index == -1) {
                                errorPotenciaCoincidenteArray.push(numFila.toString() + numColumna.toString());
                            }
                        }
                    }
                }
                else if (numColumna == '8') {
                    //Valida si el nuevo valor de la celda se encuentra en Peaje unitario
                    lista = null;
                    if (valorAnterior != null) valorAnterior = valorAnterior.toString();
                    if (nuevoValor != null) nuevoValor = nuevoValor.toString();
                    bEsNumero = sRegExVal.test(nuevoValor);
                    header = "Peaje Unitario";
                    sMensaje = "Valor del Peaje Unitario no es válido";
                    if (bEsNumero) {   //Validamos la condición: la potencia egreso no sean superiores a 400,000.
                        if (potenciaMaxima < nuevoValor) {
                            //console.log(nuevoValor);
                            //console.log(potenciaMaxima);
                            bEsNumero = false;
                            sMensaje = "El valor " + nuevoValor + " de la Potencia Egreso no debe ser superior a 400,000";
                        }
                        let index = errorPeajeUnitarioArray.indexOf(numFila.toString() + numColumna.toString());
                        if (index > -1) {
                            errorPeajeUnitarioArray.splice(index, 1);
                        }
                    } else {
                        if (hot.getData()[numFila][1] != null) {
                            let index = errorPeajeUnitarioArray.indexOf(numFila.toString() + numColumna.toString());
                            if (index == -1) {
                                errorPeajeUnitarioArray.push(numFila.toString() + numColumna.toString());
                            }
                        }
                    }
                }
                else if (numColumna == '9') {
                    //Valida si el nuevo valor de la celda se encuentra en listaBarras 
                    lista = null;
                    bEsNumero = true;
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
            if (row == 0) {
                cellProperties.renderer = firstRowRendererCabecera;
            }
            else {
                cellProperties.renderer = cellRowRendererValues;
                //if (option == 1) {
                //}
            }
            //if (row == 0 || row == 1) {
            //    cellProperties.renderer = firstRowRendererCabecera;
            //}
            //else if (col == 0 || col == 1 || col == 2 || col == 3 || col == 12) {
            //    cellProperties.renderer = firstRowRendererCeleste;
            //}
            //else if (col == 9 || col == 10 || col == 11) {
            //    cellProperties.renderer = firstRowRendererAmarillo;
            //}
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

firstRowRendererCabecera = function (instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);

    if (col == 5 || col == 6) {
        td.style.color = 'white';
        td.style.background = '#BBDF8D';
        td.style.fontWeight = 'bold';
        console.log('value ', value);
    } else if (col == 7 || col == 8) {
        td.style.color = 'white';
        td.style.background = "#EA8A1C";
        td.style.fontWeight = 'bold';
        console.log('value ', value);
    } else {
        td.style.color = 'white';
        td.style.background = '#3D8AB8';
        td.style.fontWeight = 'bold';
        console.log('value ', value);
    }
    cellProperties.className = "htCenter",
        cellProperties.readOnly = true;
    //console.log(cellProperties);
}

cellRowRendererValues = function (instance, td, row, col, prop, value, cellProperties) {
    instanciaTabla = instance;
    Handsontable.renderers.TextRenderer.apply(this, arguments);
    let valida = false;
    if (row > cantidadRows) {
        return;
    }

    //if ((row <= cantidadRows) && (col == 0 || col == 1 || col == 2 || col == 3 || col == 4))
    if ((row <= cantidadRows) && (col == 0 || col == 1 || col == 2)) {
        td.style.background = "#EDF5FC";
    }

    if (col == 1) {
        if (value == "") {
            return;
        }
    }

    if (col == 7 || col == 8) {
        if (instance.getData()[row][3] === "AUTOCONSUMO") {
            cellProperties.readOnly = true;
            let index = errorPeajeUnitarioArray.indexOf(row.toString() + col.toString());
            if (index > -1) {
                errorPeajeUnitarioArray.splice(index, 1);
            }
            let index2 = errorPotenciaCoincidenteArray.indexOf(row.toString() + col.toString());
            if (index2 > -1) {
                errorPotenciaCoincidenteArray.splice(index2, 1);
            }
            td.style.background = "yellow";
            valida = true;
        } else {
            valida = false;
        }
    }

    if (col == 5 || col == 8) {
        if (value <= 0 || value == "") {
            td.style.color = 'white';
            td.style.background = "#EA8A1C";
            td.style.fontWeight = 'bold';
            if (col == 5) {
                errorPrecioPotenciaArray.push(row.toString() + col.toString());
            } else {
                if (!valida) {
                    errorPeajeUnitarioArray.push(row.toString() + col.toString());
                } else {
                    td.style.background = "white";
                }
            }
        }
    } else if (col == 6) {
        if (value == "" || value <= 0) {
            td.style.color = 'white';
            td.style.background = "#FF0000";
            td.style.fontWeight = 'bold';
            errorPotenciaCoincidenteArray.push(row.toString() + col.toString());
        }
    } else if (col == 7) {
        if ((value == "" || value <= 0) && !valida) {
            td.style.color = 'white';
            td.style.background = "#FF0000";
            td.style.fontWeight = 'bold';
            errorPotenciaCoincidenteArray.push(row.toString() + col.toString());
        }
    }

    if (col == 7 || col == 8) {
        if (instance.getData()[row][3] === "AUTOCONSUMO") {
            td.style.background = "yellow";
        }
    }


    errorPrecioPotenciaArray = errorPrecioPotenciaArray.filter((item, index) => {
        return errorPrecioPotenciaArray.indexOf(item) === index;
    });
    errorPeajeUnitarioArray = errorPeajeUnitarioArray.filter((item, index) => {
        return errorPeajeUnitarioArray.indexOf(item) === index;
    });
    errorPotenciaCoincidenteArray = errorPotenciaCoincidenteArray.filter((item, index) => {
        return errorPotenciaCoincidenteArray.indexOf(item) === index;
    });
    //console.log(cellProperties);
}

descargarArchivo = function (formato) {
    $.ajax({
        type: 'POST',
        url: controler + 'exportardata',
        data: { pericodi: $('#pericodi').val(), recpotcodi: $('#recpotcodi').val(), pegrcodi: $('#pegrcodi').val(), emprcodi: $('#emprcodi').val(), formato: formato },
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

descargarArchivoValidaciones = function (formato) {
    $.ajax({
        type: 'POST',
        url: controler + 'exportardatavalidaciones',
        data: { pericodi: $('#pericodi').val(), recpotcodi: $('#recpotcodi').val(), pegrcodi: $('#pegrcodi').val(), emprcodi: $('#emprcodi').val(), formato: formato },
        dataType: 'json',
        success: function (result) {
            Fn_Corregir_Errores();
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


Fn_Corregir_Errores = function () {
    $(".b-close").click();
};


abrirPopup = function () {
    var html = '<span class="button b-close"><span>X</span></span>';
    html += '<p style="border-bottom: 1px solid #6ba9d0;"><b style="color: #6ba9d0;font-size: 16px;">Confirmación de envio de información<div style="height: 5px;"></div></b><p>';
    html += '<table border="0" class="pretty tabla-icono" id="tabla">'
    html += '<thead>'
    if ((errorPrecioPotenciaArray.length > 0 || errorPeajeUnitarioArray.length > 0) || !carga) {
        html += '<tr style="height: 25px !important;">'
        html += '<th style="background: #EA8A1C; border: none;width: 80px;"></th>'
        html += '<th style="background: white; border: none;"></th>'
        html += '<th style="background: white; border: none; color: black;text-align: left;">Se están reportando datos vacíos, negativos o con 0 para el precio de potencia o peaje unitario</th>'
        html += '</tr>'
    }
    html += '<tr style="height: 15px !important;">'
    html += '</tr>'
    if (errorPotenciaCoincidenteArray.length > 0 || !carga) {
        html += '<tr style="height: 25px !important;">'
        html += '<th style="background: #FF0000; border: none;width: 80px;"></th>'
        html += '<th style="background: white; border: none;"></th>'
        html += '<th style="background: white; border: none; color: black;text-align: left;">Se están reportando valores con cero o negativos para la potencia coincidente o potencia declarada.</th>'
        html += '</tr>'
    }

    html += '</thead>'
    html += '<tbody>'
    html += '</tbody>'
    html += '</table>'

    if (withButtons) {
        html += '<table class="table-search" style="width:100%">'
        html += '<tr>'
        html += '<td style="width:90px; height:40px; text-align:right;">'
        html += '<input type="button" id="btnEnviarConErrores" onclick="fnSaveGrillaErrores();" value="Confirmar envío" />'
        html += '</td>'
        html += '<td style="width:90px; height:40px;">'
        html += '<input type="button" id="btnCorregirErrores" onclick="Fn_Corregir_Errores();" value="Corregir datos" />'
        html += '</td>'
        html += '</tr>'
        html += '</table>'
    }

    $('#popup2').html(html);
    $('#popup2').bPopup({
        easing: 'easeOutBack',
        speed: 450,
        transition: 'slideDown'
    });
    //mostrarError("Lo sentimos, la hoja del cálculo tiene errores");
}

mostrarErrorHtml = function (mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-error');
}

fnSaveGrillaErrores = function () {
    if (!validarNegativos()) {
        $(".b-close").click();
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        grabarExcel();
    } else {
        mostrarErrorAlert("No se permite el envío porque existen datos en negativo, revise por favor.");
    }
}