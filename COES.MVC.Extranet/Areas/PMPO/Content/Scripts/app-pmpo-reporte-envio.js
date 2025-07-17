var controlador = siteRoot + 'PMPO/ReportSubmission/';

$(function () {

    $(getIdElemento(hojaPrincipal, '#cbEmpresa')).unbind('change');
    $(getIdElemento(hojaPrincipal, '#cbEmpresa')).change(function () {
        setVerUltimoEnvioGlobal(MAIN_VER_ULTIMO_ENVIO);
        cargarFormatosXEmpresa(hojaPrincipal);
    });

    $(getIdElemento(hojaPrincipal, '#cbTipoFormato')).change(function () {
        setVerUltimoEnvioGlobal(MAIN_VER_ULTIMO_ENVIO);
        cargarFormato(hojaPrincipal);
    });

    cargarFormato(hojaPrincipal); //hojaFormato.js

    //recordatorio
    listarRecordatorio();
    $('#ddl-sender_recordatorio').change(function () {
        listarRecordatorio();
    });

    //mensajes pmpo
    $('#btnVerMensajesPMPO').click(function () {
        listarMensajesPmpo(true);
    });

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

});

///////////////////////////////////////////////////////////////////////
/// Recodatorio
///////////////////////////////////////////////////////////////////////
function listarRecordatorio() {
    var mesElaboracion = $("#txtMesElaboracion").val();
    var tipoRemitente = $("#ddl-sender_recordatorio").val();

    //$("#idTablaRecordatorio").html('');

    $.ajax({
        type: 'POST',
        url: controlador + "ListaRecordatorio",
        dataType: 'json',
        data: {
            mes: mesElaboracion,
            tipoRemitente: tipoRemitente
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                $('#idTablaRecordatorio').html(dibujarTablaRecordatorio(evt.ListaMensaje));
                setTimeout(function () {
                    $('#popupRecordatorio').bPopup({
                        easing: 'easeOutBack',
                        speed: 450,
                        transition: 'slideDown',
                        modalClose: false
                    });
                    $('#tablaRecordatorio').dataTable({
                        "scrollY": 330,
                        "scrollX": false,
                        "sDom": 't',
                        "ordering": false,
                        "bPaginate": false,
                        "iDisplayLength": -1,
                        "stripeClasses": []
                    });
                }, 50);
            } else {
                alert(evt.Mensaje);
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function dibujarTablaRecordatorio(lista) {

    var cadena = "";
    cadena += `
        <div style='clear:both; height:5px'></div>
            <table id='tablaRecordatorio' border='1' class='pretty tabla-adicional' cellspacing='0' style='width: 1000px'>
        <thead>
            <tr>
                <th style='width: 300px'>Empresa</th>
                <th style='width: 150px'>Tipo de <br/>información</th>
                <th style='width: 50px'>Estado de <br/> mensaje</th>
                <th style='width: 50px'>Remitente</th>
                <th style='width: 400px'>Mensaje</th>
            </tr>
        </thead>
        <tbody>
    `;

    if (lista != null && lista.length > 0) {
        for (key in lista) {
            var reg = lista[key];

            var claseTdEstado = ' ';
            if (reg.EsLeido) claseTdEstado = 'color-green';
            else claseTdEstado = 'color-red';

            var claseTd = '';
            if (!reg.EsRemitenteAgente) claseTd += "bg-color-ligthBlue ";

            cadena += `
            <tr>  
                <td style='width: 300px; word-break: break-all;white-space: break-spaces;' class='${claseTd}'>${reg.Emprnomb}</td>
                <td style='width: 150px; word-break: break-all;white-space: break-spaces;' class='${claseTd}'>${reg.Formatnombre}</td>
                <td style='width: 50px; word-break: break-all;white-space: break-spaces;' class='${claseTd} ${claseTdEstado}'>${reg.MsjestadoDesc}</td>
                <td style='width: 50px; word-break: break-all;white-space: break-spaces;' class='${claseTd}'>${reg.Remitente}</td>
                <td style='width: 400px; word-break: break-all;white-space: break-spaces;text-align: left;' class='${claseTd}'><b class="color-red">${reg.MsjfeccreacionDesc}</b> >> ${reg.Msjdescripcion}</td>
            </tr> 

        `;
        }
    }

    cadena += "</tbody></table>";
    return cadena;

}

///////////////////////////////////////////////////////////////////////
/// Mensajes
///////////////////////////////////////////////////////////////////////
function listarMensajesPmpo(abrirPopup) {
    var mesElaboracion = $("#txtMesElaboracion").val();
    var idEmpresa = $("#cbEmpresa").val();
    var idFormato = $("#cbTipoFormato").val();
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
    var idEmpresa = $("#cbEmpresa").val();
    var idFormato = $("#cbTipoFormato").val();
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
                    alert("Se envió el mensaje al COES.");
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
        multi_selection: false,
        unique_names: true,
        init: {
            FilesAdded: function (sender, files) {
                if (fupAttachment.files.length == 2) {
                    fupAttachment.removeFile(fupAttachment.files[0]);
                }

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
    var idEmpresa = $("#cbEmpresa").val();
    var idFormato = $("#cbTipoFormato").val();
    var tipoRemitente = $("#ddl-sender").val();
    var estadoMensaje = $("#ddl-state").val();

    window.location.href = controlador + `DownloadFilePdfListadoMensaje?mes=${mes}&idEmpresa=${idEmpresa}&idFormato=${idFormato}&tipoRemitente=${tipoRemitente}&estadoMensaje=${estadoMensaje}`;
}
