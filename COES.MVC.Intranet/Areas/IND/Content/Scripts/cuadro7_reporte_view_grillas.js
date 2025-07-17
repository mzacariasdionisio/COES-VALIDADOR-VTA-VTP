var container;
var previoChange = 0;
var HoraIniPunta = 1020;
var HoraFinPunta = 1380;

var CUADRO_FACTOR_K = 3;
var CUADRO_CUADRO5 = 5;
var CUADRO_POTASEGURADA = 6;
var CUADRO_CUADRO7 = 7;

const COL_CENTRAL = 2;

var COL_PE_CUADROASG = 5;
var COL_PE_FACTORK = 5;
var tablaCambios;

function crearGrillaExcelCuadro7(tab, container, handson, heightHansonTab) {
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

            if (row >= 0 && col <= 3) {
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

            if (row >= 0 && col > 3) {
                var resto = col % 4;
                if (resto == 0 || resto == 2 || resto == 3) {
                    //hop
                    cellProperties.className = "htRight htMiddle";
                    cellProperties.type = 'numeric';
                    cellProperties.format = '0,0.00';
                }

                if (resto == 1) {
                    //numarra
                    cellProperties.className = "htRight htMiddle";
                    cellProperties.type = 'numeric';
                    cellProperties.format = '0,0';
                }
            }
            return cellProperties;
        }
    });
}
