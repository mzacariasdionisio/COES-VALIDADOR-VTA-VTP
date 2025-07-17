var controlador = siteRoot + 'hidrologia/';
var listFormatCodi = [];
var listFormatPeriodo = [];
$(function () {
    $('#cbEmpresa').multipleSelect({
        width: '222px',
        filter: true
    });
    $('#cbEmpresa').multipleSelect('checkAll');
    $('#cbLectura').change(function () {
        listarFormato();
    });
    $('#cbFormato').change(function () {
        horizonte();
    });
    $('#FechaDesde').Zebra_DatePicker({

    });

    $('#FechaHasta').Zebra_DatePicker({

    });
    getPeriodoDefault();

    $('#txtMes').Zebra_DatePicker({
        format: 'm Y'
    });
    $('#txtMes2').Zebra_DatePicker({
        format: 'm Y'
    });
    $('#Anho').Zebra_DatePicker({
        format: 'Y',
        onSelect: function () {
            cargarSemanaAnho()
        }
    });
    $('#Anho2').Zebra_DatePicker({
        format: 'Y',
        onSelect: function () {
            cargarSemanaAnho()
        }
    });
    
   
    $('#Anho').val(periodo.anho);
    $('#cbSemana1').val($('#hfPrueba').val());
    $('#Anho2').val(periodo.anho);
    $('#cbSemana2').val($('#hfPrueba').val());
    cargarSemanaAnho();
    cargarSemanaAnho2();

    $('#btnBuscar').click(function () {
        if ($('#cbFormato').val() > 0) {
            mostrarListado();
        }
        else {
            alert("Error: Seleccionar Formato");
        }
    });

    $('#btnExportar').click(function () {
        exportarExcel();
    });

    var strFormatCodi = $('#hfFormatCodi').val();
    var strFormatPeriodo = $('#hfFormatPeriodo').val();
    listFormatCodi = strFormatCodi.split(',');
    listFormatPeriodo = strFormatPeriodo.split(',');

    listarFormato();
   // mostrarListado();
});

