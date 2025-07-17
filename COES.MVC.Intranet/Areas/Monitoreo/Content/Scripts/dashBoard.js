var controlador = siteRoot + 'Monitoreo/Reporte/';
var ancho = 1200;

$(function () {
    $('#tab-container').easytabs({
        animate: false
    });

    ///////////////////////////
    /// Combo de Empresas Indicadores
    ///////////////////////////
    var fecha = $('#hfMes').val();
    for (var i = 1 ; i <= 6 ; i++) {
        if (i == 5) {
            $('#cbEmpresaIMM-' + i).multipleSelect({
                width: '200px',
                filter: true,
                onClose: function (view) {
                    FiltroBarrasIndicador(5);
                }
            });
        }
        else if (i == 6) {
            $('#cbEmpresaIMM-' + i).multipleSelect({
                width: '200px',
                filter: true,
                onClose: function (view) {
                    FiltroBarrasIndicador(6);
                }
            });
        }
        else {
            $('#cbEmpresaIMM-' + i).multipleSelect({
                width: '200px',
                filter: true,
                onClose: function (view) {
                }
            });
        }
        /// CAMBIO FECHA - DIA ,  SEMANA Y MES 
        $('#divDiaMesAnioIndicador' + i).show()
        $('#divDiaSemanaIndicador' + i).hide()
        $('#divMesAnioIndicador' + i).hide()
        $('#divSemanaIndicador' + i).hide()
        //INDICADOR 1
        $('#txtPeriodoDiaIndicador' + i).Zebra_DatePicker({
            format: 'd/m/Y',
            direction: fecha,
            onSelect: function () {
            }
        });
        $('#txtPeriodoMesIndicador' + i).Zebra_DatePicker({
            format: 'm/Y',
            direction: fecha,
            onSelect: function () {
            }
        });
        $('#txtPeriodoSemanaIndicador' + i).Zebra_DatePicker({
            format: 'Y',
            direction: fecha,
            onSelect: function () {
            }
        });
    }
    $('#cbBarraIndicador5').multipleSelect('checkAll');
    $('#cbBarraIndicador6').multipleSelect('checkAll');

    ///////////////////////////
    /// CAMBIO DE PERIODOS
    ///////////////////////////

    // INDICADOR 1
    var fechaPeriodo = $("#txtPeriodoDiaIndicador1").val();
    $('#cbPeriodoIndicador1').change(function () {

        fechaIngreso = fechaPeriodo.split('/');
        if ($("#cbPeriodoIndicador1").val() == '01') {
            $('#divDiaMesAnioIndicador1').show()
            $('#divDiaSemanaIndicador1').hide()
            $('#divMesAnioIndicador1').hide()
            $('#divSemanaIndicador1').hide()
        }
        else if ($("#cbPeriodoIndicador1").val() == '02') {
            fechaR = fechaIngreso[2];
            $('#divSemanaIndicador1').show()
            $('#txtPeriodoSemanaIndicador1').val(fechaR);
            $('#divDiaMesAnioIndicador1').hide()
            $('#divSemanaIndicador1').show()
            $('#divMesAnioIndicador1').hide()
            $('#txtPeriodoMesIndicador1').val(fechaR);
            cargarSemanaIndicador(1);
        }
        else if ($("#cbPeriodoIndicador1").val() == '03') {
            fechaR = fechaIngreso[0] + '/' + fechaIngreso[2];
            $('#divDiaMesAnioIndicador1').hide()
            $('#divSemanaIndicador1').hide()
            $('#divMesAnioIndicador1').show()
            $('#divSemanaIndicador1').hide()
            $('#txtPeriodoMesIndicador1').val(fechaR);
        }
    })
    // INDICADOR 2
    var fechaPeriodo = $("#txtPeriodoDiaIndicador2").val();
    $('#cbPeriodoIndicador2').change(function () {

        fechaIngreso = fechaPeriodo.split('/');
        if ($("#cbPeriodoIndicador2").val() == '01') {
            $('#divDiaMesAnioIndicador2').show()
            $('#divDiaSemanaIndicador2').hide()
            $('#divMesAnioIndicador2').hide()
            $('#divSemanaIndicador2').hide()
        }
        else if ($("#cbPeriodoIndicador2").val() == '02') {
            fechaR = fechaIngreso[2];
            $('#divSemanaIndicador2').show()
            $('#txtPeriodoSemanaIndicador2').val(fechaR);
            $('#divDiaMesAnioIndicador2').hide()
            $('#divSemanaIndicador2').show()
            $('#divMesAnioIndicador2').hide()
            $('#txtPeriodoMesIndicador2').val(fechaR);
            cargarSemanaIndicador(2);
        }
        else if ($("#cbPeriodoIndicador2").val() == '03') {
            fechaR = fechaIngreso[0] + '/' + fechaIngreso[2];
            $('#divDiaMesAnioIndicador2').hide()
            $('#divSemanaIndicador2').hide()
            $('#divMesAnioIndicador2').show()
            $('#divSemanaIndicador2').hide()
            $('#txtPeriodoMesIndicador2').val(fechaR);
        }
    })
    // INDICADOR 3
    var fechaPeriodo = $("#txtPeriodoDiaIndicador3").val();
    $('#cbPeriodoIndicador3').change(function () {

        fechaIngreso = fechaPeriodo.split('/');
        if ($("#cbPeriodoIndicador3").val() == '01') {
            $('#divDiaMesAnioIndicador3').show()
            $('#divDiaSemanaIndicador3').hide()
            $('#divMesAnioIndicador3').hide()
            $('#divSemanaIndicador3').hide()
        }
        else if ($("#cbPeriodoIndicador3").val() == '02') {
            fechaR = fechaIngreso[2];
            $('#divSemanaIndicador3').show()
            $('#txtPeriodoSemanaIndicador3').val(fechaR);
            $('#divDiaMesAnioIndicador3').hide()
            $('#divSemanaIndicador3').show()
            $('#divMesAnioIndicador3').hide()
            $('#txtPeriodoMesIndicador3').val(fechaR);
            cargarSemanaIndicador(3);
        }
        else if ($("#cbPeriodoIndicador3").val() == '03') {
            fechaR = fechaIngreso[0] + '/' + fechaIngreso[2];
            $('#divDiaMesAnioIndicador3').hide()
            $('#divSemanaIndicador3').hide()
            $('#divMesAnioIndicador3').show()
            $('#divSemanaIndicador3').hide()
            $('#txtPeriodoMesIndicador3').val(fechaR);
        }
    })
    // INDICADOR 4
    var fechaPeriodo = $("#txtPeriodoDiaIndicador4").val();
    $('#cbPeriodoIndicador4').change(function () {

        fechaIngreso = fechaPeriodo.split('/');
        if ($("#cbPeriodoIndicador4").val() == '01') {
            $('#divDiaMesAnioIndicador4').show()
            $('#divDiaSemanaIndicador4').hide()
            $('#divMesAnioIndicador4').hide()
            $('#divSemanaIndicador4').hide()
        }
        else if ($("#cbPeriodoIndicador4").val() == '02') {
            fechaR = fechaIngreso[2];
            $('#divSemanaIndicador4').show()
            $('#txtPeriodoSemanaIndicador4').val(fechaR);
            $('#divDiaMesAnioIndicador4').hide()
            $('#divSemanaIndicador4').show()
            $('#divMesAnioIndicador4').hide()
            $('#txtPeriodoMesIndicador4').val(fechaR);
            cargarSemanaIndicador(4);
        }
        else if ($("#cbPeriodoIndicador4").val() == '03') {
            fechaR = fechaIngreso[0] + '/' + fechaIngreso[2];
            $('#divDiaMesAnioIndicador4').hide()
            $('#divSemanaIndicador4').hide()
            $('#divMesAnioIndicador4').show()
            $('#divSemanaIndicador4').hide()
            $('#txtPeriodoMesIndicador4').val(fechaR);
        }
    })
    // INDICADOR 5
    var fechaPeriodo = $("#txtPeriodoDiaIndicador5").val();
    $('#cbPeriodoIndicador5').change(function () {

        fechaIngreso = fechaPeriodo.split('/');
        if ($("#cbPeriodoIndicador5").val() == '01') {
            $('#divDiaMesAnioIndicador5').show()
            $('#divDiaSemanaIndicador5').hide()
            $('#divMesAnioIndicador5').hide()
            $('#divSemanaIndicador5').hide()
        }
        else if ($("#cbPeriodoIndicador5").val() == '02') {
            fechaR = fechaIngreso[2];
            $('#divSemanaIndicador5').show()
            $('#txtPeriodoSemanaIndicador5').val(fechaR);
            $('#divDiaMesAnioIndicador5').hide()
            $('#divSemanaIndicador5').show()
            $('#divMesAnioIndicador5').hide()
            $('#txtPeriodoMesIndicador5').val(fechaR);
            cargarSemanaIndicador(5);
        }
        else if ($("#cbPeriodoIndicador5").val() == '03') {
            fechaR = fechaIngreso[0] + '/' + fechaIngreso[2];
            $('#divDiaMesAnioIndicador5').hide()
            $('#divSemanaIndicador5').hide()
            $('#divMesAnioIndicador5').show()
            $('#divSemanaIndicador5').hide()
            $('#txtPeriodoMesIndicador5').val(fechaR);
        }
    })
    // INDICADOR 6
    var fechaPeriodo = $("#txtPeriodoDiaIndicador6").val();
    $('#cbPeriodoIndicador6').change(function () {

        fechaIngreso = fechaPeriodo.split('/');
        if ($("#cbPeriodoIndicador6").val() == '01') {
            $('#divDiaMesAnioIndicador6').show()
            $('#divDiaSemanaIndicador6').hide()
            $('#divMesAnioIndicador6').hide()
            $('#divSemanaIndicador6').hide()
        }
        else if ($("#cbPeriodoIndicador6").val() == '02') {
            fechaR = fechaIngreso[2];
            $('#divSemanaIndicador6').show()
            $('#txtPeriodoSemanaIndicador6').val(fechaR);
            $('#divDiaMesAnioIndicador6').hide()
            $('#divSemanaIndicador6').show()
            $('#divMesAnioIndicador6').hide()
            $('#txtPeriodoMesIndicador6').val(fechaR);
            cargarSemanaIndicador(6);
        }
        else if ($("#cbPeriodoIndicador6").val() == '03') {
            fechaR = fechaIngreso[0] + '/' + fechaIngreso[2];
            $('#divDiaMesAnioIndicador6').hide()
            $('#divSemanaIndicador6').hide()
            $('#divMesAnioIndicador6').show()
            $('#divSemanaIndicador6').hide()
            $('#txtPeriodoMesIndicador6').val(fechaR);
        }
    })


    ///////////////////////////
    /// MULTISELECT EMPRESAS INDICADORES
    ///////////////////////////
    $('#cbEmpresaIMM-1').multipleSelect('checkAll');
    $('#cbEmpresaIMM-2').multipleSelect('checkAll');
    $('#cbEmpresaIMM-3').multipleSelect('checkAll');
    $('#cbEmpresaIMM-4').multipleSelect('checkAll');
    $('#cbEmpresaIMM-5').multipleSelect('checkAll');
    $('#cbEmpresaIMM-6').multipleSelect('checkAll');

    ///////////////////////////
    /// Botones Indicadores 
    ///////////////////////////
    $('#btnProcesarIndicador1').click(function () {
        generaciondashboard(1);
    });

    $('#btnProcesarIndicador2').click(function () {
        generaciondashboard(2);
    });

    $('#btnProcesarIndicador3').click(function () {
        generaciondashboard(3);
    });

    $('#btnProcesarIndicador4').click(function () {
        generaciondashboard(4);
    });

    $('#btnProcesarIndicador5').click(function () {
        generaciondashboard(5);
    });

    $('#btnProcesarIndicador6').click(function () {
        generaciondashboard(6);
    });

    FiltroBarrasIndicador(5);
    FiltroBarrasIndicador(6);

});


