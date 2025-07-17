var controlador = siteRoot + 'calculoresarcimiento/consolidado/';
var uploader;

$(function () {

    $('#tab-container').easytabs({
        animate: false
    });

    $('#btnHabilitar').on('click', function () {
        habilitar();
    });

    $('#btnConsultar').on('click', function () {
        consultar();
    });

    $("#btnManualUsuario").click(function () {
        window.location = controlador + 'DescargarManualUsuario';
    });

    $('#txtAnio').Zebra_DatePicker({
        format: 'Y',
        onSelect: function (date) {
            cargarPeriodos(date)
        }
    });

    $('#cbPeriodo').on('change', function () {
        $('#detalleFormato').html("");
        $('#tab-container').hide();
    });

    $('#cbTipoInformacion').on('change', function () {
        $('#detalleFormato').html("");
        $('#tab-container').hide();        
    });

    $('#cbSuministrador').on('change', function () {
        cargarFiltrosActualizados();
    });

    $('#tab-container').hide();    
    $('#btnOcultarColumnas').hide();

    $('#btnGrabarInterrupcion').on('click', function () {
        grabarInterrupcion();
    });

    $('#btnExportar').on('click', function () {
        exportarConsolidado();
    });

    $('#btnOcultarColumnas').on('click', function () {
        ocultarColumnas();
    });

    $('#btnDescargarArchivos').on('click', function () {
        descargarArchivosMasivo();
    });

    $('#btnOcultar').on('click', function () {
        if ($('#cbTipoInformacion').val() == "S")
            aplicarOcultado();
        if ($('#cbTipoInformacion').val() == "R")
            aplicarOcultadoRC();
    });

    crearPupload();
});

function cargarPeriodos(anio) {
    $.ajax({
        type: 'POST',
        url: controlador + 'ObtenerPeriodos',
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
                $('#tab-container').hide();
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

function habilitar() {
    limpiarMensaje('mensaje');
    if ($('#cbPeriodo').val() != "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'HabilitarConsulta',
            data: {
                periodo: $('#cbPeriodo').val(),
                tipo: $('#cbTipoInformacion').val()
            },
            dataType: 'json',
            global: false,
            success: function (result) {                
                $('#detalleFormato').html("");
                $('#cbBarra').get(0).options.length = 0;
                $('#cbBarra').get(0).options[0] = new Option("--SELECCIONE--", "-1");
                $.each(result.ListaPuntoEntrega, function (i, item) {
                    $('#cbBarra').get(0).options[$('#cbBarra').get(0).options.length] = new Option(item.Repentnombre, item.Repentcodi);
                });

                $('#cbEvento').get(0).options.length = 0;
                $('#cbEvento').get(0).options[0] = new Option("--SELECCIONE--", "-1");
                $.each(result.ListaEvento, function (i, item) {
                    $('#cbEvento').get(0).options[$('#cbEvento').get(0).options.length] = new Option(item.Reevedescripcion, item.Reevecodi);
                });

                $('#cbCausaInterrupcion').get(0).options.length = 0;
                $('#cbCausaInterrupcion').get(0).options[0] = new Option("--SELECCIONE--", "-1");
                $.each(result.ListaCausaInterrupcion, function (i, item) {
                    $('#cbCausaInterrupcion').get(0).options[$('#cbCausaInterrupcion').get(0).options.length] = new Option(item.Recintnombre, item.Recintcodi);
                });

                $('#cbSuministrador').get(0).options.length = 0;
                $('#cbSuministrador').get(0).options[0] = new Option("--SELECCIONE--", "-1");
                $.each(result.ListaSuministrador, function (i, item) {
                    $('#cbSuministrador').get(0).options[$('#cbSuministrador').get(0).options.length] = new Option(item.Emprnomb, item.Emprcodi);
                });

                $('#cbResponsable').get(0).options.length = 0;
                $('#cbResponsable').get(0).options[0] = new Option("--SELECCIONE--", "-1");
                $.each(result.ListaResponsables, function (i, item) {
                    $('#cbResponsable').get(0).options[$('#cbResponsable').get(0).options.length] = new Option(item.Emprnomb, item.Emprcodi);
                });
                                
                $('#btnOcultarColumnas').hide();

                //- Habilitamos contenido
                $('#tab-container').show();

                if ($('#cbTipoInformacion').val() == "S") {
                    $('.campo-interrupcion').show();
                    $('.campo-rechazo').hide();
                }
                else {
                    $('.campo-interrupcion').hide();
                    $('.campo-rechazo').show();
                }

                // Habilito la descarga del reporte comparativo
                if (result.MuestraReporteComparativo == 1) {
                    $('#filaComparativa').show();
                } else {
                    $('#filaComparativa').hide();
                }
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            }
        });
    }
    else {
        mostrarMensaje('mensaje', 'alert', 'Debe seleccionar un periodo.');
    }
}

function consultar() {
    if ($('#cbPeriodo').val() != "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'ConsultarInterrupcion',
            data: {
                periodo: $('#cbPeriodo').val(),               
                tipo: $('#cbTipoInformacion').val(),
                suministrador: $('#cbSuministrador').val(),
                barra: $('#cbBarra').val(),
                causaInterrupcion: $('#cbCausaInterrupcion').val(),
                conformidadResponsable: $('#cbConformidadResponsable').val(),
                conformidadSuministrador: $('#cbConformidadSuministrador').val(),
                evento: $('#cbEvento').val(),
                alimentadorSED: $('#txtAlimentadorSED').val(),
                estado: $('#cbEstado').val(),
                responsable: $('#cbResponsable').val(),
                disposicion: $('#cbAplicacion').val(),
                compensacion: $('#cbCompensacionCero').val(),
                buscar: $('#txtBuscar').val()
            },
            dataType: 'json',
            global: false,
            success: function (result) {
                if ($('#cbTipoInformacion').val() == "S") {
                    
                    $('#btnOcultarColumnas').show();
                    cargarGrillaInterrupciones(result);
                }
                else {
                    
                    $('#btnOcultarColumnas').show();
                    cargarGrillaRechazoCarga(result);
                }
               
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            }
        });
    }
    else {
        mostrarMensaje('mensaje', 'alert', 'Debe seleccionar un periodo.');
    }
}

