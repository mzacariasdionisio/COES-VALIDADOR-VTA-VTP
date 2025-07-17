
var controlador = siteRoot + 'ValorizacionDiaria/Consultas/';

$(function () {
    $('#txtFechaInicioMEx').Zebra_DatePicker({

    });
    $('#txtFechaFinalMEx').Zebra_DatePicker({

    });
});

//btnBuscarMontoPorExceso
$('#btnBuscarMontoPorExceso').click(function () {
    pintarPaginadoPorRangoFechaMEx();
    ListarMontoExcesoPorRangoFecha(1);
});

//btnExportarExcelMEx
$('#btnExportarExcelMEx').click(function () {
    ExportarMontoPorExcesoRangoFecha();
});


pintarPaginadoPorRangoFechaMEx = function () {
    var emprcodi = $('#cbxEmpresaMEx').val();
    var fechaInicio = $('#txtFechaInicioMEx').val();
    var fechaFinal = $('#txtFechaFinalMEx').val();
    $.ajax({
        type: 'POST',
        url: controlador + "PaginadoPorRangoFechaMEx",
        data: { emprcodi:emprcodi,fechaInicio: fechaInicio, fechaFinal: fechaFinal },
        success: function (evt) {
            $('#PaginadoMontoExceso').html(evt);
            mostrarPaginadoMExceso();
        }
    });
}


var ListarMontoExcesoPorRangoFecha = function (nroPagina) {
    var emprcodi = $('#cbxEmpresaMEx').val();
    var fechaInicio = $('#txtFechaInicioMEx').val();
    var fechaFinal = $('#txtFechaFinalMEx').val();
    $.ajax({
        type: 'POST',
        url: controlador + "ListaMontoPorExceso",
        data: { emprcodi:emprcodi,fechaInicio: fechaInicio, fechaFinal: fechaFinal, nroPagina: nroPagina },
        success: function (evt) {
            $('#ListadoMontoPorExceso').css("width", $('#mainLayout').width() + "px");
            $('#ListadoMontoPorExceso').html(evt);
        }
    });
};



pintarBusquedaPorRangoFechaMExceso = function (nroPagina) {
    ListarMontoExcesoPorRangoFecha(nroPagina);
}

IniciarTabs = function () {

    $(function () {
        $('#tab-container').easytabs();
    });

}

function ExportarMontoPorExcesoRangoFecha() {
    var emprcodi = $('#cbxEmpresaMEx').val();
    var fechaInicio = $('#txtFechaInicioMEx').val();
    var fechaFinal = $('#txtFechaFinalMEx').val();
    var retorno = 0;

    $.ajax({
        type: 'POST',
        url: controlador + "GenerarReporteXLSPorRangoFechaMEx",
        data: {emprcodi:emprcodi, fechaInicio: fechaInicio, fechaFinal: fechaFinal },
        dataType: 'json',
        cache: false,
        async: false,
        success: function (resultado) {
            retorno = resultado;
            if (resultado == 1) {
                alert('Se Genero El Excel');
                location.href = controlador + "ExportarReporteMEx";
            } else {
                alert('Error al Generar Excel');
            }
        },
        error: function () {
            alert('Ocurrió un problema al generar el archivo EXCEL');
        }
    });

    return retorno;
};