$(function () {

    mostrarPromedioCaudales(2);
});

function mostrarReporteByFiltros() {
    mostrarPromedioCaudales(2);
}


function mostrarPromedioCaudales(item) {
    var codigoVersion = getCodigoVersion();
    var ireporcodi = parseInt($("#hdReportecodi").val());

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaPromMensualEvolCaudales',
        data: {
            codigoVersion: codigoVersion,
            param: item,
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


//function mostrarPromedioCaudales(item) {
//    var codigoVersion = getCodigoVersion();

//    $.ajax({
//        type: 'POST',
//        url: controlador + 'CargarListaPromMensualCaudales',
//        data: {
//            codigoVersion: codigoVersion,
//            param: item
//        },
//        success: function (aData) {

//            GraficoNatural(aData.Graficos[0], "grafico2");
//            GraficoNatural(aData.Graficos[1], "grafico3");
//            GraficoNaturalDoble(aData.Graficos[2], "grafico4");
//        },
//        error: function (err) {
//            alert("Ha ocurrido un error");
//        }
//    });

//}

//function GraficoNaturalDoble(data, content) {
//    var series = [];
//    var categoria = [];

//    if (data === undefined) return;

//    for (var d in data.SerieData) {
//        var item = data.SerieData[d];
//        if (item === null) {
//            continue;
//        }

//        series.push({
//            name: item.Name,
//            data: item.Data,
//            type: item.Type,
//            color: item.Color,
//            yAxis: item.YAxis
//        });
//    }

//    for (var c in data.Categorias) {
//        var item1 = data.Categorias[c];
//        if (item1 === null) {
//            continue;
//        }
//        categoria.push({
//            name: item1.Name,
//            categories: item1.Categories
//        });
//    }

//    Highcharts.chart(content, {
//        chart: {
//            type: data.Type,
//            shadow: true
//        },
//        tooltip: {
//            valueDecimals: 2,
//            shared: true,
//            valueSuffix: data.TooltipValueSuffix
//        },
//        title: {
//            text: data.TitleText
//        },
//        xAxis: {
//            title: {
//                text: data.XAxisTitle
//            },
//            categories: categoria,
//            labels: {
//                autoRotation: [0]
//            },
//            crosshair: true,
//            tickInterval: 2
//        },
//        yAxis: [

//            {
//                labels: {
//                    format: '{value} ' + data.YaxixLabelsFormat
//                },
//                title: {
//                    //text: data.YAxixTitle
//                    x: 0,
//                    y: -20,
//                    align: 'high',
//                    rotation: 0,
//                    offset: -60,
//                    //text: '(Charcani y Aricota)'
//                    text: data.YAxixTitle[0]
//                },
//            },
//            {
//                labels: {
//                    format: '{value} ' + data.YaxixLabelsFormat
//                },
//                title: {
//                    x: -40,
//                    y: -20,
//                    align: 'high',
//                    rotation: 0,
//                    offset: -120,
//                    //text: '(Chili, San Gabán y Vilcanota)'
//                    text: data.YAxixTitle[1]
//                },
//                opposite: true
//            }
//        ],
//        subtitle: {
//            text: data.Subtitle,
//            align: 'left',
//            verticalAlign: 'bottom',
//            floating: false,
//            x: 10,
//            y: 9
//        },
//        plotOptions: {
//            series: {
//                marker: {
//                    enabled: false //desabilitar marker
//                }
//            }
//        },
//        series: series
//    });
//}


//function GraficoNatural(data, content) {
//    var series = [];
//    var categoria = [];

//    if (data === undefined) return;

//    for (var d in data.SerieData) {
//        var item = data.SerieData[d];
//        if (item === null) {
//            continue;
//        }

//        series.push({
//            name: item.Name,
//            data: item.Data,
//            type: item.Type,
//            color: item.Color,
//            yAxis: item.YAxis
//        });
//    }

//    for (var c in data.Categorias) {
//        var item1 = data.Categorias[c];
//        if (item1 === null) {
//            continue;
//        }
//        categoria.push({
//            name: item1.Name,
//            categories: item1.Categories
//        });
//    }

//    Highcharts.chart(content, {
//        chart: {
//            type: data.Type,
//            shadow: true
//        },
//        tooltip: {
//            valueDecimals: 2,
//            shared: true,
//            valueSuffix: data.TooltipValueSuffix
//        },
//        title: {
//            text: data.TitleText
//        },
//        xAxis: {
//            title: {
//                text: data.XAxisTitle
//            },
//            categories: categoria,
//            labels: {
//                autoRotation: [0]
//            },
//            crosshair: true,
//            tickInterval: 2
//        },
//        yAxis: {
//            labels: {
//                format: '{value} ' + data.YaxixLabelsFormat
//            },
//            title: {
//                text: data.YAxixTitle
//            }
//        },
//        subtitle: {
//            text: data.Subtitle,
//            align: 'left',
//            verticalAlign: 'bottom',
//            floating: false,
//            x: 10,
//            y: 9
//        },
//        plotOptions: {
//            series: {
//                marker: {
//                    enabled: false //desabilitar marker
//                }
//            }
//        },
//        series: series
//    });
//}
