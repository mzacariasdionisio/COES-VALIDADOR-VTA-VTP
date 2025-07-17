var controlador = siteRoot + 'costooportunidad/copiardatos/';

var hot1 = null;
var hot2 = null;
var hot3 = null;
var hot4 = null;
var hot5 = null;
var hot6 = null;

$(function () {

    $('#btnImportar').on('click', function () {
        importar();
    });

    $('#btnConsultar').on('click', function () {
        consultar();
    });

    $('#btnExportar').on('click', function () {
        exportar();
    });

    $('#cbPeriodo').on('click', function () {
        cargarVersion();
    });

    $('#tab-container').easytabs({
        animate: false
    });

    cargarVersion();
});

consultar = function () {
    if ($('#cbVersion').val() != "0") {
    $.ajax({
        type: 'POST',
        url: controlador + 'obtenerdatos',
        data: {
            idVersion: $('#cbVersion').val()
        },
        dataType: 'json',
        success: function (result) {

            if (result != -1) {
                
                if (typeof hot1 != 'undefined' && hot1 != null) {
                    hot1.destroy();
                }
                if (typeof hot2 != 'undefined' && hot2 != null) {
                    hot2.destroy();
                }
                if (typeof hot3 != 'undefined' && hot3 != null) {
                    hot3.destroy();
                }
                if (typeof hot4 != 'undefined' && hot4 != null) {
                    hot4.destroy();
                }
                if (typeof hot5 != 'undefined' && hot5 != null) {
                    hot5.destroy();
                }
                if (typeof hot6 != 'undefined' && hot6 != null) {
                    hot6.destroy();
                }

                pintarTabla('programaConReserva', result.DatosPrograma, 1, 1);
                pintarTabla('programaSinReserva', result.DatosProgramaSinReserva, 2, 1);
                pintarTabla('raProgramadaDown', result.DatosRAProgramadaDown, 3, 2);
                pintarTabla('raProgramadaUp', result.DatosRAProgramadaUp, 4, 2);
                pintarTabla('raEjecutadaDown', result.DatosRAEjecutadaDown, 5, 2);
                pintarTabla('raEjecutadaUp', result.DatosRAEjecutadaUp, 6, 2);
               
               

            }
            else {
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
            }
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
        });
    }
    else {
        mostrarMensaje('mensaje', 'alert', 'Seleccione periodo y versión');
    }
};

pintarTabla = function (idContainer, datos, hot, tipo) {

    function headerRender(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontWeight = 'bold';
        td.style.fontSize = '11px';
        td.style.color = '#ffffff';
        td.style.background = '#2980B9';
    }

    function normalRender(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontSize = '11px';
        td.style.background = '#E8F6FF';
    }

    function oddRender(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontSize = '11px';
        td.style.background = '#EAF7D9';
    }

    function contentRender(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontSize = '11px';
        td.style.textAlign = 'right';
    }

    var container = document.getElementById(idContainer);
    var hotOptions = {
        data: datos,
        height: 600,
        maxRows: datos.length,
        maxCols: datos[0].length,
        colHeaders: true,
        rowHeaders: true,
        fillHandle: true,
        fixedRowsTop: tipo,
        columnSorting: false,
        className: "htCenter",
        readOnly: true,
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

            if (tipo == 2) {
                if (row == 1) {
                    cellProperties.renderer = headerRender;
                }  
            }

            return cellProperties;
        }
    };

    if (hot == 1) {
        hot1 = new Handsontable(container, hotOptions);
    }
    else if (hot == 2) {
        hot2 = new Handsontable(container, hotOptions);
    }
    else if (hot == 3) {
        hot3 = new Handsontable(container, hotOptions);
    }
    else if (hot == 4) {
        hot4 = new Handsontable(container, hotOptions);
    }
    else if (hot == 5) {
        hot5 = new Handsontable(container, hotOptions);
    }
    else if (hot == 6) {
        hot6 = new Handsontable(container, hotOptions);
    }
}

cargarVersion = function () {

    $.ajax({
        type: 'POST',
        url: controlador + 'obtenerversiones',
        data: {
            idPeriodo: $('#cbPeriodo').val()
        },
        dataType: 'json',
        global: false,
        success: function (result) {
            if (result != -1) {
                $('#cbVersion').get(0).options.length = 0;
                $('#cbVersion').get(0).options[0] = new Option("--SELECCIONE--", "0");
                $.each(result, function (i, item) {
                    $('#cbVersion').get(0).options[$('#cbVersion').get(0).options.length] = new Option(item.Coverdesc, item.Covercodi);
                });
            }
            else {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            }
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        }
    });
};

importar = function () {
    if (confirm('¿Está seguro de realizar esta operación?')) {
        if ($('#cbVersion').val() != "0") {
            $.ajax({
                type: 'POST',
                url: controlador + 'importar',
                data: {
                    idVersion: $('#cbVersion').val()
                },
                dataType: 'json',
                success: function (result) {
                    if (result != -1) {
                        mostrarMensaje('mensaje', 'exito', 'Los datos se importaron correctamente. Ahora puede calcular los costos de oportunidad, ingresar a la opción de procesamiento.');
                    }
                    else {
                        mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
                    }
                },
                error: function () {
                    mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
                }
            });
        }
        else {
            mostrarMensaje('mensaje', 'alert', 'Seleccione periodo y versión');
        }
    }
   
};


exportar = function () {

    if ($('#cbVersion').val() != "0" && $('#cbVersion').val() != "" && $('#cbVersion').val() != null) {
        $.ajax({
            type: 'POST',
            url: controlador + "exportar",
            data: {
                idVersion: $('#cbVersion').val()
            },
            dataType: 'json',
            cache: false,
            success: function (resultado) {
                if (resultado == 1) {
                    location.href = controlador + "descargar";
                }
                else {
                    mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
                }
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
            }
        });
    }
    else {
        mostrarMensaje('mensaje', 'alert', 'Seleccione periodo y versión');
    }
}


mostrarMensaje = function (id, tipo, mensaje) {
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);
};