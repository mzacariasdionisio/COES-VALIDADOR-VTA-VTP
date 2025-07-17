var controlador = siteRoot + 'Siosein/TablasPrieDeclaracionMen/'

$(function () {        
    $('#txtFecha').Zebra_DatePicker({
        format: 'm Y'
    });

    $('#btnEjecutar').click(function () {
        cargarDifusionPotenciaFirme();
    });

    cargarDifusionPotenciaFirme();
});

function cargarDifusionPotenciaFirme() {
    var mesAnio = $('#txtFecha').val();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarDifusionPotenciaFirme',
        data: { mesAnio: mesAnio },
        success: function (aData) {

            if (aData.NRegistros > 0) {
                $('#listado1').html(aData.Resultado);
                GraficoPie(aData.Grafico, "idGraficoContainer");
            }else if (aData.NRegistros === 0) {
                alert("No se encontraron datos");
            } else {
                alert("Ha ocurrido un error");
            }
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}