var controlador = siteRoot + 'cortoplazo/congestion/';

/*==== Manejo de Eventos ====*/

$(function () {

    $('#tab-container').easytabs({
        animate: false
    });

    $('#txtFechaEjecutadoDesde').Zebra_DatePicker({
        pair: $('#txtFechaEjecutadoHasta'),
        onSelect: function (date) {
            var date1 = getFecha(date);
            var date2 = getFecha($('#txtFechaEjecutadoHasta').val());
            if (date1 > date2) {
                $('#txtFechaEjecutadoHasta').val(date);
            }
        }
    });

    $('#txtFechaEjecutadoHasta').Zebra_DatePicker({
        direction: true
    });

    $('#btnConsultarEjecutado').on('click', function () {
        consultarEjecutado();
    });

    $('#btnNuevoEjecutado').on('click', function () {
        editarEjecutado(0);
    });

    consultarEjecutado();
});

/*==== Funciones de congestion ejecutada ====*/

consultarEjecutado = function () {
    var fechaInicio = $('#txtFechaEjecutadoDesde').val();
    var fechaFin = $('#txtFechaEjecutadoHasta').val();

    if (fechaInicio != "" && fechaFin != "") {
        var nroDias = getNroDias(getFecha(fechaFin), getFecha(fechaInicio));

        if (nroDias <= 7) {
            $.ajax({
                type: 'POST',
                url: controlador + 'ejecutadolist',
                data: {
                    fechaInicio: fechaInicio,
                    fechaFin: fechaFin
                },
                success: function (evt) {
                    $('#listadoEjecutado').html(evt);
                    $('#tablaEjecutadoSimple').dataTable({
                    });
                    $('#tablaEjecutadoConjunto').dataTable({
                    });
                },
                error: function () {
                    mostrarMensaje('mensajeEjecutadoList', 'error', 'Se produjo un error.');
                }
            });
        }
        else {
            mostrarMensaje('mensajeEjecutadoList', 'alert', 'Debe seleccionar como máximo 7 días.');
        }
    }
    else {
        mostrarMensaje('mensajeEjecutadoList', 'alert', 'Por favor seleccione el rango de fechas.');
    }
}

editarEjecutado = function (id) {

    $('#mensajeEjecutadoList').removeClass();
    $('#mensajeEjecutadoList').text("");

    $.ajax({
        type: 'POST',
        url: controlador + 'ejecutadoedit',
        data: {
            idCongestion: id
        },
        global: false,
        success: function (evt) {
            $('#contenidoEjecutado').html(evt);
            setTimeout(function () {
                $('#popupEjecutado').bPopup({
                    autoClose: false
                });
            }, 50);

            $('#txtInicioEditEjecutado').Zebra_DatePicker({
                readonly_element: false,
                pair: $('#txtFinEditEjecutado'),
                onSelect: function (date) {
                    var date1 = getFecha(date);
                    var date2 = getFecha($('#txtFinEditEjecutado').val());
                    if (date1 > date2) {
                        $('#txtFinEditEjecutado').val(date);
                    }
                    $('#txtInicioEditEjecutado').val(date + " 00:00:00");
                }
            });

            $('#txtFinEditEjecutado').Zebra_DatePicker({
                readonly_element: false,
                direction: true,
                onSelect: function (date) {
                    $('#txtFinEditEjecutado').val(date + " 00:00:00");
                }
            });

            $('#txtInicioEditEjecutado').inputmask({
                mask: "1/2/y h:s:s",
                placeholder: "dd/mm/yyyy hh:mm:ss",
                alias: "datetime",
                hourFormat: "24"
            });

            $('#txtFinEditEjecutado').inputmask({
                mask: "1/2/y h:s:s",
                placeholder: "dd/mm/yyyy hh:mm:ss",
                alias: "datetime",
                hourFormat: "24"
            });

            $('#btnGrabarEjecutado').on("click", function () {
                grabarEjecutado();
            });

            $('#btnCancelarEjecutado').on("click", function () {
                $('#popupEjecutado').bPopup().close();
            });

            $('#cbLineaEjecutado').on("change", function () {
                cargarBarras();
            });

            $('#cbTipoEjecutado').on("change", function(){
                cargarConfiguracion();
            });

            $('#cbTipoEjecutado').val($('#hfTipoEjecutado').val());
            $('#cbLineaEjecutado').val($('#hfLineaEjecutado').val());
            //$('#cbBarra1').val($('#hfBarra1').val());
            //$('#cbBarra2').val($('#hfBarra2').val());
            $('#cbConjuntoLinea').val($('#hfConjuntoLinea').val());

            $('#trLinea').hide();
            $('#trBarra1').hide();
            $('#trBarra2').hide();
            $('#trConjunto').hide();

            if ($('#hfTipoEjecutado').val() == "S") {
                $('#trLinea').show();
                //$('#trBarra1').show();
                //$('#trBarra2').show();
            }
            else {
                $('#trConjunto').show();
            }
        },
        error: function () {
            mostrarMensaje('mensajeEjecutadoList', 'error', 'Se ha producido un error.');
        }
    });
}

cargarConfiguracion = function () {
    $('#trLinea').hide();
    //$('#trBarra1').hide();
    //$('#trBarra2').hide();
    $('#trConjunto').hide();

    if ($('#cbTipoEjecutado').val() == "S") {
        $('#trLinea').show();
       // $('#trBarra1').show();
       // $('#trBarra2').show();
    }
    else {
        $('#trConjunto').show();
    }
}

