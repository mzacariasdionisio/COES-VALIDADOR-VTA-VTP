var controler = siteRoot + "transfpotencia/ajustesaldos/";
var error = [];
$(document).ready(function () {
    $('#tab-container').easytabs({
        animate: false
    });
    //CargoAjuste
    $('#btnConsultarCargoAjuste').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        mostrarGrillaExcelCargoAjuste();
        mostrarExito('Ahora puede consultar la información');
    });

    $('#btnValidarGrillaExcelCargoAjuste').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        if (parseInt(error.length) > 0) {
            abrirPopup();
        } else {
            mostrarExito("La hoja de cálculo no tiene errores...!");
        }
    });

    $('#btnGrabarExcelCargoAjuste').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        if (parseInt(error.length) > 0) {
            abrirPopup();
        } else {
            grabarExcelCargoAjuste();
        }
    });

    $('#btnEliminarCargoAjuste').click(function () {
        if (confirm("¿Desea eliminar la información de la hoja de cálculo?")) {
            mostrarAlerta("Espere un momento, se esta procesando su solicitud");
            eliminarCargoAjuste();
            mostrarExito('Felicidades, se ha ejecutado correctamente su solicitud.');
        }
    });

    //PeajeAjuste
    $('#btnConsultarPeajeAjuste').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        mostrarGrillaExcelPeajeAjuste();
        mostrarExito('Ahora puede consultar la información');
    });

    $('#btnValidarGrillaExcelPeajeAjuste').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        if (parseInt(error.length) > 0) {
            abrirPopup();
        } else {
            mostrarExito("La hoja de cálculo no tiene errores...!");
        }
    });

    $('#btnGrabarExcelPeajeAjuste').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        if (parseInt(error.length) > 0) {
            abrirPopup();
        } else {
            grabarExcelPeajeAjuste();
        }
    });

    $('#btnEliminarPeajeAjuste').click(function () {
        if (confirm("¿Desea eliminar la información de la hoja de cálculo?")) {
            mostrarAlerta("Espere un momento, se esta procesando su solicitud");
            eliminarPeajeAjuste();
            mostrarExito('Felicidades, se ha ejecutado correctamente su solicitud.');
        }
    });

    //SaldoAjuste
    $('#btnConsultarSaldoAjuste').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        mostrarGrillaExcelSaldoAjuste();
        mostrarExito('Ahora puede consultar la información');
    });

    $('#btnValidarGrillaExcelSaldoAjuste').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        if (parseInt(error.length) > 0) {
            abrirPopup();
        } else {
            mostrarExito("La hoja de cálculo no tiene errores...!");
        }
    });

    $('#btnGrabarExcelSaldoAjuste').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        if (parseInt(error.length) > 0) {
            abrirPopup();
        } else {
            grabarExcelSaldoAjuste();
        }
    });

    $('#btnEliminarSaldoAjuste').click(function () {
        if (confirm("¿Desea eliminar la información de la hoja de cálculo?")) {
            mostrarAlerta("Espere un momento, se esta procesando su solicitud");
            eliminarSaldoAjuste();
            mostrarExito('Felicidades, se ha ejecutado correctamente su solicitud.');
        }
    });

    //IngresoAjuste
    $('#btnConsultarIngresoAjuste').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        mostrarGrillaExcelIngresoAjuste();
        mostrarExito('Ahora puede consultar la información');
    });

    $('#btnValidarGrillaExcelIngresoAjuste').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        if (parseInt(error.length) > 0) {
            abrirPopup();
        } else {
            mostrarExito("La hoja de cálculo no tiene errores...!");
        }
    });

    $('#btnGrabarExcelIngresoAjuste').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        if (parseInt(error.length) > 0) {
            abrirPopup();
        } else {
            grabarExcelIngresoAjuste();
        }
    });

    $('#btnEliminarIngresoAjuste').click(function () {
        if (confirm("¿Desea eliminar la información de la hoja de cálculo?")) {
            mostrarAlerta("Espere un momento, se esta procesando su solicitud");
            eliminarIngresoAjuste();
            mostrarExito('Felicidades, se ha ejecutado correctamente su solicitud.');
        }
    });

    // maps function to lookup string
    Handsontable.renderers.registerRenderer('negativeValueRenderer', negativeValueRenderer);
});

