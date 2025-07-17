var controlador = siteRoot + 'eventos/pruebasaleatorias/';

$(function () {

    $('#FechaDesde').Zebra_DatePicker({
    });

    $('#FechaHasta').Zebra_DatePicker({
    });

    $('#btnNuevo').click(function() {
        editar(0, 1);
    });

    //buscar();
    $("#btnBuscar").persisteBusqueda(".content-tabla-search", buscar);

    $('#btnBuscar').click(function () {
        buscar();
    });
    
});


function retornar() {
    return false;
}

function convertirFecha(dateStr) {
    var parts = dateStr.split("/");
    return new Date(parts[2], parts[1] - 1, parts[0]);
}

buscar = function () {
    var fechaini = convertirFecha($('#FechaDesde').val());
    var fechafin = convertirFecha($('#FechaHasta').val());

    if (fechaini <= fechafin) {
        pintarPaginado();
        mostrarListado(1);
    } else {
        alert("Fecha inicial supera a la final");
    }
}

pintarBusqueda = function (nroPagina) {
    mostrarListado(nroPagina);
}


mostrarExito = function () {
    $('#mensaje').removeClass();
    $('#mensaje').addClass('action-exito');
    $('#mensaje').html('La operación se realizó con éxito.');
}


mostrarAlerta = function (mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').addClass('action-alert');
    $('#mensaje').html(mensaje);
}


pintarPaginado = function () {
    $.ajax({
        type: 'POST',
        url: controlador + "paginado",
        data: {
            fechaIni: $('#FechaDesde').val(),
            fechaFin: $('#FechaHasta').val()
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


mostrarListado = function (nroPagina) {

    $('#hfNroPagina').val(nroPagina);
    $.ajax({
        type: 'POST',
        url: controlador + "lista",
        data: {
            fechaIni: $('#FechaDesde').val(),
            fechaFin: $('#FechaHasta').val(),
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


mostrarError = function() {
    alert('Ha ocurrido un error.');
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
                    mostrarError();
                }
            },
            error: function() {
                mostrarError();
            }
        });
    }
}


grabarRegistro = function() {
    return;
}


editar = function(id, accion) {
    document.location.href = controlador + "editar?id=" + id + "&accion=" + accion;
}

