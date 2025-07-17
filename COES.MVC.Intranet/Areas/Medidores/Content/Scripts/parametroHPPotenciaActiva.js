$(function () {
    $('#btnNuevoHPPotenciaActiva').click(function () {
        nuevoHPPotenciaActiva();
    });

    cargarHPPotenciaActiva();
});

function cargarHPPotenciaActiva() {
    $('#mensaje').css("display", "none");

    $.ajax({
        type: 'POST',
        url: controlador + 'ListadoHPPotenciaActiva',
        data: {
        },
        success: function (data) {
            $('#hpPotenciaActiva').html(data);

            $('#tablaHPPotenciaActiva').dataTable({
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
    dibujarHora('timeMinima', $("#txtHoraMinima").val());
    dibujarHora('timeMedia', $("#txtHoraMedia").val());
    dibujarHora('timeMaxima', $("#txtHoraMaxima").val());
}

////////////////////////////////////////////////////////////////////////
/// Hora Punta para Potencia Activa
////////////////////////////////////////////////////////////////////////
function nuevoHPPotenciaActiva() {
    $.ajax({
        type: 'POST',
        url: controlador + "NuevoHPPotenciaActiva",
        success: function (evt) {
            $('#nuevoHPPotenciaActiva').html(evt);

            $('#mensaje').css("display", "none");

            setTimeout(function () {
                $('#popupNuevoHPPotenciaActiva').bPopup({
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

function registrarHPPotenciaActiva() {
    var horaMinima = $("#horaMinimaHPPotenciaActiva").val();
    var horaMedia = $("#horaMediaHPPotenciaActiva").val();
    var horaMaxima = $("#horaMaximaHPPotenciaActiva").val();
    var fecha = $("#txtFechaInicioHPPotenciaActiva").val();

    if (validarHPPotenciaActiva() && confirm('¿Está seguro que desea guardar la Hora Punta para Potencia Activa?')) {
        $.ajax({
            type: 'POST',
            url: controlador + 'RegistrarHPPotenciaActiva',
            data: {
                fechaInicio: fecha,
                horaMinima: horaMinima,
                horaMedia: horaMedia,
                horaMaxima: horaMaxima
            },
            cache: false,
            success: function (resultado) {
                if (resultado == 1) {
                    mostrarExitoOperacion();
                    $('#popupNuevoHPPotenciaActiva').bPopup().close();

                    cargarHPPotenciaActiva();
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

function validarHPPotenciaActiva() {
    return true;
}

function editarHPPotenciaActiva(idHoraMinima, idHoraMedia, idHoraMaxima) {
    $.ajax({
        type: 'POST',
        url: controlador + "EditarHPPotenciaActiva",
        data: {
            idHoraMinima: idHoraMinima,
            idHoraMedia: idHoraMedia,
            idHoraMaxima: idHoraMaxima
        },
        success: function (evt) {
            $('#editarHPPotenciaActiva').html(evt);

            $('#mensaje').css("display", "none");
            setTimeout(function () {
                $('#popupEditarHPPotenciaActiva').bPopup({
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

function actualizarHPPotenciaActiva() {
    var idHoraMinima = $("#hfHoraMinimaCodi").val();
    var idHoraMedia = $("#hfHoraMediaCodi").val();
    var idHoraMaxima = $("#hfHoraMaximaCodi").val();
    var horaMinima = $("#horaMinimaHPPotenciaActivaEdit").val();
    var horaMedia = $("#horaMediaHPPotenciaActivaEdit").val();
    var horaMaxima = $("#horaMaximaHPPotenciaActivaEdit").val();
    var estado = $("#cbEstadoHPPotenciaActiva").val();

    if (validarHPPotenciaActiva() && confirm('¿Está seguro que desea actualizar la Hora Punta para Potencia Activa?')) {
        $.ajax({
            type: 'POST',
            url: controlador + 'ActualizarHPPotenciaActiva',
            data: {
                idHoraMinima: idHoraMinima,
                idHoraMedia: idHoraMedia,
                idHoraMaxima: idHoraMaxima,
                horaMinima: horaMinima,
                horaMedia: horaMedia,
                horaMaxima: horaMaxima,
                estado: estado
            },
            success: function (resultado) {
                if (resultado == 1) {
                    mostrarExitoOperacion();
                    $('#popupEditarHPPotenciaActiva').bPopup().close();

                    cargarHPPotenciaActiva();
                } else
                    mostrarError(resultado);
            },
            error: function () {
                mostrarError();
            }
        });
    }
}

function verHPPotenciaActiva(idHoraMinima, idHoraMedia, idHoraMaxima) {
    $("#mensaje").hide();

    $.ajax({
        type: 'POST',
        url: controlador + "VerHPPotenciaActiva",
        data: {
            idHoraMinima: idHoraMinima,
            idHoraMedia: idHoraMedia,
            idHoraMaxima: idHoraMaxima
        },
        success: function (evt) {
            $('#verHPPotenciaActiva').html(evt);
            $('#mensaje').css("display", "none");

            setTimeout(function () {
                $('#popupVerHPPotenciaActiva').bPopup({
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

function eliminarHPPotenciaActiva(idHoraMinima, idHoraMedia, idHoraMaxima) {
    if (confirm("¿Desea eliminar el registro?")) {
        $.ajax({
            type: 'POST',
            url: controlador + "EliminarHPPotenciaActiva",
            data: {
                idHoraMinima: idHoraMinima,
                idHoraMedia: idHoraMedia,
                idHoraMaxima: idHoraMaxima
            },
            success: function (evt) {
                alert("La eliminación se realizó correctamente.");

                cargarHPPotenciaActiva();

            },
            error: function () {
                mostrarError();
            }
        });
    }
}