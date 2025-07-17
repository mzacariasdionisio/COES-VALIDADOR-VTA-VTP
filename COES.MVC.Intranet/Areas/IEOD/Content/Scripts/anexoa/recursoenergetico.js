var controlador = siteRoot + 'IEOD/AnexoA/';
var USAR_COMBO_TIPOCOMBUSTIBLE = false;
var ancho = 900;

$(function () {
    parametro2 = getTipoLectura48();
    $('input[name=cbLectura48]').change(function () {
        parametro2 = getTipoLectura48();
        generarReporte();
    });

    $('#cbEmpresa').multipleSelect({
        width: '150px',
        filter: true,
        onClose: function (view) {
            cargarCentralxEmpresa();
        }
    });

    ancho = $("#mainLayout").width() > 900 ? $("#mainLayout").width() - 30 : 900;

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

    cargarCentralxEmpresa();
}

function mostrarReporteByFiltros() {
    generarReporte();
}

function GetValorCheckRecurso() {
    var soloRecursos = 2;
    parametro1 = '2';
    if ($("#ckRecursos").prop('checked')) {
        soloRecursos = 1; //3
        parametro1 = '1'; //3
    }

    return soloRecursos;
}

function cargarCentralxEmpresa() {
    var idEmpresa = getEmpresa();
    validacionesxFiltro(1);
    if (resultFiltro) {
        $.ajax({
            type: 'POST',
            url: controlador + 'CargarCentralxEmpresa',
            data: { idEmpresa: idEmpresa },
            success: function (aData) {
                $('#centrales').html(aData);
                $('#cbCentral').multipleSelect({
                    width: '150px',
                    filter: true,
                    onClose: function (view) {
                        cargarTipoCombustible();
                    }
                });
                $('#cbCentral').multipleSelect('checkAll');
                cargarTipoCombustible();
            },
            error: function (err) {
                alert("Ha ocurrido un error");
            }
        });
    }
}

function cargarTipoCombustible() {
    var idCentral = getCentral();

    validacionesxFiltro(1);
    if (resultFiltro) {
        $.ajax({
            type: 'POST',
            url: controlador + 'CargarTipoCombustibleXCentral',
            data: { idCentral: idCentral },
            success: function (aData) {
                $('#cbTipoCombustible').html(aData);
                $('#cbTipoCombustibleXcentral').multipleSelect({
                    width: '150px',
                    filter: true,
                    onClose: function (view) {
                        generarReporte();
                    }
                });
                $('#cbTipoCombustibleXcentral').multipleSelect('checkAll');
                generarReporte();
            },
            error: function (err) {
                alert("Ha ocurrido un error");
            }
        });
    }
}

function generarReporte() {
    $("#listado").hide();
    $("#listadoHidro").hide();
    $("#listado").html('');
    $("#listadoHidro").html('');
    $("#btnGraficoDiagramaCarga").hide();
    $("#btnGraficoParticipacionTipoRecurso").hide();
    $("#btnGraficoCentralRER").hide();
    $("#btnGraficoTipoGeneracionRER").hide();

    var fechaInicio = getFechaInicio();
    var fechaFin = getFechaFin();
    var idEmpresa = getEmpresa();
    var idCentral = getCentral();
    var idTipoRecurso = getTipoCombustibleXcentral();

    validacionesxFiltro(2);
    if (resultFiltro) {
        $.ajax({
            type: 'POST',
            url: controlador + 'CargarListaRecursosEnergeticosSEIN',
            data: {
                tipoDato48: getTipoLectura48(),
                idempresa: idEmpresa,
                idCentral: idCentral,
                idtiporecurso: idTipoRecurso,
                fcdesde: fechaInicio, fchasta: fechaFin,
                soloRecursos: GetValorCheckRecurso()
            },
            success: function (listaModel) {
                $("#listado").show();
                $('.filtro_fecha_desc').html(listaModel[0].FiltroFechaDesc);

                if (GetValorCheckRecurso() == 1) {//RER
                    $('#listado').html(listaModel[0].Resultado);

                    var anchoReporte = $('#reporte').width();
                    $("#resultado").css("width", (anchoReporte > ancho ? ancho : anchoReporte) + "px");

                    if (listaModel[0].NRegistros > 0) {
                        $('#reporte').dataTable({
                            "scrollX": true,
                            "scrollCollapse": true,
                            "sDom": 't',
                            "ordering": false,
                            paging: false,
                            fixedColumns: {
                                leftColumns: 1,
                                rightColumns: 1
                            }
                        });
                    } else {
                        $('#reporte').dataTable({
                            "scrollX": true,
                            "scrollCollapse": true,
                            "sDom": 't',
                            "ordering": false,
                            paging: false
                        });
                    }

                    disenioGraficoCentralRER('idVistaGrafica3', listaModel[1]);
                    disenioGraficoParticipacionTipoRecurso('idVistaGrafica4', listaModel[2]);

                    $("#btnGraficoCentralRER").show();
                    $("#btnGraficoTipoGeneracionRER").show();
                } else {
                    $('#listado').html(listaModel[0].Resultado);
                    $("#listado").css("width", ancho + "px");
                    $("#listadoHidro").show();
                    $('#listadoHidro').html(listaModel[0].Resultado2);

                    disenioGraficoDiagramaCarga('idVistaGrafica1', listaModel[1]);
                    disenioGraficoParticipacionTipoRecurso('idVistaGrafica2', listaModel[2]);

                    $("#btnGraficoDiagramaCarga").show();
                    $("#btnGraficoParticipacionTipoRecurso").show();

                    if (listaModel[0].ListaMensaje != null && listaModel[0].ListaMensaje.length > 0) {
                        var html = "";
                        html += '<ul>';
                        for (var i = 0; i < listaModel[0].ListaMensaje.length; i++) {
                            html += `<li>${listaModel[0].ListaMensaje[i]}</li>`;
                        }
                        html += '</ul>';
                        $("#html_mensaje_fenerg").html(html);
                    }

                }
            },
            error: function (err) {
                alert("Ha ocurrido un error");
            }
        });
    }
}

//validaciones de los filtros de busqueda
function validacionesxFiltro(valor) {
    var idEmpresa = getEmpresa();
    var idCentral = getCentral();
    var idTCombustibleXcentral = getTipoCombustibleXcentral();

    var arrayUbicacion = arrayUbicacion || [];

    if (valor == 1)
        arrayUbicacion.push({ id: idEmpresa, mensaje: "Seleccione la opcion Empresa" }); //, { id: idCentral, mensaje: "Seleccione la opcion Central" });
    else
        //if ((valor == 2)) {
        //    arrayUbicacion.push({ id: idEmpresa, mensaje: "Seleccione la opcion Empresa" }, { id: idCentral, mensaje: "Seleccione la opcion Central" });
        //}
        //else {
        arrayUbicacion.push({ id: idEmpresa, mensaje: "Seleccione la opcion Empresa" }, { id: idCentral, mensaje: "Seleccione la opcion Central" }, { id: idTCombustibleXcentral, mensaje: "Seleccione la opcion Tipo Combustible" });
    //}

    validarFiltros(arrayUbicacion);
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
            color: g.Color,
            sliced: i == 0,
            selected: i == 0
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
            type: 'pie',
            options3d: {
                enabled: true,
                alpha: 45,
                beta: 0
            }
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
                showInLegend: true,
                depth: 35,
                slicedOffset: 50
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
