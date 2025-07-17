var controlador = siteRoot + 'eventos/AnalisisFallas/';

$(function () {

    $('#dtEditEveIni').inputmask({
        mask: "1/2/y h:s:s",
        placeholder: "dd/mm/yyyy hh:mm:ss",
        alias: "datetime",
        hourFormat: "24"
    });

    $('#dtEditEveIni').Zebra_DatePicker({
        readonly_element: false,
        onSelect: function (date) {
            $('#dtEditEveIni').val(date + " 00:00:00");
        }
    });

    $('#btnEnviar').click(function () {
        grabar();
    });
    $('#btnCancelar').click(function () {
        location.href = controlador + "IndexSolicitudes";
    });    

    adjuntarArchivo1();
    adjuntarArchivo2();

    valoresIniciales();
});

function grabar() {
    var emprcodi = $("#cbEmpresa").val();
    var dtFechEve = $("#dtEditEveIni").val();
    var descripcion = $("#txtDescripcion").val();

    var msjValidacion = validarSolicitud();
    if (msjValidacion != "")
        mostrarAlerta(msjValidacion);
    else {
        limpiarMensaje();
        var solicitud = {
            Emprcodi: emprcodi,
            FechEvento: dtFechEve,
            Sorespdesc: descripcion
        }
        $.ajax({
            type: 'POST',
            url: controlador + 'EnviarSolicitud',
            contentType: "application/json; charset=utf-8",

            data: JSON.stringify(solicitud),

            success: function (eData) {
                if (eData.Resultado == '-1') {
                    alert(eData.StrMensaje);
                }
                else {
                    if (eData.Resultado != "") {
                        $("#mensaje2").hide();
                        if (eData.Resultado == "1") {
                            alert(eData.StrMensaje);
                            location.href = controlador + "IndexSolicitudes";
                        } else
                            if (eData.Resultado == "-2") {
                                mostrarAlerta(eData.StrMensaje)
                            }
                            else
                            alert(eData.StrMensaje);

                    }
                }
            },
            error: function (err) {
                alert("Ha ocurrido un error");
            }
        });
    }
}

function valoresIniciales() {

}

function adjuntarArchivo1() {
    var nombreModulo = document.getElementById('NombreModulo').value;
    var fullDate = new Date();
    var twoDigitMonth = ((fullDate.getMonth().length) == 1) ? '0' + (fullDate.getMonth() + 1) : (fullDate.getMonth() + 1);
    var sFecha = fullDate.getFullYear().toString() + twoDigitMonth.toString() + fullDate.getDate().toString() + fullDate.getHours().toString() + fullDate.getMinutes().toString() + fullDate.getSeconds().toString();
    var intArchivo = 1;

    uploaderN = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: "btnSelectFile1",
        url: controlador + 'UploadSolicitudes?sFecha=' + sFecha + '&sModulo=' + nombreModulo + '&nroOrden=' + intArchivo,
        multi_selection: false,
        filters: {
            max_file_size: '10mb',
            mime_types: [
                { title: "Archivos pdf .pdf", extensions: "pdf" },
                { title: "Archivos Word .doc", extensions: "doc" },
                { title: "Archivos Word .docx", extensions: "docx" }
            ]
        },
        init: {
            FilesAdded: function (up, files) {
                document.getElementById('filelist').innerHTML = '';
                $('#container').css('display', 'block');

                if (uploaderN.files.length === 2) {
                    uploaderN.removeFile(uploaderN.files[0]);
                }
                for (i = 0; i < uploaderN.files.length; i++) {
                    var file = uploaderN.files[i];
                }

                uploaderN.start();
            },
            UploadProgress: function (up, file) {
            },
            UploadComplete: function (up, file) {
                $('#container').css('display', 'block');
                mostrarListaArchivos(1);
            },
            Error: function (up, err) {
                if (err.code === -600) {
                    alert("La capacidad máxima del archivo es de 10MB. \nSeleccionar archivo con el tamaño adecuado."); return;
                }
            }
        }
    });
    uploaderN.init();
}

