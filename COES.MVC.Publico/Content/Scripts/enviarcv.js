
var controlador = siteRoot + "OportunidadesTrabajo/";
var uploader;
$(function () {
    cargarConvocatoria();

    cargaDeArchivo();

    ValidarSize = function (obj) {
        if (getBrowserInfo().lastIndexOf("IE") == 0)
            return;
        var fileSize = obj.files[0].size;
        var sizekiloByte = parseInt(fileSize / 1024);
        var sizeMegaByte = parseInt(sizekiloByte / 1024);
        if (sizeMegaByte > 8) {
            obj.value = "";
            alert('Ingrese un archivo menor a 8mb');
        }

        extension = (obj.value.substring(obj.value.lastIndexOf("."))).toLowerCase();
        if (extension != '.pdf') {
            obj.value = "";
            alert('Ingrese un archivo en formato PDF');
        }

    }

    $("#btnEnviar").click(function () {
        if ($("#frmRegistro").valid()) {

            if ($('#hfArchivo').val() == "S") {

                if ($("#cbCondiciones").is(':checked') == true) {
                    Enviararchivo();
                }
                else {
                    $('#mensaje').removeClass();
                    $('#mensaje').addClass("action-alert");
                    $('#mensaje').text("Por favor acepte las condiciones de tratamiento de datos personales");
                }

            }
            else {
                $('#mensaje').removeClass();
                $('#mensaje').addClass("action-alert");
                $('#mensaje').text("Por favor adjunte su CV");
            }
        }
    });


    $("#frmRegistro").validate({
        rules: {
            NombresCompletos: { required: true, maxlength: 50 },
            Correo: { required: true, maxlength: 50, email: true },
            Estado: { required: true },
            NumeroDeIdentificacion: { required: true },
            Telefonocontacto: { maxlength: 50 },
            Apellidos: { required: true }
           },
        messages: {
            NombresCompletos: {
                required: "Ingrese nombre y apellidos",
                maxlength: "Máximo 50 caracteres"
            },
            Correo: {
                required: "Ingrese email",
                maxlength: "Máximo 50 caracteres",
                email: "Ingrese un correo válido"
            },
         
            NumeroDeIdentificacion: {
                required: "Ingrese Número de Identificación"
            },
            Telefonocontacto: {
                required: "Ingrese teléfono",
                maxlength:"Máximo 50 caracteres"
            },
            Apellidos: {
                required: "Ingrese Apellidos"
            },
           
        }
    });

    $("#Convocatoria").change(function () {
        $("#ConvocatoriaDesc").val($('#Convocatoria option:selected').html());
    });

});

cargarConvocatoria = function () {
    $.ajax({
        type: 'POST',
        url: controlador + 'CargarConvocatoria',
        success: function (result) {
            $("#Convocatoria").empty();
            $("#Convocatoria").append('<option value="">Seleccionar Convocatoria</option>');
            for (var i in result) {
                $("#Convocatoria").append('<option value="' + result[i].Convcodi + '">' + result[i].Convnomb + '</option>');
            }
        },
        error: function () {
            alert('Se ha producido un error.');
        }
    });
}

Enviararchivo = function () {   //enviar todos los datos del formulario al server
    $("#nombrearchivo").val($("#fileInfo").text());
    $.ajax({
        type: "post",
        url: controlador + "Enviar",
        data: $('#frmRegistro').serialize(),
        datatype: 'Json',           //estoy llmnd al servdor,una funcnin q retrna un tipo json
        success: function (result) {
            if (result == 1) {
                $('#mensaje').removeClass();
                $('#mensaje').addClass("action-exito");
                $('#mensaje').text("Su envío ha sido exitoso.");
                limpiar();
            } else {
                $('#mensaje').removeClass();
                $('#mensaje').addClass("action-error");
                $('#mensaje').text("Hubo un error al enviar los datos.");
            }
        }
    });
}

function limpiar() {

    $("#NumeroIdentificacion").val("");
    $("#NombresCompletos").val("");
    $("#Apellido").val("");
    $("#ciudad").val("Amazonas");
    $("#correo").val("");
    $("#correoa").val("");
    $("#telefonocontacto").val("");
    $("#descripcion").val("");
    $("#fileInfo").text("Ninguno archivo selec.");
    $("#nombrearchivo").val("");

    cargarConvocatoria();
    
}

openCondiciones = function () {

    $('#popupEdicion').bPopup({
        easing: 'easeOutBack',
        speed: 450,
        transition: 'slideDown',
        modalClose: false
    });
}

cargaDeArchivo = function () {
    uploader = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: 'btnSelectFile',
        container: document.getElementById('generalPupload'),
        url: controlador + 'Upload',
        flash_swf_url: '~/Content/Scripts/Moxie.swf',
        silverlight_xap_url: '~/Content/Scripts/Moxie.xap',
        multi_selection: false,
        init: {
           PostInit: function () {
                document.getElementById('btnSelectFile').onclick = function () {
                    if (uploader.files.length > 0) {
                        uploader.start();
                    }
                    else
                        loadValidacionFile("Ninguno archivo selec.");
                    return false;
                };
            },
            FilesAdded: function (up, files) {               
                if (uploader.files.length == 2) {
                    uploader.removeFile(uploader.files[0]);
                }
                plupload.each(files, function (file) {
                    loadInfoFile(file.name);
                    $('#hfArchivo').val("S");
                });
                up.refresh();
                uploader.settings.multipart_params = {
                    "nombreArchivo": $('#fileInfo').text()
                }
                uploader.start();
            },
            UploadComplete: function (up, file) {
                $("#nombrearchivo").val($("#fileInfo").text());
            },
            Error: function (up, err) {
                loadValidacionFile(err.code + "-" + err.message);
            }
        }
    });
    uploader.init();
}

function loadValidacionFile(mensaje) {
    $('#fileInfo').html(mensaje);
}

function loadInfoFile(fileName) {
    $('#fileInfo').html(fileName);
}
