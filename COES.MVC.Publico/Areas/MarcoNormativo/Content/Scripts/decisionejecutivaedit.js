var controlador = siteRoot + 'marconormativo/decisionejecutiva/';
var uploaderPrincipal;
var uploaderCarta;
var uploaderAntecedente;

$(function () {

    $('#txtFechaPublicacion').Zebra_DatePicker({
    });

    $('#btnGrabar').on('click', function () {
        procesar();
    });

    $('#btnCancelar').on('click', function () {
        cancelar();
    });

    tinymce.init({
        selector: '#txtDescripcion',
        plugins: [
        'paste textcolor colorpicker textpattern'
        ],
        toolbar1: 'insertfile undo redo | bold italic | alignleft aligncenter alignright alignjustify |  bullist numlist outdent indent| forecolor backcolor',
        menubar: false,
        language: 'es',
        statusbar: false,
        setup: function (editor) {
            editor.on('change', function () {
                editor.save();
            });
        }
    });

    $('#cbTipoDocumento').val($('#hfTipoDocumento').val());

    if ($('#hfCodigoRegistro').val() != "0") {
        $('#btnGrabar').val("Modificar");
        $('#btnAddPrincipal').val("Reemplazar");
        $('#btnAddFileCarta').val("Agregar");
        $('#btnAddAntecedente').val("Agregar");
    }

    uploaderPrincipal =  configurarUpload( 1, 'btnAddPrincipal', 'containerPrincipal', 'filelistPrincipal', 'loadingcargaPrincipal', false);
    uploaderCarta = configurarUpload(2, 'btnAddFileCarta', 'containerCarta', 'filelistCarta', 'loadingcargaCarta', true);
    uploaderAntecedente = configurarUpload(3, 'btnAddAntecedente', 'containerAntecedente', 'filelistAntecedente', 'loadingcargaAntecedente', true);
});

procesar = function () {
    
    var mensaje = validar();

    if (mensaje == "") {

        $('#loadingcarga').css('display', 'block');

        var arreglo = new Array();
        $('#tablaCarta > tbody  > tr').each(function () {
            var id = $(this).attr('id');
            var txt = $('#txt' + id).val();
            arreglo.push([id, txt]);
        });

        $('#tablaAntecedente > tbody  > tr').each(function () {
            var id = $(this).attr('id');
            var txt = $('#txt' + id).val();
            arreglo.push([id, txt]);
        });

        $.ajax({
            type: 'POST',
            url: controlador + 'grabar',
            contentType: 'application/json',
            traditional: true,
            data: JSON.stringify({
                codigo: $('#hfCodigoRegistro').val(),
                tipoRegistro: $('#cbTipoDocumento').val(),
                fecha: $('#txtFechaPublicacion').val(),
                descripcion: $('#txtDescripcion').val(),
                datos: arreglo
            }),
            dataType: 'json',
            success: function (result) {
                if (result > 0) {
                    
                    $('#hfCodigoRegistro').val(result);

                    uploaderPrincipal.settings.multipart_params = {                        
                        "codigo": result,
                        "indicador": "1"
                    }

                    uploaderCarta.settings.multipart_params = {                        
                        "codigo": result,
                        "indicador": "2"
                    }

                    uploaderAntecedente.settings.multipart_params = {                        
                        "codigo": result,
                        "indicador": "3"
                    }

                    uploaderPrincipal.start();
                    uploaderCarta.start();
                    uploaderAntecedente.start();
                    
                }
                else {
                    mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
                }
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
            }
        })
    }
    else {
        mostrarMensaje('mensaje', 'alert', mensaje);
    }
}

