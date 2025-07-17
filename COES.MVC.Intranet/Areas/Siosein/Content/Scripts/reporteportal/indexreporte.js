//variables globales
var controlador = siteRoot + 'Siosein/ReportesOpeSein/'
var idEmpresa = "";
var idCentral = "";
var idTCombustible = "";
var fechaInicio = "";
var fechaFin = "";

$(function () {
    $('#txtFecha').Zebra_DatePicker({ format: 'm Y' });
    $('#Anho').Zebra_DatePicker({ format: 'Y' });
    $('#AnhoIni').Zebra_DatePicker({ format: 'Y' });
    $('#AnhoIniA').Zebra_DatePicker({ format: 'Y' });
    $('#AnhoFinA').Zebra_DatePicker({ format: 'Y' });

    $('#cbComparar').change(function () {
        cargarIni();
        switch ($(this).val()) {
            case "1": $('#idTr1 td').show(); break;
            case "2": $('#idTr2 td').show(); break;
            case "3": $('#idTr3 td').show(); break;
        }
    });
    $('#btnConsultar').click(function () {
        buscarRepor(1, 1);
    });
    $('#btnExportar').click(function () {
        buscarRepor(2);
    });
    $('#btnConsultarA').click(function () {
        buscarRepor(2, 2);
    });
    $('#btnExportarA').click(function () {
    });

    cargarIni();
    cargarMenu();
    $('#idTr1 td').show();
});

