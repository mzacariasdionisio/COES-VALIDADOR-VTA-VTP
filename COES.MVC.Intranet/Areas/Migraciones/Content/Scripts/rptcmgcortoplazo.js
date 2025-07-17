var controlador = siteRoot + 'Migraciones/Reporte/'

$(function () {
    $('#txtFechaini').Zebra_DatePicker();
    $('#txtFechafin').Zebra_DatePicker();

    $("#btnConsultar").click(function () {
        cargarRptCmgCortoPlazo(1);
    });

    $("#btnExportar").click(function () {
        cargarRptCmgCortoPlazo(2);
    });
});

function cargarRptCmgCortoPlazo(x) {
    $.ajax({
        type: 'POST',
        url: controlador + "CargarRptCmgCortoPlazo",
        dataType: 'json',
        data: {
            fec1: $('#txtFechaini').val(), fec2: $('#txtFechafin').val(), xx: x
        },
        success: function (evt) {
            if (x == 1) {
                $('#listado').html(evt.Resultado);
                if (evt.nRegistros > 0) {
                    $("#tb_info").dataTable({
                        "pageLength": 48,
                        "bLengthChange": false,
                        "ordering": false,
                        "bInfo": false,
                    });
                }
            } else {
                switch (evt.nRegistros) {
                    case 1: window.location = controlador + "Exportar?fi=" + evt.Resultado; break;// si hay elementos
                    case 2: alert("No existen registros !"); break;// sino hay elementos
                    case -1: alert("Error en reporte result"); break;// Error en C#
                }
            }
        },
        error: function (err) { alert("Error..!!"); }
    });
}
