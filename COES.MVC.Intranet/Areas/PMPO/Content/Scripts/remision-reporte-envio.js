var controlador = siteRoot + 'PMPO/ReportSubmission/';

var ANCHO_REPORTE = 1000;
var TIPO_INDEX_REPORTE = "1"
var TIPO_INDEX_VALIDACION = "2"

$(function () {
    $('#cntMenu').css("display", "none");

    var tipoIndex = $("#hdTipoIndex").val();

    if (tipoIndex == TIPO_INDEX_REPORTE)
        iniciarJsReporte();

    if (tipoIndex == TIPO_INDEX_VALIDACION)
        iniciarJsValidacion();
});

///////////////////////////////////////////////////////////////////////
/// Reporte
///////////////////////////////////////////////////////////////////////

function iniciarJsReporte() {
    $('#tab-container-x-envio').easytabs({
        animate: false
    });
    $('#tab-container-x-envio').bind('easytabs:after', function () {
        refrehDatatable();
    });

    $('#txtMesElaboracion').Zebra_DatePicker({
        format: 'm Y',
        onSelect: function () {
            consultarReporte();
        }
    });

    $('#btn-accept-report').click(function () {
        consultarReporte();
    });

    $('#btn-notificar-apertura').click(function () {
        notificarAgentes(1);
    });
    $('#btn-notificar-vencimiento').click(function () {
        notificarAgentes(2);
    });
    $('#txtMesRptMasivo').Zebra_DatePicker({
        format: 'm Y',
        onSelect: function () {
        }
    });

    $('#btn-ventana-reporte-masivo').click(function () {
        openExportar();
    });

    $('#btnExportar').click(function () {
        exportar();
    });

    $('#btnExportarMasivo').click(function () {
        var valoresCheck = "";
        $("input[type=checkbox]:checked").each(function () {
            if (valoresCheck == "") {
                valoresCheck = this.value;
            }
            else {
                valoresCheck = valoresCheck + "," + this.value;
            }
        });
        var mesReporte = $("#txtMesRptMasivo").val();

        generarArchivos(valoresCheck, mesReporte);
    });

    $('#btn-ver-filserver').click(function () {
        window.open(siteRoot + 'PMPO/ConfiguracionParametros/IndexFileServer', '_blank').focus();
    });

    //mensajes pmpo
    $('#ddl-sender').change(function () {
        listarMensajesPmpo();
    });
    $('#ddl-state').change(function () {
        listarMensajesPmpo();
    });

    //inicializar envio de mensaje
    crearUploaderMensaje();
    $('#btn-send').click(function () {
        guardarMensaje();
    });

    $('#download-message').click(function () {
        descargarPdfListadoMensaje();
    });

    ANCHO_REPORTE = $("#mainLayout").width() > 1100 ? $("#mainLayout").width() - 30 : 1100;

    consultarReporte();
}

///////////////////////////////////////////////////////////////////////
/// Generar reporte masivo
///////////////////////////////////////////////////////////////////////
openExportar = function () {
    $('#divExportar').css('display', 'block');
}

closeExportar = function () {
    $('#divExportar').css('display', 'none');
}

