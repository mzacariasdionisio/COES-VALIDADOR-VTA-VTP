var controlador = siteRoot + 'modelo/';

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
            indicador: $('#hfCodigoArchivo').val()
        };
        redirect(controlador + 'descargararchivo', data);        
    }
    else {
        alert("Por favor acepte los términos y condiciones del licenciamiento.");
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
                descargarArchivo(id, $(this).attr('data-id'),$(this).attr('id'));
            });      
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        }
    });
};

descargarArchivo = function (id, indicador, codigo) {

    var aplicativo = $('#hfCodigoModelo').val();

    if (aplicativo == "2" || aplicativo == "3" || aplicativo == "5") {


         if (indicador == 2) {
            var data = {
                idVersion: id,
                indicador: codigo
            };
            redirect(controlador + 'descargararchivo', data);
        }
         else {
             $('#hfCodigoArchivo').val(codigo); 
             $('#validaciones').bPopup({
             });
             if ($('#hfCodigoArchivo').val() == "86") {
                 $("#tcYupanaCodigoFuente").show();
                 $("#tcYupana").hide();
                 $('#tituloTyC').hide();
             } else {
                                
                 $("#tcYupana").show();
                 $("#tcYupanaCodigoFuente").hide();     
                 $('#tituloTyC').show();
             }
        }
    }
    else {
        var data = {
            idVersion: id,
            indicador: codigo
        };
        redirect(controlador + 'descargararchivo', data);
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