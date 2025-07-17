var controlador = siteRoot + 'IEOD/DashBoard/';
$(function () {

    $('#txtFechaInicio').Zebra_DatePicker({/*direction: -1*/
        onSelect: function () {
        }
    });

    $("#btnManualUsuario").click(function () {
        window.location = controlador + 'DescargarManualUsuario';
    });

    $('#cbDashBoard').on('change', function () {
    });

    $('#btnConsultar').click(function () {
        onload_Ini();
    });

});

function onload_Ini() {
    $("#dashboard1").hide();
    $("#dashboard2").hide();
    $("#dashboard3").hide();
    $("#dashboard6").hide();
    $("#dashboard10").hide();

    cargarDashboard();
}

function cargarDashboard() {
    var tipo = $('#cbDashBoard').val();
    $.ajax({
        type: 'POST',
        url: controlador + 'CargarTipoDashboard',
        data: { strFecha: $("#txtFechaInicio").val(), tipo: tipo },
        success: function (e) {
            switch (tipo) {
                case "1": //Produccion Energia y Maxima Demanda

                    $("#listado1_1").hide();
                    $('#grafico1_1').hide();
                    $('#grafico1_2').hide();
                    $('#grafico1_3').hide();
                    $('#grafico1_4').hide();

                    if (e.length != undefined && e.length > 0) {
                        $("#listado1_1").html(e[0].Resultado);
                        $("#listado1_1").show();

                        $('#grafico1_1').show(); mostrarGrafico(e[1].Grafico, 'grafico1_1');
                        $('#grafico1_2').show(); mostrarGrafico(e[2].Grafico, 'grafico1_2');
                        $('#grafico1_3').show(); mostrarGrafico(e[3].Grafico, 'grafico1_3');
                        $('#grafico1_4').show(); mostrarGrafico(e[4].Grafico, 'grafico1_4');

                        $("#listado1_2").html(e[5].Resultado);
                        $("#listado1_2").show();
                    }

                    $("#dashboard1").show();
                    break;
                case "2": //Demanda por Area Operativa
                    $("#listado2_1").hide();
                    $('#grafico2_1').hide();
                    $('#grafico2_2').hide();
                    $('#grafico2_3').hide();
                    $('#grafico2_4').hide();

                    if (e.length != undefined && e.length > 0) {
                        $("#listado2_1").html(e[0].Resultado);
                        $("#listado2_1").show();

                        $('#grafico2_1').show(); mostrarGrafico(e[1].Grafico, 'grafico2_1');
                        $('#grafico2_2').show(); mostrarGrafico(e[2].Grafico, 'grafico2_2');
                        $('#grafico2_3').show(); mostrarGrafico(e[3].Grafico, 'grafico2_3');
                        $('#grafico2_4').show(); mostrarGrafico(e[4].Grafico, 'grafico2_4');

                        //Generacion de Html
                        var cont = 1;
                        var htmlDash = '';
                        var listaAreaOp = e[0].listaAreaOperativa;

                        for (var i = 0; i < listaAreaOp.length; i++) {
                            var reporte = listaAreaOp[i];
                            var htmlTitulo = '<h2>Área Operativa ' + reporte.Ptomedibarranomb + '</h2>';
                            var htmlTabla = '<div style="clear:both; height:15px"></div>';
                            htmlTabla += '<div id="listado2_2_' + cont + '" style="display: block; float: left; width: 100%;">' + e[4 + cont].Resultado + '</div>';

                            if (e[4 + cont].Resultado != null) {
                                htmlDash += '<tr>'
                                htmlDash += '<td style="padding-top: 50px;">';
                                htmlDash += htmlTitulo;
                                htmlDash += '</td>';
                                htmlDash += '</tr>'

                                htmlDash += '<tr>'
                                htmlDash += '<td>';
                                htmlDash += htmlTabla;
                                htmlDash += '</td>';
                                htmlDash += '</tr>'
                            }

                            cont++;
                        }

                        $("#listado2_2").html(htmlDash);
                        $("#listado2_2").show();
                    }

                    $("#dashboard2").show();
                    break;
                case "3": //Consumo Grandes Usuarios
                    $("#listado3_1").hide();
                    $('#grafico3_1').hide();
                    $('#grafico3_2').hide();
                    $('#grafico3_3').hide();
                    $('#grafico3_4').hide();

                    if (e.length != undefined && e.length > 0) {
                        $("#listado3_1").html(e[0].Resultado);
                        $("#listado3_1").show();

                        $('#grafico3_1').show(); mostrarGrafico(e[1].Grafico, 'grafico3_1');
                        $('#grafico3_2').show(); mostrarGrafico(e[2].Grafico, 'grafico3_2');
                        $('#grafico3_3').show(); mostrarGrafico(e[3].Grafico, 'grafico3_3');
                        $('#grafico3_4').show(); mostrarGrafico(e[4].Grafico, 'grafico3_4');

                    }
                    $("#dashboard3").show();
                    break;
                case "4": //Costos Operación 
                    break;
                case "5": //Costos Marginales
                    break;
                case "6": //Participación Recursos Energéticos

                    $("#listado6_1").hide();
                    $('#grafico6_1').hide();
                    $('#grafico6_2').hide();
                    $('#grafico6_3').hide();
                    $('#grafico6_4').hide();

                    $("#listado6_2_1").hide();
                    $('#grafico6_2_1').hide();
                    $('#grafico6_2_2').hide();
                    $('#grafico6_2_3').hide();
                    $('#grafico6_2_4').hide();

                    if (e.length != undefined && e.length > 0) {
                        $("#listado6_1").html(e[0].Resultado);
                        $("#listado6_1").show();

                        $('#grafico6_1').show(); graficarPieDonut(e[1].Grafico, 'grafico6_1');
                        $('#grafico6_2').show(); graficarPieDonut(e[2].Grafico, 'grafico6_2');
                        $('#grafico6_3').show(); graficarPieDonut(e[3].Grafico, 'grafico6_3');
                        $('#grafico6_4').show(); graficarPieDonut(e[4].Grafico, 'grafico6_4');

                        $("#listado6_2").html(e[5].Resultado);
                        $("#listado6_2").show();

                        $('#grafico6_2_1').show(); graficarPieDonut(e[6].Grafico, 'grafico6_2_1');
                        $('#grafico6_2_2').show(); graficarPieDonut(e[7].Grafico, 'grafico6_2_2');
                        $('#grafico6_2_3').show(); graficarPieDonut(e[8].Grafico, 'grafico6_2_3');
                        $('#grafico6_2_4').show(); graficarPieDonut(e[9].Grafico, 'grafico6_2_4');
                    }

                    $("#dashboard6").show();
                    break;
                case "7": //Participación RER
                    break;
                case "8": //Combustibles Utilizados
                    break;
                case "9": //Costos Variables
                    break;
                case "10": //Energía Primaria
                    $("#listado10_1").hide();
                    $("#grafico10").hide();

                    if (e.length != undefined && e.length > 0) {
                        $("#listado10_1").html(e[0].Resultado);
                        $("#listado10_1").show();

                        //Generacion de Html
                        var cont = 1;
                        var htmlDash = '';
                        var listaRepEnerg = e[0].ListaTipoEnergiaPrimaria;

                        for (var i = 0; i < listaRepEnerg.length; i++) {
                            var reporte = listaRepEnerg[i];
                            var htmlTitulo = '<h2>' + reporte.Energprimnomb + '</h2>';
                            var htmlGrafico = '<div style="clear:both; height:15px"></div>';
                            for (var j = 0; j < 4; j++) {
                                htmlGrafico += '<div id="grafico10_' + cont + '" style="display: none; float: left; width: 400px"></div>';
                                cont++;
                            }

                            htmlDash += '<tr>'
                            htmlDash += '<td style="padding-top: 50px;">';
                            htmlDash += htmlTitulo;
                            htmlDash += '</td>';
                            htmlDash += '</tr>'

                            htmlDash += '<tr>'
                            htmlDash += '<td>';
                            htmlDash += htmlGrafico;
                            htmlDash += '</td>';
                            htmlDash += '</tr>'
                        }

                        $("#grafico10").html(htmlDash);

                        //Setear grafico
                        for (var i = 1; i < cont; i++) {
                            mostrarGraficoTipoptomedicodi(e[i].Grafico, 'grafico10_' + i);
                            $("#" + 'grafico10_' + i).show();
                        }

                        $("#grafico10").show();
                    }
                    $("#dashboard10").show();
                    break;
            }

        },
        error: function () {
            alert("Ha ocurrido un error.");
        }
    });
}

