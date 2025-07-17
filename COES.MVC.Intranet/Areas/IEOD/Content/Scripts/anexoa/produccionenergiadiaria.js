var USAR_COMBO_TIPO_RECURSO = false;

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

    $('#btnGrafico').click(function () {
        setTimeout(function () {
            $('#idGrafico2').bPopup({
                easing: 'easeOutBack',
                speed: 450,
                transition: 'slideDown',
                modalClose: false
            });
        }, 50);
    });

    ancho = $("#mainLayout").width() > 900 ? $("#mainLayout").width() - 30 : 900;

    cargarValoresIniciales();
});

function cargarValoresIniciales() {
    $('#cbEmpresa').multipleSelect('checkAll');
    cargarCentralxEmpresa();
}

function mostrarReporteByFiltros() {
    cargarListaProduccionEnergiaDiaria();
}

function cargarCentralxEmpresa() {
    var idEmpresa = getEmpresa();
    validacionesxFiltro(1);
    if (resultFiltro) {
        $.ajax({
            type: 'POST',
            url: controlador + 'CargarCentralxEmpresa',
            data: { idEmpresa: idEmpresa },
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
            error: function (err) {
                alert("Ha ocurrido un error");
            }
        });
    }
    else {

    }
}

function cargarTipoGeneracion() {
    var idCentral = getCentral();
    validacionesxFiltro(1);
    if (resultFiltro) {
        $.ajax({
            type: 'POST',
            async: false,
            url: controlador + 'CargarTipoGeneracionxCentral',
            data: { equicodi: idCentral },
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
            error: function (err) {
                alert("Ha ocurrido un error");
            }
        });
    }
    else {

    }
}

function cargarListaProduccionEnergiaDiaria() {
    var idEmpresa = getEmpresa();
    var idCentral = getCentral();
    var idTipoGeneracion = getTipoGeneracion();
    var fechaInicio = getFechaInicio();
    var fechaFin = getFechaFin();

    validacionesxFiltro(2);
    if (resultFiltro) {
        $.ajax({
            type: 'POST',
            url: controlador + 'CargarListaProduccionEnergiaDiaria',
            data: {
                idEmpresa: idEmpresa, idCentral: idCentral, idGeneracion: idTipoGeneracion, fechaInicio: fechaInicio, fechaFin: fechaFin
            },
            success: function (listaModel) {
                var aData = listaModel[0];
                $('.filtro_fecha_desc').html(aData.FiltroFechaDesc);

                $('#listado_S').html(aData.Resultado);
                $('#listado_N').html(aData.Resultado2);

                var anchoReporte = $('#reporte_S').width();
                $("#resultado_S").css("width", (anchoReporte > ancho ? ancho : anchoReporte) + "px");

                var noTieneDataS = parseInt($("#no_tiene_intg_S").val()) || 0;
                if (noTieneDataS == 0) {
                    $('#reporte_S').dataTable({
                        "scrollX": true,
                        "scrollY": "450px",
                        "scrollCollapse": true,
                        "sDom": 't',
                        "ordering": false,
                        paging: false,
                        fixedColumns: {
                            leftColumns: 4
                        }
                    });
                }

                var anchoReporte2 = $('#reporte_N').width();
                $("#resultado_N").css("width", (anchoReporte2 > ancho ? ancho : anchoReporte2) + "px");

                var noTieneDataN = parseInt($("#no_tiene_intg_N").val()) || 0;
                if (noTieneDataN == 0) {
                    $('#reporte_N').dataTable({
                        "scrollX": true,
                        "scrollY": "780px",
                        "scrollCollapse": true,
                        "sDom": 't',
                        "ordering": false,
                        paging: false,
                        fixedColumns: {
                            leftColumns: 4
                        }
                    });
                }

                DisenioGrafico(listaModel[1]);
            },
            error: function (err) {
                alert("Ha ocurrido un error");
            }
        });
    }
}

function DisenioGrafico(result) {
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
            type: 'category',
            labels: {
                style: {
                    fontSize: 8
                }
            },
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
}

//validaciones de los filtros de busqueda
function validacionesxFiltro(valor) {
    var idEmpresa = getEmpresa();
    var idCentral = getCentral();
    var idTipoGeneracion = getTipoGeneracion();

    var arrayFiltro = arrayFiltro || [];

    if (valor == 1)
        arrayFiltro.push({ id: idEmpresa, mensaje: "Seleccione la opcion Empresa" }, { id: idCentral, mensaje: "Seleccione la opcion Central" });

    else
        arrayFiltro.push({ id: idEmpresa, mensaje: "Seleccione la opcion Empresa" }, { id: idCentral, mensaje: "Seleccione la opcion Central" }, { id: idTipoGeneracion, mensaje: "Seleccione la opcion Generación" });
    validarFiltros(arrayFiltro);
}