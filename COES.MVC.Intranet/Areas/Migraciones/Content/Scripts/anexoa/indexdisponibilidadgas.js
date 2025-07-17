//variables globales
var controlador = siteRoot + 'Migraciones/AnexoA/'
var idEmpresa = "";
var idCentral = "";
var idTCombustible = "";
var fechaInicio = "";
var fechaFin = "";
$(function () {
    $('#cbEmpresa').multipleSelect({
        width: '250px',
        filter: true,
        onClose: function (view) {
            cargarListaDisponibilidadgas();
        }
    });

    cargarValoresIniciales();
});

function cargarValoresIniciales() {
    $('#cbEmpresa').multipleSelect('checkAll');

    cargarListaDisponibilidadgas();
}

function cargarListaDisponibilidadgas() {
    validacionesxFiltro(2);
    if (resultFiltro) {
        $.ajax({
            type: 'POST',
            url: controlador + 'CargarListaDisponibilidadGas',
            data: { idEmpresa: idEmpresa, fechaInicio: fechaInicio, fechaFin: fechaFin },
            success: function (aData) {
                $('#listado').html(aData.Resultado);
                $('#idReporteCantidadCombustibleCentralTermica').dataTable();
            },
            error: function () {
                alert("Ha ocurrido un error");
            }
        });
    }
}

//validaciones de los filtros de busqueda
function validacionesxFiltro(valor) {

    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    if (empresa == "[object Object]") empresa = "-1";
    $('#hfEmpresa').val(empresa);
    idEmpresa = $('#hfEmpresa').val();

    var centrales = $('#cbCentral').multipleSelect('getSelects');
    if (centrales == "[object Object]") centrales = "-1";
    $('#hfCentral').val(centrales);
    idCentral = $('#hfCentral').val();

    var tcombustible = $('#cbTipoCombustibles').multipleSelect('getSelects');
    if (tcombustible == "[object Object]") tcombustible = "-1";
    $('#hfTipoCombustible').val(tcombustible);
    idTCombustible = $('#hfTipoCombustible').val();

    fechaInicio = $('#txtFechaInicio').val();
    fechaFin = $('#txtFechaFin').val();

    var arrayFiltro = arrayFiltro || [];

    if (valor == 1)
           arrayFiltro.push({ id: idEmpresa, mensaje: "Seleccione la opcion Empresa" }, { id: idCentral, mensaje: "Seleccione la opcion Central" });
    else
        arrayFiltro.push({ id: idEmpresa, mensaje: "Seleccione la opcion Empresa" }, { id: idCentral, mensaje: "Seleccione la opcion Central" }, { id: idTCombustible, mensaje: "Seleccione la opcion Tipo Combustible" });

    validarFiltros(arrayFiltro);
}