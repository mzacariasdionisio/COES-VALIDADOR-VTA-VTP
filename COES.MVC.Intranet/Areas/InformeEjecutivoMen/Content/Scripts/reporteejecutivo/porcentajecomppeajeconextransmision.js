var controlador = siteRoot + 'InformeEjecutivoMen/ReporteEjecutivo/'

$(function () {
    CargarCompensacionPeajeConexTransmision();
});

function mostrarReporteByFiltros() {
    CargarCompensacionPeajeConexTransmision();
}

function CargarCompensacionPeajeConexTransmision() {
    var codigoVersion = getCodigoVersion();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarCompensacionPeajeConexTransmision',
        data: {
            codigoVersion: codigoVersion,
            tip: 2
        },
        success: function (aData) {
            GraficoPie(aData.Grafico, "grafico1");
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}


