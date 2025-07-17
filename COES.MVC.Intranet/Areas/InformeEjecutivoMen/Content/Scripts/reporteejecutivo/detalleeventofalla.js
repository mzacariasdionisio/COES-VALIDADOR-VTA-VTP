var controlador = siteRoot + 'InformeEjecutivoMen/reporteejecutivo/'

$(function () {
    GetEventoFallaSuministroElect();
});

function mostrarReporteByFiltros() {
    GetEventoFallaSuministroElect();
}

function GetEventoFallaSuministroElect() {
    var codigoVersion = getCodigoVersion();

    $.ajax({
        type: 'POST',
        url: controlador + 'GetEventoFallaSuministroElect',
        data: {
            codigoVersion: codigoVersion
        },
        success: function (aData) {
            $('#listado').html(aData.Resultado);
            $('#tblEventos').dataTable({
                pageLength: 3
            });
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}