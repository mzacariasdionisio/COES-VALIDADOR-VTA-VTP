var controlador = siteRoot + 'admin/versionmodplan/';
var uploaderModelo;
var uploaderManual;

$(function () {
    $('#tab-container').easytabs({
        animate: false
    });

    $('#btnAgregarPlan').on('click', function () {
        agregarVersion(0, 0);
    });

    $('#btnAgregarVersion').on('click', function () {
        agregarVersion(0, 1);
    });

    listarVersionModplan();   
});

agregarVersion = function (id, tipo) {
    $.ajax({
        type: 'POST',
        url: controlador + 'editar',
        data: {
            id: id,
            tipo: tipo
        },
        success: function (evt) {
            $('#contenidoEdicion').html(evt);

            setTimeout(function () {
                $('#popupEdicion').bPopup({
                    autoClose: false
                });
            }, 50);

            $('#btnGrabar').on('click', function () {
                grabar();
            });

            $('#btnCancelar').on('click', function () {
                $('#popupEdicion').bPopup().close();
            });

            $('#cbListaPadre').val($('#hfCodigoPadre').val());

            $('#hfTipo').val(tipo);

            if (tipo == 0) {
                $('#trPlanTransmision').hide();
            }
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        }
    });
};

grabar = function () {
    var validacion = validarDatos();

    if (validacion == "") {

        $.ajax({
            type: 'POST',
            url: controlador + 'grabar',
            data: $('#frmRegistro').serialize(),
            dataType: 'json',
            success: function (result) {
                $('#popupEdicion').bPopup().close();
                listarVersionModplan();
                mostrarMensaje('mensaje', 'exito', 'Operación registrada correctamente.');
            },
            error: function () {
                mostrarMensaje('mensajeEdicion', 'error', 'Se ha producido un error.');
            }        
        });
    }
    else {
        mostrarMensaje('mensajeEdicion', 'alert', validacion);
    }
};

validarDatos = function () {
    var tipo = $('#hfTipo').val();
    var flag = true;
    var html = "<ul>";
    if (tipo == 1) {
        if ($('#cbListaPadre').val() == "") {
            flag = false;
            html = html + "<li>Seleccione Plan de Transmisión.</li>";
        }
    }

    if ($('#txtDescripcion').val() == "") {
        flag = false;
        html = html + "<li>Ingrese una descripción.</li>";
    }
    html = html + "</ul>";

    if (flag == true) html = "";

    return html;
};

listarVersionModplan = function () {
    $.ajax({
        type: 'POST',
        url: controlador + 'listado',
        success: function (evt) {
            $('#listadoPlanTransmision').html(evt);

            $.contextMenu({
                selector: '.version-mod',
                callback: function (key, options) {
                    var id = $(this).attr('id');
                    var tipo = $(this).attr('data-tipo');
                  
                    if (key == "editar") {
                        agregarVersion(id, tipo);
                    }
                    if (key == "delete") {
                        eliminar(id, tipo);
                    }
                },
                items: {
                    "editar": { name: "Editar", icon: "editar" },
                    "delete": { name: "Eliminar", icon: "eliminar" }
                }
            });

            $('.version-hijo').on('click', function () {
                var id = $(this).attr('id');
                cargarDetalleVersion(id);
                cargarReporteVersion(id);
            });
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        }        
    });
};

cargarDetalleVersion = function (id) {

    $.ajax({
        type: 'POST',
        url: controlador + 'detalle',
        data: {
            id: id
        },
        success: function (evt) {
            $('#divDetalle').html(evt);

            $('.resumen-file').on('click', function () {
                descargarArchivo(id, $(this).attr('id'));
            });

            $('.delete-file').on('click', function () {
                eliminarArchivo(id, $(this).attr('data-id'));
            });

            uploaderModelo = configurarUpload(1, 'btnAddModelo', 'containerModelo', 'filelistModelo', 'loadingcargaModelo', false);
            uploaderManual = configurarUpload(2, 'btnAddManual', 'containerManual', 'filelistManual', 'loadingcargaManual', false);
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        }
    });
};

