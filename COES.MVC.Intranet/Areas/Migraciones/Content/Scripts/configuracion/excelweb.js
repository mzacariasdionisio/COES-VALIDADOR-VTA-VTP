/// Crea El objeto Handson en la pagina web
var container;
var hotRol;
var hotOptions;
var LISTA_ACT_NO_REPETIBLES = ["A1", "A2", "A3", "C1", "C2", "C3", "P1", "P2", "P3", "SC", "sjC1", "sjC2", "sjC3", "sjE1", "sjE2", "sjE3", "sjA1", "sjA2", "sjA3", "SJOI", "SJOI2", "SJOI3"];
var LISTA_ACT_NO_REPETIBLES_MAS_2_VECES = [ "E1", "E2", "E3"];
var grillaBD;
var grillaBDComment;
var evtHot;

//constantes error
var ERROR_BLANCO = 0;
var ERROR_NUMERO = 1;
var ERROR_NO_ACTIVIDAD = 2;
var ERROR_ACTIVIDAD_REPETIDA = 3;

var ERROR_GLOBAL = [
    { tipo: 'BLANCO', Descripcion: 'BLANCO', total: 0, codigo: ERROR_BLANCO, Background_color: '', Color: '', validar: false },
    { tipo: 'NUMERO', Descripcion: 'NÚMERO', total: 0, codigo: ERROR_NUMERO, Background_color: 'white', Color: '', validar: false },
    { tipo: 'NO_ACTIVIDAD', Descripcion: 'NO ES ACTIVIDAD VÁLIDA', total: 0, codigo: ERROR_NO_ACTIVIDAD, BackgroundColor: 'red', Color: '', validar: false },
    { tipo: 'ACTIVIDAD_REPETIDA', Descripcion: 'ACTIVIDAD REPETIDA', total: 0, codigo: ERROR_ACTIVIDAD_REPETIDA, BackgroundColor: 'orange', Color: '', validar: false }
];

function crearExcelRolturnos(evt, heightHansonTab) {
    grillaBD = evt.Handson.ListaExcelData;
    grillaBDComment = evt.Handson.ListaExcelComment;

    var activid = evt.ListaString;
    var ListaEmpty = evt.ListaStringNoRepet;
    container = document.getElementById('excelweb');
    var tituloRenderer = function (instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.backgroundColor = '#16365C';
        td.style.fontSize = '11px';
        td.style.color = '#FFFFFF';
        td.style.verticalAlign = 'middle';
    };
    var ColorResponsable = function (instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.backgroundColor = '#4F81BD';
        td.style.fontSize = '11px';
        td.style.color = 'white'
    };
    var validarActi = function (instance, td, row, col, prop, value, cellProperties) {

        if (td != null) {
            if (value != null) {
                var numFilCabecera = 2;
                var error = obtenerErrorGlobal(value, row, col, ERROR_GLOBAL, numFilCabecera);
                var tipoError = error != null ? error.Tipo : '';

                switch (tipoError) {
                    case ERROR_NO_ACTIVIDAD:
                        td.style.background = ERROR_GLOBAL[ERROR_NO_ACTIVIDAD].BackgroundColor;

                        break;
                    case ERROR_ACTIVIDAD_REPETIDA:
                        td.style.background = ERROR_GLOBAL[ERROR_ACTIVIDAD_REPETIDA].BackgroundColor;
                        break;
                    default:
                        render_ColorResult(td, row, col, 1, 1);
                        render_ColorResult(td, row, col, 1, 2)
                        break;
                }
            }
        }

        Handsontable.TextCell.renderer.apply(this, arguments);
    };

    hotOptions = {
        data: grillaBD,
        height: heightHansonTab,
        colHeaders: true,
        rowHeaders: true,
        hiddenColumns: {
            columns: [2],
            indicators: true
        },
        fillHandle: true,
        columnSorting: false,
        className: "htCenter",
        colWidths: evt.Handson.ListaColWidth,
        fixedRowsTop: 2,
        fixedColumnsLeft: 2,
        afterChange: changess,
        readOnly: evt.Handson.ReadOnly,
        cells: function (row, col, prop) {

            var cellProperties = {};

            if (row == 0 || row == 1) {
                cellProperties.renderer = tituloRenderer;
                cellProperties.readOnly = true;
            }

            if ((col == 0 || col == 1) && row > 1) {
                cellProperties.renderer = ColorResponsable;
            }

            if (row > 1) {
                switch (col) {
                    case 0: cellProperties.className = "htCenter htMiddle"; cellProperties.readOnly = true; break;
                    case 1: cellProperties.className = "htLeft htMiddle"; cellProperties.readOnly = true; break;
                    case 2: cellProperties.className = "htLeft htMiddle"; cellProperties.readOnly = true; break;
                    default:
                        cellProperties.className = "htRight htMiddle";
                        cellProperties.renderer = validarActi;
                        break;
                }
            }
            return cellProperties;
        }
    };
    hotRol = new Handsontable(container, hotOptions);
    hotRol.render();
}

