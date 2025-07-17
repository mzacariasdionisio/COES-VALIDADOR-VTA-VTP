//variables globales
var controlador = siteRoot + 'IEOD/AnexoA/'

$(function () {
    $('#cbEmpresa').multipleSelect({
        width: '250px',
        filter: true,
        onClose: function (view) {
            cargarCentralxEmpresa();
        }
    });

    ancho = $("#mainLayout").width() > 900 ? $("#mainLayout").width() - 30 : 900;

    cargarValoresIniciales();
});

function cargarValoresIniciales() {
    $('#cbEmpresa').multipleSelect('checkAll');
    $('#cbCentral').multipleSelect('checkAll');
    $('#cbTipoCombustibles').multipleSelect('checkAll');

    cargarCentralxEmpresa();
}

function mostrarReporteByFiltros() {
    cargarListaCantidadCombustibleCentralTermica();
}

function cargarCentralxEmpresa() {
    validacionesxFiltro(1);
    if (resultFiltro) {
        $.ajax({
            type: 'POST',
            url: controlador + 'CargarCentralxEmpresa',
            data: { idEmpresa: $('#hfEmpresa').val() },
            success: function (aData) {
                $('#centrales').html(aData);
                $('#cbCentral').multipleSelect({
                    width: '250px',
                    filter: true,
                    onClose: function (view) {
                        cargarTipoCombustible();
                    }
                });
                $('#cbCentral').multipleSelect('checkAll');
                cargarTipoCombustible();
            },
            error: function (err) {
                alert("Ha ocurrido un error");
            }
        });
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
                $('#cbTipoCombustibleXcentral').multipleSelect({
                    width: '150px',
                    filter: true,
                    onClose: function (view) {
                        cargarListaCantidadCombustibleCentralTermica();
                    }
                });
                $('#cbTipoCombustibleXcentral').multipleSelect('checkAll');
                cargarListaCantidadCombustibleCentralTermica();
            },
            error: function (err) {
                alert("Ha ocurrido un error");
            }
        });
    }
}

function cargarListaCantidadCombustibleCentralTermica() {
    var idEmpresa = getEmpresa();
    var fechaInicio = getFechaInicio();
    var fechaFin = getFechaFin();
    var idCentral = getCentral();
    var idTCombustibleXcentral = getTipoCombustibleXcentral();

    validacionesxFiltro(2);
    if (resultFiltro) {
        $.ajax({
            type: 'POST',
            url: controlador + 'CargarListaCantidadCombustibleCentralTermica',
            data: { idEmpresa: idEmpresa, idCentral: idCentral, idTipoComb: idTCombustibleXcentral, fechaInicio: fechaInicio, fechaFin: fechaFin },
            success: function (aData) {
                $('.filtro_fecha_desc').html(aData.FiltroFechaDesc);
                $('#listado').html(aData.Resultado);

                var anchoReporte = $('#reporte').width();
                $("#resultado").css("width", (anchoReporte > ancho ? ancho : anchoReporte) + "px");
                $('#reporte').dataTable({
                    "scrollX": true,
                    "scrollY": "550px",
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
    var idCentral = getCentral();
    var idTCombustibleXcentral = getTipoCombustibleXcentral();

    var arrayFiltro = arrayFiltro || [];

    if (valor == 1)
        arrayFiltro.push({ id: idEmpresa, mensaje: "Seleccione la opcion Empresa" }, { id: idCentral, mensaje: "Seleccione la opcion Central" });
    else
        arrayFiltro.push({ id: idEmpresa, mensaje: "Seleccione la opcion Empresa" }, { id: idCentral, mensaje: "Seleccione la opcion Central" }, { id: idTCombustibleXcentral, mensaje: "Seleccione la opcion Tipo Combustible" });

    validarFiltros(arrayFiltro);
}