var controlador = siteRoot + "PotenciaFirmeRemunerable/Automatizacion/";

$(document).ready(function () {


    $('#cbAnio').change(function () {
        listadoPeriodo();
    });

    $('#cbPeriodo').change(function () {
        buscarUnifilar();
    });

    //adjuntarArchivo();
    buscarUnifilar();
});

//consulta
function buscarUnifilar() {
    var pericodi = parseInt($("#cbPeriodo").val()) || 0;

    $.ajax({
        type: 'POST',
        url: controlador + "ListadoUnifilar",
        data: {
            pfrpercodi: pericodi
        },
        success: function (evt) {

            $('#listado_unifilar').html(evt);
            $('#tabla_unifilar').dataTable({
                "sDom": 'ft',
                "ordering": true,
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

                buscarUnifilar();
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
        url: controlador + 'UploadUnifilar?pfrpercodi=' + pericodi,
        multi_selection: false,
        filters: {
            max_file_size: '2mb',
            mime_types: [
                { title: "Archivos Excel .xlsx", extensions: "xlsx" },
                { title: "Archivos Excel .xls", extensions: "xls" },
                { title: "Archivos Excel .xlsm", extensions: "xlsm" }
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
                buscarUnifilar();
            },
            Error: function (up, err) {
                if (err.code === -600) {
                    alert("La capacidad máxima del archivo es de 2MB. \nSeleccionar archivo con el tamaño adecuado."); return;
                }
            }
        }
    });

    uploader.init();
}

