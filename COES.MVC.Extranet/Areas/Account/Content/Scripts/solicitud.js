var controlador = siteRoot + 'account/solicitar/'

$(document).ready(function () {

    cargarListado();

    $('#btnConsultar').click(function () {
        cargarListado();
    });

    $('#btnNuevo').click(function () {
        verSolicitud(0);
    });

});

verSolicitud = function (id) {
    document.location.href = controlador + 'nuevo?id=' + id;
}

anularSolicitud = function (id) {
    if (confirm('¿Está seguro de anular esta solicitud?')) {
        $.ajax({
            type: 'POST',
            url: controlador + 'anularsolicitud',
            data: {
                idSolicitud: id
            },
            dataType: 'json',
            cache: false,
            success: function (result) {
                if (result == 1) {
                    cargarListado();
                    $('#mensaje').removeClass();
                    $('#mensaje').addClass('action-exito');
                    $('#mensaje').text('La operación se realizó correctamente.');
                }
                else {
                    mostrarError();
                }
            },
            error: function () {
                mostrarError();
            }
        });
    }
}

cargarListado = function () {
    $.ajax({
        type: 'POST',
        url: controlador + 'grilla',
        data: {
            idEmpresa: $('#hfEmpresa').val(),
            estado: $('#cbEstado').val()
        },
        success: function (evt) {
            $('#listado').html(evt);
            $('#tabla').dataTable({
            });
        },
        error: function () {
            mostrarError();
        }
    });
}

mostrarError = function () {
    alert("Error");
}