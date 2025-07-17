$(function () {
    mostrarListado();
});

function mostrarReporteByFiltros() {
    mostrarListado();
}

function mostrarListado() {
    var codigoVersion = getCodigoVersion();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarResumenProduccion',
        data: {
            codigoVersion: codigoVersion
        },
        success: function (aData) {
            $('.filtro_fecha_desc').html(aData[0].FiltroFechaDesc);

            $('#listado1').html(aData[0].Resultado);
            $('#listado2').html(aData[1].Resultado);
            $('#listado3').html(aData[2].Resultado);
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}