function getIndicesOf(searchStr, str, lista) {
    var searchStrLen = searchStr.length;
    if (searchStrLen == 0) {
        return [];
    }
    var startIndex = 0, index, indices = [];

    str = str.toUpperCase();
    searchStr = searchStr.toUpperCase();

    var contador = 0;
    for (var i = 0; i < lista.length; i++) {
        if (searchStr != null && searchStr != "" && searchStr.toUpperCase() == lista[i].toUpperCase()) {
            contador++;
        }
    }

    return contador >= 2;
}

function changess(changes, source) {
    if (!changes) {
        return;
    }
    var instance = this;

    var listaMsj = '';
    var cont = 0;
    changes.forEach(function (change) {
        row = change[0];
        col = change[1];
        newValue = change[3];

        if (newValue != "") {
            var result = "";
            var listaTmp = [];
            for (var x = 2; x < grillaBD.length; x++) {
                result += grillaBD[x][col] + ",";
                listaTmp.push(grillaBD[x][col]);
            }

            if (esValidable(newValue)) {
                var firstVal = getIndicesOf(newValue, result, listaTmp);
                var secondVal = esActividadNoRepetible(newValue);
                var thirdVal = esActividadNoRepetible2Veces(newValue, listaTmp);
                if (firstVal && secondVal) {
                    listaMsj += "Actividad " + newValue + " ya se encuentra asignado." + "\n";
                }
                if (firstVal && thirdVal) {
                    listaMsj += "Actividad " + newValue + " ya se encuentra asignado." + "\n";
                }
            }
        }
    });
}

function esActividadNoRepetible(value) {
    for (var i = 0; i < LISTA_ACT_NO_REPETIBLES.length; i++) {
        if (value != null && value != "" && value.toUpperCase() == LISTA_ACT_NO_REPETIBLES[i]) {
            return true;
        }
    }

    return false;
}

function esActividadNoRepetible2Veces(value, ltemp) {

    var cont = 0;
    for (var j = 0; j < ltemp.length; j++) {
        for (var i = 0; i < LISTA_ACT_NO_REPETIBLES_MAS_2_VECES.length; i++) {
            if (value != null && value != "" && value.toUpperCase() == LISTA_ACT_NO_REPETIBLES_MAS_2_VECES[i]) {
                
                if (ltemp[j] == value) { cont += 1;}
                if (cont > 2) return true;
            }
        }
    }

    return false;
}

function esValidable(str) {
    if (str != null && str != "" && str.length >= 2) {
        var cadena = str.toUpperCase();

        var part1 = cadena.substring(0, 1);
        var part2 = cadena.substring(1, str.length);
        if (part1 == "A" || part1 == "C" || part1 == "P" || part1 == "E") {
            return (parseInt(part2) || 0) > 0;
        }

        if (str.length == 2) {
            var part3 = cadena.substring(0, 2);
            return part3 == "SC";
        }
    }

    return false;
}

function render_ColorResult(td, row, col, tipo, x) {
    if (x == 1) {
        if (grillaBD[0][col] == 'S' || grillaBD[0][col] == 'D') {
            td.style.background = '#DCE6F2';
        }
    } else {
        if (grillaBDComment[row][col] == "1") {
            td.style.background = 'lightgreen';
        }
    }
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