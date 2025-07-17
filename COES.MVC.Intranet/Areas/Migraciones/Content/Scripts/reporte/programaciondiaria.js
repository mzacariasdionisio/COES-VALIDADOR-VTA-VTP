var controlador = siteRoot + 'Migraciones/Reporte/'

$(function () {
    $('#txtFecha').Zebra_DatePicker();

    $("#btnConsultar").click(function () {
        cargarProgramacionDiaria();
    });
});

function cargarProgramacionDiaria() {
    $.ajax({
        type: 'POST',
        url: controlador + "CargarProgramacionDiaria",
        dataType: 'json',
        data: {
            fecha: $('#txtFecha').val()
        },
        success: function (evt) {
            $('#listado').html(evt.Resultado);
            if (evt.nRegistros > 0) {
                $("#tabla").dataTable({
                    "ordering": false//,
                    //"bLengthChange": false,
                    //"bInfo": false,
                });
            }
        },
        error: function (err) { alert("Error..!!"); }
    });
}
