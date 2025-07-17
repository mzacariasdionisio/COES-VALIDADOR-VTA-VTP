var controlador = siteRoot + 'regionesdeseguridad/regionseguridad/';
var OPCION_VER = 1;
var OPCION_EDITAR = 2;
var OPCION_NUEVO = 3;

$(function () {

    var estado =  $('#cboEstado').find('option:selected').val()
    $('#btnConsultar').on('click', function () {
        listadoRegion(estado);
    });

    $('#cboEstado').on('change', function () {
        var value = $(this).find('option:selected').val();
        listadoRegion(value);
    });


    $('#btnNuevo').on('click', function () {
        nuevaRegion();
    });

    listadoRegion(estado);
    crearPupload();
});

listadoRegion = function (estado) {
    $.ajax({
        type: 'POST',
        url: controlador + 'RegionListado',
        data: {
            estado : estado
        },
        success: function (evt) {
            $('#listadoRegion').html(evt);
            $('#tablaRegion').dataTable({
                "iDisplayLength": 25
            });
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
};


function nuevaRegion() {
    formularioRegion(0, OPCION_NUEVO);
}

function editarRegion(id) {
    formularioRegion(id, OPCION_EDITAR);
}

function formularioRegion(id, opcion) {
    $.ajax({
        type: 'POST',
        url: controlador + "RegionObjeto",
        dataType: "json",
        data: {
            id: id
        },
        success: function (obj) {

            $("#hfCodigoRegion").val(id);
            $("#nombRegion").val(obj.SegRegion.Regnombre);

            $("#btnGrabar").unbind();
            $('#btnGrabar').click(function () {
                guardarRegion();
            });

            $("#btnCancelar").unbind();
            $('#btnCancelar').on("click", function () {
                $('#popupRegion').bPopup().close();
            });

            setTimeout(function () {
                $('#popupRegion').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown'
                });
            }, 50);

        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

function guardarRegion() {
    var entity = getObjetoJsonRegion();
    if (confirm('¿Desea guardar la Región?')) {
        var msj = validarRegion(entity);

        if (msj == "") {
            $.ajax({
                type: 'POST',
                dataType: 'json',
                traditional: true,
                url: controlador + 'RegionGuardar',
                data: {
                    id: entity.id,
                    nombre: entity.Nombre,
                },
                cache: false,
                success: function (result) {
                    if (result.Resultado == '-1') {
                        alert('Ha ocurrido un error: ' + result.Mensaje);
                    } else {
                        alert("Se guardó correctamente la Región");
                        $('#popupRegion').bPopup().close();
                        var estado = $('#cboEstado').val();
                        listadoRegion(estado);
                    }
                },
                error: function (err) {
                    alert('Ha ocurrido un error.');
                }
            });
        } else {
            alert(msj);
        }
    }
}

function getObjetoJsonRegion() {
    var obj = {};
    obj.id = parseInt($("#hfCodigoRegion").val()) || 0;
    obj.Nombre = $("#nombRegion").val();

    return obj;
}

function validarRegion(obj) {
    var msj = "";

    if (obj.Nombre == null || obj.Nombre.trim() == "") {
        msj += "Debe ingresar nombre." + "\n";
    }

    return msj;
}

function eliminarRegion(id,estado) {

    if (confirm('¿Desea cambiar el estado de la Región?')) {
        $.ajax({
            type: 'POST',
            dataType: 'json',
            traditional: true,
            url: controlador + 'RegionEliminar',
            data: {
                idRegion: id,
                estado: estado
            },
            cache: false,
            success: function (result) {
                if (result.Resultado == '-1') {
                    alert('Ha ocurrido un error:' + result.Mensaje);
                } else {
                    alert("Se cambio el estado correctamente");
                    var estado = $('#cboEstado').val();
                    listadoRegion(estado);
                }
            },
            error: function (err) {
                alert('Ha ocurrido un error.');
            }
        });
    }
}

function verRestriccion(regCodi, idtipo) {
    location.href = controlador + "RestriccionesIndex?regCodi=" + regCodi + "&idtipo=" + idtipo;
};

configuracionRegionSeguridad = function (id, tipo) {
    
    $.ajax({
        type: 'POST',
        url: controlador + 'enlaceregionseguridadindex',
        data: {
            idRegion: id,
            idTipo: tipo
        },
        global: false,
        success: function (evt) {
            $('#contenidoRegionSeguridad').html(evt);
            setTimeout(function () {
                $('#popupRegionSeguridad').bPopup({
                    autoClose: false
                });
            }, 50);

            enlaceRegionSeguridadList(id,tipo);

            $('#btnAgregarEnlaceRegionSeguridad').on('click', function () {
                enlaceRegionSeguridadSave();
                enlaceRegionSeguridadList(id, tipo);
            });

            $('#btnCancelarEnlaceRegionSeguridad').on('click', function () {
                $('#popupRegionSeguridad').bPopup().close();
            });

            $('#cbTipoRegionSeguridad').on('change', function () {
                cargarEquiposRegionSeguridad();
            });

            $('#hfCodigoRegionSeguridad').val(id);
            $('#hfTipoRegionSeguridad').val(tipo);
        },
        error: function () {
            mostrarMensaje('mensajeRegionSeguridad', 'error', 'Se ha producido un error');
        }
    });
};

enlaceRegionSeguridadList = function (idRegion,idTipo) {
    $.ajax({
        type: 'POST',
        url: controlador + 'enlaceregionseguridadlist',
        data: {
            idRegion: idRegion,
            idTipo : idTipo
        },
        global: false,
        success: function (evt) {
            $('#contentEnlaceRegionSeguridad').html(evt);
            $('#tablaEnlaceRegionSeguridad').dataTable({
                //"sDom": 't',
            });
        },
        error: function () {
            mostrarMensaje('mensajeEnlaceRegionSeguridad', 'error', 'Se ha producido un error');
        }
    });
};

cargarEquiposRegionSeguridad = function () {
    $('#cbEnlaceRegionSeguridad').get(0).options.length = 0;

    if ($('#cbTipoRegionSeguridad').val() != "") {

        mostrarMensaje('mensajeEnlaceRegionSeguridad', 'message', 'Complete los datos por favor.');
        $.ajax({
            type: 'POST',
            url: controlador + 'obtenerequiposregionseguridad',
            data: {
                tipo: $('#cbTipoRegionSeguridad').val()
            },
            dataType: 'json',
            global: false,
            success: function (result) {
                if (result != -1) {

                    $('#cbEnlaceRegionSeguridad').get(0).options[0] = new Option("--SELECCIONE--", "0");
                    $.each(result, function (i, item) {
                        $('#cbEnlaceRegionSeguridad').get(0).options[$('#cbEnlaceRegionSeguridad').get(0).options.length] = new Option(item.Nombretna, item.Equicodi);
                    });
                }
                else {
                    mostrarMensaje('mensajeEnlaceRegionSeguridad', 'error', 'Se ha producido un error.');
                }
            },
            error: function () {
                mostrarMensaje('mensajeEnlaceRegionSeguridad', 'error', 'Se ha producido un error.');
            }
        });
    }
    else {
        mostrarMensaje('mensajeEnlaceRegionSeguridad', 'alert', 'Seleccione tipo de equipo.');
    }
};

enlaceRegionSeguridadSave = function () {

    if ($('#cbEnlaceRegionSeguridad').val() != "") {
        var idEquipo = $('#cbEnlaceRegionSeguridad').val();
        var idRegion = $('#hfCodigoRegionSeguridad').val();
        var idTipoRegion = $('#hfTipoRegionSeguridad').val();
        var tipo = $('#cbTipoRegionSeguridad').val();
        $.ajax({
            type: 'POST',
            url: controlador + 'enlaceregionseguridadsave',
            data: {
                idRegion: idRegion,
                idEquipo: idEquipo,
                tipoRegion: idTipoRegion,
                tipo: tipo
            },
            dataType: 'json',
            global: false,
            success: function (result) {

                if (result == 1) {
                    mostrarMensaje('mensajeEnlaceRegionSeguridad', 'exito', 'La operación se realizó correctamente.');
                    enlaceRegionSeguridadList(idRegion, idTipoRegion);
                }
                else if (result == 2) {
                    mostrarMensaje('mensajeEnlaceRegionSeguridad', 'alert', 'El equipo ya se encuentra agregado.');
                }
                else {
                    mostrarMensaje('mensajeEnlaceRegionSeguridad', 'error', 'Se ha producido un error');
                }
            },
            error: function () {
                mostrarMensaje('mensajeEnlaceRegionSeguridad', 'error', 'Se ha producido un error');
            }
        });
    }
    else {
        mostrarMensaje('mensajeEnlaceRegionSeguridad', 'alert', 'Por favor seleccione un equipo.');
    }
};

enlaceRegionSeguridadDelete = function (idEquipo, tipo) {
    if (confirm('¿Está seguro de eliminar este registro?')) {
        var idRegion = $('#hfCodigoRegionSeguridad').val();
        var tipoRegion = $('#hfTipoRegionSeguridad').val();
        $.ajax({
            type: 'POST',
            url: controlador + 'enlaceregionseguridaddelete',
            data: {
                idRegion: idRegion,
                idEquipo: idEquipo,
                tipoRegion: tipoRegion,
                tipo: tipo
            },
            dataType: 'json',
            global: false,
            success: function (result) {
                if (result == 1) {
                    mostrarMensaje('mensajeEnlaceRegionSeguridad', 'exito', 'La operación se realizó correctamente.');
                    enlaceRegionSeguridadList(idRegion, tipoRegion);
                }
                else {
                    mostrarMensaje('mensajeEnlaceRegionSeguridad', 'error', 'Se ha producido un error');
                }
            },
            error: function () {
                mostrarMensaje('mensajeEnlaceRegionSeguridad', 'error', 'Se ha producido un error');
            }
        });
    }
};


function leerFileUpExcel() {
    var retorno = 0;

    $.ajax({
        type: 'POST',
        url: controlador + 'LeerFileUpExcel',
        dataType: 'json',
        async: false,
        success: function (res) {
            retorno = res;
        },
        error: function () {
            mostrarError("Ha ocurrido un error");
        }
    });
    return retorno;
}
function crearPupload() {
    uploader = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: 'btnSelectExcel',
        url: siteRoot + 'regionesdeseguridad/RegionSeguridad/Upload',
        flash_swf_url: 'Scripts/Moxie.swf',
        silverlight_xap_url: 'Scripts/Moxie.xap',
        multi_selection: false,
        filters: {
            max_file_size: '5mb',
            mime_types: [
                { title: "Archivos Excel .xlsx", extensions: "xlsx,xls" }
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
               // mostrarAlerta("Por favor espere ...(<strong>" + file.percent + "%</strong>)");
            },
            UploadComplete: function (up, file) {
                //mostrarAlerta("Subida completada <strong>Por favor espere</strong>");
                var retorno = leerFileUpExcel();
                if (retorno == 1) {

                    alert("Datos Enviados Correctamente");
                    var estado = $('#cboEstado').find('option:selected').val()
                    listadoRegion(estado);
                }
                else {
                    alert("Error");
                }

            },
            Error: function (up, err) {
                mostrarError(err.code + "-" + err.message);
            }
        }
    });
    uploader.init();
}

function mostrarError(mensaje) {
    alert(mensaje);
}



