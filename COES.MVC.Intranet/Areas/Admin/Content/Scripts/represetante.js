var controlador = siteRoot + 'admin/representante/'

$(function () {

    $('#btnNuevo').click(function () {
        editar(0);
    });

    $('#btnSalir').click(function () {
        document.location.href = siteRoot + 'home/default';
    });

    $('#btnConsultar').click(function () {
        cargarGrilla();
    });

    $('#cbTipoEmpresa').change(function () {
        cargarEmpresas();
    })

    cargarGrilla();
});

cargarEmpresas = function () {
    if ($('#cbTipoEmpresa').val() != "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'obtenerempresas',
            dataType: 'json',
            data: {
                idTipoEmpresa: $('#cbTipoEmpresa').val()
            },
            cache: false,
            success: function (aData) {
                $('#cbEmpresa').get(0).options.length = 0;
                $('#cbEmpresa').get(0).options[0] = new Option("-TODOS-", "");
                $.each(aData, function (i, item) {
                    $('#cbEmpresa').get(0).options[$('#cbEmpresa').get(0).options.length] = new Option(item.Emprnomb, item.Emprcodi);
                });
            },
            error: function () {
                mostrarError();
            }
        });
    }
    else {
        $('option', '#cbEmpresa').remove();
    }
}

cargarGrilla = function () {
    $.ajax({
        type: 'POST',
        url: controlador + 'lista',
        data: {
            idTipoEmpresa: $('#cbTipoEmpresa').val(),
            idEmpresa: $('#cbEmpresa').val()
        },
        success: function (evt) {
            $('#listado').html(evt);
            $('#tabla').dataTable({
            });
        },
        error: function () {
            mostrarError();
        }
    });
}

editar = function (idUsuario) {
    $.ajax({
        type: 'POST',
        data: {
            idUsuario: idUsuario
        },
        url: controlador + 'detalle',
        success: function (evt) {
            $('#contenidoEdicion').html(evt);
            setTimeout(function () {
                $('#popupEdicion').bPopup({
                    autoClose: false
                });
            }, 50);

            $('#btnGrabar').click(function () {               
                if ($("#frmRegistro").valid()) {
                    grabar();
                }
            });

            $('#btnCancelar').click(function () {
                $('#popupEdicion').bPopup().close();
            });

            $("#frmRegistro").validate({
                rules: {
                    Nombre: { required: true, maxlength: 50 },
                    Email: { required: true, maxlength: 60, email: true },
                    Estado: { required: true },
                    EmpresaId: { required: true },
                    Telefono: { maxlength: 50 },
                    Celular: { maxlength: 50 },
                    AreaLaboral: { maxlength: 50 },
                    Cargo: {maxlength: 50 },
                    MotivoContacto: {maxlength: 308 }
                },
                messages: {
                    Nombre: {
                        required: "Ingrese nombre y apellidos",
                        maxlength: "Máximo 50 caracteres"
                    },
                    Email: {
                        required: "Ingrese email",
                        maxlength: "Máximo 50 caracteres",
                        email: "Ingrese un correo válido"
                    },
                    Estado: {
                        required: "Seleccione estado"
                    },
                    EmpresaId: {
                        required: "Seleccione empresa"
                    },
                    Telefono: {
                        required: "Ingrese teléfono",
                        maxlength:"Máximo 50 caracteres"
                    },
                    Celular: {
                        required: "Ingrese celular",
                        maxlength: "Máximo 50 caracteres"
                    },
                    AreaLaboral: {
                        required: "Ingrese el área laboral",
                        maxlength: "Máximo 50 caracteres"
                    },
                    Cargo: {
                        required: "Ingrese el cargo",
                        maxlength: "Máximo 50 caracteres"
                    },
                    MotivoContacto: {
                        required: "Ingrese el motivo del registro",
                        maxlength: "Máximo 308 caracteres"
                    }
                }
            });
        },
        error: function () {
            mostrarError();
        }
    });
}

usuarios = function (idUsuario, idEmpresa)
{
    document.location.href = controlador + 'usuarios?uid=' + idUsuario + "&eid=" + idEmpresa;
}

darbaja = function (idUsuario) {
    if (confirm('¿Está seguro de realizar esta operación?')) {
        $.ajax({
            type: "POST",
            url: controlador + "darbajausuario",
            data: {
                idUsuario: idUsuario
            },
            datatype: "json",
            success: function (result) {
                if (result == 1) {
                    cargarGrilla();
                    $('#mensaje').removeClass();
                    $('#mensaje').addClass('action-exito');
                    $('#mensaje').text('La operación se realizó correctamente.');
                }
                else {
                    mostrarError();
                }
            },
            error: function () {
                mostrarError();
            }
        });
    }
}

grabar = function () {
    $.ajax({
        type: 'POST',
        url: controlador + 'grabar',
        data: $('#frmRegistro').serialize(),
        dataType: 'json',
        success: function (result) {
            if (result > 0) {
                cargarGrilla();
                $('#popupEdicion').bPopup().close();
                $('#mensaje').removeClass();
                $('#mensaje').addClass('action-exito');
                $('#mensaje').text('La operación se realizó correctamente.');
            }
            else if (result == 0) {
                $('#mensajeEdit').removeClass();
                $('#mensajeEdit').addClass('action-alert');
                $('#mensajeEdit').text('El correo ingresado ya se encuentra registrado, ingrese otro');
            }
            else {
                mostrarError();
            }
        },
        error: function () {
            mostrarError();
        }
    });
}

mostrarError = function () {
    alert("Ha ocurrido un error.");
}

