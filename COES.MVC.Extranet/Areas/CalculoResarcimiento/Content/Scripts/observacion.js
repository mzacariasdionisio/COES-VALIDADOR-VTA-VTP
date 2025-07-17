var controlador = siteRoot + 'calculoresarcimiento/observacion/';
var uploader;
var uploaderPtoEntrega;
var uploaderZip;
var arregloArchivos = [];

$(function () {

    $('#txtAnio').Zebra_DatePicker({
        format: 'Y',
        onSelect: function (date) {
            cargarPeriodos(date);
        }
    });

    $('#btnConsultar').on('click', function () {
        consultar(0);
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

    $('#btnSubirArchivos').on('click', function () {
        subirArhivosPrev();
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
    cargarUploaderZip();
});

function consultar(opcion) {
    mostrarMensajeDefecto();
    if ($('#hfIdEmpresa').val() != "") {
        if ($('#cbPeriodo').val() != "") {
            $.ajax({
                type: 'POST',
                url: controlador + 'consultar',
                data: {
                    empresa: $('#hfIdEmpresa').val(),
                    periodo: $('#cbPeriodo').val()
                },
                dataType: 'json',
                success: function (result) {
                    if (result.Result == 1) {
                        cargarGrillaInterrupcion(result);
                        arregloArchivos = [];
                        habilitarEnvio(true);
                        if (result.PlazoObservacion == "N") {
                            deshabilitarBotonesEnvio(true);
                        }
                        else {
                            deshabilitarBotonesEnvio(false);
                        }

                        if (opcion == 1) {
                            mostrarMensaje('mensaje', 'exito', "Los archivos se cargaron correctamente.");
                        }
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
        mostrarMensaje('mensaje', 'alert', 'Su usuario no se encuentra asociado a una empresa.');
    }
}

function deshabilitarBotonesEnvio(flag) {

    if (flag) {
        $('#btnDescargarFormato').hide();
        $('#btnSubirFormato').hide();
        $('#btnEnviarDatos').hide();
        $('#btnSubirArchivos').hide();
        $('#btnMostrarErrores').hide();
        $('#btnOcultarColumna').hide();
        mostrarMensaje('mensaje', 'message', 'Podrá llenar observaciones culminada la etapa 1.');
    }
    else {
        $('#btnDescargarFormato').show();
        $('#btnSubirFormato').show();
        $('#btnEnviarDatos').show();
        $('#btnSubirArchivos').show();
        $('#btnMostrarErrores').show();
        $('#btnOcultarColumna').show();
        mostrarMensaje('mensaje', 'message', 'Complete los datos');
    }
    
}

function cambiarEmpresa() {
    $.ajax({
        type: 'POST',
        url: controlador + 'empresa',
        success: function (evt) {
            $('#contenidoEmpresa').html(evt);
            setTimeout(function () {
                $('#popupEmpresa').bPopup({
                    autoClose: false
                });
            }, 50);

            $('#btnAceptar').on("click", function () {
                seleccionarEmpresa();
            });

            $('#btnCancelar').on("click", function () {
                $('#popupEmpresa').bPopup().close();
            });
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        }
    });
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

function seleccionarEmpresa() {
    if ($('#cbEmpresa').val() != "") {
        $('#hfIdEmpresa').val($('#cbEmpresa').val());
        $('#lbEmpresa').text($("#cbEmpresa option:selected").text());
        $('#popupEmpresa').bPopup().close();
        habilitarEnvio(false);
    }
    else {
        mostrarMensaje('mensajeEdicion', 'alert', 'Debe seleccionar empresa.');
    }
}

function habilitarEnvio(flag) {

    if (!flag) {
        $('#divAcciones').hide();
        $('#ctnBusqueda').hide();
        $('#leyenda').hide();
        $('#detalleFormato').html('');
    }
    else {
        $('#divAcciones').show();
        $('#ctnBusqueda').show();
        $('#leyenda').show();
    }
}

function descargarFormato() {
    $.ajax({
        type: 'POST',
        url: controlador + "GenerarFormato",
        data: {
            empresa: $('#hfIdEmpresa').val(),
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
        if ($('#hfIdEmpresa').val() != "") {
            if ($('#hfPlazoEnvio').val() == "S") {
                $('#popupEnvio').bPopup({});
            }
            else {
                grabarInterrupciones();
            }
        }
        else {
            mostrarMensaje('mensaje', 'alert', 'Debe seleccionar empresa.');
        }
    }
}

function grabarInterrupciones() {
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
                empresa: $('#hfIdEmpresa').val(),
                periodo: $('#cbPeriodo').val(),
                comentario: $('#txtComentario').val()
            }),
            dataType: 'json',
            success: function (result) {
                if (result == 1) {

                    $('#popupEnvio').bPopup().close();
                    consultar(0);
                    mostrarMensaje('mensaje', 'exito', 'Los datos fueron grabados correctamente.');
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
            idEmpresa: $('#hfIdEmpresa').val(),
            idPeriodo: $('#cbPeriodo').val()
        },
        dataType: 'json',
        global: false,
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
            empresa: $('#hfIdEmpresa').val(),
            periodo: $('#cbPeriodo').val()
        },
        success: function (result) {           
            if (result.Result == 1) {               
                mostrarMensaje('mensaje', 'exito', 'Los datos se cargaron correctamente en el Excel web, presione el botón <strong>Enviar</strong> para grabar.');
                actualizarDataGrilla(result.Data);
                arregloArchivos = result.Archivos;
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

function showFile(id, row, col, indicador) {
    console.log("Si llega");
    $('#fileId').val(id);
    $('#fileRow').val(row);
    $('#fileColumn').val(col);
    $('#fileIndicador').val(indicador);
    $('#accionesArchivo').hide();
    $('#fileInfoPtoEntrega').html("");
    $('#progresoPtoEntrega').html("");
    limpiarMensaje('fileInfoPtoEntrega');
    limpiarMensaje('progresoPtoEntrega');
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

function showFileInterrupcion(id, row, col, indicador) {
    location.href = controlador + 'DescargarArchivoEvidencia?id=' + id +
        "&row=" + row + "&extension=" + indicador;
}

function showFileInterrupcionDetalle(id, row, col, indicador) {
   
    location.href = controlador + 'DescargarArchivoInterrupcion?id=' + id +
        "&row=" + row + "&extension=" + indicador;
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

function subirArhivosPrev() {        
    var errores = [];
    console.log(arregloArchivos);

    if (arregloArchivos.length == 0) {
        errores.push({ fila: 0, texto: "Debe importar el formato de carga con los nombres de los archivos de sustento." });
    }
    else {

        for (var i in arregloArchivos) {            
            if (arregloArchivos[i].FileName != null && arregloArchivos[i].FileName != "") {
                var mensajeArchivo = validarNombreArchivo(arregloArchivos[i].FileName);
                if (mensajeArchivo != "") {
                    errores.push({ fila: arregloArchivos[i].Fila, texto: mensajeArchivo });
                }
            }
        }
    }

    if (errores.length == 0) {
        $('#btnCargarZip').click();
    }
    else {
        $('#contenidoErrorArchivo').html(pintarErroresArchivos(errores));
        $('#popupErroresArchivo').bPopup({});
    }
}

function validarNombreArchivo(nombreArchivo) {
    
    const extensionesPermitidas = ['.docx', '.xlsx', '.zip', '.pdf', '.rar'];
   
    if (!nombreArchivo.includes('.')) {
        return "El archivo debe tener una extensión.";
    }
        
    if (/\s/.test(nombreArchivo)) {
        return "El nombre del archivo no debe contener espacios.";
    }
    
    const extension = nombreArchivo.slice(nombreArchivo.lastIndexOf('.')).toLowerCase();
    if (!extensionesPermitidas.includes(extension)) {
        return `La extensión del archivo no es válida. Extensiones permitidas: ${extensionesPermitidas.join(', ')}`;
    }
        
    return "";
}

function cargarUploaderZip() {
    uploaderZip = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: 'btnCargarZip',
        url: controlador + 'UploadZip',
        flash_swf_url: 'Scripts/Moxie.swf',
        silverlight_xap_url: 'Scripts/Moxie.xap',
        multi_selection: false,
        filters: {
            max_file_size: '100mb',
            mime_types: [
                { title: "Archivos comprimidos .zip", extensions: "zip" }
            ]
        },
        init: {
            FilesAdded: async function (up, files) {
                const erroresZip = [];
                const listaArchivos = [];
                const zipPromises = [];

                plupload.each(files, function (file) {
                    const promise = new Promise((resolve, reject) => {
                        const fileReader = new FileReader();

                        fileReader.onload = async function (e) {
                            try {
                                const zipData = e.target.result;
                                const zip = new JSZip();
                                const contents = await zip.loadAsync(zipData);                                
                                if (Object.keys(contents.files).length > 0) {

                                    for (const fileName in contents.files) {
                                        const file = contents.files[fileName];

                                        if (!fileName.includes("/") || fileName.indexOf("/") === fileName.length - 1) {
                                            if (file.dir) {
                                                erroresZip.push({
                                                    fila: 0,
                                                    texto: `El archivo comprimido no puede contener carpetas. Folder: ${fileName}`,
                                                });
                                            } else {
                                                const existeExcel = arregloArchivos.some(a => a.FileName === fileName);
                                                if (existeExcel) {
                                                    const uncompressedSize = file._data.uncompressedSize;
                                                    listaArchivos.push({ name: fileName, size: uncompressedSize });
                                                } else {
                                                    erroresZip.push({
                                                        fila: 0,
                                                        texto: `El archivo ${fileName} encontrado en el comprimido no se encuentra en el archivo Excel de carga.`,
                                                    });
                                                }
                                            }
                                        }
                                    }
                                }
                                else {
                                    erroresZip.push({
                                        fila: 0,
                                        texto: `El archivo comprimido debe contener archivos.`,
                                    });
                                }
                                resolve();
                            } catch (err) {
                                console.error(`Error leyendo el archivo ZIP (${file.name}):`, err);
                                reject(err);
                            }
                        };

                        fileReader.readAsArrayBuffer(file.getNative());
                    });

                    zipPromises.push(promise);
                });

                try {
                    await Promise.all(zipPromises);

                    for (const archivo of arregloArchivos) {
                        if (archivo.FileName) {
                            const archivoEncontrado = listaArchivos.find(a => a.name === archivo.FileName);

                            if (!archivoEncontrado) {
                                erroresZip.push({
                                    fila: archivo.Fila,
                                    texto: `No se encuentra el archivo ${archivo.FileName} en el archivo comprimido.`,
                                });
                            } else if (archivoEncontrado.size / (1024 * 1024) >= 5) {
                                erroresZip.push({
                                    fila: archivo.Fila,
                                    texto: `El archivo ${archivo.FileName} supera el peso máximo permitido de 5MB.`,
                                });
                            }
                        }
                    }

                    if (erroresZip.length === 0) {
                        console.log(arregloArchivos);
                        uploaderZip.settings.multipart_params = {
                            "empresa": $('#hfIdEmpresa').val(),
                            "periodo": $('#cbPeriodo').val(),
                            "archivos": JSON.stringify(arregloArchivos)
                        };
                        uploaderZip.start();
                        up.refresh();
                    } else {
                        $('#contenidoErrorArchivo').html(pintarErroresArchivos(erroresZip));
                        $('#popupErroresArchivo').bPopup({});
                        up.splice();
                    }
                }catch (err) {
                    console.error("Error procesando los archivos ZIP:", err);
                }
                
            },
            UploadProgress: function (up, file) {
                mostrarMensaje('mensaje', 'alert', "Por favor espere ...(<strong>" + file.percent + "%</strong>)");
            },           
            FileUploaded: function (up, file, response) {
                try {
                    // Parsear la respuesta del servidor
                    const result = JSON.parse(response.response);

                    if (result.success) {
                        for (const archivo of arregloArchivos) {
                            if (archivo.FileName) {
                                var extensionActualizar = archivo.FileName.split('.').pop() || '';;
                                actualizarFile((archivo.Fila - 1), extensionActualizar);
                            }
                        }
                        mostrarMensaje('mensaje', 'exito', 'Los archivos se cargaron correctamente, presione el botón <strong>Enviar</strong> para grabar.');

                    } else {
                        mostrarMensaje('mensaje', 'error', result.message);
                    }
                } catch (err) {
                    console.error("Error procesando la respuesta del servidor:", err);
                    mostrarMensaje('mensaje', 'error', "Ocurrió un error inesperado al procesar la respuesta.");
                }
            },
            Error: function (up, err) {
                mostrarMensaje('mensaje', 'error', err.code + "-" + err.message);
            }
        }
    });

    uploaderZip.init();
}

function pintarErroresArchivos(data) {
    var html = "<table class='pretty tabla-adicional' id='tablaErrores'>";
    html = html + " <thead>";
    html = html + "     <tr>";
    html = html + "         <th>Fila</th>";    
    html = html + "         <th>Error</th>";
    html = html + "     </tr>";
    html = html + " </thead>";
    html = html + " <tbody>";
    for (var k in data) {
        html = html + "     <tr>";
        html = html + "         <td>" + ((data[k].fila != 0) ? data[k].fila: '' )+ "</td>";
        html = html + "         <td>" + data[k].texto + "</td>";
        html = html + "     </tr>";
    }
    html = html + " </tbody>";
    html = html + "</table>";

    return html;
}