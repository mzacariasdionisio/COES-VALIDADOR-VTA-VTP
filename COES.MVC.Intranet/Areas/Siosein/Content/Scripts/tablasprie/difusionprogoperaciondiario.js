var controlador = siteRoot + 'Siosein/TablasPrieDeclaracionMen/'

$(function () {

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

    $('#cbEmpresa').multipleSelect('checkAll');
    $('#cbTipGene').multipleSelect('checkAll');
    $('#cbRecEnerg').multipleSelect('checkAll');

    llamarMetodos();
    function llamarMetodos() {
        cargarListaDifusionProgOperacionDiario();
        cargarListaDifusionProgOperacionDiarioXEmpresa();
        cargarListaDifusionProgOperacionDiarioXCentralRecEnerg();
        cargarListaDifusionProgOperacionDiarioMaxDemanda();
        cargarListaDifusionProgOperacionDiarioXRecEnergTipTecnologia();
    }
});

function cargarListaDifusionProgOperacionDiarioXEmpresa() {
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
        url: controlador + 'CargarListaDifusionProgOperacionDiarioXEmpresa',
        data: { tipParametro: parametro, idEmpresa: $('#hfEmpresa').val(), tipoGene: $('#hfTipGene').val(), recenerg: $('#hfRecEnerg').val(), periodo: periodo },
        success: function (aData) {
            $('#listado2').html(aData.Resultado);
            cargarGraficoDifusionProgOperacionDiarioXEmpresa();
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function cargarListaDifusionProgOperacionDiario() {
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
        url: controlador + 'CargarListaDifusionProgOperacionDiario',
        data: { tipParametro: parametro, idEmpresa: $('#hfEmpresa').val(), tipoGene: $('#hfTipGene').val(), recenerg: $('#hfRecEnerg').val(), periodo: periodo },
        success: function (aData) {
            $('#listado1').html(aData.Resultado);
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function cargarGraficoDifusionProgOperacionDiarioXEmpresa() {
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
        url: controlador + 'CargarGraficoDifusionProgOperacionDiarioXEmpresa',
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

function cargarListaDifusionProgOperacionDiarioXCentralRecEnerg() {
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
        url: controlador + 'CargarListaDifusionProgOperacionDiarioXCentralRecEnerg',
        data: { tipParametro: parametro, idEmpresa: $('#hfEmpresa').val(), tipoGene: $('#hfTipGene').val(), recenerg: $('#hfRecEnerg').val(), periodo: mesAnio },
        success: function (aData) {
            $('#listado3').html(aData);
            cargarGraficoDifusionProgOperacionDiarioXCentralRecEnerg();
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function cargarListaDifusionProgOperacionDiarioMaxDemanda() {
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
        url: controlador + 'CargarListaDifusionProgOperacionDiarioMaxDemanda',
        data: { tipParametro: parametro, idEmpresa: $('#hfEmpresa').val(), tipoGene: $('#hfTipGene').val(), recenerg: $('#hfRecEnerg').val(), periodo: mesAnio },
        success: function (aData) {
            $('#listado4').html(aData.Resultado);
            cargarGraficoDifusionProgOperacionDiarioMaxDemanda();
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function cargarGraficoDifusionProgOperacionDiarioMaxDemanda() {
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
        url: controlador + 'CargarGraficoDifusionProgOperacionDiarioMaxDemanda',
        data: { tipParametro: parametro, idEmpresa: $('#hfEmpresa').val(), tipoGene: $('#hfTipGene').val(), recenerg: $('#hfRecEnerg').val(), periodo: periodo },
        dataType: 'json',
        success: function (data) {
            disenioGrafico(data, 'idGrafico2', 'PARTICIPACION DE LA MAXIMA DEMANDA DEL SEIN POR EMPRESAS INTEGRANTES DEL COES', 2);
            //if (aData.Lista1.length > 0) {
            //    diseñoGrafico(aData.Lista1, 'idGrafico1', 'POTENCIA EFECTIVA POR EMPRESAS');
            //} else $('#idGrafico1').html('Sin Grafico - No existen registros a mostrar!');
        },
        error: function () {
            alert('Ha ocurrido un error');
        }
    });
}

function cargarGraficoDifusionProgOperacionDiarioXCentralRecEnerg() {
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
        url: controlador + 'CargarGraficoDifusionProgOperacionDiarioXCentralRecEnerg',
        data: { tipParametro: parametro, idEmpresa: $('#hfEmpresa').val(), tipoGene: $('#hfTipGene').val(), recenerg: $('#hfRecEnerg').val(), periodo: periodo },
        dataType: 'json',
        success: function (data) {
            disenioGrafico(data, 'idGrafico3', 'DESPACHO POR TIPO DE COMBUSTIBLE Y TECNOLOGIA EN EL DIA DE MAXIMA DEMANDA DEL SEIN', 3);
            //if (aData.Lista1.length > 0) {
            //    diseñoGrafico(aData.Lista1, 'idGrafico1', 'POTENCIA EFECTIVA POR EMPRESAS');
            //} else $('#idGrafico1').html('Sin Grafico - No existen registros a mostrar!');
        },
        error: function () {
            alert('Ha ocurrido un error');
        }
    });
}

function cargarListaDifusionProgOperacionDiarioXRecEnergTipTecnologia() {
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
        url: controlador + 'CargarListaDifusionProgOperacionDiarioXRecEnergTipTecnologia',
        data: { tipParametro: parametro, idEmpresa: $('#hfEmpresa').val(), tipoGene: $('#hfTipGene').val(), recenerg: $('#hfRecEnerg').val(), periodo: mesAnio },
        success: function (aData) {
            $('#listado5').html(aData.Resultado);
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function disenioGrafico(data, grafico, texto, tip) {
    console.log("data", data);
    if (tip == 2) {
        var serieData = [];
        //SIOSEIN-PRIE-2021
        if (data != null && data.Grafico != null && data.Grafico.Series != null && data.Grafico.Series.length > 0) {
        //
            for (var i = 0; i < data.Grafico.Series.length; i++) {
                var obj = [data.Grafico.Series[i].Name, data.Grafico.Series[i].Acumulado];
                serieData.push(obj);
            }
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
                text: texto
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
    if (tip == 1) {
        var datos = [];
        //SIOSEIN-PRIE-2021
        if (data != null && data.Grafico != null && data.Grafico.Series != null && data.Grafico.Series.length > 0) {
        //
            for (var i = 0; i < data.Grafico.Series.length; i++) {
                var obj = {
                    name: data.Grafico.Series[i].Name,
                    y: data.Grafico.Series[i].Acumulado
                }
                datos.push(obj);
            }
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

    if (tip == 3) {
        var categorias = [];
        var series = [];
        //SIOSEIN-PRIE-2021
        if (data != null && data.Grafico != null && data.Grafico.XAxisCategories != null && data.Grafico.XAxisCategories.length > 0) {
        //
            for (var i = 0; i < data.Grafico.XAxisCategories.length; i++) {
                categorias.push(data.Grafico.XAxisCategories[i]);
            }
        }

        //SIOSEIN-PRIE-2021
        if (data != null && data.Grafico != null && data.Grafico.Series != null && data.Grafico.Series.length > 0) {
        //
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
        }

        var charts = Highcharts.chart(grafico, {
            chart: {
            },
            title: {
                text: texto
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

        //var maxValue;
        //for (var i = 0; i < charts.series[0].length; i++) {
        //    for (var j = 0; j < charts.series[0][i].data.length; j++) {

        //    }
        //}
    }
}
