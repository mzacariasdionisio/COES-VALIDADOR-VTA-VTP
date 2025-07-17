var controlador = siteRoot + 'registrointegrante/reporte/';

$(function () {

    $('#btnConsultar').on('click', function () {
        consultar();
    });

    consultar();
});

consultar = function () {

    $.ajax({
        type: 'POST',
        url: controlador + 'listhistorico',
        data: {
            panio: $('#cbAnio').val(),
            ptipo: $('#cbTipo').val()
        },
        success: function (evt) {
            $('#listado').html(evt);
           
        },
        error: function () {

        }
    });    
};