///////////////////////////
/// METTODO GENERAL REPORTE DASHBOARD
///////////////////////////
function generaciondashboard(indicador) {

    var empresa = $('#cbEmpresaIMM-' + indicador).multipleSelect('getSelects');
    if (empresa == "[object Object]" || empresa.length == 0) empresa = "-1";
    var fechaDia = $('#txtPeriodoDiaIndicador' + indicador).val();

    var barra = $('#cbBarraIndicador' + indicador).multipleSelect('getSelects');
    if (barra == "[object Object]" || barra.length == 0) barra = [-1];


    var fechaSemana = getSemanaIni(indicador);
    var fechaMes = $('#txtPeriodoMesIndicador' + indicador).val();
    var periodo = $('#cbPeriodoIndicador' + indicador).val();


    $.ajax({
        url: controlador + "ConstruirDashboardIndicador",
        data: {
            empresa: empresa.join(','),
            fechaDia: fechaDia,
            fechaSemana: fechaSemana,
            fechaMes: fechaMes,
            periodo: periodo,
            indicador: indicador,
            barra: barra.join(',')
        },
        type: 'POST',
        success: function (result) {

            if (result.Indicador == 1) {
                GraficoMedidorIndicador1(result.Grafico, "divMedidorIndicador1");
                GraficoGrupoDespachoIndicador1(result.GraficoBarra, "divGrupoDespachoIndicador1");
                GraficoColumnasPeriodoMensual(result.GraficoMedidorCurva, "divReporteBarraEmpresaIndicador1");
                $('#divTotalMwIndicador1').html(result.Resultado);

            } else if (result.Indicador == 2) {
                GraficoCurvasIndicador2('divCurvasIndicador2', result.GraficoMedidorCurva);
                GraficoColumnasPeriodoMensual(result.GraficoMedidorCurva2, "divReporteBarraGrafica2");
                GraficoMedidorTotalHHI(result.GraficoCurva, "divReporteHHITotal");
                $('#divResultadoIndicador2').html(result.Resultado);

            } else if (result.Indicador == 3) {
                GraficoIndiceOfertaResidual('divCurvasIndicador3', result.GraficoMedidorCurva);
                GraficoColumnasPeriodoMensual(result.GraficoMedidorCurva2, "divBarraGraficaIndicador3");
                $('#divResultado1Indicador3').html(result.Resultado);

            } else if (result.Indicador == 4) {
                GraficoIndiceOfertaResidual('divCurvasIndicador4', result.GraficoMedidorCurva);
                GraficoColumnasPromedioResidual(result.GraficoMedidorCurva2, "divBarraGraficaIndicador4");
                $('#divResultado1Indicador4').html(result.Resultado);
                $('#divResultado2Indicador4').html(result.Resultado);
            }
            else if (result.Indicador == 5) {

                GraficoCurvasLenerCostoPrecio('panelGraficoIndicador5', result.GraficoMedidorCurva);
            } else if (result.Indicador == 6) {
                GraficoCurvasLenerCostoPrecio('panelGraficoIndicador6', result.GraficoMedidorCurva);
            }

            else if (result.Indicador == -1) {
                alert(' Debe seleccionar una semana');
            }

        },
        error: function (xhr, status) {

            alert(' Ocurio un error en la generación de DashBoard');
        },
        complete: function (xhr, status) {
        }
    });
};

