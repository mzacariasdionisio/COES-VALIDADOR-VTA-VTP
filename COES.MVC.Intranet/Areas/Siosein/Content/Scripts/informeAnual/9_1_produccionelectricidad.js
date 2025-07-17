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
        url: controlador + 'CargarProdElectAnualEmpresasTipoGen',
        data: {
            codigoVersion: codigoVersion
        },
        success: function (aData) {
            $('#listado').html(aData.Resultado);
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}
