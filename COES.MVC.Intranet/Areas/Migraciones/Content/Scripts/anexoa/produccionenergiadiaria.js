//variables globales
var controlador = siteRoot + 'Migraciones/AnexoA/';
var USAR_COMBO_TIPO_RECURSO = false;
var idEmpresa = "";
var idCentral = "";
var idGeneracion = "";
var fechaInicio = "";
var fechaFin = "";

$(function () {

    $('#cbEmpresa').multipleSelect({
        width: '150px',
        filter: true,
        onClose: function (view) {
            cargarCentralxEmpresa();
        }
    });

    $('#btnBuscar').click(function () {
        cargarListaProduccionEnergiaDiaria();
    });

    $('#txtFechaInicio').Zebra_DatePicker({
        //direction: -1
    });

    $('#txtFechaFin').Zebra_DatePicker({
        //direction: -1
    });

    $('#btnGrafico').click(function () {
        cargarGrafico();

    });
    cargarValoresIniciales();
});


function cargarValoresIniciales() {
    $('#cbEmpresa').multipleSelect('checkAll');
    cargarCentralxEmpresa();
}

function cargarCentralxEmpresa() {
    validacionesxFiltro(1);
    if (resultFiltro) {
        $.ajax({
            type: 'POST',
            url: controlador + 'CargarCentralxEmpresa',
            data: { idEmpresa: $('#hfEmpresa').val() },
            success: function (aData) {
                $('#centrales').html(aData);
                $('#cbCentral').multipleSelect({
                    width: '150px',
                    filter: true,
                    onClose: function (view) {
                        //view = 2,
                        cargarTipoGeneracion();
                    }
                });
                $('#cbCentral').multipleSelect('checkAll');
                cargarTipoGeneracion();
            },
            error: function () {
                alert("Ha ocurrido un error");
            }
        });
    }
    else {

    }
}

function cargarTipoGeneracion() {
    validacionesxFiltro(1);
    if (resultFiltro) {
        $.ajax({
            type: 'POST',
            async: false,
            url: controlador + 'CargarTipoGeneracionxCentral',
            data: { equicodi: $('#hfCentral').val() },
            success: function (aData) {
                $('#TipoGeneracion').html(aData);
                $('#cbTipoGeneracion').multipleSelect({
                    width: '150px',
                    filter: true,
                    onClose: function (view) {
                        cargarListaProduccionEnergiaDiaria();
                    }
                });
                $('#cbTipoGeneracion').multipleSelect('checkAll');
                cargarListaProduccionEnergiaDiaria();
            },
            error: function () {
                alert("Ha ocurrido un error");
            }
        });
    }
    else {

    }
}

function cargarListaProduccionEnergiaDiaria() {
    validacionesxFiltro(2);
    if (resultFiltro) {
        $.ajax({
            type: 'POST',
            async: false,
            url: controlador + 'CargarListaProduccionEnergiaDiaria',
            data: { idEmpresa: idEmpresa, idCentral: idCentral, idGeneracion: idGeneracion, fechaInicio: fechaInicio, fechaFin: fechaFin },
            success: function (aData) {
                $('#listado').html(aData);
                $('#idGraficoContainer').html('');
            },
            error: function () {
                alert("Ha ocurrido un error");
            }
        });
    }
    else {
    }
}

function cargarGrafico() {
    validacionesxFiltro(2);
    if (resultFiltro) {
        $.ajax({
            type: 'POST',
            url: controlador + 'CargarGraficoProduccionEnergiaDiaria',
            data: { idEmpresa: idEmpresa, idCentral: idCentral, idGeneracion: idGeneracion, fechaInicio: fechaInicio, fechaFin: fechaFin },
            dataType: 'json',
            success: function (aData) {

                if (aData.Grafico.Series.length > 0) {
                    DiseñoGrafico(aData);
                    $('#idGraficoContainer').html("");
                }
                else {
                    $('#idGraficoContainer').html("No existen registros !");
                }
            },
            error: function () {
                alert("Ha ocurrido un error");
            }
        });
    }
    else {

    }

}

function DiseñoGrafico(result) {
    var seriesData = [];

    for (i = 0; i < result.Grafico.Series.length; i++) {
        var obj = {
            name: result.Grafico.SeriesName[i],
            y: result.Grafico.Series[i].Data[0].Y
        }
        seriesData.push(obj);
    }

    Highcharts.chart('idVistaGrafica', {
        chart: {
            type: 'column',
            inverted: true
        },
        title: {
            text: result.Grafico.TitleText
        },
        subtitle: {
            text: ''
        },
        xAxis: {
            type: 'category'
        },
        yAxis: {
            title: {
                text: ''
            },
            opposite: true,
            labels: {
                format: '{value}'
            }
        },
        legend: {
            enabled: false
        },
        plotOptions: {
            series: {
                borderWidth: 0,
                dataLabels: {
                    format: '{value}',
                    enabled: true
                }
            }
        },

        series: [{
            name: 'MWh',
            colorByPoint: true,
            data: seriesData
        }]
    });

    mostrarGrafico();
}

// Ventana flotante
function mostrarGrafico() {

    setTimeout(function () {
        $('#idGrafico2').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
            modalClose: false
        });
    }, 50);
}

//validaciones de los filtros de busqueda
function validacionesxFiltro(valor) {

    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    if (empresa == "[object Object]") empresa = "-1";
    $('#hfEmpresa').val(empresa);
    idEmpresa = $('#hfEmpresa').val();

    var central = $('#cbCentral').multipleSelect('getSelects');
    if (central == "[object Object]") central = "-1";
    $('#hfCentral').val(central);
    idCentral = $('#hfCentral').val();

    var generacion = $('#cbTipoGeneracion').multipleSelect('getSelects');
    if (generacion == "[object Object]") generacion = "-1";
    $('#hfTipoGeneracion').val(generacion);
    idGeneracion = $('#hfTipoGeneracion').val();

    fechaInicio = $('#txtFechaInicio').val();
    fechaFin = $('#txtFechaFin').val();


    var arrayFiltro = arrayFiltro || [];

    if (valor == 1) 
        arrayFiltro.push({ id: idEmpresa, mensaje: "Seleccione la opcion Empresa" }, { id: idCentral, mensaje: "Seleccione la opcion Central" });
    
    else 
        arrayFiltro.push({ id: idEmpresa, mensaje: "Seleccione la opcion Empresa" }, { id: idCentral, mensaje: "Seleccione la opcion Central" }, { id: idGeneracion, mensaje: "Seleccione la opcion Generación" });
        validarFiltros(arrayFiltro);
    
}