/// Crea El objeto Handson en la pagina web
var container;

function crearGrillaFormatoTipo1(modelo, habilitarEditar, val_envio) {
    
    ptosmedicion = modelo.ListaHojaPto;
    mostrarmensajes(modelo.EnPlazo, modelo.IdEnvio, modelo.FechaEnvio, modelo.TipoPlazo, habilitarEditar, modelo.EsEmpresaVigente);

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
        td.style.background = 'LightSkyBlue';
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
        td.style.color = 'black';
        td.style.background = '#FFFFD7';
        if (parseFloat(value))
            $(td).html(formatFloat(parseFloat(value), 3, '.', ','));
        else {
            if (value == "0")
                $(td).html("0.000");
        }
    }

    function negativeValueRenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        // if row contains negative number
        if (parseInt(value, 10) < 0) {
            // add class "negative"
            td.style.color = 'orange';
            td.style.fontStyle = 'italic';
            //td.className = 'make-me-red';
        }
    }

    function limitesCellRenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontSize = '12px';
        td.style.textAlign = 'right';
        if (Number(value) && value != "") {
            console.log("Entro :" + value);
            if (Number(value) < ptosmedicion[col - 1].Hojaptoliminf) {
                td.style.background = errores[errorLimInferior].BackgroundColor;
                $(td).html(formatFloat(Number(value), 3, '.', ','));
                var celda = getExcelColumnName(parseInt(col) + 1) + (parseInt(row) + 1).toString();
                agregarError(row, col, value, 3, ptosmedicion[col - 1].Ptomedicodi);

            }
            else {
                if (Number(value) > ptosmedicion[col - 1].Hojaptolimsup) {
                    td.style.background = errores[errorLimSuperior].BackgroundColor;
                    $(td).html(formatFloat(Number(value), 3, '.', ','));
                    var celda = getExcelColumnName(parseInt(col) + 1) + (parseInt(row) + 1).toString();
                    agregarError(row, col, value, 4, ptosmedicion[col - 1].Ptomedicodi);
                }
                else {
                    td.style.background = 'white';
                    $(td).html(formatFloat(Number(value), 3, '.', ','));
                }
            }
        }
        else {
            console.log("No Entro:" + value);
            if (value == "0")
                $(td).html("0.000");
            else if (value != "") {
                if (!Number(value)) {
                    td.style.background = 'red';
                    var celda = getExcelColumnName(parseInt(col) + 1) + (parseInt(row) + 1).toString();
                    agregarError(row, col, value, 2, ptosmedicion[col - 1].Ptomedicodi);
                }
            }
        }
    }

    function readOnlyValueRenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontSize = '12px';
        td.style.textAlign = 'center';
        td.style.color = 'DimGray';
        td.style.background = 'Gainsboro';
        if (parseFloat(value))
            $(td).html(formatFloat(parseFloat(value), 3, '.', ','));
        else {
            if (value == "0")
                $(td).html("0.000");
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
        if (hot != undefined && hot != null)
            hot.render();
        //alert("v1:" + availableWidth + "  , top:  " + availableHeight);

    }

    container = document.getElementById('detalleFormato');
    // maps function to lookup string
    Handsontable.renderers.registerRenderer('negativeValueRenderer', negativeValueRenderer);
    Handsontable.Dom.addEvent(window, 'resize', calculateSize);
    Handsontable.Dom.addEvent(container, 'click', function () {
        validaInicial = false;
        hideMensajeEvento();
    });

    matriz = modelo.Handson.ListaExcelData;
    myDate = new Date();
    hours = myDate.getHours();
    var _filamatriz = modelo.FilasCabecera - 1;
   
    for (var i = 0; i < this.matriz.length; i++) {
        if (i <= _filamatriz)
            P = ""
        else {
            if (matriz[i][0].substring(11, 13) <= val_envio)
                P = true;
            else if (parseInt(val_envio) == 24)
                P = true;
            else
                P = false;
        }
        this.matriz[i].push(P);
    }
    

    hotOptions = {
        data: matriz,
        height: 500,
        maxRows: matriz.length,
        maxCols: matriz[0].length,
        colHeaders: true,
        rowHeaders: true,
        fillHandle: true,
        columnSorting: false,
        className: "htCenter",
        readOnly: modelo.Handson.ReadOnly,
        fixedRowsTop: modelo.FilasCabecera,
        fixedColumnsLeft: modelo.ColumnasCabecera,
        mergeCells: modelo.Handson.ListaMerge,
        colWidths: modelo.Handson.ListaColWidth,
        afterLoadData: function () {
            this.render();
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
                if (!modelo.Handson.ReadOnly) {//Primero prevalece el readonly de todo el excel
                    readOnly = modelo.Handson.ListaFilaReadOnly[row];
                }
                else {
                    render = readOnlyValueRenderer;
                }
            }
            if (row < modelo.FilasCabecera) {
                if (row == 0) {
                    var indMerge = findIndiceMerge(col, modelo.Handson.ListaMerge);
                    if ((indMerge % 2) == 0) {
                        render = firstRowRenderer;
                    }
                    else {
                        render = firstRowRenderer2;
                    }
                }
                else {
                    if ((row % 2) == 0) {
                        render = descripRowRenderer;
                    }
                    else {
                        render = descripRowRenderer2;
                    }
                }
            }
            if ((row >= modelo.FilasCabecera) && (col >= modelo.ColumnasCabecera) && (col == matriz[0].length - 1) && (modelo.Handson.ListaFilaReadOnly[row] == true)) {
                tipo = 'checkbox';
                render = checkRenderer;
            }
            else if ((row >= modelo.FilasCabecera) && (col >= modelo.ColumnasCabecera) && (col == matriz[0].length - 1) && (modelo.Handson.ReadOnly)) {
                tipo = 'checkbox';
                render = checkRenderer;
            }
            else if ((row >= modelo.FilasCabecera) && (col >= modelo.ColumnasCabecera) && (!modelo.Handson.ReadOnly)) {

                if (modelo.Handson.ListaFilaReadOnly[row]) {
                    render = readOnlyValueRenderer;
                }
                else {
                    render_celda_color3(this, evtHot, row, col)
                }
                if (col == matriz[0].length - 1) {
                    tipo = 'checkbox';
                }
                else {
                    formato = '0,0.000';
                    tipo = 'numeric';
                }

            }
             

            for (var i in modelo.ListaCambios) {
                if ((row == modelo.ListaCambios[i].Row) && (col == modelo.ListaCambios[i].Col) && (col < matriz[0].length - 1)) {
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

function render_celda_color3(ht, evtHot, row, col) {
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

       if (isNaN(value) != true && (value + "") != "" && col <= evtHot.ListaHojaPto.length) {

            limiteInf = listaPtos[col - 1].Hojaptoliminf;
            limiteSup = listaPtos[col - 1].Hojaptolimsup;
            if (Number(value) < limiteInf) {
                td.style.background = errores[errorLimInferior].BackgroundColor;
                $(td).html(formatFloat(Number(value), 3, '.', ','));
                eliminarError(celda, errorLimInferior);
                agregarError(row, col, value, errorLimInferior, listaPtos[col - 1].Ptomedicodi);
            }
            else {
                eliminarError(celda, errorLimInferior);
                if (Number(value) > limiteSup) {
                    td.style.background = errores[errorLimSuperior].BackgroundColor;
                    $(td).html(formatFloat(Number(value), 3, '.', ','));
                    var celda = getExcelColumnName(parseInt(col) + 1) + (parseInt(row) + 1).toString();
                    eliminarError(celda, errorLimSuperior);
                    agregarError(row, col, value, errorLimSuperior, listaPtos[col - 1].Ptomedicodi);
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
           if (value.toString() == "0")
               $(td).html("0.000");
           else if (value != "") {
               if (!Number(value)) {
                   td.style.background = errores[errorNoNumero].BackgroundColor;
                   var celda = getExcelColumnName(parseInt(col) + 1) + (parseInt(row) + 1).toString();
                   agregarError(row, col, value, errorNoNumero, listaPtos[col - 1].Ptomedicodi);
               }
           }
           else if (value == "" && col <= evtHot.ListaHojaPto.length && listaPtos[col - 1].Famcodi == 42) {
               td.style.background = errores[errorBlanco].BackgroundColor;
               var celda = getExcelColumnName(parseInt(col) + 1) + (parseInt(row) + 1).toString();
               agregarError(row, col, value, errorBlanco, listaPtos[col - 1].Ptomedicodi);
           }
           else if (value == "" && ht.instance.getDataAtCell(row, evtHot.ListaHojaPto.length + 1) == true) {
               td.style.background = errores[errorBlanco].BackgroundColor;
               var celda = getExcelColumnName(parseInt(col) + 1) + (parseInt(row) + 1).toString();
               agregarError(row, col, value, errorBlanco, listaPtos[col - 1].Ptomedicodi);
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

function render_time(ht, row, col) {
    $(ht.instance.getCell(row, col)).inputmask({
        mask: "h:s:s",
        placeholder: "hh:mm:ss",
        alias: "datetime",
        hourFormat: "24"
    });
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