var controlador = siteRoot + 'admin/integrante/';

$(function () {

    $('#btnAgregar').on('click', function () {
        editar(0);
    });

    $('#btnAgregarRpte').on('click', function () {
        editarRepresentante(0);
    });

    $('#btnRegresar').on('click', function () {
        document.location.href = controlador + 'index';
    });
    
    listarRepresentantes();
    listarCorreos();
});

listarRepresentantes = function () {
    $.ajax({
        type: 'POST',
        url: controlador + 'representantes',
        data: {
            id: $('#hfCodigoEmpresa').val(),
            ind: $('#hfIntegrante').val()
        },
        success: function (evt) {

            $('#listadoRepresentantes').html(evt);
            oTable = $('#tablaRepresentante').dataTable({
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

};

listarCorreos = function () {
    $.ajax({
        type: 'POST',
        url: controlador + 'correos',
        data: {
            idEmpresa: $('#hfCodigoEmpresa').val()
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
};


editar = function (idCorreo) {

    $.ajax({
        type: 'POST',
        url: controlador + 'detallecorreo',
        data: {
            idCorreo: idCorreo,
            idEmpresa: $('#hfCodigoEmpresa').val()
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

editarRepresentante = function (id) {
    $.ajax({
        type: 'POST',
        url: controlador + 'detallerepresentante',
        data: {
            id: id,
            idEmpr: $('#hfCodigoEmpresa').val(),
            ind: $('#hfIntegrante').val()
        },
        global: false,
        success: function (evt) {
            $('#contenidoRepresentante').html(evt);
            setTimeout(function () {
                $('#popupRepresentante').bPopup({
                    autoClose: false
                });
            }, 50);
                       
            $('#cbIndNotificacion').val($('#hfIndNotificacion').val());

            $('#btnGrabarRepresentante').on("click", function () {
                grabarRepresentante();
            });

            $('#btnCancelarRepresentante').on("click", function () {
                $('#popupRepresentante').bPopup().close();
            });

        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        }
    });
};

eliminarRepresentante = function (idRpt) {
    if (confirm('¿Está seguro de realizar esta acción?')) {
        $.ajax({
            type: 'POST',
            url: controlador + 'EliminarRepresentante',
            data: {
                idRpt: idRpt,
            },
            global: false,
            dataType: 'json',
            success: function (result) {
                if (result == 1) {
                    listarRepresentantes();
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

eliminarCuenta = function (idCuenta) {
    if (confirm('¿Está seguro de realizar esta acción?')) {
        $.ajax({
            type: 'POST',
            url: controlador + 'eliminarcorreo',
            data: {
                idEmpresaCorreo: idCuenta,
                idEmpresa: $('#hfCodigoEmpresa').val()
            },
            global: false,
            dataType: 'json',
            success: function (result) {
                if (result == 1) {
                    listarCorreos();
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

grabarRepresentante = function () {
    $.ajax({
        type: 'POST',
        url: controlador + 'grabarrepresentante',
        data: $('#frmRegistroAdicional').serialize(),
        dataType: 'json',
        global: false,
        success: function (result) {
            if (result > 0) {
                mostrarMensaje('mensaje', 'exito', 'Los datos se grabaron correctamente.');
                $('#popupRepresentante').bPopup().close();
                listarRepresentantes();
            }
            else {
                mostrarMensaje('mensajeCuenta', 'error', 'Se ha producido un error.');
            }
        },
        error: function () {
            mostrarMensaje('mensajeCuenta', 'error', 'Se ha producido un error.');
        }
    });

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