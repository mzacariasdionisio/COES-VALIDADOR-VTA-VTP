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

    $('#cbTipGene').multipleSelect({
        width: '150px',
        filter: true,
        onClose: function (view) {
            //cargarCentralxEmpresa();
            activarLLamados();
        }
    });

    $('#cbParam').multipleSelect({
        width: '150px',
        filter: true,
        onClose: function (view) {
            //cargarCentralxEmpresa();
            activarLLamados();
        }
    });

    activarLLamados();

    $('#cbEmpresa').multipleSelect('checkAll');
    $('#cbTipGene').multipleSelect('checkAll');
    $('#cbParam').multipleSelect('checkAll');
});

function activarLLamados() {
    cargarListaDifusionDesviaciones();
    cargarGraficoDifusionDesviaciones();
	cargarListaDifusionResumenDesviaciones();
	cargarGraficoDifusionResumenDesviaciones(2);
	cargarGraficoDifusionResumenDesviaciones(3);
	cargarGraficoDifusionResumenDesviaciones(4);
	cargarGraficoDifusionResumenDesviaciones(5);
}

function cargarGraficoDifusionResumenDesviaciones(x) {
    var parametro = $('#cbTipGene').multipleSelect('getSelects');
    if (parametro == "") parametro = "-1";
    $('#hfParam').val(parametro);

    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    if (empresa == "") empresa = "-1";
    $('#hfEmpresa').val(empresa);

    var tipogene = $('#cbTipGene').multipleSelect('getSelects');
    if (tipogene == "") tipogene = "-1";
    $('#hfTipGene').val(tipogene);

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarGraficoDifusionDesviaciones',
        data: { tipParametro: $('#hfParam').val(), idEmpresa: $('#hfEmpresa').val(), tipoGene: $('#hfTipGene').val(), mesAnio: $('#txtFecha').val(), param: x },
        dataType: 'json',
        success: function (data) {
            if (data.nRegistros > 0) {
                disenioGrafico(data, 'idGrafico' + x, 1);
            } else { $('#idGrafico' + x).html('Sin Grafico - No existen registros a mostrar!'); }
        },
        error: function () {
            alert('Ha ocurrido un error');
        }
    });
}

function cargarGraficoDifusionDesviaciones() {
    var parametro = $('#cbTipGene').multipleSelect('getSelects');
    if (parametro == "") parametro = "-1";
    $('#hfParam').val(parametro);

    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    if (empresa == "") empresa = "-1";
    $('#hfEmpresa').val(empresa);

    var tipogene = $('#cbTipGene').multipleSelect('getSelects');
    if (tipogene == "") tipogene = "-1";
    $('#hfTipGene').val(tipogene);

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarGraficoDifusionDesviaciones',
        data: { tipParametro: $('#hfParam').val(), idEmpresa: $('#hfEmpresa').val(), tipoGene: $('#hfTipGene').val(), mesAnio: $('#txtFecha').val(), param: 0 },
        dataType: 'json',
        success: function (data) {
            if (data.nRegistros > 0) {
                disenioGrafico(data, 'idGrafico1', 1);
            } else { $('#idGrafico1').html('Sin Grafico - No existen registros a mostrar!'); }
        },
        error: function () {
            alert('Ha ocurrido un error');
        }
    });
}

function cargarListaDifusionDesviaciones() {
    var parametro = $('#cbTipGene').multipleSelect('getSelects');
    if (parametro == "") parametro = "-1";
    $('#hfParam').val(parametro);

    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    if (empresa == "") empresa = "-1";
    $('#hfEmpresa').val(empresa);

    var tipogene = $('#cbTipGene').multipleSelect('getSelects');
    if (tipogene == "") tipogene = "-1";
    $('#hfTipGene').val(tipogene);

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaDifusionDesviaciones',
        data: { tipParametro: $('#hfParam').val(), idEmpresa: $('#hfEmpresa').val(), tipoGene: $('#hfTipGene').val(), mesAnio: $('#txtFecha').val() },
        success: function (aData) {
            $('#listado1').html(aData.Resultado);
            if (aData.nRegistros > 0) {
                $('#tabla6').dataTable();
            }
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function cargarListaDifusionResumenDesviaciones() {
    var parametro = $('#cbTipGene').multipleSelect('getSelects');
    if (parametro == "") parametro = "-1";
    $('#hfParam').val(parametro);

    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    if (empresa == "") empresa = "-1";
    $('#hfEmpresa').val(empresa);

    var tipogene = $('#cbTipGene').multipleSelect('getSelects');
    if (tipogene == "") tipogene = "-1";
    $('#hfTipGene').val(tipogene);

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaDifusionResumenDesviaciones',
        data: { tipParametro: $('#hfParam').val(), idEmpresa: $('#hfEmpresa').val(), tipoGene: $('#hfTipGene').val(), mesAnio: $('#txtFecha').val() },
        success: function (aData) {
            $('#listado2').html(aData.Resultado);
            if (aData.nRegistros > 0) {
                $('#tabla6_2').dataTable();
            }
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function disenioGrafico(result, idGrafico, tip) {
    var opcion;
    if (tip == 1) {
        opcion = {
            title: {
                text: result.Grafico.TitleText,
                style: {
                    fontSize: '14'
                }
            },
            xAxis: {
                categories: result.Grafico.XAxisCategories
            },
            yAxis: [{ //Primary Axes
                title: {
                    text: result.Grafico.YAxixTitle[0],
                    color: result.Grafico.Series[0].Color
                },
                labels: {
                    style: {
                        color: result.Grafico.Series[0].Color
                    }
                }

            },
                { ///Secondary Axis
                    title: {
                        text: result.Grafico.YAxixTitle[1],
                        color: result.Grafico.Series[1].Color
                    },
                    labels: {

                        style: {
                            color: result.Grafico.Series[1].Color
                        }
                    },
                    opposite: true
                }],
            tooltip: {
                headerFormat: '<b>{series.name}</b><br>',
                pointFormat: '{point.y:.3f}'
            },
            plotOptions: {
                area: {
                    fillOpacity: 0.5
                }
            },

            series: []
        };

        for (var i in result.Grafico.Series) {
            opcion.series.push({
                name: result.Grafico.Series[i].Name,
                data: result.Grafico.SeriesData[i],
                type: result.Grafico.Series[i].Type,
                //color: result.Grafico.Series[i].Color,
                yAxis: result.Grafico.Series[i].YAxis
            });
        }
    }
    $('#' + idGrafico).highcharts(opcion);
}
