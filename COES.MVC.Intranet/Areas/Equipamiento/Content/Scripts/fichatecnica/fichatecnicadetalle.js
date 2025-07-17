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
        setTimeout(function () {
            $("#popupExportarReporte").bPopup({
                easing: 'easeOutBack',
                speed: 450,
                transition: 'slideDown',
                modalClose: false
            });
        }, 50);
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

function verDatosMatrix(nombreColumna, datos, nombre) {
    $("#popupMatriz .popup-title").html(nombre);
    $("#divTablapMatriz").html("");

    nombreColumna = nombreColumna != null ? nombreColumna : "";
    datos = datos != null ? datos : "";

    var arrayNomCol = nombreColumna.split(",");
    var arrayFilas = datos.split("#");
    var numCol = arrayNomCol.length;

    if (numCol > 0) {
        var htmlTable = '';
        htmlTable += "<table border='0' class='pretty tabla-icono' cellspacing='0'><thead>";
        for (var i = 0; i < numCol; i++) {
            htmlTable += "<th style='width: 100px'>" + arrayNomCol[i] + "</th>";
        }
        htmlTable += "</tr></thead><tbody>";

        var tbody = '';
        for (var j = 0; j < arrayFilas.length; j++) {
            var fila = "<tr>";
            var arrayDataXFila = arrayFilas[j].split("%");
            for (var m = 0; m < numCol; m++) {
                var data = arrayDataXFila.length <= numCol ? (arrayDataXFila[m] != undefined ? arrayDataXFila[m] : "") : "";
                fila += "<td class='desc' style='text-align: center'>" + data + "</td>";
            }
            tbody += fila;
        }

        htmlTable += tbody + "</tbody></table>";

        $("#divTablapMatriz").html(htmlTable);
    } else {
        //$("#divTablapMatriz").html("");
    }
    setTimeout(function () {
        $('#popupMatriz').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown'
        });
    }, 50);
}

function exportarDetalleFichaTecnica() {
    var idFicha = parseInt($("#hfCodigoFicha").val()) || 0;
    var idElemento = parseInt($("#hfCodigo").val());

    var existe = $("#hfExisteElemento").val();

    var opcion = parseInt($('input[name="tipoExportar"]:checked').val()) || 0;

    if (existe == "1" && opcion > 0) {
        $.ajax({
            type: 'POST',
            url: controlador + 'ExportarFichaTecnica',
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

function mostrarError() {
    $('#textoMensaje').css("display", "block");
    $('#textoMensaje').removeClass();
    $('#textoMensaje').addClass('action-alert');
    $('#textoMensaje').text("Ha ocurrido un error");
}

///////////////////////////
/// Ver detalle de los hijos
///////////////////////////
function verDetalle(idFicha, idElemento) {
    window.location.href = controlador + "IndexDetalle?idFicha=" + idFicha + "&idElemento=" + idElemento;
}