var controlador = siteRoot + 'serviciorpfNuevo/frecuencia/'

$(function () {
    $('#txtFecha').Zebra_DatePicker({
    });

    $('#btnConsultar').click(function () {
        cargarDatos();
    });

    $('#btnReemplazar').click(function () {
        openReemplazo();
    });



    cargarDatos();
});

cargarDatos = function () {
    if ($('#txtFecha').val() != '') {
        $.ajax({
            type: "POST",
            url: controlador + "lista",
            data: { fecha: $('#txtFecha').val() },
            success: function (evt) {
                $('#listado').html(evt);
                $('#tabla').dataTable({
                    "iDisplayLength": 50,
                    sDom: 't'
                });
            },
            error: function () {
                mostrarError();
            }
        });
    }
    else { mostrarMensaje("Seleccione fecha"); }
}

completarDatos = function () {
    if (confirm('¿Está seguro de realizar esta operación?')) {
        $.ajax({
            type: "POST",
            url: controlador + "completar",
            data: { fecha: $('#txtFecha').val() },
            dataType: 'json',
            success: function (result) {
                if (result == 0) {
                    mostrarAlerta('No existen datos de los GPS para poder completar la data.');
                }
                else if (result == 1) {
                    mostrarExito('Los datos se han completado correctamente.');
                    cargarDatos();
                }
                else if (result == 2) {
                    mostrarAlerta('Algunos datos están en cero no existen datos en la fuente de datos.');
                    cargarDatos();
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

openReemplazo = function () {
    setTimeout(function () {
        $('#popupUnidad').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown'
        });
    }, 50);
}

reemplazar = function () {
    if (confirm("¿Está seguro de reemplazar los datos?")) {
        if ($('#cbGpsOrigen').val() != "" && $('#cbGpsDestino').val() != "") {
            if ($('#cbGpsOrigen').val() != $('#cbGpsDestino').val()) {
                $.ajax({
                    type: 'POST',
                    url: controlador + 'reemplazar',
                    data: { fecha: $('#txtFecha').val(), gpsorigen: $('#cbGpsOrigen').val(), gpsdestino: $('#cbGpsDestino').val() },
                    dataType: 'json',
                    success: function (result) {
                        if (result == 1) {
                            cancelar();
                            cargarDatos();
                        }
                        if (result == -1) {
                            mostrarError();
                        }
                    },
                    error: function () {
                        mostrarError();
                    }
                });
            }
            else {
                mostrarMensaje("No puede seleccionar el mismo GPS.");
            }
        }
        else {
            mostrarMensaje("Seleccione el GPS origen y GPS destino.");
        }
    }
}

cancelar = function () {
    $('#popupUnidad').bPopup().close();
}

descargarFrecuencia = function (gpscodi) {
    $.ajax({
        type: 'POST',
        url: controlador + 'exportar',
        data: {
            fecha: $('#txtFecha').val(),
            gpscodi: gpscodi
        },
        dataType: 'json',
        success: function (result) {
            if (result == 1) {
                window.location = controlador + "descargar";
            }
            if (result == -1) {
                mostrarError();
            }
        },
        error: function () {
            mostrarError();
        }
    });
}

descargarMacro = function (gpscodi) {
    var fecha = $('#txtFecha').val().split("/");
    nombre = fecha[0] + fecha[1];

    $.ajax({
        type: 'POST',
        url: controlador + 'exportarmacro',
        data: {
            fecha: $('#txtFecha').val(),
            gpscodi: gpscodi,
            nombre: nombre
        },
        dataType: 'json',
        success: function (result) {
            if (result == 1) {
                window.location = controlador + "descargarmacro?nombre=" + nombre;
            }
            if (result == -1) {
                mostrarError();
            }
        },
        error: function () {
            mostrarError();
        }
    });
}

mostrarError = function () {
    $('#mensaje').removeClass();
    $('#mensaje').addClass('action-error');
    $('#mensaje').text("Ha ocurrido un error.");
}

mostrarMensaje = function (mensaje) {
    alert(mensaje);
}

mostrarAlerta = function (mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').addClass('action-alert');
    $('#mensaje').text(mensaje);
}

mostrarExito = function (mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').addClass('action-exito');
    $('#mensaje').text(mensaje);
}