var controlador = siteRoot + 'web/calendario/';

$(function () {

    $('#btnConsultar').on('click', function () {
        consultar();
    });
    
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
        url: controlador + 'infolistado',
        data: {
            anio: $('#cbAnioSearch').val(),
            mes: $('#cbMesSearch').val()
        },
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
        url: controlador + 'infoeditar',
        data: {
            idRegistro: id
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

            $('#cbEstado').val($('#hfEstado').val());
            $('#cbMes').val($('#hfMes').val());
            $('#cbAnio').val($('#hfAnio').val());

          
            $('#txtColorMes').colorpicker({
                showOn: 'focus',
                displayIndicator: false,
                history: false
            });

            $('#txtColorSabado').colorpicker({
                    showOn: 'focus',
                    displayIndicator: false,
                    history: false
            });

            $('#txtColorDomingo').colorpicker({
                showOn: 'focus',
                displayIndicator: false,
                history: false
            });

            $('#txtColorTitulo').colorpicker({
                showOn: 'focus',
                displayIndicator: false,
                history: false
            });
                       
            $('#txtColorDia').colorpicker({
                showOn: 'focus',
                displayIndicator: false,
                history: false
            });

            $('#txtColorSubtitulo').colorpicker({
                showOn: 'focus',
                displayIndicator: false,
                history: false
            });
            
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
            url: controlador + 'infoeliminar',
            data: {
                idRegistro: id
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
            url: controlador + 'infograbar',
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

    if ($('#txtTitulo').val() == "") {
        mensaje = mensaje + "<li>Ingrese el título del Evento.</li>";
        flag = false;
    }

    if ($('#cbAnio').val() == "") {
        mensaje = mensaje + "<li>Seleccione el año.</li>";
        flag = false;
    }

    if ($('#cbMes').val() == "") {
        mensaje = mensaje + "<li>Seleccione mes.</li>";
        flag = false;
    }

    if ($('#cbEstado').val() == "") {
        mensaje = mensaje + "<li>Seleccione el estado.</li>";
        flag = false;
    }

    mensaje = mensaje + "</ul>";

    if (flag) mensaje = "";

    return mensaje;
}

quitarImagen = function (id, archivo) { 
    $.ajax({
        type: 'POST',
        url: controlador + 'quitarimagen',
        data: {
            idRegistro: id,
            file: archivo
        },
        dataType: 'json',
        success: function (result) {
            if (result == 1) {
                mostrarMensaje('mensajeEdicion', 'exito', 'El registro se eliminó correctamente.');
                $('#linkQuitarImagen').hide();
                $('#imgBanner').hide();
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

mostrarMensaje = function (id, tipo, mensaje) {
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);
}
