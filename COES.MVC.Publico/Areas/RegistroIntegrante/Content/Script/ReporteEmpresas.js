var controlador = siteRoot + 'RegistroIntegrante/Reporte/'

$(function () {

    $('#btnConsultar').click(function () {
        buscarEvento();
    });

    $('#btnExportar').click(function () {
        ExportarExcelEmpresasPublico();
    });

    $('#btnHistorico').click(function () {
        document.location.href = controlador + 'historico';
    });

    //Seleccionar Todos
    $('#cbTipoEmpresa > option[value="-1"]').attr('selected', 'selected');
    $('#cbTipoSolicitudes > option[value="-1"]').attr('selected', 'selected');

    //Eliminar los tipos de solicitudes que no se requiere
    $("#cbTipoSolicitudes ").find("option[value='2']").remove();  
    $("#cbTipoSolicitudes ").find("option[value='5']").remove();  


    buscarEvento();
});

buscarEvento = function () {
    mostrarListado();
};

mostrarListado = function () {
    $.ajax({
        type: 'POST',
        url: controlador + "ListarEmpresasPublico",
        data: {
            tipoempresa: $('#cbTipoEmpresa').val(),
            tiposolicitud: $('#cbTipoSolicitudes').val()
        },
        success: function (evt) {
            $('#listado').css("width", $('#mainLayout').width() + "px");

            $('#listado').html(evt);
            $('#tabla').dataTable({
                "iDisplayLength": 50
            });
        },
        error: function () {
            mostrarError();
        }
    });
};

mostrarError = function () {
    $('#mensaje').removeClass();
    $('#mensaje').addClass('action-error');
    $('#mensaje').html("Ha ocurrido un error.");
};

mostrarMensaje = function (mensaje) {
    alert(mensaje);
};

ExportarExcelEmpresasPublico = function () {

    $.ajax({
        type: 'POST',
        url: controlador + "GenerarArchivoReporteEmpresasPublico",
        data: {
            tipoempresa: $('#cbTipoEmpresa').val(),
            tiposolicitud: $('#cbTipoSolicitudes').val()
        },
        dataType: 'json',
        success: function (result) {
            if (result == 1) {
                window.location = controlador + "ExportarExcelEmpresasPublico";
            }
            if (result == -1) {
                alert("Error al imprimir.");
            }
        },
        error: function () {
            mostrarError();
        }
    });
};
