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

            if (result.Graficos[0].SerieData[0].Data.length > 0) {
                $("#grafico1").css('display','block')
                Grafico("grafico1", result.Graficos[0], '#EE7D32');
            } else
                $("#grafico1").css('display', 'none')
                
            if (result.Graficos[1].SerieData[0].Data.length > 0) {
                $("#grafico2").css('display', 'block')
                Grafico("grafico2", result.Graficos[1], '#5A9BD5');
            } else
                $("#grafico2").css('display', 'none')
                
            if (result.Graficos[2].SerieData[0].Data.length > 0) {
                $("#grafico3").css('display', 'block')
                Grafico("grafico3", result.Graficos[2], '#70AE44');
            } else
                $("#grafico3").css('display', 'none')
                

        },
        error: function (xhr, status) {
        },
        complete: function (xhr, status) {

        }
    });
}

function Grafico(container, data, color) {

    var series = [];
    for (var d in data.SerieData) {
        var item = data.SerieData[d];
        if (item === null) {
            continue;
        }
        series.push({ name: item.Name, data: item.Data, type:  item.Type, color: color});
    }

    Highcharts.setOptions({
        colors: ['#ED561B', '#058DC7', '#50B432']
    });


    Highcharts.chart(container, {
        chart: {
            //type: 'line',
            type: data.Type,
            shadow: true
        },
        title: {
            text: data.TitleText,
        },
        xAxis: {
            gridLineWidth: 0,
            categories: data.XAxisCategories,
            crosshair: {
                width: 12
            }
        },
        legend: {
            enabled: false
        },
        yAxis: {
            gridLineWidth: 1,
            title: {
                text: 'Cmg (S/./MWh)'
            },
            min: 0,
            labels: {
                format: "{value:.2f}"
            },
            tickInterval: 2.00
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
        subtitle: {
            text: data.Subtitle,            
            verticalAlign: 'bottom',
            align: 'left',
            
            x: 10,
            y: 10
        },
        tooltip: {
            valueDecimals: 2,
            shared: true
        },
        series: series
    });

}
