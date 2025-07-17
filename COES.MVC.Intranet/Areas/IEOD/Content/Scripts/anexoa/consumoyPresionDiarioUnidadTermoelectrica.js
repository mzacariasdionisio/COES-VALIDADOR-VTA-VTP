var controlador = siteRoot + 'IEOD/AnexoA/'

$(function () {
    $('#cbEmpresa').multipleSelect({
        width: '150px',
        filter: true,
        onClose: function (view) {
            cargarCentralxEmpresa();
        }
    });

    $('#cbCentral').multipleSelect({
        width: '150px',
        filter: true,
        onClose: function (view) {
            cargarTipoCombustible();
        }
    });

    $('#btnBuscar').click(function () {
        cargarListaReporte();
    });

    parametro1 = $('input[name=cbGasPresion]:checked').val();
    $('input[name=cbGasPresion]').change(function () {
        parametro1 = $('input[name=cbGasPresion]:checked').val();
        cargarListaReporte();
    });

    cargarValoresIniciales();
});

function cargarValoresIniciales() {
    $('#cbEmpresa').multipleSelect('checkAll');
    $('#cbCentral').multipleSelect('checkAll');

    cargarListaReporte();
}

function mostrarReporteByFiltros() {
    cargarListaReporte();
}

function cargarCentralxEmpresa() {
    var idEmpresa = getEmpresa();
    validacionesxFiltro(1);
    if (resultFiltro) {
        $.ajax({
            type: 'POST',
            url: controlador + 'CargarCentralxEmpresa',
            data: { idEmpresa: idEmpresa },
            success: function (aData) {
                $('#centrales').html(aData);
                $('#cbCentral').multipleSelect({
                    width: '150px',
                    filter: true
                });
                $('#cbCentral').multipleSelect('checkAll');
                cargarTipoCombustible();

            },
            error: function (err) {
                alert("Ha ocurrido un error");
            }
        });
    }
    else {

    }
}

function cargarTipoCombustible() {
    var idCentral = getCentral();

    validacionesxFiltro(1);
    if (resultFiltro) {
        $.ajax({
            type: 'POST',
            url: controlador + 'CargarTipoCombustibleXCentral',
            data: { idCentral: idCentral },
            success: function (aData) {
                $('#cbTipoCombustible').html(aData);
                $('#cbTipoCombustibles').multipleSelect({
                    width: '150px',
                    filter: true,
                    onClose: function (view) {
                        cargarListaReporte();
                    }
                });
                $('#cbTipoCombustibles').multipleSelect('checkAll');
                cargarListaReporte();
            },
            error: function (err) {
                alert("Ha ocurrido un error");
            }
        });
    }
    else {

    }
}

function cargarListaReporte() {
    var opcion = $('input[name=cbGasPresion]:checked').val();
    $("#listado").html('');

    switch (opcion) {
        case '1': //presion
            cargarListaPresionDiarioUnidadTermoelectrica(1);
            break;
        case '2': //temperatura
            cargarListaPresionDiarioUnidadTermoelectrica(2);
            break;
        case '3': //consumo
            cargarListaConsumoDiarioUnidadTermoelectrica();
            break;
        default:
            break;
    }
}

function cargarListaPresionDiarioUnidadTermoelectrica(idParametro) {
    var idEmpresa = getEmpresa();
    var fechaInicio = getFechaInicio();
    var fechaFin = getFechaFin();
    var idCentral = getCentral();

    validacionesxFiltro(2);
    if (resultFiltro) {
        $.ajax({
            type: 'POST',
            url: controlador + 'CargarListaReporteGas',
            data: { idEmpresa: idEmpresa, idCentral: idCentral, fechaInicio: fechaInicio, fechaFin: fechaFin, idParametro: idParametro },
            success: function (aData) {
                $('.filtro_fecha_desc').html(aData.FiltroFechaDesc);

                var ancho = $("#mainLayout").width() > 900 ? $("#mainLayout").width() - 30 : 900;

                $('#listado').html(aData.Resultado);
                var anchoReporte = $('#reporte').width();
                $("#resultado").css("width", (anchoReporte > ancho ? ancho : anchoReporte) + "px");
                $('#reporte').dataTable({
                    "autoWidth": false,
                    "scrollX": true,
                    "scrollY": "550px",
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
    else {

    }
}

function cargarListaConsumoDiarioUnidadTermoelectrica() {
    var idEmpresa = getEmpresa();
    var fechaInicio = getFechaInicio();
    var fechaFin = getFechaFin();
    var idCentral = getCentral();

    validacionesxFiltro(2);
    if (resultFiltro) {
        $.ajax({
            type: 'POST',
            url: controlador + 'CargarListaReporteGas',
            data: { idEmpresa: idEmpresa, idCentral: idCentral, fechaInicio: fechaInicio, fechaFin: fechaFin, idParametro: 3 },
            success: function (aData) {
                $('.filtro_fecha_desc').html(aData.FiltroFechaDesc);

                var ancho = $("#mainLayout").width() > 900 ? $("#mainLayout").width() - 30 : 900;

                $('#listado').html(aData.Resultado);
                var anchoReporte = $('#reporte').width();
                var scrollX = anchoReporte > ancho;
                $("#resultado").css("width", (scrollX ? ancho : anchoReporte) + 10 + "px");
                $('#reporte').dataTable({
                    "autoWidth": false,
                    "scrollX": scrollX,
                    "scrollCollapse": false,
                    "sDom": 't',
                    "ordering": false,
                    paging: false,
                    fixedColumns: {
                        leftColumns: 5
                    }
                });
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
    var idCentral = getCentral();
    var idTCombustibleXcentral = getTipoCombustibleXcentral();

    var arrayFiltro = arrayFiltro || [];

    if (valor == 1)
        arrayFiltro.push({ id: idEmpresa, mensaje: "Seleccione la opcion Empresa" }, { id: idCentral, mensaje: "Seleccione la opcion Ubicación" });
    else
        arrayFiltro.push({ id: idEmpresa, mensaje: "Seleccione la opcion Empresa" }, { id: idCentral, mensaje: "Seleccione la opcion Central" }, { id: idTCombustibleXcentral, mensaje: "Seleccione la opcion Tipo Combustible" });

    validarFiltros(arrayFiltro);
}