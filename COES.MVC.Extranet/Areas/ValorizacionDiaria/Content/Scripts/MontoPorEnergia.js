var controlador = siteRoot + 'ValorizacionDiaria/Consultas/';

$(function () {
    $('#txtFechaInicioMR').Zebra_DatePicker({

    });
    $('#txtFechaFinalMR').Zebra_DatePicker({

    });    
});

//btnBuscarMontoEnergia
$('#btnBuscarMontoEnergia').click(function () {
        pintarPaginadoPorRangoFechaME();
        ListarMontoEnergiaPorRangoFecha(1);   
});

//btnExportarExcelME
$('#btnExportarExcelME').click(function () {
        ExportarMontoPorEnergiaRangoFecha();      
});


pintarPaginadoPorRangoFechaME = function () {
    var emprcodi = $('#cbxEmpresaME').val();
    var fechaInicio = $('#txtFechaInicioMR').val();
    var fechaFinal = $('#txtFechaFinalMR').val();
    $.ajax({
        type: 'POST',
        url: controlador + "PaginadoPorRangoFechaME",
        data: { emprcodi:emprcodi,fechaInicio: fechaInicio, fechaFinal: fechaFinal },
        success: function (evt) {
            $('#PaginadoMontoEnergia').html(evt);
            mostrarPaginadoME();
        }
    });
}


var ListarMontoEnergiaPorRangoFecha = function (nroPagina) {
    var emprcodi = $('#cbxEmpresaME').val();
    var fechaInicio = $('#txtFechaInicioMR').val();
    var fechaFinal = $('#txtFechaFinalMR').val();
    $.ajax({
        type: 'POST',
        url: controlador + "ListaMontoPorEnergia",
        data: {emprcodi:emprcodi, fechaInicio: fechaInicio, fechaFinal: fechaFinal, nroPagina: nroPagina },
        success: function (evt) {
            $('#ListadoMontoPorEnergia').css("width", $('#mainLayout').width() + "px");
            $('#ListadoMontoPorEnergia').html(evt);
        }
    });
};

pintarBusquedaPorRangoFechaME = function (nroPagina) {
    ListarMontoEnergiaPorRangoFecha(nroPagina);
}

IniciarTabs = function () {

    $(function () {
        $('#tab-container').easytabs();
    });

}

function ExportarMontoPorEnergiaRangoFecha() {
    var emprcodi = $('#cbxEmpresaME').val();
    var fechaInicio = $('#txtFechaInicioMR').val();
    var fechaFinal = $('#txtFechaFinalMR').val();
    var retorno = 0;

    $.ajax({
        type: 'POST',
        url: controlador + "GenerarReporteXLSPorRangoFechaME",
        data: { emprcodi:emprcodi,fechaInicio: fechaInicio, fechaFinal: fechaFinal },
        dataType: 'json',
        cache: false,
        async: false,
        success: function (resultado) {
            retorno = resultado;
            if (resultado == 1) {
                alert('Se Generó El Excel');
                location.href = controlador + "ExportarReporteME";
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