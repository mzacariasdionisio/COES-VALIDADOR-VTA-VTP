var controlador = siteRoot + 'RegistroIntegrante/Historico/'

$(function () {

    $("input[type=text]").keyup(function () {
        $("#" + this.id).val($("#" + this.id).val().toUpperCase());
    });

    $('#FechaDesde').Zebra_DatePicker({
    });

    $('#FechaHasta').Zebra_DatePicker({
    });

    $('#cbTipoEmpresa').change(function () {
        cargarEmpresas();
    });

    $('#btnConsultar').click(function () {
        buscarEvento();
    });

    buscarEvento();
});

cargarEmpresas = function () {
    $.ajax({
        type: 'POST',
        url: controlador + 'cargarempresas',
        dataType: 'json',
        data: { idTipoEmpresa: $('#cbTipoEmpresa').val() },
        cache: false,
        success: function (aData) {
            $('#cbEmpresa').get(0).options.length = 0;
            $('#cbEmpresa').get(0).options[0] = new Option("--TODOS--", "");
            $.each(aData, function (i, item) {
                $('#cbEmpresa').get(0).options[$('#cbEmpresa').get(0).options.length] = new Option(item.Text, item.Value);
            });
        },
        error: function () {
            mostrarError();
        }
    });
}


buscarEvento = function () {

    pintarPaginado();
    mostrarListado(1);
}

pintarPaginado = function () {

    var finicio = $('#FechaDesde').val();
    var ffin = $('#FechaHasta').val();


    $.ajax({
        type: 'POST',
        url: controlador + "PaginadoHistoricoRevisiones",
        data: {
            finicios: finicio,
            ffins: ffin,
            tipoempresa: $('#cbTipoEmpresa').val(),
            tiporevision: $('#cbTipoRevision').val(),
            empresa: $('#cbEmpresa').val()
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
        url: controlador + "ListadoHistoricoRevisiones",
        data: {
            finicios: finicio,
            ffins: ffin,
            tipoempresa: $('#cbTipoEmpresa').val(),
            tiporevision: $('#cbTipoRevision').val(),
            empresa: $('#cbEmpresa').val(),
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

ExportarExcel = function () {
    var finicio = $('#FechaDesde').val();
    var ffin = $('#FechaHasta').val();

    $.ajax({
        type: 'POST',
        url: controlador + "GenerarArchivoReporteHistoricoRevisiones",
        data: {
            finicios: finicio,
            ffins: ffin,
            tipoempresa: $('#cbTipoEmpresa').val(),
            tiporevision: $('#cbTipoRevision').val(),
            empresa: $('#cbEmpresa').val()
        },
        dataType: 'json',
        success: function (result) {
            if (result == 1) {
                window.location = controlador + "ExportarExcelHistoricoRevisiones";
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
