var controlador = siteRoot + 'marconormativo/decisionejecutiva/';

$(function () {

    $('#btnNuevo').on('click', function () {
        editar(0, $('#cbTipoDocumento').val());
    });   

    $('#btnConsultar').on('click', function () {
        consultar();
    });

    consultar();
});

consultar = function () {
    $.ajax({
        type: 'POST',
        url: controlador + 'listar',
        data: {
            tipo: $('#cbTipoDocumento').val()
        },
        success: function (evt) {
            $('#listado').html(evt);
            $('#tabla').dataTable({
                "iDisplayLength": 25
            });
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    })
}

eliminar = function (id) {
    if (confirm('¿Está seguro de realizar esta operación?')) {
        
        $.ajax({
            type: 'POST',
            url: controlador + 'eliminar',
            data: {
                id: id
            },
            dataType: 'json',
            success: function (result) {
                if (result == 1) {
                    mostrarMensaje('mensaje', 'exito', 'El registro fué eliminado correctamente.');
                    consultar();
                }
                else {
                    mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
                }
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
            }
        })
    }
}

editar = function (id, tipo) {
    document.location.href = controlador + 'editar?id=' + id + '&tipo=' + tipo;
}

mostrarMensaje = function (id, tipo, mensaje) {
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);
}

descargar = function (url) {
    document.location.href = controlador + "Download?file=" + url;
}