/// Crea El objeto Handson en la pagina web
var container;

function crearGrillaFormatoTipo1(modelo, habilitarEditar, contenedor = 'detalleFormato') {

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

        if (contenedor == 'detalleFormatoE') {
            if (hotE != undefined && hotE != null)
                hotE.render();
        }
        else {
            if (hot != undefined && hot != null)
                hot.render();
        }


    }

    container = document.getElementById(contenedor);  //'detalleFormato'
    // maps function to lookup string
    Handsontable.renderers.registerRenderer('negativeValueRenderer', negativeValueRenderer);
    Handsontable.Dom.addEvent(window, 'resize', calculateSize);
    Handsontable.Dom.addEvent(container, 'click', function () {
        validaInicial = false;
        //hideMensajeEvento();
    });
    matriz = modelo.Handson.ListaExcelData;
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
            if ((row >= modelo.FilasCabecera) && (col >= modelo.ColumnasCabecera) && (!modelo.Handson.ReadOnly)) {
                if (modelo.Handson.ListaFilaReadOnly[row]) {
                    render = readOnlyValueRenderer;
                }
                else {
                    let objHandson = "";
                    (contenedor == 'detalleFormatoE') ? objHandson = evtHotE : objHandson = evtHot;
                    render_celda_color(this, objHandson, row, col);                                      

                }
                formato = '0,0.000';
                tipo = 'numeric';

            }
            for (var i in modelo.ListaCambios) {
                if ((row == modelo.ListaCambios[i].Row) && (col == modelo.ListaCambios[i].Col)) {
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

    if (contenedor == 'detalleFormatoE')
        hotE = new Handsontable(container, hotOptions);

    else {
        hot = new Handsontable(container, hotOptions);
    }

    calculateSize(1);
}

function calculateSize2(opcion) {
    var offset;
    offset = Handsontable.Dom.offset(container);
    console.log(container);
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

    let dataTabActivo = $("ul.etabs li.active").data("tabindex");
    (dataTabActivo == "0") ? hot.render() : hotE.render();

    //hot.render();
    //alert("v1:" + availableWidth + "  , top:  " + availableHeight);

}

function crearGrillaFormatoTipo2(modelo, contenedor = 'detalleFormato') {  //
    combo = modelo.ListaEmbalses;
    fechaDia = $("#hfFecha").val();
    ptosmedicion = modelo.ListaPtosSelect;
    grillaBD = modelo.Handson.ListaExcelData;
    agregarColumna();
    nFilasFor2 = grillaBD.length - 1;
    var optionsList = modelo.ListaPtoMedicion;
    listaPtos = modelo.ListaHojaPto;
    console.log("Valorizacion diaria");
    hideMsgFueraPlazo();
    
    container = document.getElementById(contenedor);  //detalleFormato

    var defaultRenderer = function (instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.backgroundColor = '#4682B4';
        td.style.fontSize = '11px';
    };

    var fechaRenderer = function (instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.backgroundColor = '#87CEEB';
        td.style.fontSize = '11px';
    };

    var timeRenderer = function (instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        $(".handsontableInput").inputmask({
            mask: "h:s:s",
            placeholder: "hh:mm:ss",
            alias: "datetime",
            hourFormat: "24"
        });
    };

    var limitesCellRenderer = function (instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontSize = '12px';
        td.style.textAlign = 'right';
        var columnaPtos = instance.getDataAtCol(1);
        if (value != null) {
            var celda = getExcelColumnName(parseInt(col) + 1) + (parseInt(row) + 1).toString();
            if (Number(value)) {
                indexPto = indexOfPto(columnaPtos[row]);
                if (Number(value) < listaPtos[indexPto].Hojaptoliminf) {
                    td.style.background = errores[errorLimInferior].BackgroundColor;
                    $(td).html(formatFloat(Number(value), 3, '.', ','));
                    agregarError(row, col, value, errorLimInferior, listaPtos[indexPto].Ptomedicodi);
                }
                else {
                    eliminarError(celda, errorLimInferior);
                    if (Number(value) > listaPtos[indexPto].Hojaptolimsup) {
                        td.style.background = errores[errorLimSuperior].BackgroundColor;
                        $(td).html(formatFloat(Number(value), 3, '.', ','));
                        var celda = getExcelColumnName(parseInt(col) + 1) + (parseInt(row) + 1).toString();
                        agregarError(row, col, value, errorLimSuperior, listaPtos[indexPto].Ptomedicodi);
                    }
                    else {
                        eliminarError(celda, errorLimSuperior);
                        td.style.background = 'white';
                        $(td).html(formatFloat(Number(value), 3, '.', ','));
                    }
                }
            }
            else {
                eliminarError(row, col, errorLimInferior);
                if (value == "0")
                    $(td).html("0.000");
                else if (value != "") {
                    if (!Number(value)) {
                        td.style.background = errores[errorNoNumero].BackgroundColor;
                        var celda = getExcelColumnName(parseInt(col) + 1) + (parseInt(row) + 1).toString();
                        agregarError(row, col, value, errorNoNumero, listaPtos[indexPto].Ptomedicodi);
                    }
                }
            }
        }
    }

    var horaRowRenderer = function (instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontSize = '11px';
        var columnaPtos = instance.getDataAtCol(1);
        if (col == 3 && value != null) {
            indexPto = indexOfPto(columnaPtos[row]);
            var celda = getExcelColumnName(parseInt(col) + 1) + (parseInt(row) + 1).toString();
            var columnaFechaIni = instance.getDataAtCol(col - 1);
            var errorRango = verificarRangoHoras(columnaFechaIni[row], value);
            if (errorRango) {

                agregarError(row, col, value, 5, listaPtos[indexPto].Ptomedicodi);
                td.style.color = errores[errorRangoFecha].Color;
                td.style.background = errores[errorRangoFecha].BackgroundColor;
            }
            else {
                eliminarError(celda, 5);
                if (verificaCruceRagoHora(instance, row)) {
                    td.style.color = errores[errorCrucePeriodo].Color;
                    td.style.background = errores[errorCrucePeriodo].BackgroundColor;
                    agregarError(row, col, value, 6, listaPtos[indexPto].Ptomedicodi);
                }
                else {
                    eliminarError(celda, 6);
                }
            }
        }
    }

    var datosBDRenderer = function (instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        //td.style.backgroundColor = '#DCDCDC';
        td.style.fontSize = '11px';
    };

    var tituloRenderer = function (instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.backgroundColor = '#4682B4';
        td.style.fontSize = '11px';
        td.style.color = '#FFFFFF'
    };

    var customDropdownRenderer = function (instance, td, row, col, prop, value, cellProperties) {
        var selectedId;
        td.style.fontSize = '11px';
        for (var index = 0; index < optionsList.length; index++) {
            if (parseInt(value) === optionsList[index].id) {
                selectedId = optionsList[index].id;
                value = optionsList[index].text;
            }
        }
        //td.className = "estilocombo";
        $(td).addClass("estilocombo");

        Handsontable.TextCell.renderer.apply(this, arguments);

        // you can use the selectedId for posting to the DB or server
        $('#selectedId').text(selectedId);
    }

    function safeHtmlRenderer(instance, td, row, col, prop, value, cellProperties) {
        var escaped = Handsontable.helper.stringify(value);
        escaped = strip_tags(escaped, '<em><b><strong><a><big>'); //be sure you only allow certain HTML tags to avoid XSS threats (you should also remove unwanted HTML attributes)
        td.innerHTML = escaped;

        return td;
    }

    function delete_row_renderer(instance, td, row, col, prop, value, cellProperties) {
        var div;
        // Remove existing buttons to avoid duplicating them.
        $(td).children('.btn').remove();
        div = document.createElement('div');
        div.className = 'btn';
        div.appendChild(document.createTextNode("."));
        td.appendChild(div);
        $(div).on('mouseup', function () {
            if (confirm("Desea eliminar?")) {
                eliminarFilaHandson();
                return instance.alter("remove_row", row);
            }
        });
        $(td).addClass("estilodelete");
        return td;
    }

    hotOptions = {
        data: grillaBD,
        height: 500,
        maxRows: modelo.Handson.ListaExcelData.length,
        maxCols: 6,
        colHeaders: true,//['FECHA', 'EMBALSE', 'HORA INICIO','HORA FIN','CAUDAL m/3','DESCRIPCIÓN'],
        rowHeaders: true,
        //stretchH: 'all',
        fillHandle: true,
        columnSorting: false,
        //contextMenu: true,
        //className: "htCenter",
        colWidths: modelo.Handson.ListaColWidth,
        //contextMenu: ['row_below', 'remove_row'],
        columns: [
            {},
            {},
            {
                type: 'time',
                timeFormat: 'HH:mm:ss',
                correctFormat: true
            },
            {
                type: 'time',
                timeFormat: 'HH:mm:ss',
                correctFormat: true
            },
            {},
            {
                maxLength: maxCadena
            },
            {}
        ],
        cells: function (row, col, prop) {

            var cellProperties = {};

            if (col == 0 || col == 1) {
                cellProperties.readOnly = true;
            }

            if (row == 0) {
                cellProperties.renderer = tituloRenderer;
                cellProperties.readOnly = true;
            }

            if (row > 0) {
                switch (col) {
                    case 0:
                        render_vertical_align(this, row, col);
                        cellProperties.renderer = fechaRenderer;
                        break;
                    case 1:
                        cellProperties.editor = 'select2';
                        cellProperties.renderer = customDropdownRenderer;
                        cellProperties.width = "200px";
                        cellProperties.select2Options = {
                            data: optionsList,
                            dropdownAutoWidth: true,
                            width: 'resolve'
                        };
                        break;
                    case 2: case 3:
                        render_vertical_align(this, row, col);
                        cellProperties.renderer = horaRowRenderer;

                        break;
                    case 4:
                        cellProperties.format = '0,0.000';
                        cellProperties.type = 'numeric';
                        render_celda_color2(this, evtHot, row, col);
                        break;
                    case 6:
                        cellProperties.renderer = delete_row_renderer;
                        break;
                    default:
                        cellProperties.renderer = datosBDRenderer;
                        break;
                }

                // cellProperties.readOnly = true;
            }

            return cellProperties;
        },
        afterCreateRow: function (index, amount) {
            hot.setDataAtCell(index, 0, fechaDia);
            hot.setDataAtCell(index, 2, getHoraInicio());
            hot.setDataAtCell(index, 3, getHoraFin());
            hot.setDataAtCell(index, 1, optionsList[0].id);
            hot.setDataAtCell(index, 4, "0");
        }
    };

    //hot = new Handsontable(container, hotOptions);
    if (contenedor == 'detalleFormatoE')
        hotE = new Handsontable(container, hotOptions);

    else {
        hot = new Handsontable(container, hotOptions);
    }
        
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
        if (Number(value) != NaN && (value + "") != "") {
            limiteInf = listaPtos[col - 1].Hojaptoliminf;
            limiteSup = listaPtos[col - 1].Hojaptolimsup;
            if (Number(value) < limiteInf) {
                td.style.background = errores[errorLimInferior].BackgroundColor;
                $(td).html(formatFloat(Number(value), 3, '.', ','));
                //mostrarAlerta("Error al ingresar valores, verifique la celda de color o de clic en Ver Errores");
                //mostrarMsgErrorValores();
                mostrarAletarErrorValores("Error al ingresar valores, verifique la celda de color o de clic en Ver Errores");
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
                    document.getElementById("mensajeEvento").style.display = "none";
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
                    agregarError(row, col, value, errorNoNumero, listaPtos[col - 1].Ptomedicodi);
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

function render_celda_color2(ht, evtHot, row, col) {
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
    var columnaPtos = ht.instance.getDataAtCol(1);

    if (value != null) {
        celda = getExcelColumnName(parseInt(col) + 1) + (parseInt(row) + 1).toString();
        if (Number(value) && (value + "") != "") {
            indexPto = indexOfPto(columnaPtos[row]);
            limiteInf = listaPtos[indexPto].Hojaptoliminf;
            limiteSup = listaPtos[indexPto].Hojaptolimsup;
            if (Number(value) < limiteInf) {
                td.style.background = errores[errorLimInferior].BackgroundColor;
                $(td).html(formatFloat(Number(value), 3, '.', ','));
                eliminarError(celda, errorLimInferior);
                agregarError(row, col, value, errorLimInferior, listaPtos[indexPto].Ptomedicodi);
            }
            else {
                eliminarError(celda, errorLimInferior);
                if (Number(value) > limiteSup) {
                    td.style.background = errores[errorLimSuperior].BackgroundColor;
                    $(td).html(formatFloat(Number(value), 3, '.', ','));
                    var celda = getExcelColumnName(parseInt(col) + 1) + (parseInt(row) + 1).toString();
                    eliminarError(celda, errorLimSuperior);
                    agregarError(row, col, value, errorLimSuperior, listaPtos[indexPto].Ptomedicodi);
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
                    agregarError(row, col, value, errorNoNumero, listaPtos[indexPto].Ptomedicodi);
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

