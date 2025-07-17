$(function () {
    mostrarListado();
});

function mostrarReporteByFiltros() {
    mostrarListado();
}

function mostrarListado() {
    var codigoVersion = getCodigoVersion();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaPotenciaInstaladaSEIN',
        data: {
            codigoVersion: codigoVersion
        },
        success: function (aData) {            
            $('#listado').html(aData.Resultado);

            if (aData.Resultados != null && aData.Resultados.length > 0) {
                var html = "Equipos integrantes del COES que no tienen potencia instalada:<br/>";
                html += '<ul>';
                for (var i = 0; i < aData.Resultados.length; i++) {
                    html += `<li>${aData.Resultados[i]}</li>`;
                }
                html += '</ul>';
                $("#html_mensaje_eq").html(html);
            }

            if (aData.Grafico.SerieData.length > 0) {
                $('#grafico1').css("display", "block");
                GraficoColumnasBar(aData.Grafico, "grafico1");
            } else {
                $('#grafico1').css("display", "none");
            }

        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function GraficoColumnasBar(data, content) {

    var series = [];

    if (data === undefined) {
        return;
    }

    for (var d in data.SerieData) {
        var item = data.SerieData[d];
        if (item === null) {
            continue;
        }

        series.push({
            name: item.Name,
            data: item.Data,
            color:item.Color
        });
    }

    Highcharts.chart(content, {
        chart: {
            type: data.Type,
            shadow: true
        },
        title: {
            text: data.TitleText
        },
        tooltip: {
            valueDecimals: 2,
            shared: true,
            valueSuffix: ' ' + data.YaxixLabelsFormat
        },
        xAxis: {
            categories: data.XAxisCategories,
            crosshair: true,
            labels: {
                rotation: data.XAxisLabelsRotation
            }
        },
        yAxis: {
            labels: {
                format: '{value} ' + data.YaxixLabelsFormat
            },
            title: {
                text: data.YaxixTitle,
                //align: 'high',
                textAlign: 'center',
                //rotation: 0,
                //offset: 0,
                //y: -5,
                //x: -25,
            }
        },
        subtitle: {
            text: data.Subtitle,
            align: 'left',
            verticalAlign: 'bottom',
            floating: true,
            x: 10,
            y: 9
        },
        plotOptions: {
            column: {
                pointPadding: 0.2,
                borderWidth: 0,
                stacking: 'normal'
            }
            
        },
        legend: {
            layout: 'vertical',
            align: 'right',
            verticalAlign: 'middle'
        },
        series: series
    });

}