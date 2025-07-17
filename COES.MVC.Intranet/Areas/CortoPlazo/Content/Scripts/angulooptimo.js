var controlador = siteRoot + 'cortoplazo/reproceso/';
var hot = null;
$(function () {

    $('#txtFecha').Zebra_DatePicker({
        direction: false,
        onSelect: function (date) {
            $('#contentTabla').html("");
            $('#contentDatos').hide();
            validarModelo();
        }
    });

    $('#btnConsultar').on('click', function () {
        cargarPeriodos();
    });

    $('#btnCargarParametros').on('click', function () {
        cargarParametros();
    });

    $('#btnCalcular').on('click', function () {
        calcularAngulo();
    });

    $('#cbLinea').on('change', function () {
        
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

    validarModelo();

});

cargarPeriodos = function () {     

    if ($('#txtFecha').val() != "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'periodos',
            data: {
                fecha: $('#txtFecha').val(),
                version: $('#cbModelo').val()
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

                $('#contentDatos').show();
                $('#contentBoton').show();

                $('#mensaje').removeClass();
                $('#mensaje').html('');

                $('#contenedorParametro').hide();
               
              
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
            }
        });
    }
    else {
        mostrarMensaje('mensaje', 'alert', 'Por favor seleccione una fecha.');
    }
};

cargarParametros = function () {

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
        if ($('#cbLinea').val() != "") {

            $.ajax({
                type: 'POST',
                url: controlador + 'obtenerparametrosangulo',
                data: {
                    fecha: $('#txtFecha').val(),
                    linea: $('#cbLinea').val(),
                    horas: newhoras
                },
                dataType: 'json',
                success: function (result) {
                    if (result.Resultado == 1 || result.Resultado == 2) {
                        $('#contenedorParametro').show();
                        cargarGrillaParametros(result.Datos);

                        $('#mensaje').removeClass();
                        $('#mensaje').html('');

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
            mostrarMensaje('mensaje', 'alert', 'Por favor seleccione una línea de transmisión.');
        }
    }
    else {
        mostrarMensaje('mensaje', 'alert', 'Por favor seleccione al menos un periodo');
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

    var merge = [
        {
            row : 0, col : 0, rowspan : 2, colspan : 1
        },
        {
            row : 0, col : 1, rowspan : 1, colspan : 3
        },
        {
            row : 0, col : 4, rowspan : 1, colspan : 3
        },
        {
            row : 0, col : 7, rowspan : 1, colspan : 3
        },
        {
            row : 0, col : 10, rowspan : 1, colspan : 3
        }
    ];

    hot = new Handsontable(container, {
        data: data,        
        fixedRowsTop: 2,
        maxRows: result.length,       
        cells: function (row, col, prop) {
            var cellProperties = {};

            if (row == 0 || row == 1) {
                cellProperties.renderer = tituloRenderer;
                cellProperties.readOnly = true;
            }

            if (row > 1 && col < 11) {
                cellProperties.renderer = disbledRenderer;
                cellProperties.readOnly = true;
            }

            if (row > 1 && col == 12) {
                cellProperties.renderer = calculoRenderer;
                cellProperties.readOnly = true;
            }

            if (row > 1 && col == 11) {
                cellProperties.format = '0,0.00000';
                cellProperties.type = 'numeric';
            }

            return cellProperties;
        },
        mergeCells: merge
    });

}

calcularAngulo = function () {
    $('#mensaje').removeClass();
    $('#mensaje').html('');
    var datos = hot.getData(0, 0, hot.countRows() - 1, hot.countCols() - 1);

    if (datos.length > 2) {
        var flag = true;
        var flagValido = true;
        for (var i = 2; i < datos.length; i++) {
            var dato = datos[i][11];

            if (dato == "") {
                flag = false;
            }
            else {
                if (!validarNumero(dato)) {
                    flagValido = false;
                }
            }
        }

        if (flag) {
            if (flagValido) {
                $.ajax({
                    type: "POST",
                    url: controlador + 'calcularangulooptimo',
                    dataType: "json",
                    contentType: 'application/json',
                    traditional: true,
                    data: JSON.stringify({
                        data: datos
                    }),
                    success: function (result) {
                        if (result.Resultado == 1) {
                            cargarGrillaParametros(result.Datos);
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
                mostrarMensaje('mensaje', 'alert', 'Debe ingresar valores numéricos en el campo "Flujo Límite Requerido".');
            }
        }
        else {
            mostrarMensaje('mensaje', 'alert', 'Debe ingresar todos los valores de "Flujo Límite Requerido".');
        }
    }
    else {
        mostrarMensaje('mensaje', 'alert', 'No hay filas de datos.');
    }
}

confirmReprocesar = function () {

    var datos = hot.getData(0, 0, hot.countRows() - 1, hot.countCols() - 1);

    if (datos.length > 2) {
        var flag = true;
        var flagValido = true;
        for (var i = 2; i < datos.length; i++) {
            var dato = datos[i][12];

            if (dato == "") {
                flag = false;
            }
            else {
                if (!validarNumero(dato)) {
                    flagValido = false;
                }
            }
        }

        if (flag) {
            if (flagValido) {
                $('#popupConfirmar').bPopup({
                });
            }
            else {
                mostrarMensaje('mensaje', 'alert', 'Debe calcular primero los ángulos óptimos.');
            }
        }
        else {
            mostrarMensaje('mensaje', 'alert', 'Debe calcular primero los ángulos óptimos.');
        }
    }
    else {
        mostrarMensaje('mensaje', 'alert', 'No hay filas de datos.');
    }


};

reprocesar = function () {
    $('#popupConfirmar').bPopup().close();
    var datos = hot.getData(0, 0, hot.countRows() - 1, hot.countCols() - 1);
    $.ajax({
        type: 'POST',
        url: controlador + 'reprocesarporangulooptimo',
        contentType: 'application/json',
        
        data: JSON.stringify({
            fecha: $('#txtFecha').val(),
            linea: $('#cbLinea').val(),
            data: datos,
            version: $('#cbModelo').val()
        }),        
        dataType: 'json',
        success: function (result) {
            if (result.Resultado == 1) {
                mostrarMensaje('mensaje', 'exito', 'La operación se realizó correctamente.');
                $('#contentBoton').hide();
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

mostrarValidaciones = function (list) {
    var html = "";

    html = html + "<ul>";

    for (var i in list) {
        html = html + "<li>" + list[i] + "</li>";
    }

    html = html + "</ul>";
    mostrarMensaje('mensaje', 'alert', html);

};

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