var controlador = siteRoot + 'costooportunidad/admin/';

$(function () {

    $('#btnConsultar').on('click', function () {
        consultar();
    });

    $('#btnNuevo').on('click', function () {
        editar(0);
    });   

    consultar();
});

consultar = function () {
    $.ajax({
        type: 'POST',
        url: controlador + 'periodolist',
        data: {
            anio: $('#cbAnioFiltro').val()
        },
        success: function (evt) {
            $('#listado').html(evt);
            $('#tabla').dataTable({
                "iDisplayLength": 25
            });
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
};

editar = function (id) {

    $.ajax({
        type: 'POST',
        url: controlador + 'periodoedit',
        data: {
            idPeriodo: id
        },
        success: function (evt) {
            $('#contenidoEdicion').html(evt);
            setTimeout(function () {
                $('#popupEdicion').bPopup({
                    autoClose: false
                });
            }, 50);

            $('#cbAnio').val($('#hfAnio').val());
            $('#cbMes').val($('#hfMes').val());
            $('#cbEstado').val($('#hfEstado').val());

            $('#btnGrabar').on("click", function () {
                grabar();
            });

            $('#btnCancelar').on("click", function () {
                $('#popupEdicion').bPopup().close();
            });

        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        }
    });
};

versiones = function (id) {
    document.location.href = controlador + 'versionindex?id=' + id;
};

consultarEnvios = function (id) {
    $.ajax({
        type: 'POST',
        url: controlador + 'envioliquidacion',
        data: {
            idPeriodo: id
        },
        success: function (evt) {
            $('#contenidoEnvioLiquidacion').html(evt);

            setTimeout(function () {
                $('#popupEnvioLiquidacion').bPopup({
                    autoClose: false
                });
            }, 50);

        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
};

verBandas = function (id) {
    document.location.href = controlador + 'bandas?id=' + id;
};

eliminar = function (id) {
    if (confirm('¿Está seguro de realizar esta operación?')) {

        $.ajax({
            type: 'POST',
            url: controlador + 'periododelete',
            data: {
                idPeriodo: id
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
};

verReprogramas = function(id){
    $.ajax({
        type: 'POST',
        url: controlador + "exportar",
        data: {
            idPeriodo: id           
        },
        dataType: 'json',
        cache: false,
        success: function (resultado) {
            if (resultado == 1) {
                location.href = controlador + "descargar";
            }
            else {
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
            }
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
};

grabar = function () {
    var validacion = validar();

    if (validacion == "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'periodosave',
            data: $('#frmRegistro').serialize(),
            dataType: 'json',
            global: false,
            success: function (result) {
                if (result == 1) {
                    mostrarMensaje('mensaje', 'exito', 'Los datos se grabaron correctamente.');
                    $('#popupEdicion').bPopup().close();
                    consultar();
                }
                else if (result == 2) {
                    mostrarMensaje('mensajeEdicion', 'alert', 'El periodo ya se encuentra registrado.');
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

validar = function () {

    var mensaje = "<ul>";
    var flag = true;

    if ($('#cbAnio').val() == "") {
        mensaje = mensaje + "<li>Seleccione el año.</li>";
        flag = false;
    }

    if ($('#cbMes').val() == "") {
        mensaje = mensaje + "<li>Seleccione el mes.</li>";
        flag = false;
    }

    if ($('#txtDescripcion').val() == "") {
        mensaje = mensaje + "<li>Ingrese una descripción.</li>";
        flag = false;
    }

    if ($('#cbEstado').val() == "") {
        mensaje = mensaje + "<li>Seleccione el estado</li>";
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