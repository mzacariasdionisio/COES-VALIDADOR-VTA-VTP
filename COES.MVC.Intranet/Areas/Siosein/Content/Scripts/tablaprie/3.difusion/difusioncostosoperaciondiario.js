var controlador = siteRoot + 'Siosein/TablasPrieDeclaracionMen/'

$(function () {

    $('#txtFecha').Zebra_DatePicker({
        format: 'm Y'
    });
    cargarListaDifusionCostosOperacionDiario();
});

function cargarListaDifusionCostosOperacionDiario() {

    var mesAnio = $('#txtFecha').val();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaDifusionCostosOperacionDiario',
        data: { mesAnio: mesAnio },
        success: function (aData) {
            $('#listado1').html(aData.Resultado);

            if (aData.NRegistros > 0) {
                GraficoCombinadoDual(aData.Grafico, "idGrafico1");
            }

        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}