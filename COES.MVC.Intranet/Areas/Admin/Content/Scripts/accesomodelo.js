var controlador = siteRoot + 'admin/accesomodelo/';

$(function () {

    $('#btnAgregar').on('click', function () {
        if ($('#cbEmpresa').val() != "") {
            editar(0);
        }
        else {
            mostrarMensaje('mensaje', 'alert', 'Seleccione empresa.');
        }
    });

    $('#tab-container').easytabs({
        animate: false
    });

    $('#cbEmpresa').on('change', function () {
        listarAccesos();
        listarCorreos();
    });

    $('#btnConsultar').on('click', function () {
        listarAccesos();
        listarCorreos();
    });
    //listarCorreos();
});


listarAccesos = function () {

    if ($('#cbEmpresa').val() != "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'listaacceso',
            data: {
                idEmpresa: $('#cbEmpresa').val(),
                idModulo: $('#cbModulo').val()
            },
            success: function (evt) {

                $('#divAccesos').html(evt);
                oTable = $('#tablaAcceso').dataTable({
                    "aaSorting": [[0, "asc"]],
                    "destroy": "true",
                    "sDom": 'fti',
                    "iDisplayLength": 400
                });

                mostrarMensaje('mensaje', '', '');
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
            }
        });
    }
    else {
        mostrarMensaje('mensaje', 'alert', 'Seleccione empresa.');
    }
};

listarCorreos = function () {

    if ($('#cbEmpresa').val() != "") {
    $.ajax({
        type: 'POST',
        url: controlador + 'correos',
        data: {
            idEmpresa: $('#cbEmpresa').val(),
            idModulo: $('#cbModulo').val()
        },
        success: function (evt) {

            $('#divCorreos').html(evt);
            oTable = $('#tablaCuenta').dataTable({
                "aaSorting": [[0, "asc"]],
                "destroy": "true",
                "sDom": 'fti',
                "iDisplayLength": 400
            });

        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
        });
    }
    else {
        mostrarMensaje('mensaje', 'alert', 'Seleccione empresa.');
    }
};


editar = function (idCorreo) {

    $.ajax({
        type: 'POST',
        url: controlador + 'detallecorreo',
        data: {
            idCorreo: idCorreo,
            idEmpresa: $('#cbEmpresa').val(),
            idModulo: $('#cbModulo').val()
        },
        global: false,
        success: function (evt) {
            $('#contenidoEdicion').html(evt);
            setTimeout(function () {
                $('#popupEdicion').bPopup({
                    autoClose: false
                });
            }, 50);

            $('#cbEstadoCuenta').val($('#hfEstadoCuenta').val());

            $('#cbIncluirNotificacion').val($('#hfIncluirNotificacion').val());

            $('#btnGrabarCuenta').on("click", function () {
                grabar();
            });

            $('#btnCancelarCuenta').on("click", function () {
                $('#popupEdicion').bPopup().close();
            });

        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        }
    });
};


eliminarCuenta = function (idCuenta) {
    if (confirm('¿Está seguro de realizar esta acción?')) {
        $.ajax({
            type: 'POST',
            url: controlador + 'eliminarcorreo',
            data: {
                idEmpresaCorreo: idCuenta
            },
            global: false,
            dataType: 'json',
            success: function (result) {
                if (result == 1) {
                    listarCorreos();
                    listarAccesos();
                    mostrarMensaje('mensaje', 'exito', 'Operación realizada correctamente');
                }
                else {
                    mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
                }
            },
            error: function () {

            }
        });
    }
};

eliminarAcceso = function (id) {
    if (confirm('¿Está seguro de realizar esta acción?')) {
        $.ajax({
            type: 'POST',
            url: controlador + 'eliminaracceso',
            data: {
                id: id
            },
            global: false,
            dataType: 'json',
            success: function (result) {
                if (result == 1) {
                    listarAccesos();
                    mostrarMensaje('mensaje', 'exito', 'Operación realizada correctamente');
                }
                else {
                    mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
                }
            },
            error: function () {

            }
        });
    }
};

crearCredencial = function (idEmpresaCorreo) {
    $.ajax({
        type: 'POST',
        url: controlador + 'acceso',
        data: {
            idEmpresaCorreo: idEmpresaCorreo,
            idEmpresa: $('#cbEmpresa').val(),
            idModulo: $('#cbModulo').val()
        },
        global: false,
        success: function (evt) {
            $('#contenidoEdicion').html(evt);
            setTimeout(function () {
                $('#popupEdicion').bPopup({
                    autoClose: false
                });
            }, 50);

          
            $('#btnGrabarAcceso').on("click", function () {
                grabarAcceso();
            });

            $('#btnCancelarAcceso').on("click", function () {
                $('#popupEdicion').bPopup().close();
            });

            $('#txtActivoDesde').Zebra_DatePicker({
                pair: $('#txtActivoHasta'),
                onSelect: function (date) {
                   
                    var date1 = getFecha(date);
                    var date2 = getFecha($('#txtFechaFinal').val());

                    if (date1 > date2) {
                        $('#txtFechaFinal').val(date);
                    }

                }
            });

            $('#txtActivoHasta').Zebra_DatePicker({
                direction: true
            });

        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        }
    });
};

getFecha = function (date) {
    var parts = date.split("/");
    var date1 = new Date(parts[1] + "/" + parts[0] + "/" + parts[2]);
    return date1.getTime();
}

grabar = function () {

    if (validarEmail($('#txtCorreoCuenta').val())) {
        $.ajax({
            type: 'POST',
            url: controlador + 'grabarcorreo',
            data: $('#frmRegistro').serialize(),
            dataType: 'json',
            global: false,
            success: function (result) {
                if (result > 0) {
                    mostrarMensaje('mensaje', 'exito', 'Los datos se grabaron correctamente.');
                    $('#popupEdicion').bPopup().close();
                    listarCorreos();
                }
                else {
                    mostrarMensaje('mensajeCuenta', 'error', 'Se ha producido un error.');
                }
            },
            error: function () {
                mostrarMensaje('mensajeCuenta', 'error', 'Se ha producido un error.');
            }
        });
    }
    else {
        mostrarMensaje('mensajeCuenta', 'alert', 'Ingrese un correo válido.');
    }
};


grabarAcceso = function () {
    if (confirm('¿Está seguro de realizar esta acción?')) {
        $.ajax({
            type: 'POST',
            url: controlador + 'grabaracceso',
            data: $('#frmRegistroAcceso').serialize(),
            dataType: 'json',
            global: false,
            success: function (result) {
                if (result > 0) {
                    mostrarMensaje('mensaje', 'exito', 'Los datos se grabaron correctamente.');
                    $('#popupEdicion').bPopup().close();
                    listarAccesos();
                }
                else {
                    mostrarMensaje('mensajeAcceso', 'error', 'Se ha producido un error.');
                }
            },
            error: function () {
                mostrarMensaje('mensajeAcceso', 'error', 'Se ha producido un error.');
            }
        });
    }
};


mostrarMensaje = function (id, tipo, mensaje) {
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);
};

validarEmail = function (email) {

    if (email == "") {
        return false;
    }
    else {
        var regex = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
        return regex.test(email);
    }
}