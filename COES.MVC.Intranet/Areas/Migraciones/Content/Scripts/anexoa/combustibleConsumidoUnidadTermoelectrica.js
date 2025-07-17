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
            cargarListaCombustibleConsumidoUnidadTermoelectrica();
        }
    });
    $('#cbCentrales').multipleSelect({
        width: '150px',
        filter: true,
        onClose: function (view) {
            cargarTipoCombustible();
            cargarListaCombustibleConsumidoUnidadTermoelectrica();
        }
    });

    cargarValoresIniciales();
});

function cargarValoresIniciales() {
    $('#cbCentrales').multipleSelect('checkAll');
    $('#cbEmpresa').multipleSelect('checkAll');
    $('#cbTipoCombustibles').multipleSelect('checkAll');

    cargarCentralxEmpresa();
    cargarTipoCombustible();
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
            },
            error: function () {
                alert("Ha ocurrido un error");
            }
        });
    }
}

function cargarTipoCombustible() {
    validacionesxFiltro(1);
    if (resultFiltro) {
        $.ajax({
            type: 'POST',
            url: controlador + 'CargarTipoCombustibleXCentral',
            data: { idCentral: $('#hfCentrales').val() },
            success: function (aData) {
                $('#cbTipoCombustible').html(aData);
                $('#cbTipoCombustibles').multipleSelect({
                    width: '150px',
                    filter: true,
                    onClose: function (view) {                  
                        cargarListaCombustibleConsumidoUnidadTermoelectrica();
                    }
                });
                $('#cbTipoCombustibles').multipleSelect('checkAll');
                cargarListaCombustibleConsumidoUnidadTermoelectrica();
            },
            error: function () {
                alert("Ha ocurrido un error");
            }
        });
    }
}

function cargarListaCombustibleConsumidoUnidadTermoelectrica() {
    validacionesxFiltro(2);
    if (resultFiltro) {
        $.ajax({
            type: 'POST',
            url: controlador + 'ListarCombustiblesConsumidoUnidTermo',
            data: { idEmpresa: idEmpresa, idCentral: idCentral, fechaInicio: fechaInicio, fechaFin: fechaFin, tipoCombustible: idTCombustible },
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

    var centrales = $('#cbCentrales').multipleSelect('getSelects');
    if (centrales == "[object Object]") centrales = "-1";
    $('#hfCentrales').val(centrales);
    idCentral = $('#hfCentrales').val();

    var tcombustible = $('#cbTipoCombustibles').multipleSelect('getSelects');
    if (tcombustible == "[object Object]") tcombustible = "-1";
    $('#hfTipoCombustible').val(tcombustible);
    idTCombustible = $('#hfTipoCombustible').val();

    fechaInicio = $('#txtFechaInicio').val();
    fechaFin = $('#txtFechaFin').val();

    var arrayUbicacion = arrayUbicacion || [];

    if (valor == 1)
        arrayUbicacion.push({ id: idEmpresa, mensaje: "Seleccione la opcion Empresa" }); //, { id: idCentral, mensaje: "Seleccione la opcion Central" });
    else
        //if ((valor == 2)) {
        //    arrayUbicacion.push({ id: idEmpresa, mensaje: "Seleccione la opcion Empresa" }, { id: idCentral, mensaje: "Seleccione la opcion Central" });
        //}
        //else {
            arrayUbicacion.push({ id: idEmpresa, mensaje: "Seleccione la opcion Empresa" }, { id: idCentral, mensaje: "Seleccione la opcion Central" }, { id: idTCombustible, mensaje: "Seleccione la opcion Tipo Combustible" });
        //}

    validarFiltros(arrayUbicacion);
}