function mostrarGrafico(result, idGrafico) {
    var opcion, json, _titulo;
    _titulo = result.TitleText;
    json = result.Series;

    var jsondata = [];

    for (var i = 0; i < json.length; i++) {
        var jsonLista = json[i];
        jsondata.push({
            name: jsonLista.Name,
            y: jsonLista.Acumulado,
            color: jsonLista.Color,
            sliced: i == 0,
            selected: i == 0
        });
    }

    Highcharts.chart(idGrafico, {
        chart: {
            plotBackgroundColor: null,
            plotBorderWidth: null,
            plotShadow: false,
            type: 'pie'
        },
        title: {
            text: _titulo
        },
        tooltip: {
            pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b> <br/> Valor(GWh): <b>{point.y:.1f}</b>'
        },
        plotOptions: {
            pie: {
                allowPointSelect: true,
                cursor: 'pointer',
                dataLabels: {
                    enabled: false
                },
                showInLegend: true
            }
        },
        series: [{
            name: 'Total',
            colorByPoint: true,
            data: jsondata
        }]
    });
}


function mostrarGraficoTipoptomedicodi(result, idGrafico) {
    var opcion, json, _titulo;
    _titulo = result.TitleText;
    json = result.Series;

    var jsondata = [];

    for (var i = 0; i < json.length; i++) {
        var jsonLista = json[i];
        jsondata.push({
            name: jsonLista.Name,
            y: jsonLista.Acumulado,
            color: jsonLista.Color,
            sliced: i == 0,
            selected: i == 0,
            medida: jsonLista.YAxisTitle
        });
    }

    Highcharts.chart(idGrafico, {
        chart: {
            plotBackgroundColor: null,
            plotBorderWidth: null,
            plotShadow: false,
            type: 'pie'
        },
        title: {
            text: _titulo
        },
        tooltip: {
            pointFormat: 'Porcentaje: <b>{point.percentage:.1f}%</b> <br/> Valor: <b>{point.y:.1f} {point.medida}</b>'
        },
        plotOptions: {
            pie: {
                allowPointSelect: true,
                cursor: 'pointer',
                dataLabels: {
                    enabled: false
                },
                showInLegend: true
            }
        },
        series: [{
            name: 'Total',
            colorByPoint: true,
            data: jsondata
        }]
    });
}

