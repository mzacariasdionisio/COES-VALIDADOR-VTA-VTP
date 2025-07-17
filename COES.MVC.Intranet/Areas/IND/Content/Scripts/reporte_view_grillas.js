var container;
var previoChange = 0;
var HoraIniPunta = 1020;
var HoraFinPunta = 1380;

var CUADRO_1 = 1;
var CUADRO_2 = 2;
var CUADRO_FACTOR_K = 3;
var CUADRO_CUADRO5 = 5;
var CUADRO_CUADRO7 = 7;

var TAB_FORTUITO = 0;
var TAB_PROGRAMADO = 1;

var FORMATO_HTML_EVENTO = 'EVENTO';
var FORMATO_HTML_RESTRIC = 'RESTRIC';
var FORMATO_HTML_ALERTA = 'ALERTA';
var FORMATO_HTML_LIMCOMB = 'LIMCOMB';

const COL_CENTRAL = 2;

var COL_PE_CUADROASG = 5;
var COL_PA_CUADROASG = 6;

var COL_PE_FACTORK = 5;
var COL_K_FACTORK = 6;
var COL_DESC_FACTORK = 7;
var tablaCambios;
var LISTA_CELDA_CAMBIOS = [];

function crearGrillaExcel(tab, container, handson, heightHansonTab) {
    var ColorEmpresa = function (instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.backgroundColor = '#4F81BD';
        td.style.fontSize = '11px';
        td.style.color = 'white'
    };
    var ColorContent = function (instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.backgroundColor = '#D9D9D9';
        td.style.fontSize = '11px';
        td.style.color = 'black'
    };
    var LateralIzq = function (instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.backgroundColor = '#DCE6F1';
        td.style.fontSize = '11px';
        td.style.color = 'black';
    };

    var columns = handson.Columnas;
    var headers = handson.Headers;
    var widths = handson.ListaColWidth;
    var data = handson.ListaExcelData;
    var dataDescrip = handson.ListaExcelDescripcion;
    var dataFormato = handson.ListaExcelFormatoHtml;
    var arrMergeCells = handson.ListaMerge;
    var listaCambios = handson.ListaCambios;

    LISTA_HoT[tab] = new Handsontable(container, {
        data: data,
        stretchH: "all",
        observeChanges: true,
        colHeaders: headers,
        colWidths: widths,
        rowHeaders: true,
        columnSorting: false,
        minSpareRows: 0,
        readOnly: true,
        columns: columns,
        height: heightHansonTab,
        mergeCells: arrMergeCells,
        fixedColumnsLeft: 7,
        contextMenu: {
            items: {
                "Nuevo": {
                    name: 'Nuevo',
                    callback: function (key, selection, clickEvent) {
                        initPopupNuevo(obtenerDataFila(this, selection));
                    }
                },
                "Modificar": {
                    name: 'Modificar',
                    callback: function (key, selection, clickEvent) {
                        initPopupModificar(obtenerDataFila(this, selection));
                    }
                },
                "Eliminar": {
                    name: 'Eliminar',
                    callback: function (key, selection, clickEvent) {
                        initPopupEliminar(obtenerDataFila(this, selection))
                    }
                },

            }
        },
        cells: function (row, col, prop) {
            var cellProperties = {
                correctFormat: true
            };

            if (col == 0 && row >= 0) {
                cellProperties.renderer = ColorEmpresa;
            }
            if (col > 0 && col < 6 && row >= 0) {
                cellProperties.renderer = LateralIzq;
            }
            var restoRow = row % 3;
            if (restoRow == 2 && col > 3 && row > 0) {
                cellProperties.renderer = ColorContent;
                cellProperties.readOnly = true;
            }
            if (row >= 0) {
                switch (col) {
                    case 0: cellProperties.className = "htCenter htMiddle"; cellProperties.readOnly = true; break;
                    case 1: cellProperties.className = "htCenter htMiddle"; cellProperties.readOnly = true; break;
                    case 2: cellProperties.className = "htCenter htMiddle"; cellProperties.readOnly = true; break;
                    case 3: cellProperties.className = "htCenter htMiddle"; cellProperties.readOnly = true; break;
                    case 4: cellProperties.className = "htCenter htMiddle"; cellProperties.readOnly = true; break;
                    case 5: cellProperties.className = "htCenter htMiddle"; cellProperties.readOnly = true; break;
                    case 6: cellProperties.className = "htCenter htMiddle"; cellProperties.readOnly = true; break;
                    default:

                        cellProperties.className = "htCenter htMiddle";
                        cellProperties.renderer = render_ColorResult(this, row, col, tab, dataDescrip, dataFormato, listaCambios);
                        break;
                }
            }
            return cellProperties;
        }
    });
}

function crearGrillaExcelCuadro2(tab, container, handson, heightHansonTab) {
    var ColorEmpresa = function (instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.backgroundColor = '#4F81BD';
        td.style.fontSize = '11px';
        td.style.color = 'white'
    };
    var ColorContent = function (instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.backgroundColor = '#D9D9D9';
        td.style.fontSize = '11px';
        td.style.color = 'black'
    };
    var LateralIzq = function (instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.backgroundColor = '#DCE6F1';
        td.style.fontSize = '11px';
        td.style.color = 'black';
    };

    var columns = handson.Columnas;
    var headers = handson.Headers;
    var widths = handson.ListaColWidth;
    var data = handson.ListaExcelData;
    var dataDescrip = handson.ListaExcelDescripcion;
    var dataFormato = handson.ListaExcelFormatoHtml;
    var arrMergeCells = handson.ListaMerge;
    var listaCambios = handson.ListaCambios;

    LISTA_HoT[tab] = new Handsontable(container, {
        data: data,
        stretchH: "all",
        observeChanges: true,
        colHeaders: headers,
        colWidths: widths,
        rowHeaders: true,
        columnSorting: false,
        contextMenu: false,
        minSpareRows: 0,
        readOnly: true,
        columns: columns,
        height: heightHansonTab,
        mergeCells: arrMergeCells,
        fixedColumnsLeft: 8,
        contextMenu: {
            items: {
                "Nuevo": {
                    name: 'Nuevo',
                    callback: function (key, selection, clickEvent) {
                        initPopupNuevo(obtenerDataFila2(this, selection));
                    }
                },
                "Modificar": {
                    name: 'Modificar',
                    callback: function (key, selection, clickEvent) {
                        initPopupModificar(obtenerDataFila2(this, selection));
                    }
                },
                "Eliminar": {
                    name: 'Eliminar',
                    callback: function (key, selection, clickEvent) {
                        initPopupEliminar(obtenerDataFila2(this, selection))
                    }
                },

            }
        },
        cells: function (row, col, prop) {
            var cellProperties = {
            };

            if (col == 0 && row >= 0) {
                cellProperties.renderer = ColorEmpresa;
            }
            if (col > 0 && col < 8 && row >= 0) {
                cellProperties.renderer = LateralIzq;
            }
            var restoRow = row % 5;
            if (restoRow >= 2 && col > 4 && col <= 7 && row > 0) {
                cellProperties.renderer = ColorContent;
                cellProperties.readOnly = true;
            }
            if (row >= 0) {
                switch (col) {
                    case 0: cellProperties.className = "htCenter htMiddle"; cellProperties.readOnly = true; break;
                    case 1: cellProperties.className = "htCenter htMiddle"; cellProperties.readOnly = true; break;
                    case 2: cellProperties.className = "htCenter htMiddle"; cellProperties.readOnly = true; break;
                    case 3: cellProperties.className = "htCenter htMiddle"; cellProperties.readOnly = true; break;
                    case 4: cellProperties.className = "htCenter htMiddle"; cellProperties.readOnly = true; break;
                    case 5: cellProperties.className = "htCenter htMiddle"; cellProperties.readOnly = true; break;
                    case 6: cellProperties.className = "htCenter htMiddle"; cellProperties.readOnly = true; break;
                    case 7: cellProperties.className = "htCenter htMiddle"; cellProperties.readOnly = true; break;
                    default:

                        cellProperties.className = "htCenter htMiddle";
                        cellProperties.renderer = render_ColorResult_Cuadro2(this, row, col, tab, dataDescrip, dataFormato, listaCambios);
                        break;
                }
            }
            return cellProperties;
        }
    });
}

