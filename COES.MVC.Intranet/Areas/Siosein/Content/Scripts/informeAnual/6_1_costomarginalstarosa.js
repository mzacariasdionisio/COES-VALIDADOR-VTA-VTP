$(function () {
    ConsultaCostosMarginalesBarraStaRosa();
});

function mostrarReporteByFiltros() {
    ConsultaCostosMarginalesBarraStaRosa();
}

function ConsultaCostosMarginalesBarraStaRosa() {
    var codigoVersion = getCodigoVersion();

    $.ajax({
        url: controlador + "ConsultaAnualCostosMarginalesBarraStaRosa",
        type: 'POST',
        data: {
            codigoVersion: codigoVersion
        },
        success: function (result) {
            $('#listado').html(result.Resultado);

            if (result.Grafico != null) {
                $("#grafico1").css('display', 'block')
                //Grafico("grafico1", result.Grafico);
                //Grafico2(result.Grafico);
                Grafico2("grafico1",result.Grafico);
            }               

        },
        error: function (xhr, status) {
        },
        complete: function (xhr, status) {

        }
    });
}
function Grafico2(container, result) {
    var opcion;
    var json = result.Series;
    var jsondata = [];
    for (var i in json) {
        var jsonValor = [];
        var jsonLista = json[i].Data;
        for (var j in jsonLista) {
            jsonValor.push(jsonLista[j].Y);
        }
        jsondata.push({
            name: json[i].Name,
            data: jsonValor,
            color: json[i].Color,
        });
    }

    //opcion = {
    Highcharts.chart(container, {
        chart: {
            type: 'line',
            shadow: true,
            spacingTop: 50,
            spacingBottom: 50
        },
        title: {
            text: ''
        },
        subtitle: {
            text: result.Subtitle,
            align: 'left',
            //x: -10,
            verticalAlign: 'bottom',
            floating: true,
            x: 10,
            y: 15
        },
        xAxis: {
            categories: result.XAxisCategories,
            type: 'datetime'
        },
        yAxis: {
            title: {
                text: getHtmlSaltoLinea(result.YaxixTitle),
                align: 'high',
                textAlign: 'center',
                rotation: 0,
                offset: 0,
                y: -25,
                x: -10,
            },
            labels: {
                format: '{value:,.1f}'
            },
            lineWidth: 1
        },
        tooltip: {
            valueDecimals: 2,
            shared: true
        },
        //tooltip: {
        //    pointFormat: 'Producción <b>{point.y:,.1f}</b><br/> en el año {series.name}'
        //},
        plotOptions: {
            area: {
                stacking: 'normal',
                pointStart: 1,
                marker: {
                    enabled: false,
                    symbol: 'circle',
                    radius: 2,
                    states: {
                        hover: {
                            enabled: true
                        }
                    }
                }
            }
        },
        series: jsondata
    });
    //$('#grafico1').highcharts(opcion);
}

function Grafico(container, data) {

    var series = [];
    for (var d in data.SerieData) {
        var item = data.SerieData[d];
        if (item === null) {
            continue;
        }
        series.push({ name: item.Name, data: item.Data, type:  item.Type, color: item.Color});
    }

    Highcharts.setOptions({
        colors: ['#ED561B', '#058DC7', '#50B432']
    });


    Highcharts.chart(container, {
        chart: {
            type: 'column',
            //type: data.Type,
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
                text: 'Cmg (USD/MWh)'
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
