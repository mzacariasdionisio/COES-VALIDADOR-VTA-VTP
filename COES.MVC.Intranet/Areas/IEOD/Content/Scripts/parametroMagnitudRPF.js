var controlador = siteRoot + 'IEOD/Parametro/';
var TIPO_PERIODO_AVENIDA = 0;
var TIPO_PERIODO_ESTIAJE = 0;

$(function () {
    TIPO_PERIODO_AVENIDA = $("#periodo_avenida").val();
    TIPO_PERIODO_ESTIAJE = $("#periodo_estiaje").val();

    $('#btnNuevoMagnitudRPFAvenida').click(function () {
        nuevoMagnitudRPF(TIPO_PERIODO_AVENIDA);
    });
    $('#btnNuevoMagnitudRPFEstiaje').click(function () {
        nuevoMagnitudRPF(TIPO_PERIODO_ESTIAJE);
    });

    cargarMagnitudRPF();
});

function cargarMagnitudRPF() {
    $('#mensaje').css("display", "none");

    $.ajax({
        type: 'POST',
        url: controlador + 'ListadoMagnitudRPF',
        data: {
        },
        success: function (data) {
            $('#magnitudRPF').html(data);

            $('#tablaMagnitudRPF').dataTable({
                "scrollY": 400,
                "scrollX": true,
                "sDom": 't',
                "ordering": false,
                "bPaginate": false,
                "iDisplayLength": -1
            });
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function cargarFecha() {
    var anio = $("#Anho").val();
    var periodo = $("#periodo_actual").val();

    if (periodo == TIPO_PERIODO_AVENIDA) {
        $("#txtFechaInicioMagnitudRPF").val("01/01/" + anio);
        $("#txtFechaFinMagnitudRPF").val("01/01/" + anio);
    }

    if (periodo == TIPO_PERIODO_ESTIAJE) {
        $("#txtFechaInicioMagnitudRPF").val("01/06/" + anio);
        $("#txtFechaFinMagnitudRPF").val("01/06/" + anio);
    }
}
////////////////////////////////////////////////////////////////////////
/// Magnitud de la Reserva Rotante para la RPF
////////////////////////////////////////////////////////////////////////
function nuevoMagnitudRPF(periodo) {
    $.ajax({
        type: 'POST',
        url: controlador + "NuevoMagnitudRPF",
        data: {
            periodo: periodo
        },
        success: function (evt) {
            $('#nuevoMagnitudRPF').html(evt);

            $('#mensaje').css("display", "none");

            setTimeout(function () {
                $('#popupNuevoMagnitudRPF').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown'
                });
            }, 50);
        },
        error: function () {
            mostrarError();
        }
    });
}

function registrarMagnitudRPF() {
    var fechaInicio = $("#txtFechaInicioMagnitudRPF").val();
    var fechaFin = $("#txtFechaFinMagnitudRPF").val();
    var periodo = $("#periodo_actual").val()
    var magnitud = $("#txtMagnitud").val();

    if (confirm('¿Está seguro que desea guardar el registro?') && validarMagnitudRPF()) {
        $.ajax({
            type: 'POST',
            url: controlador + 'RegistrarMagnitudRPF',
            data: {
                fechaInicio: fechaInicio,
                fechaFin: fechaFin,
                periodo: periodo,
                magnitud: magnitud
            },
            cache: false,
            success: function (resultado) {
                if (resultado == 1) {
                    mostrarExitoOperacion();
                    $('#popupNuevoMagnitudRPF').bPopup().close();

                    cargarMagnitudRPF();
                } else {
                    mostrarError(resultado);
                }
            },
            error: function () {
                mostrarError();
            }
        });
    }
}

function validarMagnitudRPF() {
    var msj = '';
    var anio = $("#Anho").val();

    var periodo = $("#periodo_actual").val();
    var fechaIniText = $("#txtFechaInicioMagnitudRPF").val();
    var fechaFinText = $("#txtFechaFinMagnitudRPF").val();

    fechaIni = convertStringToDate(fechaIniText);
    fechaFin = convertStringToDate(fechaFinText);

    var anioIni = fechaIni.getFullYear();
    var mesIni = fechaIni.getMonth();
    var diaIni = fechaIni.getDay();

    var anioFin = fechaFin.getFullYear();
    var mesFin = fechaFin.getMonth();
    var diaFin = fechaFin.getDay();

    if (anioIni != anio || anioIni != anio) {
        msj = "La fecha de Inicio y/o Fin no corresponden al año seleccionado";
        alert(msj);
        return false;
    }

    if (periodo == TIPO_PERIODO_AVENIDA) {
        var fechaAv1IniText = "01/01/" + anio;
        var fechaAv1FinText = "31/05/" + anio;

        var fechaAv2IniText = "01/12/" + anio;
        var fechaAv2FinText = "31/12/" + anio;

        var fechaAv1Ini = convertStringToDate(fechaAv1IniText);
        var fechaAv1Fin = convertStringToDate(fechaAv1FinText);
        var fechaAv2Ini = convertStringToDate(fechaAv2IniText);
        var fechaAv2Fin = convertStringToDate(fechaAv2FinText);

        var msj1 = "";
        msj1 += (fechaIni >= fechaAv1Ini && fechaIni <= fechaAv1Fin) ? "" : "Fecha Inicial " + fechaIniText + " no pertenece al rango [" + fechaAv1IniText + "," + fechaAv1FinText + "]" + "\n";
        msj1 += (fechaFin >= fechaAv1Ini && fechaFin <= fechaAv1Fin) ? "" : "Fecha de Fin " + fechaIniText + " no pertenece al rango [" + fechaAv1IniText + "," + fechaAv1FinText + "]" + "\n";
        var b1 = msj1 != "";

        var msj2 = "";
        msj2 += (fechaIni >= fechaAv2Ini && fechaIni <= fechaAv2Fin) ? "" : "Fecha Inicial " + fechaIniText + " no pertenece al rango [" + fechaAv2IniText + "," + fechaAv2FinText + "]" + "\n";
        msj2 += (fechaFin >= fechaAv2Ini && fechaFin <= fechaAv2Fin) ? "" : "Fecha de Fin " + fechaIniText + " no pertenece al rango [" + fechaAv2IniText + "," + fechaAv2FinText + "]" + "\n";
        var b2 = msj2 != "";

        if (b1 && b2) {
            msj += msj1 + msj2;
        }
    }

    if (periodo == TIPO_PERIODO_ESTIAJE) {
        var fechaAv1IniText = "01/06/" + anio;
        var fechaAv1FinText = "30/11/" + anio;

        var fechaAv1Ini = convertStringToDate(fechaAv1IniText);
        var fechaAv1Fin = convertStringToDate(fechaAv1FinText);

        msj += (fechaIni >= fechaAv1Ini && fechaIni <= fechaAv1Fin) ? "" : "Fecha Inicial " + fechaIniText + " no pertenece al rango [" + fechaAv1IniText + "," + fechaAv1FinText + "]" + "\n";
        msj += (fechaFin >= fechaAv1Ini && fechaFin <= fechaAv1Fin) ? "" : "Fecha de Fin " + fechaIniText + " no pertenece al rango [" + fechaAv1IniText + "," + fechaAv1FinText + "]" + "\n";
    }

    if (msj != "") {
        alert(msj);
        return false;
    }
    return true;
}

function editarMagnitudRPF(idRango) {
    $.ajax({
        type: 'POST',
        url: controlador + "EditarMagnitudRPF",
        data: {
            idRango: idRango
        },
        success: function (evt) {
            $('#editarMagnitudRPF').html(evt);

            $('#mensaje').css("display", "none");
            setTimeout(function () {
                $('#popupEditarMagnitudRPF').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown'
                });
            }, 50);

        },
        error: function () {
            mostrarError();
        }
    });
}

