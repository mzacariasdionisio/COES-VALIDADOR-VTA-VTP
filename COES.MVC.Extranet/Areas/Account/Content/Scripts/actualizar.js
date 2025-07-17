var controlador = siteRoot + 'account/actualizar/';
$(document).ready(function () {
    $("#btnActualizar").click(function () {
        actualizar();
    });

    $("#btnCancelar").click(function () {
        document.location.href = siteRoot + 'home/default';
    });
});

actualizar = function () {
    var validacion = validarActualizacion();
    if (validacion == "") {
        $('#mensaje').removeClass();
        $('#mensaje').html("Todos los campos estan correctos");
        $('#mensaje').addClass('action-exito');

        $.ajax({
            type: 'POST',
            url: controlador + 'actualizar',
            data: $('#frmRegistro').serialize(),
            dataType: 'json',
            success: function (result) {
                if (result == 1) {
                    $('#mensaje').removeClass();
                    $('#mensaje').html("Los datos han sido actualizados correctamente");
                    $('#mensaje').addClass('action-exito');
                } else if (result == 2) {
                    $('#mensaje').removeClass();
                    $('#mensaje').html("El email ingresado ya se encuentra registrado en el sistema.");
                    $('#mensaje').addClass('action-alert');
                }
                else {
                    mostrarError();
                }
            },
            error: function () {
                mostrarError();
            }
        });
    } else {
        $('#mensaje').removeClass();
        $('#mensaje').html(validacion);
        $('#mensaje').addClass('action-alert');
    }
}

validarActualizacion = function () {
    var mensaje = "<ul>", flag = true;

    if ($("#txtNombre").val() == "") {
        mensaje = mensaje + "<li>Ingrese Nombre</li>";
        flag = false;
    } else {
        if (IsText($('#txtNombre').val()) == false) {
            mensaje = mensaje + "<li>Ingrese letras en el nombre</li>";
            flag = false;
        }
    }
    if ($('#cbEmpresa').val() == "") {
        mensaje = mensaje + "<li>Seleccione empresa</li>";
        flag = false;
    }
    else {
        $('#hfEmpresa').val($('#cbEmpresa option:selected').text())
    }
    if ($('#txtCorreo').val() == "") {
        mensaje = mensaje + "<li>Ingrese correo electrónico</li>";
        flag = false;
    }
    else {
        if (IsEmail($('#txtCorreo').val()) == false) {
            mensaje = mensaje + "<li>Ingrese un correo válido.</li>";
            flag = false;
        }
    }
    if ($('#txtTelefono').val() == "") {
        mensaje = mensaje + "<li>Ingrese teléfono</li>";
        flag = false;
    }
    else {
        if (IsOnlyDigit($('#txtTelefono').val()) == false)
            mensaje = mensaje + "<li>Ingrese un número telefónico válido</li>";
    }
    if ($('#txtArea').val() == "") {
        mensaje = mensaje + "<li>Ingrese área laboral</li>";
        flag = false;
    }
    if ($('#txtCargo').val() == "") {
        mensaje = mensaje + "<li>Ingrese cargo</li>";
        flag = false;
    }
    mensaje = mensaje + "</ul>";
    if (flag) mensaje = "";

    return mensaje;
}

function IsText(texto) {
    var regex = /^[A-Za-záéíóú ]+$/;
    return regex.test(texto);
}
function IsEmail(email) {
    var regex = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    return regex.test(email);
}
function IsOnlyDigit(texto) {
    for (var i = 0; i < texto.length; i++)
        if (!(IsDigito(texto[i]) || texto[i] == ' ' || texto[i] == '+' || texto[i] == '(' || texto[i] == ')' || texto[i] == '-'))
            return false;
    return true;
}
function IsDigito(caracter) {
    if (caracter == '0' || caracter == '1' || caracter == '2' || caracter == '3' || caracter == '4' || caracter == '5' || caracter == '6' || caracter == '7' || caracter == '8' || caracter == '9')
        return true;
    return false;
}

function mostrarError() {
    $('#mensaje').removeClass();
    $('#mensaje').html("Ha ocurrido un error, por favor intente nuevamente.");
    $('#mensaje').addClass('action-error');
}