var controlador = siteRoot + 'rolturnos/rolturno/';
var uploader;

$(function () {

    $('#txtMesAnio').Zebra_DatePicker({
        format: 'm-Y',
        onSelect: function () {
        }
    });

    $('#btnConfigurar').on('click', function () {
        document.location.href = siteRoot + 'rolturnos/configuracion/';
    });

    $('#btnReporte').on('click', function () {
        mostrarReporte();
    });

    $('#btnConsultar').on('click', function () {
        consultar();
    });

    $('#btnGrabar').on('click', function () {
        grabarDatos();
    });

    $('#btnDescargar').on('click', function () {
        descargarFormato();
    });

    consultar();
    crearPupload();
});

function consultar() {
    limpiarMensaje('mensaje');
    if ($('#txtMesAnio').val() != "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'consultar',
            data: {
                fecha: $('#txtMesAnio').val()
            },
            dataType: 'json',
            success: function (result) {
                if (result.Result == 1) {
                    cargarGrilla(result);
                    cargarDatosAdicionales(result);
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
        mostrarMensaje('mensaje', 'alert', 'Seleccione mes año.');
    }
}

function grabarDatos() {

    if (!validarExistencia()) {
        mostrarMensaje('mensaje', 'alert', 'Debe ingresar datos de al menos de un día para cualquier personal.');
    }
    else {
        grabarRolTurno();
    }
}

function grabarRolTurno() {
    $.ajax({
        type: 'POST',
        url: controlador + 'GrabarRolTurno',
        contentType: 'application/json',
        data: JSON.stringify({
            data: getData(),
            modalidad: getModalidad(),
            fecha: $('#txtMesAnio').val()
        }),
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

function cargarDatosAdicionales(result) {
    $('#strUsuario').text(result.Usuario);
    $('#strFecha').text(result.Fecha);
    $('#txtConfiguracion').text(result.Configuracion);
    $("#tablaActividad > tbody").empty();

    for (var i in result.ListaActividad) {
        var str = "<tr>";
        str = str + "<td>" + result.ListaActividad[i].label + "</td><td>" + result.ListaActividad[i].descripcion + "</td>";
        str = str + "</tr>";
        $("#tablaActividad > tbody").append(str);
    }
}

function descargarFormato() {
    $.ajax({
        type: 'POST',
        url: controlador + "GenerarFormato",
        data: {
            fecha: $('#txtMesAnio').val()
        },
        dataType: 'json',
        cache: false,
        success: function (resultado) {
            if (resultado == 1) {
                location.href = controlador + "DescargarFormato?fecha=" + $('#txtMesAnio').val();
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

function crearPupload() {
    uploader = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: 'btnImportar',
        url: controlador + 'Upload',
        flash_swf_url: 'Scripts/Moxie.swf',
        silverlight_xap_url: 'Scripts/Moxie.xap',
        multi_selection: false,
        filters: {
            max_file_size: '5mb',
            mime_types: [
                { title: "Archivos Excel .xlsx", extensions: "xlsx" }
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
        url: controlador + 'Importar',
        dataType: 'json',
        data: {
            fecha: $('#txtMesAnio').val()
        },
        success: function (result) {
            if (result.Result == 1) {
                cargarGrilla(result);
                mostrarMensaje('mensaje', 'exito', 'Los datos se cargaron correctamente en el Excel web, presione el botón <strong>Grabar</strong>.');
            }
            else if (result.Result == 2) {
                mostrarMensaje('mensaje', 'alert', 'No se importaton los datos dado que existen errores de datos en el Excel. Presione <a href="JavaScript:mostrarErrores();">aquí</a> para ver los errores nuevamenente.');
                pintarErrores(result.Errores);
            }
            else if (result.Result == 3) {
                mostrarMensaje('mensaje', 'alert', 'El archivo importado debe tener la estructura correcta.');
            }
            else if (result.Result == 4) {
                mostrarMensaje('mensaje', 'alert', 'No existe la hoja RolTurno en el libro Excel.');
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

function pintarErrores(errores) {
    $("#tableError > tbody").empty();
    for (var i in errores) {
        var str = "<tr>";
        str = str + "<td>" + errores[i].Responsable + "</td>";
        str = str + "<td>" + errores[i].NroDia + "</td>";
        str = str + "<td>" + errores[i].Valor + "</td>";
        str = str + "<td>" + errores[i].Motivo + "</td>";
        str = str + "</tr>";
        $("#tableError > tbody").append(str);
    }

    $('#validaciones').bPopup({

    });

    $('#tableError').dataTable({
        "destroy": true,
        "scrollY": 200,
        "scrollX": true,
        "sDom": 't',
        "ordering": false
    });

}

function mostrarErrores() {
    $('#validaciones').bPopup({

    });
}

function mostrarReporte() {
    $.ajax({
        type: 'POST',
        url: controlador + 'Reporte',
        data: {
            fecha: $('#txtMesAnio').val()
        },
        success: function (evt) {
            $('#contenidoReporte').html(evt);
            setTimeout(function () {
                $('#reporte').bPopup({
                    autoClose: false,
                    modalClose: false,
                    escClose: false,
                    follow: [false, false]
                });
            }, 50);
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        }
    });
}

function mostrarMensaje(id, tipo, mensaje) {
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);
}

function limpiarMensaje(id) {
    $('#' + id).removeClass('action-alert');
    $('#' + id).removeClass('action-exito');
    $('#' + id).removeClass('action-message');
    $('#' + id).removeClass('action-error');
    $('#' + id).html('');
}