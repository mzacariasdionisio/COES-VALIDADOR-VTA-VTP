var controlador = siteRoot + "IND/General/";
var contador = 0;

$(document).ready(function () {
    $('#cntMenu').css("display", "none");

    $('td div.suboptions').click(function () {
        $('td div.suboptions').removeClass('selected');

        $(this).addClass('selected');
    });

    $('td div.suboptions').click(function () {
        cargarFrameSubasta($(this).attr('data-url'), $(this).attr('data-alto'));
    });

    $("#opcionInicial").click()

    //
    $('#btnPopupPreliminar').click(function () {
        setTimeout(function () {
            $('#popupPreliminar').bPopup({
                easing: 'easeOutBack',
                speed: 450,
                transition: 'slideDown',
                modalClose: true
            });
        }, 50);
    });
    $('#btnDescargar').click(function () {
        descargarCuadro();
    });
    $('#btnCerrar').click(function () {
        $('#popupPreliminar').bPopup().close();
    });

});

function cargarFrameSubasta(url, alto) {
    $('#cntMenu').css("display", "none");

    var periodo = parseInt($("#cbPeriodoFiltroGeneral").val()) || 0;
    var recalculo = parseInt($("#cbRevisionFiltroGeneral").val()) || 0;
    var urlFrame = (siteRoot != null && siteRoot.length > 3 ? siteRoot : "") + url;

    $('#iframeIndisp').html('');
    $('#iframeIndisp').attr("src", urlFrame);
    $('#iframeIndisp').attr("height", alto);
    $('#iframeIndisp').attr("width", $(window).width() - 50);

    if (contador != 0)
        navigationFn.goToSection('#iframeIndisp');

    contador++;
}

var navigationFn = {
    goToSection: function (id) {
        $('html, body').animate({
            scrollTop: $(id).offset().top - 30
        }, 150);
    }
}

////////////////////////////////////////////////////////////////////////////////////////////////////////////
///
