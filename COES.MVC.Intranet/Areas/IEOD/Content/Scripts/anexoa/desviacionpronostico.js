$(function () {
    buscar();
});

function mostrarReporteByFiltros() {
    buscar();
}

buscar = function () {
    cargarLista();
}

function cargarLista() {
    var fechaInicio = getFechaInicio();
    var fechaFin = getFechaFin();
    $("#graficos").hide();
    $("#listado1").html('');
    $("#listado1").hide();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaDesviacionesDemandaPronostico',
        data: { fechaInicio: fechaInicio, fechaFin: fechaFin },
        success: function (model) {
            var aData = model[0];
            $("#listado1").show();
            $('#listado1').html(aData.Resultado);
            $('.filtro_fecha_desc').html(aData.FiltroFechaDesc);

            $('#tabla').dataTable({
                "bSort": false,
                "sPaginationType": "full_numbers",
                "scrollX": true,
                "sDom": 'tp',
                "iDisplayLength": 48,
                "lengthMenu": [[48, 96, -1], [48, 96, "Todo"]],
                "destroy": "true",
                "bInfo": true,
            });

            var aData2 = model[1];
            if (aData2.Grafico.XAxisCategories != null) {
                graficoReporteDesviacionesDemanda(aData2);
                $("#graficos").show();
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

graficoReporteDesviacionesDemanda = function (result) {
    var series = [];
    var series1 = [];
    var series2 = [];

    if (result.Grafico.SerieDataS[0][0] != null) {
        for (k = 0; k < result.Grafico.SerieDataS[0].length; k++) {
            var seriePoint = [];
            var now = parseJsonDate(result.Grafico.SerieDataS[0][k].X);
            var nowUTC = Date.UTC(now.getFullYear(), now.getMonth(), now.getDate(), now.getHours(), now.getMinutes(), now.getSeconds());
            seriePoint.push(nowUTC);
            seriePoint.push(result.Grafico.SerieDataS[0][k].Y);
            series.push(seriePoint);
        }
    }
    if (result.Grafico.SerieDataS[1][0] != null) {
        for (k = 0; k < result.Grafico.SerieDataS[1].length; k++) {
            var seriePoint = [];
            var now = parseJsonDate(result.Grafico.SerieDataS[1][k].X);
            var nowUTC = Date.UTC(now.getFullYear(), now.getMonth(), now.getDate(), now.getHours(), now.getMinutes(), now.getSeconds());
            seriePoint.push(nowUTC);
            seriePoint.push(result.Grafico.SerieDataS[1][k].Y);
            series1.push(seriePoint);
        }
    }

    var opcion = {
        chart: {
            zoomType: 'xy'
        },
        rangeSelector: {
            selected: 1
        },

        title: {
            text: result.Grafico.titleText,
            style: {
                fontSize: '8'
            }
        },
        yAxis: [{
            labels: {
                format: '{value}'
            },
            title: {
                text: result.Grafico.YAxixTitle[0]
            },
            min: 4500
        },
        {
            title: {
                text: result.Grafico.YAxixTitle[1]
            },
            opposite: false
        }],
        tooltip: {
            pointFormat: '<span style="color:{series.color}">{series.name}</span>: <b>{point.y:,.2f}</b> <br/>',
            shared: true
        },
        legend: {
            layout: 'horizontal',
            align: 'center',
            verticalAlign: 'bottom',
            borderWidth: 0,
            enabled: true
        },
        series: []
    };
    opcion.series.push({
        name: result.Grafico.Series[0].Name,
        color: result.Grafico.Series[0].Color,
        data: series,
        type: result.Grafico.Series[0].Type
    });
    if (result.Grafico.SerieDataS[1]) {
        opcion.series.push({
            name: result.Grafico.Series[1].Name,
            color: result.Grafico.Series[1].Color,
            data: series1,
            type: result.Grafico.Series[1].Type
        });
    }

    $('#graficos').highcharts('StockChart', opcion);
}

function parseJsonDate(jsonDateString) {
    return new Date(parseInt(jsonDateString.replace('/Date(', '')));
}