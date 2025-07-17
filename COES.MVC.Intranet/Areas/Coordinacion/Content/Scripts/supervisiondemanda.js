var controlador = siteRoot + 'coordinacion/supervisiondemanda/';
var hot;
var numeroTotal = 31;

$(function () {    

    $('#btnConsultar').click(function () {
        consultar();
    }); 

    $('#btnExportar').click(function () {
        exportar();
    }); 

    consultar();
});

function headerRender(instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);
    td.style.fontWeight = 'bold';
    td.style.fontSize = '11px';
    td.style.color = '#ffffff';
    td.style.background = '#2980B9';
};

function normalRender(instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);
    td.style.fontSize = '11px';
    td.style.background = '#E8F6FF';
};

function oddRender(instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);
    td.style.fontSize = '11px';
    td.style.background = '#EAF7D9';
};

function contentRender(instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);
    td.style.fontSize = '11px';
    td.style.textAlign = 'right';
};


consultar = function () {

    $.ajax({
        type: 'POST',
        url: controlador + 'grilla',
        data: {
            tipo: $('#cbFuente').val(),
            idGrupo: $('#cbGrupo').val(),
            tipoGeneracion: $('#cbTipoGeneracion').val()
        },
        dataType: 'json',
        success: function (result) {

            if (typeof hot != 'undefined') {
                hot.destroy();
            }

            container = document.getElementById('listado');
            hotOptions = {
                data: result.Datos,
                height: 600,
                maxRows: result.Datos.length,
                maxCols: result.Datos[0].length,
                colHeaders: true,
                rowHeaders: true,
                fillHandle: true,
                columnSorting: false,
                className: "htCenter",
                readOnly: true,
                fixedRowsTop: 1,
                fixedColumnsLeft: 1,
                cells: function (row, col, prop) {
                    var cellProperties = {};

                    if (row == 0) {
                        cellProperties.renderer = headerRender;
                    }
                    else if (row > 0 && col == 0) {
                        cellProperties.renderer = normalRender;
                    }
                    else {
                        cellProperties.renderer = contentRender;
                    }

                    if (row > 0 && result.Indices[col] == 1) {
                        cellProperties.renderer = oddRender;
                    }

                    return cellProperties;
                }
            };
            hot = new Handsontable(container, hotOptions);
        },
        error: function () {
            alert("Error.");
        }
    });
};

exportar = function () {

    $.ajax({
        type: 'POST',
        url: controlador + 'exportar',
        data: {
            tipo: $('#cbFuente').val(),
            idGrupo: $('#cbGrupo').val(),
            tipoGeneracion: $('#cbTipoGeneracion').val()
        },
        dataType: 'json',
        success: function (result) {
            if (result == 1) {
                document.location.href = controlador + 'descargar';
            }           
        },
        error: function () {
            alert("Error.");
        }
    });

}
