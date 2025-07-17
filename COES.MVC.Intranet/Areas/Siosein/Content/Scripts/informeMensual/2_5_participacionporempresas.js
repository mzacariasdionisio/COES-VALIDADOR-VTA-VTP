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
        url: controlador + 'CargarListaProduccionEmpresas',
        data: {
            codigoVersion: codigoVersion
        },
        success: function (aData) {

            $('#listado').html(aData.Resultado);
            $('#reporte').dataTable({
                "sDom": 't',
                "ordering": false,
                paging: false
            });

            $('#grafico1').css("display", "block");
            graficoBarra(aData, "grafico1");
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function graficoBarra(result, idGrafico) {
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
            spacingBottom: 50
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
                formatter: function () {
                    return this.value;
                }
            },
            lineWidth: 1,
            opposite: false
        },
        tooltip: {
            pointFormat: '{series.name} <b>{point.y:,.00f} MW</b><br/>'
        },
        plotOptions: {
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