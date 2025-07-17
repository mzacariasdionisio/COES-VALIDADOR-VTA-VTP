//variables globales
var controlador = siteRoot + 'Migraciones/AnexoA/'
var idEmpresa = "";
var idCentral = "";
var fechaInicio = "";
var fechaFin = "";

$(function () {
    $('#cbEmpresa').multipleSelect({
        width: '150px',
        filter: true,
        onClose: function (view) {
            listarPuntoMedicion();
        }
    });
    $('#cbFamilia').multipleSelect({
        width: '150px',
        filter: true,
        onClose: function (view) {
            listarPuntoMedicion();
        }
    });
    $('#btnGrafico').click(function () {
        generarGrafico();
    });
    cargarValoresIniciales();
});

function cargarValoresIniciales() {
    $('#cbEmpresa').multipleSelect('checkAll');
    $('#cbFamilia').multipleSelect('checkAll');
    listarPuntoMedicion();
}

function listarPuntoMedicion() {
    //var unidad = $('#cbUnidades').val();
    //$('#hfUnidad').val(unidad);
    $.ajax({
        type: 'POST',
        url: controlador + "ListarPuntosMedicion",
        data: {
            //iUnidad: $('#hfUnidad').val()
        },
        success: function (evt) {
            $('#listPuntoMedicion').html(evt);
            cargarListaCaudalesCentralHidroelectrica();
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function cargarListaCaudalesCentralHidroelectrica() {
    validacionesxFiltro(2);
    
    var familia = $('#cbFamilia').multipleSelect('getSelects');
    if (familia == "[object Object]") familia = "-1";
    if (familia == "") familia = "-1";
    $('#hfFamilia').val(familia);

    if (resultFiltro) {
        $.ajax({
            type: 'POST',
            url: controlador + 'CargarListaCaudalesCentralHidroelectrica',
            data: {
                idEmpresa: idEmpresa, fechaInicio: fechaInicio, fechaFin: fechaFin, nroPagina: 1,
                idsFamilia: $('#hfFamilia').val()
            },
            success: function (aData) {
               
                $('#listado1').css("width", $('#mainLayout').width() + "px");

                $('#listado1').html(aData.Resultado);

                $('#tabla').dataTable({
                    // "aoColumns": aoColumns(),
                    "bAutoWidth": false,
                    "bSort": false,
                   // "scrollY": 430,
                    "scrollX": true,
                    "sDom": 't',
                    "iDisplayLength": 60
                });
            },
            error: function () {
                alert("Ha ocurrido un error");
            }
        });
    }
}

function generarGrafico(nroPagina) {
    validacionesxFiltro(2);

    var ptomedicion = $('#cbPtoMedicion').multipleSelect('getSelects');
    if (ptomedicion == "[object Object]") ptomedicion = "-1";
    if (ptomedicion == "") ptomedicion = "-1";
    $('#hfPtoMedicion').val(ptomedicion);

    var cuenca = $('#cbCuenca').multipleSelect('getSelects');
    if (cuenca == "[object Object]") cuenca = "-1";
    if (cuenca == "") cuenca = "-1";
    $('#hfCuenca').val(cuenca);

    var familia = $('#cbFamilia').multipleSelect('getSelects');
    if (familia == "[object Object]") familia = "-1";
    if (familia == "") familia = "-1";
    $('#hfFamilia').val(familia);

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarGraficoCaudalesCentralHidroelectrica',
        data: {
            idEmpresa: idEmpresa, fechaInicio: fechaInicio, fechaFin: fechaFin, nroPagina: 1,
            idsPtoMedicion: $('#hfPtoMedicion').val(), idsCuenca: $('#hfCuenca').val(), idsFamilia: $('#hfFamilia').val()
        },
        dataType: 'json',
        success: function (result) {
            if (result.Total > 0) {
                graficoHSHidrologiaM24(result);
            }
        },
        error: function () {
            alert("Ha ocurrido un error en generar grafico");
        }
    });
}

//validaciones de los filtros de busqueda
function validacionesxFiltro(valor) {
    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    if (empresa == "[object Object]") empresa = "-1";
    $('#hfEmpresa').val(empresa);
    idEmpresa = $('#hfEmpresa').val();

    var centrales = $('#cbCentrales').multipleSelect('getSelects');
    if (centrales == "[object Object]") centrales = "-1";
    $('#hfCentrales').val(centrales);
    idCentral = $('#hfCentrales').val();


    fechaInicio = $('#txtFechaInicio').val();
    fechaFin = $('#txtFechaFin').val();


    var arrayFiltro = arrayFiltro || [];

    if (valor == 1)
        arrayFiltro.push({ id: idEmpresa, mensaje: "Seleccione la opcion Empresa" });
    else
        arrayFiltro.push({ id: idEmpresa, mensaje: "Seleccione la opcion Empresa" }, { id: idCentral, mensaje: "Seleccione la opcion Central" });

    validarFiltros(arrayFiltro);
}

function graficoHSHidrologiaM24(result) {
    var series = [];
    var itemseries = [];
    for (z = 0; z < result.Grafico.Series.length; z++) {
        {
            for (k = 0; k < result.Grafico.Series[z].Data.length; k++) {
                var seriePoint = [];
                var now = parseJsonDate(result.Grafico.Series[z].Data[k].X);
                var nowUTC = Date.UTC(now.getFullYear(), now.getMonth(), now.getDate(), now.getHours(), now.getMinutes(), now.getSeconds());
                seriePoint.push(nowUTC);
                seriePoint.push(result.Grafico.Series[z].Data[k].Y);
                itemseries.push(seriePoint);
            }
            series.push(itemseries);
            itemseries = [];
        }
    }
    // alert(series.length);
    var opcion = {


        title: {
            text: result.Grafico.titleText,
            style: {
                fontSize: '8'
            }
        },
        yAxis: {
            title: {
                text: result.Grafico.YaxixTitle,
                style: {
                    color: 'blue'
                }
            },
            labels: {
                style: {
                    color: 'blue'
                }
            },
            opposite: false
        },
        legend: {
            borderColor: "#909090",
            layout: 'vertical',
            align: 'left',
            verticalAlign: 'top',
            borderWidth: 0,
            enabled: true,
            itemStyle: {
                fontSize: "9px"
            }
        },

        series: []
    };
    for (i = 0; i < series.length; i++) {
        opcion.series.push({
            name: result.Grafico.Series[i].Name,
            //color: result.Grafico.Series[i].Color,
            data: series[i],
            type: result.Grafico.Series[i].Type
        });
    }
    $('#idVistaGrafica').highcharts('StockChart', opcion);

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