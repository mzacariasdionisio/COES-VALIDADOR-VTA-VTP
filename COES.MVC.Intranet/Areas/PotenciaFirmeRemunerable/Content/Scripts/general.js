var controlador = siteRoot + "PotenciaFirmeRemunerable/General/";
var contador = 0;

$(document).ready(function () {
    $('#cntMenu').css("display", "none");

    $('td div.suboptions').click(function () {
        $('td div.suboptions').removeClass('selected');

        $(this).addClass('selected');
    });

    $('td div.suboptions').click(function () {
        cargarFrame($(this).attr('data-url'), $(this).attr('data-alto'));
    });

    $("#opcionPeriodo").click();

    //
    $('#cbAnioFiltroGeneral').change(function () {
        listadoPeriodo();
    });

    $('#cbPeriodoFiltroGeneral').change(function () {
        cargarRevisionesFiltroGeneral();
    });

    //listadoPeriodo();

    
    $('#btnGams').click(function () {
        procesarPotFirmeR();
    });
});

function procesarPotFirmeR() {
    var pfpericodi = parseInt($("#cbPeriodoFiltroGeneral").val()) || 0;
    $.ajax({
        type: 'POST',
        url: controlador + "ProcesarPotFirmeRemunerable",
        data: {
            pfpericodi: pfpericodi,
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                alert("Exito " );
            }
            else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

function cargarFrame(url, alto) {
    //$('#cntMenu').css("display", "none");

    var periodo = parseInt($("#cbPeriodoFiltroGeneral").val()) || 0;
    var recalculo = parseInt($("#cbRevisionFiltroGeneral").val()) || 0;
    var urlFrame = (siteRoot != null && siteRoot.length > 3 ? siteRoot : "") + url;
    urlFrame = urlFrame.slice(-1) !== "/" ? urlFrame : urlFrame.substring(0, urlFrame.length - 1);

    $('#myiframe').html('');
    $('#myiframe').attr("src", urlFrame + "?pericodi=" + periodo + "&recacodi=" + recalculo);
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

function listadoPeriodo() {

    var anio = parseInt($("#cbAnioFiltroGeneral").val()) || 0;

    $("#cbPeriodoFiltroGeneral").empty();

    $.ajax({
        type: 'POST',
        url: controlador + "PeriodoListado",
        data: {
            anio: anio,
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                if (evt.ListaPeriodo.length > 0) {
                    $.each(evt.ListaPeriodo, function (i, item) {
                        $('#cbPeriodoFiltroGeneral').get(0).options[$('#cbPeriodoFiltroGeneral').get(0).options.length] = new Option(item.Pfrpernombre, item.Pfrpercodi);
                    });
                } else {
                    $('#cbPeriodoFiltroGeneral').get(0).options[0] = new Option("--", "0");
                }

                cargarRevisionesFiltroGeneral();
            } else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
};

function cargarRevisionesFiltroGeneral() {

    var pfpericodi = parseInt($("#cbPeriodoFiltroGeneral").val()) || 0;

    $("#cbRevisionFiltroGeneral").empty();

    $.ajax({
        type: 'POST',
        url: controlador + "CargarRevisiones",
        data: {
            pfpericodi: pfpericodi,
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {

                if (evt.ListaRecalculo.length > 0) {
                    $.each(evt.ListaRecalculo, function (i, item) {
                        $('#cbRevisionFiltroGeneral').get(0).options[$('#cbRevisionFiltroGeneral').get(0).options.length] = new Option(item.Pfrrecnombre, item.Pfrreccodi);
                    });
                } else {
                    $('#cbRevisionFiltroGeneral').get(0).options[0] = new Option("--", "0");
                }
            } else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}
