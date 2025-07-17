$(function () {
    $('#cbEmpresa').multipleSelect({
        width: '200px',
        filter: true,
        onClose: function () {
            mostrarReporteByFiltros();
        }
    });
    $('#cbTipoEquipo').multipleSelect({
        width: '200px',
        filter: true,
        onClose: function () {
            mostrarReporteByFiltros();
        }
    });

    cargarValoresIniciales();
    mostrarReporteByFiltros();
});

function cargarValoresIniciales() {
    $('#cbEmpresa').multipleSelect('checkAll');
    $('#cbTipoEquipo').multipleSelect('checkAll');
}

function mostrarReporteByFiltros() {
    var idEmpresa = getEmpresa();
    var idTipoEquipo = getTipoEquipo();
    var fechaInicio = getFechaInicio();
    var fechaFin = getFechaFin();

    validacionesxFiltro(2);

    if (resultFiltro) {

        $.ajax({
            type: 'POST',
            url: controlador + 'CargarListaIngresoOperacion',
            data: { empresas: idEmpresa, sTipoEquipo: idTipoEquipo, fechaInicio: fechaInicio, fechaFin: fechaFin },
            success: function (aData) {
                $('.filtro_fecha_desc').html(aData.FiltroFechaDesc);
                $('#listado1').html(aData.Resultado);
                $('#idGraficoContainer').html('');
            },
            error: function (err) {
                alert("Ha ocurrido un error");
            }
        });
    }
    else {
    }
}

//validaciones de los filtros de busqueda
function validacionesxFiltro(valor) {
    var idEmpresa = getEmpresa();
    var idTipoEquipo = getTipoEquipo();
    var fechaInicio = getFechaInicio();
    var fechaFin = getFechaFin();

    var arrayFiltro = arrayFiltro || [];

    if (valor == 1)
        arrayFiltro.push({ id: idTipoEquipo, mensaje: "Seleccione la opcion Tipo Equipo" }, { id: idEmpresa, mensaje: "Seleccione empresa" });

    else
        arrayFiltro.push({ id: idTipoEquipo, mensaje: "Seleccione la opcion Tipo Equipo" }, { id: idEmpresa, mensaje: "Seleccione empresa" });

    validarFiltros(arrayFiltro);
}