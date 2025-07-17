var controlador = siteRoot + 'IEOD/';
var listFormatCodi = [];
var listFormatPeriodo = [];
$(function () {
    $('#cbEmpresa').multipleSelect({
        width: '222px',
        filter: true
    });
    $('#cbEmpresa').multipleSelect('checkAll');

    $('#cbFormato').change(function () {
        cargarEmpresas();
    });
    $('#fechaInicio').Zebra_DatePicker({

    });
   

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

    cargarEmpresas();
});


function mostrarListado() {
    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    if (empresa == "[object Object]") empresa = "-1";
    if (empresa == "") empresa = "-1";
    var formato = $('#cbFormato').val();
    $('#hfEmpresa').val(empresa);

    $.ajax({
        type: 'POST',
        url: controlador + "CambioEnvio/lista",
        data: {
            sEmpresas: $('#hfEmpresa').val(),
            idFormato: formato,
            fInicio: $('#fechaInicio').val()
        },
        success: function (evt) {
            $('#listado').css("width", $('#mainLayout').width() + "px");
            $('#listado').html(evt);
            $('#tabla').dataTable({
                "bAutoWidth": false,
                "bSort": false,
                "scrollY": 320,
                "scrollX": true,
                "sDom": 't',
                "iDisplayLength": 50,
                "fixedColumns": true
            });
            $("#tabla_wrapper").css("width", "100%");
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function exportarExcel() {
    var empresa = $('#cbEmpresa').multipleSelect('getSelects');
    if (empresa == "[object Object]") empresa = "-1";
    if (empresa == "") empresa = "-1";
    var formato = $('#cbFormato').val();
    $('#hfEmpresa').val(empresa);
    $.ajax({
        type: 'POST',
        url: controlador + 'CambioEnvio/GenerarArchivoReporteCumplimiento',
        data: {
            sEmpresas: $('#hfEmpresa').val(),
            idFormato: formato,
            mes: $('#mes').val()
        },
        dataType: 'json',
        success: function (result) {
            if (result == 1)
                window.location = controlador + "CambioEnvio/ExportarReporte";
            if (result == -1)
                alert("Error en exportar reporte...");
        },
        error: function () {
            alert("Error en reporte...");
        }
    });
}

function cargarEmpresas() {
    var formato = $('#cbFormato').val();
    $.ajax({
        type: 'POST',
        url: controlador + 'CambioEnvio/CargarEmpresas',

        data: { idFormato: formato },

        success: function (aData) {
            $('#empresas').html(aData);
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}