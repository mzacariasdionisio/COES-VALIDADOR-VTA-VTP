//variables globales
var controlador = siteRoot + 'IEOD/AnexoA/'

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

function mostrarReporteByFiltros() {
    cargarListaCaudalesCentralHidroelectrica();
}

function listarPuntoMedicion() {
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
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function cargarListaCaudalesCentralHidroelectrica() {
    $("#listado1").html('');

    validacionesxFiltro(1);

    var idEmpresa = getEmpresa();
    var fechaInicio = getFechaInicio();
    var fechaFin = getFechaFin();
    var idFamilia = getFamilia();

    if (resultFiltro) {
        $.ajax({
            type: 'POST',
            url: controlador + 'CargarListaCaudalesCentralHidroelectrica',
            data: {
                idEmpresa: idEmpresa, fechaInicio: fechaInicio, fechaFin: fechaFin, nroPagina: 1,
                idsFamilia: idFamilia
            },
            success: function (aData) {
                $('.filtro_fecha_desc').html(aData.FiltroFechaDesc);

                var ancho = $("#mainLayout").width() > 900 ? $("#mainLayout").width() - 30 : 900;

                $('#listado1').html(aData.Resultado);
                var anchoReporte = $('#reporte').width();
                var scrollX = anchoReporte > ancho;
                $("#resultado").css("width", (scrollX ? ancho : anchoReporte) + 10 + "px");
                $('#reporte').dataTable({
                    "autoWidth": false,
                    "scrollX": scrollX,
                    "scrollY": "550px",
                    "scrollCollapse": false,
                    "sDom": 't',
                    "ordering": false,
                    paging: false,
                    fixedColumns: {
                        leftColumns: 1
                    }
                });
            },
            error: function (err) {
                alert("Ha ocurrido un error");
            }
        });
    }
}

function generarGrafico() {
    validacionesxFiltro(1);
    var idEmpresa = getEmpresa();
    var idPtoMedicion = getPtoMedicion();
    var idCuenca = getCuenca();
    var idFamilia = getFamilia();
    var fechaInicio = getFechaInicio();
    var fechaFin = getFechaFin();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarGraficoCaudalesCentralHidroelectrica',
        data: {
            idEmpresa: idEmpresa, fechaInicio: fechaInicio, fechaFin: fechaFin, nroPagina: 1,
            idsPtoMedicion: idPtoMedicion, idsCuenca: idCuenca, idsFamilia: idFamilia
        },
        dataType: 'json',
        success: function (result) {
            if (result.Total > 0) {
                graficoHSHidrologiaM24(result);
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error en generar grafico");
        }
    });
}

//validaciones de los filtros de busqueda
function validacionesxFiltro(valor) {
    var idEmpresa = getEmpresa();

    var arrayFiltro = arrayFiltro || [];

    if (valor == 1)
        arrayFiltro.push({ id: idEmpresa, mensaje: "Seleccione la opcion Empresa" });

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