var controlador = siteRoot + 'Siosein/TablasPrieDeclaracionMen/'

$(function () {
    $('#cbEmpresa').multipleSelect({
        width: '150px',
        filter: true,
        onClose: function (view) {
            llamarMetodos();
        }
    });
    $('#cbEmpresa').multipleSelect('checkAll');

    llamarMetodos();
    function llamarMetodos() {
        cargarListaDifusionFlujoInterconexionEjec();
        cargarGraficoDifusionFlujoInterconexionEjec();
        cargarGraficoDifusionFlujoInterconexionEjecExpImp();
    }
});
function cargarListaDifusionFlujoInterconexionEjec() {
    var interconexion = $('#cbInterconexiones').val();
    if (interconexion == "") interconexion = "-1";
    var periodo = $("#txtFecha1").val();
    $('#hfEmpresa').val(interconexion);

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaDifusionFlujoInterconexionEjec',
        data: { IDinterconexion: interconexion, periodo: periodo },
        success: function (aData) {
            $('#listado1').html(aData.Resultado);
            $("#tabla17").dataTable();
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function cargarGraficoDifusionFlujoInterconexionEjec() {
    var periodo = $("#txtFecha1").val();
    $.ajax({
        type: 'POST',
        url: controlador + 'CargarGraficoDifusionFlujoInterconexionEjec',
        data: { idEmpresa: $('#hfEmpresa').val(), periodo: periodo },
        dataType: 'json',
        success: function (data) {
            disenioGrafico(data,'idGrafico1', 'EVOLUCÓN DE LA ENERGIA Y MÁXIMA POTENCIA IMPORTADA DESDE ECUADOR DURANTE LA SEMANA OPERATIVA N° 30 - 2016', 1);
            //if (aData.Lista1.length > 0) {
            //    diseñoGrafico(aData.Lista1, 'idGrafico1', 'POTENCIA EFECTIVA POR EMPRESAS');
            //} else $('#idGrafico1').html('Sin Grafico - No existen registros a mostrar!');
        },
        error: function () {
            alert('Ha ocurrido un error');
        }
    });
}
function cargarGraficoDifusionFlujoInterconexionEjecExpImp() {
    var periodo = $("#txtFecha1").val();
    $.ajax({
        type: 'POST',
        url: controlador + 'CargarGraficoDifusionFlujoInterconexionEjecExpImp',
        data: { idEmpresa: $('#hfEmpresa').val(), periodo: periodo },
        dataType: 'json',
        success: function (data) {
            //disenioGrafico(data,'idGrafico2', 'CONTRATO DE INTERCAMBIO INTERNACIONALES (EXPORTACION - IMPORTACION)', 2);
            graficoReporteFlujoPotenciaSt(data, 'idGrafico2')
            //if (aData.Lista1.length > 0) {
            //    diseñoGrafico(aData.Lista1, 'idGrafico1', 'POTENCIA EFECTIVA POR EMPRESAS');
            //} else $('#idGrafico1').html('Sin Grafico - No existen registros a mostrar!');
        },
        error: function () {
            alert('Ha ocurrido un error');
        }
    });
}
function graficoReporteFlujoPotenciaSt (data, grafico) {
    var series = [];
    var series1 = [];

    for (k = 0; k < data.Grafico.SerieDataS[0].length; k++) {
        var seriePoint = [];
        var now = parseJsonDate(data.Grafico.SerieDataS[0][k].X);
        var nowUTC = Date.UTC(now.getFullYear(), now.getMonth(), now.getDate(), now.getHours(), now.getMinutes(), now.getSeconds());
        seriePoint.push(nowUTC);
        seriePoint.push(data.Grafico.SerieDataS[0][k].Y);
        series.push(seriePoint);
    }
    for (k = 0; k < data.Grafico.SerieDataS[1].length; k++) {
        var seriePoint = [];
        var now = parseJsonDate(data.Grafico.SerieDataS[1][k].X);
        var nowUTC = Date.UTC(now.getFullYear(), now.getMonth(), now.getDate(), now.getHours(), now.getMinutes(), now.getSeconds());
        seriePoint.push(nowUTC);
        seriePoint.push((data.Grafico.SerieDataS[1][k].Y) * -1);
        series1.push(seriePoint);
    }

    var opcion = {
        rangeSelector: {
            selected: 1
        },

        title: {
            text: data.Grafico.titleText,
            style: {
                fontSize: '8'
            }
        },
        yAxis: [{
            title: {
                text: data.Grafico.YAxixTitle[0]
            }
            //,
            //min: 0
        },
    {
        title: {
            text: data.Grafico.YAxixTitle[1]
        },
        opposite: false

    }],
        legend: {
            layout: 'horizontal',
            align: 'center',
            verticalAlign: 'bottom',
            borderWidth: 0,
            enabled: true
        },
        series: []
    };
    opcion.series.push({
        name: data.Grafico.Series[0].Name,
        color: data.Grafico.Series[0].Color,
        data: series,
        type: data.Grafico.Series[0].Type
    });
    if (data.Grafico.SerieDataS[1]) {
        opcion.series.push({
            name: data.Grafico.Series[1].Name,
            color: data.Grafico.Series[1].Color,
            data: series1,
            type: data.Grafico.Series[1].Type
        });
    }

    $('#'+ grafico).highcharts('StockChart', opcion);
}
function disenioGrafico(data, grafico, texto, tip) {
    if (tip == 1) {
        
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

        Highcharts.chart(grafico, {
            chart: {
                zoomType: 'xy'
            },
            title: {
                text: texto
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
                    text: '',
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
        });
    }
    if (tip == 2) {

        var horas = [];
        var energiaImportada = [];
        var energiaExportada = [];
        var series = [];
       
        for (var i = 0; i < data.ListaMemedicion96.length ; i++) {
            horas.push(data.ListaMemedicion96[i].strMedifechamin);
        }
        for (var i = 0; i < data.ListaMemedicion96.length; i++) {
            energiaExportada.push(data.ListaMemedicion96[i].TotalEnergiaExportada);
            energiaImportada.push(data.ListaMemedicion96[i].TotalEnergiaImportada);
        }
        for (var i = 0; i < energiaExportada.length; i++) {
            energiaExportada[i] = energiaExportada[i] * -1;
        }

     
    }
}