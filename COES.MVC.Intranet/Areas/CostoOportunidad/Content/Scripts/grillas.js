/// Crea El objeto Handson en la pagina web
var container;
var previoChange = 0;


function crearGrillaExcelCostos(evt) {

    var nColumnas = evt.Handson.ListaExcelData[0].length;
    //var listaPtos = evt.ListaHojaPto;
    mostrarmensajes(evt.EnPlazo, evt.IdEnvio, evt.FechaEnvio);

    function descripRowRenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontSize = '10px';
        td.style.color = 'White';
        td.style.background = '#2980B9';
        if (col > 0)
            if (parseInt(listaPtos[col - 1].Tipoptomedicodi) == 40) {
                td.style.background = '#E8F6FF';
                td.style.color = 'MidnightBlue';
            }
    }

    var firstRowRenderer = function (instance, td, row, col, prop, value, cellProperties) {
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
        maxRows: 27,
        height: 500,
        colHeaders: true,
        rowHeaders: true,
        fillHandle: true,
        columnSorting: false,
        className: "htCenter",
        colWidths: evt.Handson.ListaColWidth,
        //readOnly: evt.Handson.ReadOnly,
        hiddenColumns: {
            columns: [5], indicators: true
        },

        cells: function (row, col, prop) {
            var cellProperties = {};
            var render;
            var formato = "";
            var tipo;
            readOnly = false;
            if (row == 0) {
                //render = firstRowRenderer;
                render = firstRowRenderer;
                readOnly = true;
            }
            if (row > 0 && row <= 2) {
               // render = descripRowRenderer;
            }
            if (row >= 1 && col == 0) {
                render = fechaRenderer;
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

}

function render_celda_color(ht, evtHot, row, col) {
    fondoLimSup = "yellow";
    fondoLimInf = "orange";
    fondo = "white"
    cell_color = "#000";
    font_color = "#fff";
    limiteInf = 0;
    limiteSup = 0;
    value = ht.instance.getDataAtCell(row, col);
    td = ht.instance.getCell(row, col);
    listaPtos = evtHot.ListaHojaPto;
    if (value != null) {
        celda = getExcelColumnName(parseInt(col) + 1) + (parseInt(row) + 1).toString();
        if (Number(value)) {
            limiteInf = listaPtos[col - 1].Hojaptoliminf;
            limiteSup = listaPtos[col - 1].Hojaptolimsup;
            if (Number(value) < limiteInf) {
                td.style.background = errores[errorLimInferior].BackgroundColor;
                $(td).html(formatFloat(Number(value), 3, '.', ','));
                eliminarError(celda, errorLimInferior);
                agregarError(celda, value, errorLimInferior);
            }
            else {
                eliminarError(celda, errorLimInferior);
                if (Number(value) > limiteSup) {
                    td.style.background = errores[errorLimSuperior].BackgroundColor;
                    $(td).html(formatFloat(Number(value), 3, '.', ','));
                    var celda = getExcelColumnName(parseInt(col) + 1) + (parseInt(row) + 1).toString();
                    eliminarError(celda, errorLimSuperior);
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
                if (!Number(value)) {
                    td.style.background = errores[errorNoNumero].BackgroundColor;
                    var celda = getExcelColumnName(parseInt(col) + 1) + (parseInt(row) + 1).toString();
                    agregarError(celda, value, errorNoNumero);
                }
            }
        }

    }

    //$(ht.instance.getCell(row, col)).css(
    //    {
    //        "color": "black",
    //        "background-color": fondo,
    //        "vertical-align": "middle"
    //    });
}

function render_celda_color_stock(ht, row, col) {
    fondoLimSup = "yellow";
    fondoLimInf = "orange";
    fondo = "white"
    cell_color = "#000";
    font_color = "#fff";
    indexPto = 0;
    limiteInf = 0;
    limiteSup = 0;
    value = ht.instance.getDataAtCell(row, col);
    celda = getExcelColumnName(parseInt(col) + 1) + (parseInt(row) + 1).toString();
    td = ht.instance.getCell(row, col);
    if (value != null) {
        var celda = getExcelColumnName(parseInt(col) + 1) + (parseInt(row) + 1).toString();
        if (Number(value)) {
            if (row >= 2 && row < 2 + nfilConsumo) {
                indexPto = row - 2;
                limiteInf = listaPtos[indexPto].Hojaptoliminf;
                limiteSup = listaPtos[indexPto].Hojaptolimsup;
            }
            else {
                if (row >= nfilConsumo + 5) {
                    switch (col) {
                        case columnas[0].inicial:
                        case columnas[0].final:
                            indexPto = (row - nfilConsumo - 5) * 2;
                            break;
                        case columnas[0].recepcion:
                            indexPto = (row - nfilConsumo - 5) * 2 + 1;
                            break;
                    }
                    limiteInf = listaPtosStock[indexPto].Hojaptoliminf;
                    limiteSup = listaPtosStock[indexPto].Hojaptolimsup;
                }
            }
            // indexPto = indexOfPto(ptoSeleccionado);
            if (Number(value) < limiteInf) {
                td.style.background = errores[errorLimInferior].BackgroundColor;
                $(td).html(formatFloat(Number(value), 3, '.', ','));
                eliminarError(celda, errorLimInferior);
                agregarError(celda, value, errorLimInferior);
            }
            else {
                eliminarError(celda, errorLimInferior);
                if (Number(value) > limiteSup) {
                    td.style.background = errores[errorLimSuperior].BackgroundColor;
                    $(td).html(formatFloat(Number(value), 3, '.', ','));
                    var celda = getExcelColumnName(parseInt(col) + 1) + (parseInt(row) + 1).toString();
                    eliminarError(celda, errorLimSuperior);
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
                    //console.log("Error:" + Number("PP"));
                    td.style.background = errores[errorNoNumero].BackgroundColor;
                    var celda = getExcelColumnName(parseInt(col) + 1) + (parseInt(row) + 1).toString();
                    agregarError(celda, value, errorNoNumero);
                }
            }
        }
    }
    $(ht.instance.getCell(row, col)).css(
    {
        "vertical-align": "middle"
    });
}

function render_total(ht, row, col) {
    fondo = "#4682B4"
    font_color = "white";

    value = ht.instance.getDataAtCell(row, col);

    $(ht.instance.getCell(row, col)).css(
        {
            "color": font_color,
            "font-size": "14px",
            "background-color": fondo,
            "vertical-align": "middle",
            "font-weight": "bold"
        })

}

function render_total_declarado(ht, row, col) {
    fondo = "white"
    font_color = "#2E8B57";
    value = ht.instance.getDataAtCell(row, col);//ht.instance.plugin.helper.cellValue(getExcelColumnName(columnas[0].declarado) + (row + 1).toString());
    eserror = false;
    indexPto = 0;
    limiteInf = 0;
    limiteSup = 0;
    celda = getExcelColumnName(parseInt(col) + 1) + (parseInt(row) + 1).toString();
    td = ht.instance.getCell(row, col);

    if (value != null) {
        var celda = getExcelColumnName(parseInt(col) + 1) + (parseInt(row) + 1).toString();
        if (Number(value)) {
            indexPto = (row - nfilConsumo - 5) * 2;
            limiteInf = listaPtosStock[indexPto].Hojaptoliminf;
            limiteSup = listaPtosStock[indexPto].Hojaptolimsup;

            // indexPto = indexOfPto(ptoSeleccionado);
            if (Number(value) < limiteInf) {
                td.style.background = errores[errorLimInferior].BackgroundColor;
                $(td).html(formatFloat(Number(value), 3, '.', ','));
                agregarError(celda, value, errorLimInferior);
                eserror = true;
            }
            else {
                eliminarError(celda, errorLimInferior);
                if (Number(value) > limiteSup) {
                    td.style.background = errores[errorLimSuperior].BackgroundColor;
                    $(td).html(formatFloat(Number(value), 3, '.', ','));
                    var celda = getExcelColumnName(parseInt(col) + 1) + (parseInt(row) + 1).toString();
                    agregarError(celda, value, errorLimSuperior);
                    eserror = true;
                }
                else {
                    eliminarError(celda, errorLimSuperior);
                    td.style.background = 'white';
                    $(td).html(formatFloat(Number(value), 3, '.', ','));
                }
            }
        }
        else {
            eliminarError(celda, errorLimInferior);
            if (value == "0")
                $(td).html("0.000");
            else if (value != "") {
                if (isNaN(Number(value))) {
                    td.style.background = errores[errorNoNumero].BackgroundColor;
                    var celda = getExcelColumnName(parseInt(col) + 1) + (parseInt(row) + 1).toString();
                    agregarError(celda, value, errorNoNumero);
                    eserror = true;
                }
            }
        }
    }

    if (value == null) {
        $(td).html("0.000");
    }


    if (!eserror) {
        $(ht.instance.getCell(row, col)).css(
            {
                "color": font_color,
                "font-size": "14px",
                "background-color": fondo,
                "font-weight": "bold"
            });
    }
}

function render_merge(ht, row, col) {
    fondo = "#E0FFFF"
    font_color = "MidnightBlue";
    value = ht.instance.getDataAtCell(row, col);
    $(ht.instance.getCell(row, col)).css(
        {
            "color": font_color,
            "font-size": "12px",
            "background-color": fondo,
            "vertical-align": "middle"
            //"readOnly": true

        })

}

function render_celda_error(ht, row, col) {
    fondoLimSup = "yellow";
    fondoLimInf = "orange";
    fondo = "white"
    cell_color = "#000";
    font_color = "#fff";

    value = ht.instance.getDataAtCell(row, col);
    ptoSeleccionado = ht.instance.getDataAtCell(row, 0);
    if (ptoSeleccionado == null)
        return;
    if (ptoSeleccionado == '')
        return;
    td = ht.instance.getCell(row, col);
    ///////
    var columnaPtos = ht.instance.getDataAtCol(0);

    if (value != null) {
        var celda = getExcelColumnName(parseInt(col) + 1) + (parseInt(row) + 1).toString();
        if (!isNaN(Number(value))) {
            indexPto = indexOfPto(ptoSeleccionado);
            if ((Number(value) <= listaPtos[indexPto].Hojaptoliminf)) {
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

function render_celda_time(ht, row, col) {
    value = ht.instance.getDataAtCell(row, col);
    fecha = ht.instance.getDataAtCell(row, 1);
    td = ht.instance.getCell(row, col);
    var celda = getExcelColumnName(parseInt(col) + 1) + (parseInt(row) + 1).toString();
    if (validaTime(value)) {
        eliminarError(celda, errorTime);
        if (validaExtremoTime(value, fecha)) {
            eliminarError(celda, errorExtremoTime);
            td.style.background = 'white';
        }
        else {
            agregarError(celda, value, errorExtremoTime);
            td.style.background = errores[errorExtremoTime].BackgroundColor;
        }
    }
    else {
        agregarError(celda, value, errorTime);
    }

}

function render_vertical_align(ht, row, col) {
    $(ht.instance.getCell(row, col)).css(
    {
        "vertical-align": "middle"
    });
}

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

function render_celda_time_quema(ht, row, col) {
    value = ht.instance.getDataAtCell(row, col);
    fecha = ht.instance.getDataAtCell(row, 1);
    td = ht.instance.getCell(row, col);
    var celda = getExcelColumnName(parseInt(col) + 1) + (parseInt(row) + 1).toString();
    if (validaTime(value)) {
        eliminarError(celda, errorTime);
        td.style.background = 'white';
    }
    else {
        agregarError(celda, value, errorTime);
    }
}

