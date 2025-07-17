$(function () {
    mostrarVolUtilListadoGrafico(1);

});

function mostrarReporteByFiltros() {
    mostrarVolUtilListadoGrafico(1);
}

function mostrarVolUtilListadoGrafico(item) {
    var codigoVersion = getCodigoVersion();    

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaVolEmbalesLagunas',
        data: {
            codigoVersion: codigoVersion,
        },
        success: function (aData) {

            $('#listado').html(aData.Resultado);
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}