///////////////////////////
/// METODOS GRAFICOS INDICADORES 
///////////////////////////

//MEDIDOR INDICADOR 1
function GraficoMedidorIndicador1(dataResult, content) {
    var data = dataResult;
    try {
        var dataPlot = [];
        for (var i in data.Series) {
            var item = data.Series[i];
            if (item == null) {
                continue;
            }
            dataPlot.push({ from: item.From, to: item.To, color: item.Color, thickness: item.Porcentaje });
        }

        var series = [];
        var total = 0;
        for (var d in data.SerieDataS) {
            var item = data.SerieDataS[d][0];
            var align = (d % 2 == 0) ? "left" : "right";
            total = item.Z;
            series.push({
                name: 'Cuota de mercado',
                color: item.Color,
                data: item.Z,
                tooltip: {
                    valueSuffix: ' %'
                },
                dial: {
                    backgroundColor: item.Color
                },
                showInLegend: true,
                dataLabels: {
                    align: align,
                    enabled: true,
                    color: item.Color,
                    allowOverlap: true
                }
            });
        }
        Highcharts.chart(content, {

            chart: {
                type: 'gauge', shadow: true
            },

            title: {
                text: data.TitleText,
                align: 'left'
            },
            pane: {
                startAngle: 90,
                endAngle: -90,
                background: null
            },
            yAxis: {
                min: data.YaxixMin,
                max: data.YaxixMax,
                lineColor: 'transparent',
                minorTickWidth: 0,
                tickLength: 0,
                tickPosition: 'inside',
                tickPositions: data.SeriesData[0],
                labels: {
                    step: 1,
                    distance: 10
                },
                plotBands: dataPlot
            }, plotOptions: {
                solidgauge: {
                    dataLabels: {
                        y: 5,
                        borderWidth: 0,
                        useHTML: true
                    }
                }
            }, legend: {
                align: 'center',
                verticalAlign: 'top',
            },
            series: [{
                name: 'Porcentaje',
                data: [total],
                dataLabels: {
                    format: '<div style="text-align:center"><span style="font-size:20px;color:' +
                        ((Highcharts.theme && Highcharts.theme.contrastTextColor) || 'blue') + '">{y}</span> ' +
                           '<span style="font-size:20px;color:blue">%</span></div>'
                },
                tooltip: {
                    valueSuffix: '%'
                }

            }]

        });
    }
    catch (err) {
        alert(err);
    }
};

