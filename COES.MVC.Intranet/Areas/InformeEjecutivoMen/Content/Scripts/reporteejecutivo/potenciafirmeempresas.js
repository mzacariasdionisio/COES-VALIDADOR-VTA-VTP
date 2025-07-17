var controlador = siteRoot + 'InformeEjecutivoMen/ReporteEjecutivo/'

$(function () {        
    CargarPotenciaFirmeEmpresas();
});

function mostrarReporteByFiltros() {
    CargarPotenciaFirmeEmpresas();
}

function CargarPotenciaFirmeEmpresas() {
    var codigoVersion = getCodigoVersion();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarPotenciaFirmeEmpresas',
        data: {
            codigoVersion: codigoVersion
        },
        success: function (aData) {
            $('#listado1').html(aData.Resultado);
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}