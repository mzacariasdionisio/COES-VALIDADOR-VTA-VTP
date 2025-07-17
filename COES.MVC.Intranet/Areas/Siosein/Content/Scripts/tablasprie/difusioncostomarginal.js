var controlador = siteRoot + 'Siosein/TablasPrieDeclaracionMen/'

$(function () {

    $('#cbBarrTrans').multipleSelect({
        width: '150px',
        filter: true,
        onClose: function (view) {
            cargarTensionesXBarra();
            cargarAreaOpeXBarra();
            llamarMetodos();
        }
    });

    $('#cbTension').multipleSelect({
        width: '150px',
        filter: true,
        onClose: function (view) {
            //cargarCentralxEmpresa();
            llamarMetodos();
        }
    });

    $('#cbAreaOpe').multipleSelect({
        width: '150px',
        filter: true,
        onClose: function (view) {
            //cargarCentralxEmpresa();
            llamarMetodos();
        }
    });

    $('#cbRangos').multipleSelect({
        width: '150px',
        filter: true,
        onClose: function (view) {
            //cargarCentralxEmpresa();
            llamarMetodos();
        }
    });

    $('#txtFecha1').Zebra_DatePicker({ format: 'm Y' });
    $('#txtFecha2').Zebra_DatePicker({ format: 'm Y' });

    load_valoresIniciales();
    
    function llamarMetodos() {
        cargarListaDifusionCostoMarginal();
        cargarListaDifusionCostoMarginalPromedioDemanda();
    }
    llamarMetodos();
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
            $('#cbTensiones').html(aData);
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
            $('#cbAreaOpes').html(aData);
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

function cargarListaDifusionCostoMarginal() {
    var mesAnio1 = $('#txtFecha1').val();
    var mesAnio2 = $('#txtFecha2').val();

    var barras = $('#cbBarrTrans').multipleSelect('getSelects');
    if (barras == "") barras = "0";
    $('#hfBarrTrans').val(barras);

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaDifusionCostoMarginal',
        data: { mesAnio1: mesAnio1, mesAnio2: mesAnio2, barracodi: $('#hfBarrTrans').val() },
        success: function (aData) {
            $('#listado1').html(aData.Resultado);
            cargarGraficoDifusionCostoMarginal(mesAnio1, mesAnio2, $('#hfBarrTrans').val());
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function cargarListaDifusionCostoMarginalPromedioDemanda() {
    var mesAnio1 = $('#txtFecha1').val();
    var mesAnio2 = $('#txtFecha1').val();

    var barras = $('#cbBarrTrans').multipleSelect('getSelects');
    if (barras == "") barras = "0";
    $('#hfBarrTrans').val(barras);

    var tensiones = $('#cbTension').multipleSelect('getSelects');
    if (tensiones == "") tensiones = "0";
    $('#hfTension').val(tensiones);

    var areasope = $('#cbAreaOpe').multipleSelect('getSelects');
    if (areasope == "") areasope = "0";
    $('#hfAreaOpe').val(areasope);

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaDifusionCostoMarginalPromedioDemanda',
        data: { mesAnio1: mesAnio1, mesAnio2: mesAnio2, barracodi: $('#hfBarrTrans').val(), idTension: $('#hfTension').val(), idAreaOpe: $('#hfAreaOpe').val() },
        success: function (aData) {
            $('#listado2').html(aData.Resultado);
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function cargarGraficoDifusionCostoMarginal(fec1, fec2, barras) {
    var mesAnio = $('#txtFecha').val();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarGraficoDifusionCostoMarginal',
        data: { mesAnio1: fec1, mesAnio2: fec2, barracodi: barras },
        dataType: 'json',
        success: function (aData) {
            if (aData.Lista1.length > 0) {
                disegnGrafico(aData.Lista1, 'idGrafico1', 'EVOLUCION DE LOS COSTOS MARGINALES PROMEDIO MENSUAL POR BARRAS SEIN');
            } else $('#idGrafico1').html('Sin Grafico - No existen registros a mostrar!');
        },
        error: function () {
            alert('Ha ocurrido un error');
        }
    });
}

function disegnGrafico(result, grafico, texto) {
    var var_textos = [], var_text = [], var_costos = [];

    var opcion = null;

    opcion = {
        chart: {
            type: 'line'
        },
        title: {
            text: texto
        },
        xAxis: [],
        yAxis: {
            title: {
                text: 'S/./kWh'
            }
        },
        plotOptions: {
            line: {
                dataLabels: {
                    enabled: false
                },
                enableMouseTracking: true
            }
        },series: []
    };

    for (i = 0 ; i < result.length; i++) {
        var_textos[i] = [];
        var_textos[i].push(result[i].text);
    }
    
    opcion.xAxis.push({
        categories: var_textos
    });

    for (i = 0 ; i < result.length; i++) {
        var_costos[i] = [];
        var_costos[i].push(result[i].monto);
    }
    
    opcion.series.push({
        name: "Lista",
        data: var_costos
    });

    Highcharts.chart(grafico, opcion);
}