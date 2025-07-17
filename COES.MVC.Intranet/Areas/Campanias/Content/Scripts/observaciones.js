var controlador = siteRoot + 'campanias/plantransmision/';
var controladorRevision = siteRoot + 'campanias/revisionenvio/';
var idEliminacion = 0;
var idConformidad = 0;
var tipoArchivo = "Observacion";
var tipoArchivoResp = "Respuesta";
var observacionId = 0;
var idProyecto

$(function () {
    idProyecto = $("#txtPoyCodi").val();
    ObtenerListadoObservacion(idProyecto);

    $('#btnCancelarConforme').on('click', function () {
        $('.b-close').trigger('click');
    });

});

function ObtenerListadoObservacion(codigo) {
    $("#listadoObservacion").html('');
    $.ajax({
        type: 'POST',
        url: controladorRevision + 'ListadoObservacion',
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

agregarObservacion = function (id) {
    $.ajax({
        type: 'POST',
        url: controladorRevision + 'observacion',
        data: {
            id: id
        },
        success: function (evt) {
            $('#contenidoObservacion').html(evt);
            $('#FormRespuesta').hide();

                $('#popupObservacion').modal({
                    escapeClose: false,
                    clickClose: false,
                    showClose: true, closeExisting: false
                });
         
            observacionId = 0;
            $('#txtFechaObservacion').val(obtenerFechaActual());
            // $('#txtFechaObservacion').Zebra_DatePicker({
            //     format: 'd/m/Y',
            //     showIcon: false
            // });

            $('#btnGrabarObservacion').on("click", function () {
                grabarObservacion();
            });
             
            crearUploaderObservacion('btnSubirArchivoObservacion', '#tablaArchivoObservacion', tipoArchivo);
        },
        error: function () {
            mostrarMensaje('mensajeFicha', 'error', 'Error no se pudo completar la operación.');
        }
    });
}
$(document).on("click", "#btnCancelarObservacion", function () {
    $.modal.close(); // Cierra el modal si se está utilizando jQuery Modal
});

getObservacion = function () {
    var param = {};
    param.FechaObservacion = $("#txtFechaObservacion").val();
    param.Descripcion = $("#txtObservacion").val();
    return param;
}

setObservacion = function (observacion) {
    var fechaObservacion = convertirFechaObservacion(observacion.FechaObservacion);
    setFechaPicker("#txtFechaObservacion", fechaObservacion, "dd/mm/aaaa");
    $("#txtObservacion").val(observacion.Descripcion);
    if(observacion.Estado != "Enviada"){
        if(observacion.FechaRespuesta != null){
            var fechaRespuesta = convertirFechaObservacion(observacion.FechaRespuesta);
            setFechaPicker("#txtFechaRespuesta", fechaRespuesta, "dd/mm/aaaa");
            $("#txtRespuestaObservacion").val(observacion.Respuesta);
        }  
    }
    observacionId = observacion.ObservacionId;
}

function grabarObservacion() {
  
    var param = {};
    idProyecto = $("#txtPoyCodi").val();
    if (idProyecto != "0") {
        if (observacionId == "0") {
            param.ProyCodi = idProyecto;
            param.ObservDTO = getObservacion();
            $.ajax({
                type: 'POST',
                url: controladorRevision + 'GrabarObservacion',
                data: JSON.stringify(param),
                contentType: 'application/json',
                success: function (result) {
                    if (result.responseResult) {
                        ObtenerListadoObservacion(idProyecto);
                        ObtenerListado();
                        observacionId = result.id;
                        mostrarMensaje('mensajeFicha', 'exito', 'Los datos se grabaron correctamente.');

                        $.modal.close();

                    }
                    else {
                        mostrarMensaje('mensajeFicha', 'error', 'Error no se pudo completar la operación.');
                    }
                },
                error: function () {
                    mostrarMensaje('mensajeFicha', 'error', 'Error no se pudo completar la operación.');
                }
            });
        } else {
            var observacion = {};
            observacion.observacionId;
            editarObservacion(observacion);

        }
    }
}

function popupEliminarObservacion(id) {
    
    idEliminacion = id
    $('#popupEliminarObservacion').bPopup({
        easing: 'easeOutBack',
        speed: 450,
        transition: 'slideDown'
    });
}

function popupConformarObservacion(id) {
    idConformidad = id
    $('#popupConformarObservacion').bPopup({
        easing: 'easeOutBack',
        speed: 450,
        transition: 'slideDown'
    });
}

async function consultarObservacion(observacion) {

    $.ajax({
        type: 'POST',
        url: controladorRevision + 'observacion',
        data: {
            id: 0
        },
        success: function (evt) {
            $('#contenidoObservacion').html(evt);
            if(observacion.FechaRespuesta != null && (observacion.Estado == "Absuelta" || observacion.Estado == "Cerrada")){
                cargarArchivosObservacionResp(observacion.ObservacionId, tipoArchivoResp, '#tablaArchivoRespuestaObs').then(() => {
                    desactivarFormularioObservacion('frmRegistroObservacion');
                });
            }
            else {$('#FormRespuesta').hide();} 
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

            $('#btnGrabarObservacion').on("click", function () {
                editarObservacion(observacion);
            });
            $('#btnCancelarObservacion').on("click", function () {
                // $('#popupObservacion').bPopup().close();
            });
            setObservacion(observacion);
            cargarArchivosObservacion(observacion.ObservacionId, tipoArchivo, '#tablaArchivoObservacion').then(() => {
                if(observacion.FechaRespuesta != null){
                    desactivarFormularioObservacion('frmRegistroObservacion');
                }
            });
            crearUploaderObservacion('btnSubirArchivoObservacion', '#tablaArchivoObservacion', tipoArchivo);
            
        },
        error: function () {
            mostrarMensaje('mensajeFicha', 'error', 'Error no se pudo completar la operación.');
        }
    });
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
                    '<td> <a href="#"><img src="' + siteRoot + 'Content/Images/eliminar.png" class="btnEliminar" onclick="eliminarFileObservacion(' + archivosGuardados[i].ArchivoId + ')" title="Eliminar archivo"/></a></td>' +
                    '</tr>';
                tabla.append(nuevaFila);
            }
            resolve();
        },
        error: function () {
            mostrarMensaje('mensajeFicha', 'error', 'Error no se pudo completar la operación.');
            reject();
        }
    });
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
                    mostrarMensaje("mensajeObservacion", "error", "Error no se pudo completar la operación.");
                }
            },
            error: function () {
                mostrarMensaje("mensajeObservacion", "error", "Error no se pudo completar la operación.");
            },
        });
        popupClose('popupProyectoObservacion');
    });
}

