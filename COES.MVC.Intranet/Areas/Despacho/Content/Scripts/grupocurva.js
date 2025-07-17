var controlador = siteRoot + 'despacho/grupocurva/';

$(function () {

    cargarGrupoCurva();

    $('#btnAgregar').click(function () {
        Agregar();
    });  


});

cargarGrupoCurva = function () {
    $.ajax({
        type: 'POST',
        url: controlador + 'listado',
        success: function (evt) {
            $('#listadoGrupoCurva').html(evt);
        },
        error: function () {
            mostrarError();
        }
    });
}

Agregar = function () {
    $("#mensaje").hide();
    enabledFields();
    setTimeout(function () {
        $('#popupAgregar').bPopup({
            autoClose: false,
        });
    }, 50);
    $('#btnCancelarNuevo').click(function () {
        $('#popupAgregar').bPopup().close();
        LimpiarPopUp();
    });

    $("#btnGrabarNuevo").click(function () {

        if (!ValidarRequerido()) {
            $("#mensaje").show();
            return false;
        } else
            $("#mensaje").hide();

        var validacion = validar();
        if (validacion == "") {
            $('#mensaje').removeClass();
            $('#mensaje').html("Todos los campos estan correctos");
            $('#mensaje').addClass('action-exito');

            var formData = new FormData();

            formData.append("Nombres", $("#txtNombres").val());

            $.ajax({
                type: 'POST',
                url: controlador + "GrabarNuevo",
                data: formData,
                dataType: 'html',
                contentType: false,
                processData: false,
                global:false,
                success: function (result) {
                    if (result == 1) {
                        cargarGrupoCurva();
                        $('#mensaje').removeClass();
                        $('#mensaje').html("Los datos han sido guardados correctamente");
                        $('#mensaje').addClass('action-exito');
                        disabledFields();
                        setTimeout(function () {
                            $('#popupAgregar').bPopup().close();
                            LimpiarPopUp();
                        }, 50);
                    } else {
                        mostrarError();
                    }
                },
                error: function () {
                    mostrarError();
                }
            });
        } else {
            $("#mensaje").show();
            $('#mensaje').removeClass();
            $('#mensaje').html(validacion);
            $('#mensaje').addClass('action-alert');
        }
    });
}

Editar = function (CurvCodi) {
    

    $("#mensajeEdicion").hide();
    $.ajax({
        type: 'POST',
        data: {
            CurvCodi: CurvCodi
        },
        global: false,
        url: controlador + 'edicion',
        success: function (evt) {
            $('#contenidoEdicion').html(evt);
            setTimeout(function () {
                $('#popupEdicion').bPopup({
                    autoClose: false,
                });
            }, 50);
            disabledFieldsEdicion();
            enabledFieldsEdicion();
            $('#btnCancelarEdicion').click(function () {
                $('#popupEdicion').bPopup().close();
                LimpiarEditarPopUp();
            });
            $("#btnGrabarEdicion").click(function () {
                $("#mensajeEdicion").show();
                grabarEdicion(CurvCodi);
            });
        }
    });
}

grabarEdicion = function (CurvCodi) {

  

    if (!ValidarRequeridoEdicion()) {
        $("#mensajeEdicion").show();
        return false;
    } else
        $("#mensajeEdicion").hide();

    var validacion = validarEdicion();
    if (validacion == "") {
        $('#mensajeEdicion').removeClass();
        $('#mensajeEdicion').html("Todos los campos estan correctos");
        $('#mensajeEdicion').addClass('action-exito');


        var formData = new FormData();

        // Datos de edicion

        formData.append("Codigo", CurvCodi);
        formData.append("Nombres", $("#txtNombresEdicion").val());

        $.ajax({
            type: 'POST',
            url: controlador + "GrabarEdicion",
            data: formData,
            dataType: 'html',
            contentType: false,
            processData: false,
            global: false,
            success: function (result) {
                if (result == 1) {
                    cargarGrupoCurva();
                    $('#mensajeEdicion').removeClass();
                    $('#mensajeEdicion').html("Los datos han sido guardados correctamente");
                    $('#mensajeEdicion').addClass('action-exito');
                    disabledFieldsEdicion();
                    setTimeout(function () {
                        $('#popupEdicion').bPopup().close();
                        LimpiarEditarPopUp();
                    }, 50);
                } else {
                    mostrarErrorEdicion();
                }
            },
            error: function () {
                mostrarErrorEdicion();
            }
        });
    } else {
        $('#mensajeEdicion').show();
        $('#mensajeEdicion').removeClass();
        $('#mensajeEdicion').html(validacion);
        $('#mensajeEdicion').addClass('action-alert');
    }
}


