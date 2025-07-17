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
        url: controlador + 'CargarResumenRelevante',
        data: {
            codigoVersion: codigoVersion
        },
        success: function (aData) {
            $('.filtro_fecha_desc').html(aData[0].FiltroFechaDesc);

            $('#listado').html(aData[0].Resultado);

            graficoPie(aData[1].Grafico, "grafico1_1");
            graficoPie(aData[2].Grafico, "grafico1_2");

        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function graficoPie(result, idGrafico) {
    var json = result.Series;

    var jsondata = [];

    for (var i = 0; i < json.length; i++) {
        var jsonLista = json[i];
        jsondata.push({
            name: jsonLista.Name,
            y: jsonLista.Acumulado,
            color: jsonLista.Color,
            sliced: true
        });
    }

    Highcharts.chart(idGrafico, {
        chart: {
            shadow: true,
            plotBackgroundColor: null,
            plotBorderWidth: null,
            type: 'pie',
            spacingBottom: 50
        },
        title: {
            text: result.TitleText
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
            pointFormat: '<b>{point.percentage:.1f}%</b></b>'
        },
        plotOptions: {
            pie: {
                allowPointSelect: true,
                cursor: 'pointer',
                dataLabels: {
                    enabled: true,
                    format: '{point.percentage:.1f} %'
                },
                showInLegend: true
            }
        },
        series: [{
            name: 'Total',
            colorByPoint: true,
            data: jsondata
        }]
    });
}