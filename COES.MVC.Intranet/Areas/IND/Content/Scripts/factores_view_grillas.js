var container;
var previoChange = 0;

var FACTOR_FORT_TERMICO = 8;
var FACTOR_PROG_TERMICO = 9;
var FACTOR_PROG_HIDRO = 10;
var FACTOR_PRESENCIA = 11;

const COL_CENTRAL = 2;
var COL_PE_CUADROASG = 4;

function crearGrillaExcelFactorPresencia(tab, container, handson, heightHansonTab) {
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

    LISTA_HoT[tab]  = new Handsontable(container, {
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
        fixedColumnsLeft: 5,
        cells: function (row, col, prop) {
            var cellProperties = {
                type: 'time',
                timeFormat: 'HH:mm',
                correctFormat: true
            };

            if (col == 0 && row >= 0) {
                cellProperties.renderer = ColorEmpresa;
            }
            if (col > 0 && col < 2 && row >= 0) {
                cellProperties.renderer = LateralIzq;
            }

            if (row >= 0) {
                switch (col) {
                    case 0:
                    case 1:
                    case 3:
                        cellProperties.className = "htCenter htMiddle"; cellProperties.readOnly = true;
                        break;
                    case 2:
                        cellProperties.className = "htRight htMiddle";
                        cellProperties.renderer = render_fp_flag;
                        break;
                    case 4:
                        cellProperties.className = "htRight htMiddle";
                        cellProperties.type = 'numeric';
                        cellProperties.renderer = render_fp;
                        break;
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

function crearGrillaExcelFactorProgHidro(tab, container, handson, heightHansonTab) {
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
    var posAntepenultimaColumna = columns.length - 3;
    var posPenultimaColumna = columns.length - 2;
    var posUltimaColumna = columns.length - 1;

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
        cells: function (row, col, prop) {
            var cellProperties = {
                type: 'time',
                timeFormat: 'HH:mm',
                correctFormat: true
            };

            if (col == 0 && row >= 0) {
                cellProperties.renderer = ColorEmpresa;
            }
            if (col > 0 && col < 2 && row >= 0) {
                cellProperties.renderer = LateralIzq;
            }

            if (row >= 0) {
                switch (col) {
                    case 0:
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                        cellProperties.className = "htCenter htMiddle"; cellProperties.readOnly = true;
                        break;
                    case posAntepenultimaColumna:
                    case posPenultimaColumna:
                        cellProperties.className = "htCenter htMiddle";
                        cellProperties.type = 'numeric';
                        cellProperties.format = '0,0.00%';

                        break;
                    case posUltimaColumna:
                        cellProperties.className = "htCenter htMiddle";
                        break;
                    default: //rangos de meses
                        cellProperties.className = "htCenter htMiddle";
                        cellProperties.type = 'numeric';
                        cellProperties.format = '0,0.00';
                        break;
                }
            }
            return cellProperties;
        }
    });
}

function crearGrillaExcelFactorFortTermico(tab, container, handson, heightHansonTab) {
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
    var posPenultimaColumna = columns.length - 2;
    var posUltimaColumna = columns.length - 1;

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
        fixedColumnsLeft: 5,
        cells: function (row, col, prop) {
            var cellProperties = {
                type: 'time',
                timeFormat: 'HH:mm',
                correctFormat: true
            };

            if (col == 0 && row >= 0) {
                cellProperties.renderer = ColorEmpresa;
            }
            if (col > 0 && col < 2 && row >= 0) {
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
                        cellProperties.renderer = render_incremental;
                        break;
                    case posPenultimaColumna:
                        cellProperties.className = "htCenter htMiddle";
                        cellProperties.type = 'numeric';
                        cellProperties.format = '0,0.00%';
                        break;
                    case posUltimaColumna:
                        cellProperties.className = "htCenter htMiddle";
                        break;
                    default: //rangos de meses
                        cellProperties.className = "htCenter htMiddle";
                        cellProperties.type = 'numeric';
                        cellProperties.format = '0,0.00';
                        break;
                }
            }
            return cellProperties;
        }
    });
}

function crearGrillaExcelFactorProgTermico(tab, container, handson, heightHansonTab) {
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
    var posAntepenultimaColumna = columns.length - 3;
    var posPenultimaColumna = columns.length - 2;
    var posUltimaColumna = columns.length - 1;

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
        cells: function (row, col, prop) {
            var cellProperties = {
                type: 'time',
                timeFormat: 'HH:mm',
                correctFormat: true
            };

            if (col == 0 && row >= 0) {
                cellProperties.renderer = ColorEmpresa;
            }
            if (col > 0 && col < 2 && row >= 0) {
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
                        cellProperties.renderer = render_incremental;
                        break;
                    case posAntepenultimaColumna:
                    case posPenultimaColumna:
                        cellProperties.className = "htCenter htMiddle";
                        cellProperties.type = 'numeric';
                        cellProperties.format = '0,0.00%';

                        break;
                    case posUltimaColumna:
                        cellProperties.className = "htCenter htMiddle";
                        break;
                    default: //rangos de meses
                        cellProperties.className = "htCenter htMiddle";
                        cellProperties.type = 'numeric';
                        cellProperties.format = '0,0.00';
                        break;
                }
            }
            return cellProperties;
        }
    });
}


/////////////////////////////////////

function render_fp_flag(instance, td, row, col, prop, value, cellProperties) {
    cellProperties.className = "htCenter htMiddle";

    Handsontable.renderers.TextRenderer.apply(this, arguments);

    if (value != null) {

        //var colorTexto = '#777';
        if (value != "NO") {
            var colorFondo = '#FFC7CE';
            var color = 'red';
            var objCss = {
                "background-color": colorFondo,
                "color": color,
            };

            $(td).css(objCss);
        }
    }

    return td;
}

function render_fp(instance, td, row, col, prop, value, cellProperties) {
    cellProperties.className = "htCenter htMiddle";
    cellProperties.type = 'numeric';

    Handsontable.renderers.TextRenderer.apply(this, arguments);

    if (value != null) {

        //var colorTexto = '#777';
        if (value != 1) {
            var colorFondo = '#FFC7CE';
            var color = 'red';
            var objCss = {
                "background-color": colorFondo,
                "color": color,
            };

            $(td).css(objCss);

            var objCss2 = {
                "color": color,
            };
            $(instance.getCell(row, col - 2)).css(objCss2);
            $(instance.getCell(row, col - 1)).css(objCss2);
        }

        $(td).html(formatFloat(parseFloat(value), 2, '.', ','));
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

    return td;
}

function render_incremental(instance, td, row, col, prop, value, cellProperties) {
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

//funciones genericas

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
