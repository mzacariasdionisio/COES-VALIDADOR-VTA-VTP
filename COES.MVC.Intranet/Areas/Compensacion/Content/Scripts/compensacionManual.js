var controlador = siteRoot + "compensacion/general/";

$( /**
   * Llamadas iniciales
   * @returns {} 
   */
   $(document).ready(function () {
       mostrarMensaje("Selecionar el periodo y la versión");

       $('#fechaini').Zebra_DatePicker({
           format: 'd/m/Y'
       });

       $('#fechafin').Zebra_DatePicker({
           format: 'd/m/Y'
       });

       $('#btnConsultar').click(function () {
           if (ValidarVersion('Consultar', 1)) {
               pintarBusqueda(1);
           }
       });

       $('#btnProcesar').click(function () {
           if (ValidarVersion('Procesar', 0)) {
               procesarCompensacion();
           }
       });

       $('#btnDescargar').click(function () {
           descargarFormato();
     
       });

       $("#empresa").change(function () { ObtenerListaCentral(this.value, ''); });
       //$("#central").change(function () { ObtenerListaGrupo(this.value, ''); });
       //$("#grupo").change(function () { ObtenerListaModo(this.value, ''); });

       $("#pericodi").change(function () { ObtenerPeriodoCalculo(this.value, ''); });

        $('#btnEliminar').click(function () {
            if (ValidarVersion('Eliminar', 0)) {
                eliminarPorVersion();
            }
        });

       crearPupload();

       //Inicializamos la pantalla
       ObtenerPeriodoCalculo($("#pericodi").val(), '');
   }));


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


function crearPupload() {

    uploader = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: 'btnSelectFile',
        container: document.getElementById('container'),
        url: siteRoot + 'compensacion/general/upload',
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
                        loadValidacionFile("Seleccionar archivo.");
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
                procesarArchivo();
            },
            Error: function (up, err) {
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
    if ($("#pecacodi").val() == "" || $("#pecacodi").val() == "0" || $("#pecacodi").val() == null)
    {
        mostrarAlerta("Verificar la selección del periodo y la versión");

        return false;
    }

    $.ajax({
        type: 'POST',
        url: controlador + 'cargarILO',
        data: {
            pecacodi: $("#pecacodi").val()
        },
        dataType: 'json',
        cache: false,
        success: function (resul) {

            if (resul.resultado == 1) {
                mostrarExito(resul.mensaje);
            }
            else {
                mostrarError(resul.mensaje)
            }

        },
        error: function () {
                mostrarError('Opcion Procesar: Ha ocurrido un error');
        }
    });
}


function procesarCompensacion() {
    $.ajax({
        type: 'POST',
        url: controlador + 'procesarCompensacionRegular',
        data: {
            pecacodi: $("#pecacodi").val()
        },
        dataType: 'json',
        success: function (result) {

        },
        error: function () {
            mostrarMensaje('mensaje', 'Ha ocurrido un error', $tipoMensajeError, $modoMensajeCuadro);
        }
    });
}

var pintarBusqueda =
    /**
    * Pinta el listado de periodos según el año seleccionado
    * @returns {} 
    */
    function (verMsj) {


        $.ajax({
            type: "POST",
            url: controlador + "listarCompensacionesManuales",
            data: {
                pecacodi: $("#pecacodi").val(),
                empresa: $("#empresa").val(),
                central: $("#central").val(),
                grupo: '',
                modo: '',
                tipo: $("#tipo").val(),
                fecIni: $("#fechaini").val(),
                fecFin: $("#fechafin").val()
            },
            success: function (evt) {

                $('#listado').html(evt);
                $('#tabla').dataTable({
                    "ordering": true,
                    "paging": false,
                    "scrollY": 272,
                    "bDestroy": true
                });
                if (verMsj == 1) {
                    mostrarMensaje("Consulta generada");
                }
                
            },
            error: function() {
                    mostrarError('Opcion Consultar: Ha ocurrido un error');
            }
        });
    };



function ObtenerListaCentral(valor, selected) {

    if (valor != "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'obtenerListaCentral',
            data: {
                emprcodi: valor
            },
            dataType: 'json',
            success: function (data) {
                var jsonData = JSON.parse(data);
                //dwr.util.removeAllOptions("modo");
                //dwr.util.removeAllOptions("grupo");
                dwr.util.removeAllOptions("central");
                dwr.util.addOptions("central", jsonData, 'id', 'name');
                dwr.util.setValue("central", selected);
            },
            error: function () {
                mostrarError('Ha ocurrido un error', $tipoMensajeError, $modoMensajeCuadro);
            }
        });
    }
    else {
        //dwr.util.removeAllOptions("modo");
        //dwr.util.removeAllOptions("grupo");
        dwr.util.removeAllOptions("central");

    }
}

