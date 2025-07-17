var controlador = siteRoot + 'Equipamiento/Categoria/';
$(function () {
    mostrarListaCategoria();

    $('#btnBuscar').click(function () {
        mostrarListaCategoria();
    });

    $('#btnNuevo').click(function () {
        $("#editarCategoria").html('');
        nuevaCategoria();
    });

    $("#cbTipoEquipoFiltro").change(function () {
        mostrarListaCategoria();
    });

    $("#cbEstadoCategoriaFiltro").change(function () {
        mostrarListaCategoria();
    });

    $(window).resize(function () {
        $('#listado').css("width", $('#mainLayout').width() + "px");
    });
});

function mostrarListaCategoria() {
    $("#mensaje").hide();

    $.ajax({
        type: 'POST',
        url: controlador + "ListaCategoriaByEstado",
        data: {
            idFamilia: $('#cbTipoEquipoFiltro').val(),
            sEstado: $("#cbEstadoCategoriaFiltro").val()
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

function nuevaCategoria() {
    $.ajax({
        type: 'POST',
        url: controlador + "NuevaCategoria",
        success: function (evt) {
            $('#nuevaCategoria').html(evt);
            $('#mensaje').css("display", "none");
            setTimeout(function () {
                $('#popupNuevaCategoria').bPopup({
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

function verCategoria(idcategoria) {
    $("#mensaje").hide();

    $.ajax({
        type: 'POST',
        url: controlador + "VerCategoria",
        data: {
            idCategoria: idcategoria
        },
        success: function (evt) {
            $('#verCategoria').html(evt);
            $('#mensaje').css("display", "none");
            setTimeout(function () {
                $('#popupVerCategoria').bPopup({
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

function detallesEquipo(ctg) {
    location.href = controlador + "IndexCategoriaDetalle?ctg=" + ctg;
}

function editarCategoria(idcategoria) {
    $.ajax({
        type: 'POST',
        url: controlador + "EditarCategoria",
        data: {
            idCategoria: idcategoria
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

function actualizarListaCategoriaPadre() {
    $('option', '#cbCategoriaPadre').remove();
    $('#cbCategoriaPadre').get(0).options.length = 0;
    $('#cbCategoriaPadre').get(0).options[0] = new Option("-NINGUNO-", "0");

    var idFamilia = $("#cbTipoEquipo").val();
    var idCtg = $("#hfCtgcodi").val();

    $.ajax({
        type: 'POST',
        url: controlador + 'ListaCategoriaByTipoEquipoExistentes',
        dataType: 'json',
        global: false,
        data: {
            idFamilia:idFamilia,
            idCtg: idCtg
        },
        cache: false,
        success: function (aData) {
            $.each(aData, function (i, item) {
                $('#cbCategoriaPadre').get(0).options[$('#cbCategoriaPadre').get(0).options.length] = new Option(item.Text, item.Value);
            });
        },
        error: function () {
            alert("Ha ocurrido un error.");
        }
    });
}

function eliminarCategoria(idCtg) {
    if (confirm('¿Está seguro que desea eliminar la categoría?')) {
        $.ajax({
            type: 'POST',
            url: controlador + 'EliminarCategoria',
            dataType: 'json',
            data: {
                idCategoria: idCtg
            },
            cache: false,
            success: function (resultado) {
                switch (resultado) {
                    case 1:
                        mostrarExitoOperacion();
                        mostrarListaCategoria();
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

function registrarCategoria() {
    if (confirm('¿Está seguro que desea guardar la categoría?')) {
        $.ajax({
            type: 'POST',
            url: controlador + 'RegistrarCategoria',
            dataType: 'json',
            data: $('#frmNewCategoria').serialize(),
            cache: false,
            success: function (resultado) {
                if (resultado == 1) {
                    mostrarExitoOperacion();
                    $('#popupNuevaCategoria').bPopup().close();
                    mostrarListaCategoria();
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

function actualizarCategoria() {
    if (confirm('¿Está seguro que desea actualizar la categoría?')) {
        $.ajax({
            type: 'POST',
            url: controlador + 'ActualizarCategoria',
            dataType: 'json',
            data: $('#frmEditCategoria').serialize(),
            cache: false,
            success: function (resultado) {
                if (resultado == 1) {
                    mostrarExitoOperacion();
                    $('#popupEditarCategoria').bPopup().close();
                    mostrarListaCategoria();
                } else
                    mostrarError(resultado);
            },
            error: function () {
                mostrarError();
            }
        });
    }
}

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

    $('#mensaje').removeClass("action-exito");
    $('#mensaje').addClass("action-error");
    $('#mensaje').text(mensaje);
    $('#mensaje').css("display", "block");
};