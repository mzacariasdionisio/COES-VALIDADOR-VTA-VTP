var controlador = siteRoot + 'InformeEjecutivoMen/reporteejecutivo/';

$(function () {
    mostrarVolUtilListadoGrafico();

});

function mostrarReporteByFiltros() {
    mostrarVolUtilListadoGrafico();
}

function mostrarVolUtilListadoGrafico() {
    var codigoVersion = getCodigoVersion();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaVolEmbalesLagunas',
        data: {
            codigoVersion: codigoVersion
        },
        success: function (aData) {

            $('#listado').html(aData.Resultado);
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}