function crearGrillaExcelFactorK(tab, container, handson, heightHansonTab) {
    var ColorEmpresa = function (instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.backgroundColor = '#4F81BD';
        td.style.fontSize = '11px';
        td.style.color = 'white'
    };
    var ColorContent = function (instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.backgroundColor = '#D9D9D9';
        td.style.fontSize = '11px';
        td.style.color = 'black'
    };
    var LateralIzq = function (instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.backgroundColor = '#DCE6F1';
        td.style.fontSize = '11px';
        td.style.color = 'black';
    };

    var columns = handson.Columnas;
    var headers = handson.Headers;
    var widths = handson.ListaColWidth;
    var data = handson.ListaExcelData;
    var dataDescrip = handson.ListaExcelDescripcion;
    var dataFormato = handson.ListaExcelFormatoHtml;
    var arrMergeCells = handson.ListaMerge;
    var listaCambios = handson.ListaCambios;

    LISTA_HoT[tab] = new Handsontable(container, {
        data: data,
        stretchH: "all",
        observeChanges: true,
        colHeaders: headers,
        colWidths: widths,
        rowHeaders: true,
        columnSorting: false,
        minSpareRows: 0,
        readOnly: true,
        columns: columns,
        height: heightHansonTab,
        mergeCells: arrMergeCells,
        fixedColumnsLeft: 8, //IND.PR25.2022
        contextMenu: {
            items: {
                "Modificar": {
                    name: 'Modificar',
                    callback: function (key, selection, clickEvent) {
                        initPopupModificarFactorK(obtenerDataFilaFactorK(this, selection));
                    }
                }
            }
        },
        cells: function (row, col, prop) {
            var cellProperties = {
            };

            if (col == 0 && row >= 0) { 
                cellProperties.renderer = ColorEmpresa;
            }
            if (col > 0 && col < 4 && row >= 0) { 
                cellProperties.renderer = LateralIzq;
            }

            if (row >= 0) {
                switch (col) {
                    case 0:
                    case 1:
                    case 2:
                    case 3:
                        cellProperties.className = "htCenter htMiddle"; cellProperties.readOnly = true;
                        break;
                    case 4:
                        cellProperties.renderer = render_a_incremental;
                        break;
                    //case 5:
                    //    cellProperties.className = "htRight htMiddle";
                    //    cellProperties.type = 'numeric';
                    //    cellProperties.format = '0,0.000';
                    //    break;
                    case 5:
                        cellProperties.className = "htRight htMiddle";
                        cellProperties.type = 'numeric';
                        cellProperties.format = '0,0.000';
                        cellProperties.renderer = render_diferente_k_pe;
                        break;
                    case 6:
                        cellProperties.className = "htRight htMiddle";
                        cellProperties.type = 'numeric';
                        cellProperties.format = '0,0.0000';
                        cellProperties.renderer = render_diferente_factork;
                        break;
                    case 7:
                        cellProperties.className = "htLeft htMiddle";
                        cellProperties.renderer = render_diferente_factork_obs;
                        break;
                    case 8:
                        break;
                    case 9:
                        break;
                    case 10:
                        break;
                }
            }
            return cellProperties;
        }
    });
}

function crearGrillaExcelCuadro5(tab, container, handson, heightHansonTab) {

    var ColorEmpresa = function (instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.backgroundColor = '#4F81BD';
        td.style.fontSize = '11px';
        td.style.color = 'white'
    };
    var ColorContent = function (instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.backgroundColor = '#D9D9D9';
        td.style.fontSize = '11px';
        td.style.color = 'black'
    };
    var LateralIzq = function (instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.backgroundColor = '#DCE6F1';
        td.style.fontSize = '11px';
        td.style.color = 'black';
    };

    var columns = handson.Columnas;
    var headers = handson.Headers;
    var widths = handson.ListaColWidth;
    var data = handson.ListaExcelData;
    var arrMergeCells = handson.ListaMerge;

    LISTA_HoT[tab] = new Handsontable(container, {
        data: data,
        stretchH: "all",
        observeChanges: true,
        colHeaders: headers,
        colWidths: widths,
        rowHeaders: true,
        columnSorting: false,
        contextMenu: false,
        minSpareRows: 0,
        readOnly: true,
        columns: columns,
        height: heightHansonTab,
        mergeCells: arrMergeCells,
        fixedColumnsLeft: 6,
        contextMenu: {
            items: {
                "Modificar": {
                    name: 'Modificar',
                    callback: function (key, selection, clickEvent) {
                        initPopupModificarCuadro5(obtenerDataFila5(this, selection));
                    }
                },
            }
        },
        cells: function (row, col, prop) {
            var cellProperties = {};

            var cellProperties = {
                type: 'time',
                timeFormat: 'HH:mm',
                correctFormat: true
            };

            if (col == 0 && row >= 0) {
                cellProperties.renderer = ColorEmpresa;
            }
            if (col > 0 && col < 3 && row >= 0) {
                cellProperties.renderer = LateralIzq;
            }

            if (row >= 0) {
                switch (col) {
                    case 0:
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                    case 5:
                        cellProperties.className = "htCenter htMiddle"; cellProperties.readOnly = true; break;
                    default:

                        cellProperties.className = "htCenter htMiddle";
                        cellProperties.type = 'numeric';
                        cellProperties.renderer = render_diferente_disp;
                        break;
                }
            }
            return cellProperties;
        }
    });
}

function crearGrillaExcelCuadro7(tab, evthot, container, handson, heightHansonTab) {
    var ColorEmpresa = function (instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.backgroundColor = '#4F81BD';
        td.style.fontSize = '11px';
        td.style.color = 'white'
    };
    var ColorContent = function (instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.backgroundColor = '#D9D9D9';
        td.style.fontSize = '11px';
        td.style.color = 'black'
    };
    var LateralIzq = function (instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.backgroundColor = '#DCE6F1';
        td.style.fontSize = '11px';
        td.style.color = 'black';
    };

    var columns = handson.Columnas;
    var headers = handson.Headers;
    var widths = handson.ListaColWidth;
    var data = handson.ListaExcelData;
    var dataDescrip = handson.ListaExcelDescripcion;
    var dataFormato = handson.ListaExcelFormatoHtml;
    var arrMergeCells = handson.ListaMerge;
    var listaCambios = handson.ListaCambios;

    evthot = new Handsontable(container, {
        data: data,
        stretchH: "all",
        observeChanges: true,
        colHeaders: headers,
        colWidths: widths,
        rowHeaders: true,
        columnSorting: false,
        contextMenu: false,
        minSpareRows: 0,
        readOnly: true,
        columns: columns,
        height: heightHansonTab,
        mergeCells: arrMergeCells,
        fixedColumnsLeft: 4,
        cells: function (row, col, prop) {
            var cellProperties = {
                type: 'time',
                timeFormat: 'HH:mm',
                correctFormat: true
            };

            if (col == 0 && row >= 0) {
                cellProperties.renderer = ColorEmpresa;
            }
            if (col > 0 && col < 4 && row >= 0) {
                cellProperties.renderer = LateralIzq;
            }

            if (row >= 0) {
                switch (col) {
                    case 0:
                    case 1:
                    case 2:
                    case 3:
                        cellProperties.className = "htCenter htMiddle"; cellProperties.readOnly = true;
                        break;
                    case 4:
                    case 6:
                    case 7:
                        cellProperties.className = "htRight htMiddle";
                        cellProperties.type = 'numeric';
                        cellProperties.format = '0,0.00';
                        break;
                    case 5: //numarra
                        cellProperties.className = "htRight htMiddle";
                        cellProperties.type = 'numeric';
                        cellProperties.format = '0,0';
                        break;
                }
            }
            return cellProperties;
        }
    });
}

