var controlador = siteRoot + 'Migraciones/AnexoA/'
var USAR_COMBO_TIPOCOMBUSTIBLE = false;
var anchoListado = 900;

$(function () {

    $('#cbEmpresa').multipleSelect({
        width: '150px',
        filter: true,
        onClose: function (view) {
            cargarTipoCentral();
        }
    });

    $('#txtFechaInicio').Zebra_DatePicker({
        //direction: -1
    });

    $('#txtFechaFin').Zebra_DatePicker({
        //direction: -1
    });

    anchoListado = $("#mainLayout").width() - 20;

    $('#btnGraficoDiagramaCarga').click(function () {
        setTimeout(function () {
            $('#idGrafico1').bPopup({
                easing: 'easeOutBack',
                speed: 450,
                transition: 'slideDown',
                modalClose: false
            });
        }, 50);
    });

    $('#btnGraficoParticipacionTipoRecurso').click(function () {
        setTimeout(function () {
            $('#idGrafico2').bPopup({
                easing: 'easeOutBack',
                speed: 450,
                transition: 'slideDown',
                modalClose: false
            });
        }, 50);
    });

    $('#btnGraficoCentralRER').click(function () {
        setTimeout(function () {
            $('#idGrafico3').bPopup({
                easing: 'easeOutBack',
                speed: 450,
                transition: 'slideDown',
                modalClose: false
            });
        }, 50);
    });

    $('#btnGraficoTipoGeneracionRER').click(function () {
        setTimeout(function () {
            $('#idGrafico4').bPopup({
                easing: 'easeOutBack',
                speed: 450,
                transition: 'slideDown',
                modalClose: false
            });
        }, 50);
    });

    $("#ckRecursos").change(function () {
        generarReporte();
    });

    cargarValoresIniciales();
});

function cargarValoresIniciales() {
    $("#GraficoRecursosEnergeticosParticipacionRecurso").hide();
    $("#btnGraficoParticipacionTipoRecurso").hide();
    $('#cbEmpresa').multipleSelect('checkAll');

    cargarTipoCentral();
}

function GetValorCheckRecurso() {
    var soloRecursos = 0;
    if ($("#ckRecursos").prop('checked')) {
        var soloRecursos = 1;
    }

    return soloRecursos;
}

