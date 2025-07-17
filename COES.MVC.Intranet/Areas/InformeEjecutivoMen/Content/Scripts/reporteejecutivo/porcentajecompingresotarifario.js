var controlador = siteRoot + 'InformeEjecutivoMen/ReporteEjecutivo/'

$(function () {
    CargarCompensacionIngresoTarifario();
});

function mostrarReporteByFiltros() {
    CargarCompensacionIngresoTarifario();
}

function CargarCompensacionIngresoTarifario() {
    var codigoVersion = getCodigoVersion();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarCompensacionIngresoTarifario',
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