/////////////////////////////////////

function render_ColorResult(ht, row, col, tab, dataDescrip, dataFormato) {

    var restoRow = row % 3;

    //fila Minutos
    if (restoRow >= 2 && col >= 6) {

        value = ht.instance.getDataAtCell(row, col);
        td = ht.instance.getCell(row, col);

        //si no existe cambio
        var fondoIndisp = "#D9D9D9";
        var fondoFuenteDato = '';
        var htmlAlertaFuenteDato = '';
        var font_color = "black";
        var font_style = 'normal';

        if (value != null && value != "") {
            switch (tab) {
                case TAB_FORTUITO: //fortuito
                    fondoIndisp = "#F4B00F";
                    break;
                case TAB_PROGRAMADO: //programado
                    fondoIndisp = "#84C641";
                    break;
            };

            var descrip = dataDescrip[row][col];
            var formatoHtml = dataFormato[row][col];
            switch (formatoHtml) {
                case FORMATO_HTML_EVENTO:
                    fondoFuenteDato = "#2ECC71";
                    break;
                case FORMATO_HTML_RESTRIC:
                    fondoFuenteDato = "#FF5050";
                    break;
                case FORMATO_HTML_ALERTA:
                    htmlAlertaFuenteDato = 'S';
                    break;
            }

            $(ht.instance.getCell(row - 2, col)).attr('title', descrip);
            $(ht.instance.getCell(row - 1, col)).attr('title', descrip);
            $(ht.instance.getCell(row, col)).attr('title', descrip);
        }

        var objCssIndisp = {
            "color": font_color,
            "font-size": "12px",
            "background-color": fondoIndisp,
            "vertical-align": "middle",
            "font-style": font_style
        };

        $(ht.instance.getCell(row, col)).css(objCssIndisp);

        if (fondoFuenteDato != '') {
            var objCssFuente = {
                "color": font_color,
                "font-size": "12px",
                "background-color": fondoFuenteDato,
                "vertical-align": "middle"
            };
            $(ht.instance.getCell(row - 2, col)).css(objCssFuente);
            $(ht.instance.getCell(row - 1, col)).css(objCssFuente);
        }

        if (htmlAlertaFuenteDato == 'S') {
            var valueIni = ht.instance.getDataAtCell(row - 2, col);
            var valueFin = ht.instance.getDataAtCell(row - 1, col);
            $(ht.instance.getCell(row - 2, col)).html("<span style='color: red'>!</span> " + valueIni);
            $(ht.instance.getCell(row - 1, col)).html("<span style='color: red'>!</span> " + valueFin);
        }

        var regCambio = getCeldaCambioFilaMultiple(row - 2, col, tab);
        if (regCambio != null) {
            var claseCambio = '';
            if (regCambio.Tipocambio == 'C')
                claseCambio = "ht_celda_nuevo";
            if (regCambio.Tipocambio == 'M')
                claseCambio = "ht_celda_editable";
            if (regCambio.Tipocambio == 'E')
                claseCambio = "ht_celda_eliminado";

            $(ht.instance.getCell(row - 2, col)).attr('class', claseCambio);
            $(ht.instance.getCell(row - 1, col)).attr('class', claseCambio);
        }
    }
}

function render_ColorResult_Cuadro2(ht, row, col, tab, dataDescrip, dataFormato) {

    var restoRow = row % 5;

    if (restoRow >= 4 && col >= 6) {

        value = ht.instance.getDataAtCell(row, col);
        td = ht.instance.getCell(row, col);

        var fondoIndisp = "#D9D9D9";
        var fondoFuenteDato = '';
        var htmlAlertaFuenteDato = '';
        var font_color = "black";
        var font_style = 'normal';

        if (value != null && value != "") {
            switch (tab) {
                case TAB_FORTUITO: //fortuito
                    fondoIndisp = "#F4B00F";
                    break;
                case TAB_PROGRAMADO: //programado
                    fondoIndisp = "#84C641";
                    break;
            };

            var descrip = dataDescrip[row][col];
            var formatoHtml = dataFormato[row][col];
            switch (formatoHtml) {
                case FORMATO_HTML_EVENTO:
                    fondoFuenteDato = "#2ECC71";
                    break;
                case FORMATO_HTML_RESTRIC:
                    fondoFuenteDato = "#FF5050";
                    break;
                case FORMATO_HTML_LIMCOMB:
                    fondoFuenteDato = "#CCD5F0";
                    break;
                case FORMATO_HTML_ALERTA:
                    htmlAlertaFuenteDato = 'S';
                    break;
            }

            $(ht.instance.getCell(row - 2, col)).attr('title', descrip);
            $(ht.instance.getCell(row - 1, col)).attr('title', descrip);
            $(ht.instance.getCell(row, col)).attr('title', descrip);
        }


        var objCssIndisp = {
            "color": font_color,
            "font-size": "12px",
            "background-color": fondoIndisp,
            "vertical-align": "middle",
            "font-style": font_style
        };
        $(ht.instance.getCell(row - 1, col)).css(objCssIndisp);
        $(ht.instance.getCell(row, col)).css(objCssIndisp);

        if (fondoFuenteDato != '') {
            var objCssFuente = {
                "color": font_color,
                "font-size": "12px",
                "background-color": fondoFuenteDato,
                "vertical-align": "middle"
            };
            $(ht.instance.getCell(row - 2, col)).css(objCssFuente);
            $(ht.instance.getCell(row - 1, col)).css(objCssFuente);
        }

        if (htmlAlertaFuenteDato == 'S') {
            var valueIni = ht.instance.getDataAtCell(row - 2, col);
            var valueFin = ht.instance.getDataAtCell(row - 1, col);
            $(ht.instance.getCell(row - 2, col)).html("<span style='color: red'>!</span> " + valueIni);
            $(ht.instance.getCell(row - 1, col)).html("<span style='color: red'>!</span> " + valueFin);
        }

        var regCambio = getCeldaCambioFilaMultiple(row - 4, col, tab);
        if (regCambio != null) {
            var claseCambio = '';
            if (regCambio.Tipocambio == 'C')
                claseCambio = "ht_celda_nuevo";
            if (regCambio.Tipocambio == 'M')
                claseCambio = "ht_celda_editable";
            if (regCambio.Tipocambio == 'E')
                claseCambio = "ht_celda_eliminado";

            $(ht.instance.getCell(row - 4, col)).attr('class', claseCambio);
            $(ht.instance.getCell(row - 3, col)).attr('class', claseCambio);
            $(ht.instance.getCell(row - 2, col)).attr('class', claseCambio);
        }
    }
}

function render_diferente_k_pe(instance, td, row, col, prop, value, cellProperties) {
    cellProperties.className = "htRight htMiddle";
    cellProperties.type = 'numeric';
    cellProperties.format = '0,0.000';

    Handsontable.renderers.TextRenderer.apply(this, arguments);

    if (value != null) {
        var valuePe = instance.getDataAtCell(row, COL_PE_FACTORK);

        //var colorTexto = '#777';
        if (value != valuePe) {
            var colorTexto = 'red';
            var objCss = {
                "color": colorTexto,
            };

            $(td).css(objCss);
        }

        if (value != '-')
            $(td).html(formatFloat(Number(value), 3, '.', ','));
    }

    return td;
}

