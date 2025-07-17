var hot = null;
var hotConsulta = null;
var hotInterconexion = null;

function cargarGrillaConsulta(result) {

    if (hot != null) hot.destroy();
    var merge = [];
    for (var i in result.ListaMerge) {
        merge.push({ row: result.ListaMerge[i].Row, col: result.ListaMerge[i].Col, rowspan: 1, colspan: result.ListaMerge[i].Colspan });
    }

    var tipo = parseInt($('#hfTipoCarga').val());
    var ini = (tipo == 1) ? 4 : 5;

    var grilla = document.getElementById('detalleFormato');


    hot = new Handsontable(grilla, {
        data: result.Data,
        mergeCells: merge,
        fixedRowsTop: ini,
        fixedRowsBottom: 5,
        height: 540,
        rowHeaders: true,
        fixedColumnsLeft: 1,
        cells: function (row, col, prop) {
            var cellProperties = {};

            if (row < ini || col == 0 || row > result.Data.length - 6) {
                cellProperties.renderer = tituloRenderer;
                cellProperties.readOnly = true;
            }
            else {
                var flag = false;
                for (var i in result.ListaInformacionCelda) {
                    if (result.ListaInformacionCelda[i].Row == row && result.ListaInformacionCelda[i].Col == col) {
                        flag = true;
                        var tipo = result.ListaInformacionCelda[i].Tipo;

                        if (tipo == -1) {
                            cellProperties.renderer = sinDatosRenderer;
                            cellProperties.readOnly = true;
                        }
                        else if (tipo == 1) {
                            cellProperties.renderer = estimadoRenderer;
                            cellProperties.readOnly = true;
                        }
                        else if (tipo == 2) {
                            cellProperties.renderer = equipoRenderer;
                            cellProperties.readOnly = true;
                        }
                        else {
                            cellProperties.renderer = defaultRenderer;
                            cellProperties.readOnly = true;
                        }
                    }
                }
                if (!flag) {
                    cellProperties.renderer = defaultRenderer;
                    cellProperties.readOnly = true;
                }
            }

            return cellProperties;
        },
        afterLoadData: function () {
            this.render();
        }
    });
    hot.render();
}

function cargarGrillaReporte(result) {

    if (hotConsulta != null) hotConsulta.destroy();
    var merge = [];
    for (var i in result.ListaMerge) {
        merge.push({ row: result.ListaMerge[i].Row, col: result.ListaMerge[i].Col, rowspan: 1, colspan: result.ListaMerge[i].Colspan });
    }
    var tipo = parseInt($('#hfTipoCarga').val());
    var ini = (tipo == 1) ? 4 : 5;

    var grilla = document.getElementById('detalleConsulta');

    hotConsulta = new Handsontable(grilla, {
        data: result.Data,
        mergeCells: merge,
        fixedRowsTop: ini,
        fixedRowsBottom: 5,
        height: 680,
        rowHeaders: true,
        fixedColumnsLeft: 1,
        cells: function (row, col, prop) {
            var cellProperties = {};

            if (row < ini || col == 0 || row > result.Data.length - 6) {
                cellProperties.renderer = tituloRenderer;
                cellProperties.readOnly = true;
            }
            else {
                var flag = false;
                for (var i in result.ListaFeriados) {
                    if (result.ListaFeriados[i].Row == row && result.ListaFeriados[i].Col == col) {
                        cellProperties.renderer = feriadoRenderer;
                        cellProperties.readOnly = true;
                        flag = true;
                    }
                }
                if (!flag) {
                    cellProperties.renderer = defaultRenderer;
                    cellProperties.readOnly = true;
                }
            }

            return cellProperties;
        },
        afterLoadData: function () {
            this.render();
        }
    });
    hotConsulta.render();
}

function cargarGrillaConsultaInterconexion(result) {

    if (hotInterconexion != null) hotInterconexion.destroy();
    var merge = [];
    for (var i in result.ListaMerge) {
        merge.push({ row: result.ListaMerge[i].Row, col: result.ListaMerge[i].Col, rowspan: 1, colspan: result.ListaMerge[i].Colspan });
    }
    var widths = [];
    for (var i = 0; i <= 100; i++) {
        widths.push(120);
    }

    var grilla = document.getElementById('detalleFormato');

    hotInterconexion = new Handsontable(grilla, {
        data: result.Data,
        mergeCells: merge,
        colWidths: widths,
        fixedRowsTop: 4,
        fixedRowsBottom: (result.Data.length > 8) ? 4 : 0,
        height: 590,
        rowHeaders: true,
        cells: function (row, col, prop) {
            var cellProperties = {};

            if (row < 4 || col == 0 || row > result.Data.length - 5) {
                cellProperties.renderer = tituloRenderer;
                cellProperties.readOnly = true;
            }
            else {
                cellProperties.renderer = defaultRenderer;
                cellProperties.readOnly = true;
            }

            return cellProperties;
        },
        afterLoadData: function () {
            this.render();
        }
    });
    hotInterconexion.render();
}

var tituloRenderer = function (instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);
    td.style.fontWeight = 'bold';
    td.style.fontSize = '11px';
    td.style.textAlign = 'center';
    td.style.verticalAlign = 'middle';
    td.style.color = '#fff';

    //color viene del Generador de reporte
    td.style.backgroundColor = obtenerColorXColumna(row, col);
};

var defaultRenderer = function (instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);
    td.style.fontSize = '11px';
    td.style.verticalAlign = 'middle';
    td.style.textAlign = 'center';
};

var sinDatosRenderer = function (instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);
    td.style.fontSize = '11px';
    td.style.backgroundColor = '#B4C6E7';
    td.style.verticalAlign = 'middle';
    td.style.textAlign = 'center';
};

var estimadoRenderer = function (instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);
    td.style.fontSize = '11px';
    td.style.backgroundColor = '#D9D9D9';
    td.style.verticalAlign = 'middle';
    td.style.textAlign = 'center';
};

var equipoRenderer = function (instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);
    td.style.fontSize = '11px';
    td.style.backgroundColor = '#305496';
    td.style.verticalAlign = 'middle';
    td.style.color = '#fff';
};

var feriadoRenderer = function (instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);
    td.style.fontSize = '11px';
    td.style.backgroundColor = '#22B14C';
    td.style.verticalAlign = 'middle';
    td.style.color = '#000000';
    td.style.textAlign = 'center';
};

function obtenerErrores(data) {
    var html = "<table class='pretty tabla-adicional' id='tablaErrores'>";
    html = html + " <thead>";
    html = html + "     <tr>";
    html = html + "         <th>Error</th>";
    html = html + "     </tr>";
    html = html + " </thead>";
    html = html + " <tbody>";
    for (var k in data) {
        html = html + "     <tr>";
        html = html + "         <td>" + data[k] + "</td>";
        html = html + "     </tr>";
    }
    html = html + " </tbody>";
    html = html + "</table>";

    return html;
}

function getData() {
    return hot.getData();
}

function getSize() {
    return hot.getData().length;
}

function obtenerColorXColumna(row, col) {
    var color = '#4C97C3';
    try {
        //filas cabecera
        if (row < 10) {

            var colorTmp = LISTA_COLOR_PTO[col - 1];
            if (colorTmp === undefined || colorTmp == null) colorTmp = '#4C97C3';

            var tipo = parseInt($('#hfTipoCarga').val());
            if (tipo == 1 && row == 3) {
                color = colorTmp;
            }

            if (tipo != 1) {
                color = colorTmp;
            }
        }
    } catch (e) {
        color = '#4C97C3';
    }

    return color;
}
