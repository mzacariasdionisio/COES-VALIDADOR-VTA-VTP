var controlador = siteRoot + 'Migraciones/AnexoA/';

$(function () {
    cargarValoresIniciales();
});

function cargarValoresIniciales() {
    cargarGrafico();
}

function cargarGrafico() {
    $.ajax({
        type: 'POST',
        url: controlador + 'CargarGraficoCostoTotalOperacionEjecutada',
        data: {
            fecha: $("#txtFecha").val()
        },
        dataType: 'json',
        success: function (aData) {
            if (aData.Grafico.Series.length > 0) {
                generarGrafico(aData, 'idVistaGrafica');
            }
            else {
                $('#idGraficoContainer').html("No existen registros !");
            }
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function generarGrafico(result, id) {
    //generar series
    var series = [];
    for (var i = 0; i < result.Grafico.Series.length; i++) {
        var serie = result.Grafico.Series[i];

        var dataSerie = [];
        for (var j = 0; j < result.Grafico.SerieDataS[i].length; j++) {
            var auxSerie = result.Grafico.SerieDataS[i][j];
            var aux = {
                y: auxSerie.Y,
                name: auxSerie.Name
            }
            dataSerie.push(aux);
        }

        var obj = {
            name: serie.Name,
            type: serie.Type,
            yAxis: serie.YAxis,
            data: dataSerie,
            color: serie.Color,
            dataLabels: {
                enabled: true,
                color: '#FFFFFF',
                align: 'right',
                format: '{point.name}',
                y: 5, // 10 pixels down from the top
                style: {
                    fontSize: '13px',
                    fontFamily: 'Verdana, sans-serif'
                }
            }
        };

        series.push(obj);
    }
    var dataCategoria = result.Grafico.XAxisCategories;
    var tituloGrafico = result.Grafico.TitleText;
    var tituloY = result.Grafico.YaxixTitle;
    var tituloX = result.Grafico.XAxisTitle;

    Highcharts.chart(id, {
        chart: {
            type: 'column'
        },
        title: {
            text: tituloGrafico
        },
        subtitle: {
            text: ''
        },
        xAxis: {
            categories: dataCategoria,
            crosshair: true,
            title: {
                text: tituloX,
                align: 'high'
            }
        },
        yAxis: {
            min: 0,
            title: {
                text: tituloY,
                align: 'high'
            },
            labels: {
                format: '{value}',
            }
        },
        tooltip: {
            headerFormat: '<table>',
            pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
                '<td style="padding:0"><b>{point.y:.1f} mm</b></td></tr>',
            footerFormat: '</table>',
            shared: true,
            useHTML: true
        },
        plotOptions: {
            column: {
                pointPadding: 0.2,
                borderWidth: 0
            }
        },
        series: series
    });
}
