// =======================================================================================
//
// (c) COES Sinac. Todos los derechos reservados. 
//
// Autor: Henry Manuel Díaz Tuesta
// Acronimo: HDT
// Requerimiento: compensaciones
//
// Fecha creacion: 21/03/2017
// Descripcion: Archivo para la atencion del requerimiento.
//
// Historial de cambios:
// 
// Correlativo	Fecha		Requerimiento		Comentario
//
// =======================================================================================

var controlador = siteRoot + "compensacion/general/";

$( /**
   * Llamadas iniciales
   * @returns {} 
   */
   $(document).ready(function () {

       mostrarExito("Registre los incrementos/reducciones");

       $('#fecha').Zebra_DatePicker({
           format: 'd/m/Y'
       });

       $('#btnNuevoRegistro').click(function () {
           nuevoRegistro();
       });

       $('#btnRegresar').click(function () {
           regresar();
       });

       $('#btnGuadarEdicion').click(function () {
           guardarEdicion();
       });

       $('#btnCancelarEdicion').click(function () {
           cancelarEdicion();
       });

       $('#btnDescargar').click(function () {
           descargarFormato();
       });

       crearPupload();
       pintarBusqueda();

   }));

function crearPupload() {

    uploader = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: 'btnSelectFile',
        container: document.getElementById('container'),
        url: siteRoot + 'compensacion/general/uploadIR',
        flash_swf_url: '~/Content/Scripts/Moxie.swf',
        silverlight_xap_url: '~/Content/Scripts/Moxie.xap',
        multi_selection: false,
        filters: {
            max_file_size: '5mb',
            mime_types: [
                { title: "Archivos Excel .xlsx", extensions: "xlsx" }
            ]
        },
        init: {
            PostInit: function () {
                document.getElementById('btnProcesarFile').onclick = function () {
                    if (uploader.files.length > 0) {
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
                up.refresh();
            },
            UploadProgress: function (up, file) {
                mostrarAlerta("Por favor espere ...(<strong>" + file.percent + "%</strong>)");
            },
            UploadComplete: function (up, file) {
                //alert('Upload complete');
                procesarArchivo();
            },
            Error: function (up, err) {
                //alert('Error');
                loadValidacionFile(err.code + "-" + err.message);
            }
        }
    });

    uploader.init();
}

loadInfoFile = function (fileName, fileSize) {
    mostrarMensaje(fileName + " (" + fileSize + ")");
}

loadValidacionFile = function (mensaje) {
    mostrarAlerta(mensaje);
}

procesarArchivo = function () {
    $.ajax({
        type: 'POST',
        url: controlador + 'cargarILOIncred',
        data: {
            pericodi: $("#pericodi").val(),
            pecacodi: $("#pecacodi").val()
        },
        dataType: 'json',
        cache: false,
        success: function (resul) {

            if (resul.resultado == 1) {
                mostrarExito(resul.mensaje);
                pintarBusqueda();
            }
            else {
                mostrarError(resul.mensaje)
            }

        },
        error: function () {
            mostrarError();
        }
    });
}

var pintarBusqueda =
    /**
    * Pinta el listado
    * @returns {} 
    */
    function () {


        $.ajax({
            type: "POST",
            url: controlador + "listarIncrementosReducciones",
            data: {
                periodo: $.get("periodo")
            },
            success: function (evt) {

                $('#listado').css("width", $('#mainLayout').width() + "px");
                $('#listado').html(evt);
                $('#tabla').dataTable({
                    "ordering": false,
                    "paging": false,
                    "scrollY": 350,
                    "scrollX": true,
                    "bDestroy": true
                });

            },
            error: function () {
                mostrarMensaje("mensaje", $mensajeErrorGeneral, $tipoMensajeError, $modoMensajeCuadro);
            }
        });
    };

var regresar = function () {
    parent.history.back();
};

var nuevoRegistro = function () {
    $("#modoOperacion").prop("disabled", false);
    $("#fecha").prop("disabled", false);

    $('#esNuevo').val(1);
    $('#modoOperacion').val([]);
    var currentdate = new Date();
    var ahora = currentdate.getDate().toString().lpad("0", 2) + "/" + (currentdate.getMonth() + 1).toString().lpad("0", 2) + "/" + currentdate.getFullYear();
    $('#fecha').val(ahora);
    $('#incrementos').val(0);
    $('#reducciones').val(0);
    
    $("#popupEdicion").bPopup({
        autoClose: false
    });
};

