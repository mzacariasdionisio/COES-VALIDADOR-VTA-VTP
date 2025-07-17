$(function () {
    $('#cbEmpresa').multipleSelect({
        width: '200px',
        filter: true,
        onClose: function (view) {
            cargarLista();
        }
    });

    ancho = $("#mainLayout").width() > 900 ? $("#mainLayout").width() - 30 : 900;

    cargarValoresIniciales();
});

function cargarValoresIniciales() {
    $('#cbEmpresa').multipleSelect('checkAll');
    cargarLista();
}

function mostrarReporteByFiltros() {
    cargarLista();
}

function cargarLista() {
    var idEmpresa = getEmpresa();
    var fechaInicio = getFechaInicio();
    var fechaFin = getFechaFin();

    if (idEmpresa != "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'CargarListaDesviacionesProduccionUG',
            data: {
                fechaInicio: fechaInicio,
                fechaFin: fechaFin,
                empresa: idEmpresa
            },
            success: function (aData) {
                $('.filtro_fecha_desc').html(aData.FiltroFechaDesc);
                $('#listado').html(aData.Resultado);
                var anchoReporte = $('#reporte').width();
                $("#resultado").css("width", (anchoReporte > ancho ? ancho : anchoReporte) + "px");
                $('#reporte').dataTable({
                    "scrollX": true,
                    "scrollY": "1000px",
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
    } else {
        alert("Debe seleccionar al menos una empresa");
        $('#cbEmpresa').multipleSelect('checkAll');
    }
}