function buscarRepor(x, y) {
    var _fec, _trimes, _anio, _anio1, _chk, _mes1, _mes2, _anio2, _anio3;
    var _combo = $('#cbComparar').val();
    if ($('#chkRepo').is(':checked')) { _chk = 1; }
    if ($('#chkGraf').is(':checked')) { _chk = 2; }
    if (x == 2) { _combo = "4"; }
    switch (_combo) {
        case "1": _fec = $('#txtFecha').val(); break;
        case "2": _trimes = $('#cbTrimestre').val(); _anio = $('#Anho').val(); break;
        case "3": _anio1 = $('#AnhoIni').val(); break;
        case "4": _chk = 0; _mes1 = $('#cbMesIni').val(); _mes2 = $('#cbMesFin').val(); _anio2 = $('#AnhoIniA').val(); _anio3 = $('#AnhoFinA').val(); break;
    }
    
    $.ajax({
        type: 'POST',
        url: controlador + "ListaReporteOpeSein",
        data: { combo: _combo, fec: _fec, trimes: _trimes, anio: _anio, anio1: _anio1, chk: _chk, mes1: _mes1, mes2: _mes2, anio2: _anio2, anio3: _anio3 },
        success: function (aData) {
            if (_chk == 1 || _chk == 0) { $('#idGrafico').hide(); $('#listado').show(); $('#listado').html(aData.Resultado); if (aData.nRegistros > 0) { if (aData.NroMostrar != 27 && aData.NroMostrar !=29) { $('#tabla' + aData.NroMostrar).dataTable(); } } }
            if (_chk == 2) {
                if (aData.nRegistros > 0) {
                    $('#listado').hide(); $('#idGrafico').show(); disenioGrafico(aData, 'idGrafico', aData.Titulo, aData.NroMostrar);
                }
            }
            //mostrarPopup();
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function cargarIni() {
    $('#idTr1 td').hide();
    $('#idTr2 td').hide();
    $('#idTr3 td').hide();
}

function cargarMenu() {
    $.ajax({
        type: 'POST',
        url: controlador + 'CargarMenu',
        success: function (aData) {
            $('#menuOpe').html(aData.Resultado);
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function fn_set(x) {
    $.ajax({
        type: 'POST',
        url: controlador + 'Fn_set',
        data: { id: x },
        success: function (aData) { $("#listado").html(''); },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function mostrarPopup() {
    setTimeout(function () {
        $('#popupTabla').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
            modalClose: false
        });
    }, 50);
}
function disenioGrafico(data, grafico, titulo, tip) {
    // Build the chart
    var opcion = null;

    if (tip == 1) {
        var Varserie = [[]];

        opcion = {
            chart: {
                plotBackgroundColor: null,
                plotBorderWidth: null,
                plotShadow: false,
                type: 'pie'
            },
            title: { text: 'T O T A L' },
            tooltip: {
                pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
            },
            plotOptions: {
                pie: {
                    allowPointSelect: true,
                    dataLabels: {
                        enabled: true,
                        format: '<b>{point.name}</b>: {point.percentage:.1f} %',
                        style: {
                            color: (Highcharts.theme && Highcharts.theme.contrastTextColor) || '#2980B9'
                        },
                        connectorColor: 'silver'
                    }
                }
            },
            series: []
        };

        for (i = 0 ; i < data.Lista1.length; i++) {
            Varserie[i] = [];
            Varserie[i].push(data.Lista1[i].text, data.Lista1[i].monto);
        }

        opcion.series.push({
            name: 'Porcentaje',
            colorByPoint: true,
            data: Varserie
        });
    }
    if (tip == 2) {
        var Varserie = [[]];

        opcion = {
            chart: {
                plotBackgroundColor: null,
                plotBorderWidth: null,
                plotShadow: false,
                type: 'pie'
            },
            title: { text: '' },
            subtitle: {
                text: titulo
            },
            tooltip: {
                pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
            },
            plotOptions: {
                pie: {
                    dataLabels: {
                        enabled: true,
                        format: '<b>{point.name}</b>: {point.percentage:.1f} %',
                        style: {
                            color: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black'
                        },
                        connectorColor: 'silver'
                    }
                }
            },
            series: []
        };

        for (i = 0 ; i < data.length; i++) {
            Varserie[i] = [];
            Varserie[i].push(data[i].text, data[i].monto);
        }

        opcion.series.push({
            name: 'Porcentaje',
            colorByPoint: true,
            data: Varserie
        });
    }
    if (tip == 3) {
        var var_textos = [], var_text = [], var_costos = [];
        var seriesX = [], var_seriesx = [];

        opcion = {
            chart: {
                type: 'line'
            },
            title: {
                text: titulo
            },
            xAxis: [],
            yAxis: {
                title: {
                    text: 'S/./kWh'
                }
            },
            legend: {
                enabled: false
            },
            plotOptions: {
                line: {
                    dataLabels: {
                        enabled: false
                    },
                    marker: {
                        enabled: false
                    },

                    enableMouseTracking: true
                }
            }, series: []
        };

        for (i = 0 ; i < data.length; i++) {
            var_textos[i] = [];
            var_textos[i].push(data[i].string1);
        }

        opcion.xAxis.push({
            categories: var_textos
        });

        for (var r = 0; r < data[0].Series.length; r++) {
            var_seriesx = [];

            for (var i = 0 ; i < data.length; i++) {

                var_seriesx.push(data[i].Series[r]);

            }

            var dato = {
                name: "Dia" + (r + 1).toString(),
                data: var_seriesx
            }

            opcion.series.push(dato);
        }
    }
    if (tip == 4) {
        var series1 = [];

        for (var j = 0; j < data.SeriesBarr.length; j++) {
            var sub = [];
            for (var r = 0; r < data.SeriesBarr[j].subSerie.length; r++) {
                sub.push([data.SeriesBarr[j].subSerie[r].string1, data.SeriesBarr[j].subSerie[r].decimal1]);
            }

            var temp = {
                name: data.SeriesBarr[j].string1,
                id: data.SeriesBarr[j].string1,
                data: sub
            }


            series1.push(temp);

        }

        //for (var x = 0; x < data.listaGenerica.length; x++) {
        //    var button = '<input type="submit" value="' + data.listaGenerica[x].string2 + '" id="Boton' + data.listaGenerica[x].string1 + '" name="Numero Parrafos" onclick="verGrafico(' + data.listaGenerica[x].string1 + ')" />';
        //    $('#idBotonera').append(button);
        //}
        opcion = {
            chart: {
                type: 'column'
            },
            title: {
                text: titulo
            },
            subtitle: {
                text: ''
            },
            xAxis: {
                type: 'category'
            },
            yAxis: {
                title: {
                    text: 'S/./kWh'
                }

            },
            legend: {
                enabled: true
            },
            plotOptions: {
                series: {
                    borderWidth: 0,
                    dataLabels: {
                        enabled: true,
                        format: '{point.y:.2f}'
                    }
                }
            },

            tooltip: {
                headerFormat: '<span style="font-size:11px">{series.name}</span><br>',
                pointFormat: '<span style="color:{point.color}">{point.name}</span>: <b>{point.y:.2f}</b><br/>'
            },

            series: series1
        }
    }
    if (tip == 5) {
        var categorias = [];
        var series = [];

        for (var i = 0; i < data.Grafico.XAxisCategories.length; i++) {
            categorias.push(data.Grafico.XAxisCategories[i]);
        }
        for (var i = 0; i < data.Grafico.Series.length; i++) {
            var obj = { name: data.Grafico.Series[i].Name };
            var valores = [];
            for (var j = 0; j < data.Grafico.Series[i].Data.length; j++) {
                valores.push(data.Grafico.Series[i].Data[j].Y);
            }
            var serie = { name: obj.name, data: valores };
            series.push(serie);
        }

        opcion = {
            chart: {
                type: 'column'
            },
            title: {
                text: titulo
            },
            xAxis: {
                categories: categorias
            },
            yAxis: {
                min: 0,
                title: {
                    text: 'GWh'
                },
                stackLabels: {
                    enabled: true,
                    style: {
                        fontWeight: 'bold',
                        color: (Highcharts.theme && Highcharts.theme.textColor) || 'gray'
                    }
                }
            },
            legend: {
                align: 'right',
                x: -30,
                verticalAlign: 'top',
                y: 25,
                floating: true,
                backgroundColor: (Highcharts.theme && Highcharts.theme.background2) || 'white',
                borderColor: '#CCC',
                borderWidth: 1,
                shadow: false
            },
            tooltip: {
                headerFormat: '<b>{point.x}</b><br/>',
                pointFormat: '{series.name}: {point.y}<br/>Total: {point.stackTotal}'
            },
            plotOptions: {
                column: {
                    stacking: 'normal',
                    dataLabels: {
                        enabled: true,
                        color: (Highcharts.theme && Highcharts.theme.dataLabelsColor) || 'white'
                    }
                }
            },
            series: series
        };
    }
    if (tip == 7) {
        var Categorias = [];
        var Series1 = [];
        var Series2 = [];
        var Series3 = [];
        var Series4 = [];

        for (var j = 0; j < data.Categoria.length; j++) {
            Categorias[j] = data.Categoria[j];
        }

        for (var x = 0; x < data.Serie1.length; x++) {
            Series1[x] = data.Serie1[x];
        }
        for (var x = 0; x < data.Serie2.length; x++) {
            Series2[x] = data.Serie2[x];
        }
        for (var x = 0; x < data.Serie3.length; x++) {
            Series3[x] = data.Serie3[x];
        }
        for (var x = 0; x < data.Serie4.length; x++) {
            Series4[x] = data.Serie4[x];
        }

        opcion = {
            chart: {
                type: 'column'
            },
            title: {
                text: titulo
            },
            xAxis: {
                categories: Categorias
            },
            yAxis: {
                title: {
                    text: 'MWh'
                }
            },
            plotOptions: {
                column: {
                    stacking: 'normal'
                }
            },
            series: [{
                name: 'GENERACION',
                color: '#356CAF',
                data: Series1
            }, {
                name: 'CONTRATO LICITACION',
                color: '#85A83D',
                data: Series2
            }, {
                name: 'CONTRATO BILATERAL',
                color: '#AD3330',
                data: Series3
            }, {
                name: 'RETIRO SIN CONTRATO',
                color: '#66669A',
                data: Series4
            }]
        }
    }
    if (tip == 8) {
        var Categorias = [];
        var Series1 = [];
        var Series2 = [];

        for (var j = 0; j < data.Categoria.length; j++) {
            Categorias[j] = data.Categoria[j];
        }

        for (var x = 0; x < data.Serie1.length; x++) {
            Series1[x] = data.Serie1[x];
        }

        opcion = {
            chart: {
                type: 'column'
            },
            title: {
                text: titulo
            },
            yAxis: {
                title: {
                    text: 'GW.h'
                },
                labels: {
                    format: '{value}'
                }
            },
            xAxis: {
                categories: Categorias
            },
            credits: {
                enabled: false
            },
            legend: {
                enabled: false
            },
            series: [{
                name: 'Transferencia',
                color: '#356CAF',
                negativeColor: '#AD3330',

                data: Series1
            }
            ]
        }
    }
    if (tip == 9) {
        var Categorias = [];
        var Series1 = [];
        var Series2 = [];
        var Series3 = [];
        var Series4 = [];
        var Series5 = [];
        var Series6 = [];

        for (var j = 0; j < data.Categoria.length; j++) {
            Categorias[j] = data.Categoria[j];
        }

        for (var x = 0; x < data.Serie1.length; x++) {
            Series1[x] = data.Serie1[x];
        }
        for (var x = 0; x < data.Serie2.length; x++) {
            Series2[x] = data.Serie2[x];
        }
        for (var x = 0; x < data.Serie3.length; x++) {
            Series3[x] = data.Serie3[x];
        }
        for (var x = 0; x < data.Serie4.length; x++) {
            Series4[x] = data.Serie4[x];
        }
        for (var x = 0; x < data.Serie5.length; x++) {
            Series5[x] = data.Serie5[x];
        }
        for (var x = 0; x < data.Serie6.length; x++) {
            Series6[x] = data.Serie6[x];
        }

        opcion = {
            title: {
                text: titulo
            },
            xAxis: {
                categories: Categorias
            },
            yAxis: {
                title: {
                    text: 'MWh'
                }
            },
            plotOptions: {
                column: {
                    stacking: 'normal'
                }
            },
            series: [{
                type: 'column',
                name: 'TRANSF. ENERGIA ACTIVA',
                color: '#C4BC96',
                data: Series1
            }, {
                type: 'column',
                name: 'SALDO RESUL.',
                color: '#FFBD00',
                data: Series2
            }, {
                type: 'column',
                name: 'RETIRO SIN CONTRATO',
                color: '#84A147',
                data: Series3
            }, {
                type: 'column',
                name: 'COMPENSAC.(**)',
                color: '#612321',
                data: Series4
            }, {
                name: 'SALDO MES ANT.',
                color: '#BD1010',
                data: Series5
            }, {
                name: 'INTERCAMBIOS INTERNACIONALES',
                color: '#D98036',
                data: Series6
            }]
        }
    }
    if (tip == 10) {
        var Categorias = [];
        var Series1 = [];
        var Series2 = [];

        for (var j = 0; j < data.Categoria.length; j++) {
            Categorias[j] = data.Categoria[j];
        }

        for (var x = 0; x < data.Serie1.length; x++) {
            Series1[x] = data.Serie1[x];
        }
        /*    for (var x = 0; x < data.Serie2.length; x++) {
                Series2[x] = data.Serie2[x];
            }*/

        opcion = {
            chart: {
                type: 'column'
            },
            title: {
                text: titulo
            },
            yAxis: {
                title: {
                    text: 'GW.h'
                },
                labels: {
                    format: '{value}'
                }
            },
            xAxis: {
                categories: Categorias
            },
            credits: {
                enabled: false
            },
            legend: {
                enabled: false
            },
            series: [{
                name: 'Transferencia',
                color: '#356CAF',
                negativeColor: '#AD3330',

                data: Series1
            }
            ]
        }
    }
    if (tip == 11) {

        var Datoseries = [];
        for (j = 0; j < data.SeriesPie.length; j++) {
            var dato = {
                name: data.SeriesPie[j].string1,
                y: data.SeriesPie[j].decimal1
            }

            Datoseries.push(dato);
        }
        var subtexto = '';

        var months = ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio",
               "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"];

        var mesyear = months[parseInt(mesAnio.substring(0, 2)) - 1] + mesAnio.substring(2, 7);
        var newyear = parseInt(mesAnio.substring(2, 7)) - 1;
        var mesyearant = months[parseInt(mesAnio.substring(0, 2)) - 1] + ' ' + newyear;

        /*  if (data.TotalMesAnt != null &&  data.TotalMesAnt != 0) {*/
        subtexto = '<span>Total Soles S/.</span><br><span>' + mesyear + ' = ' + data.TotalMesAct + '</span><br><span>' + mesyearant + ' = ' + data.TotalMesAnt + '</span><br><span style="fill:#ff0000;"> Variacion ' + mesyear + ' / ' + mesyearant + ' = ' + data.Variacion + ' %</span>';
        /*  }*/

        opcion = {
            chart: {
                plotBackgroundColor: null,
                plotBorderWidth: null,
                plotShadow: false,
                type: 'pie'
            },
            title: {
                text: titulo
            },
            subtitle: {
                text: subtexto
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
                        format: '<b>{point.name}</b>: {point.percentage:.1f} %',
                        style: {
                            color: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black'
                        }
                    }
                }
            },
            series: [{
                type: 'pie',
                name: 'Compensacion',
                data: Datoseries
            }]
        }
    }
    if (tip == 12) {
        var Datoseries = [];
        for (j = 0; j < data.SeriesPie.length; j++) {
            var dato = {
                name: data.SeriesPie[j].string1,
                y: data.SeriesPie[j].decimal1
            }

            Datoseries.push(dato);
        }
        var subtexto = '';

        var months = ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio",
               "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"];

        var mesyear = months[parseInt(mesAnio.substring(0, 2)) - 1] + mesAnio.substring(2, 7);
        var newyear = parseInt(mesAnio.substring(2, 7)) - 1;
        var mesyearant = months[parseInt(mesAnio.substring(0, 2)) - 1] + ' ' + newyear;

        /*  if (data.TotalMesAnt != null &&  data.TotalMesAnt != 0) {*/
        subtexto = '<span>Total Soles S/.</span><br><span>' + mesyear + ' = ' + data.TotalMesAct + '</span><br><span>' + mesyearant + ' = ' + data.TotalMesAnt + '</span><br><span style="fill:#ff0000;"> Variacion ' + mesyear + ' / ' + mesyearant + ' = ' + data.Variacion + ' %</span>';
        /*  }*/
        opcion = {
            chart: {
                plotBackgroundColor: null,
                plotBorderWidth: null,
                plotShadow: false,
                type: 'pie'
            },
            title: {
                text: titulo
            },
            subtitle: {
                text: subtexto
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
                        format: '<b>{point.name}</b>: {point.percentage:.1f} %',
                        style: {
                            color: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black'
                        }
                    }
                }
            },
            series: [{
                type: 'pie',
                name: 'Compensacion',
                data: Datoseries
            }]
        }
    }
    if (tip == 13) {
        var Categorias = [];

        var Name1 = "";
        var Name2 = "";
        var Name3 = "";
        var Name4 = "";

        var Serie1 = [];
        var Serie2 = [];
        var Serie3 = [];
        var Serie4 = [];
        var Serie5 = [];
        var Serie6 = [];
        var Serie7 = [];
        var Serie8 = [];
        var Serie9 = [];

        for (j = 0; j < data.Categoria.length; j++) {
            Categorias[j] = data.Categoria[j];
        }
        for (j = 0; j < data.Serie1.length; j++) {
            Serie1[j] = data.Serie1[j];
        }
        for (j = 0; j < data.Serie2.length; j++) {
            Serie2[j] = data.Serie2[j];
        }
        for (j = 0; j < data.Serie3.length; j++) {
            Serie3[j] = data.Serie3[j];
        }
        for (j = 0; j < data.Serie4.length; j++) {
            Serie4[j] = data.Serie4[j];
        }
        for (j = 0; j < data.Serie5.length; j++) {
            Serie5[j] = data.Serie5[j];
        }
        for (j = 0; j < data.Serie6.length; j++) {
            Serie6[j] = data.Serie6[j];
        }
        for (j = 0; j < data.Serie7.length; j++) {
            Serie7[j] = data.Serie7[j];
        }
        for (j = 0; j < data.Serie8.length; j++) {
            Serie8[j] = data.Serie8[j];
        }
        for (j = 0; j < data.Serie9.length; j++) {
            Serie8[j] = data.Serie9[j];
        }


        opcion = {
            title: {
                text: 'PROYECCION DE MEDIANO PLAZO DE LA PRODUCCION DE ENERGIA EN EL SEIN (MWh)'
            },
            xAxis: {
                categories: Categorias
            },
            yAxis: {
                title: {
                    text: 'MWh'
                },
            },
            labels: {
                items: [{
                    html: '',
                    style: {
                        left: '50px',
                        top: '18px',
                        color: (Highcharts.theme && Highcharts.theme.textColor) || 'black'
                    }
                }]
            },
            plotOptions: {
                column: {
                    stacking: 'normal',
                    dataLabels: {
                        enabled: true,
                        color: (Highcharts.theme && Highcharts.theme.dataLabelsColor) || 'white'
                    }
                },
                series: {
                    label: {
                        connectorAllowed: false
                    }
                }
            },
            series: [{
                type: 'column',
                name: 'RECUADACION TRANSMISION',
                data: Serie1
            }, {
                type: 'column',
                name: 'RECAUACION GENERACION ADICIONAL',
                data: Serie2
            }, {
                type: 'column',
                name: 'RECAUDACION SEG SUMNRF',
                data: Serie3
            }, {
                type: 'column',
                name: 'RECAUDACION SEG SUM RESERVA FRIA',
                data: Serie4
            }, {
                type: 'column',
                name: 'RECAUDACION PRIMA RER',
                data: Serie5
            }, {
                type: 'column',
                name: 'RECAUDACION PRIMA FISE',
                data: Serie6
            }, {
                type: 'column',
                name: 'RECAUDACION PRIMA CASE',
                data: Serie7
            }, {
                type: 'column',
                name: 'RECAUDACION CONFIABILIDAD',
                data: Serie8
            }, {
                type: 'column',
                name: 'RECAUDACION OTROS CARGOS',
                data: Serie9
            }
            ]
        }
    }
    if (tip == 14) {

        var Categorias = [];
        var Serie1 = [];
        var Serie2 = [];
        var Serie3 = [];

        for (j = 0; j < data.Categoria.length; j++) {
            Categorias[j] = data.Categoria[j];
        }
        for (j = 0; j < data.Serie1.length; j++) {
            Serie1[j] = data.Serie1[j];
        }
        for (j = 0; j < data.Serie2.length; j++) {
            Serie2[j] = data.Serie2[j];
        }
        for (j = 0; j < data.Serie3.length; j++) {
            Serie3[j] = data.Serie3[j];
        }

        opcion = {
            title: {
                text: titulo
            },
            xAxis: {
                categories: Categorias,
                crosshair: true
            },
            yAxis: [{ // Primary yAxis
                labels: {
                    format: '{value}%',
                    style: {
                        color: Highcharts.getOptions().colors[2]
                    }
                },
                title: {
                    text: 'Variacion',
                    style: {
                        color: Highcharts.getOptions().colors[2]
                    }
                },
                opposite: true

            }, { // Secondary yAxis
                gridLineWidth: 0,
                title: {
                    text: 'Soles',
                    style: {
                        color: Highcharts.getOptions().colors[0]
                    }
                },
                labels: {
                    format: '{value}',
                    style: {
                        color: Highcharts.getOptions().colors[0]
                    }
                }

            }],
            labels: {
                items: [{
                    html: '',
                    style: {
                        left: '50px',
                        top: '18px',
                        color: (Highcharts.theme && Highcharts.theme.textColor) || 'black'
                    }
                }]
            },
            series: [{
                type: 'column',
                name: 'EJECUTADO',
                yAxis: 1,
                data: Serie1
            }, {
                type: 'column',
                name: 'PROGRAMADO',
                yAxis: 1,
                data: Serie2
            }, {
                type: 'spline',
                name: 'Variacion(%)',
                data: Serie3,
                marker: {
                    lineWidth: 2,
                    lineColor: Highcharts.getOptions().colors[3],
                    fillColor: 'white'
                }
            }]


        };
    }
    if (tip == 15) {
        opcion = {
            title: {
                text: data.Grafico.TitleText,
                style: {
                    fontSize: '14'
                }
            },
            xAxis: {
                categories: data.Grafico.XAxisCategories
            },
            yAxis: [{ //Primary Axes
                title: {
                    text: data.Grafico.YaxixTitle
                },
                labels: {
                    style: {
                        color: data.Grafico.Series[0].Color
                    }
                }

            }],
            tooltip: {
                headerFormat: '<b>{series.name}</b><br>',
                pointFormat: '{point.y:.3f}'
            },
            plotOptions: {
                column: {
                    stacking: 'normal'
                }
            },

            series: []
        };

        for (var i in data.Grafico.Series) {
            opcion.series.push({
                name: data.Grafico.Series[i].Name,
                data: data.Grafico.SeriesData[i],
                type: data.Grafico.Series[i].Type,
                yAxis: data.Grafico.Series[i].YAxis
            });
        }
    }
    if (tip == 16) {
        var tituloTipEmpresa = [];
        var series = [];

        for (var i = 0; i < data.Grafico.XAxisCategories.length; i++) {
            tituloTipEmpresa.push(data.Grafico.XAxisCategories[i]);
        }
        for (var i = 0; i < data.Grafico.Series.length; i++) {
            var obj = { name: data.Grafico.Series[i].Name };

            var valores = [];
            for (var j = 0; j < data.Grafico.Series[i].Data.length; j++) {
                valores.push(data.Grafico.Series[i].Data[j].Y);
            }
            var serie = { name: obj.name, data: valores };
            series.push(serie);
        }

        opcion = {
            chart: {
                type: 'column'
            },
            title: {
                text: titulo
            },
            xAxis: {
                categories: tituloTipEmpresa
            },
            yAxis: {
                min: 0,
                title: {
                    text: 'ENS (MWH)'
                },
                stackLabels: {
                    enabled: true,
                    style: {
                        fontWeight: 'bold',
                        color: (Highcharts.theme && Highcharts.theme.textColor) || 'gray'
                    }
                }
            },
            legend: {
                align: 'right',
                x: -30,
                verticalAlign: 'top',
                y: 25,
                floating: true,
                backgroundColor: (Highcharts.theme && Highcharts.theme.background2) || 'white',
                borderColor: '#CCC',
                borderWidth: 1,
                shadow: false
            },
            tooltip: {
                headerFormat: '<b>{point.x}</b><br/>',
                pointFormat: '{series.name}: {point.y}<br/>Total: {point.stackTotal}'
            },
            plotOptions: {
                column: {
                    stacking: 'normal',
                    dataLabels: {
                        enabled: true,
                        color: (Highcharts.theme && Highcharts.theme.dataLabelsColor) || 'white'
                    }
                }
            },
            series: series
        }
    }
    if (tip == 17) {
        var fechas = [];
        var energiaImportada = [];
        var maximaEnergiaI = [];

        for (var i = 0; i < data.FechasCategoria.length ; i++) {
            fechas.push(data.FechasCategoria[i]);
        }
        for (var i = 0; i < data.ListaMemedicion96.length; i++) {
            energiaImportada.push(data.ListaMemedicion96[i].TotalEnergiaImportada);
            maximaEnergiaI.push(data.ListaMemedicion96[i].MaximaEnergiaImportada);
        }

        opcion = {
            chart: {
                zoomType: 'xy'
            },
            title: {
                text: titulo
            },
            xAxis:
                [{
                    categories: fechas
                    ,
                    crosshair: true
                }],
            yAxis: [{ // Primary yAxis
                labels: {
                    format: '{value}',
                    style: {
                        color: Highcharts.getOptions().colors[2]
                    }
                },
                title: {
                    text: 'MW',
                    style: {
                        color: Highcharts.getOptions().colors[2]
                    }
                },
                opposite: true

            }, { // Secondary yAxis
                gridLineWidth: 0,
                title: {
                    text: 'MWH',
                    style: {
                        color: Highcharts.getOptions().colors[0]
                    }
                }
                ,
                labels: {
                    format: '{value} mm',
                    style: {
                        color: Highcharts.getOptions().colors[0]
                    }
                }

            }, { // Tertiary yAxis
                gridLineWidth: 0,
                title: {
                    text: 'Sea-Level Pressure',
                    style: {
                        color: Highcharts.getOptions().colors[1]
                    }
                },
                labels: {
                    format: '{value} mb',
                    style: {
                        color: Highcharts.getOptions().colors[1]
                    }
                },
                opposite: true
            }],
            tooltip: {
                shared: true
            },
            legend: {
                layout: 'vertical',
                align: 'left',
                x: 80,
                verticalAlign: 'top',
                y: 55,
                floating: true,
                backgroundColor: (Highcharts.theme && Highcharts.theme.legendBackgroundColor) || '#FFFFFF'
            },
            series:
                [{
                    name: 'Energia Importada Desde Ecuador',
                    type: 'column',
                    yAxis: 1,
                    data: energiaImportada
                    ,
                    tooltip: {
                        valueSuffix: ' mm'
                    }

                }, {
                    name: 'Maxima Energia',
                    type: 'spline',
                    data: maximaEnergiaI
                }]
        }
    }
    if (tip == 26) {
        var Datoseries = [];
        for (j = 0; j < data.SeriesPie.length; j++) {
            var dato = {
                name: data.SeriesPie[j].string1,
                y: data.SeriesPie[j].decimal1
            }

            Datoseries.push(dato);
        }

        opcion = {
            chart: {
                type: 'pie',
                options3d: {
                    enabled: false,
                    alpha: 45,
                    beta: 0
                }
            },
            title: {
                text: titulo
            },
            tooltip: {
                pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
            },
            plotOptions: {
                pie: {
                    allowPointSelect: true,
                    cursor: 'pointer',
                    depth: 35,
                    dataLabels: {
                        enabled: true,
                        format: '{point.name}'
                    }
                }
            },
            series: [{
                type: 'pie',
                name: 'Participacion',
                data: Datoseries
            }]
        }
    }
    if (tip == 27) {
        var Categorias = [];

        var Name1 = "";
        var Name2 = "";
        var Name3 = "";
        var Name4 = "";
        var Name5 = "";

        var Serie1 = [];
        var Serie2 = [];
        var Serie3 = [];
        var Serie4 = [];
        var Serie5 = [];

        for (j = 0; j < data.Categoria.length; j++) {
            Categorias[j] = data.Categoria[j];
        }
        for (j = 0; j < data.Serie1.length; j++) {
            Serie1[j] = data.Serie1[j];
        }
        for (j = 0; j < data.Serie2.length; j++) {
            Serie2[j] = data.Serie2[j];
        }
        for (j = 0; j < data.Serie3.length; j++) {
            Serie3[j] = data.Serie3[j];
        }
        for (j = 0; j < data.Serie4.length; j++) {
            Serie4[j] = data.Serie4[j];
        }
        for (j = 0; j < data.Serie5.length; j++) {
            Serie5[j] = data.Serie5[j];
        }

        opcion = {
            title: {
                text: titulo
            },
            xAxis: {
                categories: Categorias
            },
            yAxis: {
                title: {
                    text: 'S/./kWh'
                },
            },
            labels: {
                items: [{
                    html: '',
                    style: {
                        left: '50px',
                        top: '18px',
                        color: (Highcharts.theme && Highcharts.theme.textColor) || 'black'
                    }
                }]
            },
            plotOptions: {               
                series: {
                    label: {
                        connectorAllowed: false
                    }
                }
            },
            series: [{
                type: 'column',
                name: 'PUNTA',
                color: '#85A83D',
                data: Serie3
            }, {
                type: 'column',
                name: 'MEDIA',
                color: '#AD3330',
                data: Serie4
            }, {
                type: 'column',
                name: 'BASE',
                color: '#356CAF',
                data: Serie5
            }, {                
                name: 'PUNTA MAXIMA',
                data: Serie1,
                marker: {
                    lineWidth: 2,                    
                    radius: 4
                }
            }, {                
                name: 'MEDIA MAXIMA',
                data: Serie2,
                marker: {
                    lineWidth: 2,                    
                    radius: 4
                }
            }]
        };
    }
    if (tip == 28) {

        var serieName = [];
        var serie1 = [];
        var serie2 = [];
        var serie3 = [];
        var titleText = "Programa de Operación Mensual (POPE)";
        var mes = "";
        for (j = 0; j < data.Categoria.length; j++) {
            serieName[j] = data.Categoria[j];
        }
        for (j = 0; j < data.Serie1.length; j++) {
            serie1[j] = data.Serie1[j];
        }
        for (j = 0; j < data.Serie2.length; j++) {
            serie2[j] = data.Serie2[j];
        }
        for (j = 0; j < data.Serie3.length; j++) {
            serie3[j] = data.Serie3[j];
        }

        opcion = {
            chart: {
                zoomType: 'xy'
            },
            title: {
                text: data.nomGrafico
            },
            subtitle: {
                text: ''
            },
            xAxis: [{
                categories: serieName,
                crosshair: true
            }],
            yAxis: [{ // Primary yAxis
                labels: {
                    format: '{value}',
                    style: {
                        color: Highcharts.getOptions().colors[1]
                    }
                },
                title: {
                    text: '',
                    style: {
                        color: Highcharts.getOptions().colors[1]
                    }
                }
            }, { // Secondary yAxis
                title: {
                    text: '',
                    style: {
                        color: Highcharts.getOptions().colors[0]
                    }
                },
                labels: {
                    format: '{value}',
                    style: {
                        color: Highcharts.getOptions().colors[0]
                    }
                },
                opposite: true
            }],
            tooltip: {
                shared: true
            },
            legend: {
                /* layout: 'horizontal',*/
                align: 'right',
                x: -30,
                verticalAlign: 'top',
                y: 25,
                floating: true,
                backgroundColor: (Highcharts.theme && Highcharts.theme.legendBackgroundColor) || '#FFFFFF'
            },
            series: [{
                name: 'Variacion',
                type: 'column',
                yAxis: 1,
                data: serie3,
                tooltip: {
                    valueSuffix: ' %'
                }

            }, {
                name: 'Proyeccion Mensual',
                type: 'line',
                data: serie1,
                tooltip: {
                    valueSuffix: ''
                }
            }, {
                name: 'Proyeccion Mensual Anterior',
                type: 'line',
                data: serie2,
                tooltip: {
                    valueSuffix: ''
                }
            }]
        };
    }
    if (tip == 31) {

        var Categorias = [];
        var Serie1 = [];
        var Serie2 = [];
        var Serie3 = [];

        for (j = 0; j < data.Categoria.length; j++) {
            Categorias[j] = data.Categoria[j];
        }
        for (j = 0; j < data.Serie1.length; j++) {
            Serie1[j] = data.Serie1[j];
        }
        for (j = 0; j < data.Serie2.length; j++) {
            Serie2[j] = data.Serie2[j];
        }
        for (j = 0; j < data.Serie3.length; j++) {
            Serie3[j] = data.Serie3[j];
        }

        opcion = {
            title: {
                text: titulo
            },
            xAxis: {
                categories: Categorias,
                crosshair: true
            },
            yAxis: [{ // Primary yAxis
                labels: {
                    format: '{value}%',
                    style: {
                        color: Highcharts.getOptions().colors[2]
                    }
                },
                title: {
                    text: 'Variacion',
                    style: {
                        color: Highcharts.getOptions().colors[2]
                    }
                },
                opposite: true

            }, { // Secondary yAxis
                gridLineWidth: 0,
                title: {
                    text: 'Soles',
                    style: {
                        color: Highcharts.getOptions().colors[0]
                    }
                },
                labels: {
                    format: '{value}',
                    style: {
                        color: Highcharts.getOptions().colors[0]
                    }
                }

            }],
            labels: {
                items: [{
                    html: '',
                    style: {
                        left: '50px',
                        top: '18px',
                        color: (Highcharts.theme && Highcharts.theme.textColor) || 'black'
                    }
                }]
            },
            series: [{
                type: 'column',
                name: 'SEMANAL',
                yAxis: 1,
                data: Serie1
            }, {
                type: 'column',
                name: 'EJECUTADO',
                yAxis: 1,
                data: Serie2
            }, {
                type: 'spline',
                name: 'Variacion(%)',
                data: Serie3,
                marker: {
                    lineWidth: 2,
                    lineColor: Highcharts.getOptions().colors[3],
                    fillColor: 'white'
                }
            }]


        };
    }
    if (tip == 32) {

        var categorias = [];
        var series = [];
        var nombreBarra = "";
        for (var i = 0; i < data.Grafico.XAxisCategories.length; i++) {
            categorias.push(data.Grafico.XAxisCategories[i]);
        }
        for (var i = 0; i < data.Grafico.Series.length; i++) {
            var valores = [];
            for (var j = 0; j < data.Grafico.Series[i].Data.length; j++) {
                valores.push(data.Grafico.Series[i].Data[j].Y);
            }
            series.push({
                name: data.Grafico.Series[i].Name,
                data: valores
            });
            Barrnombre = data.Grafico.Series[i].Name;
        }
        opcion = {
            chart: {
                type: 'line'
            },
            title: {
                text: titulo + '<br>' + Barrnombre
            },
            yAxis: {
                title: {
                    text: 'S/.KWh'
                }
            },
            xAxis: {
                categories: categorias
            },
            plotOptions: {
                line: {
                    dataLabels: {
                        enabled: false
                    },
                    marker: {
                        enabled: false
                    }, enableMouseTracking: true
                }
            },

            series: series

        };
    }
    if (tip == 34) {

        var Categorias = [];
        var Serie1 = [];
        var Serie2 = [];
        var Serie3 = [];

        for (j = 0; j < data.Categoria.length; j++) {
            Categorias[j] = data.Categoria[j];
        }
        for (j = 0; j < data.Serie1.length; j++) {
            Serie1[j] = data.Serie1[j];
        }
        for (j = 0; j < data.Serie2.length; j++) {
            Serie2[j] = data.Serie2[j];
        }
        for (j = 0; j < data.Serie3.length; j++) {
            Serie3[j] = data.Serie3[j];
        }

        opcion = {
            title: {
                text: titulo
            },
            xAxis: {
                categories: Categorias,
                crosshair: true
            },
            yAxis: [{ // Primary yAxis
                labels: {
                    format: '{value}%',
                    style: {
                        color: Highcharts.getOptions().colors[2]
                    }
                },
                title: {
                    text: 'Variacion',
                    style: {
                        color: Highcharts.getOptions().colors[2]
                    }
                },
                opposite: true

            }, { // Secondary yAxis
                gridLineWidth: 0,
                title: {
                    text: 'Soles',
                    style: {
                        color: Highcharts.getOptions().colors[0]
                    }
                },
                labels: {
                    format: '{value}',
                    style: {
                        color: Highcharts.getOptions().colors[0]
                    }
                }

            }],
            labels: {
                items: [{
                    html: '',
                    style: {
                        left: '50px',
                        top: '18px',
                        color: (Highcharts.theme && Highcharts.theme.textColor) || 'black'
                    }
                }]
            },
            series: [{
                type: 'column',
                name: 'DIARIO',
                yAxis: 1,
                data: Serie1
            }, {
                type: 'column',
                name: 'EJECUTADO',
                yAxis: 1,
                data: Serie2
            }, {
                type: 'spline',
                name: 'Variacion(%)',
                data: Serie3,
                marker: {
                    lineWidth: 2,
                    lineColor: Highcharts.getOptions().colors[3],
                    fillColor: 'white'
                }
            }]


        };
    }
    if (tip == 35) {

        var categorias = [];
        var series = [];
        var nombreBarra = "";
        for (var i = 0; i < data.Grafico.XAxisCategories.length; i++) {
            categorias.push(data.Grafico.XAxisCategories[i]);
        }
        for (var i = 0; i < data.Grafico.Series.length; i++) {
            var valores = [];
            for (var j = 0; j < data.Grafico.Series[i].Data.length; j++) {
                valores.push(data.Grafico.Series[i].Data[j].Y);
            }
            series.push({
                name: data.Grafico.Series[i].Name,
                data: valores
            });
            Barrnombre = data.Grafico.Series[i].Name;
        }
        opcion = {
            chart: {
                type: 'line'
            },
            title: {
                text: titulo + '<br>' + Barrnombre
            },
            yAxis: {
                title: {
                    text: 'S/.KWh'
                }
            },
            xAxis: {
                categories: categorias
            },
            plotOptions: {
                line: {
                    dataLabels: {
                        enabled: false
                    },
                    marker: {
                        enabled: false
                    }, enableMouseTracking: true
                }
            },

            series: series

        };
    }
    Highcharts.chart(grafico, opcion);
}