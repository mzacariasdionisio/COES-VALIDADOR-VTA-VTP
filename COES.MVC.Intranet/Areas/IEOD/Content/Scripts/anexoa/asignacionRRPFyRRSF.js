$(function () {
    cargarLista();
});

function mostrarReporteByFiltros() {
    cargarLista();
}

function cargarLista() {
    $("#listado").html('');
    var fechaInicio = getFechaInicio();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaAsignacionRRPFyRRSF',
        data: {
            fechaInicio: fechaInicio
        },
        success: function (aData) {
            var ancho = $("#mainLayout").width() > 900 ? $("#mainLayout").width() - 30 : 900;

            $('.filtro_fecha_desc').html(aData.FiltroFechaDesc);
            $("txtMagnitudRpf").html(aData.Resultado2);
            $('#listado').html(aData.Resultado);

            var anchoReporte = $('#reporte').width();
            var scrollX = anchoReporte > ancho;
            $("#resultado").css("width", (scrollX ? ancho : anchoReporte) + 10 + "px");
            $('#reporte').dataTable({
                "autoWidth": false,
                "scrollX": scrollX,
                "scrollCollapse": false,
                "sDom": 't',
                "ordering": false,
                paging: false,
                fixedColumns: {
                    leftColumns: 4
                }
            });
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}