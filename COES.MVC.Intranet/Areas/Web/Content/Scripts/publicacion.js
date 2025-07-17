var controlador = siteRoot + 'web/suscripcion/';

$(function () {
       
    consultar();

    $('#btnNuevo').on('click', function () {
        editar(0);
    });

    $('#btnCancelar').on('click', function () {
        document.location.href = controlador + 'index';
    });
});

consultar = function () {
    $.ajax({
        type: 'POST',
        url: controlador + 'publicacionlist',   
        success: function (evt) {
            $('#listado').html(evt);
            $('#tabla').dataTable({
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
        url: controlador + 'publicacionedit',
        data: {
            idPublicacion: id
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

            $('#cbEstado').val($('#hfEstado').val());
            $('#cbArea').val($('#hfArea').val());

            $('#btnCancelar').on("click", function () {
                $('#popupEdicion').bPopup().close();
            });

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
            url: controlador + 'publicaciondelete',
            data: {
                idPublicacion: id
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
            url: controlador + 'publicacionsave',
            data: $('#frmRegistro').serialize(),
            dataType: 'json',
            global: false,
            success: function (result) {
                if (result > 0) {
                    mostrarMensaje('mensaje', 'exito', 'Los datos se grabaron correctamente.');
                    $('#popupEdicion').bPopup().close();
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
    else {
        mostrarMensaje('mensajeEdicion', 'alert', validacion);
    }
}

validarRegistro = function () {

    var mensaje = "<ul>";
    var flag = true;
    
    if ($('#txtNombre').val() == "") {
        mensaje = mensaje + "<li>Ingrese su nombre.</li>";
        flag = false;
    }
        
    if ($('#txtCorreo').val() == "") {
        mensaje = mensaje + "<li>Ingrese correo.</li>";
        flag = false;
    }
    else if (!validarEmail($('#txtCorreo').val())) {
        mensaje = mensaje + "<li>Ingrese correo válido.</li>";
        flag = false;
    }

    if ($('#cbArea').val() == "") {
        mensaje = mensaje + "<li>Seleccione el área responsable.</li>";
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

getFecha = function (date) {
    var parts = date.split("/");
    var date = new Date(parts[1] + "/" + parts[0] + "/" + parts[2]);
    return date.getTime();
}


