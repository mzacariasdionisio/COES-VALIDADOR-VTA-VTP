var controlador = siteRoot + 'Siosein/InformesOpeSein/';

$(function () {
    CargarResumenInformeAnual(); 
});

function CargarResumenInformeAnual() {
    $.ajax({
        type: 'POST',
        url: controlador + 'CargarResumenInformeAnual',
        data: {},
        dataType: 'json',
        success: function (e) {
            $('#listado').html(e.Resultado);
            if (e.ListaGrafico.length > 0) {
                $('#grafico1').css("display", "block");
                mostrarGrafico(e, 'grafico1', 'grafico2', 1);
            }
            else {
                $('#grafico1').css("display", "none");
            }
            if (e.ListaGrafico2.length > 0) {
                $('#grafico2').css("display", "block");
                mostrarGrafico(e, 'grafico1', 'grafico2', 2);
            }
            else {
                $('#grafico2').css("display", "none");
            }
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function mostrarGrafico(result, idGrafico, idGrafico2, tipografico) {
    var opcion;
    if (tipografico == 1) {
        var json = result.ListaGrafico;
        var jsondata = [];
        for (var i in json) {
            var jsonLista = json[i];
            jsondata.push({
                name: jsonLista.SerieName,
                y: jsonLista.Valor
            });
        }

        opcion = {
            chart: {
                type: 'pie',
                options3d: {
                    enabled: true,
                    alpha: 25
                }
            },
            title: {
                text: result.Titulo
            },
            credits: {
                enabled: false
            },
            plotOptions: {
                pie: {
                    allowPointSelect: true,
                    cursor: 'pointer',
                    innerSize: 150,
                    depth: 45,
                    dataLabels: {
                        enabled: true,
                        format: '<b>{point.name}</b>: {point.percentage:.1f} %',
                        style: {
                            color: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black'
                        }
                    }
                }
            },
            series: [{
                colorByPoint: true,
                name: 'Total',
                data: jsondata
            }]
        };

        $('#' + idGrafico).highcharts(opcion);
    }

    if (tipografico == 2) {
        json = result.ListaGrafico2;
        jsondata = [];
        for (var i in json) {
            var jsonLista = json[i];
            jsondata.push({
                name: jsonLista.SerieName,
                y: jsonLista.Valor
            });
        }

        opcion = {
            chart: {
                type: 'pie',
                options3d: {
                    enabled: true,
                    alpha: 25
                }
            },
            title: {
                text: result.SubTitulo
            },
            credits: {
                enabled: false
            },
            plotOptions: {
                pie: {
                    allowPointSelect: true,
                    cursor: 'pointer',
                    innerSize: 150,
                    depth: 45,
                    dataLabels: {
                        enabled: true,
                        format: '<b>{point.name}</b>: {point.percentage:.1f} %',
                        style: {
                            color: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black'
                        }
                    }
                }
            },
            series: [{
                colorByPoint: true,
                name: 'Total',
                data: jsondata
            }]
        };
        $('#' + idGrafico2).highcharts(opcion);
    }
}