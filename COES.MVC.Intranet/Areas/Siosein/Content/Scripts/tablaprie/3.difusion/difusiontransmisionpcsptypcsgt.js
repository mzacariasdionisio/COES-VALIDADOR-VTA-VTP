var controlador = siteRoot + 'Siosein/TablasPrieDeclaracionMen/'

$(function () {
    $('#txtFecha').Zebra_DatePicker({
        format: 'm Y'
    });

    cargarListaDifusionTransmisionPCSPTyPCSGT();
    
});

function cargarListaDifusionTransmisionPCSPTyPCSGT() {
    var mesAnio = $('#txtFecha').val();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaDifusionTransmisionPCSPTyPCSGT',
        data: { mesAnio: mesAnio },
        success: function (aData) {
            $('#listado1').html(aData.Resultado);

            if (aData.NRegistros > 0) {

                $('#tabla_TransSPT').dataTable({
                    scrollY: "400px",
                    scrollCollapse: true,
                    paging: false

                });

                GraficoPie3D(aData.Grafico, "idGrafico1");
            }
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}
