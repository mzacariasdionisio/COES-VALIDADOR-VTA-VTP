var controlador = siteRoot + 'Equipamiento/FTVigente/';
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
            $('#chkComent').prop('checked', false);
            $('.coment').prop('hidden', true);
        }

        var mostrarComent = $("#hflagChekComentario").val();
        var mostrarCheckSustento = $("#hflagChekSustento").val();

        if (mostrarCheckSustento == 1 || (flag == 1 && mostrarComent == 1)) {
            setTimeout(function () {
                $("#popupExportarReporte").bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown',
                    modalClose: false
                });
            }, 50);
        }
        else {
            exportarDetalleFichaTecnica();
        }
    });
    $('#btnCancelarExportacion').click(function () {
        $('#popupExportarReporte').bPopup().close();
    });

    ancho = $('#mainLayout').width() - 50;

    inicializarDetalle();

    $('#txtFechaConsulta').Zebra_DatePicker({
        readonly_element: false,
        onSelect: function (date) {
            $("#hfFecha").val($('#txtFechaConsulta').val());
            generarDetalle();
        }
    });

    $("#chkSustento").prop('checked', false);

    $("#chkSustento").on("click", function () {
        var check = $('#chkSustento').is(":checked");
        $(".ChkSeleccion").prop("checked", check);

        if (check) {
            $("#hflagsustento").val(true);
        } else {
            $("#hflagsustento").val(false);
        }

        generarDetalle();
    });

});

function inicializarDetalle() {
    $(".label_filtro").hide();
    $(".valor_filtro").hide();

    $('#textoMensaje').css("display", "none");
    var existe = $("#hfExisteElemento").val();

    if (existe == "1") {
        $("#filtro_elemento").show();
        $("#titulo").html($("#hfTituloFicha").val());

        var mostrarCheckSustento = $("#hflagChekSustento").val();
        if (mostrarCheckSustento == 1) {
            $(".td_sustento").show();
        }

        var mostrarFecha = $("#hflagChekFecha").val();
        if (mostrarFecha == 1) {
            $(".td_fecha").show();
        }

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
    var fecha = $("#hfFecha").val();

    $("#detalle_ficha_tecnica").html('');
    GenerarDetalleFT(tipo, idTipo, idElemento, idFicha, ancho, 1, fecha);
}

function exportarDetalleFichaTecnica() {
    var idFicha = parseInt($("#hfCodigoFicha").val()) || 0;
    var idElemento = parseInt($("#hfCodigo").val());

    var existe = $("#hfExisteElemento").val();

    var opcionComent = $('#chkComent').is(":checked") ? true : false;
    var opcionSust = $('#chkSust').is(":checked") ? true : false;
    var fechaConsulta = $('#txtFechaConsulta').val();

    if (existe == "1") {
        $.ajax({
            type: 'POST',
            url: controlador + 'ExportarFichaTecnica',
            data: {
                idFicha: idFicha,
                idElemento: idElemento,
                opcionComentario: opcionComent,
                opcionSustento: opcionSust,
                fecha: fechaConsulta
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
    window.location.href = controlador + "IndexDetalle?idFicha=" + idFicha + "&idElemento=" + idElemento;
}