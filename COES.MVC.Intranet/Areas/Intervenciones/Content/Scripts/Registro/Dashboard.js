var controlador = siteRoot + 'Intervenciones/Registro/';

$(document).ready(function () {

    $('#txtFecha').Zebra_DatePicker({
        format: 'm Y',
        onSelect: function () {
            generarDashboard();
        }
    });

    $('#btnConsultar').on('click',
        function () {
            generarDashboard();
        }
    );

    $('#btnExportar').on('click',
        function () {
            exportarDashboard();
        }
    );

    generarDashboard();
});

function generarDashboard() {
    $.ajax({
        url: controlador + "ConstruirDashboardSPR",
        data: { fecha: $('#txtFecha').val() },
        type: 'POST',
        success: function (result) {

            if (result.Graficos.length > 0) {
                graficoTacometro(result.Graficos[0], 'tacometroF1');
                graficoTacometro(result.Graficos[1], 'tacometroF2');
                graficoReporteMensual(result.Graficos[2], "reporteMensualF1", 1);
                graficoReporteMensual(result.Graficos[3], "reporteMensualF2", 2);
            }
            else {
                alert("Error, no se generó versión para la fecha seleccionada");
            }
        },
        error: function (xhr, status) {
        },
        complete: function (xhr, status) {
        }
    });
};

function graficoReporteMensual(dataResult, content, indicador) {
    var data = dataResult;

    var series = [];

    for (var d in data.SerieData) {
        var item = data.SerieData[d];
        if (item === null) {
            continue;
        }
        series.push({ name: item.Name, data: item.Data });
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
            valueSuffix: ' ' + data.YaxixLabelsFormat
        },
        xAxis: {
            categories: data.XAxisCategories
        },
        yAxis: {
            min: 0,
            max: 100,
            plotLines: [{
                value: indicador == 1 ? 10 : 20,
                color: '#00B050',
                dashStyle: 'shortDash',
                width: 2
            },
            {
                value: indicador == 1 ? 20 : 35,
                color: '#FFFF00',
                dashStyle: 'shortDash',
                width: 2
            },
            {
                value: 100,
                color: '#FF0000',
                dashStyle: 'shortDash',
                width: 2
            }],
            title: {
                text: data.YaxixTitle
            },
            labels: {
                format: '{value} ' + data.YaxixLabelsFormat
            }
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
        legend: {
            layout: 'vertical',
            align: 'right',
            verticalAlign: 'middle'
        },
        credits: {
            enabled: false
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

function graficoTacometro(dataResult, content) {
    //var data = dataResult.Grafico;
    var data = dataResult;

    //console.log("Gráfico");
    //console.log(data);

    if (data.PlotBands.length < 1) {
        return;
    }
    var dataPlot = [];
    for (var i in data.PlotBands) {
        var item = data.PlotBands[i];
        if (item === null) {
            continue;
        }
        dataPlot.push({ from: item.From, to: item.To, color: item.Color, thickness: item.Thickness });
    }

    var series = [];
    var xposicion = -40;
    for (var d in data.SerieData) {
        xposicion = xposicion + 25;
        item = data.SerieData[d];
        //var align = (d == "0") ? "right" : (d == "1") ? "center" : "left";
        series.push({
            name: item.Name,
            color: item.Color,
            data: item.Data,
            tooltip: {
                valueSuffix: ' %'
            },
            dial: {
                backgroundColor: item.Color
            },
            showInLegend: true,
            dataLabels: {
                x: xposicion,
                verticalAlign: 'center',
                enabled: true,
                color: item.Color,
                allowOverlap: false,
                allowOverlap: true
            }
        });
    }

    Highcharts.chart(content, {

        chart: {
            type: 'gauge', shadow: true
        },

        title: {
            text: data.TitleText
        },
        pane: {
            startAngle: -90,
            endAngle: 90,
            background: null
        },
        yAxis: {
            min: data.YaxixMin,
            max: data.YaxixMax,
            lineColor: 'transparent',
            minorTickWidth: 0,
            tickLength: 0,
            tickPositions: data.YaxixTickPositions,
            labels: {
                step: 1,
                distance: 10
            },
            plotBands: dataPlot
        }, legend: {
            align: 'center',
            verticalAlign: 'top'
        },
        credits: {
            enabled: false
        },

        series: series

    });


};

function exportarDashboard() {
    $.ajax({
        type: 'POST',
        url: controlador + "GenerarArchivoExcelDashboardSPR",
        data: {
            fechaInicio: $("#txtFecha").val()
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                window.location = controlador + "AbrirArchivo?file=" + evt.Resultado;
            } else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}