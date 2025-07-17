var controlador = siteRoot + 'cortoplazo/forzada/';

$(function () {

    $('#tab-container').easytabs({
        animate: false
    });
    
    $('#txtFechaHidroDesde').Zebra_DatePicker({
        pair: $('#txtFechaHidroHasta'),
        onSelect: function (date) {            
            var date1 = getFecha(date);
            var date2 = getFecha($('#txtFechaHidroHasta').val());
            if (date1 > date2) {
                $('#txtFechaHidroHasta').val(date);
            }
        }
    });

    $('#txtFechaHidroHasta').Zebra_DatePicker({
        direction: true        
    });        
    
    $('#btnConsultarHidro').on('click', function () {
        consultarHidraulico();
    });

    $('#btnNuevoHidro').on('click', function () {
        editarHidraulico(0);
    });

    $('#btnNuevoMaestro').on('click', function () {
        editarMaestro(0);
    });

    consultarHidraulico();
    consultarMaestro();
});

/**==== Funciones para Gen forzada CH ====*/

consultarHidraulico = function () {

    var fechaInicio = $('#txtFechaHidroDesde').val();
    var fechaFin = $('#txtFechaHidroHasta').val();

    if (fechaInicio != "" && fechaFin != "") {
        var nroDias = getNroDias(getFecha(fechaFin), getFecha(fechaInicio));

        if (nroDias <= 7) {
            $.ajax({
                type: 'POST',
                url: controlador + 'hidraulicolist',
                data: {
                    fechaInicio: fechaInicio,
                    fechaFin: fechaFin
                },
                success: function (evt) {
                    $('#listadoHidro').html(evt);
                    $('#tablaHidraulico').dataTable({
                    });
                },
                error: function () {
                    mostrarMensaje('mensajeHidroList', 'error', 'Se produjo un error.');
                }
            });
        }
        else {
            mostrarMensaje('mensajeHidroList', 'alert', 'Debe seleccionar como máximo 7 días.');
        }
    }
    else {
        mostrarMensaje('mensajeHidroList', 'alert', 'Por favor seleccione el rango de fechas.');
    }
}

editarHidraulico = function (id) {
        
    $('#mensajeListHidraulico').removeClass();
    $('#mensajeListHidraulico').text("");

    $.ajax({
        type: 'POST',
        url: controlador + 'hidraulicoedit',
        data: {
            idHidraulico: id
        },
        global: false,
        success: function (evt) {
            $('#contenidoHidraulico').html(evt);
            setTimeout(function () {
                $('#popupHidraulico').bPopup({
                    autoClose: false
                });
            }, 50);

            $('#cbGeneradorHidraulico').val($('#hfGeneradorHidraulico').val());
            $('#cbSimboloHidraulico').val($('#hfSimboloHidraulico').val());
            $('#cbSubcausaevento').val($('#hfSubcausaevento').val());

            $('#txtInicioEditHidraulico').Zebra_DatePicker({
                readonly_element: false,
                pair: $('#txtFinalEditHidraulico'),
                onSelect: function (date) {
                    var date1 = getFecha(date);
                    var date2 = getFecha($('#txtFinalEditHidraulico').val());
                    if (date1 > date2) {
                        $('#txtFinalEditHidraulico').val(date);
                    }
                    $('#txtInicioEditHidraulico').val(date + " 00:00:00");
                }
            });

            $('#txtFinalEditHidraulico').Zebra_DatePicker({
                readonly_element: false,
                direction: true,
                onSelect: function (date) {
                    $('#txtFinalEditHidraulico').val(date + " 00:00:00");
                }
            });

            $('#txtInicioEditHidraulico').inputmask({
                mask: "1/2/y h:s:s",
                placeholder: "dd/mm/yyyy hh:mm:ss",
                alias: "datetime",
                hourFormat: "24"
            });
           
            $('#txtFinalEditHidraulico').inputmask({
                mask: "1/2/y h:s:s",
                placeholder: "dd/mm/yyyy hh:mm:ss",
                alias: "datetime",
                hourFormat: "24"
            });

            $('#btnGrabarHidraulico').on("click", function () {
                grabarHidraulico();
            });

            $('#btnCancelarHidraulico').on("click", function () {
                $('#popupHidraulico').bPopup().close();
            });

        },
        error: function () {
            mostrarMensaje('mensajeListMaestro', 'error', 'Se ha producido un error.');
        }
    });
}

