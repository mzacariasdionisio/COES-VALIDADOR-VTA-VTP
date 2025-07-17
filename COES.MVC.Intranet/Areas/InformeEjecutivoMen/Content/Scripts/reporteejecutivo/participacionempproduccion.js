var controlador = siteRoot + 'InformeEjecutivoMen/reporteejecutivo/';

$(function () {
    ConsultarParticipacionEmpresasProduccionMes();
});

function mostrarReporteByFiltros() {
    ConsultarParticipacionEmpresasProduccionMes();
}

function ConsultarParticipacionEmpresasProduccionMes() {
    var codigoVersion = getCodigoVersion();

    $.ajax({
        url: controlador + "ConsultarParticipacionEmpresasProduccionMes",
        type: 'POST',
        data: {
            codigoVersion: codigoVersion
        },
        success: function (result) {
            GraficoPie(result.Graficos[0], "graficoParticipanEmpresas");
            GraficoColumnasBar(result.Graficos[1], "graficoParticipanEmpresasAnexo");  
        },
        error: function (xhr, status) {
        },
        complete: function (xhr, status) {
        }
    });
}




