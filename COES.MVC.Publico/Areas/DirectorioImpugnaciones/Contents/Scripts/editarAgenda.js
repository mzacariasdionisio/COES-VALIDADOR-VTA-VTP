var controlador = siteRoot + 'DirectorioImpugnaciones/AdminAgenda/';
var uploader;

$(function () {
    $('#btnRegresar').click(function () {
        window.location.href = controlador + 'Index?tipo=' + tipoEventoAgenda;
    });

    tinymce.init({
        selector: '#Ubicacion',
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
        selector: '#Descripcion',
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

    $('#fechaEvento').Zebra_DatePicker();
    $('#horaInicio').inputmask({
        mask: "1/2/y h:s",
        placeholder: "dd/mm/yyyy hh:mm",
        alias: "datetime",
        hourFormat: "24"
    });

    $('#horaInicio').Zebra_DatePicker({
        readonly_element: false,
        onSelect: function (date) {
            $('#horaInicio').val(date + " 00:00");
        }
    });
    $('#horaFin').inputmask({
        mask: "1/2/y h:s",
        placeholder: "dd/mm/yyyy hh:mm",
        alias: "datetime",
        hourFormat: "24"
    });

    $('#horaFin').Zebra_DatePicker({
        readonly_element: false,
        onSelect: function (date) {
            $('#horaFin').val(date + " 00:00");
        }
    });
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
            grabarEventoAgenda();
        },
        rules: {
            titulo: { required: true },
            FechaEvento: { required: true },
            HoraInicio: { required: true },
            HoraFin: { required: true },
            Ubicacion: { required: true }
        },
        messages: {
            titulo: { required: "Ingrese titulo del Evento" },
            FechaEvento: { required: "Ingrese fecha del evento" },
            HoraInicio: { required: "Ingrese hora de inicio" },
            HoraFin: { required: "Ingrese hora fin" },
            Ubicacion: { required: "Ingrese la ubicación" }
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
                            grabarEventoAgenda();
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
            },
            UploadProgress: function (up, file) {
                mostrarProgreso(file.percent);
            },
            UploadComplete: function (up, file) {
                grabarEventoAgenda();
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

grabarEventoAgenda = function () {
    $.ajax({
        type: 'POST',
        url: controlador + 'Grabar',
        data: $('#frmRegistro').serialize(),
        dataType: 'json',
        success: function (result) {
            if (result != -1) {
                window.location.href = controlador + 'Index?tipo=' + tipoEventoAgenda;
            } else {
                alert('Ha ocurrido un error');
            }
        },
        error: function () {
            alert('Ha ocurrido un error');
        }
    });
}