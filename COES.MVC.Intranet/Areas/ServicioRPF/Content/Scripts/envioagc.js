var controlador = siteRoot + 'serviciorpf/envioagc/'

$(function () {
    $('#txtFecha').Zebra_DatePicker({
        direction: false
    });

    $('#btnDescargarFormato').on('click', function () {
        descargaFormato();
    });

    cargarFile();
});

function descargaFormato() {

    var idEmpresa = $('#cbEmpresa').val();
    var fecha = $('#txtFecha').val();

    if (idEmpresa != '' && idEmpresa != '-1' && idEmpresa != '0') {
        if (fecha != '') {
            $.ajax({
                type: 'POST',
                url: controlador + "GenearFormatoCarga",
                data: {
                    idEmpresa: idEmpresa,
                    fecha: fecha
                },
                dataType: 'json',
                cache: false,
                success: function (resultado) {
                    if (resultado.Result == 1 || resultado.Result == 3) {

                        if (resultado.Result == 1) {
                            location.href = controlador + "DescargarFormato?fileName=" + resultado.FileName;
                        }
                        if (resultado.Mensajes.length > 0) {
                            var html = "<ul>";
                            for (var i in resultado.Mensajes) {
                                var html = html + "<li>" + resultado.Mensajes[i].Mensaje + "</li>";
                            }
                            var html = html + "<ul>";
                            mostrarMensaje('mensaje', 'alert', html);
                        }
                        else {
                            mostrarMensaje('mensaje', 'message', 'Por favor complete los datos exigidos.');
                        }
                    }
                    else if (resultado.Result == 2) {
                        mostrarMensaje('mensaje', 'alert', 'No existe formato para la fecha seleccionada.');
                    }
                    else {
                        mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
                    }
                },
                error: function () {
                    mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
                }
            });
        }
        else {
            $('#percentValidacion').addClass('etapa-alert');
            $('#percentValidacion').text("Seleccione fecha de proceso.");
        }
    }
    else {
        $('#percentValidacion').addClass('etapa-alert');
        $('#percentValidacion').text("Seleccione empresa.");
    }
}

function procesarArchivo() {
    $('#percentCargaArchivo').removeClass('etapa-progress');
    $('#percentCargaArchivo').addClass('etapa-ok');
    $('#percentCargaArchivo').text("OK...!");

    $('#percentValidacion').removeClass('etapa-progress');
    $('#percentValidacion').removeClass('etapa-ok');
    $('#percentValidacion').removeClass('etapa-error');
    $('#percentValidacion').removeClass('etapa-alert');

    $('#percentValidacion').addClass('etapa-progress');
    $('#percentValidacion').text("Validando...");

    $('#validacion').html("");

    $('#percentGrabado').removeClass('etapa-progress');
    $('#percentGrabado').removeClass('etapa-error');
    $('#percentGrabado').removeClass('etapa-alert');
    $('#percentGrabado').removeClass('etapa-ok');



    var idEmpresa = $('#cbEmpresa').val();
    var fecha = $('#txtFecha').val();

    if (idEmpresa != '' && idEmpresa != '-1' && idEmpresa != '0') {

        if (fecha != '') {
            $.ajax({
                type: 'POST',
                url: controlador + 'ProcesarArchivo',
                dataType: 'json',
                data: {
                    idEmpresa: idEmpresa,
                    fecha: fecha
                },
                cache: false,
                success: function (resultado) {
                    if (resultado.Result == 1) {

                        $('#percentValidacion').addClass('etapa-ok');
                        $('#percentValidacion').text("Validación de formato correcto.");
                        $('#percentGrabado').addClass('etapa-ok');
                        $('#percentGrabado').text("Los datos se almacenaron correctamente.");
                        $('#btnProcesarFile').css('display', 'none');
                        $('#btnNuevoCarga').css('display', 'block');

                        mostrarMensaje('mensaje', 'message', 'Por favor complete los datos exigidos.');
                    }
                    else {

                        if (resultado.Result == 6) {
                            $('#percentValidacion').addClass('etapa-error');
                            $('#percentValidacion').text("No se ha cargado archivo.");
                        }
                        if (resultado.Result == 3) {
                            $('#percentValidacion').addClass('etapa-alert');
                            $('#percentValidacion').text("Se ha superado el plazo de envio.");
                        }
                        if (resultado.Result == 4) {
                            $('#percentValidacion').addClass('etapa-alert');
                            $('#percentValidacion').text("Solo se permiten fechas del dia anterior.");
                        }
                        if (resultado.Result == 5) {
                            $('#percentValidacion').addClass('etapa-alert');
                            $('#percentValidacion').text("No existe plazo configurado.");
                        }

                        if (resultado.Result == 7) {
                            $('#percentValidacion').addClass('etapa-alert');
                            $('#percentValidacion').text("Existen errores en el formato.");
                            var strHtml = '<ul>';
                            for (var i in resultado.ListaError) {
                                strHtml = strHtml + '<li>' + resultado.ListaError[i] + '</li>';
                            }
                            strHtml = strHtml + '</ul>';
                            $('#validacion').html(strHtml);
                        }
                        if (resultado.Result == -1) {

                            $('#percentValidacion').addClass('etapa-ok');
                            $('#percentValidacion').text("Validación de formato correcto.");
                            $('#percentGrabado').addClass('etapa-error');
                            $('#percentGrabado').text("Ha ocurrido un error.");
                        }

                        mostrarMensaje('mensaje', 'alert', 'Existen errores.');
                    }
                },
                error: function () {
                    $('#percentValidacion').addClass('etapa-error');
                    $('#percentValidacion').text("Ha ocurrido un error.");
                }
            });
        }
        else {
            $('#percentValidacion').addClass('etapa-alert');
            $('#percentValidacion').text("Seleccione fecha de proceso.");
        }
    }
    else {
        $('#percentValidacion').addClass('etapa-alert');
        $('#percentValidacion').text("Seleccione empresa.");
    }
}

