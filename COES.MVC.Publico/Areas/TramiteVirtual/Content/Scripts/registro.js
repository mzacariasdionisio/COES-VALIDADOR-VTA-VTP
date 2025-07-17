var controlador = siteRoot + "tramitevirtual/registro/";
var uploader;
$(function () {

    $("#btnEnviar").click(function () {

        if ($("#frmRegistro").valid()) {
            registrar();
        }
        else {
            showMensaje('alert', 'Por favor complete los datos indicados.');
        }
    });

    $('#btnCancelar').on('click', function () {
        document.location.href = controlador;
    });


    jQuery.extend(jQuery.validator.messages, {
        required: "Ingrese el dato solicitado.",
        email: "Ingrese un correo válido."
});

    $("#frmRegistro").validate({
        rules: {
            NumeroRUC: { required: true, maxlength: 11 },
            RazonSocial: { required: true, maxlength: 100 },
            NombreComercial: { required: false, maxlength: 200 },
            DireccionFiscal: { required: false, maxlength: 200 },

            NombreRepresentante: { required: true, maxlength: 100 },
            ApellidoRepresentante: { required: true, maxlength: 100 },
            CargorRepresentante: { required: true, maxlength: 100 },
            CorreoRepresentante: { required: true, maxlength: 100, email: true },
            TelefonoRepresentante: { required: true, maxlength: 20 },
            MovilRepresentante: { required: true, maxlength: 20 },

            NombreContacto: { required: true, maxlength: 100 },
            ApellidoContacto: { required: true, maxlength: 100 },
            CargoContacto: { required: true, maxlength: 100 },
            CorreoContacto: { required: true, maxlength: 100, email: true },
            TelefonoContacto: { required: true, maxlength: 20 },
            MovilContacto: { required: true, maxlength: 20 },

            NombreContacto1: { required: false, maxlength: 100 },
            ApellidoContacto1: { required: false, maxlength: 100 },
            CargoContacto1: { required: false, maxlength: 100 },
            CorreoContacto1: { required: false, maxlength: 100, email: true },
            TelefonoContacto1: { required: false, maxlength: 20 },
            MovilContacto1: { required: false, maxlength: 20 },

            NombreContacto2: { required: false, maxlength: 100 },
            ApellidoContacto2: { required: false, maxlength: 100 },
            CargoContacto2: { required: false, maxlength: 100 },
            CorreoContacto2: { required: false, maxlength: 100, email: true },
            TelefonoContacto2: { required: false, maxlength: 20 },
            MovilContacto2: { required: false, maxlength: 20 }
        },
        messages: {
            NumeroRUC: {
                required: "Ingrese número de RUC",
                maxlength: "Máximo 11 caracteres"
            },
            RazonSocial: {
                required: "Ingrese razón social",
                maxlength: "Máximo 100 caracteres"
            }
        }
    });

    

    $('#txtNroRUC').change(function () {
        obtenerDatos();
    });

    $('#btnAgregarContacto2').on('click', function () {
        $('#divPersonaContacto2').show();
        $('#btnAgregarContacto2').hide();
        $('#hfContacto2').val("S");
    });

    $('#btnAgregarContacto3').on('click', function () {
        $('#divPersonaContacto3').show();
        $('#btnAgregarContacto3').hide();
        $('#hfContacto3').val("S");
    });

    $('#btnQuitarContacto2').on('click', function () {
        $('#divPersonaContacto2').hide();
        $('#btnAgregarContacto2').show();
        $('#hfContacto2').val("N");
        $('#txtContactoCorreo1').val("");
    });

    $('#btnQuitarContacto3').on('click', function () {
        $('#divPersonaContacto3').hide();
        $('#btnAgregarContacto3').show();
        $('#hfContacto3').val("N");
        $('#txtContactoCorreo1').val("");
    });
});

registrar = function () {   //enviar todos los datos del formulario al server

    var mensaje = validarEntrada();

    if (mensaje == "") {
        if ($("#cbCondiciones").is(':checked') == true && $("#cbTerminos").is(':checked') ==true) {
            $.ajax({
                type: "post",
                url: controlador + "grabardatos",
                data: $('#frmRegistro').serialize(),
                datatype: 'Json',
                success: function (result) {
                    if (result == 1) {
                        $('#cntResultado').show();
                        $('#formulario').hide();
                    } else if (result == -2) {
                        alert("Por favor validar el Número de RUC ingresado");
                    }
                    else {
                        showMensaje('error', 'Ha ocurrido un error al enviar los datos.');
                    }
                    },
                error: function () {

                }
            });
        }
        else
        {
            if ($("#cbCondiciones").is(':checked') == true) {
                showMensaje('alert', 'Por favor acepte los Terminos y condiciones de la Plataforma Virtual de COES');        
            }
            else if ($("#cbTerminos").is(':checked') == true) {
                showMensaje('alert', 'Por favor acepte las condiciones de tratamiento de datos');
            }
            else {
                showMensaje('alert', 'Por favor acepte los Terminos y condiciones');
            }
         }
           
    }
    else {
        showMensaje('alert', mensaje);
    }
};

