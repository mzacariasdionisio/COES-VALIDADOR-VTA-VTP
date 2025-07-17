
var controler = siteRoot + "transferencias/GestionEquivalenciaVTEAVTP/";
var editar = false;
$(document).ready(function () {
    buscar();
    pintarPaginado();

    $('#btnBuscar').click(() => {
 
        buscar(); 
    })

    $('#btnExportar').click(() => {
        generarExcel();
    })

    //$('#btnAgregarRelacion').click(function () {
    //    nuevo();
    //})



    //$(document).on("click", '#listado table#tabla td > a.delete', function (event) {
    //    var id = $(event.currentTarget).attr('id').split('_')[1];

    //    $.ajax({
    //        type: 'POST',
    //        url: controler + "Baja",
    //        data: {
    //            iCoReSoCodi: id
    //        },
    //        success: function (evt) {
    //            if (evt.EsCorrecto == 1) {
    //                $('#popupMensajeZ #btnAceptarMsj').hide();
    //                $('#popupMensajeZ #cmensaje').html('<div class="exito mensajes">' + evt.Mensaje + '</div>');
    //                setTimeout(function () {
    //                    $('#popupMensajeZ').bPopup({
    //                        easing: 'easeOutBack',
    //                        speed: 450,
    //                        transition: 'slideDown',
    //                    });
    //                }, 50);
    //                mostrarListado($('#paginaActual').val());
    //            } else {
    //                alert(evt.Mensaje)
    //            }

    //        },
    //        error: function () {
    //            alert("Lo sentimos, ha ocurrido un error inesperado");
    //        }
    //    });
    //});
});


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
            paginadoActual: $('#paginaActual').val(),
            genemprcodi: $('#EMPRCODI2').val(),
            cliemprcodi: $('#CLICODI2').val(),
            barrcoditra: $('#BARRCODI2').val(),
            barrcodisum: $('#BARRCODI3').val(),
            tipConCodi: $('#TIPOCONTCODI2').val(),
            tipUsuCodi: $('#TIPOUSUACODI2').val(),
            estado: $('#ESTCODSOL').val(),
            codigo: $('#txtCodigoVTEAVTP').val()
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
            genemprcodi: $('#EMPRCODI2').val(),
            cliemprcodi: $('#CLICODI2').val(),
            barrcoditra: $('#BARRCODI2').val(),
            barrcodisum: $('#BARRCODI3').val(),
            tipConCodi: $('#TIPOCONTCODI2').val(),
            tipUsuCodi: $('#TIPOUSUACODI2').val(),
            estado: $('#ESTCODSOL').val(),
            codigo: $('#txtCodigoVTEAVTP').val()
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

function nuevo() {
    $.ajax({
        type: 'POST',
        url: controler + "New",
        success: function (evt) {
            $('#popupEq #contenidoPopup').html(evt);
            setTimeout(function () {
                $('#popupEq').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown'
                });
            }, 50);

        },
        error: function () {
            alert("Lo sentimos, ha ocurrido un error inesperado");
        }
    });
}

generarExcel = function () {
    //if ($('#EMPRCODI2').val() == '') {

    //    alert('Es necesario seleccionar una empresa.')
    //    return false;
    //}

    $.ajax({
        type: 'POST',
        url: controler + 'generarexcel',
        data: {
            genemprcodi: $('#EMPRCODI2').val(),
            cliemprcodi: $('#CLICODI2').val(),
            barrcoditra: $('#BARRCODI2').val(),
            barrcodisum: $('#BARRCODI3').val(),
            tipConCodi: $('#TIPOCONTCODI2').val(),
            tipUsuCodi: $('#TIPOUSUACODI2').val(),
            estado: $('#ESTCODSOL').val(),
            codigo: $('#txtCodigoVTEAVTP').val()
        },
        dataType: 'json',
        success: function (result) {
            if (result == 1) {
                window.location = controler + 'abrirexcel';
            }
            if (result == -1) {
                mostrarError();
            }
        },
        error: function () {
            mostrarError();
        }
    });
}


function mostrarMensajeError(mensaje) {
    $('#popupMensajeZ #btnAceptarMsj').hide();
    $('#popupMensajeZ #cmensaje').html('<div class="error mensajes">' + mensaje + '</div>');
    setTimeout(function () {
        $('#popupMensajeZ').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
        });
    }, 50);

}


mostrarError = function () {
    alert("Ha ocurrido un error.");
}
