//variables globales
var controlador = siteRoot + 'Migraciones/AnexoA/'
var idEmpresa = "";
var idCentral = "";
var idTCombustible = "";
var fechaInicio = "";
var fechaFin = "";

$(function () {
    $('#cbEmpresa').multipleSelect({
        width: '150px',
        filter: true,
        onClose: function (view) {
            cargarCentralxEmpresa();
        }
    });

    $('#cbCentrales').multipleSelect({
        width: '150px',
        filter: true,
        onClose: function (view) {
            cargarTipoCombustible();
        }
    });

    $('#btnBuscar').click(function () {
        cargarListaReporte();
    });

    $('#cbGasPresion').change(function () {
        cargarListaReporte();
    });

    cargarValoresIniciales();
});

function cargarValoresIniciales() {
    $('#cbEmpresa').multipleSelect('checkAll');
    $('#cbCentrales').multipleSelect('checkAll');

    cargarCentralxEmpresa();
    cargarTipoCombustible();
}

function cargarCentralxEmpresa() {
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
                cargarTipoCombustible();

            },
            error: function () {
                alert("Ha ocurrido un error");
            }
        });
    }
    else {

    }
}

function cargarTipoCombustible() {
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
            error: function () {
                alert("Ha ocurrido un error");
            }
        });
    }
    else {

    }
}

function cargarListaReporte() {
    var opcion = $("#cbGasPresion").val();
    $("#listado").html('');

    switch (opcion) {
        case '1': //gas
            cargarListaPresionDiarioUnidadTermoelectrica();
            break;
        case '2': //consumo
            cargarListaConsumoDiarioUnidadTermoelectrica();
            break;
        default:
            //alert("Seleccionar opción Presión de Gas / Consumo");
    }
}

function cargarListaPresionDiarioUnidadTermoelectrica() {

    validacionesxFiltro(2);
    if (resultFiltro) {
        $.ajax({
            type: 'POST',
            url: controlador + 'CargarListaPresionDiarioUnidadTermoelectrica',
            data: { idEmpresa: idEmpresa, idCentral: idCentral, fechaInicio: fechaInicio, fechaFin: fechaFin },
            success: function (aData) {
                $('#listado').css("width", $('#mainLayout').width() - 20 + "px");
                $('#listado').html(aData.Resultado);
                $('#listado').css("overflow-x", "auto");
            },
            error: function () {
                alert("Ha ocurrido un error");
            }
        });
    }
    else {

    }
}

function cargarListaConsumoDiarioUnidadTermoelectrica() {

    validacionesxFiltro(2);
    if (resultFiltro) {
        $.ajax({
            type: 'POST',
            url: controlador + 'CargarListaConsumoDiarioUnidadTermoelectrica',
            data: { idEmpresa: idEmpresa, idCentral: idCentral, fechaInicio: fechaInicio, fechaFin: fechaFin },
            success: function (aData) {
                $('#listado').html(aData.Resultado);
            },
            error: function () {
                alert("Ha ocurrido un error");
            }
        });
    }
    else {

    }
}

//validaciones de los filtros de busqueda
function validacionesxFiltro(valor) {

    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    if (empresa == "[object Object]") empresa = "-1";
    $('#hfEmpresa').val(empresa);
    idEmpresa = $('#hfEmpresa').val();

    var central = $('#cbCentrales').multipleSelect('getSelects');
    if (central == "[object Object]") central = "-1";
    $('#hfCentrales').val(central);
    idCentral = $('#hfCentrales').val();

    var tcombustible = $('#cbTipoCombustibles').multipleSelect('getSelects');
    if (tcombustible == "[object Object]") tcombustible = "-1";
    $('#hfTipoCombustible').val(tcombustible);
    idTCombustible = $('#hfTipoCombustible').val();

    fechaInicio = $('#txtFechaInicio').val();
    fechaFin = $('#txtFechaFin').val();


    var arrayFiltro = arrayFiltro || [];

    if (valor == 1)
        arrayFiltro.push({ id: idEmpresa, mensaje: "Seleccione la opcion Empresa" }, { id: idCentral, mensaje: "Seleccione la opcion Ubicación" });
    else
        arrayFiltro.push({ id: idEmpresa, mensaje: "Seleccione la opcion Empresa" }, { id: idCentral, mensaje: "Seleccione la opcion Central" }, { id: idTCombustible, mensaje: "Seleccione la opcion Tipo Combustible" });

    validarFiltros(arrayFiltro);



}