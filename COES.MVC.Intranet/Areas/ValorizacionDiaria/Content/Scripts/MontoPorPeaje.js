var controlador = siteRoot + 'ValorizacionDiaria/Consultas/';

$(function () {
    $('#txtFechaInicioMP').Zebra_DatePicker({
        format: 'M Y'
    });
    $('#txtFechaFinalMP').Zebra_DatePicker({
        format: 'M Y'
    });
});

//btnBuscarMontoEnergia
$('#btnBuscarMontoPeaje').click(function () {
    //pintarPaginadoPorRangoFechaMP();
    ListarMontoPeajePorRangoFecha(1);
});

//btnExportarExcelME
$('#btnExportarExcelMP').click(function () {
    ExportarMontoPorPeajeRangoFecha();
});


function diasEnUnMesMP(mes, año) {
    return new Date(año, mes, 0).getDate();
}

FechaInicioMP = function () {
    var fechaInicio = $('#txtFechaInicioMP').val();
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
FechaFinalMP = function () {
    var fechaFinal = $('#txtFechaFinalMP').val();
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

    var dias = diasEnUnMesMP(number, fechaAnio);

    fechaFinals = dias + " " + number + " " + fechaAnio;
    return fechaFinals;
}

pintarPaginadoPorRangoFechaMP = function () {
    var emprcodi = $('#cbxEmpresaMP').val();
    var fechaInicio = FechaInicioMP();
    var fechaFinal = FechaFinalMP();
    $.ajax({
        type: 'POST',
        url: controlador + "PaginadoPorRangoFechaMP",
        data: { emprcodi:emprcodi,fechaInicio: fechaInicio, fechaFinal: fechaFinal },
        success: function (evt) {
            $('#PaginadoMontoPeaje').html(evt);
            mostrarPaginadoMP();
        }
    });
}


var ListarMontoPeajePorRangoFecha = function (nroPagina) {
    var emprcodi = $('#cbxEmpresaMP').val();
    var fechaInicio = FechaInicioMP();
    var fechaFinal = FechaFinalMP();
    $.ajax({
        type: 'POST',
        url: controlador + "ListaMontoPorPeaje",
        data: { emprcodi:emprcodi,fechaInicio: fechaInicio, fechaFinal: fechaFinal, nroPagina: nroPagina },
        success: function (evt) {
            $('#ListadoMontoPorPeaje').css("width", $('#mainLayout').width() + "px");
            $('#ListadoMontoPorPeaje').html(evt);
        }
    });
};



pintarBusquedaPorRangoFechaMP = function (nroPagina) {
    ListarMontoPeajePorRangoFecha(nroPagina);
}

IniciarTabs = function () {

    $(function () {
        $('#tab-container').easytabs();
    });

}

function ExportarMontoPorPeajeRangoFecha() {
    var emprcodi = $('#cbxEmpresaMP').val();
    var fechaInicio = FechaInicioMP();
    var fechaFinal = FechaFinalMP();
    var retorno = 0;

    $.ajax({
        type: 'POST',
        url: controlador + "GenerarReporteXLSPorRangoFechaMP",
        data: {emprcodi:emprcodi, fechaInicio: fechaInicio, fechaFinal: fechaFinal },
        dataType: 'json',
        cache: false,
        async: false,
        success: function (resultado) {
            retorno = resultado;
            if (resultado == 1) {
                alert('Se Generó El Excel');
                location.href = controlador + "ExportarReporteMP";
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