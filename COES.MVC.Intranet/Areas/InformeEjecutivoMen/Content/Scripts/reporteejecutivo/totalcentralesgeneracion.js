var controlador = siteRoot + 'InformeEjecutivoMen/reporteejecutivo/';
 
$(function () {
    ConsultarTotalCentralesGeneracion();
});

function mostrarReporteByFiltros() {
    ConsultarTotalCentralesGeneracion();
}

function ConsultarTotalCentralesGeneracion() {
    var codigoVersion = getCodigoVersion();

    $.ajax({
        url: controlador + "ConsultarTotalCentralesGeneracion",
        type: 'POST',
        data: {
            codigoVersion: codigoVersion
        },
        success: function (result) {

            $('#listado').html(result.Resultado);
        },
        error: function (xhr, status) {
        },
        complete: function (xhr, status) {
        }
    });
};
 
 



