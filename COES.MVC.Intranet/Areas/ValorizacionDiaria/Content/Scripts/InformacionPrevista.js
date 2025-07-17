var controlador = siteRoot + 'ValorizacionDiaria/Consultas/';
function t(x) { return document.getElementById(x); }

$(function () {
    $('#txtFechaInicioIPRP').Zebra_DatePicker({

    });
    $('#txtFechaFinalIPRP').Zebra_DatePicker({

    });
});

//
$('#btnBuscarInformacionPRP').click(function () {
    nuevoFormato();
});

//btnExportarExcelVD
$('#btnExportarExcelIPRP').click(function () {
    ExportarInformacionPRP();
});

pintarPaginadoInformacionPRP = function () {
    var emprcodi = $('#cbxEmpresa').val();
    var fechaInicio = $('#txtFechaInicioIPRP').val();
    var fechaFinal = $('#txtFechaFinalIPRP').val();
    $.ajax({
        type: 'POST',
        url: controlador + "PaginadoPorFiltroIPRemitidaAlParticipante",
        data: { fechaInicio: fechaInicio, fechaFinal: fechaFinal, emprcodi: emprcodi },
        success: function (evt) {
            $('#PaginadoInformacionPRP').html(evt);
            console.log('true paginado');
            mostrarPaginadoIPRP();
        },
        error: function () {
            alert('mal paginado');
        }
    });
}

var ListarInformacionPrevista = function (nroPagina) {
    var emprcodi = $('#cbxEmpresa').val();
    var fechaInicio = $('#txtFechaInicioIPRP').val();
    var fechaFinal = $('#txtFechaFinalIPRP').val();
    $.ajax({
        type: 'POST',
        url: controlador + "ListaInformacionPrevistaRemitidaAlParticipante",
        data: { fechaInicio: fechaInicio, fechaFinal: fechaFinal, emprcodi: emprcodi, nroPagina: nroPagina },
        success: function (evt) {
            $('#ListadoInformacionPRP').css("width", $('#mainLayout').width() + "px");
            $('#ListadoInformacionPRP').html(evt);
        }
        , error: function () {
            alert('Ha ocurrido un error en buscar por rango de fecha');
        }
    });
};

pintarBusquedaPorRangoFechaIPRP = function (nroPagina) {
    ListarInformacionPrevista(nroPagina);
}

function ExportarInformacionPRP() {
    var emprcodi = $('#cbxEmpresa').val();
    var fechaInicio = $('#txtFechaInicioIPRP').val();
    var fechaFinal = $('#txtFechaFinalIPRP').val();
    var retorno = 0;
    var idformato = $('#cbxFormato').val();

    $.ajax({
        type: 'POST',
        url: controlador + "GenerarFormato",
        data: { fechaInicio: fechaInicio, fechaFinal: fechaFinal, idEmpresa: emprcodi, idformato: idformato },
        dataType: 'json',
        cache: false,
        async: false,
        success: function (resultado) {
            retorno = resultado;
            if (resultado == 1) {
                location.href = controlador + "DescargarFormato";
            } else {
                alert('Error al Generar el Excel');
            }
        },
        error: function () {
            alert('Ocurrio un problema al generar el archivo EXCEL');
        }
    });

    return retorno;
};

function nuevoFormato() {
    cargarDataExcelWeb(2);
}

function cargarDataExcelWeb(accion) {
    if (typeof hot != 'undefined') {
        hot.destroy();
    }
    getModelFormato(accion);
}

function getModelFormato(accion) {
    idEmpresa = $('#cbxEmpresa').val();
    var fechaInicio = $('#txtFechaInicioIPRP').val();
    var fechaFinal = $('#txtFechaFinalIPRP').val();
    var idformato = $('#cbxFormato').val();

    $.ajax({
        type: 'POST',
        url: controlador + "MostrarHojaExcelWeb",
        dataType: 'json',
        data: {
            idEmpresa: idEmpresa,
            fechaInicio: fechaInicio,
            fechaFinal: fechaFinal,
            idformato: idformato
        },
        success: function (evt) {
            evtHot = evt;

            formato = 'tipo1';
            crearGrillaFormatoTipo1(evt, 0);
        },
        error: function () {
            alert("Error al cargar Excel Web");
            evtHot = null;
        }
    });

}

function getExcelColumnName(pi_columnNumber) {
    var li_dividend = pi_columnNumber;
    var ls_columnName = "";
    var li_modulo;

    while (li_dividend > 0) {
        li_modulo = (li_dividend - 1) % 26;
        ls_columnName = String.fromCharCode(65 + li_modulo) + ls_columnName;
        li_dividend = Math.floor((li_dividend - li_modulo) / 26);
    }

    return ls_columnName;
}