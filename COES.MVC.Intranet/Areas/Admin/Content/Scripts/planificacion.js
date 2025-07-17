var controlador = siteRoot + 'admin/planificacion/';

$(function () {

    $('#btnConsultar').on('click', function () {
        consultar();
    });

    $('#txtFechaInicio').Zebra_DatePicker({

    });

    $('#txtFechaFin').Zebra_DatePicker({

    });

    consultar();
});

consultar = function () {
    $.ajax({
        type: 'POST',
        url: controlador + "listado",
        data: {
            fechaInicio: $('#txtFechaInicio').val(),
            fechaFin: $('#txtFechaFin').val()
        },
        success: function (evt) {
            $('#listado').html(evt);
            $('#tabla').dataTable({
                "iDisplayLength": 50
            });
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}