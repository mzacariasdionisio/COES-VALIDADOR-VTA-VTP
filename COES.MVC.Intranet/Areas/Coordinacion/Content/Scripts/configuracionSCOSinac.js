var controlador = siteRoot + 'Coordinacion/ConfiguracionSCOSinac/';

$(function () {
    buscarEvento();
});

buscarEvento = function () {
    pintarPaginado();
    mostrarListado(1);
}

pintarPaginado = function () {
    $.ajax({
        type: 'POST',
        url: controlador + "paginado",
        success: function (evt) {
            $('#paginado').html(evt);
            mostrarPaginado();
        },
        error: function () {
            mostrarError();
        }
    });
}

mostrarListado = function (nroPagina) {
    $('#hfNroPagina').val(nroPagina);
    $.ajax({
        type: 'POST',
        url: controlador + "listado",
        data: {nroPagina: nroPagina
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

pintarBusqueda = function (nroPagina) {
    mostrarListado(nroPagina);
}

mostrarError = function () {
    $('#mensaje').removeClass();
    $('#mensaje').addClass('action-error');
    $('#mensaje').html("Ha ocurrido un error.");
}

mostrarMensaje = function (mensaje) {
    alert(mensaje);
}

Editar = function (fechadat, concepcodi, grupocodi, deleted, gruponomb, concepdesc) {
    $("#mensajeEdicion").hide();
    $.ajax({
        type: 'POST',
        data: {
            fechadat: fechadat,
            concepcodi: concepcodi,
            grupocodi: grupocodi,
            deleted: deleted,
            gruponomb: gruponomb,
            concepdesc: concepdesc
        },
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
                LimpiarPopUp();
            });
            $("#btnGrabarEdicion").click(function () {
                $("#mensajeEdicion").show();
                grabarEdicion();
            });
        }
    });
}

ValidarRequeridoEdicion = function () {
    var validator = true;
    var contError = 0;
    // Validar datos
    $(".DatosRequeridos").each(function () {
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

grabarEdicion = function () {

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

        // Datos

        formData.append("Formuladat", $("#txtFormuladatEdicion").val());

        formData.append("Fechadat", $("#hdffechadat").val());
        formData.append("Concepcodi", $("#hdfconcepcodi").val());
        formData.append("Grupocodi", $("#hdfgrupocodi").val());
        formData.append("Deleted", $("#hdfdeleted").val());
      
        $.ajax({
            type: 'POST',
            url: controlador + "GrabarEdicion",
            data: formData,
            dataType: 'html',
            contentType: false,
            processData: false,
            success: function (result) {
                if (result == 0) {                    
                    $('#mensajeEdicion').show();
                    $('#mensajeEdicion').removeClass();
                    $('#mensajeEdicion').html("Ingrese números separados por comas.");
                    $('#mensajeEdicion').addClass('action-alert');
                }else if (result == 1) {
                    buscarEvento();
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

validarEdicion = function () {
    var mensaje = "<ul>", flag = true;

    if ($('#txtFormuladatEdicion').val() == "") {
        mensaje = mensaje + "<li>Ingrese Código Scada</li>";
        flag = false;
    }
    
    mensaje = mensaje + "</ul>";
    if (flag) mensaje = "";

    return mensaje;
}

function mostrarErrorEdicion() {
    $('#mensaje').removeClass();
    $('#mensaje').html("Ha ocurrido un error, por favor intente nuevamente.");
    $('#mensaje').addClass('action-error');
}

LimpiarEditarPopUp = function () {
    $(".DatosRequeridosEdicion").each(function () {
        $(this).val("");
    });
}

LimpiarPopUp = function () {
    $(".DatosRequeridos").each(function () {
        $(this).val("");
    });
}

enabledFieldsEdicion = function () {
    $('#txtFormuladatEdicion').prop("disabled", false);
   
    $("#txtFormuladatEdicion").css("backgroundColor", "white")   

}

disabledFieldsEdicion = function () {

    $('#txtFormuladatEdicion').prop("disabled", true);
}