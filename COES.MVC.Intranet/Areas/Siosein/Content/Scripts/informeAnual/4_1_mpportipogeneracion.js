$(function () {
    cargarMaximaDemandaTipoGeneracionAnual();
});

function mostrarReporteByFiltros() {
    cargarMaximaDemandaTipoGeneracionAnual();
}

function cargarMaximaDemandaTipoGeneracionAnual() {
    var codigoVersion = getCodigoVersion();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarMaximaDemandaTipoGeneracionAnual',
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

            $('#grafico1').css("display", "block");
            graficoBarraComparacionMDxTgeneracion(aData[1], 'grafico1');


        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function graficoBarraComparacionMDxTgeneracion(result, idGrafico) {
    var opcion;

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

    opcion = {
        chart: {
            type: 'column',
            shadow: true,
            inverted: true,
            spacingBottom: 60
        },
        title: {
            text: result.Grafico.TitleText
        },
        subtitle: {
            text: result.Grafico.Subtitle,
            align: 'left',
            //x: -10,
            verticalAlign: 'bottom',
            floating: true,
            x: 10,
            y: 15
        },
        legend: {
            verticalAlign: 'top',
        },
        xAxis: {
            title: {
                text: result.Grafico.XAxisTitle,
                textAlign: 'center',
                align: 'high',
                rotation: 0
            },
            categories: result.Grafico.XAxisCategories,
            crosshair: true,
            scrollbar: {
                enabled: true
            },
        },
        yAxis: {
            title: {
                text: result.Grafico.YaxixTitle //MW
            },
            labels: {
            },
            opposite: false,
            reversedStacks: false,
            lineWidth: 1
        },
        tooltip: {
            pointFormat: '{series.name} <b>{point.y:,.1f} ' + result.Grafico.YaxixTitle + '</b><br/>',
            shared: true
        },
        plotOptions: {
            column: {
                stacking: 'normal',
                dataLabels: {
                    enabled: false
                }
            },
            series: {
                label: {
                    connectorAllowed: false
                }
            }
        },
        series: series
    };

    $('#' + idGrafico).highcharts(opcion);
}
