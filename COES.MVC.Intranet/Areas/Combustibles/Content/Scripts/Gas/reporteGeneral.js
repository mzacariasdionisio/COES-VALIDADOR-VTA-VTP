var controlador = siteRoot + 'Combustibles/reportegas/';
var contador = 0;

$(document).ready(function () {
    //$('#cntMenu').css("display", "none");

    $('td div.suboptions').click(function () {
        $('td div.suboptions').removeClass('selected');

        $(this).addClass('selected');
    });

    $('td div.suboptions').click(function () {
        cargarFrame($(this).attr('data-url'), $(this).attr('data-alto'));
    });

    //$("#opcionPeriodo").click()

    //listadoPeriodo();

});

function cargarFrame(url, alto) {
    $('#cntMenu').css("display", "none");

    var periodo = 0;
    var recalculo = 0;
    var urlFrame = (siteRoot != null && siteRoot.length > 3 ? siteRoot : "") + url;

    //if (urlFrame.slice(-1) !== "/")
    //    urlFrame = urlFrame + "&pericodi=" + periodo + "&recacodi=" + recalculo;
    //else
    //    urlFrame = urlFrame.substring(0, urlFrame.length - 1) + "?pericodi=" + periodo + "&recacodi=" + recalculo;

    $('#myiframe').html('');
    $('#myiframe').attr("src", urlFrame);
    $('#myiframe').attr("height", alto);
    $('#myiframe').attr("width", $(window).width() - 50);

    if (contador != 0)
        navigationFn.goToSection('#myiframe');

    contador++;
}

var navigationFn = {
    goToSection: function (id) {
        $('html, body').animate({
            scrollTop: $(id).offset().top - 30
        }, 150);
    }
}

