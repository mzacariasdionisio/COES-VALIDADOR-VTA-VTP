$(function () {
    mostrarPromedioCaudales(1);
});

function mostrarReporteByFiltros() {
    mostrarPromedioCaudales(1);
}

function mostrarPromedioCaudales(item) {
    var codigoVersion = getCodigoVersion();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaPromMensualCaudales',
        data: {
            codigoVersion: codigoVersion,
            param: item,
        },
        success: function (aData) {
            $('.filtro_fecha_desc').html(aData.FiltroFechaDesc);
            $('#listado1').html(aData.Resultado);
            $('#listado2').html(aData.Resultado2);
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });

}

function Grafico(data, content) {
    var series = [];
    var categoria = [];

    if (data === undefined) return;

    for (var d in data.SerieData) {
        var item = data.SerieData[d];
        if (item === null) {
            continue;
        }

        series.push({
            name: item.Name,
            data: item.Data,
            type: item.Type,
            color: item.Color
        });
    }

    for (var c in data.Categorias) {
        var item1 = data.Categorias[c];
        if (item1 === null) {
            continue;
        }
        categoria.push({
            name: item1.Name,
            categories: item1.Categories
        });
    }

    Highcharts.chart(content, {
        chart: {
            type: data.Type,
            shadow: true
        },
        tooltip: {
            valueDecimals: 2,
            shared: true,
            valueSuffix: data.TooltipValueSuffix
        },
        title: {
            text: data.TitleText
        },
        xAxis: {
            title: {
                text: data.XAxisTitle
            },
            categories: categoria,
            labels: {
                autoRotation: [0]
            },
            crosshair: true,
            tickInterval: 2
        },
        yAxis: {
            labels: {
                format: '{value} ' + data.YaxixLabelsFormat
            },
            title: {
                text: data.YAxixTitle
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
        plotOptions: {
            series: {
                marker: {
                    enabled: false //desabilitar marker
                }
            }
        },
        series: series
    });
}
