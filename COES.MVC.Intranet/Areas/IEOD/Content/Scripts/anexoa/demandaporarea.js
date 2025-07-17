$(function () {
    $('#cbAreaOperativa').multipleSelect({
        width: '150px',
        filter: true,
        onClose: function (view) {
            cargarListaDemandaPorArea();
        }
    });

    parametro2 = getTipoLectura48();
    $('input[name=cbLectura48]').change(function () {
        parametro2 = getTipoLectura48();
        cargarListaDemandaPorArea();
    });

    $("#leyenda").hide();
    cargarValoresIniciales();
});

function cargarValoresIniciales() {
    $('#cbAreaOperativa').multipleSelect('checkAll');
    cargarListaDemandaPorArea();
}

function mostrarReporteByFiltros() {
    cargarListaDemandaPorArea();
}

function cargarListaDemandaPorArea() {
    var tipoData48 = $('input[name=cbLectura48]:checked').val();

    validacionesxFiltro(1);
    if (resultFiltro) {
        $.ajax({
            type: 'POST',
            url: controlador + 'CargarListaDemandaPorArea',
            data: {
                tipoDato48: getTipoLectura48(),
                idArea: getAreaOperativa(),
                fechaInicio: getFechaInicio(),
                fechaFin: getFechaFin()
            },
            success: function (aData) {
                $('.filtro_fecha_desc').html(aData.FiltroFechaDesc);
                $('#listado').html(aData[0].Resultado);
                $("#leyenda").show();
                $('#idGraficoContainer').html('');
                DisenioGrafico(aData[1]);
            },
            error: function (err) {
                alert("Ha ocurrido un error");
            }
        });
    }
}

function DisenioGrafico(result) {
    //generar series
    var tituloFuente1 = '';
    var tituloFuente2 = '';
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

        if (serie.YAxis == 0) {
            tituloFuente1 = serie.YAxisTitle;
        }
        if (serie.YAxis == 1) {
            tituloFuente2 = serie.YAxisTitle;
        }

        series.push(obj);
    }
    var dataHora = result.Grafico.XAxisCategories;
    var tituloGrafico = result.Grafico.TitleText;

    //Generar grafica
    Highcharts.chart('grafico1', {
        chart: {
            zoomType: 'xy'
        },
        title: {
            text: tituloGrafico
        },
        subtitle: {
            text: ''
        },
        xAxis: [{
            categories: dataHora,
            crosshair: true
        }],
        yAxis: [{ // Primary yAxis
            title: {
                text: tituloFuente1,
                style: {
                    color: 'blue'
                },
                align: 'high',
                rotation: 0,
                y: -15,
                offset: 0
            },
            labels: {
                format: '{value}',
                style: {
                    color: 'blue'
                }
            },
        }, { // Secondary yAxis
            title: {
                text: tituloFuente2,
                style: {
                    color: 'red'
                },
                align: 'high',
                rotation: 0,
                y: -15,
                offset: 0
            },
            labels: {
                format: '{value}',
                style: {
                    color: 'red'
                }
            },
            opposite: true,
        }],
        tooltip: {
            shared: true
        },
        legend: {
            layout: 'horizontal',
            align: 'center',
            verticalAlign: 'bottom',
            floating: false,
            backgroundColor: (Highcharts.theme && Highcharts.theme.legendBackgroundColor) || '#FFFFFF'
        },
        plotOptions: {
            spline: {
                lineWidth: 1,
                states: {
                    hover: {
                        lineWidth: 5
                    }
                },
                marker: {
                    enabled: false
                }
            }
        },
        series: series
    });
}

//validaciones de los filtros de busqueda
function validacionesxFiltro(valor) {

    var areaOperativa = getAreaOperativa();

    var arrayFiltro = arrayFiltro || [];

    if (valor == 1)
        arrayFiltro.push({ id: areaOperativa, mensaje: "Seleccione la opcion Área" });
    
    validarFiltros(arrayFiltro);
}

function verPuntoCalculado(ptomedicodi) {
    var url = siteRoot + 'ReportesMedicion/FormatoReporte/' + "IndexDetalleCalculado?pto=" + ptomedicodi;
    window.open(url).focus();
}

function verReporte(reporcodi) {
    var url = siteRoot + 'ReportesMedicion/FormatoReporte/' + "IndexDetalle?id=" + reporcodi;
    window.open(url).focus();
}