function crearPupload() {
        uploader = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: 'btnSelectExcel3',
        container: document.getElementById('container'),
        url: siteRoot + 'hidrologia/envio/Upload',
        flash_swf_url: 'Scripts/Moxie.swf',
        silverlight_xap_url: 'Scripts/Moxie.xap',
        multi_selection: false,
        filters: {
            max_file_size: '5mb',
            mime_types: [
                { title: "Archivos Excel .xlsx", extensions: "xlsx,xls" }
            ]
        },
        init: {
            PostInit: function () {
                document.getElementById('btnProcesarFile').onclick = function () {
                    if (uploader.files.length > 0) {
                        var valida = validacion();
                        if (valida == "") {
                            uploader.start();

                        }
                        else {
                            mostrarAlerta(valida);
                        }
                    }
                    else {
                        loadValidacionFile("Seleccione archivo.");
                    }
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
                //limpiarTodo();
                //limpiarMensaje();
                //$('#consulta').html("");
                //$('#btnProcesarFile').css('display', 'block');
                alert("CArgando");
                up.refresh();
            },
            UploadProgress: function (up, file) {
                mostrarProgreso(file.percent);
            },
            UploadComplete: function (up, file) {
                validarArchivo();
            },
            Error: function (up, err) {
                loadValidacionFile(err.code + "-" + err.message);
            }
        }
    });
}

