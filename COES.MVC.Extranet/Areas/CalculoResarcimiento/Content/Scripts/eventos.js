var controlador = siteRoot + 'calculoresarcimiento/calidadproducto/';

$(function () {

    $('#btnConsultar').on('click', function () {
        consultar();
    });

    $('#spanCambiarEmpresa').on('click', function () {
        cambiarEmpresa();
    });

    $('#cbAnio').val($('#hfAnio').val());
    $('#cbMes').val($('#hfMes').val());

    consultar();
});

function consultar() {
    mostrarMensajeDefecto();
    if ($('#hfIdEmpresa').val() != "") {
        if ($('#cbAnio').val() != "0" && $('#cbMes').val() != "0") {
            $.ajax({
                type: 'POST',
                url: controlador + 'listado',
                data: {
                    empresa: $('#hfIdEmpresa').val(),
                    anio: $('#cbAnio').val(),
                    mes: $('#cbMes').val(),
                    buscar: $('#txtBuscar').val()
                },
                success: function (evt) {
                    $('#listado').html(evt);
                },
                error: function () {
                    mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
                }
            });
        }
        else {
            $('#listado').html("");
            if ($('#cbAnio').val() == "0" && $('#cbMes').val() == "0")
                mostrarMensaje('mensaje', 'alert', 'Seleccione año y mes.');
            else if ($('#cbAnio').val() == "0" && $('#cbMes').val() != "0")
                mostrarMensaje('mensaje', 'alert', 'Seleccione año.');
            else if ($('#cbAnio').val() != "0" && $('#cbMes').val() == "0")
                mostrarMensaje('mensaje', 'alert', 'Seleccione mes.');
        }
    }
    else {
        mostrarMensaje('mensaje', 'alert', 'Su usuario no se encuentra asociado a una empresa.');
    }
}

function cambiarEmpresa() {
    $.ajax({
        type: 'POST',
        url: controlador + 'empresa',
        success: function (evt) {
            $('#contenidoEmpresa').html(evt);
            setTimeout(function () {
                $('#popupEmpresa').bPopup({
                    autoClose: false
                });
            }, 50);

            $('#btnAceptar').on("click", function () {
                seleccionarEmpresa();
            });

            $('#btnCancelar').on("click", function () {
                $('#popupEmpresa').bPopup().close();
            });
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        }
    });
}

function seleccionarEmpresa() {
    if ($('#cbEmpresa').val() != "") {
        $('#hfIdEmpresa').val($('#cbEmpresa').val());
        $('#lbEmpresa').text($("#cbEmpresa option:selected").text());
        $('#popupEmpresa').bPopup().close();        
    }
    else {
        mostrarMensaje('mensajeEdicion', 'alert', 'Debe seleccionar empresa.');
    }
}

function mostrarMensajeDefecto() {
    mostrarMensaje('mensaje', 'message', 'Por favor seleccione los filtros de búsqueda.');
}

function cargarMediciones(id) {
    document.location.href = siteRoot + "calculoresarcimiento/medicion/index?id=" + id + "&empresa=" + $('#hfIdEmpresa').val();
}

function mostrarMensaje(id, tipo, mensaje) {
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);
}
