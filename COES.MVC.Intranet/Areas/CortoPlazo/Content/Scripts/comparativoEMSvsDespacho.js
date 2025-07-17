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
    $('#cbGrupo').change(function () {
    });

    //
    $('#btnConsultar').click(function () {
        cargarComparativoEMSvsDespacho();
    });
    $('#btnExportarExcel').click(function () {
        generarExcelComparativoEMSvsDespacho();
    });

    //
    $('#txtExportarDesde').Zebra_DatePicker({
        pair: $('#txtExportarHasta'),
        onSelect: function (date) {
            $('#txtExportarHasta').val(date);
        },
        direction: [FILTRO_FECHA_INICIO, FILTRO_FECHA_HOY],
    });
    $('#txtExportarHasta').Zebra_DatePicker({
        direction: [true, FILTRO_FECHA_HOY],
    });
    $('#btnExportarDiferencia').click(function () {
        //$('#txtExportarDesde').val($('#fechaPeriodo').val());
        //$('#txtExportarHasta').val($('#fechaPeriodo').val());
        $('#divExportar').css('display', 'block');
    });
    $('#btnProcesarFile').click(function () {
        generarExcelDiferenciaEMSvsDespacho();
    });

    //mostrar inconsistencias
    var tieneVal = parseInt($("#tieneVal").val()) || 0;
    if (tieneVal > 0)
        $("#div_validaciones").show();
});

function closeExportar() {
    $('#divExportar').css('display', 'none');
}
///////////////////////////
/// filtros 
///////////////////////////

function cargarListaFiltro(tipoFiltro) {
    $("#mensaje").hide();
    $("#div_validaciones").hide();

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
        url: controlador + 'CargarFiltroEMSvsDespacho',
        dataType: 'json',
        data: {
            strFecha: $("#fechaPeriodo").val(),
            emprcodi: empresa,
            grupopadre: central,
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

                    if (data.ListaGrupoCentral.length > 0) {
                        if (data.ListaGrupoCentral.length == 1)
                            $('#cbCentral').empty();

                        $.each(data.ListaGrupoCentral, function (i, item) {
                            $('#cbCentral').get(0).options[$('#cbCentral').get(0).options.length] = new Option(item.Gruponomb, item.Grupocodi);
                        });
                    }
                }

                $('#cbGrupo').empty();
                $('#cbGrupo').append("<option value='-1'>--SELECCIONE--</option>");
                if (tipoFiltro == 1 || tipoFiltro == 2 || tipoFiltro == 3) {
                    if (getCentral() > 0 && data.ListaGrupoDespacho.length > 0) {
                        if (data.ListaGrupoDespacho.length == 1)
                            $('#cbGrupo').empty();

                        $.each(data.ListaGrupoDespacho, function (i, item) {
                            $('#cbGrupo').get(0).options[$('#cbGrupo').get(0).options.length] = new Option(item.Gruponomb, item.Grupocodi);
                        });
                    }
                }


                var listaMsj = data.ListaMensajeValidacion;
                if (listaMsj.length > 0) {

                    var strMsj = "";
                    for (var k = 0; k < listaMsj.length; k++) {
                        strMsj += `<li>${listaMsj[k]}</li>`;
                    }

                    var html = `
                        <ul>
                            ${strMsj}
                        </ul>
                    `;

                    $("#div_validaciones").show();
                    $("#div_validaciones").html(html);
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

function getGrupo() {
    var modo = $('#cbGrupo').val() || 0;

    return modo;
}

///////////////////////////
/// web 
///////////////////////////
function cargarComparativoEMSvsDespacho() {
    $("#mensaje").hide();

    if (getGrupo() > 0) {
        $.ajax({
            type: 'POST',
            url: controlador + 'CargarComparativoEMSvsDespacho',
            dataType: 'json',
            data: {
                strFecha: $("#fechaPeriodo").val(),
                emprcodi: getEmpresa(),
                grupopadre: getCentral(),
                grupocodi: getGrupo(),
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

                            var listaMsj = data.ListaReporte[i].DatoXGrupo.ListaMensaje;
                            if (listaMsj.length > 0) {

                                var strMsj = "";
                                for (var k = 0; k < listaMsj.length; k++) {
                                    strMsj += `<li>${listaMsj[k]}</li>`;
                                }

                                html += `
                                    <div style=" float: left;width: 100%; " class="action-alert">
                                        <ul>
                                            ${strMsj}
                                        </ul>
                                    </div>
                                `;
                            }

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
                        $("#mensaje").show();
                        mostrarMensaje("mensaje", "No se encontraron datos para los filtros seleccionados", $tipoMensajeAlerta, $modoMensajeCuadro);
                    }

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
        mostrarMensaje("mensaje", "Debe seleccionar un Grupo de despacho antes de consultar", $tipoMensajeAlerta, $modoMensajeCuadro);
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
            pointFormat: '<span style="color:{series.color}">{series.name}</span>: <b>{point.y:,.2f}MW</b> <br/>',
            formatter: function () {
                var pointHo = this.points[0];
                var pointDesp = this.points[1];
                var descHo = '';
                if (pointHo !== undefined && pointHo.point !== undefined && pointHo.point.descripcionHO !== undefined)
                    descHo = pointHo.point.descripcionHO.replace(/(?:\r\n|\r|\n)/g, '<br>');

                var htmlDesc = `
                    <b>${this.x}</b> <br/>
                    <span style="color: ${pointHo.color}">${pointHo.point.series.name}</span>: <b>${Highcharts.numberFormat(pointHo.y, 2)}MW</b> <br/>
                `;
                if (pointDesp !== undefined) {
                    htmlDesc += `
                    <span style="color: ${pointDesp.color}">${pointDesp.point.series.name}</span>: <b>${Highcharts.numberFormat(pointDesp.y, 2)}MW</b> <br/>
                    `;
                }
                htmlDesc += `
                     - <br/>
                    ${descHo}
                    `;

                return htmlDesc;
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

function generarExcelComparativoEMSvsDespacho() {
    $("#mensaje").hide();

    if (getGrupo() > 0) {
        $.ajax({
            type: 'POST',
            url: controlador + "GenerarExcelComparativoEMSvsDespacho",
            data: {
                strFecha: $("#fechaPeriodo").val(),
                emprcodi: getEmpresa(),
                grupopadre: getCentral(),
                grupocodi: getGrupo(),
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
        mostrarMensaje("mensaje", "Debe seleccionar un Grupo de despacho", $tipoMensajeAlerta, $modoMensajeCuadro);
    }
}

function generarExcelDiferenciaEMSvsDespacho() {
    $("#mensaje").hide();
    $("#mensaje_exportar_datos").hide();

    $.ajax({
        type: 'POST',
        url: controlador + "GenerarExcelDiferenciaEMSvsDespacho",
        data: {
            strFechaIni: $("#txtExportarDesde").val(),
            strFechaFin: $("#txtExportarHasta").val(),
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                window.location = controlador + "Exportar?file_name=" + evt.Resultado;
            } else {
                $("#mensaje_exportar_datos").show();
                mostrarMensaje("mensaje_exportar_datos", evt.Mensaje, $tipoMensajeAlerta, $modoMensajeCuadro);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

