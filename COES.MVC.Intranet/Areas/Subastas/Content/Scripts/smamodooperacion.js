var controlador = siteRoot + 'Subastas/ModoOperacion/'

$(document).ready(function () {

    //Método Eliminar Configuración
    $('.btnEliminarModoOperacion').unbind();
    $('.btnEliminarModoOperacion').click(function () {
        var valor = $(this).attr('data-valor');
        var reg = $(this).attr('data-registro');
        mensajeOperacion("\u00BF" + "Esta seguro de eliminar el Modo Operación: " + valor + "?", null
            , {
                showCancel: true,
                onOk: function () {
                    $.ajax({
                        type: "POST",
                        url: controlador + 'EliminarModoOperacion',
                        //dataType: 'json',
                        data: {
                            registro: reg
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
                },
                onCancel: function () {

                }
            });
    });

    $('.btnEditarConfiguracion').click(function () {
        var reg = $(this).attr('data-registro');
        $.ajax({
            type: "POST",
            global: false,
            url: controlador + 'EditarModoOperacion',
            //dataType: 'json',
            data: { registro: reg },
            cache: false,
            success: function (resultado) {
                $('#ele-popup-content').html('<div class="title_tabla_pop_up">' + $('.btnEditarConfiguracion').attr('title') + '</div>' + resultado);
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

    $('.btnNuevoModoOperacion').click(function () {
        $.ajax({
            type: "POST",
            global: false,
            url: controlador + 'NuevoModoOperacion',
            //dataType: 'json',
            data: {

            },
            cache: false,
            success: function (resultado) {
                $('#ele-popup-content').html('<div class="title_tabla_pop_up">' + $('.btnNuevoModoOperacion').attr('title') + '</div>' + resultado);
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

});