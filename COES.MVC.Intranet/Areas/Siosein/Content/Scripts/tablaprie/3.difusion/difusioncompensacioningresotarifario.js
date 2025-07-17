var controlador = siteRoot + 'Siosein/TablasPrieDeclaracionMen/'

$(function () {
    $('#txtFecha').Zebra_DatePicker({
        format: 'm Y'
    });
    cargarListaDifusionCompensacionIngresoTarifario();
});

function cargarListaDifusionCompensacionIngresoTarifario() {

    var mesAnio = $('#txtFecha').val();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaDifusionCompensacionIngresoTarifario',
        data: { mesAnio: mesAnio },
        success: function (aData) {
            $('#listado1').html(aData.Resultado);

            if (aData.NRegistros > 0) {

                $('#tabla_CompIngTarif').dataTable({
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

