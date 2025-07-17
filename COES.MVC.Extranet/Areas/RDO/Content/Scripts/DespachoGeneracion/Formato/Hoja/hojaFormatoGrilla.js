var container;
var DATA_POSICION_PTO_MEDICION = [];
var HEIGHT_MINIMO = 500;

function crearGrillaExcel(evt, numHoja, val_envio) {

    //variable segun Número de hoja
    var idHandsontable = 'detalleFormato' + getIdHoja(numHoja);

    var nColumnas = evt.Handson.ListaExcelData[0].length;
    var listaFilasOcultas = evt.ListaFilasOcultas != null ? evt.ListaFilasOcultas: [];

    var matrizTipoEstado = evt.Handson.MatrizTipoEstado;

    if (val_envio === undefined) {
        val_envio = 0;
    }
        

    function descripRowRenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontSize = '10px';
        td.style.color = 'White';
        td.style.background = 'rgb(47, 78, 99) none repeat scroll 0% 0%';
        hideColumn(td, row, col, numHoja);
    }

    function firstRowRenderer2(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontWeight = 'bold';
        td.style.fontSize = '10px';
        td.style.color = 'MidnightBlue';
        td.style.background = 'rgb(90, 161, 206) none repeat scroll 0% 0%';
        hideColumn(td, row, col, numHoja);
    }

    function firstRowRenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontWeight = 'bold';
        td.style.fontSize = '10px';
        td.style.color = 'White';
        td.style.background = '#2980B9';
        hideColumn(td, row, col, numHoja);
    }

    function fechaRenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontSize = '12px';
        td.style.color = 'MidnightBlue';
        td.style.background = '#E8F6FF';
        hideColumn(td, row, col, numHoja);
    }

    function cambiosCellRenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontSize = '12px';
        td.style.textAlign = 'right';
        td.style.color = 'black';
        td.style.background = '#FFFFD7';
        hideColumn(td, row, col, numHoja);
        if (value == '') {
            $(td).html('');
        } else {
            $(td).html(formatFloat(parseFloat(value), 3, '.', ','));
        }
    }

    function checkRenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.CheckboxRenderer.apply(this, arguments);
        td.style.fontSize = '12px';
        td.style.textAlign = 'center';
        td.style.color = 'DimGray';
        td.style.background = 'Gainsboro';
    }

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


    matriz = evt.Handson.ListaExcelData;
    matrizEjecutadosRER = evt.Handson.ListaExcelDataEjecutadosRER;
    myDate = new Date();
    hours = myDate.getHours();

    var _filamatriz = evt.FilasCabecera - 1;
    var _aux = 0;
    for (var z = 0; z < evt.ListaEquipo.length; z++) {

        if ((evt.ListaEquipo[z].Famcodi == 37 || evt.ListaEquipo[z].Famcodi == 39) && _aux == 0) {
            for (var i = 0; i < this.matriz.length; i++) {
                if (i <= _filamatriz) {
                    P = ""
                }
                else if (parseInt(matriz[i][0].substring(11, 13)) <= parseInt(val_envio) && matriz[i][0].substring(11, 16) != val_envio + ":30" && i != this.matriz.length - 1) {
                    P = true;
                }
                else if (parseInt(val_envio) == 24) {
                    P = true;
                }
                else {
                    P = false;
                }
                this.matriz[i].push(P);
            }
            _aux = 1;
        }
        else if (evt.ListaEquipo[z].Famcodi != 37 && evt.ListaEquipo[z].Famcodi != 39) {
            for (var x = evt.FilasCabecera ; x < this.matriz.length; x++) {
                var hr_rdb = val_envio + ":00";
                if (parseInt(matriz[x][0].substring(11, 13)) >= parseInt(val_envio) && matriz[x][0].substring(11, 16).toString() != hr_rdb.toString()) {

                    for (j = 1; j < matrizTipoEstado[0].length; j++) {
                        if (matrizEjecutadosRER[x][j] != "E") {
                            matrizTipoEstado[x][j] = -1;
                        }
                    }      
                }
                if (x == this.matriz.length - 1) {
                    for (y = 1; y < matrizTipoEstado[0].length; y++) {
                        if (matrizEjecutadosRER[x][y] != "E") {
                            matrizTipoEstado[x][y] = -1;
                        }
                        
                    }
                }
            }
        }
    }

    

    //definicion
    var container = document.getElementById(idHandsontable);
    //Handsontable.Dom.addEvent(window, 'resize', calculateSize());
    hotOptions = {
        //data: evt.Handson.ListaExcelData,
        data: matriz,
        mergeCells: evt.Handson.ListaMerge,
        maxCols: matriz[0].length,
        maxRows: matriz.length,//52,
        height: HEIGHT_MINIMO,
        colHeaders: true,
        rowHeaders: true,
        fillHandle: true,
        columnSorting: false,
        className: "htCenter",
        fixedColumnsLeft: 1,
        fixedRowsTop: evt.FilasCabecera,
        colWidths: evt.Handson.ListaColWidth,
        readOnly: evt.Handson.ReadOnly,
        hiddenRows: {
            rows: listaFilasOcultas,
            indicators: false
        },
        cells: function (row, col, prop) {
            var cellProperties = {};
            var readOnly;
            var formato;
            var tipo;
            var render;

            if (row == 0 && col > 0) {
                render = firstRowRenderer;
                readOnly = true;
            }
            if (row >= 0 && row <= (evt.FilasCabecera - 1) && col == 0) {
                render = descripRowRenderer;
                readOnly = true;
            }
            if (row >= 0 && row <= (evt.FilasCabecera - 1) && col > 0) {
                var indMerge = findIndiceMerge(col, evt.Handson.ListaMerge);
                if ((indMerge % 2) == 0) {
                    render = firstRowRenderer;
                }
                else {
                    render = firstRowRenderer2;
                }
                readOnly = true;
            }
            if (row >= evt.FilasCabecera && row <= evt.Handson.ListaExcelData.length && col == 0) {
                render = fechaRenderer;
                readOnly = true;
            }

            if (row >= evt.FilasCabecera && row <= evt.Handson.ListaExcelData.length && col >= 1 && col <= nColumnas) {
              
                if (evt.Handson.ReadOnly) {
                    render_readonly(this, row, col, 0);
                    getVisibleRender(this, row, col, numHoja);
                    readOnly = true;

                    if ((row >= evt.FilasCabecera) && (col >= evt.ColumnasCabecera) && (col == matriz[0].length - 1) && (nColumnas == matriz[0].length - 1)) {
                        tipo = 'checkbox';
                        render = checkRenderer;
                    }
                }
                else {                
                    if (matrizTipoEstado[row][col] == 1 || matrizTipoEstado[row][col] == 0) { // Tiene Hora de Operacion
                        render_celda_color(this, evt, row, col, evt.FilasCabecera, numHoja, matriz[0].length - 1, matrizTipoEstado[row][col], nColumnas);
                        getVisibleRender(this, row, col, numHoja);
                    }
                    else {
                        if ((nColumnas == matriz[0].length - 1)) {
                            render_readonly(this, row, col, matrizTipoEstado[row][col]);
                            getVisibleRender(this, row, col, numHoja);
                            if (col == matriz[0].length - 1 && nColumnas < matriz[0].length)
                                readOnly = false;
                            else
                                readOnly = true;
                        }
                        else {
                            render_readonly(this, row, col, matrizTipoEstado[row][col]);
                            getVisibleRender(this, row, col, numHoja);
                            readOnly = true;
                        }
                        
                    }
                    if (col == matriz[0].length - 1 && nColumnas < matriz[0].length)
                    {
                        tipo = 'checkbox';
                    }
                    else {
                        formato = '0,0.000';
                        tipo = 'numeric';
                    }
                }
            }
             
            for (var i in evt.ListaCambios) {
                if ((row == evt.ListaCambios[i].Row) && (col == evt.ListaCambios[i].Col) && (col < matriz[0].length - 1)) {
                    render = cambiosCellRenderer;
                    formato = '0,0.000';
                    tipo = 'numeric';
                }
            }

            cellProperties = {
                format: formato,
                type: tipo,
                readOnly: readOnly,
                renderer: render
            }
            return cellProperties;
        }
    };
    setVariableHot(new Handsontable(container, hotOptions), numHoja);
    calculateSize(1);
}

