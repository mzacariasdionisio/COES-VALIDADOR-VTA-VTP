var controlador = siteRoot + "PrimasRER/General/";
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

    $("#opcionPeriodo").click();

    //
    $('#cbAnioFiltroGeneral').change(function () {
        listarPeriodos();
    });

    $('#cbPeriodoFiltroGeneral').change(function () {
        cargarRevisionesFiltroGeneral();
    });

    listarPeriodos();

    $("#btnManualUsuario").click(function () {
        window.location = controlador + 'DescargarManualUsuario';
    });

});

function cargarFrameSubasta(url, alto) {
    $('#cntMenu').css("display", "none");

    let anio = parseInt($("#cbAnioFiltroGeneral").val()) || 0;
    let ipericodi = parseInt($("#cbPeriodoFiltroGeneral").val()) || 0;
    let rerrevcodi = parseInt($("#cbRevisionFiltroGeneral").val()) || 0;
    let urlFrame = (siteRoot != null && siteRoot.length > 3 ? siteRoot : "") + url;

    if (urlFrame.slice(-1) !== "/") {
        urlFrame = urlFrame + "anio=" + anio + "&ipericodi=" + ipericodi + "&rerrevcodi=" + rerrevcodi;
    }
    else {
        urlFrame = urlFrame.substring(0, urlFrame.length - 1) + "?anio=" + anio + "&ipericodi=" + ipericodi + "&rerrevcodi=" + rerrevcodi;
    }

    $('#iframeIndisp').html('');
    $('#iframeIndisp').attr("src", urlFrame);
    $('#iframeIndisp').attr("height", alto);
    $('#iframeIndisp').attr("width", $(window).width() - 50);

    if (contador != 0) {
        navigationFn.goToSection('#iframeIndisp');
    }

    contador++;
}

var navigationFn = {
    goToSection: function (id) {
        $('html, body').animate({
            scrollTop: $(id).offset().top - 30
        }, 150);
    }
}

/*
Carga los periodos filtrado por año en la lista de despliegue del periodo ubicada en la parte superior de la interfaz 
*/
function listarPeriodos() {

    let anio = parseInt($("#cbAnioFiltroGeneral").val()) || 0;
    $("#cbPeriodoFiltroGeneral").empty();

    $.ajax({
        type: 'POST',
        url: controlador + "ListarPeriodos",
        data: {
            anio: anio,
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                if (evt.ListaPeriodo.length > 0) {
                    $.each(evt.ListaPeriodo, function (i, item) {
                        $('#cbPeriodoFiltroGeneral').get(0).options[$('#cbPeriodoFiltroGeneral').get(0).options.length] = new Option(item.Iperinombre, item.Ipericodi);
                    });

                    if (evt.IdPeriodo > 0) {
                        $('#cbPeriodoFiltroGeneral').val(evt.IdPeriodo);
                    }
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
}

/*
Carga las revisiones del periodo en la lista de despliegue de revisiones ubicada en la parte superior de la interfaz
*/
function cargarRevisionesFiltroGeneral() {

    let ipericodi = parseInt($("#cbPeriodoFiltroGeneral").val()) || 0;
    $("#cbRevisionFiltroGeneral").empty();

    $.ajax({
        type: 'POST',
        url: controlador + "ListarRevisiones",
        data: {
            ipericodi: ipericodi,
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                if (evt.ListaRevision.length > 0) {
                    $.each(evt.ListaRevision, function (i, item) {
                        $('#cbRevisionFiltroGeneral').get(0).options[$('#cbRevisionFiltroGeneral').get(0).options.length] = new Option(item.Rerrevnombre, item.Rerrevcodi);
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

function descargarEventosYCausas() {
    let ipericodi = parseInt($("#cbPeriodoFiltroGeneral").val()) || 0;

    $.ajax({
        type: 'POST',
        url: siteRoot + 'PrimasRER/insumo/IndexEventosCausas',
        contentType: 'application/json;',
        data: JSON.stringify({
            ipericodi: ipericodi,
        }),
        datatype: 'json',
        success: function (evt) {
            if (evt.Resultado == "-1") {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
            else {
                window.location = controlador + 'abrirarchivo?tipo=' + 1 + '&nombreArchivo=' + evt.Resultado;
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error.");
        }
    });
}