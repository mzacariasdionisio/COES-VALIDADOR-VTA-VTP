//variables globales
var controlador = siteRoot + 'Migraciones/AnexoA/'
var idEmpresa = "";
var idUbicacion = "";
var idEquipo = "";
var fechaInicio = "";
var fechaFin = "";
var anchoListado = 900;

$(function () {
    $('#cbEmpresa').multipleSelect({
        width: '150px',
        filter: true,
        onClose: function (view) {
            //cargarEquipo();
            cargarListaDemandaGrandesUsuarios();
        }
    });

    $('#btnBuscar').click(function () {
        cargarListaDemandaGrandesUsuarios();
    });

    $('#txtFechaInicio').Zebra_DatePicker({
        //direction: -1
    });

    $('#txtFechaFin').Zebra_DatePicker({
        //direction: -1
    });

    $('#btnGraficos').click(function () {
        cargarGrafico();
    });

    anchoListado = $("#mainLayout").width() - 20;

    cargarValoresIniciales();
});

function cargarValoresIniciales() {
    $('#cbEmpresa').multipleSelect('checkAll');
    //cargarUbicacion();
    cargarListaDemandaGrandesUsuarios();
}

function cargarListaDemandaGrandesUsuarios() {
    validacionesxFiltro(2);
    if (resultFiltro) {
        $.ajax({
            type: 'POST',
            url: controlador + 'CargarListaDemandaGrandesUsuarios',
            data: { idEmpresa: idEmpresa, idUbicacion: idUbicacion, idEquipo: idEquipo, fechaInicio: fechaInicio, fechaFin: fechaFin },
            success: function (aData) {
                if (aData !== undefined && aData != null && aData.length > 0) {
                    cargarListado(aData[0]);
                    cargarGrafico(1, aData[1]);
                    cargarGrafico(2, aData[2]);
                    cargarGrafico(3, aData[3]);
                    cargarGrafico(4, aData[4]);
                } else {
                    alert("Ha ocurrido un error");
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

function cargarListado(aData) {
    $('#listado').html(aData.Resultado);
    $('#idGraficoContainer').html('');
    $("#div_reporte").css("width", anchoListado+ "px");
    $("#div_reporte").css("overflow", "auto");
    $('#reporte').dataTable({
        "scrollX": true,
        "scrollCollapse": true,
        "sDom": 't',
        "ordering": false,
        paging: false,
        fixedColumns: {
            leftColumns: 1
        }
    });
}

function cargarGrafico(tipoGrafico, aData) {
    if (aData.Grafico.Series.length > 0) {
        var id = 'idVistaGrafica' + tipoGrafico;
        $("#"+id).css("width", anchoListado + "px");
        DisenioGrafico(aData, id);
    }
    else {
        $('#idGraficoContainer').html("No existen registros !");
    }
}

function DisenioGrafico(result, id) {
    //generar series
    var series = [];
    for (var i = 0; i < result.Grafico.Series.length; i++) {
        var serie = result.Grafico.Series[i];
        var obj = {
            name: serie.Name,
            type: serie.Type,
            yAxis: serie.YAxis,
            data: result.Grafico.SeriesData[i],
            color: serie.Color
        };

        series.push(obj);
    }
    var dataHora = result.Grafico.XAxisCategories;
    var tituloGrafico = result.Grafico.TitleText;

    //Generar grafica
    Highcharts.chart(id, {
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
            categories: dataHora,
            crosshair: true
        }],
        yAxis: {
            title: {
                text: 'MW'
            },
            labels: {
                format: '{value}',
            }
        },
        tooltip: {
            shared: true
        },
        legend: {
            align: 'center',
            verticalAlign: 'bottom',
            layout: 'horizontal'
        },
        plotOptions: {
            spline: {
                lineWidth: 1,
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
        series: series
    });
}

//validaciones de los filtros de busqueda
function validacionesxFiltro(valor) {

    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    if (empresa == "[object Object]") empresa = "-1";
    $('#hfEmpresa').val(empresa);
    idEmpresa = $('#hfEmpresa').val();

    var ubicacion = $('#cbUbicacion').multipleSelect('getSelects');
    if (ubicacion == "[object Object]") ubicacion = "-1";
    $('#hfUbicacion').val(ubicacion);
    idUbicacion = $('#hfUbicacion').val();

    var equipo = $('#cbEquipo').multipleSelect('getSelects');
    if (equipo == "[object Object]") equipo = "-1";
    $('#hfEquipo').val(equipo);
    idEquipo = $('#hfEquipo').val();

    fechaInicio = $('#txtFechaInicio').val();
    fechaFin = $('#txtFechaFin').val();

    var arrayFiltro = arrayFiltro || [];

    if (valor == 1)
        arrayFiltro.push({ id: idEmpresa, mensaje: "Seleccione la opcion Empresa" }, { id: idUbicacion, mensaje: "Seleccione la opcion Área" });
    else
        arrayFiltro.push({ id: idEmpresa, mensaje: "Seleccione la opcion Empresa" }, { id: idUbicacion, mensaje: "Seleccione la opcion Área" }, { id: idEquipo, mensaje: "Seleccione la opcion Equipo" });

    validarFiltros(arrayFiltro);
}