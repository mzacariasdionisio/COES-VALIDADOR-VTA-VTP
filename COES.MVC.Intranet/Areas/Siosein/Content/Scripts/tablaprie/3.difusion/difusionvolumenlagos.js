var controlador = siteRoot + 'Siosein/TablasPrieDeclaracionMen/'

$(function () {

    $('#txtFecha').Zebra_DatePicker({
        format: 'm Y'
    });

    cargarListaDifusionVolumenLagos();
    //cargarGraficoDifusionVolumenLagos();
});

function viewGrafico(id) {

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarViewGraficoDifusionVolumenLagos',
        data: { id: id, mesAnio: $('#txtFecha').val() },
        dataType: 'json',
        success: function (data) {
            if (data.Grafico.Series.length > 0) {
                disenioGrafico(data, 'idGrafico1', 1);
            } else $('#idGrafico1').html('Sin Grafico - No existen registros a mostrar!');
        },
        error: function () {
            alert('Ha ocurrido un error');
        }
    });
    mostrarGrafico();
}

function cargarListaDifusionVolumenLagos() {
    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    if (empresa == "") empresa = "-1";
    $('#hfEmpresa').val(empresa);

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaDifusionVolumenLagos',
        data: { idEmpresa: $('#hfEmpresa').val(), mesAnio: $('#txtFecha').val() },
        success: function (aData) {
            $('#listado1').html(aData.Resultado);
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function cargarGraficoDifusionVolumenLagos() {
    $.ajax({
        type: 'POST',
        url: controlador + 'CargarGraficoDifusionVolumenLagos',
        data: { idEmpresa: $('#hfEmpresa').val(), mesAnio: $('#txtFecha').val() },
        dataType: 'json',
        success: function (aData) {
            disenioGrafico('idGrafico1', 'EVOLUCION DIARIA DE LOS VOLUMENES INCIALES Y FINALES DE HUINCO', 1);
            //if (aData.Lista1.length > 0) {
            //    diseñoGrafico(aData.Lista1, 'idGrafico1', 'POTENCIA EFECTIVA POR EMPRESAS');
            //} else $('#idGrafico1').html('Sin Grafico - No existen registros a mostrar!');
        },
        error: function () {
            alert('Ha ocurrido un error');
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
            yAxis: { //Primary Axes
                title: {
                    text: result.Grafico.YaxixTitle
                }
            },
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