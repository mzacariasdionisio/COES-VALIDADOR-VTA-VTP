var controlador = siteRoot + 'Admin/contacto/';

$(function () {

    consultar();

    $('#btnNuevoProceso').on('click', function () {
        editar(0);
    });

    $('#btnCancelarProceso').on('click', function () {
        document.location.href = controlador + 'index';
    });
});

consultar = function () {
    $.ajax({
        type: 'POST',
        url: controlador + 'procesolist',
        success: function (evt) {
            $('#listado').html(evt);
            $('#tablaProceso').dataTable({
                "iDisplayLength": 20
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
        url: controlador + 'ProcesoEdit',
        data: {
            idProceso: id
        },
        global: false,
        success: function (evt) {
            $('#contenidoEdicionProceso').html(evt);
            setTimeout(function () {
                $('#popupEdicionProceso').bPopup({
                    autoClose: false
                });
            }, 50);

            $('#btnGrabar').on("click", function () {
                grabar();
            });

            $('#btnCancelar').on("click", function () {
                $('#popupEdicionProceso').bPopup().close();
            });

        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        }
    });
}

grabar = function () {
    $.ajax({
        type: 'POST',
        url: controlador + 'ProcesoSave',
        data: $('#frmRegistro').serialize(),
        dataType: 'json',
        global: false,
        success: function (result) {
            if (result > 0) {
                mostrarMensaje('mensaje', 'exito', 'Los datos se grabaron correctamente.');
                $('#popupEdicionProceso').bPopup().close();
                consultar();
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

eliminar = function (id) {
    if (confirm('¿Está seguro de realizar esta operación?')) {

        $.ajax({
            type: 'POST',
            url: controlador + 'procesodelete',
            data: {
                idComite: id
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

mostrarMensaje = function (id, tipo, mensaje) {
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);
}

e