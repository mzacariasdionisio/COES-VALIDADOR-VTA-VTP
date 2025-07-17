
function cargarPrimeraPagina() {
    var id = 1;
    $('#hfPaginaActual').val(id);
    mostrarPaginado();
    pintarBusqueda(id);
}

function cargarAnteriorPagina() {
    var id = $('#hfPaginaActual').val();
    if (id > 1) {
        id = id - 1;
        $('#hfPaginaActual').val(id);
        mostrarPaginado();
        pintarBusqueda(id);
    }
}

function cargarPagina(id) {
    $('#hfPaginaActual').val(id);
    mostrarPaginado();
    pintarBusqueda(id);
}

function cargarSiguientePagina() {
    var id = parseInt($('#hfPaginaActual').val()) + 1;
    $('#hfPaginaActual').val(id);
    mostrarPaginado();
    pintarBusqueda(id);
}

function cargarUltimaPagina() {
    var id = $('#hfNroPaginas').val();
    $('#hfPaginaActual').val(id);
    mostrarPaginado();
    pintarBusqueda(id);
}

function mostrarPaginado() {
    var nroToShow = parseInt($('#hfNroMostrar').val());
    var nroPaginas = parseInt($('#hfNroPaginas').val());
    var nroActual = parseInt($('#hfPaginaActual').val());

    $('.pag-ini').css('display', 'none');
    $('.pag-item').css('display', 'none');
    $('.pag-fin').css('display', 'none');
    $('.pag-item').removeClass('paginado-activo');

    $('#pag' + nroActual).addClass('paginado-activo');

    if (nroToShow - nroPaginas >= 0) {
        $('.pag-item').css('display', 'block');
        $('.pag-ini').css('display', 'none');
        $('.pag-fin').css('display', 'none');
    }
    else {
        $('.pag-fin').css('display', 'block');
        if (nroActual > 1) {
            $('.pag-ini').css('display', 'block');
        }
        var anterior = 0;
        var siguiente = 0;

        if (nroActual == 1) {

            anterior = 1;
            siguiente = nroToShow;
        }
        else {
            if (nroActual + nroToShow - 1 - nroPaginas > 0) {
                siguiente = nroPaginas;
                anterior = nroPaginas - nroToShow + 1;
            }
            else {
                anterior = nroActual;
                siguiente = nroActual + nroToShow - 1;
            }
        }

        for (j = anterior; j <= siguiente; j++) {
            $('#pag' + j).css('display', 'block')
        }
    }
}