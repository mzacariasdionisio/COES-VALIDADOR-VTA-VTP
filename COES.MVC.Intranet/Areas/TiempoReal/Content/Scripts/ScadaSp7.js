var controlador = siteRoot + 'tiemporeal/scadasp7/';

$(function () {

    /*
    $('#txtFechaDesde').Zebra_DatePicker({
    });

    $('#txtFechaHasta').Zebra_DatePicker({
        direction: ['2025-04-01', '2025-04-26']
    });*/


    //buscar();

    $('#btnBuscar').click(function () {
        let fechaDesdeVal = new Date($('#txtFechaDesde').val());
        let fechaHastaVal = new Date($('#txtFechaHasta').val());
        if (fechaDesdeVal <= fechaHastaVal) {
            buscar();
        } else {
            alert("Debe ingresar un rango de fechas válido. La fecha del campo \"desde\" no puede ser superior a la fecha del campo \"hasta\"");
        }
    });


    $('#cbZona').val(0);
    $('#cbFiltro').val(0);

    $('#rb30min').prop('checked', true);


    $(window).resize(function () {
        $('#listado').css("width", $('#mainLayout').width() + "px");
    });

    $('#rbSSEE').prop('checked', true);

    activarSSEEoFiltro();


    $('#btnExcel').click(function () {
        crearReporte();
    });

});

buscar = function () {




    if (document.getElementById('rbSSEE').checked) {
        //if ($('#cbZona').val() == "0") {
        if ($('#cbZona').val() == "0" || $('#cbZona').val() == null || $('#cbZona').val() == undefined) {
            alert("Seleccione una Ubicación");
            return;
        }
    } else {

        //if ($('#cbFiltro').val() == "0" || $('#cbFiltro').val() == "-1") {
        if ($('#cbFiltro').val() == "0" || $('#cbFiltro').val() == null || $('#cbFiltro').val() == undefined) {
            alert("Seleccione un Filtro");
            return;
        }

    }


    pintarPaginado();
    mostrarListado(1);


}


activarSSEEoFiltro = function () {


    if (document.getElementById('rbSSEE').checked) {
        $('#cbZona').show();
        $('#cbFiltro').hide();
    } else {
        $('#cbFiltro').show();
        $('#cbZona').hide();
    }

}


pintarPaginado = function () {

    var ssee = document.getElementById('rbSSEE').checked;
    var codigo = "0";

    if (ssee)
        codigo = $('#cbZona').val();
    else
        codigo = $('#cbFiltro').val();


    $.ajax({
        type: 'POST',
        url: controlador + "paginado",
        data: {
            ssee: ssee,
            zonaCodi: codigo,
            mediFechaIni: $('#txtFechaDesde').val(),
            mediFechaFin: $('#txtFechaHasta').val(),
        },
        success: function (evt) {
            $('#paginado').html(evt);
            mostrarPaginado();
        },
        error: function () {
            mostrarError();
        }
    });
}


pintarBusqueda = function (nroPagina) {
    mostrarListado(nroPagina);
}


mostrarListado = function (nroPagina) {

    var ssee = document.getElementById('rbSSEE').checked;
    var codigo = "0";

    if (ssee)
        codigo = $('#cbZona').val();
    else
        codigo = $('#cbFiltro').val();


    $('#hfNroPagina').val(nroPagina);
    $.ajax({
        type: 'POST',
        url: controlador + "lista",
        data: {
            ssee: ssee,
            zonaCodi: codigo,
            mediFechaIni: $('#txtFechaDesde').val(),
            mediFechaFin: $('#txtFechaHasta').val(),
            nroPage: nroPagina
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
}


crearReporte = function (nroPagina) {

    if (document.getElementById('rbSSEE').checked) {
        if ($('#cbZona').val() == "0" || $('#cbZona').val() == null || $('#cbZona').val() == undefined) {
            alert("Seleccione una Ubicación");
            return;
        }
    } else {
        if ($('#cbFiltro').val() == "0" || $('#cbFiltro').val() == null || $('#cbFiltro').val() == undefined) {
            alert("Seleccione un Filtro");
            return;
        }

    }


    var ssee = document.getElementById('rbSSEE').checked;
    var codigo = "0";

    var dato15Minuto = document.getElementById('rb15min').checked;

    if (ssee)
        codigo = $('#cbZona').val();
    else
        codigo = $('#cbFiltro').val();


    $('#hfNroPagina').val(nroPagina);
    $.ajax({
        type: 'POST',
        url: controlador + "exportar",
        data: {
            lect15Min: dato15Minuto,
            ssee: ssee,
            zonaCodi: codigo,
            mediFechaIni: $('#txtFechaDesde').val(),
            mediFechaFin: $('#txtFechaHasta').val(),
            nroPage: nroPagina
        },
        success: function (evt) {
            window.location = controlador + "descargar";

        },
        error: function () {
            mostrarError();
        }
    });
}


mostrarError = function () {

    alert('Ha ocurrido un error.');
}

