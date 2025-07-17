var controlador = siteRoot + 'serviciorpfNuevo/analisisalternativo/'

$(function () {

    $('#tab-container').easytabs({
        animate: false
    });
    
    $('#txtFechaConsulta').Zebra_DatePicker({
        direction: -1
    });

    $('#btnProcesar').click(function () {
        procesar();
    });    

    cargarParametros();
});

procesar = function () {
    if ($('#txtFechaConsulta').val() != "" && $('#txtFrecuenciaMax').val() != "") {
        $.ajax({
            type: "POST",
            url: controlador + "seleccionrango",
            data: {
                fecha: $('#txtFechaConsulta').val(),
                varFmax: $('#txtFrecuenciaMax').val()
            },
            success: function (evt) {
                $('#seleccionRango').html(evt);
            },
            error: function () {
                mostrarError();
            }
        });
    }
    else {
        alert("Ingrese la fecha y FMax");
    }
}

cargarParametros = function () {
    $.ajax({
        type: "POST",
        url: controlador + "configuracion",
        success: function (evt) {
            $('#parametros').html(evt);
        },
        error: function () {
            mostrarError();
        }
    });
}

grabarParametro = function () {

    if ($('#txtVariaconPotencia').val() == "" || $('#txtConstanteD').val() == "" || $('#txtFrecuenciaNominal').val() == "" ||
        $('#txtBalance').val() == "" || $('#txtFrecuenciaLimMax').val() == "" || $('#txtFrecuenciaLimMin').val() == "" ||
        $('#txtNroIteraciones').val() == "" || $('#txtCantidadSinRango').val() == "" || $('#txtNroDatos').val() == "") {
            mostrarMensaje("Debe ingresar todo los parámetros.");
    }
    else {
        $.ajax({
            type: 'POST',
            url: controlador + 'grabarparametro',
            data: {
                variacionPotencia : $('#txtVariaconPotencia').val(),
                constanteD: $('#txtConstanteD').val(),
                frecuenciaNominal: $('#txtFrecuenciaNominal').val(),
                balance: $('#txtBalance').val(),
                frecuenciaLimMax: $('#txtFrecuenciaLimMax').val(),
                frecuenciaLimMin: $('#txtFrecuenciaLimMin').val(),
                nroIteraciones: $('#txtNroIteraciones').val(),
                cantidadSinRango: $('#txtCantidadSinRango').val(),
                nroDatos: $('#txtNroDatos').val()
            },
            dataType: 'json',
            cache: false,
            success: function (result) {
                if (result == 1) {
                    mostrarExito("Los parámetros fueron modificados correctamente.");
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

exportar = function () {
    $.ajax({
        type: 'POST',
        url: controlador + 'exportar',
        dataType: 'json',
        success: function (result) {
            if (result == 1) {
                window.location = controlador + 'descargar';
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

validarNumero = function (item, evt) {
    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;

    if (charCode == 46) {
        var regex = new RegExp(/\./g)
        var count = $(item).val().match(regex).length;
        if (count > 1) {
            return false;
        }
    }

    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }

    return true;
}

mostrarError = function () {
    alert("Ha ocurrido un error.");
}

mostrarExito = function (mensaje) {
    alert(mensaje);
}