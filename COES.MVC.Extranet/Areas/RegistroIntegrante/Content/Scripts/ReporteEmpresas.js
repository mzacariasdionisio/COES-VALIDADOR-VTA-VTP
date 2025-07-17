var controlador = siteRoot + 'RegistroIntegrante/Reporte/'

$(function () {

    $("input[type=text]").keyup(function () {
        $("#" + this.id).val($("#" + this.id).val().toUpperCase());
    });

    $('#FechaDesde').Zebra_DatePicker({
    });

    $('#FechaHasta').Zebra_DatePicker({
    });

    $('#btnConsultar').click(function () {
        buscarEvento();
    });

    buscarEvento();
});

buscarEvento = function () {
    pintarPaginado();
    mostrarListado(1);
}

pintarPaginado = function () {

    var finicio = $('#FechaDesde').val();
    var ffin = $('#FechaHasta').val();

    $.ajax({
        type: 'POST',
        url: controlador + "paginadoEmpresa",
        data: {
            finicios: finicio,
            ffins: ffin,
            tipoempresa: $('#cbTipoEmpresa').val(),
            nombre: $('#txtNombreFiltro').val(),
            tipomodalidad: $('#cbTipoModalidad').val()
        },
        success: function (evt) {
            $('#paginado').html(evt);
            mostrarPaginado();
        },
        error: function () {
            //mostrarError();
        }
    });
}

mostrarListado = function (nroPagina) {

    var finicio = $('#FechaDesde').val();
    var ffin = $('#FechaHasta').val();

    $('#hfNroPagina').val(nroPagina);
    $.ajax({
        type: 'POST',
        url: controlador + "listadoEmpresas",
        data: {
            finicios: finicio,
            ffins: ffin,
            tipoempresa: $('#cbTipoEmpresa').val(),
            nombre: $('#txtNombreFiltro').val(),
            tipomodalidad: $('#cbTipoModalidad').val(),
            nroPagina: nroPagina
        },
        success: function (evt) {
            $('#listado').css("width", $('#mainLayout').width() + "px");

            $('#listado').html(evt);
            $('#tabla').dataTable({
                "scrollY": 430,
                "scrollX": true,
                "sDom": 't',
                "ordering": false,
                "iDisplayLength": 50
            });
        },
        error: function () {
            mostrarError();
        }
    });
}

pintarBusqueda = function (nroPagina) {
    mostrarListado(nroPagina);
}

mostrarError = function () {
    $('#mensaje').removeClass();
    $('#mensaje').addClass('action-error');
    $('#mensaje').html("Ha ocurrido un error.");
}

mostrarMensaje = function (mensaje) {
    alert(mensaje);
}

ExportarExcelEmpresa = function () {
    var finicio = $('#FechaDesde').val();
    var ffin = $('#FechaHasta').val();

    $.ajax({
        type: 'POST',
        url: controlador + "GenerarArchivoReporteEmpresa",
        data: {
            finicios: finicio,
            ffins: ffin,
            tipoempresa: $('#cbTipoEmpresa').val(),
            nombre: $('#txtNombreFiltro').val(),
            tipomodalidad: $('#cbTipoModalidad').val()
        },
        dataType: 'json',
        success: function (result) {
            if (result == 1) {
                window.location = controlador + "ExportarExcelEmpresa";
            }
            if (result == -1) {
                alert("Error al imprimir.");
            }
        },
        error: function () {
            mostrarError();
        }
    });

}
