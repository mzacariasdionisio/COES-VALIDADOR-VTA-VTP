var controlador = siteRoot + 'Siosein/TablasPrieDeclaracionMen/'

$(function () {

    $('#cbEmpresa').multipleSelect({
        width: '150px',
        filter: true,
        onClose: function (view) {
            //cargarCentralxEmpresa();
            cargarListaDifusionBalanceEmpresas();

        }
    });

    cargarListaDifusionBalanceEmpresas();

    $('#cbEmpresa').multipleSelect('checkAll');
});

function cargarListaDifusionBalanceEmpresas() {
    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    if (empresa == "") empresa = "-1";
    $('#hfEmpresa').val(empresa);
    var mesAnio = $('#txtFecha').val();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaDifusionBalanceEmpresas',
        data: { mesAnio: mesAnio, idEmpresa: $('#hfEmpresa').val() },
        success: function (aData) {
            $('#listado1').html(aData.Resultado);
            $('#tabla9').dataTable();
            cargarGraficoDifusionBalanceEmpresas();
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function cargarGraficoDifusionBalanceEmpresas() {
    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    if (empresa == "") empresa = "-1";
    $('#hfEmpresa').val(empresa);
    var mesAnio = $('#txtFecha').val();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarGraficoDifusionBalanceEmpresas',
        data: { mesAnio: mesAnio, idEmpresa: $('#hfEmpresa').val() },
        dataType: 'json',
        success: function (aData) {
            if (aData.Serie1 != null) {
                disenioGrafico(aData, 'idGrafico1', 'RESULTADOS NETOS DE LAS TRANSFERENCIAS DE ENERGIA DE LOS GENERADORES', 1);
            }
            
            //if (aData.Lista1.length > 0) {
            //    diseñoGrafico(aData.Lista1, 'idGrafico1', 'POTENCIA EFECTIVA POR EMPRESAS');
            //} else $('#idGrafico1').html('Sin Grafico - No existen registros a mostrar!');
        },
        error: function () {
            alert('Ha ocurrido un error');
        }
    });
}

function disenioGrafico(data, grafico, texto, tip) {
    if (tip == 1) {

        var Categorias = [];
        var Series1 = [];
        var Series2 = [];
        var Series3 = [];
        var Series4 = [];
        var Series5 = [];
        var Series6 = [];

        for (var j = 0; j < data.Categoria.length; j++) {
            Categorias[j] = data.Categoria[j];
        }

        for (var x = 0; x < data.Serie1.length; x++) {
            Series1[x] = data.Serie1[x];
        }
        for (var x = 0; x < data.Serie2.length; x++) {
            Series2[x] = data.Serie2[x];
        }
        for (var x = 0; x < data.Serie3.length; x++) {
            Series3[x] = data.Serie3[x];
        }
        for (var x = 0; x < data.Serie4.length; x++) {
            Series4[x] = data.Serie4[x];
        }
        for (var x = 0; x < data.Serie5.length; x++) {
            Series5[x] = data.Serie5[x];
        }
        for (var x = 0; x < data.Serie6.length; x++) {
            Series6[x] = data.Serie6[x];
        }


        Highcharts.chart(grafico, {
           /* chart: {
                type: 'column'
            },*/
            title: {
                text: texto
            },
            xAxis: {
                categories: Categorias
            },
            yAxis: {
                //min: 0,
                title: {
                    text: 'MWh'
                }
                ,
               
            },
            
            plotOptions: {
                column: {
                    stacking: 'normal'
                }
            },
            series: [{
                type: 'column',
                name: 'TRANSF. ENERGIA ACTIVA',
                color: '#C4BC96',
                data: Series1
            }, {
                type: 'column',
                name: 'SALDO RESUL.',
                color: '#FFBD00',
                data: Series2
            }, {
                type: 'column',
                name: 'RETIRO SIN CONTRATO',
                color: '#84A147',
                data: Series3
            }, {
                type: 'column',
                name: 'COMPENSAC.(**)',
                color: '#612321',
                data: Series4
            }, {
                name: 'SALDO MES ANT.',
                color: '#BD1010',
                data: Series5
            }, {
                name: 'INTERCAMBIOS INTERNACIONALES',
                color: '#D98036',
                data: Series6
            }]
        });
    }
}