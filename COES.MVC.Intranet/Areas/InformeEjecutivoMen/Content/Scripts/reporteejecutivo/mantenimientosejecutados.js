var controlador = siteRoot + 'InformeEjecutivoMen/reporteejecutivo/';

$(function () {
    ConsultaMantenimientosEjecutados();
});

function mostrarReporteByFiltros() {
    ConsultaMantenimientosEjecutados();
}

function ConsultaMantenimientosEjecutados() {
    var codigoVersion = getCodigoVersion();

    $.ajax({
        url: controlador + "ConsultaMantenimientosEjecutados",
        type: 'POST',
        data: {
            codigoVersion: codigoVersion
        },
        success: function (result) {
            $("#listado").html(result.Resultado);
            $('#tb_restri').dataTable({
                "scrollX": true,
                "ordering": false
            });
            GraficoColumnas(result.Grafico, "grafico1");
        },
        error: function (xhr, status) {
        },
        complete: function (xhr, status) {

        }
    });
}
