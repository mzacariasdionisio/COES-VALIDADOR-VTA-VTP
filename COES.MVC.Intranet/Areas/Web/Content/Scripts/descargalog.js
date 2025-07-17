var controlador = siteRoot + 'web/descargalog/';

$(function () {


    

    $('#txtFechaInicio').Zebra_DatePicker({
        pair: $('#txtFechaFin'),
        onSelect: function (date) {
            var date1 = getFecha(date);
            var date2 = getFecha($('#txtFechaFin').val());
            if (date1 > date2) {
                $('#txtFechaFin').val(date);
            }
        }
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
        url: controlador + 'listar',
        data: {
            url: "Mercado Mayorista/Liquidaciones del MME/",
            fechaInicio: $('#txtFechaInicio').val(),
            fechaFin: $('#txtFechaFin').val(),
       
        },
        success: function (evt) {
            $('#listado').html(evt);
            $('#tabla').dataTable({
                "iDisplayLength": 20
            });
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
}
