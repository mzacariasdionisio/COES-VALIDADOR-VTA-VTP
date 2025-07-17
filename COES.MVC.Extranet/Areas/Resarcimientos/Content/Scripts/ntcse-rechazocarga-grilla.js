var controlador = siteRoot + "resarcimientos/rechazocarga/";

$(document).ready(function () {
    $('.btnOpenFileManagerRegistro .select-btnclick').click(function () {
        $.ajax({
            type: "POST",
            global: false,
            url: controlador +"buscaregistroparacombobox",
            //dataType: 'json',
            data: { registro: $(this).closest('.btnOpenFileManagerRegistro').attr('data-registro') },
            cache: false,
            success: function (resultado) {
                $('#CboCliente').val(resultado.RRCCLIENTE);
                $('#CboPuntoEntrega').val(resultado.RRCPUNTOENTREGA);
                $('#CboTension').val(resultado.RRCNIVELTENSION);
            },
            error: function (req, status, error) {
                mensajeOperacion(error);
                validaErrorOperation(req.status);
            }
        });
    });
    $('.btnOpenFileManagerVerDettalle').click(function () {
        var url = controlador + "DetalleCarga";
        $.ajax({
            type: "POST",
            global: false,
            url: url,
            //dataType: 'json',
            data: { registro: $(this).attr('data-registro') },
            cache: false,
            success: function (resultado) {
                $('#ele-popup-content').html('<div class="title_tabla_pop_up">' + $('.btnOpenFileManagerVerDettalle').attr('title') + '</div>' + resultado);

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
    $('.btnOpenFileManagerRCarga').click(function () {
        var id = $(this).attr('data-registro');
        if ($('#CboEmpresasGeneradoras').val() != "0") {
            if($('#CboPeriodo').val() != "0"){
        $.ajax({
            type: "POST",
            url: controlador + "validaempresasgeneradoras",
            //dataType: 'json',
            data: {
                empresa: $('#CboEmpresasGeneradoras').val()
            },
            cache: false,
            success: function (resultado) {
                //Content Registrar
                if (resultado) {
                $.ajax({
                    type: "POST",
                    global: false,
                    url: controlador+"RegistrarCarga",
                    //dataType: 'json',
                    data: {registro: id},
                    cache: false,
                    success: function (resultado) {
                        $('#ele-popup-content').html('<div class="title_tabla_pop_up">' + $('.btnOpenFileManagerRCarga').attr('title') + '</div>' + resultado);
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
                    mensajeOperacion($('#btnUsuarioEmpresa').attr('title'));
                }
                //end Content
            },
            error: function (req, status, error) {
                validaErrorOperation(req.status);
            }
        });
        } else {
            mensajeOperacion("Selecione Un Periodo para Continuar...");
        }
    } else {
        mensajeOperacion("Selecione Un Empresa Generadora para Continuar...");
    }
    });
    $('.btnOpenFileManagerEliminarCarga').click(function () {
        var id = $(this).attr('data-registro');
        $.ajax({
            type: "POST",
            url: controlador + "validaempresasgeneradoras",
            //dataType: 'json',
            data: {
                empresa: $('#CboEmpresasGeneradoras').val()
            },
            cache: false,
            success: function (resultado) {
                //Content Registrar
                if (resultado) {
                $.ajax({
                    type: "POST",
                    global: false,
                    url: controlador + "eliminarcarga",
                    //dataType: 'json',
                    data: { registro: id },
                    cache: false,
                    success: function (resultado) {
                        $('#ele-popup-content').html('<div class="title_tabla_pop_up">' + $('.btnOpenFileManagerEliminarCarga').attr('title') + '</div>' + resultado);

                        $('#ele-popup-content > .tabla-formulario-eliminar').dataTable();
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
                    mensajeOperacion($('#btnUsuarioEmpresa').attr('title'));
                }
                //end Content
            },
            error: function (req, status, error) {
                validaErrorOperation(req.status);
            }
        });
    });
});