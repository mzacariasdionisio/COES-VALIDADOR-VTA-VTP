$(function () {
    $('#btnNuevoRangoSolar').click(function () {
        nuevoRangoSolar();
    });

    cargarRangoOpCentral();
});

function cargarRangoOpCentral() {
    $('#mensaje').css("display", "none");

    $.ajax({
        type: 'POST',
        url: controlador + 'ListadoRangoOperacionCentralSolar',
        data: {
        },
        success: function (data) {
            $('#rangoOpCentral').html(data);

            $('#tablaSolar').dataTable({
                "scrollY": 400,
                "scrollX": true,
                "sDom": 't',
                "ordering": false,
                "bPaginate": false,
                "iDisplayLength": -1
            });

            mostrarHora();
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function mostrarHora() {

    dibujarHora('timeInicio', $("#txtHoraInicio").val());
    dibujarHora('timeFin', $("#txtHoraFin").val());
}

////////////////////////////////////////////////////////////////////////
/// Rango de Operación de Centrales Solares
////////////////////////////////////////////////////////////////////////
function nuevoRangoSolar() {
    $.ajax({
        type: 'POST',
        url: controlador + "NuevoRangoOperacionCentralSolar",
        success: function (evt) {
            $('#nuevoRangoSolar').html(evt);

            $('#mensaje').css("display", "none");

            setTimeout(function () {
                $('#popupNuevoRangoSolar').bPopup({
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

function registrarRangoSolar() {
    var horaInicio = $("#horaInicioRangoSolar").val();
    var horaFin = $("#horaFinRangoSolar").val();
    var fecha = $("#txtFechaInicioRangoSolar").val();

    if (validarRangoSolar() && confirm('¿Está seguro que desea guardar el Rango de Operación de Central Solar?')) {
        $.ajax({
            type: 'POST',
            url: controlador + 'RegistrarRangoOperacionCentralSolar',
            data: {
                fechaInicio: fecha,
                horaInicio: horaInicio,
                horaFin: horaFin
            },
            cache: false,
            success: function (resultado) {
                if (resultado == 1) {
                    mostrarExitoOperacion();
                    $('#popupNuevoRangoSolar').bPopup().close();

                    cargarRangoOpCentral();
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

function validarRangoSolar() {
    return true;
}

function editarRangoSolar(idHoraIni, idHoraFin) {
    $.ajax({
        type: 'POST',
        url: controlador + "EditarRangoOperacionCentralSolar",
        data: {
            idHoraIni: idHoraIni,
            idHoraFin: idHoraFin
        },
        success: function (evt) {
            $('#editarRangoSolar').html(evt);

            $('#mensaje').css("display", "none");
            setTimeout(function () {
                $('#popupEditarRangoSolar').bPopup({
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

function actualizarRangoSolar() {
    var idHoraIni = $("#hfHoraIniCodi").val();
    var idHoraFin = $("#hfHoraFinCodi").val();
    var horaInicio = $("#horaInicioRangoSolarEdit").val();
    var horaFin = $("#horaFinRangoSolarEdit").val();
    var estado = $("#cbEstadoRangoSolar").val();

    if (validarRangoSolar() && confirm('¿Está seguro que desea actualizar el Rango de Operación de Central Solar?')) {
        $.ajax({
            type: 'POST',
            url: controlador + 'ActualizarOperacionCentralSolar',
            data: {
                idHoraIni: idHoraIni,
                idHoraFin: idHoraFin,
                horaInicio: horaInicio,
                horaFin: horaFin,
                estado: estado
            },
            success: function (resultado) {
                if (resultado == 1) {
                    mostrarExitoOperacion();
                    $('#popupEditarRangoSolar').bPopup().close();

                    cargarRangoOpCentral();
                } else
                    mostrarError(resultado);
            },
            error: function () {
                mostrarError();
            }
        });
    }
}

function verRangoSolar(idHoraIni, idHoraFin) {
    $("#mensaje").hide();

    $.ajax({
        type: 'POST',
        url: controlador + "VerRangoOperacionCentralSolar",
        data: {
            idHoraIni: idHoraIni,
            idHoraFin: idHoraFin
        },
        success: function (evt) {
            $('#verRangoSolar').html(evt);
            $('#mensaje').css("display", "none");

            setTimeout(function () {
                $('#popupVerRangoSolar').bPopup({
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
