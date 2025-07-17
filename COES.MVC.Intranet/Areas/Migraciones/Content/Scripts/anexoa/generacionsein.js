var controlador = siteRoot + 'Migraciones/AnexoA/'
$(function () {
    $('#btnBuscar').click(function () {
        cargarLista();
    });

    $('#txtFechaInicio').Zebra_DatePicker({
        //direction: -1
    });

    $('#txtFechaFin').Zebra_DatePicker({
        //direction: -1
    });

    $('#btnGrafico1').click(function () {
        cargarGrafico(1);
    });

    $('#btnGrafico2').click(function () {
        cargarGrafico(2);
    });

    $('#btnGrafico3').click(function () {
        cargarGrafico(3);
    });

    $('#btnDescargar').click(function () {
        descargar();
    });

    cargarLista();
});

function cargarLista() {
    var fechaInicio = $("#txtFechaInicio").val();
    var fechaFin = $("#txtFechaFin").val();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaGeneracionDelSEIN',
        data: { fechaInicio: fechaInicio, fechaFin: fechaFin },
        success: function (aData) {
            $('#listado').html(aData[0].Resultado);
            $('#idGraficoContainer').html('');
            disenioGrafico(aData[1], 'grafico1');
            disenioGrafico(aData[2], 'grafico2');
            disenioGrafico(aData[3], 'grafico3');
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function cargarGrafico(valor) {
    var fechaInicio = $('#txtFechaInicio').val();
    var fechaFin = $('#txtFechaFin').val();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarGraficoGeneracionSein',
        data: { fechaInicio: fechaInicio, fechaFin: fechaFin, tipoReporte: valor },
        dataType: 'json',
        success: function (aData) {
            if (aData.Grafico.Series.length > 0) {
                disenioGrafico(aData);
                $("#idGraficoContainer").html('');
            }
            else {
                $('#idGraficoContainer').html('No existen registros !');
            }
        },
        error: function () {
            alert('Ha ocurrido un error');
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

    //mostrarGrafico();
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

function descargar() {
    $.ajax({
        type: 'POST',
        async: true,
        contentType: 'application/json',
        url: controlador + 'GenerarReporteAnexoAxls',
        data: JSON.stringify({
            fecha: getFechaInicio(),
        }),
        beforeSend: function () {
        },
        success: function (result) {
            if (result != null && result.Resultado != "-1") {
                window.location.href = controlador + 'ExportarReporteXls?nameFile=' + result.Resultado;
            }
            else {
                alert("Error en descargar el archivo");
            }
        },
        error: function (result) {
            alert('ha ocurrido un error al descargar el archivo excel. ' + result.status + ' - ' + result.statusText + '.');
        }
    });
}