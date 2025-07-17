var controlador = siteRoot + 'InformeEjecutivoMen/ReporteEjecutivo/'

$(function () {        
    CargarEvolucionCMGbarra();
});

function mostrarReporteByFiltros() {
    CargarEvolucionCMGbarra();
}

function CargarEvolucionCMGbarra() {
    var codigoVersion = getCodigoVersion();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarEvolucionCMGbarra',
        data: {
            codigoVersion: codigoVersion
        },
        success: function (aData) {
            $('#listado1').html(aData.Resultado);
            if (aData.Grafico.SerieData.length > 0) {
                disenioGrafico(aData.Grafico, 'grafico1');
            }
        },
        error: function (err) { alert("Ha ocurrido un error"); }
    });
}

function disenioGrafico(result, grafico) {
    //generar series
    var series = [];

    for (var i = 0; i < result.SerieData.length; i++) {
        var serie = result.SerieData[i];
        var obj = {
            name: serie.Name,
            type: serie.Type,
            data: serie.Data,
            color: serie.Color,
            marker: {
                enabled: serie.MarkerEnabled
            }
        };
        series.push(obj);
    }

    var dataHora = result.XAxisCategories;
    var tituloGrafico = result.TitleText;

    Highcharts.chart(grafico, {
        chart: {
            shadow: true
        },
        title: {
            text: tituloGrafico
        },
        xAxis: [{
            categories: dataHora,
            crosshair: true,
            labels: {
                rotation: -90
            }
        }],
        yAxis: {
            title: {
                text: 'USD/MWh'
            }
        },
        tooltip: {
            shared: true,
            valueDecimals: 2,
            valueSuffix: ' USD/MWh'
        },
        legend: {
            align: 'center',
            verticalAlign: 'bottom',
            layout: 'horizontal'
        },
        series: series
    });
}