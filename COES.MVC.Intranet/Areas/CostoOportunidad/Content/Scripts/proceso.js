var controlador = siteRoot + 'costooportunidad/proceso/';
var hot1 = null;
var hot2 = null;
var hot3 = null;
var hot4 = null;
var hot5 = null;
var hot6 = null;
var hot7 = null;
var hot8 = null;      

$(function () {

    $('#btnImportar').on('click', function () {
        importar();
    });

    $('#btnConsultar').on('click', function () {
        consultar();
    });

    $('#cbPeriodo').on('click', function () {
        cargarVersion();
    });

    $('#btnConsultarRADetalle').on('click', function () {
        consultarRaDetalle();
    });

    $('#tab-container').easytabs({
        animate: false
    });

    $('#tab-container-datos').easytabs({
        animate: false
    });

    $('#tab-container-resultados').easytabs({
        animate: false
    });

    $('#btnProcesar').on('click', function () {
        procesar(0);
    });

    $('#btnProcesarConDatos').on('click', function () {
        procesar(1);
    });

    $('#btnReprocesar').on('click', function () {
        openReprocesar();
    });

    $('#btnEnviarLiquidacion').on('click', function () {
        openLiquidaciones();
    });

    $('#btnExportarResultado').on('click', function () {
        exportarResultado();
    });

    $('#btnExportarInsumo').on('click', function () {
        exportarInsumo();
    });

    $('#btnHistorioEnvio').on('click', function () {
        historioLiquidaciones();
    });

    $('#btnProcesar').hide();
    $('#btnProcesarConDatos').hide();
    $('#btnReprocesar').hide();
    $('#btnEnviarLiquidacion').hide();
    $('#btnExportarResultado').hide();
    $('#btnHistorioEnvio').hide();
    

    cargarVersion();
});

