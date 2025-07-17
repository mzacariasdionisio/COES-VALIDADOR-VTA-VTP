var controlador = siteRoot + 'planificacion/descargamodplan/';

$(function () {   

    $('#btnVerificar').on('click', function () {
        verificarAcceso();
    });
   
});

verificarAcceso = function () {
    $.ajax({
        type: 'POST',
        url: controlador + 'listado',
        data: {
            identificador: $('#hfCodigo').val(),
            codigo: $('#txtCodigo').val()
        },
        success: function (evt) {
            $('#divListado').html(evt);      

            $('#btnModplanCancelar').on('click', function () {
                $('#validaciones').bPopup().close();
            });

            $('#btnModplanAceptar').on('click', function () {
                aceptarTerminos();
            });
            
            $('.version-hijo').on('click', function () {
                var id = $(this).attr('id');
                cargarDetalleVersion(id);
            });

            if ($('#hfResultado').val() == '1') {
                $('#divDatos').hide();
            }
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        }
    });
};

aceptarTerminos = function () {
    if ($('#cbTerminos').prop('checked')) {

        var data = {
            idVersion: $('#hfCodigoVersion').val(),
            indicador: 1,
            identificador: $('#hfIndicadorAcceso').val()
        };
        redirect(controlador + 'descargararchivo', data);        
    }
    else {
        alert("Por favor acepte los términos y condiciones del licensamiento del MODPLAN.");
    }
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
            indicador: indicador,
            identificador: $('#hfIndicadorAcceso').val()
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

    $('#validaciones').bPopup().close();
    mostrarMensaje('mensaje', 'exito', 'La descarga se ha realizado correctamente.');
};

mostrarMensaje = function (id, tipo, mensaje) {
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);
};