cargarBarras = function () {

    $('option', '#cbBarra1').remove();
    $('option', '#cbBarra2').remove();

    if ($('#cbLineaEjecutado').val() != "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'cargarbarras',
            dataType: 'json',
            data: {
                idLinea: $('#cbLineaEjecutado').val()
            },
            cache: false,
            global: false,
            success: function (result) {
                $('#cbBarra1').get(0).options.length = 0;
                $('#cbBarra2').get(0).options.length = 0;

                for (var i in result.ListaBarra1) {
                    $('#cbBarra1').get(0).options[$('#cbBarra1').get(0).options.length] = new Option(result.ListaBarra1[i], result.ListaBarra1[i]);
                }

                for (var i in result.ListaBarra2) {
                    $('#cbBarra2').get(0).options[$('#cbBarra2').get(0).options.length] = new Option(result.ListaBarra2[i], result.ListaBarra2[i]);
                }
            },
            error: function () {
                mostrarMensaje('mensajeEditEjecutado', 'error', 'Se ha producido un error.');
            }
        });
    }
}

eliminarEjecutado = function (id) {
    if (confirm('¿Está seguro de realizar esta operación?')) {
        $.ajax({
            type: 'POST',
            url: controlador + 'ejecutadodelete',
            data: {
                idCongestion: id
            },
            dataType: 'json',
            success: function (result) {
                if (result == 1) {
                    mostrarMensaje('mensajeEjecutadoList', 'exito', 'La operación se realizó correctamente.');
                    consultarEjecutado();
                }
                else {
                    mostrarMensaje('mensajeEjecutadoList', 'error', 'Se ha producido un error.');
                }
            },
            error: function () {
                mostrarMensaje('mensajeEjecutadoList', 'error', 'Se ha producido un error.');
            }
        });
    }
}

grabarEjecutado = function () {
    var validacion = validarEjecutado();

    if (validacion == "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'ejecutadosave',
            data: $('#frmRegistroEjecutado').serialize(),
            dataType: 'json',
            global: false,
            success: function (result) {
                if (result == 1) {
                    mostrarMensaje('mensajeEjecutadoList', 'exito', 'Los datos se grabaron correctamente.');
                    $('#popupEjecutado').bPopup().close();
                    consultarEjecutado();
                }
                else if (result == 2) {
                    mostrarMensaje('mensajeEditEjecutado', 'alert', 'La fecha final debe ser mayor o igual a la fecha inicial.');
                }
                else {
                    mostrarMensaje('mensajeEditEjecutado', 'error', 'Se ha producido un error.');
                }
            },
            error: function () {
                mostrarMensaje('mensajeEditEjecutado', 'error', 'Se ha producido un error.');
            }
        });
    }
    else {
        mostrarMensaje('mensajeEditEjecutado', 'alert', validacion);
    }
}

validarEjecutado = function () {
    var mensaje = "<ul>";
    var flag = true;

    var tipo = $('#cbTipoEjecutado').val();

    if (tipo == "S") {
        if ($('#cbLineaEjecutado').val() == "") {
            mensaje = mensaje + "<li>Seleccione la línea.</li>";
            flag = false;
        }
        /*if ($('#cbBarra1').val() == "") {
            mensaje = mensaje + "<li>Seleccione la barra 1.</li>";
            flag = false;
        }
        if ($('#cbBarra2').val() == "") {
            mensaje = mensaje + "<li>Seleccione la barra 2.</li>";
            flag = false;
        }*/
    }
    else if (tipo == "C") {
        if ($('#cbConjuntoLinea').val() == "") {
            mensaje = mensaje + "<li>Seleccione el conjunto de líneas.</li>";
            flag = false;
        }
    }


    if ($('#txtInicioEditEjecutado').val() == '') {
        mensaje = mensaje + "<li>Ingrese la fecha inicial.</li>";
        flag = false;
    }
    else {
        if (!$('#txtInicioEditEjecutado').inputmask("isComplete")) {
            mensaje = mensaje + "<li>Ingrese fecha inicial.</li>";
            flag = false;
        }
    }

    if ($('#txtFinEditEjecutado').val() == '') {
        mensaje = mensaje + "<li>Ingrese fecha final.</li>";
        flag = false;
    }
    else {
        if (!$('#txtFinEditEjecutado').inputmask("isComplete")) {
            mensaje = mensaje + "<li>Ingrese fecha final.</li>";
            flag = false;
        }
    }

    mensaje = mensaje + "</ul>";
    
    if (flag)
        mensaje = "";

    return mensaje;
}

/*==== Funciones Generales =====*/

mostrarMensaje = function (id, tipo, mensaje) {
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);
}

validarNumero = function (item, evt) {
    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;

    if (charCode == 46) {
        var regex = new RegExp(/\./g)
        var count = $(item).val().match(regex).length;
        if (count > 1) {
            return false;
        }
    }

    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }

    return true;
}

validarDecimal = function (n) {
    if (n == "")
        return false;

    var count = 0;
    var strCheck = "0123456789.";
    var i;

    for (i in n) {
        if (strCheck.indexOf(n[i]) == -1)
            return false;
        else {
            if (n[i] == '.') {
                count = count + 1;
            }
        }
    }
    if (count > 1) return false;
    return true;
}

getFecha = function (date) {
    var parts = date.split("/");
    var date = new Date(parts[1] + "/" + parts[0] + "/" + parts[2]);
    return date.getTime();
}

getNroDias = function (fechaInicio, fechaFin) {
    return Math.round(Math.abs((fechaFin - fechaInicio) / (24 * 60 * 60 * 1000)));
}