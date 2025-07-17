var controlador = siteRoot + "FileManager/InformacionAgentes/";
$(function () {

    $('#txtFechaIni').Zebra_DatePicker({
    });
    $('#txtFechaFin').Zebra_DatePicker({
    });

    $('#btnConsultar').click(function () {
        consultarDatos();
    });
    consultarDatos();
    $(window).resize(function () {
        $('#listado').css("width", $('#mainLayout').width() + "px");
    });
});
consultarDatos = function () {
    pintarPaginado();
    mostrarListado(1);
};
pintarPaginado = function () {
    $.ajax({
        type: 'POST',
        url: controlador + "Paginado",
        data: {
            iEmpresa: $('#cbEmpresa').val(),
            sFechaInicio: $('#txtFechaIni').val(),
            sFechaFin: $('#txtFechaFin').val(),
        },
        success: function (evt) {
            $('#paginado').html(evt);
            mostrarPaginado();
        },
        error: function () {
            mostrarError();
        }
    });
};
mostrarListado = function (nroPagina) {
    $('#hfNroPagina').val(nroPagina);
    $.ajax({
        type: 'POST',
        url: controlador + "ListadoArchivos",
        data: {
            iEmpresa: $('#cbEmpresa').val(),
            sFechaInicio: $('#txtFechaIni').val(),
            sFechaFin: $('#txtFechaFin').val(),
            nroPagina: $('#hfNroPagina').val()
        },
        success: function (evt) {
            $('#listado').css("width", $('#mainLayout').width() + "px");

            $('#listado').html(evt);
            $('#tabla').dataTable({
                "scrollY": 430,
                "scrollX": true,
                "sDom": 't',
                "ordering": false,
                "iDisplayLength": 50
            });
        },
        error: function () {
            mostrarError();
        }
    });
};
pintarBusqueda = function (nroPagina) {
    mostrarListado(nroPagina);
};