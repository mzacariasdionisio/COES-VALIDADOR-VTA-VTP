var ancho = 900;

$(function () {
    mostrarListado();
    ancho = $("#mainLayout").width() > 900 ? $("#mainLayout").width() - 30 : 900;
});

function mostrarReporteByFiltros() {
    mostrarListado();
}

function mostrarListado() {
    var codigoVersion = getCodigoVersion();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaEvolCostosMarginalesPorAreaOperativa',
        data: {
            codigoVersion: codigoVersion
        },
        success: function (aData) {
            $('.filtro_fecha_desc').html(aData.FiltroFechaDesc);

            $('#listado').html(aData.Resultado);

            var anchoReporte = $('#reporte').width();
            $("#resultado").css("width", (anchoReporte > ancho ? ancho : anchoReporte) + "px");

            $('#reporte').dataTable({
                "scrollX": true,
                "scrollY": "780px",
                "scrollCollapse": true,
                "sDom": 't',
                "ordering": false,
                paging: false,
                fixedColumns: {
                    leftColumns: 1
                }
            });
           
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });

}
