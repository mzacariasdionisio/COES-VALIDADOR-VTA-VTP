var controlador = siteRoot + 'InformeEjecutivoMen/reporteejecutivo/';


$(function () {
    ConsultaCostosMarginalesBarrasSein();
});

function mostrarReporteByFiltros() {
    ConsultaCostosMarginalesBarrasSein();
}

function ConsultaCostosMarginalesBarrasSein() {
    var codigoVersion = getCodigoVersion();

    $.ajax({
        url: controlador + "ConsultaCostosMarginalesBarrasSein",
        type: 'POST',
        data: {
            codigoVersion: codigoVersion
        },
        success: function (result) {
            $("#listado1").html(result.Resultados[0]);
            $("#listado2").html(result.Resultados[1]);
            $("#listado3").html(result.Resultados[2]);

            Grafico("grafico1",result.Graficos[0]);
            Grafico("grafico2",result.Graficos[1]);
            Grafico("grafico3",result.Graficos[2]);
        },
        error: function (xhr, status) {
        },
        complete: function (xhr, status) {

        }
    });
}

function Grafico(container, data) {

    var series = [];
    for (var d in data.SerieData) {
        var item = data.SerieData[d];
        if (item === null) {
            continue;
        }
        series.push({ name: item.Name, data: item.Data });
    }

    Highcharts.setOptions({
        colors: ['#ED561B', '#058DC7', '#50B432']
    });


    Highcharts.chart(container, {
        chart: {
            type: 'line',
            shadow: true
        },
        title: {
            text: data.TitleText
        },
        xAxis: {
            gridLineWidth: 1,
            categories: data.XAxisCategories,
            crosshair: {
                width: 12
            }
        },
        yAxis: {
            gridLineWidth: 0,
            title: {
                text: 'Cmg (USD/MWh)'
            },
            min: 0,
            labels: {
                format: "{value:.2f}"
            },
            tickInterval: 5.00
        },
        plotOptions: {
            series: {
                states: {
                    hover: {
                        enabled: false
                    }
                },
                marker: {
                    radius: 6,
                    states: {
                        hover: {
                            enabled: true
                        }
                    }
                },
                lineWidth: 0

            }
        },
        tooltip: {
            valueDecimals: 2,
            shared: true
        },
        series: series
    });

}
