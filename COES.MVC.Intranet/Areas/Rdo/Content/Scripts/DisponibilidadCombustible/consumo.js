var controlador = siteRoot + 'Rdo/DisponibilidadCombustible/'
var getTipoGrafico = 1;
$(function () {
    $('#cbTipoAgente').multipleSelect({
        width: '150px',
        filter: true,
        onClose: function (view) {
            cargarAgentes();
        }
    });


    $('#cbCentralInt').multipleSelect({
        width: '150px',
        filter: true
    });

    $('#cbRecurso').multipleSelect({
        width: '150px',
        filter: true
    });

    $('#cbAgente').multipleSelect({
        width: '150px',
        filter: true
    });

    $('#cbEstado').multipleSelect({
        width: '150px',
        filter: true,
        onClose: function (view) {
            cargarRecursoEnergetico();
        }
    });

    $('#FechaInicio').Zebra_DatePicker({
        pair: $('#FechaHasta'),
        onSelect: function (date) {
            var date1 = getFecha(date);
            var date2 = getFecha($('#FechaHasta').val());

            if (date1 > date2) {
                $('#FechaHasta').val(date);
            }
        }
    });
    $('#FechaHasta').Zebra_DatePicker({
        direction: true
    });

    $('#btnBuscar').click(function () {
        $('#listado').html("");
        $('#paginado').html("");
        $('#btnExpotar').show();
        buscarDatos();
    });
    $('#btnGrafico').click(function () {
        generarGrafico(1);

    });
    $('#btnExpotar').click(function () {
        exportarExcelReporte();

    });
    cargarPrevio();
    cargarAgentes();
    cargarRecursoEnergetico();
    buscarDatos();
});


function exportarExcel() {
    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    var formato = $('#cbFormato').multipleSelect('getSelects');
    var lectura = $('#cbLectura').multipleSelect('getSelects');
    var estado = $('#cbEstado').multipleSelect('getSelects');

    if (empresa == "[object Object]") empresa = "-1";
    if (empresa == "") empresa = "-1";
    if (formato == "[object Object]") formato = "-1";
    if (formato == "") formato = "-1";
    if (lectura == "[object Object]") lectura = "-1";
    if (lectura == "") lectura = "-1";
    if (estado == "[object Object]") estado = "-1";
    if (estado == "") estado = "-1";
    $('#hfEmpresa').val(empresa);
    $('#hfFormato').val(formato);
    $('#hfLectura').val(lectura);
    $('#hfEstado').val(estado);

    $.ajax({
        type: 'POST',
        url: controlador + 'Envio/GenerarArchivoReporteXLS',
        data: {
            idsEmpresa: $('#hfEmpresa').val(),
            fechaIni: $('#FechaDesde').val(),
            fechaFin: $('#FechaHasta').val(),
            idsFormato: $('#hfFormato').val(),
            idsLectura: $('#hfLectura').val(),
            idsEstado: $('#hfEstado').val()
        },
        dataType: 'json',
        success: function (result) {
            if (result == 1)
                window.location = controlador + "Envio/ExportarReporte";
            if (result == -1)
                alert("Error en exportar reporte...");
        },
        error: function () {
            alert("Error en reporte...");
        }
    });
}



function cargarPrevio() {
    $('#cbTipoAgente').multipleSelect('checkAll');
    $('#cbCentralInt').multipleSelect('checkAll');
    $('#cbRecurso').multipleSelect('checkAll');
    $('#cbAgente').multipleSelect('checkAll');
    $('#cbEstado').multipleSelect('checkAll');
}

function buscarDatos() {
    //pintarPaginado(1);
    mostrarListado();
}

function mostrarListado() {
    var tipoAgente = $('#cbTipoAgente').multipleSelect('getSelects');
    if (tipoAgente == "[object Object]") tipoAgente = -1;
    if (tipoAgente == "") tipoAgente = "-1";

    var centralInt = $('#cbCentralInt').multipleSelect('getSelects');
    if (centralInt == "[object Object]") centralInt = -1;
    if (centralInt == "") centralInt = "-1";

    var recurso = $('#cbRecurso').multipleSelect('getSelects');
    if (recurso == "[object Object]") recurso = "-1";
    if (recurso == "") recurso = "-1";
    var agente = $('#cbAgente').multipleSelect('getSelects');
    if (agente == "[object Object]") agente = "-1";
    if (agente == "") agente = "-1";

    var estado = $('#cbEstado').multipleSelect('getSelects');
    if (estado == "[object Object]") estado = "-1";
    if (estado == "") estado = "-1";

    var fechaInicio = $('#FechaInicio').val();
    var fechaFin = $('#FechaHasta').val();

    var hora = $('#cbHorario').val()

    $('#hfTipoAgente').val(tipoAgente);
    $('#hfCentralInt').val(centralInt);
    $('#hfRecurso').val(recurso);
    $('#hfAgente').val(agente);
    $('#hfEstado').val(estado);

    $.ajax({
        type: 'POST',
        url: controlador + "listaConsumoCombustible",
        data: {
            idsTipoAgente: $('#hfTipoAgente').val(),
            idsCentralInt: $('#hfCentralInt').val(),
            idsRecurso: $('#hfRecurso').val(),
            idsAgente: $('#hfAgente').val(),
            idsEstado: $('#hfEstado').val(),
            idsFechaIni: fechaInicio,
            idsFechaFin: fechaFin,
            horario: hora
        },
        success: function (evt) {
            $('#listado').css("width", $('#mainLayout').width() + "px");

            $('#listado').html(evt);

            if ($('#tabla th').length > 1) {
                $('#tabla').dataTable({
                    "bAutoWidth": false,
                    "bSort": false,
                    "scrollY": 320,
                    "scrollX": true,
                    "sDom": 't',
                    "iDisplayLength": -1
                });
            }
            $("#tabla_wrapper").css("width", "100%");

        },
        error: function () {
            alert("Ha ocurrido un error en lista");
        }
    });
}

