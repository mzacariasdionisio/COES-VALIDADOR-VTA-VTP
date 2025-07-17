var controller = siteRoot + "Intervenciones/General/";
var contador = 0;

$(document).ready(function () {
    $('#cntMenu').css("display", "none");

    $('td div.suboptions').click(function () {
        $('td div.suboptions').removeClass('selected');

        $(this).addClass('selected');
    });

    $('td div.suboptions').click(function () {
        var urlOpcion = $(this).attr('data-url');
        var altoPantalla = $(this).attr('data-alto');

        validarSesionModulo(urlOpcion, altoPantalla)
    });

    $("#opcionRegistro").click()

});

function cargarFrameIntervenciones(url, alto) {
    $('#cntMenu').css("display", "none");

    $('#iframeIntervenciones').html('');
    $('#iframeIntervenciones').attr("src", (siteRoot != null && siteRoot.length > 3 ? siteRoot : "") + url);
    $('#iframeIntervenciones').attr("height", alto);
    $('#iframeIntervenciones').attr("width", $(window).width() - 50);

    if (contador != 0)
        navigationFn.goToSection('#iframeIntervenciones');

    contador++;
}

var navigationFn = {
    goToSection: function (id) {
        $('html, body').animate({
            scrollTop: $(id).offset().top - 30
        }, 150);
    }
}

function validarSesionModulo(urlOpcion, altoPantalla) {
    $.ajax({
        type: 'POST',
        url: controller + "ValidarModulo",
        datatype: "json",
        contentType: 'application/json',
        success: function (model) {
            if (model.EsValidoSesion) {
                if (model.EsValidoOpcion) {
                    cargarFrameIntervenciones(urlOpcion, altoPantalla);
                } else {
                    window.location = siteRoot + 'home/default/';
                }
            } else {
                window.location = siteRoot + 'home/login/';
            }
        },
        error: function (err) {
            alert("Se perdió la conexión usuario/servidor COES.");
        }
    });
}