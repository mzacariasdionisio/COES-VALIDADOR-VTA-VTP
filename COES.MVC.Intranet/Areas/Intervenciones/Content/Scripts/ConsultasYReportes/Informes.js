var sControlador = siteRoot + "intervenciones/consultasyreportes/";

$(document).ready(function () {

    $('.txtFecha').inputmask({
        mask: "1/2/y",
        placeholder: "dd/mm/yyyy",
        alias: "datetime"
    });

    $('.txtFecha').Zebra_DatePicker({
        readonly_element: false
    }); 

    $('#btnGenerarWord').click(function () {
        GenerarWordInforme();
    });
});

function GenerarWordInforme() {
    var tipoInforme = $('#cboInforme').val();
    var fechaProceso = $('#FechaProceso').val();

    if (tipoInforme == "0") {
        alert("No ha seleccionado el Informe");
        return;
    }

    if (fechaProceso == null || fechaProceso == undefined || fechaProceso == "") {
        alert("No ha seleccionado la fecha de proceso");
        return;
    }   

    $.ajax({
        type: 'POST',
        url: sControlador + 'GenerarWordInforme',
        data: { tipoInforme: tipoInforme, fechaProceso: toDate(fechaProceso).toISOString() },
        dataType: 'json',
        success: function (result) {
            if (result != -1) {
                document.location.href = sControlador + 'Descargar?file=' + result;
            }
            else {
                alert("Ha ocurrido un error");
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function toDate(dateStr) {
    var parts = dateStr.split("/");
    return new Date(parts[2], parts[1] - 1, parts[0]);
}