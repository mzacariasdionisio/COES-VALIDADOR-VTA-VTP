var controlador = siteRoot + 'cortoplazo/configuracion/'

$(function () {

    $('#tab-container').easytabs({
        animate: false
    });

    $('#btnGrabar').on('click', function () {
        confirmar();
    });

    $('#btnGrabarAngulo').on('click', function () {
        confirmar();
    });

    $('#btnCancelar').on('click', function () {
        $('#popupConfirmar').bPopup().close();
    });

    $('#btnConfirmar').on('click', function () {

        var tabActivo = $("#tab-container .tab.active a").attr("href");
        if (tabActivo == "#configuracion")
            grabar();
        else
            grabarAngulo();
    });
});

function confirmar() {

    var tabActivo = $("#tab-container .tab.active a").attr("href");
    var mensaje = "";
    if (tabActivo == "#configuracion") {
        mensaje = validar();
        if (mensaje == "") {
            mostrarMensaje('mensaje', 'message', 'Por favor confirme la operación');
            $('#popupConfirmar').bPopup({

            });
        }
        else {
            mostrarMensaje('mensaje', 'alert', mensaje);
        }

    }
    else {
        mensaje = validarAngulo();
        if (mensaje == "") {
            mostrarMensaje('mensaje2', 'message', 'Por favor confirme la operación');
            $('#popupConfirmar').bPopup({

            });
        }
        else {
            mostrarMensaje('mensaje2', 'alert', mensaje);
        }

    }
};

function grabar() {
    $.ajax({
        type: 'POST',
        url: controlador + 'grabarumbral',
        data: $('#frmRegistro').serialize(),
        dataType: 'json',
        global: false,
        success: function (result) {
            if (result.Resultado == '-1') {
                alert('Ha ocurrido un error:' + result.Mensaje);
                $('#popupConfirmar').bPopup().close();
            } else {
                mostrarMensaje('mensaje', 'exito', 'Los datos se grabaron correctamente.');
                $('#popupConfirmar').bPopup().close();
            }
        },
        error: function () {
            alert('Ha ocurrido un error.');
        }
    });
};

function grabarAngulo() {
    $.ajax({
        type: 'POST',
        url: controlador + 'grabarumbral',
        data: $('#frmRegistroAngulo').serialize(),
        dataType: 'json',
        global: false,
        success: function (result) {
            if (result.Resultado == '-1') {
                alert('Ha ocurrido un error:' + result.Mensaje);
                $('#popupConfirmar').bPopup().close();
            } else {
                mostrarMensaje('mensaje2', 'exito', 'Los datos se grabaron correctamente.');
                $('#popupConfirmar').bPopup().close();
            }
        },
        error: function () {
            alert('Ha ocurrido un error.');
        }
    });
};

function validar() {

    var mensaje = "<ul>";
    var flag = true;

    if ($('#txtHoraOperacion').val() == "") {
        mensaje = mensaje + "<li>Ingrese % HOP vs Despacho Ejecutado.</li>";
        flag = false;
    }
    else {
        if (validarNumero($('#txtHoraOperacion').val())) {
            var porcentaje = parseFloat($('#txtHoraOperacion').val());

            if (porcentaje <= 0 || porcentaje > 100) {
                mensaje = mensaje + "<li>El % HOP vs Despacho Ejecutado debe ser un número mayor a cero y menor o igual a 100.</li>";
                flag = false;
            }
        }
        else {
            mensaje = mensaje + "<li>El % HOP vs Despacho Ejecutado debe ser un número.</li>";
            flag = false;
        }
    }

    if ($('#txtGeneracionEMS').val() == "") {
        mensaje = mensaje + "<li>Ingrese umbral Generación EMS vs Despacho Ejecutado.</li>";
        flag = false;
    }
    else {
        if (validarNumero($('#txtGeneracionEMS').val())) {
            var umbral = parseFloat($('#txtGeneracionEMS').val());

            if (umbral <= 0) {
                mensaje = mensaje + "<li>El umbral Generación EMS vs Despacho Ejecutado debe ser un número mayor a cero.</li>";
                flag = false;
            }
        }
        else {
            mensaje = mensaje + "<li>El umbral Generación EMS vs Despacho Ejecutado debe ser un número.</li>";
            flag = false;
        }
    }

    if ($('#txtDemandaEMS').val() == "") {
        mensaje = mensaje + "<li>Ingrese umbral Demanda EMS vs Demanda Ejecutada.</li>";
        flag = false;
    }
    else {
        if (validarNumero($('#txtDemandaEMS').val())) {
            var umbral = parseFloat($('#txtDemandaEMS').val());

            if (umbral <= 0) {
                mensaje = mensaje + "<li>El umbral Demanda EMS vs Demanda Ejecutada debe ser un número mayor a cero.</li>";
                flag = false;
            }
        }
        else {
            mensaje = mensaje + "<li>El umbral Demanda EMS vs Demanda Ejecutada debe ser un número.</li>";
            flag = false;
        }
    }

    if ($('#txtCI').val() == "") {
        mensaje = mensaje + "<li>Ingrese costo incremental.</li>";
        flag = false;
    }
    else {
        if (validarNumero($('#txtCI').val())) {
            var umbral = parseFloat($('#txtCI').val());

            if (umbral <= 0) {
                mensaje = mensaje + "<li>El costo incremental debe ser un número mayor a cero.</li>";
                flag = false;
            }
        }
        else {
            mensaje = mensaje + "<li>El costo incremental debe ser un número.</li>";
            flag = false;
        }
    }

    mensaje = mensaje + "</ul>";

    if (flag) mensaje = "";

    return mensaje;
};

function validarAngulo() {

    var mensaje = "<ul>";
    var flag = true;

    if ($('#txtNumIter').val() == "") {
        mensaje = mensaje + "<li>Ingrese número de iteraciones.</li>";
        flag = false;
    }
    else {
        if (validarNumero($('#txtNumIter').val())) {
            var umbral = parseFloat($('#txtNumIter').val());

            if (umbral <= 0) {
                mensaje = mensaje + "<li>El número de iteraciones debe ser un número mayor a cero.</li>";
                flag = false;
            }
        }
        else {
            mensaje = mensaje + "<li>El número de iteraciones debe ser un número.</li>";
            flag = false;
        }
    }

    if ($('#txtVarAng').val() == "") {
        mensaje = mensaje + "<li>Ingrese la variación del ángulo.</li>";
        flag = false;
    }
    else {
        if (validarNumero($('#txtVarAng').val())) {
            var umbral = parseFloat($('#txtVarAng').val());

            if (umbral <= 0) {
                mensaje = mensaje + "<li>La variación del ángulo debe ser un número mayor a cero.</li>";
                flag = false;
            }
        }
        else {
            mensaje = mensaje + "<li>La variación del ángulo debe ser un número.</li>";
            flag = false;
        }
    }
    mensaje = mensaje + "</ul>";

    if (flag) mensaje = "";

    return mensaje;
};

function mostrarMensaje(id, tipo, mensaje) {
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);
};

function validarNumero(texto) {
    return /^-?[\d.]+(?:e-?\d+)?$/.test(texto);
};