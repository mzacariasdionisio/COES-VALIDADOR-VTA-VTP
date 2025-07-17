
var controlador = siteRoot + 'eventos/enviarcorreos/';
var uploader;

$(function () {
        

});

descargarArchivoFormato = function (file) {   
    var data = {
        filename: file
    };
    redireccionar(controlador, 'descargararchivo', data);   
}

redireccionar = function (controller, action, params) {
    redirect(controller + action, params);
}

//** Permite redireccionar a una pagína con parametros
redirect = function (url, params) {
    var $form = $("<form />");
    $form.attr("action", url);
    $form.attr("method", 'GET');
    for (var data in params)
        $form.append('<input type="hidden" name="' + data + '" value="' + params[data] + '" />');
    $("body").append($form);
    $form.submit();
}

quitarArchivoFormato = function () {
    $('#hfLinkArchivo').val('');
    $('#contenedorArchivoAdjunto').hide();
}

enviarCorreo = function (from, to, cc, bcc, asunto, contenido,plantcodi) {
   
    var contenido2 = $(contenido).html();
    
    $.ajax({
        type: 'POST',
        url: controlador + "EnviarCorreo",
        data: {
            from: from,
            to: to,
            cc: cc,
            bcc: bcc,
            asunto: asunto,
            contenido: contenido2,
            plantcodi: plantcodi
        },
        success: function(result) {
            if (result >= 0) {
                $('#popupEdicion').bPopup().close();
            }
            if (result == -1) {
                mostrarError();
            }

        },
        error: function() {
            alert('Ha ocurrido un error');
        }
    });
}


cargaDeArchivo = function() {
    
    //uploader = new plupload.Uploader({
    //    runtimes: 'html5,flash,silverlight,html4',
    //    browse_button: 'btnSelectFile',
    //    container: document.getElementById('container'),
    //    url: controlador + 'Upload',
    //    flash_swf_url: '~/Content/Scripts/Moxie.swf',
    //    silverlight_xap_url: '~/Content/Scripts/Moxie.xap',
    //    multi_selection: true,
    //    init: {
    //        PostInit: function() {
                
    //        },
    //        FilesAdded: function(up, files) {
    //            if (uploader.files.length == 2) {
    //                uploader.removeFile(uploader.files[0]);
    //            }
    //            plupload.each(files,
    //                function(file) {
    //                    loadInfoFile(file.name);
    //                    $('#hfArchivo').val("S");
    //                });
    //            up.refresh();
    //            uploader.settings.multipart_params = {
    //                "nombreArchivo": $('#fileInfo').text()
    //            }
    //            uploader.start();
    //        },
    //        UploadComplete: function(up, file) {
    //            $("#nombrearchivo").val($("#fileInfo").text());
    //        },
    //        Error: function(up, err) {
    //            loadValidacionFile(err.code + "-" + err.message);
    //        }
    //    }
    //});
    //uploader.init();

    uploader = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: 'btnSelectFile',
        container: document.getElementById('container'),
        url: controlador + 'Upload',
        flash_swf_url: '~/Content/Scripts/Moxie.swf',
        silverlight_xap_url: '~/Content/Scripts/Moxie.xap',
        multi_selection: true,
        chunk_size: '5mb',
        filters: {
            max_file_size: '15mb',
            mime_types: [
                { title: "Imagenes", extensions: "jpg,jpeg,gif,png" },
                { title: "Archivos comprimidos", extensions: "zip,rar" },
                { title: "Documentos", extensions: "pdf,doc,docx,xls,xlsx,ppt,pptx,csv" }
            ]
        },
        init: {
            PostInit: function () {
                $('#filelist').html('');
                $('#container').css('display', 'none');
            },
            FilesAdded: function (up, files) {
                $('#filelist').html('');
                $('#container').css('display', 'block');

                for (i = 0; i < uploader.files.length; i++) {
                    var file = uploader.files[i];

                    $('#filelist').html($('#filelist').html() + '<div class="file-name" id="' + file.id + '">' + file.name +
                            ' (' + plupload.formatSize(file.size) + ') <a class="remove-item" href="JavaScript:eliminarFile(' +
                            '\'' + file.id + '\');">X</a> <b></b></div>');

                    $('#fileList').val($('#fileList').val() + "/" + file.name);
                }
                uploader.start();
            },
            UploadProgress: function (up, file) {
                document.getElementById(file.id).getElementsByTagName('b')[0].innerHTML = '<span>' + file.percent + "%</span>";
                $('#' + file.id).css('background-color', '#FFFFB7');
            },
            UploadComplete: function (up, file) {
                $('#loadingcarga').css('display', 'none');               
                //finalizar();
            },
            Error: function (up, err) {
                $('#container').css('display', 'none');
                $('#filelist').html('<div class="action-alert">' + err.message + '</div>');
            }
        }
    });
    uploader.init();

    eliminarFile = function (id) {
        uploader.removeFile(id);
        $('#' + id).remove();
    }

    //return uploader;
}

function loadValidacionFile(mensaje) {
    $('#fileInfo').html(mensaje);
}

function loadInfoFile(fileName) {
    $('#fileInfo').html(fileName);
}



