var controlador = siteRoot + 'Siosein/TablasPrieDeclaracionMen/'

$(function () {
    $('#cbEmpresa').multipleSelect({
        width: '150px',
        filter: true,
        onClose: function (view) {
            //cargarCentralxEmpresa();
            CargarCostoMarginal();
        }
    });

    CargarCostoMarginal();

    $('#cbEmpresa').multipleSelect('checkAll');
});

function CargarCostoMarginal() {
	cargarListaDifusionHechosRelevantes();
	cargarGraficoDifusionHechosRelevantes();
	cargarGraficoDifusionHechosRelevantesEmpresa();
	cargarGraficoDifusionHechosRelevantesCentral();
}

function cargarListaDifusionHechosRelevantes() {
    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    if (empresa == "") empresa = "-1";
    $('#hfEmpresa').val(empresa);

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaDifusionHechosRelevantes',
        data: { idEmpresa: $('#hfEmpresa').val(), mesAnio: $('#txtFecha').val() },
        success: function (aData) {
            $('#listado1').html(aData.Resultado);
			if($('#tabla24 th').length > 0){
				$('#tabla24').dataTable();
			}
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function cargarGraficoDifusionHechosRelevantes() {
    $.ajax({
        type: 'POST',
        url: controlador + 'CargarGraficoDifusionHechosRelevantes',
        data: { idEmpresa: $('#hfEmpresa').val(), mesAnio: $('#txtFecha').val() },
        dataType: 'json',
        success: function (aData) {
            if (aData.nRegistros > 0) {
                disenioGrafico(aData, 'idGrafico1', 2);
            } else $('#idGrafico1').html('Sin Grafico - No existen registros a mostrar!');
        },
        error: function () {
            alert('Ha ocurrido un error');
        }
    });
}

function cargarGraficoDifusionHechosRelevantesEmpresa() {
    $.ajax({
        type: 'POST',
        url: controlador + 'CargarGraficoDifusionHechosRelevantesEmpresa',
        data: { idEmpresa: $('#hfEmpresa').val(), mesAnio: $('#txtFecha').val() },
        dataType: 'json',
        success: function (aData) {
            if (aData.nRegistros > 0) {
                disenioGrafico(aData, 'idGrafico2', 1);
            } else $('#idGrafico2').html('Sin Grafico - No existen registros a mostrar!');
        },
        error: function () {
            alert('Ha ocurrido un error');
        }
    });
}

function cargarGraficoDifusionHechosRelevantesCentral() {
    $.ajax({
        type: 'POST',
        url: controlador + 'CargarGraficoDifusionHechosRelevantesCentral',
        data: { idEmpresa: $('#hfEmpresa').val(), mesAnio: $('#txtFecha').val() },
        dataType: 'json',
        success: function (aData) {
            if (aData.nRegistros > 0) {
                disenioGrafico(aData, 'idGrafico3', 1);
            } else $('#idGrafico3').html('Sin Grafico - No existen registros a mostrar!');
        },
        error: function () {
            alert('Ha ocurrido un error');
        }
    });
}

function disenioGrafico(result, grafico, tip) {
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
                    text: result.Grafico.YaxixTitle
                },
                labels: {
                    style: {
                        color: result.Grafico.Series[0].Color
                    }
                }

            }],
            tooltip: {
                headerFormat: '<b>{series.name}</b><br>',
                pointFormat: '{point.y:.3f}'
            },
            plotOptions: {
                column: {
                    stacking: 'normal'
                }
            },

            series: []
        };

        for (var i in result.Grafico.Series) {
            opcion.series.push({
                name: result.Grafico.Series[i].Name,
                data: result.Grafico.SeriesData[i],
                type: result.Grafico.Series[i].Type,
                yAxis: result.Grafico.Series[i].YAxis
            });
        }
    }
    if (tip == 2) {
        var dataGrafico = [];
        for (var i = 0; i < result.Grafico.Series.length; i++) {
            var g = result.Grafico.Series[i];
            dataGrafico.push({
                name: g.Name,
                y: g.Acumulado
            });
        }

        opcion = {
            chart: {
                type: 'pie',
                options3d: {
                    enabled: true,
                    alpha: 45,
                    beta: 0
                }
            },
            title: {
                text: result.Grafico.TitleText
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
                data: dataGrafico
            }]
        }
    }
    $('#' + grafico).highcharts(opcion);
}