var controlador = siteRoot + 'RegistroIntegrante/Representante/';
var RowRL = null;
$(function () {    

    $("input[type=text]").keyup(function () {

        if (!(this.id == "txtCorreoRL"))
            $("#" + this.id).val($("#" + this.id).val().toUpperCase());
    });

    cargarRepresentantes();

    $("#btnCancelarRL").click(function () {
        $('#popupEdicion').bPopup().close();
        LimpiarRepresentanteLegalPopUp();        
    });

    $("#btnGrabarRL").click(function () {
        document.getElementById("mensaje").style.display = "inline-block";
        grabar();
    });

});

verDocumentoIdentidad = function () {
    window.open(controlador + 'ver?url=' + $('#hdfNombreArchivoDocumentoIdentidad').val(), "_blank", 'fullscreen=yes');
}

descargarDocumentoIdentidad = function () {
    document.location.href = controlador + 'Download?url=' + $('#hdfNombreArchivoDocumentoIdentidad').val() + '&nombre=' + $('#hdfAdjuntoDocumentoIdentidad').val();
}

verVigenciaPoder = function () {
    window.open(controlador + 'ver?url=' + $('#hdfNombreArchivoVigenciaPoder').val(), "_blank", 'fullscreen=yes');
}

descargarVigenciaPoder = function () {
    document.location.href = controlador + 'Download?url=' + $('#hdfNombreArchivoVigenciaPoder').val() + '&nombre=' + $('#hdfAdjuntoVigenciaPoder').val();
}

cargarRepresentantes = function () {    
    $.ajax({
        type: 'POST',
        url: controlador + 'listado',
        success: function (evt) {
            $('#listadoRepresentantes').html(evt);
        },
        error: function () {
            mostrarError();
        }
    });
}

EditarRepresentanteLegal = function (idRepresentante) {

    $.ajax({
        type: 'POST',
        data: {
            idRepresentante: idRepresentante
        },
        url: controlador + 'edicion',
        success: function (evt) {
            $('#contenidoEdicion').html(evt);
            setTimeout(function () {
                $('#popupEdicion').bPopup({
                    autoClose: false,                   
                });
            }, 50);
            disabledFields();
            enabledFields();
            $('#btnCancelarRL').click(function () {
                $('#popupEdicion').bPopup().close();
                LimpiarRepresentanteLegalPopUp();
            });
            $('#btnGrabarRL').click(function () {                
                $("#mensaje").show();
                grabar(idRepresentante);
            });
        } 
    });
    

    $(".DNIRL").each(function () {
        $(this).hide();
    });
    $(".VigenciaPoderRL").each(function () {
        $(this).hide();
    });

    $("#flDNIRL").show();
    $("#flVigenciaPoderRL").show();
    
}

grabar = function (idRepresentante) {
    var validacion = validarEdicion();
    if (validacion == "") {
        $('#mensaje').removeClass();
        $('#mensaje').html("Todos los campos estan correctos");
        $('#mensaje').addClass('action-exito');

        $.ajax({
            type: 'POST',
            url: controlador + 'ActualizarGestionModificacion',
            data: {
                idRepresentante: idRepresentante,
                telefono: $('#txtTelefonoRL').val(),
                telefonoMovil: $('#txtMovilRL').val()
            },
            dataType: 'json',
            success: function (result) {
                if (result == 1) {
                    cargarRepresentantes();
                    $('#mensaje').removeClass();
                    $('#mensaje').html("Los datos han sido actualizados correctamente");
                    $('#mensaje').addClass('action-exito');
                    disabledAllFields();
                    setTimeout(function () {
                        $('#popupEdicion').bPopup().close();
                        LimpiarRepresentanteLegalPopUp();
                    }, 50);                    
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

validarEdicion = function () {
    var mensaje = "<ul>", flag = true;

    if ($('#txtTelefonoRL').val() == "") {
        mensaje = mensaje + "<li>Ingrese número de teléfono</li>";
        flag = false;
    }
    else {
        if (IsOnlyDigit($('#txtTelefonoRL').val()) == false)
            mensaje = mensaje + "<li>Ingrese un número de teléfono válido</li>";
    }

    if ($('#txtMovilRL').val() == "") {
        mensaje = mensaje + "<li>Ingrese número de teléfono móvil</li>";
        flag = false;
    }
    else {
        if (IsOnlyDigit($('#txtMovilRL').val()) == false)
            mensaje = mensaje + "<li>Ingrese un número de teléfono válido</li>";
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

LimpiarRepresentanteLegalPopUp = function () {
    $(".RepresentanteLegal").each(function () {
        $(this).val("");
    });
}

disabledFields = function () {

    $('#txtDNIRL').prop("disabled", true);
    $('#flDNIRL').prop("disabled", true);
    $('#txtNombresRL').prop("disabled", true);
    $('#txtApellidosRL').prop("disabled", true);
    $('#flVigenciaPoderRL').prop("disabled", true);
    $('#txtCargoEmpresaRL').prop("disabled", true);
    $('#txtCorreoRL').prop("disabled", true);
}

disabledAllFields = function () {

    $('#txtDNIRL').prop("disabled", true);
    $('#flDNIRL').prop("disabled", true);
    $('#txtNombresRL').prop("disabled", true);
    $('#txtApellidosRL').prop("disabled", true);
    $('#flVigenciaPoderRL').prop("disabled", true);
    $('#txtCargoEmpresaRL').prop("disabled", true);
    $('#txtTelefonoRL').prop("disabled", true);
    $('#txtMovilRL').prop("disabled", true);
    $('#txtCorreoRL').prop("disabled", true);

}

enabledFields = function () {
    $('#txtTelefonoRL').prop("disabled", false);
    $('#txtMovilRL').prop("disabled", false);
    $('#txtTelefonoRL').focus();

    $("#txtTelefonoRL").css("backgroundColor", "white")
    $("#txtMovilRL").css("backgroundColor", "white")

}

