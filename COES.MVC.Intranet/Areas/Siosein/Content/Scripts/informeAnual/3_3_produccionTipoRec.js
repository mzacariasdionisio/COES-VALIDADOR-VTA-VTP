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
        url: controlador + 'CargarListaProduccionTipoRecurso',
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
            graficoComparacionProduccionEnergiaAcumuladaXTipoRecursoEnergetico(aData[1].Grafico);

        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function graficoComparacionProduccionEnergiaAcumuladaXTipoRecursoEnergetico(result) {
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
            inverted: true,
            spacingTop: 20,
            spacingBottom: 50,
            spacingLeft: 10,
            spacingRight: 70
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
                y: 0,
                x: 40,
            },
            labels: {
                formatter: function () {
                    return this.value;
                }
            }
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
