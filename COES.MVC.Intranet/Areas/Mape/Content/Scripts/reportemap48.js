var controlador = siteRoot + 'mape/mape/';
var TipoReporte = {
    MENSUAL: 1,
    SEMANALDIARIO: 2
};
$(function () {
    $(".rmensual").show();
    $(".rsemanal").hide();

    $('#txtAñoFin').Zebra_DatePicker({
        format: 'Y'
    });

    $('#txtAñoInicio').Zebra_DatePicker({
        format: 'Y',
        pair: $('#txtAñoFin'),
        onSelect: function (date) {
     
            var date1 = $('#txtAñoFin').val();
            if ( date>date1) {
                $('#txtAñoFin').val(date);
            }
        }

    });


    $('#txtAño').Zebra_DatePicker({
        format: 'Y',
        onSelect: function (date) {
            consultarSemanas(date);
        }
    });

    $('#btnConsultar').on('click',function () {
        var value = $('#cboTipoReporte').find('option:selected').val();

        if (TipoReporte.MENSUAL==value) {
            consultarEvolucionMapeMensual();
        }
        if (TipoReporte.SEMANALDIARIO==value) {
            consultarEvolucionMapeSemanalDiario();
        }


    });

    $('#btnExportar').on('click', function () {
        var value = $('#cboTipoReporte').find('option:selected').val();

        if (TipoReporte.MENSUAL==value) {
            exportarEvolucionMapeMensual();
        }
        if (TipoReporte.SEMANALDIARIO==value) {
            exportarEvolucionMapeSemanalDiario();
        }
    });

    consultarSemanas($('#txtAño').val());

    function consultarSemanas(date) {
        $.ajax({
            url: controlador + "ObtenerSemanas",
            data: { anho: date  },
            type: 'POST',
            dataType: "json",
            success: function (result) {
                $("#cboSemana option").remove();
                $.each(result, function(key, value) {
                    $("#cboSemana").append(new Option(value.text,value.id));
                });
            },
            error: function (xhr, status) {
            },
            complete: function (xhr, status) {
            }
        });
    }


    $('#cboTipoReporte').on('change', function () {

        var value = $(this).find('option:selected').val();
        mostrarCompontesPorTipoRepor(value);

    }); 

});

function mostrarCompontesPorTipoRepor(value) {
    $("#tituloListado").html("");
    if (TipoReporte.MENSUAL == value) {
        $(".rmensual").show();
        $(".rsemanal").hide();
        $("#listado1").html("");
        $("#listado2").html("");
        $("#evolucionMapeSemanalDiario").html("");
        $("#evolucionDesviacionSemanalDiario").html("");
    }
    if (TipoReporte.SEMANALDIARIO == value) {
        $("#listado").html("");
        $(".rmensual").hide();
        $(".rsemanal").show();
    }
}


function consultarEvolucionMapeMensual() {

    $.ajax({
        url: controlador + "ConsultarEvolucionMapeMensual",
        data: { anhorinicio: $('#txtAñoInicio').val(), anhofin: $('#txtAñoFin').val() },
        type: 'POST',
        success: function (result) {
            $("#tituloListado").html("Evolucion del Mape mensual");
            $("#listado").html(result.Resultado);
            $("#listado1").html("");
            $("#listado2").html("");   
            $("#evolucionMapeSemanalDiario").html("");
            $("#evolucionDesviacionSemanalDiario").html("");
        },
        error: function (xhr, status) {
        },
        complete: function (xhr, status) {
        }
    });
}

function exportarEvolucionMapeMensual() {

    $.ajax({
        url: controlador + "ExportarEvolucionMapeMensual",
        data: { anhorinicio: $('#txtAñoInicio').val(), anhofin: $('#txtAñoFin').val() },
        type: 'POST',
        dataType: 'json',
        cache: false,
        success: function (result) {
            switch (result.NRegistros) {
                case 1: window.location = controlador + "Exportar?fi=" + result.Resultado; break;// si hay elementos
                case 2: alert("No existen registros !"); break;// sino hay elementos
                case -1: alert("Error en reporte result"); break;// Error en C#
            }
        },
        error: function (xhr, status) {
        },
        complete: function (xhr, status) {
        }
    });
}

function consultarEvolucionMapeSemanalDiario() {

    $.ajax({
        url: controlador + "ConsultarEvolucionMapeSemanalDiario",
        data: { anho: $('#txtAño').val(), semana: $('#cboSemana').val() },
        type: 'POST',
        success: function (result) {
            $("#tituloListado").html("Evolucion semanal-diario");
            $("#listado").html("");
            $("#listado1").html(result[0].Resultado);
            $("#listado2").html(result[1].Resultado);
            graficoEvolucionSemanal(result[0], "evolucionMapeSemanalDiario");
            graficoEvolucionSemanal(result[1], "evolucionDesviacionSemanalDiario");
        },
        error: function (xhr, status) {
        },
        complete: function (xhr, status) {
        }
    });
}

function exportarEvolucionMapeSemanalDiario() {

    $.ajax({
        url: controlador + "ExportarEvolucionMapeSemanalDiario",
        data: { anho: $('#txtAño').val(), semana: $('#cboSemana').val() },
        type: 'POST',
        success: function (result) {

            switch (result.NRegistros) {
            case 1: window.location = controlador + "Exportar?fi=" + result.Resultado; break;// si hay elementos
            case 2: alert("No existen registros !"); break;// sino hay elementos
            case -1: alert("Error en reporte result"); break;// Error en C#
            }
        },
        error: function (xhr, status) {
        },
        complete: function (xhr, status) {
        }
    });
}

function graficoEvolucionSemanal(dataResult, content) {
    var data = dataResult.Grafico;

    var series = [];
    for (var d in data.SerieData) {
        var item = data.SerieData[d];
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