function adjuntarArchivo2() {
    var nombreModulo = document.getElementById('NombreModulo').value;
    var fullDate = new Date();
    var twoDigitMonth = ((fullDate.getMonth().length) == 1) ? '0' + (fullDate.getMonth() + 1) : (fullDate.getMonth() + 1);
    var sFecha = fullDate.getFullYear().toString() + twoDigitMonth.toString() + fullDate.getDate().toString() + fullDate.getHours().toString() + fullDate.getMinutes().toString() + fullDate.getSeconds().toString();
    var intArchivo = 2;

    uploaderN2 = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: "btnSelectFile2",
        url: controlador + 'UploadSolicitudes?sFecha=' + sFecha + '&sModulo=' + nombreModulo + '&nroOrden=' + intArchivo,
        multi_selection: false,
        filters: {
            max_file_size: '10mb',
            mime_types: [
                { title: "Archivos Excel .xlsx", extensions: "xlsx" },
                { title: "Archivos Excel .xls", extensions: "xls" },
                { title: "Archivos pdf .pdf", extensions: "pdf" },
                { title: "Archivos Word .doc", extensions: "doc" },
                { title: "Archivos Word .docx", extensions: "docx" },
                { title: "Archivos Zip .zip", extensions: "zip" },
                { title: "Archivos Rar .rar", extensions: "rar" }
            ]
        },
        init: {
            FilesAdded: function (up, files) {
                document.getElementById('filelist2').innerHTML = '';
                $('#container2').css('display', 'block');

                if (uploaderN2.files.length === 2) {
                    uploaderN2.removeFile(uploaderN2.files[0]);
                }
                for (i = 0; i < uploaderN2.files.length; i++) {
                    var file = uploaderN2.files[i];
                }
                uploaderN2.start();
            },
            UploadProgress: function (up, file) {
            },
            UploadComplete: function (up, file) {
                $('#container2').css('display', 'block');
                mostrarListaArchivos(2);
            },
            Error: function (up, err) {
                if (err.code === -600) {
                    alert("La capacidad máxima es de 10MB. \nEliminar carpetas o archivos que no son parte del contenido del archivo ZIP."); return;
                }
            }
        }
    });
    uploaderN2.init();
}

function mostrarListaArchivos(orden) {
    var autoId = 0;
    var nombreModulo = document.getElementById('NombreModulo').value;

    $.ajax({
        type: 'POST',
        url: controlador + 'ListaArchivosNuevo',
        data: { sModulo: nombreModulo, nroOrden: orden},
        success: function (result) {
            var listaArchivos = result.ListaDocumentosFiltrado;

            $.each(listaArchivos, function (index, value) {
                autoId++;
                if (orden == 1) {
                    document.getElementById('filelist').innerHTML += '<div class="file-name" id="' + autoId + '">' + value.FileName + ' (' + value.FileSize + ') <a class="remove-item" href="JavaScript:eliminarFile(\'' + autoId + "@" + value.FileName + "@" + orden + '\');">X</a> <b></b></div>';
                } else {
                    document.getElementById('filelist2').innerHTML += '<div class="file-name" id="' + autoId + '">' + value.FileName + ' (' + value.FileSize + ') <a class="remove-item" href="JavaScript:eliminarFile(\'' + autoId + "@" + value.FileName + "@" + orden + '\');">X</a> <b></b></div>';
                }
            })
            var msjValidacion = validarSolicitud();
            if (msjValidacion != "")
                mostrarAlerta(msjValidacion);
            else {
                limpiarMensaje();
            }
        },
        error: function () {
        }
    });
}

function eliminarFile(id) {
    var string = id.split("@");
    var idInter = string[0];
    var nombreArchivo = string[1];
    var orden = string[2];

    uploaderN.removeFile(idInter);

    $.ajax({
        type: 'POST',
        url: controlador + 'EliminarArchivosNuevo',
        data: { nombreArchivo: nombreArchivo, nroOrden:orden },
        success: function (result) {
            if (result == -1) {
                $('#' + idInter).remove();
            } else {
                alert("Algo salió mal");
            }
        },
        error: function () {
        }
    });
}

descargarFormato = function () {
    window.location = controlador + 'descargarformato';
    //var mensaje = validacion();
    //if (mensaje == "") {
    //    window.location = controlador + 'descargarformato';
    //}
    //else {
    //    mostrarAlerta(mensaje);
    //}
}

validarSolicitud = function () {
    var validacion = "<ul>";
    var flag = true;
    if ($('#cbEmpresa').val() == "" || $('#dtEditEveIni').val() == "" || $('#txtDescripcion').val() == "") {
        validacion = validacion + "<li>Por favor complete los datos solicitados.</li>";
        flag = false;
    }

    if (!$('#filelist').find(".file-name").length) {
        validacion = validacion + "<li>Por favor adjunte el Informe Final</li>";
        flag = false;
    }
    validacion = validacion + "</ul>";

    if (flag == true) validacion = "";

    return validacion;
}

mostrarAlerta = function (alerta) {
    $('#mensaje2').html(alerta);
    $('#mensaje2').css("display", "block");
}

limpiarMensaje = function () {
    $('#mensaje2').html("");
    $('#mensaje2').css("display", "none");
}