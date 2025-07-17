var controlador = siteRoot + 'Equipamiento/Categoria/';
var idFormIndex = "frmBusquedaClasificacion";
var idFormNew = "frmNewCategoriaEquipo";
var idFormEdit = "frmEditCategoriaEquipo";
var tipoComboSeleccione = 1;
var tipoComboTodos = 2;

$(function () {
    //inicializar 
    $('#cbTipoEmpresa').val(-2);
    $('#cbEmpresa').val(-2);
    $('#cbTipoEquipo').val(-2);
    $('#cbEstado').val(' ');
    cargarEmpresas(idFormIndex, tipoComboTodos);
    buscarClasificaciones();

    //eventos
    $('#btnBuscar').click(function () {
        $("#mensaje").hide();
        buscarClasificaciones();
    });
    $('#btnNuevo').click(function () {
        $("#mensaje").hide();
        nuevaClasificacion();
    });
    $('#' + idFormIndex + ' #cbTipoEmpresa').change(function () {
        $("#mensaje").hide();
        cargarEmpresas(idFormIndex, tipoComboTodos);
    });
    $('#' + idFormIndex + ' #cbEmpresa').change(function () {
        $("#mensaje").hide();
        cargarEquipo(idFormIndex, tipoComboTodos);
    });
    $('#' + idFormIndex + ' #cbTipoEquipo').change(function () {
        $("#mensaje").hide();
        cargarCategoria(idFormIndex, tipoComboTodos);
        cargarEquipo(idFormIndex, tipoComboTodos);
    });
    $('#' + idFormIndex + ' #cbCategoria').change(function () {
        $("#mensaje").hide();
        cargarSubclasificacion(idFormIndex, tipoComboTodos);
    });

    $(window).resize(function () {
        $('#listado').css("width", $('#mainLayout').width() + "px");
    });
});

cargarEmpresas = function (idForm, tipoCombo) {
    $('#' + idForm + ' #cbEmpresa').get(0).options.length = 0;
    var opcion = "-TODOS-";
    if (tipoCombo == tipoComboSeleccione) {
        opcion = "-SELECCIONE-";
    }
    $('#' + idForm + ' #cbEmpresa').get(0).options[0] = new Option(opcion, "-2");

    $.ajax({
        type: 'POST',
        url: controlador + '/CargarEmpresas',
        dataType: 'json',
        data: { idTipoEmpresa: $('#' + idForm + ' #cbTipoEmpresa').val() },
        cache: false,
        success: function (aData) {
            $.each(aData, function (i, item) {
                $('#' + idForm + ' #cbEmpresa').get(0).options[$('#' + idForm + ' #cbEmpresa').get(0).options.length] = new Option(item.Text, item.Value);
            });
        },
        error: function () {
            mostrarError();
        }
    });
};

cargarEquipo = function (idForm, tipoCombo) {
    $('#' + idForm + ' #cbEquipo').get(0).options.length = 0;
    var opcion = "-TODOS-";
    if (tipoCombo == tipoComboSeleccione) {
        opcion = "-SELECCIONE-";
    }
    $('#' + idForm + ' #cbEquipo').get(0).options[0] = new Option(opcion, "-2");

    $.ajax({
        type: 'POST',
        url: controlador + '/ListaEquipoByEmpresaAndFamilia',
        dataType: 'json',
        data: {
            idEmpresa: $('#' + idForm + ' #cbEmpresa').val(),
            idFamilia: $('#' + idForm + ' #cbTipoEquipo').val(),
        },
        cache: false,
        success: function (aData) {
            $.each(aData, function (i, item) {
                $('#' + idForm + ' #cbEquipo').get(0).options[$('#' + idForm + ' #cbEquipo').get(0).options.length] = new Option(item.Text, item.Value);
            });
        },
        error: function () {
            mostrarError();
        }
    });
};

