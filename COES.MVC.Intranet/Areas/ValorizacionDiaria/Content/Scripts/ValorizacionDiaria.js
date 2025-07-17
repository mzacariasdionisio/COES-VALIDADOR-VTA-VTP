var controlador = siteRoot + 'ValorizacionDiaria/Consultas/';
function t(x) { return document.getElementById(x); }

$(function () {
    $('#txtFechaInicioVD').Zebra_DatePicker({

    });
    $('#txtFechaFinalVD').Zebra_DatePicker({

    });
});

//
$('#btnBuscarValorizacionD').click(function () {
    pintarPaginadoValorizacionDiaria();
    ListarValorizacionDiaria(1);
    
});

//btnExportarExcelVD
$('#btnExportarExcelVD').click(function () {
    ExportarValorizacionDiaria();
});

pintarPaginadoValorizacionDiaria = function () {
    var emprcodi = $('#cbxEmpresa').val();
    var fechaInicio = $('#txtFechaInicioVD').val();
    var fechaFinal = $('#txtFechaFinalVD').val();
    $.ajax({
        type: 'POST',
        url: controlador + "PaginadoPorFiltroValorizacionDiaria",
        data: { emprcodi: emprcodi,fechaInicio: fechaInicio, fechaFinal: fechaFinal },
        success: function (evt) {
            $('#PaginadoValorizacionDiaria').html(evt);
            mostrarPaginadoVD();
        }
    });
}

var ListarValorizacionDiaria = function (nroPagina) {
    var emprcodi = $('#cbxEmpresa').val();
    var fechaInicio = $('#txtFechaInicioVD').val();
    var fechaFinal = $('#txtFechaFinalVD').val();
    $.ajax({
        type: 'POST',
        url: controlador + "ListaValorizacionDiaria",
        data: { emprcodi: emprcodi,fechaInicio: fechaInicio, fechaFinal: fechaFinal, nroPagina: nroPagina },
        success: function (evt) {
            $('#ListadoValorizacionDiaria').css("width", $('#mainLayout').width() + "px");
            $('#ListadoValorizacionDiaria').html(evt);
        }
    });
};

pintarBusquedaPorRangoFechaVD = function (nroPagina) {
    ListarValorizacionDiaria(nroPagina);
}

function ExportarValorizacionDiaria() {
    var emprcodi = $('#cbxEmpresa').val();
    var fechaInicio = $('#txtFechaInicioVD').val();
    var fechaFinal = $('#txtFechaFinalVD').val();
    var retorno = 0;

    $.ajax({
        type: 'POST',
        url: controlador + "GenerarReporteXLSValorizacionDiaria",
        data: { emprcodi: emprcodi,fechaInicio: fechaInicio, fechaFinal: fechaFinal },
        dataType: 'json',
        cache: false,
        async: false,
        success: function (resultado) {
            retorno = resultado;
            if (resultado == 1) {
                alert('Se Genero El Excel');
                location.href = controlador + "ExportarReporteVD";
            } else {
                alert('Error al Generar Excel');
            }
        },
        error: function () {
            alert('Ocurrio un problema al generar el archivo EXCEL');
        }
    });

    return retorno;
};