function render_celda_color(ht, evtHot, row, col, numFilCabecera, numHoja, nColumnas, estado, nColHanson) {
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
    var errores = getErrores(numHoja);

    var tipoError = '';
    if (td != null && value != null && estado == 1 || estado == 0) {
        if (row > evtHot.FilasCabecera - 1 && col < nColumnas && nColHanson == nColumnas) {
            var valor_check = ht.instance.getDataAtCell(row, nColumnas);
            var error = obtenerErrorGlobal(value, row, col, listaPtos, errores, numFilCabecera, numHoja, valor_check);
        }
        else {
            var error = obtenerErrorGlobal(value, row, col, listaPtos, errores, numFilCabecera, numHoja, "NS");
        }
        
        tipoError = error != null ? error.Tipo : '';
       
        var valorNumerico = 0;
        if (Number(value)) {
            var valorNumerico = Number(value);
        }
        else {
            if (value == "0") {
                $(td).html("0.000");
            }
        }
    } else {
        var error = obtenerErrorGlobal('', row, col, listaPtos, errores, numFilCabecera, numHoja,'');
        tipoError = error != null ? error.Tipo : '';
        
    }

    if (td != null) {
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

function render_vertical_align(ht, row, col) {
    $(ht.instance.getCell(row, col)).css(
    {
        "vertical-align": "middle"
    });
}

function render_readonly(ht, row, col, tipo) {
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

    value = ht.instance.getDataAtCell(row, col);

    $(ht.instance.getCell(row, col)).css(
        {
            "color": font_color,
            "background-color": fondo,
            "vertical-align": "middle"
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