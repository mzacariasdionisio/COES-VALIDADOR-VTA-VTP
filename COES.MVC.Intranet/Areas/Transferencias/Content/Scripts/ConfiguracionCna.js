var controlador = siteRoot + 'Transferencias/EvaluacionParticipante/'
let domingo, lunes, martes, miercoles, jueves, viernes, sabado;

$(function () {
    
    $('#btnGrabarNewDia').click(function () {
        grabarConfiguracionDias();
    });

    $("#btnCancelarNewDia").click(function () {
        $('#popupNuevaConfiguracionDias').bPopup().close();
    });
    $("#cbSemana").change(function () {
        CargarFiltros($("#cbSemana").val());
    });
    $("#btnReiniciar").click(function () {
        ReiniciarContador();
    });

    $("#cbSemana").val($("#semanaActual").val());
    CargarFiltros($("#semanaActual").val());
    
});

function NuevaConfiguracionDias() {

    $.ajax({
        type: 'POST',
        url: controlador + 'ConfigurarDias',
        success: function (evtDias) {
            $('#contenidoDetalleDias').html(evtDias);

            setTimeout(function () {
                $('#popupNuevaConfiguracionDias').bPopup({
                    autoClose: false,
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown',
                    onClose: function () {
                        $('#popup').empty();
                    }
                });
            }, 500);
        },
        error: function (err) {
            alert("Lo sentimos, ha ocurrido un error inesperado");
        }
    });
}

function grabarConfiguracionDias() {
    if ($('#chkDomingo').is(':checked'))
        domingo = 'S';
    else
        domingo = 'N';

    if ($('#chkLunes').is(':checked'))
        lunes = 'S';
    else
        lunes = 'N';

    if ($('#chkMartes').is(':checked'))
        martes = 'S';
    else
        martes = 'N';

    if ($('#chkMiercoles').is(':checked'))
        miercoles = 'S';
    else
        miercoles = 'N';

    if ($('#chkJueves').is(':checked'))
        jueves = 'S';
    else
        jueves = 'N';

    if ($('#chkViernes').is(':checked'))
        viernes = 'S';
    else
        viernes = 'N';

    if ($('#chkSabado').is(':checked'))
        sabado = 'S';
    else
        sabado = 'N';

    $.ajax({
        type: 'POST',
        url: controlador + 'grabarConfiguracionDias',
        dataType: 'json',
        data: {
            diasemana: $("#cbSemana").val(),
            Dd: domingo,
            Dl: lunes,
            Dm: martes,
            Dmm: miercoles,
            Dj: jueves,
            Dvr: viernes,
            Ds: sabado
        },
        success: function (result) {
            if (result.Resultado == "1") {
                mostrarMensajeEval('mensajeDias', 'exito', "Los datos se guardaron correctamente.");
            }
            else if (result.Resultado == "-1") {
                mostrarMensajeEval('mensajeDias', 'alert', result.StrMensaje);
            }
        },
        error: function () {
            alert("Error");
        }
    });
}

function mostrarMensajeEval(id, tipo, mensaje) {
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);
};

function CargarFiltros(fecha) {
    $.ajax({
        type: 'POST',
        url: controlador + 'CargarSemanas',
        dataType: 'json',
        data: {
            fecha: fecha
        },
        success: function (evtDias) {
            if (evtDias.Resultado == "1") {
                document.getElementById("txtFechaInicio").innerHTML = evtDias.FechaInicio;
                document.getElementById("txtFechaFin").innerHTML = evtDias.FechaFin;

                if (evtDias.EntidadConfiguracionDias.Percnacodi > 0) {
                    if (evtDias.EntidadConfiguracionDias.Dd == "S")
                        document.getElementById("chkDomingo").checked = true;
                    else if (evtDias.EntidadConfiguracionDias.Dd == "N")
                        document.getElementById("chkDomingo").checked = false;

                    if (evtDias.EntidadConfiguracionDias.Dl == "S")
                        document.getElementById("chkLunes").checked = true;
                    else
                        document.getElementById("chkLunes").checked = false;

                    if (evtDias.EntidadConfiguracionDias.Dm == "S")
                        document.getElementById("chkMartes").checked = true;
                    else
                        document.getElementById("chkMartes").checked = false;

                    if (evtDias.EntidadConfiguracionDias.Dmm == "S")
                        document.getElementById("chkMiercoles").checked = true;
                    else
                        document.getElementById("chkMiercoles").checked = false;

                    if (evtDias.EntidadConfiguracionDias.Dj == "S")
                        document.getElementById("chkJueves").checked = true;
                    else
                        document.getElementById("chkJueves").checked = false;

                    if (evtDias.EntidadConfiguracionDias.Dvr == "S")
                        document.getElementById("chkViernes").checked = true;
                    else
                        document.getElementById("chkViernes").checked = false;

                    if (evtDias.EntidadConfiguracionDias.Ds == "S")
                        document.getElementById("chkSabado").checked = true;
                    else
                        document.getElementById("chkSabado").checked = false;
                }
                else {
                    document.getElementById("chkDomingo").checked = true;
                    document.getElementById("chkLunes").checked = true;
                    document.getElementById("chkMartes").checked = true;
                    document.getElementById("chkMiercoles").checked = true;
                    document.getElementById("chkJueves").checked = true;
                    document.getElementById("chkViernes").checked = true;
                    document.getElementById("chkSabado").checked = true;
                }
            }
            console.log(evtDias);
        },
        error: function (err) {
            alert("Lo sentimos, ha ocurrido un error inesperado");
        }
    });
}

function ReiniciarContador() {
    $.ajax({
        type: 'POST',
        url: controlador + 'ReiniciarContador',
        dataType: 'json',
        data: {
            emprcodi: $("#cbEmpresaConfig").val()
        },
        success: function (result) {          
            if (result.Resultado == "1") {
                mostrarMensajeEval('mensajeDias', 'exito', "El contador de correo se reinició correctamente.");
            }
            else if (result.Resultado == "-1") {
                mostrarMensajeEval('mensajeDias', 'alert', result.StrMensaje);
            }
        },
        error: function (err) {
            alert("Lo sentimos, ha ocurrido un error inesperado");
        }
    });
}

