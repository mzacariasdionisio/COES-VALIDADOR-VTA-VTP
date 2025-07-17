var hot;
var MODELO_GRID = null;

var OPCION_POPUP_VENTANA = "";

// btn expandir / contraer;
var ocultar = 0;

$(document).ready(function ($) {

    $(document).on('click', '.intervencion', function (e) {
        // your function here
        e.preventDefault();
        var interCodi = $(this).attr("class").split(" ")[1].split("_")[1];
        editarCeldaIntervencion(interCodi);
    });

});

function generarHoTweb() {
    if (typeof hot != 'undefined') {
        hot.destroy();
    }

    var nuevoTamanioTabla = getHeightTablaListado();
    $("#grillaExcel").show();
    nuevoTamanioTabla = nuevoTamanioTabla > 250 ? nuevoTamanioTabla : 250;

    var container = document.getElementById('grillaExcel');
    hot = new Handsontable(container, {
        data: MODELO_GRID.Data,
        maxCols: MODELO_GRID.Columnas.length,
        colHeaders: MODELO_GRID.Headers,
        colWidths: MODELO_GRID.Widths,
        height: nuevoTamanioTabla,
        fixedColumnsLeft: MODELO_GRID.FixedColumnsLeft,
        columnSorting: false,
        manualColumnResize: true,
        sortIndicator: false,
        rowHeaders: false,
        columns: MODELO_GRID.Columnas,
        //hiddenColumns: { //no ocultar columnas porque luego no se renderizan bien algunas celdas
        //    // specify columns hidden by default
        //    columns: [4, 5, 6]
        //},
        outsideClickDeselects: false,
        copyPaste: false,
        autoRowSize: { syncLimit: 30000 },
        cells: function (row, col, prop) {
            var cellProperties = {};

            var columnsColor = MODELO_GRID.ListaColumnasColor;
            for (var i = 0; i < columnsColor.length; i++) {
                if (col == columnsColor[i].indexcabecera && columnsColor[i].isendofweek) {
                    if (columnsColor[i].itypeendofweek == 1) //sabado
                        cellProperties.renderer = firstRowRendererColorSabadoAndSafeHtml;
                    if (columnsColor[i].itypeendofweek == 2) //domingo / feriado
                        cellProperties.renderer = firstRowRendererColorDomingoAndSafeHtml;
                } else if (col == columnsColor[i].indexcabecera && columnsColor[i].isendofweek == false) {
                    cellProperties.renderer = safeHtmlRenderer;
                }
            }

            if (col < MODELO_GRID.FixedColumnsLeft) {
                cellProperties.renderer = titleHtmlRenderer;
            }

            return cellProperties;
        },
        
    });

    container.addEventListener('dblclick', function (e) {
        hot.getActiveEditor().close();
    });

    hot.render();
    updateDimensionHandson(hot);
}

//#endregion

//#region HandsonTable

//#region HandsonTable Style

function safeHtmlRenderer(instance, td, row, col, prop, value, cellProperties) {
    var escaped = Handsontable.helper.stringify(value);

    escaped = strip_tags(escaped, '<em><b><strong><a><big>'); // be sure you only allow certain HTML tags to avoid XSS threats (you should also remove unwanted HTML attributes)
    td.innerHTML = escaped;
    $(td).css({
        "text-align": "center",
    });

    return td;
}

function titleHtmlRenderer(instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);
    td.innerHTML = value;
    $(td).attr('title', value);

    return td;
}

//sabado
function firstRowRendererColorSabadoAndSafeHtml(instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);
    td.style.color = 'black';
    td.style.background = '#B4FFFF';

    var escaped = Handsontable.helper.stringify(value);
    escaped = strip_tags(escaped, '<em><b><strong><a><big>'); // be sure you only allow certain HTML tags to avoid XSS threats (you should also remove unwanted HTML attributes)
    td.innerHTML = escaped;
}

//domingo
function firstRowRendererColorDomingoAndSafeHtml(instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);
    td.style.color = 'black';
    td.style.background = '#00FFFF';

    var escaped = Handsontable.helper.stringify(value);
    escaped = strip_tags(escaped, '<em><b><strong><a><big>'); // be sure you only allow certain HTML tags to avoid XSS threats (you should also remove unwanted HTML attributes)
    td.innerHTML = escaped;
}

function updateDimensionHandson(hot) {
    if (typeof (hot) != "undefined" && hot !== undefined && hot != null) {
        var offset = Handsontable.Dom.offset(document.getElementById('grillaExcel'));
        var widthHT;
        var heightHT;

        if (offset.top == 222) {
            heightHT = $(window).height() - offset.top - 90;
        }
        else {
            heightHT = $(window).height() - offset.top - 20;
        }

        if (ocultar == 1) {
            heightHT = $(window).height() - 10;
        }

        widthHT = $(window).width() - offset.left - 30;

        hot.updateSettings({
            width: widthHT
        });
    }
}

//#endregion

//
function GetArrayHtmlValue(vData) {
    vData = vData != null ? vData : "";

    var listaCadenaCelda = vData.split(' ');

    var arrayIds = []
    for (var i = 0; i < listaCadenaCelda.length; i++) {
        if (listaCadenaCelda[i].startsWith('intercodi_')) {

            var listaCadenaCodigo = listaCadenaCelda[i].split('_');
            arrayIds.push(listaCadenaCodigo[1]);
        }
    }

    return arrayIds;
}

function strip_tags(input, allowed) {
    //var tags = /<\/?([a-z][a-z0-9]*)\b[^>]*>/gi,
    //    commentsAndPhpTags = /<!--[\s\S]*?-->|<\?(?:php)?[\s\S]*?\?>/gi;

    //// making sure the allowed arg is a string containing only tags in lowercase (<a><b><c>)
    //allowed = (((allowed || "") + "").toLowerCase().match(/<[a-z][a-z0-9]*>/g) || []).join('');

    //return input.replace(commentsAndPhpTags, '').replace(tags, function ($0, $1) {
    //    return allowed.indexOf('<' + $1.toLowerCase() + '>') > -1 ? $0 : '';
    //});

    return input;
}

function _inicializarObjetoPosicion() {
    var objPos = { rowIni: 0, colIni: 0, rowFin: 0, colFin: 0, fechaIni: '', equicodi: 0 };
    return objPos;
}

//#endregion

//#region Ventana Nuevo/Edicion Intervenciones Cruzadas

// Carga la ventana de detalle (popup)
function editarCeldaIntervencion(vinterCodi) {
    $('#popupFormIntervencion').html('');
    $("#busquedaEquipo").remove();

    if (APP_MOSTRAR_MODAL_EQUIPO_YA_CARGADO === undefined) { } else {
        APP_MOSTRAR_MODAL_EQUIPO_YA_CARGADO = 0;
    }

    var objParam = {
        interCodi: vinterCodi,
        progrCodi: 0,
        tipoProgramacion: 0,
        escruzadas: true,
    };

    //IntervencionesFormulario.js
    mostrarIntervencion(objParam);
};

//#endregion
