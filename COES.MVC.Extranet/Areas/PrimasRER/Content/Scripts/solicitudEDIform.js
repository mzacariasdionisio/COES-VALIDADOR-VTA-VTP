var controlador = siteRoot + 'PrimasRER/SolicitudEDI/';
const origenFuerzaMayor = 6;

$(document).ready(function () {
    $('.txtFecha').Zebra_DatePicker({
        onSelect: function (dateFormat1, dateFormat2, dateFormat3) {
            const iperiodomes = parseInt($("#iperimes").val(), 10);
            const iperiodoanio = parseInt($("#iperianio").val(), 10);
            if ($('#EntidadSolicitudEDI_Reroricodi').val() != origenFuerzaMayor) {
                //Mostrar alerta si la fecha de inicio o fin es menor a la del periodo
                if ((dateFormat3.getFullYear()) < iperiodoanio || (dateFormat3.getFullYear()) == iperiodoanio && (dateFormat3.getMonth() + 1) < iperiodomes) {
                    alert("La solicitud por enviar es anterior al 'Periodo de Reporte EDI' vigente. Por lo tanto, la solicitud está fuera de plazo.");
                }
            }
        }
    });

    $('.txtHora').inputmask({
        mask: "h:s",
        placeholder: "hh:mm",
        alias: "datetime",
        hourformat: 24
    });

    jQuery.extend(jQuery.validator.messages, {
        required: "Este campo es obligatorio.",
        remote: "Por favor, rellena este campo.",
        email: "Por favor, escribe una dirección de correo válida",
        url: "Por favor, escribe una URL válida.",
        date: "Por favor, escribe una fecha válida.",
        dateISO: "Por favor, escribe una fecha (ISO) válida.",
        number: "Por favor, escribe un número entero válido.",
        digits: "Por favor, escribe sólo dígitos.",
        creditcard: "Por favor, escribe un número de tarjeta válido.",
        equalTo: "Por favor, escribe el mismo valor de nuevo.",
        accept: "Por favor, escribe un valor con una extensión aceptada.",
        maxlength: jQuery.validator.format("Por favor, no escribas más de {0} caracteres."),
        minlength: jQuery.validator.format("Por favor, no escribas menos de {0} caracteres."),
        rangelength: jQuery.validator.format("Por favor, escribe un valor entre {0} y {1} caracteres."),
        range: jQuery.validator.format("Por favor, escribe un valor entre {0} y {1}."),
        max: jQuery.validator.format("Por favor, escribe un valor menor o igual a {0}."),
        min: jQuery.validator.format("Por favor, escribe un valor mayor o igual a {0}.")
    });

    $("#descargarFormato").on('click', function () {
        descargarFormato();
    });

    $("#descargarSustento").on('click', function () {
        descargarSustento();
    });

    $("#importarSustento").on('click', function () {
        $("#archivoSustento").trigger('click');
    });

    $("#descargarFormatoWord").on('click', function () {
        $("#archivoSustento").trigger('click');
    });

    $('#archivoSustento').on('change', function () {
        const archivo = $(this)[0].files[0];
        let maxMB = parseInt(iMaxSizeSustento, 10);

        if (!isNaN(maxMB)) {
        } else {
            maxMB = 10;
        }
        // Validar el tamaño máximo del archivo (en bytes)
        const tamanoMaximo = maxMB * 1024 * 1024;
        //console.log("maxMB: ", maxMB);
        if (archivo.size > tamanoMaximo) {
            mostrarError('El archivo excede el tamaño máximo permitido de ' + maxMB + ' MB');
            $(this).val(''); // Limpiar el valor del input file
            return;
        }
        // Validar el tipo de archivo (zip,rar,pdf,xlsx,docx,xls,doc,xlsm)
        var archivosPermitidos = [
            'application/zip', 'application/octet-stream', 'application/x-zip-compressed', 'multipart/x-zip',
            'application/vnd.rar', 'application/x-rar-compressed', 'application/octet-stream',
            'application/pdf',
            'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet',
            'application/vnd.openxmlformats-officedocument.wordprocessingml.document',
            'application/vnd.ms-excel', 'application/msword', 'application/vnd.ms-excel.sheet.macroEnabled.12'
            ];
        if (!archivosPermitidos.includes(archivo.type)) {
            mostrarError('Tipo de archivo no permitido. Se permiten archivos zip, rar, pdf, xlsx, docx, xls, doc y xlsm.');
            $(this).val(''); // Limpiar el valor del input file
            return;
        }
        mostrarExito('Archivo válido');
    });

    uploadExcel();

    $('#descargarDosArchivos').on('click', function () {
        descargarArchivo(window.rutaPlantilla1, 'Informe de solicitud de EDI 1.docx');
        descargarArchivo(window.rutaPlantilla2, 'Informe de solicitud de EDI (Ejemplo) 1.docx');
    });
   
});

