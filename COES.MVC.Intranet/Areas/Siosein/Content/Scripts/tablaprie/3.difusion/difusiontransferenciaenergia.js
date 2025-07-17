var controlador = siteRoot + 'Siosein/TablasPrieDeclaracionMen/'

$(function () {

    $('#txtFecha').Zebra_DatePicker({ format: 'm Y' });

    cargarListaDifusionTransferenciaEnergia();   
});

function cargarListaDifusionTransferenciaEnergia() {

    var mesAnio = $('#txtFecha').val();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaDifusionTransferenciaEnergia',
        data: { mesAnio: mesAnio },
        success: function (aData) {

            $('#listado1').html(aData.Resultado);

            $('#tabla7').dataTable({
                scrollY: "400px",
                scrollCollapse: true,
                paging: false
            });

            GraficoColumnas(aData.Graficos[0], 'idGrafico1');
            GraficoColumnas(aData.Graficos[1], 'idGrafico2');
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}