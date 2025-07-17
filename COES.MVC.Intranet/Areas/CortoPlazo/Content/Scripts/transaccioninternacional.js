var controlador = siteRoot + 'cortoplazo/reproceso/';
var hot = null;
$(function () {

    $('#txtFecha').Zebra_DatePicker({
        direction: false,
        onSelect: function (date) {
            $('#contentTabla').html("");
            $('#contentDatos').hide();
        }
    });

    $('#btnConsultar').on('click', function () {
        cargarPeriodos();
    });

    $('#btnCalcularEnergia').on('click', function () {
        calcularEnergia();
    });

    $('#btnReprocesar').on('click', function () {
        confirmReprocesar();
    });

    $('#btnCancelar').on('click', function () {
        $('#popupConfirmar').bPopup().close();
    });

    $('#btnConfirmar').on('click', function () {
        reprocesar();
    });

    $('#cbModelo').on('change', function () {
        validarModelo();
    });

});

cargarPeriodos = function () {

    if ($('#txtFecha').val() != "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'operacionesvarias',
            data: {
                fecha: $('#txtFecha').val()
            },
            success: function (evt) {
                $('#contentTabla').html(evt);

                $('#tablaPeriodo').dataTable({
                    "iDisplayLength": 50
                });

                $('#cbSelectAll').click(function (e) {
                    var table = $(e.target).closest('table');
                    $('td input:checkbox', table).prop('checked', this.checked);
                });

                $('#mensaje').removeClass();
                $('#mensaje').html('');

                $('#contentDatos').show();
                $('#contentAction').show();

                $('#cbModelo').val("2");
                validarModelo();
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
            }
        });
    }
    else {
        $('#contentTabla').html("");
        mostrarMensaje('mensaje', 'alert', 'Por favor seleccione una fecha.');
    }
};

