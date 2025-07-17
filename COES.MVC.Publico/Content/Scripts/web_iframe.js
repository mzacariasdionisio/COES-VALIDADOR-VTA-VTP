﻿var controlador = siteRoot + 'web/'

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

    $('#btn-indice').click(function () {

        if ($('#contentBusqueda').css("display") == "none") {
            $('#pdf').css("width", "770px");
            $('#pdf').css("margin-left", "10px");
            $('#contentBusqueda').css("display", "block");
            $('#lblIndice').text("Ocultar");
        }
        else if ($('#contentBusqueda').css("display") == "block") {
            $('#pdf').css("width", "1170px")
            $('#pdf').css("margin-left", "0px");
            $('#contentBusqueda').css("display", "none");
            $('#lblIndice').text("Mostrar");
        }
    });

    browser();
});

openDirectory = function (url) {
    $('#hfRelativeDirectory').val(url);
    browser();
}

buscarArchivos = function () {
    var contador = 0;
    var listId = new Array();
    var listValue = new Array();
    var mensaje = '';

    $("#formularioBusqueda :input").each(function (index, element) {

        if ($(this).attr("type") != 'button') {
            var id = $(this).attr("id");
            id = id.replace("campo", "");
            var value = $(this).val();
            if (value != "") {
                listId.push(id);
                listValue.push(value);
                contador++;
            }
        }
    });
    if (contador > 0) {
        pintarPaginado(listId, listValue);
        /*   browser();*/
        procesarFormulario(1);
    }
    else {
        //alert("Complete al menos un campo.");
        browser();
    }
}

function borrardatos() {
    $('#campo7').val('');
    $('#campo39').val('');
    $('#campo42').val('');
}

pintarPaginado = function (listId, listValue) {
    var ids = listId.join(',');
    var valores = listValue.join('&');
    var controladorweb = controlador == null ? "/PublicoNuevo/web/" : controlador;
    $.ajax({
        type: "POST",
        url: controladorweb + "paginado",
        data: {
            url: $('#hfRelativeDirectory').val(),
            ids: ids,
            valores: valores
        },
        success: function (evt) {
            $('#cntPaginado').html(evt);
            mostrarPaginado();
        },
        error: function () {
            mostrarError();
        }
    });
}

