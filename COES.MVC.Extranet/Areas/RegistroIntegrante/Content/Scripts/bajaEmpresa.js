var controlador = siteRoot + 'RegistroIntegrante/Solicitud/'

$(function () {

    $("input[type=text]").keyup(function () {
        $("#" + this.id).val($("#" + this.id).val().toUpperCase());
    });

    var estado = $("#txtESTADO").val();
    

    if (estado == "") //NUEVO
    {        
        $('#spnDescripcionCondicionBajaCode').hide();
        $('#spnDescripcionCondicionBaja').text($('#cbCondicionBaja').val());

        $('#divEstado').hide();        

        $('#divObservacion').hide();
        $('#divObservacionTitulo').hide();

        $('#divDocumentoSustentatorio').hide();
        $('#divDocumentoSustentatorioTitulo').hide();
    }
    else if (estado == "PENDIENTE" || estado == "APROBADO_FISICA" || estado == "APROBADO_DIGITAL") {
        
        $('#cbCondicionBaja').hide();
       
        $('#divObservacion').hide();
        $('#divObservacionTitulo').hide();

        $('#divSolicitar').hide();
        $('#divUpload').hide();

    } else if (estado == "DENEGADO")
    {
        $('#cbCondicionBaja').hide();
        $('#divSolicitar').hide();   
        $('#divUpload').hide();
    }

    var solicitudencurso = $("#hdfSolicitudenCurso").val();
    if (solicitudencurso == "SI") {
        $('#mensaje').html("Ya existe una solicitud en estado PENDIENTE, no es posible iniciar una nueva.");
        $("#mensaje").show();
        deshabilitaControles();
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

    $("#cbCondicionBaja").change(function () {
        $('#spnDescripcionCondicionBaja').text($('#cbCondicionBaja').val());
    });
    
    $('#btnVer').click(function () {
        window.open(controlador + 'ver?url=' + $('#hdfAdjunto').val(), "_blank", 'fullscreen=yes');
    });

    $('#btnDescargar').click(function (e) {
        document.location.href = controlador + 'Download?url=' + $('#hdfAdjunto').val() + "&nombre=" + $('#hdfNombreArchivo').val();
    });

});




mostrarError = function () {
    $('#mensaje').removeClass();
    $('#mensaje').addClass('action-error');
    $('#mensaje').html("Ha ocurrido un error.");
}

mostrarMensaje = function (mensaje) {
    alert(mensaje);
}

deshabilitaControles = function () {
    $('#divSolicitar').hide();
    $('#divUpload').hide();
    $('#cbCondicionBaja').prop("disabled", true);
}