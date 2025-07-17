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
        //onClose: function (view) {
        onClose: function () {
            llamarMetodos();
        }
    });

    $('#cbEmpresa').multipleSelect('checkAll');
    $('#cbTipGene').multipleSelect('checkAll');
    $('#cbRecEnerg').multipleSelect('checkAll');

    llamarMetodos();
    function llamarMetodos() {
        cargarListaDifusionProgOperacionSemanal();
        cargarListaDifusionProgOperacionSemanalXEmpresa();
        cargarListaDifusionProgOperacionSemanalXCentralRecEnerg();
        cargarListaDifusionProgOperacionSemanalMaxDemanda();
        cargarListaDifusionProgOperacionSemanalXRecEnergTipTecnologia();
        CargarGraficoDifusionProgOperacionSemanalMaxDemanda();
    }
});


function cargarListaDifusionProgOperacionSemanal() {
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

    var mesAnio = $('#txtFecha1').val();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaDifusionProgOperacionSemanal',
        data: { tipParametro: parametro, idEmpresa: $('#hfEmpresa').val(), tipoGene: $('#hfTipGene').val(), recenerg: $('#hfRecEnerg').val(), periodo: mesAnio },
        success: function (aData) {
            $('#listado1').html(aData.Resultado);
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}


function cargarListaDifusionProgOperacionSemanalXEmpresa() {
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

    var mesAnio = $('#txtFecha1').val();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaDifusionProgOperacionSemanalXEmpresa',
        data: { tipParametro: parametro, idEmpresa: $('#hfEmpresa').val(), tipoGene: $('#hfTipGene').val(), recenerg: $('#hfRecEnerg').val(), periodo: mesAnio },
        success: function (aData) {
            $('#listado2').html(aData.Resultado);
            cargarGraficoDifusionProgOperacionSemanalXEmpresa();
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}


function cargarGraficoDifusionProgOperacionSemanalXEmpresa() {
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

    var periodo = $("#txtFecha1").val();

    $.ajax({
        type: 'POST',
        url: controlador + 'cargarGraficoDifusionProgOperacionSemanalXEmpresa',
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


function cargarListaDifusionProgOperacionSemanalMaxDemanda() {
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

    var mesAnio = $('#txtFecha1').val();

    $.ajax({
        type: 'POST',
        url: controlador + 'cargarListaDifusionProgOperacionSemanalMaxDemanda',
        data: { tipParametro: parametro, idEmpresa: $('#hfEmpresa').val(), tipoGene: $('#hfTipGene').val(), recenerg: $('#hfRecEnerg').val(), periodo: mesAnio },
        success: function (aData) {
            $('#listado4').html(aData.Resultado);
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}


function CargarGraficoDifusionProgOperacionSemanalMaxDemanda() {
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

    var periodo = $("#txtFecha1").val();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarGraficoDifusionProgOperacionSemanalMaxDemanda',
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

    //var parametro = 0;
    //if ($('#idEneAct').is(':checked')) parametro = 1;
    //if ($('#idMaxDem').is(':checked')) parametro = 2;

    //var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    //if (empresa == "") empresa = "0";
    //$('#hfEmpresa').val(empresa);

    //var tipogene = $('#cbTipGene').multipleSelect('getSelects');
    //if (tipogene == "") tipogene = "0";
    //$('#hfTipGene').val(tipogene);

    //var recenerg = $('#cbRecEnerg').multipleSelect('getSelects');
    //if (recenerg == "") recenerg = "0";
    //$('#hfRecEnerg').val(recenerg);
    //var mesAnio = $('#txtFecha1').val();
    //$.ajax({
    //    type: 'POST',
    //    url: controlador + 'CargarGraficoDifusionProgOperacionSemanalMaxDemanda',
    //    data: { tipParametro: parametro, idEmpresa: $('#hfEmpresa').val(), tipoGene: $('#hfTipGene').val(), recenerg: $('#hfRecEnerg').val(), periodo: mesAnio },
    //    dataType: 'json',
    //    success: function (data) {
    //        disenioGrafico(data, 'idGrafico2', 'PARTICIPACION DE LA MAXIMA DEMANDA DEL SEIN POR EMPRESAS INTEGRANTES DEL COES', 2);
    //        //if (aData.Lista1.length > 0) {
    //        //    diseñoGrafico(aData.Lista1, 'idGrafico1', 'POTENCIA EFECTIVA POR EMPRESAS');
    //        //} else $('#idGrafico1').html('Sin Grafico - No existen registros a mostrar!');
    //    },
    //    error: function () {
    //        alert('Ha ocurrido un error');
    //    }
    //});
}


function cargarListaDifusionProgOperacionSemanalXCentralRecEnerg() {
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

    var mesAnio = $('#txtFecha1').val();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaDifusionProgOperacionSemanalXCentralRecEnerg',
        data: { tipParametro: parametro, idEmpresa: $('#hfEmpresa').val(), tipoGene: $('#hfTipGene').val(), recenerg: $('#hfRecEnerg').val(), periodo: mesAnio },
        success: function (aData) {
            $('#listado3').html(aData.Resultado);
            cargarGraficoDifusionProgOperacionSemanalXCentralRecEnerg();
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}


function cargarGraficoDifusionProgOperacionSemanalXCentralRecEnerg() {
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

    var periodo = $("#txtFecha1").val();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarGraficoDifusionProgOperacionSemanalXCentralRecEnerg',
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


function cargarListaDifusionProgOperacionSemanalXRecEnergTipTecnologia() {
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

    var mesAnio = $('#txtFecha1').val();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaDifusionProgOperacionSemanalXRecEnergTipTecnologia',
        data: { tipParametro: parametro, idEmpresa: $('#hfEmpresa').val(), tipoGene: $('#hfTipGene').val(), recenerg: $('#hfRecEnerg').val(), periodo: mesAnio },
        success: function (aData) {
            $('#listado5').html(aData.Resultado);
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
    //var parametro = 0;
    //if ($('#idEneAct').is(':checked')) parametro = 1;
    //if ($('#idMaxDem').is(':checked')) parametro = 2;

    //var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    //if (empresa == "") empresa = "0";
    //$('#hfEmpresa').val(empresa);

    //var tipogene = $('#cbTipGene').multipleSelect('getSelects');
    //if (tipogene == "") tipogene = "0";
    //$('#hfTipGene').val(tipogene);

    //var recenerg = $('#cbRecEnerg').multipleSelect('getSelects');
    //if (recenerg == "") recenerg = "0";
    //$('#hfRecEnerg').val(recenerg);

    //var mesAnio = $('#txtFecha1').val();

    //$.ajax({
    //    type: 'POST',
    //    url: controlador + 'CargarListaDifusionProgOperacionSemanalXRecEnergTipTecnologia',
    //    data: { tipParametro: parametro, idEmpresa: $('#hfEmpresa').val(), tipoGene: $('#hfTipGene').val(), recenerg: $('#hfRecEnerg').val(), periodo: mesAnio },
    //    success: function (aData) {
    //        $('#listado5').html(aData);
    //    },
    //    error: function () {
    //        alert("Ha ocurrido un error");
    //    }
    //});
}


function disenioGrafico(data, grafico, texto, tip) {
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
                //SIOSEIN-PRIE-2021
                if (data.Grafico.Series[i] != null && data.Grafico.Series[i].Data != null && data.Grafico.Series[i].Data.length > 0) {
                //
                    for (var j = 0; j < data.Grafico.Series[i].Data.length; j++) {
                        valores.push(data.Grafico.Series[i].Data[j].Y);
                    }
                }
                //SIOSEIN-PRIE-2021
                if (data.Grafico.Series[i] != null) {
                //
                    series.push({
                        type: data.Grafico.Series[i].Type,
                        name: data.Grafico.Series[i].Name,
                        data: valores
                    });
                }
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

        var maxValue;
        //SIOSEIN-PRIE-2021
        //for (var i = 0; i < charts.series[0].length; i++) {
        //    for (var j = 0; j < charts.series[0][i].data.length; j++) {

        //    }
        //}
        //
    }
}
