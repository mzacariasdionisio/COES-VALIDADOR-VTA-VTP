var controlador = siteRoot + 'ValorizacionDiaria/Consultas/';

$(function () {
    $('#txtFechaInicioMC').Zebra_DatePicker({
        format: 'M Y',
    });
    $('#txtFechaFinalMC').Zebra_DatePicker({
        format: 'M Y',
    });
});

//btnBuscarMontoCapacidad
$('#btnBuscarMontoCapacidad').click(function () {

    //pintarPaginadoPorRangoFechaMC();
    ListarMontoCapacidadPorRangoFecha(1);
});

//btnExportarExcelMC
$('#btnExportarExcelMC').click(function () {
        ExportarMontoPorCapacidadRangoFecha();
});

function diasEnUnMesMC(mes, año) {
    return new Date(año, mes, 0).getDate();
}

FechaInicioMC = function () {
    var fechaInicio = $('#txtFechaInicioMC').val();
    var fechaInicios = "";
    var fechaMes = fechaInicio.split(" ", 1);
    var fechaAnio = fechaInicio.substr(4, 7);

    fechaMes = fechaMes[0];

    var number = 0;
    switch (fechaMes) {
        case "Ene":
            number = 1;
            break;
        case "Feb":
            number = 2;
            break;
        case "Mar":
            number = 3;
            break;
        case "Abr":
            number = 4;
            break;
        case "May":
            number = 5;
            break;
        case "Jun":
            number = 6;
            break;
        case "Jul":
            number = 7;
            break;
        case "Ago":
            number = 8;
            break;
        case "Set":
            number = 9;
            break;
        case "Oct":
            number = 10;
            break;
        case "Nov":
            number = 11;
            break;
        case "Dic":
            number = 12;
            break;
            
    }

    fechaInicios = "1 " + number + " " + fechaAnio;
    return fechaInicios;
}
FechaFinalMC = function () {
    var fechaFinal = $('#txtFechaFinalMC').val();
    var fechaFinals = "";
    var fechaMes = fechaFinal.split(" ", 1);
    var fechaAnio = fechaFinal.substr(4, 7);

    fechaMes = fechaMes[0];

    var number = 0;
    switch (fechaMes) {
        case "Ene":
            number = 1;
            break;
        case "Feb":
            number = 2;
            break;
        case "Mar":
            number = 3;
            break;
        case "Abr":
            number = 4;
            break;
        case "May":
            number = 5;
            break;
        case "Jun":
            number = 6;
            break;
        case "Jul":
            number = 7;
            break;
        case "Ago":
            number = 8;
            break;
        case "Set":
            number = 9;
            break;
        case "Oct":
            number = 10;
            break;
        case "Nov":
            number = 11;
            break;
        case "Dic":
            number = 12;
            break;
    }

    var dias = diasEnUnMesMC(number,fechaAnio);

    fechaFinals = dias+" " + number + " " + fechaAnio;
    return fechaFinals;
}

pintarPaginadoPorRangoFechaMC = function () {
    var emprcodi = $('#cbxEmpresaMC').val();
    var fechaInicio = FechaInicioMC();
    var fechaFinal = FechaFinalMC();
    $.ajax({
        type: 'POST',
        url: controlador + "PaginadoPorRangoFechaMC",
        data: { emprcodi:emprcodi,fechaInicio: fechaInicio, fechaFinal: fechaFinal },
        success: function (evt) {
            $('#PaginadoMontoCapacidad').html(evt);
            mostrarPaginadoMC();
        }
    });
}


var ListarMontoCapacidadPorRangoFecha = function (nroPagina) {
    var emprcodi = $('#cbxEmpresaMC').val();
    var fechaInicio = FechaInicioMC();
    var fechaFinal = FechaFinalMC();

    $.ajax({
        type: 'POST',
        url: controlador + "ListaMontoPorCapacidad",
        data: {emprcodi:emprcodi, fechaInicio: fechaInicio, fechaFinal: fechaFinal, nroPagina: nroPagina },
        success: function (evt) {
            $('#ListadoMontoCapacidad').css("width", $('#mainLayout').width() + "px");
            $('#ListadoMontoCapacidad').html(evt);
        }
    });
};

pintarBusquedaPorRangoFechaMC = function (nroPagina) {
    ListarMontoCapacidadPorRangoFecha(nroPagina);
}

IniciarTabs = function () {

    $(function () {
        $('#tab-container').easytabs();
    });

}


function ExportarMontoPorCapacidadRangoFecha() { 
    var emprcodi = $('#cbxEmpresaMC').val();
    var fechaInicio = FechaInicioMC();
    var fechaFinal = FechaFinalMC();
    var retorno = 0;

    $.ajax({
        type: 'POST',
        url: controlador + "GenerarReporteXLSPorRangoFechaMC",
        data: {emprcodi:emprcodi, fechaInicio: fechaInicio, fechaFinal: fechaFinal },
        dataType: 'json',
        cache: false,
        async: false,
        success: function (resultado) {
            retorno = resultado;
            if (resultado == 1) {
                alert('Se Generó El Excel');
                location.href = controlador + "ExportarReporteMC";
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