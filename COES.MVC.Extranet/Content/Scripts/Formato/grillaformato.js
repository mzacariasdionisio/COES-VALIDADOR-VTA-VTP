var container;
var DATA_POSICION_PTO_MEDICION = [];

function crearGrillaExcel(evt) {
    listaGrupo = [];
    listaSize = evt.Handson.ListaColWidth;
    var nColumnas = evt.Handson.ListaExcelData[0].length;
    filasCab = evt.FilasCabecera;
    listaPtos = evt.ListaHojaPto;
    manttos = evt.ListaMantenimiento;
    eventos = evt.ListaEvento;
    listaBloqueMantos = evt.ListaBloqueMantos;
    mostrarmensajes(evt.EnPlazo, evt.IdEnvio, evt.FechaEnvio);
    var matrizTipoEstado = evt.Handson.MatrizTipoEstado;
    DATA_POSICION_PTO_MEDICION = getListaGrupo();

    function descripRowRenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontSize = '10px';
        td.style.color = 'White';
        td.style.background = 'rgb(47, 78, 99) none repeat scroll 0% 0%';
        hideColumn(td, row, col);
    }

    function firstRowRenderer2(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontWeight = 'bold';
        td.style.fontSize = '10px';
        td.style.color = 'MidnightBlue';
        td.style.background = 'rgb(90, 161, 206) none repeat scroll 0% 0%';
        hideColumn(td, row, col);
    }

    function firstRowRenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontWeight = 'bold';
        td.style.fontSize = '10px';
        td.style.color = 'White';
        td.style.background = '#2980B9';
        hideColumn(td, row, col);
    }

    function fechaRenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontSize = '12px';
        td.style.color = 'MidnightBlue';
        td.style.background = '#E8F6FF';
        hideColumn(td, row, col);
    }

    function cambiosCellRenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontSize = '12px';
        td.style.textAlign = 'right';
        td.style.color = 'black';
        td.style.background = '#FFFFD7';
        hideColumn(td, row, col);
        if (value == '') {
            $(td).html('');
        } else {
            $(td).html(formatFloat(parseFloat(value), 3, '.', ','));
        }

    }

    function calculateSize() {
        var offset;
        offset = Handsontable.Dom.offset(container);

        //Verificar en que ventana esta, expandida -> 2 o extranet normal -> 1
        //alert(offset.top);
        if (offset.top == 222) {
            availableHeight = $(window).height() - offset.top - 90;
        }
        else {
            availableHeight = $(window).height() - offset.top - 20;
        }


        availableWidth = $(window).width() - 2 * offset.left;
        container.style.height = availableHeight + 'px';
        hot.render();
        //alert("v1:" + availableWidth + "  , top:  " + availableHeight);

    }

    var container = document.getElementById('detalleFormato');
    Handsontable.Dom.addEvent(window, 'resize', calculateSize);
    hotOptions = {
        data: evt.Handson.ListaExcelData,
        mergeCells: evt.Handson.ListaMerge,
        maxRows: evt.Handson.ListaExcelData.length,//52,
        height: 500,
        colHeaders: true,
        rowHeaders: true,
        fillHandle: true,
        columnSorting: false,
        className: "htCenter",
        fixedColumnsLeft: 1,
        fixedRowsTop: evt.FilasCabecera,
        colWidths: evt.Handson.ListaColWidth,
        readOnly: evt.Handson.ReadOnly,
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
                //render = firstRowRenderer;
                //readOnly = true;
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
                    getVisibleRender(this, row, col);
                    readOnly = true;
                }
                else {

                    //verificamos si hay celdas desactivadas                    
                    if (matrizTipoEstado[row][col] == 1 || matrizTipoEstado[row][col] == 0) { // Tiene Hora de Operacion
                        render_celda_color(this, evt, row, col);
                        getVisibleRender(this, row, col);
                    }
                    else {
                        render_readonly(this, row, col, matrizTipoEstado[row][col]);
                        getVisibleRender(this, row, col);
                        readOnly = true;
                    }
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
                format: formato,
                type: tipo,
                readOnly: readOnly,
                renderer: render
            }
            return cellProperties;
        }
    };
    hot = new Handsontable(container, hotOptions);
    calculateSize(1);
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
    if (td != null && value != null) {
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
            eliminarError(celda, errorDespacho);
            if (value == "0") {
                $(td).html("0.000");
                if (typeof VALIDAR_CERO_BLANCO != 'undefined' && VALIDAR_CERO_BLANCO === true) {
                    td.style.background = errores[errorDespacho].BackgroundColor;
                    agregarError(celda, value, errorDespacho);
                }
            }
            else {
                if (value != "") {
                    if (!Number(value)) {
                        td.style.background = errores[errorNoNumero].BackgroundColor;
                        var celda = getExcelColumnName(parseInt(col) + 1) + (parseInt(row) + 1).toString();
                        agregarError(celda, value, errorNoNumero);
                    }
                } else {
                    if (typeof VALIDAR_CERO_BLANCO != 'undefined' && VALIDAR_CERO_BLANCO === true) {
                        td.style.background = errores[errorDespacho].BackgroundColor;
                        agregarError(celda, value, errorDespacho);
                    }
                }
            }
        }
    }

    if (typeof VALIDAR_CERO_BLANCO != 'undefined' && VALIDAR_CERO_BLANCO === true) {
        if (td != null && value == null) {
            eliminarError(celda, errorDespacho);
            agregarError(celda, "", errorDespacho);
        }
    }

    if (typeof VALIDAR_UNIDAD != 'undefined' && VALIDAR_UNIDAD === true) {
        validarUnidad(ht, row, col);
    }

    //$(ht.instance.getCell(row, col)).css(
    //    {
    //        "color": "black",
    //        "background-color": fondo,
    //        "vertical-align": "middle"
    //    });
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

