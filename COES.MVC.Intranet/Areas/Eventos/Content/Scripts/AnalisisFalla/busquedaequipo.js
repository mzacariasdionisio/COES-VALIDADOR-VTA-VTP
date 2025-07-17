var controlador = siteRoot + 'Eventos/AnalisisFallas/';

$(function () {
    cargarBusquedaEquipo();
    $('#btnBuscarEquipo').click(function () {
        openBusquedaEquipo();
        $('#hfIdTipoBusqueda').val(1);
    });

    $('#btnBuscarEquipoObs').click(function () {
        openBusquedaEquipo();
        $('#hfIdTipoBusqueda').val(2);
    });

    $('#btnBuscarEquipoRecomendacion').click(function () {
        debugger;
        openBusquedaEquipo();
        $('#hfIdTipoBusqueda').val(3);
    });

    $('#btnBuscarEquipoObservacion').click(function () {
        debugger;
        openBusquedaEquipo();
        $('#hfIdTipoBusqueda').val(4);
    });
})

//busqueda de equipo
openBusquedaEquipo = function () {

    $('#busquedaEquipo').bPopup({
        follow: [true, true],
        position: ['auto', 'auto'],
        positionStyle: 'fixed'
    });
    $('#txtFiltro').focus();
}

cargarBusquedaEquipo = function () {

    $.ajax({
        type: "POST",
        url: controlador + "BusquedaEquipo",
        data: {},
        global: false,
        success: function (evt) {
            $('#busquedaEquipo').html(evt);
        },
        error: function (req, status, error) {
            mostrarError();
        }
    });
}

seleccionarEquipo = function (idEquipo, substacion, equipo, empresa, idEmpresa) {
    debugger;
    if ($('#hfIdTipoBusqueda').val() == 1) {
        $('#hfIdEmpresa').text(idEmpresa);
        $('#hfEquiCodi').val(idEquipo);
        $('#txtEmpresaAO').val(empresa);
        $('#hfIdSubestacion').val(substacion);
    }
    else if ($('#hfIdTipoBusqueda').val() == 2) {
        $('#hfIdEmpresa').text(idEmpresa);
        $('#hfEquiCodi').val(idEquipo);
        $('#txtEmpresaAO').val(empresa);
        $('#hfIdSubestacion').val(substacion);
    }
    else if ($('#hfIdTipoBusqueda').val() == 3) {
        $('#hfIdEmpresaRecomendacion').val(idEmpresa);
        $('#txtEmpresaRecomendacion').val(empresa);
    }
    else if ($('#hfIdTipoBusqueda').val() == 4) {
        $('#hfIdEmpresaObservacion').val(idEmpresa);
        $('#txtEmpresaObservacion').val(empresa);
    }

    $('#busquedaEquipo').bPopup().close();
}