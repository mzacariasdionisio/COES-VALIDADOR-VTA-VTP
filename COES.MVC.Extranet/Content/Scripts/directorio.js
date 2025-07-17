var controlador = siteRoot + 'directorio/';

$(function () {
    consultar();

    $('#btnConsultar').click(function () {
        consultar();
    });
});

consultar = function () {
    $.ajax({
        type: 'POST',
        url: controlador + 'listado',
        data: {
            idArea: $('#cbArea').val()
        },
        success: function (evt) {
            $('#listado').html(evt);
            $('#tabla').dataTable({

            });
        },
        error: function () {
            alert("Error");
        }
    });
}