// GRUPO DESPACHO HIDRO Y TERMO  INDICADOR 1
function GraficoGrupoDespachoIndicador1(result, idGrafico) {

    var dataGrafico = [];
    var series = result.Series;

    for (var i = 0; i < series.length; i++) {
        var g = series[i];
        dataGrafico.push({
            name: g.Name,
            y: g.TipoPto,
            color: g.Color
        });
    }

    var tituloGrafico = result.TitleText;

    var tituloY = result.YaxixTitle;
    var tituloX = result.XAxisTitle;

    Highcharts.chart(idGrafico, {
        chart: {
            type: 'bar', shadow: true
        },
        title: {
            text: tituloGrafico
        },
        xAxis: {
            allowDecimals: false,
            type: 'category',
            title: {
                text: tituloX,
                align: 'high'
            },
        },

        yAxis: {
            allowDecimals: false,
            title: {
                text: '',
                align: 'high'
            }
        },
        legend: {
            enabled: false,
            allowDecimals: false,
        },
        plotOptions: {
            series: {
                borderWidth: 0,
                dataLabels: {
                    enabled: true
                }
            }
        },

        tooltip: {
            valueDecimals: 0
        },
        series: [{
            colorByPoint: true,
            data: dataGrafico,
            name: 'Total'
        }]
    });
}

