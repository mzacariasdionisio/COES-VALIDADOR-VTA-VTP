/// Crea El objeto Handson en la pagina web
var container;
function crearHandsonTable(evtHot) {

    function errorRowRenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);

        td.style.fontSize = '12px';
        td.style.color = 'black';
        td.style.background = '#FF4C42';
    }

    function fechaRowRenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontSize = '12px';
        td.style.color = 'MidnightBlue';
        td.style.background = '#E8F6FF';
    }

    function firstRowRenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontWeight = 'bold';
        td.style.fontSize = '10px';
        td.style.color = 'MidnightBlue';
        td.style.background = '#2980B9';
    }

    function firstRowRenderer2(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontWeight = 'bold';
        td.style.fontSize = '10px';
        td.style.color = 'White';
        td.style.background = '#2980B9';
    }

    function descripRowRenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontSize = '10px';
        td.style.color = 'MidnightBlue';
        td.style.background = '#EAF7D9';
    }

    function descripRowRenderer2(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontSize = '10px';
        td.style.color = 'MidnightBlue';
        td.style.background = '#E8F6FF';
    }

    function cambiosCellRenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontSize = '12px';
        td.style.textAlign = 'right';
        td.style.background = '#FFFFD7';
        if (value != "") {
            $(td).html(formatFloat(parseFloat(value), 3, '.', ','));
        }
    }

    function negativeValueRenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);

        if (parseInt(value, 10) < 0) {
            td.style.color = 'orange';
            td.style.fontStyle = 'italic';
        }
    }

    function limitesCellRenderer(instance, td, row, col, prop, value, cellProperties) {

        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontSize = '12px';
        td.style.textAlign = 'right';
        if (Number(value) && value != "") {
            if (Number(value) < evtHot.ListaHojaPto[col - 1].Hojaptoliminf) {
                td.style.background = 'orange';
                $(td).html(formatFloat(Number(value), 3, '.', ','));
                var celda = getExcelColumnName(parseInt(col) + 1) + (parseInt(row) + 1).toString();
                agregarError(celda, value, 3);

            }
            else {
                if (Number(value) > evtHot.ListaHojaPto[col - 1].Hojaptolimsup) {
                    td.style.background = 'yellow';
                    $(td).html(formatFloat(Number(value), 3, '.', ','));
                    var celda = getExcelColumnName(parseInt(col) + 1) + (parseInt(row) + 1).toString();
                    agregarError(celda, value, 4);
                }
                else {
                    td.style.background = 'white';
                    $(td).html(formatFloat(Number(value), 3, '.', ','));
                }
            }
        }
        else {
            if (value == "0")
                $(td).html("0.000");
            else if (value != "") {
                if (!Number(value)) {
                    td.style.background = 'red';
                    var celda = getExcelColumnName(parseInt(col) + 1) + (parseInt(row) + 1).toString();
                    agregarError(celda, value, 2);
                }
            }
        }

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

    function calculateSize() {
        var offset;
        offset = Handsontable.Dom.offset(container);

        if (offset.top == 222) {
            availableHeight = $(window).height() - offset.top - 90;
        }
        else {
            availableHeight = $(window).height() - offset.top - 20;
        }

        availableWidth = $(window).width() - 2 * offset.left;
        container.style.height = availableHeight + 'px';
        hot.render();
    }

    container = document.getElementById('detalleFormato');
    Handsontable.renderers.registerRenderer('negativeValueRenderer', negativeValueRenderer);
    Handsontable.Dom.addEvent(window, 'resize', calculateSize);
    Handsontable.Dom.addEvent(container, 'click', function () {
        validaInicial = false;
    });

    hotOptions = {
        data: evtHot.Handson.ListaExcelData,
        height: 500,
        maxRows: evtHot.Handson.ListaExcelData.length,
        maxCols: evtHot.Handson.ListaExcelData[0].length,
        colHeaders: true,
        rowHeaders: true,
        fillHandle: true,
        columnSorting: false,
        className: "htCenter",
        readOnly: evtHot.Handson.ReadOnly,
        fixedRowsTop: evtHot.FilasCabecera,
        fixedColumnsLeft: evtHot.ColumnasCabecera,
        mergeCells: evtHot.Handson.ListaMerge,
        colWidths: evtHot.Handson.ListaColWidth,
        afterLoadData: function () {
            this.render();
        },
        beforeChange: function (changes, source) {
            for (var i = changes.length - 1; i >= 0; i--) {
                var valorOld = changes[i][2];
                var valorNew = changes[i][3];
                var liminf = evtHot.ListaHojaPto[changes[i][1] - 1].Hojaptoliminf;
                var limsup = evtHot.ListaHojaPto[changes[i][1] - 1].Hojaptolimsup;
                var tipoOld = getTipoError(valorOld, liminf, limsup);//isNaN(changes[i][2]) ? true : false;
                var tipoNew = getTipoError(valorNew, liminf, limsup); //isNaN(changes[i][3]) ? true : false;
                var celda = getExcelColumnName(parseInt(changes[i][1]) + 1) + (parseInt(changes[i][0]) + 1).toString();
                if (tipoOld > 1) {
                    eliminarError(celda, tipoOld);
                    if (tipoNew > 1) {
                        agregarError(celda, changes[i][3], tipoNew);
                    }
                }
                else {
                    if (tipoNew > 1) {
                        agregarError(celda, changes[i][3], tipoNew);
                    }
                }
            }
        },

        cells: function (row, col, prop) {
            var cellProperties = {};
            var formato = "";
            var render;
            var readOnly = true;
            var tipo;
            if (col === 0) {
                render = fechaRowRenderer;
                readOnly = true;
            }
            else {
                if (!evtHot.Handson.ReadOnly) {//Primero prevalece el readonly de todo el excel
                    readOnly = evtHot.Handson.ListaFilaReadOnly[row];
                }
                else {
                    render = readOnlyValueRenderer;
                }
            }

            if (row < evtHot.FilasCabecera) {
                //cellProperties.readOnly = true;
                if (row == 0) {
                    var indMerge = findIndiceMerge(col, evtHot.Handson.ListaMerge);
                    if ((indMerge % 2) == 0) {
                        render = firstRowRenderer;
                    }
                    else {
                        render = firstRowRenderer2;
                    }
                }
                else {
                    if ((row % 2) == 0) {
                        //cellProperties.renderer = descripRowRenderer;
                        render = descripRowRenderer;
                    }
                    else {
                        //cellProperties.renderer = firstRowRenderer;
                        render = descripRowRenderer2;
                    }
                }
            }

            if ((row >= evtHot.FilasCabecera) && (col >= evtHot.ColumnasCabecera) && (!evtHot.Handson.ReadOnly)) {
                if (evtHot.Handson.ListaFilaReadOnly[row]) {
                    render = readOnlyValueRenderer;
                }
                else {
                    render = limitesCellRenderer;
                }
                formato = '0,0.000';
                tipo = 'numeric';

            }
            for (var i in evtHot.ListaCambios) {
                if ((row == evtHot.ListaCambios[i].Row) && (col == evtHot.ListaCambios[i].Col)) {
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
    calculateSize(1);
}

function calculateSize2(opcion) {
    var offset;
    offset = Handsontable.Dom.offset(container);

    if (opcion == 1) {
        availableHeight = $(window).height() - offset.top - 10;
    }
    else {
        availableHeight = $(window).height() - offset.top - 80;
    }

    availableWidth = $(window).width() - 2 * offset.left;
    container.style.height = availableHeight + 'px';
    container.style.width = availableWidth + 'px';
    hot.render();

}

function crearHandsonTable2() {
    container = document.getElementById('detalleFormato');
    hotOptions = {
        data: evtHot.Handson.ListaExcelData,
        height: 500,
        maxRows: evtHot.Handson.ListaExcelData.length,
        maxCols: evtHot.Handson.ListaExcelData[0].length,

        colHeaders: true,
        rowHeaders: true,
        fillHandle: true,
        columnSorting: false,
        className: "htCenter",
        readOnly: evtHot.Handson.ReadOnly,
        fixedRowsTop: evtHot.FilasCabecera,
        fixedColumnsLeft: evtHot.ColumnasCabecera,
        mergeCells: evtHot.Handson.ListaMerge,
        colWidths: evtHot.Handson.ListaColWidth,
    };
    hot = new Handsontable(container, hotOptions);
}
