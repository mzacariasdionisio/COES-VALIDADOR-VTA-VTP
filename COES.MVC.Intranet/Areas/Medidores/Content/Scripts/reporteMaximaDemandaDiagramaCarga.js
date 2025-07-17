var controlador = siteRoot + 'Medidores/reportes/'

$(function () {
    $('#mes').Zebra_DatePicker({
        format: 'm Y',
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
    $("#diagramaCargaMaxDemanda").html("");
    if (validarConsulta) {
        mostrarDiagramaCargaMaximaDemanda();
    }
}

function validarConsulta() {
    return true;
}

function exportar() {
    var mes = $("#mes").val();

    $.ajax({
        type: 'POST',
        url: controlador + 'ExportarDiagramaCargaMaximaDemanda',
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

function mostrarDiagramaCargaMaximaDemanda() {
    var mes = $("#mes").val();

    $.ajax({
        type: 'POST',
        url: controlador + 'ReporteDiagramaCargaMaximaDemanda',
        data: {
            mes: mes
        },
        success: function (data) {
            $('#diagramaCargaMaxDemanda').html(data);

            // tabla
            $('#tablaDetalle').dataTable({
                "scrollY": 430,
                "scrollX": true,
                "sDom": 't',
                "ordering": false,
                "bPaginate": false,
                "iDisplayLength": -1
            });

            // diagrama
            var objDiagrama = JSON.parse($("#hfJsonGrafico").val());

            if (objDiagrama) {
                generarGrafico('idVistaGrafica', objDiagrama);
            }
            else $('#idVistaGrafica').html('Sin Grafico - No existen registros a mostrar!');
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function generarGrafico(idGrafico, data) {
    //obtener data
    var dataHora = [];
    var dataDemanda = [];
    var tituloGrafico = data.Titulo;
    var leyendaFuente = data.Leyenda;

    for (var i = 0; i < data.ListaDemandaCuartoHora.length; i++) {
        var g = data.ListaDemandaCuartoHora[i];
        dataHora.push(g.FechaOnlyHora);
        dataDemanda.push(g.Valor);
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
            zoomType: 'x'
        },
        title: {
            text: tituloGrafico
        },
        xAxis: {
            categories: dataHora
        },
        yAxis: {
            title: {
                text: leyendaFuente
            },
            min: 0,
            labels: {
                formatter: function () {
                    return this.value;
                }
            }
        },
        legend: {
            enabled: false
        },
        plotOptions: {
            area: {
                fillColor: {
                    linearGradient: {
                        x1: 0,
                        y1: 0,
                        x2: 0,
                        y2: 1
                    },
                    stops: [
                        [0, Highcharts.getOptions().colors[0]],
                        [1, Highcharts.Color(Highcharts.getOptions().colors[0]).setOpacity(0).get('rgba')]
                    ]
                },
                marker: {
                    enabled: false,
                    symbol: 'circle',
                    radius: 2,
                    states: {
                        hover: {
                            enabled: true
                        }
                    }
                },
                lineWidth: 1,
                states: {
                    hover: {
                        lineWidth: 1
                    }
                },
                threshold: null
            }
        },
        series: [{
            type: 'area',
            name: leyendaFuente,
            data: dataDemanda
        }]
    });

}