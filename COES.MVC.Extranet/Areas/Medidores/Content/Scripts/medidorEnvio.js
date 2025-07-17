var controlador = siteRoot + 'Medidores/Envio/';
var TIPO_GRAFICO_IGUAL_MEDIDA = 1;
var TIPO_GRAFICO_DIFERENTE_MEDIDA = 2;

var TIPO_FUENTE_MEDIDORES = 1;
var TIPO_FUENTE_DATOSSCADA = 2;
var TIPO_FUENTE_DESPACHODIARIO = 3;
var TIPO_FUENTE_CAUDALTURBINADO = 4;
var TIPO_FUENTE_RPF = 5;

$(function () {
    var hoja1 = '1'; //potencia activa
    var hoja2 = '2';
    var hoja3 = '3';
    //inicializar variables
    LISTA_OBJETO_HOJA = { '1': crearObjetoHoja(), '2': crearObjetoHoja(), '3': crearObjetoHoja() };

    getErrores(hoja1)[ERROR_NO_NUMERO].validar = true;
    getErrores(hoja1)[ERROR_LIM_INFERIOR].validar = true;
    getErrores(hoja1)[ERROR_LIM_SUPERIOR].validar = true;
    getErrores(hoja1)[ERROR_BLANCO].validar = true;
    getErrores(hoja1)[ERROR_NUMERO_NEGATIVO].validar = true;
    getErrores(hoja1)[ERROR_DATA_CENTRAL_SOLAR].validar = true;

    getErrores(hoja2)[ERROR_NO_NUMERO].validar = true;
    getErrores(hoja2)[ERROR_LIM_INFERIOR].validar = true;
    getErrores(hoja2)[ERROR_LIM_SUPERIOR].validar = true;
    getErrores(hoja2)[ERROR_BLANCO].validar = true;

    getErrores(hoja3)[ERROR_NO_NUMERO].validar = true;
    getErrores(hoja3)[ERROR_LIM_INFERIOR].validar = true;
    getErrores(hoja3)[ERROR_LIM_SUPERIOR].validar = true;
    getErrores(hoja3)[ERROR_BLANCO].validar = true;

    setTieneGrafico(true, hoja1);

    //crear vistas
    var fmt1 = $("#hfFormato1").val();
    var fmt2 = $("#hfFormato2").val();
    var fmt3 = $("#hfFormato3").val();

    cargarHoja(hoja1, fmt1, 'viewCentralPotActiva');
    cargarHoja(hoja2, fmt2, 'viewCentralPotReactiva');
    cargarHoja(hoja3, fmt3, 'viewServAuxPotReactiva');
});

function generaFiltroGrafico(numHoja) {
    //Filtro fecha
    $('#txtFechaDesde').Zebra_DatePicker({
        onSelect: function () {
            graficoFormato(numHoja);
        }
    });
    $('#txtFechaHasta').Zebra_DatePicker({
        onSelect: function () {
            graficoFormato(numHoja);
        }
    });

    //periodo
    $('input[type=radio][name=periodoGraf]').unbind('change');
    $('input[type=radio][name=periodoGraf]').change(function () {
        graficoFormato(numHoja);
    });

    //datos
    $('input[type=radio][name=datoGraf]').unbind('change');
    $('input[type=radio][name=datoGraf]').change(function () {
        graficoFormato(numHoja);
    });

    //Filtro Centrales
    $("#cbCentral2").unbind('change');
    $("#cbCentral2").change(function () {
        graficoFormato(numHoja);
    });

    $('#cbCentral2').empty();
    var central = getVariableEvt(numHoja).ListaEquipo;
    for (var i = 0; i < central.length; i++) {
        //if (central[i].Famcodi == 4) {//hidraulica
        $('#cbCentral2').append('<option value=' + central[i].Equicodi + '>' + central[i].Equinomb + '</option>');
        //}
    }

    //Fuente 1
    $("#cbFuente1").unbind('change');
    $('#cbFuente1').empty();
    var fuente1 = getVariableEvt(numHoja).ListaFuente1;
    for (var i = 0; i < fuente1.length; i++) {
        $('#cbFuente1').append('<option value=' + fuente1[i].Codigo + '>' + fuente1[i].Valor + '</option>');
    }
    //Fuente 2
    $("#cbFuente2").unbind('change');
    $("#cbFuente2").change(function () {
        mostrarTipoDato();
        graficoFormato(numHoja);
    });
    $('#cbFuente2').empty();
    var fuente2 = getVariableEvt(numHoja).ListaFuente2;
    for (var i = 0; i < fuente2.length; i++) {
        $('#cbFuente2').append('<option value=' + fuente2[i].Codigo + '>' + fuente2[i].Valor + '</option>');
    }
    mostrarTipoDato();

    //Filtro Medida
    $("#cbEjeder").unbind('change');
    $('#cbEjeder').empty();
    var medida = getVariableEvt(numHoja).ListaTipoInformacion;
    for (var i = 0; i < medida.length; i++) {
        $('#cbEjeder').append('<option value=' + medida[i].Tipoinfocodi + '>' + medida[i].Tipoinfoabrev + '</option>');
    }

    //btnExportarGrafico
    $("#btnExportarGrafico").unbind('click');
    $('#btnExportarGrafico').click(function () {
        btnExportarGrafico(numHoja);
    });
}