// COLUMNA  GRAFICAS INDICADOR 1,2 Y 3
function GraficoColumnasPeriodoMensual(data, idGrafico) {

    //obtener data
    var dataCategoria = [];

    var tituloGrafico = data.TitleText;
    var valorxEmpresa = [];
    var tituloFuente1 = data.TituloFuente1;

    var series = [];
    for (var i = 0; i < data.ListaPunto.length; i++) {
        var empresaNombre = data.ListaPunto[i].Empresa;

        var g = data.ListaPunto[i];
        var dataFuente1 = [];
        for (var j = 0; j < g.ListaFuente1.length; j++) {
            var valor = g.ListaFuente1[j];
            dataFuente1.push(valor);
        }
        series.push({
            name: empresaNombre,
            data: dataFuente1,
            showInLegend: true
        });
    }

    //Generar grafica
    Highcharts.chart(idGrafico, {
        chart: {
            height: 800,
            zoomType: 'xy',
            type: 'column', shadow: true
        },
        title: {
            text: tituloGrafico
        },

        yAxis: {

            title: {
                text: data.TituloFuente1,
            }
        },

        subtitle: {
            text: ''
        },
        xAxis: [{
            categories: data.CategoriaFecha,
            crosshair: true,
            min: 0,
            minPadding: 0,
            maxPadding: 0,
            startOnTick: false,
            endOnTick: false,
            scrollbar: {
                enabled: true
            },
        }],
        title: {
            text: data.TituloGrafico
        },
        legend: {
            x: 0,
            y: 10
        },

        rangeSelector: {
            selected: 1
        },

        plotOptions: {
            series: {
                label: {
                    connectorAllowed: false
                }
            }
        },


        series: series
    });
}

function getSemanaIni(indicador) {
    var semana = "";
    var cbsemana = $("#cbSemanasIndicador" + indicador).val();

    if (cbsemana == "0" || cbsemana == "" || cbsemana == undefined) {
        //  semana = $("#hfSemanaIniIndicador" + indicador).val();
        semana = "-1"
    }
    else {
        semana = cbsemana;
    }

    if (semana == "0" || semana == "") {
        semana = "-1";
    }
    //anho = $('#txtPeriodoSemanaIndicador' + indicador).val()

    //if (semana !== '-1' && anho !== undefined) {
    //    semana = anho.toString() + semana;
    //} else {
    //    semana = '-1';
    //}

    return semana;
}

