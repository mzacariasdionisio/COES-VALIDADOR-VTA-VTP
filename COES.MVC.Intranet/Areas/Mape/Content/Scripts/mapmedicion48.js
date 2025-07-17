var controlador = siteRoot + 'mape/mape/';
$(function () {

    $('#txtFecha').Zebra_DatePicker({
    });

    $('#btnGenerar').on('click',
        function () {
            generarMape();
        }
    );

    $('#btnConsultar').on('click',
        function () {
            consultarMape();
        }
    );
    consultarMape();

});


function generarMape() {
    $.ajax({
        url: controlador + "GenerarMape",
        data: { fecha: $('#txtFecha').val() },
        type: 'POST',
        success: function (result) {
            switch (result.NRegistros) {
                case -1:
                    mostrarMensaje('mensaje', 'alert', 'No se encontraron datos Programados ó Ejecutados para la generación.');
                    break;
                case 0:
                    mostrarMensaje('mensaje', 'alert', 'No se encontraron datos para la generación.');
                    break;
                case 1:
                    mostrarMensaje('mensaje', 'exito', 'Los datos se generaron correctamente.');
                    break;
            }
            consultarMape();
            setTimeout(function () {
                mostrarMensaje('mensaje', 'message', 'Seleccione la fecha a ser procesada y presione el botón "Generar"');
            }, 6000);
        },
        error: function (xhr, status) {
        },
        complete: function (xhr, status) {
        }
    });
}


function consultarMape() {

    $.ajax({
        url: controlador + "ConsultarMapMedicion48",
        data: { fecha: $('#txtFecha').val() },
        type: 'POST',
        success: function (result) {
            $('#listado').html(result.Resultado);

            $('#reporte').dataTable();
        },
        error: function (xhr, status) {
        },
        complete: function (xhr, status) {
        }
    });
};


function EliminarMape(vermcodi, fecha) {
    if (confirm('¿Está seguro de realizar esta operación?')) {
        $.ajax({
            type: "POST",
            url: controlador + "EliminarMapMedicion48",
            data: {
                vermcodi: vermcodi, fecha: fecha
            },
            datatype: "json",
            success: function (result) {
                if (result === 1) {
                    mostrarMensaje('mensaje', 'exito', 'La operación se realizó correctamente.');
                    consultarMape();

                    setTimeout(function () {
                        mostrarMensaje('mensaje', 'message', 'Seleccione la fecha a ser procesada y presione el botón "Generar"');
                    }, 4000);
                }
                else {
                    mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
                }
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            }
        });
    }
}

function GenerarExcelMape(version) {

    $.ajax({
        url: controlador + "ExportarMapeVersion",
        data: { vermcodi: version },
        type: 'POST',
        dataType: 'json',
        cache: false,
        success: function (result) {
            switch (result.NRegistros) {
                case 1: window.location = controlador + "Exportar?fi=" + result.Resultado; break;// si hay elementos
                case 2: alert("No existen registros !"); break;// sino hay elementos
                case -1: alert("Error en reporte result"); break;// Error en C#
            }
        },
        error: function (xhr, status) {
        },
        complete: function (xhr, status) {
        }
    });
}


function consultarDetalleMape(vermcodi, fecha) {

    $.ajax({
        type: 'POST',
        url: controlador + 'ConsultarDetalleMapMedicion48',
        data: { vermcodi: vermcodi, fecha: fecha },
        global: false,
        dataType: 'json',
        success: function (result) {

            $('#listadoDetalle').html(result.Resultado);
            $('#reporteDetalle').dataTable({
                "searching": false,
                "pageLength": 24,
                "ordering": false,
                "sDom": 'tp'
            });

            openPopupCrear();

        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        }
    });
}
function openPopupCrear() {
    setTimeout(function () {
        $('#popupDetalle').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
            modalClose: false
        });
    }, 50);
}


mostrarMensaje = function (id, tipo, mensaje) {
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);;
}



