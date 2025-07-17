var controller = siteRoot + "Subastas/General/";
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

    $("#btnManualUsuario").click(function () {
        window.location = controller + 'DescargarManualUsuario';
    });

    $("#opcionReporteSubDiaria").click();
});

function cargarFrameSubasta(url, alto) {
    $('#cntMenu').css("display", "none");

    $('#iframeSubasta').html('');
    $('#iframeSubasta').attr("src", (siteRoot != null && siteRoot.length > 3 ? siteRoot: "" ) + url);
    $('#iframeSubasta').attr("height", alto);
    $('#iframeSubasta').attr("width", $(window).width() - 50);

    if (contador != 0)
        navigationFn.goToSection('#iframeSubasta');

    contador++;
}

var navigationFn = {
    goToSection: function (id) {
        $('html, body').animate({
            scrollTop: $(id).offset().top - 30
        }, 150);
    }
}