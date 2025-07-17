var controlador = siteRoot + 'search/'

$(function () {
   
    buscar();
    $('#btnBuscar').click(function () {
        buscar();
    });

    $('#contenedorBusqueda').keypress(function (e) {
        if (e.keyCode == '13') {
            $('#btnBuscar').click();
        }
    });
});

buscar = function () {

    if ($('#txtFiltro').val() != "") {
        if ($('#txtFiltro').val().length >= 2) {
            pintarPaginado();
            mostrarListado(1);
        }
        else {
            mostrarMensaje("Ingrese al menos 2 caracteres");
        }
    }
    else {
        mostrarMensaje("Ingrese el texto a buscar");
    }
}

pintarPaginado = function () {
    $.ajax({
        type: 'POST',
        url: controlador + "paginado",
        data: {
            texto: $('#txtFiltro').val(),
            extension: $("input:radio[name='radioExtension']:checked").val()
        },
        success: function (evt) {
            $('#paginado').html(evt);
            mostrarPaginado();
        },
        error: function () {
            mostrarError();
        }
    });
}

mostrarListado = function (nroPagina) {
    $('#hfNroPagina').val(nroPagina);
    $.ajax({
        type: 'POST',
        url: controlador + "resultado",
        data: {
            texto: $('#txtFiltro').val(),
            extension: $("input:radio[name='radioExtension']:checked").val(),
            nroPagina: nroPagina
        },
        success: function (evt) {
            $('#listaResultado').html(evt);
            $('#listaResultado').highlight($('#txtFiltro').val());
            $('#mensaje').removeClass();
            $('#mensaje').text("");
        },
        error: function () {
            mostrarError();
        }
    });
}

mostrarPrevio = function (url, contador) {
    
    $('#contenidoPrevio').css('margin-top', (contador -1) * 62 + "px");
    $('#contenidoPrevio').show();
    var enlace = urlRoot + "/browser/" + "downloadpdf?url=" + url;         
    verCapitulo(enlace);
}

closePreview = function ()
{
    $('#contenidoPrevio').hide();
}

verCapitulo = function(enlace) {    
    if (!msieversion()) {
        var pdfObjeto = new PDFObject({
            url: enlace,
            id: 'myPDF',
            pdfOpenParams: {
                navpanes: 0,
                toolbar: 0,
                statusbar: 0,
                view: 'FitV'
            }
        });
        $('#pdf').addClass('pdf');
        var html = pdfObjeto.embed('pdf');
    }
    else {
        $('#pdf').addClass('pdf');
        $('#pdf').html("<iframe src='" + enlace + "' style='width: 100%; height: 100%;' ></iframe>");
    }
}

msieversion = function() {
    var flag = false;
    if (navigator.userAgent.match(/Trident\/7\./)) {
        flag = true;
    }
    return flag;

    //alert(navigator.userAgent);
    //var flag = false;
    //if (navigator.userAgent.indexOf("MSIE 10.0") > -1) flag = true;
    //return flag;
}

pintarBusqueda = function (nroPagina) {
    mostrarListado(nroPagina);
}

downloadFile = function (url)
{
    document.location.href = siteRoot + 'browser/' + "download?url=" + url;
}

mostrarError = function ()
{
    $('#mensaje').removeClass();
    $('#mensaje').addClass("action-error");
    $('#mensaje').text("Se produjo un error, intente nuevamente.");
}

mostrarMensaje = function (mensaje)
{
    $('#mensaje').removeClass();
    $('#mensaje').addClass("action-alert");
    $('#mensaje').text(mensaje);
}