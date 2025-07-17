var controlador = siteRoot + 'RegistroIntegrante/Solicitud/'

$(function () {

    $("input[type=text]").keyup(function () {
        $("#" + this.id).val($("#" + this.id).val().toUpperCase());
    });

    $('#btnConsultar').click(function () {
        buscarEvento();
    });
    
    $('#btnNuevaSolCambioDen').click(function () {
        nuevaSolicitud("SolCambioDenominacion");
    });
    $('#btnNuevaSolCambioRpte').click(function () {
        nuevaSolicitud("SolCambioRepresentante");
    });    
    $('#btnNuevaSolBajaEmp').click(function () {
        nuevaSolicitud("SolBajaEmpresa");
    });
    $('#btnNuevaSolFusionEmp').click(function () {
        nuevaSolicitud("SolFusionEmpresa");
    });
    $('#btnNuevaSolCambioTipo').click(function () {
        nuevaSolicitud("SolCambioTipo");
    });
    
    

    buscarEvento();
});

buscarEvento = function () {
    pintarPaginado();
    mostrarListado(1);
}

nuevaSolicitud = function (view) {
    window.location = controlador + "nuevo?view=" + view;
}


pintarPaginado = function () {
    $.ajax({
        type: 'POST',
        url: controlador + "paginado",
        data: {
            soliestado: $('#cbEstadoSolicitud').val()
        },
        success: function (evt) {
            $('#paginado').html(evt);
            //mostrarPaginado();
        },
        error: function () {
            mostrarError();
        }
    });
}

mostrarListado = function (nroPagina) {
    $('#hfNroPagina').val(nroPagina);
    $.ajax({
        type: 'POST',
        url: controlador + "listado",
        data: {
            soliestado: $('#cbEstadoSolicitud').val(),
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

verDetalle = function (solicodi) {
    $.ajax({
        type: 'POST',
        url: controlador + "verificarTipoSoli",
        data: {
            solicodi: solicodi
        },
        success: function (evt) {
            window.location = controlador + "verdetalle?view=" + evt.view + "&solicodi=" + evt.codisoli;
        },
        error: function () {
            mostrarError();
        }
    });
}

