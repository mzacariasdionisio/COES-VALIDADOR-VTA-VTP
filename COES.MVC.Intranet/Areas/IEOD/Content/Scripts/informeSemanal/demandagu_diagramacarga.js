$(function () {
    getdemandagudiagramacarga();
});

function mostrarReporteByFiltros() {
    getdemandagudiagramacarga();
}

function getdemandagudiagramacarga() {
    var codigoVersion = getCodigoVersion();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarGuRangoPotencia',
        data: {
            codigoVersion: codigoVersion
        },
        success: function (aData) {
            $('.filtro_fecha_desc').html(aData[0].FiltroFechaDesc);

            graficodemandaguDiagramaCarga(aData[0], "grafico1");
            graficodemandaguDiagramaCarga(aData[1], "grafico2");
            graficodemandaguDiagramaCarga(aData[2], "grafico3");
            graficodemandaguDiagramaCarga(aData[3], "grafico4");
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function graficodemandaguDiagramaCarga(result, idGrafico) {
    //generar series
    var series = [];
    for (var i = 0; i < result.Grafico.Series.length; i++) {
        var serie = result.Grafico.Series[i];
        var obj = {
            name: serie.Name,
            type: serie.Type,
            yAxis: serie.YAxis,
            data: result.Grafico.SeriesData[i],
            color: serie.Color
        };

        series.push(obj);
    }
    var dataHora = result.Grafico.XAxisCategories;
    var tituloGrafico = result.Grafico.TitleText;

    //Generar grafica
    opcion = {
        chart: {
            zoomType: 'xy',
            spacingTop: 50,
            spacingBottom: 50,
            shadow: true
        },
        title: {
            text: tituloGrafico
        },
        subtitle: {
            text: result.Grafico.Subtitle,
            verticalAlign: 'bottom',
            //floating: true,
            x: 10,
            y: 15
        },
        xAxis: [{
            categories: dataHora,
            crosshair: true,
            tickInterval: 3
        }],
        yAxis: {
            title: {
                align: 'high',
                textAlign: 'center',
                rotation: 0,
                offset: 0,
                y: -25,
                x: -10,
                text: getHtmlSaltoLinea(result.Grafico.YaxixTitle)
            },
            labels: {
                format: '{value}',
            },
            lineWidth: 1,
            min: 0
        },
        tooltip: {
            shared: true
        },
        legend: {
            align: 'center',
            verticalAlign: 'bottom',
            layout: 'horizontal'
        },
        plotOptions: {
            spline: {
                lineWidth: 1,
                states: {
                    hover: {
                        lineWidth: 5
                    }
                },
                marker: {
                    enabled: false
                }
            }
        },
        series: series
    };

    $('#' + idGrafico).highcharts(opcion);
}