function generarGrafico(tipografico) {
    $('#reporte').css("display", "none");
    //$('#paginado').css("display", "none");       
    $('#excelGrafico').css("display", "block");
    //$('#excelGrafico').html("<div><img onclick='exportarGrafico();' width='32px' style='cursor: pointer; display: inline;' src='" + siteRoot + "Content/Images/ExportExcel.png' />" + 
    //    "<div>Rep01<input type='radio' name='rbMensaje' value='M' class='rbMensaje'/>Rep02<input type='radio' name='rbMensaje' value='N' class='rbMensaje'/> </div></div>");
    if (tipografico == 1) {
        getTipoGrafico = 1;
        $('#excelGrafico').html(getHtml());
    }
    else {
        getTipoGrafico = 2;
        $('#excelGrafico').html(getHtml());
    }

    $('#graficos').css("display", "block");
    $('#tab-container').css("display", "block");


    var tipoAgente = $('#cbTipoAgente').multipleSelect('getSelects');
    if (tipoAgente == "[object Object]") tipoAgente = -1;
    if (tipoAgente == "") tipoAgente = "-1";

    var centralInt = $('#cbCentralInt').multipleSelect('getSelects');
    if (centralInt == "[object Object]") centralInt = -1;
    if (centralInt == "") centralInt = "-1";

    var recurso = $('#cbRecurso').multipleSelect('getSelects');
    if (recurso == "[object Object]") recurso = "-1";
    if (recurso == "") recurso = "-1";

    var agente = $('#cbAgente').multipleSelect('getSelects');
    if (agente == "[object Object]") agente = "-1";
    if (agente == "") agente = "-1";

    var estado = $('#cbEstado').multipleSelect('getSelects');
    if (estado == "[object Object]") estado = "-1";
    if (estado == "") estado = "-1";

    var fechaInicio = $('#FechaInicio').val();
    var fechaFin = $('#FechaHasta').val();

    $('#hfTipoAgente').val(tipoAgente);
    $('#hfCentralInt').val(centralInt);
    $('#hfRecurso').val(recurso);
    $('#hfAgente').val(agente);
    $('#hfEstado').val(estado);
    $('#hfFecha').val(fechaInicio);
    $('#hfFechaHasta').val(fechaFin);

    $.ajax({
        type: 'POST',
        url: controlador + "graficoreporteConsumo",
        data: {
            idsTipoAgente: $('#hfTipoAgente').val(),
            idsCentralInt: $('#hfCentralInt').val(),
            idsRecurso: $('#hfRecurso').val(),
            idsAgente: $('#hfAgente').val(),
            idsEstado: $('#hfEstado').val(),
            idsFechaIni: fechaInicio,
            idsFechaFin: fechaFin
        },
        dataType: 'json',
        success: function (evt) {
            if (evt.Grafico.Series.length > 0) {// si existen registros
                $('#graficos').html(evt.Resultado);
                for (var i = 0; i < evt.ListaTipoPtoMedicion.length; i++) {
                    var divgrafico = '#grafico' + evt.ListaTipoPtoMedicion[i].Tipoptomedinomb;
                    var idli = '#li' + evt.ListaTipoPtoMedicion[i].Tipoptomedinomb;
                    var resultado = null;
                    if (tipografico == 1) // Gráfico Evolucion
                        resultado = graficoConsumo(evt, divgrafico, evt.ListaTipoPtoMedicion[i].Tptomedicodi);
                    else
                        resultado = graficoConsumoPie(evt, divgrafico, evt.ListaTipoPtoMedicion[i].Tptomedicodi);
                    if (resultado == 0) {
                        $(divgrafico).css("display", "none");
                        $(idli).css("display", "none");
                    }
                }
            }
            else {// No existen registros
                $('#excelGrafico').html("No existen registros !");
                $('#graficos').css("display", "none");
                //alert("No existen registros !");

            }
        },
        error: function () {
            alert("Ha ocurrido un error en generar grafico");
        }
    });
}

