//variables globales
var controlador = siteRoot + 'Migraciones/AnexoA/'

$(function () {
    $("#leyenda").hide();
    cargarValoresIniciales();
});

function cargarValoresIniciales() {
    //$('#cbEmpresa').multipleSelect('checkAll');
    cargarAreaOperativa();
}

function cargarAreaOperativa() {

    validacionesxFiltro(1);
    if (resultFiltro) {

        $.ajax({
            type: 'POST',
            url: controlador + 'CargarAreaOperativa',
            data: {},
            success: function (aData) {
                $('#areaOperativa').html(aData);
                $('#cbAreaOperativa').multipleSelect({
                    width: '150px',
                    filter: true,
                    onClose: function (view) {
                        cargarListaDemandaPorArea();
                    }
                });

                $('#cbAreaOperativa').multipleSelect('checkAll');
                cargarListaDemandaPorArea();
            },
            error: function () {
                alert("Ha ocurrido un error");
            }
        });
    }
}

function cargarListaDemandaPorArea() {
    validacionesxFiltro(2);
    if (resultFiltro) {
        $.ajax({
            type: 'POST',
            url: controlador + 'CargarListaDemandaPorArea',
            data: {
                idEmpresa: getEmpresa(),
                idArea: getAreaOperativa(),
                idEquipo: getEquipo(),
                fechaInicio: getFechaInicio(),
                fechaFin: getFechaFin()
            },
            success: function (aData) {
                $('#listado').html(aData[0].Resultado);
                $("#leyenda").show();
                $('#idGraficoContainer').html('');
                DisenioGrafico(aData[1]);
            },
            error: function () {
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
            min: 0
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
            min: 0
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

    mostrarGrafico();
}

// Ventana flotante
function mostrarGrafico() {
    setTimeout(function () {
        $('#idGrafico2').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
            modalClose: false
        });
    }, 50);
}

//validaciones de los filtros de busqueda
function validacionesxFiltro(valor) {

    var idEmpresa = getEmpresa();
    var areaOperativa = getAreaOperativa();
    var idEquipo = getEquipo();
    var fechaInicio = getFechaInicio();
    var fechaFin = getFechaFin();

    var arrayFiltro = arrayFiltro || [];

    if (valor == 1)
        arrayFiltro.push({ id: idEmpresa, mensaje: "Seleccione la opcion Empresa" }, { id: areaOperativa, mensaje: "Seleccione la opcion Área" });
    else
        arrayFiltro.push({ id: idEmpresa, mensaje: "Seleccione la opcion Empresa" }, { id: areaOperativa, mensaje: "Seleccione la opcion Área" }, { id: idEquipo, mensaje: "Seleccione la opcion Equipo" });

    validarFiltros(arrayFiltro);
}