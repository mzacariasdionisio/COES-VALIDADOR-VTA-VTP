var controlador = siteRoot + 'Siosein/TablasPrieDeclaracionMen/'

$(function () {
    $('#cbEmpresa').multipleSelect({
        width: '150px',
        filter: true,
        onClose: function (view) {
            //cargarCentralxEmpresa();
        }
    });

    $('#btnEjecutar').click(function () {
        cargarListaDifusionVolumenCombustibleLiquido();
        cargarListaDifusionVolumenCombustibleGas();
        cargarListaDifusionVolumenCombustibleCarbon();
        cargarListaDifusionVolumenCombustibleBagazo();
        cargarGraficoDifusionVolumenCombustibleLiqui();
        cargarGraficoDifusionVolumenCombustibleGasNatu();
    });

    $('#cbEmpresa').multipleSelect('checkAll');

    cargarListaDifusionVolumenCombustibleLiquido();
    cargarListaDifusionVolumenCombustibleGas();
    cargarListaDifusionVolumenCombustibleCarbon();
    cargarListaDifusionVolumenCombustibleBagazo();
    cargarGraficoDifusionVolumenCombustibleLiqui();
    cargarGraficoDifusionVolumenCombustibleGasNatu();
});

function cargarListaDifusionVolumenCombustibleLiquido() {
    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    if (empresa == "") empresa = "-1";
    $('#hfEmpresa').val(empresa);

    var periodo = $("#txtFecha").val();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaDifusionVolumenCombustibleLiquido',
        data: { idsEmpresa: $('#hfEmpresa').val(), periodo: periodo },
        success: function (aData) {
            $('#listado1').html(aData.Resultado);
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}
function cargarListaDifusionVolumenCombustibleGas() {
    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    if (empresa == "") empresa = "-1";
    $('#hfEmpresa').val(empresa);

    var periodo = $("#txtFecha").val();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaDifusionVolumenCombustibleGas',
        data: { idsEmpresa: $('#hfEmpresa').val(), periodo: periodo },
        success: function (aData) {
            $('#listado2').html(aData.Resultado);
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}
function cargarListaDifusionVolumenCombustibleCarbon() {
    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    if (empresa == "") empresa = "-1";
    $('#hfEmpresa').val(empresa);

    var periodo = $("#txtFecha").val();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaDifusionVolumenCombustibleCarbon',
        data: { idsEmpresa: $('#hfEmpresa').val(), periodo: periodo },
        success: function (aData) {
            $('#listado3').html(aData.Resultado);
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}
function cargarListaDifusionVolumenCombustibleBagazo() {
    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    if (empresa == "") empresa = "-1";
    $('#hfEmpresa').val(empresa);

    var periodo = $("#txtFecha").val();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaDifusionVolumenCombustibleBagazo',
        data: { idsEmpresa: $('#hfEmpresa').val(), periodo: periodo },
        success: function (aData) {
            $('#listado4').html(aData.Resultado);
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function cargarGraficoDifusionVolumenCombustibleLiqui() {

    var periodo = $("#txtFecha").val();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarGraficoDifusionVolumenCombustibleLiqui',
        data: { idsEmpresa: $('#hfEmpresa').val(), periodo: periodo },
        dataType: 'json',
        success: function (aData) {
            
            if (aData.Grafico.Series.length > 0) {
                disenioGrafico(aData, 'idGrafico1', 'CONSUMO DE COMBUSTIBLES LIQUIDOS', 1);
            } else $('#idGrafico1').html('Sin Grafico - No existen registros a mostrar!');
        },
        error: function () {
            alert('Ha ocurrido un error');
        }
    });
}
function cargarGraficoDifusionVolumenCombustibleGasNatu() {
    
    var periodo = $("#txtFecha").val();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarGraficoDifusionVolumenCombustibleGasNatu',
        data: { idsEmpresa: $('#hfEmpresa').val(), periodo: periodo },
        dataType: 'json',
        success: function (aData) {
            if (aData.Grafico.Series.length > 0) {
                disenioGrafico(aData, 'idGrafico2', 'CONSUMO DE COMBUSTIBLE CON GAS NATURAL', 2);
            } else $('#idGrafico2').html('Sin Grafico - No existen registros a mostrar!');
        },
        error: function () {
            alert('Ha ocurrido un error');
        }
    });
}

function disenioGrafico(data, grafico, texto, tip) {

    if (tip == 1) {
        var datos = [];

        for (var i = 0; i < data.Grafico.Series.length; i++) {

            var obj = {
                name: data.Grafico.Series[i].Name,
                y: data.Grafico.Series[i].Acumulado
            }
            datos.push(obj);
        }
        Highcharts.chart(grafico, {
            chart: {
                type: 'pie'
            },
            title: {
                text: texto
            },
            credits: {
                enabled: false
            },
            plotOptions: {
                pie: {
                    innerSize: 100,
                    depth: 45
                }
            },
            series: [{
                name: 'Delivered amount',
                data: datos
            }]
        });

    }
    if (tip == 2) {
        var datos = [];

        for (var i = 0; i < data.Grafico.Series.length; i++) {

            var obj = {
                name: data.Grafico.Series[i].Name,
                y: data.Grafico.Series[i].Acumulado
            }
            datos.push(obj);
        }

        Highcharts.chart(grafico, {
            chart: {
                type: 'pie'
            },
            title: {
                text: texto
            },
            credits: {
                enabled: false
            },
            plotOptions: {
                pie: {
                    innerSize: 100,
                    depth: 45
                }
            },
            series: [{
                name: 'Delivered amount',
                data: datos
            }]
        });
    }
}