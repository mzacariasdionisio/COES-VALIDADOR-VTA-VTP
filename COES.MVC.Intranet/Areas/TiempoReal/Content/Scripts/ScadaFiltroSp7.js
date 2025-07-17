var controlador = siteRoot + 'tiemporeal/scadafiltrosp7/';


$(function() {

    $('#btnNuevo').click(function() {
        editar(0, 1);
    });

    //buscar();
    $("#btnBuscar").persisteBusqueda(".content-tabla-search", buscar);

    $('#btnBuscar').click(function() {
        buscar();
    });

});


buscar = function () {

    pintarPaginado();
    mostrarListado(1);
}


pintarPaginado = function() {
    $.ajax({
        type: 'POST',
        url: controlador + "paginado",
        data: {
            filtro: $('#txtFiltroNomb').val(),
            creador: $('#txtCreador').val(),
            modificador: $('#txtModificador').val()
        },
        success: function (evt) {
            $('#paginado').html(evt);
            mostrarPaginado();
        },
        error: function() {
            mostrarError();
        }
    });
}


pintarBusqueda = function (nroPagina) {

    mostrarListado(nroPagina);
}


mostrarListado = function(nroPagina) {

    $('#hfNroPagina').val(nroPagina);
    $.ajax({
        type: 'POST',
        url: controlador + "lista",
        data: {
            filtro: $('#txtFiltroNomb').val(),
            creador: $('#txtCreador').val(),
            modificador: $('#txtModificador').val(),
            nroPage: nroPagina
        },
        success: function(evt) {
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
        error: function() {
            mostrarError();
        }
    });
}


editar = function(id, accion) {

    document.location.href = controlador + "editar?id=" + id + "&accion=" + accion;
}


eliminar = function(id) {

    if (confirm('¿Está seguro de eliminar este registro?')) {

        $.ajax({
            type: 'POST',
            url: controlador + "eliminar",
            dataType: 'json',
            cache: false,
            data: { id: id },
            success: function(resultado) {
                if (resultado == 1) {
                    buscar();
                } else {

                    if (resultado == -2) {
                        alert("El registro solo lo puede eliminar su creador...");
                    } else {
                        mostrarError();
                    }
                }


            },
            error: function() {
                mostrarError();
            }
        });

    }
}

mostrarError = function() {

    alert('Ha ocurrido un error.');
}

