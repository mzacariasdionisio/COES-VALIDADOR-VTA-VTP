$(function () {    
    mostrarPromedioCaudales(2);
});

function mostrarReporteByFiltros() {
    mostrarPromedioCaudales();
}

function mostrarPromedioCaudales() {
    var codigoVersion = getCodigoVersion();
    var ireporcodi = parseInt($("#hdReporcodi").val());

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaEvolucionCaudales',
        data: {
            codigoVersion: codigoVersion,
            reporcodi: ireporcodi,
        },
        success: function (model) {
            $('.filtro_fecha_desc').html(model.FiltroFechaDesc);

            var htmlGrafico = "";
            for (var i = 0; i < model.Graficos.length; i++) {
                var codigo = model.Graficos[i].IdGrafico;
                var urlReporte = siteRoot + "ReportesMedicion/formatoreporte/IndexDetalle?id=" + codigo;

                htmlGrafico += `
                
                    <div class="action-alert" style="margin-bottom: 5px; margin-top: 10px; display: block; float: left">
                        La configuración de los puntos de medición para el gráfico se encuentra en <a target="_blank" href="${urlReporte}">Generador de Reporte</a>.
                    </div>
                    <div id="grafico_${codigo}" style="float: left; width: 100%; height: 500px;"></div>

                `;
            }

            $("#html_grafico").html(htmlGrafico);

            for (var i = 0; i < model.Graficos.length; i++) {
                var codigo = model.Graficos[i].IdGrafico;
                GraficoLineaH(model.Graficos[i], `grafico_${codigo}`);
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });

}

function GraficoLineaH(data, id) {

    var series = [];

    for (var d in data.SerieData) {
        var item = data.SerieData[d];
        if (item === null) {
            continue;
        }
        series.push({ name: item.Name, data: item.Data, type: item.Type, color: item.Color });
    }

    Highcharts.chart(id, {
        chart: {
            type: 'line',
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
            title: {
                text: data.XAxisTitle
            }
        },
        yAxis: {
            title: {
                text: data.YAxixTitle
            },
            labels: {
                format: '{value} ' + data.YaxixLabelsFormat
            },
            max: data.YaxixMax
        },
        plotOptions: {
            line: {
                dataLabels: {
                    enabled: false
                },
                marker: {
                    enabled: true
                },
                enableMouseTracking: true
            }
        },
        subtitle: {
            text: data.Subtitle,
            align: 'left',
            verticalAlign: 'bottom',
            floating: false,
            x: 10,
            y: 9
        },
        legend: {
            layout: data.LegendLayout,
            align: data.LegendAlign,
            verticalAlign: data.LegendVerticalAlign
        },
        series: series,
        responsive: {
            rules: [{
                condition: {
                    maxWidth: 500
                },
                chartOptions: {
                    legend: {
                        layout: 'horizontal',
                        align: 'center',
                        verticalAlign: 'bottom'
                    }
                }
            }]
        }
    });
}
