var controlador = siteRoot + 'demandabarras/psee/';

$(function () {
    
    $('#btnConsultar').on('click', function () {
        consultar();
    });

    $('#btnNuevo').on('click', function () {
        editarRelacion(0);
    });

    consultar();

});

consultar = function () {

    $.ajax({
        type: 'POST',
        url: controlador + 'relacionlist',
        data: {
            idEmpresa: $('#cbEmpresa').val(),
            estado: $('#cbEstado').val()
        },
        success: function (evt) {
            $('#listado').html(evt);
            $('#tablaRelacion').dataTable({
                "iDisplayLength": 20
            });
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
}

cargarEquipos = function () {

    var idEmpresa = $('#cbEmpresaEdit').val();
    var idFamilia = $('#cbFamiliaEdit').val();
    $('option', '#cbEquipoEdit').remove();

    if (idEmpresa != "-1" && idFamilia != "-1") {
        $.ajax({
            type: 'POST',
            url: controlador + 'cargarequipos',
            dataType: 'json',
            data: {
                emprcodi: idEmpresa,
                famcodi: idFamilia
            },
            cache: false,
            global: false,
            success: function (aData) {
                $('#cbEquipoEdit').get(0).options.length = 0;
                $('#cbEquipoEdit').get(0).options[0] = new Option("--SELECCIONE--", "-1");
                $.each(aData, function (i, item) {
                    $('#cbEquipoEdit').get(0).options[$('#cbEquipoEdit').get(0).options.length] = new Option(item.Text, item.Value);
                });
                mostrarMensaje('mensajeRelacion', 'mensaje', 'Por favor complete todos los datos.');
            },
            error: function () {
                mostrarMensaje('mensajeRelacion', 'error', 'Se ha producido un error.');
            }
        });
    }
    else {
        mostrarMensaje('mensajeRelacion', 'alert', 'Seleccione empresa y tipo de equipo.');
    }
}

editarRelacion = function (id) {

    $.ajax({
        type: 'POST',
        url: controlador + 'relacionedit',
        data: {
            idRelacion: id
        },
        global: false,
        success: function (evt) {
            $('#contenidoRelacion').html(evt);
            setTimeout(function () {
                $('#popupRelacion').bPopup({
                    autoClose: false
                });
            }, 50);

            $('#cbEmpresaEdit').val($('#hfEmpresa').val());
            $('#cbEquipoEdit').val($('#hfEquipo').val());
            $('#cbEstadoEdit').val($('#hfEstado').val());
            $('#cbFamiliaEdit').val($('#hfFamilia').val());

            $('#btnGrabarRelacion').on("click", function () {
                grabarRelacion();
            });

            $('#btnCancelarRelacion').on("click", function () {
                $('#popupRelacion').bPopup().close();
            });

            $('#cbEmpresaEdit').on("change", function () {
                cargarEquipos();
            });

            $('#cbFamiliaEdit').on("change", function () {
                cargarEquipos();
            });

            if (id != 0) {
                $('#cbEmpresaEdit').css('pointer-events', 'none');
                $('#cbEquipoEdit').css('pointer-events', 'none');
            }
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        }
    });
}

eliminarRelacion = function (id) {
    if (confirm('¿Está seguro de realizar esta operación?')) {

        $.ajax({
            type: 'POST',
            url: controlador + 'relaciondelete',
            data: {
                idRelacion: id
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

grabarRelacion = function () {
    var validacion = validarRegistro();

    if (validacion == "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'relacionsave',
            data: $('#frmRegistro').serialize(),
            dataType: 'json',
            global: false,
            success: function (result) {
                if (result == 1) {
                    mostrarMensaje('mensaje', 'exito', 'Los datos se grabaron correctamente.');
                    $('#popupRelacion').bPopup().close();
                    consultar();

                }
                else if (result == 2) {
                    mostrarMensaje('mensajeRelacion', 'alert', 'La equivalencia del equipo ya se encuentra registrada.');
                }
                else {
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

    if ($('#cbEquipoEdit').val() == "") {
        mensaje = mensaje + "<li>Seleccione el equipo SGOCOES.</li>";
        flag = false;
    }

    if ($('#txtNomBarra').val() == "") {
        mensaje = mensaje + "<li>Ingrese el nombre de barra.</li>";
        flag = false;
    }

    if ($('#txtIdGenerador').val() == "") {
        mensaje = mensaje + "<li>Ingrese el ID del generador.</li>";
        flag = false;
    }

    if ($('#txtCodigoNCP').val() == "") {
        mensaje = mensaje + "<li>Ingrese el código NCP</li>";
        flag = false;
    }

    if ($('#txtNombreNCP').val() == "") {
        mensaje = mensaje + "<li>Ingrese el nombre NCP</li>";
        flag = false;
    }

    if ($('#cbEstadoEdit').val() == "") {
        mensaje = mensaje + "<li>Seleccione el estado.</li>";
        flag = false;
    }

    mensaje = mensaje + "</ul>";

    if (flag) {
        mensaje = "";
    }

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