$(function () {
    mostrarListadoEventos();

});

function mostrarReporteByFiltros() {
    mostrarListadoEventos();
}

function mostrarListadoEventos() {
    var codigoVersion = getCodigoVersion();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListadoEventos',
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
