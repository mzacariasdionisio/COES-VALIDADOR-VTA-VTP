var controlador = siteRoot + 'Siosein/TablasPrieDeclaracionMen/'

$(function () {
    $('#cbEmpresa').multipleSelect({
        width: '150px',
        filter: true,
        onClose: function (view) {
            //cargarCentralxEmpresa();
            cargarListaDifusionEmbalsesEstacionalesProgMensual();
        }
    });

    cargarListaDifusionEmbalsesEstacionalesProgMensual();

    $('#cbEmpresa').multipleSelect('checkAll');
});

function cargarListaDifusionEmbalsesEstacionalesProgMensual() {
    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    if (empresa == "") empresa = "-1";
    $('#hfEmpresa').val(empresa);

    var mesAnio = $('#txtFecha').val();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaDifusionEmbalsesEstacionalesProgMensual',
        data: { mesAnio: mesAnio, idEmpresa: $('#hfEmpresa').val() },
        success: function (aData) {
            $('#listado1').html(aData.Resultado);
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function viewGrafico(id) {
    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    if (empresa == "") empresa = "-1";
    $('#hfEmpresa').val(empresa);
    var mesAnio = $('#txtFecha').val();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarViewGraficoDifusionEmbalsesEstacionalesProgMensual',
        data: { mesAnio: mesAnio, id: id },
        dataType: 'json',
        success: function (aData) {
            if (aData.Serie1 != null) {
                disenioGrafico('idGrafico1', 'EVOLUCION MENSUAL DE VOLUMENES INICIAL Y FINAL - ', 1, aData);
            }
            
            //if (aData.Lista1.length > 0) {
            //    disegnGrafico(aData.Lista1, 'idGrafico1', 'COSTO MARGINAL PROMEDIO DIARIO DE BARRA', mesAnio);
            //} else $('#idGrafico1').html('Sin Grafico - No existen registros a mostrar!');
        },
        error: function () {
            alert('Ha ocurrido un error');
        }
    });
    mostrarGrafico();
}

function disenioGrafico(grafico, texto, tip, data) {
    if (tip == 1) {

        var serieName = [];
        var serie1 = [];
        var serie2 = [];

        for (j = 0; j < data.Categoria.length; j++) {
            serieName[j] = data.Categoria[j];
        }
        for (j = 0; j < data.Serie1.length; j++) {
            serie1[j] = data.Serie1[j];
        }
        for (j = 0; j < data.Serie2.length; j++) {
            serie2[j] = data.Serie2[j];
        }



        chart = Highcharts.chart(grafico, {
            chart: {
                //type: 'line'
            },
            title: {
                text: data.nomGrafico
            },

            xAxis: {
                categories: serieName
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

            series: [{
                name: 'Volumen Final',
                type: 'column',
                data: serie2
            }, {
                name: 'Volumen Inicial',
                type: 'line',
                data: serie1
            }]

        });
    }
    if (tip == 2) {
        chart = Highcharts.chart(grafico, {
            chart: {
                //type: 'line'
            },
            title: {
                text: texto
            },

            xAxis: {
                categories: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec']
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

            series: [{
                name: 'Volumen Final',
                type: 'line',
                data: [502, 635, 809, -947, 1402, 3634, 5268, -5]
            }, {
                name: 'Volumen Inicial',
                type: 'line',
                data: [2, 112, 2, -6, 113, -130, 46]
            }]

        });
    }
}


function mostrarGrafico() {
    setTimeout(function () {
        $('#idGraficoPopup').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
            modalClose: false
        });
    }, 50);
}