validarEntrada = function () {

    var html = "<ul>";
    var flag = false;

    var ruc = $('#txtNroRUC').val();
    var isNumber = /^\d+$/.test(ruc);


    if (ruc.length != 11 || !isNumber) {
        alert("Por favor validar el Número de RUC ingresado");
        html = html + "<li>Por favor validar el Número de RUC ingresado</li>";
            flag = true;
    }

    if ($('#hfContacto2').val() == "S") {
        if ($('#txtContactoCorreo1').val() == "") {
            html = html + "<li>Ingrese el correo del contacto 2</li>";
            flag = true;
        }
        if ($('#txtContactoNombre1').val() == "") {
            html = html + "<li>Ingrese el nombre del contacto 2</li>";
            flag = true;
        }
        if ($('#txtContactoApellido1').val() == "") {
            html = html + "<li>Ingrese el apellido del contacto 2</li>";
            flag = true;
        }
        if ($('#txtContactoCargo1').val() == "") {
            html = html + "<li>Ingrese el cargo del contacto 2</li>";
            flag = true;
        }
        if ($('#txtContactoTelefono1').val() == "") {
            html = html + "<li>Ingrese el teléfono del contacto 2</li>";
            flag = true;
        }
        if ($('#txtContactoMovil1').val() == "") {
            html = html + "<li>Ingrese el móvil del contacto 2</li>";
            flag = true;
        }
    }
    if ($('#hfContacto3').val() == "S") {
        if ($('#txtContactoCorreo2').val() == "") {
            html = html + "<li>Ingrese el correo del contacto 3</li>";
            flag = true;
        }

        if ($('#txtContactoNombre2').val() == "") {
            html = html + "<li>Ingrese el nombre del contacto 3</li>";
            flag = true;
        }
        if ($('#txtContactoApellido2').val() == "") {
            html = html + "<li>Ingrese el apellido del contacto 3</li>";
            flag = true;
        }
        if ($('#txtContactoCargo2').val() == "") {
            html = html + "<li>Ingrese el cargo del contacto 3</li>";
            flag = true;
        }
        if ($('#txtContactoTelefono2').val() == "") {
            html = html + "<li>Ingrese el teléfono del contacto 3</li>";
            flag = true;
        }
        if ($('#txtContactoMovil2').val() == "") {
            html = html + "<li>Ingrese el móvil del contacto 3</li>";
            flag = true;
        }
    }
    html = html + "</ul>";

    if (!flag) {
        html = "";
    }

    return html;
};

obtenerDatos = function () {

    var ruc = $('#txtNroRUC').val();
    var isNumber = /^\d+$/.test(ruc);
    $('#txtNombreComercial').val("");
    $('#txtRazonSocial').val("");
    $('#txtDireccionFiscal').val("");

    if (ruc.length == 11 && isNumber) {
        $('#loadEmpresa').show();
        $.ajax({
            type: 'POST',
            url: controlador + 'obtenerdatos',
            data: {
                ruc: ruc
            },
            dataType: 'json',
            global: true,
            success: function (result) {
              
                if (result.Resultado == 1) {
                    $('#txtNombreComercial').val(result.Entidad.NombreComercial);
                    $('#txtRazonSocial').val(result.Entidad.RazonSocial);
                    $('#txtDireccionFiscal').val(result.Entidad.DomicilioLegal);
                    if (result.Entidad.NombreComercial != "") {
                        if (result.Entidad.NombreComercial.trim() != "") {
                            $('#txtNombreComercial').val($('#txtRazonSocial').val());
                        }
                    }
                }
                else if (result.Resultado == 2) {
                    showMensajeEdit('alert', 'La empresa ya cuenta con acceso al Portal de Trámite Virtual.');
                    showMensaje('alert', 'La empresa ya cuenta con acceso al Portal de Trámite Virtual.');
                    $('#btnEnviar').hide();
                    
                }
                else if (result.Resultado == 3) {
                    showMensajeEdit('alert', 'La empresa no se encuentra registrada en SUNAT.');
                    $("#frmRegistro").children(':input').attr('disabled', 'disabled');                    
                }
                else if (result.Resultado == 4) {
                    showMensajeEdit('alert', 'No se puede establecer conexión con SUNAT, por favor ingrese la Razón Social de la Empresa.');
                    //$("#frmRegistro").children(':input').attr('disabled', 'disabled');
                }
                else if (result.Resultado == 5) {
                    showMensajeEdit('alert', 'El RUC ingresado no es válido.');
                    $("#frmRegistro").children(':input').attr('disabled', 'disabled');   
                }
                else if (result.Resultado == 6) {
                    $('#txtNombreComercial').val(result.Entidad.NombreComercial);
                    $('#txtRazonSocial').val(result.Entidad.RazonSocial);
                    $('#txtDireccionFiscal').val(result.Entidad.DomicilioLegal);
                    if (result.Entidad.NombreComercial != "") {
                        if (result.Entidad.NombreComercial.trim() != "") {
                            $('#txtNombreComercial').val($('#txtRazonSocial').val());
                        }
                    }
                }
                else if (result.Resultado == -1) {
                    showMensajeEdit('error', 'Error al intentar consultar el RUC.');                   
                }
            },
            error: function () {
                
                showMensajeEdit('error', 'Error al intentar consultar el RUC');
            }
        });
        $('#loadEmpresa').hide();
    }
    else {
        showMensajeEdit('alert', 'El RUC debe contener 11 dígitos.');
    }
}

openCondiciones = function () {

    $('#popupEdicion').bPopup({
        easing: 'easeOutBack',
        speed: 450,
        transition: 'slideDown',
        modalClose: false
    });
};

openTerrminos = function () {

    $('#popupEdicionTermino').bPopup({
        easing: 'easeOutBack',
        speed: 450,
        transition: 'slideDown',
        modalClose: false
    });
};

showMensaje = function (action, mensaje) {

    $('#mensaje').removeClass();
    $('#mensaje').addClass('action-' + action);
    $('#mensaje').html(mensaje);
}


showMensajeEdit = function (action, mensaje) {

    $('#mensajeRegistro').removeClass();
    $('#mensajeRegistro').addClass('action-' + action);
    $('#mensajeRegistro').html(mensaje);
}