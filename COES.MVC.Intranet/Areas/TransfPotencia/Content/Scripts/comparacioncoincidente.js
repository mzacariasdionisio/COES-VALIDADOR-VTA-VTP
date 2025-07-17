var controler = siteRoot + "transfpotencia/comparacioncoincidente/";

var error = [];

$(document).ready(function () {

    $('#btnConsultar').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        buscar();
    });

    $('#btnExportar').click(function (e) {
        e.preventDefault();
        exportarExcel();
    });

});

exportarExcel = function () {
    $.ajax({
        type: 'POST',
        url: controler + 'generarexcel',
        data: {
            NroPagina: $('#hfNroPagina').val(),
            genemprcodi: null,
            cliemprcodi: null,
            barrcoditra: null,
            barrcodisum: null,
            tipConCodi: null,
            tipUsuCodi: null,
            estado: '',
            codigo: '',
            pericodi: $("#pericodi").val(),
            recpotcodi: $("#recpotcodi").val()
        },
        dataType: 'json',
        success: function (result) {
            if (result != -1) {
                window.location = controler + 'abrirexcel?file=' + result;
            }
            else {
                mostrarMensaje('mensaje', 'Ha ocurrido un error', $tipoMensajeError, $modoMensajeCuadro);
            }
            
        },
        error: function () {
            mostrarError();
        }
    });
}

Recargar = function () {
    var cmbPericodi = document.getElementById('pericodi');
    window.location.href = controler + "index?pericodi=" + cmbPericodi.value;
}

function buscar() {

    pintarPaginado(function () {
        mostrarListado($('#paginaActual').val());
    });

}

function pintarPaginado(callback) {
    $.ajax({
        type: 'POST',
        url: controler + "paginado",
        data: {
            paginadoActual: $('#paginaActual').val()
        }, success: function (evt) {
            $('#paginado').html(evt);
            mostrarPaginado();
            if (callback != null)
                callback();
        },
        error: function () {
            alert("Lo sentimos, ha ocurrido un error inesperado");
        }
    });
}

function mostrarListado(nroPagina) {
    $('#hfNroPagina').val(nroPagina);
    $.ajax({
        type: 'POST',
        url: controler + "Lista",
        data: {
            NroPagina: $('#hfNroPagina').val(),
            genemprcodi: null,
            cliemprcodi: null,
            barrcoditra: null,
            barrcodisum: null,
            tipConCodi: null,
            tipUsuCodi: null,
            estado: '',
            codigo: '',
            pericodi: $("#pericodi").val(),
            recpotcodi: $("#recpotcodi").val()
        },
        success: function (evt) {
            $('#listado').html(evt);


            $('#listado table > tbody > tr').hover(function (event) {
                $('#listado table > tbody > tr').removeClass('table-row-hover')
                $('#listado table > tbody > tr[data-row-for-id] > td[data-row-index="0"]').removeClass('table-row-hover')
                let target = $(event.currentTarget);

                if (target.hasClass('child')) {
                    let forId = target.attr('data-row-for-id');
                    let index = target.attr('data-row-index');
                    if (index != 0) {
                        target.addClass('table-row-hover')
                        $('#listado table > tbody > tr[data-row-for-id="' + forId + '"][data-row-index="0"] td[data-row-index="0"]').addClass('table-row-hover')
                    }
                    $('#listado table > tbody > tr[data-row-id="' + forId + '"]').addClass('table-row-hover')
                } else {
                    let forId = target.attr('data-row-id');

                    target.addClass('table-row-hover')
                    $('#listado table > tbody > tr[data-row-for-id="' + forId + '"]').addClass('table-row-hover')
                }

            })

        },
        error: function () {
            alert("Lo sentimos, ha ocurrido un error inesperado");
        }
    });
}

function pintarBusqueda(nroPagina) {
    mostrarListado(nroPagina);
}