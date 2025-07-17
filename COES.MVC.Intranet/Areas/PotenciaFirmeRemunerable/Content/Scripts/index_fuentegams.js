var controlador = siteRoot + "PotenciaFirmeRemunerable/Automatizacion/";

$(document).ready(function () {


    $('#cbAnio').change(function () {
        listadoPeriodo();
    });

    $('#cbPeriodo').change(function () {
        buscarFuenteGams();
    });

    //adjuntarArchivo();
    buscarFuenteGams();
});

//consulta
function buscarFuenteGams() {
    var pericodi = parseInt($("#cbPeriodo").val()) || 0;

    $.ajax({
        type: 'POST',
        url: controlador + "ListadoFuenteGams",
        data: {
            pfrpercodi: pericodi
        },
        success: function (evt) {

            $('#listado_fuentegams').html(evt);
            $('#tabla_fuentegams').dataTable({
                "sDom": 'ft',
                "ordering": false,
                "iDisplayLength": -1,
                "bFilter": false
            });
        },
        error: function (err) {
            alert('Ha ocurrido un error: ' + err);
        }
    });
};

function listadoPeriodo() {

    var anio = parseInt($("#cbAnio").val()) || 0;

    $("#cbPeriodo").empty();

    $.ajax({
        type: 'POST',
        url: controlador + "PeriodoListado",
        data: {
            anio: anio,
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                if (evt.ListaPeriodo.length > 0) {
                    $.each(evt.ListaPeriodo, function (i, item) {
                        $('#cbPeriodo').get(0).options[$('#cbPeriodo').get(0).options.length] = new Option(item.Pfrpernombre, item.Pfrpercodi);
                    });
                } else {
                    $('#cbPeriodo').get(0).options[0] = new Option("--", "0");
                }

                buscarFuenteGams();
            } else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

function adjuntarArchivo() {

    var pericodi = parseInt($("#cbPeriodo").val()) || 0;

    var uploader = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: "btnSelectFile",
        url: controlador + 'UploadFuenteGams?pfrpercodi=' + pericodi + '&orden=' + 1,
        multi_selection: false,
        filters: {
            max_file_size: '10mb',
            mime_types: [
                { title: "Archivos .gms", extensions: "gms" }
            ]
        },
        init: {
            FilesAdded: function (up, files) {
                uploader.start();
                up.refresh();
            },
            UploadProgress: function (up, file) {
            },
            FileUploaded: function (up, file, info) {
                var result = JSON.parse(info.response);
                if (result.Resultado != "-1") {
                    alert("Los datos se cargaron correctamente");
                } else {
                    alert(result.Mensaje);
                }
            },
            UploadComplete: function (up, file) {
                buscarFuenteGams();
            },
            Error: function (up, err) {
                if (err.code === -600) {
                    alert("La capacidad máxima del archivo es de 10MB. \nSeleccionar archivo con el tamaño adecuado."); return;
                }
            }
        }
    });

    uploader.init();
}

function adjuntarArchivo2() {

    var pericodi = parseInt($("#cbPeriodo").val()) || 0;

    var uploader = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: "btnSelectFile2",
        url: controlador + 'UploadFuenteGams?pfrpercodi=' + pericodi + '&orden=' + 2,
        multi_selection: false,
        filters: {
            max_file_size: '10mb',
            mime_types: [
                { title: "Archivos .gms", extensions: "gms" }
            ]
        },
        init: {
            FilesAdded: function (up, files) {
                uploader.start();
                up.refresh();
            },
            UploadProgress: function (up, file) {
            },
            FileUploaded: function (up, file, info) {
                var result = JSON.parse(info.response);
                if (result.Resultado != "-1") {
                    alert("Los datos se cargaron correctamente");
                } else {
                    alert(result.Mensaje);
                }
            },
            UploadComplete: function (up, file) {
                buscarFuenteGams();
            },
            Error: function (up, err) {
                if (err.code === -600) {
                    alert("La capacidad máxima del archivo es de 10MB. \nSeleccionar archivo con el tamaño adecuado."); return;
                }
            }
        }
    });

    uploader.init();
}

