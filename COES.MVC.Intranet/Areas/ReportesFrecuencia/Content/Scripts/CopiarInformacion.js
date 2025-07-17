var copiarInformacion = "CopiarInformacion/";
var controler = siteRoot + "ReportesFrecuencia/" + copiarInformacion;

//Funciones de busqueda
$(document).ready(function () {

    $('#tabla').dataTable({
        "sPaginationType": "full_numbers",
        "destroy": "true",
        "aaSorting": [[6, "desc"], [1, "asc"]]
    });

    $('#frmCopiarInformacion').submit(function (event) {
        if ($('#frmCopiarInformacion').valid()) {
            if ($('#Entidad_FechaHoraInicio').val() == '') {
                alert('Ingrese la fecha hora de inicio.');
                return false;
            }
            if ($('#Entidad_FechaHoraFin').val() == '') {
                alert('Ingrese la fecha hora de fin.');
                return false;
            }
            $.ajax({
                type: 'GET',
                url: controler + 'Save',
                data: $('#frmCopiarInformacion').serialize(),
                success: function (data) {
                    if (data.sError == '') {
                        location.href = controler + 'index';
                    }
                    else {
                        $('#sError').html(data.sError)
                    }
                }
            })
        }
        event.preventDefault();
        return false;
    })


});

function limpiarMensajeError() {
    $('#sError').html('');
}

function cancelarCopiarInformacion() {
    $('#Entidad_GPSCodiOrigen').val('');
    $('#Entidad_GPSCodiDest').val('');
    $('#Entidad_FechaHoraInicio').val('');
    $('#Entidad_FechaHoraFin').val('');
    $('#Entidad_Motivo').val('');
}