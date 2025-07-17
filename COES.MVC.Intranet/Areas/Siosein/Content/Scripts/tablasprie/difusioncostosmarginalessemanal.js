var controlador = siteRoot + 'Siosein/TablasPrieDeclaracionMen/'

$(function () {

    $('#cbBarrTrans').multipleSelect({
        width: '150px',
        filter: true,
        onClose: function (view) {
            cargarTensionesXBarra();
            cargarAreaOpeXBarra();
        }
    });

    $('#cbTension').multipleSelect({
        width: '150px',
        filter: true,
        onClose: function (view) {
            activarLLamados();
        }
    });

    $('#cbAreaOpe').multipleSelect({
        width: '150px',
        filter: true,
        onClose: function (view) {
            activarLLamados();
        }
    });

    $('#cbRangos').multipleSelect({
        width: '150px',
        filter: true,
        onClose: function (view) {
            activarLLamados();
        }
    });

    $('#txtFecha').Zebra_DatePicker({ format: 'm Y' });
    $('#txtFecha2').Zebra_DatePicker({ format: 'm Y' });

    load_valoresIniciales();
    
    function activarLLamados() {
        cargarListaDifusionCostosMarginalesSemanal();
    }
    activarLLamados();
});

function load_valoresIniciales() {
    $('#cbBarrTrans').multipleSelect('checkAll');
    $('#cbRangos').multipleSelect('checkAll');

    cargarTensionesXBarra();
    cargarAreaOpeXBarra();
}

function cargarTensionesXBarra() {
    var barras = $('#cbBarrTrans').multipleSelect('getSelects');
    if (barras == "") barras = "0";
    $('#hfBarrTrans').val(barras);

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaTensionesXBarra',
        data: { barras: $('#hfBarrTrans').val() },
        success: function (aData) {
            $('#cbTensiones').html(aData.Resultado);
            $('#cbTension').multipleSelect({
                width: '150px',
                filter: true
            });
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function cargarAreaOpeXBarra() {
    var barras = $('#cbBarrTrans').multipleSelect('getSelects');
    if (barras == "") barras = "0";
    $('#hfBarrTrans').val(barras);

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaAreaOpeXBarra',
        data: { barras: $('#hfBarrTrans').val() },
        success: function (aData) {
            $('#cbAreaOpes').html(aData.Resultado);
            $('#cbAreaOpe').multipleSelect({
                width: '150px',
                filter: true
            });
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function cargarListaDifusionCostosMarginalesSemanal() {
    var mesAnio1 = $('#txtFecha').val();

    var barras = $('#cbBarrTrans').multipleSelect('getSelects');
    if (barras == "") barras = "0";
    $('#hfBarrTrans').val(barras);

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaDifusionCostosMarginalesSemanal',
        data: { periodo: mesAnio1, barracodi: $('#hfBarrTrans').val() },
        success: function (aData) {
            $('#listado1').html(aData.Resultado);
            cargarGraficoDifusionCostosMarginalesSemanal(mesAnio1, $('#hfBarrTrans').val());
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function cargarGraficoDifusionCostosMarginalesSemanal(fec1, barras) {

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarGraficoDifusionCostosMarginalesSemanal',
        data: { periodo: fec1, barracodi: barras },
        dataType: 'json',
        success: function (data) {
            if (data.Grafico.Series.length > 0) {
                disenioGrafico( data, 'idGrafico1', 'EVOLUCION DE LOS COSTOS MARGINALES PROMEDIO MENSUAL POR BARRAS SEIN', 1);
            } else $('#idGrafico1').html('Sin Grafico - No existen registros a mostrar!');
        },
        error: function () {
            alert('Ha ocurrido un error');
        }
    });
}


function disenioGrafico(data, grafico, texto, tip) {
    if (tip == 1) {

        var categorias = [];
        var series = [];
        var nombreBarra = "";
        for (var i = 0; i < data.Grafico.XAxisCategories.length; i++) {
            categorias.push(data.Grafico.XAxisCategories[i]);
        }
        for (var i = 0; i < data.Grafico.Series.length; i++) {
            var valores = [];
            for (var j = 0; j < data.Grafico.Series[i].Data.length; j++) {
                valores.push(data.Grafico.Series[i].Data[j].Y);
            }
            series.push({
                name: data.Grafico.Series[i].Name,
                data: valores
            });
            Barrnombre = data.Grafico.Series[i].Name;
        }
        Highcharts.chart(grafico, {
            chart: {
                type: 'line'
            },
            title: {
                text: texto + '<br>' + Barrnombre
            },
            yAxis: {
                title: {
                    text: 'S/.KWh'
                }
            },
            xAxis: {
                categories: categorias
            },
            plotOptions: {
                line: {
                    dataLabels: {
                        enabled: false
                    },
                    marker: {
                        enabled: false
                    }, enableMouseTracking: true
                }
            },

            series: series

        });
    }
    if (tip == 2) {
        Highcharts.chart(grafico, {

            title: {
                text: texto
            },

            xAxis: {
                tickInterval: 1
            },

            yAxis: {
                type: 'logarithmic',
                minorTickInterval: 0.1
            },

            tooltip: {
                headerFormat: '<b>{series.name}</b><br />',
                pointFormat: 'x = {point.x}, y = {point.y}'
            },

            series: [{
                data: [1, 2, 4, 8, 16, 32, 64, 128, 256, 512],
                pointStart: 1
            }]
        });
    }
    if (tip == 3) {
        Highcharts.chart(grafico, {

            title: {
                text: texto
            },

            xAxis: {
                tickInterval: 1
            },

            yAxis: {
                type: 'logarithmic',
                minorTickInterval: 0.1
            },

            tooltip: {
                headerFormat: '<b>{series.name}</b><br />',
                pointFormat: 'x = {point.x}, y = {point.y}'
            },

            series: [{
                data: [1, 2, 4, 8, 16, 32, 64, 128, 256, 512],
                pointStart: 1
            }]
        });
    }
}