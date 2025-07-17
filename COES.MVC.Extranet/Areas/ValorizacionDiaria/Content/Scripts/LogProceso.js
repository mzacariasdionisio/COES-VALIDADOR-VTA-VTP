var controlador = siteRoot + 'ValorizacionDiaria/Consultas/';

$(function () {
    $('#txtFechaLog').Zebra_DatePicker({

    });
});


//btnBuscarLogProceso
$('#btnBuscarLogProceso').click(function () {
    pintarPaginadoLogProceso();
    ListarLogProcesoPorFecha(1);
});

pintarPaginadoLogProceso = function () {
    var fecha = $('#txtFechaLog').val();
    $.ajax({
        type: 'POST',
        url: controlador + "PaginadoLogProceso",
        data: { date: fecha },
        success: function (evt) {
            $('#PaginadoLogProceso').html(evt);
            mostrarPaginadoLP();
        }
    });
}


var ListarLogProcesoPorFecha = function (nroPagina) {
    var fecha = $('#txtFechaLog').val();
    $.ajax({
        type: 'POST',
        url: controlador + "ListaLogProceso",
        data: { date: fecha, nroPagina: nroPagina },
        success: function (evt) {
            $('#ListadoLogProceso').css("width", $('#mainLayout').width() + "px");
            $('#ListadoLogProceso').html(evt);
        }
    });
};



pintarBusquedaPorRangoFechaLP = function (nroPagina) {
    ListarLogProcesoPorFecha(nroPagina);
}