//IMPRIME GRAFICO TIPO EVOLUCION (DE LINEAS)
graficoConsumo = function (result, divgrafico, tipoptomedicodi) {
    var series = [[]];
    var serieName = [];
    var z = 0;
    for (i = 0; i < result.Grafico.Series.length; i++) {
        if (result.Grafico.Series[i].TipoPto == tipoptomedicodi) {
            series[z] = [];
            serieName[z] = result.Grafico.Series[i].Name;
            for (k = 0; k < result.Grafico.SerieDataS[i].length; k++) {
                var seriePoint = [];
                var now = parseJsonDate(result.Grafico.SerieDataS[i][k].X);

                var nowUTC = Date.UTC(now.getFullYear(), now.getMonth(), now.getDate(), now.getHours(), now.getMinutes(), now.getSeconds());

                seriePoint.push(nowUTC);
                seriePoint.push(result.Grafico.SerieDataS[i][k].Y);
                series[z].push(seriePoint);
            }
            z++;
        }
    }

    var opcion = {
        rangeSelector: {
            selected: 1
        },

        title: {
            text: result.Grafico.TitleText,
            style: {
                fontSize: '8'
            }
        },
        yAxis: [{
            title: {
                text: result.Grafico.YAxixTitle[0]
            },
            min: 0
        },
        {
            title: {
                text: result.Grafico.YAxixTitle[1]
            },
            opposite: false

        }],
        legend: {
            layout: 'horizontal',
            align: 'center',
            verticalAlign: 'bottom',
            borderWidth: 0,
            enabled: true
        },
        series: []
    };

    for (i = 0; i < serieName.length; i++) {
        opcion.series.push({
            name: serieName[i],
            data: series[i],
        });
    }
    $(divgrafico).highcharts('StockChart', opcion);
    return serieName.length;


}

//GRAFICO TIPO pie
graficoConsumoPie = function (result, divgrafico, tipoptomedicodi) {
    var series = [];
    var jsondata = [];
    var serieName = [];
    var z = 0;
    for (i = 0; i < result.Grafico.Series.length; i++) {
        if (result.Grafico.Series[i].TipoPto == tipoptomedicodi) {
            serieName[z] = result.Grafico.Series[i].Name;
            series.push(result.Grafico.Series[i].Acumulado);
            z++;
        }
    }

    for (i = 0; i < serieName.length; i++) {
        jsondata.push([serieName[i], series[i]]);
    }

    var opcion = {
        chart: {
            plotBackgroundColor: null,
            plotBorderWidth: null,
            plotShadow: false,
            type: 'pie'
        },
        title: {
            text: result.Grafico.TitleText
        },
        tooltip: {
            pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
        },
        plotOptions: {
            pie: {
                allowPointSelect: true,
                cursor: 'pointer',
                dataLabels: {
                    enabled: true,
                    format: '<b>{point.name}</b>: {point.percentage:.1f} %',
                    style: {
                        color: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black'
                    }
                },
            }

        },
        series: [{
            name: 'Participación',
            colorByPoint: true,
            data: jsondata
        }]
    };


    $(divgrafico).highcharts(opcion);
    return serieName.length;
}

function parseJsonDate(jsonDateString) {
    return new Date(parseInt(jsonDateString.replace('/Date(', '')));
}

mostrarPaginado = function () {

    $.ajax({
        type: 'POST',
        url: controlador + 'Paginado',
        success: function (evt) {
            $('#paginado').html(evt);
            mostrarPaginado();
        },
        error: function () {
            alert("Error paginado");
        }
    });
}


