var controlador = siteRoot + 'RegistroIntegrante/Reporte/'

$(function () {

    $('#chkVigencia').change(function () {

        if (!$(this).is(':checked'))
            $("#txtFechaAlertaVigenciaVencida").prop('disabled', true);
        else
            $("#txtFechaAlertaVigenciaVencida").prop('disabled', false);

    });

    $("input[type=text]").keyup(function () {
        $("#" + this.id).val($("#" + this.id).val().toUpperCase());
    });

    $('#txtFechaAlertaVigenciaVencida').Zebra_DatePicker({
    });

    $('#btnConsultar').click(function () {
        buscarEvento();
    });

    $('#btnAlertaVigenciaVencida').click(function () {
        AlertaVigenciaVencida();
    });

    $('#btnCorreoInformeRpteLegal').click(function () {
        CorreoInformeRpteLegal();
    });

    buscarEvento();
});

buscarEvento = function () {
    pintarPaginado();
    mostrarListado(1);
}

pintarPaginado = function () {

    var fvigenciapoder = "";

    if ($('#chkVigencia').prop('checked')) {
        fvigenciapoder = $('#txtFechaAlertaVigenciaVencida').val();
    }


    $.ajax({
        type: 'POST',
        url: controlador + "paginadoRepresentanteLegal",
        data: {
            tipoempresa: $('#cbTipoEmpresa').val(),
            nombre: $('#txtNombreFiltro').val(),
            tiporepresentante: $('#cbTipoRepresentante').val(),
            tiporepresentantecontacto: 'L',
            estado: $('#cbEstado').val(),
            fecha: fvigenciapoder
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

    var fvigenciapoder = "";

    if ($('#chkVigencia').prop('checked')) {
        fvigenciapoder = $('#txtFechaAlertaVigenciaVencida').val();
    }

    $('#hfNroPagina').val(nroPagina);
    $.ajax({
        type: 'POST',
        url: controlador + "listadoRepresentantesLegales",
        data: {
            tipoempresa: $('#cbTipoEmpresa').val(),
            nombre: $('#txtNombreFiltro').val(),
            tiporepresentante: $('#cbTipoRepresentante').val(),
            tiporepresentantecontacto: 'L',
            estado: $('#cbEstado').val(),
            fecha: fvigenciapoder,
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

AlertaVigenciaVencida = function () {
    $.ajax({
        type: 'POST',
        url: controlador + "AlertaVigenciaVencida",
        success: function (evt) {
            mostrarMensaje("Se finalizo la acción solicitada.");
        },
        error: function () {
            mostrarError();
        }
    });

}

CorreoInformeRpteLegal = function () {

    $.ajax({
        type: 'POST',
        url: controlador + "CorreoInformeRpteLegal",
        success: function (evt) {
            mostrarMensaje("Se finalizo la acción solicitada.");
        },
        error: function () {
            mostrarError();
        }
    });

}

ExportarExcelRepresentanteLegal = function () {
    var fvigenciapoder = "";

    if ($('#chkVigencia').prop('checked')) {
        fvigenciapoder = $('#txtFechaAlertaVigenciaVencida').val();
    }


    $.ajax({
        type: 'POST',
        url: controlador + "GenerarArchivoReporteRepresentantesLegales",
        data: {
            tipoempresa: $('#cbTipoEmpresa').val(),
            nombre: $('#txtNombreFiltro').val(),
            tiporepresentante: $('#cbTipoRepresentante').val(),
            tiporepresentantecontacto: 'L',
            estado: $('#cbEstado').val(),
            fecha: fvigenciapoder
        },
        dataType: 'json',
        success: function (result) {
            if (result == 1) {
                window.location = controlador + "ExportarExcelRepresentanteLegal";
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


ExportarTextoRepresentanteLegal = function () {

    $.ajax({
        type: 'POST',
        url: controlador + "ExportarTextoRepresentanteLegal",
        success: function (evt) {
            mostrarMensaje("Se finalizo la acción solicitada.");
        },
        error: function () {
            mostrarError();
        }
    });

}


ExportarWordRepresentanteLegal = function () {

    $.ajax({
        type: 'POST',
        url: controlador + "ExportarWordRepresentanteLegal",
        success: function (evt) {
            mostrarMensaje("Se finalizo la acción solicitada.");
        },
        error: function () {
            mostrarError();
        }
    });

}
