var controlador = siteRoot + 'IEOD/AnexoA/'
var ancho = 1200;

$(function () {
    ancho = $("#mainLayout").width() > 1200 ? $("#mainLayout").width() - 40 : 1200;

    $('#cbEmpresa').multipleSelect({
        width: '150px',
        filter: true,
        onClose: function (view) {
            cargarListaRegistroEnergia30Unidades();
        }
    });

    $('#cbEmpresa').multipleSelect('checkAll');

    cargarListaRegistroEnergia30Unidades();
});

function mostrarReporteByFiltros() {
    cargarListaRegistroEnergia30Unidades();
}

function cargarListaRegistroEnergia30Unidades() {
    var idEmpresa = getEmpresa();
    var fechaInicio = getFechaInicio();
    var fechaFin = getFechaFin();

    validacionesxFiltro(1);
    if (resultFiltro) {
        var fechaInicio = $('#txtFechaInicio').val();
        var fechaFin = $('#txtFechaFin').val();

        $.ajax({
            type: 'POST',
            url: controlador + 'CargarListaRegistroEnergia30Unidades',
            data: { fechaInicio: fechaInicio, fechaFin: fechaFin, idEmpresa: idEmpresa },
            success: function (aData) {
                $('.filtro_fecha_desc').html(aData.FiltroFechaDesc);
                $('#listado').html(aData.Resultado);
                var anchoReporte = $('#reporte').width();
                $("#resultado").css("width", (anchoReporte > ancho ? ancho : anchoReporte) + "px");
                $('#reporte').dataTable({
                    "scrollX": true,
                    "scrollY": "722px",
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
}

//validaciones de los filtros de busqueda
function validacionesxFiltro(valor) {
    var idEmpresa = getEmpresa();

    var arrayUbicacion = arrayUbicacion || [];

    if (valor == 1)
        arrayUbicacion.push({ id: idEmpresa, mensaje: "Seleccione la opcion Empresa" });
    else
        arrayUbicacion.push({ id: idEmpresa, mensaje: "Seleccione la opcion Empresa" }, { id: idCentral, mensaje: "Seleccione la opcion Central" }, { id: idTCombustible, mensaje: "Seleccione la opcion Tipo Combustible" });

    validarFiltros(arrayUbicacion);
}