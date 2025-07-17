var controlador = siteRoot + 'cortoplazo/consulta/';

$(function () {

    $('#txtFechaInicio').Zebra_DatePicker({
        pair: $('#txtFechaFin')
    });

    $('#txtFechaFin').Zebra_DatePicker({
        direction: true      
    });

    $('#btnConsultar').on('click', function () {
        consultar();
    });
});

consultar = function () {
    
    $.ajax({
        type: 'POST',
        url: controlador + "exportar",
        dataType: 'json',
        data: {
            tipo: $('#cbDatos').val(),
            fechaInicio: $('#txtFechaInicio').val(),
            fechaFin: $('#txtFechaFin').val()
        },
        cache: false,
        success: function (resultado) {
            if (resultado == 1) {
                location.href = controlador + "descargar";
            }
            else {
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
            }
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });

};

mostrarMensaje = function (id, tipo, mensaje) {
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);
}