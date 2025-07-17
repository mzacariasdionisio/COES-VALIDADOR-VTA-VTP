/// Crea El objeto Handson en la pagina web
var container;
var arrayFilasVacias = new Array;
function crearHandsonTable() {
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
        $(td).html(formatFloat(parseFloat(value), 3, '.', ','));
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
                agregarError(celda, value, 3, row);
                
            }
            else {
                if (Number(value) > evtHot.ListaHojaPto[col - 1].Hojaptolimsup) {
                    td.style.background = 'orange';
                    $(td).html(formatFloat(Number(value), 3, '.', ','));
                    var celda = getExcelColumnName(parseInt(col) + 1) + (parseInt(row) + 1).toString();
                    agregarError(celda, value, 4, row);
                }
                else {
                    //if (evtHot.Handson.ListaExcelDescripcion[row][col] == "1") {
                    //    td.style.background = 'yellow';
                    //} else {
                    //    td.style.background = 'white';
                    //}
                    td.style.background = 'white';
                    $(td).html(formatFloat(Number(value), 3, '.', ','));
                }
            }
        }
        else {
            if (value == "0") {
                $(td).html("0.000");
            } else if (value == "") {
                //if (!Number(value)) {
                    td.style.background = 'red';
                    var celda = getExcelColumnName(parseInt(col) + 1) + (parseInt(row) + 1).toString();
                    agregarError(celda, value, 5, row);
                //}
            } else if (value != "") {
                if (!Number(value)) {
                    td.style.background = 'red';
                    var celda = getExcelColumnName(parseInt(col) + 1) + (parseInt(row) + 1).toString();
                    agregarError(celda, value, 2, row);
                }
            }
        }

    }

    function limitesCellRendererSinError(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontSize = '12px';
        td.style.textAlign = 'right';
        if (Number(value) && value != "") {
            if (Number(value) < evtHot.ListaHojaPto[col - 1].Hojaptoliminf) {
                td.style.background = 'orange';
                $(td).html(formatFloat(Number(value), 3, '.', ','));
                var celda = getExcelColumnName(parseInt(col) + 1) + (parseInt(row) + 1).toString();
                //agregarError(celda, value, 3, row);

            }
            else {
                if (Number(value) > evtHot.ListaHojaPto[col - 1].Hojaptolimsup) {
                    td.style.background = 'orange';
                    $(td).html(formatFloat(Number(value), 3, '.', ','));
                    var celda = getExcelColumnName(parseInt(col) + 1) + (parseInt(row) + 1).toString();
                    //agregarError(celda, value, 4, row);
                }
                else {
                    //if (evtHot.Handson.ListaExcelDescripcion[row][col] == "1") {
                    //    td.style.background = 'yellow';
                    //} else {
                    //    td.style.background = 'white';
                    //}
                    td.style.background = 'white';
                    $(td).html(formatFloat(Number(value), 3, '.', ','));
                }
            }
        }
        else {
            if (value == "0") {
                $(td).html("0.000");
            } else if (value == "") {
                //if (!Number(value)) {
                //td.style.background = 'red';
                //var celda = getExcelColumnName(parseInt(col) + 1) + (parseInt(row) + 1).toString();
                //agregarError(celda, value, 5, row);
                //}
            } else if (value != "") {
                if (!Number(value)) {
                    td.style.background = 'red';
                    var celda = getExcelColumnName(parseInt(col) + 1) + (parseInt(row) + 1).toString();
                    //agregarError(celda, value, 2, row);
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
        if (evtHot.Handson.ListaExcelDescripcion[row][col] == "1") {
            td.style.background = '#FFFFD7';
        }
        
        if (parseFloat(value))
            $(td).html(formatFloat(parseFloat(value), 3, '.', ','));
        else {
            if(value == "0")
                $(td).html("0.000");
        }
    }

    function calculateSize() {
        var offset;
        offset = Handsontable.Dom.offset(container);
        
        if (offset.top == 222) {
            availableHeight = $(window).height() - offset.top - 90 + 280;
        }
        else {
            availableHeight = $(window).height() - offset.top - 20 + 136;
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
        height: 800,
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
                        agregarError(celda, changes[i][3], tipoNew, parseInt(changes[i][0]));
                    }
                }
                else {
                    if (tipoNew > 1) {
                        agregarError(celda, changes[i][3], tipoNew, parseInt(changes[i][0]));
                    }
                }
            }
        },

        cells: function (row, col, prop) {
            var cellProperties = {};
            var formato = "";
            var render;
            var readOnly=true;
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
            //var blnAnioMinimo = true;
            //var intAnioMinimo = 0;
            //var valhdfAnioMinimo = $('#hdfAnioMinimo').val();
            //var arrayFilasVacias = new Array;
            if ((row >= evtHot.FilasCabecera) && (col >= evtHot.ColumnasCabecera) && (!evtHot.Handson.ReadOnly)) {
                
                if (evtHot.Handson.ListaFilaReadOnly[row]) {
                    render = readOnlyValueRenderer;
                } else {
                    //render = limitesCellRenderer;
                    if ((evtHot.Handson.ListaExcelData[row][1] == "") && (evtHot.Handson.ListaExcelData[row][2] == "") && (evtHot.Handson.ListaExcelData[row][3] == "") && (evtHot.Handson.ListaExcelData[row][4] == "") && (evtHot.Handson.ListaExcelData[row][5] == "") && (evtHot.Handson.ListaExcelData[row][6] == "") && (evtHot.Handson.ListaExcelData[row][7] == "") && (evtHot.Handson.ListaExcelData[row][8] == "") && (evtHot.Handson.ListaExcelData[row][9] == "") && (evtHot.Handson.ListaExcelData[row][10] == "") && (evtHot.Handson.ListaExcelData[row][11] == "") && (evtHot.Handson.ListaExcelData[row][12] == "")) {
                        
                        arrayFilasVacias[row] = 1;
                        //render = limitesCellRendererSinError;
                        var blnError = verificarErrorFilaVacia(row);
                        if ($("#hfOpcion").val() == 'Consultar') {
                            render = limitesCellRendererSinError;
                        } else {
                            if (blnError == true) {
                                render = limitesCellRenderer;
                            } else {
                                render = limitesCellRendererSinError;
                            }
                        }

                        setTimeout(() => {
                            //console.log("Delayed for 2 second.");
                            eliminarErrorFila(row);
                        }, "1000");
                        
                    } else {
                        //blnAnioMinimo = false;
                        arrayFilasVacias[row] = 0;
                        render = limitesCellRenderer;
                    }
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

function verificarErrorFilaVacia(row) {
    var blnTerminaAnio = true;
    var arrayFilasInicio = new Array;
    var numAnios = 0;
    var blnError = false;
    if (arrayFilasVacias) {
        if ($('#cbTipoPuntoMedicion').val() != "7") {
            if (arrayFilasVacias[row] == 1) {
                blnError = true;
            }
        } else {

            for (i = 1; i <= arrayFilasVacias.length; i++) {
                if ((arrayFilasVacias[i] == 1) && (blnTerminaAnio)) {
                    arrayFilasInicio[i] = 1;
                } else {
                    blnTerminaAnio = false;
                }
            }
            if ((arrayFilasVacias[row] == 1) && (arrayFilasInicio[row] == 1)) {
                blnError = false; //console.log('es fila inicial y esta vacia');
            } else {
                blnError = true; //console.log('no es fila inicial y esta vacia');
            }

        }



    }
    return blnError;

}

function calculateSize2(opcion) {
    offset = Handsontable.Dom.offset(container);    

    if (opcion == 1) {
        availableHeight = $(window).height() - offset.top - 10 + 280;        
    }
    else {
        availableHeight = $(window).height() - offset.top - 80 +  136;
    }
    
    availableWidth = $(window).width() - 2 * offset.left;    
    container.style.height = availableHeight + 'px';
    container.style.width = availableWidth + 'px';
    hot.render();   

}

function crearHandsonTable2()
{
    container = document.getElementById('detalleFormato');
    hotOptions = {
        data: evtHot.Handson.ListaExcelData,
        height: 800,
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

//crear jason
function formatJavaScriptSerializer(ptos) {

    if (ptos != null) {

        if (evtHot.Handson.ListaExcelData.length == 4) {
            return formatJavaScriptSerializerVI(ptos);
        }
        else
        {
            var total = ptos.length;
            var cab = evtHot.Formato.Formatrows;
            var lista = [];
            var matriz = evtHot.Handson.ListaExcelData;
            var valor = 0.0;

            var stValor = "";
            var nCol = total + 1;
            var nFil = 24 * 2 * evtHot.Formato.Formathorizonte;
            var checkBlanco = evtHot.Formato.Formatcheckblanco;

            var contador = 0;
            for (var i = 1; i < nCol; i++) {
                for (var j = 0; j < nFil; j++) {
                    //verificar inicio de dia
                    if ((j % 48) == 0) {
                        if (j != 0) {
                            lista[contador] = pnto;
                            contador++;
                        }
                        pnto = new Object();
                        pnto.Ptomedicodi = ptos[i - 1].Ptomedicodi;
                        //pnto.Lectcodi = 226;
                        pnto.Meditotal = 0;
                        pnto.Tipoinfocodi = ptos[i - 1].Tipoinfocodi;
                        pnto.Emprcodi = ptos[i - 1].Emprcodi;

                        var fechaPto = matriz[j + cab][0];
                        pnto.MedifechaPto = fechaPto;

                        stValor = matriz[j + cab][i];

                        stValor = stValor == "" ? stValor : parseFloat(stValor);

                        if (typeof stValor == 'number') {
                            valor = parseFloat(stValor);
                            pnto.H1 = valor;
                        }
                        else {
                            if (checkBlanco == 0)
                                pnto.H1 = null;
                            else
                                pnto.H1 = 0;
                        }
                    }
                    else {
                        var indice = j % 48 + 1;
                        stValor = matriz[j + cab][i];
                        stValor = stValor == "" ? stValor : parseFloat(stValor);
                        if (typeof stValor == 'number') {
                            valor = parseFloat(stValor);
                            pnto["H" + indice.toString()] = valor;
                        }
                        else {
                            if (checkBlanco == 0)
                                pnto["H" + indice.toString()] = null;
                            else
                                pnto["H" + indice.toString()] = 0;
                        }
                    }
                }
                lista[contador] = pnto;
                contador++;
            }
            return lista;
        }
    }
    return {};
}


//crear jason Volumen Inicial
function formatJavaScriptSerializerVI(ptos) {
    var total = ptos.length;
    var lista = [];
    var matriz = evtHot.Handson.ListaExcelData;
    var valor = 0.0;

    var stValor = "";
    var nCol = 3;
    var nFil = total;
    var checkBlanco = evtHot.Formato.Formatcheckblanco;

    var contador = 0;
    var countPtos = 0;
    for (var i = 1; i <= nFil; i += 3) {
        countPtos++;
        for (var j = 0; j < nCol; j++) {
            pnto = new Object();
            pnto.Ptomedicodi = ptos[i - 1].Ptomedicodi;
            pnto.Meditotal = 0;
            pnto.Tipoptomedicodi = ptos[contador].Tptomedicodi;
            pnto.Tipoinfocodi = ptos[i - 1].Tipoinfocodi;
            pnto.Emprcodi = ptos[i - 1].Emprcodi;

            stValor = matriz[countPtos + 1][j + 1];

            stValor = stValor == "" ? stValor : parseFloat(stValor);

            if (typeof stValor == 'number') {
                valor = parseFloat(stValor);
                pnto.H1 = valor;
            }
            else {
                if (checkBlanco == 0)
                    pnto.H1 = null;
                else
                    pnto.H1 = 0;
            }

            lista[contador] = pnto;
            contador++;
        }
    }

    return lista;
}
