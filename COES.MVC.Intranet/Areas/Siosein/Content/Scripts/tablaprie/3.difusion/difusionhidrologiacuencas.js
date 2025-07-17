var controlador = siteRoot + 'Siosein/TablasPrieDeclaracionMen/'

$(function () {

    $('#txtFecha').Zebra_DatePicker({
        format: 'm Y'
    });
    cargarListaDifusionHidrologiaCuencas();
});

function cargarListaDifusionHidrologiaCuencas() {

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaDifusionHidrologiaCuencas',
        data: { mesAnio: $('#txtFecha').val() },
        success: function (aData) {
            $('#listado1').html(aData.Resultado);
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}