function ObtenerListaGrupo(valor, selected) {
    var empresa;
    empresa = $("#empresa").val();

    if (valor != "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'obtenerListaGrupo',
            data: {
                emprcodi: empresa,
                grupopadre: valor
            },
            dataType: 'json',
            success: function (data) {
                var jsonData = JSON.parse(data);
                dwr.util.removeAllOptions("modo");
                dwr.util.removeAllOptions("grupo");
                dwr.util.addOptions("grupo", jsonData, 'id', 'name');
                dwr.util.setValue("grupo", selected);
            },
            error: function () {
                mostrarError('Ha ocurrido un error', $tipoMensajeError, $modoMensajeCuadro);
            }
        });
    }
    else {
        dwr.util.removeAllOptions("modo");
        dwr.util.removeAllOptions("grupo");
    }
}

function ObtenerListaModo(valor, selected) {
    var empresa;
    empresa = $("#empresa").val();

    if (valor != "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'obtenerListaModo',
            data: {
                emprcodi: empresa,
                grupopadre: valor
            },
            dataType: 'json',
            success: function (data) {
                var jsonData = JSON.parse(data);
                dwr.util.removeAllOptions("modo");
                dwr.util.addOptions("modo", jsonData, 'id', 'name');
                dwr.util.setValue("modo", selected);
            },
            error: function () {
                mostrarError('Ha ocurrido un error', $tipoMensajeError, $modoMensajeCuadro);
            }
        });
    }
    else {
        dwr.util.removeAllOptions("modo");
    }
}

function ObtenerPeriodoCalculo(valor, selected) {

    if (valor != "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'obtenerPeriodoCalculo',
            data: {
                pericodi: valor
            },
            dataType: 'json',
            success: function (data) {
                var jsonData = JSON.parse(data);
                dwr.util.removeAllOptions("pecacodi");
                dwr.util.addOptions("pecacodi", jsonData, 'id', 'name');
                dwr.util.setValue("pecacodi", selected);
            },
            error: function () {
                mostrarError('Ha ocurrido un error', $tipoMensajeError, $modoMensajeCuadro);
            }
        });
    }
    else {
        dwr.util.removeAllOptions("pecacodi");
    }
}

function ValidarVersion(titulo_opcion, limpiar_listado) {
    if ($("#pecacodi").val() == "" || $("#pecacodi").val() == null) {

        if (limpiar_listado == 1) {
            $("#listado").empty();
        }

        mostrarAlerta("Opcion " + titulo_opcion + ": Verificar la selección del periodo y la versión");

        return false;
    }
    else {
        return true;
    }
}

function eliminarCompensacionManual(pecaCodi, grupoCodi, subcausaCodi, crcbeHorini)  {
    if (confirm("¿Desea eliminar el registro de compensacion?")) {
        $.ajax({
            type: 'POST',
            dataType: 'json',
            url: controlador + "eliminarCompensacionManual",
            data: {
                pecacodi: pecaCodi,
                grupocodi: grupoCodi,
                subcausacodi: subcausaCodi,
                crcbehorini: crcbeHorini
            },
            success: function (result) {
                if (result.success) {
                    mostrarExito("Se ha eliminado el registro.");
                    pintarBusqueda(0);
                }
                else {
                    mostrarAlerta(result.message);
                }

            },
            error: function () {
                    mostrarError("Ha Ocurrido un error");
            }
        });
    }
}

function eliminarPorVersion() {

    if (confirm("¿Desea eliminar todos los registros de la versión seleccionada?")) {
        $.ajax({
            type: 'POST',
            dataType: 'json',
            url: controlador + "EliminarCompensacionManualPorVersion",
            data: {
                pecacodi: $("#pecacodi").val() 
            },
            success: function (result) {
                if (result.success) {
                    mostrarExito("Se ha eliminado los registros de la versión seleccionada.");
                    pintarBusqueda(0);
                }
                else {
                    mostrarAlerta(result.message);
                }

            },
            error: function () {
                mostrarError("Ha Ocurrido un error");
            }
        });
    }
}

function descargarFormato() {

    $.ajax({
        type: 'POST',
        url: controlador + 'descargarFormatoCompensacionManual',
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
