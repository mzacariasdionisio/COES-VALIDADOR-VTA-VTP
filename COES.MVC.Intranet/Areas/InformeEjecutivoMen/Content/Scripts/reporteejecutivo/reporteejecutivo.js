var controlador = siteRoot + 'InformeEjecutivoMen/reporteejecutivo/';

$(function () {
    ConsultarProduccionEmpresaGeneradora();
});

function mostrarReporteByFiltros() {
    ConsultarProduccionEmpresaGeneradora();
}

function ConsultarProduccionEmpresaGeneradora() {
    var codigoVersion = getCodigoVersion();

    $.ajax({
        url: controlador + "ConsultarProduccionEmpresaGeneradora",
        type: 'POST',
        data: {
            codigoVersion: codigoVersion
        },
        success: function (result) {
            $('#listado').html(result);
        },
        error: function (xhr, status) {
        },
        complete: function (xhr, status) {
        }
    });
};

