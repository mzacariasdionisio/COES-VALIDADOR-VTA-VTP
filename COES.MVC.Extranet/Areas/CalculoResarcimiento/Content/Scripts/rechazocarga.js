var controlador = siteRoot + 'calculoresarcimiento/rechazocarga/';
var uploader;

$(function () {

    $('#txtAnio').Zebra_DatePicker({
        format: 'Y',
        onSelect: function (date) {
            cargarPeriodos(date);
        }
    });

    $('#btnConsultar').on('click', function () {
        //consultar();
        var tipoDato = 2; //Rechazo de carga
        ValidarHabilitacionCargaYConsultar(tipoDato);
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

    $('#btnAgregarFila').on('click', function () {
        agregarFila();
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

    $('#btnCancelarAnulacion').on('click', function () {
        $('#popupAnulacion').bPopup().close();
    });
      
    $('#cbPeriodo').on('change', function () {
        habilitarEnvio(false);
    });

    $('#cbRecalcularAnulacion').on('click', function () {
        if ($('#cbRecalcularAnulacion').is(':checked')) {
            $('#btnEnviarAnulacion').show();
        }
        else {
            $('#btnEnviarAnulacion').hide();
        }
    });

    habilitarEnvio(false);
    crearPupload();    
});

function consultar(indicador) {
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
                        //cargarGrillaInterrupcion(result.false);
                        //habilitarEnvio(true);

                        //Si tiene datos cargados previamente, no podra volver enviar informacion //SOS I2
                        if (result.ConDatos) {
                            cargarGrillaInterrupcion(result, true);
                            deshabilitarBotonesEnvio(true);
                            console.log("Estamos aqui");
                            if (indicador == "TIENE_DATOS") {
                                mostrarMensaje('mensaje', 'alert', 'Esta empresa no tiene habilitada la carga de información para el periodo seleccionado.');
                                habilitarEnvioAdicional();
                            }

                        } else {
                            deshabilitarBotonesEnvio(false);
                            cargarGrillaInterrupcion(result, false);
                            habilitarEnvio(true);
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
        $('#detalleFormato').html('');
    }
    else {
        $('#divAcciones').show();
        $('#ctnBusqueda').show();
    }
}

function habilitarEnvioAdicional() {

    $('#divAcciones').show();
    $('#btnDescargarFormato').show();
    $('#btnSubirFormato').hide();
    $('#btnEnviarDatos').hide();
    $('#btnMostrarErrores').hide();
    $('#btnAgregarFila').hide();
    $('#btnOcultarColumna').hide();
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
                if (result.Result == 1) {

                    $('#popupEnvio').bPopup().close();
                    consultar("");
                    mostrarMensaje('mensaje', 'exito', 'Los datos fueron grabados correctamente.');
                }
                else if (result.Result == 2 || result.Result == 3 || result.Result == 4) {
                 
                    var html = mostrarErroresCalculo(result.Data);
                    $('#contenidoError').html(html);
                    $('#popupErrores').bPopup({});
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

function agregarFila() {
    agregarFilaGrilla();
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

function anularInterrupcion(id, row) {
   
    $('#hfIdInterrupcion').val(id);
    limpiarMensaje('mensajeAnulacion');
    $('#hfRowAnular').val(row);
    $('#txtComentarioAnulacion').val("");
    $('#cbRecalcularAnulacion').prop('checked', false);

    var datosAnulacion = obtenerDatosAnulacion(id);

    $.ajax({
        type: 'POST',
        url: controlador + 'ObtenerDatosAnulacion',
        contentType: 'application/json',
        data: JSON.stringify({
            data: datosAnulacion,
            periodo: $('#cbPeriodo').val()
        }),
        dataType: 'json',
        success: function (result) {
            if (result.Result == 1) {
                var html = mostrarErroresCalculoAnulacion(result.Data);
                $('#contenidoErrorAnulacion').html(html);
                $('#contenidoErrorAnulacion').show();
                $('#confirmarRecalculoAnulacion').show();
                $('#btnEnviarAnulacion').hide();
            }
            else if (result.Result == 2) {
                $('#contenidoErrorAnulacion').hide();
                $('#confirmarRecalculoAnulacion').hide();
                $('#btnEnviarAnulacion').show();
            }

            $('#popupAnulacion').bPopup({});
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        }
    });
}

function confirmarAnulacion() {

    if ($('#txtComentarioAnulacion').val() != "") {

        var datos = [];
        var recalculo = "N";
        if ($('#cbRecalcularAnulacion').is(':checked')) {
            recalculo = "S";
            var dataTotal = getData();

            $('#tablaRecalculoAnulacion tbody tr').each(function () {
                console.log("ebtri");
                var fila = $(this).find('td');
                if (fila.length >= 4) {
                    var itemDato = [];
                    var orden = $(fila[0]).text().trim();
                    itemDato[0] = dataTotal[parseInt(orden) - 1][0];
                    itemDato[1] = $(fila[3]).text().trim();                    
                    datos.push(itemDato);
                }
            });
        }
       

        $.ajax({
            type: 'POST',
            url: controlador + 'AnularInterrupcion',
            data: {
                idPeriodo: $('#cbPeriodo').val(),
                idInterrupcion: $('#hfIdInterrupcion').val(),
                comentario: $('#txtComentarioAnulacion').val(),
                recalculo: recalculo,
                data: datos
            },
            dataType: 'json',
            success: function (result) {
                if (result == 1) {
                    //eliminarFilaGrilla($('#hfRowAnular').val());
                    $('#popupAnulacion').bPopup().close();
                    consultar();
                    mostrarMensaje('mensaje', 'exito', 'El registro se eliminó correctamente.');
                }
                else if (result == 2) {
                    mostrarMensaje('mensajeAnulacion', 'alert', 'No se puede anular dado que ha superado el plazo de la etapa 7.');
                }
                else {
                    mostrarMensaje('mensajeAnulacion', 'error', 'Se ha producido un error.');
                }
            },
            error: function () {
                mostrarMensaje('mensajeAnulacion', 'error', 'Se ha producido un error.');
            }
        });
    }
    else {
        mostrarMensaje('mensajeAnulacion', 'alert', 'Ingrese el motivo de anulación.');
    }
}

function ValidarHabilitacionCargaYConsultar(tipoDato) {

    mostrarMensajeDefecto();
    if ($('#hfIdEmpresa').val() != "") {
        if ($('#cbPeriodo').val() != "") {
            var periodoId = parseInt($("#cbPeriodo").val()) || 0;
            var empresaId = parseInt($("#hfIdEmpresa").val()) || 0;
            var mensaje = "";
            $.ajax({
                type: 'POST',
                url: controlador + 'validarHabilitacionCarga',
                contentType: "application/json",
                dataType: 'json',
                data: JSON.stringify({
                    repercodi: periodoId,
                    emprcodi: empresaId,
                    tipo: tipoDato
                }
                ),
                success: function (evt) {

                    mensaje = evt.Mensaje;

                    //Si esta habilitdo y no hay errores, muestra el formulario web
                    if (mensaje == "") {
                        consultar("");
                    } else {
                        if (mensaje == "TIENE_DATOS") {
                            
                            consultar("TIENE_DATOS");
                        }
                        else {
                            mostrarMensaje('mensaje', 'alert', mensaje);
                        }
                    }
                },
                error: function (err) {
                    mostrarMensaje('mensaje', 'alert', 'Ha ocurrido un error.');
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

    //Oculto los botones
    if (flag) {
        $('#btnDescargarFormato').hide();
        $('#btnSubirFormato').hide();
        $('#btnEnviarDatos').hide();
        $('#btnMostrarErrores').hide();
        $('#btnAgregarFila').hide();
        $('#btnOcultarColumna').hide();

        mostrarMensaje('mensaje', 'message', 'Información enviada previamente.');
    }
    else {
        $('#btnDescargarFormato').show();
        $('#btnSubirFormato').show();
        $('#btnEnviarDatos').show();
        $('#btnMostrarErrores').show();
        $('#btnAgregarFila').show();
        $('#btnOcultarColumna').show();
    }
}