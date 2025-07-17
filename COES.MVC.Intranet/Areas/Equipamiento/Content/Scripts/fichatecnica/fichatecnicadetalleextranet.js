var controlador = siteRoot + 'Equipamiento/fichatecnica/';
var ancho = 1200;

$(function () {
    $("#btnRegresar").click(function () {
        history.back();
    });

    $("#btnExportar").click(function () {
        exportarDetalleFichaTecnica();
    });

    $('#btnExportarReporte').click(function () {
        var flag = parseInt($("#hfFlagExisteComentario").val()) || 0;
        if (flag == 0) {
            $('#rd2').prop('checked', true);
            exportarDetalleFichaTecnica();
        }
        else {
            setTimeout(function () {
                $("#popupExportarReporte").bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown',
                    modalClose: false
                });
            }, 50);
        }
    });
    $('#btnCancelarExportacion').click(function () {
        $('#popupExportarReporte').bPopup().close();
    });

    ancho = $('#mainLayout').width() - 50;

    inicializarDetalle();
});

function inicializarDetalle() {
    $(".label_filtro").hide();
    $(".valor_filtro").hide();
    $(".cb_estado").show();

    $('#textoMensaje').css("display", "none");
    var existe = $("#hfExisteElemento").val();

    if (existe == "1") {
        $("#filtro_elemento").show();
        $("#titulo").html($("#hfTituloFicha").val());

        var idtipodato = $("#hfTipoElementoId").val();
        var tipo = $("#hfTipoElemento").val();

        if (idtipodato == 1) { //subestacion
            $(".cb_empresa").show();
            $('#filtro_tipo_ubicacion').css("paddingLeft", "0px");
        }
        else {
            $(".td_empresa").show();
            $("#filtro_empresa").html($("#hfEmpresa").val());
        }

        if ($("#hfOrigenPadreTipoDesc").val() != null && $("#hfOrigenPadreTipoDesc").val() != '') {
            //$("#filtro_tipo_ubicacion").html($("#hfOrigenPadreTipoDesc").val() + ":");
            $("#filtro_valor_ubicacion").html($("#hfUbicacion").val());

            $(".td_ubicacion").show();
        }

        //$("#filtro_tipo_elemento").html($("#hfOrigenTipoDesc").val() + ":");
        if (tipo == 2) $("#filtro_tipo_elemento").html("Modo de Operación:");
        $("#filtro_nombre_elemento").html($("#hfNombre").val());
        $("#filtro_tipo_elemento").show();
        $("#filtro_nombre_elemento").show();

        $("#btnRegresar").show();

        generarDetalle();
    }
    else {
        $('#textoMensaje').css("display", "block");
        $('#textoMensaje').removeClass();
        $('#textoMensaje').addClass('action-alert');
        $('#textoMensaje').text("No existe Ficha Técnica.");
        $("#btnExportar").hide();
    }
}

function generarDetalle() {

    var tipo = $("#hfTipoElemento").val();
    var idTipo = $("#hfTipoElementoId").val();
    var idElemento = $("#hfCodigo").val();
    var idFicha = $("#hfCodigoFicha").val();

    $("#detalle_ficha_tecnica").html('');
    GenerarDetalleFT(tipo, idTipo, idElemento, idFicha, ancho, 1);
}

function exportarDetalleFichaTecnica() {
    var idFicha = parseInt($("#hfCodigoFicha").val()) || 0;
    var idElemento = parseInt($("#hfCodigo").val());

    var existe = $("#hfExisteElemento").val();

    var opcion = parseInt($('input[name="tipoExportar"]:checked').val()) || 0;

    if (existe == "1" && opcion > 0) {
        $.ajax({
            type: 'POST',
            url: controlador + 'ExportarFichaTecnicaExtranet',
            data: {
                idFicha: idFicha,
                idElemento: idElemento,
                opcionComentario: opcion
            },
            success: function (evt) {
                if (evt.Error == undefined) {
                    window.location.href = controlador + 'DescargarExcel?archivo=' + evt[0] + '&nombre=' + evt[1];
                }
                else {
                    alert("Error:" + evt.Descripcion);
                }
            },
            error: function (result) {
                alert('Ha ocurrido un error al descargar el archivo excel. ' + result.status + ' - ' + result.statusText + '.');
            }
        });

    }
    else {
        alert("Debe seleccionar un tipo de exportación.");
    }
}

///////////////////////////
/// Ver detalle de los hijos
///////////////////////////
function verDetalle(idFicha, idElemento) {
    window.location.href = controlador + "IndexDetalleExtranet?idFicha=" + idFicha + "&idElemento=" + idElemento;
}