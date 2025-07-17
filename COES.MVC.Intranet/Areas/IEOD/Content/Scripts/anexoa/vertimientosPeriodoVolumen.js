var controlador = siteRoot + 'IEOD/AnexoA/'
$(function () {

    $('#cbEmpresa').multipleSelect({
        width: '150px',
        filter: true,
        onClose: function (view) {
            cargarListaVertimientosPeriodoVolumen()
        }
    });

    $('#btnBuscar').click(function () {
        cargarListaVertimientosPeriodoVolumen();
    });
    cargarValoresIniciales();

});

function cargarValoresIniciales() {
    $('#cbEmpresa').multipleSelect('checkAll');
    cargarListaVertimientosPeriodoVolumen()
}
function mostrarReporteByFiltros() {
    cargarListaVertimientosPeriodoVolumen();
}

function cargarListaVertimientosPeriodoVolumen() {
    var idEmpresa = getEmpresa();

    var idEmpresa = $('#hfEmpresa').val();
    var fechaInicio = getFechaInicio();
    var fechaFin = getFechaFin();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaVertimientosPeriodoVolumen',
        data: { idEmpresa: idEmpresa, fechaInicio: fechaInicio, fechaFin: fechaFin },
        success: function (aData) {
            $('.filtro_fecha_desc').html(aData.FiltroFechaDesc);
            $('#listado').html(aData.Resultado);
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}