eliminarHidraulico = function (id) {
    if (confirm('¿Está seguro de realizar esta operación?')) {
        $.ajax({
            type: 'POST',
            url: controlador + 'hidraulicodelete',
            data: {
                idHidraulico: id
            },
            dataType: 'json',
            success: function (result) {
                if (result == 1) {
                    mostrarMensaje('mensajeListHidraulico', 'exito', 'La operación se realizó correctamente.');
                    consultarHidraulico();
                }
                else {
                    mostrarMensaje('mensajeListHidraulico', 'error', 'Se ha producido un error.');
                }
            },
            error: function () {
                mostrarMensaje('mensajeListHidraulico', 'error', 'Se ha producido un error.');
            }
        });
    }
}

grabarHidraulico = function () {

    var validacion = validarHidraulico();

    if (validacion == "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'hidraulicosave',
            data: $('#frmRegistroHidraulico').serialize(),
            dataType: 'json',
            global: false,
            success: function (result) {
                if (result == 1) {
                    mostrarMensaje('mensajeHidroList', 'exito', 'Los datos se grabaron correctamente.');
                    $('#popupHidraulico').bPopup().close();
                    consultarHidraulico();
                }
                else if (result == 2) {
                    mostrarMensaje('mensajeEditHidraulico', 'alert', 'La fecha final debe ser mayor o igual a la fecha inicial.');
                }
                else {
                    mostrarMensaje('mensajeEditHidraulico', 'error', 'Se ha producido un error.');
                }
            },
            error: function () {
                mostrarMensaje('mensajeEditHidraulico', 'error', 'Se ha producido un error.');
            }
        });
    }
    else {
        mostrarMensaje('mensajeEditHidraulico', 'alert', validacion);
    }
}

validarHidraulico = function () {
    var mensaje = "<ul>";
    var flag = true;

    if ($('#cbGeneradorHidraulico').val() == '-1') {
        mensaje = mensaje + "<li>Por favor seleccione el generador asociado.</li>";
        flag = false;
    }

    if ($('#cbSimboloHidraulico').val() == '') {
        mensaje = mensaje + "<li>Por favor seleccione el símbolo a utilizar.</li>";
        flag = false;
    }

    if ($('#txtInicioEditHidraulico').val() == '') {
        mensaje = mensaje + "<li>Ingrese la fecha inicial.</li>";
        flag = false;
    }
    else {
        if (!$('#txtInicioEditHidraulico').inputmask("isComplete")) {
            mensaje = mensaje + "<li>Ingrese hora inicial.</li>";
            flag = false;
        }
    }

    if ($('#txtFinalEditHidraulico').val() == '') {
        mensaje = mensaje + "<li>Ingrese la fecha final.</li>";
        flag = false;
    }
    else {
        if (!$('#txtFinalEditHidraulico').inputmask("isComplete")) {
            mensaje = mensaje + "<li>Ingrese hora inicial.</li>";
            flag = false;
        }
    }

    if ($('#cbSubcausaevento').val() == '') {
        mensaje = mensaje + "<li>Por favor seleccione la calificación.</li>";
        flag = false;
    }

    mensaje = mensaje + "</ul>";

    if (flag) {
        mensaje = "";
    }

    return mensaje;
}

/**==== Funciones para la tabla maestro ====*/

consultarMaestro = function () {
    $.ajax({
        type: 'POST',
        url: controlador + 'maestrolist',
        success: function (evt) {
            $('#listadoMaestro').html(evt);
            $('#tablaMaestro').dataTable({
            });
        },
        error: function () {
            mostrarMensaje('mensajeListMaestro', 'error', 'Se produjo un error.');
        }
    });
}

