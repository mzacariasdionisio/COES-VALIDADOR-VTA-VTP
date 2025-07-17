var controlador = siteRoot + 'DirectorioImpugnaciones/AdminImpugnacion/';
var uploader;

$(function () {
    $('#btnRegresar').click(function () {
        window.location.href = controlador + '?tipo=' + tipoImpugnacion;
    });

    tinymce.init({
        selector: '#decisionImpugnada',
        plugins: 'link preview',
        toolbar: 'link bullist numlist bold preview ',
        menubar: false,
        language: 'es',
        statusbar: false,
        setup: function (editor) {
            editor.on('change', function () {
                editor.save();
            });
        }
    });

    tinymce.init({
        selector: '#petitorio',
        plugins: 'link preview',
        toolbar: 'link bullist numlist bold preview ',
        menubar: false,
        language: 'es',
        statusbar: false,
        setup: function (editor) {
            editor.on('change', function () {
                editor.save();
            });
        }
    });

    tinymce.init({
        selector: '#decision',
        plugins: 'link preview',
        toolbar: 'link bullist numlist bold preview ',
        menubar: false,
        language: 'es',
        statusbar: false,
        setup: function (editor) {
            editor.on('change', function () {
                editor.save();
            });
        }
    });

    $('#fecRecepcion').Zebra_DatePicker();
    $('#fecPublicacion').Zebra_DatePicker();
    $('#plazoIncorporacion').Zebra_DatePicker();
    $('#fecDesicion').Zebra_DatePicker();

    $(window).resize(function () {
        setTimeout(function () {
            $('.Zebra_DatePicker_Icon_Wrapper').attr('style', '');
            $('.Zebra_DatePicker_Icon').attr('style', 'top: 29px; right: 20px;');
        }, 100);
    });
    $('.Zebra_DatePicker_Icon_Wrapper').attr('style', '');
    $('.Zebra_DatePicker_Icon').attr('style', 'top: 29px; right: 20px;');

    $("#frmRegistro").validate({
        submitHandler: function () {
            grabar();
        },
        rules: {
            Titulo: { required: true },
            NumeroMes: { required: true },
            Impugnante: { required: true },
            FecAnio: { required: true },
            FecMes: { required: true },
            RegistroSGDOC: { required: true },
            FecRecepcion: { required: true },
            FecPublicacion: { required: true },
            PlazoIncorporacion: { required: true }
        },
        messages: {
            Titulo: { required: "Ingrese titulo del documento" },
            NumeroMes: { required: "Ingrese número en el mes" },
            Impugnante: { required: "Ingrese el Impugnante" },
            FecAnio: { required: "Seleccione año" },
            FecMes: { required: "Seleccione mes" },
            RegistroSGDOC: { required: "Ingrese el registro SGDOC" },
            FecRecepcion: { required: "Ingrese la fecha de recepción" },
            FecPublicacion: { required: "Ingrese la fecha de publicación" },
            PlazoIncorporacion: { required: "Ingrese el plazo de incorporación" }
        }
    });

    // Cargar de archivo
    uploader = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: 'btnCambiarArchivo',
        url: controlador + 'Upload',
        flash_swf_url: '~/Content/Scripts/Moxie.swf',
        silverlight_xap_url: '~/Content/Scripts/Moxie.xap',
        multi_selection: false,
        chunk_size: '5mb',
        filters: {
            max_file_size: '100mb',
            mime_types: [
                { title: "Imagenes", extensions: "jpg,jpeg,gif,png" },
                { title: "Archivos comprimidos", extensions: "zip,rar" },
                { title: "Documentos", extensions: "pdf,doc,docx,xls,xlsx,ppt,pptx,csv" }
            ]
        },
        init: {
            PostInit: function () {
                $('#btnGrabar').click(function () {
                    if ($("#frmRegistro").valid()) {
                        if (uploader.files.length > 0 && $('#nuevo').val() == "S") {
                            uploader.start();
                        } else if (uploader.files.length == 0 && $('#nuevo').val() == "S") {
                            loadValidacionFile("Seleccione archivo");
                        } else if ($('#nuevo').val() == "N" && $('#cambiarArchivo').val() == "S") {
                            uploader.start();
                        } else if ($('#nuevo').val() == "N") {
                            grabar();
                            return true;
                        }
                    }
                    return false;
                });
            },
            FilesAdded: function (up, files) {
                if (uploader.files.length == 2) {
                    uploader.removeFile(uploader.files[0]);
                }
                up.refresh();
                var tipo = files[0].name.substring(files[0].name.lastIndexOf(".") + 1, files[0].name.length);
                var titulo = files[0].name.substring(0, files[0].name.lastIndexOf("."));
                $('#tipoArchivo').html("." + tipo);
                $('#textoNombre').html(titulo);
                $('#extension').val(tipo);
                $('#nombreArchivo').val(titulo);
                $('#cambiarArchivo').val("S");
                $('#titulo').val(titulo);
            },
            UploadProgress: function (up, file) {
                mostrarProgreso(file.percent);
            },
            UploadComplete: function (up, file) {
                grabar();
            },
            Error: function (up, err) {
                loadValidacionFile(err.message);
            }
        }
    });
    uploader.init();
});

function loadValidacionFile(mensaje) { $('#progreso').html(mensaje); }

function mostrarProgreso(porcentaje) { $('#progreso').text(porcentaje + "%"); }

downloadBlob = function (id) {
    document.location.href = controlador + "Download?id=" + id;
}

grabar = function () {
    $.ajax({
        type: 'POST',
        url: controlador + 'Grabar',
        data: $('#frmRegistro').serialize(),
        dataType: 'json',
        success: function (result) {
            if (result != -1) {
                window.location.href = controlador + 'Index?tipo=' + tipoImpugnacion;
            } else {
                alert('Ha ocurrido un error');
            }
        },
        error: function () {
            alert('Ha ocurrido un error');
        }
    });
}