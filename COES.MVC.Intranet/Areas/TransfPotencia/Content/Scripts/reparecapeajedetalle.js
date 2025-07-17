var controler = siteRoot + "transfpotencia/reparecapeajedetalle/";
var hot;
var MatrizError = [];
var Noerror = true;
var error = [];
$(function () {
    mostrarGrillaExcel();

    $('.rbMensaje').click(function () {
        mostrarMensajes();
    });

    $('#btnDescargarExcel').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        descargarArchivo(1);
    });

    $('#btnEliminarDatos').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        eliminarDatos();
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
        }
        else if (Noerror == true) {
            grabarExcel(); 
        } else {
            mostrarError("Ingrese valor correcto para que sume 100");
        }
    });

    $('#btnAgregar').click(function () {
        var cantidad = $('#Cantidad').val();
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        mostrarGrillaExcel(cantidad);
        mostrarExito('Puede consultar y modificar la información');
    });

    // maps function to lookup string
    Handsontable.renderers.registerRenderer('negativeValueRenderer', negativeValueRenderer);
});

mostrarGrillaExcel = function (cantidad) {
    if (typeof hot != 'undefined') {
        hot.destroy();
    }
    var container = document.getElementById('grillaExcel');
    $.ajax({
        type: 'POST',
        url: controler + "GrillaExcel",
        data: { pericodi: $('#EntidadRecalculoPotencia_Pericodi').val(), recpotcodi: $('#EntidadRecalculoPotencia_Recpotcodi').val(), cantidad: cantidad },       
        dataType: 'json',
        success: function (result) {
            var cantidadEmpresas = result.CantidadEmpresas;
            var listaEmpresas = result.ListaEmpresas;
            var columns = result.Columnas;
            var headers = result.Headers;
            var widths = result.Widths;
            var sRegExVal = new RegExp(/^\d+(?:[\.\,]\d+)?$/); //Validacion solo numero con decimales sin comillas 
            // para numero enteros: RegExp("^(?:100|[1-9]?[0-9])$");
            columns[1].renderer = customColumn;
            hot = new Handsontable(container, {
                data: result.Data,          
                maxRows: result.Data.length,
                maxCols: result.Columnas.length,
                colHeaders: headers,
                colWidths: widths,
                columnSorting: false,
                contextMenu: false,
                minSpareRows: 1,
                minSpareCols: 1,
                fixedRowsTop: result.FixedRowsTop,
                fixedColumnsLeft: result.FixedColumnsLeft,
                columns: columns,
                currentRowClassName: 'currentRow',
                beforeChange: function (changes, source) {
                    //changes[i][0] = fila
                    //changes[i][1] = columna
                    //changes[i][2] = valor anteriror
                    //changes[i][3] = nuevo valor    
                    for (var i = changes.length - 1; i >= 0; i--) {
                        var numFila = changes[i][0];
                        var numColumna = parseInt(changes[i][1]);
                        var sValorAnterior = changes[i][2];
                        var sNuevoValor = changes[i][3];
                        var lista;
                        var bEsNumero;
                        var header;
                        var sMensaje;

                        if ((numColumna >= 2) && (numColumna % 2 == 0)) {
                            //Valida si el nuevo valor de la celda se encuentra en listaEmpresas 
                            lista = listaEmpresas;
                            bEsNumero = null;
                            header = "Empresa " + (numColumna / 2).toString();
                            sMensaje = "Nombre de Empresa incorrecto";
                        }
                        else if ((numColumna >= 2) && (numColumna % 2 == 1)) {
                            //Valida si el nuevo valor de la celda se encuentra en Potencia Remunerable
                            lista = null;
                            if (sValorAnterior != null) sValorAnterior = sValorAnterior.toString();
                            if (sNuevoValor != null) sNuevoValor = sNuevoValor.toString();
                            bEsNumero = sRegExVal.test(sNuevoValor);
                            header = "[%] " + (((numColumna-1) / 2) + 1).toString();
                            sMensaje = "Valor del Porcentaje no es válido";
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
                    if (col == 0) {
                        cellProperties.renderer = firstRowRendererCeleste;
                    }
                    else if ((col > 1) && !(col % 2)) {
                        cellProperties.renderer = firstRowRendererAmarillo;
                    }
                    else if ((col > 2) && (col % 2)) {
                        //
                        cellProperties.renderer = "negativeValueRenderer";
                    }
                    return cellProperties;
                }
            });
            hot.render();
            //Para la validacion de la columna total
            function customColumn(instance, TD, row, col, prop, value, cellProperties) {
                Handsontable.NumericCell.renderer.apply(this, arguments);
                var total = 0;
                var Noerror = false;

                if (col == 0 || col == 1) {
                    Noerror = true;
                }
                for (var i = 2, l = columns.length; i < l; i++) {
                    if (i % 2 != 0) {
                        //Para la columnas de porcentaje
                        if (sRegExVal.test((instance.getDataAtCell(row, i)))) {
                            total = total + parseFloat(instance.getDataAtCell(row, i));
                            Noerror = Noerror & true;
                        }
                        else if ((instance.getDataAtCell(row, i) == null || instance.getDataAtCell(row, i) == "" || instance.getDataAtCell(row, i) == undefined)) {
                            Noerror = Noerror & true;
                            continue;
                        }
                        else {
                            Noerror = Noerror & false;
                        }
                    }
                }
                total = total.toFixed(2);
                console.log(total);
                if (Noerror == false) {
                    cellProperties.readOnly = true;
                    cellProperties.strict = true;
                    TD.style.color = 'red';
                    TD.style.background = '#EEE';
                    TD.innerHTML = total;
                } else {
                    if (total != 100 && total != 0) {
                        Noerror = false
                        TD.style.color = 'red';
                        TD.style.background = '#EEE';
                        TD.innerHTML = parseFloat(total);
                    }
                    else {
                        TD.style.background = '#EEE';
                        TD.innerHTML = parseFloat(total);
                    }
                }
                cellProperties.readOnly = true;
                cellProperties.strict = true;
                cellProperties.className = "htRight";
            }
            $('#divAcciones').css('display', 'block');
        },
        error: function () {
            mostrarError("Lo sentimos, se ha producido un error...")
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

eliminarDatos = function () {
    $.ajax({
        type: 'POST',
        url: controler + 'eliminardatos',
        data: { pericodi: $('#EntidadRecalculoPotencia_Pericodi').val(), recpotcodi: $('#EntidadRecalculoPotencia_Recpotcodi').val() },
        dataType: 'json',
        success: function (result) {
            if (result == "1") {
                mostrarExito("Felicidades, la información se elimino correctamente...!");
                $('#grillaExcel').empty();
            }
            else if (result == "-1") {
                mostrarError("Lo sentimos, se ha producido un error...");
            }
        },
        error: function () {
            mostrarError("Lo sentimos, se ha producido un error");
        }
    });
    mostrarGrillaExcel();
}

grabarExcel = function () {  
    $.ajax({
        type: "POST",
        url: controler + 'grabargrillaexcel',
        dataType: "json",
        contentType: 'application/json',
        traditional: true,
        data: JSON.stringify({ pericodi:$('#EntidadRecalculoPotencia_Pericodi').val(), recpotcodi: $('#EntidadRecalculoPotencia_Recpotcodi').val(), datos: hot.getData()}),
        success: function (result) {
            //console.log(result);
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

firstRowRendererCeleste = function (instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);
    td.style.color = 'black';
    td.style.background = '#E8F6FF';
}

firstRowRendererAmarillo = function (instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);
    td.style.color = 'black';
    if (!value || value === '') {
        td.style.background = '#E6E6E6';
    }
    else {
        td.style.background = '#FFFFD7';
    }
}

negativeValueRenderer = function (instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);
    cellProperties.className = "htRight";
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