//PeajeAjuste - Peajes

RecargarPeajeAjuste = function () {
    var cmbPericodi = document.getElementById('pericodiPeajeAjuste');
    window.location.href = controler + "index?pericodi=" + cmbPericodi.value;
}

mostrarGrillaExcelPeajeAjuste = function () {
    error = [];
    if (typeof hot != 'undefined') {
        hot.destroy();
    }
    var container = document.getElementById('grillaExcelPeajeAjuste');
    $.ajax({
        type: 'POST',
        url: controler + "grillaexcelpeajeajuste",
        data: { pericodi: $('#pericodiPeajeAjuste').val() },
        dataType: 'json',
        success: function (result) {
            var listaEmpresas = result.ListaEmpresas;
            var listaPeajeIngreso = result.ListaPeajeIngreso;

            var columns = result.Columnas;
            var widths = result.Widths;
            var sRegExVal = new RegExp(/^-{0,1}\d*\.{0,1}\d+$/); //^\d+(?:[\.\,]\d+)?$
            hot = new Handsontable(container, {
                data: result.Data,
                maxCols: result.Columnas.length,
                colHeaders: false,
                rowHeaders: true,
                colWidths: widths,
                contextMenu: true,
                minSpareRows: 1,
                columns: columns,
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
                        var sMensajeAux;
                        if (numColumna == '0') {
                            //Valida si el nuevo valor de la celda se encuentra en listaEmpresas 
                            lista = listaEmpresas;
                            bEsNumero = null;
                            header = "Empresa generadora";
                            sMensaje = "Nombre del Empresa generadora es incorrecto";
                        }
                        else if (numColumna == '2') {
                            //Valida si el nuevo valor de la celda se encuentra en listaPeajeIngreso 
                            lista = listaPeajeIngreso;
                            bEsNumero = null;
                            header = "Sistema de Transmisión";
                            sMensaje = "Nombre del Sistema de Transmisión es incorrecto";
                        }
                            //else if (numColumna == '2') {
                            //    //Valida si el nuevo valor de la celda se encuentra en listaEmpresas 
                            //    lista = listaEmpresas;
                            //    bEsNumero = null;
                            //    header = "Empresa Cargo";
                            //    sMensaje = "Nombre del Empresa cargo es incorrecto";
                            //}
                        else if (numColumna == '3') {
                            //Valida si el nuevo valor de la celda se encuentra en Pecajajuste
                            lista = null;
                            if (sValorAnterior != null) sValorAnterior = sValorAnterior.toString();
                            if (sNuevoValor != null) sNuevoValor = sNuevoValor.toString();
                            bEsNumero = sRegExVal.test(sNuevoValor);
                            header = "Saldo Peaje";
                            sMensaje = "Valor del Saldo Peaje no es válido";
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
                    if (row == 0) {
                        cellProperties.renderer = firstRowRendererCabecera;
                    }
                    else if (col == 0 || col == 1 || col == 2) {
                        cellProperties.renderer = firstRowRendererCeleste;
                    }
                    else if (col >= 3) {
                        //Para el llenado de datos
                        cellProperties.renderer = "negativeValueRenderer";
                    }
                    return cellProperties;
                },
            });
            hot.render();
            $('#divAccionesPeajeAjuste').css('display', 'block');
        },
        error: function () {
            mostrarError('Lo sentimos no se puede mostrar la hoja de cálculo')
        }
    });
}

