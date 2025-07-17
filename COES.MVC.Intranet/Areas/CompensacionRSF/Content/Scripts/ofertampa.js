var sControlador = siteRoot + "compensacionrsf/ofertampa/";
var sControladorSubasta = siteRoot + "subastas/Oferta/";

$(document).ready(function () {
    $('#btnCopiarOD').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        copiarOD();
    });

    $('#btnExportarOD').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        exportarOD(1);
    });

    uploadExcelOD();

    //$('#btnRefrescar').click(function () {
    //    refrescar();
    //});
});

exportarOD = function (formato) {
    var resultado = validarFechas();
    if (resultado === "") {
        var pericodi = document.getElementById('pericodi').value;
        var oferfechaenvio = document.getElementById('sFechaEnvioIni').value;
        var oferfechaenviofin = document.getElementById('sFechaEnvioFin').value;
        var emprcodi = -1;
        var tipooferta = 1;
        var usercode = -1;
        var opcion = 1;
        $.ajax({
            type: 'POST',
            url: sControlador + "ExportarReporte",
            data: {
                empresacodi: emprcodi,
                tipooferta: tipooferta,
                oferfechaenvio: oferfechaenvio,
                oferfechaenviofin: oferfechaenviofin,
                usercode: usercode,
                opcion: opcion
            },
            dataType: 'json',
            cache: false,
            success: function (model) {
                if (model.Resultado != -1) {
                    location.href = sControladorSubasta + "DescargarReporte";
                    mostrarExito("Felicidades, el archivo se descargo correctamente...!");
                } else {
                    mostrarError('mensaje', 'Lo sentimos, ha ocurrido un error: ' + model.Mensaje);
                }
            },
            error: function (err) {
                mostrarError('mensaje', 'Lo sentimos, ha ocurrido un error: ' + err);
            }
        });
    } else {
        mostrarError('mensaje', resultado);
    }
}

copiarOD = function () {
    var pericodi = document.getElementById('pericodi').value;
    var vcrecacodi = document.getElementById('vcrecacodi').value;
    $.ajax({
        type: 'POST',
        url: sControlador + 'copiarOD',
        data: { pericodi: pericodi, vcrecacodi: vcrecacodi},
        dataType: 'json',
        success: function (model) {
            if (model.sError == "") {
                mostrarExito("Felicidades, se copio correctamente " + model.iNumReg + " registros.");
            }
            else {
                mostrarError("Lo sentimos, ha ocurrido el siguiente error: " + model.sError);
            }
        },
        error: function () {
            mostrarMensaje('mensaje', 'Ha ocurrido un error', $tipoMensajeError, $modoMensajeCuadro);
        }
    });
}

refrescar = function () {
    var cmbPericodi = document.getElementById('pericodi');
    window.location.href = sControlador + "index?pericodi=" + cmbPericodi.value;
}

function validarFechas() {
    var resultado = "";
    if ($('#modulo').val() == 1) {
        var fini = $('#sFechaEnvioIni').val();
        var ffin = $('#sFechaEnvioFin').val();

        var arrFechaIni = fini.split("/");
        var arrFechaFin = ffin.split("/");

        var fecha = new Date(arrFechaIni[2], arrFechaIni[1] - 1, arrFechaIni[0]);
        var fechaFin = new Date(arrFechaFin[2], arrFechaFin[1] - 1, arrFechaFin[0]);
        if (fecha > fechaFin) {
            resultado = "La fecha inicial no puede ser mayor que la fecha final’";
        }
    }
    return resultado;
};

uploadExcelOD = function () {
    var pericodi = document.getElementById('pericodi').value;
    var vcrecacodi = document.getElementById('vcrecacodi').value;
    console.log(vcrecacodi);
    if (vcrecacodi == "") {
        mostrarError("Por favor, seleccionar una versión de recalculo");
    }
    else {
        uploaderOD = new plupload.Uploader({
            runtimes: 'html5,flash,silverlight,html4',
            browse_button: 'btnImportarOD',
            url: sControlador + 'uploadexcel',
            flash_swf_url: 'Scripts/Moxie.swf',
            silverlight_xap_url: 'Scripts/Moxie.xap',
            multi_selection: false,
            filters: {
                max_file_size: '100mb',
                mime_types: [
                    { title: "Archivos Excel .xlsx", extensions: "xlsx,xls" }
                ]
            },
            init: {
                FilesAdded: function (up, files) {
                    if (uploaderOD.files.length == 2) {
                        uploaderOD.removeFile(uploaderOD.files[0]);
                    }
                    uploaderOD.start();
                    up.refresh();
                },
                UploadProgress: function (up, file) {
                    mostrarAlerta("Por favor espere ...(<strong>" + file.percent + "%</strong>)");
                },
                UploadComplete: function (up, file) {
                    mostrarAlerta("Subida completada <strong> procesando el archivo...</strong>");
                    procesarArchivoOD(file[0].name);
                },
                Error: function (up, err) {
                    mostrarError(err.code + "-" + err.message);
                }
            }
        });
        uploaderOD.init();
    }
}

procesarArchivoOD = function (sFile) {
    var vcrecacodi = document.getElementById('vcrecacodi').value;
    $.ajax({
        type: 'POST',
        url: sControlador + 'ProcesarArchivoOD',
        data: { sarchivo: sFile, vcrecacodi: vcrecacodi },
        dataType: 'json',
        cache: false,
        success: function (result) {
            if (result.sError == "")
                mostrarExito(result.sMensaje);
            else
                mostrarError("Lo sentimos no se puedo grabar la información ingresada " + result.sError);
        },
        error: function () {
            mostrarError('Lo sentimos no se puedo grabar la información ingresada');
        }
    });
}





 
 
 
 
 