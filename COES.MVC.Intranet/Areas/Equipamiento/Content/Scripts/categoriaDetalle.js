var controlador = siteRoot + 'Equipamiento/Categoria/';
$(function () {
    mostrarListaCategoriaDetalle();

    $('#btnBuscar').click(function () {
        mostrarListaCategoriaDetalle();
    });

    $('#btnNuevo').click(function () {
        nuevoCategoriaDetalle();
    });

    $("#btnRegresar").click(function () {
        regresarCategoria();
    });

    $(window).resize(function () {
        $('#listado').css("width", $('#mainLayout').width() + "px");
    });
});

function mostrarListaCategoriaDetalle() {
    $("#mensaje").hide();

    $.ajax({
        type: 'POST',
        url: controlador + "/ListaCategoriaDetalle",
        data: {
            ctgcodi: $("#hfCtgcodi").val()
        },
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
            mostrarError();
        }
    });
}

function nuevoCategoriaDetalle() {
    $.ajax({
        type: 'POST',
        url: controlador + "NuevoCategoriaDetalle",
        data: {
            ctgcodi: $("#hfCtgcodi").val()
        },
        success: function (evt) {
            $('#nuevaCategoriaDetalle').html(evt);
            $('#mensaje').css("display", "none");
            setTimeout(function () {
                $('#popupNuevoCategoriaDetalle').bPopup({
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

function verCategoriaDetalle(idcategoriadet) {
    $("#mensaje").hide();

    $.ajax({
        type: 'POST',
        url: controlador + "VerCategoriaDetalle",
        data: {
            idCategoriaDet: idcategoriadet
        },
        success: function (evt) {
            $('#verCategoriaDetalle').html(evt);
            $('#mensaje').css("display", "none");
            setTimeout(function () {
                $('#popupVerCategoriaDetalle').bPopup({
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

function editarCategoriaDetalle(idcategoriadet) {
    $.ajax({
        type: 'POST',
        url: controlador + "EditarCategoriaDetalle",
        data: {
            idCtgdet: idcategoriadet
        },
        success: function (evt) {
            $('#editarCategoria').html(evt);
            $('#mensaje').css("display", "none");
            setTimeout(function () {
                $('#popupEditarCategoria').bPopup({
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

function eliminarCategoriaDetalle(idcategoriadet) {
    if (confirm('¿Está seguro que desea eliminar la subcategoria?')) {
        $.ajax({
            type: 'POST',
            url: controlador + 'EliminarCategoriaDetalle',
            dataType: 'json',
            data: {
                idCtgdet: idcategoriadet
            },
            cache: false,
            success: function (resultado) {
                switch (resultado) {
                    case 1:
                        mostrarExitoOperacion();
                        mostrarListaCategoriaDetalle();
                        break;
                    case -1:
                        mostrarError();
                        break;
                    default:
                        mostrarError(resultado);
                        break;
                }
            },
            error: function () {
                mostrarError();
            }
        });
    }
}

function registrarCategoriaDetalle() {
    if (confirm('¿Está seguro que desea guardar la subcategoria?')) {
        $.ajax({
            type: 'POST',
            url: controlador + 'RegistrarCategoriaDetalle',
            dataType: 'json',
            data: $('#frmNewCategoriaDetalle').serialize(),
            cache: false,
            success: function (resultado) {
                if (resultado == 1) {
                    mostrarExitoOperacion();
                    $('#popupNuevoCategoriaDetalle').bPopup().close();
                    mostrarListaCategoriaDetalle();
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

function actualizarCategoriaDetalle() {
    if (confirm('¿Está seguro que desea actualizar la subcategoria?')) {
        $.ajax({
            type: 'POST',
            url: controlador + 'ActualizarCategoriaDetalle',
            dataType: 'json',
            data: $('#frmEditCategoriaDetalle').serialize(),
            cache: false,
            success: function (resultado) {
                if (resultado == 1) {
                    mostrarExitoOperacion();
                    $('#popupEditarCategoria').bPopup().close();
                    mostrarListaCategoriaDetalle();
                } else
                    mostrarError(resultado);
            },
            error: function () {
                mostrarError();
            }
        });
    }
}

function regresarCategoria() {
    location.href = controlador + "Index";
}

mostrarExitoOperacion = function () {
    $('#mensaje').removeClass("action-error");
    $('#mensaje').addClass("action-exito");
    $('#mensaje').text("La operación se realizó con exito...!");
    $('#mensaje').css("display", "block");
};

mostrarError = function (msj) {
    var mensaje = "Ha ocurrido un error";
    if (msj != undefined && msj != null && msj!= -1) {
        mensaje = msj;
    }

    $('#mensaje').removeClass("action-exito");
    $('#mensaje').addClass("action-error");
    $('#mensaje').text(mensaje);
    $('#mensaje').css("display", "block");
};