function modificarIncrementoReduccion(pecaCodi, grupoCodi, apinrefechaDesc) {
    
    $('#esNuevo').val(0);

    $("#modoOperacion").prop("disabled", true);
    $("#fecha").prop("disabled", true);

    $.ajax({
        type: 'POST',
        url: controlador + 'ObtenerIncrementoReduccion',
        data: {
            PecaCodi: pecaCodi, 
            GrupoCodi: grupoCodi,
            ApinrefechaDesc: apinrefechaDesc
        },
        dataType: 'json',
        success: function (data) {
            var jsonData = JSON.parse(data);

            $('#modoOperacion').val(jsonData.Grupocodi);
            var partesFecha = jsonData.ApinrefechaDesc.split("/");
            var currentdate = new Date(partesFecha[2], (partesFecha[1] - 1), partesFecha[0]);
            var fecha = currentdate.getDate().toString().lpad("0", 2) + "/" + (currentdate.getMonth() + 1).toString().lpad("0", 2) + "/" + currentdate.getFullYear();
            $('#fecha').val(fecha);
            $('#incrementos').val(jsonData.Apinrenuminc);
            $('#reducciones').val(jsonData.Apinrenumdis);

            $("#popupEdicion").bPopup({
                autoClose: false
            });
        },
        error: function () {
            mostrarMensaje('mensaje', 'Ha ocurrido un error', $tipoMensajeError, $modoMensajeCuadro);
        }
    });

}

/*
* Guarda la edición del incremento/reducción
*/
function guardarEdicion() {

    var pecacodi = $.get("periodo");
    if (pecacodi == "" || pecacodi == null) {
        alert('No se ha podido identificar el periodo');
        return;
    }

    var grupocodi = $('#modoOperacion').val();
    if (grupocodi == "" || grupocodi == null) {
        alert('Por favor selecciones el modo de operación');
        return;
    }

    var apinrefechaDesc = $('#fecha').val();
    if (apinrefechaDesc == "" || apinrefechaDesc == null) {
        alert('Por favor indique una fecha válida');
        return;
    }

    var apinrenuminc = $('#incrementos').val();
    if (apinrenuminc == "" || apinrenuminc == null) {
        alert('Por favor proporcione el valor de los incrementos');
        return;
    }

    var apinrenumdis = $('#reducciones').val();
    if (apinrenumdis == "" || apinrenumdis == null) {
        alert('Por favor proporcione el valor de las reducciones');
        return;
    }

    var esNuevo = false;

    if ($('#esNuevo').val() == 1) {
        esNuevo = true;
    }

    $.ajax({
        type: 'POST',
        url: controlador + 'GuardarIncrementoReduccion',
        data: {
            Pecacodi: pecacodi,
            Grupocodi: grupocodi,
            ApinrefechaDesc: apinrefechaDesc,
            Apinrenuminc: apinrenuminc,
            Apinrenumdis: apinrenumdis,
            EsNuevo: esNuevo
        },
        dataType: 'json',
        success: function (result) {

            if (result.success) {
                $('#popupEdicion').bPopup().close();
                pintarBusqueda();
                if (esNuevo) {
                    mostrarExito("Se ha creado el incremento/reducción");
                }
                else {
                    mostrarExito("Se ha modificado el incremento/reducción");
                }
            }
            else {
                alert(result.message);
            }

        },
        error: function () {
            mostrarMensaje('mensaje', 'Ha ocurrido un error', $tipoMensajeError, $modoMensajeCuadro);
        }
    });
}

function cancelarEdicion() {

    $('#popupEdicion').bPopup().close();

}

function eliminarIncrementoReduccion(pecaCodi, grupoCodi, apinrefechaDesc) {
    if (confirm("¿Desea eliminar el incremento/reducción?")) {
        $.ajax({
            type: 'POST',
            dataType: 'json',
            url: controlador + "EliminarIncrementoReduccion",
            data: {
                PecaCodi: pecaCodi,
                GrupoCodi: grupoCodi,
                ApinreFechaDesc: apinrefechaDesc
            },
            success: function (result) {

                if (result.success) {
                    mostrarExito("Se ha eliminado el incremento/reducción")
                    pintarBusqueda();
                }
                else {
                    alert(result.message);
                }

            },            
            error: function () {
                mostrarError();
            }
        });
    }
}

function descargarFormato() {

    $.ajax({
        type: 'POST',
        url: controlador + 'descargarFormatoIncrementoReduccion',
        dataType: 'json',
        success: function (result) {
            if (result != -1) {
                document.location.href = controlador + 'descargar?formato=' + 1 + '&file=' + result
                mostrarMensaje("Descarga realizada");
            }
            else {
                mostrarError('Opcion Descargar Formato: Ha ocurrido un error');
            }
        },
        error: function () {
            mostrarError('Opcion Descargar Formato: Ha ocurrido un error');
        }
    });
}

function mostrarExito(mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-exito');
}

function mostrarError(mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-error');
}

function mostrarAlerta(mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-alert');
}

function mostrarMensaje(mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-message');
}

//- Permite obtener el valor del parámetro GET.
$.get = function (key) {
    key = key.replace(/[\[]/, '\\[');
    key = key.replace(/[\]]/, '\\]');
    var pattern = "[\\?&]" + key + "=([^&#]*)";
    var regex = new RegExp(pattern);
    var url = unescape(window.location.href);
    var results = regex.exec(url);
    if (results === null) {
        return null;
    } else {
        return results[1];
    }
}

function soloNumeros(e) {
    var key = window.Event ? e.which : e.keyCode
    return (key <= 13 || (key >= 48 && key <= 57) || key == 46);
}

String.prototype.lpad = function (padString, length) {
    var str = this;
    while (str.length < length)
        str = padString + str;
    return str;
}