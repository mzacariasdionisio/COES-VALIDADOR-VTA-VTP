var controlador = siteRoot + 'Medidores/Reportes/';
var numHoja = '';
var TIPO_GRAFICO_IGUAL_MEDIDA = 1;
var TIPO_GRAFICO_DIFERENTE_MEDIDA = 2;

var TIPO_FUENTE_MEDIDORES = 1;
var TIPO_FUENTE_DATOSSCADA = 2;
var TIPO_FUENTE_DESPACHODIARIO = 3;
var TIPO_FUENTE_CAUDALTURBINADO = 4;
var TIPO_FUENTE_RPF = 5;

$(function () {
    $('#btnConsultar').click(function () {
        mostrarGrafico();
    });

    $('#btnExportar').click(function () {
        btnExportarGrafico();
    });

    generaFiltroGrafico();
});

function generaFiltroGrafico() {
    //Filtro fecha
    $('#txtFechaDesde').Zebra_DatePicker({
        onSelect: function () {
            limpiarReporte();
        }
    });
    $('#txtFechaHasta').Zebra_DatePicker({
        onSelect: function () {
            limpiarReporte();
        }
    });

    //periodo
    $('input[type=radio][name=periodoGraf]').change(function () {
        limpiarReporte();
    });

    //datos
    $('input[type=radio][name=datoGraf]').change(function () {
        limpiarReporte();
    });

    $("#cbTipoGen").change(function () {
        listarEmpresaByTipoGeneracion();
    });

    //filtro empresas
    $("#cbEmpresa").change(function () {
        listarDataEmpresa();
    });

    //Filtro Centrales
    $("#cbCentral2").change(function () {
        listarFuente2();
    });

    //Fuente 2
    $("#cbFuente2").change(function () {
        mostrarTipoDato();
        limpiarReporte();
    });

    mostrarTipoDato();
    listarDataEmpresa();
}

function listarDataEmpresa() {
    $('#cbCentral2').empty();
    $('#cbEjeder').empty();

    var idEmpresa = $('#cbEmpresa').val();
    var tipoGeneracion = $('#cbTipoGen').val();

    if (idEmpresa == null) {
        alert('No hay empresas para el tipo de generación seleccionado');
        $('#idVistaGrafica').html('');
        return false;
    }

    $.ajax({
        type: 'POST',
        async: true,
        url: controlador + 'ListarCentralByEmpresa',
        data: {
            tipoGeneracion: tipoGeneracion,
            idEmpresa: idEmpresa
        },
        success: function (result) {
            var central = result.ListaEquipo;
            for (var i = 0; i < central.length; i++) {
                $('#cbCentral2').append('<option value=' + central[i].Equicodi + '>' + central[i].Equinomb + '</option>');
            }

            var tipoInfo = result.ListaTipoInformacion;
            for (var j = 0; j < tipoInfo.length; j++) {
                $('#cbEjeder').append('<option value=' + tipoInfo[j].Tipoinfocodi + '>' + tipoInfo[j].Tipoinfoabrev + '</option>');
            }

            listarFuente2();
        },
        error: function (result) {
            alert('Ha ocurrido un error');
        }
    });
}

function listarEmpresaByTipoGeneracion() {
    var tipoGeneracion = $('#cbTipoGen').val();

    $('#cbEmpresa').empty();

    $.ajax({
        type: 'POST',
        url: controlador + 'ListarEmpresaXTipoGeneracion',
        data: {
            tipoGeneracion: tipoGeneracion
        },
        success: function (result) {
            if (result.length > 0) {
                for (var i = 0; i < result.length; i++) {
                    $('#cbEmpresa').append('<option value="' + result[i].Emprcodi + '">' + result[i].Emprnomb + '</option>');
                }
            }
            listarDataEmpresa();
        },
        error: function () {
            alert('Ha ocurrido un error.');
        }
    });
}

function listarFuente2() {
    $('#cbFuente2').empty();

    var idEmpresa = $('#cbEmpresa').val();
    var idCentral = $('#cbCentral2').val();
    $.ajax({
        type: 'POST',
        async: true,
        url: controlador + 'ListarFuenteByCentral',
        data: {
            idEmpresa: idEmpresa,
            idCentral: idCentral
        },
        success: function (result) {
            var fuente2 = result.ListaFuente2;
            for (var i = 0; i < fuente2.length; i++) {
                $('#cbFuente2').append('<option value=' + fuente2[i].Codigo + '>' + fuente2[i].Nombre + '</option>');
            }
            mostrarTipoDato();
            limpiarReporte();
        },
        error: function (result) {
            alert('Ha ocurrido un error');
        }
    });

}