//MEDIDOR INDICADOR 2
function GraficoCurvasIndicador2(idGrafico, data) {
    //obtener data
    var dataCategoria = [];
    var dataFuente1 = [];
    var dataFuente2 = [];
    var dataFuente3 = [];
    var dataFuente4 = [];
    var tituloGrafico = data.TituloGrafico;
    var tituloFuente1 = data.TituloFuente1;
    var tituloFuente2 = data.TituloFuente2;

    var leyendaFuente1 = data.LeyendaFuente1;
    var leyendaFuente2 = 'Límite 1';
    var leyendaFuente3 = 'Límite 2';
    var leyendaFuente4 = 'Límite 3';

    for (var i = 0; i < data.ListaPunto.length; i++) {
        var g = data.ListaPunto[i];
        dataCategoria.push(g.FechaString);
        dataFuente1.push(g.ValorFuente1);
        dataFuente2.push(999);

        dataFuente3.push(1800);
        dataFuente4.push(1801);

    }

    //Generar grafica
    Highcharts.chart(idGrafico, {
        chart: {
            zoomType: 'xy', shadow: true
        },
        title: {
            text: tituloGrafico
        },
        subtitle: {
            text: ''
        },
        xAxis: [{
            categories: dataCategoria,
            crosshair: true,
            min: 0,
            labels: {
                rotation: 60
            }
        }],
        yAxis: [{ // Primary yAxis
            title: {
                text: tituloFuente1,
                style: {
                    color: 'blue'
                }
            },
            labels: {
                format: '{value}',
                style: {
                    color: 'blue'
                }
            },
            min: 0
        }],
        tooltip: {
            shared: true
        },
        legend: {
            layout: 'horizontal',
            align: 'center',
            verticalAlign: 'top',
            y: 40,
            floating: false,
            backgroundColor: (Highcharts.theme && Highcharts.theme.legendBackgroundColor) || '#FFFFFF'
        },
        plotOptions: {
            spline: {
                lineWidth: 4,
                states: {
                    hover: {
                        lineWidth: 5
                    }
                },
                marker: {
                    enabled: false
                }
            },

        },
        series: [{
            name: leyendaFuente1,
            type: 'line',
            data: dataFuente1,
            color: 'blue',
        }, {
            name: leyendaFuente2,
            type: 'spline',
            data: dataFuente2,
            color: 'orange'
        }

        , {
            name: leyendaFuente3,
            type: 'spline',
            data: dataFuente3,
            color: 'black'
        }

      , {
          name: leyendaFuente4,
          type: 'spline',
          data: dataFuente4,
          color: 'yellow'
      }

        ]
    });
}

//BARRAS GRAFICA INDICADOR 2
function GraficoBarraEmpresaIndicador2(result, idGrafico) {
    var opcion;

    var json = result.Series;
    var jsondata = [];
    var indice = 3;
    for (var i in json) {
        var jsonValor = [];
        var jsonLista = json[i].Data;
        for (var j in jsonLista) {
            jsonValor.push(jsonLista[j].Y);
        }
        jsondata.push({
            name: json[i].Name,
            //type: json[i].Type,
            //yAxis: json[i].YAxis,
            data: jsonValor,
            color: json[i].Color,
            //index: indice
        });
        indice--;
    }

    opcion = {
        chart: {
            type: 'column', shadow: true
        },
        title: {
            text: result.TitleText
        },

        xAxis: {
            categories: result.XAxisCategories,
            type: 'datetime'
        },
        yAxis: {
            title: {
                text: result.YaxixTitle
            },
            labels: {
                formatter: function () {
                    return this.value;
                }
            }
        },
        tooltip: {
            pointFormat: 'Producción <b>{point.y:,.0f}</b><br/> en el año {series.name}'
        },
        plotOptions: {
            area: {
                stacking: 'normal',
                pointStart: 1,
                marker: {
                    enabled: false,
                    symbol: 'circle',
                    radius: 2,
                    states: {
                        hover: {
                            enabled: true
                        }
                    }
                }
            }
        },
        series: jsondata
    };


    $('#' + idGrafico).highcharts(opcion);
}

