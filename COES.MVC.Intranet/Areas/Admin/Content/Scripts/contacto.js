var controlador = siteRoot + 'admin/contacto/';

$(function () {

    $('#btnConsultar').on('click', function () {
        consultar();
    });

    $('#btnNuevoComite').on('click', function () {
        $('#popupNuevoContacto').bPopup().close();
        editar(0, 'P', 'S');
    });

    $('#btnNuevoProceso').on('click', function () {
        $('#popupNuevoContacto').bPopup().close();
        editar(0, 'P', 'P');
    });

    $('#btnExportar').on('click', function () {
        exportar();
    });

    $('#cbTipoEmpresa').on('change', function () {
        cargarEmpresas('cbTipoEmpresa', 'cbEmpresa', 'mensaje');
    });

    $('#tdComite').hide();
    $('#tdProceso').hide();
    $('#tdListas').hide();

    $('#cbFuente').on('change', function () {
        $('#tdComite').hide();
        $('#tdProceso').hide();
        $('#tdListas').hide();
        if ($('#cbFuente').val() == "C") {
            $('#tdComite').show();
            $('#tdListas').show();
        }
        if ($('#cbFuente').val() == "O") {
            $('#tdProceso').show();
        }
    });

    $('#cbComite').on('change', function () {

        select = document.getElementById("cbListas");
        id = $('#cbComite').val();

        if (id > 0) {

            $.ajax({
                type: 'POST',
                url: controlador + "ListByComite",
                data: {
                    idComiteLista: id
                },
                dataType: 'json',
                cache: false,
                success: function (resultado) {
                    if (resultado) {

                        for (let i = select.options.length; i >= 0; i--) {
                            select.remove(i);
                        }

                        option = document.createElement("option");
                        option.value = "-1";
                        option.text = "-TODOS-";
                        select.appendChild(option);

                        for (i = 0; i < resultado.length; i++) {
                            option = document.createElement("option");
                            option.value = resultado[i].Comitelistacodi;
                            option.text = resultado[i].Comitelistaname;
                            select.appendChild(option);
                        }
                    }
                    else {
                        for (let i = select.options.length; i >= 0; i--) {
                            select.remove(i);
                        }

                        option = document.createElement("option");
                        option.value = "-1";
                        option.text = "-TODOS-";
                        select.appendChild(option);
                    }
                },
            });

        } else {
            for (let i = select.options.length; i >= 0; i--) {
                select.remove(i);
            }

            option = document.createElement("option");
            option.value = "-1";
            option.text = "-TODOS-";
            select.appendChild(option);
        }


    });

    $('#btnComites').on('click', function () {
        document.location.href = controlador + "Comite";
    });

    $('#btnProcesos').on('click', function () {
        document.location.href = controlador + "Proceso";
    });

    $('#btnNuevo').on('click', function () {
        $('#popupNuevoContacto').bPopup({
            autoClose: false
        });
    });

    consultar();
});


exportar = function () {
    $.ajax({
        type: 'POST',
        url: controlador + "exportar",
        data: {
            idTipoEmpresa: $('#cbTipoEmpresa').val(),
            idEmpresa: $('#cbEmpresa').val(),
            fuente: $('#cbFuente').val(),
            publico: $('#hfIndicadorPublico').val(),
            idComite: $('#cbComite').val(),
            idComiteLista: $('#cbListas').val(),
            idProceso: $('#cbProceso').val()
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
            idTipoEmpresa: $('#cbTipoEmpresa').val(),
            idEmpresa: $('#cbEmpresa').val(),
            fuente: $('#cbFuente').val(),
            publico: $('#hfIndicadorPublico').val(),
            idComite: $('#cbComite').val(),
            idComiteLista: $('#cbListas').val(),
            idProceso: $('#cbProceso').val()
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
}

editar = function (id, fuente, tipo) {

    $.ajax({
        type: 'POST',
        url: controlador + 'editar',
        data: {
            idContacto: id,
            fuente: fuente,
            tipo: tipo
        },
        global: false,
        success: function (evt) {
            $('#contenidoEdicion').html(evt);
            setTimeout(function () {
                $('#popupEdicion').bPopup({
                    autoClose: false
                });
            }, 50);

            $('#cbTipoEmpresaEdit').val($('#hfTipoEmpresaEdit').val());
            $('#cbEmpresaEdit').val($('#hfEmpresaEdit').val());
            $('#cbEstadoEdit').val($('#hfEstadoEdit').val());

            $('#cbTipoEmpresaEdit').on('change', function () {
                cargarEmpresas('cbTipoEmpresaEdit', 'cbEmpresaEdit', 'mensajeEdit');
            });

            $('#btnGrabar').on("click", function () {
                grabar();
            });

            $('#btnGrabarProceso').on("click", function () {
                grabarProceso();
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


cargarEmpresas = function (idpadre, idcombo, idmensaje) {

    $('option', '#' + idcombo).remove();

    if ($('#' + idpadre).val() != "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'cargarempresas',
            dataType: 'json',
            data: {
                idTipoEmpresa: $('#' + idpadre).val()
            },
            cache: false,
            success: function (aData) {
                $('#' + idcombo).get(0).options.length = 0;
                $('#' + idcombo).get(0).options[0] = new Option("--SELECCIONE--", "");
                $.each(aData, function (i, item) {
                    $('#' + idcombo).get(0).options[$('#' + idcombo).get(0).options.length] = new Option(item.Text, item.Value);
                });
            },
            error: function () {
                mostrarMensaje(idmensaje, 'error', 'Ha ocurrido un error.');
            }
        });
    }
    else {
        $('option', '#' + idcombo).remove();
    }
}

eliminar = function (id) {
    if (confirm('¿Está seguro de realizar esta operación?')) {

        $.ajax({
            type: 'POST',
            url: controlador + 'eliminar',
            data: {
                idContacto: id
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

        recuperarComites();

        recuperarCorreos();

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

grabarProceso = function () {
    var validacion = validar();

    if (validacion == "") {

        var procesos = "";
        $('#proceso-content input:checked').each(function () {
            procesos = procesos + $(this).val() + ",";
        });

        $('#hfDetalleProcesos').val(procesos);

        $.ajax({
            type: 'POST',
            url: controlador + 'grabarproc',
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

validar = function () {

    var mensaje = "<ul>";
    var flag = true;

    if ($('#txtNombre').val() == "") {
        mensaje = mensaje + "<li>Ingrese el nombre.</li>";
        flag = false;
    }

    if ($('#txtApellido').val() == "") {
        mensaje = mensaje + "<li>Ingrese el apellido.</li>";
        flag = false;
    }

    if ($('#txtCorreo').val() != "") {
        if (!validarEmail($('#txtCorreo').val())) {
            mensaje = mensaje + "<li>Ingrese correo válido.</li>";
            flag = false;
        }
    }

    if ($('#txtTelefono').val() == "" && $('#txtMovil').val() == "") {
        mensaje = mensaje + "<li>Ingrese número de teléfono o celular</li>";
    }

    if ($('#cbEmpresaEdit').val() == "") {
        mensaje = mensaje + "<li>Seleccione empresa.</li>";
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