$(function () {
    $('#btnNuevoRangoPotenciaInductiva').click(function () {
        nuevoRangoPotenciaInductiva();
    });

    cargarRangoPotenciaInductiva();
});

function cargarRangoPotenciaInductiva() {
    $('#mensaje').css("display", "none");

    $.ajax({
        type: 'POST',
        url: controlador + 'ListadoRangoPotenciaInductiva',
        data: {
        },
        success: function (data) {
            $('#rangoPotenciaInductiva').html(data);

            $('#tablaRangoPotenciaInductiva').dataTable({
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
    dibujarHora('timeH1Ini', $("#txtH1Ini").val());
    dibujarHora('timeH1Fin', $("#txtH1Fin").val());
    dibujarHora('timeH2Ini', $("#txtH2Ini").val());
    dibujarHora('timeH2Fin', $("#txtH2Fin").val());
}

////////////////////////////////////////////////////////////////////////
/// Rango de Análisis de Potencia Inductiva
////////////////////////////////////////////////////////////////////////
function nuevoRangoPotenciaInductiva() {
    $.ajax({
        type: 'POST',
        url: controlador + "NuevoRangoPotenciaInductiva",
        success: function (evt) {
            $('#nuevoRangoPotenciaInductiva').html(evt);

            $('#mensaje').css("display", "none");

            setTimeout(function () {
                $('#popupNuevoRangoPotenciaInductiva').bPopup({
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

function registrarRangoPotenciaInductiva() {
    var fecha = $("#txtFechaInicioRangoPotenciaInductiva").val();
    var h1Ini = $("#h1IniRangoPotenciaInductiva").val();
    var h1Fin = $("#h1FinRangoPotenciaInductiva").val();
    var h2Ini = $("#h2IniRangoPotenciaInductiva").val();
    var h2Fin = $("#h2FinRangoPotenciaInductiva").val();

    if (validarRangoPotenciaInductiva() && confirm('¿Está seguro que desea guardar Rango de Análisis de Potencia Inductiva?')) {
        $.ajax({
            type: 'POST',
            url: controlador + 'RegistrarRangoPotenciaInductiva',
            data: {
                fechaInicio: fecha,
                h1Ini: h1Ini,
                h1Fin: h1Fin,
                h2Ini: h2Ini,
                h2Fin: h2Fin
            },
            cache: false,
            success: function (resultado) {
                if (resultado == 1) {
                    mostrarExitoOperacion();
                    $('#popupNuevoRangoPotenciaInductiva').bPopup().close();

                    cargarRangoPotenciaInductiva();
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

function validarRangoPotenciaInductiva() {
    return true;
}

function editarRangoPotenciaInductiva(idH1Ini, idH1Fin, idH2Ini, idH2Fin) {
    $.ajax({
        type: 'POST',
        url: controlador + "EditarRangoPotenciaInductiva",
        data: {
            idH1Ini: idH1Ini,
            idH1Fin: idH1Fin,
            idH2Ini: idH2Ini,
            idH2Fin: idH2Fin
        },
        success: function (evt) {
            $('#editarRangoPotenciaInductiva').html(evt);

            $('#mensaje').css("display", "none");
            setTimeout(function () {
                $('#popupEditarRangoPotenciaInductiva').bPopup({
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

function actualizarRangoPotenciaInductiva() {
    var idH1Ini = $("#hfH1Ini").val();
    var idH1Fin = $("#hfH1Fin").val();
    var idH2Ini = $("#hfH2Ini").val();
    var idH2Fin = $("#hfH2Fin").val();
    var h1Ini = $("#h1IniRangoPotenciaInductivaEdit").val();
    var h1Fin = $("#h1FinRangoPotenciaInductivaEdit").val();
    var h2Ini = $("#h2IniRangoPotenciaInductivaEdit").val();
    var h2Fin = $("#h2FinRangoPotenciaInductivaEdit").val();
    var estado = $("#cbEstadoRangoPotenciaInductiva").val();

    if (validarRangoPotenciaInductiva() && confirm('¿Está seguro que desea actualizar el Rango de Análisis de Potencia Inductiva?')) {
        $.ajax({
            type: 'POST',
            url: controlador + 'ActualizarRangoPotenciaInductiva',
            data: {
                idH1Ini: idH1Ini,
                idH1Fin: idH1Fin,
                idH2Ini: idH2Ini,
                idH2Fin: idH2Fin,
                h1Ini: h1Ini,
                h1Fin: h1Fin,
                h2Ini: h2Ini,
                h2Fin: h2Fin,
                estado: estado
            },
            success: function (resultado) {
                if (resultado == 1) {
                    mostrarExitoOperacion();
                    $('#popupEditarRangoPotenciaInductiva').bPopup().close();

                    cargarRangoPotenciaInductiva();
                } else
                    mostrarError(resultado);
            },
            error: function () {
                mostrarError();
            }
        });
    }
}

function verRangoPotenciaInductiva(idH1Ini, idH1Fin, idH2Ini, idH2Fin) {
    $("#mensaje").hide();

    $.ajax({
        type: 'POST',
        url: controlador + "VerRangoPotenciaInductiva",
        data: {
            idH1Ini: idH1Ini,
            idH1Fin: idH1Fin,
            idH2Ini: idH2Ini,
            idH2Fin: idH2Fin
        },
        success: function (evt) {
            $('#verRangoPotenciaInductiva').html(evt);
            $('#mensaje').css("display", "none");

            setTimeout(function () {
                $('#popupVerRangoPotenciaInductiva').bPopup({
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