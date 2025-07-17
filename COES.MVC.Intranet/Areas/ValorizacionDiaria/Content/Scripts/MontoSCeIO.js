var controlador = siteRoot + 'ValorizacionDiaria/Consultas/';

$(function () {
    $('#txtFechaInicioSCeIO').Zebra_DatePicker({

    });
    $('#txtFechaFinalSCeIO').Zebra_DatePicker({

    });    
});

//btnBuscarMontoEnergia
$('#btnBuscarMontoSCeIO').click(function () {
        pintarPaginadoPorRangoFechaSCeIO();
        ListarMontoSCeIOPorRangoFecha(1);   
});

//btnExportarExcelME
$('#btnExportarExcelSCeIO').click(function () {
        ExportarMontoSCeIORangoFecha();      
});


pintarPaginadoPorRangoFechaSCeIO = function () {
    var emprcodi = $('#cbxEmpresaMSCeIO').val();
    var fechaInicio = $('#txtFechaInicioSCeIO').val();
    var fechaFinal = $('#txtFechaFinalSCeIO').val();
    $.ajax({
        type: 'POST',
        url: controlador + "PaginadoPorRangoFechaSCeIO",
        data: { emprcodi:emprcodi,fechaInicio: fechaInicio, fechaFinal: fechaFinal },
        success: function (evt) {
            $('#PaginadoMontoSCeIO').html(evt);
            mostrarPaginadoSCeIO();
        }
    });
}


var ListarMontoSCeIOPorRangoFecha = function (nroPagina) {
    var emprcodi = $('#cbxEmpresaMSCeIO').val();
    var fechaInicio = $('#txtFechaInicioSCeIO').val();
    var fechaFinal = $('#txtFechaFinalSCeIO').val();
    $.ajax({
        type: 'POST',
        url: controlador + "ListaMontoSceIO",
        data: { emprcodi:emprcodi,fechaInicio: fechaInicio, fechaFinal: fechaFinal, nroPagina: nroPagina },
        success: function (evt) {
            $('#ListadoMontoSCeIO').css("width", $('#mainLayout').width() + "px");
            $('#ListadoMontoSCeIO').html(evt);
        }
    });
};



pintarBusquedaPorRangoFechaSCeIO = function (nroPagina) {
    ListarMontoSCeIOPorRangoFecha(nroPagina);
}

IniciarTabs = function () {

    $(function () {
        $('#tab-container').easytabs();
    });

}

function ExportarMontoSCeIORangoFecha() {
    var emprcodi = $('#cbxEmpresaMSCeIO').val();
    var fechaInicio = $('#txtFechaInicioSCeIO').val();
    var fechaFinal = $('#txtFechaFinalSCeIO').val();
    var retorno = 0;

    $.ajax({
        type: 'POST',
        url: controlador + "GenerarReporteXLSPorRangoFechaSCeIO",
        data: { emprcodi:emprcodi,fechaInicio: fechaInicio, fechaFinal: fechaFinal },
        dataType: 'json',
        cache: false,
        async: false,
        success: function (resultado) {
            retorno = resultado;
            if (resultado == 1) {
                alert('Se Generó El Excel');
                location.href = controlador + "ExportarReporteSCeIO";
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