function cargarTipoCentral() {
    var empresa = $('#cbEmpresa').multipleSelect('getSelects');

    if (empresa == "[object Object]") empresa = "-1";
    $('#hfEmpresa').val(empresa);
    $.ajax({
        type: 'POST',
        url: controlador + 'CargarTipoCentral',
        data: { idEmpresa: $('#hfEmpresa').val() },
        success: function (aData) {
            $('#tipocentral').html(aData);
            $('#cbTipoCentral').multipleSelect({
                width: '150px',
                filter: true,
                onClose: function (view) {
                    cargarTipoRecurso();
                }
            });
            cargarTipoRecurso();
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function cargarTipoRecurso() {
    var central = $('#cbTipoCentral').multipleSelect('getSelects');

    if (central == "[object Object]") central = "-1";
    $('#hfTipoCentral').val(central);

    $.ajax({
        type: 'POST',
        async: false,
        url: controlador + 'CargarTipoRecursoXTipo',
        data: { idTipoCentral: $('#hfTipoCentral').val() },
        success: function (aData) {
            $('#cbTipoRecursos').html(aData);
            $('#cbTipoRecurso').multipleSelect({
                width: '150px',
                filter: true,
                onClose: function (view) {
                    //$('#listado').html("");
                    //if ($('#cbTipoRecurso').val() == 5) //if ($(this).val() == 5)
                    //    cargarCentral();
                    //if ($('#cbTipoRecurso').val() == 4) //if ($(this).val() == 4)
                    //{ mostrarOcultarCentral(0); }
                    generarReporte();
                }
            });
            $('#cbTipoRecurso').multipleSelect('checkAll');
            generarReporte();
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function generarReporte() {
    $("#reporte").hide();
    $("#listadoHidro").hide();
    $("#reporte").html('');
    $("#listadoHidro").html('');
    $("#btnGraficoDiagramaCarga").hide();
    $("#btnGraficoParticipacionTipoRecurso").hide();
    $("#btnGraficoCentralRER").hide();
    $("#btnGraficoTipoGeneracionRER").hide();

    var fcdesde = $('#txtFechaInicio').val();
    var fchasta = $('#txtFechaFin').val();

    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    if (empresa == "[object Object]") empresa = "-1";
    $('#hfEmpresa').val(empresa);

    var tipoCentral = $('#cbTipoCentral').multipleSelect('getSelects');
    if (tipoCentral == "[object Object]") tipoCentral = "-1";
    $('#hfTipoCentral').val(tipoCentral);

    var recurso = $('#cbTipoRecurso').multipleSelect('getSelects');
    if (recurso == "[object Object]") recurso = "-1";
    $('#hfRecurso').val(recurso);

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaRecursosEnergeticosSEIN',
        data: {
            idempresa: $('#hfEmpresa').val(),
            idtipocentral: $("#hfTipoCentral").val(),
            idtiporecurso: $('#hfRecurso').val(),
            fcdesde: fcdesde, fchasta: fchasta,
            soloRecursos: GetValorCheckRecurso()
        },
        success: function (listaModel) {
            $("#reporte").show();

            if (GetValorCheckRecurso() == 1) {//RER
                $('#reporte').html(listaModel[0].Resultado);
                $("#reporte").css("width", anchoListado + "px");

                disenioGraficoCentralRER('idVistaGrafica3', listaModel[1]);
                disenioGraficoParticipacionTipoRecurso('idVistaGrafica4', listaModel[2]);

                $("#btnGraficoCentralRER").show();
                $("#btnGraficoTipoGeneracionRER").show();
            } else {
                $('#reporte').html(listaModel[0].Resultado);
                $("#reporte").css("width", anchoListado + "px");
                $("#listadoHidro").show();
                $('#listadoHidro').html(listaModel[0].Resultado2);

                disenioGraficoDiagramaCarga('idVistaGrafica1', listaModel[1]);
                disenioGraficoParticipacionTipoRecurso('idVistaGrafica2', listaModel[2]);

                $("#btnGraficoDiagramaCarga").show();
                $("#btnGraficoParticipacionTipoRecurso").show();
            }
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

///////////////////////////////////////////////////////////////////////////////////
/// REPORTE POTENCIA GENERADA POR TIPO DE RECURSO
///////////////////////////////////////////////////////////////////////////////////
function disenioGraficoDiagramaCarga(idVista, result) {
    var Varserie = [[]];
    var serieName = result.Grafico.SeriesName;

    var opcion = {
        chart: {
            type: 'area'
        },
        title: {
            text: result.Grafico.TitleText
        },
        subtitle: {
            title: {
                enabled: true,
                text: ''
            }
        },
        xAxis: {
            categories: serieName
        },
        yAxis: {
            title: {
                text: 'MW'
            },
            labels: {
                formatter: function () {
                    return this.value;
                }
            }
        },
        tooltip: {
            pointFormat: '<span style="color:{series.color}">{series.name}</span>: ({point.y:.1f})<br/>',
            split: true
        },
        plotOptions: {
            area: {
                stacking: 'normal',
                lineColor: '#ffffff',
                lineWidth: 1,
                marker: {
                    lineWidth: 1,
                    lineColor: '#ffffff'
                }
            }
        },
        series: []
    };

    for (i = 0; i < result.Grafico.Series.length; i++) {
        Varserie[i] = [];
        $.each(result.Grafico.Series[i].Data, function (key, item) {
            var seriePoint = [];
            seriePoint.push(item.Y);
            Varserie[i].push(seriePoint);
        });
        opcion.series.push({
            name: result.Grafico.Series[i].Name,
            data: Varserie[i]
        });
    }

    Highcharts.chart(idVista, opcion);
}

function disenioGraficoParticipacionTipoRecurso(idGrafico, result) {
    var dataGrafico = [];
    var series = result.Grafico.Series;

    var tituloGrafico = result.Grafico.TitleText;
    var leyendaFuente = result.Grafico.Subtitle;

    for (var i = 0; i < series.length; i++) {
        var g = series[i];
        dataGrafico.push({
            name: g.Name,
            y: g.Porcentaje,
            color: g.Color
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
        subtitle: {
            text: leyendaFuente,
            align: 'center',
            x: -10
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
            name: 'Porcentaje',
            colorByPoint: true,
            data: dataGrafico
        }]
    });
}

///////////////////////////////////////////////////////////////////////////////////
/// GENERACIÓN ELÉCTRICA DE LAS CENTRALES RER (MW)
///////////////////////////////////////////////////////////////////////////////////
function disenioGraficoCentralRER(idGrafico, result) {
    var dataGrafico = [];
    var series = result.Grafico.Series;

    for (var i = 0; i < series.length; i++) {
        var g = series[i];
        dataGrafico.push({
            name: g.Name,
            y: g.Acumulado,
            color: g.Color
        });
    }

    var tituloGrafico = result.Grafico.TitleText;
    var tituloY = result.Grafico.YaxixTitle;
    var tituloX = result.Grafico.XAxisTitle;

    Highcharts.chart(idGrafico, {
        chart: {
            type: 'bar'
        },
        title: {
            text: tituloGrafico
        },
        xAxis: {
            type: 'category',
            title: {
                text: tituloX,
                align: 'high'
            }
        },
        yAxis: {
            title: {
                text: tituloY,
                align: 'high'
            }
        },
        legend: {
            enabled: false
        },
        plotOptions: {
            series: {
                borderWidth: 0,
                dataLabels: {
                    enabled: true,
                    //format: '{point.y:.2f}'
                    formatter: function () {
                        return this.y > 0 ? this.y.toFixed(2) : null;
                    }
                }
            }
        },

        tooltip: {
            headerFormat: '<span style="font-size:11px">{series.name}</span><br>',
            pointFormat: '<span style="color:{point.color}">{point.name}</span>: <b>{point.y:.2f}<br/>'
        },
        series: [{
            name: 'MWh',
            colorByPoint: true,
            data: dataGrafico
        }]
    });
}