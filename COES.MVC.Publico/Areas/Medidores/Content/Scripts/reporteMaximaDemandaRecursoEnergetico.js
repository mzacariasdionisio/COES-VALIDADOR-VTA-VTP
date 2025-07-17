var controlador = siteRoot + 'Medidores/maximademanda/'

$(function () {
    $('#mes').change({
        format: 'Y-m',
        onSelect: function () {
            consultar();
        }
    });

    $('#btnConsultar').click(function () {
        consultar();
    });

    $('#btnExportar').click(function () {
        exportar();
    });

    consultar();
});

function consultar() {
    $("#recursoEnergetico").html("");
    if (validarConsulta) {
        mostrarRecursoEnergetico();
    }
}

function validarConsulta() {
    return true;
}

function exportar() {
    var mes = $("#mes").val();

    $.ajax({
        type: 'POST',
        url: controlador + 'ExportarRecursoEnergetico',
        data: {
            mes: mes
        },
        success: function (result) {
            if (result.length > 0) {
                archivo = result[0];
                nombre = result[1];
                if (archivo != '-1') {
                    window.location.href = controlador + 'DescargarArchivo?rutaArchivoTemp=' + archivo + "&nombreArchivo=" + nombre;
                } else {
                    alert("Error en descargar el archivo");
                }
            }
            else {
                alert("Error en descargar el archivo");
            }
        },
        error: function () {
            alert('ha ocurrido un error al descargar el archivo excel.');
        }
    });
}

function mostrarRecursoEnergetico() {
    var mes = $("#mes").val();

    $.ajax({
        type: 'POST',
        url: controlador + 'ReporteRecursoEnergetico',
        data: {
            mes: mes
        },
        success: function (data) {
            $('#recursoEnergetico').html(data);

            var objGrafico = JSON.parse($("#hfJsonGrafico").val());

            if (objGrafico) {
                generarGrafico('idVistaGrafica', objGrafico);
            }
            else {
                $('#idVistaGrafica').html('Sin Grafico - No existen registros a mostrar!');
            }
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function generarGrafico(idGrafico, data) {
    //obtener data
    var dataGrafico = [];
    var tituloGrafico = data.Titulo;
    var leyendaFuente = data.Leyenda;

    for (var i = 0; i < data.ListaConsolidadoRecursoEnergetico.length; i++) {
        var g = data.ListaConsolidadoRecursoEnergetico[i];
        dataGrafico.push({
            name: g.Fenergnomb,
            y: g.Porcentaje,
            color: g.Fenercolor
        });
    }

    //Generar grafica
    Highcharts.chart(idGrafico, {
        exporting: {
            chartOptions: {
                plotOptions: {
                    series: {
                        dataLabels: {
                            enabled: true
                        }
                    }
                }
            },
            fallbackToExportServer: false,
            filename: tituloGrafico
        },
        chart: {
            plotBackgroundColor: null,
            plotBorderWidth: null,
            plotShadow: false,
            type: 'pie'
        },
        title: {
            text: tituloGrafico
        },
        tooltip: {
            pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
        },
        plotOptions: {
            pie: {
                allowPointSelect: true,
                cursor: 'pointer',
                dataLabels: {
                    enabled: true,
                    format: '{point.percentage:.1f} %',
                    style: {
                        color: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black'
                    }
                },
                showInLegend: true
            }
        },
        series: [{
            name: 'Participación',
            colorByPoint: true,
            data: dataGrafico
        }]
    });
}