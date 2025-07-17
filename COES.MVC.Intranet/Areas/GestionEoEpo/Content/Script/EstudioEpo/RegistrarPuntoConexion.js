var controlador = siteRoot + 'gestioneoepo/PuntoConexion/';
$(document).ready(function () {
    $("#btnGuardar").click(function ()
    {
       
        if ($("#Zoncodi").val() == "") {
            mostrarMensajePopup("Seleccione la zona a registrar", 1);
            return false;
        }
        if ($("#PuntDescripcion").val().trim() == "") {
            mostrarMensajePopup("Ingrese un valor sin espacios en blanco", 1);
            return false;
        }
        mostrarConfirmacion("¿Desea guardar la información ingresada?", Guardar, "c");
    });
});

function regresar() {
    window.location = controlador + "Index";
}

function Guardar() {
    $("#btnGuardar").submit();
}


$('#frmPuntoConexionEpo').submit(function (e) {

    var data = $('#frmPuntoConexionEpo').serializeArray();
    elemento = JSON.stringify(getFormData(data));
    //console.log(elemento);
    if (!$(this).valid()) {
        $("#mensaje").show();
        $('#popupConfirmarOperacion').bPopup().close();
        return false;
    }

    $.ajax({
        type: "POST",
        url: controlador + "RegistrarPuntoConexionEpo",
        contentType: 'application/json',
        //dataType: "json",
        data: elemento,
        success: function (resultado) {
            if (resultado == 1) {
                $('#popupZ').bPopup().close();
                $("#popupConfirmarOperacion .b-close").click();

                mostrarMensajePopup("La información se actualizó correctamente!.", 1);

                window.location = controlador + "Index";
            }
            if (resultado == 2) {
                $('#popupZ').bPopup().close();
                $("#popupConfirmarOperacion .b-close").click();

                mostrarMensajePopup("La descripción del punto de conexión ya existe en la base de datos", 1);

                //window.location = controlador + "Index";
            }


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