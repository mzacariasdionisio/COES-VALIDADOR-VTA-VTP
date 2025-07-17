var controlador = siteRoot + 'tiemporeal/estadistica/'

var flagActivarRefresco = "";
var SEG_REFRESCA_LOG = 15;

var gifcargando = '<img src="' + siteRoot + 'Content/Images/loading1.gif" alt="" width="17" height="17" style="padding-left: 17px;">';

$(function () {

    $('#btnImportar').click(function () {
        importarReporte();
    });

    $('#txtAnio').Zebra_DatePicker({
        format: 'Y',
        onSelect: function (date) {
        }
    });

    adjuntarArchivo();

    mostrarLoadingValidacion();
});

function importarReporte() {
    var anio = parseInt($("#txtAnio").val()) || 0;
    var tipoArchivo = parseInt($("#cbTipoArchivo").val()) || 0;
    var archivoCargado = document.getElementById('ExcelBuscar').getAttribute("name");

    $.ajax({
        type: 'POST',
        url: controlador + "ImportarExcel",
        datatype: 'json',
        data: JSON.stringify({
            tipoArchivo: tipoArchivo,
            filename: archivoCargado, anio: anio
        }),
        contentType: "application/json",
        success: function (evt) {
            if (evt.Resultado != '-1') {
                if (evt.Resultado == "0") {
                    flagActivarRefresco = 'N';
                } else {
                    flagActivarRefresco = 'S';
                    mostrarLogProceso();
                }
            } else {
                alert(evt.Mensaje);

                if (evt.FileName != null) {
                    $('#Message').html("<div class='action-error mensajes'>" + evt.Mensaje + "</div>" +
                        "<div class='action-message mensajes'>" + 'Se generó un archivo excel con las observaciones' + "</div>");
                    $("#listado").html("");

                    window.location.href = controlador + "DescargarExcelLog?nombre=" + evt.FileName;
                }
            }

        },
        error: function (err) {
            alert("Lo sentimos, ha ocurrido un error");
        }
    });
}

function mostrarLoadingValidacion() {
    setInterval(function () {
        if (flagActivarRefresco == "S") {
            mostrarLogProceso();
        }
    }, SEG_REFRESCA_LOG * 1000);
}

function mostrarLogProceso() {
    var anio = parseInt($("#txtAnio").val()) || 0;

    $("#mensaje_log").hide();
    $("#div_log").hide();

    $.ajax({
        type: 'POST',
        url: controlador + 'VerificarProcesamiento',
        dataType: 'json',
        data: {
            anio: anio,
        },
        success: function (model) {
            if (model.Resultado != "-1") {
                if (model.Envio != null) {
                }

                if (flagActivarRefresco == 'S' && model.Resultado == "1") { //si estaba procesando y termina entonces actualizar el listado de archivos
                    window.location.href = controlador + "ExportarZip?file_name=" + model.FileName;
                }

                flagActivarRefresco = model.Resultado == "0" ? 'S' : 'N';

                if (model.Resultado == "0") {

                    var porcentaje = model.Envio != null ? model.Envio.Enviodesc : '';
                    if (porcentaje != '-1') {
                        $("#mensaje_log").show();
                        var htmlMsj = `Procesamiento de Archivos en ejecución. Porcentaje de avance: ${porcentaje}%. ${gifcargando}`;
                        mostrarMensaje("mensaje_log", htmlMsj, $tipoMensajeAlerta, $modoMensajeCuadro);
                    }
                }
                else {
                }
            } else {
                flagActivarRefresco = 'N';
                mostrarMensaje("mensaje_log", model.Mensaje, $tipoMensajeAlerta, $modoMensajeCuadro);
                $("#mensaje_log").show();
            }
        },
        error: function (err) {
            flagActivarRefresco = 'N';
            mostrarMensaje("mensaje", 'Ha ocurrido un error', $tipoMensajeError, $modoMensajeCuadro);
        }
    });

}

//
function adjuntarArchivo() {

    uploaderN = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: 'btnSelectFile',
        container: document.getElementById('container'),
        url: controlador + 'UploadArchivo',
        flash_swf_url: '~/Content/Scripts/Moxie.swf',
        silverlight_xap_url: '~/Content/Scripts/Moxie.xap',
        multi_selection: false,
        filters: {
            mime_types: [
                { title: "Archivos Excel .xlsx", extensions: "xlsx" },
            ]
        },
        init: {
            PostInit: function () {
                $('#loadingcarga').css('display', 'none');
                document.getElementById('filelist').innerHTML = '';
            },

            FilesAdded: function (up, files) {
                document.getElementById('filelist').innerHTML = '';
                $('#container').css('display', 'block');
                for (i = 0; i < uploaderN.files.length; i++) {
                    var file = uploaderN.files[i];
                }
                uploaderN.start();
            },
            UploadProgress: function (up, file) {
            },
            UploadComplete: function (up, file) {
                limpiarInterfaz();

                $('#container').css('display', 'block');
                $('#loadingcarga').css('display', 'none');

                //Envia la fecha y el nombre del archivo seleccionado
                mostarListaArchivos(file[file.length - 1].name);
            },
            Error: function (up, err) {
                $('#container').css('display', 'true');
                if (err.code == -601) {
                    document.getElementById('filelist').innerHTML = '<div class="action-alert">' + 'La extensión del archivo no es válido' + '</div>';
                }
            }
        }
    });

    uploaderN.init();
}

function eliminarFile(id) {
    var string = id.split("@");
    var idProp = string[0];
    var nombreArchivo = string[1];

    uploaderN.removeFile(idProp);

    limpiarInterfaz();

    $.ajax({
        type: 'POST',
        url: controlador + 'EliminarArchivosImportacionNuevo',
        data: { nombreArchivo: nombreArchivo },
        success: function (result) {
            if (result == -1) {
                $('#' + idProp).remove();
            } else {
                alert("Algo salio mal");
            }
        },
        error: function (err) {
        }
    });
}

function mostarListaArchivos(fileName) {
    var autoId = "ExcelBuscar";

    $.ajax({
        type: 'POST',
        url: controlador + 'MostrarArchivoImportacion',
        data: { sFilename: fileName },
        success: function (result) {
            var archivo = result.Documento;
            document.getElementById('filelist').innerHTML = '<div name="' + archivo.FileName + '" class="file-name" id="' + autoId + '">' + archivo.FileName + ' (' + archivo.FileSize + ') <a class="remove-item" href="JavaScript:eliminarFile(\'' + autoId + "@" + archivo.FileName + '\');">X</a> <b></b></div>';
        },
        error: function (err) {
        }
    });
}

function limpiarInterfaz() {
    $('#Message').html('');
    $('#listado').html('');
}