function cargarFiltrosActualizados() {
    limpiarMensaje('mensaje');
   
        $.ajax({
            type: 'POST',
            url: controlador + 'ObtenerDatosPorSuministrador',
            data: {
                periodo: $('#cbPeriodo').val(),
                suministrador: $('#cbSuministrador').val(),
                tipo: $('#cbTipoInformacion').val()
            },
            dataType: 'json',
            global: false,
            success: function (result) {
                //--cambio

                $('#cbBarra').get(0).options.length = 0;
                $('#cbBarra').get(0).options[0] = new Option("--SELECCIONE--", "-1");
                $.each(result.ListaPuntoEntrega, function (i, item) {
                    $('#cbBarra').get(0).options[$('#cbBarra').get(0).options.length] = new Option(item.Repentnombre, item.Repentcodi);
                });

                $('#cbEvento').get(0).options.length = 0;
                $('#cbEvento').get(0).options[0] = new Option("--SELECCIONE--", "-1");
                $.each(result.ListaEvento, function (i, item) {
                    $('#cbEvento').get(0).options[$('#cbEvento').get(0).options.length] = new Option(item.Reevedescripcion, item.Reevecodi);
                });

                $('#cbCausaInterrupcion').get(0).options.length = 0;
                $('#cbCausaInterrupcion').get(0).options[0] = new Option("--SELECCIONE--", "-1");
                $.each(result.ListaCausaInterrupcion, function (i, item) {
                    $('#cbCausaInterrupcion').get(0).options[$('#cbCausaInterrupcion').get(0).options.length] = new Option(item.Recintnombre, item.Recintcodi);
                });

                //- Cambio
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            }
        });
   
}

