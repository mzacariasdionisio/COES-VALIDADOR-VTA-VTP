
/// Crea El objeto Handson en la pagina web
var container;
var previoChange = 0;


function crearGrillaExcel(evt) {

    var nColumnas = evt.Handson.ListaExcelData[0].length;
    listaPtos = evt.ListaHojaPto;
    
    mostrarmensajes(evt.EnPlazo, evt.IdEnvio, evt.FechaEnvio);

    function descripRowRenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontSize = '10px';
        td.style.color = 'White';
        td.style.background = '#2980B9';
        if (col > 0)
            if (parseInt(listaPtos[col - 1].Tptomedicodi) == 40) {
                td.style.background = '#E8F6FF';
                td.style.color = 'MidnightBlue';
            }
    }

    function firstRowRenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontWeight = 'bold';
        td.style.fontSize = '10px';
        td.style.color = 'White';
        td.style.background = '#2980B9';
    }

    function fechaRenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontSize = '12px';
        td.style.color = 'MidnightBlue';
        td.style.background = '#E8F6FF';
    }

    function cambiosCellRenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontSize = '12px';
        td.style.textAlign = 'right';
        td.style.color = 'black';
        td.style.background = '#FFFFD7';
        $(td).html(formatFloat(parseFloat(value), 3, '.', ','));
    }

    function readOnlyValueRenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontSize = '12px';
        td.style.textAlign = 'right';
        td.style.color = 'DimGray';
        td.style.background = 'Gainsboro';
        if (parseFloat(value))
            $(td).html(formatFloat(parseFloat(value), 3, '.', ','));
        else {
            if (value == "0")
                $(td).html("0.000");
        }
    }

    var container = document.getElementById('detalleFormato');
    hotOptions = {
        data: evt.Handson.ListaExcelData,
        mergeCells: evt.Handson.ListaMerge,
        maxRows: 99,
        height: 500,
        colHeaders: true,
        rowHeaders: true,
        fillHandle: true,
        columnSorting: false,
        className: "htCenter",
        colWidths: evt.Handson.ListaColWidth,
        readOnly: evt.Handson.ReadOnly,

        cells: function (row, col, prop) {
            var cellProperties = {};
            var render;
            var formato = "";
            var tipo;
            readOnly = false;
            if (row == 0) {
                render = firstRowRenderer;
            }

            if (row > 0 && row <= 2) {
                render = descripRowRenderer;
            }
            if (row >= 3 && row <= 99 && col == 0) {
                render = fechaRenderer;
            }
            if (row >= 3 && row <= 99 && col >= 1 && col <= nColumnas) {
                if (evt.Handson.ReadOnly) {
                    render_readonly(this, row, col);
                }
                else {
                    render_celda_error(this, row, col);
                }
                formato = '0,0.000';
                tipo = 'numeric';
            }

            for (var i in evt.ListaCambios) {
                if ((row == evt.ListaCambios[i].Row) && (col == evt.ListaCambios[i].Col)) {
                    render = cambiosCellRenderer;
                    formato = '0,0.000';
                    tipo = 'numeric';
                }
            }
            cellProperties = {
                renderer: render,
                format: formato,
                type: tipo,
                readOnly: readOnly
            }

            return cellProperties;
        }
    };
    hot = new Handsontable(container, hotOptions);


    function render_readonly(ht, row, col) {
        fondo = "Silver"
        font_color = "DimGray";
        
        value = ht.instance.getDataAtCell(row, col);

        $(ht.instance.getCell(row, col)).css(
            {
                "color": font_color,
                "background-color": fondo,
                "vertical-align": "middle"
            })        
    }
}


