$(document).ready(function ($) {
    $('#btnActualizarEstadoPopup').unbind();
    $('#btnActualizarEstadoPopup').click(function () {
        if (confirm("¿Esta seguro de actualizar estado de intervenciones seleccionadas?")) {
            /// actualizar estado a intervenciones
            actualizarestado();
        }
    });

    $('#btnSalirEstadoPopup').click(function () {
        $('#popupFormCambiarEstado').bPopup().close();
    });
});

function actualizarestado() {
    var opcion = $("#Opcion").val();
    var idEstado = $("#cboEstado").val();
    var codigos = $("#intercodis").val();

    $("#divAlertaActualizarEstadoPopup").show();
    $('#btnActualizarEstadoPopup').hide();
    $("#cboEstado").unbind();

    $.ajax({
        type: "POST",
        url: controler + "ActualizarEstado",
        traditional: true,
        data: {
            estadocodi: idEstado,
            intercodis: codigos
        },
        success: function (evt) {
            $("#divAlertaActualizarEstadoPopup").hide();
            if (evt.Resultado != "-1") {
                $('#popupFormCambiarEstado').bPopup().close();
                _mostrarMensajeAlertaTemporal(true, evt.StrMensaje);
                if (opcion == 1) {
                    mostrarLista(true);
                }
                if (opcion == 0) {
                    mostrarGrillaExcel(true);
                }


            } else {
                alert(evt.StrMensaje);
            }
        },
        error: function (err) {
            alert("Lo sentimos, ha ocurrido un error inesperado");
        }
    });

}