function graficarPieDonut(result, idGrafico) {
    var opcion, json, _titulo;

    _titulo = result.TitleText;
    var primerPie = result.XAxisTitle;
    var segundoPie = result.YaxixTitle;

    // Generar los objetos
    var series = result.Series;
    var seriesData = result.SerieDataS;

    var categorias = [], detalle = [];
    for (var i = 0; i < series.length; i++) {
        var jsonLista = series[i];
        var valorCtg = jsonLista.Acumulado != null ? Math.round(jsonLista.Acumulado * 100) / 100 : 0;

        categorias.push({
            name: jsonLista.Name,
            y: valorCtg,
            color: jsonLista.Color
        });

        var listaDet = seriesData[i];
        for (var j = 0; j < listaDet.length; j++) {
            var jsonListaDet = listaDet[j];

            var valorDet = jsonListaDet.Y != null ? Math.round(jsonListaDet.Y * 100) / 100 : 0;

            detalle.push({
                name: jsonListaDet.Name,
                y: valorDet,
                color: jsonListaDet.Color
            });
        }
    }

    // Create the chart
    Highcharts.chart(idGrafico, {
        chart: {
            type: 'pie'
        },
        title: {
            text: _titulo
        },
        plotOptions: {
            pie: {
                shadow: false,
                center: ['50%', '50%']
            }
        },
        tooltip: {
            pointFormat: '{series.name}: <b>{point.y:.1f} GWh</b> <br/> Porcentaje: <b>{point.percentage:.1f}%</b>'
        },
        series: [{
            name: primerPie,
            data: categorias,
            size: '60%',
            dataLabels: {
                formatter: function () {
                    return this.percentage > 0 ? this.point.name : null;
                },
                color: '#ffffff',
                distance: -30
            }
        }, {
            name: segundoPie,
            data: detalle,
            size: '80%',
            innerSize: '60%',
            dataLabels: {
                formatter: function () {
                    return this.percentage > 0 ? this.point.name : null;
                }
            },
            id: 'detalleCtg'
        }],
        responsive: {
            rules: [{
                condition: {
                    maxWidth: 400
                },
                chartOptions: {
                    series: [{
                        id: 'detalleCtg',
                        dataLabels: {
                            enabled: false
                        }
                    }]
                }
            }]
        }
    });
}