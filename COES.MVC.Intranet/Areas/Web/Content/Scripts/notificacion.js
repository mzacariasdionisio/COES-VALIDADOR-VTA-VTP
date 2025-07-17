var controlador = siteRoot + 'web/notificacion/';
$(function () {

    //$('#txtFechaInicio').Zebra_DatePicker({
    //    pair: $('#txtFechaFin'),
    //    onSelect: function (date) {
    //        var date1 = getFecha(date);
    //        var date2 = getFecha($('#txtFechaFin').val());
    //        if (date1 > date2) {
    //            $('#txtFechaFin').val(date);
    //        }
    //    }
    //});

    //$('#txtFechaFin').Zebra_DatePicker({
    //    direction: true
    //});
    //$('#txtFechaInicio').inputmask({
    //    mask: "1/2/y h:s",
    //    placeholder: "dd/mm/yyyy 00:00",
    //    alias: "datetime",
    //    hourFormat: "24"
    //});

    $('#txtFechaInicio').Zebra_DatePicker({
        readonly_element: false,
        onSelect: function (date) {
            $('#txtHoraInicial').val(date);
        }
    });
    
    $('#txtFechaFin').Zebra_DatePicker({
        readonly_element: false,
    });

    $('#btnNuevo').on('click', function () {
        editar(0);
    });
    $('#btnConsultar').on('click', function () {
        consultar();
    });

    //$('#txtHoraFinal').inputmask({
    //    mask: "1/2/y h:s:s",
    //    placeholder: "dd/mm/yyyy hh:mm",
    //    alias: "datetime",
    //    hourFormat: "24"
    //});

    //$('#txtFechaFin').inputmask({
    //    mask: "1/2/y h:s:s",
    //    placeholder: "dd/mm/yyyy hh:mm",
    //    alias: "datetime",
    //    hourFormat: "24"
    //});

    consultar();
})


consultar = function () {
    $.ajax({
        type: 'POST',
        url: controlador + 'listado',
        data: {
            titulo: $('#txtTitulo').val(),
           // descripcion: $('#txtDescipcion').val(),
            fechaInicio: $('#txtFechaInicio').val(),
            fechaFin: $('#txtFechaFin').val()
        },
        success: function (evt) {
            $('#listado').html(evt);
            $('#tabla').dataTable({
            });
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
}

editar = function (id) {

    $.ajax({
        type: 'POST',
        url: controlador + 'editar',
        data: {
            notiCodi: id
        },
        global: false,
        success: function (evt) {
            $('#contenidoEdicion').html(evt);
            setTimeout(function () {
                $('#popupEdicion').bPopup({
                    autoClose: false
                });
            }, 50);

            $('#btnGrabar').on("click", function () {
                grabar();
            });

            $('#btnCancelar').on("click", function () {
                $('#popupEdicion').bPopup().close();
            });

            //$('#NotiEjecucion').Zebra_DatePicker({
            //    readonly_element: false,
            //});
            $('#FechaEjecucion').Zebra_DatePicker({
                readonly_element: false,
                onSelect: function (date) {
                    $('#txtHoraFinal').val(date + " 00:00");
                }
            });
            $('#FechaEjecucion').inputmask({
                mask: "1/2/y h:s",
                placeholder: "dd/mm/yyyy 00:00",
                alias: "datetime",
                hourFormat: "24"
            });

            //$('#cbEstado').val($('#hfEstado').val());

            //$('#cbTipoEvento').val($('#hfTipoEvento').val());

        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        }
    });
}

grabar = function () {
    var validacion = validarRegistro();

    if (validacion == "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'grabar',
            data: $('#frmRegistro').serialize(),
            dataType: 'json',
            global: false,
            success: function (result) {
                if (result > 0) {
                    mostrarMensaje('mensaje', 'exito', 'Los datos se grabaron correctamente.');
                    $('#popupEdicion').bPopup().close();
                    consultar();
                }
                else if (result == -2) {
                    mostrarMensaje('mensajeEdicion', 'alert', 'La fecha final debe ser mayor a la inicial.');
                }
                else {
                    mostrarMensaje('mensajeEdicion', 'error', 'Se ha producido un error.');
                }
            },
            error: function () {
                mostrarMensaje('mensajeEdicion', 'error', 'Se ha producido un error.');
            }
        });
    }
    else {
        mostrarMensaje('mensajeEdicion', 'alert', validacion);
    }
}
eliminar = function (id) {
    if (confirm('¿Está seguro de realizar esta operación?')) {

        $.ajax({
            type: 'POST',
            url: controlador + 'eliminar',
            data: {
                notiCodi: id
            },
            dataType: 'json',
            success: function (result) {
                if (result == 1) {
                    mostrarMensaje('mensaje', 'exito', 'El registro se eliminó correctamente.');
                    consultar();
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
}


validarRegistro = function () {

    var mensaje = "<ul>";
    var flag = true;

    if ($('#NotiTitulo').val() == "") {
        mensaje = mensaje + "<li>Ingrese el título.</li>";
        flag = false;
    }

    if ($('#NotiDescripcion').val() == "") {
        mensaje = mensaje + "<li>Ingrese la descripción.</li>";
        flag = false;
    }

    if ($('#FechaEjecucion').val() == "") {
        mensaje = mensaje + "<li>Ingrese la fecha del Evento.</li>";
        flag = false;
    }


    mensaje = mensaje + "</ul>";

    if (flag) mensaje = "";

    return mensaje;
}

mostrarMensaje = function (id, tipo, mensaje) {
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);
}