function eliminarObservacion() {
    if (idEliminacion != "0") {
        $.ajax({
            type: 'POST',
            url: controladorRevision + 'EliminarObservacion',
            data: {
                id: idEliminacion,
                proyCodi: idProyecto
            },
            success: function (result) {
                if (result) {
                    idProyecto = $("#txtPoyCodi").val();
                    ObtenerListadoObservacion(idProyecto);
                    ObtenerListado();
                    $(`#popupEliminarObservacion`).bPopup().close();
                    mostrarMensaje('mensajeFicha', 'exito', 'La observacion ha sido eliminado correctamente.')
                }
                else {
                    mostrarMensaje('mensajeFicha', 'error', 'Error no se pudo completar la operación.');
                }
            },
            error: function () {
                mostrarMensaje('mensajeFicha', 'error', 'Error no se pudo completar la operación.');
                $(`#popupEliminarObservacion`).bPopup().close();
            }
        });
    }
    $(`#popupEliminarObservacion`).bPopup().close();
}

function cancelarEliminarObservacion() 
{

    $(`#popupEliminarObservacion`).bPopup().close();
    $(`#popupEliminarObservacion`).bPopup().close();
}
function conformarObservacion() {
    if (idConformidad != "0") {
        $.ajax({
            type: 'POST',
            url: controladorRevision + 'ConformarObservacion',
            data: {
                id: idConformidad
            },
            success: function (result) {
                if (result) {
                    idProyecto = $("#txtPoyCodi").val();
                    ObtenerListadoObservacion(idProyecto)
                    mostrarMensaje('mensajeFicha', 'exito', 'La observacion ha sido cerrada correctamente.')
                }
                else {
                    mostrarMensaje('mensajeFicha', 'error', 'Error no se pudo completar la operación.');
                }
            },
            error: function () {
                mostrarMensaje('mensajeFicha', 'error', 'Error no se pudo completar la operación.');
                $(`#popupConformarObservacion`).bPopup().close();
            }
        });
    }
    $("#btnCancelarConforme").click();
    $(`#popupConformarObservacion`).bPopup().close();
}

