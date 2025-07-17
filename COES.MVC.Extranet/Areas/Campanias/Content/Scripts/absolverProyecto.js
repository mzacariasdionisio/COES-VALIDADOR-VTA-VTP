var controlador = siteRoot + 'campanias/plantransmision/';
var controladorRevision = siteRoot + 'campanias/revisionenvio/';
var tipoArchivo = "Observacion";
var tipoArchivoResp = "Respuesta"
var idGrabar = 0;
$(function () {
    var idProyecto = $('#txtPoyCodi').val();
    ObtenerListadoObservacion(idProyecto)
});

function ObtenerListadoObservacion(codigo) {
    $("#listadoObservacion").html('');
    $.ajax({
        type: 'POST',
        url: controlador + 'ListadoObservacion',
        data: {
            id: codigo,
        },
        success: function (eData) {
            $('#listadoObservacion').css("width", $('.form-main').width() + "px");
            $('#listadoObservacion').html(eData);  
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}


consultarObservacion = function (element) {
    let observacion = JSON.parse(element.getAttribute("data-obs"));
    $.ajax({
        type: 'POST',
        url: controlador + 'observacion',
        data: {
            id: 0
        },
        success: function (evt) {
            $('#contenidoObservacion').html(evt);
            setTimeout(function () {
                $('#popupObservacion').modal({
                    escapeClose: false,
                    clickClose: false,
                    showClose: true, closeExisting: false

                });
            }, 50);

            // $('#txtFechaObservacion').Zebra_DatePicker({
            //     format: 'd/m/Y',
            // });
            const today = new Date();
            const formattedDate = today.getDate().toString().padStart(2, '0') + '/' + (today.getMonth() + 1).toString().padStart(2, '0') + '/' + today.getFullYear();
            $('#txtFechaRespuesta').val(formattedDate);
            // $('#txtFechaRespuesta').Zebra_DatePicker({
            //     format: 'd/m/Y'
            // });
            //$('#btnCancelarObservacion').on("click", function () {
            //    $('#popupObservacion').bPopup().close();
            //});
            setObservacion(observacion);
            cargarArchivosObservacion(observacion.ObservacionId, tipoArchivo, '#tablaArchivoObservacion').then(() => {
            });
            cargarArchivosObservacionResp(observacion.ObservacionId, tipoArchivoResp, '#tablaArchivoRespuestaObs').then(() => {
                if(observacion.FechaRespuesta != null){
                    desactivarFormularioObservacion('frmRegistroObservacion');
                }
            });
        },
        error: function () {
            mostrarError('mensaje');
        }
    });
}

setObservacion = function (observacion) {
    var fechaObservacion = convertirFechaObservacion(observacion.FechaObservacion);
    if(observacion.FechaRespuesta != null){
        var fechaRespuesta = convertirFechaObservacion(observacion.FechaRespuesta);
        setFechaPicker("#txtFechaRespuesta", fechaRespuesta, "dd/mm/aaaa");
    }
    
    setFechaPicker("#txtFechaObservacion", fechaObservacion, "dd/mm/aaaa");
    $("#txtObservacion").val(observacion.Descripcion);
    $("#txtRespuestaObservacion").val(observacion.Respuesta);
    observacionId = observacion.ObservacionId;
}

function convertirFechaObservacion(date) {
    var d = new Date(date);
    var day = ('0' + d.getDate()).slice(-2);
    var month = ('0' + (d.getMonth() + 1)).slice(-2);
    var year = d.getFullYear();
    return day + '/' + month + '/' + year;
}


function setFechaPicker(selector, valor, formatoPlaceholder) {
    $(selector).attr("placeholder", formatoPlaceholder); // Establece el placeholder siempre

    if (!valor || valor.trim() === "") {
        $(selector).val(""); // Borra el valor si est� vac�o
    } else {
        $(selector).val(valor); // Asigna el valor si existe
    }
}

async function editarObservacion(element) {
    let observacion = JSON.parse(element.getAttribute("data-obs"));
    $.ajax({
        type: 'POST',
        url: controlador + 'observacion',
        data: {
            id: 0
        },
        success: function (evt) {
            $('#contenidoObservacion').html(evt);
            setTimeout(function () {
                $('#popupObservacion').modal({
                    escapeClose: false,
                    clickClose: false,
                    showClose: true, closeExisting: false

                });
            }, 50);

            // $('#txtFechaObservacion').Zebra_DatePicker({
            //     format: 'd/m/Y',
            // });
            const today = new Date();
            const formattedDate = today.getDate().toString().padStart(2, '0') + '/' + (today.getMonth() + 1).toString().padStart(2, '0') + '/' + today.getFullYear();
            $('#txtFechaRespuesta').val(formattedDate);
            // $('#txtFechaRespuesta').Zebra_DatePicker({
            //     format: 'd/m/Y'
            // });
            $('#btnGrabarObservacion').on("click", function () {
                popupGrabarRespuestaObs(observacion.ObservacionId);
            });
            //$('#btnCancelarObservacion').on("click", function () {
            //    $('#popupObservacion').bPopup().close();
            //});
            setObservacion(observacion);
            cargarArchivosObservacion(observacion.ObservacionId, tipoArchivo, '#tablaArchivoObservacion').then(() => {
            });
            cargarArchivosObservacionResp(observacion.ObservacionId, tipoArchivoResp, '#tablaArchivoRespuestaObs').then(() => {
            });
            crearUploaderObservacion('btnSubirArchivoObservacion', '#tablaArchivoRespuestaObs', tipoArchivoResp);
        },
        error: function () {
            mostrarError('mensaje');
        }
    });
}
$(document).on("click", "#btnCancelarObservacion", function () {
    $.modal.close(); // Cierra el modal si se está utilizando jQuery Modal
});

getRespuestaObservacion = function (id) {
    var param = {};
    param.ObservacionId = id;
    param.FechaRespuesta = $("#txtFechaRespuesta").val();
    param.Respuesta = $("#txtRespuestaObservacion").val();
    return param;
}

function popupGrabarRespuestaObs(id) {
    idGrabar = id
    $('#popupGrabarRespuestaObs').bPopup({
        easing: 'easeOutBack',
        speed: 450,
        transition: 'slideDown'
    });
}

function grabarRespuestaObs() {
    $('#popupGrabarRespuestaObs').bPopup().close();
    var param = {};
    idProyecto = $("#txtPoyCodi").val();
    if(idGrabar != "0"){
        param.respuestaObsDTO = getRespuestaObservacion(idGrabar);
        $.ajax({
            type: 'POST',
            url: controladorRevision + 'GrabarRespuestaObservacion',
            data: JSON.stringify(param),
            contentType: 'application/json',
            success: function (result) {
                if (result.responseResult) {
                    ObtenerListadoObservacion(idProyecto);

                    mostrarMensaje('mensajeFicha','exito', 'Los datos se grabaron correctamente.');
                }
                else {
                    mostrarMensaje('mensajeFicha', 'error', 'Se ha producido un error.', 2);
                }
                $("#btnCancelarObservacion").click(); 
                //$('#popupObservacion').bPopup().close();
                
            },
            error: function () {
                $("#btnCancelarObservacion").click(); 
                //$('#popupObservacion').bPopup().close();
                mostrarMensaje('mensajeFicha', 'error', 'Se ha producido un error.');
            }
        });
    }
}

function cargarArchivosObservacion(id, tipo, idTabla) {
    return new Promise((resolve, reject) => {
        var param = {};
        param.ObservacionId = id;
        param.Tipo = tipo;
        $.ajax({
            type: 'POST',
            url: controladorRevision + 'ListarArchivosObservacion',
            data: param,
            success: function (result) {
                var tabla = $(idTabla);
                var tablaBody = $(idTabla + ' tbody');
                tablaBody.empty();
                var archivosGuardados = result.responseResult;
                for (var i = 0; i < archivosGuardados.length; i++) {
                    var nuevaFila = '<tr id="fila' + archivosGuardados[i].ArchivoId + '">' +
                        '<td><a href="#"  class="btnActive" onclick="descargarFileCampaniaObservacion(\'' + archivosGuardados[i].NombreArchGen + '\')">' + archivosGuardados[i].NombreArch + '</a></td>' +
                        '<td></td>' +
                        '</tr>';
                    tabla.append(nuevaFila);
                }
                resolve();
            },
            error: function () {
                mostrarMensaje('mensajeFicha', 'error', 'Se ha producido un error.');
                reject();
            }
        });
    });

}

function cargarArchivosObservacionResp(id, tipo, idTabla) {
    return new Promise((resolve, reject) => {
        var param = {};
        param.ObservacionId = id;
        param.Tipo = tipo;
        $.ajax({
            type: 'POST',
            url: controladorRevision + 'ListarArchivosObservacion',
            data: param,
            success: function (result) {
                var tabla = $(idTabla);
                var tablaBody = $(idTabla + ' tbody');
                tablaBody.empty();
                var archivosGuardados = result.responseResult;
                for (var i = 0; i < archivosGuardados.length; i++) {
                    var nuevaFila = '<tr id="fila' + archivosGuardados[i].ArchivoId + '">' +
                        '<td><a href="#"  class="btnActive" onclick="descargarFileCampaniaObservacion(\'' + archivosGuardados[i].NombreArchGen + '\')">' + archivosGuardados[i].NombreArch + '</a></td>' +
                        '<td> <a href="#"><img src="' + siteRoot + 'Content/Images/eliminar.png" class="btnEliminar" onclick="eliminarFileObservacion(' + archivosGuardados[i].ArchivoId + ')" title="Eliminar archivo"/></a></td>' +
                        '</tr>';
                    tabla.append(nuevaFila);
                }
                resolve();
            },
            error: function () {
                mostrarMensaje('mensajeFicha', 'error', 'Se ha producido un error.');
                reject();
            }
        });
    });
}

function crearUploaderObservacion(buttonId, tableSelector, tipoName) {
    var filters;
    filters = {
        max_file_size: '100mb',
        mime_types: [
            {
                title: "Archivos permitidos",
                extensions: "doc,pdf,xlsx,csx,dwg,kml,kmz,zip"
            }
        ]
    };
    var uploader = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: buttonId,
        url: controladorRevision + 'UploadArchivoObservacion',
        flash_swf_url: 'Scripts/Moxie.swf',
        silverlight_xap_url: 'Scripts/Moxie.xap',
        multi_selection: false,
        filters: filters,
        init: {
            FilesAdded: function (up, files) {
                var fileName = files[0].name;
                var fileExtension = fileName.split('.').pop();
                param = {};
                param.Tipo = tipoName;
                param.ObservacionId = observacionId;
                param.NombreArch = fileName;

                $.ajax({
                    type: 'POST',
                    url: controladorRevision + 'ValidarArchivo',
                    data: param,
                    success: function (result) {
                        if (result.responseResult.length > 0) {
                            mostrarMensaje('mensajeObservacion', 'error', "El archivo '" + fileName + "' ya fue subido anteriormente en esta tabla.");
                            up.removeFile(files[0]);
                            up.refresh();
                            return;
                        } else {
                            if (files.length > 1) {
                                mostrarMensaje('mensajeObservacion', 'error', "Solo puedes subir un archivo a la vez.");
                                up.splice(1, files.length - 1);
                            }
                            up.start();
                            up.refresh();
                        }
                    },
                    error: function () {
                        mostrarMensaje('mensajeObservacion', 'error', "El archivo '" + fileName + "' ya fue subido anteriormente en esta tabla.");
                    }
                });
            },
            UploadProgress: function (up, file) {
                $("#barraProgreso").css("width", file.percent + "%").text(file.percent + "%");
                mostrarMensaje('mensajeObservacion', 'alert', "Por favor espere ...(<strong>" + file.percent + "%</strong>)");
            },
            FileUploaded: function (up, file, response) {
                var json = JSON.parse(response.response);
                if (json.indicador == 1) {
                    up.removeFile(file);
                    procesarArchivoObservacion(json.fileNameNotPath, json.nombreReal, tableSelector, tipoName, observacionId);
                    mostrarMensaje('mensajeObservacion','exito', "Archivo subido correctamente.");
                } else {
                    mostrarMensaje('mensajeObservacion', 'error', "Error al procesar el archivo.");
                }
            },
            UploadComplete: function (up, file) {
                mostrarMensaje('mensajeObservacion', 'alert', "Subida completada. <strong>Por favor espere...</strong>");
            },
            Error: function (up, err) {
                switch (err.code) {
                    case plupload.FILE_SIZE_ERROR:
                        mostrarMensaje('mensajeObservacion', 'error', "El archivo excede el tamaño máximo permitido de " + filters.max_file_size);
                        break;
                    case plupload.FILE_EXTENSION_ERROR:
                        mostrarMensaje('mensajeObservacion', 'error', "El tipo de archivo no es compatible. Solo se permiten: " + filters.mime_types.map(m => m.extensions).join(', '));
                        break;
                    default:
                        mostrarMensaje('mensajeObservacion', 'error', "Error" + err.code + ": " + err.message);
                }
            }
        }
    });

    uploader.init();
}

function procesarArchivoObservacion(fileNameNotPath, nombreReal, tabla, tipoName, observacionId) {
    param = {};
    param.Tipo = tipoName;
    param.ObservacionId = observacionId;
    param.NombreArch = nombreReal;
    param.NombreArchGen = fileNameNotPath;

    $.ajax({
        type: 'POST',
        url: controladorRevision + 'GrabarRegistroArchivo',
        data: param,
        success: function (result) {
            if (result.responseResult > 0) {
                mostrarMensaje('mensajeObservacion', 'exito', 'Archivo registrado correctamente.');
                agregarFilaTablaObservacion(nombreReal, result.id, tabla, fileNameNotPath);
            }
            else {
                mostrarMensaje('mensajeObservacion', 'error', 'Se ha producido un error.');
            }
        },
        error: function () {
            mostrarMensaje('mensajeObservacion', 'error', 'Se ha producido un error.');
        }
    });
}

function agregarFilaTablaObservacion(nombre, id, tabla, archivoNombre) {
    var nuevaFila = '<tr id="fila' + id + '">' +
        '<td><a href="#" class="btnActive" onclick="descargarFileCampaniaObservacion(\'' + archivoNombre + '\')">' + nombre + '</a></td>' +
        '<td> <a href="#"><img src="' + siteRoot + 'Content/Images/eliminar.png" class="btnEliminar" onclick="eliminarFileObservacion(' + id + ')" title="Eliminar archivo"/></a></td>' +
        '</tr>';
    var tabla = $(tabla);
    tabla.append(nuevaFila);
}

function desactivarFormularioObservacion(formu){
    var formulario = document.getElementById(formu);
    var elementos = formulario.querySelectorAll('input, button, textarea, select, a');

    elementos.forEach(function (elemento) {
        if (elemento.tagName === 'INPUT') {
            if (['text', 'email', 'password', 'number'].includes(elemento.type)) {
                elemento.readOnly = true;
            } else {
                if (!elemento.classList.contains('btnActive')) {
                    elemento.disabled = true;
                    elemento.style.display = 'none';
                } 
            }
            // Deshabilitar Zebra_DatePicker
            if (elemento.classList.contains('Zebra_DatePicker_Icon') || elemento.parentElement.querySelector('.Zebra_DatePicker_Icon')) {
                elemento.disabled = true;
                elemento.nextElementSibling?.classList.add('Zebra_DatePicker_Icon_Disabled');
            }
        } else if (['TEXTAREA', 'SELECT'].includes(elemento.tagName)) {
            elemento.disabled = true;
        } else if (elemento.tagName === 'BUTTON') {
            if (!elemento.classList.contains('btnActive')) {
                elemento.disabled = true;
                elemento.style.display = 'none';
            }
        } else if (elemento.tagName === 'A') {
            if (!elemento.classList.contains('btnActive')) {
                elemento.addEventListener('click', function (event) {
                    event.preventDefault();
                });
                elemento.style.pointerEvents = 'none'; // Desactiva el click visualmente
                elemento.style.display = 'none';
            }
        }
    });
}

function eliminarFileObservacion(id){
    document.getElementById("contenidoPopupObs").innerHTML = '¿Esta seguro de realizar esta operacion?';
    $('#popupProyectoObservacion').bPopup({
        easing: 'easeOutBack',
        speed: 450,
        transition: 'slideDown'
    });
    $('#btnConfirmarPopupObs').off('click').on('click', function() {
        $.ajax({
            type: "POST",
            url: controladorRevision + "EliminarFileObservacion",
            data: {
                id: id,
            },
            dataType: "json",
            success: function (result) {
                if (result == 1) {
                    $("#fila" + id).remove();
                    mostrarMensaje(
                        "mensajeObservacion",
                        "exito",
                        "El archivo se elimino correctamente."
                    );
                } else {
                    mostrarMensaje("mensajeObservacion", "error", "Se ha producido un error.");
                }
            },
            error: function () {
                mostrarMensaje("mensajeObservacion", "error", "Se ha producido un error.");
            },
        });
        popupClose('popupProyectoObservacion');
    });
}

function descargarFileCampaniaObservacion(nombre) {
    location.href = controlador + 'DescargarArchivoCampaniaObservacion?nombre=' + nombre;
}

function EnviarRespuesta(){
    $('#popupEnviarRespuestaObs').bPopup({
        easing: 'easeOutBack',
        speed: 450,
        transition: 'slideDown'
    });
    $('#btnEnviarRespuesta').off('click').on('click', function() {
        idProyecto = $("#txtPoyCodi").val();
        $.ajax({
            type: 'POST',
            url: controladorRevision + 'EnviarRespuesta',
            data: {
                idProyecto: idProyecto,
            },
            success: function (result) {
                if (result == 1) {
                    console.log('enviar observacion');
                    ObtenerListadoObservacion(idProyecto);
                    mostrarMensaje('mensajeFicha', 'exito', 'Se enviaron correctamente las respuestas del proyecto.');
                }
                else {
                    alert("Ha ocurrido un error al enviar respuestas");
                }
    
            },
            error: function (err) {
                alert("Ha ocurrido un error");
            }
        });
        popupClose('popupEnviarRespuestaObs');
    });
}

function mostrarMensaje(id, tipo, mensaje) {
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);
}