procesar = function (option) {
    if (confirm('¿Está seguro de realizar esta operación?')) {

        $.ajax({
            type: 'POST',
            url: controlador + 'procesar',
            data: {
                idVersion: $('#cbVersion').val(),
                option: option
            },
            dataType: 'json',
            success: function (result) {               
                if (result == 1) {
                    cargarResultados();
                }
                else if (result == 2) {
                    mostrarMensaje('mensaje', 'alert', 'No existen datos SP7 para todos los dias.');
                }
                else if (result == 3) {
                    mostrarMensaje('mensaje', 'alert', 'Existen días con proceso de completado de datos SP7 con estado fallido.');
                }
                else if (result == 4) {
                    mostrarMensaje('mensaje', 'alert', 'El periodo de programación debe ser 0.5.');
                }
                else if (result == 5) {
                    mostrarMensaje('mensaje', 'alert', 'Error al ejecutar las alfas y betas.');
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
};

cargarResultados = function () {

    $.ajax({
        type: 'POST',
        url: controlador + 'obtenerresultado',
        data: {
            idVersion: $('#cbVersion').val()
        },
        dataType: 'json',
        success: function (result) {

            if (result != -1) {
                        
                $('#despachoConReserva').html('');
                $('#despachoSinReserva').html('');

                if (typeof hot7 != 'undefined' && hot7 != null) {
                    hot7.destroy();
                }
                if (typeof hot8 != 'undefined' && hot8 != null) {
                    hot8.destroy();
                }               

                pintarTablaDespacho('despachoConReserva', result.DatosDespacho, result.ColoresDespacho, 7, 2);
                pintarTablaDespacho('despachoSinReserva', result.DatosDespachoSinR, result.ColoresDespachoSinR, 8, 2);          

            }
            else {
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
            }
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });

};

consultar = function () {

    if ($('#cbVersion').val() != "0") {

        $.ajax({
            type: 'POST',
            url: controlador + 'validarperiodo',
            data: {
                idVersion: $('#cbVersion').val()
            },
            dataType: 'json',
            success: function (validacion) {                

                if (validacion > 0) {
                    $('#btnProcesar').hide();
                    $('#btnProcesarConDatos').hide();
                    $('#btnReprocesar').show();
                    $('#btnEnviarLiquidacion').show();
                    $('#btnExportarResultado').show();
                    $('#btnHistorioEnvio').show();

                    cargarResultados();
                }
                else {
                    $('#btnProcesar').hide();
                    $('#btnProcesarConDatos').show();
                    $('#btnReprocesar').hide();
                    $('#btnEnviarLiquidacion').hide();
                    $('#btnExportarResultado').hide();
                    $('#btnHistorioEnvio').hide();
                }

                $.ajax({
                    type: 'POST',
                    url: controlador + 'obtenerdatos',
                    data: {
                        idVersion: $('#cbVersion').val()
                    },
                    dataType: 'json',
                    success: function (result) {

                        if (result != -1) {

                            $('#programaConReserva').html('');
                            $('#programaSinReserva').html('');
                            $('#raProgramadaDown').html('');
                            $('#raProgramadaUp').html('');
                            $('#raEjecutadaDown').html('');
                            $('#raEjecutadaUp').html('');                          


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

                            $('#contentResultado').show();

                        }
                        else {
                            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
                        }
                    },
                    error: function () {
                        mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
                    }
                });

            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
            }
        });
    }
    else
    {
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
    else if (hot == 7) {
        hot7 = new Handsontable(container, hotOptions);
    }
    else if (hot == 8) {
        hot8 = new Handsontable(container, hotOptions);
    }
}

pintarTablaDespacho = function (idContainer, datos, colores, hot, tipo) {

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

    function rosaroRender(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontSize = '11px';
        td.style.background = '#FF69B4';
        td.style.textAlign = 'right';
    }

    function naranroRender(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontSize = '11px';
        td.style.background = '#ffd569';
        td.style.textAlign = 'right';
    }

    function verdeRender(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontSize = '11px';
        td.style.background = '#B8F367';
        td.style.textAlign = 'right';
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

                if (col > 0) {
                    if (row >= 2) {
                        if (row - 2 < colores.length) {
                         
                            if (colores[row - 2][col - 1] == 1) {
                                cellProperties.renderer = naranroRender;
                            }
                            if (colores[row - 2][col - 1] == 2) {
                                cellProperties.renderer = rosaroRender;
                            }
                            if (colores[row - 2][col - 1] == 3) {
                                cellProperties.renderer = verdeRender;
                            }
                        }
                    }
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
    else if (hot == 7) {
        hot7 = new Handsontable(container, hotOptions);
    }
    else if (hot == 8) {
        hot8 = new Handsontable(container, hotOptions);
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
        $.ajax({
            type: 'POST',
            url: controlador + 'importar',
            data: {
                idVersion: $('#cbVersion').val()
            },
            dataType: 'json',           
            success: function (result) {
                if (result != -1) {
                    mostrarMensaje('mensaje', 'exito', 'Los datos se importaron correctamente. Ahora puede calcular los costos de oportunidad.');
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

openReprocesar = function () {
    $.ajax({
        type: 'POST',
        url: controlador + 'reprocesar',
        data: {
            idVersion: $('#cbVersion').val()
        },        
        success: function (evt) {
            $('#contenidoReprocesar').html(evt);

            $('#trFechaInicio').hide();
            $('#trFechaFin').hide();
            $('#trImportarSP7').hide();

            setTimeout(function () {
                $('#popupReprocesar').bPopup({
                    autoClose: false
                });
            }, 50);

            $('#cbReprocesar').on('change', function () {
                if ($('#cbReprocesar').val() == '1') {
                    $('#trFechaInicio').show();
                    $('#trFechaFin').show();
                }
            });

            $('#cbUtilizarAlfas').on('change', function () {
                if ($('#cbUtilizarAlfas').val() == '0') {
                    $('#trImportarSP7').show();
                }
                else {
                    $('#cbImportarSP7').val("0");
                }
            });

            $('#txtFecInicioReproceso').Zebra_DatePicker({
                direction: [$('#hfFecInicio').val(), $('#hfFecFin').val()],
                pair: $('#txtFecFinReproceso'),
                onSelect: function (date) {                  
                    var date1 = getFecha(date);
                    var date2 = getFecha($('#txtFecFinReproceso').val());

                    if (date1 > date2) {
                        $('#txtFecFinReproceso').val(date);
                    }
                }
            });

            $('#txtFecFinReproceso').Zebra_DatePicker({
                direction: [$('#hfFecInicio').val(), $('#hfFecFin').val()],
                onSelect: function (date) {
                    var date1 = getFecha(date);
                    var date2 = getFecha($('#txtFecInicioReproceso').val());

                    if (date1 < date2) {
                        $('#txtFecFinReproceso').val($('#txtFecInicioReproceso').val());
                    }
                }
            });

            $('#btnReprocesarOK').on('click', function () {
                ejecutarReproceso();
            });

            $('#btnReprocesarCancelar').on('click', function () {
                $('#popupReprocesar').bPopup().close();
            });
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
};

openLiquidaciones = function () {
    $.ajax({
        type: 'POST',
        url: controlador + 'liquidacion',
        data: {
            idVersion: $('#cbVersion').val()
        },
        success: function (evt) {
            $('#contenidoLiquidacion').html(evt);

            setTimeout(function () {
                $('#popupLiquidacion').bPopup({
                    autoClose: false
                });
            }, 50);           

            $('#cbPeriodoTrn').on('change', function () {
                cargarVersionTrn();
            });

            $('#btnLiquidacionOK').on('click', function () {
                ejecutarLiquidacion();
            });

            $('#btnLiquidacionCancelar').on('click', function () {
                $('#popupLiquidacion').bPopup().close();
            });

            if ($('#hfPeriodoTrn').val() != '-1') {
                $('#cbPeriodoTrn').val($('#hfPeriodoTrn').val());
                cargarVersionTrn();
                $('#cbPeriodoTrn').prop('disabled', true);

            }            
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
};

ejecutarLiquidacion = function () {
    if ($('#cbVersionTrn').val() != "0" && $('#cbVersionTrn').val() != "" && $('#cbVersionTrn').val() != null) {
        if (confirm('¿Está seguro de realizar esta operación?')) {
            $.ajax({
                type: 'POST',
                url: controlador + 'ejecutarliquidacion',
                data: {
                    idVersion: $('#cbVersion').val(),
                    idPeriodoTrn: $('#cbPeriodoTrn').val(),
                    idVersionTrn: $('#cbVersionTrn').val()                    
                },
                dataType: 'json',
                success: function (result) {
                    if (result == 1) {
                        $('#popupLiquidacion').bPopup().close();
                        mostrarMensaje('mensaje', 'exito', 'La operación se realizó correctamente.');
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
        alert("Seleccione la versión del aplicativo Compensaciones RSF");
    }
};

ejecutarReproceso = function () {

    if (confirm('¿Está seguro de realizar esta operación?')) {
        $.ajax({
            type: 'POST',
            url: controlador + 'ejecutarreproceso',
            data: {
                idVersion: $('#cbVersion').val(),
                indicador: $('#cbReprocesar').val(),
                fechaInicio: $('#txtFecInicioReproceso').val(),
                fechaFin: $('#txtFecFinReproceso').val(),
                indicadorDatos: $('#cbReemplazarDatos').val(),
                option: $('#cbUtilizarAlfas').val(),
                importarSP7: $('#cbImportarSP7').val()
            },
            dataType: 'json',
            success: function (result) {
                if (result == 1) {
                    mostrarMensaje('mensaje', 'exito', 'La operación se realizó correctamente.');
                    $('#popupReprocesar').bPopup().close();
                    consultar();                    
                    $('#btnProcesar').hide();
                    $('#btnReprocesar').show();
                    $('#btnEnviarLiquidacion').show();
                    $('#btnExportarResultado').show();
                    $('#btnHistorioEnvio').show();
                }
                else if (result == 2) {
                    mostrarMensaje('mensaje', 'alert', 'No existen datos SP7 para todos los dias.');
                }
                else if (result == 3) {
                    mostrarMensaje('mensaje', 'alert', 'Existen días con proceso de completado de datos SP7 con estado fallido.');
                }
                else if (result == 4) {
                    mostrarMensaje('mensaje', 'alert', 'El periodo de programación debe ser 0.5.');
                }
                else if (result == 5) {
                    mostrarMensaje('mensaje', 'alert', 'Error al ejecutar las alfas y betas.');
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
};

cargarVersionTrn = function () {

    $.ajax({
        type: 'POST',
        url: controlador + 'obtenerrecalculotrn',
        data: {
            idPeriodo: $('#cbPeriodoTrn').val()
        },
        dataType: 'json',
        global: false,
        success: function (result) {
            if (result != -1) {
                $('#cbVersionTrn').get(0).options.length = 0;
                $('#cbVersionTrn').get(0).options[0] = new Option("--SELECCIONE--", "0");
                $.each(result, function (i, item) {
                    $('#cbVersionTrn').get(0).options[$('#cbVersionTrn').get(0).options.length] = new Option(item.Vcrecanombre, item.Vcrecacodi);
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

exportarInsumo = function () {

    if ($('#cbVersion').val() != "0" && $('#cbVersion').val() != "" && $('#cbVersion').val() != null) {
        $.ajax({
            type: 'POST',
            url: controlador + "exportarinsumo",
            data: {
                idVersion: $('#cbVersion').val()
            },
            dataType: 'json',
            cache: false,
            success: function (resultado) {
                if (resultado == 1) {
                    location.href = controlador + "descargarinsumo";
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

exportarResultado = function () {

    if ($('#cbVersion').val() != "0" && $('#cbVersion').val() != "" && $('#cbVersion').val() != null) {
        $.ajax({
            type: 'POST',
            url: controlador + "exportarresultado",
            data: {
                idVersion: $('#cbVersion').val()
            },
            dataType: 'json',
            cache: false,
            success: function (resultado) {
                if (resultado == 1) {
                    location.href = controlador + "descargarresultado";
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

historioLiquidaciones = function () {
    $.ajax({
        type: 'POST',
        url: controlador + 'envioliquidacion',
        data: {
            idVersion: $('#cbVersion').val()
        },
        success: function (evt) {
            $('#contenidoEnvioLiquidacion').html(evt);

            setTimeout(function () {
                $('#popupEnvioLiquidacion').bPopup({
                    autoClose: false
                });
            }, 50);
            
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
};

consultarRaDetalle = function () {
    $.ajax({
        type: 'POST',
        url: controlador + 'reservaasignada',
        data: {
            idVersion: $('#cbVersion').val()
        },
        success: function (evt) {
            $('#contenidoRADetalle').html(evt);

            setTimeout(function () {
                $('#popupRADetalle').bPopup({
                    autoClose: false
                });
            }, 50);

        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
};

mostrarMensaje = function (id, tipo, mensaje) {
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);
};

getFecha = function (date) {
    var parts = date.split("/");
    var date = new Date(parts[1] + "/" + parts[0] + "/" + parts[2]);
    return date.getTime();
}