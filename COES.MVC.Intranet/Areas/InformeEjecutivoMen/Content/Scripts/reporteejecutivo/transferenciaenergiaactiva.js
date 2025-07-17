var controlador = siteRoot + 'InformeEjecutivoMen/ReporteEjecutivo/'

$(function () {        
    CargarTransferenciaEnergiaActiva();
});

function mostrarReporteByFiltros() {
    CargarTransferenciaEnergiaActiva();
}

function CargarTransferenciaEnergiaActiva() {
    var codigoVersion = getCodigoVersion();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarTransferenciaEnergiaActiva',
        data: {
            codigoVersion: codigoVersion
        },
        success: function (aData) {
            $('#listado1').html(aData.Resultados[0]);
            $('#listado2').html(aData.Resultados[1]);
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}