grabarExcelPeajeAjuste = function () {
    $.ajax({
        type: "POST",
        url: controler + 'grabargrillaexcelpeajeajuste',
        dataType: "json",
        contentType: 'application/json',
        traditional: true,
        data: JSON.stringify({
            pericodi: $('#pericodiPeajeAjuste').val(), datos: hot.getData()
        }),
        success: function (result) {
            var sError = result.sError;
            var sMensaje = result.sMensaje;
            if (sError == "") {
                mostrarExito('Operación Exitosa - ' + sMensaje);
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

eliminarPeajeAjuste = function () {
    $.ajax({
        type: 'POST',
        url: controler + 'eliminarpeajeajuste',
        data: { pericodi: $('#pericodiPeajeAjuste').val() },
        dataType: 'json',
        success: function (result) {
            if (result == "1") {
                alert("Felicidades, la información se elimino correctamente...!");
                mostrarGrillaExcelPeajeAjuste();
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

//IngresoAjuste - Ingreso tarifario

RecargarIngresoAjuste = function () {
    var cmbPericodi = document.getElementById('pericodiIngresoAjuste');
    window.location.href = controler + "index?pericodi=" + cmbPericodi.value + "#paso2";
}

mostrarGrillaExcelIngresoAjuste = function () {
    error = [];
    if (typeof hot != 'undefined') {
        hot.destroy();
    }
    var container = document.getElementById('grillaExcelIngresoAjuste');
    $.ajax({
        type: 'POST',
        url: controler + "grillaexcelingresoajuste",
        data: { pericodi: $('#pericodiIngresoAjuste').val() },
        dataType: 'json',
        success: function (result) {
            var listaEmpresas = result.ListaEmpresas;
            var listaPeajeIngreso = result.ListaPeajeIngreso;

            var columns = result.Columnas;
            var widths = result.Widths;
            var sRegExVal = new RegExp(/^-{0,1}\d*\.{0,1}\d+$/); //^\d+(?:[\.\,]\d+)?$
            hot = new Handsontable(container, {
                data: result.Data,
                maxCols: result.Columnas.length,
                colHeaders: false,
                rowHeaders: true,
                colWidths: widths,
                contextMenu: true,
                minSpareRows: 1,
                columns: columns,
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
                        var sMensajeAux;
                        if (numColumna == '0') {
                            //Valida si el nuevo valor de la celda se encuentra en listaEmpresas 
                            lista = listaEmpresas;
                            bEsNumero = null;
                            header = "Empresa generadora";
                            sMensaje = "Nombre del Empresa generadora es incorrecto";
                        }
                        else if (numColumna == '2') {
                            //Valida si el nuevo valor de la celda se encuentra en listaPeajeIngreso 
                            lista = listaPeajeIngreso;
                            bEsNumero = null;
                            header = "Sistema de transmisión";
                            sMensaje = "Nombre del Sistema de transmisión es incorrecto";
                        }
                        else if (numColumna == '3') {
                            //Valida si el nuevo valor de la celda se encuentra en Pecajajuste
                            lista = null;
                            if (sValorAnterior != null) sValorAnterior = sValorAnterior.toString();
                            if (sNuevoValor != null) sNuevoValor = sNuevoValor.toString();
                            bEsNumero = sRegExVal.test(sNuevoValor);
                            header = "Saldo IT";
                            sMensaje = "Valor del Saldo IT no es válido";
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
                    if (row == 0) {
                        cellProperties.renderer = firstRowRendererCabecera;
                    }
                    else if (col == 0 || col == 1 || col == 2) {
                        cellProperties.renderer = firstRowRendererCeleste;
                    }
                    else if (col >= 3) {
                        //Para el llenado de datos
                        cellProperties.renderer = "negativeValueRenderer";
                    }
                    return cellProperties;
                },
            });
            hot.render();
            $('#divAccionesIngresoAjuste').css('display', 'block');
        },
        error: function () {
            mostrarError('Lo sentimos no se puede mostrar la hoja de cálculo')
        }
    });
}

grabarExcelIngresoAjuste = function () {
    $.ajax({
        type: "POST",
        url: controler + 'grabargrillaexcelingresoajuste',
        dataType: "json",
        contentType: 'application/json',
        traditional: true,
        data: JSON.stringify({
            pericodi: $('#pericodiIngresoAjuste').val(), datos: hot.getData()
        }),
        success: function (result) {
            var sError = result.sError;
            var sMensaje = result.sMensaje;
            if (sError == "") {
                mostrarExito('Operación Exitosa - ' + sMensaje);
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

eliminarIngresoAjuste = function () {
    $.ajax({
        type: 'POST',
        url: controler + 'eliminaringresoajuste',
        data: { pericodi: $('#pericodiIngresoAjuste').val() },
        dataType: 'json',
        success: function (result) {
            if (result == "1") {
                alert("Felicidades, la información se elimino correctamente...!");
                mostrarGrillaExcelIngresoAjuste();
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

//CargoAjuste - 

RecargarCargoAjuste = function () {
    var cmbPericodi = document.getElementById('pericodiCargoAjuste');
    window.location.href = controler + "index?pericodi=" + cmbPericodi.value + "#paso3";
}

mostrarGrillaExcelCargoAjuste = function () {
    error = [];
    if (typeof hot != 'undefined') {
        hot.destroy();
    }
    var container = document.getElementById('grillaExcelCargoAjuste');
    $.ajax({
        type: 'POST',
        url: controler + "grillaexcelcargoajuste",
        data: { pericodi: $('#pericodiCargoAjuste').val() },
        dataType: 'json',
        success: function (result) {
            var listaEmpresas = result.ListaEmpresas;
            var listaPeajeIngreso = result.ListaPeajeIngreso;

            var columns = result.Columnas;
            var widths = result.Widths;
            var sRegExVal = new RegExp(/^-{0,1}\d*\.{0,1}\d+$/);
            hot = new Handsontable(container, {
                data: result.Data,
                maxCols: result.Columnas.length,
                colHeaders: false,
                rowHeaders: true,
                colWidths: widths,
                contextMenu: true,
                minSpareRows: 1,
                columns: columns,
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
                        var sMensajeAux;
                        if (numColumna == '0') {
                            //Valida si el nuevo valor de la celda se encuentra en listaEmpresas 
                            lista = listaEmpresas;
                            bEsNumero = null;
                            header = "Empresa Generadora";
                            sMensaje = "Nombre del Empresa es incorrecto";
                        }
                        else if (numColumna == '1') {
                            //Valida si el nuevo valor de la celda se encuentra en listaPeajeIngreso 
                            lista = listaPeajeIngreso;
                            bEsNumero = null;
                            header = "Cargo";
                            sMensaje = "Nombre del Cargo es incorrecto";
                        }
                        else if (numColumna == '2') {
                            //Valida si el nuevo valor de la celda se encuentra en Pecajajuste
                            lista = null;
                            if (sValorAnterior != null) sValorAnterior = sValorAnterior.toString();
                            if (sNuevoValor != null) sNuevoValor = sNuevoValor.toString();
                            bEsNumero = sRegExVal.test(sNuevoValor);
                            header = "Ajuste Saldo";
                            sMensaje = "Valor del Ajuste Saldo no es válido";
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
                    if (row == 0) {
                        cellProperties.renderer = firstRowRendererCabecera;
                    }
                    else if (col == 0 || col == 1) {
                        cellProperties.renderer = firstRowRendererCeleste;
                    }
                    else if (col >= 2) {
                        //Para el llenado de datos
                        cellProperties.renderer = "negativeValueRenderer";
                    }
                    return cellProperties;
                },
            });
            hot.render();
            $('#divAccionesCargoAjuste').css('display', 'block');
        },
        error: function () {
            mostrarError('Lo sentimos no se puede mostrar la hoja de cálculo')
        }
    });
}

grabarExcelCargoAjuste = function () {
    $.ajax({
        type: "POST",
        url: controler + 'grabargrillaexcelcargoajuste',
        dataType: "json",
        contentType: 'application/json',
        traditional: true,
        data: JSON.stringify({
            pericodi: $('#pericodiCargoAjuste').val(), datos: hot.getData()
        }),
        success: function (result) {
            var sError = result.sError;
            var sMensaje = result.sMensaje;
            if (sError == "") {
                mostrarExito('Operación Exitosa - ' + sMensaje);
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

eliminarCargoAjuste = function () {
    $.ajax({
        type: 'POST',
        url: controler + 'eliminarcargoajuste',
        data: { pericodi: $('#pericodiCargoAjuste').val() },
        dataType: 'json',
        success: function (result) {
            if (result == "1") {
                alert("Felicidades, la información se elimino correctamente...!");
                mostrarGrillaExcelCargoAjuste();
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

//SaldoAjuste - Saldo VTP

RecargarSaldoAjuste = function () {
    var cmbPericodi = document.getElementById('pericodiSaldoAjuste');
    window.location.href = controler + "index?pericodi=" + cmbPericodi.value + "#paso4";
}

mostrarGrillaExcelSaldoAjuste = function () {
    error = [];
    if (typeof hot != 'undefined') {
        hot.destroy();
    }
    var container = document.getElementById('grillaExcelSaldoAjuste');
    $.ajax({
        type: 'POST',
        url: controler + "grillaexcelsaldoajuste",
        data: { pericodi: $('#pericodiSaldoAjuste').val() },
        dataType: 'json',
        success: function (result) {
            var listaEmpresas = result.ListaEmpresas;

            var columns = result.Columnas;
            var widths = result.Widths;
            var sRegExVal = new RegExp(/^-{0,1}\d*\.{0,1}\d+$/); //^\d+(?:[\.\,]\d+)?$
            hot = new Handsontable(container, {
                data: result.Data,
                maxCols: result.Columnas.length,
                colHeaders: false,
                rowHeaders: true,
                colWidths: widths,
                contextMenu: true,
                minSpareRows: 1,
                columns: columns,
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
                        var sMensajeAux;
                        if (numColumna == '0') {
                            //Valida si el nuevo valor de la celda se encuentra en listaEmpresas 
                            lista = listaEmpresas;
                            bEsNumero = null;
                            header = "Empresa";
                            sMensaje = "Nombre de la Empresa generadora es incorrecto";
                        }
                        else if (numColumna == '1') {
                            //Valida si el nuevo valor de la celda se encuentra en POTSEAajuste
                            lista = null;
                            if (sValorAnterior != null) sValorAnterior = sValorAnterior.toString();
                            if (sNuevoValor != null) sNuevoValor = sNuevoValor.toString();
                            bEsNumero = sRegExVal.test(sNuevoValor);
                            header = "Saldo";
                            sMensaje = "Valor del Saldo no es válido";
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
                    if (row == 0) {
                        cellProperties.renderer = firstRowRendererCabecera;
                    }
                    else if (col == 0) {
                        cellProperties.renderer = firstRowRendererCeleste;
                    }
                    else if (col >= 1) {
                        //Para el llenado de datos
                        cellProperties.renderer = "negativeValueRenderer";
                    }
                    return cellProperties;
                },
            });
            hot.render();
            $('#divAccionesSaldoAjuste').css('display', 'block');
        },
        error: function () {
            mostrarError('Lo sentimos no se puede mostrar la hoja de cálculo')
        }
    });
}

grabarExcelSaldoAjuste = function () {
    $.ajax({
        type: "POST",
        url: controler + 'grabargrillaexcelsaldoajuste',
        dataType: "json",
        contentType: 'application/json',
        traditional: true,
        data: JSON.stringify({
            pericodi: $('#pericodiSaldoAjuste').val(), datos: hot.getData()
        }),
        success: function (result) {
            var sError = result.sError;
            var sMensaje = result.sMensaje;
            if (sError == "") {
                mostrarExito('Operación Exitosa - ' + sMensaje);
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

eliminarSaldoAjuste = function () {
    $.ajax({
        type: 'POST',
        url: controler + 'eliminarsaldoajuste',
        data: { pericodi: $('#pericodiSaldoAjuste').val() },
        dataType: 'json',
        success: function (result) {
            if (result == "1") {
                alert("Felicidades, la información se elimino correctamente...!");
                mostrarGrillaExcelSaldoAjuste();
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