editarMaestro = function (id) {
    $('#mensajeListMaestro').removeClass();
    $('#mensajeListMaestro').text("");

    $.ajax({
        type: 'POST',
        url: controlador + 'maestroedit',
        data: {
            idMaestro: id
        },
        global: false,
        success: function (evt) {
            $('#contenidoMaestro').html(evt);
            setTimeout(function () {
                $('#popupMaestro').bPopup({
                    autoClose: false
                });
            }, 50);

            $('#cbGeneradorMaestro').val($('#hfGeneradorMaestro').val());
            $('#cbSimboloMaestro').val($('#hfSimboloMaestro').val());
            $('#cbTipoMaestro').val($('#hfTipoMaestro').val());
            $('#cbEstadoMaestro').val($('#hfEstadoMaestro').val());
            $('#cbSubcausaevento').val($('#hfSubcausaevento').val());

            $('#btnGrabarMaestro').on("click", function () {
                grabarMaestro();
            });

            $('#btnCancelarMaestro').on("click", function () {
                $('#popupMaestro').bPopup().close();
            });

        },
        error: function () {
            mostrarMensaje('mensajeListMaestro', 'error', 'Se ha producido un error.');
        }
    });
}

eliminarMaestro = function (id) {
    if (confirm('¿Está seguro de realizar esta operación?')) {
        $.ajax({
            type: 'POST',
            url: controlador + 'maestrodelete',
            data: {
                idMaestro: id
            },
            dataType: 'json',
            success: function (result) {
                if (result == 1) {
                    mostrarMensaje('mensajeListMaestro', 'exito', 'La operación se realizó correctamente.');
                    consultarMaestro();
                }
                else {
                    mostrarMensaje('mensajeListMaestro', 'error', 'Se ha producido un error.');
                }
            },
            error: function () {
                mostrarMensaje('mensajeListMaestro', 'error', 'Se ha producido un error.');
            }
        });
    }
}

grabarMaestro = function () {
    var validacion = validarMaestro();

    if (validacion == "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'maestrosave',
            data: $('#frmRegistroMaestro').serialize(),
            dataType: 'json',
            global: false,
            success: function (result) {
                if (result == 1) {
                    mostrarMensaje('mensajeListMaestro', 'exito', 'Los datos se grabaron correctamente.');
                    $('#popupMaestro').bPopup().close();
                    consultarMaestro();
                }
                else if (result == 2) {
                    mostrarMensaje('mensajeEditMaestro', 'alert', 'Ya existe un registro con el mismo generador.');
                }
                else {
                    mostrarMensaje('mensajeEditMaestro', 'error', 'Se ha producido un error.');
                }
            },
            error: function () {
                mostrarMensaje('mensajeEditMaestro', 'error', 'Se ha producido un error.');
            }
        });
    }
    else {
        mostrarMensaje('mensajeEditMaestro', 'alert', validacion);
    }
}

validarMaestro = function () {
    var mensaje = "<ul>";
    var flag = true;

    if ($('#cbGeneradorMaestro').val() == '-1') {
        mensaje = mensaje + "<li>Por favor seleccione el generador asociado.</li>";
        flag = false;
    }

    if ($('#cbSimboloMaestro').val() == '') {
        mensaje = mensaje + "<li>Por favor seleccione el símbolo a utilizar.</li>";
        flag = false;
    }

    if ($('#cbTipoMaestro').val() == '') {
        mensaje = mensaje + "<li>Por favor seleccione un tipo.</li>";
        flag = false;
    }

    if ($('#cbEstadoMaestro').val() == '') {
        mensaje = mensaje + "<li>Por favor seleccione el estado.</li>";
        flag = false;
    }

    if ($('#cbSubcausaevento').val() == '') {
        mensaje = mensaje + "<li>Por favor seleccione la calificación.</li>";
        flag = false;
    }

    mensaje = mensaje + "</ul>";

    if (flag) {
        mensaje = "";
    }

    return mensaje;
}


/**===== Funciones Generales =====**/

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