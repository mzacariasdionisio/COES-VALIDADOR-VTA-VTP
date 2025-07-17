var controlador = siteRoot + 'ValorizacionDiaria/Proceso/';
var controladorConsulta = siteRoot + 'ValorizacionDiaria/Consultas/';

$(function () {
    //$('#cbParticipante').multipleSelect({
    //    width: '200px',
    //    filter: true
    //});

    $('#txtFechaInicio').Zebra_DatePicker({

    });

    $('#txtFechaFinal').Zebra_DatePicker({

    });

    $('#btnEjecutar').click(function () {
        EjecutarProceso();
    });

    //$('#cbParticipante').multipleSelect('checkAll');

    $('#btnBuscarValorizacionD').click(function () {
        pintarPaginadoValorizacionDiaria();
        ListarValorizacionDiaria(1);
    });

    //btnExportarExcelVD
    $('#btnExportarExcelVD').click(function () {
        ExportarValorizacionDiaria();
    });

});

var EjecutarProceso = function () {
    //var participantes = $('#cbParticipante');
    //console.log(participantes.multipleSelect('rowCountSelected'));

    //if (typeof (participantes.multipleSelect('rowCountSelected')) === 'object') {
    //    alert("Seleccione al menos una empresa");
    //    return false;
    //} else if (participantes.multipleSelect('rowCountSelected') <= 100 ||
    //    participantes.multipleSelect('isAllSelected') == "S") {

    //    var empresa = $('#cbParticipante').multipleSelect('getSelects');
    //    $('#hfEmpresa').val($('#cbEmpresa').multipleSelect('isAllSelected') == "S" ? "-1" : empresa);

    if ($('#cbParticipante').val() != "") {

        $.ajax({
            type: 'POST',
            url: controlador + "EjecutarProceso",
            data: {
                empresas: $('#cbParticipante').val(),
                fechaInicio: $('#txtFechaInicio').val(),
                fechaFin: $('#txtFechaFinal').val()
            },
            dataType: 'json',
            success: function (resultado) {
                retorno = resultado;
                if (resultado == 1) {
                    alert('Proceso ejecutado con exito!');
                    $('#mensajeGeneral').show();
                    $('#mensajeGeneral').removeClass();
                    $('#mensajeGeneral').addClass('action-exito');
                    $('#mensajeGeneral').html("Datos Guardados Correctamente!");
                } else {
                    alert('No se ha conseguido ejecutar el proceso');
                    $('#mensajeGeneral').show();
                    $('#mensajeGeneral').removeClass();
                    $('#mensajeGeneral').addClass('action-error');
                    $('#mensajeGeneral').html('Error Porcentaje Perdida!');
                }
            },
            error: function () {
                alert('Ocurrio un problema al ejecutar el proceso, sirvase a revisar el log de eventos');
                $('#mensajeCargoC').show();
                $('#mensajeCargoC').removeClass();
                $('#mensajeCargoC').html('Ocurrio un problema al ejecutar el proceso');
                $('#mensajeCargoC').addClass('action-error');
            }
        });
    }
    else {
        alert("Seleccione una empresa.");
    }
};


pintarPaginadoValorizacionDiaria = function () {
    //var participantes = $('#cbParticipante');

    //if (typeof (participantes.multipleSelect('rowCountSelected')) === 'object') {
    //    return false;
    //} else if (participantes.multipleSelect('rowCountSelected') == 1 &&
    //    participantes.multipleSelect('isAllSelected') == "N") {

    //    var empresa = $('#cbParticipante').multipleSelect('getSelects');
    //    $('#hfEmpresa').val($('#cbEmpresa').multipleSelect('isAllSelected') == "S" ? "-1" : empresa);
    //    console.log($('#hfEmpresa').val());

        $.ajax({
            type: 'POST',
            url: controladorConsulta + "PaginadoPorFiltroValorizacionDiaria",
            data: {
                emprcodi: $('#cbParticipante').val(),
                fechaInicio: $('#txtFechaInicio').val(),
                fechaFinal: $('#txtFechaFinal').val()
            },
            success: function (evt) {
                $('#PaginadoValorizacionDiaria').html(evt);
                mostrarPaginadoVD();
            }
        });
    //}
}

var ListarValorizacionDiaria = function (nroPagina) {
    //var participantes = $('#cbParticipante');

    //if (typeof (participantes.multipleSelect('rowCountSelected')) === 'object') {
    //    alert("Seleccione al menos una empresa");
    //    return false;
    //} else if (participantes.multipleSelect('rowCountSelected') == 1 &&
    //    participantes.multipleSelect('isAllSelected') == "N") {

    //    var empresa = $('#cbParticipante').multipleSelect('getSelects');
    //    $('#hfEmpresa').val($('#cbEmpresa').multipleSelect('isAllSelected') == "S" ? "-1" : empresa);

    if ($('#cbParticipante').val() != "") {

        $.ajax({
            type: 'POST',
            url: controladorConsulta + "ListaValorizacionDiaria",
            data: {
                emprcodi: $('#cbParticipante').val(),
                fechaInicio: $('#txtFechaInicio').val(),
                fechaFinal: $('#txtFechaFinal').val(),
                nroPagina: nroPagina
            }, success: function (evt) {
                $('#ListadoValorizacionDiaria').css("width", $('#mainLayout').width() + "px");
                $('#ListadoValorizacionDiaria').html(evt);

                if (evt.includes('No se encontraron')) {
                    $('#btnExportarExcelVD').css("display", "none");
                } else {
                    $('#btnExportarExcelVD').removeAttr('style');
                }

                $('#btnExportarExcelVD').removeAttr('style');

            }
        });
    } else {
        alert('Seleccione una empresa.');
    }
};

pintarBusquedaPorRangoFechaVD = function (nroPagina) {
    ListarValorizacionDiaria(nroPagina);
}

function ExportarValorizacionDiaria() {
    //var participantes = $('#cbParticipante');

    //if (typeof (participantes.multipleSelect('rowCountSelected')) === 'object') {
    //    return false;
    //} else if (participantes.multipleSelect('rowCountSelected') == 1 &&
    //    participantes.multipleSelect('isAllSelected') == "N") {

    //    var empresa = $('#cbParticipante').multipleSelect('getSelects');
    //    $('#hfEmpresa').val($('#cbEmpresa').multipleSelect('isAllSelected') == "S" ? "-1" : empresa);

    if ($('#cbParticipante').val() != "") {

        var retorno = 0;

        $.ajax({
            type: 'POST',
            url: controladorConsulta + "GenerarReporteXLSValorizacionDiaria",
            data: {
                emprcodi: $('#cbParticipante').val(),
                fechaInicio: $('#txtFechaInicio').val(),
                fechaFinal: $('#txtFechaFinal').val()
            },
            dataType: 'json',
            success: function (resultado) {
                retorno = resultado;
                if (resultado == 1) {
                    alert('Se Genero El Excel');
                    location.href = controladorConsulta + "ExportarReporteVD";
                } else {
                    alert('Error al Generar Excel');
                }
            },
            error: function () {
                alert('Ocurrio un problema al generar el archivo EXCEL');
            }
        });

        return retorno;

    } else {
        alert('Seleccione una empresa.');
    }
};