procesarFormulario = function (nroPagina) {
    var contador = 0;
    var listId = new Array();
    var listValue = new Array();
    var mensaje = '';

    $("#formularioBusqueda :input").each(function (index, element) {
        if ($(this).attr("type") != 'button') {
            var id = $(this).attr("id");
            id = id.replace("campo", "");
            var value = $(this).val();

            if (value != "") {
                listId.push(id);
                listValue.push(value);
                contador++;
            }
        }
    });

    var ids = listId.join(',');
    var valores = listValue.join('&');
    var controladorweb = controlador == null ? "/PublicoNuevo/web/" : controlador;
    $.ajax({
        type: "POST",
        url: controladorweb + "busqueda",
        data: {
            url: $('#hfRelativeDirectory').val(),
            ids: ids,
            valores: valores,
            nroPagina: nroPagina,
            indicador: $('#hfIndicadorHeader').val(),
            baseDirectory: $('#hfBaseDirectory').val(),
            initialLink: $('#hfBreadName').val(),
            orderFolder: $('#hfOrderFolder').val()
        },
        success: function (evt) {
            /*    $('#contentBusqueda').html(evt);*/
            $('#browserDocument').html(evt);
            $.contextMenu({
                selector: '.selector-file-contextual',
                callback: function (key, options) {
                    var urlFile = $(this).attr('id');
                    var urls = [];
                    urls.push(urlFile);
                    if (key == "downloadblob") {
                        downloadBlob(urlFile, '');
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

            $.contextMenu({
                selector: '.selector-directory-contextual',
                callback: function (key, options) {
                    var urlFolder = $(this).attr('id')
                    var urlsFolder = [];
                    urlsFolder.push(urlFolder);
                    if (key == "opendirectory") {
                        openDirectory(urlFolder);
                    }
                    else if (key == "copiarurldirectory") {
                        copiarUrlFolder(urlFolder);
                    }
                },
                items: {
                    "opendirectory": { name: "Abrir", icon: "opendirectory" },
                    "copiarurldirectory": { name: "Copiar enlace", icon: "copiarurldirectory" }
                }
            });
            if ($('#hfIndicadorCabecera').val() == "S") {
                $('#headDocument').css('display', 'none');
                $('#tbDocumentLibrary').css('width', 'auto');
            }
        },
        error: function () {
            mostrarError();
        }
    });
}

browser = function () {
    debugger;
    var url = $('#hfRelativeDirectory').val();
    var action = "vistadatos";
    var controladorweb = controlador == null ? "/PublicoNuevo/web/" : controlador;
    $.ajax({
        type: 'POST',
        url: controladorweb + action,
        data: {
            baseDirectory: $('#hfBaseDirectory').val(),
            url: url,
            indicador: $('#hfIndicadorHeader').val(),
            initialLink: $('#hfBreadName').val(),
            orderFolder: $('#hfOrderFolder').val()
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
                        downloadBlob(urlFile, '');
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

            $.contextMenu({
                selector: '.selector-directory-contextual',
                callback: function (key, options) {
                    var urlFolder = $(this).attr('id')
                    var urlsFolder = [];
                    urlsFolder.push(urlFolder);
                    if (key == "opendirectory") {
                        openDirectory(urlFolder);
                    }
                    else if (key == "copiarurldirectory") {
                        copiarUrlFolder(urlFolder);
                    }
                },
                items: {
                    "opendirectory": { name: "Abrir", icon: "opendirectory" },
                    "copiarurldirectory": { name: "Copiar enlace", icon: "copiarurldirectory" }
                }
            });

            if ($('#hfIndicadorCabecera').val() == "S") {
                $('#headDocument').css('display', 'none');
                $('#tbDocumentLibrary').css('width', 'auto');
                borrardatos();
            }
        },
        error: function () {
            mostrarError();
        }
    });
}

copiarUrlBlob = function (url) {
    var newUrl = urlRoot + "/portal/browser/download?url=" + encodeURIComponent(url);
    var $temp = $("<input>");
    $("body").append($temp);
    $temp.val(newUrl).select();
    document.execCommand("copy");
    $temp.remove();

}

copiarUrlFolder = function (url) {
    var newUrl = urlRoot + window.location.pathname + "?path=" + encodeURIComponent(url);
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

        if (extension == ".mp4" || extension == ".mov" || extension == ".avi" || extension == ".mpeg" || extension == "wmv") {
            visualizarVideo(url);
        }
        else {
            downloadBlob(url);
        }
    }
    if (type == "D") {
        $('#hfRelativeDirectory').val(url);
        browser();
    }
};

visualizarVideo = function (url) {
    var controladorweb = controlador == null ? "/PublicoNuevo/web/" : controlador;
    $.ajax({
        type: "POST",
        url: controladorweb + "visorvideo",
        data: {
            url: url
        },
        success: function (evt) {
            $('#contenidoVideo').html(evt);

            setTimeout(function () {
                $('#popupVideo').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown'
                });
            }, 50);
        },
        error: function (request, status, error) {
            console.log(request.responseText);
            mostrarError();
        }
    });
};

downloadBlob = function (url, extension) {
    var controladorweb = controlador == null ? "/PublicoNuevo/web/" : controlador;
    if (extension == '.html') {
        window.open(controladorweb + "downloadhtml?url=" + url);
    }
    else {
        document.location.href = controladorweb + "download?url=" + url;
    }
}

downloadBlobs = function (urls) {

    var origen = $('#hfOrigen').val();
    var contenedor = $('#multiDownload');
    var controladorweb = controlador == null ? "/PublicoNuevo/web/" : controlador;
    for (i in urls) {
        if (origen == "Local") {
            contenedor.append('<a href="' + controladorweb + "download?url=" + urls[i] + '" class="file-download"></a>');
        }
        else {
            contenedor.append('<a href="' + "https://s3.amazonaws.com/coesfileserver/" + urls[i] + '" class="file-download"></a>');
        }
    }
    //event.preventDefault();
    $('.file-download').multiDownload();
}

downloadAsZip = function (urls) {
    var controladorweb = controlador == null ? "/PublicoNuevo/web/" : controlador;
    $.ajax({
        type: 'POST',
        url: controladorweb + "generarzip",
        dataType: 'json',
        traditional: true,
        data: {
            urls: urls
        },
        success: function (result) {
            if (result == 1) {
                window.location = controladorweb + 'downloadzip';
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

function cargarPrimeraPagina() {
    var id = 1;
    $('#hfPaginaActual').val(id);
    mostrarPaginado();
    procesarFormulario(id);
}

function cargarAnteriorPagina() {
    var id = $('#hfPaginaActual').val();
    if (id > 1) {
        id = id - 1;
        $('#hfPaginaActual').val(id);
        mostrarPaginado();
        procesarFormulario(id);
    }
}

function cargarPagina(id) {
    $('#hfPaginaActual').val(id);
    mostrarPaginado();
    procesarFormulario(id);
}

function cargarSiguientePagina() {
    var id = parseInt($('#hfPaginaActual').val()) + 1;
    $('#hfPaginaActual').val(id);
    mostrarPaginado();
    procesarFormulario(id);
}

function cargarUltimaPagina() {
    var id = $('#hfNroPaginas').val();
    $('#hfPaginaActual').val(id);
    mostrarPaginado();
    procesarFormulario(id);
}

function mostrarPaginado() {
    var nroToShow = parseInt($('#hfNroMostrar').val());
    var nroPaginas = parseInt($('#hfNroPaginas').val());
    var nroActual = parseInt($('#hfPaginaActual').val());

    $('.pag-ini').css('display', 'none');
    $('.pag-item').css('display', 'none');
    $('.pag-fin').css('display', 'none');
    $('.pag-item').removeClass('paginado-activo');

    $('#pag' + nroActual).addClass('paginado-activo');

    if (nroToShow - nroPaginas >= 0) {
        $('.pag-item').css('display', 'block');
        $('.pag-ini').css('display', 'none');
        $('.pag-fin').css('display', 'none');
    }
    else {
        $('.pag-fin').css('display', 'block');
        if (nroActual > 1) {
            $('.pag-ini').css('display', 'block');
        }
        var anterior = 0;
        var siguiente = 0;

        if (nroActual == 1) {

            anterior = 1;
            siguiente = nroToShow;
        }
        else {
            if (nroActual + nroToShow - 1 - nroPaginas > 0) {
                siguiente = nroPaginas;
                anterior = nroPaginas - nroToShow + 1;
            }
            else {
                anterior = nroActual;
                siguiente = nroActual + nroToShow - 1;
            }
        }

        for (j = anterior; j <= siguiente; j++) {
            $('#pag' + j).css('display', 'block')
        }
    }
}

msieversion = function () {

    // Opera 8.0+
    var isOpera = (!!window.opr && !!opr.addons) || !!window.opera || navigator.userAgent.indexOf(' OPR/') >= 0;

    // Firefox 1.0+
    var isFirefox = typeof InstallTrigger !== 'undefined';

    // Safari 3.0+ "[object HTMLElementConstructor]" 
    var isSafari = /constructor/i.test(window.HTMLElement) || (function (p) { return p.toString() === "[object SafariRemoteNotification]"; })(!window['safari'] || safari.pushNotification);

    // Internet Explorer 6-11
    var isIE = /*@cc_on!@*/false || !!document.documentMode;

    // Edge 20+
    var isEdge = !isIE && !!window.StyleMedia;

    // Chrome 1+
    var isChrome = !!window.chrome && !!window.chrome.webstore;

    // Blink engine detection
    var isBlink = (isChrome || isOpera) && !!window.CSS;

    if (isChrome) {
        return false;
    }
    else {
        return true;
    }
}