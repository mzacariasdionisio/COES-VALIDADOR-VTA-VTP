$(function () {
    cargarLista();
});

function mostrarReporteByFiltros() {
    cargarLista();
}

function cargarLista() {
    $('#listado').html('');
    var alturaDisponible = getHeightTablaListado();

    $.ajax({
        type: 'POST',
        data: {
            fechaInicio: getFechaInicio(),
            fechaFin: getFechaFin()
        },
        url: controlador + 'CargarListaSistemasAisladosTemporalesYVariacionesSostenidasSubitas',
        success: function (aData) {
            $('.filtro_fecha_desc').html(aData.FiltroFechaDesc);

            var ancho = $("#mainLayout").width() - 30;

            $('#listado').html(aData.Resultado);

            $("#resultado").css("width", (ancho) + "px");
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

/////////////////////////////////////////////////////////////////////////////////////////////////////////
// Reporte de las Variaciones Sostenidas y Súbitas de Frecuencia
/////////////////////////////////////////////////////////////////////////////////////////////////////////
function fnClickReporteVariacion(x) {
    $('#idVistaGrafica2').html('');

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaVariacionesSostenidasSubitas',
        data: {
            empresas: getEmpresa(),
            fechaInicio: getFechaInicio(),
            fechaFin: getFechaFin(),
            gps: x
        },
        success: function (aData) {
            $('#listado1').html(aData.Resultado);
            $(".fila_grafico_distribucion").hide();

            setTimeout(function () {
                $('#idGrafico1').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown',
                    modalClose: false
                });
            }, 50);
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

/////////////////////////////////////////////////////////////////////////////////////////////////////////
// Distribución de frecuencia
/////////////////////////////////////////////////////////////////////////////////////////////////////////
function fnClickFrecuencia(x) {
    $.ajax({
        type: 'POST',
        url: controlador + 'CargarGraficoVariacionesSostenidasSubitas',
        data: { empresas: '', gps: x, fechaInicio: getFechaInicio() },
        dataType: 'json',
        success: function (aData) {
            if (aData.NRegistros > 0) {
                GraficoDistribucion(aData);
                $('#idVistaGrafica2').html(aData.Resultado);

                setTimeout(function () {
                    $('#idGrafico2').bPopup({
                        easing: 'easeOutBack',
                        speed: 450,
                        transition: 'slideDown',
                        modalClose: false
                    });
                }, 50);
            }
            else { alert('Sin informacion!'); }
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function GraficoDistribucion(result) {
    var opcion;

    var json = result.ListaGrafico;
    var jsondata = [];
    var indice = 3;
    for (var i in json) {
        var jsonValor = [];
        var jsonLista = json[i].ListaValores;
        for (var j in jsonLista) {
            jsonValor.push(jsonLista[j]);
        }
        jsondata.push({
            name: json[i].SerieName,
            data: jsonValor,
            color: json[i].SerieColor,
            //index: indice
        });
        indice--;
    }

    opcion = {
        chart: {
            type: 'spline'
        },
        title: {
            text: result.Grafico.TitleText
        },
        xAxis: {
            title: {
                text: ''
            },
            categories: result.Grafico.XAxisCategories
        },
        credits: {
            enabled: false
        },
        yAxis: {
            title: { text: '' },
            labels: {
                formatter: function () {
                    return this.value + '%';
                }
            }/*,
            categories: [0,10,20,30,40,50,60,70]*/
        },
        legend: {
            reversed: true
        },
        plotOptions: {
            bar: {
                dataLabels: {
                    enabled: false
                }
            }
        },
        series: jsondata
    };

    $('#idVistaGrafica').highcharts(opcion);
}