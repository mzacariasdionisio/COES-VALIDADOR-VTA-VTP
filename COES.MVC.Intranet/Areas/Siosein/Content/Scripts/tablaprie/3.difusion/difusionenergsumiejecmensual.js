var controlador = siteRoot + 'Siosein/TablasPrieDeclaracionMen/'

$(function () {

    activarLLamados();

});

function activarLLamados() {
    cargarListaDifusionEnergSumiEjecMensual();
    //cargarGraficoDifusionEnergSumiEjecMensual();
    //cargarListaDifusionEnergSumiEjecMensualDetalle();
    //cargarGraficoDifusionEnergSumiEjecMensualCausa();
    //cargarGraficoDifusionEnergSumiEjecMensualEquipo();
    //cargarGraficoDifusionEnergSumiEjecMensualTipoEquipo();
}

function cargarListaDifusionEnergSumiEjecMensual() {
    var periodo = $("#txtFecha").val();
    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaDifusionEnergSumiEjecMensual',
        data: { periodo: periodo },
        success: function (aData) {
            $('#listado1').html(aData.Resultado);
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function cargarGraficoDifusionEnergSumiEjecMensual() {
    var periodo = $("#txtFecha").val();
    $.ajax({
        type: 'POST',
        url: controlador + 'CargarGraficoDifusionEnergSumiEjecMensual',
        data: { idEmpresa: $('#hfEmpresa').val(), periodo: periodo },
        dataType: 'json',
        success: function (data) {
            disenioGrafico(data,'idGrafico1', ' ', 1);
            //if (aData.Lista1.length > 0) {
            //    diseñoGrafico(aData.Lista1, 'idGrafico1', 'POTENCIA EFECTIVA POR EMPRESAS');
            //} else $('#idGrafico1').html('Sin Grafico - No existen registros a mostrar!');
        },
        error: function () {
            alert('Ha ocurrido un error');
        }
    });
}
function cargarListaDifusionEnergSumiEjecMensualDetalle() {
    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    if (empresa == "") empresa = "-1";
    $('#hfEmpresa').val(empresa);
    var periodo = $("#txtFecha").val();
    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaDifusionEnergSumiEjecMensualDetalle',
        data: { idEmpresa: $('#hfEmpresa').val(), periodo: periodo },
        success: function (aData) {
            $('#listado2').html(aData.Resultado);
            $('#msj_listado2').show();
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}
function cargarGraficoDifusionEnergSumiEjecMensualCausa() {
    var periodo = $("#txtFecha").val();
    $.ajax({
        type: 'POST',
        url: controlador + 'CargarGraficoDifusionEnergSumiEjecMensualCausa',
        data: { idEmpresa: $('#hfEmpresa').val(), mesAnio : periodo },
        dataType: 'json',
        success: function (data) {
            disenioGrafico(data, 'idGrafico2', 'PARTICIPACION DE LA ENERGIA NO SUMINISTRADA POR TIPO DE CAUSA', 2);
            //if (aData.Lista1.length > 0) {
            //    diseñoGrafico(aData.Lista1, 'idGrafico1', 'POTENCIA EFECTIVA POR EMPRESAS');
            //} else $('#idGrafico1').html('Sin Grafico - No existen registros a mostrar!');
        },
        error: function () {
            alert('Ha ocurrido un error');
        }
    });
}
function cargarGraficoDifusionEnergSumiEjecMensualEquipo() {

    var periodo = $("#txtFecha").val();
    $.ajax({
        type: 'POST',
        url: controlador + 'CargarGraficoDifusionEnergSumiEjecMensualEquipo',
        data: { idEmpresa: $('#hfEmpresa').val(), mesAnio: periodo },
        dataType: 'json',
        success: function (data) {
            disenioGrafico(data,'idGrafico3', 'ENERGIA NO SUMINISTRADA POR TIPO DE EQUIPO', 3);
            //if (aData.Lista1.length > 0) {
            //    diseñoGrafico(aData.Lista1, 'idGrafico1', 'POTENCIA EFECTIVA POR EMPRESAS');
            //} else $('#idGrafico1').html('Sin Grafico - No existen registros a mostrar!');
        },
        error: function () {
            alert('Ha ocurrido un error');
        }
    });
}

function cargarGraficoDifusionEnergSumiEjecMensualTipoEquipo() {
    var periodo = $("#txtFecha").val();
    $.ajax({
        type: 'POST',
        url: controlador + 'CargarGraficoDifusionEnergSumiEjecMensualTipoEquipo',
        data: { idEmpresa: $('#hfEmpresa').val(), mesAnio: periodo },
        dataType: 'json',
        success: function (data) {
            disenioGrafico(data,'idGrafico4', 'ENERGIA INTERRUMPIDA APROXIMADA POR TIPO DE EQUIPO (MWh)', 4);
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
    if (tip == 4) {

        var datos = [];

        for (var i = 0; i < data.Grafico.Series.length; i++) {
            var obj = 
                {   name: data.Grafico.Series[i].Name,
                    y: data.Grafico.Series[i].Acumulado
                }
            datos.push(obj);
        }

        Highcharts.chart(grafico, {
            chart: {
                type: 'column'
            },
            title: {
                text: texto
            },
            subtitle: {
                text: ''
            },
            xAxis: {
                type: 'category'
            },
            yAxis: {
                title: {
                    text: 'MWh'
                }
            },
            legend: {
                enabled: false
            },
            plotOptions: {
                series: {
                    borderWidth: 0,
                    dataLabels: {
                        enabled: true,
                        format: '{point.y:.2f}'
                        //format: '{point.y:.1f}%'
                    }
                }
            },
            tooltip: {
                headerFormat: '<span style="font-size:11px">{series.name}</span><br>',
                pointFormat: '<span style="color:{point.color}">{point.name}</span>: <b>{point.y:.2f}</b> <br/>'
            },

            series: [{
                name: 'Brands',
                colorByPoint: true,
                data: datos
            }]

        });
    }
    if (tip == 1) {

        var tituloTipEmpresa = [];
        var series = [];

        for (var i = 0; i < data.Grafico.XAxisCategories.length; i++) {
            tituloTipEmpresa.push(data.Grafico.XAxisCategories[i]);
        }
        for (var i = 0; i < data.Grafico.Series.length; i++) {
            var obj = { name: data.Grafico.Series[i].Name };

            var valores = [];
            for (var j = 0; j < data.Grafico.Series[i].Data.length; j++) {
                valores.push(data.Grafico.Series[i].Data[j].Y);
            }
            var serie = { name: obj.name, data: valores };
            series.push(serie);
        }

        Highcharts.chart(grafico, {
            chart: {
                type: 'column'
            },
            title: {
                text: texto
            },
            xAxis: {
                categories: tituloTipEmpresa
                    //['Apples', 'Oranges', 'Pears', 'Grapes', 'Bananas']
            },
            yAxis: {
                min: 0,
                title: {
                    text: 'ENS (MWH)'
                },
                stackLabels: {
                    enabled: true,
                    style: {
                        fontWeight: 'bold',
                        color: (Highcharts.theme && Highcharts.theme.textColor) || 'gray'
                    }
                }
            },
            legend: {
                align: 'right',
                x: -30,
                verticalAlign: 'top',
                y: 25,
                floating: true,
                backgroundColor: (Highcharts.theme && Highcharts.theme.background2) || 'white',
                borderColor: '#CCC',
                borderWidth: 1,
                shadow: false
            },
            tooltip: {
                headerFormat: '<b>{point.x}</b><br/>',
                pointFormat: '{series.name}: {point.y}<br/>Total: {point.stackTotal}'
            },
            plotOptions: {
                column: {
                    stacking: 'normal',
                    dataLabels: {
                        enabled: true,
                        color: (Highcharts.theme && Highcharts.theme.dataLabelsColor) || 'white'
                    }
                }
            },
            series: series
            //    [{
            //    name: 'John',
            //    data: [5, 3, 4, 7, 2]
            //}, {
            //    name: 'Jane',
            //    data: [2, 2, 3, 2, 1]
            //}]
        });
    }
    if (tip == 2) {
        var datos = [];

        for (var i = 0; i < data.ListaEveEvento.length; i++) {

            var obj = {
                name: data.ListaEveEvento[i].Causaevendesc,
                y: data.ListaEveEvento[i].CountFallTotal
            }
            datos.push(obj);
        }

        Highcharts.chart(grafico, {
            chart: {
                plotBackgroundColor: null,
                plotBorderWidth: null,
                plotShadow: false,
                type: 'pie'
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
                    dataLabels: {
                        enabled: true,
                        format: '<b>{point.name}</b>: {point.percentage:.1f} %',
                        style: {
                            color: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black'
                        }
                    }
                }
            },
            series: [
                {
                name: 'Brands',
                colorByPoint: true,
                data: datos

            }]
        });
    }
    if (tip == 3) {

        var tituloTipEquipo = ["LÍNEAS DE TRANSMISIÓN",
                                "TRANSFORMADORES",
                                "BARRAS",
                                "UNIDADES DE GENERACIÓN"];
        var series = [];

        for (var i = 0; i < data.ListaEveEvento.length; i++) {

            var obj = {
                name: data.ListaEveEvento[i].Causaevendesc,
                data: [
                    data.ListaEveEvento[i].CountLineaTrans,
                    data.ListaEveEvento[i].CountTransform,
                    data.ListaEveEvento[i].CountBarras,
                    data.ListaEveEvento[i].CountLineaGen,
                ]
                
                
            }
            series.push(obj);
        }

        Highcharts.chart(grafico, {
            chart: {
                type: 'column'
            },
            title: {
                text: texto
            },
            xAxis: {
                categories: tituloTipEquipo
            },
            yAxis: {
                min: 0,
                title: {
                    text: 'N° DE FALLAS'
                }
            },
            tooltip: {
                pointFormat: '<span style="color:{series.color}">{series.name}</span>: <b>{point.y}</b> ({point.percentage:.0f}%)<br/>',
                shared: true
            },
            plotOptions: {
                column: {
                    stacking: 'count'
                }
            },
            series: series
        });
    }
    
}