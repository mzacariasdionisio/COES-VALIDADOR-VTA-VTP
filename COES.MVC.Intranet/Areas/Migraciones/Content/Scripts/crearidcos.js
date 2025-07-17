var controlador = siteRoot + 'Migraciones/Procedimiento25/'

$(function () {
    $('#txtFecha').Zebra_DatePicker();

    $("#btnGenerar").click(function () {
        generarIDCOS();
    });

    $("#btnGenerarRestricOp").click(function () {
        generarRestricOp();
    });

    $('#cbGps').val(1);
});

function generarIDCOS() {
    var check = $('#chkProc25').is(':checked') ? 1 : 0;
    var gpsFrec = $('#cbGps').val();
    $.ajax({
        type: 'POST',
        url: controlador + "GenerarIDCOS",
        dataType: 'json',
        data: {
            fecha: $('#txtFecha').val(),
            gpscodi: gpsFrec,
            checkProc25: check
        },
        success: function (evt) {
            if (evt.nRegistros > 0) {               
                window.location = controlador + "Exportar?fi=" + evt.Resultado;
            } else {
                alert("Error..!! Exportar");
            }
        },
        error: function (err) { alert("Error..!! GenerarIDCOS"); }
    });
}

function generarRestricOp() {
    $.ajax({
        type: 'POST',
        url: controlador + 'GenerarReporteXlsRestricionesOperativas',
        data: {
            fecha: $('#txtFecha').val()
        },
        dataType: 'json',
        success: function (result) {
            switch (result.nRegistros) {
                case 1: window.location = controlador + "Exportar?fi=" + result.Resultado; break;// si hay elementos
                case 2: alert("No existen registros !"); break;// sino hay elementos
                case -1: alert("Error en reporte result"); break;// Error en C#
            }
        },
        error: function (err) {
            alert("Error en reporte");;
        }
    });
}