cargarCategoria = function (idForm, tipoCombo) {
    //inicializar combo categoria
    $('#' + idForm + ' #cbCategoria').get(0).options.length = 0;
    var opcion = "-TODOS-";
    if (tipoCombo == tipoComboSeleccione) {
        opcion = "-SELECCIONE-";
    }
    $('#' + idForm + ' #cbCategoria').get(0).options[0] = new Option(opcion, "-3");

    //inicializar combo tipo equipo
    $('#' + idForm + ' #cbSubclasificacion').get(0).options.length = 0;
    var opcion = "-TODOS-";
    if (tipoCombo == tipoComboSeleccione) {
        opcion = "-SELECCIONE-";
    }
    $('#' + idForm + ' #cbSubclasificacion').get(0).options[0] = new Option(opcion, "-3");

    $.ajax({
        type: 'POST',
        url: controlador + '/ListaCategoriaByTipoEquipo',
        dataType: 'json',
        data: {
            idFamilia: $('#' + idForm + ' #cbTipoEquipo').val()
        },
        cache: false,
        success: function (aData) {
            $.each(aData, function (i, item) {
                $('#' + idForm + ' #cbCategoria').get(0).options[$('#' + idForm + ' #cbCategoria').get(0).options.length] = new Option(item.Text, item.Value);
            });
        },
        error: function () {
            mostrarError();
        }
    });
};

cargarSubclasificacion = function (idForm, tipoCombo) {
    $('#' + idForm + ' #cbSubclasificacion').get(0).options.length = 0;
    var opcion = "-TODOS-";
    if (tipoCombo == tipoComboSeleccione) {
        opcion = "-SELECCIONE-";
    }
    $('#' + idForm + ' #cbSubclasificacion').get(0).options[0] = new Option(opcion, "-3");

    $.ajax({
        type: 'POST',
        url: controlador + '/ListaSubclasificacion',
        dataType: 'json',
        data: {
            idCtg: $('#' + idForm + ' #cbCategoria').val()
        },
        cache: false,
        success: function (aData) {
            if (aData.length == 1) {
                $('#' + idForm + ' #cbSubclasificacion').get(0).options.length = 0;
            }
            $.each(aData, function (i, item) {
                $('#' + idForm + ' #cbSubclasificacion').get(0).options[$('#' + idForm + ' #cbSubclasificacion').get(0).options.length] = new Option(item.Text, item.Value);
            });
        },
        error: function () {
            mostrarError();
        }
    });
};

buscarClasificaciones = function () {
    $('#mensaje').css("display", "none");
    pintarPaginado();
    mostrarListado(1);
};

pintarPaginado = function () {
    $.ajax({
        type: 'POST',
        url: controlador + "PaginadoClasificacion",
        data: $('#frmBusquedaClasificacion').serialize(),
        success: function (evt) {
            $('#paginado').html(evt);
            mostrarPaginado();
        },
        error: function () {
            mostrarError();
        }
    });
};
pintarBusqueda = function (nroPagina) {
    mostrarListado(nroPagina);
};

mostrarListado = function (nroPagina) {
    $('#hfNroPagina').val(nroPagina);
    $.ajax({
        type: 'POST',
        url: controlador + "ListaClasificacion",
        data: $('#frmBusquedaClasificacion').serialize(),
        success: function (evt) {
            $('#listado').parent().css("width", ($('#mainLayout').width() - 60) + "px");
            $('#listado').css("width", ($('#mainLayout').width() - 40) + "px");

            $('#listado').html(evt);
            $('#tabla').dataTable({
                "scrollY": 480,
                "scrollX": true,
                "sDom": 'ft',
                "ordering": false,
                "iDisplayLength": 50
            });
        },
        error: function () {
            mostrarError();
        }
    });
};