function cargarAgentes() {
    var tipoAgente = $('#cbTipoAgente').multipleSelect('getSelects');

    if (tipoAgente == "[object Object]") tipoAgente = "-1";
    $('#hfTipoAgente').val(tipoAgente);
    $.ajax({
        type: 'POST',
        url: controlador + 'CargarAgentes',

        data: { idTipoAgente: $('#hfTipoAgente').val() },

        success: function (aData) {
            $('#agentes').html(aData);
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function cargarRecursoEnergetico() {
    var estadoFisico = $('#cbEstado').multipleSelect('getSelects');
    var codReporte = 2;
    if (estadoFisico == "[object Object]") estadoFisico = "-1";
    $('#hfEstado').val(estadoFisico);
    if ($('#hfEstado').val() != "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'CargarRecursoEnergetico',

            data: {
                idEstadoFisico: $('#hfEstado').val(),
                iCodReporte: codReporte
            },

            success: function (aData) {
                $('#recurso').html(aData);
            },
            error: function () {
                alert("Ha ocurrido un error");
            }
        });
    }
    else
        alert("Debe seleccionar Estado Fisico de Combustle !");
}

function exportarExcelReporte() {
    var tipoAgente = $('#cbTipoAgente').multipleSelect('getSelects');
    if (tipoAgente == "[object Object]") tipoAgente = -1;
    if (tipoAgente == "") tipoAgente = "-1";

    var centralInt = $('#cbCentralInt').multipleSelect('getSelects');
    if (centralInt == "[object Object]") centralInt = -1;
    if (centralInt == "") centralInt = "-1";

    var recurso = $('#cbRecurso').multipleSelect('getSelects');
    if (recurso == "[object Object]") recurso = "-1";
    if (recurso == "") recurso = "-1";

    var agente = $('#cbAgente').multipleSelect('getSelects');
    if (agente == "[object Object]") agente = "-1";
    if (agente == "") agente = "-1";

    var estado = $('#cbEstado').multipleSelect('getSelects');
    if (estado == "[object Object]") estado = "-1";
    if (estado == "") estado = "-1";

    var fechaInicio = $('#FechaInicio').val();
    var fechaFin = $('#FechaHasta').val();


    $('#hfTipoAgente').val(tipoAgente);
    $('#hfCentralInt').val(centralInt);
    $('#hfRecurso').val(recurso);
    $('#hfAgente').val(agente);
    $('#hfEstado').val(estado);

    $.ajax({
        type: 'POST',
        url: controlador + 'GenerarReporteXLSConsumo',
        data: {
            idsTipoAgente: $('#hfTipoAgente').val(),
            idsCentralInt: $('#hfCentralInt').val(),
            idsRecurso: $('#hfRecurso').val(),
            idsAgente: $('#hfAgente').val(),
            idsEstado: $('#hfEstado').val(),
            idsFechaIni: fechaInicio,
            idsFechaFin: fechaFin
        },
        dataType: 'json',
        success: function (result) {
            if (result == 1) {// si hay elementos
                window.location = controlador + "ExportarReporte?tipo=2";
            }
            if (result == 2) { // sino hay elementos
                alert("No existen registros !");
            }
            if (result == -1) {
                alert("Error en reporte result")
            }
        },
        error: function () {
            alert("Error en reporte");;
        }
    });
}

function exportarGrafico() {
    $.ajax({
        type: 'POST',
        url: controlador + "GenerarArchivoGraficoConsumo",
        data: {
            fechaInicial: $('#hfFecha').val(),
            fechaFinal: $('#hfFechaHasta').val(),
            tipoGrafico: getTipoGrafico
        },
        dataType: 'json',
        success: function (result) {
            if (result == 1) {
                window.location = controlador + "ExportarReporte?tipo=5";
            }
            if (result == -1) {
                alert("Error en generar datos archivo grafico !");
            }
            if (result == 2) {// no existen registros
                alert("No existen registros !");
            }
        },
        error: function () {
            alert("Error en exportar grafico a Excel");
        }
    });
}

function getHtml() {
    var html = "";
    //html = "<table><tr><td><img onclick='exportarGrafico();' width='32px' style='cursor: pointer; display: inline;' src='" + siteRoot + "Content/Images/ExportExcel.png' /></td>";
    //html = html + "<td>Gráfico Evolución<input type='radio' name='rbMensaje' value='1' class='rbMensaje' onclick='generarGrafico(1);'" + ckek1 + "/>";
    //html = html + "</td><td>Gráfico Acumulado<input type='radio' name='rbMensaje' value='2' class='rbMensaje' onclick='generarGrafico(2);'" + chek2 + "/> </td></tr></table>";
    html = "<div class='search-content' style='margin-bottom:0px; padding:10px; padding-top:3px; padding-bottom:9px; display:block'>";
    html = html + "<table cellpadding='0' cellspacing='0' border='0'>";
    html = html + "<tr><td class='content-action'><a onclick ='exportarGrafico();' href='#'>";
    html = html + "<div class='content-item-action'><img src='../../../Areas/StockCombustibles/Content/Images/ExportExcel.png' /><br/><span>Exportar</span></div></a></td>";
    html = html + "<td class='content-action'><a onclick = 'generarGrafico(1);' href='#' id='btnGraficoLineas'><div class='content-item-action'><img src='/../../../Areas/StockCombustibles/Content/Images/lines.png'/><br />";
    html = html + "<span>Evolución</span></div></a></td>";
    html = html + "<td class='content-action'><a onclick = 'generarGrafico(2);' href='#' id='btnGraficoAcumulado'><div class='content-item-action'><img src='../../../Areas/StockCombustibles/Content/Images/pie.png'/><br/>";
    html = html + "<span>Acumulado</span></div></a></td></tr></table></div>";
    return html;
}
