var controlador = siteRoot + 'cortoplazo/reproceso/';
var hot = null;
$(function () {

    $('#txtFecha').Zebra_DatePicker({
        direction: false,
        onSelect: function (date) {
            $('#contentTabla').html("");
            $('#contentDatos').hide();
            validarModelo();
        }
    });

    $('#btnConsultar').on('click', function () {
        cargarPeriodos();
    });
    

    $('#btnReprocesar').on('click', function () {
        confirmReprocesar();
    });

    $('#btnCancelar').on('click', function () {
        $('#popupConfirmar').bPopup().close();
    });

    $('#btnConfirmar').on('click', function () {
        reprocesar();
    });

    $('#cbModelo').on('change', function () {
        validarModelo();
    });

    validarModelo();

});

cargarPeriodos = function () {     

    if ($('#txtFecha').val() != "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'periodos',
            data: {
                fecha: $('#txtFecha').val(),
                version: $('#cbModelo').val()
            },           
            success: function (evt) {
                $('#contentTabla').html(evt);

                $('#tablaPeriodo').dataTable({
                    "iDisplayLength": 50
                });

                $('#cbSelectAll').click(function (e) {
                    var table = $(e.target).closest('table');
                    $('td input:checkbox', table).prop('checked', this.checked);
                });

                $('#contentDatos').show();
                $('#contentBoton').show();

                $('#mensaje').removeClass();
                $('#mensaje').html('');

                $('#contenedorParametro').hide();
               
              
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
            }
        });
    }
    else {
        mostrarMensaje('mensaje', 'alert', 'Por favor seleccione una fecha.');
    }
};

confirmReprocesar = function () {
    $('#popupConfirmar').bPopup({
    });
};

reprocesar = function () {
    $('#mensaje').removeClass();
    $('#mensaje').html('');
    var horas = "";
    var newhoras = "";
    countHoras = 0;
    $('#tablaPeriodo tbody input:checked').each(function () {
        horas = horas + $(this).val() + ",";
        countHoras++;
    });

    if (countHoras > 0) {
        newhoras = horas.substring(0, horas.length - 1);
    }

    if (countHoras > 0) {

        $.ajax({
            type: 'POST',
            url: controlador + 'reprocesarva',
            data: {
                fecha: $('#txtFecha').val(),
                horas: newhoras,
                version: $('#cbModelo').val()
            },
            dataType: 'json',
            success: function (result) {
                if (result.Resultado == 1) {
                    mostrarMensaje('mensaje', 'alert', 'Se ha iniciado el reproceso, cuando finalice le llegará un correo indicando el resultado.');
                    $('#popupConfirmar').bPopup().close();
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
        mostrarMensaje('mensaje', 'alert', 'Por favor seleccione al menos un periodo');
    }
};

mostrarValidaciones = function (list) {
    var html = "";

    html = html + "<ul>";

    for (var i in list) {
        html = html + "<li>" + list[i] + "</li>";
    }

    html = html + "</ul>";
    mostrarMensaje('mensaje', 'alert', html);

};

mostrarMensaje = function (id, tipo, mensaje) {
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);
}

getFecha = function (date) {
    var parts = date.split("/");
    var date = new Date(parts[1] + "/" + parts[0] + "/" + parts[2]);
    return date.getTime();
};

function validarNumero(texto) {
    return /^-?[\d.]+(?:e-?\d+)?$/.test(texto);
};

validarModelo = function () {
    $('#mensaje').removeClass();
    $('#mensaje').html('');

    var mensaje = validarVersionModelo();

    if (mensaje != "") {
        mostrarMensaje('mensaje', 'alert', mensaje);
    }
};

validarVersionModelo = function () {
    var mensaje = "";
    var fechaProceso = getFecha($('#txtFecha').val());
    var fechaModelo = getFecha($('#hfFechaVigenciaPR07').val());
    var version = $('#cbModelo').val();
    if (fechaProceso < fechaModelo) {
        if (version == 2) {
            mensaje = "Se está utilizando una nueva versión del modelo para una fecha anterior a su publicación.";
        }
    }
    else {
        if (version == 1) {
            mensaje = "Está utilizando una versión antigua del modelo.";
        }
    }      

    return mensaje;
};