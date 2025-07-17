var controlador = siteRoot + 'cortoplazo/barra/';

$(function () {

    $('#btnConsultar').on('click', function () {
        consultar();
    });

    $('#btnNuevo').on('click', function () {
        editar(0);
    });

    $('#btnListado').on('click', function () {
        verListado();
    });

    consultar();

});

consultar = function () {

    $.ajax({
        type: 'POST',
        url: controlador + 'listar',
        data: {            
            estado: $('#cbEstado').val(),
            publicacion: $('#cbPublicar').val()
        },
        success: function (evt) {
            $('#listado').html(evt);
            $('#tablaRelacion').dataTable({
                "iDisplayLength": 25
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
        url: controlador + 'editar',
        data: {
            id: id
        },
        global: false,
        success: function (evt) {
            $('#contenidoRelacion').html(evt);
            setTimeout(function () {
                $('#popupRelacion').bPopup({
                    autoClose: false
                });
            }, 50);
                        
            $('#cbPublicarEdit').val($('#hfPublicarEdit').val());
            $('#cbEstadoEdit').val($('#hfEstadoEdit').val());
            $('#cbIndDefecto').val($('#hfIndDefecto').val());
            $('#cbBarraYupana').val($('#hfBarraYupana').val());

            $('#btnGrabar').on("click", function () {
                grabar();
            });

            $('#btnCancelar').on("click", function () {
                $('#popupRelacion').bPopup().close();
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
            url: controlador + 'eliminar',
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
            url: controlador + 'grabar',
            data: $('#frmRegistro').serialize(),
            dataType: 'json',
            global: false,
            success: function (result) {
                if (result == 1) {
                    mostrarMensaje('mensaje', 'exito', 'Los datos se grabaron correctamente.');
                    $('#popupRelacion').bPopup().close();
                    consultar();
                }                
                else if (result == 2){
                    mostrarMensaje('mensajeRelacion', 'alert', 'El código SCADA ingresado no existe en la base de datos.');
                }
                else if (result == 3) {
                    mostrarMensaje('mensajeRelacion', 'alert', 'El código SCADA ingresado ya se encuentra asignado a otra barra.');
                }
                else if (result == 4) {
                    mostrarMensaje('mensajeRelacion', 'alert', 'La barra YUPANA seleccionada ya se encuentra asignado a otra barra.');
                }
                else if (result == -1) {
                    mostrarMensaje('mensajeRelacion', 'error', 'Se ha producido un error.');
                }
            },
            error: function () {
                mostrarMensaje('mensajeRelacion', 'error', 'Se ha producido un error.');
            }
        });
    }
    else {
        mostrarMensaje('mensajeRelacion', 'alert', validacion);
    }
}

validarRegistro = function () {
    var mensaje = "<ul>";
    var flag = true;
    
    if ($('#txtNodoBarra').val() == "") {
        mensaje = mensaje + "<li>Ingrese en nombre de nodo de la barra.</li>";
        flag = false;
    }

    if ($('#txtNombreBarra').val() == "") {
        mensaje = mensaje + "<li>Ingrese el nombre a mostrar en WEB.</li>";
        flag = false;
    }

    if ($('#cbPublicarEdit').val() == "") {
        mensaje = mensaje + "<li>Indicar si se publicará en WEB.</li>";
        flag = false;
    }

    if ($('#cbEstadoEdit').val() == "") {
        mensaje = mensaje + "<li>Seleccionar el estado del registro.</li>";
        flag = false;
    }

    /*if ($('#txtNombreTna').val() == "") {
        mensaje = mensaje + "<li>Ingrese el nombre del identificador TNA.</li>";
        flag = false;
    }*/

    if ($('#txtCodigoScada').val() != "") {

        var patron = /^\d*$/;        
        if (!patron.test($('#txtCodigoScada').val())) {
            mensaje = mensaje + "<li>Ingrese un código SCADA válido</li>";
            flag = false;
        }      
    }

    mensaje = mensaje + "</ul>";

    if (flag) {
        mensaje = "";
    }

    return mensaje;
}

coordenadas = function (id) {

    $('#popupMapa').bPopup({
        content: 'iframe',
        contentContainer: '#contenidoMapa',
        modalClose:false,
        loadUrl: controlador + 'mapa?id=' + id,
        onClose: function () {
            consultar();
        }
    });
};


mostrarMensaje = function (id, tipo, mensaje) {
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);
}