function cargarFile() {
    var uploader = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: 'btnSubirArchivo',
        container: document.getElementById('container'),
        url: siteRoot + 'serviciorpf/envioagc/upload',
        flash_swf_url: 'Scripts/Moxie.swf',
        silverlight_xap_url: 'Scripts/Moxie.xap',
        multi_selection: false,
        filters: {
            max_file_size: '30mb',
            mime_types: [
                { title: "Archivos CSV .csv", extensions: "csv" }
            ]
        },
        init: {
            PostInit: function () {
                document.getElementById('btnProcesar').onclick = function () {
                    if (uploader.files.length > 0) {
                        cargarEtapa();
                        uploader.start();
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
                limpiarTodo();
                //- Entra aqui
                uploader.settings.multipart_params = {
                    "fileName": uploader.files[0].name
                };
                up.refresh();
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

function loadInfoFile(fileName, fileSize) {
    $('#fileInfo').removeClass("file-ok");
    $('#fileInfo').removeClass("file-alert");

    $('#fileInfo').html(fileName + " (" + fileSize + ")");
    $('#fileInfo').addClass("file-ok");
}

function loadValidacionFile(mensaje) {
    $('#fileInfo').removeClass("file-ok");
    $('#fileInfo').removeClass("file-alert");

    $('#fileInfo').html(mensaje);
    $('#fileInfo').addClass("file-alert");
}

function mostrarProgreso(porcentaje) {
    $('#percentCargaArchivo').text(porcentaje + "%");
}

function cargarEtapa() {
    $('#contentEtapa').css('display', 'block');
    $('#percentCargaArchivo').addClass('etapa-progress')
}

function ocultarEtapa() {
    $('#contentEtapa').css('display', 'none');
}

function limpiarTodo() {
    $('#percentCargaArchivo').removeClass('etapa-progress');
    $('#percentCargaArchivo').removeClass('etapa-error');
    $('#percentCargaArchivo').removeClass('etapa-alert');
    $('#percentCargaArchivo').removeClass('etapa-ok');

    $('#percentValidacion').removeClass('etapa-progress');
    $('#percentValidacion').removeClass('etapa-error');
    $('#percentValidacion').removeClass('etapa-alert');
    $('#percentValidacion').removeClass('etapa-ok');

    $('#percentGrabado').removeClass('etapa-progress');
    $('#percentGrabado').removeClass('etapa-error');
    $('#percentGrabado').removeClass('etapa-alert');
    $('#percentGrabado').removeClass('etapa-ok');

    $('#percentCargaArchivo').html("");
    $('#percentValidacion').html("");
    $('#percentGrabado').html("");
    $('#validacion').html("");

    $('#contentEtapa').css("display", "none");
}

function mostrarMensaje(id, tipo, mensaje) {
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);
};