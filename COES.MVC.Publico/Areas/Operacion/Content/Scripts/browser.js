var controladorbrowse = siteRoot + 'operacion/visor/'

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
});

openDirectory = function (url) {
    $('#hfRelativeDirectory').val(url);
    browser();
}

browser = function () {
    var url = $('#hfRelativeDirectory').val();

 
    $.ajax({
        type: 'POST',
        url: controladorbrowse + 'folder',
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
    document.location.href = controladorbrowse + "download?url=" + url + "&pathAlternativo=" + $('#hfOrigen').val();
}

downloadBlobs = function (urls) {
    var contenedor = $('#multiDownload');
    contenedor.html("");
    for (i in urls) {
        contenedor.append('<a href="' + controladorbrowse + "download?url=" + urls[i] + '&pathAlternativo=' + $('#hfOrigen').val() + '" class="file-download"></a>');
    }
    $('.file-download').multiDownload();
}

downloadAsZip = function (urls) {
    $.ajax({
        type: 'POST',
        url: controladorbrowse + "generarzip",
        dataType: 'json',
        traditional: true,
        data: {
            urls: urls,
            pathAlternativo: $('#hfOrigen').val()
        },
        success: function (result) {
            if (result == 1) {
                window.location = controladorbrowse + 'downloadzip';
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