function descargarArchivo(url, nombre) {
    const link = document.createElement('a');
    link.href = url;
    link.setAttribute('download', nombre); 
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
}

function descargarFormato() {
    limpiarMensaje();
    let iMesPeriodoEDI = parseInt($("#iperimes").val(), 10);
    let iAnioPeriodoEDI = parseInt($("#iperianio").val(), 10);

    $.ajax({
        type: 'POST',
        url: controlador + 'ExportarEnergiaUnidadExcel',
        contentType: 'application/json;',
        data: JSON.stringify({
            iMesPeriodoEDI: iMesPeriodoEDI,
            iAnioPeriodoEDI: iAnioPeriodoEDI,
            rersedcodi: $('#EntidadSolicitudEDI_Rersedcodi').val(),
            rercencodi: $('#EntidadSolicitudEDI_Rercencodi').val(),
            fechainicio: $('#Fechainicio').val(),
            horainicio: $('#Horainicio').val(),
            fechafin: $('#Fechafin').val(),
            horafin: $('#Horafin').val()
        }),
        datatype: 'json',
        success: function (result) {
            if (result.sMensajeError != "") {
                mostrarError(result.sMensajeError);
            }
            else {
                window.location = controlador + 'abrirarchivo?nombreArchivo=' + result.nombreArchivo;
            }
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

function descargarSustento() {
    limpiarMensaje();
    let nomSustento = $('#EntidadSolicitudEDI_Rersedsustento').val();
    if (nomSustento === "") {
        alert("No existe ningún archivo de sustento para descargar");
    } else
    {
        window.location = controlador + 'abrirarchivo?nombreArchivo=' + nomSustento + '&tipoRuta=' + 2;
    }
}

//Procedimiento para la lectura de un archivo excel
uploadExcel = function () {
    uploader = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: 'importarFormato',          /* SE ASIGNA EL BTN QUE EJECUTARA EL EVENTO */
        url: controlador + 'UploadExcel',             /* Función en el controlador que ejecutara el evento  */
        flash_swf_url: 'Scripts/Moxie.swf',
        silverlight_xap_url: 'Scripts/Moxie.xap',
        multi_selection: false,
        filters: {
            max_file_size: '100mb',
            mime_types: [
                { title: "Archivos Excel .xlsx", extensions: "xlsx,xls" }
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
                mostrarAlerta("Por favor espere ...(" + file.percent + "%)");
            },
            UploadComplete: function (up, file) {
                mostrarAlerta("Subida completada, procesando el archivo...");
                procesarArchivo(file[0].name);
            },
            Error: function (up, err) {
                mostrarError(err.code + "-" + err.message);
            }
        }
    });
    uploader.init();
}

//Procedimiento para procesar un archivo excel
procesarArchivo = function (sFile) {
    let rercencodi = $('#EntidadSolicitudEDI_Rercencodi').val();
    let ipericodi = $('#EntidadSolicitudEDI_Ipericodi').val();
    let fechainicio = $('#Fechainicio').val();
    let horainicio = $('#Horainicio').val();
    let fechafin = $('#Fechafin').val();
    let horafin = $('#Horafin').val();

    $.ajax({
        type: 'POST',
        url: controlador + 'ProcesarArchivo',
        data: {
            sarchivo: sFile,
            rercencodi: rercencodi,
            ipericodi: ipericodi,
            fechainicio: fechainicio,
            horainicio: horainicio,
            fechafin: fechafin,
            horafin: horafin
        },
        dataType: 'json',
        cache: false,
        success: function (result) {
            if (result.iRegError > 0) {
                mostrarError("El archivo Excel no se puede leer porque contiene " + result.iRegError + " error(es)" + result.sMensajeError);
            }
            else if (result.iRegError == -1) {
                mostrarError(result.sMensajeError);
            }
            else if (result.iRegError == -2) {
                mostrarError("Lo sentimos, se interrumpió la lectura del archivo por presentar errores" + result.sMensajeError);
            }
            else {
                $('#jsonListaEnergiaUnidad').val(result.jsonListaEnergiaUnidad);
                solicitudEdi = result.EntidadSolicitudEDI;
                $('#EntidadSolicitudEDI_Rersedtotenergia').val(solicitudEdi.Rersedtotenergia);
                mostrarExito(result.sMensaje);
            }
        },
        error: function () {
            mostrarError('Lo sentimos no se puede mostrar la hoja de cálculo')
        }
    });
}

//Funciones para mostrar mensajes
mostrarExito = function (mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-exito');
}

mostrarError = function (mensaje) {
    //elimina el primer <br> del mensaje en caso lo tuviera
    mensaje = mensaje.replace(/^<br>/, function (match) {
        return '';
    });
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-error');
}

mostrarAlerta = function (mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-alert');
}

mostrarMensaje = function (mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-message');
}

limpiarMensaje = function () {
    $('#mensaje').removeClass();
    $('#mensaje').html("");
}