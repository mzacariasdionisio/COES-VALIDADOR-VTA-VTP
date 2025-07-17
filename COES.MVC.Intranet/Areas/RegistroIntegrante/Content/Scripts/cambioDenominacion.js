var controlador = siteRoot + 'RegistroIntegrante/Solicitud/'

$(function () {

    $("input[type=text]").keyup(function () {
        $("#" + this.id).val($("#" + this.id).val().toUpperCase());
    });

    $('#txtFechaProceso').Zebra_DatePicker({
    });

    if ($("#txtESTADO").val() == "APROBADO_FISICA") 
    {
        $('#divFechaProcesoTitulo').show();
        $('#divFechaProceso').show();
    }

    var estado = $("#txtESTADOINTERNO").val();
    if (estado == "") //NUEVO
    {
        //vacio
        
        $("#txtRUCCambio").val($('#hfRuc').val());
        $('#txtRUCCambio').prop("disabled", true);
        obtenerDatos();
    }
    else if (estado == "PENDIENTE" ) {

        $('#divEstado').show();
        $('#divFinalizar').show();

        $('#divDocumentoSustentatorio').show();
        $('#divDocumentoSustentatorioTitulo').show(); 

        $('#divSolicitar').hide();
        $('#divUpload').hide();

        $('#txtRUCCambio').prop("disabled", true);
        $('#txtEmprNombComercialCambio').prop("disabled", true);
        $('#txtEmprRazonSocialCambio').prop("disabled", true);
        $('#txtEmprSiglaCambio').prop("disabled", true);

        $('#spntxtRUCCambio').hide();
        $('#spntxtEmprNombComercialCambio').hide();
        $('#spntxtEmprRazonSocialCambio').hide();
        $('#spntxtEmprSiglaCambio').hide();

    }
    else if (estado == "APROBADO_FISICA" || estado == "APROBADO_DIGITAL") {

        $('#divEstado').show();

        $('#divDocumentoSustentatorio').show();
        $('#divDocumentoSustentatorioTitulo').show();
        
        $('#divSolicitar').hide();
        $('#divUpload').hide();

        $('#txtRUCCambio').prop("disabled", true);
        $('#txtEmprNombComercialCambio').prop("disabled", true);
        $('#txtEmprRazonSocialCambio').prop("disabled", true);
        $('#txtEmprSiglaCambio').prop("disabled", true);

        $('#spntxtRUCCambio').hide();
        $('#spntxtEmprNombComercialCambio').hide();
        $('#spntxtEmprRazonSocialCambio').hide();
        $('#spntxtEmprSiglaCambio').hide();

    }
    else if (estado == "DENEGADO") {

        $('#divEstado').show();

        $('#divObservacion').show();
        $('#divObservacionTitulo').show();

        $('#divDocumentoSustentatorio').show();
        $('#divDocumentoSustentatorioTitulo').show();
        
        $('#divSolicitar').hide();
        $('#divUpload').hide();

        $('#txtRUCCambio').prop("disabled", true);
        $('#txtEmprNombComercialCambio').prop("disabled", true);
        $('#txtEmprRazonSocialCambio').prop("disabled", true);
        $('#txtEmprSiglaCambio').prop("disabled", true);

        $('#spntxtRUCCambio').hide();
        $('#spntxtEmprNombComercialCambio').hide();
        $('#spntxtEmprRazonSocialCambio').hide();
        $('#spntxtEmprSiglaCambio').hide();
    }
    

    var solicitudencurso = $("#hdfSolicitudenCurso").val();
    if (solicitudencurso == "SI") {
        $('#mensaje').html("Ya existe una solicitud en estado PENDIENTE, no es posible iniciar una nueva.");
        $("#mensaje").show();
        deshabilitaControles();
    }

    $('#btnGrabar').click(function () {
        $.ajax({
            type: 'POST',
            url: controlador + "grabar",
            data: {
                solicodi: $('#hdfSolicodi').val(),
                fecha: $('#txtFechaProceso').val()
            },
            success: function (evt) {
                alert("Fecha Proceso Actualizada.");
            },
            error: function () {
                mostrarError();
            }
        });
    });

    $('#btnFinalizar').click(function () {
        $.ajax({
            type: 'POST',
            url: controlador + "finalizarDenominacion",
            data: {
                solicodi: $('#hdfSolicodi').val(),
                observacion: $('#txtObservacion').val(),
                emprcodi: $('#hdIdEmpresa').val(),
                razonsocial: $('#txtEmprRazonSocialCambio').val(),
                comercial: $('#txtEmprNombComercialCambio').val(),
                sigla: $('#txtEmprSiglaCambio').val()                
            },
            success: function (evt) {
                alert("Solicitud Finalizada.");
                deshabilitaControles();  

                var obs = $('#txtObservacion').val().length;
                if (obs > 0)
                    $("#txtESTADOINTERNO").val('DENEGADO');
                else
                    $("#txtESTADOINTERNO").val('APROBADO_DIGITAL');

            },
            error: function () {
                mostrarError();
            }
        });
    });
    
    $('#btnVer').click(function () {
        window.open(controlador + 'ver?url=' + $('#hdfAdjunto').val(), "_blank", 'fullscreen=yes');
    });

    $('#btnDescargar').click(function (e) {
        document.location.href = controlador + 'Download?url=' + $('#hdfAdjunto').val() + "&nombre=" + $('#hdfNombreArchivo').val();
    });

});

obtenerDatos = function () {

    var ruc = $('#txtRUCCambio').val();
    var isNumber = /^\d+$/.test(ruc);
    $('#txtEmprNombComercialCambio').val("");
    $('#txtEmprRazonSocialCambio').val("");
  

    if (ruc.length == 11 && isNumber) {
        $('#loadEmpresa').show();
        $.ajax({
            type: 'POST',
            url: controlador + 'obtenerdatos',
            data: {
                ruc: ruc
            },
            dataType: 'json',
            global: false,
            success: function (result) {
                if (result == -2) {
                    mostrarMensaje('El RUC ingresado no existe.');
                }
                else if (result == -1) {
                    mostrarError();
                }
                if (result == -3) {
                    mostrarMensaje('El RUC ingresado se encuentra de BAJA.');
                }
                else {
                    $('#txtEmprNombComercialCambio').val(result.NombreComercial);
                    $('#txtEmprRazonSocialCambio').val(result.RazonSocial);
                                       
                }
            },
            error: function () {
                mostrarMensaje('Error al consultar a SUNAT');
            }
        });
        $('#loadEmpresa').hide();
    }
    else {
        mostrarMensaje('El RUC debe contener 11 dígitos.');
    }
}

Validar = function () {
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

conforme = function () {
    $('#txtObservacion').val('');
    $('#txtObservacion').prop('disabled', true);
}

eliminar = function () {
    $('#txtObservacion').val('');
    $('#txtObservacion').prop('disabled', true);
}

agregarobservacion = function () {

    $('#txtObservacion').prop('disabled', false);
}

mostrarError = function () {
    $('#mensaje').removeClass();
    $('#mensaje').addClass('action-error');
    $('#mensaje').html("Ha ocurrido un error.");
}

mostrarMensaje = function (mensaje) {
    alert(mensaje);
}

deshabilitaControles = function () {
    $('#divFinalizar').hide();
    $('#divSolicitar').hide();
    $('#divUpload').hide();

    $('#txtRUCCambio').prop("disabled", true);
    $('#txtEmprNombComercialCambio').prop("disabled", true);
    $('#txtEmprRazonSocialCambio').prop("disabled", true);
    $('#txtEmprSiglaCambio').prop("disabled", true);
}


