var controlador = siteRoot + 'admin/usuarios/'

$(function () {

    $('#txtFechaInicio').Zebra_DatePicker({
    });

    $('#txtFechaFin').Zebra_DatePicker({
    });

    $('#btnConsultar').click(function () {
        consultar();
    });

    $('#btnCancelar').click(function () {
        document.location.href = siteRoot + 'home/default';
    });

    consultar();

});


consultar = function () {
    
    $.ajax({
        type: 'POST',
        url: controlador + 'listado',
        data: {
            idEmpresa: $('#cbEmpresa').val(),
            idModulo: $('#cbModulo').val(),
            estado: $('#cbEstado').val(),
            fechaInicio: $('#txtFechaInicio').val(),
            fechaFin: $('#txtFechaFin').val()
        },
        success: function (evt) {
            $('#listado').html(evt);
            $('#tabla').dataTable({
                "iDisplayLength": 15
            });
        },
        error: function () {
            mostrarError();
        }
    });
}

editar = function (id) {

    document.location.href = controlador + 'detalle?id=' + id;
}

mostrarError = function () {
    $('#mensaje').removeClass();
    $('#mensaje').text('Ha ocurrido un error');
    $('#mensaje').addClass('action-error');
}
