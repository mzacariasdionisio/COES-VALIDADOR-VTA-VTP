var controlador = siteRoot + 'devolucionaporte/liquidaciones/';

$(document).ready(function () {
    pintarBusqueda(1);

    $("#cboAnioInversion").change(function () {
        pintarBusqueda(1);
    });

    $("#chkLiquidados").change(function () {
        pintarBusqueda(1);
    });
});

function detalle(aporcodi) {
    $.ajax({
        type: 'POST',
        url: controlador + 'detalle',
        data: { aporcodi: aporcodi },
        global: false,
        success: function (evt) {
            $('#contenidoDetalle').html(evt);
            setTimeout(function () {
                $('#popupDetalle').bPopup({
                    autoClose: false
                });
            }, 50);

        },
        error: function () {
            mostrarMensaje('mensajeGrupo', 'error', 'Se ha producido un error');
        }
    });
}

function pintarBusqueda(nroPagina) {
    //$('#hfNroPagina').val(nroPagina);
    var tabcdcodiestado = 0;
    if ($("#chkLiquidados").is(":checked")) {
        tabcdcodiestado = 3;
    }

    $.ajax({
        type: 'POST',
        url: controlador + "Listado",
        data: { prescodi: $("#cboAnioInversion").val(), tabcdcodiestado: tabcdcodiestado },
        success: function (evt) {
            $('#listado').css("width", $('#mainLayout').width() + "px");

            $('#listado').html(evt);
            $('#tabla').dataTable({
                "scrollY": 430,
                "scrollX": false,
                'searching': false,
                "ordering": false,
                "pagingType": "full_numbers",
                "iDisplayLength": 50
            });
        },
        error: function () {
            mostrarError();
        }
    });
}

function procesar(aporcodi) {
    if ($("#cboAnioInversion").val() == 0) {
        mostrarError("Debe seleccionar Año Inversión");
        return;
    }

    mostrarConfirmacion("", liquidar, aporcodi);
}

function liquidar(aporcodi) {
    var acodi = aporcodi[0];
    $.ajax({
        type: 'POST',
        url: controlador + 'Liquidar',
        data: { aporcodi: acodi },
        success: function (evt) {
            $('#popupConfirmarOperacion').bPopup().close();

            if (evt == "1") {
                mostrarMensajePopup("Se realizo con exito la operación", "");
                pintarBusqueda(1);
            } else {
                mostrarMensajePopup("Ha ocurrido un error", "");
            }
        },
        error: function () {
            mostrarMensaje('mensajeGrupo', 'error', 'Se ha producido un error');
        }
    });
}

mostrarExito = function () {
    $('#mensaje').removeClass();
    $('#mensaje').addClass('action-exito');
    $('#mensaje').html("La operación se realizó correctamente");
}

mostrarError = function (mensaje) {
    if (mensaje == null) mensaje = "";
    if (mensaje.length == 0) mensaje = "Ha ocurrido un error";

    $('#mensaje').removeClass();
    $('#mensaje').addClass('action-error');
    $('#mensaje').html(mensaje);
}