function listarFormato() {
    $.ajax({
        type: 'POST',
        url: controlador + "Cumplimiento/ListarFormatosXLectura",
        dataType: 'json',
        cache: false,
        data: {
            sLectura: $('#cbLectura').val()
        },
        success: function (aData) {
            $('#cbFormato').get(0).options.length = 0;
            $('#cbFormato').get(0).options[0] = new Option("--SELECCIONE--", "");
            $.each(aData, function (i, item) {
                $('#cbFormato').get(0).options[$('#cbFormato').get(0).options.length] = new Option(item.Text, item.Value);
            });
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });

}

function mostrarListado() {
    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    if (empresa == "[object Object]") empresa = "-1";
    if (empresa == "") empresa = "-1";
    semana1 = $("#cbSemana1").val();
    anho = $("#Anho").val();
    semana1 = anho.toString() + semana1;
    semana2 = $("#cbSemana2").val();
    anho2 = $("#Anho2").val();
    semana2 = anho2.toString() + semana2;
    var formato = $('#cbFormato').val();
    $('#hfEmpresa').val(empresa);
    $.ajax({
        type: 'POST',
        url: controlador + "Cumplimiento/lista",
        data: {
            sEmpresas: $('#hfEmpresa').val(),
            idFormato: formato,
            fIni: $('#FechaDesde').val(),
            fFin: $('#FechaHasta').val(),
            mes1: $('#txtMes').val(),
            mes2: $('#txtMes2').val(),
            semana1:    semana1,
            semana2:    semana2
        },
        success: function (evt) {
           //$('#listado').css("width", $('#mainLayout').width() + "px");
            $('#listado').html(evt);
            $('#tabla').dataTable({
                scrollY: 600,
                scrollX: true,
                scrollCollapse: true,
                paging: false,
                fixedColumns: true
            });
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function horizonte() {

    var opcion = buscarPeriodo($('#cbFormato').val());
    switch (parseInt(opcion)) {
        case 1: //dia
            $('#cntFecha').css("display", "block");
            $('#cntSemana').css("display", "none");
            $('#fechasSemana').css("display", "none");
            $('#cntMes').css("display", "none");
            $('#cntFecha2').css("display", "block");
            $('#cntSemana2').css("display", "none");
            $('#fechasSemana2').css("display", "none");
            $('#cntMes2').css("display", "none");
            break;
        case 2: //semanal
            $('#cntFecha').css("display", "none");
            $('#cntSemana').css("display", "block");
            $('#fechasSemana').css("display", "block");
            $('#cntMes').css("display", "none");
            $('#cntFecha2').css("display", "none");
            $('#cntSemana2').css("display", "block");
            $('#fechasSemana2').css("display", "block");
            $('#cntMes2').css("display", "none");
            cargarSemanaAnho();
            break;
            //mensual

            //break;
        case 3: case 5:
            $('#cntFecha').css("display", "none");
            $('#cntSemana').css("display", "none");
            $('#fechasSemana').css("display", "none");
            $('#cntMes').css("display", "block");
            $('#cntFecha2').css("display", "none");
            $('#cntSemana2').css("display", "none");
            $('#fechasSemana2').css("display", "none");
            $('#cntMes2').css("display", "block");
            break;
    }
}

function buscarPeriodo(valor) {// valor: formatCodi
    for (var i = 0 ; i < listFormatCodi.length; i++)
        if (listFormatCodi[i] == valor) return listFormatPeriodo[i];
}

// Llena lista de semanas del año seleccionado
function cargarSemanaAnho() {
    var anho = $('#Anho').val();
    $.ajax({
        type: 'POST',
        dataType: 'json',
        url: controlador + 'Cumplimiento/CargarSemanas',
        cache: false,
        data: {
            idAnho: anho
        },
        success: function (aData) {
            $('#cbSemana1').get(0).options.length = 0;
            $.each(aData, function (i, item) {
                $('#cbSemana1').get(0).options[$('#cbSemana1').get(0).options.length] = new Option(item.Text, item.Value);
            });
            $('#cbSemana1').val($('#hfPrueba').val());
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });

}

// Llena lista de semanas del año seleccionado
function cargarSemanaAnho2() {
    var anho = $('#Anho2').val();
    $.ajax({
        type: 'POST',
        dataType: 'json',
        url: controlador + 'Cumplimiento/CargarSemanas',
        cache: false,
        data: {
            idAnho: anho
        },
        success: function (aData) {
            $('#cbSemana2').get(0).options.length = 0;
            $.each(aData, function (i, item) {
                $('#cbSemana2').get(0).options[$('#cbSemana2').get(0).options.length] = new Option(item.Text, item.Value);
            });
            $('#cbSemana2').val($('#hfPrueba').val());
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
    
}

/// Setea por defecto el año mes y dia al iniciar el aplicativo
function getPeriodoDefault() {
    var hoy = new Date();
    var dd = hoy.getDate();
    var mm = hoy.getMonth() + 1; //hoy es 0!
    var yyyy = hoy.getFullYear();

    if (dd < 10) {
        dd = '0' + dd
    }

    if (mm < 10) {
        mm = '0' + mm
    }
    periodo.anho = yyyy;
    periodo.mes = $("#hfMes").val();

}

function exportarExcel() {
    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    if (empresa == "[object Object]") empresa = "-1";
    if (empresa == "") empresa = "-1";
    semana1 = $("#cbSemana-1").val();
    anho = $("#Anho").val();
    semana1 = anho.toString() + semana1;
    semana2 = $("#cbSemana-2").val();
    anho2 = $("#Anho2").val();
    semana2 = anho2.toString() + semana2;
    var formato = $('#cbFormato').val();
    $('#hfEmpresa').val(empresa);

    $.ajax({
        type: 'POST',
        url: controlador + 'Cumplimiento/GenerarReporteCumplimiento',
        data: {
            sEmpresas: $('#hfEmpresa').val(),
            idFormato: formato,
            fIni: $('#FechaDesde').val(),
            fFin: $('#FechaHasta').val(),
            mes1: $('#txtMes').val(),
            mes2: $('#txtMes2').val(),
            semana1: semana1,
            semana2: semana2
        },
        dataType: 'json',
        success: function (result) {
            if (result == 1)
                window.location = controlador + "Cumplimiento/ExportarReporte";
            if (result == -1)
                alert("Error en exportar reporte...");
        },
        error: function () {
            alert("Error en reporte...");
        }
    });

}