calcularEnergia = function () {

    $('#mensaje').removeClass();
    $('#mensaje').html('');
    var horas = "";
    var newhoras = "";
    countHoras = 0;
    $('#tablaPeriodo tbody input:checked').each(function () {
        horas = horas + $(this).val() + ",";
        countHoras++;
    }); 
    if (countHoras > 0) {
        newhoras = horas.substring(0, horas.length - 1);
    }

    if (countHoras > 0) {

        if ($('#cbBarra').val() != "") {

            $.ajax({
                type: 'POST',
                url: controlador + 'calcularenergiaimportada',
                data: {
                    fecha: $('#txtFecha').val(),
                    horas: newhoras,
                    barra: $('#cbBarra').val(),
                    version: $('#cbModelo').val()
                },
                dataType: 'json',
                success: function (result) {
                    if (result.Resultado == 1 || result.Resultado == 2) {
                        cargarGrillaParametros(result.DatosTransaccion);

                        if (result.Resultado == 2) {
                            mostrarValidaciones(result.Validaciones);
                        }
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
            mostrarMensaje('mensaje', 'alert', 'Por favor seleccione una barra.');
        }
    }
    else {
        mostrarMensaje('mensaje', 'alert', 'Por favor seleccione al menos un registro de TIE');
    }
}

cargarGrillaParametros = function (result) {
    if (hot != null) {
        hot.destroy();
    }
    var container = document.getElementById('contenedorParametro');
    var data = result;

    var tituloRenderer = function (instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontWeight = 'bold';
        td.style.fontSize = '11px';
        td.style.textAlign = 'center';
        td.style.color = '#fff';
        td.style.backgroundColor = '#4C97C3';
    };

    var disbledRenderer = function (instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.backgroundColor = '#F2F2F2';
        td.style.fontSize = '11px';
        td.style.verticalAlign = 'middle';
    };

    var calculoRenderer = function (instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.backgroundColor = '#EDF5FC';
        td.style.fontWeight = 'bold';
        td.style.fontSize = '11px';
        td.style.textAlign = 'center';
    };       

    hot = new Handsontable(container, {
        data: data,
        fixedRowsTop: 0,
        width: '100%',
        colWidths: [80, 130, 90, 90, 120, 150],
        colHeaders: ['Seleccionar', 'Fecha', 'Hora', 'Id', 'Barra', 'Potencia' ],
        columns: [
            {
                data: 'Select',
                type: 'checkbox'
            },
            {
                data: 'Fecha'
            },
            {
                data: 'Hora'
            },
            {
                data: 'Codigo'
            },
            {
                data: 'Barra'
            },
            {
                data: 'Potencia'
            }
        ],
        maxRows: result.length + 1,
        cells: function (row, col, prop) {
            var cellProperties = {};

            /*if (row == 0) {
                cellProperties.renderer = tituloRenderer;
                cellProperties.readOnly = true;
            }*/

            if (col > 0 && col < 5) {
                cellProperties.renderer = disbledRenderer;
                cellProperties.readOnly = true;
            }

            if ( col == 5) {
                cellProperties.renderer = calculoRenderer;               
                cellProperties.format = '0,0.00000';
                cellProperties.type = 'numeric';
            }
           

            return cellProperties;
        }
    });
}

confirmReprocesar = function () {
    $('#popupConfirmar').bPopup({
    });
};

reprocesar = function () {
    $('#popupConfirmar').bPopup().close();
    var datos = hot.getData(0, 0, hot.countRows() - 1, hot.countCols() - 1);
    var result = [];
    
    var flag = true;
    var flagValido = true;
    var flagSeleccion = false;
    for (var i = 0; i < datos.length; i++) {        

        if (datos[i][0] == true) {
            flagSeleccion = true;

            var dato = datos[i][5];           

            if (dato == "") {
                flag = false;
            }
            else {
                if (!validarNumero(dato)) {
                    flagValido = false;
                }
            }

            result.push(datos[i]);
        }
    }

    console.log(result);
    if (flagSeleccion) {
        if (flag) {
            if (flagValido) {

                if ($('#cbBarra').val() != "") {

                    $.ajax({
                        type: 'POST',
                        url: controlador + 'reprocesarportransaccioninternacional',
                        contentType: 'application/json',
                        data: JSON.stringify({
                            fecha: $('#txtFecha').val(),
                            data: result,
                            barra: $('#cbBarra').val(),
                            version: $('#cbModelo').val()
                        }),
                        dataType: 'json',
                        success: function (result) {
                            console.log(result);
                            if (result.Resultado == 1) {
                             
                                mostrarMensaje('mensaje', 'alert', 'Se ha iniciado el reproceso, cuando finalice le llegará un correo indicando el resultado.');
                                $('#contentAction').hide();
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
                    mostrarMensaje('mensaje', 'alert', 'Por favor seleccione un barra.');
                }
            }
            else {
                mostrarMensaje('mensaje', 'alert', 'Debe ingresar valores numéricos en el campo "Potencia".');
            }
        }
        else {
            mostrarMensaje('mensaje', 'alert', 'Debe ingresar todos los valores que sean distintos de cero para el campo de "Potencia".');
        }
    }
    else {
        mostrarMensaje('mensaje', 'alert', 'Debe seleccionar al menos un periodo a reprocesar.');
    }
};

html =

mostrarValidaciones = function (list) {
    var html = "";

    html = html + "<ul>";

    for (var i in list) {
        html = html + "<li>" + list[i] + "</li>";
    }

    html = html + "</ul>";
    mostrarMensaje('mensaje', 'alert', html);

}

mostrarMensaje = function (id, tipo, mensaje) {
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);
}

getFecha = function (date) {
    var parts = date.split("/");
    var date = new Date(parts[1] + "/" + parts[0] + "/" + parts[2]);
    return date.getTime();
};

function validarNumero(texto) {
    return /^-?[\d.]+(?:e-?\d+)?$/.test(texto);
};


validarModelo = function () {
    $('#mensaje').removeClass();
    $('#mensaje').html('');

    var mensaje = validarVersionModelo();

    if (mensaje != "") {
        mostrarMensaje('mensaje', 'alert', mensaje);
    }
};

validarVersionModelo = function () {
    var mensaje = "";
    var fechaProceso = getFecha($('#txtFecha').val());
    var fechaModelo = getFecha($('#hfFechaVigenciaPR07').val());
    var version = $('#cbModelo').val();
    if (fechaProceso < fechaModelo) {
        if (version == 2) {
            mensaje = "Se está utilizando una nueva versión del modelo para una fecha anterior a su publicación.";
        }
    }
    else {
        if (version == 1) {
            mensaje = "Está utilizando una versión antigua del modelo.";
        }
    }

    return mensaje;
};