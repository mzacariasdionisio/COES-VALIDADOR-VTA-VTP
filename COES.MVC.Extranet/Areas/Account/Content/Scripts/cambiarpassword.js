var controlador = siteRoot + 'account/cambiarpassword/';

$(document).ready(function () {
    $('#btnActualizar').click(function () {
        actualizar();
    });

    $('#btnCancelar').click(function () {

    })
});

actualizar = function () {
    var validacion = validar();  
    $('#mensaje').removeClass();
    if (validacion == "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'cambiar',
            data: {
                passwordActual: $('#txtPassword').val(),
                passwordNueva: $('#txtPasswordNuevo').val()
            },
            dataType: 'json',
            success: function (result) {
                if (result == 1) {
                    $('#mensaje').html("La contraseña de modificó correctamente");
                    $('#mensaje').addClass('action-exito');
                    $('#txtPassword').val('');
                    $('#txtPasswordNuevo').val('');
                    $('#txtPasswordConfirmacion').val('');
                } else {
                    if (result == 2)
                        $('#mensaje').html("<ul><li>La Contraseña actual no es la correcta, por favor ingrese de nuevo</li></ul>");
                    if (result == -1) 
                        $('#mensaje').html("<ul><li>Hubo un error</li></ul>");
                    $('#mensaje').addClass('action-alert');
                }
            },
            error: function () {
                alert('Hubo un error');
            }
        });
    } else {
        $('#mensaje').html(validacion);
        $('#mensaje').addClass('action-alert');
    }
}

validar = function () {
    var mensaje = "<ul>", flag = true;
    if ($("#txtPassword").val() == "") {
        mensaje = mensaje + "<li>Ingrese Contraseña actual</li>";
        flag = false;
    }

    if ($("#txtPasswordNuevo").val() == "") {
        mensaje = mensaje + "<li>Ingrese Contraseña Nueva</li>";
        flag = false;
    } else {
        if ($("#txtPasswordNuevo").val().length < 6) {
            mensaje = mensaje + "<li>Se necesita un mínimo 6 caracteres para la contraseña</li>";
            flag = false;
        }
    }

    if ($("#txtPasswordConfirmacion").val() == "") {
        mensaje = mensaje + "<li>Ingrese Contraseña de Confirmación</li>";
        flag = false;
    } else {
        if ($("#txtPasswordNuevo").val() != $("#txtPasswordConfirmacion").val()) {
            mensaje = mensaje + "<li>Las contraseñas no coinciden</li>";
            flag = false;
        }
    }

    mensaje = mensaje + "</ul>";
    if (flag) mensaje = "";

    return mensaje;
}