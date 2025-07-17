var controlador = siteRoot + 'web/calendario/';

$(function () {

    $('#btnConsultar').on('click', function () {
        consultar();
    });

    $('#txtFechaInicio').Zebra_DatePicker({
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
        direction: true
    });
    
    $('#btnNuevo').on('click', function () {
        editar(0);
    });

    $('#btnExportar').on('click', function () {
        exportar();
    });

    $('#btnCalendario').on('click', function () {
        openCalendario();
    });

    $('#btnMeses').on('click', function () {
        document.location.href = controlador + 'infoindex';
    });

    $('#btnTiposEvento').on('click', function () {
        document.location.href = controlador + 'tipoindex';
    });

    consultar();
});

exportar = function () {
    $.ajax({
        type: 'POST',
        url: controlador + "exportar",
        data: {
            fechaInicio: $('#txtFechaInicio').val(),
            fechaFin: $('#txtFechaFin').val(),
            idPublicacion: $('#cbPublicacionFiltro').val()
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

consultar = function () {
    $.ajax({
        type: 'POST',
        url: controlador + 'listado',
        data: {
            nombre: $('#txtNombre').val(),
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
            idEvento: id
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


            //$('#txtTimeInicial').inputmask({
            //    mask: "h:s",
            //    placeholder: "hh:mm",
            //    alias: "datetime",
            //    hourFormat: "24"
            //});


            //$('#txtTimeFinal').inputmask({
            //    mask: "h:s",
            //    placeholder: "hh:mm",
            //    alias: "datetime",
            //    hourFormat: "24"
            //});

            $('#txtHoraInicial').Zebra_DatePicker({
                readonly_element: false,
                //onSelect: function (date) {
                //    $('#txtHoraInicial').val(date + " 00:00");
                //}
            });

            //$('#txtHoraFinal').Zebra_DatePicker({
            //    readonly_element: false,
            //    //onSelect: function (date) {
            //    //    $('#txtHoraFinal').val(date + " 00:00");
            //    //}
            //});

            $('#cbEstado').val($('#hfEstado').val());
            //$("input:radio[name='Icono'][value='" + $('#hfIcono').val() + "']").prop('checked', true);            
            //$("input:radio[name='Color'][value='" + $('#hfColor').val() + "']").prop('checked', true);
            $('#cbTipoEvento').val($('#hfTipoEvento').val());

        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        }
    });
}

eliminar = function (id) {
    if (confirm('¿Está seguro de realizar esta operación?')) {

        $.ajax({
            type: 'POST',
            url: controlador + 'eliminar',
            data: {
                idEvento: id
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

validarRegistro = function () {

    var mensaje = "<ul>";
    var flag = true;
    
    if ($('#txtTitulo').val() == "") {
        mensaje = mensaje + "<li>Ingrese el título del Evento.</li>";
        flag = false;
    }

    if ($('#txtHoraInicial').val() == "") {
        mensaje = mensaje + "<li>Ingrese la fecha del Evento.</li>";
        flag = false;
    }

    //if ($('#txtHoraFinal').val() == "") {
    //    mensaje = mensaje + "<li>Ingrese la fecha final del Evento.</li>";
    //    flag = false;
    //}

    //if ($('#txtTimeInicial').val() != "") {
    //    if (!validarHora($('#txtTimeInicial').val())) {
    //        mensaje = mensaje + "<li>Ingrese una hora inicial válida.</li>";
    //    }
    //}

    //if ($('#txtTimeFinal').val() != "") {
    //    if (!validarHora($('#txtTimeFinal').val())) {
    //        mensaje = mensaje + "<li>Ingrese una final inicial válida.</li>";
    //    }
    //}
    
    if ($('#cbTipoEvento').val() == ""){
        mensaje = mensaje + "<li>Seleccione el tipo de evento.</li>";
        flag = false;
    }

    if ($('#cbEstado').val() == "") {
        mensaje = mensaje + "<li>Seleccione el estado.</li>";
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

openCalendario = function () {
    $.ajax({
        type: 'POST',
        url: controlador + 'calendario',
        success: function (evt) {
            $('#contenidoCalendario').html(evt);
            setTimeout(function () {
                $('#popupCalendario').bPopup({
                    autoClose: false
                });
                $("#calendar").fullCalendar('render');
            }, 100);           
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        }
    });
        
}


validarHora = function (inputStr) {

    if (!inputStr || inputStr.length < 1) { return false; }
    var time = inputStr.split(':');
    return (time.length === 2
           && parseInt(time[0], 10) >= 0
           && parseInt(time[0], 10) <= 23
           && parseInt(time[1], 10) >= 0
           && parseInt(time[1], 10) <= 59) ||
           (time.length === 3
           && parseInt(time[0], 10) >= 0
           && parseInt(time[0], 10) <= 23
           && parseInt(time[1], 10) >= 0
           && parseInt(time[1], 10) <= 59
           && parseInt(time[2], 10) >= 0
           && parseInt(time[2], 10) <= 59)
}