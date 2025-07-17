var controlador = siteRoot + 'gestioneoepo/estudioepo/';

$(document).ready(function () {
    $("#btnCancelar").click(function () {
        window.history.back();
    });

    $("#btnGuardar").click(function () {
        mostrarConfirmacion("¿Desea guardar la información ingresada?", Guardar, "c");
    });
});

function Guardar() {
    $("#btnGuardar").submit();
}

$('#frmEstablecerNoVigencia').submit(function (e) {
    var data = $('#frmEstablecerNoVigencia').serializeArray();
    elemento = JSON.stringify(getFormData(data));
    if (!$(this).valid()) {
        $("#mensaje").show();
        $('#popupConfirmarOperacion').bPopup().close();
        return false;
    }

    $.ajax({
        type: "POST",
        url: controlador + "EstablecerNoVigencia",
        contentType: 'application/json',
        //dataType: "json",
        data: elemento,
        success: function (d) {
            if (d.bResult) {
                $('#popupZ').bPopup().close();
                $("#popupConfirmarOperacion .b-close").click();

                mostrarMensajePopup("El registro se actualizó correctamente!.", 1);

                window.location = controlador + "index";
            }
            else { alert(d.sMensaje); }
        },
        error: function (req, status, error) {

        }
    });
});

function getFormData(data) {
    var unindexed_array = data;
    var indexed_array = {};

    $.map(unindexed_array, function (n, i) {
        indexed_array[n['name']] = n['value'];
    });

    return indexed_array;
}