descargarArchivo = function (id, indicador) {

    var data = {
        idVersion: id,
        indicador: indicador
    };
    redirect(controlador + 'descargararchivo', data);
};

eliminarArchivo = function (id, indicador) {
    if (confirm('¿Está seguro de realizar esta acción?')) {
        $.ajax({
            type: 'POST',
            url: controlador + 'eliminararchivo',
            data: {
                idVersion: id,
                indicador: indicador
            },
            dataType: 'json',
            success: function (result) {
                if (result == 1) {
                    cargarDetalleVersion(id);
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
};

redirect = function (url, params) {
    var $form = $("<form />");
    $form.attr("action", url);
    $form.attr("method", 'GET');
    for (var data in params)
        $form.append('<input type="hidden" name="' + data + '" value="' + params[data] + '" />');
    $("body").append($form);
    $form.submit();
};

cargarReporteVersion = function (id) {
    $.ajax({
        type: 'POST',
        url: controlador + 'reporte',
        data: {
            id: id
        },
        success: function (evt) {
            $('#divReporte').html(evt);            
            $('#tabla').dataTable({
                "iDisplayLength": 50
            });
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        }
    });
};

eliminar = function (id, tipo) {
    if (confirm('¿Está seguro de realizar esta operación?')) {
        $.ajax({
            type: 'POST',
            url: controlador + 'eliminar',
            data: {
                id: id,
                tipo: tipo
            },
            dataType: 'json',
            success: function (result) {
                if (result == 1) {
                    listarVersionModplan();
                    mostrarMensaje('mensaje', 'exito', 'La operación se realizó correctamente.');
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
}

configurarUpload = function (opcion, btnAddFile, container, fileList, loadingCarga, multiselect) {

    var uploader = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: btnAddFile,
        container: document.getElementById(container),
        url: controlador + 'upload',
        flash_swf_url: '~/Content/Scripts/Moxie.swf',
        silverlight_xap_url: '~/Content/Scripts/Moxie.xap',
        multi_selection: multiselect,
        chunk_size: '5mb',
        filters: {
            max_file_size: '50mb',
            mime_types: [
                { title: "Imagenes", extensions: "jpg,jpeg,gif,png" },
                { title: "Archivos comprimidos", extensions: "zip,rar" },
                { title: "Documentos", extensions: "pdf,doc,docx,xls,xlsx,ppt,pptx,csv" }
            ]
        },
        init: {
            PostInit: function () {
                $('#' + fileList).html('');
                $('#' + container).css('display', 'none');
            },
            FilesAdded: function (up, files) {
                $('#' + fileList).html('');
                $('#' + container).css('display', 'block');

                for (i = 0; i < uploader.files.length; i++) {
                    var file = uploader.files[i];

                    $('#' + fileList).html($('#' + fileList).html() + '<div class="file-name" id="' + file.id + '">' + file.name +
                        ' (' + plupload.formatSize(file.size) + ') <a class="remove-item" href="JavaScript:eliminarFile(' +
                        opcion + ',\'' + file.id + '\');">X</a> <b></b></div>');
                }

                uploader.settings.multipart_params = {
                    "idVersion": $('#hfCodigoVersion').val(),
                    "indicador": opcion
                };  

                uploader.start();
            },
            UploadProgress: function (up, file) {
                document.getElementById(file.id).getElementsByTagName('b')[0].innerHTML = '<span>' + file.percent + "%</span>";
                $('#' + file.id).css('background-color', '#FFFFB7');
            },
            UploadComplete: function (up, file) {
                $('#' + loadingCarga).css('display', 'none');
                cargarDetalleVersion($('#hfCodigoVersion').val());
            },
            Error: function (up, err) {
                $('#' + container).css('display', 'none');
                $('#' + fileList).html('<div class="action-alert">' + err.message + '</div>');
            }
        }
    });
    uploader.init();

    eliminarFile = function (opcion, id) {
        uploader.removeFile(id);
        $('#' + id).remove();
    };

    return uploader;
};

mostrarMensaje = function (id, tipo, mensaje) {
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);
};