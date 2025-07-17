var container;
var DATA_POSICION_PTO_MEDICION = [];
var HEIGHT_MINIMO = 500;

function crearGrillaExcel(evt, numHoja) {

    //variable segun Número de hoja
    var idHandsontable = 'detalleFormato' + getIdHoja(numHoja);

    var nColumnas = evt.Handson.ListaExcelData[0].length;
    var listaFilasOcultas = evt.ListaFilasOcultas != null ? evt.ListaFilasOcultas : [];

    var matrizTipoEstado = evt.Handson.MatrizTipoEstado;

    function calculateSize() {
        var offset = Handsontable.Dom.offset(container);

        //Verificar en que ventana esta, expandida -> 2 o extranet normal -> 1
        if (offset.top == 222) {
            availableHeight = $(window).height() - 140 - offset.top - 20;
        }
        else {
            availableHeight = $(window).height() - 140 - offset.top - 20;
        }

        if (offset.left > 0)
            availableWidth = $(window).width() - 2 * offset.left; //$("#divGeneral").width() - 50; //
        if (offset.top > 0) {
            availableHeight = availableHeight < HEIGHT_MINIMO ? HEIGHT_MINIMO : availableHeight;
            container.style.height = availableHeight + 'px';
        }
        if (LISTA_OBJETO_HOJA[getNumeroHoja(numHoja)] != undefined && getVariableHot(numHoja) != undefined) {
            getVariableHot(numHoja).render();
        }
    }


    var nestedHeader = getNestedHeader(evt.Handson.NestedHeader.ListCellNestedHeaders);

    //definicion
    var container = document.getElementById(idHandsontable);
    //Handsontable.Dom.addEvent(window, 'resize', calculateSize());
    hotOptions = {
        nestedHeaders: nestedHeader,
        data: evt.Handson.ListaExcelData,
        mergeCells: evt.Handson.ListaMerge,
        height: HEIGHT_MINIMO,
        colHeaders: true,
        rowHeaders: true,
        fillHandle: true,
        columnSorting: false,
        className: 'htRight',
        fixedColumnsLeft: evt.Handson.ColCabecera,
        colWidths: evt.Handson.ListaColWidth,
        readOnly: evt.Handson.ReadOnly,
        //columns: function (index) {
        //    return {
        //        className: index < 6 ? 'htCenter' : 'htRight'
        //    }
        //},
        cells: function (row, col, prop) {
        }
    };

    var hotTmp = new Handsontable(container, hotOptions);
    setVariableHot(hotTmp, numHoja);

    hotTmp.addHook('afterRender', function () {
        hotTmp.validateCells();
    });

    hotTmp.addHook('afterRenderer', function (TD, row, col, prop, value, cellProperties) {
        var readOnly = cellProperties.readOnly;
        var format = cellProperties.format;
        var type = cellProperties.type;

        if (col < 6) {
            readOnly = true;
            styleColDefault(TD);
        }

        if (col >= 6) {
            if (evt.Handson.ReadOnly) {
                render_readonly(TD, row, col, 0);
                readOnly = true;
            }
            else {
                if (matrizTipoEstado[row][col] == 1 || matrizTipoEstado[row][col] == 0) {
                    //
                    render_celda_color(TD, row, col, prop, value, evt, numHoja);
                }
                else {
                    //verificamos si hay celdas desactivadas
                    render_readonly(TD, row, col, matrizTipoEstado[row][col]);
                    readOnly = true;
                }
            }
            format = '0,0.000';
            type = 'numeric';
        }

        if (evt.Handson.ReadOnly) {
            var listaCambio = evt.Handson.ListaCambios;
            for (var i in listaCambio) {
                if ((row == listaCambio[i].Row) && (col == listaCambio[i].Col)) {
                    styleCellCambio(TD);
                }
            }
        }

        cellProperties.format = format;
        cellProperties.type = type;
        cellProperties.readOnly = readOnly;
    });

    calculateSize(1);
}

function getNestedHeader(lstNestedHeaders) {

    var nestedHeaders = [];

    lstNestedHeaders.forEach(function (currentValue, index, array) {
        var nestedHeader = [];
        currentValue.forEach(function (item) {
            if (item.Colspan == 0) {
                nestedHeader.push(item.Label);
            } else {
                nestedHeader.push({ label: item.Label, colspan: item.Colspan });
            }
        });

        nestedHeaders.push(nestedHeader);
    });

    return nestedHeaders;
}

