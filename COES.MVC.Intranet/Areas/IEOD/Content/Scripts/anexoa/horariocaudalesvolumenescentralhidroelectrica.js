//variables globales
var controlador = siteRoot + 'IEOD/AnexoA/'
var ancho = 900;

$(function () {
    $('#tab-container').easytabs({
        animate: false
    });

    $('#cbEmpresa').multipleSelect({
        width: '150px',
        filter: true,
        onClose: function (view) {
            cargarListaHorariosCaudalVolumenCentralHidroelectrica();
        }
    });

    ancho = $("#mainLayout").width() > 900 ? $("#mainLayout").width() - 30 : 900;

    cargarValoresIniciales();
});

function cargarValoresIniciales() {
    $('#cbEmpresa').multipleSelect('checkAll');
    validacionesxFiltro();
    cargarListaHorariosCaudalVolumenCentralHidroelectrica();
}

function mostrarReporteByFiltros() {
    cargarListaHorariosCaudalVolumenCentralHidroelectrica();
}

function cargarListaHorariosCaudalVolumenCentralHidroelectrica() {
    $("#listado1").html('');
    $("#listado2").html('');

    var idEmpresa = getEmpresa();
    var fechaInicio = getFechaInicio();
    var fechaFin = getFechaFin();

    validacionesxFiltro();
    if (resultFiltro) {
        $.ajax({
            type: 'POST',
            url: controlador + 'CargarReporteVertimiento',
            data: { idEmpresa: idEmpresa, fechaInicio: fechaInicio, fechaFin: fechaFin, tipoRpte: '1' },
            success: function (aData) {
                $('.filtro_fecha_desc').html(aData[0].FiltroFechaDesc);
                $('#listado1').html(aData[0].Resultado);
                var anchoReporte = $('#reporte').width();
                $("#listado1").css("width", (anchoReporte > ancho ? ancho : anchoReporte) + "px");
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

                $('#listado2').html(aData[1].Resultado);
            },
            error: function (err) {
                alert("Ha ocurrido un error");
            }
        });
    }
}

function validacionesxFiltro() {
    var idEmpresa = getEmpresa();

    var arrayFiltro = arrayFiltro || [];
    arrayFiltro.push({ id: idEmpresa, mensaje: "Seleccione la opcion Empresa" });

    validarFiltros(arrayFiltro);
}