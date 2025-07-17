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
            tip: 1
        },
        success: function (aData) {
            $('#listado1').html(aData.Resultado);
            if (aData.NRegistros > 0) {
                $('#tabla_TransSPT').dataTable({
                    "ordering": false
                });
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}