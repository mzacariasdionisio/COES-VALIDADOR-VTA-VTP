$(function () {

    $('#cbEmpresa').multipleSelect({
        width: '150px',
        filter: true,
        onClose: function (view) {
        }
    });
    $('#cbTipoCentral').multipleSelect({
        width: '150px',
        filter: true,
        onClose: function (view) {
        }
    });

    cargarValoresIniciales();
});

function cargarValoresIniciales() {
    $('#cbEmpresa').multipleSelect('checkAll');
    $('#cbTipoCentral').multipleSelect('checkAll');

    cargarLista();
}

function mostrarReporteByFiltros() {
    cargarLista();
}

function cargarLista() {
    var fechaInicio = getFechaInicio();
    var fechaFin = getFechaFin();

    $('#listado').html();
    var ancho = $("#mainLayout").width() > 900 ? $("#mainLayout").width() - 30 : 900;

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarHorasIndiponibilidad',
        data: { fechaInicio: fechaInicio, fechaFin: fechaFin, empresas: getEmpresa(), tipoCentral: getTipocentral() },
        success: function (aData) {
            $('.filtro_fecha_desc').html(aData.FiltroFechaDesc);

            $('#listado').html(aData.Resultado);
            var anchoReporte = $('#tablaMantto').width();
            $("#listado").css("width", (anchoReporte > ancho ? ancho : anchoReporte) + "px");
            $('#tablaMantto').dataTable({
                "destroy": "true",
                "info": false,
                "searching": true,
                "paging": false,
                "scrollX": true,
                "scrollY": $('#listado').height() > 200 ? 500 + "px" : "100%"
            });
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}