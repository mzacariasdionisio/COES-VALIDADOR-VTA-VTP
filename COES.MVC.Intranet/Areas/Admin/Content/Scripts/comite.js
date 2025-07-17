var controlador = siteRoot + 'Admin/contacto/';

$(function () {

    consultar();

    $('#btnNuevoComite').on('click', function () {
        editar(0);
    });

    $('#btnCancelarComite').on('click', function () {
        document.location.href = controlador + 'index';
    });
});

consultar = function () {
    $.ajax({
        type: 'POST',
        url: controlador + 'comitelist',
        success: function (evt) {
            $('#listado').html(evt);
            $('#tablaComite').dataTable({
                "iDisplayLength": 20
            });
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
}

editListas = function (id) {

    $.ajax({
        type: 'POST',
        url: controlador + 'ComiteLista',
        data: {
            idComiteLista: id
        },
        global: false,
        success: function (evt) {
            $('#contenidoListasComite').html(evt);
            setTimeout(function () {
                $('#popupListasComite').bPopup({
                    autoClose: false
                });
            }, 50);

            $('#btnGrabarLista').on("click", function () {
                grabarLista();
            });

            $('#btnCancelarLista').on("click", function () {
                $('#popupListasComite').bPopup().close();
            });

        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        }
    });
}

editar = function (id) {

    $.ajax({
        type: 'POST',
        url: controlador + 'ComiteEdit',
        data: {
            idComite: id
        },
        global: false,
        success: function (evt) {
            $('#contenidoEdicionComite').html(evt);
            setTimeout(function () {
                $('#popupEdicionComite').bPopup({
                    autoClose: false
                });
            }, 50);

            $('#btnGrabar').on("click", function () {
                grabar();
            });

            $('#btnCancelar').on("click", function () {
                $('#popupEdicionComite').bPopup().close();
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
        url: controlador + 'ComiteSave',
        data: $('#frmRegistro').serialize(),
        dataType: 'json',
        global: false,
        success: function (result) {
            if (result > 0) {
                mostrarMensaje('mensaje', 'exito', 'Los datos se grabaron correctamente.');
                $('#popupEdicionComite').bPopup().close();
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
            url: controlador + 'comitedelete',
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

grabarLista = function () {
    $.ajax({
        type: 'POST',
        url: controlador + 'ComiteListaSave',
        data: $('#frmRegistroLista').serialize(),
        dataType: 'json',
        global: false,
        success: function (result) {
            if (result > 0) {
                mostrarMensajeLista('mensajeListaEdicion', 'exito', 'Los datos se grabaron correctamente.');
                setTimeout(function () {
                    $('#popupListasComite').bPopup().close();
                }, 50);
            }
            else {
                mostrarMensajeLista('mensajeListaEdicion', 'error', 'Se ha producido un error.');
            }
        },
        error: function () {
            mostrarMensajeLista('mensajeListaEdicion', 'error', 'Se ha producido un error.');
        }
    });
}

eliminarListabyComite = function (id) {
    $.ajax({
        type: 'POST',
        url: controlador + 'ComiteListaDelete',
        data: {
            idComiteLista: id
        },
        success: function (result) {
            if (result > 0) {
                mostrarMensajeLista('mensajeListaEdicion', 'exito', 'Lista eliminada correctamente.');
                $('#popupListasComite').bPopup().close();
            }
            else {
                mostrarMensajeLista('mensajeListaEdicion', 'error', 'No se logró eliminar la lista.');
            }
        },
        error: function () {
            mostrarMensajeLista('mensajeListaEdicion', 'error', 'Se ha producido un error.');
        }
    });
}

mostrarMensajeLista = function (id, tipo, mensaje) {
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);
}