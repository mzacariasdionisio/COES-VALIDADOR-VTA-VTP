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
    $('#txtNroRegistro').prop("disabled", true);


    $('#txtNumeroRuc').css("backgroundColor", "#f8f8f8");
    $('#txtNombreComercial').css("backgroundColor", "#f8f8f8");
    $('#txtDenominacionRazonSocial').css("backgroundColor", "#f8f8f8");
    $('#txtDomicilioLegal').css("backgroundColor", "#f8f8f8");
    $('#txtSigla').css("backgroundColor", "#f8f8f8");
    $('#txtNumeroPartidaRegistral').css("backgroundColor", "#f8f8f8");
    $('#txtTelefono').css("backgroundColor", "#f8f8f8");
    $('#txtFax').css("backgroundColor", "#f8f8f8");
    $('#txtPaginaWeb').css("backgroundColor", "#f8f8f8");
    $('#txtNroRegistro').css("backgroundColor", "#f8f8f8");
};

enabledFields = function () {

    //$('#txtNumeroRuc').prop("disabled", false);
    $('#txtNombreComercial').prop("disabled", false);
    $('#txtDenominacionRazonSocial').prop("disabled", false);
    $('#txtDomicilioLegal').prop("disabled", false);
    $('#txtSigla').prop("disabled", false);
    $('#txtNumeroPartidaRegistral').prop("disabled", false);
    $('#txtTelefono').prop("disabled", false);
    $('#txtFax').prop("disabled", false);
    $('#txtPaginaWeb').prop("disabled", false);    
    $('#txtNroRegistro').prop("disabled", false);

    //$('#txtNumeroRuc').css("backgroundColor", "white");
    $('#txtNombreComercial').css("backgroundColor", "white");
    $('#txtDenominacionRazonSocial').css("backgroundColor", "white");
    $('#txtDomicilioLegal').css("backgroundColor", "white");
    $('#txtSigla').css("backgroundColor", "white");
    $('#txtNumeroPartidaRegistral').css("backgroundColor", "white");
    $('#txtTelefono').css("backgroundColor", "white");
    $('#txtFax').css("backgroundColor", "white");
    $('#txtPaginaWeb').css("backgroundColor", "white");
    $('#txtNroRegistro').css("backgroundColor", "white");


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
                idEmpresa: $('#hdfemprcodi').val(),
                nombreComercial: $('#txtNombreComercial').val(),
                razonSocial: $('#txtDenominacionRazonSocial').val(),
                domicilioLegal: $('#txtDomicilioLegal').val(),
                sigla: $('#txtSigla').val(),
                nroPartida: $('#txtNumeroPartidaRegistral').val(),
                telefono: $('#txtTelefono').val(),
                fax: $('#txtFax').val(),
                paginaWeb: $('#txtPaginaWeb').val(),
                nroRegistro: $('#txtNroRegistro').val()
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

    //if (IsOnlyDigit($('#txtTelefono').val()) == false) {
    //    mensaje = mensaje + "<li>Ingrese un número de teléfono válido</li>";
    //    flag = false;
    //}

    //if (IsOnlyDigit($('#txtFax').val()) == false) { 
    //    mensaje = mensaje + "<li>Ingrese un número de fax válido</li>";
    //    flag = false;
    //}

    if (IsOnlyDigit($('#txtNroRegistro').val()) == false) {
        mensaje = mensaje + "<li>Ingrese un número de registro válido</li>";
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