//////////////////////////////////////////////////////////
//// btnExportarGrafico
//////////////////////////////////////////////////////////
function btnExportarGrafico(numHoja) {
    exportarGrafico(numHoja);
}

function mostrarGrafico(numHoja) {
    //cargar filtros de hoja a popup
    //var cbCentral = $(getIdElemento(numHoja, '#cbCentral')).val();
    //$("#cbCentral2").val(cbCentral);
    //var fecha = $(getIdElemento(numHoja, '#hfFecha')).val();
    //var mes = $(getIdElemento(numHoja, '#txtMes')).val();
    //fecha = getFechaFromMes(fecha, mes);
    //$("#txtFechaDesde").val(fecha);
    //$("#txtFechaHasta").val(fecha);

    if (validarGrafico(numHoja)) {
        graficoFormato(numHoja);
    }

}

function validarGrafico(numHoja) {

    if ($("#cbCentral2").val() == null) {
        alert("No tiene central hidraúlica");
        return false;
    }

    //var tipoCentral = getTipoCentral(numHoja);
    //switch (tipoCentral) {
    //    case 4: //Hidraulicas
    //        break;
    //    case 5: //Térmicas
    //    case 37: //Solares
    //    case 39: //Eolicas
    //    default:
    //        alert("Seleccioné una central Hidraúlica");
    //        return false;
    //}

    return true;
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

function graficoFormato(numHoja) {
    var mes = $(getIdElemento(numHoja, '#txtMes')).val();
    var central = $("#cbCentral2").val();
    var fuente2 = $("#cbFuente2").val();
    var periodo = $('input[name="periodoGraf"]:checked').val();
    var tipoDato = $('input[name="datoGraf"]:checked').val();

    $.ajax({
        type: 'POST',
        dataType: 'json',
        contentType: 'application/json',
        traditional: true,
        url: controlador + 'GenerarGraficoCargaDatoUsuarioAgentes',
        data: JSON.stringify({
            data: getVariableHot(numHoja).getData(),
            idEmpresa: getIdEmpresa(numHoja),
            idFormato: getIdFormato(numHoja),
            mes: mes,
            periodo: periodo,
            tipoDato: tipoDato,
            idCentral: central,
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

function exportarGrafico(numHoja) {
    var mes = $(getIdElemento(numHoja, '#txtMes')).val();
    var central = $("#cbCentral2").val();
    var fuente2 = $("#cbFuente2").val();
    var periodo = $('input[name="periodoGraf"]:checked').val();
    var tipoDato = $('input[name="datoGraf"]:checked').val();

    $.ajax({
        type: 'POST',
        dataType: 'json',
        contentType: 'application/json',
        traditional: true,
        url: controlador + 'ExportarExcelGraficoCargaDatoUsuarioAgentes',
        data: JSON.stringify({
            data: getVariableHot(numHoja).getData(),
            idEmpresa: getIdEmpresa(numHoja),
            idFormato: getIdFormato(numHoja),
            mes: mes,
            periodo: periodo,
            tipoDato: tipoDato,
            idCentral: central,
            fuente2: fuente2
        }),
        beforeSend: function () {
            mostrarExito(numHoja, "Descargando información ...");
        },
        success: function (result) {
            if (result.length > 0 && result[0] != "-1") {
                mostrarExito(numHoja, "<strong>Los datos se descargaron correctamente</strong>");
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
//////////////////// funciones auxiliares

function getTipoCentral(numHoja) {
    var central = $(getIdElemento(numHoja, '#cbCentral')).val();
    var listaEquipo = getVariableEvt(numHoja).ListaEquipo;

    for (var i = 0; i < listaEquipo.length; i++) {
        if (listaEquipo[i].Equicodi == central) {
            return listaEquipo[i].Famcodi;
        }
    }

    return -1;
}

function validarCentralSolar(numHoja) {

}
    function mostrarExito(mensaje) {
        $('#mensajePrincipal').removeClass();
        $('#mensajePrincipal').html(mensaje);
        $('#mensajePrincipal').addClass('action-exito');
    }