//MEDIDOR GRAFICO INDICADOR 2
function GraficoMedidorTotalHHI(dataResult, content) {
    var data = dataResult;
    try {
        var dataPlot = [];
        for (var i in data.Series) {
            var item = data.Series[i];
            if (item == null) {
                continue;
            }
            dataPlot.push({ from: item.From, to: item.To, color: item.Color, thickness: item.Porcentaje });
        }

        var series = [];
        var total = 0;
        for (var d in data.SerieDataS) {
            var item = data.SerieDataS[d][0];
            var align = (d % 2 == 0) ? "left" : "right";
            total = item.Z;
            series.push({
                name: 'Cuota de mercado',
                color: item.Color,
                data: item.Z,

                dial: {
                    backgroundColor: item.Color
                },
                showInLegend: true,
                dataLabels: {
                    align: align,
                    enabled: true,
                    color: item.Color,
                    allowOverlap: false,
                    allowOverlap: true
                }
            });
        }
        Highcharts.chart(content, {

            chart: {
                type: 'gauge', shadow: true
            },

            title: {
                text: data.TitleText
            },
            pane: {
                startAngle: 90,
                endAngle: -90,
                background: null
            },
            yAxis: {
                min: data.YaxixMin,
                max: data.YaxixMax,
                lineColor: 'transparent',
                minorTickWidth: 0,
                tickLength: 0,
                tickPosition: 'inside',
                tickPositions: data.SeriesData[0],
                labels: {
                    step: 1,
                    distance: 10
                },
                plotBands: dataPlot,
                labels: {
                    formatter: function () {
                        return this.value;
                    }
                }

            }, plotOptions: {
                solidgauge: {
                    dataLabels: {
                        y: 5,
                        borderWidth: 0,
                        useHTML: true
                    }
                }
            }, legend: {
                align: 'center',
                verticalAlign: 'top',
            },
            series: [{
                name: 'HHI',
                data: [total],
                dataLabels: {
                    format: '<div style="text-align:center"><span style="font-size:20px;color:' +
                        ((Highcharts.theme && Highcharts.theme.contrastTextColor) || 'blue') + '">{y} <br> </span> ' +
                           '<span style="font-size:20px;color:blue">HHI</span></div>'
                }

            }]

        });
    }
    catch (err) {
        alert(err);
    }
};

//MEDIDOR INDICADOR 3 y 4
function GraficoIndiceOfertaResidual(idGrafico, data) {

    //obtener data
    var dataCategoria = [];

    var tituloGrafico = data.TituloGrafico;
    var valorxEmpresa = [];
    var tituloFuente1 = data.TituloFuente1;

    var series = [];
    for (var i = 0; i < data.ListaPunto.length; i++) {
        var empresaNombre = data.ListaPunto[i].Empresa;

        var g = data.ListaPunto[i];
        var dataFuente1 = [];
        for (var j = 0; j < g.ListaFuente1.length; j++) {
            var valor = g.ListaFuente1[j];
            dataFuente1.push(valor);
        }
        series.push({
            name: empresaNombre,
            data: dataFuente1
        });
    }

    //Generar grafica
    Highcharts.chart(idGrafico, {
        chart: {
            zoomType: 'xy',
            height: 400, shadow: true
        },

        legend: {
            align: 'right',
            verticalAlign: 'top',
            layout: 'vertical',
            x: 0,
            y: 0
        },

        title: {
            text: tituloGrafico
        },
        subtitle: {
            text: ''
        },
        xAxis: [{
            categories: data.CategoriaFecha,
            crosshair: true,
            min: 0
        }],
        yAxis: [{ // Primary yAxis
            title: {
                text: tituloFuente1,
                style: {
                    color: 'blue'
                }
            },
            labels: {
                format: '{value}',
                style: {
                    color: 'blue'
                }
            },
            min: 0
        }],

        plotOptions: {
            area: {
                stacking: 'normal',
                pointStart: 1,
                marker: {
                    enabled: false,
                    symbol: 'circle',
                    radius: 2,
                    states: {
                        hover: {
                            enabled: true
                        }
                    }
                }
            }
        },

        series: series
    });
}

