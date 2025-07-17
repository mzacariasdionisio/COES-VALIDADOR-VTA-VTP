var controlador = siteRoot + 'hidrologia/'

$(function () {
    $('#btnBuscar').click(function () {
        buscarVerificacion();
    });

    $('#btnNuevo').click(function () {
        nuevoVerificacionFormato();
    });

    $('#btnRegresar').click(function () {
        regresar();
    });

    buscarVerificacion();

    $(window).resize(function () {
        $('#listVerificacion').css("width", $('#mainLayout').width() + "px");
    });
});

function regresar() {
    var codigoApp = parseInt($("#hdCodigoApp").val()) || 0;
    document.location.href = controlador + "formatomedicion/Index?app=" + codigoApp;
}

function buscarVerificacion() {
    mostrarListado();
}

function mostrarListado() {
    $("#mensaje").hide();
    var idFormato = $("#hfFormato").val();

    $.ajax({
        type: 'POST',
        url: controlador + "formatomedicion/ListaVerificacionFormato",
        data: {
            idFormato: idFormato
        },
        success: function (evt) {
            $('#listVerificacion').css("width", $('#mainLayout').width() + "px");

            $('#listVerificacion').html(evt);
            $('#tabla').dataTable({
                bJQueryUI: true,
                "scrollY": 550,
                "scrollX": false,
                "sDom": 't',
                "ordering": true,
                "iDisplayLength": 200
            });
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function mostrarDetalle(idFormato, idVerificacion) {
    $("#mensaje").hide();

    $.ajax({
        type: 'POST',
        url: controlador + "formatomedicion/MostrarVerificacionFormato",
        data: {
            idFormato: idFormato,
            idVerif: idVerificacion
        },
        success: function (evt) {
            $('#verVerificacionFormato').html(evt);
            $('#mensaje').css("display", "none");

            setTimeout(function () {
                $('#popupVerVerificacionFormato').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown'
                });
            }, 50);
        },
        error: function () {
            mostrarError();
        }
    });
}

function editarVerificacionFormato(idFormato, idVerificacion) {
    $.ajax({
        type: 'POST',
        url: controlador + "formatomedicion/EditarVerificacionFormato",
        data: {
            idFormato: idFormato,
            idVerif: idVerificacion
        },
        success: function (evt) {
            $('#editarVerificacionFormato').html(evt);
            $('#mensaje').css("display", "none");

            setTimeout(function () {
                $('#popupEditarVerificacionFormato').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown'
                });
            }, 50);
        },
        error: function () {
            mostrarError();
        }
    });
}

function nuevoVerificacionFormato() {
    var idFormato = $("#hfFormato").val();
    $.ajax({
        type: 'POST',
        url: controlador + "formatomedicion/AgregarVerificacionFormato",
        data: {
            idFormato: idFormato
        },
        success: function (evt) {
            $('#nuevaVerificacionFormato').html(evt);
            $('#mensaje').css("display", "none");

            setTimeout(function () {
                $('#popupNuevaVerificacionFormato').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown'
                });
            }, 50);
        },
        error: function () {
            mostrarError();
        }
    });
}

function registrarVerificacionFormato() {
    if (confirm('¿Está seguro que desea guardar la verificación de formato?')) {
        if (validarRegistro()) {
            $.ajax({
                type: 'POST',
                url: controlador + 'formatomedicion/RegistrarVerificacionFormato',
                dataType: 'json',
                data: $('#frmAgregarVerificacionFormato').serialize(),
                cache: false,
                success: function (resultado) {
                    if (resultado == 1) {
                        mostrarExitoOperacion();
                        $('#popupNuevaVerificacionFormato').bPopup().close();
                        mostrarListado();
                    } else {
                        mostrarError(resultado);
                    }
                },
                error: function () {
                    mostrarError();
                }
            });
        }
    }
}

function validarRegistro() {
    var verificacion = $("#frmAgregarVerificacionFormato #cbVerificacion").val();
    if (verificacion == null || verificacion == '-3') {
        alert("Seleccione una verificación");
        return false;
    }
    return true;
}

function actualizarVerificacionFormato() {
    if (confirm('¿Está seguro que desea actualizar la Verificación de formato?')) {
        $.ajax({
            type: 'POST',
            url: controlador + 'formatomedicion/ActualizarVerificacionFormato',
            dataType: 'json',
            data: $('#frmEditarVerificacionFormato').serialize(),
            cache: false,
            success: function (resultado) {
                if (resultado == 1) {
                    mostrarExitoOperacion();
                    $('#popupEditarVerificacionFormato').bPopup().close();
                    mostrarListado();
                } else
                    mostrarError(resultado);
            },
            error: function () {
                mostrarError();
            }
        });
    }
}

mostrarExitoOperacion = function () {
    $('#mensaje').removeClass("action-error");
    $('#mensaje').addClass("action-exito");
    $('#mensaje').text("La operación se realizó con exito...!");
    $('#mensaje').css("display", "block");
};

mostrarError = function (msj) {
    var mensaje = "Ha ocurrido un error";
    if (msj != undefined && msj != null && msj != -1) {
        mensaje = msj;
    }

    $('#mensaje').removeClass("action-exito");
    $('#mensaje').addClass("action-error");
    $('#mensaje').text(mensaje);
    $('#mensaje').css("display", "block");
};