Eliminar = function (CurvCodi) {

  
    mensajeOperacion("Este seguro de eliminar el grupo curva seleccionado?", null
        , {
            showCancel: true,
            onOk: function () {

                $.ajax({
                    type: 'POST',
                    url: controlador + 'eliminar',
                    data: {
                        CurvCodi: CurvCodi
                    },
                    global: false,
                    dataType: 'json',
                    success: function (result) {
                        if (result == 1) {
                
                            cargarGrupoCurva();
                        } else {
                            
                            mostrarError();
                        }
                    },
                    error: function () {
                      
                        mostrarError();
                    }
                });


            },
            onCancel: function () {
          
            }
        });

 
}


Detalle = function (CurvCodi) {


    $("#mensajeDetalle").hide();
    $.ajax({
        type: 'POST',
        data: {
            CurvCodi: CurvCodi
        },
        url: controlador + 'detalle',
        global: false,
        success: function (evt) {

            $('#contenidoDetalle').html(evt);          

            cargarDetalleGrupoCurva(CurvCodi);

            $('#btnAgregarDetalle').click(function () {
                AgregarDetalle();
            });  
            

            setTimeout(function () {
                $('#popupDetalle').bPopup({
                    autoClose: false,
                });
            }, 50);
            $('#btnCerrar').click(function () {
                $('#popupDetalle').bPopup().close();
            });
        }
    });
}


cargarDetalleGrupoCurva = function (CurvCodi) {
    $.ajax({
        type: 'POST',
        data: {
            CurvCodi: CurvCodi
        },
        url: controlador + 'listadoDetalle',
        global: false,
        success: function (evt) {
            $('#listadoDetalleGrupoCurva').html(evt);
        },
        error: function () {
            mostrarError();
        }
    });
}

AgregarDetalle = function () {

    var validacion = validarDetalle();
    if (validacion == "") {
        $('#mensajeDetalle').removeClass();
        $('#mensajeDetalle').html("Todos los campos estan correctos");
        $('#mensajeDetalle').addClass('action-exito');


        var formData = new FormData();

        // Datos de detalle

        formData.append("codigoCurva", $("#hdfCodigo").val());
        formData.append("codigoGrupo", $("#cbGrupoB").val());
        formData.append("grupoPrincipal", $("#cbAgrupado").val());

        $.ajax({
            type: 'POST',
            url: controlador + "GrabarDetalle",
            data: formData,
            dataType: 'html',
            contentType: false,
            processData: false,
            global: false,
            success: function (result) {
                if (result == 1) {
                    
                    cargarDetalleGrupoCurva($("#hdfCodigo").val());

                    $('#mensajeDetalle').removeClass();
                    $('#mensajeDetalle').html("Los datos han sido guardados correctamente");
                    $('#mensajeDetalle').addClass('action-exito');
                   
                    setTimeout(function () {
                      
                        
                    }, 50);
                } else {
                    mostrarErrorDetalle();
                }
            },
            error: function () {
                mostrarErrorDetalle();
            }
        });
    } else {
        $('#mensajeDetalle').show();
        $('#mensajeDetalle').removeClass();
        $('#mensajeDetalle').html(validacion);
        $('#mensajeDetalle').addClass('action-alert');
    }
}


