var controlador = siteRoot + "Proveedores/";

$(function () {
    $('#btnConsultar').click(function () {
        listar();
    });

    $('#txtFechaDesde').Zebra_DatePicker();
    $('#txtFechaHasta').Zebra_DatePicker();

    listar();
});

listar = function () {
    $.ajax({
        type: 'POST',
        url: controlador + 'list',
        data : {
            nombre: $('#txtNombre').val(),
            tipo: $('#cbTipoProveedor').val(),
            fechaDesde: $('#txtFechaDesde').val(),
            fechaHasta: $('#txtFechaHasta').val()
        },
        success: function (result) {
            $('#listaproveedor').html(result);
            $('#tablaProveedor').dataTable({                                          
                "iDisplayLength": 25,
                "sDom": 'ftip',
            });
        },
        error: function () {
            alert("Error");
        }
    });
}   
