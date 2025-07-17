var controlador = siteRoot + 'IEOD/AnexoA/';
var ancho = 900;

$(function () {
    parametro2 = getTipoLectura48();
    $('input[name=cbLectura48]').change(function () {
        parametro2 = getTipoLectura48();
        cargarLista();
    });

    $('#btnBuscar').click(function () {
        cargarLista();
    });

    $('#btnDescargar').click(function () {
        descargar();
    });

    ancho = $("#mainLayout").width() > 900 ? $("#mainLayout").width() - 20 : 900;
    cargarLista();
});

function mostrarReporteByFiltros() {
    cargarLista();
}

function cargarLista() {
    var fechaInicio = getFechaInicio();
    var fechaFin = getFechaFin();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaGeneracionDelSEIN',
        data: {
            tipoDato48: getTipoLectura48(),
            fechaInicio: fechaInicio,
            fechaFin: fechaFin
        },
        success: function (aData) {
            $('.filtro_fecha_desc').html(aData[0].FiltroFechaDesc);
            $('#listado').html(aData[0].Resultado);
            var anchoReporte = $('#reporte').width();
            $('#reporte').dataTable({
                "scrollX": true,
                "scrollY": "780px",
                "scrollCollapse": true,
                "sDom": 't',
                "ordering": false,
                paging: false,
                fixedColumns: {
                    leftColumns: 1
                }
            });

            $("#resultado").css("width", (anchoReporte > ancho ? ancho : anchoReporte) + "px");
            $('#idGraficoContainer').html('');
            disenioGrafico(aData[1], 'grafico1');
            disenioGrafico(aData[2], 'grafico2');
            disenioGrafico(aData[3], 'grafico3');
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function disenioGrafico(result, grafico) {
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
    Highcharts.chart(grafico, {
        chart: {
            type: 'area',
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
            area: {
                stacking: 'normal',
                lineColor: '#666666',
                lineWidth: 1,
                marker: {
                    lineWidth: 1,
                    lineColor: '#666666'
                }
            }
        },
        series: series
    });
}

function verPuntoCalculado(ptomedicodi) {
    var url = siteRoot + 'ReportesMedicion/FormatoReporte/' + "IndexDetalleCalculado?pto=" + ptomedicodi;
    window.open(url).focus();
}