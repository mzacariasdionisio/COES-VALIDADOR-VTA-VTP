var controlador = siteRoot + 'Siosein/TablasPrieDeclaracionMen/';

$(function () {


    $('#txtFecha').Zebra_DatePicker({ format: 'm Y' });

    $('#cbBarrTrans').multipleSelect({
        width: '200px',
        filter: true,
        onClose: function () {
            cargarTensionesXBarra();
            cargarAreaOpeXBarra();
        }
    });

    cargarCamposBusqInicial();

    $("#btnConsultar").click(function () {
        CargarCostoMarginal();
    });
});

function CargarCostoMarginal() {
    cargarListaDifusionCostosMarginales();
}

function cargarCamposBusqInicial() {
    $('#cbBarrTrans').multipleSelect('checkAll');
}

function cargarListaDifusionCostosMarginales() {

    var mesAnio = $('#txtFecha').val();

    var barras = $('#cbBarrTrans').multipleSelect('getSelects');

    var dataJson = { mesAnio: mesAnio, barracodi: barras.join() };

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaDifusionCostosMarginales',
        async: false,
        dataType: 'json',
        contentType: 'application/json; charset=utf8',
        data: JSON.stringify(dataJson),
        success: function (aData) {
            $('#listado1').html(aData.Resultado);
            $('#listado2').html(aData.Resultado2);

            if (aData.NRegistros > 0) {
                $('#tabla3').dataTable({
                    filter: true,
                    info: true,
                    scrollY: "500px",
                    scrollCollapse: true,
                    paging: false
                });
            }
            GraficoLinea(aData.Graficos[0], "idGrafico1");
            GraficoCombinadoDual(aData.Graficos[1], "idGrafico2");
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function cargarGraficoDifusionDemanda() {
    var mesAnio1 = $('#txtFecha').val();
    var mesAnio2 = $('#txtFecha2').val();


    var barras = $('#cbBarrTrans').multipleSelect('getSelects');
    if (barras == "") barras = "0";
    $('#hfBarrTrans').val(barras);


    var tensiones = $('#cbTension').multipleSelect('getSelects');
    if (tensiones == "") tensiones = "0";


    $('#hfTensiones_').val(tensiones);

    var areas = $('#cbAreaOpe').multipleSelect('getSelects');
    if (areas == "") areas = "0";
    $('#hfAreaOpes_').val(areas);

    var rangos = $('#cbRangos').multipleSelect('getSelects');
    if (rangos == "") rangos = "0";
    $('#hfRangos').val(rangos);

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarGraficoDifusionDemanda',
        async: false,
        data: { mesAnio1: mesAnio1, mesAnio2: mesAnio2, barracodi: $('#hfBarrTrans').val(), idAreas: $('#hfAreaOpes_').val(), Tensiones: $('#hfTensiones_').val(), rangos: $('#hfRangos').val() },
        dataType: 'json',
        success: function (aData) {
            if (aData.Serie1 != null) {
                dibujarGrafico(aData, 'idGrafico2', 'COSTOS MARGINALES PROMEDIO HORARIO VS DEMANDA HORARIA PROMEDIO DEL SEIN');
            } else $('#idGrafico2').html('Sin Grafico - No existen registros a mostrar!');
        },
        error: function () {
            alert('Ha ocurrido un error');
        }
    });
}

function disegnGrafico(result, grafico, texto) {
    var var_textos = [], var_text = [], var_costos = [];
    var seriesX = [], var_seriesx = [];

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
        legend: {
            enabled: false
        },
        plotOptions: {
            line: {
                dataLabels: {
                    enabled: false
                },
                marker: {
                    enabled: false
                },

                enableMouseTracking: true
            }
        },series: []
    };

    for (i = 0 ; i < result.length; i++) {
        var_textos[i] = [];
        var_textos[i].push(result[i].string1);
    }
    
    opcion.xAxis.push({
        categories: var_textos
    });

  /*  for (i = 0 ; i < result.length; i++) {
        var_costos[i] = [];
        var_costos[i].push(result[i].monto);
    }*/

    for (var r = 0; r < result[0].Series.length; r++) {
        var_seriesx = [];

        for (var i = 0 ; i < result.length; i++) {

            var_seriesx.push(result[i].Series[r]);

        }

        var dato = {
            name: "Dia"+(r+1).toString(),
            data: var_seriesx
        }

        opcion.series.push(dato);
    }


    
  /*  opcion.series.push({
        name: "Lista",
        data: var_costos
    });
    */
    Highcharts.chart(grafico, opcion);
}

function dibujarGrafico(result, grafico, texto) {
    
    var categorias = [];
    var serie1 = [];
    var serie2 = [];

    for (var x = 0; x < result.Categoria.length; x++) {
        categorias[x] = result.Categoria[x];
    }
    for (var x = 0; x < result.Serie1.length; x++) {
        serie1[x] = result.Serie1[x];
    }
    for (var x = 0; x < result.Serie2.length; x++) {
        serie2[x] = result.Serie2[x];
    }

    Highcharts.chart(grafico, {
        chart: {
            zoomType: 'xy'
        },
        title: {
            text: texto
        },
        subtitle: {
            text: ''
        },
        xAxis: [{
            categories: categorias
            /*,
            crosshair: true*/
        }],
        yAxis: [{ // Primary yAxis
           
            title: {
                text: 'Demanda'
            },
            opposite: true,
            min:0
        }, { // Secondary yAxis
            gridLineWidth: 0,
            title: {
                text: 'CMg'
            },            
            minTickInterval: 5
        }],
        tooltip: {
            shared: true
        },
        legend: {
            layout: 'vertical',
            align: 'left',
            x: 80,
            verticalAlign: 'top',
            y: 55,
            floating: true,
            backgroundColor: (Highcharts.theme && Highcharts.theme.legendBackgroundColor) || '#FFFFFF'
        },
        series: [{
            name: 'CMg',            
            yAxis: 1,
            color: '#85A83D',
            data: serie1,
            tooltip: {
                valueSuffix: ' S/./kWh'
            }

        }, {
            name: 'Demanda',
            color: '#AD3330',
            data: serie2,
            tooltip: {
                valueSuffix: ' MW'
            }
        }]
    });

}