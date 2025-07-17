var controlador = siteRoot + 'coordinacion/registroobservacion/'


$(document).ready(function () {    
   

    $('#btnProcesar').click(function () {
        procesar();
    });

    

});


procesar = function () {
    if ($('#cbEmpresa').val() > 0 && $('#txtCorreo').val() != '') {

        $.ajax({
            type: 'POST',
            url: controlador + 'procesarautomatico',
            data: {
                idEmpresa: $('#cbEmpresa').val(),
                correo: $('#txtCorreo').val(),
                tipo: $('#cbProceso').val()
            },
            dataType: 'json',
            success: function (result) {
                if (result == 1) {
                    mostrarMensaje('mensaje', 'exito', 'Se procesó correctamente.');
                }
                else {
                    mostrarMensaje('mensaje', 'error', result);
                }
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            }
        })
    }
    else {
        mostrarMensaje('mensaje', 'alert', 'Seleccione una empresa e ingrese correo válido.');
    }
}

mostrarMensaje = function (id, tipo, mensaje) {
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);
}
