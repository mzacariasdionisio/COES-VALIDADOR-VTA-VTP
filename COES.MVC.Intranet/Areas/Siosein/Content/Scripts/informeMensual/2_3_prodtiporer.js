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
        url: controlador + 'CargarListaProduccionRER',
        data: {
            codigoVersion: codigoVersion
        },
        success: function (aData) {

            $('#listado').html(aData[0].Resultado);
            $('#reporte').dataTable({
                "sDom": 't',
                "ordering": false,
                paging: false
            });

            $('#idGraficoContainer').html('');

            $('#grafico1').css("display", "block");
            graficoComparacionProduccionRERAcumulada(aData[1].Grafico);

            $('#grafico2').css("display", "block");
            GraficoPieParticipacionMatrizGeneracionSEIN(aData[2].Grafico, "grafico2");

            

        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function graficoComparacionProduccionRERAcumulada(result) {
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

    opcion = {
        chart: {
            type: 'column',
            shadow: true,
            spacingTop: 0,
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
        legend: {
            align: 'center',
            verticalAlign: 'top',
            layout: 'horizontal'
        },
        xAxis: {
            categories: result.XAxisCategories,
            type: 'datetime'
        },
        yAxis: {
            title: {
                text: result.YaxixTitle,
                align: 'high',
                textAlign: 'center',
                rotation: 0,
                offset: 0,
                y: -25,
                x: -10,
            },
            labels: {
                formatter: function () {
                    return this.value;
                }
            },
            lineWidth: 1
        },
        tooltip: {
            pointFormat: 'Producción <b>{point.y:,.0f}</b><br/> en el año {series.name}'
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
    $('#grafico1').highcharts(opcion);
}

function GraficoPieParticipacionMatrizGeneracionSEIN(result, idGrafico) {
    var titulo = getHtmlSaltoLinea(result.TitleText);
    var jsondata = [];
    var json = result.Series;

    for (var i = 0; i < json.length; i++) {
        var jsonLista = json[i];

        var serie = {};
        var arrayData = [];
        for (var j = 0; j < jsonLista.Data.length; j++) {
            var regDato = jsonLista.Data[j];
            arrayData.push({
                name: regDato.Name,
                y: regDato.Y,
                color: regDato.Color,
                sliced: true
            });
        }
        serie.center = [jsonLista.Center];
        serie.name = jsonLista.Name;
        serie.data = arrayData;
        serie.dataLabels = {
            formatter: function () {
                return this.point.name + '<br/>' + this.y + '%';
            }
        };
        if (i == 0)
            serie.dataLabels.distance = -30;
        serie.startAngle = i == 0 ? 95 : 90;

        jsondata.push(serie);
    }

    Highcharts.chart(idGrafico, {
        chart: {
            shadow: true,
            type: 'pie',
            spacingBottom: 50
        },
        title: {
            text: titulo
        },
        subtitle: {
            text: result.Subtitle,
            align: 'center',
            //x: -10,
            verticalAlign: 'bottom',
            floating: true,
            x: 10,
            y: 15
        },
        tooltip: {
        },
        plotOptions: {
            pie: {
                dataLabels: {
                    enabled: true
                }
            }
        },
        series: jsondata
    });
}
