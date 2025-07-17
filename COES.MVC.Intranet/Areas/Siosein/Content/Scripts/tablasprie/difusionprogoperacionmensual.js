var controlador = siteRoot + 'Siosein/TablasPrieDeclaracionMen/'

$(function () {

    $('#txtFecha').Zebra_DatePicker({
        format: 'm Y'
    });

    $('#cbEmpresa').multipleSelect({
        width: '150px',
        filter: true,
        onClose: function (view) {
            //cargarCentralxEmpresa();
            llamarMetodos();
        }
    });

    $('#cbTipGene').multipleSelect({
        width: '150px',
        filter: true,
        onClose: function (view) {
            //cargarCentralxEmpresa();
            llamarMetodos();
        }
    });

    $('#cbRecEnerg').multipleSelect({
        width: '150px',
        filter: true,
        onClose: function (view) {
            //cargarCentralxEmpresa();
            llamarMetodos();
        }
    });

    llamarMetodos();

    $('#cbEmpresa').multipleSelect('checkAll');
    $('#cbTipGene').multipleSelect('checkAll');
    $('#cbRecEnerg').multipleSelect('checkAll');

    function llamarMetodos() {
        cargarListaDifusionProgOperacionMensual();
        cargarListaDifusionProgOperacionMensualXEmpresa();
        cargarListaDifusionProgOperacionMensualXCentralRecEnerg();
        cargarListaDifusionProgOperacionMensualProdEnergSein();
    }
});

