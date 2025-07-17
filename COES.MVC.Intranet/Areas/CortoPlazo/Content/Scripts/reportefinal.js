var controlador = siteRoot + 'cortoplazo/reportefinal/';

$(function () {
   
    $('#txtFecha').Zebra_DatePicker({
        direction:false
    });

    $('#txtFechaInicio').Zebra_DatePicker({
        direction: false
    });

    $('#txtFechaFin').Zebra_DatePicker({
        direction: false
    });

    $('#btnConsultar').on('click', function () {
        consultar();
    });

    $('#btnGenerar').on('click', function () {
        generarReporte();
    });

    $('#btnNuevo').on('click', function () {
        nuevoReporte();
    });

    consultar();
});

consultar = function () {

    if ($('#txtFechaInicio').val() != "" && $('#txtFechaFin').val() != "") {

       
        $.ajax({
            type: 'POST',
            url: controlador + 'reportelist',
            data: {
                fechaInicio: $('#txtFechaInicio').val(),
                fechaFin: $('#txtFechaFin').val()
            },
            success: function (evt) {
                $('#listado').html(evt);
                $('#tablaReporte').dataTable({
                });
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
            }
        });
    }
    else {
        mostrarMensaje('mensaje', 'alert', 'Seleccione rango de fechas.');
    }
};

generarReporte = function () {

    if ($('#txtFecha').val() != "") {

        if (confirm('¿Está seguro de realizar esta acción?')) {
            $.ajax({
                type: 'POST',
                url: controlador + 'GenerarVersionReporte',
                data: {
                    fecha: $('#txtFecha').val()
                },
                dataType: 'json',
                success: function (result) {
                    if (result.Resultado == 1) {
                        mostrarMensaje('mensaje', 'exito', 'La operación se realizó correctamente.');
                        $('#popupEdicion').bPopup().close();
                        consultar();
                    }
                    else
                        mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
                },
                error: function () {
                    mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
                }
            });
        }
    }
    else {
        mostrarMensaje('mensajeGeneracion', 'alert', 'Seleccione una fecha.');
    }
};

procesarReporte = function (idReporte, tipo) {
    $.ajax({
        type: 'POST',
        url: controlador + 'GenerarReporteExcel',
        data: {
            fecha: $('#txtFecha').val(),
            idReporte: idReporte,
            tipo: tipo
        },
        dataType: 'json',
        success: function (result) {
            if (result.Resultado == 1) {               
                window.location = controlador + "Exportar?file_name=" + result.Filename;

                if (tipo == 2) {
                    if (result.IndicadorFS == 1) {
                        mostrarMensaje('mensaje', 'alert', 'Se han encontrado valores FS en el reporte final.');
                    }
                }
            }
            else if (result.Resultado == 2){
                mostrarMensaje('mensaje', 'alert', 'No se ha generado el reporte dado que no existen datos.');
            }
            else
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
};

nuevoReporte = function () {
    setTimeout(function () {

        //mostrarMensaje('mensajeGeneracion', 'error', 'Se ha producido un error en el cálculo. Por favor intente nuevamente.');

        $('#popupEdicion').bPopup({
            autoClose: false
        });
    }, 50);
};

mostrarMensaje = function (id, tipo, mensaje) {
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);
}