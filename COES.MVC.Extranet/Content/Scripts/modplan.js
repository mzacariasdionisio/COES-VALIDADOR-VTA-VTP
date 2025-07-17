var controlador = siteRoot + 'planificacion/';

$(function () {   
    listarVersionModplan();

    $('#btnModplanCancelar').on('click', function () {
        $('#validaciones').bPopup().close();
    });

    $('#btnModplanAceptar').on('click', function () {
        aceptarTerminos();
    });
});

aceptarTerminos = function () {
    if ($('#cbTerminos').prop('checked')) {

        var data = {
            idVersion: $('#hfCodigoVersion').val(),
            indicador: 1
        };
        redirect(controlador + 'descargararchivo', data);        
    }
    else {
        alert("Por favor acepte los términos y condiciones del licenciamiento del MODPLAN.");
    }
};

listarVersionModplan = function () {
    $.ajax({
        type: 'POST',
        url: controlador + 'listado',
        success: function (evt) {
            $('#listadoPlanTransmision').html(evt);           

            $('.version-hijo').on('click', function () {
                var id = $(this).attr('id');
                cargarDetalleVersion(id);               
            });
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        }
    });
};

cargarDetalleVersion = function (id) {

    $.ajax({
        type: 'POST',
        url: controlador + 'detalle',
        data: {
            id: id
        },
        success: function (evt) {
            $('#divDetalle').html(evt);
            $('#cbTerminos').prop('checked', false);

            $('.resumen-file').on('click', function () {
                descargarArchivo(id, $(this).attr('id'));
            });      
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        }
    });
};

descargarArchivo = function (id, indicador) {

    if (indicador == 2) {
        var data = {
            idVersion: id,
            indicador: indicador
        };
        redirect(controlador + 'descargararchivo', data);
    }
    else {
        $('#validaciones').bPopup({
        });
    }   
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

mostrarMensaje = function (id, tipo, mensaje) {
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);
};