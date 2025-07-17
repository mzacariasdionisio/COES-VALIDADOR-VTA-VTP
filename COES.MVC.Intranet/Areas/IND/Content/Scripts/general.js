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

    $("#opcionPeriodo").click()

    //
    $('#cbAnioFiltroGeneral').change(function () {
        listadoPeriodo();
    });

    $('#cbPeriodoFiltroGeneral').change(function () {
        cargarRevisionesFiltroGeneral();
    });

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

    listadoPeriodo();

});

function cargarFrameSubasta(url, alto) {
    $('#cntMenu').css("display", "none");

    var periodo = parseInt($("#cbPeriodoFiltroGeneral").val()) || 0;
    var recalculo = parseInt($("#cbRevisionFiltroGeneral").val()) || 0;
    var urlFrame = (siteRoot != null && siteRoot.length > 3 ? siteRoot : "") + url;

    if (urlFrame.slice(-1) !== "/")
        urlFrame = urlFrame + "&pericodi=" + periodo + "&recacodi=" + recalculo;
    else
        urlFrame = urlFrame.substring(0, urlFrame.length - 1) + "?pericodi=" + periodo + "&recacodi=" + recalculo;

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
                        $('#cbPeriodoFiltroGeneral').get(0).options[$('#cbPeriodoFiltroGeneral').get(0).options.length] = new Option(item.Iperinombre, item.Ipericodi);
                    });

                    if (evt.IdPeriodo > 0)
                        $('#cbPeriodoFiltroGeneral').val(evt.IdPeriodo);
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

function cargarRevisionesFiltroGeneral() {

    var ipericodi = parseInt($("#cbPeriodoFiltroGeneral").val()) || 0;
    var omitirQuincenal = parseInt($("#flagMensual").val()) || 0;

    $("#cbRevisionFiltroGeneral").empty();

    $.ajax({
        type: 'POST',
        url: controlador + "RecalculoListado",
        data: {
            ipericodi: ipericodi,
            omitirQuincenal: omitirQuincenal
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {

                if (evt.ListaRecalculo.length > 0) {
                    $.each(evt.ListaRecalculo, function (i, item) {
                        $('#cbRevisionFiltroGeneral').get(0).options[$('#cbRevisionFiltroGeneral').get(0).options.length] = new Option(item.Irecanombre, item.Irecacodi);
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

function descargarCuadro() {
    var obj = getObjetoJsonCuadro();

    $.ajax({
        type: 'POST',
        url: controlador + 'CuadroAplicativoGuardar',
        data: {
            cuadro: obj.cuadro,
            irecacodi: obj.irecacodi,
            tipoReporte: obj.tipoReporte,
            tiempo: obj.tiempo,
            medicionorigen: obj.medicionorigen,
            famcodi: obj.famcodi,
            idReporteAsegurada: obj.idReporteAsegurada,
            idReporteFactorK: obj.idReporteK
        },
        cache: false,
        success: function (result) {
            if (evt.Resultado != "-1") {
                window.location = controlador + "Exportar?file_name=" + evt.Resultado;
            } else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

function getObjetoJsonCuadro() {
    var obj = {};

    obj.cuadro = parseInt($("#hfCuadro").val()) || 0;
    obj.irecacodi = parseInt($("#cbRecalculo").val()) || 0;
    obj.tipoReporte = "A";
    obj.famcodi = parseInt($("#famcodi_cuadro").val()) || 0;
    obj.tiempo = $('input[name=form_tiempo]:checked').val();
    obj.medicionorigen = null;
    obj.idReporteAsegurada = parseInt($("#codigo_version_asegurada").val()) || 0;
    obj.idReporteK = parseInt($("#codigo_version_factork").val()) || 0;

    return obj;
}
