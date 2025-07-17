var controlador = siteRoot + 'IndicadoresSup/DashboardAnalitico/';
$(function () {

    $('#tab-container').easytabs();


    var numeral = $("#tab-container .tab.active").data("value");
    if (numeral !== null) {
        ConsultarConceptoNumeral(numeral);
    }

    $('#txtFecha').Zebra_DatePicker({
        format: 'm Y',
        direction: -1,
        onSelect: function () {
            setearFechas($(this).val());
            ConsultarDashboard(numeral);
        }
    });

    $('#tab-container').bind('easytabs:midTransition', function () {
        var numeral = $("#tab-container .tab.active").data("value");
        ConsultarConceptoNumeral(numeral);
        ConsultarDashboard(numeral);
    });

    ConsultarDashboard(numeral);
});

function ConsultarDashboard(numeral) {
    if (numeral == 1 || numeral == 2) {
        ConsultarNumeralDashboardAnalítico(numeral);
    }
}

function ConsultarDashboardAnalítico() {

    var numeral = $("#tab-container .tab.active").data("value");
    var cboName = "#cboConcepto" + numeral;
    var valConcepto = $(cboName).val();
    ConsultarDashboardAnalíticoXConcepYNum(valConcepto, numeral);
}

function GraficoDisponibleNoDespachada() {
    var mesesmoviles = $("#txtDiaMovil10").val();

    $.ajax({
        type: 'POST',
        url: controlador + 'GraficoDisponibleNoDespachada',
        data: {
            mesesmoviles: mesesmoviles
        },
        success: function (result) {
            GraficoColumnasAgrupadas(result.Grafico, "Grafico10");
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function ConsultarDashboardAnalíticoXConcepYNum(valConcepto, numeral) {

    var mesesmoviles = $("#txtMesMovil" + numeral).val();
    if (mesesmoviles <= 0) return;
    var grafico = $("#tab-container .tab.active").data("grafico");

    if (valConcepto !== null) {
        $.ajax({
            type: 'POST',
            url: controlador + 'ConsultarDashboardAnalíticoMensual',
            data: {
                numeral: numeral,
                mesesmoviles: mesesmoviles,
                concepto: valConcepto
            },
            success: function(result) {
                var grName = "Grafico" + numeral;
                if (parseInt(grafico) === 1) {
                    GraficoTacometro(result.Grafico, grName);
                }
                $("#Tabla" + numeral).html(result.Resultado);
            },
            error: function(err) {
                alert("Ha ocurrido un error");
            }
        });
    }
    else {
        alert("[Seleccionar concepto]");
    }
}

function __ConsultarDashboardAnalíticoDiario(numeral) {
    var diasmoviles = $("#txtDiaMovil" + numeral).val();
    if (numeral===8) {
        ConsultarDashboardAnalíticoDiario(numeral, diasmoviles, 4, 110, "84");
        ConsultarDashboardAnalíticoDiario(numeral, diasmoviles, 5, 110, "85");
    }

    if (numeral === 9) {
        ConsultarDashboardAnalíticoDiario(numeral, diasmoviles, 4, 111, "94");
        ConsultarDashboardAnalíticoDiario(numeral, diasmoviles, 5, 111, "95");
    }
}

function ConsultarDashboardAnalíticoDiario(numeral, diasmoviles, clasicodi, concepto, tabla) {
    $.ajax({
        type: 'POST',
        url: controlador + 'ConsultarDashboardAnalíticoDiario',
        data: {
            numeral: numeral,
            diasmoviles: diasmoviles,
            clasicodi: clasicodi,
            concepto: concepto
        },
        success: function (result) {
            $("#Tabla" + tabla).html(result.Resultado);
            GraficoLinea(result.Grafico, "Grafico" + tabla);
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function ConsultarNumeralDashboardAnalítico(numeral) {
    $.ajax({
        type: 'POST',
        url: controlador + 'ConsultarDashboardAnalítico',
        data: {
            numeral: numeral
        },
        success: function (result) {
            for (var i in result.Conceptos) {
                var sconcodi = result.Conceptos[i];
                var grName = "GraficoCon" + sconcodi;
                GraficoTacometro(result.Graficos[i], grName);
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function ConsultarConceptoNumeral(numeral) {

    $.ajax({
        type: 'POST',
        url: controlador + 'ConsultarConceptoNumeral',
        data: {
            numeral: numeral
        },
        success: function (result) {
            var cboName = "#cboConcepto" + numeral;
            $(cboName + " option").remove();
            $(cboName).append(new Option("[Seleccionar]", ""));
            $(cboName + ' option:first').prop('disabled', true);
            $.each(result, function (i, val) {
                $(cboName).append(new Option(val.Sconnomb, val.Sconcodi));
            });
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });

}

function GraficoTacometro(data, content) {
    //var data = dataResult.Grafico;
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
    for (var d in data.SerieData) {
        var item_ = data.SerieData[d];
        series.push({
            name: item_.Name,
            color: item_.Color,
            data: item_.Data,
            tooltip: {
                valueSuffix: ' %'
            },
            dial: {
                backgroundColor: item_.Color
            },
            showInLegend: true,
            dataLabels: {
                enabled: true,
                color: item_.Color,
                allowOverlap: false,
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
                distance: 10,
                format: '{value} %'
            },
            plotBands: dataPlot,
            crosshair: true
        }, legend: {
            align: 'center',
            verticalAlign: 'middle'
        },
        credits: {
            enabled: false
        },

        series: series

    });


};

function setearFechas(fecha) {
    $.ajax({
        type: 'POST',
        async: false,
        url: controlador + 'SetearFechaFilter',
        data: {
            fec1: fecha,
            fec2: fecha
        },
        dataType: 'json',
        success: function (e) {
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}


function GraficoColumnasAgrupadas(data, content) {

    var series = [];
    var categoria = [];

    for (var d in data.SerieData) {
        var item = data.SerieData[d];
        if (item == null) {
            continue;
        }
        series.push({
            name: item.Name,
            data: item.Data,
        });
    }


    for (var d in data.Categorias) {
        var item = data.Categorias[d];
        if (item == null) {
            continue;
        }
        categoria.push({
            name: item.Name,
            categories: item.Categories,
        });
    }




    Highcharts.chart(content, {
        chart: {
            type: 'column',
            shadow: true
        },
        tooltip: {
            pointFormat: '<span style="color:{series.color}">{series.name}</span>: <b>{point.y}</b> ({point.percentage:.0f}%)<br/>',
            shared: true
        },
        title: {
            text: data.TitleText
        },
        plotOptions: {
            column: {
                stacking: 'normal'
            }
        },
        yAxis: {
            labels: {
                format: data.YaxixLabelsFormat
            },
            title: {
                text: data.YAxixTitle[0]
            },
            stackLabels: {
                enabled: true,
                style: {
                    fontWeight: 'bold',
                    color: (Highcharts.theme && Highcharts.theme.textColor) || 'gray'
                },
                formatter: function () {
                    return Highcharts.numberFormat(this.total, 5) + data.TooltipValueSuffix;
                }
            },
            reversedStacks: false
        },
        tooltip: {
            valueDecimals: 5,
            shared: true,
            valueSuffix: data.TooltipValueSuffix
        },
        xAxis: {
            categories: categoria,
            labels: {
                autoRotation: [0]
            },
            crosshair: true
        },
        series: series
    });

}
