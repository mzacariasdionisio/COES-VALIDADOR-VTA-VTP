var controlador = siteRoot + "Rpf/Rpf/";

$(function () {
    $('#txtFecha').Zebra_DatePicker({
        onSelect: function () {
            cargarCentrales();
            $('#cbPtoMedicion').get(0).options.length = 0;
            $('#cbPtoMedicion').get(0).options[0] = new Option("--SELECCIONE--", "");
        },
    });

    $('#cbEmpresa').change(function () {
        cargarCentrales();
        $('#cbPtoMedicion').get(0).options.length = 0;
        $('#cbPtoMedicion').get(0).options[0] = new Option("--SELECCIONE--", "");
    });

    $('#cbCentral').change(function () {
        cargarPuntos();
    });
});


$(document).ajaxStart(function () {
    $('#progress').css('display', 'block');
});

$(document).ajaxStop(function () {
    $('#progress').css('display', 'none');
});

function cargarCentrales() {
    if ($('#cbEmpresa').val() != "") {
        var fecha = $('#txtFecha').val();
        $.ajax({
            type: 'POST',
            url: controlador + 'cargarcentrales',
            dataType: 'json',
            data: { fecha: fecha, idEmpresa: $('#cbEmpresa').val() },
            cache: false,
            success: function (aData) {
                $('#cbCentral').get(0).options.length = 0;
                $('#cbCentral').get(0).options[0] = new Option("--SELECCIONE--", "");
                $.each(aData, function (i, item) {
                    $('#cbCentral').get(0).options[$('#cbCentral').get(0).options.length] = new Option(item.Text, item.Value);
                });
            },
            error: function () { mostrarError(); }
        });
    }
    else {
        $('option', '#cbCentral').remove();
        $('option', '#cbPtoMedicion').remove();
    }
}

function cargarPuntos() {
    if ($('#cbCentral').val() != "") {
        var fecha = $('#txtFecha').val();
        $.ajax({
            type: 'POST',
            url: controlador + 'cargarptosmedicion',
            dataType: 'json',
            data: { fecha: fecha, idEquipo: $('#cbCentral').val(), emprcodi: $('#cbEmpresa').val() },
            cache: false,
            success: function (aData) {
                $('#cbPtoMedicion').get(0).options.length = 0;
                $('#cbPtoMedicion').get(0).options[0] = new Option("--SELECCIONE--", "");
                $.each(aData, function (i, item) {
                    $('#cbPtoMedicion').get(0).options[$('#cbPtoMedicion').get(0).options.length] = new Option(item.Text, item.Value);
                });
            },
            error: function () { mostrarError() }
        });
    }
    else {
        $('option', '#cbPtoMedicion').remove();
    }
}

function consultarDatos() {
    var ptoMedicion = $('#cbPtoMedicion').val();
    var idConsulta = $('#cbConsulta').val();
    var fecha = $('#txtFecha').val();

    if (ptoMedicion != '') {
        if (idConsulta != '') {
            if (fecha != '') {
                $.ajax({
                    type: 'POST',
                    url: controlador + 'grilla',
                    data: { fecha: fecha, ptoMedicion: ptoMedicion, horaInicio: $('#cbHoraDesde').val(), horaFin: $('#cbHoraHasta').val(), idTipoDato: idConsulta },
                    success: function (evt) {
                        limpiarMensaje();
                        $('#listado').show();
                        $('#listado').html(evt.Resultado);
                    },
                    error: function () {
                        mostrarError();
                    }
                });
            }
            else {
                mostrarAlerta("Seleccione fecha.");
            }
        }
        else {
            mostrarAlerta("Seleccione tipo de dato a consultar.");
        }
    }
    else {
        mostrarAlerta("Seleccione punto de medición.");
    }

}

function mostrarError() {
    $('#mensaje').removeClass("action-alert");
    $('#mensaje').removeClass("action-error");
    $('#mensaje').removeClass("action-exito");

    $('#mensaje').text("Ha ocurrido un error.");
    $('#mensaje').addClass("action-error");
}

function mostrarAlerta(msg) {
    $('#mensaje').removeClass("action-alert");
    $('#mensaje').removeClass("action-error");
    $('#mensaje').removeClass("action-exito");

    $('#mensaje').text(msg);
    $('#mensaje').addClass("action-alert");
}

function limpiarMensaje() {
    $('#mensaje').removeClass("action-alert");
    $('#mensaje').removeClass("action-error");
    $('#mensaje').removeClass("action-exito");
    $('#mensaje').text("");
}