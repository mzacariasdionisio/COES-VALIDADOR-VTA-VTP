var controlador = siteRoot + 'RegistroIntegrante/Reporte/'

$(function () {

    $("input[type=text]").keyup(function () {
        $("#" + this.id).val($("#" + this.id).val().toUpperCase());
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
    $.ajax({
        type: 'POST',
        url: controlador + "paginadoContacto",
        data: {
            tipoempresa: $('#cbTipoEmpresa').val(),
            nombre: $('#txtNombreFiltro').val()      
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
    $('#hfNroPagina').val(nroPagina);
    $.ajax({
        type: 'POST',
        url: controlador + "listadoContactos",
        data: {
            tipoempresa: $('#cbTipoEmpresa').val(),
            nombre: $('#txtNombreFiltro').val(),
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


ExportarExcelContactos = function () {
    
    $.ajax({
        type: 'POST',
        url: controlador + "GenerarArchivoReporteRepresentantesLegales",
        data: {
            tipoempresa: $('#cbTipoEmpresa').val(),
            nombre: $('#txtNombreFiltro').val(),
            tiporepresentante: '-1',
            tiporepresentantecontacto: '-1',
            estado: 'N',
            fecha: ''
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


ExportarTextoContactos = function () {

    $.ajax({
        type: 'POST',
        url: controlador + "ExportarTextoContactos",
        success: function (evt) {
            mostrarMensaje("Se finalizo la acción solicitada.");
        },
        error: function () {
            mostrarError();
        }
    });

}