function render_diferente_factork(instance, td, row, col, prop, value, cellProperties) {
    cellProperties.className = "htRight htMiddle";
    cellProperties.type = 'numeric';
    cellProperties.format = '0,0.0000';

    Handsontable.renderers.TextRenderer.apply(this, arguments);

    if (value != null) {
        //var colorTexto = '#777';
        if (value != 1.0) {
            var colorTexto = 'red';
            var objCss = {
                "color": colorTexto,
            };

            $(td).css(objCss);
        }

        if (value != "-") {
            if (value > 0.99994 && value < 1) {
                $(td).html(formatFloat(0.99988, 4, '.', ','));
            }
            else {
                $(td).html(formatFloat(Number(value), 4, '.', ','));
            }
        } else $(td).html("-");



        /*if (value != '-')
            $(td).html(formatFloat(Number(value), 4, '.', ','));
  */  }
    

    if (col == COL_K_FACTORK) {
        var regCambio = getCeldaCambio(row, col);
        if (regCambio != null) {
            $(td).attr("class", "ht_celda_editable");

            /*if (regCambio.Valor != '-')
                $(td).html(formatFloat(Number(regCambio.Valor), 3, '.', ','));*/

            if (regCambio.Valor != "-")
                if (regCambio.Valor > 0.99994 && regCambio.Valor < 1) {
                    $(td).html(formatFloat(0.99988, 4, '.', ','));
                }
                else $(td).html(formatFloat(Number(regCambio.Valor), 4, '.', ','));


            if (regCambio.Obs != null && regCambio.Obs != "") {
                var tdObs = instance.getCell(row, COL_DESC_FACTORK);
                $(tdObs).attr("class", "ht_celda_editable_txt");
            }
        }
    }

    return td;
}

function render_diferente_factork_obs(instance, td, row, col, prop, value, cellProperties) {
    cellProperties.className = "htLeft htMiddle";
    cellProperties.type = 'text';

    Handsontable.renderers.TextRenderer.apply(this, arguments);

    if (col == COL_DESC_FACTORK) {
        var regCambio = getCeldaCambio(row, COL_K_FACTORK);
        if (regCambio != null) {
            if (regCambio.Obs != null && regCambio.Obs != "") {
                $(td).attr("class", "ht_celda_editable_txt");
            }
        }
    }

    return td;
}

function render_a_incremental(instance, td, row, col, prop, value, cellProperties) {
    cellProperties.className = "htCenter htMiddle";
    cellProperties.readOnly = true;

    Handsontable.renderers.TextRenderer.apply(this, arguments);

    if ((value + "").toUpperCase().includes("INCREMENTAL") || (value + "").toUpperCase().includes("ADICIONAL")) {
        $(td).addClass("celdaEspecial");
    } else {
        $(td).addClass("celdaDefault");
    }

    return td;
}

function render_diferente_a_pe(instance, td, row, col, prop, value, cellProperties) {
    cellProperties.className = "htRight htMiddle";
    cellProperties.type = 'numeric';
    cellProperties.format = '0,0.000';

    Handsontable.renderers.TextRenderer.apply(this, arguments);

    if (value != null) {
        var valuePe = instance.getDataAtCell(row, COL_PE_CUADROASG);

        //var colorTexto = '#777';
        if (value != valuePe) {
            var colorTexto = 'red';
            var objCss = {
                "color": colorTexto,
            };

            $(td).css(objCss);
        }

        if (value != '-')
            $(td).html(formatFloat(Number(value), 3, '.', ','));
    }

    if (col == COL_PA_CUADROASG) {
        var regCambio = getCeldaCambio(row, col);
        if (regCambio != null) {
            $(td).attr("class", "ht_celda_editable");

            if (regCambio.Valor != '-')
                $(td).html(formatFloat(Number(regCambio.Valor), 3, '.', ','));
        }
    }

    return td;
}

