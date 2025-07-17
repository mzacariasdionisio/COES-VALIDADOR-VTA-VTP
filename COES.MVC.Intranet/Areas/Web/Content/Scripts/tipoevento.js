var controlador = siteRoot + 'web/calendario/';

$(function () {

    $('#btnNuevo').on('click', function () {
        editar(0);
    });

    $('#btnCancelar').on('click', function () {
        document.location.href = controlador + 'index';
    });

    $('#cbAnioSearch').val($('#hfAnioSerach').val());

    consultar();
});


consultar = function () {
    $.ajax({
        type: 'POST',
        url: controlador + 'tipolistado',      
        success: function (evt) {
            $('#listado').html(evt);
            $('#tabla').dataTable({

            });
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
}

editar = function (id) {

    $.ajax({
        type: 'POST',
        url: controlador + 'tipoeditar',
        data: {
            id: id
        },
        global: false,
        success: function (evt) {
            $('#contenidoEdicion').html(evt);
            setTimeout(function () {
                $('#popupEdicion').bPopup({
                    autoClose: false,
                    follow: [true, true],
                });
            }, 50);

            $('#btnGrabar').on("click", function () {
                grabar();
            });

            $('#btnCancelarEdicion').on("click", function () {
                $('#popupEdicion').bPopup().close();
            });

                        
            $('#txtColor').colorpicker({
                showOn: 'focus',
                displayIndicator: false,
                history: false
            });

            $("input:radio[name='Icono'][value='" + $('#hfIcono').val() + "']").prop('checked', true);   

        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        }
    });
}

eliminar = function (id) {
    if (confirm('¿Está seguro de realizar esta operación?')) {

        $.ajax({
            type: 'POST',
            url: controlador + 'tipoeliminar',
            data: {
                id: id
            },
            dataType: 'json',
            success: function (result) {
                if (result == 1) {
                    mostrarMensaje('mensaje', 'exito', 'El registro se eliminó correctamente.');
                    consultar();
                }
                else {
                    mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
                }
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            }
        });
    }
}

grabar = function () {
    var validacion = validarRegistro();

    if (validacion == "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'tipograbar',
            data: $('#frmRegistro').serialize(),
            dataType: 'json',
            global: false,
            success: function (result) {
                if (result > 0) {
                    mostrarMensaje('mensaje', 'exito', 'Los datos se grabaron correctamente.');
                    $('#popupEdicion').bPopup().close();
                    consultar();
                }
                else {
                    mostrarMensaje('mensajeEdicion', 'error', 'Se ha producido un error.');
                }
            },
            error: function () {
                mostrarMensaje('mensajeEdicion', 'error', 'Se ha producido un error.');
            }
        });
    }
    else {
        mostrarMensaje('mensajeEdicion', 'alert', validacion);
    }
}

validarRegistro = function () {

    var mensaje = "<ul>";
    var flag = true;

    if ($('#txtDescripcion').val() == "") {
        mensaje = mensaje + "<li>Ingrese la descripción del Tipo de Evento.</li>";
        flag = false;
    }

    if ($('#txtColor').val() == "") {
        mensaje = mensaje + "<li>Seleccione color.</li>";
        flag = false;
    }

    if ($("input:radio[name='Icono']").val() == "") {
        mensaje = mensaje + "<li>Seleccione ícono</li>";
        flag = false;
    }

    mensaje = mensaje + "</ul>";

    if (flag) mensaje = "";

    return mensaje;
}

mostrarMensaje = function (id, tipo, mensaje) {
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);
}
