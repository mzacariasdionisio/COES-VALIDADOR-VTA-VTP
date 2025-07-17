var controlador = siteRoot + 'Equipamiento/';
$(function () {

    buscarTipoRel();

    $('#btnBuscar').click(function () {
        buscarTipoRel();
    });
    $("#popupTiporel").addClass("general-popup");
    $("#popUpDetalleTiporel").addClass("general-popup");
    $("#popUpEditarTiporel").addClass("general-popup");

    $('#btnNuevo').click(function () {
        NuevoTipoRel();
    });
    $('#NombreTiporel').keyup(function (e) {
        if (e.keyCode == 13) {
            buscarTipoRel();
        }
    });

});
function buscarTipoRel() {
    ocultarMensaje();
    mostrarListado(1);
}
function mostrarListado(nroPagina) {
    $('#hfNroPagina').val(nroPagina);

    $.ajax({
        type: 'POST',
        url: controlador + "Relacion/ListaTipoRel",
        data: $('#frmBusqueda').serialize(),
        success: function (evt) {
            $('#listado').css("width", $('#mainLayout').width() + "px");

            $('#listado').html(evt);
            $('#tabla').dataTable({
                "scrollY": 430,
                "scrollX": true,
                "sDom": 't',
                "ordering": false,
                "iDisplayLength": 50
            });
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
};
mostrarError = function () {
    $('#mensaje').removeClass("action-exito");
    $('#mensaje').addClass("action-error");
    $('#mensaje').text("Ha ocurrido un error");
    $('#mensaje').css("display", "block");
};
ocultarMensaje = function () {
    $('#mensaje').css("display", "none");
};
mostrarExitoOperacion = function () {
    $('#mensaje').removeClass("action-error");
    $('#mensaje').addClass("action-exito");
    $('#mensaje').text("Se realizó la operación..!");
    $('#mensaje').css("display", "block");
};
function mostrarDetalle(e) {
    $.ajax({
        type: 'POST',
        url: controlador + "Relacion/DetalleTipoRel?idTiporel=" + e,
        success: function (evt) {
            $('#detalletiporel').html(evt);
            $('#mensaje').css("display", "none");
            setTimeout(function () {
                $('#popUpDetalleTiporel').bPopup({
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

function editarTipoRel(e) {
    $.ajax({
        type: 'POST',
        url: controlador + "Relacion/EditarTipoRel?idTiporel=" + e,
        success: function (evt) {
            $('#editartiporel').html(evt);
            $('#mensaje').css("display", "none");
            setTimeout(function () {
                $('#popUpEditarTiporel').bPopup({
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
actualizar = function () {
    if (confirm('¿Está seguro de actualizar el tipo de relación?'))
    {
        $.ajax({
            type: 'POST',
            url: controlador + "Relacion/ActualizarTipoRel",
            data: {
                iTipoRelCodi: $("#Tiporelcodi").val(),
                sNombreTipoRel: $("#Tiporelnomb").val(),
                sEstado: $("#cbEstadoEdit").val()
            },
            success: function (resultado) {
                if (resultado == 1) {
                    mostrarExitoOperacion();
                    $('#popUpEditarTiporel').bPopup().close();
                    mostrarListado(1);
                } else
                    mostrarError();
            },
            error: function () {
                alert("Ha ocurrido un error");
            }
        });
    }
};
function NuevoTipoRel() {
    $.ajax({
        type: 'POST',
        url: controlador + "Relacion/NuevoTipoRel",
        success: function (evt) {
            $('#nuevotiporel').html(evt);
            $('#mensaje').css("display", "none");
            setTimeout(function () {
                $('#popupTiporel').bPopup({
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
guardarTipoRel = function () {
    if (confirm('¿Está seguro de agregar el nuevo tipo de relación?')) {
        $.ajax({
            type: 'POST',
            url: controlador + 'Relacion/GuardarTipoRel',
            dataType: 'json',
            data: {
                sNombreTipoRel: $("#txtNombreTipoRel").val(),
                sEstado: $("#cbEstadoNuevo").val()
            },
            cache: false,
            success: function (resultado) {
                if (resultado == 1) {
                    mostrarExitoOperacion();
                    $('#popupTiporel').bPopup().close();
                    mostrarListado(1);
                } else
                    mostrarError();
            },
            error: function () {
                mostrarError();
            }
        });
    }
};
mostrarFamRel = function (tipoRelcodi) {
    location.href = controlador + "Relacion/RelacionEquiposTipoRel?idTipoRel=" + tipoRelcodi;
};

mostrarEquipoRel = function (tipoRelcodi) {
    location.href = controlador + "Relacion/IndexEquiposRel?idTipoRel=" + tipoRelcodi;
};