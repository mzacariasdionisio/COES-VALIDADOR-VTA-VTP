var controlador = siteRoot + 'IEOD/Parametro/';
var controladorHO = siteRoot + 'IEOD/HorasOperacion/';
var cant_edits = 0;
$(function () {
    $('#tab-container').easytabs();

    $('#btnBuscar').click(function () {
        buscarEquipos();
    });

    $('#btnRegresar').click(function () {
        document.location.href = controladorHO + "IndexCoordinador";
    });

    //Umbral
    $('#txtValorUmbral').mask("999.9");
    $('#btnGuardarUmbral').on("click", function () {
        guardarUmbral();
    });

    //pestaña color central
    mostrarListado(1);

    //pestaña Umbral
    obtenerUmbral();
});

//>>>>>>>>>>>>>>>>>>>>>>>> color central >>>>>>>>>>>>>>>>>>>
buscarEquipos = function () {
    $('#mensaje').css("display", "none");
    mostrarListado(1);
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
ocultarMensaje = function () {
    $('#mensaje').css("display", "none");
};
mostrarListado = function (nroPagina) {
    $('#hfNroPagina').val(nroPagina);
    var idEmpresa = $('#cbEmpresa').val();
    $.ajax({
        type: 'POST',
        url: controlador + "ListadoCentralesColor",
        data: {
            idEmpresa: idEmpresa
        },
        success: function (evt) {

            $('#listado').html(evt);
            $('#tabla').dataTable({
                "scrollY": 430,
                "scrollX": false,
                "sDom": 'ft',
                "ordering": false,
                "iDisplayLength": -1
            });
        },
        error: function () {
            mostrarError();
        }
    });
};

mostrarEditarColor = function (Equicodi) {
    // Buscamos el elemento div con el ID
    const elementoDiv = $('#' + Equicodi);

    if (elementoDiv) {
        // Verificamos si el elemento no tiene la clase "editar"
        if (!elementoDiv.hasClass('editar')) {
            // Si no tiene la clase, la agregamos
            elementoDiv.addClass('editar');

            $(".cnt_accion").addClass("editar");
            cant_edits++;
        }
    }
};
ocultarEditorColor = function (Equicodi) {
    // Buscamos el elemento div con el ID
    const elementoDiv = $('#' + Equicodi);

    if (elementoDiv) {
        // Verificamos si el elemento no tiene la clase "editar"
        if (elementoDiv.hasClass('editar')) {
            // Si no tiene la clase, la agregamos
            elementoDiv.removeClass('editar');

            cant_edits--;
            if (cant_edits == 0) {
                $(".cnt_accion").removeClass("editar");
            }
        }
    }
};
cancelarEditarColor = function (Equicodi, Hcolor) {
    // Buscamos el elemento div con el ID
    const elementoDiv = $('#' + Equicodi);
    const pickerColor = $('#CL' + Equicodi);

    if (elementoDiv) {
        // Verificamos si el elemento no tiene la clase "editar"
        if (elementoDiv.hasClass('editar')) {
            // Si no tiene la clase, la agregamos
            elementoDiv.removeClass('editar');

            cant_edits--;
            if (cant_edits == 0) {
                $(".cnt_accion").removeClass("editar");
            }
            if (pickerColor) {
                pickerColor.val(Hcolor);
            }
        }
    }
};
guardarEditarColor = function (Equicodi) {
    var colorSeleccionado = $("#CL" + Equicodi).val();

    //iniciamos el pop up
    $('#loading').bPopup({
        fadeSpeed: 'fast',
        opacity: 0.4,
        followSpeed: 500,
        modalColor: '#000000',
        modalClose: false,
        position: ['auto', 100] //x, y
    });

    $.ajax({
        type: 'POST',
        url: controlador + "GrabarPropiedadColor",
        dataType: 'json',
        data: {
            equicodi: Equicodi,
            color: colorSeleccionado
        },
        success: function (resultado) {
            if (resultado == 1) {
                ocultarEditorColor(Equicodi);
                mostrarExitoOperacion();
            } else {
                mostrarError();
            }
        },
        complete: function () {
            //ocultamos el popup
            $('#loading').bPopup().close();
        }
    });
}


//>>>>>>>>>>>>>>>>>>>>>>>> Umbral >>>>>>>>>>>>>>>>>>>
function obtenerUmbral() {
    $.ajax({
        type: 'POST',
        url: controlador + "ObtenerUmbral",
        dataType: 'json',
        success: function (evt) {
            if (evt.Resultado != -1) {

                $('#txtValorUmbral').val(evt.ValorUmbral)
                $("#txtUmbralUsuarioModif").text(evt.UsuarioModificacion);
                $("#txtUmbralFechaModif").text(evt.FechaModificacion);

            } else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function guardarUmbral() {

    var msj = validarUmbral();

    if (msj == "") {
        var valorPorcentaje = $('#txtValorUmbral').val();

        $.ajax({
            type: 'POST',
            url: controlador + "GuardarUmbral",
            dataType: 'json',
            data: {
                valor: valorPorcentaje
            },
            success: function (evt) {
                if (evt.Resultado != -1 ) {
                    alert("Se actualizó correctamente la configuración");

                    obtenerUmbral();
                } else {
                    alert("Ocurrió un error: " + evt.Mensaje);
                }
            },
            error: function (err) {
                alert("Ha ocurrido un error en guardar la configuración");
            }
        });
    }
    else {
        alert(msj);
    }
}

function validarUmbral() {
    var msj = "";

    var valorPorcentaje = $('#txtValorUmbral').val();

    //validar valor
    if ($('#txtValorUmbral').val().replace(/\s/g, '') === "") {
        $('#txtValorUmbral').focus();
        msj += "Debe ingresar valor numérico positivo" + "\n";
    }
    else {
        if (valorPorcentaje < 0) {
            $('#txtValorUmbral').focus();
            msj += "Debe ingresar valor numérico positivo" + "\n";
        }
    }

    return msj;
}