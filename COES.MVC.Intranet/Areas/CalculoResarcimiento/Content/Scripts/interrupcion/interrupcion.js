var controlador = siteRoot + 'calculoresarcimiento/interrupcion/';
var uploader;
var uploaderPtoEntrega;

$(function () {

    $('#txtAnio').Zebra_DatePicker({
        format: 'Y',
        onSelect: function (date) {
            cargarPeriodos(date);
        }
    });

    $('#btnConsultar').on('click', function () {
        consultar();
    });

    $("#btnManualUsuario").click(function () {
        window.location = controlador + 'DescargarManualUsuario';
    });

    $('#spanCambiarEmpresa').on('click', function () {
        cambiarEmpresa();
    });       

    $('#btnDescargarFormato').on('click', function () {
        descargarFormato();
    });

    $('#btnSubirFormato').on('click', function () {
        subirFormato();
    });

    $('#btnEnviarDatos').on('click', function () {
        enviarDatos();
    });

    $('#btnMostrarErrores').on('click', function () {
        mostrarErrores();
    });

    $('#btnVerEnvios').on('click', function () {
        verEnvios();
    });

    $('#btnAgregarFila').on('click', function () {
        agregarFila();
    });

    $('#btnOcultarColumna').on('click', function () {
        limpiarMensaje('mensajeColumna');
        $('#popupColumna').bPopup();
    });

    $('#btnOcultar').on('click', function () {
        aplicarOcultado();
    });

    $('#btnEnviarComentario').on('click', function () {
        grabarInterrupciones();
    });

    $('#btnEnviarAnulacion').on('click', function () {
        confirmarAnulacion();
    });

    $('#btnCancelarAnulacion').on('click', function () {
        $('#popupAnulacion').bPopup().close();
    });

    $('#descargarFileInterrupcion').on('click', function () {
        descargarFileInterrupcion();
    });

    $('#eliminarFileInterrrupcion').on('click', function () {
        eliminarFileInterrupcion();
    });

    $('#cbPeriodo').on('change', function () {
        habilitarEnvio(false);
    })

    habilitarEnvio(false);
    crearPupload();
    cargarGrillaUploader();
});

function consultar() {
    mostrarMensajeDefecto();
    if ($('#cbEmpresa').val() != "") {
        if ($('#cbPeriodo').val() != "") {
            $.ajax({
                type: 'POST',
                url: controlador + 'consultar',
                data: {
                    empresa: $('#cbEmpresa').val(),
                    periodo: $('#cbPeriodo').val()
                },
                dataType: 'json',               
                success: function (result) {
                    if (result.Result == 1) {
                        cargarGrillaInterrupcion(result);                        
                        habilitarEnvio(true);
                    }
                    else {
                        mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
                    }
                },
                error: function () {
                    mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
                }
            });
        }
        else {
            mostrarMensaje('mensaje', 'alert', 'Debe seleccionar periodo.');
        }
    }
    else {
        mostrarMensaje('mensaje', 'alert', 'Seleccione suministrador.');
    }
}


function cargarPeriodos(anio) {
    $.ajax({
        type: 'POST',
        url: controlador + 'obtenerperiodos',
        data: {
            anio: anio
        },
        dataType: 'json',
        global: false,
        success: function (result) {
            if (result != -1) {
                $('#cbPeriodo').get(0).options.length = 0;
                $('#cbPeriodo').get(0).options[0] = new Option("--SELECCIONE--", "");
                $.each(result, function (i, item) {
                    $('#cbPeriodo').get(0).options[$('#cbPeriodo').get(0).options.length] = new Option(item.Repernombre, item.Repercodi);
                });
            }
            else {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            }
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        }
    });
}


function habilitarEnvio(flag) {
    
    if (!flag) {
        $('#divAcciones').hide();
        $('#ctnBusqueda').hide();
        $('#detalleFormato').html('');
    }
    else {
        $('#divAcciones').show();
        $('#ctnBusqueda').show();
    }  
}

