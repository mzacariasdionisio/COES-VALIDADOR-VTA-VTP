//variables globales
var controlador = siteRoot + 'Siosein/InformesOpeSein/'
var idEmpresa = "";
var idCentral = "";
var idTCombustible = "";
var fechaInicio = "";
var fechaFin = "";

$(function () {
    cargarMaximaDemandaTipoGeneracionAnexo();
});

function cargarMaximaDemandaTipoGeneracionAnexo() {
    $.ajax({
        type: 'POST',
        url: controlador + 'CargarMaximaDemandaTipoGeneracionAnexo',
        data: { fechaInicio: $("#txtFechaInicio").val(), fechaFin: $("#txtFechaFin").val() },
        success: function (aData) {
            $('#listado').html(aData.Resultado);
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}