//BARRAS GRAFICA INDICADOR 4
function GraficoColumnasPromedioResidual(result, idGrafico) {
    var opcion;

    var series = [];
    for (var i = 0; i < result.ListaPunto.length; i++) {
        var g = result.ListaPunto[i];

        var dataFuente1 = [];
        for (var j = 0; j < g.ListaFuente1.length; j++) {
            var valor = g.ListaFuente1[j];
            dataFuente1.push(valor);
        }

        series.push({
            name: 'Promedio RSI',
            data: dataFuente1
        });
    }

    opcion = {
        chart: {
            type: 'line',
            zoomType: 'xy', shadow: true
        },
        title: {
            text: result.TituloGrafico
        },

        xAxis: {
            categories: result.NombreEmpresa
        },
        yAxis: {

            labels: {
                formatter: function () {
                    return this.value;
                }
            }
        },

        legend: {

            y: 80


        },
        plotOptions: {
            line: {
                dataLabels: {
                    enabled: true
                },
                enableMouseTracking: true
            },
            states: {
                hover: {
                    enabled: true
                }
            }
        },
        series: series
    };


    $('#' + idGrafico).highcharts(opcion);
}

//GRAFICOS CURVAS  INDICADOR 5 y 6 
function GraficoCurvasLenerCostoPrecio(idGrafico, data) {

    //obtener data

    var tituloGrafico = data.TituloGrafico;
    var tituloFuente1 = data.TituloFuente1;
    var series = [];



    for (var i = 0; i < data.ListaPunto.length; i++) {
        var empresaNombre = data.ListaPunto[i].Empresa;

        var g = data.ListaPunto[i];
        var dataFuente1 = [];
        for (var j = 0; j < g.ListaFuente1.length; j++) {
            var valor = g.ListaFuente1[j];
            dataFuente1.push(valor);
        }
        series.push({
            name: empresaNombre,
            data: dataFuente1
        });
    }

    //Generar grafica
    Highcharts.chart(idGrafico, {
        chart: {
            height: 700,
            type: 'line',
            zoomType: 'xy', shadow: true
        },


        title: {
            text: tituloGrafico
        },

        xAxis: {
            categories: data.CategoriaFecha,
            minPadding: 0,
            maxPadding: 0,
            startOnTick: false,
            endOnTick: false,

            labels: {
                format: '{value}:00'
            }
        }
        ,
        yAxis: [{
            title: {
                text: tituloFuente1,
                style: {
                    color: 'blue'
                }
            },
            labels: {
                style: {
                    color: 'blue'
                }
            },
            min: -4,
            y: 200

        }],

        plotOptions: {
            area: {
                stacking: 'normal',
                pointStart: 1,
                marker: {
                    enabled: false,
                    symbol: 'circle',
                    radius: 2,
                    states: {
                        hover: {
                            enabled: true
                        }
                    }
                }
            }
        }
 ,
        series: series


    });
}

// FILTRO DE BARRAS INDICADOR 5 Y 6 
function FiltroBarrasIndicador(indicador) {
    var empresa = $('#cbEmpresaIMM-' + indicador).multipleSelect('getSelects');
    var fechaDia = $('#txtPeriodoDiaIndicador' + indicador).val();
    if (empresa == "[object Object]" || empresa.length == 0) empresa = "-1";

    $.ajax({
        type: 'POST',
        url: controlador + 'ComboBarra',
        data: {
            empresa: empresa.join(','),
            fechaDia: fechaDia,
            indicador: indicador
        },
        success: function (aData) {
            $('#divBarraIndicador' + indicador).html(aData);
            $('#cbBarraIndicador' + indicador).multipleSelect({
                width: '150px',
                filter: true,
                onClose: function (view) {
                    //   generaciondashboard(indicador);
                }
            });
            $('#cbBarraIndicador' + indicador).multipleSelect('checkAll');
        },
        error: function () {
            alert('Ha ocurrido un error');
        }
    });
}

//COMBO DE CARGA DE SEMANA
function cargarSemanaIndicador(indicador) {

    var anho = $('#txtPeriodoSemanaIndicador' + indicador).val();

    if (anho == '') {
        anho = '2018'
    }

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarSemanas',

        data: {
            idAnho: anho,
            indicador: indicador
        },

        success: function (aData) {
            $('#divSemanaIndicador' + indicador).html(aData);

            $("#cbSemanasIndicador" + indicador).change(function () {
            });
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}