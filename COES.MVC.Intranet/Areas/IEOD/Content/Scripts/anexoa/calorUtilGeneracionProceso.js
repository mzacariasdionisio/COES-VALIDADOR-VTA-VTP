var ancho = 1200;

$(function () {
    ancho = $("#mainLayout").width() > 1200 ? $("#mainLayout").width() - 40 : 1200;

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
    var idEmpresa = getEmpresa();
    var fechaInicio = getFechaInicio();
    var fechaFin = getFechaFin();

    validacionesxFiltro(2);

    if (resultFiltro) {
        $.ajax({
            type: 'POST',
            async: false,
            url: controlador + 'CargarListaCalorUtilGeneracionProceso',
            data: { idEmpresa: idEmpresa, fechaInicio: fechaInicio, fechaFin: fechaFin },
            success: function (aData) {
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
}

//validaciones de los filtros de busqueda
function validacionesxFiltro(valor) {
    var idEmpresa = getEmpresa();

    var arrayUbicacion = arrayUbicacion || [];

    if (valor == 1)
        arrayUbicacion.push({ id: idEmpresa, mensaje: "Seleccione la opcion Empresa" });
    else
        arrayUbicacion.push({ id: idEmpresa, mensaje: "Seleccione la opcion Empresa" });

    validarFiltros(arrayUbicacion);
}