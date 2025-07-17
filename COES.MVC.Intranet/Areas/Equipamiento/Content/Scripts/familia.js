var controlador = siteRoot + 'Equipamiento/Familia/';
$(function () {
    $('#btnBuscar').click(function () {
        buscarFamilias();
    });
    buscarFamilias();
    $('#btnNuevo').click(function() {
        nuevaFamilia();
    });
    $(window).resize(function () {
        $('#listado').css("width", $('#mainLayout').width() + "px");
    });
    $("#popupDetalleFam").addClass("general-popup");
    $("#popupNuevaFam").addClass("general-popup");
    $("#popupEditarFam").addClass("general-popup");
});
function buscarFamilias() {
    //ocultarMensaje();
    mostrarListado(1);
};
function mostrarListado(nroPagina) {
    $('#hfNroPagina').val(nroPagina);

    $.ajax({
        type: 'POST',
        url: controlador + "/Lista",
        data: {
            sFamEstado: $('#cbEstadoFamilia').val()
        },
        success: function (evt) {
            $('#listado').css("width", $('#mainLayout').width() + "px");

            $('#listado').html(evt);
            $('#tabla').dataTable({
                "scrollY": 430,
                "scrollX": true,
                "sDom": 't',
                "ordering": false,
                "iDisplayLength": 100
            });
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
};
mostrarDetalle = function (famcodi) {
    $.ajax({
        type: 'POST',
        url: controlador + "DetalleFamilia",
        data: {
            famcodi: famcodi
        },
        success: function (evt) {
            $('#detalleFamilia').html(evt);
            $('#mensaje').css("display", "none");
            setTimeout(function () {
                $('#popupDetalleFam').bPopup({
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
};
nuevaFamilia = function () {
    $.ajax({
        type: 'POST',
        url: controlador + "NuevaFamilia",
        success: function (evt) {
            $('#nuevaFamilia').html(evt);
            $('#mensaje').css("display", "none");
            setTimeout(function () {
                $('#popupNuevaFam').bPopup({
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
};
guardarFamilia = function () {
    if (confirm('¿Está seguro que desea guardar el tipo de equipo?')) {
        $.ajax({
            type: 'POST',
            url: controlador + 'GuardarFamilia',
            dataType: 'json',
            data: $('#frmNewFamilia').serialize(),
            cache: false,
            success: function (resultado) {
                if (resultado == 1) {
                    mostrarExitoOperacion();
                    $('#popupNuevaFam').bPopup().close();
                    mostrarListado();
                } else
                    mostrarError();
            },
            error: function () {
                mostrarError();
            }
        });
    }
};
editarFamilia = function (famcodi) {
    $.ajax({
        type: 'POST',
        url: controlador + "EditarFamilia",
        data: {
            famcodi: famcodi
        },
        success: function (evt) {
            $('#editarFamilia').html(evt);
            $('#mensaje').css("display", "none");
            setTimeout(function () {
                $('#popupEditarFam').bPopup({
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
};
actualizarFamilia = function () {
    if (confirm('¿Está seguro que desea actualizar el tipo de equipo?')) {
        $.ajax({
            type: 'POST',
            url: controlador + 'ActualizarFamilia',
            dataType: 'json',
            data: $('#frmEditFamilia').serialize(),
            cache: false,
            success: function (resultado) {
                if (resultado == 1) {
                    mostrarExitoOperacion();
                    $('#popupEditarFam').bPopup().close();
                    mostrarListado();
                } else
                    mostrarError();
            },
            error: function () {
                mostrarError();
            }
        });
    }
};
mostrarExitoOperacion = function () {
    $('#mensaje').removeClass("action-error");
    $('#mensaje').addClass("action-exito");
    $('#mensaje').text("La operación se realizó con exito...!");
    $('#mensaje').css("display", "block");
};

mostrarError = function () {
    $('#mensaje').removeClass("action-exito");
    $('#mensaje').addClass("action-error");
    $('#mensaje').text("Ha ocurrido un error");
    $('#mensaje').css("display", "block");
};