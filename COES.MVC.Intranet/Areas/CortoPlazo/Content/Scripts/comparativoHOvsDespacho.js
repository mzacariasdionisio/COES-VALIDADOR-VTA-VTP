var controlador = siteRoot + 'CortoPlazo/Comparativo/';
var FILTRO_FECHA_INICIO = '01/01/2000';
var FILTRO_FECHA_HOY = '';

$(function () {
    FILTRO_FECHA_HOY = $("#fechaPeriodo").val();

    $('#fechaPeriodo').Zebra_DatePicker({
        direction: [FILTRO_FECHA_INICIO, FILTRO_FECHA_HOY],
        onSelect: function () {
            cargarListaFiltro(1);
        }
    });

    $('#cbEmpresa').change(function () {
        cargarListaFiltro(2);
    });

    $('#cbCentral').change(function () {
        cargarListaFiltro(3);
    });
    $('#cbModo').change(function () {
    });

    //
    $('#btnConsultar').click(function () {
        cargarComparativoHOvsDespacho();
    });
    $('#btnExportarExcel').click(function () {
        generarExcelComparativoHOvsDespacho();
    });
});

///////////////////////////
/// filtros 
///////////////////////////

function cargarListaFiltro(tipoFiltro) {
    $("#mensaje").hide();

    var empresa = getEmpresa();
    var central = getCentral();
    if (tipoFiltro == 1) { //filtro fecha
        empresa = -1;
        central = -1;
    }

    if (tipoFiltro == 2) //filtro empresa
        central = -1;

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarFiltroHOvsDespacho',
        dataType: 'json',
        data: {
            strFecha: $("#fechaPeriodo").val(),
            emprcodi: empresa,
            equipadre: central,
        },
        cache: false,
        success: function (data) {
            if (data.Resultado != "-1") {
                if (tipoFiltro == 1) {
                    $('#cbEmpresa').empty();
                    $('#cbEmpresa').append("<option value='-1'>--SELECCIONE--</option>");

                    if (data.ListaEmpresa.length > 0) {
                        $.each(data.ListaEmpresa, function (i, item) {
                            $('#cbEmpresa').get(0).options[$('#cbEmpresa').get(0).options.length] = new Option(item.Emprnomb, item.Emprcodi);
                        });
                    }
                }

                if (tipoFiltro == 1 || tipoFiltro == 2) {
                    $('#cbCentral').empty();
                    $('#cbCentral').append("<option value='-1'>--SELECCIONE--</option>");

                    if (data.ListaCentral.length > 0) {
                        if (data.ListaCentral.length == 1)
                            $('#cbCentral').empty();

                        $.each(data.ListaCentral, function (i, item) {
                            $('#cbCentral').get(0).options[$('#cbCentral').get(0).options.length] = new Option(item.Equinomb, item.Equicodi);
                        });
                    }
                }

                $('#cbModo').empty();
                $('#cbModo').append("<option value='-1'>--SELECCIONE--</option>");
                if (tipoFiltro == 1 || tipoFiltro == 2 || tipoFiltro == 3) {
                    if (getCentral() > 0 && data.ListaModo.length > 0) {
                        if (data.ListaModo.length == 1)
                            $('#cbModo').empty();

                        $.each(data.ListaModo, function (i, item) {
                            $('#cbModo').get(0).options[$('#cbModo').get(0).options.length] = new Option(item.Gruponomb, item.Grupocodi);
                        });
                    }
                }

            } else {
                alert("Ha ocurrido un error: " + data.Mensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

function getEmpresa() {
    var idEmpresa = $('#cbEmpresa').val() || 0;

    return idEmpresa;
}

function getCentral() {
    var central = $('#cbCentral').val() || 0;

    return central;
}

function getModo() {
    var modo = $('#cbModo').val() || 0;

    return modo;
}

///////////////////////////
/// web 
///////////////////////////
function cargarComparativoHOvsDespacho() {
    $("#mensaje").hide();

    if (getModo() > 0) {
        $.ajax({
            type: 'POST',
            url: controlador + 'CargarComparativoHOvsDespacho',
            dataType: 'json',
            data: {
                strFecha: $("#fechaPeriodo").val(),
                emprcodi: getEmpresa(),
                equipadre: getCentral(),
                grupocodi: getModo(),
            },
            cache: false,
            success: function (data) {
                if (data.Resultado != "-1") {
                    $("#listado").html('');

                    var html = '';
                    if (data.ListaReporte.length > 0) {
                        for (var i = 0; i < data.ListaReporte.length; i++) {
                            console.log(data.ListaReporte[i]);
                            var htmlTabla = data.ListaReporte[i].ReporteHtml;
                            html += `
                                <div style=" float: left; ">
                                    ${htmlTabla}
                                </div>

                                <div style=" float: left; width: 50%;height: 700px; background-color: #F2F5F7; border: 1px solid #DDDDDD; padding:15px; border-radius:5px;">
                                    <div id="grafico_${i}" style='height: 550px'></div>
                                    <div style='margin-top: 10px; padding: 5px; background: white;'><b>Nota:</b> Pasar el cursor sobre los puntos del gráfico para ver el detalle adicional. </div>
                                </div>
                            ` ;
                        }
                    }

                    $("#listado").html(html);

                    if (data.ListaReporte.length > 0) {
                        for (var i = 0; i < data.ListaReporte.length; i++) {
                            pintarGrafico(data.ListaReporte[i].Grafico, "#grafico_" + i);
                        }
                    }
                    $('.tbl_comparativo').dataTable({
                        "sPaginationType": "full_numbers",
                        "destroy": "true",
                        "ordering": false,
                        "searching": false,
                        "iDisplayLength": -1,
                        "info": false,
                        "paging": false,
                        "scrollX": true,
                    });
                } else {
                    alert("Ha ocurrido un error: " + data.Mensaje);
                }
            },
            error: function (err) {
                alert('Ha ocurrido un error.');
            }
        });
    } else {
        $("#listado").html('');

        $("#mensaje").show();
        mostrarMensaje("mensaje", "Debe seleccionar un modo de operación antes de consultar", $tipoMensajeAlerta, $modoMensajeCuadro);
    }
}

function pintarGrafico(grafico, idGrafico) {
    var seriesHo = [];
    var seriesDesp = [];

    //horas de operacion
    if (grafico.SerieDataS[0][0] != null) {
        for (k = 0; k < grafico.SerieDataS[0].length; k++) {
            seriesHo.push({
                //x: grafico.XAxisCategories[k],
                y: grafico.SerieDataS[0][k].Y,
                color: grafico.Series[0].Color,
                descripcionHO: grafico.SerieDataS[0][k].Type
            });
        }
    }

    //despacho ejecutado
    if (grafico.SerieDataS[1][0] != null) {
        for (k = 0; k < grafico.SerieDataS[1].length; k++) {
            seriesDesp.push({
                //x: grafico.XAxisCategories[k],
                y: grafico.SerieDataS[1][k].Y,
                color: grafico.Series[1].Color
            });
        }
    }

    var series = [];
    series.push({
        name: grafico.Series[0].Name,
        data: seriesHo,
        color: grafico.Series[0].Color,
        type: 'line'
    });
    series.push({
        name: grafico.Series[1].Name,
        data: seriesDesp,
        color: grafico.Series[1].Color,
        type: 'line'
    });

    $(idGrafico).highcharts({
        chart: {
            type: 'line'
        },
        title: {
            text: grafico.TitleText,
            x: -20
        },
        xAxis: {
            categories: grafico.XAxisCategories,
            labels: {
                rotation: -90
            },
            crosshair: true
        },
        yAxis: {
            title: {
                text: grafico.YAxixTitle
            },
            plotLines: [{
                value: 0,
                width: 1,
                color: '#808080'
            }],
            min: 0
        },
        tooltip: {
            //pointFormat: '<span style="color:{series.color}">{series.name}</span>: <b>{point.y:,.2f}</b> <br/>',
            formatter: function () {
                var pointHo = this.points[0];
                var pointDesp = this.points[1];
                var descHo = pointHo.point.descripcionHO.replace(/(?:\r\n|\r|\n)/g, '<br>');

                return `
                    <b>${this.x}</b> <br/>
                    <span style="color: ${pointHo.color}">${pointHo.point.series.name}</span>: <b>${Highcharts.numberFormat(pointHo.y, 2)}MW</b> <br/>
                    <span style="color: ${pointDesp.color}">${pointDesp.point.series.name}</span>: <b>${Highcharts.numberFormat(pointDesp.y, 2)}MW</b> <br/>
                     - <br/>
                    ${descHo}
                `;
            },
            shared: true
        },
        legend: {
            borderWidth: 0
        },
        series: series
    });
}

///////////////////////////
/// excel 
///////////////////////////

function generarExcelComparativoHOvsDespacho() {
    $("#mensaje").hide();

    if (getModo() > 0) {
        $.ajax({
            type: 'POST',
            url: controlador + "GenerarExcelComparativoHOvsDespacho",
            data: {
                strFecha: $("#fechaPeriodo").val(),
                emprcodi: getEmpresa(),
                equipadre: getCentral(),
                grupocodi: getModo(),
            },
            success: function (evt) {
                if (evt.Resultado != "-1") {
                    window.location = controlador + "Exportar?file_name=" + evt.Resultado;
                } else {
                    alert("Ha ocurrido un error: " + evt.Mensaje);
                }
            },
            error: function (err) {
                alert('Ha ocurrido un error.');
            }
        });
    } else {
        $("#mensaje").show();
        mostrarMensaje("mensaje", "Debe seleccionar un modo de operación", $tipoMensajeAlerta, $modoMensajeCuadro);
    }
}