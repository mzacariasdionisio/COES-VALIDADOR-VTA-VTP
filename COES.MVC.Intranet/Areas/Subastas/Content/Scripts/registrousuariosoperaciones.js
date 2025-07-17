var controlador = siteRoot + 'Subastas/RegistroUsuarios/'

$(document).ready(function () {

    // Valida los campos de Crear y Modificar Usuarios por URS
         function validar() {
            var mensaje = "";

            if ($('#empresa').val() == "" || $('#empresa').val() == -1) {
                mensaje = mensaje + " Falta Seleccionar Empresa" + "<br />";
            }
            if ($('#username').val() == "" || $('#username').val() == -1) {
                mensaje = mensaje + "Falta seleccionar en Usuario" + "<br / >";
            }
            if ($('#urs').val() == "" || $('#urs').val() == null) {
                mensaje = mensaje + " Falta Escoger URS" + "<br />";
            }
            return mensaje;
        };

    //Método de Registrar Usuarios por URS
        $('#btnAgregarUsuarios').click(function () {
            var rmj = validar();
            if (rmj == "" || rmj == -1 || rmj == null) {
            $.ajax({
            type: "POST",
            url: controlador + "ValidarUsuarioUrs",
            //dataType: 'json',
            data: {
                registro: "0",
                Usercode: $('#username').val(),
                Urscodi: $('#urs').val()
            },
                    cache: false,
                    success: function (resultado) {
                        if (resultado) {
                    $.ajax({
                        type: "POST",
                        url: controlador + "MantenimientoUser",
                        //dataType: 'json',
                        data: {
                            Usercode: $('#username').val(),
                            Urscodi: $('#urs').val(),
                            accion: "Nuevo"
                        },
                        success: function (result) {
                            if (result.Resultado == '-1') {
                                alert('Ha ocurrido un error:' + result.Mensaje);
                            } else {
                                //todos
                                $('#btnCancelar').click();
                                mensajeOperacion(result.Resultado, 1);
                            }
                        },
                        error: function (req, status, error) {
                            mensajeOperacion(error);
                            validaErrorOperation(req.status);
                        }
                    });
                        } else {
                            mensajeOperacion("No puede REGISTRAR este Usuario por URS: EL URS que intenta registrar ya existe...");
                        }
                            },
                            error: function (req, status, error) {
                                mensajeOperacion(error);
                                validaErrorOperation(req.status);
                            }
                });
            } else {
                mensajesError(rmj);
            }
        });

    //Método de Modificar Usuarios por URS
        $('#btnModificarUsuario').click(function () {
            var rmj = validar();
            if (rmj == "" || rmj == -1) {
            var reg = $(this).attr('data-registro');
            $.ajax({
                type: "POST",
                url: controlador + "ValidarUsuarioUrsMod",
                //dataType: 'json',
                data: {
                    registro: reg,
                    Usercode: $('#usernameedit').val(),
                    Urscodi: $('#urs').val()
                },
                cache: false,
                success: function (resultado) {
                    if (resultado) {
                    $.ajax({
                        type: "POST",
                        url: controlador + "MantenimientoUser",
                        //dataType: 'json',
                        data: {
                            registro: reg,
                            Usercode: $('#usernameedit').val(),
                            Urscodi: $('#urs').val(),
                            accion: "Editar"
                        },
                        success: function (result) {
                            if (result.Resultado == '-1') {
                                alert('Ha ocurrido un error:' + result.Mensaje);
                            } else {
                                //todos
                                $('#btnCancelar').click();
                                mensajeOperacion(result.Resultado, 1);
                            }
                        },
                        error: function (req, status, error) {
                            mensajeOperacion(error);
                            validaErrorOperation(req.status);
                        }
                    });
                        } else {
                        mensajeOperacion("No puede MODIFICAR este Usuario por URS: EL URS que desea ingresar ya existe o es el mismo...");
                    }
                    },
                    error: function (req, status, error) {
                        mensajeOperacion(error);
                        validaErrorOperation(req.status);
                    }
            });
            } else {
                mensajesError(rmj);
            }
        });

    // Filtro para listar el combo USUARIO al escoger EMPRESA
        $('#empresa').change(function () {
            $('#username > option').remove();
            $.ajax({
                type: "POST",
                global: false,
                url: controlador + 'ListarUsuarios',
                data: { tipo: $(this).val() },
                cache: false,
                success: function (resultado) {
                    $('#username').append($('<option>', { value: -1, text: "(SELECCIONAR)" }));
                    for (i = 0, l = resultado.length; i < l; i++) {
                        $('#username').append($('<option>', { value: resultado[i].Usercode, text: resultado[i].Username }));
                    }
                },
                error: function (req, status, error) {
                    mensajeOperacion(error);
                }
            });
        });

    // Filtro para listar el combo EMPRESA al escoger USUARIO
        $('#username').change(function () {
            $('#empresa > option').remove();
            $.ajax({
                type: "POST",
                global: false,
                url: controlador + 'ListarEmpresa',
                data: { tipo: $(this).val() },
                cache: false,
                success: function (resultado) {
                    for (i = 0, l = resultado.length; i < l; i++) {
                        $('#empresa').append($('<option>', { value: resultado[i].Emprcodi, text: resultado[i].Emprnomb }));
                    }
                    $('#empresa').append($('<option>', { value: -1, text: "(SELECCIONAR)" }));
                },
                error: function (req, status, error) {
                    mensajeOperacion(error);
                }
            });
        });

    // Filtro para listar el MULTISELECT al escoger USUARIO
        $('#username').change(function () {
            $('#urs').html( $('#urs').multipleSelect({
                    width: '210px',
                    onClick: function (view) {
                    },
                    onClose: function (view) {
                    }
                })
            );
            $.ajax({
                type: "POST",
                global: false,
                url: controlador + 'ListarUrs',
                data: { tipo: $(this).val() },
                cache: false,
                success: function (resultado) {
              
                    for (i = 0, l = resultado.length; i < l; i++) {
                 
                        $('#urs').append($('<option>', { value: resultado[i].Urscodi, text: resultado[i].Ursnomb }));
                    }
                        $('#urs').append($('#urs').multipleSelect({
                            width: '210px',
                            onClick: function (view) {
                            },
                            onClose: function (view) {
                            }
                        })
                    );
                },
                error: function (req, status, error) {
                    mensajeOperacion(error);
                }
            });
        });
});

