var controlador = siteRoot + 'admin/directorio/';

$(function () {
    consultar();

    $('#btnConsultar').click(function () {
        consultar();
    });
});

consultar = function () {
    $.ajax({
        type: 'POST',
        url: controlador + 'listado',
        data: {
            idArea: $('#cbArea').val()
        },
        success: function (evt) {
            $('#listado').html(evt);
            $('#tabla').dataTable({

            });
        },
        error: function () {
            alert("Error");
        }
    });
}

editar = function (idDirectorio, nombre, apellido, anexo, indanexo, indextranet) {
    $.ajax({
        type: 'POST',
        url: controlador + 'edicion',
        data: {
            idDirectorio: idDirectorio,
            nombre: nombre,
            apellido: apellido,
            anexo: anexo,
            indAnexo: indanexo,
            indExtranet: indextranet
        },
        global: false,
        success: function (evt) {
            $('#contenidoEdicion').html(evt);
            setTimeout(function () {
                $('#popupEdicion').bPopup({
                    autoClose: false
                });
            }, 50);

            $('#cbIndAnexo').val($('#hfIndAnexo').val());
            $('#cbIndExtranet').val($('#hfIndExtranet').val());
            
            $('#btnGrabar').on("click", function () {
                grabar();
            });

            $('#btnCancelar').on("click", function () {
                $('#popupEdicion').bPopup().close();
            });
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error');
        }
    });
}

grabar = function () {
    if (confirm('¿Está seguro de realizar esta acción?')) {
        $.ajax({
            type: 'POST',
            url: controlador + 'actualizaranexo',
            data: {
                idDirectorio: $('#hfIdDirectorio').val(),
                anexo: $('#txtAnexo').val(),
                indAnexo: $('#cbIndAnexo').val(),
                indExtranet: $('#cbIndExtranet').val(),
            },
            global: false,
            dataType: 'json',
            success: function (result) {
                if (result == 1) {
                    $('#popupEdicion').bPopup().close();
                    consultar();
                    mostrarMensaje('mensaje', 'exito', 'La operación se realizó correctamente.');
                }
                else {
                    mostrarMensaje('mensajeEdit', 'error', 'Ha ocurrido un error.');
                }
            },
            error: function () {
                mostrarMensaje('mensajeEdit', 'error', 'Ha ocurrido un error.');
            }
        });
    }
}

mostrarMensaje = function (id, tipo, mensaje) {
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);
}