function render_diferente_disp(instance, td, row, col, prop, value, cellProperties) {
    cellProperties.className = "htCenter htMiddle";
    cellProperties.type = 'numeric';

    Handsontable.renderers.TextRenderer.apply(this, arguments);

    if (value != null && value != "") {

        //var colorTexto = '#777';
        if (value != 1) {
            var colorFondo = '#C6EFCE';
            var objCss = {
                "background-color": colorFondo,
            };

            $(td).css(objCss);
        }
    } else {
        var colorFondo = '#BFBFBF';
        var objCss = {
            "background-color": colorFondo,
        };

        $(td).css(objCss);
    }

    var regCambio = getCeldaCambioFilaMultiple(row, col, 0);
    if (regCambio != null) {
        var claseCambio = "ht_celda_editable";

        $(td).attr('class', claseCambio);
    }

    return td;
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

/////////////////////////////////////


function getCeldaCambio(row, col) {
    if (LISTA_CELDA_CAMBIOS != null && LISTA_CELDA_CAMBIOS.length > 0) {
        for (var i = 0; i < LISTA_CELDA_CAMBIOS.length; i++) {
            var reg = LISTA_CELDA_CAMBIOS[i];
            if (reg.Row == row)
                return reg;
        }
    }

    return null;
}

function getCeldaCambioFilaMultiple(row, col, tab) {
    if (LISTA_CELDA_CAMBIOS != null && LISTA_CELDA_CAMBIOS.length > 0) {
        for (var i = 0; i < LISTA_CELDA_CAMBIOS.length; i++) {
            var reg = LISTA_CELDA_CAMBIOS[i];
            if (reg.Hoja == tab && reg.Row == row && reg.Col == col)
                return reg;
        }
    }

    return null;
}

/////////////////////////////////////
function formatFloat(num, casasDec, sepDecimal, sepMilhar) {
    if (num == 0) {
        var cerosDer = '';
        while (cerosDer.length < casasDec)
            cerosDer = '0' + cerosDer;

        return "0" + sepDecimal + cerosDer;
    }

    if (num < 0) {
        num = -num;
        sinal = -1;
    } else
        sinal = 1;
    var resposta = "";
    var part = "";
    if (num != Math.floor(num)) // decimal values present
    {
        part = Math.round((num - Math.floor(num)) * Math.pow(10, casasDec)).toString(); // transforms decimal part into integer (rounded)
        while (part.length < casasDec)
            part = '0' + part;
        if (casasDec > 0) {
            resposta = sepDecimal + part;
            num = Math.floor(num);
        } else
            num = Math.round(num);
    } // end of decimal part
    else {
        while (part.length < casasDec)
            part = '0' + part;
        if (casasDec > 0) {
            resposta = sepDecimal + part;
        }
    }

    var numBefWhile = num; //A veces no aparece el número cero (0) antes del punto decimal
    while (num > 0) // integer part
    {
        part = (num - Math.floor(num / 1000) * 1000).toString(); // part = three less significant digits
        num = Math.floor(num / 1000);
        if (num > 0)
            while (part.length < 3) // 123.023.123  if sepMilhar = '.'
                part = '0' + part; // 023
        resposta = part + resposta;
        if (num > 0)
            resposta = sepMilhar + resposta;
    }
    if (sinal < 0)
        resposta = '-' + ((numBefWhile == 0) ? '0' : '') + resposta;
    else {
        if (numBefWhile == 0)
            resposta = '0' + resposta;
    }
    return resposta;
}

//#region Edicion de Celda

function inicializarFormulariosCambiosCuadro1() {
    $("#frmModificacion").submit(function (event) {
        event.preventDefault();
        var data = getFormData($(this));
        data.Tipocambio = "Modificacion";
        //Se agrega nuevo campo -Assetec (RAC)
        if (data.Idetconsval == "on") {
            data.Idetconsval = 1;
        }
        else if (data.Idetconsval ==undefined) {
            data.Idetconsval = 0;
        }

        agregarDataCambio(data)

        agregarCeldaCambioMultipleGlobal(CUADRO_1, data);
        $(this).trigger("reset");
        $(`#popupEditar`).bPopup().close()
    });

    $("#frmNuevo").submit(function (event) {
        event.preventDefault();
        var data = getFormData($(this));
        data.Tipocambio = "Creacion";
        //Se agrega nuevo campo -Assetec (RAC)
        if (data.Idetconsval == "on") {
            data.Idetconsval = 1;
        }
        else if (data.Idetconsval == undefined) {
            data.Idetconsval = 0;
        }
        agregarDataCambio(data)

        agregarCeldaCambioMultipleGlobal(CUADRO_1, data);
        $(this).trigger("reset");
        $(`#popupNuevo`).bPopup().close()
    });

    $("#frmEliminar").submit(function (event) {
        event.preventDefault();

        if (confirm("¿Desea eliminar este registro?")) {
            var data = getFormData($(this));
            data.Tipocambio = "Eliminacion";

            agregarDataCambio(data)

            agregarCeldaCambioMultipleGlobal(CUADRO_1, data);
            $(this).trigger("reset");
            $(`#popupEliminar`).bPopup().close()
        }

    });

    tablaCambios = $('#tblCambioFor').DataTable({
        data: [],
        columns: [
            { data: "Indisponibilidad", width: "10%" },
            { data: "Idettipoindisp", visible: false },
            { data: "Equicodi", visible: false },
            { data: "Grupocodi", visible: false },
            { data: "Tipocambio", width: "5%" },
            { data: "Emprnomb" },
            { data: "Central" },
            { data: "Unidadnomb", width: "10%" },
            { data: "Fecha", width: "10%" },
            { data: "Rangohoraold", width: "15%" },
            { data: "Rangohora", width: "15%" },
            { data: "Idetjustf", visible: true },
            //Se agrega nuevo campo -Assetec (RAC)
            { data: "Idetconsval", visible: true },
            {
                mRender: function (data, type, row) {
                    return " <a id='btnDelete'><img src='" + siteRoot + "Content/Images/btn-cancel.png' alt='Eliminar Fila'></a>";
                },
                searchable: false,
                orderable: false,
                width: "5%"
            }
        ],
        scrollY: "200px",
        scrollCollapse: true,
        paging: false,
        rowCallback: function (row, data) { },
        filter: false,
        info: true,
        ordering: false,
        processing: true,
        retrieve: true
    });


    $('#tblCambioFor tbody').on('click', '#btnDelete', function () {
        tablaCambios.row($(this).parents('tr')).remove().draw();
    });

    $(".readonly").on('keydown paste', function (e) {
        e.preventDefault();
    });


    //Editar
    document.getElementById("txtHInicioMod").addEventListener("input", function () {

        var hFin = document.getElementById("txtHFinMod");
        setMinTime(this, hFin);
        var min = calcularDifMinutos(this, hFin);
        $("#txtValorMod").val(min);

    }, false);

    document.getElementById("txtHFinMod").addEventListener("input", function () {

        var hIni = document.getElementById("txtHInicioMod");
        var min = calcularDifMinutos(hIni, this);
        $("#txtValorMod").val(min);

    }, false);


    //Nuevo
    document.getElementById("txtHInicioN").addEventListener("input", function () {

        var hFin = document.getElementById("txtHFinN");
        setMinTime(this, hFin);
        var min = calcularDifMinutos(this, hFin);
        $("#txtValor").val(min);

    }, false);

    document.getElementById("txtHFinN").addEventListener("input", function () {

        var hIni = document.getElementById("txtHInicioN");
        var min = calcularDifMinutos(hIni, this);
        $("#txtValor").val(min);

    }, false);
}

function inicializarFormulariosCambiosCuadro2() {
    $("#frmModificacion").submit(function (event) {
        event.preventDefault();
        var data = getFormData($(this));
        data.Tipocambio = "Modificacion";
        //Se agrega nuevo campo -Assetec (RAC)
        if (data.Idetconsval == "on") {//Conservarvalor
            data.Idetconsval = 1;
        }
        else if (data.Idetconsval == undefined) {
            data.Idetconsval = 0;
        }
        agregarDataCambio(data, CUADRO_2)

        agregarCeldaCambioMultipleGlobal(CUADRO_2, data);
        $(this).trigger("reset");
        $(`#popupEditar`).bPopup().close()
    });

    $("#frmNuevo").submit(function (event) {
        event.preventDefault();
        var data = getFormData($(this));
        data.Tipocambio = "Creacion";
        //Se agrega nuevo campo -Assetec (RAC)
        if (data.Idetconsval == "on") {
            data.Idetconsval = 1;
        }
        else if (data.Idetconsval == undefined) {
            data.Idetconsval = 0;
        }
        agregarDataCambio(data, CUADRO_2)

        agregarCeldaCambioMultipleGlobal(CUADRO_2, data);
        $(this).trigger("reset");
        $(`#popupNuevo`).bPopup().close()
    });

    $("#frmEliminar").submit(function (event) {
        event.preventDefault();

        if (confirm("¿Desea eliminar este registro?")) {
            var data = getFormData($(this));
            data.Tipocambio = "Eliminacion";

            agregarDataCambio(data, CUADRO_2)

            agregarCeldaCambioMultipleGlobal(CUADRO_2, data);
            $(this).trigger("reset");
            $(`#popupEliminar`).bPopup().close()
        }

    });


    tablaCambios = $('#tblCambioFor').DataTable({
        data: [],
        columns: [
            { data: "Indisponibilidad", width: "10%" },
            { data: "Idettipoindisp", visible: false },
            { data: "Equicodi", visible: false },
            { data: "Grupocodi", visible: false },
            { data: "Tipocambio", width: "5%" },
            { data: "Emprnomb" },
            { data: "Central" },
            { data: "Unidadnomb", width: "10%" },
            { data: "Fecha", width: "10%" },
            { data: "Rangohoraold", width: "15%" },
            { data: "Rangohora", width: "15%" },
            { data: "Idetjustf", visible: true },
            //Se agrega nuevo campo -Assetec (RAC)
            { data: "Idetconsval", visible: true },//Conservarvalor
            {
                mRender: function (data, type, row) {
                    return " <a id='btnDelete'><img src='" + siteRoot + "Content/Images/btn-cancel.png' alt='Eliminar Fila'></a>";
                },
                searchable: false,
                orderable: false,
                width: "5%"
            }
        ],
        scrollY: "200px",
        scrollCollapse: true,
        paging: false,
        rowCallback: function (row, data) { },
        filter: false,
        info: true,
        ordering: false,
        processing: true,
        retrieve: true
    });


    $('#tblCambioFor tbody').on('click', '#btnDelete', function () {
        tablaCambios.row($(this).parents('tr')).remove().draw();
    });

    $(".readonly").on('keydown paste', function (e) {
        e.preventDefault();
    });


    //Editar
    document.getElementById("txtHInicioMod").addEventListener("input", function () {

        var hFin = document.getElementById("txtHFinMod");
        setMinTime(this, hFin);
        var min = calcularDifMinutos(this, hFin);
        $("#txtValorMod").val(min);

    }, false);

    document.getElementById("txtHFinMod").addEventListener("input", function () {

        var hIni = document.getElementById("txtHInicioMod");
        var min = calcularDifMinutos(hIni, this);
        $("#txtValorMod").val(min);

    }, false);


    //Nuevo
    document.getElementById("txtHInicioN").addEventListener("input", function () {

        var hFin = document.getElementById("txtHFinN");
        setMinTime(this, hFin);
        var min = calcularDifMinutos(this, hFin);
        $("#txtValor").val(min);

    }, false);

    document.getElementById("txtHFinN").addEventListener("input", function () {

        var hIni = document.getElementById("txtHInicioN");
        var min = calcularDifMinutos(hIni, this);
        $("#txtValor").val(min);

    }, false);
}

function inicializarFormulariosCambiosCuadroFactorK() {
    $("#frmModificacion").submit(function (event) {
        event.preventDefault();
        var data = getFormData($(this));
        data.Tipocambio = "Modificacion";
        //Se agrega nuevo campo -Assetec (RAC)
        if (data.Itotconsval == "on") {
            data.Itotconsval = 1;
        }
        else if (data.Itotconsval == undefined) {
            data.Itotconsval = 0;
        }
        var factorK = +data.Itotfactork;
        if (factorK > 1 || factorK < 0) {
            alert('El "Factor K Modificado" no esta en el rango correcto');
            return;
        }

        agregarDataCambio(data)

        agregarCeldaCambioGlobal(CUADRO_FACTOR_K, data);
        $(this).trigger("reset");
        $(`#popupEditar`).bPopup().close()
    });

    tablaCambios = $('#tblCambioK').DataTable({
        data: [],
        columns: [
            { data: "Equicodi", visible: false },
            { data: "Grupocodi", visible: false },
            { data: "Tipocambio", width: "5%" },
            { data: "Emprnomb" },
            { data: "Central" },
            { data: "Unidadnomb", width: "10%" },
            { data: "Factork", width: "10%" },
            { data: "Itotfactork", width: "15%" },
            { data: "Descadic", width: "15%" },
            { data: "Itotdescadic", width: "15%" },
            { data: "Itotjustf", },
            //Se agrega nuevo campo -Assetec (RAC)
            { data: "Itotconsval", visible: true },
            {
                mRender: function (data, type, row) {
                    return " <a id='btnDelete'><img src='" + siteRoot + "Content/Images/btn-cancel.png' alt='Eliminar Fila'></a>";
                },
                searchable: false,
                orderable: false,
                width: "5%"
            }
        ],
        scrollY: "200px",
        scrollCollapse: true,
        paging: false,
        rowCallback: function (row, data) { },
        filter: false,
        info: true,
        ordering: false,
        processing: true,
        retrieve: true
    });


    $('#tblCambioK tbody').on('click', '#btnDelete', function () {
        tablaCambios.row($(this).parents('tr')).remove().draw();
    });

    $(".readonly").on('keydown paste', function (e) {
        e.preventDefault();
    });

}

function inicializarFormulariosCambiosCuadro5() {
    $("#frmModificacion").submit(function (event) {
        event.preventDefault();
        var data = getFormData($(this));
        data.Tipocambio = "Modificacion";
        //Se agrega nuevo campo -Assetec (RAC)
        if (data.Idetconsval == "on") {
            data.Idetconsval = 1;
        }
        else if (data.Idetconsval == undefined) {
            data.Idetconsval = 0;
        }
        if (validarDataPopup(data, CUADRO_CUADRO5)) {

            agregarDataCambio(data, CUADRO_CUADRO5)

            agregarCeldaCambioMultipleGlobal(CUADRO_CUADRO5, data);
            $(this).trigger("reset");
            $(`#popupEditar`).bPopup().close();
        }
    });

    tablaCambios = $('#tblCambioC5').DataTable({
        data: [],
        columns: [
            { data: "Equicodi", visible: false },
            { data: "Grupocodi", visible: false },
            { data: "Tipocambio", width: "5%" },
            { data: "Emprnomb" },
            { data: "Central" },
            { data: "Fecha", width: "10%" },
            { data: "IdethorainiDesc", visible: false },
            { data: "Idettienedispold", width: "15%" },
            { data: "Idettienedisp", width: "15%" },
            { data: "Idetjustf", visible: true },
            //Se agrega nuevo campo -Assetec (RAC)
            { data: "Idetconsval", visible: true },
            {
                mRender: function (data, type, row) {
                    return " <a id='btnDelete'><img src='" + siteRoot + "Content/Images/btn-cancel.png' alt='Eliminar Fila'></a>";
                },
                searchable: false,
                orderable: false,
                width: "5%"
            }
        ],
        scrollY: "200px",
        scrollCollapse: true,
        paging: false,
        rowCallback: function (row, data) { },
        filter: false,
        info: true,
        ordering: false,
        processing: true,
        retrieve: true
    });


    $('#tblCambioC5 tbody').on('click', '#btnDelete', function () {
        tablaCambios.row($(this).parents('tr')).remove().draw();
    });

    $(".readonly").on('keydown paste', function (e) {
        e.preventDefault();
    });
}

Number.isFloat = Number.isFloat || function (value) {
    return typeof value === 'number' &&
        isFinite(value) &&
        Math.floor(value) !== value;
};

function validarDataPopup(data, cuadro) {
    if (cuadro == CUADRO_CUADRO5) {
        if (IsNumeric(data.Idettienedisp)) {
            if (data.Idettienedisp == 0 || data.Idettienedisp == 1) { }
            else {
                alert("Debe ingresar un valor de 0 o 1.");
                return false;
            }
        } else {
            alert("El valor de 'Disponibilidad Modificado' no es numérico, vuelva a ingresar un valor de 0 o 1.");
            return false;
        }
    }

    return true;
}

function getFormData($form) {

    var disabled = $form.find(':input:disabled').removeAttr('disabled');
    var unindexed_array = $form.serializeArray();
    disabled.attr('disabled', 'disabled');

    var indexed_array = {};

    $.map(unindexed_array, function (n, i) {
        indexed_array[n['name']] = n['value'];
    });
    return indexed_array;
}

function initPopupNuevo(rowData) {
    $("#txtEmpresaN").val(rowData.Empresa);
    $("#txtCentralN").val(rowData.Central);
    $("#txtUnidadN").val(rowData.Unidad);
    $("#txtFechaN").val(rowData.Fecha);
    $("#txtEquicodiN").val(rowData.Equicodi);
    $("#txtGrupocodiN").val(rowData.Grupocodi);
    $("#txtRowN").val(rowData.Row);
    $("#txtColN").val(rowData.Col);

    $("#txtPrN").val(rowData.Pr);
    $("#txtValorParcialN").val(rowData.MinutosParcial);

    $('#popupNuevo').bPopup({
        easing: 'easeOutBack',
        speed: 450,
        transition: 'slideDown'
    });
}

function initPopupEliminar(rowData) {

    $("#txtEmpresaD").val(rowData.Empresa);
    $("#txtCentralD").val(rowData.Central);
    $("#txtUnidadD").val(rowData.Unidad);
    $("#txtFechaD").val(rowData.Fecha);
    $("#txtHInicioD").val(rowData.Horainicio);
    $("#txtHFinD").val(rowData.Horafin);
    $("#txtValorD").val(rowData.Minutos);
    $("#txtEquicodiD").val(rowData.Equicodi);
    $("#txtGrupocodiD").val(rowData.Grupocodi);
    $("#txtRowD").val(rowData.Row);
    $("#txtColD").val(rowData.Col);

    $('#popupEliminar').bPopup({
        easing: 'easeOutBack',
        speed: 450,
        transition: 'slideDown'
    });
}

function initPopupModificar(rowData) {

    $("#txtEmpresa").val(rowData.Empresa);
    $("#txtCentral").val(rowData.Central);
    $("#txtUnidad").val(rowData.Unidad);
    $("#txtFecha").val(rowData.Fecha);
    $("#txtHInicio").val(rowData.Horainicio);
    $("#txtHFin").val(rowData.Horafin);
    $("#txtValorAnt").val(rowData.Minutos);
    $("#txtEquicodi").val(rowData.Equicodi);
    $("#txtGrupocodi").val(rowData.Grupocodi);
    $("#txtRow").val(rowData.Row);
    $("#txtCol").val(rowData.Col);

    $("#txtPrAnt").val(rowData.Pr);
    $("#txtValorParcialAnt").val(rowData.MinutosParcial);

    $('#popupEditar').bPopup({
        easing: 'easeOutBack',
        speed: 450,
        transition: 'slideDown'
    });
}

function initPopupModificarFactorK(rowData) {
    $("#txtEmpresa").val(rowData.Empresa);
    $("#txtCentral").val(rowData.Central);
    $("#txtUnidad").val(rowData.Unidad);

    $("#txtPe").val(rowData.Pe);
    $("#txtK").val(rowData.K);
    $("#txtDesc").val(rowData.Obs);

    $("#txtEquicodi").val(rowData.Equicodi);
    $("#txtGrupocodi").val(rowData.Grupocodi);
    $("#txtRow").val(rowData.Row);

    //Inicio: IND.PR25.2022
    $("#txtFamcodi").val(rowData.Famcodi);
    $("#txtEmprcodi").val(rowData.Emprcodi);
    $("#txtEquipadre").val(rowData.Equipadre);
    //Fin: IND.PR25.2022

    $('#popupEditar').bPopup({
        easing: 'easeOutBack',
        speed: 450,
        transition: 'slideDown'
    });
}

function initPopupModificarCuadro5(rowData) {

    $("#txtEmpresa").val(rowData.Empresa);
    $("#txtCentral").val(rowData.Central);
    $("#txtUnidad").val(rowData.Unidad);
    $("#txtFecha").val(rowData.Fecha);
    $("#txtDisp").val(rowData.Disponibilidad);
    $("#txtEquicodi").val(rowData.Equicodi);
    $("#txtGrupocodi").val(rowData.Grupocodi);
    $("#txtRow").val(rowData.Row);
    $("#txtCol").val(rowData.Col);
    $("#txtKMod").val('');
    $("#txtDescadic").val('');

    $('#popupEditar').bPopup({
        easing: 'easeOutBack',
        speed: 450,
        transition: 'slideDown'
    });
}

function initPopupModificarFactorPa(rowData) {
    $("#txtEmpresa").val(rowData.Empresa);
    $("#txtCentral").val(rowData.Central);
    $("#txtUnidad").val(rowData.Unidad);

    $("#txtPe").val(rowData.Pe);
    $("#txtPa").val(rowData.Pa);

    $("#txtEquicodi").val(rowData.Equicodi);
    $("#txtGrupocodi").val(rowData.Grupocodi);
    $("#txtRow").val(rowData.Row);

    $('#popupEditar').bPopup({
        easing: 'easeOutBack',
        speed: 450,
        transition: 'slideDown'
    });
}

function obtenerDataFila(handson, selection) {
    var filaIni = handson.mergeCells.mergedCellInfoCollection.getInfo(selection.start.row, COL_CENTRAL).row;
    var columna = selection.start.col;

    var filaData = handson.getDataAtRow(selection.start.row);
    var editData = handson.getData(filaIni, columna, filaIni + 2, columna);

    var cabDia = handson.getColHeader(columna);

    var fecha = obtenerFechaCelda(cabDia);


    var data =
    {
        Empresa: filaData[0],
        Central: filaData[2],
        Equicodi: filaData[3],
        Grupocodi: filaData[4],
        Unidad: filaData[5],
        Fecha: fecha,
        Horainicio: editData[0],
        Horafin: editData[1],
        Minutos: editData[2],
        Row: filaIni,
        Col: columna
    }

    return data;
}

function obtenerDataFila2(handson, selection) {
    var filaIni = handson.mergeCells.mergedCellInfoCollection.getInfo(selection.start.row, COL_CENTRAL).row;
    var columna = selection.start.col;

    var filaData = handson.getDataAtRow(selection.start.row);
    var editData = handson.getData(filaIni, columna, filaIni + 4, columna);

    var cabDia = handson.getColHeader(columna);

    var fecha = obtenerFechaCelda(cabDia);


    var data =
    {
        Empresa: filaData[0],
        Central: filaData[2],
        Equicodi: filaData[3],
        Grupocodi: filaData[4],
        Unidad: filaData[5],
        Fecha: fecha,

        Horainicio: editData[0],
        Horafin: editData[1],
        Pr: editData[2],
        Minutos: editData[3],
        MinutosParcial: editData[4],
        Row: filaIni,
        Col: columna
    }

    return data;
}

function obtenerDataFilaFactorK(handson, selection) {

    var row = selection.start.row;
    var filaData = handson.getDataAtRow(row);

    var data =
    {
        Empresa: filaData[0],
        Central: filaData[1],
        Equicodi: filaData[2],
        Grupocodi: filaData[3],
        Unidad: filaData[4],
        Pe: filaData[5],
        K: filaData[6],
        Obs: filaData[7],
        //Inicio: IND.PR25.2022
        Row: row,
        Famcodi: filaData[8],
        Emprcodi: filaData[9],
        Equipadre: filaData[10]
        //Fin: IND.PR25.2022
    }

    return data;
}

function obtenerDataFila5(handson, selection) {
    var filaIni = selection.start.row;
    var columna = selection.start.col;

    var filaData = handson.getDataAtRow(selection.start.row);
    var editData = handson.getData(filaIni, columna, filaIni, columna);

    var cabDia = handson.getColHeader(columna);

    var fecha = obtenerFechaCelda(cabDia);


    var data =
    {
        Empresa: filaData[0],
        Central: filaData[2],
        Equicodi: filaData[3],
        Grupocodi: filaData[4],
        Unidad: filaData[5],
        Fecha: fecha,
        Disponibilidad: editData[0],
        Row: filaIni,
        Col: columna
    }

    return data;
}

function obtenerDataFilaFactorPa(handson, selection) {

    var row = selection.start.row;
    var filaData = handson.getDataAtRow(row);

    var data =
    {
        Empresa: filaData[0],
        Central: filaData[1],
        Equicodi: filaData[2],
        Grupocodi: filaData[3],
        Unidad: filaData[4],
        Pe: filaData[5],
        Pa: filaData[6],
        Row: row
    }

    return data;
}

function obtenerFechaCelda(headDay) {
    var dia = headDay.substr(0, 2);
    var fechaInicio = moment($("#desc_fecha_ini").val(), 'DD/MM/YYYY');
    var fecha = fechaInicio.add(dia - 1, 'd').format("YYYY-MM-DD");
    return fecha;
}

function popupClose(id) {
    $(`#${id}`).bPopup().close();
}

//#endregion


//#region Tabla Cambios

//Agrega los cambios realizados a la tabla temporal
function agregarDataCambio(data, cuadro) {
    data.Rangohoraold = null;
    if (data.Idethorainiold) {
        data.Rangohoraold = `${data.Idethorainiold} - ${data.Idethorafinold} (${data.Idetminold} min)`;
        data.Idethorainiold = `${data.Fecha} ${data.Idethorainiold}`;
        data.Idethorafinold = `${data.Fecha} ${data.Idethorafinold}`;
    } else {
        data.Idethorainiold = null;
        data.Idethorafinold = null;
    }

    data.Rangohora = null;
    data.HoraIniMinCambio = null;
    data.HoraFinMinCambio = null;

    if (data.Idethoraini) {
        data.HoraIniMinCambio = data.Idethoraini;
        data.HoraFinMinCambio = data.Idethorafin;

        data.Rangohora = `${data.Idethoraini} - ${data.Idethorafin} (${data.Idetmin} min)`;
        data.Idethoraini = `${data.Fecha} ${data.Idethoraini}`;
        data.Idethorafin = `${data.Fecha} ${data.Idethorafin}`;
    } else {
        data.Idethoraini = null;
        data.Idethorafin = null;
    }

    if (cuadro == CUADRO_CUADRO5) {
        data.IdethorainiDesc = `${data.Fecha}`;
        data.IdethorafinDesc = `${data.Fecha}`;
    } else {
        data.IdethorainiDesc = data.Idethoraini;
        data.IdethorafinDesc = data.Idethorafin;
    }

    data.Indisponibilidad = $("#tab-container .tab.active").text();

    if (cuadro != CUADRO_2) {
        if (data.Indisponibilidad == "Fortuito") data.Idettipoindisp = "FT";
        if (data.Indisponibilidad == "Programado") data.Idettipoindisp = "PT";
    } else {
        if (data.Indisponibilidad == "Fortuito") data.Idettipoindisp = "FP";
        if (data.Indisponibilidad == "Programado") data.Idettipoindisp = "PP";
    }
    //Condicion para cambiar el valor del campo y poder mostrarla en la vista -- Assetec
    data.Idetconsval = (data.Idetconsval == 1) ? "Si" : "NO";
    data.Itotconsval = (data.Itotconsval == 1) ? "Si" : "NO";
    //agregar datos al datatable
    tablaCambios.rows.add([data]).draw();
}

function agregarCeldaCambioGlobal(cuadro, data) {

    if (cuadro == CUADRO_FACTOR_K) {
        //lista global
        var regCambio = {
            Row: data.Row,
            Valor: data.Itotfactork,
            Obs: data.Itotdescadic
        };
        LISTA_CELDA_CAMBIOS.push(regCambio);

        //update cell
        LISTA_HoT[0].setDataAtCell(regCambio.Row, COL_K_FACTORK, regCambio.Valor);

        if (regCambio.Obs != null && regCambio.Obs != "")
            LISTA_HoT[0].setDataAtCell(regCambio.Row, COL_DESC_FACTORK, regCambio.Obs);
    }
}

function agregarCeldaCambioMultipleGlobal(cuadro, data) {
    if (cuadro == CUADRO_1) {
        //lista global
        var regCambio = {
            Col: parseInt(data.Col) || 0,
            Row: parseInt(data.Row) || 0,
            HoraIni: data.HoraIniMinCambio,
            HoraFin: data.HoraFinMinCambio,
            Min: data.Idetmin,
            Obs: data.Itotdescadic,
            Tipocambio: data.Tipocambio.substring(0, 1),
            Hoja: data.Idettipoindisp == "FT" ? 0 : 1,
        };
        LISTA_CELDA_CAMBIOS.push(regCambio);

        //update cell
        LISTA_HoT[regCambio.Hoja].setDataAtCell(regCambio.Row, regCambio.Col, regCambio.HoraIni);
        LISTA_HoT[regCambio.Hoja].setDataAtCell(regCambio.Row + 1, regCambio.Col, regCambio.HoraFin);
        LISTA_HoT[regCambio.Hoja].setDataAtCell(regCambio.Row + 2, regCambio.Col, regCambio.Min);
    }

    if (cuadro == CUADRO_2) {
        //lista global
        var regCambio = {
            Col: parseInt(data.Col) || 0,
            Row: parseInt(data.Row) || 0,
            HoraIni: data.HoraIniMinCambio,
            HoraFin: data.HoraFinMinCambio,
            Pr: data.Idetpr,
            Min: data.Idetmin,
            MinParcial: data.Idetminparcial,
            Obs: data.Itotdescadic,
            Tipocambio: data.Tipocambio.substring(0, 1),
            Hoja: data.Idettipoindisp == "FP" ? 0 : 1,
        };
        LISTA_CELDA_CAMBIOS.push(regCambio);

        //update cell
        LISTA_HoT[regCambio.Hoja].setDataAtCell(regCambio.Row, regCambio.Col, regCambio.HoraIni);
        LISTA_HoT[regCambio.Hoja].setDataAtCell(regCambio.Row + 1, regCambio.Col, regCambio.HoraFin);
        LISTA_HoT[regCambio.Hoja].setDataAtCell(regCambio.Row + 2, regCambio.Col, regCambio.Pr);
        LISTA_HoT[regCambio.Hoja].setDataAtCell(regCambio.Row + 3, regCambio.Col, regCambio.Min);
        LISTA_HoT[regCambio.Hoja].setDataAtCell(regCambio.Row + 4, regCambio.Col, regCambio.MinParcial);
    }

    if (cuadro == CUADRO_CUADRO5) {
        var regCambio = {
            Col: parseInt(data.Col) || 0,
            Row: parseInt(data.Row) || 0,
            Disp: data.Idettienedisp,
            Hoja: 0
        };
        LISTA_CELDA_CAMBIOS.push(regCambio);

        LISTA_HoT[0].setDataAtCell(regCambio.Row, regCambio.Col, regCambio.Disp);
    }
}

//Calcula la diferencia en minutos entre la hora inicio y fin
function calcularDifMinutos(hIni, hfin) {
    if (hfin.validity.valid && hIni.validity.valid) {
        var min = moment.duration(moment(hfin.value, "HH:mm").diff(moment(hIni.value, "HH:mm"))).asMinutes();
        return min;
    }
    return "";
}

//Asignar tiempo minimo
function setMinTime(hIni, hFin) {

    if (hIni.validity.valid) {
        var min = moment(hIni.value, "HH:mm").add(1, 'minutes').format("HH:mm");
        hFin.setAttribute("min", min);

    }
}

//Permite obtener Object Array de un DataTable
function GetDataDataTable(data) {
    var dataList = [];
    //Condicion para cambiar el valor del campo y enviarla al controlador -- Assetec (RAC)
    $.each(data, function (index, value) {
        if (value.Idetconsval == 1 || value.Idetconsval == 0) {
            return;
        } else {
            value.Idetconsval = (value.Idetconsval == "Si") ? 1 : 0;
        }
    });
    /********************************/
    $.each(data, function (index, value) {
        dataList.push(value);
    });
    return dataList;
}

function guardarCambios() {
    var cuadro = parseInt($('#hfCuadro').val()) || 0;
    var famcodi = parseInt($('#hfFamilia').val()) || 0;

    var data1 = GetDataDataTable(tablaCambios.rows().data());
    var data2 = GetDataDataTable(tablaCambios.rows().data());
    if (data1.length > 0) {
        if (cuadro == 1) {
            data1 = [];
        }
        if (cuadro == CUADRO_FACTOR_K) {
            data2 = [];
        }

        var dataJson = {
            icuacodi: cuadro,
            famcodi: famcodi,
            irecacodi: $('#hfRecalculo').val(),
            irptcodi: $('#hfReporteVersion').val(),
            listaTotData: data1,
            listaData: data2
        };

        $.ajax({
            type: 'POST',
            url: controlador + 'GuardarCambiosHandson',
            data: JSON.stringify(dataJson),
            contentType: 'application/json; charset=UTF-8',
            dataType: 'json',
            success: function (evt) {
                if (evt.Resultado != "-1") {
                    if (evt.IdReporte > 0) {
                        alert("Se guardaron exitosamente los cambios");
                        editarReporte(evt.IdReporte, cuadro);
                    } else {
                        alert('No se generó nueva versión. No existen cambios.');
                    }
                } else {
                    alert("Ha ocurrido un error: " + evt.Mensaje);
                }
            },
            error: function () {
                alert("Ha ocurrido un error");
            }
        });

    } else {
        alert("No existen cambios.");
    }

}

function editarReporte(id, cuadro) {
    switch (cuadro) {
        case CUADRO_FACTOR_K:
            window.location.href = controlador + "ViewVersionCuadro3?irptcodi=" + id;
            break;
        case CUADRO_CUADRO5:
            window.location.href = controlador + "ViewVersionCuadro5?irptcodi=" + id;
            break;
        case CUADRO_CUADRO7:
            window.location.href = controlador + "ViewVersionCuadro7?irptcodi=" + id;
            break;
        default:
            window.location.href = controlador + "ViewVersionCuadro?irptcodi=" + id;
            break;
    }
}

function aprobarVersion(id) {
    $.ajax({
        type: 'POST',
        url: controlador + 'AprobarVersion',
        data: {
            irptcodi: id
        },
        cache: false,
        success: function (result) {
            if (result.Resultado == '-1') {
                alert('Ha ocurrido un error:' + result.Mensaje);
            } else {
                alert("Se actualizó correctamente el registro.");
                $("#btnRegresar").click();
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

function IsNumeric(input) {
    var RE = /^-{0,1}\d*\.{0,1}\d+$/;
    return (RE.test(input));
}

function validarNumero(item, evt) {
    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;

    if (charCode == 46) {
        var regex = new RegExp(/\./g)
        var count = $(item).val().match(regex).length;
        if (count > 1) {
            return false;
        }
    }

    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }

    return true;
}

//#endregion

