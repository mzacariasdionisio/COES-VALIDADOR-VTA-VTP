var controler = siteRoot + "supervisionplanificacion/rpfenergiapotencia/";

$(document).ready(function () {

    $('#btnBuscar').click(function () {
        listar();
    });
});

$(document).ready(function () {

    $('#btnGrabar').click(function () {

        grabar("update");


    });
});

$(document).ready(function () {

    $('#btnInsertar').click(function () {

        grabar("new");

    });
});

$(document).ready(function () {

    $('#btnRegresar').click(function () {
        regresar();
    });
});

$(document).ready(function () {

    $('#btnEliminar').click(function () {
        eliminar();
    });
});

$(document).ready(function () {

    $('#btnAnadir').click(function () {
        mostrarDetalle();
    });
});

$(function () {

    $('#txtFechaini').Zebra_DatePicker({
        format: 'm/Y'

    });

    $('#txtFechafin').Zebra_DatePicker({
        format: 'm/Y'
    });

    $('#txtFecha').Zebra_DatePicker({
        format: 'm/Y'
        
    });



});

function mostrarDetalle(fecha) {
    location.href = controler + "detalle?id=" + fecha;

}



function regresar() {
    location.href = controler + "index";

}

function listar() {

    $.ajax({
        type: 'POST',
        url: controler + "grilla",
        data: { fechaini: $('#txtFechaini').val(), fechafin: $('#txtFechafin').val() },

        success: function (evt) {
            $('#listado').html(evt);
            oTable = $('#tabla').dataTable({
                "sPaginationType": "full_numbers",
                "destroy": "true"
            });
        },
        error: function () {
            mostrarError("Error.");

        }
    });
}

function grabar(parametr) {
    if ($('#txtFecha').val() == "" || $('#rpftotal').val() == "" || $('#rpfmedia').val() == "" ||
       $('#eneind').val() == "" || $('#potind').val() == "") {
        mostrarError("Debe ingresar todo los parámetros.");
    }
    else {
        $.ajax({
            type: 'POST',
            url: controler + 'grabar',
            dataType: 'json',
            data: {
                fecha: $('#txtFecha').val(),
                rpftotal: $('#rpftotal').val(),
                rpfmedia: $('#rpfmedia').val(),
                eneind: $('#eneind').val(),
                potind: $('#potind').val(),
                parametr: parametr
            },
            cache: false,
            success: function (resultado) {
                switch (resultado) {
                    case 1: mostrarError("Archivo Grabado"); break;
                    case 2: mostrarError("No existe el Registro"); break;
                    case 3: mostrarError("No es posible añadir, ya existe el Registro"); break;
                    case -1: mostrarError("Error al grabar"); break;

                }

            },
            error: function () {
                mostrarError("Error.");
            }
        });
    }
}

function eliminar() {

    $.ajax({
        type: 'POST',
        url: controler + 'eliminar',
        dataType: 'json',
        data: {
            fecha: $('#txtFecha').val()
        },
        cache: false,
        success: function (resultado) {
            if (resultado == 1) {
                mostrarError("Archivo Eliminado");
                regresar();
            }
            else {
                mostrarError("Fecha a eliminar no existe");
            }
        },
        error: function () {
            mostrarError("Error.");
        }
    });
}

mostrarError = function (datos) {
    alert(datos);
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