var controlador = siteRoot + 'mercadomayorista/visualizaciondatos/';

$(function () {

    $('#cbTipoEmpresa').on('change', function () {
        cargarListado();
    });

    $('#btnConsultar').on('click', function () {
        cargarListado();
    });

    cargarListado();
});

cargarListado = function () {
    $.ajax({
        type: 'POST',
        url: controlador + 'listado',
        data: {
            tipo: $('#cbTipoEmpresa').val()            
        },
        success: function (evt) {
            $('#listado').html(evt);
            $('#tabla').dataTable({
                "iDisplayLength": 25,
                "info": false
            });
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        }
    });
};

mostrarMensaje = function (id, tipo, mensaje) {
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);
};