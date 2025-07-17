$(function () {
    cargarDetalleEventos();
});

function mostrarReporteByFiltros() {
    cargarDetalleEventos();
}

function cargarDetalleEventos() {
    var codigoVersion = getCodigoVersion();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarDetalleEventos',
        data: {
            codigoVersion: codigoVersion
        },
        success: function (aData) {
            $('.filtro_fecha_desc').html(aData.FiltroFechaDesc);
            $('#listado').html(aData.Resultado);
           
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}