function render_celda_color(td, row, col, prop, value, evtHot, numHoja) {
    var listaPtos = evtHot.ListaHojaPto;
    var errores = getErrores(numHoja);
    var numColFijo = evtHot.Handson.ColCabecera;

    var tipoError = '';
    if (value != null) {
        var error = obtenerErrorGlobal(value, row, col, listaPtos, errores, numColFijo, numHoja);
        tipoError = error != null ? error.Tipo : '';

        var valorNumerico = 0;
        if (Number(value)) {
            var valorNumerico = Number(value);
        }
        else {
            if (value == "0") {
                //$(td).html("0.000");
            }
        }
    } else {
        var error = obtenerErrorGlobal('', row, col, listaPtos, errores, numHoja);
        tipoError = error != null ? error.Tipo : '';
    }

    switch (tipoError) {
        case ERROR_NO_NUMERO:
            td.style.background = errores[ERROR_NO_NUMERO].BackgroundColor;

            break;
        case ERROR_BLANCO:
            td.style.background = errores[ERROR_BLANCO].BackgroundColor;
            break;
        case ERROR_LIM_INFERIOR:
            td.style.background = errores[ERROR_LIM_INFERIOR].BackgroundColor;
            $(td).html(formatFloat(valorNumerico, 3, '.', ','));

            break;
        case ERROR_LIM_SUPERIOR:
            td.style.background = errores[ERROR_LIM_SUPERIOR].BackgroundColor;
            $(td).html(formatFloat(valorNumerico, 3, '.', ','));

            break;
        case ERROR_NUMERO_NEGATIVO:
            td.style.background = errores[ERROR_NUMERO_NEGATIVO].BackgroundColor;

            break;
        case ERROR_DATA_CENTRAL_SOLAR:
            td.style.background = errores[ERROR_DATA_CENTRAL_SOLAR].BackgroundColor;

            break;
        case ERROR_DATA_INTERCONEXION:
            td.style.background = errores[ERROR_DATA_INTERCONEXION].BackgroundColor;

            break;
        case ERROR_DATA_DESPACHO:
            td.style.background = errores[ERROR_DATA_DESPACHO].BackgroundColor;

            break;
        default:
            if (Number(value)) {
                td.style.background = 'white';
                $(td).html(formatFloat(valorNumerico, 3, '.', ','));
            } else {
                if (value == "0") {
                    $(td).html("0.000");
                }
            }

            break;
    }
}

function render_readonly(td, row, col, tipo) {
    switch (tipo) {
        case -1:// solo lectura toda la matriz
            fondo = "Silver"
            font_color = "DimGray";
            break;

        case 0:
            fondo = "Bisque"
            font_color = "DimGray";
            break;
        case 1:
            fondo = "Bisque"
            font_color = "DimGray";
            break;
        case 2:
            fondo = "MediumPurple "
            font_color = "DimGray";
            break;
        case 3:
            fondo = "Salmon "
            font_color = "DimGray";
            break;
    }

    $(td).css(
        {
            "color": font_color,
            "background-color": fondo,
            "vertical-align": "middle"
        })

}

function styleColDefault(td) {
    $(td).css(
        {
            "color": 'MidnightBlue',
            "background-color": 'rgb(232, 246, 255)',
            "vertical-align": "middle",
            "text-align": "center"
        })
}

function styleCellCambio(td) {
    $(td).css(
        {
            "color": 'black',
            "background-color": '#FFFFD7',
        })
}

function getCustomRenderer(colsToHide) {
    return function (instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        if (colsToHide.indexOf(col) > -1) {
            td.hidden = true;
        } else {
            td.hidden = false;
        }
    }
}

function getVisibleRender(ht, row, col, numHoja) {
    var colsToHide = getColsToHide(numHoja);
    td = ht.instance.getCell(row, col);
    if (td !== undefined && td != null) {
        if (colsToHide.indexOf(col) > -1) {
            td.hidden = true;
        } else {
            td.hidden = false;
        }
    }
}

function hideColumn(td, row, col, numHoja) {
    var colsToHide = getColsToHide(numHoja);
    colVisible = colsToHide.indexOf(col);

    if (colVisible > -1) {
        td.hidden = true;
    } else {
        td.hidden = false;
    }
}

function findIndiceMerge(col, lista) {
    for (key in lista) {
        if ((col >= lista[key].col) && (col < (lista[key].col + lista[key].colspan))) {
            return key;
        }
    }
    return -1;
}

function getExcelColumnName(pi_columnNumber) {
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

function getFechaFila(data, totalFila, fila) {
    var anio = data.substring(6, 10);
    var mes = data.substring(3, 5);
    var dia = data.substring(0, 2);

    var hora = data.substring(11, 16) + ':00';

    var fechaDate = anio + '/' + mes + '/' + dia + ' ' + hora;
    var res = new Date(fechaDate);

    return res;
}

function updateDimensionHandson(numHoja) {
    if (!getTieneHojaView(numHoja)) {
        var idHandsontable = 'detalleFormato' + getIdHoja(numHoja);
        var container = document.getElementById(idHandsontable)
        var hot = getVariableHot(numHoja);
        var offset = {};
        try {
            offset = Handsontable.Dom.offset(container);
        }
        catch (err) {
            console.log(err);
        }

        if (offset.length != 0) {
            var widthHT;
            var heightHT;

            if (offset.top == 222) {
                heightHT = $(window).height() - 140 - offset.top - 20;
            }
            else {
                heightHT = $(window).height() - 140 - offset.top - 20;
            }

            heightHT = heightHT < HEIGHT_MINIMO ? HEIGHT_MINIMO : heightHT;
            if (offset.left > 0 && offset.top > 0) {
                widthHT = $(window).width() - 2 * offset.left; // $("#divGeneral").width() - 50; //
                hot.updateSettings({
                    width: widthHT,
                    height: heightHT
                });
            }
        }
    }
}