var controlador = siteRoot + 'costooportunidad/consultas/';
var hot = null;
$(function () {   

    $('#btnConsultar').on('click', function () {
        consultar();
    });

    $('#btnExportar').on('click', function () {
        exportar();
    });

    $('#cbPeriodo').on('change', function () {
        cargarVersion();
    });   

    $('#cbVersion').on('change', function () {
        cargarDatosVersion();
    });

    $('#btnPublicacion').on('click', function () {
        exportarPublicacion();
    });

});


function consultar() {

    if ($('#cbVersion').val() != "0" && $('#cbVersion').val() != "" && $('#cbVersion').val() != null) {

        if (parseInt($('#cbTipoInformacion').val()) <= 13) {

            $.ajax({
                type: 'POST',
                url: controlador + 'consultar',
                data: {
                    idVersion: $('#cbVersion').val(),
                    fechaInicio: $('#txtFechaInicio').val(),
                    fechaFin: $('#txtFechaFin').val(),
                    idUrs: $('#cbUrs').val(),
                    idTipoInformacion: $('#cbTipoInformacion').val()
                },
                dataType: 'json',
                success: function (result) {

                    if (result != -1) {

                        if (typeof hot != 'undefined' && hot != null) {
                            hot.destroy();
                        }

                        pintarTabla('divResultado', result.Data, result.Indicador);
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
            mostrarMensaje('mensaje', 'alert', 'No está habilitado esta consulta.');
        }
    }
    else {
        mostrarMensaje('mensaje', 'alert', 'Seleccione periodo y versión');
    }
};

function exportar() {

    if ($('#cbVersion').val() != "0" && $('#cbVersion').val() != "" && $('#cbVersion').val() != null) {

        var tipo = parseInt($('#cbTipoInformacion').val());

        if (tipo <= 13) {

            $.ajax({
                type: 'POST',
                url: controlador + "exportar",
                data: {
                    idVersion: $('#cbVersion').val(),
                    fechaInicio: $('#txtFechaInicio').val(),
                    fechaFin: $('#txtFechaFin').val(),
                    idUrs: $('#cbUrs').val(),
                    idTipoInformacion: $('#cbTipoInformacion').val()
                },
                dataType: 'json',
                cache: false,
                success: function (resultado) {
                    if (resultado == 1) {
                        location.href = controlador + "descargar?idTipoInformacion=" + $('#cbTipoInformacion').val();
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
            
            $.ajax({
                type: 'POST',
                url: controlador + "ExportarSeniales",
                data: {
                    idVersion: $('#cbVersion').val(),
                    fechaInicio: $('#txtFechaInicio').val(),
                    fechaFin: $('#txtFechaFin').val(),
                    idUrs: $('#cbUrs').val(),
                    idTipoInformacion: $('#cbTipoInformacion').val()
                },
                dataType: 'json',
                cache: false,
                success: function (resultado) {
                    if (resultado.Result == 1) {
                        location.href = controlador + "DescargarSeniales?fileName=" + resultado.FileName;
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
    }
    else {
        mostrarMensaje('mensaje', 'alert', 'Seleccione periodo y versión');
    }
};


function exportarPublicacion() {

    if ($('#cbVersion').val() != "0" && $('#cbVersion').val() != "" && $('#cbVersion').val() != null) {
        $.ajax({
            type: 'POST',
            url: controlador + "exportarpublicacion",
            data: {
                idVersion: $('#cbVersion').val(),
                fechaInicio: $('#txtFechaInicio').val(),
                fechaFin: $('#txtFechaFin').val()                
            },
            dataType: 'json',
            cache: false,
            success: function (resultado) {
                if (resultado == 1) {
                    location.href = controlador + "descargarpublicacion";
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

function pintarTabla(idContainer, datos, tipo) {

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
    hot = new Handsontable(container, hotOptions);
}

function cargarVersion() {

    if ($('#cbPeriodo').val() != "0") {

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
    }
};

function cargarDatosVersion() {
    if ($('#cbVersion').val() != "0" && $('#cbVersion').val() != "" && $('#cbVersion').val() != null) {

        $.ajax({
            type: 'POST',
            url: controlador + 'obtenerdatoversion',
            data: {
                idVersion: $('#cbVersion').val()
            },
            dataType: 'json',
            success: function (result) {

                $('#txtFechaInicio').val(result.FechaInicio);
                $('#txtFechaFin').val(result.FechaFin);

                $('#txtFechaInicio').Zebra_DatePicker({
                    direction: [result.FechaInicio, result.FechaFin],
                    pair: $('#txtFechaFin'),
                    onSelect: function (date) {
                        var date1 = getFecha(date);
                        var date2 = getFecha($('#txtFechaFin').val());

                        if (date1 > date2) {
                            $('#txtFechaFin').val(date);
                        }
                    }
                });

                $('#txtFechaFin').Zebra_DatePicker({
                    direction: [result.FechaInicio, result.FechaFin],
                    onSelect: function (date) {
                        var date1 = getFecha(date);
                        var date2 = getFecha($('#txtFechaInicio').val());

                        if (date1 < date2) {
                            $('#txtFechaFin').val($('#txtFechaInicio').val());
                        }
                    }
                });

            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
            }
        });
    }
};

function mostrarMensaje(id, tipo, mensaje) {
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);
};

function getFecha(date_) {
    var parts = date_.split("/");
    var date = new Date(parts[1] + "/" + parts[0] + "/" + parts[2]);
    return date.getTime();
};