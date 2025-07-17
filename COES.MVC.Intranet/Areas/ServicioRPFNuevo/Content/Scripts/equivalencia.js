var controlador = siteRoot + 'serviciorpfNuevo/comparativo/';

$(function () {
    $('#btnConsultar').on('click', function () {
        obtenerEquivalencia();
    });

    $('#btnNuevo').on('click', function () {
        if ($('#cbCentral').val() != '') {
            editarRelacion($('#cbCentral').val());
        }
        else {
            mostrarMensaje('mensaje', 'alert', 'Seleccione central');
        }
    });

    $('#cbEmpresa').on('change', function () {
        cargarCentrales();
        obtenerEquivalencia();
    });

    $('#cbCentral').on('change', function () {
        obtenerEquivalencia();
    });

    $('#btnRegresar').on('click', function () {
        document.location.href = controlador + 'index';
    });

    obtenerEquivalencia();
});

cargarCentrales = function ()
{
    var empresa = $('#cbEmpresa').val();
    if (empresa == "") empresa = "-1";

    $("#cbCentral").empty();
    $('#cbCentral').append($('<option></option>').val("").html("TODOS"));
    $.ajax({
        type: 'POST',
        url: controlador + 'obtenercentrales',
        data: {
            idEmpresa: empresa
        },
        dataType: 'json',
        success: function (result) {
            for (var item in result) {
                $('#cbCentral').append($('<option></option>').val(result[item].Equicodi).html(result[item].Equinomb));
            }
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
}

obtenerEquivalencia = function () {
    $.ajax({
        type: 'POST',
        url: controlador + 'obtenerequivalencia',
        data: {
            idEmpresa: $('#cbEmpresa').val(),
            idCentral: $('#cbCentral').val()
        },
        dataType: 'json',
        success: function (result) {
            $('#listado').html(result);
        },
        error: function(){
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
}

editarRelacion = function (idCentral) {
    $.ajax({
        type: 'POST',
        url: controlador + 'equivalenciaedit',
        data: {
            idCentral: idCentral
        },
        global: false,
        success: function (evt) {
            $('#contenidoEdicion').html(evt);
            setTimeout(function () {
                $('#popupEdicion').bPopup({
                    autoClose: false
                });
            }, 50);                       

            $('#btnGrabar').on("click", function () {
                grabarRelacion();
            });

            $('#btnCancelar').on("click", function () {
                $('#popupEdicion').bPopup().close();
            });

            $('#cbAllRpf').click(function (e) {
                var table = $(e.target).closest('table');
                $('td input:checkbox', table).prop('checked', this.checked);
            });

            $('#cbAllDespacho').click(function (e) {
                var table = $(e.target).closest('table');
                $('td input:checkbox', table).prop('checked', this.checked);
            });
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        }
    });
}

grabarRelacion = function () {

    var rpf = "";
    var countRpf = 0;
    var totalRpf = $('#tableRpf tbody input:checked').length;
    $('#tableRpf tbody input:checked').each(function (index) {
        if (index === totalRpf - 1)
            rpf = rpf + $(this).val();
        else
            rpf = rpf + $(this).val() + ","

        countRpf++;
    });

    var despacho = "";
    var countDespacho = 0;
    var totalDespacho = $('#tableDespacho tbody input:checked').length;
    $('#tableDespacho tbody input:checked').each(function (index) {
        if (index === totalDespacho - 1)
            despacho = despacho + $(this).val();
        else
            despacho = despacho + $(this).val() + ",";
        countDespacho++;
    });

    if ((countRpf == 0 && countDespacho == 0) || (countRpf > 0 && countDespacho > 0)) {

        $.ajax({
            type: 'POST',
            url: controlador + 'equivalenciasave',
            data: {
                idCentral: $('#hfIdCentral').val(),
                idsRpf: rpf,
                idsDespacho: despacho
            },
            dataType: 'json',
            success: function (result) {
                if (result == 1) {
                    obtenerEquivalencia();
                    mostrarMensaje('mensaje', 'exito', 'La relación se creó correctamente.');
                    $('#popupEdicion').bPopup().close();
                }
                else {
                    mostrarMensaje('mensajeEdicion', 'error', 'Ha ocurrido un error.');
                }
            },
            error: function () {
                mostrarMensaje('mensajeEdicion', 'error', 'Ha ocurrido un error.');
            }
        });
    }
    else {
        mostrarMensaje('mensajeEdicion', 'alert', 'No puede seleccionar puntos de una sola fuente.');
    }
}

mostrarMensaje = function (id, tipo, mensaje) {
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);
}