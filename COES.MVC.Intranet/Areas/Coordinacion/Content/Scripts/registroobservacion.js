var controlador = siteRoot + 'coordinacion/registroobservacion/';

$(function () {

    $('#txtFechaInicio').Zebra_DatePicker({
        pair: $('#txtFechaFin'),
        onSelect: function (date) {
            var date1 = getFecha(date);
            var date2 = getFecha($('#txtFechaFin').val());

            if (date1 > date2) {
                $('#txtFechaFin').val(date);
            }
        }
    });

    $('#txtFechaFin').Zebra_DatePicker({
        direction: true
    });

    $('#btnNuevo').on('click', function () {
        nuevo();
    });

    $('#btnExportar').on('click', function () {
        exportar();
    });

    $('#btnConsultar').on('click', function () {
        consultar();
    });

    consultar();
});

consultar = function () {
    if ($('#txtFechaInicio').val() != "" && $('#txtFechaFin').val() != "") {

        $.ajax({
            type: 'POST',
            url: controlador + 'listado',
            data: {
                idEmpresa: $('#cbEmpresa').val(),
                fechaInicio: $('#txtFechaInicio').val(),
                fechaFin: $('#txtFechaFin').val()
            },
            success: function (evt) {
                $('#listado').html(evt);
                $('#tabla').dataTable({
                   
                });
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
            }
        });
    }
    else {
        mostrarMensaje('mensaje', 'alert', 'Seleccione el rango de fechas.');
    }
}

nuevo = function () {
    editar(0);
}

editar = function (id) {
    document.location.href = controlador + 'editar?id=' + id;
}

eliminar = function (id) {
    if (confirm('¿Está seguro de realizar esta operación?')) {

        $.ajax({
            type: 'POST',
            url: controlador + 'eliminar',
            data: {
                obscodi: id
            },
            dataType: 'json',
            success: function (result) {
                if (result == 1) {
                    mostrarMensaje('mensaje', 'exito', 'La operación se realizó correctamente.');
                    consultar();
                }
                else {
                    mostrarMensaje('mensaje', 'error', 'No se pudo eliminar el registro.');
                }
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'No se pudo eliminar el registro.');
            }
        });
    }
}

exportar = function () {
    if ($('#txtFechaInicio').val() != "" && $('#txtFechaFin').val() != "") {

        $.ajax({
            type: 'POST',
            url: controlador + 'exportar',
            data: {
                idEmpresa: $('#cbEmpresa').val(),
                fechaInicio: $('#txtFechaInicio').val(),
                fechaFin: $('#txtFechaFin').val()
            },
            success: function (resultado) {
                if (resultado == 1) {
                    location.href = controlador + "descargar";
                }
                else {
                    mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
                }
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
            }
        });
    }
    else {
        mostrarMensaje('mensaje', 'alert', 'Seleccione el rango de fechas.');
    }
}

/*===Funciones Generales===*/

mostrarMensaje = function (id, tipo, mensaje) {
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);
}

getFecha = function (date) {
    var parts = date.split("/");
    var date = new Date(parts[1] + "/" + parts[0] + "/" + parts[2]);
    return date.getTime();
}
