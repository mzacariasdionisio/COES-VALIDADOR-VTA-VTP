var controlador = siteRoot + 'publicaciones/suscripcion/';

$(function () {

    $('#btnEnviar').on('click', function () {
        enviar();
    });

});

enviar = function (){
    var mensaje = validar();

    if (mensaje === "") {
        var publicaciones = "";        
        $('#publicacion-content input:checked').each(function () {
            publicaciones = publicaciones + $(this).val() + ",";
        });

        $('#hfDetalle').val(publicaciones);

        if ($("#cbCondiciones").is(':checked') == true) {
            $.ajax({
                type: 'POST',
                url: controlador + 'grabar',
                data: $('#frmRegistro').serialize(),
                dataType: 'json',
                success: function (result) {
                    if (result > 0) {
                        showMensaje('exito', 'Los datos fueron enviados correctamente.');
                        $('#frmRegistro').hide();
                        $('#boxPrincipal').css('border', 'none');
                    }
                    else {
                        showMensaje('error', 'Ha ocurrido un error.');
                    }
                },
                error: function () {
                    showMensaje('error', 'Ha ocurrido un error');
                }
            });
        }
        else {
            showMensaje('alert', 'Por favor acepte las condiciones de tratamiento de datos personales');
        }
    }
    else {
        showMensaje('alert', mensaje);
    }
}

$("#mensaje").css("display", "none");

validar = function () {
    var mensaje = "<ul style='margin-bottom:0px; padding-bottom: 0px;'>";
    var flag = true;
    var contador = 0;

    $('#publicacion-content input:checked').each(function () {
        contador = 1;
    });

    if (contador == 0) {
        mensaje = mensaje + "<li style='margin-bottom:0px; padding-bottom: 0px;'>Seleccione al menos una publicación.</li>";
        flag = false;
    }

    if ($('#txtNombre').val() == "") {
        mensaje = mensaje + "<li style='margin-bottom:0px; padding-bottom: 0px;'>Ingrese su nombre.</li>";
        flag = false;
    }

    if ($('#txtApellido').val() == "") {
        mensaje = mensaje + "<li style='margin-bottom:0px; padding-bottom: 0px;'>Ingrese su apellido.</li>";
        flag = false;
    }

    if ($('#txtCorreo').val() == "") {
        mensaje = mensaje + "<li style='margin-bottom:0px; padding-bottom: 0px;'>Ingrese correo.</li>";
        flag = false;
    }
    else if (!validarEmail($('#txtCorreo').val())) {
        mensaje = mensaje + "<li style='margin-bottom:0px; padding-bottom: 0px;'>Ingrese correo válido.</li>";
        flag = false;
    }

    if ($('#txtTelefono').val() == "") {
        mensaje = mensaje + "<li style='margin-bottom:0px; padding-bottom: 0px;'>Ingrese teléfono.</li>";
        flag = false;
    }

    if ($('#txtEmprpesa').val() == "") {
        mensaje = mensaje + "<li style='margin-bottom:0px; padding-bottom: 0px;'>Ingrese nombre de la empresa.</li>";
        flag = false;
    }

    mensaje = mensaje + "</ul></div>";

    if (flag) mensaje = "";

    return mensaje;
}

validarEmail = function (email) {
    var regex = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    return regex.test(email);
}

openCondiciones = function () {

    $('#popupEdicion').bPopup({
        easing: 'easeOutBack',
        speed: 450,
        transition: 'slideDown',
        modalClose: false
    });
}

showMensaje = function (action, mensaje) {

    $('#mensaje').removeClass();
    $("#mensaje").css("display", "flex");
    if (action === 'exito') {
        $('#mensaje').addClass('coes-form-item--info coes-form-item coes-box coes-box--content pt-3 pe-3 ps-3 pb-3');
    } else {
        $('#mensaje').addClass('coes-form-item--error coes-form-item coes-box coes-box--content pt-3 pe-3 ps-3 pb-3');
    }
    $('#mensaje').html(mensaje);
}