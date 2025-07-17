var controlador = siteRoot + 'calculoresarcimiento/ingreso/';
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

    $('#spanCambiarEmpresa').on('click', function () {
        cambiarEmpresa();
    });

    $('#btnEnviarDatos').on('click', function () {
        enviarDatos();
    });

    $('#btnVerEnvios').on('click', function () {
        verEnvios();
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
    cargarGrillaUploader();
});

function consultar() {
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
                    if (result.Existe == 1) {
                        $('#cbMoneda').val(result.Entidad.Reingmoneda);
                        $('#txtIngreso').val(result.Entidad.Reingvalor);
                        $('#hfCodigoIngreso').val(result.Entidad.Reingcodi);
                        cargarMoneda($('#cbMoneda').val());
                        showFile(result.Entidad.Reingcodi, result.Entidad.Reingsustento);
                    }
                    else {
                        $('#cbMoneda').val("S");
                        $('#txtIngreso').val("");
                        $('#hfCodigoIngreso').val("");
                        cargarMoneda($('#cbMoneda').val());
                        showFile(0, "");
                        cargarMoneda("S");
                    }

                    $('#cbMoneda').on('change', function () {
                        cargarMoneda($('#cbMoneda').val());
                    });

                    $('#hfPlazo').val(result.Plazo);
                    habilitarEnvio(true);
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

function cargarMoneda(moneda) {
    if (moneda == "S") {
        $('#spanMoneda').text("S/.");
    }
    else {
        $('#spanMoneda').text("$");
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
        $('#detalleFormato').hide();
    }
    else {
        $('#divAcciones').show();
        $('#ctnBusqueda').show();
        $('#detalleFormato').show();
    }
}

function showFile(id, indicador) {    
    $('#fileId').val(id);    
    $('#fileIndicador').val(indicador);
    $('#accionesArchivo').hide();
    $('#fileInfoPtoEntrega').html("");
    $('#progresoPtoEntrega').html("");
    limpiarMensaje('fileInfoPtoEntrega');
    limpiarMensaje('progresoPtoEntrega');
    limpiarMensaje('mensajeFileInterrupcion');

    if (uploaderPtoEntrega.files.length == 1) {
        uploaderPtoEntrega.removeFile(uploaderPtoEntrega.files[0]);
    }
    if (indicador != null && indicador != "") {
        $('#accionesArchivo').show();
        $('#fileIndicador').val(indicador);
        $('#descargarFileInterrupcion').show();
        $('#eliminarFileInterrrupcion').show();
    }    
}

function descargarFileInterrupcion() {
    location.href = controlador + 'DescargarArchivoIngreso?id=' + $('#fileId').val() + "&extension=" + $('#fileIndicador').val();
}

function eliminarFileInterrupcion() {
    if (confirm('¿Está seguro de realizar esta operación?')) {
        $.ajax({
            type: 'POST',
            url: controlador + 'EliminarArchivoIngreso',
            data: {
                id: $('#fileId').val(),                
                extension: $('#fileIndicador').val()
            },
            dataType: 'json',
            success: function (result) {
                if (result == 1) {
                    mostrarMensaje('mensajeFileInterrupcion', 'exito', 'El archivo se eliminó correctamente.');
                    $('#accionesArchivo').hide();
                    $('#descargarFileInterrupcion').hide();
                    $('#eliminarFileInterrrupcion').hide();                  
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

function enviarDatos() {
    console.log($('#hfPlazo').val());
    if ($('#hfPlazo').val() == "1") {

        var validacion = validarDatos();

        if (validacion == "") {
            grabarIngreso();
        }
        else {
            mostrarMensaje('mensaje', 'alert', validacion);
        }
    }
    else {
        mostrarMensaje('mensaje', 'alert', "No puede realizar el registro porque se encuentra en fuera de plazo.");
    }
}

function validarDatos() {

    var html = "<ul>";
    var flag = true;

    if ($('#txtIngreso').val() == "") {
        html = html + "<li>Ingrese el monto de ingreso por transmisión semestral.</li>";
        flag = false;
    }
    else {
        if (!validarNumero($('#txtIngreso').val())) {
            html = html + "<li>El ingreso por transmisión debe ser un valor numérico.</li>";
            flag = false;
        }
    }
    html = html + "</ul>";

    if (flag) html = "";
    return html;
}

function grabarIngreso() {

    $.ajax({
        type: 'POST',
        url: controlador + 'GrabarIngreso',        
        data: {
            codigo: ($('#hfCodigoIngreso').val() != "") ? $('#hfCodigoIngreso').val(): "0",
            empresa: $('#hfIdEmpresa').val(),
            moneda: $('#cbMoneda').val(),
            ingreso: $('#txtIngreso').val(),
            periodo: $('#cbPeriodo').val(),
            archivo: $('#hfArchivo').val()
        },
        dataType: 'json',
        success: function (result) {
            if (result == 1) {                
                consultar();
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

function cargarGrillaUploader() {
    uploaderPtoEntrega = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: 'btnSelectFilePtoEntrega',
        container: document.getElementById('containerPtoEntrega'),
        url: controlador + 'UploadArchivoIngreso',
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
            FilesAdded: function (up, files) {
                if (uploaderPtoEntrega.files.length == 2) {
                    uploaderPtoEntrega.removeFile(uploaderPtoEntrega.files[0]);
                }
                plupload.each(files, function (file) {
                    loadInfoFile('fileInfoPtoEntrega', file.name, plupload.formatSize(file.size));
                });

                uploaderPtoEntrega.settings.multipart_params = {
                    "id": $('#hfCodigoIngreso').val()
                }
                uploaderPtoEntrega.start();
                uploaderPtoEntrega.refresh();
            },
            UploadProgress: function (up, file) {
                mostrarProgreso('progresoPtoEntrega', file.percent);
            },
            FileUploaded: function (up, file, response) {
                var json = JSON.parse(response.response);
                if (json.indicador == 1) {                    
                    $('#hfArchivo').val(json.extension);
                    up.removeFile(file);
                    showFile($('#hfCodigoIngreso').val(), json.extension);
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

function obtenerHtmlEnvios(result) {
    var html = "<table class='pretty tabla-adicional' id='tablaEnvios'>";
    html = html + " <thead>";
    html = html + "     <tr>";
    html = html + "         <th>ID Envío</th>";
    html = html + "         <th>Usuario</th>";
    html = html + "         <th>Fecha</th>";  
    html = html + "     </tr>";
    html = html + " </thead>";
    html = html + " <tbody>";
    for (var k in result) {
        
        html = html + "     <tr>";
        html = html + "         <td>" + result[k].Reenvcodi + "</td>";
        html = html + "         <td>" + result[k].Reenvusucreacion + "</td>";
        html = html + "         <td>" + result[k].ReenvfechaDesc + "</td>"; 
        html = html + "     </tr>";
    }
    html = html + " </tbody>";
    html = html + "</table>";

    return html;
}

function loadInfoFile(id, fileName, fileSize) {
    $('#' + id).html(fileName + " (" + fileSize + ")");
    $('#' + id).removeClass('action-alert');
    $('#' + id).addClass('action-exito');
    $('#' + id).css('margin-bottom', '10px');
}

function loadValidacionFile(id, mensaje) {
    $('#' + id).html(mensaje);
    $('#' + id).removeClass('action-exito');
    $('#' + id).addClass('action-alert');
    $('#' + id).css('margin-bottom', '10px');
}

function mostrarProgreso(id, porcentaje) {
    $('#' + id).text(porcentaje + "%");
}

function mostrarMensajeDefecto() {
    mostrarMensaje('mensaje', 'message', 'Complete los datos solicitados.');
}

function limpiarMensaje(id) {
    $('#' + id).removeClass();
    $('#' + id).html('');
}

function validarNumero(texto) {
    return /^-?[\d.]+(?:e-?\d+)?$/.test(texto);
}

function mostrarMensaje(id, tipo, mensaje) {
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);
}
