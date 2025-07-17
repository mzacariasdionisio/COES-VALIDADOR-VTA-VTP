var controlador = siteRoot + 'StockCombustibles/Reportes/'

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


    $('#cbAgente').multipleSelect({
        width: '150px',
        filter: true
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
        //$('#paginado').html("");
        //pintarPaginado(0);

        generarGrafico()
        //mostrarPaginado();
        //$('#btnExpotar').hide();

    });
    $('#btnExpotar').click(function () {
        exportarExcelReporte();

    });
    cargarPrevio();
    cargarAgentes();
    buscarDatos();
});

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

    var agente = $('#cbAgente').multipleSelect('getSelects');
    if (agente == "[object Object]") agente = "-1";
    if (agente == "") agente = "-1";

    var fechaInicio = $('#FechaInicio').val();
    var fechaFin = $('#FechaHasta').val();


    $('#hfTipoAgente').val(tipoAgente);
    $('#hfCentralInt').val(centralInt);
    $('#hfAgente').val(agente);

    $.ajax({
        type: 'POST',
        url: controlador + "listaQuemaGas",
        data: {
            idsTipoAgente: $('#hfTipoAgente').val(),
            idsCentralInt: $('#hfCentralInt').val(),
            idsAgente: $('#hfAgente').val(),
            idsFechaIni: fechaInicio,
            idsFechaFin: fechaFin
        },
        success: function (evt) {
            $('#listado').css("width", $('#mainLayout').width() + "px");

            $('#listado').html(evt);

            $('#tabla').dataTable({
                // "aoColumns": aoColumns(),                
                "bAutoWidth": false,
                "bSort": false,
                "scrollY": 430,
                "scrollX": true,
                "sDom": 't',
                "ordering": true,
                "iDisplayLength": 50
            });

        },
        error: function () {
            alert("Ha ocurrido un error en lista");
        }
    });
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
        url: controlador + 'GenerarReporteXLSQuema',
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
            if (result == 1) {//
                window.location = controlador + "ExportarReporte?tipo=8";
            }
            if (result == -1) {
                alert("Error en reporte result")
            }
            if (result == 2) {// Si no existen registros
                alert("No existen registros !");
            }
        },
        error: function () {
            alert("Error en reporte");;
        }
    });
}
