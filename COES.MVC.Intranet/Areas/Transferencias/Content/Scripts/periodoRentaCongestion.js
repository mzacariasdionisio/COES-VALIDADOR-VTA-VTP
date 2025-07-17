var controlador = siteRoot + "transferencias/transferenciarentacongestion/";

//Funciones de busqueda
$(document).ready(function () {
    mostrarPaginado();
    pintarBusqueda(1);
    
    $('#btnConsultar').click(function () {
        mostrarPaginado();
        pintarBusqueda(1);
    });
});

function pintarBusqueda(id) {
    $.ajax({
        type: 'POST',
        url: controlador + "listarperiodosrentacongestion",
        data: {            
            nroPagina: id
        },
        success: function (evt) {
            $('#listado').css("width", $('#mainLayout').width() + "px");
            $('#listado').html(evt);
            $('#tabla').dataTable({
                "scrollY": 480,
                "scrollX": true,
                "sDom": 't',
                "ordering": false,
                "bDestroy": true,
                "bPaginate": false,
                "iDisplayLength": 50
            });
            mostrarMensaje("Consulta generada.")
        },
        error: function () {
            mostrarAlerta("Lo sentimos, se ha producido un error");
        }
    });
};

function mostrarPaginado() {
    $.ajax({
        type: 'POST',
        url: controlador + 'paginado',
        success: function (evt) {
            $('#paginado').html(evt);
            mostrarPaginado();
        },
        error: function () {
            mostrarError("Error al cargar el paginado");
        }
    });
}

function mostrarExito(mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-exito');
}

function mostrarError(mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-error');
}

function mostrarAlerta(mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-alert');
}

function mostrarMensaje(mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-message');
}

function modificarPeriodoRentaCongestion(pericodi, recacodi) {
    window.location.href = siteRoot + "transferencias/calculorentacongestion/calculorentacongestion?pericodi=" + pericodi + "&recacodi=" + recacodi;
}

