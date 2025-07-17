var controlador = siteRoot + 'Siosein/TablasPrieDeclaracionMen/';

$(function () {
    $('#cbModoOpe').multipleSelect({
        width: '150px',
        filter: true,
        onClose: function (view) {
            cargarListaDifusionCostosVariables();
        }
    });

    $('#cbTipoCombustible').multipleSelect({
        width: '150px',
        filter: true,
        onClose: function (view) {
            cargarListaDifusionCostosVariables();
        }
    });

    $('#cbTipoCostoVar').multipleSelect({
        width: '150px',
        filter: true,
        onClose: function (view) {
            cargarListaDifusionCostosVariables();
        }
    });

    $('#cbModoOpe').multipleSelect('checkAll');
    $('#cbTipoCostoVar').multipleSelect('checkAll');
    $('#cbTipoCombustible').multipleSelect('checkAll');

    cargarListaDifusionCostosVariables();
});

function cargarListaDifusionCostosVariables() {

    var modo_ope = $('#cbModoOpe').multipleSelect('getSelects');
    var tipo_costovar = $('#cbTipoCostoVar').multipleSelect('getSelects');
    var tipo_combus = $('#cbTipoCombustible').multipleSelect('getSelects');

    var mesAnio = $('#txtFecha').val();
    var dataSend = { mesAnio: mesAnio, modoOpe: modo_ope.join(), tipoCombustible: tipo_combus, tipoCostoVar: tipo_costovar };

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaDifusionCostosVariables',
        dataType: 'json',
        contentType: 'application/json; charset=utf8',
        data: JSON.stringify(dataSend),
        success: function (aData) {

            $('#listado1').html(aData.Resultado);
            $('#tabla_Consolidado1').dataTable({
                scrollY: "400px",
                scrollCollapse: true,
                paging: false
            });

            GraficoColumnas(aData.Graficos[0], 'idGrafico1');

            $('#listado2').html(aData.Resultado2);
            $('#tabla_Consolidado2').dataTable({
                scrollY: "400px",
                scrollCollapse: true,
                paging: false
            });

            GraficoLinea(aData.Graficos[1], 'idGrafico2');

            $('#listado3').html(aData.Resultado3);

            GraficoColumnas(aData.Graficos[2], 'idGrafico3');
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}