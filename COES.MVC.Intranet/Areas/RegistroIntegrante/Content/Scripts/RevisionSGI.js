var controlador = siteRoot + 'RegistroIntegrante/Revision/'

$(function () {

    $("input[type=text]").keyup(function () {
        $("#" + this.id).val($("#" + this.id).val().toUpperCase());
    });

    var notificado = $("#hfRevinotificado").val();
    if (notificado == "S") {
        $('#mensaje').html("La solicitud se encuentra notificada.");
        $("#mensaje").show();
        deshabilitaControles();
    }

    var terminado = $("#hfReviterminado").val();
    if (terminado == "S") {
        $('#mensaje').html("La solicitud se encuentra aprobada.");
        $("#mensaje").show();
        deshabilitaControles();
    }

    var enviado = $("#hfRevienviado").val();
    if (enviado == "S") {
        $('#mensaje').html("La solicitud se encuentra aprobada fisicamente y enviada.");
        $("#mensaje").show();
        deshabilitaControles();
    }

    $('#btnFinalizar').click(function (e) {
        grabar('Finalizar');
    });

    $('#btnGrabar').click(function () {
        grabar('Grabar');
    });

    $('#btnRevAsistente').click(function (e) {
        e.preventDefault();
        $.ajax({
            type: 'POST',
            url: controlador + "RevisarAsistente",
            data: {
                revicodi: $('#hfRevicodi').val()
            },
            success: function (evt) {
                alert("Se cambio la revisión a no finalizado.");
                window.location = "../Revision/index"
                ///deshabilitaControles();
            },
            error: function () {
                mostrarError();
            }
        });
    });
});

grabar = function (estado) {


    // Validar Representantes validados 

    var table = document.getElementById("tblRevision");
    var nrows = table.rows.length;
    var row;
    var observacion = "";
    var adjunto = "";
    var adjuntofilename = "";
    var contError = 0;
    var contErrorObs = 0;
    var data = "";
    var valor = "";
    var campo = "";
    var id = 0;
    if (nrows > 1) {
        for (var i = 1; i < nrows; i++) {
            row = table.rows[i];
            //id = $(row).attr("data-id");
            adjunto = $(row.cells[1]).find("input").attr("data-file");
            adjuntofilename = $(row.cells[1]).find("input").attr("data-file-name");
            valor = $(row.cells[1]).find("input").val();
            campo = $(row.cells[1]).find("input").attr("data-name");
            if ($(row).hasClass("Conforme")) {
                data += campo + "*" + valor + "*" + "" + "*" + adjunto + "*" + adjuntofilename + "*Conforme";
                data += "|";
            } else if ($(row).hasClass("Observado")) {
                observacion = $(row.cells[2]).find("textarea").val();
                if (observacion == "") {
                    contErrorObs++;
                }
                data += campo + "*" + valor + "*" + observacion + "*" + adjunto + "*" + adjuntofilename + "*Observado";
                data += "|";
            }
            else {
                contError++;
            }
        }
    }
    data = data.substring(0, data.length - 1);
    if (contError > 0) {
        alert("Por favor validar todos los campos.");
        return;
    }
    if (contErrorObs > 0) {
        alert("Por favor ingrese las observaciones correspondientes");
        return;
    }




    var obj = {
        EmprCodi: $("#hfEmprcodi").val(),
        Revicodi: $("#hfRevicodi").val(),
        Revifinalizado: "",
        Data: data
    };

    var mensajefinal = "";

    if (estado == "Finalizar") {
        obj.Revifinalizado = "S";
        mensajefinal = "Se dio por finalizada la revisión.";
    } else {
        obj.Revifinalizado = "N";
        mensajefinal = "Se guardo la revisión.";
    }

    $.ajax({
        type: 'POST',
        url: controlador + "GrabarSGI",
        data: obj,
        success: function (evt) {
            if (evt) {
                alert(mensajefinal);
                window.location = "../Revision/index"
                //deshabilitaControles();
            } else {
                alert("Se ha producido un error");
            }
        },
        error: function () {
            mostrarError();
        }
    });

}

ver = function () {
    var adjunto = $('#hdfAdjunto_AD').val();
    window.open(controlador + 'ver?url=' + adjunto, "_blank", 'fullscreen=yes');
}

descargar = function () {
    var adjunto = $('#hdfAdjunto_AD').val();
    document.location.href = controlador + 'Download?url=' + adjunto;
}



conforme = function (obj) {
    $(obj).closest("tr").addClass("Conforme");
    $(obj).closest("td").find("textarea").val('');
    $(obj).closest("td").find("textarea").prop('disabled', true);
    $(obj).closest("td").find("textarea").removeClass("Habilitado");
    $(obj).closest("td").find("textarea").addClass("Deshabilitado");
}

eliminar = function (obj) {
    $(obj).closest("tr").removeClass("Conforme");
    $(obj).closest("td").find("textarea").val('');
    $(obj).closest("td").find("textarea").prop('disabled', true);    
    $(obj).closest("td").find("textarea").removeClass("Habilitado");
    $(obj).closest("td").find("textarea").addClass("Deshabilitado");
}

agregarobservacion = function (obj) {
    $(obj).closest("tr").removeClass("Conforme");
    $(obj).closest("tr").addClass("Observado");
    $(obj).closest("td").find("textarea").prop('disabled', false);
    $(obj).closest("td").find("textarea").removeClass("Deshabilitado");
    $(obj).closest("td").find("textarea").addClass("Habilitado");
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
    $('#form-main :input').prop("disabled", true);
    $('#btnGrabar').hide();
    $('#btnFinalizar').hide();
    $('#btnRevAsistente').hide();

    $('.control').hide();

}