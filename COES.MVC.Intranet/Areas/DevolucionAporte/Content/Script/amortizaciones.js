var controlador = siteRoot + 'devolucionaporte/amortizaciones/';

$(document).ready(function () {
    pintarBusqueda(1);
});

function pintarBusqueda(nroPagina) {
    //$('#hfNroPagina').val(nroPagina);

    $.ajax({
        type: 'POST',
        url: controlador + "Listado",
        success: function (evt) {
            $('#listado').css("width", $('#mainLayout').width() + "px");

            $('#listado').html(evt);
            $('#tabla').dataTable({
                "scrollY": 430,
                "scrollX": false,
                'searching': false,
                "ordering": false,
                "pagingType": "full_numbers",
                "iDisplayLength": 50,
            });
        },
        error: function () {
            mostrarError();
        }
    });
}

function GuardarAmortizacion() {
    if ($("#txtPresmonto").val() == "") {
        $("#lblMensajeMonto").show();
        return;
    } else {
        $("#lblMensajeMonto").hide();
    }

    mostrarConfirmacion("", Guardar, "");
}

function Guardar(){
    var amortizacion = {
        Presanio: $("#txtPresanio").html(),
        monto: $("#txtPresmonto").val(),
        Presamortizacion: $("#txtPresamortizacion").val(),
        Presactivo: '1'
    };

    $.ajax({
        type: 'POST',
        url: controlador + 'GuardarAmortizacion',
        data: JSON.stringify(amortizacion),
        contentType: 'application/json',
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
            mostrarError();
        }
    });
}

function EliminarAmortizacion(prescodi) {
    $.ajax({
        type: 'POST',
        url: controlador + 'EliminarAmortizacion',
        data: {
            prescodi: prescodi
        },
        global: false,
        success: function (evt) {
            if (evt == "1") {
                mostrarExito();
                pintarBusqueda(1);
            } else {
                mostrarError();
            }
        },
        error: function () {
            mostrarError();
        }
    });
}

mostrarExito = function () {
    $('#mensaje').removeClass();
    $('#mensaje').addClass('action-exito');
    $('#mensaje').html("La operación se realizó correctamente");
}

mostrarError = function (mensaje) {
    if (mensaje == null) mensaje = "Ha ocurrido un error";

    $('#mensaje').removeClass();
    $('#mensaje').addClass('action-error');
    $('#mensaje').html(mensaje);
}