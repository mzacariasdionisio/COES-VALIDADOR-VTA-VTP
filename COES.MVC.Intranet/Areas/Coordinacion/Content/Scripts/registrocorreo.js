/*-------------------------------------------------------------------
Clase: notificacion.js
Descripción: Contiene la funcionalidad de la pantalla de notificacion
Fecha creación: 19-08-2016
Autor: Raúl Castro
Versión: 1.0
-------------------------------------------------------------------*/

var controlador = siteRoot + 'coordinacion/registrocorreo/';

$(function () {      

    $('#btnCargarCuenta').on("click", function () {
        cargarCuentas();
    });

    $('#btnNuevaCuenta').on("click", function () {
        editarCuenta(0);
    });

    $('#cbEmpresa').on("change", function () {
        cargarCuentas();
    });

    cargarCuentas();  

});

cargarCuentas = function () {
    $.ajax({
        type: 'POST',
        url: controlador + 'listacuentas',
        data: {          
            empresa: $('#cbEmpresa').val()
        },
        success: function (evt) {
            $('#listaCuenta').html(evt);
            $('#tablaCuenta').dataTable({
            });
        },
        error: function () {
            mostrarError('mensaje');
        }
    });
}

editarCuenta = function (id) {
    $.ajax({
        type: 'POST',
        url: controlador + 'addcuenta',
        data: {
            idCuenta: id
        },
        success: function (evt) {
            $('#contenidoCuenta').html(evt);
            setTimeout(function () {
                $('#popupCuenta').bPopup({
                    autoClose: false
                });
            }, 50);

            $('#cbEmpresaCuenta').val($('#hfEmpresaCuenta').val());
            $('#cbEstadoCuenta').val($('#hfEstadoCuenta').val());

            $('#btnGrabarCuenta').on("click", function () {
                grabarCuenta();
            });
            $('#btnCancelarCuenta').on("click", function () {
                $('#popupCuenta').bPopup().close();
            });
        },
        error: function () {
            mostrarError('mensaje');
        }
    });
}

eliminarCuenta = function (id) {
    if (confirm('¿Está seguro de eliminar esta cuenta?')) {

        $.ajax({
            type: 'POST',
            url: controlador + 'eliminarcuenta',
            data: {
                idCuenta: id
            },
            dataType: 'json',
            success: function (result) {
                if (result == 1) {
                    cargarCuentas();
                    mostrarExito('La cuenta ha sido eliminada...!', 'mensajeListCuenta');
                }
                else {
                    mostrarError('mensajeListCuenta');
                }
            },
            error: function () {
                mostrarError('mensajeListCuenta');
            }
        });
    }
}

grabarCuenta = function () {
    var mensaje = validarCuenta();

    if (mensaje == "") {

        $.ajax({
            type: 'POST',
            url: controlador + 'grabarcuenta',
            data: {
                idCuenta: $('#hfIdCuenta').val(),
                emprcodi: $('#cbEmpresaCuenta').val(),
                nombre: $('#txtNombreCuenta').val(),
                email: $('#txtCorreoCuenta').val(),                
                estado: $('#cbEstadoCuenta').val()
            },
            dataType: 'json',
            success: function (result) {
                if (result > 0) {
                    cargarCuentas();
                    $('#popupCuenta').bPopup().close();
                    mostrarExito('Los datos se grabaron correctamente.', 'mensajeListCuenta');
                }
                else {
                    mostrarError('mensajeCuenta');
                }
            },
            error: function () {
                mostrarError('mensajeCuenta');
            }
        });
    }
    else {
        mostrarAlerta(mensaje, 'mensajeCuenta')
    }
}

validarCuenta = function () {
    var flag = true;
    var mensaje = "<ul>";

    if ($('#cbEmpresaCuenta').val() == '-1') {
        mensaje = mensaje + "<li>Seleccione empresa.</li>";
        flag = false;
    }

    if ($('#txtNombreCuenta').val() == '') {
        mensaje = mensaje + "<li>Ingrese nombre de la cuenta.</li>";
        flag = false;
    }

    if ($('#txtCorreoCuenta').val() == '') {
        mensaje = mensaje + "<li>Ingrese correo.</li>";
        flag = false;
    }
    else if (!validarEmail($('#txtCorreoCuenta').val())) {
        mensaje = mensaje + "<li>Ingrese un correo válido.</li>";
        flag = false;
    }

    if ($('#cbEstadoCuenta').val() == '') {
        mensaje = mensaje + "<li>Seleccione estado.</li>";
        flag = false;
    }

    mensaje = mensaje + "</ul>";
    if (flag) mensaje = "";
    return mensaje;
}

validarEmail = function (email) {
    var regex = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    return regex.test(email);
}

mostrarExito = function (mensaje, id) {
    $('#' + id).removeClass();
    $('#' + id).addClass('action-exito');
    $('#' + id).html(mensaje);
}

mostrarError = function (id) {
    $('#' + id).removeClass();
    $('#' + id).addClass('action-error');
    $('#' + id).html('Ha ocurrido un error.');
}

mostrarAlerta = function (mensaje, id) {
    $('#' + id).removeClass();
    $('#' + id).addClass('action-alert');
    $('#' + id).html(mensaje);
}