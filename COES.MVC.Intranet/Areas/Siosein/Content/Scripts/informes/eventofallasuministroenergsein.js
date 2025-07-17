//variables globales
var controlador = siteRoot + 'Siosein/InformesOpeSein/'
var idEmpresa = "";
var idCentral = "";
var idTCombustible = "";
var fechaInicio = "";
var fechaFin = "";

$(function () {
    cargarEventoFallaSuministroEnergAnexo();
});

function cargarEventoFallaSuministroEnergAnexo() {
    $.ajax({
        type: 'POST',
        url: controlador + 'CargarEventoFallaSuministroEnergAnexo',
        data: { fechaInicio: $("#txtFechaInicio").val(), fechaFin: $("#txtFechaFin").val()},
        success: function (aData) {
            $('#listado').html(aData.Resultado);
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}