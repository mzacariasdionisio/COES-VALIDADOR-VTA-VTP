var controlador = siteRoot + 'costooportunidad/datossp7/';

$(function () {

    $('#btnConsultar').on('click', function () {       
        consultar();
    });

    $('#txtFechaInicio').Zebra_DatePicker({
    });

    $('#txtFechaFin').Zebra_DatePicker({

    });

    $('#cbUrs').on('change', function () {
        cargarCanales();
    });

    $('#btnSubirArchivo').on('click', function () {
        openExportar();
    });

    cargarUploader();
});

function consultar() {
    console.log($('#cbSenial').val());
    if ($('#cbSenial').val() != null) {

        if ($('#txtFechaInicio').val() != "" && $('#txtFechaFin').val() != "") {

            $.ajax({
                type: 'POST',
                url: controlador + 'consultasenial',
                data: {
                    fechaInicio: $('#txtFechaInicio').val(),
                    fechaFin: $('#txtFechaFin').val(),
                    urs: $('#cbUrs').val(),
                    canalcodi: $('#cbSenial').val(),
                },
                success: function (evt) {
                    $('#listado').html(evt);
                    $('#tabla').dataTable({
                        "iDisplayLength": 50
                    });
                },
                error: function () {
                    mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
                }
            });
        }
        else {
            mostrarMensaje('mensaje', 'alert', 'Seleccione todos los filtros.');
        }
    }
    else {
        mostrarMensaje('mensaje', 'alert', 'Seleccione todos los filtros.');
    }
}

function cargarCanales() {
    $('#cbSenial').get(0).options.length = 0;
    $('#cbSenial').get(0).options[0] = new Option("--TODOS--", "0");

    if ($('#cbUrs').val() != "0") {
        $.ajax({
            type: 'POST',
            url: controlador + 'obtenerseniales',
            dataType: 'json',
            data: {
                idUrs: $('#cbUrs').val()
            },
            cache: false,
            success: function (result) {
                $.each(result, function (i, item) {
                    $('#cbSenial').get(0).options[$('#cbSenial').get(0).options.length] = new Option(item.Canalnomb, item.Canalcodi);
                });
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
            }
        });
    }
}

function openExportar() {
    $('#mensaje').html("");
    $('#mensaje').removeClass();
    $('#progreso').html('');
    $('#fileInfo').html('');
    $('#progreso').removeClass();
    $('#fileInfo').removeClass();
    $('#divExportar').css('display', 'block');
}

function closeExportar() {
    $('#divExportar').css('display', 'none');
}

function cargarUploader() {
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
            mime_types: [
                { title: "Archivos Excel .xlsx", extensions: "xlsx" }
            ]
        },
        init: {
            PostInit: function () {
                document.getElementById('btnProcesarFile').onclick = function () {
                    if (uploader.files.length > 0)                  
                            uploader.start();                    
                    else
                        loadValidacionFile("Por favor seleccione el archivo Excel.");
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
            },
            UploadProgress: function (up, file) {
                mostrarProgreso(file.percent);
            },
            UploadComplete: function (up, file) {
                cargarDatos();
                uploader.removeFile(uploader.files[0]);
            },
            Error: function (up, err) {
                loadValidacionFile(err.code + "-" + err.message);
            }
        }
    });

    uploader.init();
}

function loadInfoFile(fileName, fileSize) {
    $('#fileInfo').html(fileName + " (" + fileSize + ")");
    $('#fileInfo').removeClass('action-alert');
    $('#fileInfo').addClass('action-exito');
    $('#fileInfo').css('margin-bottom', '10px');
}

function loadValidacionFile(mensaje) {
    $('#fileInfo').html(mensaje);
    $('#fileInfo').removeClass('action-exito');
    $('#fileInfo').addClass('action-alert');
    $('#fileInfo').css('margin-bottom', '10px');
}

function mostrarProgreso(porcentaje) {
    $('#progreso').text(porcentaje + "%");
}

function cargarDatos() {
    $.ajax({
        type: 'POST',
        url: controlador + 'CargarSeniales',
        dataType: 'json',      
        cache: false,
        success: function (result) {
            if (result.Result == 1) {
                if (result.Validaciones.length == 0) {
                    mostrarMensaje('mensaje', 'exito', 'Los datos se cargaron correctamente.');
                }
                else {
                    var html = "Por favor revise los siguientes errores: <br />";
                    html = html + "<ul>";

                    for (var i in result.Validaciones) {
                        html = html + "<li>" + result.Validaciones[i] + "</li>";
                    }

                    html = html + "</ul>";
                    mostrarMensaje('mensaje', 'alert', html);
                }

                closeExportar();
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

function mostrarMensaje(id, tipo, mensaje) {
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);
}
