var controlador = siteRoot + 'admin/proveedor/';

$(function () {

    $('#btnConsultar').on('click', function () {
        consultar();
    });

    consultar();
});

consultar = function () {
    $.ajax({
        type: 'POST',
        url: controlador + 'empresas',
        data: {
            tipoEmpresa: $('#cbTipoEmpresa').val()
        },
        success: function (evt) {
            $('#listado').css("width", $('#mainLayout').width() + "px");
            $('#listado').html(evt);
            $('#tabla').dataTable({
            });
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });   
};

verCorreo = function (id) {
    $.ajax({
        type: 'POST',
        url: controlador + 'correos',
        data: {
            idEmpresa: id
        },
        success: function (evt) {
            $('#contenidoEdicion').html(evt);
            setTimeout(function () {
                $('#popupEdicion').bPopup({
                    autoClose: false
                });
            }, 50);

            $('#btnNuevoCorreo').on('click', function () {
                editarCorreo(0);
            });

            $('#btnGrabarCorreo').on('click', function () {
                grabarCorreo();
            });

            $('#registroCorreo').hide();

            listarCorreos(id);
        },
        error: function () {

        }
    });
};

editarCorreo = function (id) {

    $.ajax({
        type: 'POST',
        url: controlador + 'obtenercorreo',
        data: {
            idEmpresaCorreo: id
        },
        global:false,
        dataType: 'json',
        success: function (result) {
            $('#txtCorreo').val(result);
            $('#registroCorreo').show();
            $('#hfCodigoCorreo').val(id);

            if (id == 0) {
                $('#textoCorreo').text("Ingrese correo:");
            }
            else {
                $('#textoCorreo').text("Modifique correo:");
            }
        },
        error: function () {
        }
    });  

};

listarCorreos = function (id) {

    $.ajax({
        type: 'POST',
        url: controlador + 'listado',
        data: {
            idEmpresa: id
        },     
        global: false,
        success: function (eve) {
            $('#listadoCorreo').html(eve);
        },
        error: function () {
        }
    });

};

eliminarCorreo = function (id) {

    if (confirm('¿Está seguro de realizar esta acción?')) {
        $.ajax({
            type: 'POST',
            url: controlador + 'eliminarcorreo',
            data: {
                idEmpresaCorreo: id
            },
            global: false,
            dataType: 'json',
            success: function (result) {
                if (result == 1) {
                    listarCorreos($('#hfCodigoEmpresa').val());
                    mostrarMensaje('mensajeCorreo', 'exito', 'Operación realizada correctamente');
                }
                else {
                    mostrarMensaje('mensajeCorreo', 'error', 'Ha ocurrido un error.');
                }
            },
            error: function () {
               
            }
        });
    }
};

grabarCorreo = function () {

    if (validarEmail($('#txtCorreo').val())) {
        $.ajax({
            type: 'POST',
            url: controlador + 'grabarcorreo',
            data: {
                idEmpresa: $('#hfCodigoEmpresa').val(),
                idEmpresaCorreo: $('#hfCodigoCorreo').val(),
                email: $('#txtCorreo').val()
            },
            dataType: 'json',
            global: false,
            success: function (result) {
                $('#registroCorreo').hide();

                if (result == 1) {
                    listarCorreos($('#hfCodigoEmpresa').val());
                    mostrarMensaje('mensajeCorreo', 'exito', 'Operación realizada correctamente');
                }
                else {
                    mostrarMensaje('mensajeCorreo', 'error', 'Ha ocurrido un error.');
                }
            },
            error: function () {
                mostrarMensaje('mensajeCorreo', 'error', 'Ha ocurrido un error.');
            }
        });
    }
    else {
        mostrarMensaje('mensajeCorreo', 'alert', 'Ingrese correo válido');
    }
};

enviarCredenciales = function (id) {
    if (confirm('Se va a generar una nueva clave para el acceso al Portal de Proveedores y se enviará esta a los correos electrónicos de los usuarios registrados de la empresa.¿Está seguro ?')) {
        $.ajax({
            type: 'POST',
            url: controlador + 'crearcredenciales',
            data: {
                idEmpresa: id
            },
            dataType: 'json',
            global: false,
            success: function (result) {               
                if (result == 1) {                   
                    mostrarMensaje('mensaje', 'exito', 'Operación realizada correctamente');
                }
                else if (result == -2) {
                    mostrarMensaje('mensaje', 'alert', 'No se puede crear la cuenta ya que no existe cuenta de correo.');
                }
                else {
                    mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
                }
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'No se pudo desencriptar.');
            }
        });
    }
};

validarEmail = function (email) {

    if (email == "") {
        return false;
    }
    else {
        var regex = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
        return regex.test(email);
    }
}

mostrarMensaje = function (id, tipo, mensaje) {
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);
}
