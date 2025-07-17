var controlador = siteRoot + 'seguimientorecomendacion/comentario/';

$(function () {

    $('#txtFechaDesde').Zebra_DatePicker({
    });

    $('#txtFechaHasta').Zebra_DatePicker({
    });

    $('#btnNuevo').click(function () {
        editar(0,1);
    });

    buscar();

    $('#btnBuscar').click(function () {
        buscar();
    });

    $(document).ready(function () {
        buscar();
    });
});

buscar = function () {
    pintarPaginado();
    mostrarListado(1);
}

pintarPaginado = function () {

    $.ajax({
        type: 'POST',
        url: controlador + "paginado",
        data: {            
            srmreccodi: $('#cbSrmrecfecharecomend').val(),
            usercode: $('#cbUsername').val(),
            emprCodi: $('#cbEmprNomb').val(),
            srmcomFechacoment: $('#txtFechaDesde').val(),
            srmcomFeccreacion: $('#txtFechaHasta').val(),
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

    $('#hfNroPagina').val(nroPagina);
    $.ajax({
        type: 'POST',
        url: controlador + "lista",
        data: {            
            srmreccodi: $('#cbSrmrecfecharecomend').val(),
            usercode: $('#cbUsername').val(),
            emprCodi: $('#cbEmprNomb').val(),
            srmcomFechacoment: $('#txtFechaDesde').val(),
            srmcomFeccreacion: $('#txtFechaHasta').val(),
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

editar = function (id, accion) {

    document.location.href = controlador + "editar?id=" + id + "&accion=" + accion;
}

eliminar = function (id) {

    if (confirm('¿Está seguro de eliminar este registro?')) {

        $.ajax({
            type: 'POST',
            url: controlador + "eliminar",
            dataType: 'json',
            cache: false,
            data: { id: id },
            success: function (resultado) {
                if (resultado == 1) {
                    buscar();
                }
                else {
                    mostrarError();
                }


            },
            error: function () {
                mostrarError();
            }
        });

    }
}

mostrarError = function () {

    alert('Ha ocurrido un error.');
}

