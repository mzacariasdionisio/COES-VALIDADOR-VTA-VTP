var controler = siteRoot + "PrimasRER/FactorPerdidaDetalle/";

$(document).ready(function () {
    buscar();
    $('.txtFecha').Zebra_DatePicker({
        disabled: true
    });
    $("#popupEditFactorDetalle").addClass("general-popup");

    uploadExcel();
});

buscar = function () {
    console.log("IdFacPerMedDTO: ",$('#IdFacPerMedDTO').val())
    $.ajax({
        type: 'POST',
        url: controler + "lista",
        data: {id: $('#IdFacPerMedDTO').val()},
        success: function (evt) {
            $('#listado').html(evt);
            addDeleteEvent();
            viewEvent();
            oTable = $('#tabla').dataTable({
                "sPaginationType": "full_numbers",
                "destroy": "true",
                "aaSorting": [[0, "asc"], [1, "asc"], [2, "asc"]]
            });
        },
        error: function () {
            mostrarError("Lo sentimos, ha ocurrido un error");
        }
    });
};


//Funciones de eliminado de registro
function addDeleteEvent() {
    $(".delete").click(function (e) {
        e.preventDefault();
        if (confirm("¿Desea eliminar la información seleccionada?")) {
            id = $(this).attr("id").split("_")[1];
            $.ajax({
                type: "post",
                dataType: "text",
                url: controler + "Delete/" + id,
                data: addAntiForgeryToken({ id: id }),
                success: function (resultado) {
                    if (resultado == "true")
                        $("#fila_" + id).remove();
                    else
                        alert("No se ha logrado eliminar el registro");
                }
            });
        }
    });
};

addAntiForgeryToken = function (data) {
    data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
    return data;
};

function viewEvent() {

    $('.view').click(function () {
        id = $(this).attr("id").split("_")[1];
        abrirPopup(id);
    });
};

editar = function (Rerpprcodi) {
    console.log("Editar Factor de perdidas")
    $.ajax({
        type: 'POST',
        url: controler + "Edit",
        data: {
            Rerfpdcodi: Rerfpdcodi
        },
        success: function (evt) {
            $('#edit').html(evt);
            $('#mensaje').css("display", "none");
            setTimeout(function () {
                $('#popupEditFactorDetalle').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown'
                });
            }, 50);

        },
        error: function () {
            mostrarError("Lo sentimos, ha ocurrido un error");
        }
    });
};

update = function () {
    if (confirm('¿Está seguro que desea actualizar el factor de pérdida?')) {
        var Rerfpdfactperdida = $('#Rerfpdfactperdida').val();
        if (!isNaN(Rerfpdfactperdida)) {
            $.ajax({
                type: 'POST',
                url: controler + 'update',
                dataType: 'json',
                data: $('#frmEditFactorDetalle').serialize(),
                cache: false,
                success: function (resultado) {
                    if (resultado == 1) {
                        mostrarExito("La operación se realizó con exito...!");
                        $('#popupEditFactorDetalle').bPopup().close();
                        buscar();
                    } else if (resultado == -2) {
                        alert("El valor del factor de pérdida debe estar entre 0 y 2");
                        $('#Rerfpdfactperdida').focus();
                    } else
                        mostrarError("Lo sentimos, ha ocurrido un error");
                },
                error: function () {
                    mostrarError("Lo sentimos, ha ocurrido un error");
                }
            });
        } else {
            alert("El valor ingresado debe ser es un número entre 0 y 2");
            $('#Rerfpdfactperdida').focus();
        }
        
    }
};

mostrarExito = function (msg) {
    $('#mensaje').removeClass();
    $('#mensaje').html(msg);
    $('#mensaje').addClass("action-exito");
};
mostrarError = function (msg) {
    $('#mensaje').removeClass();
    $('#mensaje').html(msg);
    $('#mensaje').addClass("action-error");
};
mostrarAlerta = function (msg) {
    $('#mensaje').removeClass();
    $('#mensaje').html(msg);
    $('#mensaje').addClass("action-alert");
};
mostrarMensaje = function (msg) {
    $('#mensaje').removeClass();
    $('#mensaje').html(msg);
    $('#mensaje').addClass('action-message');
}

/* Paso 2: Procedimiento para la lectura de un archivos */
uploadExcel = function () {
    uploader = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: 'btnCargarArchivo',          /* SE ASIGNA EL BTN QUE EJECUTARA EL EVENTO */
        url: controler + 'uploadExcel',             /* Función en el controlador que ejecutara el evento  */
        flash_swf_url: 'Scripts/Moxie.swf',
        silverlight_xap_url: 'Scripts/Moxie.xap',
        multi_selection: false,
        filters: {
            max_file_size: '100mb',
            mime_types: [
                { title: "Archivos Excel .xlsx", extensions: "xlsx,xls" }
            ]
        },
        init: {
            FilesAdded: function (up, files) {
                if (uploader.files.length == 2) {
                    uploader.removeFile(uploader.files[0]);
                }
                uploader.start();
                up.refresh();
            },
            UploadProgress: function (up, file) {
                mostrarAlerta("Por favor espere ...(" + file.percent + "%)");
            },
            UploadComplete: function (up, file) {
                mostrarAlerta("Subida completada, procesando el archivo...");
                procesarArchivo(file[0].name);
            },
            Error: function (up, err) {
                mostrarError(err.code + "-" + err.message);
            }
        }
    });
    uploader.init();
}

procesarArchivo = function (sFile) {
    console.log(sFile);
    let fecDesde = $('#txtFecDesde').val()
    let fecHasta = $('#txtFecHasta').val()

    $.ajax({
        type: 'POST',
        url: controler + 'procesarArchivo',
        data: { sarchivo: sFile, fecDesde: fecDesde, fecHasta: fecHasta },
        dataType: 'json',
        cache: false,
        success: function (result) {
            console.log(result);
            if (result.RegError > 0) {
                mostrarError("Lo sentimos, " + result.RegError + " registro(s) no ha(n) sido leido(s) por presentar errores" + result.MensajeError);
            }
            else {
                $('#IdFacPerMedDTO').val(result.IdRegistro);
                buscar();
                mostrarExito(result.Mensaje);
            }
        },
        error: function () {
            mostrarError('Lo sentimos no se puede mostrar la hoja de cálculo')
        }
    });
}