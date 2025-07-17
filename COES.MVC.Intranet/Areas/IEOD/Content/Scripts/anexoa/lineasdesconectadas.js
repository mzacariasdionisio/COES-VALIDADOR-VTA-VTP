$(function () {
    $('#cbEmpresa').multipleSelect({
        width: '150px',
        filter: true,
        onClose: function (view) {
            cargarLista();
        }
    });

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
    $('#listado').html('');
    var alturaDisponible = getHeightTablaListado();

    var idEmpresa = getEmpresa();
    var fechaInicio = getFechaInicio();
    var fechaFin = getFechaFin();

    validacionesxFiltro(1);
    if (resultFiltro) {
        $.ajax({
            type: 'POST',
            data: { fechaInicio: fechaInicio, fechaFin: fechaFin, empresa: idEmpresa},
            url: controlador + 'CargarListaDesconectadasPorTension',
            success: function (aData) {
                $('.filtro_fecha_desc').html(aData.FiltroFechaDesc);

                var ancho = $("#mainLayout").width() - 20;

                $('#listado').html(aData.Resultado);

                var anchoReporte = $('#reporte').width();
                var alturaReporte = $('#reporte').outerHeight(true);
                alturaDisponible = alturaDisponible - 55; //$(".dataTables_scrollHeadInner").outerHeight(true);
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
}

//validaciones de los filtros de busqueda
function validacionesxFiltro(valor) {
    var idEmpresa = getEmpresa();

    var arrayFiltro = arrayFiltro || [];

    if (valor == 1)
        arrayFiltro.push({ id: idEmpresa, mensaje: "Seleccione la opcion Empresa" });

    validarFiltros(arrayFiltro);
}