function cargarListaDifusionProgOperacionMensual() {
   /* var parametro = 0;
    if ($('#idEneAct').is(':checked')) parametro = 1;
    if ($('#idMaxDem').is(':checked')) parametro = 2;*/

    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    if (empresa == "") empresa = "0";
    $('#hfEmpresa').val(empresa);

    var tipogene = $('#cbTipGene').multipleSelect('getSelects');
    if (tipogene == "") tipogene = "0";
    $('#hfTipGene').val(tipogene);

    var recenerg = $('#cbRecEnerg').multipleSelect('getSelects');
    if (recenerg == "") recenerg = "0";
    $('#hfRecEnerg').val(recenerg);

    var mesAnio = $('#txtFecha').val();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaDifusionProgOperacionMensual',
        data: { mesAnio: mesAnio, idEmpresa: $('#hfEmpresa').val(), tipoGene: $('#hfTipGene').val(), recenerg: $('#hfRecEnerg').val() },
        success: function (aData) {
            $('#listado1').html(aData.Resultado);
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function cargarListaDifusionProgOperacionMensualXEmpresa() {
   
    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    if (empresa == "") empresa = "0";
    $('#hfEmpresa').val(empresa);

    var tipogene = $('#cbTipGene').multipleSelect('getSelects');
    if (tipogene == "") tipogene = "0";
    $('#hfTipGene').val(tipogene);

    var recenerg = $('#cbRecEnerg').multipleSelect('getSelects');
    if (recenerg == "") recenerg = "0";
    $('#hfRecEnerg').val(recenerg);

    var mesAnio = $('#txtFecha').val();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaDifusionProgOperacionMensualXEmpresa',
        data: { mesAnio: mesAnio, idEmpresa: $('#hfEmpresa').val(), tipoGene: $('#hfTipGene').val(), recenerg: $('#hfRecEnerg').val() },
        success: function (aData) {
            $('#listado2').html(aData.Resultado);
            cargarGraficoDifusionProgOperacionMensualXEmpresa();
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}
function cargarGraficoDifusionProgOperacionMensualXEmpresa() {
   
    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    if (empresa == "") empresa = "0";
    $('#hfEmpresa').val(empresa);

    var tipogene = $('#cbTipGene').multipleSelect('getSelects');
    if (tipogene == "") tipogene = "0";
    $('#hfTipGene').val(tipogene);

    var recenerg = $('#cbRecEnerg').multipleSelect('getSelects');
    if (recenerg == "") recenerg = "0";
    $('#hfRecEnerg').val(recenerg);

    var mesAnio = $('#txtFecha').val();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarGraficoDifusionProgOperacionMensualXEmpresa',
        data: { mesAnio: mesAnio, idEmpresa: $('#hfEmpresa').val(), tipoGene: $('#hfTipGene').val(), recenerg: $('#hfRecEnerg').val() },
        dataType: 'json',
        success: function (aData) {
            if(aData.SeriesPie != null){
                disenioGrafico('idGrafico1', 'PRODUCCION DE ENERGIA ELECTRICA POR EMPRESAS', 1, aData);
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

function cargarListaDifusionProgOperacionMensualXCentralRecEnerg() {
   
    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    if (empresa == "") empresa = "0";
    $('#hfEmpresa').val(empresa);

    var tipogene = $('#cbTipGene').multipleSelect('getSelects');
    if (tipogene == "") tipogene = "0";
    $('#hfTipGene').val(tipogene);

    var recenerg = $('#cbRecEnerg').multipleSelect('getSelects');
    if (recenerg == "") recenerg = "0";
    $('#hfRecEnerg').val(recenerg);

    var mesAnio = $('#txtFecha').val();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaDifusionProgOperacionMensualXCentralRecEnerg',
        data: { mesAnio: mesAnio, idEmpresa: $('#hfEmpresa').val(), tipoGene: $('#hfTipGene').val(), recenerg: $('#hfRecEnerg').val() },
        success: function (aData) {
            $('#listado3').html(aData.Resultado);
           /* cargarGraficoDifusionProgOperacionMensualXCentralRecEnerg();*/
        },
        error: function () {
            alert("Ha ocurrido un error XD");
        }
    });
}

function cargarListaDifusionProgOperacionMensualProdEnergSein() {
    
    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    if (empresa == "") empresa = "0";
    $('#hfEmpresa').val(empresa);

    var tipogene = $('#cbTipGene').multipleSelect('getSelects');
    if (tipogene == "") tipogene = "0";
    $('#hfTipGene').val(tipogene);

    var recenerg = $('#cbRecEnerg').multipleSelect('getSelects');
    if (recenerg == "") recenerg = "0";
    $('#hfRecEnerg').val(recenerg);

    var mesAnio = $('#txtFecha').val();

    $.ajax({
        type: 'POST',
        url: controlador + 'cargarListaDifusionProgOperacionMensualProdEnergSein',
        data: { mesAnio: mesAnio, idEmpresa: $('#hfEmpresa').val(), tipoGene: $('#hfTipGene').val(), recenerg: $('#hfRecEnerg').val() },
        success: function (aData) {
            $('#listado4').html(aData.Resultado);
            cargarGraficoDifusionProgOperacionMensualProdEnergSein();
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function cargarGraficoDifusionProgOperacionMensualProdEnergSein() {
   
    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    if (empresa == "") empresa = "0";
    $('#hfEmpresa').val(empresa);

    var tipogene = $('#cbTipGene').multipleSelect('getSelects');
    if (tipogene == "") tipogene = "0";
    $('#hfTipGene').val(tipogene);

    var recenerg = $('#cbRecEnerg').multipleSelect('getSelects');
    if (recenerg == "") recenerg = "0";
    $('#hfRecEnerg').val(recenerg);

    var mesAnio = $('#txtFecha').val();

    $.ajax({
        type: 'POST',
        url: controlador + 'cargarGraficoDifusionProgOperacionMensualProdEnergSein',
        data: { mesAnio: mesAnio, idEmpresa: $('#hfEmpresa').val(), tipoGene: $('#hfTipGene').val(), recenerg: $('#hfRecEnerg').val() },
        dataType: 'json',
        success: function (aData) {
            if (aData.Serie1 != null) {
                disenioGrafico('idGrafico2', 'PRODUCCION DE ENERGIA ELECTRICA POR EMPRESAS', 4, aData);
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




function disenioGrafico(grafico, texto, tip, data) {
    if (tip == 1) {

        var Datoseries=[];
        for (j = 0; j < data.SeriesPie.length; j++) {
            var dato = {
                name: data.SeriesPie[j].string1,
                y: data.SeriesPie[j].decimal1
            }

            Datoseries.push(dato);
        }


        Highcharts.chart(grafico, {
            chart: {
                type: 'pie',
                options3d: {
                    enabled: false,
                    alpha: 45,
                    beta: 0
                }
            },
            title: {
                text: texto
            },
            tooltip: {
                pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
            },
            plotOptions: {
                pie: {
                    allowPointSelect: true,
                    cursor: 'pointer',
                    depth: 35,
                    dataLabels: {
                        enabled: true,
                        format: '{point.name}'
                    }
                }
            },
            series: [{
                type: 'pie',
                name: 'Participacion',
                data: Datoseries
            }]
        });
    }
    if (tip == 2) {
        Highcharts.chart(grafico, {
            chart: {
                type: 'pie',
                options3d: {
                    enabled: true,
                    alpha: 45,
                    beta: 0
                }
            },
            title: {
                text: texto
            },
            tooltip: {
                pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
            },
            plotOptions: {
                pie: {
                    allowPointSelect: true,
                    cursor: 'pointer',
                    depth: 35,
                    dataLabels: {
                        enabled: true,
                        format: '{point.name}'
                    }
                }
            },
            series: [{
                type: 'pie',
                name: 'Browser share',
                data: [
                    ['Firefox', 45.0],
                    ['IE', 26.8],
                    {
                        name: 'Chrome',
                        y: 12.8,
                        sliced: true,
                        selected: true
                    },
                    ['Safari', 8.5],
                    ['Opera', 6.2],
                    ['Others', 0.7]
                ]
            }]
        });
    }
    if (tip == 3) {
        Highcharts.chart(grafico, {
            chart: {
                type: 'area'
            },
            title: {
                text: texto
            },
            xAxis: {
                categories: ['1050', '1800', '1850', '1900', '1950', '1999', '2050'],
                tickmarkPlacement: 'on',
                title: {
                    enabled: false
                }
            },
            yAxis: {
                title: {
                    text: 'Billions'
                },
                labels: {
                    formatter: function () {
                        return this.value / 1000;
                    }
                }
            },
            tooltip: {
                //split: true,
                valueSuffix: ' millions'
            },
            plotOptions: {
                area: {
                    stacking: 'normal',
                    lineColor: '#666666',
                    lineWidth: 1,
                    marker: {
                        lineWidth: 1,
                        lineColor: '#666666'
                    }
                }
            },
            series: [{
                name: 'Asia',
                data: [502, 635, 809, 947, 1402, 3634, 4268]
            }, {
                name: 'Africa',
                data: [106, 107, 111, 133, 221, 767, 1766]
            }, {
                name: 'Europe',
                data: [163, 203, 276, 408, 547, 729, 628]
            }, {
                name: 'America',
                data: [118, 31, 54, 156, 339, 818, 1201]
            }, {
                name: 'Oceania',
                data: [202, 2, 2, 6, 13, 30, 46]
            }]
        });
    }

    if (tip == 4) {

        var Categorias = [];

        var Name1 = "";
        var Name2 = "";
        var Name3 = "";
        var Name4 = "";

        var Serie1 = [];
        var Serie2 = [];
        var Serie3 = [];
        var Serie4 = [];

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
        for (j = 0; j < data.Serie4.length; j++) {
            Serie4[j] = data.Serie4[j];
        }




        Highcharts.chart(grafico, {
            title: {
                text: 'PROYECCION DE MEDIANO PLAZO DE LA PRODUCCION DE ENERGIA EN EL SEIN (MWh)'
            },
            xAxis: {
                categories: Categorias
            },
            yAxis: {
                title: {
                    text: 'MWh'
                },
            },
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
            plotOptions: {
                column: {
                    stacking: 'normal',
                    dataLabels: {
                        enabled: true,
                        color: (Highcharts.theme && Highcharts.theme.dataLabelsColor) || 'white'
                    }
                },
                series: {
                    label: {
                        connectorAllowed: false
                    }
                }
            },
            series: [{
                type: 'column',
                name: 'RENOVABLES',
                color: '#85A83D',
                data: Serie3
            }, {
                type: 'column',
                name: 'TERMOELECTRICO',
                color: '#AD3330',
                data: Serie2
            }, {
                type: 'column',
                name: 'HIDROELECTRICO',
                color: '#356CAF',
                data: Serie1
            }, {
                /* type: 'spline',*/
                name: 'Generacion SEIN',
                data: Serie4,
                marker: {
                    lineWidth: 2,
                    /*lineColor: 'blue',
                   /* enabled: false,*/
                    radius: 4
                }
            }]
        });
    }
}