render_celda_error = function (ht, row, col) {
    fondoLimSup = "yellow";
    fondoLimInf = "orange";
    fondo = "white"
    cell_color = "#000";
    font_color = "#fff";

    value = ht.instance.getDataAtCell(row, col);
    ptoSeleccionado = ht.instance.getDataAtCell(0, 1);
    if (ptoSeleccionado == null)
        return;
    if (ptoSeleccionado == '')
        return;
    td = ht.instance.getCell(row, col);
    
    var columnaPtos = ht.instance.getDataAtCol(0);

    if (value != null) {
        var celda = getExcelColumnName(parseInt(col) + 1) + (parseInt(row) + 1).toString();
        if (!isNaN(Number(value))) {
            indexPto = indexOfPto(ptoSeleccionado); 
            
            if ((Number(value) < listaPtos[indexPto].Hojaptoliminf)) {
                console.log("Menor " + value);
                td.style.background = errores[errorLimInferior].BackgroundColor;
                $(td).html(formatFloat(Number(value), 3, '.', ','));
                eliminarError(celda, errorLimInferior);
                agregarError(celda, value, errorLimInferior);
            }
            else {
                eliminarError(celda, errorLimInferior);
                if (Number(value) > listaPtos[indexPto].Hojaptolimsup) {
                    td.style.background = errores[errorLimSuperior].BackgroundColor;
                    $(td).html(formatFloat(Number(value), 3, '.', ','));
                    var celda = getExcelColumnName(parseInt(col) + 1) + (parseInt(row) + 1).toString();
                    agregarError(celda, value, errorLimSuperior);
                }
                else {
                    eliminarError(celda, errorLimSuperior);
                    td.style.background = 'white';
                    $(td).html(formatFloat(Number(value), 3, '.', ','));
                }
            }
        }
        else {
            eliminarError(celda, errorNoNumero);
            if (value == "0")
                $(td).html("0.000");
            else if (value != "") {
                if (isNaN(Number(value))) {
                    td.style.background = errores[errorNoNumero].BackgroundColor;
                    var celda = getExcelColumnName(parseInt(col) + 1) + (parseInt(row) + 1).toString();
                    agregarError(celda, value, errorNoNumero);
                }
            }
        }
    }
}

getExcelColumnName = function (pi_columnNumber) {
    var li_dividend = pi_columnNumber;
    var ls_columnName = "";
    var li_modulo;

    while (li_dividend > 0) {
        li_modulo = (li_dividend - 1) % 26;
        ls_columnName = String.fromCharCode(65 + li_modulo) + ls_columnName;
        li_dividend = Math.floor((li_dividend - li_modulo) / 26);
    }

    return ls_columnName;
}

eliminarError = function (celda, tipoError) {
    var index = indexOfError(celda);
    if (index != -1) {
        listErrores.splice(index, 1);
        switch (tipoError) {
            case errorNoNumero:
                errores[errorNoNumero].total--;
                break;
            case errorLimInferior:
                errores[errorLimInferior].total--;
                break;
            case errorLimSuperior:
                errores[errorLimSuperior].total--;
                break;
            case errorRangoFecha:
                errores[errorRangoFecha].total--;
                break;
            case errorCrucePeriodo:
                errores[errorCrucePeriodo].total--;
                break;
            case errorTime:
                errores[errorTime].total--;
                break;
        }
    }
}

indexOfError = function (celda) {
    var index = -1;
    for (var i = 0; i < listErrores.length; i++) {
        if (listErrores[i].Celda == celda) {
            index = i;
            break;
        }
    }

    return index;
}

agregarError = function (celda, valor, tipo) {
    if (validarError(celda)) {
        var regError = {
            Celda: celda,
            Valor: valor,
            Tipo: tipo
        };
        listErrores.push(regError);
        switch (tipo) {
            case errorNoNumero:
                errores[errorNoNumero].total++;
                break;
            case errorLimInferior:
                errores[errorLimInferior].total++;
                break;
            case errorLimSuperior:
                errores[errorLimSuperior].total++;
                break;
            case errorRangoFecha:
                errores[errorRangoFecha].total++;
                break;
            case errorCrucePeriodo:
                errores[errorCrucePeriodo].total++;
                break;
            case errorTime:
                errores[errorTime].total++;
                break;
            case errorExtremoTime:
                errores[errorExtremoTime].total++;
                break;
        }
    }
}

validarError = function (celda) {
    for (var j in listErrores) {
        if (listErrores[j]['Celda'] == celda) {
            return false;
        }
    }
    return true;
}


