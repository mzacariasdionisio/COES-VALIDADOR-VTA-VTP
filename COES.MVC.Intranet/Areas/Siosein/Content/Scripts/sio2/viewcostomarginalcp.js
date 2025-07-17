var controlador = siteRoot + 'Siosein/Numerales/'

$(function () {
    $('#txtFecha').Zebra_DatePicker({
        onSelect: function () {
            consultaCMCP();
            $('#txtFechaInicio').val($('#txtFecha').val());
            $('#txtFechaFin').val($('#txtFecha').val());
        }
    });
    $('#txtFechaInicio').Zebra_DatePicker();
    $('#txtFechaFin').Zebra_DatePicker();

    $('#btnExportar').click(function () {
        openPopup();
    });

    $('#btnExportarXls').click(function () {
        exportarExcel();
        $('#txtFechaInicio').val($('#txtFecha').val());
        $('#txtFechaFin').val($('#txtFecha').val());
    });

    $('#btnExportarNulos').click(function () {
        exportarValoresNulos();
    });

    consultaCMCP();
});

function openPopup() {
    setTimeout(function () {
        $('#popupTablaNuevo').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
            modalClose: false
        });
    }, 50);
}

function consultaCMCP() {
    $.ajax({
        type: 'POST',
        url: controlador + "ConsultaCMCP",
        dataType: 'json',
        data: {
            fecha: $('#txtFecha').val(), url: siteRoot
        },
        success: function (evt) {
            $("#listado2").html('');
            $("#listado1").html(evt.Resultado);
            $("#cmcp").dataTable({
                "ordering": false,
                "bLengthChange": false,
                "bInfo": false,
                "bPaginate": false
            });
        },
        error: function (err) { alert("Error..!!"); }
    });
}

function exportarExcel() {
    $.ajax({
        type: 'POST',
        url: controlador + "ExportarExcelCMmasivo",
        dataType: 'json',
        data: {
            fechaini: $('#txtFechaInicio').val(), fechafin: $('#txtFechaFin').val()
        },
        success: function (result) {
            if (result.IdEnvio > 0) {
                window.location = controlador + "ExportarReporteXls?nameFile=" + result.Resultado;
            } else { alert("No existe informacion a exportar"); }
        },
        error: function (err) { alert("Error..!!"); }
    });
}

function exportarValoresNulos() {
    $.ajax({
        type: 'POST',
        url: controlador + "ExportarValoresNulos",
        dataType: 'json',
        data: {
            fechaini: $('#txtFechaInicio').val(), fechafin: $('#txtFechaFin').val()
        },
        success: function (result) {
            if (result.IdEnvio > 0) {
                window.location = controlador + "DescargarValoresNulos?nameFile=" + result.Resultado;
            } else { alert("No existe informacion a exportar"); }
        },
        error: function (err) { alert("Error..!!"); }
    });
}

function view(x, y) {
    $.ajax({
        type: 'POST',
        url: controlador + "ConsultaCMCPdet",
        dataType: 'json',
        data: {
            fecha: x, hora: y
        },
        success: function (evt) {
            $("#listado2").html(evt.Resultado);
            $("#cmcpdet").dataTable({
                "ordering": false,
                "bLengthChange": false,
                "bInfo": false,
                "pageLength": 80
            });
        },
        error: function (err) { alert("Error..!!"); }
    });
}
