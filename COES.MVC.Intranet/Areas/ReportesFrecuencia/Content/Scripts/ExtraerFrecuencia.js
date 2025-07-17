var ExtraerFrecuencia = "ExtraerFrecuencia/";
var controler = siteRoot + "ReportesFrecuencia/" + ExtraerFrecuencia;

//Funciones de busqueda
$(document).ready(function () {

    $("#btnManualUsuario").click(function () {
        window.location = controler + 'DescargarManualUsuario';
    });

    $('#tabla').dataTable({
        "sPaginationType": "full_numbers",
        "destroy": "true",
        "aaSorting": [[1, "asc"], [2, "asc"]]
    });

    $('#frmExtraerFrecuencia').submit(function (event) {
        if ($('#frmExtraerFrecuencia').valid()) {
            $.ajax({
                type: 'POST',
                url: controler + 'Save',
                data: $('#frmExtraerFrecuencia').serialize(),
                success: function (data) {
                    if (data.sError == '') {
                        location.href = controler + 'index'
                    } else {
                        $('#sError').html(data.sError)
                    }
                }
            })
        }
        event.preventDefault();
        return false;
    })

    /*$('#Entidad_FechaCargaInicio').Zebra_DatePicker({
    });

    $('#Entidad_FechaCargaFin').Zebra_DatePicker({
    });*/

    

    




});

function exportarExcel(IdCarga)  ///para exportar arhivos excel 
{
    console.log('exportarExcel');
    
    $.ajax({
        type: 'POST',
        url: controler + 'ExportarExcel',
        data: {
            IdCarga: IdCarga
        },
        dataType: 'json',
        success: function (result) {
            if (result == 1) {
                window.location = controler + "ExportarReporte";
            }
            if (result == -1) {
                alert("Ha ocurrido un error al generar reporte excel");
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error al generar archivo excel");
            console.log(err);
        }
    });
}

function limpiarMensajeError() {
    $('#sError').html('');
}

function cancelarExtraerInformacion() {
    $('#Entidad_GPSCodi').val('');
    $('#Entidad_FechaHoraInicio').val('');
    $('#Entidad_FechaHoraFin').val('');
}



