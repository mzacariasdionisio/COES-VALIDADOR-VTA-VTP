var controlador = siteRoot + 'Siosein/TablasPrieDeclaracionMen/'

$(function () {        
    $('#txtFecha').Zebra_DatePicker({
        format: 'm Y'
    });
    //$('#btnEjecutar').click(function () {
     //   cargarListaDifusionCostosOperacionProgMensual();
     //   cargarGraficoDifusionCostosOperacionProgMensual();
    //});
    $('#btnExportar').click(function () {
        //Por implementar
    });
    $('#cbEmpresa').multipleSelect({
        width: '150px',
        filter: true,
        onClose: function (view) {
            //cargarCentralxEmpresa();
            cargarListaDifusionCostosOperacionProgMensual();
          //  cargarGraficoDifusionCostosOperacionProgMensual();
        }
    });

    cargarListaDifusionCostosOperacionProgMensual();
    //cargarGraficoDifusionCostosOperacionProgMensual();
});

function cargarListaDifusionCostosOperacionProgMensual() {
    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    if (empresa == "") empresa = "-1";
    $('#hfEmpresa').val(empresa);

    var mesAnio = $('#txtFecha').val();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaDifusionCostosOperacionProgMensual',
        data: { mesAnio: mesAnio, idEmpresa: $('#hfEmpresa').val() },
        success: function (aData) {
            $('#hfIdEnvio').val(aData.length);
            $('#listado1').html(aData.Resultado);
            cargarGraficoDifusionCostosOperacionProgMensual()
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function cargarGraficoDifusionCostosOperacionProgMensual() {
    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    if (empresa == "") empresa = "-1";
    $('#hfEmpresa').val(empresa);
    var mesAnio = $('#txtFecha').val();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarGraficoDifusionCostosOperacionProgMensual',
        data: { mesAnio: mesAnio, idEmpresa: $('#hfEmpresa').val() },
        dataType: 'json',
        success: function (aData) {
            if (aData.Serie1 !=null) {
            disenioGrafico('idGrafico1', 'GRAFICO EVOLUCION DIARIA MENSUAL DE COMBUSTIBLE POR CENTRAL', 1, aData);
            //if (aData.Lista1.length > 0) {
            //    diseñoGrafico(aData.Lista1, 'idGrafico1', 'POTENCIA EFECTIVA POR EMPRESAS');
            } else $('#idGrafico1').html('Sin Grafico - No existen registros a mostrar!');
        },
        error: function () {
            alert('Ha ocurrido un error');
        }
    });
}

function disenioGrafico(grafico, texto, tip, data) {
    if (tip == 1) {

        var serieName = [];
        var serie1 = [];
        var serie2 = [];
        var serie3 = [];
        var titleText = "Programa de Operación Mensual (POPE)";
        var mes = "";
        for (j = 0; j < data.Categoria.length; j++) {
            serieName[j] = data.Categoria[j];
        }
        for (j = 0; j < data.Serie1.length; j++) {
            serie1[j] = data.Serie1[j];
        }
        for (j = 0; j < data.Serie2.length; j++) {
            serie2[j] = data.Serie2[j];
        }
        for (j = 0; j < data.Serie3.length; j++) {
            serie3[j] = data.Serie3[j];
        }

        Highcharts.chart(grafico, {
            chart: {
                zoomType: 'xy'
            },
            title: {
                text: data.nomGrafico
            },
            subtitle: {
                text: ''
            },
            xAxis: [{
                categories: serieName,
                crosshair: true
            }],
            yAxis: [{ // Primary yAxis
                labels: {
                    format: '{value}',
                    style: {
                        color: Highcharts.getOptions().colors[1]
                    }
                },
                title: {
                    text: '',
                    style: {
                        color: Highcharts.getOptions().colors[1]
                    }
                }
            }, { // Secondary yAxis
                title: {
                    text: '',
                    style: {
                        color: Highcharts.getOptions().colors[0]
                    }
                },
                labels: {
                    format: '{value}',
                    style: {
                        color: Highcharts.getOptions().colors[0]
                    }
                },
                opposite: true
            }],
            tooltip: {
                shared: true
            },
            legend: {
                /* layout: 'horizontal',*/
                align: 'right',
                x: -30,
                verticalAlign: 'top',
                y: 25,
                floating: true,
                backgroundColor: (Highcharts.theme && Highcharts.theme.legendBackgroundColor) || '#FFFFFF'
            },
            series: [{
                name: 'Variacion',
                type: 'column',
                yAxis: 1,
                data: serie3,
                tooltip: {
                    valueSuffix: ' %'
                }

            }, {
                name: 'Proyeccion Mensual',
                type: 'line',
                data: serie1,
                tooltip: {
                    valueSuffix: ''
                }
            }, {
                name: 'Proyeccion Mensual Anterior',
                type: 'line',
                data: serie2,
                tooltip: {
                    valueSuffix: ''
                }
            }]
        });
    }


}