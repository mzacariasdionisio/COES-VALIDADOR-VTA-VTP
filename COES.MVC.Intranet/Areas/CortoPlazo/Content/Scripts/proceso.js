var controlador = siteRoot + 'cortoplazo/proceso/';

$(function () {
       
    $('#tab-container').easytabs({
        animate: false
    });
   
    $('#txtFechaProceso').Zebra_DatePicker({
        readonly_element: false,       
        onSelect: function (date) {
            $('#txtFechaProceso').val(date + " 00:00");
        }
    });

    $('#txtFechaProceso').inputmask({
        mask: "1/2/y h:s",
        placeholder: "dd/mm/yyyy hh:mm",
        alias: "datetime",
        hourFormat: "24"
    });

    $('#btnDescargar').on('click', function () {
        descargar();
    });

    $('#btnDescargar').hide();
});

descargar = function () {
    document.location.href = controlador + 'descargar?fecha=' + $('#txtFechaProceso').val();
}

procesar = function () {

    if ($('#txtFechaProceso').val() != "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'procesar',
            data: {
                fecha: $('#txtFechaProceso').val()
            },
            dataType: 'json',
            success: function (result) {
                if (result != -1) {
                    $('#contenidoResultado').html(result);
                    mostrarMensaje('mensaje', 'exito', 'El proceso se ha ejecutado correctamente.');
                    $('#tab-container').easytabs('select', '#resultado');
                    $('#btnDescargar').show();
                }
                else {
                    mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
                }
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            }
        })
    }
    else
    {
        mostrarMensaje('mensaje', 'alert', 'Seleccione fecha');
    }
}

mostrarMensaje = function (id, tipo, mensaje) {
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);
}