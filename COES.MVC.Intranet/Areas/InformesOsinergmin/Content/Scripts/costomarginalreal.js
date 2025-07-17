var controlador = siteRoot + 'informesosinergmin/costomarginalreal/';

$(function () {
    $('#mesInforme').Zebra_DatePicker({
        format: 'm Y',
        onSelect: function (dataFecha) {
            cargarCMRMensual(dataFecha);
            cargarCMRDiario(dataFecha);
            mostrarNombreArchivoExcel(dataFecha);
            $('#fileInfo').html("");
            $('#progreso').text("");
            $('#mensaje').text("");
            $('#mensaje').removeClass();
        },
        direction: -1,
        show_clear_date: false
    });

    $('#tab-container-principal').easytabs({
        animate: false
    });
    cargarCMRMensual($('#mesInforme').val());
    cargarCMRDiario($('#mesInforme').val());
    cargarArchivo();
});

cargarArchivo = function () {
    var uploader = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: 'btnSelectFile',
        container: document.getElementById('container'),
        url: controlador + 'upload',
        flash_swf_url: '~/Content/Scripts/Moxie.swf',
        silverlight_xap_url: '~/Content/Scripts/Moxie.xap',
        multi_selection: false,
        filters: {
            max_file_size: '2mb',
            mime_types: [ { title: "Archivos Excel .xlsx", extensions: "xlsx" } ]
        },
        init: {
            PostInit: function () {
                document.getElementById('btnProcesarFile').onclick = function () {
                    if (uploader.files.length > 0) {
                        var nombreCorrectoArchivo =
                            "cmgdtr" +
                            $('#mesInforme').val().replace($('#mesInforme').val().substring(2, 5),"") +
                            ".xlsx";
                        if (nombreCorrectoArchivo == uploader.files[0].name) {
                            uploader.start();
                        } else {
                            mensajeError("El archivo " + uploader.files[0].name + ", no corresponde");
                        }
                    }
                    else
                        loadValidacionFile("Seleccione archivo.");
                    return false;
                };
            },
            FilesAdded: function (up, files) {
                if (uploader.files.length == 2) {
                    uploader.removeFile(uploader.files[0]);
                }
                plupload.each(files, function (file) {
                    loadInfoFile(file.name, plupload.formatSize(file.size));
                });
                up.refresh();
                uploader.settings.multipart_params = {
                    "mesInforme": $('#mesInforme').val()
                }
            },
            UploadProgress: function (up, file) {
                mostrarProgreso(file.percent);
            },
            UploadComplete: function (up, file) {             
                procesarArchivo();
            },
            Error: function (up, err) {
                loadValidacionFile(err.code + "-" + err.message);
            }
        }
    });

    uploader.init();
}

function loadValidacionFile(mensaje) {
    $('#fileInfo').html(mensaje);
}

function loadInfoFile(fileName, fileSize) {
    $('#fileInfo').html(fileName + " (" + fileSize + ")");
}

function mostrarProgreso(porcentaje) {
    $('#progreso').text(porcentaje + "%");
}

cargarCMRMensual = function (dataFecha) {
    $.ajax({
        type: "POST",
        url: controlador + "cargaCMRMensual",
        data: {
            mesInforme: dataFecha
        },
        success: function (evt) {
            $("#vistaCMRMensual").html(evt);
        },
        error: function () {
            mostrarError();
        }
    });
}

cargarCMRDiario = function (dataFecha) {
    $.ajax({
        type: "POST",
        url: controlador + "cargaCMRDiario",
        data: {
            mesInforme: dataFecha
        },
        success: function (evt) {            
            $('#vistaCRMDiario').html(evt);

        },
        error: function () {
            mostrarError();
        }
    });
}

mostrarNombreArchivoExcel = function (dataFecha) {
    var nombreCorrectoArchivo =
                            "cmgdtr" +
                            dataFecha.replace(dataFecha.substring(2, 5), "") +
                            ".xlsx";
    $('#nombreArchivo').text(nombreCorrectoArchivo);
}

procesarArchivo = function () {
    $.ajax({
        type: 'POST',
        url: controlador + 'datoscmr',
        dataType: 'json',
        data: {
            mesInforme: $('#mesInforme').val()
        },
        cache: false,
        success: function (resultado) {
            $('#vistaLog').removeClass();
            $('#vistaLog').html("");
            if (resultado == 1) {
                cargarCMRMensual($('#mesInforme').val());
                cargarCMRDiario($('#mesInforme').val());
                mostrarExito();
            }
            else if (resultado == -1) {
                mostrarError();
            } else {
                mensajeError("Se han encontrado errores en el archivo, revisar Logs")
                $('#vistaLog').removeClass();
                $('#vistaLog').html(resultado);
                $('#vistaLog').addClass("action-alert");
            }
        },
        error: function () {
            mostrarError();
        }
    });
}

mostrarError = function () {
    alert("Ha ocurrido un error.");
}

mensajeError = function (msj) {
    $('#mensaje').removeClass();
    $('#mensaje').text(msj);
    $('#mensaje').addClass("action-alert");
}

mostrarExito = function () {
    $('#mensaje').removeClass();
    $('#mensaje').text("Proceso ejecutado correctamente");
    $('#mensaje').addClass("action-exito");
}