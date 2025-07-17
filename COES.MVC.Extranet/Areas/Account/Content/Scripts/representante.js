var controlador = siteRoot + 'account/representante/'

$(function () {

    $('#btnNuevo').click(function () {
       
        editar(0);
    });

    $('#btnSalir').click(function () {
        document.location.href = controlador + 'index';
    });



    cargarGrilla();
});

cargarGrilla = function () {

    $.ajax({
        type: 'POST',
        url: controlador + 'grilla',
        data: {
            idEmpresa: $('#hfEmpresa').val()
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
            idUsuario: idUsuario,
            idEmpresa: $('#hfEmpresa').val()
        },
        url: controlador + 'edicion',
        success: function (evt) {
            $('#contenidoEdicion').html(evt);
            setTimeout(function () {
                $('#popupEdicion').bPopup({
                    autoClose: false
                });
            }, 50);

            $('#btnGrabar').click(function () {

                var modulos = "";
                countModulo = 0;
                $('#tablaModulos tbody input:checked').each(function () {
                    modulos = modulos + $(this).val() + ",";
                    countModulo++;
                });

                $('#hfModulos').val("");
                if (countModulo > 0) {
                    $('#hfModulos').val(modulos);
                }
                countEmpresa = $('#tablaEmpresa >tbody >tr').length;

                if (countModulo > 0 && countEmpresa > 0) {
                    if ($("#frmRegistro").valid()) {
                        grabar();
                    }
                    else {
                        $('#mensajeEdit').text('Complete los datos del usuario');
                    }
                }
                else {
                    $('#mensajeEdit').removeClass();
                    $('#mensajeEdit').addClass('action-alert');
                    $('#mensajeEdit').text('Seleccione módulos y agregue empresas');
                }
            });

            $('#btnAddEmpresa').click(function () {
                agregarEmpresa();
            });

            $('#btnCancelar').click(function () {
                $('#popupEdicion').bPopup().close();
            });

            $("#frmRegistro").validate({
                rules: {
                    Nombre: { required: true, maxlength: 50 },
                    Email: { required: true, maxlength: 50, email: true },
                    Estado: { required: true },
                    EmpresaId: { required: true },
                    Telefono: { maxlength: 50 },
                    AreaLaboral: { maxlength: 50 },
                    Cargo: { maxlength: 50 },
                    MotivoContacto: { maxlength: 308 }
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
                        maxlength: "Máximo 50 caracteres"
                    },
                    AreaLaboral: {
                        maxlength: "Máximo 50 caracteres"
                    },
                    Cargo: {
                        maxlength: "Máximo 50 caracteres"
                    },
                    MotivoContacto: {
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
        url: controlador + 'grabarusuario',
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

cargarEmpresas = function () {
    $.ajax({
        type: "POST",
        url: controlador + "empresa",
        global: false,
        success: function (evt) {
            $('#empresa-selected').html(evt);

        },
        error: function () {
            mostrarError();
        }
    });
}

agregarEmpresa = function () {
    if ($('#cbEmpresaAdd').val() != "") {
        $.ajax({
            type: "POST",
            url: controlador + "addempresa",
            global: false,
            data: {
                idEmpresa: $('#cbEmpresaAdd').val()
            },
            success: function (evt) {
                $('#empresa-selected').html(evt);
            },
            error: function () {
                mostrarError();
            }
        });
    }
}

removeEmpresa = function (idEmpresa) {
    if (confirm("¿Está seguro de realizar esta operación?")) {
        $.ajax({
            type: "POST",
            url: controlador + "removeempresa",
            global: false,
            data: {
                idEmpresa: idEmpresa
            },
            success: function (evt) {
                $('#empresa-selected').html(evt);
            },
            error: function () {
                mostrarError();
            }
        });
    }
}

mostrarError = function () {
    alert("Error");
}