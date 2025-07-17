var controlador = siteRoot + 'filemanager/browser/'

$(function () {

    $('#btn-downloadfile').click(function () {
        var urls = validarSeleccion();
        if (urls.length >= 1) {
            downloadBlobs(urls);
        }
        else {
            mostrarMensajeSeleccionUnico();
        }
    });

    $('#btn-downloadzip').click(function () {
        var urls = validarSeleccion();
        if (urls.length >= 1) {
            downloadAsZip(urls);
        }
        else {
            mostrarMensajeSeleccion();
        }
    });


    browser();
    //crearPupload();

    $('#btn-saveData').click(function () {
        remitirTodo();
    });
});

openDirectory = function (url) {
    $('#hfRelativeDirectory').val(url);
    browser();
}

browser = function () {
    var url = $('#hfRelativeDirectory').val();
    var bas = $('#hfBaseDirectory').val();

    $.ajax({
        type: 'POST',
        url: controlador + 'folder',
        data: {
            baseDirectory: $('#hfBaseDirectory').val(),
            url: url,
            pathAlternativo: $('#hfOrigen').val()
        },
        success: function (evt) {
            $('#browserDocument').html(evt);

            $.contextMenu({
                selector: '.selector-file-contextual',
                callback: function (key, options) {
                    var urlFile = $(this).attr('id');
                    var urls = [];
                    urls.push(urlFile);
                    if (key == "downloadblob") {
                        downloadBlob(urlFile);
                    }
                },
                items: {
                    "downloadblob": { name: "Descargar", icon: "downloadblob" }
                }
            });

            $.contextMenu({
                selector: '.selector-directory-contextual',
                callback: function (key, options) {
                    var urlFolder = $(this).attr('id')
                    var urlsFolder = [];
                    urlsFolder.push(urlFolder);

                    if (key == "opendirectory") {
                        openDirectory(urlFolder);
                    }
                },
                items: {
                    "opendirectory": { name: "Abrir", icon: "opendirectory" }
                }
            });
        },
        error: function () {
            mostrarError();
        }
    });
}

validarSeleccion = function () {
    var urls = [];
    $('#tbDocumentLibrary tbody tr').each(function (i, row) {
        var $actualrow = $(row);
        $checkbox = $actualrow.find('input:checked');
        if ($checkbox.is(':checked')) {
            urls.push($checkbox.val());
        }
    });
    return urls;
}

openBlob = function (url, type, extension) {
    if (type == "F") {
        downloadBlob(url);
    }
    if (type == "D") {
        $('#hfRelativeDirectory').val(url);
        browser();
    }
}

downloadBlob = function (url) {
    document.location.href = controlador + "download?url=" + url + "&pathAlternativo=" + $('#hfOrigen').val();
}

downloadBlobs = function (urls) {
    var contenedor = $('#multiDownload');
    for (i in urls) {
        contenedor.append('<a href="' + controlador + "download?url=" + urls[i] + '" class="file-download"></a>');
    }
    $('.file-download').multiDownload();
}

downloadAsZip = function (urls) {
    $.ajax({
        type: 'POST',
        url: controlador + "generarzip",
        dataType: 'json',
        traditional: true,
        data: {
            urls: urls,
            pathAlternativo: $('#hfOrigen').val()
        },
        success: function (result) {
            if (result == 1) {
                window.location = controlador + 'downloadzip';
            }
            else {
                mostrarError();
            }
        },
        error: function () {
            mostrarError()
        }
    });
}

mostrarError = function () {
    alert("Ha ocurrido un error.");
}

mostrarMensajeSeleccionUnico = function () {
    alert("Seleccion un archivo.");
}

mostrarMensajeSeleccion = function () {
    alert("Seleccione al menos un archivo.");
}

mostrarAlerta = function (mensaje) {
    alert(mensaje);
}

//function crearPupload() {
//    var msjOpc = "", msjCarga = "";
//    var uploader = new plupload.Uploader({
//        runtimes: 'html5,flash,silverlight,html4',
//        browse_button: "btn-saveData",
//        url: controlador + 'Upload',
//        flash_swf_url: 'Scripts/Moxie.swf',
//        silverlight_xap_url: 'Scripts/Moxie.xap',
//        multi_selection: false,
//        filters: {
//            max_file_size: 0,
//            mime_types: [
//                { title: "Archivos XML .zip", extensions: "zip" },
//            ]
//        },
//        init: {
//            FilesAdded: function (up, files) {
//                if (uploader.files.length == 2) {
//                    uploader.removeFile(uploader.files[0]);
//                }
//                uploader.start();
//                up.refresh();
//            },
//            UploadProgress: function (up, file) {
//                mAlerta("Por favor espere ...(<strong>" + file.percent + "%</strong>)");
//            },
//            UploadComplete: function (up, file) {
//                $('#btn-msj').hide();
//                mAlerta("Subida completada <strong>Por favor espere</strong>");

//                $.ajax({
//                    type: 'POST',
//                    url: controlador + 'LeerFileUpArchivo',
//                    dataType: 'json',
//                    async: false,
//                    success: function (evt) {
//                        var res = evt.Origen.split(',');
//                        if (res[0] != '0') {
//                            alert("Carga de datos Historico exitoso..!!");
//                        } else { alert(res[1]); }
//                        $('#btn-msj').hide();
//                    },
//                    error: function () {
//                        mostrarError("Ha ocurrido un error");
//                    }
//                });
//            },
//            Error: function (up, err) {
//                //mostrarError(err.code + "-" + err.message);
//                if (err.code == -600) {
//                    alert("La capacidad máxima de Zip es de 30Mb..... \nEliminar carpetas o archivos que no son parte del contenido del archivo ZIP."); return;
//                }
//            }
//        }
//    });

//    uploader.init();
//}

function mAlerta(mensaje) {
    $('#btn-msj').show();
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-alert');
}

function remitirTodo() {
    var cadena = "";
    var checkboxes = document.getElementById('tbSeleccionados').getElementsByTagName('input');
    for (var i = 0; i < checkboxes.length; i++) {
        if (checkboxes[i].type == 'checkbox' && checkboxes[i].checked == true) {
            var valor = checkboxes[i].id;
            if (cadena == "") {
                cadena = valor;
            }
            else {
                cadena = cadena + "," + valor;
            }
        }
    }

    if (cadena != "") {
        if (cadena.split(',').length == 1) {
            //var arr = cadena.split(',');
            //for (var i = 0; i < arr.length; i++) {
            $.ajax({
                type: 'POST',
                url: controlador + 'LeerFileUpArchivo',
                data: { nameFile: cadena },
                dataType: 'json',
                //async: false,
                success: function (evt) {
                    var res = evt.Origen.split(',');
                    if (res[0] != '0') {
                        alert("Carga de datos Historico exitoso..!!");
                    } else { alert(res[1]); }
                },
                error: function (err) {
                    mostrarError("Ha ocurrido un error");
                }
            });
            //}
        } else { alert('Solo se permite seleccionar un archivo...'); }
    } else { alert('Por favor seleccione un registro'); }
}