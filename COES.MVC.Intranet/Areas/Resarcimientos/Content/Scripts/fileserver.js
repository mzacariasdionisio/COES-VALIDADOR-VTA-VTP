var controladorfile = siteRoot + "resarcimientos/fileserver/";

$(function () {

    $(document).ready(function () {        
        SubirExcel();
    })

    function SubirExcel() {
        var uploader = new plupload.Uploader({
            runtimes: 'html5,flash,silverlight,html4',
            browse_button: 'btn-excel-plantilla',
            url: controladorfile + 'SubirExcel',
            flash_swf_url: '../js/Moxie.swf',
            silverlight_xap_url: '../js/Moxie.xap',
            filters: {
                max_file_size: '10mb',
                mime_types: [
                    { title: "Archivos de Excel", extensions: "xls,xlsm,xlsx" }
                ]
            },
            init: {
                FilesAdded: function (up, files) {
                    plupload.each(files, function (file) {
                        uploader.start();
                    });
                },
                FileUploaded: function (up, file, info) {
                    $.ajax({
                        type: 'POST',
                        url: siteRoot + 'filemanager/Browser/folder?url=' + $('#btnRutaApp').val(),
                        async: false,
                        data: {
                        },
                        success: function (info) {
                            $('#browserDocument').html(info);

                            $('#mensaje').html('El archivo ha subio satisfactoriamente.');
                            setTimeout(function () {
                                $("#mensaje").fadeOut(1000);
                            }, 2000);
                        },
                        error: function (response) {
                            $('#mensaje').html('ha ocurrido un error al tratar de subir el archivo.');
                        }

                    });
                    
                },
                Error: function (up, err) {
                    $('#mensaje').html('El archivo \'<b>' + up.files[0].name + '</b>\' no tiene el formato correcto.');
                }
            }
        });
        uploader.init();
    }

});
