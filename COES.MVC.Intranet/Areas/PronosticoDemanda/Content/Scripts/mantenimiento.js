var controlador = siteRoot + 'pronosticodemanda/mantenimiento/';
var error = [];

$(function () {

    $('#sAnio').Zebra_DatePicker({
        format: 'Y',
        onSelect: function () {
            refrescar()
        }
    });

    $('#btnGrabarExcel').click(function () {
        mostrarMensaje("mensaje", 'message', "Espere un momento, se esta procesando su solicitud");
        if (parseInt(error.length) > 0) {
            abrirPopup();
        } else {
            grabarExcel();
        }
    });

    var sAnio = document.getElementById('sAnio');
    var sNroSemana = document.getElementById('sNroSemana');
    if (sAnio && sNroSemana) {
        mostrarGrillaExcel();
    }
});

refrescar = function () {
    var sAnio = document.getElementById('sAnio');
    var sNroSemana = document.getElementById('sNroSemana');
    window.location.href = controlador + "index?sAnio=" + sAnio.value + "&sNroSemana=" + sNroSemana.value;
}

mostrarMensaje = function (id, tipo, mensaje) {
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);
}

/////////////////////////////////////////////HANSOMETABLE//////////////////////////////////////////////////////////

mostrarGrillaExcel = function () {
    error = [];
    if (typeof hot != 'undefined') {
        hot.destroy();
    }
    var container = document.getElementById('grillaExcel');
    var sAnio = document.getElementById('sAnio');
    var sNroSemana = document.getElementById('sNroSemana');//id-tipo
    var sTipo = document.getElementById('sTipo');//id-tipo
    $.ajax({
        type: 'POST',
        url: controlador + "grillaexcel",
        data: { sAnio: sAnio.value, sNroSemana: sNroSemana.value, sTipo: sTipo.value },
        dataType: 'json',
        success: function (result) {
            var bGrabar = result.Grabar;
            var columns = result.Columnas;
            var iNroColumnas = result.NumeroColumnas;
            var widths = result.Widths;
            var data = result.Data;
            var sRegExVal = new RegExp(/^-{0,1}\d*\.{0,1}\d+$/);
            hot = new Handsontable(container, {
                data: data,
                maxCols: result.Columnas.length,
                colHeaders: false,
                rowHeaders: true,
                colWidths: widths,
                contextMenu: true,
                rowHeaders: true,
                columns: columns,
                fixedRowsTop: result.FixedRowsTop,
                fixedColumnsLeft: result.FixedColumnsLeft,
                currentRowClassName: 'currentRow',
                mergeCells: [{ row: 0, col: 1, rowspan: 1, colspan: 2 },
                { row: 0, col: 3, rowspan: 1, colspan: 2 },
                { row: 0, col: 5, rowspan: 1, colspan: 2 },
                { row: 0, col: 7, rowspan: 1, colspan: 2 }
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
                        if ((numColumna % 2) == 1) {
                            //Valida si el nuevo valor de la celda Mantenimiento
                            lista = null;
                            if (sValorAnterior != null) sValorAnterior = sValorAnterior.toString();
                            if (sNuevoValor != null) sNuevoValor = sNuevoValor.toString();
                            bEsNumero = sRegExVal.test(sNuevoValor);
                            header = "Mantenimiento";
                            sMensaje = "Valor del Mantenimiento en " + data[0][numColumna] + " no es válido";
                        }
                        else if ((numColumna % 2) == 0) {
                            //Valida si el nuevo valor de la celda Falla
                            lista = null;
                            if (sValorAnterior != null) sValorAnterior = sValorAnterior.toString();
                            if (sNuevoValor != null) sNuevoValor = sNuevoValor.toString();
                            bEsNumero = sRegExVal.test(sNuevoValor);
                            header = "Falla";
                            sMensaje = "Valor de la Falla en " + data[0][numColumna - 1] + " no es válido";
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
                    return cellProperties;
                }
            });
            hot.render();
            if (!bGrabar) {
                $('#btnGrabarExcel').css('display', 'block');
            }
            else {
                $('#btnGrabarExcel').css('display', 'none');
            }
            var iNumRegistros = hot.countRows();
            if (iNumRegistros >= 2) iNumRegistros = iNumRegistros - 3;
            mostrarMensaje("mensaje", 'exito', 'Validar la información de la tabla y luego proceder a grabar la información');
        },
        error: function () {
            mostrarMensaje("mensaje", 'error', 'Lo sentimos no se puede mostrar la hoja de cálculo')
        }
    });
}

grabarExcel = function () {
    var sAnio = document.getElementById('sAnio');
    var sNroSemana = document.getElementById('sNroSemana');
    var sTipo = document.getElementById('sTipo');//id-tipo
    $.ajax({
        type: "POST",
        url: controlador + 'grabargrillaexcel',
        dataType: "json",
        contentType: 'application/json',
        traditional: true,
        data: JSON.stringify({
            sTipo: sTipo.value, sAnio: sAnio.value, sNroSemana: sNroSemana.value, datos: hot.getData()
        }),
        success: function (result) {
            var sError = result.sError;
            var sMensaje = result.sMensaje;
            if (sError == "") {
                //var iNumRegistros = hot.countRows();
                //if (iNumRegistros >= 2) iNumRegistros = iNumRegistros - 3;
                mostrarMensaje("mensaje", 'exito', 'Operación Exitosa - ' + sMensaje);
            }
            else {
                mostrarMensaje("mensaje", 'error', 'Lo sentimo ocurrio un error: ' + sError);
            }
        },
        error: function (response) {
            mostrarMensaje("mensaje", 'error', response.status + " " + response.statusText);
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