function actualizarMagnitudRPF() {
    var idRango = $("#hfParvcodi").val();
    var estado = $("#cbEstadoMagnitudRPF").val();
    var periodo = $("#periodo_editar").val()
    var magnitud = $("#txtEditarMagnitud").val();

    if (validarMagnitudRPF() && confirm('¿Está seguro que desea actualizar el registro?')) {
        $.ajax({
            type: 'POST',
            url: controlador + 'ActualizarMagnitudRPF',
            data: {
                idRango: idRango,
                estado: estado,
                periodo: periodo,
                magnitud: magnitud
            },
            success: function (resultado) {
                if (resultado == 1) {
                    mostrarExitoOperacion();
                    $('#popupEditarMagnitudRPF').bPopup().close();

                    cargarMagnitudRPF();
                } else
                    mostrarError(resultado);
            },
            error: function () {
                mostrarError();
            }
        });
    }
}

function verMagnitudRPF(idRango) {
    $("#mensaje").hide();

    $.ajax({
        type: 'POST',
        url: controlador + "VerMagnitudRPF",
        data: {
            idRango: idRango
        },
        success: function (evt) {
            $('#verMagnitudRPF').html(evt);
            $('#mensaje').css("display", "none");

            setTimeout(function () {
                $('#popupVerMagnitudRPF').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown'
                });
            }, 50);
        },
        error: function () {
            mostrarError();
        }
    });
}

//
mostrarExitoOperacion = function () {
    $('#mensaje').removeClass("action-error");
    $('#mensaje').addClass("action-exito");
    $('#mensaje').text("La operación se realizó con exito...!");
    $('#mensaje').css("display", "block");
};

mostrarError = function (msj) {
    var mensaje = "Ha ocurrido un error";
    if (msj != undefined && msj != null && msj != -1) {
        mensaje = msj;
    }

    alert(msj);
};
