var controlador = siteRoot + 'Siosein/TablasPrieDeclaracionMen/'

$(function () {

    $('#txtFecha').Zebra_DatePicker({
        format: 'm Y'
    });

    cargarListaDifusionPotenciaEfectiva();

});

function cargarListaDifusionPotenciaEfectiva() {
    var mesAnio = $('#txtFecha').val();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaDifusionPotenciaEfectiva',
        data: { mesAnio: mesAnio},
        success: function (aData) {
            $('#listado1').html(aData.Resultados[0]);
            GraficoPie3D(aData.Graficos[0], "idGrafico1");

            $('#listado2').html(aData.Resultados[1]);
            GraficoPie3D(aData.Graficos[1], "idGrafico2");

            $('#listado3').html(aData.Resultados[2]);
            GraficoPie(aData.Graficos[2], "idGrafico3");
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}