configurarUpload = function ( opcion, btnAddFile, container, fileList, loadingCarga, multiselect) {

   var uploader = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: btnAddFile,
        container: document.getElementById(container),
        url: controlador + 'upload',
        flash_swf_url: '~/Content/Scripts/Moxie.swf',
        silverlight_xap_url: '~/Content/Scripts/Moxie.xap',
        multi_selection: multiselect,
        chunk_size: '5mb',
        filters: {
            max_file_size: '50mb',
            mime_types: [
                { title: "Imagenes", extensions: "jpg,jpeg,gif,png" },
                { title: "Archivos comprimidos", extensions: "zip,rar" },
                { title: "Documentos", extensions: "pdf,doc,docx,xls,xlsx,ppt,pptx,csv" }
            ]
        },
        init: {
            PostInit: function () {
                $('#' + fileList).html('');
                $('#' + container).css('display', 'none');
            },
            FilesAdded: function (up, files) {
                $('#' + fileList).html('');
                $('#' + container).css('display', 'block');

                for (i = 0; i < uploader.files.length; i++) {
                    var file = uploader.files[i];

                    $('#' + fileList).html($('#' + fileList).html() + '<div class="file-name" id="' + file.id + '">' + file.name +
                            ' (' + plupload.formatSize(file.size) + ') <a class="remove-item" href="JavaScript:eliminarFile(' +
                            opcion + ',\'' + file.id + '\');">X</a> <b></b></div>');
                }                
            },
            UploadProgress: function (up, file) {
                document.getElementById(file.id).getElementsByTagName('b')[0].innerHTML = '<span>' + file.percent + "%</span>";
                $('#' + file.id).css('background-color', '#FFFFB7');
            },
            UploadComplete: function (up, file) {
                $('#' + loadingCarga).css('display', 'none');

                if (opcion == 1) {
                    $('#hfIndPrincipal').val("1");
                }
                if (opcion == 2) {
                    $('#hfIndCarta').val("2");
                }
                if (opcion == 3) {
                    $('#hfIndAntecedente').val("3");
                }
                finalizar();
            },
            Error: function (up, err) {
                $('#' + container).css('display', 'none');
                $('#' + fileList).html('<div class="action-alert">' + err.message + '</div>');
            }
        }
    });
    uploader.init();

    eliminarFile = function (opcion, id) {
        uploader.removeFile(id);
        $('#' + id).remove();
    }

    return uploader;
}

finalizar = function () {

    if ($('#hfIndPrincipal').val() == "1" &&
        $('#hfIndCarta').val() == "2" &&
        $('#hfIndAntecedente').val() == "3") {
        document.location.href = controlador + 'editar?id=' + $('#hfCodigoRegistro').val() + '&tipo=' + $('#cbTipoDocumento').val() + '&indicador=S';
    }
}

validar = function () {
    
    var mensaje = "<ul>";
    var flag = true;

    if ($('#txtFechaPublicacion').val() == "") {
        mensaje = mensaje + "<li>Seleccione la fecha de publicación.</li>";
        flag = false;
    }

    if ($('#txtDescripcion').val() == "") {
        mensaje = mensaje + "<li>Ingrese una descripción.</li>"
        flag = false;
    }
        
    if (uploaderPrincipal.files.length == 0 && $('#hfCodigoRegistro').val() == "0") {
        mensaje = mensaje + "<li>Seleccione al archivo principal.</li>";
        flag = false;
    }

    var flagCarta = true;
    $('#tablaCarta > tbody  > tr').each(function () {
        var id = $(this).attr('id');
        if ($('#txt' + id).val() == "") {
            flagCarta = false;
        }
        
    });

    var flagAntecedente = true;
    $('#tablaAntecedente > tbody  > tr').each(function () {
        var id = $(this).attr('id');
        if ($('#txt' + id).val() == "") {
            flagAntecedente = false;
        }
    });

    if (!flagCarta)
    {
        mensaje = mensaje + "<li>Ingrese las descripciones de las cartas.</li>";
        flag = false;
    }

    if (!flagAntecedente)
    {
        mensaje = mensaje + "<li>Ingrese las descripciones de los antecedentes.</li>";
        flag = false;
    }
    
    mensaje = mensaje + "</ul>";
    if (flag) mensaje = "";
    
    return mensaje;
}

cancelar = function () {
    document.location.href = controlador + 'index';
}

descargar = function (url) {
    document.location.href = controlador + "Download?file=" + url;
}

eliminarItem = function (id) {
    if (confirm('¿Está seguro de eliminar este registro?'))
    {
        $.ajax({
            type: 'POST',
            url: controlador + 'eliminaritem',
            data: {
                id: id
            },
            dataType:'json',
            success: function (result) {                
                if (result == 1) {
                    mostrarMensaje('mensaje', 'exito', 'El registro se eliminó correctamente.');
                    $('#' + id).remove();
                }
                else {
                    mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
                }
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
            }
        })
    }
}

mostrarMensaje = function (id, tipo, mensaje) {
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);
}