function descargarFormato() {
    $.ajax({
        type: 'POST',
        url: controlador + "GenerarFormato",
        data: {
            empresa: $('#cbEmpresa').val(),
            periodo: $('#cbPeriodo').val()
        },
        dataType: 'json',
        cache: false,
        success: function (resultado) {
            if (resultado == 1) {
                location.href = controlador + "DescargarFormato";
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

function subirFormato() {

}

function enviarDatos() {
    $('#txtComentario').val("");
    var validacion = validarDatos();

    if (validacion.length > 0) {
        pintarError(validacion);
    }
    else {
        if ($('#hfPlazoEnvio').val() == "S") {
            $('#popupEnvio').bPopup({});
        }
        else {
            grabarInterrupciones();
        }
    }
}

function grabarInterrupciones(){
    var flag = true;
    if ($('#hfPlazoEnvio').val() == "S" && $('#txtComentario').val() == "") {
        flag = false;
        mostrarMensaje("mensajeComentario", "alert", "Ingrese motivo del envío de datos fuera de plazo.");
    }
    if (flag) {
        $.ajax({
            type: 'POST',
            url: controlador + 'GrabarInterrupciones',
            contentType: 'application/json',
            data: JSON.stringify({
                data: getData(),
                empresa: $('#cbEmpresa').val(),
                periodo: $('#cbPeriodo').val(),
                comentario: $('#txtComentario').val()
            }),
            dataType: 'json',
            success: function (result) {
                $('#popupEnvio').bPopup().close();
                if (result.Result == 1) {                   
                    consultar();
                    mostrarMensaje('mensaje', 'exito', 'Los datos fueron grabados correctamente.');
                }
                else if (result.Result == 2) {
                   
                    var html = mostrarErroresCalculo(result.Data);
                    $('#contenidoError').html(html);
                    $('#popupErrores').bPopup({});
                }
                else
                    mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            }
        });
    }
}

function mostrarErrores() {
    mostrarMensajeDefecto();
    var validacion = validarDatos();
        
    if (validacion.length > 0) {
        mostrarMensaje('mensaje', 'alert', 'Se encontraron errores en los datos ingresados');
        pintarError(validacion);
    }
    else {
        mostrarMensaje('mensaje', 'exito', 'No se encontraron errores en los datos ingresados');
    }
}

function pintarError(validaciones) {
    $('#contenidoError').html(obtenerErroresInterrupciones(validaciones));
    $('#popupErrores').bPopup({});
}

function verEnvios() {
    mostrarMensajeDefecto();
    $.ajax({
        type: 'POST',
        url: controlador + 'Envios',
        data: {
            idEmpresa: $('#cbEmpresa').val(),
            idPeriodo: $('#cbPeriodo').val()
        },
        dataType: 'json',
        global:false,
        success: function (result) {
            var html = obtenerHtmlEnvios(result);
            $('#contenidoEnvios').html(html);
            $('#popupEnvios').bPopup({});
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        }
    });
}

function agregarFila() {
    agregarFilaGrilla();
}

function crearPupload() {
    uploader = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: 'btnSubirFormato',
        url: controlador + 'Upload',
        flash_swf_url: 'Scripts/Moxie.swf',
        silverlight_xap_url: 'Scripts/Moxie.xap',
        multi_selection: false,
        filters: {
            max_file_size: '5mb',
            mime_types: [
                { title: "Archivos Macros Excel .xlsm", extensions: "xlsm" }
            ]
        },
        init: {
            FilesAdded: function (up, files) {
                if (uploader.files.length == 2) {
                    uploader.removeFile(uploader.files[0]);
                }
                uploader.start();
                up.refresh();
            },
            UploadProgress: function (up, file) {
                mostrarMensaje('mensaje', 'alert', "Por favor espere ...(<strong>" + file.percent + "%</strong>)");
            },
            UploadComplete: function (up, file) {
                mostrarMensaje('mensaje', 'alert', "Subida completada. <strong>Por favor espere...</strong>");
                procesarArchivo();

            },
            Error: function (up, err) {
                mostrarMensaje('mensaje', 'error', err.code + "-" + err.message);
            }
        }
    });
    uploader.init();
}

function procesarArchivo() {
    $.ajax({
        type: 'POST',
        url: controlador + 'ImportarInterrupciones',        
        dataType: 'json',
        data: {
            empresa: $('#cbEmpresa').val(),
            periodo: $('#cbPeriodo').val()
        },
        success: function (result) {
            if (result.Result == 1) {
                mostrarMensaje('mensaje', 'exito', 'Los datos se cargaron correctamente en el Excel web, presione el botón <strong>Enviar</strong> para grabar.');
                actualizarDataGrilla(result.Data);                
            }
            else if (result.Result == 2) {
                var html = "No se realizó la carga por que se encontraron los siguientes errores: <ul>";
                for (var i in result.Errores) {
                    html = html + "<li>" + result.Errores[i] + "</li>";
                }
                html = html + "</ul>";
                mostrarMensaje('mensaje', 'alert', html);               
            }
            else if (result.Result == -1) {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            }
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        }
    });
}

function anularInterrupcion(id, row) {
    $('#hfIdInterrupcion').val(id);
    limpiarMensaje('mensajeAnulacion');
    $('#hfRowAnular').val(row);
    $('#txtComentarioAnulacion').val("");
    $('#popupAnulacion').bPopup({});
}

function confirmarAnulacion() {
    console.log($('#hfIdInterrupcion').val());
    if ($('#txtComentarioAnulacion').val() != "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'AnularInterrupcion',
            data: {
                idPeriodo: $('#cbPeriodo').val(),
                idInterrupcion: $('#hfIdInterrupcion').val(),
                comentario: $('#txtComentarioAnulacion').val()
            },
            dataType: 'json',
            success: function (result) {
                if (result == 1) {
                    
                    //eliminarFilaGrilla($('#hfRowAnular').val());
                    $('#popupAnulacion').bPopup().close();                  
                    consultar();
                    mostrarMensaje('mensaje', 'exito', 'El registro se eliminó correctamente.');
                }
                else if (result == 2) {
                    mostrarMensaje('mensajeAnulacion', 'alert', 'Fuera de plazo - No se puede anular interrupción.');
                }
                else {
                    mostrarMensaje('mensajeAnulacion', 'error', 'Se ha producido un error.');
                }
            },
            error: function () {
                mostrarMensaje('mensajeAnulacion', 'error', 'Se ha producido un error.');
            }
        });
    }
    else {
        mostrarMensaje('mensajeAnulacion', 'alert', 'Ingrese el motivo de anulación.');
    }
}

function showFile(id, row, col, indicador) {
    console.log(indicador);
   
    $('#fileId').val(id);    
    $('#fileRow').val(row);
    $('#fileColumn').val(col);
    $('#fileIndicador').val(indicador);
    $('#accionesArchivo').hide();   
    $('#fileInfoPtoEntrega').html("");
    $('#progresoPtoEntrega').html("");
    limpiarMensaje('fileInfoPtoEntrega');
    limpiarMensaje('progresoPtoEntrega');
    limpiarMensaje('mensajeFileInterrupcion');
    $('#descargarFileInterrupcion').hide();
    $('#eliminarFileInterrrupcion').hide();

    if (uploaderPtoEntrega.files.length == 1) {
        uploaderPtoEntrega.removeFile(uploaderPtoEntrega.files[0]);
    }
    if (indicador != null && indicador != "") {
        $('#accionesArchivo').show();    
        $('#fileIndicador').val(indicador);
        $('#descargarFileInterrupcion').show();
        $('#eliminarFileInterrrupcion').show();
    }
    $('#popupArchivo').bPopup({});
}

function descargarFileInterrupcion() {
    location.href = controlador + 'DescargarArchivoInterrupcion?id=' + $('#fileId').val() +
        "&row=" + $('#fileRow').val() + "&extension=" + $('#fileIndicador').val();
}

function eliminarFileInterrupcion() {
    if (confirm('¿Está seguro de realizar esta operación?')) {
        $.ajax({
            type: 'POST',
            url: controlador + 'EliminarArchivoInterrupcion',
            data: {
                id: $('#fileId').val(),
                row: $('#fileRow').val(),
                extension: $('#fileIndicador').val()
            },
            dataType: 'json',
            success: function (result) {
                if (result == 1) {
                    mostrarMensaje('mensajeFileInterrupcion', 'exito', 'El archivo se eliminó correctamente.');
                    $('#descargarFileInterrupcion').hide();
                    $('#eliminarFileInterrrupcion').hide();
                    actualizarFile($('#fileRow').val(), "");
                }
                else {
                    mostrarMensaje('mensajeFileInterrupcion', 'error', 'Se ha producido un error.');
                }
            },
            error: function () {
                mostrarMensaje('mensajeFileInterrupcion', 'error', 'Se ha producido un error.');
            }
        });
    }
}

function cargarGrillaUploader() {
    uploaderPtoEntrega = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: 'btnSelectFilePtoEntrega',
        container: document.getElementById('containerPtoEntrega'),
        url: controlador + 'UploadArchivoInterrupcion',
        flash_swf_url: '~/Content/Scripts/Moxie.swf',
        silverlight_xap_url: '~/Content/Scripts/Moxie.xap',
        multi_selection: false,
        filters: {
            max_file_size: '5mb',
            mime_types: [
                { title: "Imagenes", extensions: "jpg,jpeg,gif,png" },
                { title: "Archivos comprimidos", extensions: "zip,rar" },
                { title: "Documentos", extensions: "pdf,doc,docx,xls,xlsx,ppt,pptx,csv,msg" }
            ]
        },
        init: {
            PostInit: function () {
                document.getElementById('btnProcesarFilePtoEntrega').onclick = function () {
                    if (uploaderPtoEntrega.files.length > 0) {
                        uploaderPtoEntrega.settings.multipart_params = {
                            "id": $('#fileId').val(),
                            "row": $('#fileRow').val()
                        }
                        uploaderPtoEntrega.start();
                    }
                    else
                        loadValidacionFile('fileInfoPtoEntrega', 'Seleccione archivo');
                    return false;
                };
            },
            FilesAdded: function (up, files) {
                if (uploaderPtoEntrega.files.length == 2) {
                    uploaderPtoEntrega.removeFile(uploaderPtoEntrega.files[0]);
                }
                plupload.each(files, function (file) {
                    loadInfoFile('fileInfoPtoEntrega', file.name, plupload.formatSize(file.size));
                });
                uploaderPtoEntrega.refresh();
            },
            UploadProgress: function (up, file) {
                mostrarProgreso('progresoPtoEntrega', file.percent);
            },
            FileUploaded: function (up, file, response) {
                var json = JSON.parse(response.response);
                if (json.indicador == 1) {                   
                    actualizarFile($('#fileRow').val(), json.extension);
                    $('#popupArchivo').bPopup().close();
                    up.removeFile(file);
                }
            },
            UploadComplete: function (up, file) {
                
            },
            Error: function (up, err) {
                if (err.code == "-601") {
                    loadValidacionFile('fileInfoPtoEntrega', "Suba el archivo como un archivo comprimido (.zip o .rar).");
                }
                else {
                    loadValidacionFile('fileInfoPtoEntrega', err.code + "-" + err.message);
                }
            }
        }
    });

    uploaderPtoEntrega.init();
}