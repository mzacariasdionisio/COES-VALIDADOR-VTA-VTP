var controlador = siteRoot + 'Migraciones/Reporte/'

$(function () {
    $('#txtFecha').Zebra_DatePicker();

    $("#btnConsultar").click(function () {
        cargarProduccioncco(1);
    });

    $("#btnExportar").click(function () {
        var rd = 2;
        if ($("#rd2").is(":checked")) { rd = 3; }
        cargarProduccioncco(rd);
    });
});

function cargarProduccioncco(x) {
    $.ajax({
        type: 'POST',
        url: controlador + "CargarProduccioncco",
        dataType: 'json',
        data: {
            fecha: $('#txtFecha').val(),
            tipoinfocodi: $('#cbUnidades').val(), xx: x 
        },
        success: function (evt) {
            if (x == 1) {
                $('#listado').css("width", $('#mainLayout').width() - 20 + "px");
                $('#listado').html(evt.Resultado);
                if (evt.nRegistros > 0) {
                    $('#exp').show();
                    //$("#tb_info").dataTable({
                    //    "pageLength": 48,
                    //    "bLengthChange": false,
                    //    "ordering": false,
                    //    "bInfo": false,
                    //    "paging": false,
                    //    "searching": false
                    //});
                } else { $('#listado').css("width", "20%"); $('#exp').hide(); }
                $('#listado').css("overflow-x", "auto");
                $('#listado').css("overflow-y", "hidden");
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