mostrarClasificacion = function (equicodi, Ctgdetcodi) {
    $("#mensaje").hide();

    $.ajax({
        type: 'POST',
        url: controlador + "VerClasificacionEquipo",
        data: {
            idCategoriaDet: Ctgdetcodi,
            idEquipo: equicodi
        },
        success: function (evt) {
            $('#verClasificacion').html(evt);
            $('#mensaje').css("display", "none");

            setTimeout(function () {
                $('#popupVerClasificacion').bPopup({
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

editarClasificacion = function (equicodi, Ctgdetcodi) {
    $("#mensaje").hide();

    $.ajax({
        type: 'POST',
        url: controlador + "EditarClasificacionEquipo",
        data: {
            idCtgdet: Ctgdetcodi,
            idEquipo: equicodi
        },
        success: function (evt) {
            $('#editarClasificacion').html(evt);
            $('#mensaje').css("display", "none");

            setTimeout(function () {
                $('#popupEditarClasificacion').bPopup({
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

actualizarClasificacion = function () {
    if (esValidoFormularioEditar(idFormEdit)) {
        if (confirm('¿Está seguro que desea actualizar la clasificación del equipo?')) {
            $.ajax({
                type: 'POST',
                url: controlador + 'ActualizarCategoriaEquipo',
                dataType: 'json',
                data: $('#frmEditCategoriaEquipo').serialize(),
                cache: false,
                success: function (resultado) {
                    if (resultado == 1) {
                        mostrarExitoOperacion();
                        $('#popupEditarClasificacion').bPopup().close();
                        buscarClasificaciones();
                    } else
                        mostrarError(resultado);
                },
                error: function () {
                    mostrarError();
                }
            });
        }
    }
}

function esValidoFormularioEditar(idForm) {
    var msj = "";
    var idCategoria = $('#' + idForm + ' #cbCategoria').val();
    var idSubclasificacion = $('#' + idForm + ' #cbSubclasificacion').val();

    if (idCategoria == undefined || idCategoria == "-3") {
        msj += "Seleccione categoria" + "\n";
    }
    if (idSubclasificacion == undefined || idSubclasificacion == "-3") {
        msj += "Seleccione subcategoria" + "\n";
    }

    if (msj != "") {
        alert(msj);
        return false;
    }

    return true;
}

function nuevaClasificacion() {
    var htmlPopup = $("#nuevaClasificacion").html();
    if (htmlPopup == "") {
        $.ajax({
            type: 'POST',
            url: controlador + "NuevoClasificacionEquipo",
            success: function (evt) {
                $('#nuevaClasificacion').html(evt);
                $('#mensaje').css("display", "none");

                setTimeout(function () {
                    $('#popupNuevaClasificacion').bPopup({
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
    } else {
        setTimeout(function () {
            $('#popupNuevaClasificacion').bPopup({
                easing: 'easeOutBack',
                speed: 450,
                transition: 'slideDown'
            });
        }, 50);
    }
}

function registrarClasificacion() {
    if (esValidoFormulario(idFormNew)) {
        if (confirm('¿Está seguro que desea guardar el clasificación de equipo?')) {
            $.ajax({
                type: 'POST',
                url: controlador + 'RegistrarCategoriaEquipo',
                dataType: 'json',
                data: $('#' + idFormNew).serialize(),
                cache: false,
                success: function (resultado) {
                    if (resultado == 1) {
                        mostrarExitoOperacion();
                        //$('#popupNuevaClasificacion').bPopup().close();
                        buscarClasificaciones();
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
}

function esValidoFormulario(idForm) {
    var msj = "";
    var idEmpresa = $('#' + idForm + ' #cbEmpresa').val();
    var idTipoEquipo = $('#' + idForm + ' #cbTipoEquipo').val();
    var idEquipo = $('#' + idForm + ' #cbEquipo').val();
    var idCategoria = $('#' + idForm + ' #cbCategoria').val();
    var idSubclasificacion = $('#' + idForm + ' #cbSubclasificacion').val();

    if (idEmpresa == undefined || idEmpresa == "-2") {
        msj += "Seleccione empresa" + "\n";
    }
    if (idTipoEquipo == undefined || idTipoEquipo == "-2") {
        msj += "Seleccione Tipo de equipo" + "\n";
    }
    if (idEquipo == undefined || idEquipo == "-2") {
        msj += "Seleccione equipo" + "\n";
    }
    if (idCategoria == undefined || idCategoria == "-3") {
        msj += "Seleccione categoria" + "\n";
    }
    if (idSubclasificacion == undefined || idSubclasificacion == "-3") {
        msj += "Seleccione subcategoria" + "\n";
    }

    if (msj != "") {
        alert(msj);
        return false;
    }

    return true;
}

mostrarExitoOperacion = function () {
    $('#mensaje').removeClass("action-error");
    $('#mensaje').addClass("action-exito");
    $('#mensaje').text("La operación se realizó con exito...!");
    $('#mensaje').css("display", "block");
};

mostrarError = function (msj) {
    var mensaje = msj == undefined || msj == null ? "Ha ocurrido un error" : msj;
    $('#mensaje').removeClass("action-exito");
    $('#mensaje').addClass("action-error");
    $('#mensaje').text(mensaje);
    $('#mensaje').css("display", "block");
};