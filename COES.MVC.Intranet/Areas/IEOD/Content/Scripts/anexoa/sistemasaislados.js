$(function () {
    cargarLista();
});

function mostrarReporteByFiltros() {
    cargarLista();
}

function cargarLista() {
    $('#listado').html('');
    var alturaDisponible = getHeightTablaListado();

    $.ajax({
        type: 'POST',
        data: {
            fechaInicio: getFechaInicio(),
            fechaFin: getFechaFin()
        },
        url: controlador + 'CargarListaSistemasAisladosTemporales',
        success: function (aData) {
            $('.filtro_fecha_desc').html(aData.FiltroFechaDesc);

            var ancho = $("#mainLayout").width() - 30;

            $('#listado').html(aData.Resultado);

            var anchoReporte = $('#reporte').width();
            var alturaReporte = $('#reporte').outerHeight(true);
            alturaDisponible = alturaDisponible - 35;
            $("#resultado").css("width", (ancho) + "px");
            $('#reporte').dataTable({
                "autoWidth": false,
                "scrollX": true,
                "scrollY": alturaReporte > alturaDisponible ? alturaDisponible + "px" : "100%",
                "scrollCollapse": true,
                "sDom": 't',
                "ordering": false,
                paging: false
            });
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}