var controlador = siteRoot + 'registrointegrante/admin/';

$(function () {

    $('#btnNuevo').on('click', function () {
        editar(0);
    });

    $('#btnConsultar').on('click', function () {
        consultar();
    });

    consultar();
});

consultar = function () {

    $.ajax({
        type: 'POST',
        url: controlador + 'listado',
        data: {
            anio: $('#cbAnioBusqueda').val(),
            tipo: $('#cbTipoBusqueda').val()
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
};

editar = function (id) {

    $.ajax({
        type: 'POST',
        url: controlador + 'editar',
        data: {
            id: id
        },
        success: function (evt) {
            $('#contenidoEdicion').html(evt);

            setTimeout(function () {
                $('#popupEdicion').bPopup({
                });
            }, 50);

            $('#cbAnio').val($('#hfAnio').val());
            $('#cbTipo').val($('#hfTipo').val());

            $('#btnGrabar').on('click', function () {
                grabar();
            });

            $('#txtFecha').Zebra_DatePicker({
            });

            $('#frmRegistro').validate();
            
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
};

grabar = function () {

    if ($('#frmRegistro').valid()) {
        $.ajax({
            type: 'POST',
            url: controlador + 'grabar',
            data: $('#frmRegistro').serialize(),
            dataType: 'json',
            success: function (result) {
                if (result === 1) {
                    $('#popupEdicion').bPopup().close();
                    mostrarMensaje('mensaje', 'exito', 'El registro fué grabado exitósamente.');
                    consultar();
                }
                else {
                    mostrarMensaje('mensajeEdicion', 'error', 'Ha ocurrido un error.');
                }
            },
            error: function () {
                mostrarMensaje('mensajeEdicion', 'error', 'Ha ocurrido un error.');
            }
        });
    }
    else {
        mostrarMensaje('mensajeEdicion', 'alert', 'Complete los datos solicitados.');
    }
};

eliminar = function (id) {

    if (confirm('¿Está seguro de realizar esta acción?')) {
        $.ajax({
            type: 'POST',
            url: controlador + 'eliminar',
            data: {
                id: id
            },
            dataType: 'json',
            success: function (result) {
                if (result === 1) {
                    consultar();
                    mostrarMensaje('mensaje', 'exito', 'El registro se eliminó correctamente.');
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
};

mostrarMensaje = function (id, tipo, mensaje) {
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);
};