var controlador = siteRoot + 'IndicadoresSup/numeral/';
$(function () {
    $('#txtFecha').Zebra_DatePicker({
        format: 'm Y',
        direction: -1
    });
    $('#tab-container').easytabs();
    $('#btnConsultar').on('click', function () {
        consultar();
    });
});

function consultar() {

    $.ajax({
        type: 'POST',
        url: controlador + 'lista',
        data: {
            fecha: $('#txtFecha').val()
        },
        success: function (evt) {
            $('#listado').html(evt);
        },
        error: function () {
            //mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        }
    });
}


