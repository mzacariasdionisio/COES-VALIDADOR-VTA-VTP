$(function () {
    cargarListaEventos();
});

function mostrarReporteByFiltros() {
    cargarListaEventos();
}

function cargarListaEventos() {
    var codigoVersion = getCodigoVersion();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaEventosDetalle',
        data: {
            codigoVersion: codigoVersion
        },
        success: function (result) {
            $('.filtro_fecha_desc').html(result.FiltroFechaDesc);
            $('#listado').html(result.Resultado);


        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });

}
