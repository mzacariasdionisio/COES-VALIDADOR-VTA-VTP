$(function () {
    cargarHorasCongestionAreaOpe();
});

function mostrarReporteByFiltros() {
    cargarHorasCongestionAreaOpe();
}

function cargarHorasCongestionAreaOpe() {
    var codigoVersion = getCodigoVersion();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarHorasCongestionAreaOpe',
        data: {
            codigoVersion: codigoVersion
        },
        success: function (aData) {
            $('.filtro_fecha_desc').html(aData.FiltroFechaDesc);
            $('#listado').html(aData.Resultado);
            $('#grafico1').css("display", "block");
            HorasCongestionAreaOpe(aData.Grafico, "grafico1");
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function HorasCongestionAreaOpe(result, idGrafico) {
    var opcion;

    var categoria = [];

    for (var d in result.Categorias) {
        var item = result.Categorias[d];
        if (item == null) {
            continue;
        }
        categoria.push({
            name: item.Name,
            categories: item.Categories,
        });
    }

    var series_ = [];
    for (var i = 0; i < result.Series.length; i++) {
        var serie = result.Series[i];
        var obj = {
            name: serie.Name,
            type: serie.Type,
            yAxis: serie.YAxis,
            data: result.SeriesData[i],
            color: serie.Color
        };

        if (serie.YAxis == 0) {
            tituloFuente1 = serie.YAxisTitle;
        }
        if (serie.YAxis == 1) {
            tituloFuente2 = serie.YAxisTitle;
        }
        series_.push(obj);
    }

    opcion = {
        chart: {
            type: 'column'
        },
        title: {
            text: result.TitleText
        },
        xAxis: {
            categories: categoria,
            //categories: result.Grafico.XAxisCategories,
            //type: 'datetime'
            labels: {
                rotation: result.XAxisLabelsRotation
            },
            crosshair: true
        },
        yAxis: {
            title: {
                text: result.XAxisTitle
            },
            labels: {
                formatter: function () {
                    return this.value;
                },
                format: '{value:,.2f}'
            }
        },
        tooltip: {
            pointFormat: '{series.name} Potencia <b>{point.y:,.2f}</b><br/>'
        },
        subtitle: {
            text: result.Subtitle,
            align: 'left',
            verticalAlign: 'bottom',
            floating: true,
            x: 10,
            y: 9
        },
        plotOptions: {
            //series: {
            //    stacking: 'normal'
            //}
        },
        series: series_
    };

    $('#' + idGrafico).highcharts(opcion);
}