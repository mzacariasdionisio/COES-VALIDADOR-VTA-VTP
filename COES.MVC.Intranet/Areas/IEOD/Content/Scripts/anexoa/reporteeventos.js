$(function () {
    $('#cbEmpresa').multipleSelect({
        width: '150px',
        filter: true,
        onClose: function (view) {
            cargarUbicacion();
        }
    });

    cargarValoresIniciales();
});

function cargarValoresIniciales() {
    $('#cbEmpresa').multipleSelect('checkAll');
    cargarUbicacion();
}

function mostrarReporteByFiltros() {
    cargarListaEventos(1);
}

function cargarUbicacion() {
    var idEmpresa = getEmpresa();

    if (idEmpresa != "") {
        $.ajax({
            type: 'POST',

            url: controlador + 'CargarUbicacion',
            data: { idEmpresa: idEmpresa },
            success: function (aData) {
                $('#ubicacion').html(aData);
                $('#cbUbicacion').multipleSelect({
                    width: '150px',
                    filter: true,
                    onClose: function (view) {
                        mostrarReporteByFiltros();
                    }
                });
                $('#cbUbicacion').multipleSelect('checkAll');
                mostrarReporteByFiltros();
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

function cargarListaEventos(nroPagina) {
    var idEmpresa = getEmpresa();
    var idUbicacion = getUbicacion();
    var fechaInicio = getFechaInicio();
    var fechaFin = getFechaFin();

    $.ajax({
        type: 'POST',
        async: true,
        url: controlador + 'CargarListaEventos',
        data: { idEmpresa: idEmpresa, idUbicacion: idUbicacion, fechaInicio: fechaInicio, fechaFin: fechaFin, nroPagina: nroPagina },
        success: function (aData) {
            $('.filtro_fecha_desc').html(aData.FiltroFechaDesc);
            $('#listado').html(aData.Resultado);
            $('#idGraficoContainer').html('');
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}