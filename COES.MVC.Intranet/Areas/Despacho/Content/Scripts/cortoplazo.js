var controlador = siteRoot + 'despacho/cortoplazo/';

$(function () {

    $('#txtFecha').Zebra_DatePicker({               
        onSelect: function (date) {
            cargarEscenarios();
        }
    });

    $('#cbTipoCaso').on('change', function () {
        cargarEscenarios();
    });

    $('#btnConsultar').on('click', function () {
        $('#mensaje').hide();
        consultar();
    });

    $('#btnExportar').on('click', function () {
        $('#mensaje').hide();
        exportar();
    });

    $('#btnReporte').on('click', function () {
        generarReporte();
    });

    cargarEscenarios();

});

consultar = function () {
    if ($('#cbTipoInformacion').val() != "" && $('#cbTopologia').val() !="") {
        $.ajax({
            type: 'POST',
            url: controlador + "listado",
            data: {
                idEscenario: $('#cbTopologia').val(),
                fecha: $('#txtFecha').val(),
                tipoInformacion: $('#cbTipoInformacion').val()
            },
            cache: false,
            success: function (evt) {
                var ancho = parseInt( $('#mainLayout').width()) - 30;
                $('#listado').css("width", ancho + "px");

                $('#listado').html(evt);
                $('#tablaListado').dataTable({
                    "scrollY": "500px",
                    "scrollX": true,
                    "sDom": 'ft',
                    "ordering": false,
                    "bAutoWidth": false,
                    "destroy": "true",
                    "iDisplayLength": -1,
                    //stripeClasses: [],
                    fixedColumns: {
                        leftColumns: 1
                    },
                });


                $("#tablaListado").DataTable().draw();
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
            }
        });
    }
    else {
        mostrarMensaje('mensaje', 'alert', 'Seleccione tipo de información y un escenario');
    }
};


exportar = function () {
    $('#mensaje').hide();
    if ($('#cbTipoInformacion').val() != "" && $('#cbTopologia').val() !="") {
        $.ajax({
            type: 'POST',
            url: controlador + "exportar",
            data: {
                idEscenario: $('#cbTopologia').val(),
                fecha: $('#txtFecha').val(),
                tipoInformacion: $('#cbTipoInformacion').val()
            },
            dataType: 'json',
            cache: false,
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
        mostrarMensaje('mensaje', 'alert', 'Seleccione tipo de información y un escenario.');
    }
};

generarReporte = function () {

    $.ajax({
        type: 'POST',
        url: controlador + "generarreporte",
        data: {
            fecha: $('#txtFecha').val()
        },
        dataType: 'json',
        cache: false,
        success: function (resultado) {
            if (resultado.Result == 1) {
                location.href = controlador + "descargarreporte?fecha=" + resultado.Fecha;
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

cargarEscenarios = function () {
    $.ajax({
        type: 'POST',
        url: controlador + 'obtenerescenarios',
        data: {
            fecha: $('#txtFecha').val(),
            tipo: $('#cbTipoCaso').val()
        },
        dataType: 'json',
        global: false,
        success: function (result) {
            if (result != -1) {

                $('#cbTopologia').get(0).options.length = 0;
                $('#cbTopologia').get(0).options[0] = new Option("--SELECCIONE--", "0");
                $.each(result, function (i, item) {
                    $('#cbTopologia').get(0).options[$('#cbTopologia').get(0).options.length] = new Option(item.Topcodi + "-" + item.Topnombre, item.Topcodi);
                });              
            }
            else {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            }
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        }
    });
};

mostrarMensaje = function (id, tipo, mensaje) {
    $('#' + id).show();
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);
};
