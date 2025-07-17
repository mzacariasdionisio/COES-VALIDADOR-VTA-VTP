var controladorBusqueda = siteRoot + 'coordinacion/registroobservacion/'

$(function () {

    $('#btnBuscar').click(function () {
        buscarCanal();
    });

    $('#txtFiltro').focus();

    $('#searchContentCanal').keypress(function (e) {
        if (e.keyCode == '13') {
            $('#btnBuscar').click();
        }
    });
});


function buscarCanal() {
    pintarPaginado();
    pintarBusqueda(1);   
}

function pintarPaginado() {
    
    $.ajax({
        type: "POST",
        url: controladorBusqueda + "paginadocanal",
        data: {
            idEmpresa: $('#hfIdEmpresaBusqueda').val(),
            idZona: $('#cbZona').val(),
            idTipo: $('#cbTipo').val(),
            filtro: $('#txtFiltro').val()
        },
        global: false,
        success: function (evt) {
            $('#cntPaginado').html(evt);
            mostrarPaginado();
        },
        error: function (req, status, error) {
            mostrarMensajePopup('messageCanal', 'error', 'Ha ocurrido un error.');
        }
    });
}

function pintarBusqueda(nroPagina) {

    $.ajax({
        type: "POST",
        url: controladorBusqueda + "listacanal",
        global: false,
        data: {
            idEmpresa: $('#hfIdEmpresaBusqueda').val(),
            idZona: $('#cbZona').val(),
            idTipo: $('#cbTipo').val(),
            filtro: $('#txtFiltro').val(),
            nroPagina: nroPagina
        },
        success: function (evt) {
            $('#cntCanal').html(evt);
            $('#messageCanal').hide();
        },
        error: function (req, status, error) {
            mostrarMensajePopup('messageCanal', 'error', 'Ha ocurrido un error.');
        }
    });
}

mostrarMensajePopup = function (id, tipo, mensaje) {
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);
    $('#' + id).show();
}