function generarArchivos(formatos, mesReporte) {
    $.ajax({
        type: 'POST',
        url: controlador + "GenerarReportesFormatos",
        data: {
            formatos: formatos,
            mesElaboracion: mesReporte
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                window.location = controlador + "ExportarZip?file_name=" + evt.Resultado;
            } else {
                alert(evt.Mensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });

}

function consultarReporte() {
    var emprcodi = parseInt($("#ddl-companies").val()) || 0;
    var formatcodi = parseInt($("#ddl-information-types").val()) || 0;
    var mesElaboracion = $("#txtMesElaboracion").val();
    var estEnvio = parseInt($("#ddl-EstaEnvio").val()) || 0;
    var estDerivacion = parseInt($("#ddl-EstaDerivacion").val()) || 0;

    $('#tab-container-x-envio').easytabs('select', '#tabReporteEnvio');

    $.ajax({
        type: 'POST',
        url: controlador + "ListaReporteEnvio",
        dataType: 'json',
        data: {
            emprcodi: emprcodi,
            formatcodi: formatcodi,
            mesElaboracion: mesElaboracion,
            estEnvio: estEnvio,
            estDerivacion: estDerivacion
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {

                $("#reporte").html(evt.HtmlReporte);

                $('#listado').css("width", ANCHO_REPORTE + "px");
                refrehDatatable();

                mostrarPlazoMes(evt.PlazoFormato);
            } else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function refrehDatatable() {
    $('#table-resumen').dataTable({
        "scrollY": 430,
        "scrollX": true,
        "destroy": "true",
        "sDom": 'ft',
        "ordering": false,
        "iDisplayLength": -1
    });
}

function mostrarPlazoMes(obj) {
    $("#btn-notificar-apertura").hide();
    $("#btn-notificar-vencimiento").hide();
    $("#mensajePlazoPmpo").html('');
    $("#mensajePlazoPmpo").hide();

    var html = '';
    html += `
        <div class="pull">
            <b class="color-blue">Periodo:</b>
            <span id="lbl-period">${obj.Periodo}</span>
        </div>
        <div class="pull" style='display: none'>
            <b class="color-blue">Unidad:</b>
            <span id="lbl-unitmeasure" data-id="1" data-symbol="MW">Potencia (MW)</span>
            <a id="btn-unitmeasure-change" href="#">Cambiar</a>
        </div>
        `;

    if (obj.EsCerrado) {
        html += `
        <div class="pull">
            La Extranet se encuentra <b class="color-blue">CERRADO</b>. Solo se permite ampliación por empresa.
        </div>
        `;
    }

    if (!obj.EsCerrado) {
        html += `
        <div id="div-tiempo-restante" class="pull">
            <b class="color-blue">Apertura de envío de información:</b>
            <span id="lbl-clock">${obj.FechaPlazoIniDesc}</span>
        </div>
        <div id="div-fecha-max-remision" class="pull">
            <b class="color-blue">Fecha máxima de remisión:</b>
            <span id="lbl-deadline-for-submission">${obj.FechaPlazoFueraDesc}</span>
        </div>
        `;

        $("#btn-notificar-apertura").show();
        $("#btn-notificar-vencimiento").show();
        $("#mensajePlazoPmpo").show();
    }

    $("#mensajePlazoPmpo").show();
    $("#mensajePlazoPmpo").html(html);
}

///////////////////////////////////////////////////////////////////////
/// Visualizar datos de último envío
///////////////////////////////////////////////////////////////////////
function visualizarEnvio(idEnvio) {
    var tipoinfocodi = parseInt($("#ddl-unidad-filtro").val()) || 0;

    $("#divDetalleEnvio").html('');
    $('#tab-container-x-envio').easytabs('select', '#tabDetalleEnvio');

    $.ajax({
        type: 'POST',
        url: controlador + "IndexEnvio",
        data: {
            idEnvio: idEnvio,
            tipoinfocodi: tipoinfocodi,
        },
        success: function (evt) {
            $("#divDetalleEnvio").html(evt);

            cargarFormato(hojaPrincipal); //hojaFormato.js
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

///////////////////////////////////////////////////////////////////////
/// Derivación para validación de COES
///////////////////////////////////////////////////////////////////////

function derivarEnvio(enviocodi, emprcodi, formatcodi) {
    var mesElaboracion = $("#txtMesElaboracion").val();

    $.ajax({
        type: 'POST',
        url: controlador + "ExisteDerivacion",
        dataType: 'json',
        data: {
            emprcodi: emprcodi,
            formatcodi: formatcodi,
            mesElaboracion: mesElaboracion,
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {

                if (evt.Resultado == 1) {
                    generarEnvioDerivacion(enviocodi, emprcodi, formatcodi, mesElaboracion);
                } else {
                    if (confirm("Ya existe una derivación por la empresa y tipo de información, ¿Desea realizar la derivación de todas formas?")) {
                        generarEnvioDerivacion(enviocodi, emprcodi, formatcodi, mesElaboracion);
                    }
                }
            } else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function generarEnvioDerivacion(enviocodi, emprcodi, formatcodi, mesElaboracion) {

    $.ajax({
        type: 'POST',
        url: controlador + "DerivarValidacionCOES",
        dataType: 'json',
        data: {
            enviocodi: enviocodi,
            emprcodi: emprcodi,
            formatcodi: formatcodi,
            mesElaboracion: mesElaboracion,
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                if (evt.Resultado != "0") {
                    alert("Se derivó la información correctamente a VALIDACIÓN COES");

                    consultarReporte();
                } else {
                    alert("No existe actualización de datos. No se derivó el envío.");
                }
            } else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

///////////////////////////////////////////////////////////////////////
/// Validación
///////////////////////////////////////////////////////////////////////

function iniciarJsValidacion() {
    $('#tab-container-x-envio').easytabs({
        animate: false
    });
    $('#tab-container-x-envio').bind('easytabs:after', function () {
        refrehDatatable();
    });

    $('#txtMesElaboracion').Zebra_DatePicker({
        format: 'm Y',
        onSelect: function () {
            mostrarListaArchivos('E');
            consultarReporteValidacion();
            procesoImportacion();
        }
    });

    $('#btn-accept-report').click(function () {
        consultarReporteValidacion();
    });

    $('#btnImportarFormatos').click(function () {

        $('#tab-container-x-envio').easytabs('select', '#tabImportacion');

        mostrarListaArchivos('E');
        procesoImportacion();
    });

    ANCHO_REPORTE = $("#mainLayout").width() > 1100 ? $("#mainLayout").width() - 30 : 1100;

    //mensajes pmpo
    $('#ddl-sender').change(function () {
        listarMensajesPmpo();
    });
    $('#ddl-state').change(function () {
        listarMensajesPmpo();
    });

    //inicializar envio de mensaje
    crearUploaderMensaje();
    $('#btn-send').click(function () {
        guardarMensaje();
    });

    $('#download-message').click(function () {
        descargarPdfListadoMensaje();
    });

    //
    $('#osinerg-btn-send').click(function () {
        guardarComentarioOsinerg();
    });

    consultarReporteValidacion();
}

function consultarReporteValidacion() {
    var emprcodi = parseInt($("#ddl-companies").val()) || 0;
    var formatcodi = parseInt($("#ddl-information-types").val()) || 0;
    var mesElaboracion = $("#txtMesElaboracion").val();
    var estadoCumplimiento = $("#ddl-EstaCumpl").val();

    $('#tab-container-x-envio').easytabs('select', '#tabReporteEnvio');

    $.ajax({
        type: 'POST',
        url: controlador + "ListaReporteValidacion",
        dataType: 'json',
        data: {
            emprcodi: emprcodi,
            formatcodi: formatcodi,
            mesElaboracion: mesElaboracion,
            estadoCumplimiento: estadoCumplimiento,
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {

                $("#reporte").html(evt.HtmlReporte);

                $('#listado').css("width", ANCHO_REPORTE + "px");
                refrehDatatable();

                mostrarPlazoMes(evt.PlazoFormato);
            } else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function procesoImportacion() {
    mostrarLogProceso();
    mostrarLoadingValidacion();
}

///////////////////////////////////////////////////////////////////////
/// Actualizar datos de último envío
///////////////////////////////////////////////////////////////////////
function editarEnvio(idEnvio) {
    var tipoinfocodi = parseInt($("#ddl-unidad-filtro").val()) || 0;

    $("#divDetalleEnvio").html('');
    $('#tab-container-x-envio').easytabs('select', '#tabDetalleEnvio');

    $.ajax({
        type: 'POST',
        url: controlador + "IndexValidacion",
        data: {
            idEnvio: idEnvio,
            tipoinfocodi: tipoinfocodi,
        },
        success: function (evt) {
            $("#divDetalleEnvio").html(evt);

            cargarFormato(hojaPrincipal); //hojaFormato.js
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

///////////////////////////////////////////////////////////////////////
/// Notificación
///////////////////////////////////////////////////////////////////////
function notificarAgentes(indicador) {
    var mesElaboracion = $("#txtMesElaboracion").val();

    var msjAlert = "¿Desea enviar notificación de apertura de plazo a los agentes?";
    if (indicador == 2) msjAlert = "¿Desea enviar notificación de vencimiento de plazo a los agentes?";

    if (confirm(msjAlert)) {
        $("#idTablaNotificacion").html('');

        $.ajax({
            type: 'POST',
            url: controlador + "NotificarPlazo",
            dataType: 'json',
            data: {
                indicador: indicador,
                mesElaboracion: mesElaboracion,
            },
            success: function (evt) {
                if (evt.Resultado != "-1") {
                    $('#idTablaNotificacion').html(dibujarTablaNotificacion(evt.ListaResultadoNotif));
                    setTimeout(function () {
                        $('#popupNotificacion').bPopup({
                            easing: 'easeOutBack',
                            speed: 450,
                            transition: 'slideDown',
                            modalClose: false
                        });
                        $('#tablaNotificacion').dataTable({
                            "scrollY": 330,
                            "scrollX": false,
                            "sDom": 'ft',
                            "ordering": false,
                            "bPaginate": false,
                            "iDisplayLength": -1
                        });
                    }, 50);


                    if (evt.NameFileLog != null && evt.NameFileLog != '') {
                        window.location.href = controlador + 'DescargarLogNotificacion?archivo=' + evt.NameFileLog;
                    }

                    consultarReporte();
                } else {
                    alert(evt.Mensaje);
                }
            },
            error: function (err) {
                alert("Ha ocurrido un error");
            }
        });
    }
}

function dibujarTablaNotificacion(lista) {

    var cadena = "";
    cadena += `
        <div style='clear:both; height:5px'></div>
            <table id='tablaNotificacion' border='1' class='pretty tabla-adicional' cellspacing='0' style='width: 1000px'>
        <thead>
            <tr>
                <th style='width: 300px'>Empresa</th>
                <th style='width: 150px'>Tipo de información</th>
                <th style='width: 150px'>Correos</th>
                <th style='width: 400px'>Resultado</th>
            </tr>
        </thead>
        <tbody>
    `;

    for (key in lista) {
        var reg = lista[key];

        var sCorreos = reg.ListaToEmails.join('<br/>');

        cadena += `
            <tr>  
                <td style='width: 300px; word-break: break-all;white-space: break-spaces;'>${reg.Emprnomb}</td>
                <td style='width: 150px; word-break: break-all;white-space: break-spaces;'>${reg.Formatnombre}</td>
                <td style='width: 150px; word-break: break-all;white-space: break-spaces;'>${sCorreos}</td>
                <td style='width: 400px; word-break: break-all;white-space: break-spaces;'>${reg.MensajeResultado}</td>
            </tr> 

        `;
    }
    cadena += "</tbody></table>";
    return cadena;

}

///////////////////////////////////////////////////////////////////////
/// Mensajes
///////////////////////////////////////////////////////////////////////
function verMensajeEnvio(idEmpresa, idFormato) {
    $("#hfEmpresaMsj").val(idEmpresa);
    $("#hfFormatoMsj").val(idFormato);

    listarMensajesPmpo(true);
}

function listarMensajesPmpo(abrirPopup) {
    var mesElaboracion = $("#txtMesElaboracion").val();
    var idEmpresa = $("#hfEmpresaMsj").val();
    var idFormato = $("#hfFormatoMsj").val();
    var tipoRemitente = $("#ddl-sender").val();
    var estadoMensaje = $("#ddl-state").val();

    $("#lst-comments").html('');
    ARRAY_FILES_MENSAJE = [];
    $("#txt-comment").val('');
    $("#div-attachments").html('');

    $.ajax({
        type: 'POST',
        url: controlador + "ListaMensajeXAgente",
        dataType: 'json',
        data: {
            mes: mesElaboracion,
            idEmpresa: idEmpresa,
            idFormato: idFormato,
            tipoRemitente: tipoRemitente,
            estadoMensaje: estadoMensaje
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                $('#lst-comments').html(dibujarTablaMensajesPmpo(evt.ListaMensaje));

                if (abrirPopup) {
                    setTimeout(function () {
                        $('#popupMensajesPmpo').bPopup({
                            easing: 'easeOutBack',
                            speed: 450,
                            transition: 'slideDown',
                            modalClose: false
                        });
                    }, 50);
                }

            } else {
                alert(evt.Mensaje);
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function dibujarTablaMensajesPmpo(lista) {
    var htmlListado = '';

    if (lista != null && lista.length > 0) {
        for (key in lista) {
            var reg = lista[key];

            var clase = '';
            if (!reg.EsRemitenteAgente) clase += "bg-color-ligthBlue ";
            if (reg.EsLeido) clase += "state-viewed ";

            var htmlArchivos = '<br/><br/>';
            for (key in reg.ListaArchivo) {
                var value = reg.ListaArchivo[key];
                htmlArchivos += `
                    <a  onclick="descargarArchivoMensaje('${value}', ${reg.Msjcodi});" style="cursor:pointer;text-align:left;color: blue;">${value}</a>
                    <br/>
                `;
            }

            htmlListado += `
                <section class='${clase}'>
                    <header>
                        <span>${reg.MsjfeccreacionDesc}</span>
                        <h4>${reg.Msjusucreacion} » ${reg.EmpresaRemitente}</h4>
                    </header>
                    <content>
                        ${reg.Msjdescripcion}
                        ${htmlArchivos}
                    </content>
                </section>
        `;
        }
    }

    return htmlListado;
}

function guardarMensaje() {
    var mesElaboracion = $("#txtMesElaboracion").val();
    var idEmpresa = $("#hfEmpresaMsj").val();
    var idFormato = $("#hfFormatoMsj").val();
    var text = $("#txt-comment").val();
    var files = JSON.stringify(ARRAY_FILES_MENSAJE);

    if (text != null && text != "") {
        $.ajax({
            type: 'POST',
            url: controlador + "GuardarMensaje",
            dataType: 'json',
            data: {
                mes: mesElaboracion,
                idEmpresa: idEmpresa,
                idFormato: idFormato,
                text: text,
                files: files
            },
            success: function (evt) {
                if (evt.Resultado != "-1") {
                    alert("Se envió el mensaje al Agente.");
                    $('#popupMensajesPmpo').bPopup().close();

                    ARRAY_FILES_MENSAJE = [];
                    $("#txt-comment").val('');
                    $("#div-attachments").html('');
                } else {
                    alert(evt.Mensaje);
                }
            },
            error: function (err) {
                alert("Ha ocurrido un error");
            }
        });
    } else {
        alert("Debe ingresar comentario.");
    }
}

function crearUploaderMensaje() {
    var fupAttachment = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: 'btn-file-attach',
        url: controlador + 'FileAttachment',
        multiple_queues: true,
        unique_names: true,
        init: {
            FilesAdded: function (sender, files) {
                $("#div-message-mensaje").hide();

                plupload.each(files, function (file) {
                    var item = $('<div>'),
                        btnFileRemove = $('<a>');

                    btnFileRemove
                        .attr('href', 'javascript:void(0)')
                        .html('x');

                    item.append(file.name);
                    item.append('&nbsp;');
                    item.append(btnFileRemove);

                    $('#div-attachments').append(item);
                });

                $('#div-attachments').show();

                fupAttachment.start();
                sender.refresh();
            },
            UploadComplete: function (sender, files) {
                var arrFiles;
                if (files != null && files.length > 0) {
                    arrFiles = [];

                    files.forEach(function (file) {
                        arrFiles.push({
                            fileName: file.name,
                            tmpFileName: file.target_name
                        });
                    });

                    //array.each(files, function (index, file) {
                    //    arrFiles.push({
                    //        fileName: file.name,
                    //        tmpFileName: file.target_name
                    //    });
                    //})
                }

                //quitar del uploader los archivos subidos con anterioridad
                $.each(fupAttachment.files, function (i, file) {
                    if (file && file !== undefined) {
                        fupAttachment.removeFile(file);
                    }
                });


                ARRAY_FILES_MENSAJE = ARRAY_FILES_MENSAJE.concat(arrFiles);
            },
            Error: function (sender, e) {
                var message = "\nError #" + e.code + ": " + e.message.substring(0, e.message.length - 1),
                    sfxMessage;

                switch (e.code) {
                    case -600:
                        sfxMessage = e.file.size + ' bytes';
                        break;
                    case -601:
                        sfxMessage = e.file.type;
                        break;
                    case -200:
                        sfxMessage = e.status;
                        break;
                }

                if (sfxMessage) {
                    message += ' - ' + sfxMessage;
                }

                message += '.';

                $("#div-message-mensaje").show();
                $("#div-message-mensaje").html(message);
            }
        }
    });

    fupAttachment.init();
}

function descargarArchivoMensaje(nombreArchivo, codigo) {
    if (nombreArchivo !== undefined && nombreArchivo != null) {
        var paramList = [
            { tipo: 'input', nombre: 'fileName', value: nombreArchivo },
            { tipo: 'input', nombre: 'codigo', value: codigo }
        ];
        var form = CreateFormArchivo(controlador + 'DescargarArchivoXMensaje', paramList);
        document.body.appendChild(form);
        form.submit();
    }

    return true;
}

function CreateFormArchivo(path, params, method = 'post') {
    var form = document.createElement('form');
    form.method = method;
    form.action = path;

    $.each(params, function (index, obj) {
        var hiddenInput = document.createElement(obj.tipo);
        hiddenInput.type = 'hidden';
        hiddenInput.name = obj.nombre;
        hiddenInput.value = obj.value;
        form.appendChild(hiddenInput);
    });
    return form;
}

function descargarPdfListadoMensaje() {
    var mes = $("#txtMesElaboracion").val();
    var idEmpresa = $("#hfEmpresaMsj").val();
    var idFormato = $("#hfFormatoMsj").val();
    var tipoRemitente = $("#ddl-sender").val();
    var estadoMensaje = $("#ddl-state").val();

    window.location.href = controlador + `DownloadFilePdfListadoMensaje?mes=${mes}&idEmpresa=${idEmpresa}&idFormato=${idFormato}&tipoRemitente=${tipoRemitente}&estadoMensaje=${estadoMensaje}`;
}

///////////////////////////////////////////////////////////////////////
/// Comentario Osinergmin
///////////////////////////////////////////////////////////////////////
function ingresarComentarioOsinergmin(idEmpresa, idFormato, mes, text) {
    $("#osinerg-empresa").val(idEmpresa);
    $("#osinerg-formato").val(idFormato);
    $("#osinerg-mes").val(mes);
    $("#osinerg-txt-comment").val(text);

    setTimeout(function () {
        $('#popupComentarioOsinerg').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
            modalClose: false
        });
    }, 50);

}

function guardarComentarioOsinerg() {
    var mesElaboracion = $("#osinerg-mes").val();
    var idEmpresa = $("#osinerg-empresa").val();
    var idFormato = $("#osinerg-formato").val();
    var text = $("#osinerg-txt-comment").val();

    $.ajax({
        type: 'POST',
        url: controlador + "GuardarComentarioOsinergmin",
        dataType: 'json',
        data: {
            mes: mesElaboracion,
            idEmpresa: idEmpresa,
            idFormato: idFormato,
            text: text,
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                alert("Se guardó el comentario.");
                $('#popupComentarioOsinerg').bPopup().close();

                $("#osinerg-mes").val('');
                $("#osinerg-empresa").val('');
                $("#osinerg-formato").val('');
                $("#osinerg-txt-comment").val('');

                consultarReporteValidacion();
            } else {
                alert(evt.Mensaje);
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}