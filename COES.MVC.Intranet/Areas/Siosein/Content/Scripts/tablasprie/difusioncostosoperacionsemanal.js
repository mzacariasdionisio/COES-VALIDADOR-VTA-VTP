var controlador = siteRoot + 'Siosein/TablasPrieDeclaracionMen/'

$(function () {
    $('#cbEmpresa').multipleSelect({
        width: '150px',
        filter: true,
        onClose: function (view) {
            //cargarCentralxEmpresa();
            activarLLamados();
        }
    });

    activarLLamados();

    $('#cbEmpresa').multipleSelect('checkAll');
    
    function activarLLamados() {
        cargarListaDifusionCostosOperacionSemanal();
        //cargarGraficoDifusionCostosOperacionSemanal();
    }
});

function cargarListaDifusionCostosOperacionSemanal() {
    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    if (empresa == "") empresa = "-1";
    $('#hfEmpresa').val(empresa);
    var mesAnio = $('#txtFecha').val();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaDifusionCostosOperacionSemanal',
        data: { mesAnio: mesAnio, idEmpresa: $('#hfEmpresa').val() },
        success: function (aData) {
            /*$('#listado1').html(aData.Resultado);*/           // SIOSEIN-PRIE-2021

            cargarGraficoDifusionCostosOperacionSemanal();
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function cargarGraficoDifusionCostosOperacionSemanal() {
    $.ajax({
        type: 'POST',
        url: controlador + 'CargarGraficoDifusionCostosOperacionSemanal',
        data: { mesAnio: $('#txtFecha').val(), idEmpresa: $('#hfEmpresa').val() },
        dataType: 'json',
        success: function (aData) {
            if (aData.Serie1 != null) {
                $('#listado1').html(aData.Resultado); //SIOSEIN-PRIE-2021
                disenioGrafico(aData, 'idGrafico1', 'EVOLUCION DIARIA DE LOS COSTOS DE OPERACION PROGRAMADOS SEMANALMENTE Y COSTO DE OPERACION EJECUTADO DEL SEIN', 1);
            }
        },
        error: function () {
            alert('Ha ocurrido un error');
        }
    });
}

function disenioGrafico(data, grafico, texto, tip) {
    if (tip == 1) {

        var Categorias = [];
        var Serie1 = [];
        var Serie2 = [];
        var Serie3 = [];

        for (j = 0; j < data.Categoria.length; j++) {
            Categorias[j] = data.Categoria[j];
        }
        for (j = 0; j < data.Serie1.length; j++) {
            Serie1[j] = data.Serie1[j];
        }
        for (j = 0; j < data.Serie2.length; j++) {
            Serie2[j] = data.Serie2[j];
        }
        for (j = 0; j < data.Serie3.length; j++) {
            Serie3[j] = data.Serie3[j];
        }

        Highcharts.chart(grafico, {
            title: {
                text: texto
            },
            xAxis: {
                categories: Categorias,
                crosshair: true
            },
            yAxis: [{ // Primary yAxis
                labels: {
                    format: '{value}%',
                    style: {
                        color: Highcharts.getOptions().colors[2]
                    }
                },
                title: {
                    text: 'Variacion',
                    style: {
                        color: Highcharts.getOptions().colors[2]
                    }
                },
                opposite: true

            }, { // Secondary yAxis
                gridLineWidth: 0,
                title: {
                    text: 'Soles',
                    style: {
                        color: Highcharts.getOptions().colors[0]
                    }
                },
                labels: {
                    format: '{value}',
                    style: {
                        color: Highcharts.getOptions().colors[0]
                    }
                }

            }],
            labels: {
                items: [{
                    html: '',
                    style: {
                        left: '50px',
                        top: '18px',
                        color: (Highcharts.theme && Highcharts.theme.textColor) || 'black'
                    }
                }]
            },
            series: [{
                type: 'column',
                name: 'SEMANAL',
                yAxis: 1,
                data: Serie1
            }, {
                type: 'column',
                name: 'EJECUTADO',
                yAxis: 1,
                data: Serie2
            }, {
                type: 'spline',
                name: 'Variacion(%)',
                data: Serie3,
                marker: {
                    lineWidth: 2,
                    lineColor: Highcharts.getOptions().colors[3],
                    fillColor: 'white'
                }
            }]


        });
    }
}