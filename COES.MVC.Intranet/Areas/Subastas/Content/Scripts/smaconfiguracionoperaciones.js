var controlador = siteRoot + 'Subastas/configuracion/'

$(document).ready(function () {

    // Valida los campos de Crear y Modificar Configuración
        function validar() {
            var mensaje = "";

            if ($('#atributo').val() == "" || $('#atributo').val() == -1 || $('#atributo').val() == "SELECCIONE") {
                mensaje = mensaje + " Falta seleccionar Atributo" + "<br />";
            }
            if ($('#parametro').val() == "" || $('#parametro').val() == -1) {
                mensaje = mensaje + "Falta seleccionar en par\u00E1metro" + "<br / >";
            }
            if ($('#valor').val() == "" || $('#valor').val() == null) {
                mensaje = mensaje + " Falta ingresar valor" + "<br />";
            }
            return mensaje;
        };

    //Método de Registrar Configuración
        $('#btnRegistrarConfiguracion').click(function () {
            var rmj = validar();
            if (rmj == "" || rmj == -1 || rmj == null) {
                $.ajax({
                    type: "POST",
                    url: controlador + "ValidarConfiguracion",
                    //dataType: 'json',
                    data: {
                        registro: "0",
                        atributo: $('#atributo').val(),
                        parametro : $('#parametro').val(),
                        valor: $('#valor').val()
                    },
                    cache: false,
                    success: function (resultado) {
                        if (resultado) {
                            $.ajax({
                                type: "POST",
                                url: controlador + "MantenimientoConfiguracion",
                                //dataType: 'json',
                                data: {
                                    atributo: $('#atributo').val(),
                                    parametro: $('#parametro').val(),
                                    valor:$('#valor').val(),
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
                            mensajeOperacion("No puede REGISTRAR esta configuraci\u00F3n: EL valor que intenta registrar ya existe...");
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
 
    //Método de Modificar Configuración
        $('#btnModificarConfiguracion').click(function () {
            var rmj = validar();
            if (rmj == "" || rmj == -1) {
                var reg = $(this).attr('data-registro');
                $.ajax({
                    type: "POST",
                    url: controlador + "ValidarConfiguracionMod",
                    //dataType: 'json',
                    data: {
                        registro: reg,
                        atributo: $('#atributo').val(),
                        parametro: $('#parametro').val(),
                        valor: $('#valor').val()
                    },
                    cache: false,
                    success: function (resultado) {
                        if (resultado) {
                            $.ajax({
                                type: "POST",
                                url: controlador + "MantenimientoConfiguracion",
                                //dataType: 'json',
                                data: {
                                    registro: reg,
                                    atributo: $('#atributo').val(),
                                    parametro: $('#parametro').val(),
                                    valor: $('#valor').val(),
                                    accion: "Editar"
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
                        } else {
                            mensajeOperacion("No puede MODIFICAR esta configuraci\u00F3n: EL valor que desea ingresar ya existe o es el mismo...");
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

    // filtro para listar PARAMETRO al escoger ATRIBUTO
        $('#atributo').change(function () {
            $('#parametro').prop('disabled' , false);
        $('#parametro > option').remove();
        $.ajax({
            type: "POST",
            global: false,
            url: controlador + 'ListarParametro',
            data: { tipo: $(this).val() },
            cache: false,
            success: function (resultado) {
                for (i = 0, l = resultado.length; i < l; i++) {
                    $('#parametro').append($('<option>', { text: resultado[i].Confsmparametro }));
                }
            },
            error: function (req, status, error) {
                mensajeOperacion(error);
            }
        });
    });
   
});