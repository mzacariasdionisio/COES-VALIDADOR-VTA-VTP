var controlador = siteRoot + 'InformeEjecutivoMen/ReporteEjecutivo/'

$(function () {        
    CargarValorizacionTransfPotencia();
});

function mostrarReporteByFiltros() {
    CargarValorizacionTransfPotencia();
}

function CargarValorizacionTransfPotencia() {
    var codigoVersion = getCodigoVersion();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarValorizacionTransfPotencia',
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