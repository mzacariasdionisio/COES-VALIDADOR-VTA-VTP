var controlador = siteRoot + 'cortoplazo/reportefinal/';

$(function () {

    $('#txtFechaInicio').Zebra_DatePicker({
        direction: false
    });

    $('#txtFechaFin').Zebra_DatePicker({
        direction: false
    });

    $('#btnConsultar').on('click', function () {
        consultar();
    });

    $('#btnExportar').on('click', function () {
        exportar();
    });

    //consultar();
});

consultar = function () {

    if ($('#txtFechaInicio').val() != "" && $('#txtFechaFin').val() != "") {

        var fecha1 = getFecha($('#txtFechaInicio').val());
        var fecha2 = getFecha($('#txtFechaFin').val());
        var diferencia = numeroDias(fecha1, fecha2);

        if (diferencia <= 31) {

            $.ajax({
                type: 'POST',
                url: controlador + 'barradesenergizadalist',
                data: {
                    fechaInicio: $('#txtFechaInicio').val(),
                    fechaFin: $('#txtFechaFin').val()
                },
                success: function (evt) {
                    $('#listado').html(evt);
                    $('#tablaReporte').dataTable({
                    });
                    $('#mensaje').removeClass();
                    $('#mensaje').html('');
                },
                error: function () {
                    mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
                }
            });
        }
        else {
            mostrarMensaje('mensaje', 'alert', 'Seleccione rango menor a un mes');
        }
    }
    else {
        mostrarMensaje('mensaje', 'alert', 'Seleccione rango de fechas.');
    }
};

exportar = function () {
    if ($('#txtFechaInicio').val() != "" && $('#txtFechaFin').val() != "") {

        var fecha1 = getFecha($('#txtFechaInicio').val());
        var fecha2 = getFecha($('#txtFechaFin').val());
        var diferencia = numeroDias(fecha1, fecha2);

        if (diferencia <= 31) {

            $.ajax({
                type: 'POST',
                url: controlador + 'exportarbarradesenergizada',
                data: {
                    fechaInicio: $('#txtFechaInicio').val(),
                    fechaFin: $('#txtFechaFin').val()
                },
                dataType: 'json',
                success: function (result) {
                    if (result == 1 || result == 2) {
                        if (result == 2) {
                            mostrarMensaje('mensaje', 'alert', 'En el reporte se han encontrado valores FS');
                        }
                        location.href = controlador + "descargarbarradesenergizada";
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
            mostrarMensaje('mensaje', 'alert', 'Seleccione rango menor a un mes');
        }
    }
    else {
        mostrarMensaje('mensaje', 'alert', 'Seleccione rango de fechas.');
    }
}

mostrarMensaje = function (id, tipo, mensaje) {
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);
};

getFecha = function (date) {
    var parts = date.split("/");
    var date = new Date(parts[1] + "/" + parts[0] + "/" + parts[2]);
    return date.getTime();
};

numeroDias = function (inicio, final) {
    return Math.round((final - inicio) / (1000 * 60 * 60 * 24));
};