$(function () {
    mostrarPromedioCaudales(1);
});

function mostrarReporteByFiltros() {
    mostrarPromedioCaudales();
}

function mostrarPromedioCaudales() {
    var codigoVersion = getCodigoVersion();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaPromCaudales',
        data: {
            codigoVersion: codigoVersion,
        },
        success: function (aData) {
            $('.filtro_fecha_desc').html(aData.FiltroFechaDesc);

            $('#listado1').html(aData.Resultado);
            $('#listado2').html(aData.Resultado2);
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}
