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
        url: controlador + 'versionlist',
        data: {
            idPeriodo: $('#hfIdPeriodo').val()
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
        url: controlador + 'versionedit',
        data: {
            idPeriodo: $('#hfIdPeriodo').val(),
            idVersion: id
        },
        success: function (evt) {
            $('#contenidoEdicion').html(evt);
            setTimeout(function () {
                $('#popupEdicion').bPopup({
                    autoClose: false
                });
            }, 50);

            $('#cbTipoVersion').val($('#hfTipoVersion').val());            
            $('#cbEstado').val($('#hfEstado').val());

            $('#cbTipoVersion').on('change', function () {
                cargarDatosVersion();
            });

            $('#txtFechaInicio').Zebra_DatePicker({
            });

            $('#txtFechaFin').Zebra_DatePicker({
            });

            $('#btnGrabar').on("click", function () {
                grabar();
            });

            $('#btnCancelar').on("click", function () {
                $('#popupEdicion').bPopup().close();
            });

            $('#btnAgregarURS').on('click', function () {
                agregarURS();
            });

        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        }
    });
};

cargarDatosVersion = function () {
    if ($('#cbTipoVersion').val() != "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'obtenerdatosversion',
            data: {
                idPeriodo: $('#hfIdPeriodo').val(),
                idVersionBase: $('#cbTipoVersion').val()
            },
            dataType: 'json',
            success: function (result) {
                $('#txtFechaInicio').val(result.FechaInicio);
                $('#txtFechaFin').val(result.FechaFin);
                $('#txtDescripcion').val(result.Coverdesc);
            },
            error: function () {
                mostrarMensaje('mensajeEdicion', 'error', 'Se ha producido un error.');
            }
        });
    }
};

agregarURS = function () {
    if ($('#cbURS').val() != "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'obtenerurs',
            data: {
                idGrupo: $('#cbURS').val()
            },
            dataType: 'json',
            global:false,
            success: function (result) {
                var count = 0;
                var flag = true;
                $('#tablaURS>tbody tr').each(function (i) {
                    $punto = $(this).find('#hfIdGrupo');
                    if ($punto.val() == result.Grupocodi) {
                        flag = false;
                    }
                });

                if (flag) {
                    $('#tablaURS> tbody').append(
                        '<tr>' +
                        '   <td>' + result.Centralnomb + '</td>' +
                        '   <td>' + result.Gruponomb + '</td>' +
                        '   <td>' + 'Fórmula 01' + '</td>' +                     
                        '   <td style="text-align:center">' +
                        '       <input type="hidden" id="hfIdGrupo" value="' + result.Grupocodi + '" /> ' +
                        '       <a><img src="' + siteRoot + 'Content/Images/btn-cancel.png" alt="" onclick="$(this).parent().parent().parent().remove();" style="cursor:pointer" /></a>' +
                        '   </td>' +
                        '</tr>'
                    );
                }
                else {                    
                    mostrarMensaje('mensajeEdicion', 'alert', 'La URS ya se encuentra agregada.');

                }
            },
            error: function () {
                mostrarMensaje('mensajeEdicion', 'error', 'Se ha producido un error.');
            }
        });
    }
};

eliminar = function (id) {
    if (confirm('¿Está seguro de realizar esta operación?')) {

        $.ajax({
            type: 'POST',
            url: controlador + 'versiondelete',
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
}

grabar = function () {
    var validacion = validar();

    if (validacion == "") {

        var count = 0;
        var items = "";
        $('#tablaURS>tbody tr').each(function (i) {
            $punto = $(this).find('#hfIdGrupo');
            var constante = (count > 0) ? "," : "";
            items = items + constante + $punto.val();
            count++;
        });

        $('#hfListaURS').val(items);


        $.ajax({
            type: 'POST',
            url: controlador + 'versionsave',
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

    if ($('#cbTipoVersion').val() == "") {
        mensaje = mensaje + "<li>Seleccione el tipo de versión.</li>";
        flag = false;
    }

    if ($('#txtDescripcion').val() == "") {
        mensaje = mensaje + "<li>Ingrese la descripción.</li>";
        flag = false;
    }

    if ($('#txtFechaInicio').val() == "") {
        mensaje = mensaje + "<li>Ingrese la fecha de inicio.</li>";
        flag = false;
    }

    if ($('#txtFechaFin').val() == "") {
        mensaje = mensaje + "<li>Ingrese la fecha de fin.</li>";
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