//////////////////////////////////////////////////////////
//// btnConsultar
//////////////////////////////////////////////////////////

function mostrarGrafico() {
    if (validarGrafico()) {
        graficoFormato();
    } else {
        limpiarReporte();
    }
}

function validarGrafico() {

    if ($("#cbCentral2").val() == null) {
        alert("No tiene central hidraúlica");
        return false;
    }

    if ($("#cbFuente2").val() == null) {
        alert("No tiene Fuente 2");
        return false;
    }

    return true;
}

function limpiarReporte() {
    $('#idVistaGrafica').html('');
}

function mostrarTipoDato() {
    $(".datoGraf15").hide();
    $(".datoGraf30").hide();
    $(".datoGraf1").hide();

    var valor = parseInt($("#cbFuente2").val());
    switch (valor) {
        case TIPO_FUENTE_DATOSSCADA:
            $('input[type=radio][name=datoGraf][value=1]').prop('checked', true);
            $(".datoGraf15").show();
            break;
        case TIPO_FUENTE_DESPACHODIARIO:
            $('input[type=radio][name=datoGraf][value=2]').prop('checked', true);
            $(".datoGraf30").show();
            break;
        case TIPO_FUENTE_CAUDALTURBINADO:
            $('input[type=radio][name=datoGraf][value=3]').prop('checked', true);
            $(".datoGraf1").show();
            break;
        case TIPO_FUENTE_RPF:
            $('input[type=radio][name=datoGraf][value=1]').prop('checked', true);
            $(".datoGraf15").show();
            $(".datoGraf30").show();
            $(".datoGraf1").show();
            break;
    }
}

function graficoFormato() {
    var fechaIni = $("#txtFechaDesde").val();
    var fechaFin = $("#txtFechaHasta").val();
    var central = $("#cbCentral2").val();
    var fuente1 = $("#cbFuente1").val();
    var fuente2 = $("#cbFuente2").val();
    var periodo = $('input[name="periodoGraf"]:checked').val();
    var tipoDato = $('input[name="datoGraf"]:checked').val();
    var idEmpresa = $("#cbEmpresa").val();

    $.ajax({
        type: 'POST',
        dataType: 'json',
        contentType: 'application/json',
        traditional: true,
        url: controlador + 'GenerarGraficoCargaDatoUsuarioCOES',
        data: JSON.stringify({
            idEmpresa: idEmpresa,
            fechaIni: fechaIni,
            fechaFin: fechaFin,
            periodo: periodo,
            tipoDato: tipoDato,
            idCentral: central,
            fuente1: fuente1,
            fuente2: fuente2
        }),
        success: function (data) {
            //si es correcta la ejecución de la url
            strHtml = "<div id ='panelGrafico' style='display: block; width: 1250px;height:650px;margin: 0 auto;'></div>";
            $('#idVistaGrafica').html(strHtml);
            setTimeout(function () {
                $('#idGrafico').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown',
                    modalClose: false
                });
            }, 50);

            if (data) {
                generarGrafico('panelGrafico', data);
            }
            else $('#panelGrafico').html('Sin Grafico - No existen registros a mostrar!');
        },
        error: function () {
            alert('Ha ocurrido un error');
        }
    });
}

function generarGrafico(idGrafico, data) {
    if (TIPO_GRAFICO_IGUAL_MEDIDA == data.TipoGrafico) {
        generarGraficoIgualMedida(idGrafico, data);
    }

    if (TIPO_GRAFICO_DIFERENTE_MEDIDA == data.TipoGrafico) {
        generarGraficoDiferenteMedida(idGrafico, data);
    }
}

