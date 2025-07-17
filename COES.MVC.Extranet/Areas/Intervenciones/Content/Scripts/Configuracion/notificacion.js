var controlador = siteRoot + "intervenciones/configuracion/";

$(function () {

    $('#btnAceptar').on('click', function () {
        confirmar();
    });

    $('#btnConfirmar').on('click', function () {
        grabar();
    });

    $('#btnCancelar').on('click', function () {
        $('#popupConfirmacion').bPopup().close();
    });

    consultar();
});

function confirmar() {
    $('#popupConfirmacion').bPopup({
    });
}

function consultar() {
    $.ajax({
        type: 'POST',
        url: controlador + 'listanotificacion',
        data: {
            empresa: $('#cbEmpresa').val(),
            estado: $('#cbEstado').val()
        },
        success: function (evt) {
            $('#detalleFormato').html(evt);
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
}

function grabar() {
    var array = [];
    $('#tablaNotificacion tbody tr').each(function () {
        var item = [];
        item.push($(this).find("#hfEmpresa").val());
        item.push($(this).find("#hfEmpresaNombre").val());
        item.push($(this).find("#hfUsuario").val());
        item.push($(this).find("#hfUsuarioNombre").val());
        $(this).find('input:checkbox').each(function () {
            item.push(($(this).is(":checked") == true) ? 1 : 0);
        });
        item.push($(this).find("#hfCodigo").val());
        item.push($(this).find('#hfEmail').val());
        array.push(item);
    });
    $('#popupConfirmacion').bPopup().close();
    $.ajax({
        type: 'POST',
        url: controlador + 'GrabarNotificacion',
        contentType: 'application/json',
        data: JSON.stringify({
            data: array
        }),
        dataType: 'json',
        success: function (result) {
            if (result.Result == 1) {
                if (result.ListaValidacion.length == 0) {
                    mostrarMensaje('mensaje', 'exito', 'La configuración de notificación de mensajes se realizó correctamente.');
                    consultar();
                }
                else {
                    var html = "<ul>";
                    for (var i in result.ListaValidacion) {
                        html = html + "<li>" + result.ListaValidacion[i] + "</li>";
                    }
                    html = html + "</ul>";
                    mostrarMensaje('mensaje', 'alert', html);
                }
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


function verHistorico(idEmpresa, idUsuario, empresa, usuario) {

    $.ajax({
        type: 'POST',
        url: controlador + 'historiconotificacion',
        data: {
            idEmpresa: idEmpresa,
            empresa: empresa,
            idUsuario: idUsuario,
            usuario: usuario
        },
        success: function (evt) {
            $('#contenidoHistorico').html(evt);
            setTimeout(function () {
                $('#popupHistorico').bPopup({
                    autoClose: false,
                    modalClose: false,
                    escClose: false,
                    follow: [false, false]
                });
            }, 50);
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
}

function mostrarMensaje(id, tipo, mensaje) {
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);
}
