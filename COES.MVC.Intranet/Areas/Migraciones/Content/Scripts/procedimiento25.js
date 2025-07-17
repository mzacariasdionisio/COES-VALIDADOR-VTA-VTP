var controlador = siteRoot + 'Migraciones/Procedimiento25/'

$(function () {
    $('#txtFecha').Zebra_DatePicker({
        onSelect: function (date) {
            cargarInformacion();
        }
    });
    $('#cbTipo').change(function (e) {
        cargarInformacion();
    });

    $("#btnExportar").click(function () {
        exportarInformacion();
    });

    cargarInformacion();
});

function cargarInformacion() {
    var fec = $('#txtFecha').val();
    $.ajax({
        type: 'POST',
        url: controlador + "CargarInformacionP25",
        dataType: 'json',
        data: {
            tipo: $('#cbTipo').val(), fecha: fec
        },
        success: function (evt) {
            $('#listado').html(evt.Resultado);
            if (evt.nRegistros > 0) {
                $("#btnExportar").show();
                $("#tb_info").dataTable({
                    "ordering": false,
                    /*,
                    "bLengthChange": false,
                    "bInfo": false,*/
                    "iDisplayLength": 50
                });
            } else { $("#btnExportar").hide(); }
        },
        error: function (err) { alert("Error..!! en CargarInformacionP25"); }
    });
}

function exportarInformacion() {
    var fec = $('#txtFecha').val();
    $.ajax({
        type: 'POST',
        url: controlador + 'GenerarReporteXls',
        data: {
            tipo: $('#cbTipo').val(), fecha: fec
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
