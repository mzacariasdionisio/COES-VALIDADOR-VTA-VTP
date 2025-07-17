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
            tip: 1
        },
        success: function (aData) {
            $('#listado1').html(aData.Resultado);
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}