const controlador = siteRoot + 'web/busqueda/';

$(function () {
    inicializarDatePickers();
    inicializarEventos();
});

function inicializarDatePickers() {
    // Configurar Zebra_DatePicker en formato Año/Mes/Día
    $('#txtFechaInicio, #txtFechaFin').Zebra_DatePicker({
        format: 'Y/m/d'
    });
}

function inicializarEventos() {
    $('#btnConsultar').on('click', realizarBusquedaHistorico);
}

function realizarBusquedaHistorico() {
    $.ajax({
        type: 'POST',
        url: controlador + 'ListadoHistorico',
        data: {
            start_date: `${$('#txtFechaInicio').val()} 00:00:00`,
            end_date: `${$('#txtFechaFin').val()} 23:59:00`,
        },
        success: manejarRespuestaBusqueda,
        error: console.log("Sucedió un error"),
    });
}

function manejarRespuestaBusqueda(response) {
    const listadoContainer = document.getElementById('listado');
    listadoContainer.style.display = 'block';
    $('#listado').html(response);
    $('#tabla').dataTable({
        "iDisplayLength": 25,
        order: [[0, 'desc']],
    });
}
