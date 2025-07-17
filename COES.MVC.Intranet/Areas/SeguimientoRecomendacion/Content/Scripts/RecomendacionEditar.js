
$(function () {

    $('#txtSrmrecFecharecomend').inputmask({
        mask: "1/2/y",
        placeholder: "dd/mm/yyyy",
        alias: "datetime"        
    });

    $('#txtSrmrecFecharecomend').Zebra_DatePicker({
        readonly_element: false,
        onSelect: function (date) {
            $('#txtSrmrecFecharecomend').val(date);
        }
    });

    $('#txtSrmrecFechavencim').inputmask({
        mask: "1/2/y",
        placeholder: "dd/mm/yyyy",
        alias: "datetime"        
    });

    $('#txtSrmrecFechavencim').Zebra_DatePicker({
        readonly_element: false,
        onSelect: function (date) {
            $('#txtSrmrecFechavencim').val(date);
        }
    });

   
    


    $('#btnCancelar').click(function () {
        $('#popupEdicion').bPopup().close();
    });

    $(document).ready(function () {

        $('#rbSrmrecActivoS').prop('checked', true);
        if ($('#hfSrmrecActivo').val() == 'S') { $('#rbSrmrecActivoS').prop('checked', true); }
        if ($('#hfSrmrecActivo').val() == 'N') { $('#rbSrmrecActivoN').prop('checked', true); }

        if ($('#hfAccion').val() == 0) {
            $('#btnGrabar').hide();
        }

        $('#btnCancelarCarga').hide();

        crearUpload();
        procesarArchivos();
       
    });
    
});

mostrarAlerta = function (mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').addClass('action-alert');
    $('#mensaje').html(mensaje);
}

mostrarExito = function () {
    $('#mensaje').removeClass();
    $('#mensaje').addClass('action-exito');
    $('#mensaje').html('La operación se realizó con éxito.');
}

mostrarError = function () {
    alert("Ha ocurrido un error");

}
