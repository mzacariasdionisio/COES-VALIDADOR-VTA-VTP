
$(function () {

    $('#txtSrmcomFechacoment').inputmask({
        mask: "1/2/y",
        placeholder: "dd/mm/yyyy",
        alias: "datetime"        
    });

    $('#txtSrmcomFechacoment').Zebra_DatePicker({
        readonly_element: false,
        onSelect: function (date) {
            $('#txtSrmcomFechacoment').val(date);
        }
    });


    $(document).ready(function () {
        
        $('#rbSrmcomGruporesponsN').prop('checked', true);
        if ($('#hfSrmcomGruporespons').val() == 'C') { $('#rbSrmcomGruporesponsS').prop('checked', true); $('#cbUsercode').val($('#hfUsercode').val()); }
        if ($('#hfSrmcomGruporespons').val() == 'A') { $('#rbSrmcomGruporesponsN').prop('checked', true); $('#cbEmprCodi').val($('#hfEmprCodi').val()); }
        $('#rbSrmcomActivoS').prop('checked', true);
        
        if ($('#hfAccion').val() == 0) {
            $('#btnGrabarCom').hide();
        }

        $('#btnCancelarCargaCom').hide();

        crearUploadCom();
        procesarArchivosCom();


        
    });

});


mostrarAlertaCom = function (mensaje) {
    $('#mensajeCom').removeClass();
    $('#mensajeCom').addClass('action-alert');
    $('#mensajeCom').html(mensaje);
}