function generarGraficoIgualMedida(idGrafico, data) {
    //obtener data
    var dataCategoria = [];
    var dataFuente1 = [];
    var dataFuente2 = [];
    var tituloGrafico = data.TituloGrafico;
    var tituloFuente1 = data.TituloFuente1;
    var tituloFuente2 = data.TituloFuente2;
    var leyendaFuente1 = data.LeyendaFuente1;
    var leyendaFuente2 = data.LeyendaFuente2;

    for (var i = 0; i < data.ListaPunto.length; i++) {
        var g = data.ListaPunto[i];
        dataCategoria.push(g.FechaString);
        dataFuente1.push(g.ValorFuente1);
        dataFuente2.push(g.ValorFuente2);
    }

    //Generar grafica
    Highcharts.chart(idGrafico, {
        chart: {
            zoomType: 'xy'
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
            }
        },
        series: [{
            name: leyendaFuente1,
            type: 'spline',
            data: dataFuente1,
            color: 'blue'
        }, {
            name: leyendaFuente2,
            type: 'spline',
            data: dataFuente2,
            color: 'red'
        }]
    });
}

function generarGraficoDiferenteMedida(idGrafico, data) {
    //obtener data
    var dataCategoria = [];
    var dataFuente1 = [];
    var dataFuente2 = [];
    var tituloGrafico = data.TituloGrafico;
    var tituloFuente1 = data.TituloFuente1;
    var tituloFuente2 = data.TituloFuente2;
    var leyendaFuente1 = data.LeyendaFuente1;
    var leyendaFuente2 = data.LeyendaFuente2;

    for (var i = 0; i < data.ListaPunto.length; i++) {
        var g = data.ListaPunto[i];
        dataCategoria.push(g.FechaString);
        dataFuente1.push(g.ValorFuente1);
        dataFuente2.push(g.ValorFuente2);
    }

    //Generar grafica
    Highcharts.chart(idGrafico, {
        chart: {
            zoomType: 'xy'
        },
        title: {
            text: tituloGrafico
        },
        subtitle: {
            text: ''
        },
        xAxis: [{
            categories: dataCategoria,
            crosshair: true
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
        }, { // Secondary yAxis
            title: {
                text: tituloFuente2,
                style: {
                    color: 'red'
                }
            },
            labels: {
                format: '{value}',
                style: {
                    color: 'red'
                }
            },
            opposite: true,
            min: 0
        }],
        tooltip: {
            shared: true
        },
        legend: {
            layout: 'horizontal',
            align: 'center',
            verticalAlign: 'bottom',
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
            }
        },
        series: [{
            name: leyendaFuente1,
            type: 'spline',
            yAxis: 1,
            data: dataFuente1,
            color: 'blue'
        }, {
            name: leyendaFuente2,
            type: 'spline',
            data: dataFuente2,
            color: 'red'
        }]
    });
}

//////////////////////////////////////////////////////////
//// btnExportar
//////////////////////////////////////////////////////////
function btnExportarGrafico() {
    exportarGrafico();
}
function exportarGrafico() {
    var fechaIni = $("#txtFechaDesde").val();
    var fechaFin = $("#txtFechaHasta").val();
    var central = $("#cbCentral2").val();
    var fuente1 = $("#cbFuente1").val();
    var fuente2 = $("#cbFuente2").val();
    var periodo = $('input[name="periodoGraf"]:checked').val();
    var tipoDato = $('input[name="datoGraf"]:checked').val();
    var idEmpresa = $("#cbEmpresa").val();

    $.ajax({
        type: 'POST',
        dataType: 'json',
        contentType: 'application/json',
        traditional: true,
        url: controlador + 'ExportarExcelGraficoCargaDatoUsuarioCOES',
        data: JSON.stringify({
            idEmpresa: idEmpresa,
            fechaIni: fechaIni,
            fechaFin: fechaFin,
            periodo: periodo,
            tipoDato: tipoDato,
            idCentral: central,
            fuente1: fuente1,
            fuente2: fuente2
        }),
        beforeSend: function () {
            //mostrarExito(numHoja, "Descargando información ...");
        },
        success: function (result) {
            if (result.length > 0 && result[0] != "-1") {
                window.location.href = controlador + 'DescargarExcelGraficoCargaDato?archivo=' + result[0] + '&nombre=' + result[1];
            }
            else {
                alert("Error en descargar el archivo");
            }
        },
        error: function (result) {
            alert('ha ocurrido un error al descargar el archivo excel. ' + result.status + ' - ' + result.statusText + '.');
        }
    });
}