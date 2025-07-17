var controlador = siteRoot + 'operacion/costocombustible/';

$(function () {

    $('#btnBuscar').click(function () {
        cargarListado();
    });

    $('#btnExportar').click(function () {
        exportar();
    });

    cargarListado();
});

cargarListado = function () {
        
    $.ajax({
        type: 'POST',
        url: controlador + 'listado',
        data: {
            idEmpresa: $('#cbEmpresa').val()
        },
        success: function (evt) {
            $('#listado').html(evt);
            $('#tabla').dataTable({
                "iDisplayLength": 50,
                "sDom": 'ft',
            });
        },
        error: function () {
            mostrarError();
        }
    });
}

exportar = function () {
    $.ajax({
        type: 'POST',
        url: controlador + "exportar",
        dataType: 'json',
        cache: false,
        data: {
            idEmpresa: $('#cbEmpresa').val()
        },
        success: function (resultado) {
            if (resultado == 1) {
                location.href = controlador + "descargar";
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
 mostrarError = function () {
    alert("Error");
}