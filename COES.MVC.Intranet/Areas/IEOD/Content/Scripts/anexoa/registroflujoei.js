$(function () {
    cargarValoresIniciales();
});

function cargarValoresIniciales() {
    cargarLista();
}

function mostrarReporteByFiltros() {
    cargarLista();
}

function cargarLista() {
    var idEmpresa = getEmpresa();
    var fechaInicio = getFechaInicio();
    var fechaFin = getFechaFin();
    var idSubestacion = getSubestacion();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaRegistroFlujosEI',
        data: { fechaInicio: fechaInicio, fechaFin: fechaFin, idEmpresa: idEmpresa, idSubEstacion: idSubestacion },
        success: function (data) {
            $('.filtro_fecha_desc').html(data.FiltroFechaDesc);

            $('#listado').css("width", $('#mainLayout').width() + "px");
            $('#listado').html(data.Resultado);
            $('#listado').css("overflow-x", "auto");

        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}