$(function () {
    cargarListaConsumoCombustibleSemanal();
});

function mostrarReporteByFiltros() {
    cargarListaConsumoCombustibleSemanal();
}

function cargarListaConsumoCombustibleSemanal() {
    var codigoVersion = getCodigoVersion();

        $.ajax({
            type: 'POST',
            url: controlador + 'CargarListaConsumoCombustibleSemanal',
            data: {
                codigoVersion: codigoVersion
            },
            success: function (result) {
                $('.filtro_fecha_desc').html(result.FiltroFechaDesc);
                $('#listado').html(result.Resultado);
                
                if (result.Graficos[1]!=null) {
                    if (result.ListaGrafico != null) {// si existen registros
                        $('#grafico2').css("display", "block");
                        ConsumoCombustibleSem(result, "grafico2", 2);
                    }
                    else {// No existen registros
                        $('#grafico2').css("display", "none");
                    }
                } 
                if (result.Graficos[0] != null)
                {
                    if (result.Graficos[0].SeriesData[0]) {// si existen registros
                        $('#grafico1').css("display", "block");
                        ConsumoCombustibleSem(result, "grafico1", 1);
                    }
                    else {// No existen registros
                        $('#grafico1').css("display", "none");
                    }
                }
                if (result.Graficos[2] != null) {
                    if (result.Graficos[2].SerieData[0]) {// si existen registros
                        $('#grafico3').css("display", "block");                        
                        GraficoLineaH(result.Graficos[2], "grafico3");
                    }
                    else {// No existen registros
                        $('#grafico3').css("display", "none");
                    }
                }


            },
            error: function (err) {
                alert("Ha ocurrido un error");
            }
        });
    
}

function ConsumoCombustibleSem(result, idGrafico, tipografico) {
    var opcion;
    var ordenG = tipografico - 1;

    if (tipografico == 2) {
        var json = result.ListaGrafico;
        var jsondata = [];
        var indice = 3;
        for (var i in json) {
            var jsonValor = [];
            var jsonLista = json[i].ListaValores;
            for (var j in jsonLista) {
                jsonValor.push(jsonLista[j]);
            }
            jsondata.push({
                name: json[i].SerieName,
                data: jsonValor,
                color: json[i].SerieColor,
                index: indice
            });
            indice--;
        }

        opcion = {
            chart: {
                type: 'column'
            },
            title: {
                text: result.Graficos[ordenG].TitleText,
                style: {
                    fontSize: '14'
                }
            },
            xAxis: {
                categories: result.Graficos[ordenG].XAxisCategories,
                
                type: 'datetime'
            },
            yAxis: {
                title: {
                    text: '(m3)'
                },
                labels: {
                    formatter: function () {
                        return this.value;
                    }
                }
            },
            tooltip: {
                pointFormat: '{series.name} Potencia <b>{point.y:,.0f}</b><br/> en la hora {point.x}'
            },
            subtitle: {
                text: result.Graficos[ordenG].Subtitle,
                align: 'left',
                verticalAlign: 'bottom',
                floating: false,
                x: 10,
                y: 9
            },
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
        };
    } else {
        opcion = {
            title: {
                text: result.Graficos[ordenG].TitleText,
                style: {
                    fontSize: '14'
                }
            },
            xAxis: {
                categories: result.Graficos[ordenG].XAxisCategories,
                
                type: 'datetime'
            },
            yAxis: [{ //Primary Axes
                title: {
                    x: 0,
                    y: -20,
                    align: 'high', //ARRIBA
                    rotation: 0, //HORIZONTAL
                    offset: -1, //MUEVE GRAFICO
                    text: result.Graficos[ordenG].Series[0].YAxisTitle,
                    color: result.Graficos[ordenG].Series[0].Color
                },
                labels: {
                    style: {
                        color: result.Graficos[ordenG].Series[0].Color
                    }
                }

            },
            { ///Secondary Axis
                title: {
                    x: 0,
                    y: -20,
                    align: 'high', //ARRIBA
                    rotation: 0, //HORIZONTAL
                    offset: -1, //MUEVE GRAFICO
                    text: result.Graficos[ordenG].Series[1].YAxisTitle,
                    color: result.Graficos[ordenG].Series[1].Color
                },
                labels: {

                    style: {
                        color: result.Graficos[ordenG].Series[1].Color
                    }
                },
                opposite: true
            }],
            tooltip: {
                headerFormat: '<b>{series.name}</b><br>',
                pointFormat: '{point.y:.3f}'
            },
            subtitle: {
                text: result.Graficos[ordenG].Subtitle,
                align: 'left',
                verticalAlign: 'bottom',
                floating: false,
                x: 10,
                y: 9
            },
            plotOptions: {
                spline: {
                    marker: {
                        enabled: true
                    }
                }
            },

            series: []
        };

        for (var i in result.Graficos[ordenG].Series) {
            opcion.series.push({
                name: result.Graficos[ordenG].Series[i].Name,
                data: result.Graficos[ordenG].SeriesData[i],
                type: result.Graficos[ordenG].Series[i].Type,
                color: result.Graficos[ordenG].Series[i].Color,
                yAxis: result.Graficos[ordenG].Series[i].YAxis
            });
        }
    }
    $('#' + idGrafico).highcharts(opcion);
}

function GraficoLineaH(data, content) {

    var series = [];

    for (var d in data.SerieData) {
        var item = data.SerieData[d];
        if (item === null) {
            continue;
        }
        series.push({ name: item.Name, data: item.Data, type: item.Type, color: item.Color });
    }

    Highcharts.chart(content, {
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
