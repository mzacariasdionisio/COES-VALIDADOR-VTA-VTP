$(function () {
    mostrarPromedioCaudales(1);
});

function mostrarReporteByFiltros() {
    mostrarPromedioCaudales(1);
}

function mostrarPromedioCaudales(item) {
    var codigoVersion = getCodigoVersion();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaPromCaudales',
        data: {
            codigoVersion: codigoVersion,
            param: item
        },
        success: function (aData) {
            $('.filtro_fecha_desc').html(aData.FiltroFechaDesc);

            if (item == 1) {
                $('#listado1').html(aData.Resultado);
                $('#listado2').html(aData.Resultado2);
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });

}