function grabarInterrupcion() {
    var data = obtenerDataInterrupcion($('#cbTipoInformacion').val());
    var validaciones = validarDatos(data, $('#cbTipoInformacion').val());

    if (validaciones.length == 0) {
        $.ajax({
            type: 'POST',
            url: controlador + 'GrabarInterrupcion',
            contentType: 'application/json',
            data: JSON.stringify({
                data: data,
                periodo: $('#cbPeriodo').val(),
                tipo: $('#cbTipoInformacion').val(),
                suministrador: $('#cbSuministrador').val(),
                barra: $('#cbBarra').val(),
                causaInterrupcion: $('#cbCausaInterrupcion').val(),
                conformidadResponsable: $('#cbConformidadResponsable').val(),
                conformidadSuministrador: $('#cbConformidadSuministrador').val(),
                evento: $('#cbEvento').val(),
                alimentadorSED: $('#txtAlimentadorSED').val(),
                estado: $('#cbEstado').val(),
                responsable: $('#cbResponsable').val(),
                disposicion: $('#cbAplicacion').val(),
                compensacion: $('#cbCompensacionCero').val(),
                buscar: $('#txtBuscar').val()
            }),
            dataType: 'json',
            success: function (result) {
                if (result == 1) {
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
    else {
        pintarError(validaciones);
    }
}

function pintarError(validaciones) {
    $('#contenidoError').html(obtenerErroresInterrupciones(validaciones));
    $('#popupErrores').bPopup({});
}


function obtenerErroresInterrupciones(data) {
    var html = "<table class='pretty tabla-adicional' id='tablaErrores'>";
    html = html + " <thead>";
    html = html + "     <tr>";
    html = html + "         <th>Fila</th>";
    html = html + "         <th>Columna</th>";
    html = html + "         <th>Error</th>";
    html = html + "     </tr>";
    html = html + " </thead>";
    html = html + " <tbody>";
    for (var k in data) {
        html = html + "     <tr>";
        html = html + "         <td>" + data[k].row + "</td>";        
        html = html + "         <td>" + data[k].col + "</td>";      
        html = html + "         <td>" + data[k].validation + "</td>";
        html = html + "     </tr>";
    }
    html = html + " </tbody>";
    html = html + "</table>";

    return html;
}

function exportarConsolidado() {
    $.ajax({
        type: 'POST',
        url: controlador + 'ExportarConsolidado',
        data: {
            periodo: $('#cbPeriodo').val(),
            tipo: $('#cbTipoInformacion').val(),
            suministrador: $('#cbSuministrador').val(),
            barra: $('#cbBarra').val(),
            causaInterrupcion: $('#cbCausaInterrupcion').val(),
            conformidadResponsable: $('#cbConformidadResponsable').val(),
            conformidadSuministrador: $('#cbConformidadSuministrador').val(),
            evento: $('#cbEvento').val(),
            alimentadorSED: $('#txtAlimentadorSED').val(),
            estado: $('#cbEstado').val(),
            responsable: $('#cbResponsable').val(),
            disposicion: $('#cbAplicacion').val(),
            compensacion: $('#cbCompensacionCero').val(),
            buscar: $('#txtBuscar').val()
        },
        dataType: 'json',
        global: false,
        success: function (result) {
            if (result == 1) {
                location.href = controlador + "DescargarConsolidado?tipo=" + $('#cbTipoInformacion').val();
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

function showMotivoAnulacion(id) {
    $.ajax({
        type: 'POST',
        url: controlador + 'ObtenerMotivoAnulacion',
        data: {            
            tipo: $('#cbTipoInformacion').val(),
            id: id            
        },
        dataType: 'json',
        global: false,
        success: function (result) {
            $('#usuarioAnulacion').text(result.Usuario);
            $('#fechaAnulacion').text(result.Fecha);
            $('#motivoAnulacion').text(result.Motivo);
           
            setTimeout(function () {
                $('#popupAnulacion').bPopup({
                    autoClose: false
                });
            }, 50);
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        }
    });
}

function showTrazabilidad(id) {
    $.ajax({
        type: 'POST',
        url: controlador + 'ObtenerTrazabilidad',
        data: {
            tipo: $('#cbTipoInformacion').val(),
            id: id
        },
        dataType: 'json',
        global: false,
        success: function (result) {
            $('#excelTrazabilidad').html('');
            if ($('#cbTipoInformacion').val() == "S") {
                                
                $('#popupTrazabilidad').bPopup({
                    autoClose: false,
                    onOpen: function () {
                        setTimeout(function () {
                            refreshGrilla(result);
                        }, 100);                       
                    },
                });               
            }
            else {                                
                $('#popupTrazabilidad').bPopup({
                    autoClose: false,
                    onOpen: function () {
                        setTimeout(function () {
                            refreshGrillaRC(result);
                        }, 100);
                    },
                });
            }
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        }
    });
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
            max_file_size: '10mb',
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
        url: controlador + 'ImportarInterrupciones',
        dataType: 'json',
        data: {
            periodo: $('#cbPeriodo').val(),
            tipo: $('#cbTipoInformacion').val(),
            suministrador: $('#cbSuministrador').val(),
            barra: $('#cbBarra').val(),
            causaInterrupcion: $('#cbCausaInterrupcion').val(),
            conformidadResponsable: $('#cbConformidadResponsable').val(),
            conformidadSuministrador: $('#cbConformidadSuministrador').val(),
            evento: $('#cbEvento').val(),
            alimentadorSED: $('#txtAlimentadorSED').val(),
            estado: $('#cbEstado').val(),
            responsable: $('#cbResponsable').val(),
            disposicion: $('#cbAplicacion').val(),
            compensacion: $('#cbCompensacionCero').val(),
            buscar: $('#txtBuscar').val()
        },
        success: function (result) {
            if (result.Result == 1) {
                mostrarMensaje('mensaje', 'exito', 'Los datos se cargaron correctamente en el Excel web, presione el botón <strong>Grabar Interrupciones</strong> para grabar.');
                actualizarDataGrilla(result.Data, $('#cbTipoInformacion').val());
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

function ocultarColumnas() {
    limpiarMensaje('mensajeColumna');
    cargarColumnasGrilla($('#cbTipoInformacion').val());
    $('#popupColumna').bPopup();
}

function showFileInterrupcion(idInterrupcion, extension) {
    location.href = controlador + 'DescargarArchivoInterrupcion?id=' + idInterrupcion + "&extension=" + extension;
}

function showFileObservacion(idInterrupcion, idDetalle, extension) {
    location.href = controlador + 'DescargarArchivoObservacion?id=' + idInterrupcion + "&idDet=" + idDetalle + "&extension=" + extension;
}

function showFileRespuesta(idInterrupcion, idDetalle, extension) {
    location.href = controlador + 'DescargarArchivoRespuesta?id=' + idInterrupcion + "&idDet=" + idDetalle + "&extension=" + extension;
}

function descargarArchivosMasivo() {
    location.href = controlador + 'DescargarArchivosGeneral?' +
        "periodo=" + $('#cbPeriodo').val() +
        "&tipo=" + $('#cbTipoInformacion').val() +
        "&suministrador=" + $('#cbSuministrador').val() +
        "&barra=" + $('#cbBarra').val() +
        "&causaInterrupcion=" + $('#cbCausaInterrupcion').val() +
        "&conformidadResponsable=" + $('#cbConformidadResponsable').val() +
        "&conformidadSuministrador=" + $('#cbConformidadSuministrador').val() +
        "&evento=" + $('#cbEvento').val() +
        "&alimentadorSED=" + $('#txtAlimentadorSED').val() +
        "&estado=" + $('#cbEstado').val() +
        "&responsable=" + $('#cbResponsable').val() +
        "&disposicion=" + $('#cbAplicacion').val() +
        "&compensacion=" + $('#cbCompensacionCero').val() +
        "&buscar=" + $('#txtBuscar').val();
}