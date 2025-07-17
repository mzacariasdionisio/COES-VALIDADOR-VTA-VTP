var controlador = siteRoot + 'Subastas/RegistroUsuarios/'

$(document).ready(function () {

    //Método Eliminar Usuario por URS
    $('.btnEliminarUsuario').click(function () {
        var strurs = $(this).attr('data-strurs');
        var reg = $(this).attr('data-registro');
        mensajeOperacion("\u00BF"+"Esta seguro de eliminar el URS: " + strurs + "?", null
            ,{
                showCancel: true,
                onOk: function () {
        $.ajax({
            type: "POST",
            url: controlador + 'UsuarioEliminado',
            //dataType: 'json',
            data: { registro: reg },
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

    //Muestra ventana de Editar Usuario por URS
    $('.btnEditarUsuario').click(function () {
        var reg = $(this).attr('data-registro');
                    $.ajax({
                        type: "POST",
                        global: false,
                        url: controlador + 'EditarUsuario',
                        //dataType: 'json',
                        data: {
                            registro: reg,
                        },
                        cache: false,
                        success: function (resultado) {
                            $('#ele-popup-content').html('<div class="title_tabla_pop_up">' + $('.btnEditarUsuario').attr('title') + '</div>' + resultado);
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
   
    //Muestra ventana Nuevo Usuario por URS
    $('.btnNuevoUsuario').click(function () {
        $.ajax({
            type: "POST",
            global: false,
            url: controlador + 'NuevoUsuario',
            //dataType: 'json',
            data: {},
            cache: false,
            success: function (resultado) {
                
                $('#ele-popup-content').html('<div class="title_tabla_pop_up">' + $('.btnNuevoUsuario').attr('title') + '</div>' + resultado);
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
