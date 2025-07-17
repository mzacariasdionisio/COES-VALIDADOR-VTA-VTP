var controlador = siteRoot + 'admin/reportesolicitud/'

$(function () {

    $('#btnConsultar').click(function () {
        consultar();
    });

    $('#btnExportar').click(function () {
        exportar();
    });

    consultar();
});

consultar = function () {
    $.ajax({
        type: 'POST',
        url: controlador + 'consultasolicitud',
        data: {
            empresa: $('#cbEmpresa').val(),
            modulo: $('#cbModulo').val(),
            estado: $('#cbEstado').val()
        },
        dataType: 'json',
        success: function (result) {
            $('#listado').html(result);
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
        data: {
            empresa: $('#cbEmpresa').val(),
            modulo: $('#cbModulo').val(),
            estado: $('#cbEstado').val()
        },
        dataType: 'json',
        cache: false,
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

mostrarSolicitud = function (idEmpresa, idModulo, estado) {
    listarSolicitudes(idEmpresa, idModulo, estado);
}

mostrarSolicitudPorModulo = function (idEmpresa, idModulo) {
    listarSolicitudes(idEmpresa, idModulo, "-1");
}

mostrarSolicitudPorEmpresa = function (idEmpresa) {
    listarSolicitudes(idEmpresa, -1, "-1");
}

listarSolicitudes = function (idEmpresa, idModulo, estado) {

    $.ajax({
        type: 'POST',
        url: controlador + 'listado',
        data: {
            idEmpresa: idEmpresa,
            idModulo: idModulo,
            estado: estado
        },
        success: function (evt) {

            $('#contenidoDetalle').html(evt);
            setTimeout(function () {
                $('#tabla').dataTable({
                });
                $('#popupDetalle').bPopup({
                    autoClose: false
                });
            }, 50);

        },
        error: function () {
            mostrarError();
        }
    });
}

mostrarError = function () {
    $('#mensaje').removeClass();
    $('#mensaje').addClass('action-error');
    $('#mensaje').text('Ha ocurrido un error.');
}