EliminarDetalle = function (CurvCodi, GrupoCodi) {


    mensajeOperacion("Este seguro de eliminar elemento seleccionado?", null
        , {
            showCancel: true,
            onOk: function () {

                $.ajax({
                    type: 'POST',
                    url: controlador + 'eliminarDetalle',
                    data: {
                        CurvCodi: CurvCodi,
                        GrupoCodi: GrupoCodi
                    },
                    global: false,
                    dataType: 'json',
                    success: function (result) {
                        if (result == 1) {

                            cargarDetalleGrupoCurva(CurvCodi);
                        } else {

                            mostrarError();
                        }
                    },
                    error: function () {

                        mostrarError();
                    }
                });


            },
            onCancel: function () {

            }
        });


}


validar = function () {
    var mensaje = "<ul>", flag = true;

    if ($('#txtNombres').val() == "") {
        mensaje = mensaje + "<li>Ingrese Nombres</li>";
        flag = false;
    }    

    mensaje = mensaje + "</ul>";
    if (flag) mensaje = "";

    return mensaje;
}

validarEdicion = function () {
    var mensaje = "<ul>", flag = true;

    if ($('#txtNombresEdicion').val() == "") {
        mensaje = mensaje + "<li>Ingrese Nombres</li>";
        flag = false;
    }    

    mensaje = mensaje + "</ul>";
    if (flag) mensaje = "";

    return mensaje;
}

validarDetalle = function () {
    var mensaje = "<ul>", flag = true;

    if ($('#cbGrupoB').val() == "" || $('#cbGrupoB').val() == null) {
        mensaje = mensaje + "<li>Seleccione un elemento de las lista</li>";
        flag = false;
    }

    mensaje = mensaje + "</ul>";
    if (flag) mensaje = "";

    return mensaje;
}

ValidarRequerido = function () {
    var validator = true;
    var contError = 0;
    // Validar datos
    $(".DatosRequeridos").each(function () {
        if ($(this).hasClass("Required")) {
            var spn = $(this).attr("data-validator-id");
            if ($(this).val() == "" || $(this).val() == "0" || $(this).val() == null) {
                $("#" + spn).addClass("Error");
                contError++;
            } else {
                $("#" + spn).removeClass("Error");
            }
        }
    });

    if (contError > 0) {
        validator = false;
    }
    return validator;
}

ValidarRequeridoEdicion = function () {
    var validator = true;
    var contError = 0;
    // Validar datos
    $(".DatosRequeridosEdicion").each(function () {
        if ($(this).hasClass("RequiredEdicion")) {
            var spn = $(this).attr("data-validator-id");
            if ($(this).val() == "" || $(this).val() == "0" || $(this).val() == null) {
                $("#" + spn).addClass("Error");
                contError++;
            } else {
                $("#" + spn).removeClass("Error");
            }
        }
    });

    if (contError > 0) {
        validator = false;
    }
    return validator;
}

LimpiarPopUp = function () {
    $(".DatosRequeridos").each(function () {
        $(this).val("");
    });
}

LimpiarEditarPopUp = function () {
    $(".DatosRequeridosEdicion").each(function () {
        $(this).val("");
    });
}

enabledFields = function () {
    $('#txtNombres').prop("disabled", false);
}

disabledFields = function () {

    $('#txtNombres').prop("disabled", true);
}

enabledFieldsEdicion = function () {
    $('#txtNombresEdicion').prop("disabled", false);
    $("#txtNombresEdicion").css("backgroundColor", "white")
}

disabledFieldsEdicion = function () {

    $('#txtNombresEdicion').prop("disabled", true);
}

mostrarError = function () {
    alert('Error');
}

mostrarMensaje = function (mensaje) {
    alert(mensaje);
}

function mostrarErrorEdicion() {
    $('#mensajeEdicion').removeClass();
    $('#mensajeEdicion').html("Ha ocurrido un error, por favor intente nuevamente.");
    $('#mensajeEdicion').addClass('action-error');
}

function mostrarErrorDetalle() {
    $('#mensajeDetalle').removeClass();
    $('#mensajeDetalle').html("Ha ocurrido un error, por favor intente nuevamente.");
    $('#mensajeDetalle').addClass('action-error');
}