function render_vertical_align(ht, row, col) {
    $(ht.instance.getCell(row, col)).css(
    {
        "vertical-align": "middle"
    });
}

function render_readonly(ht, row, col, tipo) {
    switch (tipo) {
        case -1:// solo lectura toda la matriz, Horas de operacion
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
        case 2: //MANTENIMIENTOS
            fondo = "MediumPurple "
            font_color = "DimGray";
            break;
        case 3: //EVENTO, rESTRICCIONES
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

function getCustomRenderer() {
    return function (instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        if (colsToHide.indexOf(col) > -1) {
            td.hidden = true;
        } else {
            td.hidden = false;
        }
    }
}

function getVisibleRender(ht, row, col) {
    td = ht.instance.getCell(row, col);
    if (td !== undefined && td != null) {
        if (colsToHide.indexOf(col) > -1) {
            td.hidden = true;
        } else {
            td.hidden = false;
        }
    }
}

function hideColumn(td, row, col) {
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

function calculateSize2(opcion) {
    var offset;
    offset = Handsontable.Dom.offset(container);
    alert(offset.top);
    //Verificar en que ventana esta, expandida -> 2 o extranet normal -> 1

    if (opcion == 1) {
        availableHeight = $(window).height() - offset.top - 10;
        //alert("In:" + availableHeight);
    }
    else {
        availableHeight = $(window).height() - offset.top - 80;
    }


    availableWidth = $(window).width() - 2 * offset.left;

    container.style.height = availableHeight + 'px';
    container.style.width = availableWidth + 'px';
    hot.render();
    //alert("v1:" + availableWidth + "  , top:  " + availableHeight);

}


function compararPto(a, b) {
    if (a.Ptomedicodi < b.Ptomedicodi)
        return -1;
    if (a.Ptomedicodi > b.Ptomedicodi)
        return 1;
    return 0;
}

function getListaGrupo() {
    dataPosGrupo = [];
    valorEquiCodi = '';
    valorEquinomb = '';
    valorPtoCodi = '';

    tmpEquiCodi = '';
    tmpEquinomb = '';
    tmpPtoCodi = '';

    listaPos = '';
    listaTipoInfo = '';

    //crear clone de la lista de ptos de medicion
    listaPtosTemp = [];
    for (i = 0; i < listaPtos.length; i++) {
        var pto = listaPtos[i];
        listaPtosTemp.push({ 'Ptomedicodi': pto.Ptomedicodi, 'Equinomb': pto.Equinomb, 'Equicodi': pto.EquiCodi, 'Tipoinfoabrev': pto.Tipoinfoabrev, 'Posicion': i });
    }

    //obtener las coordenadas
    listaPtosTemp.sort(compararPto);

    if (listaPtosTemp.length > 0) {
        valorEquiCodi = listaPtosTemp[0].Equicodi;
        valorEquinomb = listaPtosTemp[0].Equinomb;
        valorPtoCodi = listaPtosTemp[0].Ptomedicodi;
    }


    for (i = 0; i < listaPtosTemp.length; i++) {
        tmpEquiCodi = listaPtosTemp[i].Equicodi;
        tmpEquinomb = listaPtosTemp[i].Equinomb;
        tmpPtoCodi = listaPtosTemp[i].Ptomedicodi;

        if (valorPtoCodi != tmpPtoCodi) {
            listaPos = listaPos.substring(0, listaPos.length - 1);
            listaTipoInfo = listaTipoInfo.substring(0, listaTipoInfo.length - 1);

            //agregar si el grupo tiene MW y MVAR
            if (listaTipoInfo.indexOf('MW') !== -1 && listaTipoInfo.indexOf('MVAR') !== -1) {
                dataPosGrupo.push({ 'Ptomedicodi': valorPtoCodi, 'Equinomb': valorEquinomb, 'EquiCodi': valorEquiCodi, 'Pto': listaPos, 'Tipoinfoabrev': listaTipoInfo });
            }

            //cambiar valores
            valorPtoCodi = tmpPtoCodi;
            valorEquiCodi = tmpEquiCodi;
            valorEquinomb = tmpEquinomb;
            listaPos = listaPtosTemp[i].Posicion + ',';
            listaTipoInfo = listaPtosTemp[i].Tipoinfoabrev + ',';
        } else {
            listaPos += listaPtosTemp[i].Posicion + ',';
            listaTipoInfo += listaPtosTemp[i].Tipoinfoabrev + ',';
        }
    }

    listaPos = listaPos.substring(0, listaPos.length - 1);
    listaTipoInfo = listaTipoInfo.substring(0, listaTipoInfo.length - 1);
    dataPosGrupo.push({ 'Ptomedicodi': valorPtoCodi, 'Equinomb': valorEquinomb, 'EquiCodi': valorEquiCodi, 'Pto': listaPos, 'Tipoinfoabrev': listaTipoInfo });

    return dataPosGrupo;
}

function validarUnidad(ht, row, col) {
    if (ht.instance !== undefined) {
        data = ht.instance.getData();
        dataInvalidaByUnidad = [];
        dataPosGrupo = DATA_POSICION_PTO_MEDICION;

        //obtener todos los errores
        for (i = 0; i < dataPosGrupo.length; i++) {
            var listaPos = dataPosGrupo[i].Pto.split(",");
            for (k = 0; k < 48; k++) {//recorrer las siguientes 48 filas del grupo

                if (filasCab + k == row) {//buscamos la fila actual
                    var valores = [];
                    for (var j = 0; j < listaPos.length; j++) {//recorrer por unidad

                        var valor = data[filasCab + k][parseInt(listaPos[j]) + 1];
                        valores.push({ 'valor': valor, 'fila': filasCab + k, 'col': parseInt(listaPos[j]) + 1 });
                    }

                    //si todos los valores son vacios, hacer nada
                    var noHayNumero = true;
                    for (var m = 0; m < valores.length; m++) {
                        noHayNumero = noHayNumero && !Number(valores[m].valor);
                    }

                    //si hay un numero, entonces agregar coordenados que faltan data
                    if (!noHayNumero) {
                        for (var m = 0; m < valores.length; m++) {
                            if (!Number(valores[m].valor)) {
                                dataInvalidaByUnidad.push(
                                    [parseInt(valores[m].fila), parseInt(valores[m].col)]
                                );
                            }
                        }
                    }
                }
            }
        }

        //pintar los errores
        for (var i = 0; i < dataInvalidaByUnidad.length; i++) {
            row = dataInvalidaByUnidad[i][0];
            col = dataInvalidaByUnidad[i][1];

            value = ht.instance.getDataAtCell(row, col);
            td = ht.instance.getCell(row, col);

            if (value != null && td !== undefined) {
                var celda = getExcelColumnName(parseInt(col) + 1) + (parseInt(row) + 1).toString();

                eliminarError(celda, errorUnidad);
                td.style.background = errores[errorUnidad].BackgroundColor;
                var celda = getExcelColumnName(parseInt(col) + 1) + (parseInt(row) + 1).toString();
                agregarError(celda, value, errorUnidad);
            }
        }
    }
}

function updateDimensionHandson(hot) {
    var offset = Handsontable.Dom.offset(document.getElementById('detalleFormato'));
    var widthHT;
    var heightHT;

    if (offset.top == 222) {
        heightHT = $(window).height() - offset.top - 90;
    }
    else {
        heightHT = $(window).height() - offset.top - 20;
    }

    widthHT = $(window).width() - 2 * offset.left;

    hot.updateSettings({
        width: widthHT,
        height: heightHT
    })
}