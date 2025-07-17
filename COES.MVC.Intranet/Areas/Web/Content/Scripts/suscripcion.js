var controlador = siteRoot + 'web/suscripcion/';

$(function () {

    $('#btnConsultar').on('click', function () {
        consultar();
    });

    $('#txtFechaInicio').Zebra_DatePicker({
        pair: $('#txtFechaFin'),
        onSelect: function (date) {
            var date1 = getFecha(date);
            var date2 = getFecha($('#txtFechaFin').val());
            if (date1 > date2) {
                $('#txtFechaFin').val(date);
            }
        }
    });

    $('#txtFechaFin').Zebra_DatePicker({
        direction: true
    });


    $('#btnPublicacion').on('click', function () {
        document.location.href = controlador + "publicacion";
    });

    $('#btnExportar').on('click', function () {
        exportar();
    });

    consultar();
});


exportar = function () {
    $.ajax({
        type: 'POST',
        url: controlador + "exportar",
        data: {
            fechaInicio: $('#txtFechaInicio').val(),
            fechaFin: $('#txtFechaFin').val(),
            idPublicacion: $('#cbPublicacionFiltro').val()
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
}

consultar = function () {
    $.ajax({
        type: 'POST',
        url: controlador + 'listar',
        data: {
            fechaInicio: $('#txtFechaInicio').val(),
            fechaFin: $('#txtFechaFin').val(),
            idPublicacion: $('#cbPublicacionFiltro').val()
        },
        success: function (evt) {
            $('#listado').html(evt);
            $('#tabla').dataTable({
                "iDisplayLength": 20
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
            idSubscripcion: id
        },
        global: false,
        success: function (evt) {
            $('#contenidoEdicion').html(evt);
            setTimeout(function () {
                $('#popupEdicion').bPopup({
                    autoClose: false
                });
            }, 50);

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
}

eliminar = function (id) {
    if (confirm('¿Está seguro de realizar esta operación?')) {

        $.ajax({
            type: 'POST',
            url: controlador + 'eliminar',
            data: {
                idSubscripcion: id
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

        var publicaciones = "";
        $('#publicacion-content input:checked').each(function () {
            publicaciones = publicaciones + $(this).val() + ",";
        });

        $('#hfDetalle').val(publicaciones);

        $.ajax({
            type: 'POST',
            url: controlador + 'grabar',
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
    var contador = 0;

    $('#publicacion-content input:checked').each(function () {
        contador = 1;
    });

    if (contador == 0) {
        mensaje = mensaje + "<li>Seleccione al menos una publicación.</li>";
        flag = false;
    }

    if ($('#txtNombre').val() == "") {
        mensaje = mensaje + "<li>Ingrese su nombre.</li>";
        flag = false;
    }

    if ($('#txtApellido').val() == "") {
        mensaje = mensaje + "<li>Ingrese su apellido.</li>";
        flag = false;
    }

    if ($('#txtCorreo').val() == "") {
        mensaje = mensaje + "<li>Ingrese correo.</li>";
        flag = false;
    }
    else if (!validarEmail($('#txtCorreo').val())) {
        mensaje = mensaje + "<li>Ingrese correo válido.</li>";
        flag = false;
    }

    if ($('#txtTelefono').val() == "") {
        mensaje = mensaje + "<li>Ingrese teléfono.</li>";
        flag = false;
    }

    if ($('#txtEmpresa').val() == "") {
        mensaje = mensaje + "<li>Ingrese nombre de la empresa.</li>";
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

validarNumero = function (item, evt) {
    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;

    if (charCode == 46) {
        var regex = new RegExp(/\./g)
        var count = $(item).val().match(regex).length;
        if (count > 1) {
            return false;
        }
    }

    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }

    return true;
}

getFecha = function (date) {
    var parts = date.split("/");
    var date = new Date(parts[1] + "/" + parts[0] + "/" + parts[2]);
    return date.getTime();
}