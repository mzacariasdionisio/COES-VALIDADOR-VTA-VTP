$.message = "";
var controler = siteRoot + "resarcimientos/periodo/";

$(document).ready(function () {

    function validar() {
        var mensaje = "";

        if ($('#periodo').val() == "") {
            mensaje = mensaje + "Falta seleccionar en Periodo";
        }
        if ($('#txtanio').val() == "") {
            mensaje = mensaje + "Falta ingresar datos en Año";
        }
        return mensaje;
    };

    $('#btnNuevoPeriodo').click(function () {
        $.ajax({
            type: "POST",
            global: false,
            url: controler + 'periodonuevo',
            //dataType: 'json',
            data: {},
            cache: false,
            success: function (resultado) {
                $('#ele-popup-content').html('<div class="title_tabla_pop_up">' + $('#btnNuevoPeriodo').attr('title') + '</div>' + resultado);

                var t = setTimeout(function () {
                    $('#ele-popup').bPopup({ modalClose: false, escClose: false });
                    clearTimeout(t);
                }, 60)
            },
            error: function (req, status, error) {
                mensajeOperacion(error);
                validaErrorOperation(req.status);
            }
        });

    });
    $('.btnEliminarPeriodo').click(function () {
        var strPeriodo = $(this).attr('data-periodo');
        var reg = $(this).attr('data-registro');

        mensajeOperacion("Esta seguro de eliminar el Periodo: " + strPeriodo + "?", null
            , {
                showCancel: true,
                onOk: function () {
                    $.ajax({
                        type: "POST",
                        url: controler + "validarperiodo",
                        //dataType: 'json',
                        data: {
                            registro: reg,
                            periodo: "-1",
                            anio: "-1"
                        },
                        cache: false,
                        success: function (resultado) {
                            if (resultado) {
                                $.ajax({
                                    type: "POST",
                                    global: false,
                                    url: controler + 'PeriodoEliminado',
                                    //dataType: 'json',
                                    data: { registro: reg },
                                    cache: false,
                                    success: function (resultado) {
                                        mensajeOperacion(resultado, 1);
                                    },
                                    error: function (req, status, error) {
                                        mensajeOperacion(error);
                                        validaErrorOperation(req.status);
                                    }
                                });
                            } else {
                                mensajeOperacion("No puede Eliminar este periodo ya existe datos asociados al mismo...");
                            }
                        },
                        error: function (req, status, error) {
                            mensajeOperacion(error);
                            validaErrorOperation(req.status);
                        }
                    });
                },
                onCancel: function () {

                }
            });
    });

    $('#btnAgregarPeriodo').click(function () {
        var rmj = validar();
        if (rmj == "") {
            $.ajax({
                type: "POST",
                url: controler + "validarperiodo",
                //dataType: 'json',
                data: {
                    registro: "0",
                    periodo: $('#periodo').val(),
                    anio: $('#txtanio').val()
                },
                cache: false,
                success: function (resultado) {
                    if (resultado) {
                        $.ajax({
                            type: "POST",
                            url: controler + "mantenimientoperiodo",
                            //dataType: 'json',
                            data: {
                                periodo: $('#periodo').val(),
                                anio: $('#txtanio').val(),
                                estado: $('#CboEstado').val(),
                                accion: "Nuevo"
                            },
                            cache: false,
                            success: function (resultado) {
                                //todos
                                $('#btnCancelar').click();
                                mensajeOperacion(resultado, 1);
                            },
                            error: function (req, status, error) {
                                mensajeOperacion(error);
                                validaErrorOperation(req.status);
                            }
                        });
                    } else {
                        mensajeOperacion("No puede Agregar este periodo ya existe otro registrado...");
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

    // EDITAR PERIODO
    $('#btnModificarPeriodo').click(function () {
        //if ($('#periodo').val() != "" && $('#anio').val() != "") {
        var rmj = validar();
        if (rmj == "") {
            var reg = $(this).attr('data-registro');
            $.ajax({
                type: "POST",
                url: controler + "validarperiodo",
                //dataType: 'json',
                data: {
                    registro: reg,
                    periodo: $('#periodo').val(),
                    anio: $('#anio').val()
                },
                cache: false,
                success: function (resultado) {
                    if (resultado) {
                        $.ajax({
                            type: "POST",
                            url: controler + "mantenimientoperiodo",
                            //dataType: 'json',
                            data: {
                                registro: reg,
                                periodo: $('#periodo').val(),
                                anio: $('#anio').val(),
                                estado: $('#CboEstado').val(),
                                accion: "Editar"
                            },
                            cache: false,
                            success: function (resultado) {
                                //todos
                                $('#btnCancelar').click();
                                mensajeOperacion(resultado, 1);
                            },
                            error: function (req, status, error) {
                                mensajeOperacion(error);
                                validaErrorOperation(req.status);
                            }
                        });
                    } else {
                        mensajeOperacion("No puede editar este período, existe periodo con los mismos datos...");
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

    $('.btnEditarPeriodo').click(function () {
        var reg = $(this).attr('data-registro');
        $.ajax({
            type: "POST",
            url: controler + "validarperiodo",
            //dataType: 'json',
            data: {
                registro: reg,
            },
            cache: false,
            success: function (resultado) {
                if (resultado) {
                    $.ajax({
                        type: "POST",
                        global: false,
                        url: controler + 'editarperiodo',
                        //dataType: 'json',
                        data: { registro: reg },
                        cache: false,
                        success: function (resultado) {
                            $('#ele-popup-content').html('<div class="title_tabla_pop_up">' + $('.btnEditarPeriodo').attr('title') + '</div>' + resultado);

                            var t = setTimeout(function () {
                                $('#ele-popup').bPopup({ modalClose: false, escClose: false });
                                clearTimeout(t);
                            }, 60)
                        },
                        error: function (req, status, error) {
                            mensajeOperacion(error);
                            validaErrorOperation(req.status);
                        }
                    });
                } else {
                    mensajeOperacion("No puede editar este período, existen datos asociados...");
                }
            },
            error: function (req, status, error) {
                mensajeOperacion(error);
                validaErrorOperation(req.status);
            }
        });
    });

    $('.btnHabilitarPeriodo').click(function () {
        var key = $(this).attr('data-registro');
        var periodo = $(this).attr('data-periodo');
        $.ajax({
            type: "POST",
            url: controler + "estadoperiodo",
            //dataType: 'json',
            data: {
                registro: $(this).attr('data-registro')
            },
            cache: false,
            success: function (resultado) {
                //todos
                $('#habilitado' + key).html(resultado);
                var estado = resultado;
                $.ajax({
                    type: "POST",
                    url: controler + "Validacion",
                    //dataType: 'json',
                    data: {
                        estado: estado,
                        periodo: periodo
                    },
                    cache: false,
                    success: function (resultado) {
                        $('#btnCancelar').click();
                        mensajeOperacion(resultado, 1);
                    },
                    error: function (req, status, error) {
                        mensajeOperacion(error);
                        validaErrorOperation(req.status);
                    }
                });

            },
            error: function (req, status, error) {
                mensajeOperacion(error);
                validaErrorOperation(req.status);
            }
        });

    });

});







