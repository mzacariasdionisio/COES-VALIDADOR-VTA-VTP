var controlador = siteRoot + 'RegistroIntegrante/Empresa/';

$(document).ready(function () {    

    $("input[type=text]").keyup(function () {
        $("#" + this.id).val($("#" + this.id).val().toUpperCase());
    });

    $("#mensaje").hide();
    disabledFields();

    $("#btnEditar").click(function () {
        $("#mensaje").hide();
        $('#btnEditar').prop("disabled", true);
        enabledFields();
    });
    $("#btnCancelar").click(function () {
        $("#mensaje").hide();
        $('#btnEditar').prop("disabled", false);
        disabledFields();
    });
    $("#btnGrabar").click(function () {
        $("#mensaje").show();
        $('#btnEditar').prop("disabled", false);
        grabar();

    });
});

disabledFields = function () {
    $("#btnCancelar").hide();
    $("#btnGrabar").hide();

    $('#txtNumeroRuc').prop("disabled", true);
    $('#txtNombreComercial').prop("disabled", true);
    $('#txtDenominacionRazonSocial').prop("disabled", true);
    $('#txtDomicilioLegal').prop("disabled", true);
    $('#txtSigla').prop("disabled", true);
    $('#txtNumeroPartidaRegistral').prop("disabled", true);
    $('#txtTelefono').prop("disabled", true);
    $('#txtFax').prop("disabled", true);
    $('#txtPaginaWeb').prop("disabled", true);

    $("#txtDomicilioLegal").css("backgroundColor", "#f8f8f8")
    $("#txtTelefono").css("backgroundColor", "#f8f8f8")
    $("#txtFax").css("backgroundColor", "#f8f8f8")
    $("#txtPaginaWeb").css("backgroundColor", "#f8f8f8")
}

enabledFields = function () {

    $('#txtDomicilioLegal').prop("disabled", false);
    $('#txtTelefono').prop("disabled", false);
    $('#txtFax').prop("disabled", false);
    $('#txtPaginaWeb').prop("disabled", false);
    $('#txtDomicilioLegal').focus();

    $("#txtDomicilioLegal").css("backgroundColor", "white")
    $("#txtTelefono").css("backgroundColor", "white")
    $("#txtFax").css("backgroundColor", "white")
    $("#txtPaginaWeb").css("backgroundColor", "white")
    $("#btnCancelar").show();
    $("#btnGrabar").show();
}

grabar = function () {
    var validacion = validarModificacion();
    if (validacion == "") {
        $('#mensaje').removeClass();
        $('#mensaje').html("Todos los campos estan correctos");
        $('#mensaje').addClass('action-exito');        

        $.ajax({
            type: 'POST',
            url: controlador + 'ActualizarGestionModificacion',
            data: {
                domicilioLegal: $('#txtDomicilioLegal').val(),
                telefono: $('#txtTelefono').val(),
                fax: $('#txtFax').val(),
                paginaWeb: $('#txtPaginaWeb').val()
            },
            dataType: 'json',
            success: function (result) {
                if (result == 1) {
                    $('#mensaje').removeClass();
                    $('#mensaje').html("Los datos de la empresa han sido actualizados correctamente");
                    $('#mensaje').addClass('action-exito');

                    disabledFields();
                } else {
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

validarModificacion = function () {
    var mensaje = "<ul>", flag = true;

    if (IsOnlyDigit($('#txtTelefono').val()) == false) {
        mensaje = mensaje + "<li>Ingrese un número de teléfono válido</li>";
        flag = false;
    }

    if (IsOnlyDigit($('#txtFax').val()) == false) { 
        mensaje = mensaje + "<li>Ingrese un número de fax válido</li>";
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