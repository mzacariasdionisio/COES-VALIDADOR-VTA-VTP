var anchoListado = 900;

$(function () {
    $('#cbEmpresa').multipleSelect({
        width: '150px',
        filter: true,
        onClose: function (view) {
            cargarListaDemandaGrandesUsuarios();
        }
    });

    $('#btnBuscar').click(function () {
        cargarListaDemandaGrandesUsuarios();
    });

    $('#btnGraficos').click(function () {
        cargarGrafico();
    });

    anchoListado = $("#mainLayout").width() - 20;

    cargarValoresIniciales();
});

function cargarValoresIniciales() {
    $('#cbEmpresa').multipleSelect('checkAll');
    cargarListaDemandaGrandesUsuarios();
}
function mostrarReporteByFiltros() {
    cargarListaDemandaGrandesUsuarios();
}

function cargarListaDemandaGrandesUsuarios() {
    var idEmpresa = getEmpresa();
    var idUbicacion = getUbicacion();
    var fechaInicio = getFechaInicio();
    var fechaFin = getFechaFin();
    var idEquipo = getEquipo();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaDemandaGrandesUsuarios',
        data: { idEmpresa: idEmpresa, idUbicacion: idUbicacion, idEquipo: idEquipo, fechaInicio: fechaInicio, fechaFin: fechaFin },
        success: function (aData) {
            if (aData !== undefined && aData != null && aData.length > 0) {
                $('.filtro_fecha_desc').html(aData[0].FiltroFechaDesc);
                cargarListado(aData[0]);
                cargarGrafico(1, aData[1]);
                cargarGrafico(2, aData[2]);
                cargarGrafico(3, aData[3]);
                cargarGrafico(4, aData[4]);
            } else {
                alert("Ha ocurrido un error");
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function cargarListado(aData) {
    $('#listado').html(aData.Resultado);
    $('#idGraficoContainer').html('');
    $("#div_reporte").css("width", anchoListado + "px");
    $('#reporte').dataTable({
        "scrollX": true,
        "scrollCollapse": true,
        "sDom": 't',
        "ordering": false,
        paging: false,
        fixedColumns: {
            leftColumns: 1
        }
    });
}

function cargarGrafico(tipoGrafico, aData) {
    if (aData.Grafico.Series.length > 0) {
        var id = 'idVistaGrafica' + tipoGrafico;
        $("#" + id).css("width", anchoListado + "px");
        DisenioGrafico(aData, id);
    }
    else {
        $('#idGraficoContainer').html("No existen registros !");
    }
}

function DisenioGrafico(result, id) {
    //generar series
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
    var dataHora = result.Grafico.XAxisCategories;
    var tituloGrafico = result.Grafico.TitleText;

    //Generar grafica
    Highcharts.chart(id, {
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
        yAxis: {
            title: {
                text: 'MW'
            },
            labels: {
                format: '{value}',
            }
        },
        tooltip: {
            shared: true
        },
        legend: {
            align: 'center',
            verticalAlign: 'bottom',
            layout: 'horizontal'
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