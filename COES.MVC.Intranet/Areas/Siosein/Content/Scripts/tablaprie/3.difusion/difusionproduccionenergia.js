var controlador = siteRoot + 'Siosein/TablasPrieDeclaracionMen/'

$(function () {

    $('#txtFecha').Zebra_DatePicker({
        format: 'm Y'
    });

    //#region SIOSEIN-PRIE-2021
    $('#cbEmpresa').multipleSelect({
        width: '350px',
        filter: true,
        onClose: function (view) {
        }
    });
    //#endregion

    $('#cbTipGene').multipleSelect({
        width: '150px',
        filter: true,
        onClose: function (view) {
            //cargarCentralxEmpresa();
            //CargarCostoMarginal();
        }
    });
    $('#cbRecEnerg').multipleSelect({
        width: '150px',
        filter: true,
        onClose: function (view) {
            //CargarCostoMarginal();
        }
    });
    
    $('#cbEmpresa').multipleSelect('checkAll');
    $('#cbTipGene').multipleSelect('checkAll');
    $('#cbRecEnerg').multipleSelect('checkAll');

    //cargarListaDifusionProduccionEnergia(); //SIOSEIN-PRIE-2021
    CargarCostoMarginal(); //SIOSEIN-PRIE-2021
});


function cargarListaDifusionProduccionEnergia() {

    var idEmpresa = $('#cbEmpresa').multipleSelect('getSelects'); //SIOSEIN-PRIE-2021
    var tipogene = $('#cbTipGene').multipleSelect('getSelects');
    var recenerg = $('#cbRecEnerg').multipleSelect('getSelects');
    var periodo = $("#txtFecha").val();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaDifusionProduccionEnergia',
        dataType: 'json',
        contentType: 'application/json; charset=utf8',
        data: JSON.stringify({ idEmpresa: idEmpresa, periodo: periodo, tipoGene: tipogene, recenerg: recenerg }), //SIOSEIN-PRIE-2021
        success: function (aData) {
            $('#listado1').html(aData.Resultado);
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}


function CargarCostoMarginal() {
    cargarListaDifusionProduccionEnergia();

    CargarListaDifusionIntercambioEnergiaDiaMaxDemanda();

    cargarListaDifusionProduccionEnergiaXEmpresa();
    CargarGraficoProdEnergXTipGenYRecurEnerg();
    CargarListaDifusionProdEnergXCentralYRecursoEner();
    CargarGraficoProdenergXTipoCentralYRecurEnerg();

    CargarListaDifusionProdEnergiaMaxDemanda();
    CargarGraficoDifusionProdEnergiaMaxDemanda();

    //CargarGraficoDifusionProdEnergialXTipoTecnologia(); //SIOSEIN-COES-PRIE
    CargarListaDifusionProdEnergiaXTipoTecnologia();
}
function CargarGraficoProdEnergXTipGenYRecurEnerg() {
    var parametro = 0;
    if ($('#idEneAct').is(':checked')) parametro = 1;
    if ($('#idMaxDem').is(':checked')) parametro = 2;

    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    if (empresa == "") empresa = "-1";
    $('#hfEmpresa').val(empresa);

    var tipogene = $('#cbTipGene').multipleSelect('getSelects');
    if (tipogene == "") tipogene = "-1";
    $('#hfTipGene').val(tipogene);

    var recenerg = $('#cbRecEnerg').multipleSelect('getSelects');
    if (recenerg == "") recenerg = "-1";
    $('#hfRecEnerg').val(recenerg);

    var mesAnio = $('#txtFecha').val();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarGraficoProdEnergXTipGenYRecurEnerg',
        data: { tipParametro: parametro, idEmpresa: $('#hfEmpresa').val(), tipoGene: $('#hfTipGene').val(), recenerg: $('#hfRecEnerg').val(), mesAnio: mesAnio },
        dataType: 'json',
        success: function (data) {
            //disenioGrafico('idGrafico1', 'HORAS DE OPERACION POR MODO DE OPERACION', 1);
            //if (data.TipoEmpresas.length > 0) {
            disenioGrafico(data, 'idGrafico2', 'PRODUCCIÓN DE ENERGÍA ELÉCTRICA POR TIPO DE GENERACIÓN Y RECURSO ENERGÉTICO', 2);
            //} else $('#idGrafico1').html('Sin Grafico - No existen registros a mostrar!');
        },
        error: function () {
            alert('Ha ocurrido un error');
        }
    });
}
function CargarGraficoDifusionProdEnergialXTipoTecnologia() {
    var parametro = 0;
    if ($('#idEneAct').is(':checked')) parametro = 1;
    if ($('#idMaxDem').is(':checked')) parametro = 2;

    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    if (empresa == "") empresa = "-1";
    $('#hfEmpresa').val(empresa);

    var tipogene = $('#cbTipGene').multipleSelect('getSelects');
    if (tipogene == "") tipogene = "-1";
    $('#hfTipGene').val(tipogene);

    var recenerg = $('#cbRecEnerg').multipleSelect('getSelects');
    if (recenerg == "") recenerg = "-1";
    $('#hfRecEnerg').val(recenerg);

    var periodo = $("#txtFecha").val();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarGraficoDifusionProdEnergialXTipoTecnologia',
        data: { tipParametro: parametro, idEmpresa: $('#hfEmpresa').val(), tipoGene: $('#hfTipGene').val(), recenerg: $('#hfRecEnerg').val(), periodo: periodo },
        dataType: 'json',
        success: function (data) {
            disenioGrafico(data, 'idGrafico5', 'PARTICIPACIÓN DE LA MÁXIMA DEMANDA DEL SEIN POR EMPRESAS INTEGRANTES DEL COES', 5);
            //if (aData.Lista1.length > 0) {
            //    diseñoGrafico(aData.Lista1, 'idGrafico1', 'POTENCIA EFECTIVA POR EMPRESAS');
            //} else $('#idGrafico1').html('Sin Grafico - No existen registros a mostrar!');
        },
        error: function () {
            alert('Ha ocurrido un error');
        }
    });
}
function CargarListaDifusionIntercambioEnergiaDiaMaxDemanda() {
    var parametro = 0;
    if ($('#idEneAct').is(':checked')) parametro = 1;
    if ($('#idMaxDem').is(':checked')) parametro = 2;

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
        url: controlador + 'CargarListaDifusionIntercambioEnergiaDiaMaxDemanda',
        data: { tipParametro: parametro, idEmpresa: $('#hfEmpresa').val(), tipoGene: $('#hfTipGene').val(), recenerg: $('#hfRecEnerg').val(), periodo: mesAnio },
        success: function (data) {
            $('#listado6').html(data.Resultado);
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}
function CargarListaDifusionProdEnergiaXTipoTecnologia() {
    var parametro = 0;
    if ($('#idEneAct').is(':checked')) parametro = 1;
    if ($('#idMaxDem').is(':checked')) parametro = 2;

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
        url: controlador + 'CargarListaDifusionProdEnergiaXTipoTecnologia',
        data: { tipParametro: parametro, idEmpresa: $('#hfEmpresa').val(), tipoGene: $('#hfTipGene').val(), recenerg: $('#hfRecEnerg').val(), periodo: mesAnio },
        success: function (aData) {
            $('#listado5').html(aData.Resultado);
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}
function CargarGraficoDifusionProdEnergiaMaxDemanda() {
    var parametro = 0;
    if ($('#idEneAct').is(':checked')) parametro = 1;
    if ($('#idMaxDem').is(':checked')) parametro = 2;

    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    if (empresa == "") empresa = "-1";
    $('#hfEmpresa').val(empresa);

    var tipogene = $('#cbTipGene').multipleSelect('getSelects');
    if (tipogene == "") tipogene = "-1";
    $('#hfTipGene').val(tipogene);

    var recenerg = $('#cbRecEnerg').multipleSelect('getSelects');
    if (recenerg == "") recenerg = "-1";
    $('#hfRecEnerg').val(recenerg);

    var periodo = $("#txtFecha").val();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarGraficoDifusionProdEnergiaMaxDemanda',
        data: { tipParametro: parametro, idEmpresa: $('#hfEmpresa').val(), tipoGene: $('#hfTipGene').val(), recenerg: $('#hfRecEnerg').val(), periodo: periodo },
        dataType: 'json',
        success: function (data) {
            disenioGrafico(data, 'idGrafico4', 'PARTICIPACIÓN DE LA MÁXIMA DEMANDA DEL SEIN POR EMPRESAS INTEGRANTES DEL COES', 4);
            //if (aData.Lista1.length > 0) {
            //    diseñoGrafico(aData.Lista1, 'idGrafico1', 'POTENCIA EFECTIVA POR EMPRESAS');
            //} else $('#idGrafico1').html('Sin Grafico - No existen registros a mostrar!');
        },
        error: function () {
            alert('Ha ocurrido un error 5');
        }
    });
}
function CargarListaDifusionProdEnergiaMaxDemanda() {
    var parametro = 0;
    if ($('#idEneAct').is(':checked')) parametro = 1;
    if ($('#idMaxDem').is(':checked')) parametro = 2;

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
        url: controlador + 'CargarListaDifusionProdEnergiaMaxDemanda',
        data: { tipParametro: parametro, idEmpresa: $('#hfEmpresa').val(), tipoGene: $('#hfTipGene').val(), recenerg: $('#hfRecEnerg').val(), periodo: mesAnio },
        success: function (data) {
            $('#listado4').html(data.Resultado);
        },
        error: function () {
            alert("Ha ocurrido un error 6");
        }
    });
}
function CargarGraficoProdenergXTipoCentralYRecurEnerg() {
    var parametro = 0;
    if ($('#idEneAct').is(':checked')) parametro = 1;
    if ($('#idMaxDem').is(':checked')) parametro = 2;

    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    if (empresa == "") empresa = "-1";
    $('#hfEmpresa').val(empresa);

    var tipogene = $('#cbTipGene').multipleSelect('getSelects');
    if (tipogene == "") tipogene = "-1";
    $('#hfTipGene').val(tipogene);

    var recenerg = $('#cbRecEnerg').multipleSelect('getSelects');
    if (recenerg == "") recenerg = "-1";
    $('#hfRecEnerg').val(recenerg);

    var periodo = $("#txtFecha").val();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarGraficoProdenergXTipoCentralYRecurEnerg',
        data: { tipParametro: parametro, idEmpresa: $('#hfEmpresa').val(), tipoGene: $('#hfTipGene').val(), recenerg: $('#hfRecEnerg').val(), periodo: periodo },
        dataType: 'json',
        success: function (data) {
            disenioGrafico(data, 'idGrafico3', 'PRODUCCIÓN DE ENERGÍA ELÉCTRICA POR TIPO DE CENTRAL Y RECURSO ENERGÉTICO', 3);
            //if (aData.Lista1.length > 0) {
            //    diseñoGrafico(aData.Lista1, 'idGrafico1', 'POTENCIA EFECTIVA POR EMPRESAS');
            //} else $('#idGrafico1').html('Sin Grafico - No existen registros a mostrar!');
        },
        error: function () {
            alert('Ha ocurrido un error');
        }
    });
}
function CargarListaDifusionProdEnergXCentralYRecursoEner() {
    var parametro = 0;
    if ($('#idEneAct').is(':checked')) parametro = 1;
    if ($('#idMaxDem').is(':checked')) parametro = 2;

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
        url: controlador + 'CargarListaDifusionProdEnergXCentralYRecursoEner',
        data: { tipParametro: parametro, idEmpresa: $('#hfEmpresa').val(), tipoGene: $('#hfTipGene').val(), recenerg: $('#hfRecEnerg').val(), periodo: mesAnio },
        success: function (data) {
            $('#listado3').html(data.Resultado);
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function cargarListaDifusionProduccionEnergiaXEmpresa() {
    var parametro = 0;
    if ($('#idEneAct').is(':checked')) parametro = 1;
    if ($('#idMaxDem').is(':checked')) parametro = 2;

    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    if (empresa == "") empresa = "-1";
    $('#hfEmpresa').val(empresa);

    var tipogene = $('#cbTipGene').multipleSelect('getSelects');
    if (tipogene == "") tipogene = "-1";
    $('#hfTipGene').val(tipogene);

    var recenerg = $('#cbRecEnerg').multipleSelect('getSelects');
    if (recenerg == "") recenerg = "-1";
    $('#hfRecEnerg').val(recenerg);

    var periodo = $("#txtFecha").val();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaDifusionProduccionEnergiaXEmpresa',
        data: { tipParametro: parametro, idEmpresa: $('#hfEmpresa').val(), tipoGene: $('#hfTipGene').val(), recenerg: $('#hfRecEnerg').val(), periodo: periodo },
        success: function (aData) {
            $('#listado2').html(aData.Resultado);
            cargarGraficoDifusionProduccionEnergiaXEmpresa();
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}
function cargarGraficoDifusionProduccionEnergiaXEmpresa() {
    var parametro = 0;
    if ($('#idEneAct').is(':checked')) parametro = 1;
    if ($('#idMaxDem').is(':checked')) parametro = 2;

    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    if (empresa == "") empresa = "-1";
    $('#hfEmpresa').val(empresa);

    var tipogene = $('#cbTipGene').multipleSelect('getSelects');
    if (tipogene == "") tipogene = "-1";
    $('#hfTipGene').val(tipogene);

    var recenerg = $('#cbRecEnerg').multipleSelect('getSelects');
    if (recenerg == "") recenerg = "-1";
    $('#hfRecEnerg').val(recenerg);

    var periodo = $("#txtFecha").val();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarGraficoDifusionProduccionEnergiaXEmpresa',
        data: { tipParametro: parametro, idEmpresa: $('#hfEmpresa').val(), tipoGene: $('#hfTipGene').val(), recenerg: $('#hfRecEnerg').val(), periodo: periodo },
        dataType: 'json',
        success: function (data) {
            disenioGrafico(data, 'idGrafico1', 'PRODUCCION DE ENERGIA ELECTRICA POR EMPRESAS', 1);
            //if (aData.Lista1.length > 0) {
            //    diseñoGrafico(aData.Lista1, 'idGrafico1', 'POTENCIA EFECTIVA POR EMPRESAS');
            //} else $('#idGrafico1').html('Sin Grafico - No existen registros a mostrar!');
        },
        error: function () {
            alert('Ha ocurrido un error');
        }
    });
}
function disenioGrafico(data, grafico, titulo, tip) {
    if (tip == 5) {
        var categorias = [];
        var series = [];

        for (var i = 0; i < data.Grafico.XAxisCategories.length; i++) {
            categorias.push(data.Grafico.XAxisCategories[i]);
        }
        for (var i = 0; i < data.Grafico.Series.length; i++) {
            var valores = [];
            for (var j = 0; j < data.Grafico.Series[i].Data.length; j++) {
                valores.push(data.Grafico.Series[i].Data[j].Y);
            }
            series.push({
                type: data.Grafico.Series[i].Type,
                name: data.Grafico.Series[i].Name,
                data: valores
            });

        }

        var charts = Highcharts.chart(grafico, {
            chart: {
            },
            title: {
                text: titulo
            },
            xAxis: {

                categories: categorias,
                type: 'datetime',
                title: {
                    text: ''
                }
            },
            yAxis: {
                title: {
                    text: 'MW'
                }
            },
            //tooltip: {
            //    valueSuffix: ' millions'
            //},
            plotOptions: {
                area: {
                    marker: {
                        enabled: false
                    }
                }
            },
            series: series
        });

        var maxValue;
        for (var i = 0; i < charts.series[0].length; i++) {
            for (var j = 0; j < charts.series[0][i].data.length; j++) {

            }
        }
    }
    if (tip == 4) {
        var serieData = [];
        for (var i = 0; i < data.Grafico.Series.length; i++) {
            var obj = [data.Grafico.Series[i].Name, data.Grafico.Series[i].Acumulado];
            serieData.push(obj);
        }
        Highcharts.chart(grafico, {
            chart: {
                type: 'pie',
                options3d: {
                    enabled: true,
                    alpha: 65,
                    beta: 0
                }
            },
            title: {
                text: titulo
            },
            tooltip: {
                pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
            },
            plotOptions: {
                pie: {
                    allowPointSelect: true,
                    cursor: 'pointer',
                    depth: 75,
                    dataLabels: {
                        enabled: true,
                        format: '{point.name}: {point.percentage:.1f} %'
                    }
                }
            },
            series: [{
                type: 'pie',
                name: 'Participación de la máxima demanda',
                data: serieData
            }]
        });
    }
    if (tip == 3) {
    
        var datos = [];

        for (var i = 0; i < data.Grafico.Series.length; i++) {
            var obj = { name: data.Grafico.Series[i].Name, y: data.Grafico.Series[i].Acumulado }
            datos.push(obj);
        }

        Highcharts.chart(grafico, {
            chart: {
                type: 'pie',
                options3d: {
                    enabled: true,
                    alpha: 65,
                    beta: 0
                }
            },
            title: {
                text: titulo
            },
            tooltip: {
                pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
            },
            plotOptions: {
                pie: {
                    allowPointSelect: true,
                    cursor: 'pointer',
                    depth: 75,
                    dataLabels: {
                        enabled: true,
                        format: '{point.name}'
                    }
                }
            },
            series: [{
                type: 'pie',
                name: 'Browser share',
                data: datos
                    //[
                    //['Firefox', 45.0],
                    //['IE', 26.8],
                    //['Chrome',12.8,],
                    //['Safari', 8.5],
                    //['Opera', 6.2],
                    //['Others', 0.7]
                    //]
            }]
        });
    }
    if (tip == 2) {
        var categorias = [];
        var series = [];

        for (var i = 0; i < data.Grafico.XAxisCategories.length; i++) {
            categorias.push(data.Grafico.XAxisCategories[i]);
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
                text: titulo
            },
            xAxis: {
                categories: categorias
            },
            yAxis: {
                min: 0,
                title: {
                    text: 'GWh' 
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
            //[
            //{ name: 'John', data:  [5, 3, 4, 7] },
            //{ name: 'Jane', data:  [2, 2, 3, 2] },
            //{ name: 'Dennis',data: [5, 3, 6, 4] }
            //]
        });
    }
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
                plotBackgroundColor: null,
                plotBorderWidth: null,
                plotShadow: false,
                type: 'pie'
            },
            title: {
                text: titulo
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
                        },
                        connectorColor: 'silver'
                    }
                }
            },
            series: [{
                name: 'Brands',
                data: datos
            }]
        });
    }
    
}