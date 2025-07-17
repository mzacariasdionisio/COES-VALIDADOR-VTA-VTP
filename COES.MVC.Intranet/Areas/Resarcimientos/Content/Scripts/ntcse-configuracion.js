$.message = "";
var controler = siteRoot + "resarcimientos/configuracion/";

$(document).ready(function () {
    function validar(){
        var mensaje ="";

        if ($('#atributo').val() == "") {
            mensaje = mensaje + "Falta ingresar datos en Atributo";
           }
        if ($('#parametro').val() == "") {
            mensaje = mensaje + "Falta ingresar datos en Parametro";
           }
        
        if ($('#confvalor').val() == "") {
            mensaje = mensaje + "Falta ingresar datos en Valor";
            }
        return mensaje;
	}
    // Boton Nuevo Conf. Param
    $('#NuevoConfig').click(function () {

        $.ajax({
            type: "POST",
            global: false,
            url: controler + 'confparamnuevo',
            //dataType: 'json',
            data: {},
            cache: false,
            success: function (resultado) {
                $('#ele-popup-content').html('<div class="title_tabla_pop_up">' + $('#NuevoConfig').attr('title') + '</div>' + resultado);

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

    //AGREGAR CONFIGIRACIÓN DE PARAMETRO
    $('#btnAgregarConfig').click(function () {
        var rmj = validar();
        if (rmj == "") {
            $.ajax({
                type: "POST",
                url: controler + 'mantenimientoconfiguracion',
                //dataType: 'json',
                data: {
                    atributo: $('#atributo').val(),
                    parametro: $('#parametros').val(),
                    valor: $('#confvalor').val(),
                    accion: "agregar"

                },
                //cache: false,
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
            mensajesError(rmj);
        }
    });

    //eliminar ConfiguraciónParaetros
    $('.btnOpenFileManagerEliminarParametro').click(function () {
        var strNombreTabla = $(this).attr('data-nombretabla');
        var reg = $(this).attr('data-registro');

        mensajeOperacion("Esta seguro de eliminar la Configuración: " + strNombreTabla + "?", null
            , {
                showCancel: true,
                onOk: function () {
                    $.ajax({
                        type: "POST",
                        global: false,
                        url: controler + "eliminarconfigparametro",
                        //dataType: 'json',
                        data: { registro: reg },
                        cache: false,
                        success: function (resultado) {
                            mensajeOperacion(resultado, 1);

                        },
                        error: function (req, status, error) {
                            mensajeOperacion(error);
                        }
                    });
                },
                onCancel: function () {

                }
            });
    });

    //Boton Editar configuraciónParametrs
    $('.btnOpenFileManagerEditarParametro').click(function () {

        $.ajax({
            type: "POST",
            global: false,
            url: controler + 'editarconfigparam',
            //dataType: 'json',
            data: { registro: $(this).attr('data-registro') },
            cache: false,
            success: function (resultado) {
                $('#ele-popup-content').html('<div class="title_tabla_pop_up">' + $('.btnOpenFileManagerEditarParametro').attr('title') + '</div>' + resultado);
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

    //Modificar tension
    $('#btnModificarTension').click(function () {
        var rmj = validar();
        if(rmj == ""){
            $.ajax({
                type: "POST",
                url: controler + "mantenimientoconfiguracion",
                //dataType: 'json',
                data: {
                    atributo: $('#atributo').val(),
                    parametro: $('#parametro').val(),
                    valor: $('#confvalor').val(),
                    registro: $(this).attr("data-registro"),
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
            mensajesError(rmj);
        }
    });

    $('#atributo').change(function () {
        $('#parametros > option').remove();
            $.ajax({
                type: "POST",
                url: controler + "ObtenerParametro",
                //dataType: 'json',
                data: {
                    atributo: $('#atributo').val(),
                },
                cache: false,
                success: function (resultado) {
                    $('#parametros').append($('<option>', { value: -1, text: "(SELECCIONAR)" }));
                   
                    for (var i = 0; i < resultado.length; i++) {
                        $('#parametros').append($('<option>', { value: resultado[i].ConfParametro, text: resultado[i].ConfParametro }));
                       
                    }

                },
                error: function (req, status, error) {
                    mensajeOperacion(error);
                }
            });
    });

});







