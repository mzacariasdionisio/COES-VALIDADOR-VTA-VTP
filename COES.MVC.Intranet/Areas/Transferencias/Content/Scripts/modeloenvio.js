var controlador = siteRoot + 'transferencias/modeloenvio/';

$(function () {

    $('#cbPeriodo').on('change', function () {
        cargarVersion();
    });

    $('#btnConsultar').on('click', function () {
        cargarListado(0);
    });

    $('#cbEmpresa').on('change', function () {
        cargarListado(0);
    });

    $('#cbVersion').on('change', function () {
        cargarListado(0);
    });  

    $('#container').show();

    cargarVersion();
});

descargarArchivo = function (id) {
    var data = {
        id: id
    };
    redirect(controlador + 'descargararchivo', data);
};

redirect = function (url, params) {
    var $form = $("<form />");
    $form.attr("action", url);
    $form.attr("method", 'GET');
    for (var data in params)
        $form.append('<input type="hidden" name="' + data + '" value="' + params[data] + '" />');
    $("body").append($form);
    $form.submit();
};


cancelarCarga = function () {
    $('#container').hide();
};

cargarVersion = function () {
    $.ajax({
        type: 'POST',
        url: controlador + 'obtenerversion',
        data: {
            pericodi: $('#cbPeriodo').val()
        },
        dataType: 'json',
        global: false,
        success: function (result) {
            if (result != -1) {
                console.log(result);
                $('#cbVersion').get(0).options.length = 0;              
                $.each(result.ListaRecalculo, function (i, item) {
                    $('#cbVersion').get(0).options[$('#cbVersion').get(0).options.length] = new Option(item.RecaNombre, item.RecaCodi);
                });

                cargarListado(0);
            }
            else {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            }
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        }
    });
};

cargarListado = function (indicador) {
    if ($('#cbPeriodo').val() != "" && $('#cbVersion').val() != "" && $('#cbEmpresa').val() != "" ) {
        $.ajax({
            type: 'POST',
            url: controlador + "listado",
            data: {
                empresa: $('#cbEmpresa').val(),
                periodo: $('#cbPeriodo').val(),
                version: $('#cbVersion').val()
            },
            cache: false,
            success: function (evt) {
                $('#listado').css("width", $('#mainLayout').width() + "px");

                $('#listado').html(evt);
                $('#tablaListado').dataTable({
                    "scrollY": 430,
                    "scrollX": true,
                    "sDom": 't',
                    "ordering": false,
                    "iDisplayLength": 100
                });

                //$('#divAcciones').css('display', 'block');

                if (indicador == 0) {
                    mostrarMensaje('mensaje', 'message', 'Por favor seleccione empresa, periodo y versión.');
                }
                else {
                    mostrarMensaje('mensaje', 'exito', 'El archivo fue cargado correctamente.');
                }
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
            }
        });
    }
    else {
        mostrarMensaje('mensaje', 'alert', 'Seleccione tipo de información y un escenario');
        $('#divAcciones').css('display', 'none');
    }
};




mostrarMensaje = function (id, tipo, mensaje) {
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);
};