function editarObservacion(observacion) {
    if (idProyecto != "0") {
        var param = {};
        observacion.FechaObservacion = $("#txtFechaObservacion").val();
        observacion.Descripcion = $("#txtObservacion").val();
        param.observacionDTO = observacion;
        $.ajax({
            type: 'POST',
            url: controladorRevision + 'EditarObservacion',
            data: JSON.stringify(param),
            contentType: 'application/json',
            success: function (result) {
                if (result) {
                    ObtenerListadoObservacion(idProyecto);
                    mostrarMensaje('mensajeFicha', 'exito', 'Los datos se grabaron correctamente.');
                    $.modal.close();

                }
                else {
                    mostrarMensaje('mensajeFicha', 'error', 'Error no se pudo completar la operación.');
                }
                // $('#popupObservacion').bPopup().close();
            },
            error: function () {
                // $('#popupObservacion').bPopup().close();
                mostrarMensaje('mensajeFicha', 'error', 'Error no se pudo completar la operación.');
            }
        });
    }
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

    if (!archivosCargadosPorTabla[tableSelector]) {
        archivosCargadosPorTabla[tableSelector] = [];
    }

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

                    archivosCargadosPorTabla[tableSelector].push({
                        nombre: file.name,
                        extension: file.name.split('.').pop()
                    });
                    if(observacionId == 0) {
                        var param = {};
                        idProyecto = $("#txtPoyCodi").val();
                        if (idProyecto != "0") {
                            param.ProyCodi = idProyecto;
                            param.ObservDTO = getObservacion();
                            $.ajax({
                                type: 'POST',
                                url: controladorRevision + 'GrabarObservacion',
                                data: JSON.stringify(param),
                                contentType: 'application/json',
                                success: function (result) {
                                    if (result.responseResult) {
                                        ObtenerListadoObservacion(idProyecto);
                                        ObtenerListado();
                                        mostrarMensaje('mensajeFicha', 'exito', 'Los datos se grabaron correctamente.')
                                        observacionId =  result.id;
                                        procesarArchivoObservacion(json.fileNameNotPath, json.nombreReal, tableSelector, tipoName, observacionId);
                                    }
                                    else {
                                        mostrarMensaje('mensajeFicha', 'error', 'Error no se pudo completar la operación.');
                                    }
                                },
                                error: function () {
                                    // $('#popupObservacion').bPopup().close();
                                    mostrarMensaje('mensajeFicha', 'error', 'Error no se pudo completar la operación.');
                                }
                            });
                        }
                    } else {
                        procesarArchivoObservacion(json.fileNameNotPath, json.nombreReal, tableSelector, tipoName, observacionId);
                    }
                    mostrarMensaje('mensajeObservacion', 'exito', "Archivo subido correctamente.");
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
                mostrarMensaje('mensajeObservacion', 'error', 'Error no se pudo completar la operación.');
            }


        },
        error: function () {
            mostrarMensaje('mensajeObservacion', 'error', 'Error no se pudo completar la operación.');
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

function mostrarMensaje(id, tipo, mensaje) {
    $('#' + id).removeClass();
    $('#' + id).attr('class', '');
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);
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
                mostrarMensaje('mensajeFicha', 'error', 'Error no se pudo completar la operación.');
                reject();
            }
        });
    });
}

function setFechaPicker(selector, valor, formatoPlaceholder) {
    $(selector).attr("placeholder", formatoPlaceholder); // Establece el placeholder siempre

    if (!valor || valor.trim() === "") {
        $(selector).val(""); // Borra el valor si est� vac�o
    } else {
        $(selector).val(valor); // Asigna el valor si existe
    }
}


function descargarFileCampaniaObservacion(nombre) {
    location.href = controlador + 'DescargarArchivoCampaniaObservacion?nombre=' + nombre;
}


