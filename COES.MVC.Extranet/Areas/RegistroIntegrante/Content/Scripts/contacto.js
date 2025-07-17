var controlador = siteRoot + 'RegistroIntegrante/Contacto/';

$(function () {

    $("input[type=text]").keyup(function () {
        if (!(this.id == "txtCorreo" || this.id == "txtCorreoEdicion"))
            $("#" + this.id).val($("#" + this.id).val().toUpperCase());
    });


    cargarContactos();

    $('#btnAgregarContacto').click(function () {
        AgregarContacto();
    });   

    $("#btnCancelarEdicion").click(function () {
        $('#popupEdicion').bPopup().close();
        LimpiarContactoPopUp();
    });

    $("#btnGrabarEdicion").click(function () {
        document.getElementById("mensajeEdicion").style.display = "inline-block";
        grabar();
    });

});

cargarContactos = function () {
    $.ajax({
        type: 'POST',
        url: controlador + 'listado',
        success: function (evt) {
            $('#listadoContactos').html(evt);
        },
        error: function () {
            mostrarError();
        }
    });
}

AgregarContacto = function () {
    $("#mensaje").hide();
    enabledFields();
    setTimeout(function () {
        $('#popupAgregar').bPopup({
            autoClose: false,
        });
    }, 50);
    $('#btnCancelarNuevo').click(function () {
        $('#popupAgregar').bPopup().close();
        LimpiarContactoPopUp();
    });

    $("#btnGrabarNuevo").click(function () {

        if (!ValidarRequerido()) {
            $("#mensaje").show();
            return false;
        } else
            $("#mensaje").hide();

        var validacion = validar();
        if (validacion == "") {
            $('#mensaje').removeClass();
            $('#mensaje').html("Todos los campos estan correctos");
            $('#mensaje').addClass('action-exito');
        


        var formData = new FormData();

        // Datos del contacto
        formData.append("Nombres", $("#txtNombres").val());
        formData.append("Apellidos", $("#txtApellidos").val());
        formData.append("CargoEmpresa", $("#txtCargoEmpresa").val());
        formData.append("Telefono", $("#txtTelefono").val());
        formData.append("TelefonoMovil", $("#txtMovil").val());
        formData.append("CorreoElectronico", $("#txtCorreo").val());

        $.ajax({
            type: 'POST',
            url: controlador + "GrabarNuevo",
            data: formData,
            dataType: 'html',
            contentType: false,
            processData: false,
            success: function (result) {
                if (result == 1) {
                    cargarContactos();
                    $('#mensaje').removeClass();
                    $('#mensaje').html("Los datos han sido guardados correctamente");
                    $('#mensaje').addClass('action-exito');
                    disabledFields();
                    setTimeout(function () {
                        $('#popupAgregar').bPopup().close();
                        LimpiarContactoPopUp();
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
            $("#mensaje").show();
            $('#mensaje').removeClass();
            $('#mensaje').html(validacion);
            $('#mensaje').addClass('action-alert');
        }
     });    
}

EliminarContacto = function (idContacto) {


    mensajeOperacion("Este seguro de eliminar el contacto seleccionado?", null
        , {
            showCancel: true,
            onOk: function () {
                $.ajax({
                    type: 'POST',
                    url: controlador + 'eliminar',
                    data: {
                        idContacto: idContacto
                    },
                    dataType: 'json',
                    success: function (result) {
                        if (result == 1) {
                            //alert("Se ha eliminado el contacto.");
                            cargarContactos();
                        } else {
                            mostrarError();
                        }
                    },
                    error: function () {
                        mostrarError();
                    }
                });
            },
            onCancel: function () {

            }
        });



}


EditarContacto = function (idContacto) {
    $("#mensajeEdicion").hide();
    $.ajax({
        type: 'POST',
        data: {
            idContacto: idContacto
        },
        url: controlador + 'edicion',
        success: function (evt) {
            $('#contenidoEdicion').html(evt);
            setTimeout(function () {
                $('#popupEdicion').bPopup({
                    autoClose: false,
                });
            }, 50);
            disabledFieldsEdicion();
            enabledFieldsEdicion();

            $("input[type=text]").keyup(function () {
                if (!(this.id == "txtCorreo" || this.id == "txtCorreoEdicion")) {
                    $("#" + this.id).val($("#" + this.id).val().toUpperCase());
                }
            });

            $('#btnCancelarEdicion').click(function () {
                $('#popupEdicion').bPopup().close();
                LimpiarContactoEditarPopUp();
            });
            $("#btnGrabarEdicion").click(function () {
                $("#mensajeEdicion").show();
                grabarEdicion(idContacto);
            });
        }
    });
}

ValidarRequerido = function () {
    var validator = true;
    var contError = 0;
    // Validar datos
    $(".DatosRequeridos").each(function () {
        if ($(this).hasClass("Required")) {
            var spn = $(this).attr("data-validator-id");
            if ($(this).val() == "" || $(this).val() == "0" || $(this).val() == null) {
                $("#" + spn).addClass("Error");
                contError++;
            } else {
                $("#" + spn).removeClass("Error");
            }
        }
    });

    if (contError > 0) {
        validator = false;
    }
    return validator;
}

ValidarRequeridoEdicion= function () {
    var validator = true;
    var contError = 0;
    // Validar datos
    $(".DatosRequeridos").each(function () {
        if ($(this).hasClass("RequiredEdicion")) {
            var spn = $(this).attr("data-validator-id");
            if ($(this).val() == "" || $(this).val() == "0" || $(this).val() == null) {
                $("#" + spn).addClass("Error");
                contError++;
            } else {
                $("#" + spn).removeClass("Error");
            }
        }
    });

    if (contError > 0) {
        validator = false;
    }
    return validator;
}

grabarEdicion = function (idContacto) {

    if (!ValidarRequeridoEdicion()) {
        $("#mensajeEdicion").show();
        return false;
    } else
        $("#mensajeEdicion").hide();

    var validacion = validarEdicion();
    if (validacion == "") {
        $('#mensajeEdicion').removeClass();
        $('#mensajeEdicion').html("Todos los campos estan correctos");
        $('#mensajeEdicion').addClass('action-exito');
    

        var formData = new FormData();

        // Datos del contacto
        
        formData.append("RpteCodi", $("#hdfRptecodi").val());
        formData.append("Nombres", $("#txtNombresEdicion").val());
        formData.append("Apellidos", $("#txtApellidosEdicion").val());
        formData.append("CargoEmpresa", $("#txtCargoEmpresaEdicion").val());
        formData.append("Telefono", $("#txtTelefonoEdicion").val());
        formData.append("TelefonoMovil", $("#txtMovilEdicion").val());
        formData.append("CorreoElectronico", $("#txtCorreoEdicion").val());


        $.ajax({
            type: 'POST',
            url: controlador + "GrabarEdicion",
            data: formData,
            dataType: 'html',
            contentType: false,
            processData: false,
            success: function (result) {
                if (result == 1) {
                    cargarContactos();
                    $('#mensajeEdicion').removeClass();
                    $('#mensajeEdicion').html("Los datos han sido guardados correctamente");
                    $('#mensajeEdicion').addClass('action-exito');
                    disabledFieldsEdicion();
                    setTimeout(function () {
                        $('#popupEdicion').bPopup().close();
                        LimpiarContactoEditarPopUp();
                    }, 50);
                } else {
                    mostrarErrorEdicion();
                }
            },
            error: function () {
                mostrarErrorEdicion();
            }
        });
    } else {
        $('#mensajeEdicion').show();
        $('#mensajeEdicion').removeClass();
        $('#mensajeEdicion').html(validacion);
        $('#mensajeEdicion').addClass('action-alert');
    }
}

validar = function () {
    var mensaje = "<ul>", flag = true;

    if ($('#txtNombres').val() == "") {
        mensaje = mensaje + "<li>Ingrese Nombres</li>";
        flag = false;
    }

    if ($('#txtApellidos').val() == "") {
        mensaje = mensaje + "<li>Ingrese Apellidos</li>";
        flag = false;
    }

    if ($('#txtCargoEmpresa').val() == "") {
        mensaje = mensaje + "<li>Ingrese Cargo</li>";
        flag = false;
    }

    if ($('#txtCorreo').val() == "") {
        mensaje = mensaje + "<li>Ingrese Correo</li>";
        flag = false;
    } else {
        if (IsEmail($('#txtCorreo').val()) == false)
        { 
            mensaje = mensaje + "<li>Ingrese un correo válido</li>";
            flag = false;
        }
    }

    if ($('#txtTelefono').val() == "") {
        mensaje = mensaje + "<li>Ingrese número de teléfono</li>";
        flag = false;
    }
    else {
        if (IsOnlyDigit($('#txtTelefono').val()) == false)
        {
            mensaje = mensaje + "<li>Ingrese un número de teléfono válido</li>";
            flag = false;
        }           
    }

    if ($('#txtMovil').val() == "") {
        mensaje = mensaje + "<li>Ingrese número de teléfono móvil</li>";
        flag = false;
    }
    else {
        if (IsOnlyDigit($('#txtMovil').val()) == false)
        {
            mensaje = mensaje + "<li>Ingrese un número de teléfono válido</li>";
            flag = false;
        }
            
    }

    mensaje = mensaje + "</ul>";
    if (flag) mensaje = "";

    return mensaje;
}

validarEdicion = function () {
    var mensaje = "<ul>", flag = true;

    if ($('#txtNombresEdicion').val() == "") {
        mensaje = mensaje + "<li>Ingrese Nombres</li>";
        flag = false;
    }

    if ($('#txtApellidosEdicion').val() == "") {
        mensaje = mensaje + "<li>Ingrese Apellidos</li>";
        flag = false;
    }

    if ($('#txtCargoEmpresaEdicion').val() == "") {
        mensaje = mensaje + "<li>Ingrese Cargo</li>";
        flag = false;
    }

    if ($('#txtCorreoEdicion').val() == "") {
        mensaje = mensaje + "<li>Ingrese Correo</li>";
        flag = false;
    } else {
        if (IsEmail($('#txtCorreoEdicion').val()) == false)
        {
            mensaje = mensaje + "<li>Ingrese un correo válido</li>";
            flag = false;
        }            
    }

    if ($('#txtTelefonoEdicion').val() == "") {
        mensaje = mensaje + "<li>Ingrese número de teléfono</li>";
        flag = false;
    }
    else {
        if (IsOnlyDigit($('#txtTelefonoEdicion').val()) == false)
        {
            mensaje = mensaje + "<li>Ingrese un número de teléfono válido</li>";
            flag = false;
        }
            
    }

    if ($('#txtMovilEdicion').val() == "") {
        mensaje = mensaje + "<li>Ingrese número de teléfono móvil</li>";
        flag = false;
    }
    else {
        if (IsOnlyDigit($('#txtMovilEdicion').val()) == false)
        {
            mensaje = mensaje + "<li>Ingrese un número de teléfono válido</li>";
            flag = false;
        } 
            
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

function mostrarErrorEdicion() {
    $('#mensaje').removeClass();
    $('#mensaje').html("Ha ocurrido un error, por favor intente nuevamente.");
    $('#mensaje').addClass('action-error');
}

LimpiarContactoPopUp = function () {
    $(".DatosRequeridos").each(function () {
        $(this).val("");
    });
}

LimpiarContactoEditarPopUp = function () {
    $(".DatosRequeridosEdicion").each(function () {
        $(this).val("");
    });
}


enabledFields = function () {
    $('#txtNombres').prop("disabled", false);
    $('#txtApellidos').prop("disabled", false);
    $('#txtCargoEmpresa').prop("disabled", false);
    $('#txtTelefono').prop("disabled", false);
    $('#txtMovil').prop("disabled", false);
    $('#txtCorreo').prop("disabled", false);

    $("#txtNombres").css("backgroundColor", "white")
    $("#txtApellidos").css("backgroundColor", "white")
    $("#txtCargoEmpresa").css("backgroundColor", "white")
    $("#txtTelefono").css("backgroundColor", "white")
    $("#txtMovil").css("backgroundColor", "white")
    $("#txtCorreo").css("backgroundColor", "white")

}

disabledFields = function () {

    $('#txtNombres').prop("disabled", true);
    $('#txtApellidos').prop("disabled", true);
    $('#txtCargoEmpresa').prop("disabled", true);
    $('#txtTelefono').prop("disabled", true);
    $('#txtMovil').prop("disabled", true);
    $('#txtCorreo').prop("disabled", true);
}


enabledFieldsEdicion = function () {
    $('#txtNombresEdicion').prop("disabled", false);
    $('#txtApellidosEdicion').prop("disabled", false);
    $('#txtCargoEmpresaEdicion').prop("disabled", false);
    $('#txtTelefonoEdicion').prop("disabled", false);
    $('#txtMovilEdicion').prop("disabled", false);
    $('#txtCorreoEdicion').prop("disabled", false);

    $("#txtNombresEdicion").css("backgroundColor", "white")
    $("#txtApellidosEdicion").css("backgroundColor", "white")
    $("#txtCargoEmpresaEdicion").css("backgroundColor", "white")
    $("#txtTelefonoEdicion").css("backgroundColor", "white")
    $("#txtMovilEdicion").css("backgroundColor", "white")
    $("#txtCorreoEdicion").css("backgroundColor", "white")

}

disabledFieldsEdicion = function () {

    $('#txtNombresEdicion').prop("disabled", true);
    $('#txtApellidosEdicion').prop("disabled", true);
    $('#txtCargoEmpresaEdicion').prop("disabled", true);
    $('#txtTelefonoEdicion').prop("disabled", true);
    $('#txtMovilEdicion').prop("disabled", true);
    $('#txtCorreoEdicion').prop("disabled", true);
}