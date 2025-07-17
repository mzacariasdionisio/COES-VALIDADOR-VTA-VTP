var controlador = siteRoot + 'browser/'


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

    $('#btn-busqueda').click(function () {
        if ($('#cuadroBusqueda').css('display') == 'none')
            $('#cuadroBusqueda').css('display', 'block');
        else
            $('#cuadroBusqueda').css('display', 'none');
    });


    browser();
});

openDirectory = function (url) {
    $('#hfRelativeDirectory').val(url);
    browser();
}

browser = function () {
    var url = $('#hfRelativeDirectory').val();
    var action = "vistadatosarbol";

    $.ajax({
        type: 'POST',
        url: controlador + action,
        data: {
            baseDirectory: $('#hfBaseDirectory').val(),
            url: url,
            indicador: '',
            initialLink: $('#hfBreadName').val()
        },
        success: function (evt) {
            $('#browserDocument').html(evt);

            $.contextMenu({
                selector: '#tbDocumentLibrary > tbody > tr',
                callback: function (key, options) {
                    var urlFile = $(this).attr('id');
                    var urls = [];
                    urls.push(urlFile);
                    if (key == "downloadblob") {
                        downloadBlob(urlFile);
                    }
                    else if (key == "copiarurlblob") {
                        copiarUrlBlob(urlFile);
                    }
                },
                items: {
                    "downloadblob": { name: "Descargar", icon: "downloadblob" },
                    "copiarurlblob": { name: "Copiar enlace", icon: "copiarurlblob" }
                }
            });           
        },
        error: function () {
            mostrarError();
        }
    });
}

copiarUrlBlob = function (url) {
    var newUrl = urlRoot +  "/portal/browser/download?url=" + encodeURIComponent(url);
    var $temp = $("<input>");
    $("body").append($temp);
    $temp.val(newUrl).select();
    document.execCommand("copy");
    $temp.remove();

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
    document.location.href = controlador + "download?url=" + url;
}

downloadBlobs = function (urls) {

    var origen = $('#hfOrigen').val();
    var contenedor = $('#multiDownload');

    for (i in urls) {
        if (origen == "Local") {
            contenedor.append('<a href="' + controlador + "download?url=" + urls[i] + '" class="file-download"></a>');
        }
        else {
            contenedor.append('<a href="' + "https://s3.amazonaws.com/coesfileserver/" + urls[i] + '" class="file-download"></a>');
        }
    }

    //event.preventDefault();
    $('.file-download').multiDownload();
}

downloadAsZip = function (urls) {
    $.ajax({
        type: 'POST',
        url: controlador + "generarzip",
        dataType: 'json',
        traditional: true,
        data: {
            urls: urls
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