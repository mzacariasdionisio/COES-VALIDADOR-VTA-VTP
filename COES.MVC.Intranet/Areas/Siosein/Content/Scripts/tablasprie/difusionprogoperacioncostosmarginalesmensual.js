var controlador = siteRoot + 'Siosein/TablasPrieDeclaracionMen/'

$(function () {

    $('#txtFecha').Zebra_DatePicker({
        format: 'm Y'
    });

    $('#cbBarra').multipleSelect({
        width: '150px',
        filter: true,
        onClose: function (view) {
            //cargarCentralxEmpresa();
            CargarListaDifusionProgOperacionCostosMarginalesMensual();
        }
    });

    $('#cbTension').multipleSelect({
        width: '150px',
        filter: true,
        onClose: function (view) {
            //cargarCentralxEmpresa();
            CargarListaDifusionProgOperacionCostosMarginalesMensual();
        }
    });

    $('#cbEmpresa').multipleSelect({
        width: '150px',
        filter: true,
        onClose: function (view) {
            //cargarCentralxEmpresa();
            CargarListaDifusionProgOperacionCostosMarginalesMensual();
        }
    });

    CargarListaDifusionProgOperacionCostosMarginalesMensual();

    $('#cbBarra').multipleSelect('checkAll');
    $('#cbTension').multipleSelect('checkAll');
});

function CargarListaDifusionProgOperacionCostosMarginalesMensual() {
    
    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    if (empresa == "") empresa = "0";
    $('#hfEmpresa').val(empresa);

    var barras = $('#cbBarra').multipleSelect('getSelects');
    if (barras == "") barras = "0";
    $('#hfBarra').val(barras);

    var tensiones = $('#cbTension').multipleSelect('getSelects');
    if (tensiones == "") tensiones = "0";
    $('#hfTension').val(tensiones);

    var mesAnio = $('#txtFecha').val();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaDifusionProgOperacionCostosMarginalesMensual',
        data: {
            mesAnio: mesAnio,            
            idEmpresa: $('#hfEmpresa').val(),
            idBarras: $('#hfBarra').val(),
            idTension: $('#hfTension').val()
        },
        success: function (aData) {
            $('#listado1').html(aData.Resultado);
            CargarGraficoDifusionProgOperacionCostosMarginalesMensual();
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function CargarGraficoDifusionProgOperacionCostosMarginalesMensual() {

    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    if (empresa == "") empresa = "0";
    $('#hfEmpresa').val(empresa);

    var barras = $('#cbBarra').multipleSelect('getSelects');
    if (barras == "") barras = "0";
    $('#hfBarra').val(barras);

    var tensiones = $('#cbTension').multipleSelect('getSelects');
    if (tensiones == "") tensiones = "0";
    $('#hfTension').val(tensiones);

    var mesAnio = $('#txtFecha').val();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarGraficoDifusionProgOperacionCostosMarginalesMensual',
        dataType: 'json',
        data: {
            mesAnio: mesAnio,
            idEmpresa: $('#hfEmpresa').val(),
            idBarras: $('#hfBarra').val(),
            idTension: $('#hfTension').val()
        },
        success: function (aData) {
            if (aData.Serie1 != null) {
                disenioGrafico('idGrafico1', 'EVOLUCION DE LA PROYECCCION DE LOS COSTOS MARGINALES POR BLOQUE DE DEMANDA DEL SEIN', 4, aData);
            }
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function disenioGrafico(grafico, texto, tip, data) {
    if (tip == 1) {

        var Datoseries = [];
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
        var Name5 = "";

        var Serie1 = [];
        var Serie2 = [];
        var Serie3 = [];
        var Serie4 = [];
        var Serie5 = [];

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
        for (j = 0; j < data.Serie5.length; j++) {
            Serie5[j] = data.Serie5[j];
        }




        Highcharts.chart(grafico, {
            title: {
                text: texto
            },
            xAxis: {
                categories: Categorias
            },
            yAxis: {
                title: {
                    text: 'S/./kWh'
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
             /*   column: {
                    stacking: 'normal',
                    dataLabels: {
                        enabled: true,
                        color: (Highcharts.theme && Highcharts.theme.dataLabelsColor) || 'white'
                    }
                },*/
                series: {
                    label: {
                        connectorAllowed: false
                    }
                }
            },
            series: [{
                type: 'column',
                name: 'PUNTA',
                color: '#85A83D',
                data: Serie3
            }, {
                type: 'column',
                name: 'MEDIA',
                color: '#AD3330',
                data: Serie4
            }, {
                type: 'column',
                name: 'BASE',
                color: '#356CAF',
                data: Serie5
            }, {
                /* type: 'spline',*/
                name: 'PUNTA MAXIMA',
                data: Serie1,
                marker: {
                    lineWidth: 2,
                    /*lineColor: 'blue',
                   /* enabled: false,*/
                    radius: 4
                }
            },
            {
                /* type: 'spline',*/
                name: 'MEDIA MAXIMA',
                data: Serie2,
                marker: {
                    lineWidth: 2,
                    /*lineColor: 'blue',
                   /* enabled: false,*/
                    radius: 4
                }
            }
            ]
        });
    }
}