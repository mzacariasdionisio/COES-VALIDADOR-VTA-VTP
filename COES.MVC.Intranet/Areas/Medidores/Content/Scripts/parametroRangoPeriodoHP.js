$(function () {
    $('#btnNuevoRangoPeriodoHP').click(function () {
        nuevoRangoPeriodoHP();
    });

    cargarRangoPeriodoHP();
});

function cargarRangoPeriodoHP() {
    $('#mensaje').css("display", "none");

    $.ajax({
        type: 'POST',
        url: controlador + 'ListadoRangoPeriodoHP',
        data: {
        },
        success: function (data) {
            $('#rangoHP').html(data);

            $('#tablaRangoPeriodoHP').dataTable({
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



////////////////////////////////////////////////////////////////////////
/// Rango de Periodos de Hora Punta
////////////////////////////////////////////////////////////////////////
function nuevoRangoPeriodoHP() {
    $.ajax({
        type: 'POST',
        url: controlador + "NuevoRangoPeriodoHP",
        success: function (evt) {
            $('#nuevoRangoPeriodoHP').html(evt);

            $('#mensaje').css("display", "none");

            setTimeout(function () {
                $('#popupNuevoRangoPeriodoHP').bPopup({
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

function registrarRangoPeriodoHP() {
    var fechaInicio = $("#txtFechaInicioRangoPeriodoHP").val();
    var fechaFin = $("#txtFechaFinRangoPeriodoHP").val();
    var normativa = $("#txtNormativaVigente").val();

    if (validarRangoPeriodoHP() && confirm('¿Está seguro que desea guardar el Rango?')) {
        $.ajax({
            type: 'POST',
            url: controlador + 'RegistrarRangoPeriodoHP',
            data: {
                fechaInicio: fechaInicio,
                fechaFin: fechaFin,
                normativa: normativa
            },
            cache: false,
            success: function (resultado) {
                if (resultado == 1) {
                    mostrarExitoOperacion();
                    $('#popupNuevoRangoPeriodoHP').bPopup().close();

                    cargarRangoPeriodoHP();
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

function validarRangoPeriodoHP() {
    return true;
}

function editarRangoPeriodoHP(idRango) {
    $.ajax({
        type: 'POST',
        url: controlador + "EditarRangoPeriodoHP",
        data: {
            idRango: idRango
        },
        success: function (evt) {
            $('#editarRangoPeriodoHP').html(evt);

            $('#mensaje').css("display", "none");
            setTimeout(function () {
                $('#popupEditarRangoPeriodoHP').bPopup({
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

function actualizarRangoPeriodoHP() {
    var idRango = $("#hfParvcodi").val();
    var estado = $("#cbEstadoRangoPeriodoHP").val();
    var normativa = $("#txtEditarNormativa").val();

    if (validarRangoPeriodoHP() && confirm('¿Está seguro que desea actualizar el Rango?')) {
        $.ajax({
            type: 'POST',
            url: controlador + 'ActualizarRangoPeriodoHP',
            data: {
                idRango: idRango,
                estado: estado,
                normativa: normativa
            },
            success: function (resultado) {
                if (resultado == 1) {
                    mostrarExitoOperacion();
                    $('#popupEditarRangoPeriodoHP').bPopup().close();

                    cargarRangoPeriodoHP();
                } else
                    mostrarError(resultado);
            },
            error: function () {
                mostrarError();
            }
        });
    }
}

function verRangoPeriodoHP(idRango) {
    $("#mensaje").hide();

    $.ajax({
        type: 'POST',
        url: controlador + "VerRangoPeriodoHP",
        data: {
            idRango: idRango
        },
        success: function (evt) {
            $('#verRangoPeriodoHP').html(evt);
            $('#mensaje').css("display", "none");

            setTimeout(function () {
                $('#popupVerRangoPeriodoHP').bPopup({
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
